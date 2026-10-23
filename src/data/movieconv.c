/* The movies in the library (movieconv.h). */
#include "movieconv.h"

#include "flacenc.h"
#include "str.h"
#include "xa.h"

#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "dr_flac.h"

#ifdef OM_HAVE_AVCODEC
#include <libavcodec/avcodec.h>
#include <libavutil/frame.h>

#define QUANTISER 2      /* MPEG-2's fixed quantiser scale (1 finest .. 31) */
#define GOP_FRAMES 15    /* a key frame a second */
#define B_FRAMES 2
/* the MDEC's YCbCr is full range; MPEG's video range: luma 16..235, chroma 16..240 */
#define LUMA_FLOOR 16
#define LUMA_SPAN 219
#define CHROMA_SPAN 224
#define FULL_SPAN 255

int movieconv_available(void) { return 1; }

typedef struct Buf {
    uint8_t *d;
    size_t n, cap;
} Buf;

static void append(Buf *b, const uint8_t *d, size_t n) {
    if (b->n + n > b->cap) {
        b->cap = (b->n + n) * 2;
        b->d = realloc(b->d, b->cap);
    }
    memcpy(b->d + b->n, d, n);
    b->n += n;
}

/* frame i in MPEG's range, into planes (y: w x h, chroma (w/2) x ((h+1)/2)) */
static int video_frame(const StrMovie *m, int i, uint8_t *y, uint8_t *cb, uint8_t *cr) {
    size_t k, luma = (size_t)m->width * m->height, chroma = (size_t)(m->width / 2) * ((m->height + 1) / 2);

    if (!str_frame(m, i, y, cb, cr)) {
        return 0;
    }
    for (k = 0; k < luma; k++) {
        y[k] = (uint8_t)(LUMA_FLOOR + (y[k] * LUMA_SPAN + FULL_SPAN / 2) / FULL_SPAN);
    }
    for (k = 0; k < chroma; k++) {
        cb[k] = (uint8_t)(128 + ((cb[k] - 128) * CHROMA_SPAN + (cb[k] >= 128 ? 127 : -127)) / FULL_SPAN);
        cr[k] = (uint8_t)(128 + ((cr[k] - 128) * CHROMA_SPAN + (cr[k] >= 128 ? 127 : -127)) / FULL_SPAN);
    }
    return 1;
}

static void drain(AVCodecContext *x, AVPacket *p, Buf *out) {
    while (avcodec_receive_packet(x, p) == 0) {
        append(out, p->data, (size_t)p->size);
        av_packet_unref(p);
    }
}

static int encode(const StrMovie *m, Buf *out, char *why, size_t nwhy) {
    const AVCodec *codec = avcodec_find_encoder(AV_CODEC_ID_MPEG2VIDEO);
    AVCodecContext *x = codec != NULL ? avcodec_alloc_context3(codec) : NULL;
    AVFrame *f = av_frame_alloc();
    AVPacket *p = av_packet_alloc();
    int i, ok = 1;

    if (x == NULL || f == NULL || p == NULL) {
        snprintf(why, nwhy, "libavcodec has no MPEG-2 encoder");
        ok = 0;
        goto out;
    }
    x->width = m->width;
    x->height = m->height;
    x->pix_fmt = AV_PIX_FMT_YUV420P;
    x->time_base = (AVRational){1, MOVIE_RATE};
    x->framerate = (AVRational){MOVIE_RATE, 1};
    x->gop_size = GOP_FRAMES;
    x->max_b_frames = B_FRAMES;
    x->flags |= AV_CODEC_FLAG_QSCALE;
    x->global_quality = FF_QP2LAMBDA * QUANTISER;
    x->color_range = AVCOL_RANGE_MPEG;
    x->colorspace = AVCOL_SPC_BT470BG;   /* (BT.601) */
    x->color_primaries = AVCOL_PRI_BT470BG;
    x->color_trc = AVCOL_TRC_GAMMA22;
    if (avcodec_open2(x, codec, NULL) < 0) {
        snprintf(why, nwhy, "libavcodec's MPEG-2 encoder won't open");
        ok = 0;
        goto out;
    }
    f->format = AV_PIX_FMT_YUV420P;
    f->width = m->width;
    f->height = m->height;
    if (av_frame_get_buffer(f, 0) < 0) {
        ok = 0;
        goto out;
    }
    {
        int cw = m->width / 2, ch = (m->height + 1) / 2, r;
        uint8_t *y = malloc((size_t)m->width * m->height), *cb = malloc((size_t)cw * ch), *cr = malloc((size_t)cw * ch);

        for (i = 0; ok && i < m->frames; i++) {
            if (!video_frame(m, i, y, cb, cr)) {
                snprintf(why, nwhy, "frame %d is damaged", i);
                ok = 0;
                break;
            }
            av_frame_make_writable(f);
            for (r = 0; r < m->height; r++) {
                memcpy(f->data[0] + (size_t)r * f->linesize[0], y + (size_t)r * m->width, (size_t)m->width);
            }
            for (r = 0; r < ch; r++) {
                memcpy(f->data[1] + (size_t)r * f->linesize[1], cb + (size_t)r * cw, (size_t)cw);
                memcpy(f->data[2] + (size_t)r * f->linesize[2], cr + (size_t)r * cw, (size_t)cw);
            }
            f->pts = i;
            ok = avcodec_send_frame(x, f) == 0;
            drain(x, p, out);
        }
        free(y);
        free(cb);
        free(cr);
    }
    if (ok) {
        avcodec_send_frame(x, NULL);
        drain(x, p, out);
    }
    if (!ok && !why[0]) {
        snprintf(why, nwhy, "the MPEG-2 encoder failed");
    }
out:
    av_packet_free(&p);
    av_frame_free(&f);
    avcodec_free_context(&x);
    return ok;
}

