/* The movie player (movie.h). */
#include "movie.h"

#include "../audio/audio.h"
#include "../core/files.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#ifdef OM_HAVE_AVCODEC
#include <libavcodec/avcodec.h>

static struct {
    int open;
    int w, h, frames;
    double rate, delay, clock;
    int shown;                 /* pictures decoded and shown (the last one's index + 1) */
    uint8_t *m2v;              /* the stream, padded */
    size_t n, at;
    int flushing;
    const AVCodec *codec;
    AVCodecContext *x;
    AVCodecParserContext *ps;
    AVFrame *f;
    AVPacket *p;
    uint8_t *rgba;
    GpuTexture tex;
} M;

static int clamp_byte(int v) { return v < 0 ? 0 : v > 255 ? 255 : v; }

/* BT.601, the STR's own: Y'CbCr 4:2:0 to RGBA (opaque) */
static void picture(const AVFrame *f) {
    int r, c;

    for (r = 0; r < M.h; r++) {
        const uint8_t *yr = f->data[0] + (size_t)r * f->linesize[0];
        const uint8_t *ur = f->data[1] + (size_t)(r / 2) * f->linesize[1];
        const uint8_t *vr = f->data[2] + (size_t)(r / 2) * f->linesize[2];
        uint8_t *o = M.rgba + (size_t)r * M.w * 4;

        for (c = 0; c < M.w; c++) {
            int y = yr[c], u = ur[c / 2] - 128, v = vr[c / 2] - 128;

            o[c * 4] = (uint8_t)clamp_byte(y + (1402 * v) / 1000);
            o[c * 4 + 1] = (uint8_t)clamp_byte(y - (344 * u + 714 * v) / 1000);
            o[c * 4 + 2] = (uint8_t)clamp_byte(y + (1772 * u) / 1000);
            o[c * 4 + 3] = 255;
        }
    }
    if (M.tex.id == 0) {
        M.tex = render_texture_rgba(M.rgba, M.w, M.h);
    } else {
        render_texture_update(M.tex, M.rgba);
    }
}

/* the next picture decoded into the texture; 0 at the stream's end */
static int next_picture(void) {
    for (;;) {
        if (avcodec_receive_frame(M.x, M.f) == 0) {
            if (M.f->width == M.w && M.f->height == M.h) {
                picture(M.f);
            }
            return 1;
        }
        if (M.flushing == 2) {
            return 0;
        }
        if (M.at < M.n) {
            uint8_t *out = NULL;
            int out_n = 0;

            M.at += (size_t)av_parser_parse2(M.ps, M.x, &out, &out_n, M.m2v + M.at, (int)(M.n - M.at), AV_NOPTS_VALUE,
                                             AV_NOPTS_VALUE, 0);
            if (out_n > 0) {
                M.p->data = out;
                M.p->size = out_n;
                avcodec_send_packet(M.x, M.p);
            }
        } else if (M.flushing == 0) {
            uint8_t *out = NULL;
            int out_n = 0;

            av_parser_parse2(M.ps, M.x, &out, &out_n, NULL, 0, AV_NOPTS_VALUE, AV_NOPTS_VALUE, 0);
            if (out_n > 0) {
                M.p->data = out;
                M.p->size = out_n;
                avcodec_send_packet(M.x, M.p);
            }
            M.flushing = 1;
        } else {
            avcodec_send_packet(M.x, NULL);
            M.flushing = 2;
        }
    }
}

/* the description: size, rate, frames, delay, sound */
static int describe(const char *name, char *sound, size_t nsound) {
    char path[160], *t, *line;
    size_t n;

    snprintf(path, sizeof(path), "movies/%s.txt", name);
    t = (char *)files_read(path, &n);
    if (t == NULL) {
        return 0;
    }
    sound[0] = 0;
    for (line = strtok(t, "\n"); line != NULL; line = strtok(NULL, "\n")) {
        sscanf(line, "size %d %d", &M.w, &M.h);
        sscanf(line, "rate %lf", &M.rate);
        sscanf(line, "frames %d", &M.frames);
        sscanf(line, "delay %lf", &M.delay);
        if (strncmp(line, "sound ", 6) == 0) {
            snprintf(sound, nsound, "movies/%s", line + 6);
        }
    }
    free(t);
    return M.w > 0 && M.h > 0 && M.rate > 0;
}

int movie_open(const char *name, float volume) {
    char path[160], sound[160];
    uint8_t *file;
    size_t n;

    movie_close();
    memset(&M, 0, sizeof(M));
    if (!describe(name, sound, sizeof(sound))) {
        return 0;
    }
    snprintf(path, sizeof(path), "movies/%s.m2v", name);
    file = files_read(path, &n);
    M.codec = avcodec_find_decoder(AV_CODEC_ID_MPEG2VIDEO);
    if (file == NULL || M.codec == NULL) {
        free(file);
        return 0;
    }
    M.m2v = calloc(n + AV_INPUT_BUFFER_PADDING_SIZE, 1);
    memcpy(M.m2v, file, n);
    free(file);
    M.n = n;
    M.x = avcodec_alloc_context3(M.codec);
    M.ps = av_parser_init(AV_CODEC_ID_MPEG2VIDEO);
    M.f = av_frame_alloc();
    M.p = av_packet_alloc();
    M.rgba = malloc((size_t)M.w * M.h * 4);
    if (M.x == NULL || M.ps == NULL || avcodec_open2(M.x, M.codec, NULL) < 0) {
        M.open = 1;
        movie_close();
        return 0;
    }
    M.open = 1;
    if (sound[0]) {
        au_flac_play(sound, volume);
    }
    return 1;
}

int movie_advance(double seconds) {
    int due;

    if (!M.open) {
        return MOVIE_ENDED;
    }
    M.clock += seconds;
    if (M.clock < M.delay) {
        return MOVIE_BEFORE;
    }
    due = (int)((M.clock - M.delay) * M.rate);
    if (due >= M.frames) {
        return MOVIE_ENDED;
    }
    while (M.shown <= due) {   /* (behind: the pictures between decoded, the last shown) */
        if (!next_picture()) {
            return MOVIE_ENDED;
        }
        M.shown++;
    }
    return M.shown - 1;
}

GpuTexture movie_texture(void) { return M.tex; }

void movie_close(void) {
    if (!M.open) {
        return;
    }
    au_xa_stop();
    if (M.tex.id != 0) {
        render_texture_free(M.tex);
    }
    av_packet_free(&M.p);
    av_frame_free(&M.f);
    if (M.ps != NULL) {
        av_parser_close(M.ps);
    }
    avcodec_free_context(&M.x);
    free(M.m2v);
    free(M.rgba);
    memset(&M, 0, sizeof(M));
}

#else   /* (no libavcodec: no movie opens) */

int movie_open(const char *name, float volume) { (void)name, (void)volume; return 0; }
int movie_advance(double seconds) { (void)seconds; return MOVIE_ENDED; }
GpuTexture movie_texture(void) { GpuTexture t = {0, 0, 0}; return t; }
void movie_close(void) {}

#endif