/* the PSNR of a decoded picture against frame i's planes */
static double psnr(const AVFrame *f, const uint8_t *y, const uint8_t *cb, const uint8_t *cr, int w, int h) {
    double se = 0;
    long count = 0;
    int r, c, cw = w / 2, ch = (h + 1) / 2;

    for (r = 0; r < h; r++) {
        for (c = 0; c < w; c++, count++) {
            double e = f->data[0][(size_t)r * f->linesize[0] + c] - y[r * w + c];
            se += e * e;
        }
    }
    for (r = 0; r < ch; r++) {
        for (c = 0; c < cw; c++, count += 2) {
            double e1 = f->data[1][(size_t)r * f->linesize[1] + c] - cb[r * cw + c];
            double e2 = f->data[2][(size_t)r * f->linesize[2] + c] - cr[r * cw + c];
            se += e1 * e1 + e2 * e2;
        }
    }
    return se == 0 ? 99.0 : 10.0 * log10(255.0 * 255.0 * count / se);
}

/* decode the .m2v and hold each picture against the STR's; the worst PSNR in *worst, the pictures in *frames */
static int compare(const StrMovie *m, const uint8_t *m2v, size_t n, double *worst, int *frames, char *why, size_t nwhy) {
    const AVCodec *codec = avcodec_find_decoder(AV_CODEC_ID_MPEG2VIDEO);
    AVCodecContext *x = codec != NULL ? avcodec_alloc_context3(codec) : NULL;
    AVCodecParserContext *ps = av_parser_init(AV_CODEC_ID_MPEG2VIDEO);
    AVFrame *f = av_frame_alloc();
    AVPacket *p = av_packet_alloc();
    int cw = m->width / 2, ch = (m->height + 1) / 2, ok = 1, flushing = 0;
    uint8_t *y = malloc((size_t)m->width * m->height), *cb = malloc((size_t)cw * ch), *cr = malloc((size_t)cw * ch);
    uint8_t *padded = calloc(n + AV_INPUT_BUFFER_PADDING_SIZE, 1);
    size_t at = 0;

    *worst = 99.0;
    *frames = 0;
    memcpy(padded, m2v, n);
    if (x == NULL || ps == NULL || avcodec_open2(x, codec, NULL) < 0) {
        snprintf(why, nwhy, "libavcodec has no MPEG-2 decoder");
        ok = 0;
    }
    while (ok) {
        uint8_t *out = NULL;
        int out_n = 0, used = 0;

        if (at < n) {
            used = av_parser_parse2(ps, x, &out, &out_n, padded + at, (int)(n - at), AV_NOPTS_VALUE, AV_NOPTS_VALUE, 0);
            at += (size_t)used;
        } else if (!flushing) {
            av_parser_parse2(ps, x, &out, &out_n, NULL, 0, AV_NOPTS_VALUE, AV_NOPTS_VALUE, 0);
            flushing = 1;
        } else {
            avcodec_send_packet(x, NULL);
            flushing = 2;
        }
        if (out_n > 0) {
            p->data = out;
            p->size = out_n;
            avcodec_send_packet(x, p);
        }
        while (ok && avcodec_receive_frame(x, f) == 0) {
            double q;

            if (*frames >= m->frames || f->width != m->width || f->height != m->height ||
                !video_frame(m, *frames, y, cb, cr)) {
                snprintf(why, nwhy, "picture %d isn't one of the STR's", *frames);
                ok = 0;
                break;
            }
            q = psnr(f, y, cb, cr, m->width, m->height);
            if (q < *worst) {
                *worst = q;
            }
            (*frames)++;
        }
        if (flushing == 2) {
            break;
        }
    }
    free(y);
    free(cb);
    free(cr);
    free(padded);
    av_frame_free(&f);
    p->data = NULL;
    p->size = 0;
    av_packet_free(&p);
    if (ps != NULL) {
        av_parser_close(ps);
    }
    avcodec_free_context(&x);
    return ok;
}
#else
int movieconv_available(void) { return 0; }
#endif

static char *description(const char *name, const StrMovie *m, int sound, size_t *len) {
    char *s = malloc(1024);
    int k = snprintf(s, 1024,
                     "\\ %s.STR: the movie (data/movieconv.h)\nom-movie 1\nsize %d %d\nrate %d\nframes %d\n"
                     "delay %.6f\nsound %s%s\n",
                     name, m->width, m->height, MOVIE_RATE, m->frames, (double)m->due_sector[0] / STR_SECTORS_PER_SECOND,
                     sound ? name : "none", sound ? ".flac" : "");
    *len = (size_t)k;
    return s;
}

int movieconv_write(const char *name, const uint8_t *str, size_t n, MovieWriteFn write, void *ctx, char *why,
                    size_t nwhy) {
#ifdef OM_HAVE_AVCODEC
    StrMovie m;
    XaStream xa;
    Buf video = {NULL, 0, 0};
    char path[160], tag[64];
    size_t len;
    char *text;
    int ok, sound;

    why[0] = 0;
    if (!str_open(&m, str, n)) {
        snprintf(why, nwhy, "%s.STR isn't a movie", name);
        return 0;
    }
    sound = m.audio_file >= 0 && xa_decode(&xa, str, n, m.audio_file, m.audio_channel);
    ok = encode(&m, &video, why, nwhy);
    snprintf(path, sizeof(path), "movies/%s.m2v", name);
    ok = ok && write(ctx, path, video.d, video.n);
    if (ok && sound) {
        const char *tags[1];
        uint8_t *flac;

        snprintf(tag, sizeof(tag), "TITLE=%s", name);
        tags[0] = tag;
        flac = flacenc(xa.pcm, xa.frames, xa.channels, xa.rate, tags, 1, &len);
        snprintf(path, sizeof(path), "movies/%s.flac", name);
        ok = flac != NULL && write(ctx, path, flac, len);
        free(flac);
    }
    text = description(name, &m, sound, &len);
    snprintf(path, sizeof(path), "movies/%s.txt", name);
    ok = ok && write(ctx, path, (const uint8_t *)text, len);
    if (!ok && !why[0]) {
        snprintf(why, nwhy, "can't write %s", path);
    }
    free(text);
    free(video.d);
    if (sound) {
        xa_free(&xa);
    }
    str_close(&m);
    return ok;
#else
    (void)str, (void)n, (void)write, (void)ctx;
    snprintf(why, nwhy, "%s: built without libavcodec", name);
    return 0;
#endif
}

int movieconv_check(const char *name, const uint8_t *str, size_t n, MovieReadFn read, void *ctx, double *psnr_out,
                    char *why, size_t nwhy) {
#ifdef OM_HAVE_AVCODEC
    StrMovie m;
    XaStream xa;
    char path[160];
    size_t len, fn;
    char *text;
    uint8_t *file;
    int ok, sound, frames = 0;

    *psnr_out = 0;
    if (!str_open(&m, str, n)) {
        snprintf(why, nwhy, "%s.STR isn't a movie", name);
        return 0;
    }
    sound = m.audio_file >= 0 && xa_decode(&xa, str, n, m.audio_file, m.audio_channel);
    text = description(name, &m, sound, &len);
    snprintf(path, sizeof(path), "movies/%s.txt", name);
    file = read(ctx, path, &fn);
    ok = file != NULL && fn == len && memcmp(file, text, len) == 0;
    free(file);
    free(text);
    if (!ok) {
        snprintf(why, nwhy, "%s: missing or different", path);
    }
    if (ok) {
        snprintf(path, sizeof(path), "movies/%s.m2v", name);
        file = read(ctx, path, &fn);
        ok = file != NULL && compare(&m, file, fn, psnr_out, &frames, why, nwhy);
        if (ok && (frames != m.frames || *psnr_out < MOVIE_MIN_PSNR)) {
            snprintf(why, nwhy, "%s: %d pictures of %d, the worst %.1f dB", path, frames, m.frames, *psnr_out);
            ok = 0;
        } else if (file == NULL) {
            snprintf(why, nwhy, "%s: missing", path);
        }
        free(file);
    }
    if (ok && sound) {
        unsigned int channels, rate;
        drflac_uint64 frames_n;
        drflac_int16 *pcm;

        snprintf(path, sizeof(path), "movies/%s.flac", name);
        file = read(ctx, path, &fn);
        pcm = file != NULL ? drflac_open_memory_and_read_pcm_frames_s16(file, fn, &channels, &rate, &frames_n, NULL) : NULL;
        ok = pcm != NULL && (long)frames_n == xa.frames && (int)channels == xa.channels && (int)rate == xa.rate &&
             memcmp(pcm, xa.pcm, sizeof(int16_t) * (size_t)xa.frames * channels) == 0;
        if (!ok) {
            snprintf(why, nwhy, "%s: not the same sound as the STR's", path);
        }
        drflac_free(pcm, NULL);
        free(file);
    }
    if (sound) {
        xa_free(&xa);
    }
    str_close(&m);
    return ok;
#else
    (void)str, (void)n, (void)read, (void)ctx, (void)psnr_out;
    snprintf(why, nwhy, "%s: built without libavcodec", name);
    return 0;
#endif
}
