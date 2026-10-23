/* The game's movies (str.h). The decoder is phase 1's (port/src/psx/movie.c). */
#include "str.h"

#include <math.h>
#include <stdlib.h>
#include <string.h>

#define CHUNK 2016          /* a video sector's frame bytes */
#define SUBHEADER 16        /* a sector's file, channel, submode */
#define SUBMODE_AUDIO 0x04
#define VIDEO_HEADER 24
#define VIDEO_MAGIC 0x0160
#define FRAME_MAGIC 0x3800
#define MACROBLOCK 16
#define MAX_CHUNKS 32
#define LEVEL_SHIFT 128     /* a sample's DC level */

/* {run, level, code} */
static const struct { uint8_t run, level; const char *code; } kAc[] = {
    {0, 1, "11"}, {1, 1, "011"}, {0, 2, "0100"}, {2, 1, "0101"}, {0, 3, "00101"}, {3, 1, "00111"},
    {4, 1, "00110"}, {1, 2, "000110"}, {5, 1, "000111"}, {6, 1, "000101"}, {7, 1, "000100"},
    {0, 4, "0000110"}, {2, 2, "0000100"}, {8, 1, "0000111"}, {9, 1, "0000101"},
    {0, 5, "00100110"}, {0, 6, "00100001"}, {1, 3, "00100101"}, {3, 2, "00100100"}, {10, 1, "00100111"},
    {11, 1, "00100011"}, {12, 1, "00100010"}, {13, 1, "00100000"},
    {0, 7, "0000001010"}, {1, 4, "0000001100"}, {2, 3, "0000001011"}, {4, 2, "0000001111"},
    {5, 2, "0000001001"}, {14, 1, "0000001110"}, {15, 1, "0000001101"}, {16, 1, "0000001000"},
    {0, 8, "000000011101"}, {0, 9, "000000011000"}, {0, 10, "000000010011"}, {0, 11, "000000010000"},
    {1, 5, "000000011011"}, {2, 4, "000000010100"}, {3, 3, "000000011100"}, {4, 3, "000000010010"},
    {6, 2, "000000011110"}, {7, 2, "000000010101"}, {8, 2, "000000010001"}, {17, 1, "000000011111"},
    {18, 1, "000000011010"}, {19, 1, "000000011001"}, {20, 1, "000000010111"}, {21, 1, "000000010110"},
    {0, 12, "0000000011010"}, {0, 13, "0000000011001"}, {0, 14, "0000000011000"}, {0, 15, "0000000010111"},
    {1, 6, "0000000010110"}, {1, 7, "0000000010101"}, {2, 5, "0000000010100"}, {3, 4, "0000000010011"},
    {5, 3, "0000000010010"}, {9, 2, "0000000010001"}, {10, 2, "0000000010000"}, {22, 1, "0000000011111"},
    {23, 1, "0000000011110"}, {24, 1, "0000000011101"}, {25, 1, "0000000011100"}, {26, 1, "0000000011011"},
    {0, 16, "00000000011111"}, {0, 17, "00000000011110"}, {0, 18, "00000000011101"},
    {0, 19, "00000000011100"}, {0, 20, "00000000011011"}, {0, 21, "00000000011010"},
    {0, 22, "00000000011001"}, {0, 23, "00000000011000"}, {0, 24, "00000000010111"},
    {0, 25, "00000000010110"}, {0, 26, "00000000010101"}, {0, 27, "00000000010100"},
    {0, 28, "00000000010011"}, {0, 29, "00000000010010"}, {0, 30, "00000000010001"},
    {0, 31, "00000000010000"}, {0, 32, "000000000011000"}, {0, 33, "000000000010111"},
    {0, 34, "000000000010110"}, {0, 35, "000000000010101"}, {0, 36, "000000000010100"},
    {0, 37, "000000000010011"}, {0, 38, "000000000010010"}, {0, 39, "000000000010001"},
    {0, 40, "000000000010000"}, {1, 8, "000000000011111"}, {1, 9, "000000000011110"},
    {1, 10, "000000000011101"}, {1, 11, "000000000011100"}, {1, 12, "000000000011011"},
    {1, 13, "000000000011010"}, {1, 14, "000000000011001"}, {1, 15, "0000000000010011"},
    {1, 16, "0000000000010010"}, {1, 17, "0000000000010001"}, {1, 18, "0000000000010000"},
    {6, 3, "0000000000010100"}, {11, 2, "0000000000011010"}, {12, 2, "0000000000011001"},
    {13, 2, "0000000000011000"}, {14, 2, "0000000000010111"}, {15, 2, "0000000000010110"},
    {16, 2, "0000000000010101"}, {27, 1, "0000000000011111"}, {28, 1, "0000000000011110"},
    {29, 1, "0000000000011101"}, {30, 1, "0000000000011100"}, {31, 1, "0000000000011011"},
};

static const uint8_t kZigzag[64] = {
    0, 1, 8, 16, 9, 2, 3, 10, 17, 24, 32, 25, 18, 11, 4, 5, 12, 19, 26, 33, 40, 48, 41, 34, 27, 20, 13, 6, 7, 14,
    21, 28, 35, 42, 49, 56, 57, 50, 43, 36, 29, 22, 15, 23, 30, 37, 44, 51, 58, 59, 52, 45, 38, 31, 39, 46, 53,
    60, 61, 54, 47, 55, 62, 63 };

static const uint8_t kQuant[64] = {
    2, 16, 19, 22, 26, 27, 29, 34, 16, 16, 22, 24, 27, 29, 34, 37, 19, 22, 26, 27, 29, 34, 34, 38,
    22, 22, 26, 27, 29, 34, 37, 40, 22, 26, 27, 29, 32, 35, 40, 48, 26, 27, 29, 32, 35, 40, 48, 58,
    26, 27, 29, 34, 38, 46, 56, 69, 27, 29, 35, 38, 46, 56, 69, 83 };

/* VLC lookup on the next 16 bits: length (0 = invalid) | run << 8 | level << 16; EOB and escape marked */
#define V_EOB 0x80
#define V_ESC 0x40
static uint32_t sVlc[65536];
static float sCos[8][8];

static void tables_init(void)
{
    static int done;
    size_t i;
    int u, x;
    if (done)
        return;
    done = 1;
    for (i = 0; i < sizeof kAc / sizeof kAc[0]; i++) {
        int len = (int)strlen(kAc[i].code), c = 0, j;
        for (j = 0; j < len; j++)
            c = c << 1 | (kAc[i].code[j] - '0');
        c <<= 16 - len;
        for (j = 0; j < 1 << (16 - len); j++)
            sVlc[c | j] = (uint32_t)len | (uint32_t)kAc[i].run << 8 | (uint32_t)kAc[i].level << 16;
    }
    for (x = 0; x < 1 << 14; x++)
        sVlc[0x8000 | x] = 2 | V_EOB;                  /* 10 */
    for (x = 0; x < 1 << 10; x++)
        sVlc[0x0400 | x] = 6 | V_ESC;                  /* 000001 */
    for (u = 0; u < 8; u++)
        for (x = 0; x < 8; x++)
            sCos[u][x] = (float)((u == 0 ? sqrt(1.0 / 8) : sqrt(2.0 / 8)) * cos((2 * x + 1) * u * M_PI / 16));
}

typedef struct Bits {
    const uint8_t *d;
    int n;          /* bytes */
    int pos;        /* bit position */
} Bits;

static uint32_t peek(const Bits *b, int n)
{
    /* 32 bits from the 16-bit words at pos */
    int w = b->pos >> 4, sh = b->pos & 15, i;
    uint64_t v = 0;
    for (i = 0; i < 3; i++) {
        int o = (w + i) * 2;
        uint32_t h = o + 1 < b->n ? (uint32_t)(b->d[o] | b->d[o + 1] << 8) : 0;
        v = v << 16 | h;
    }
    /* v holds 48 bits in its low part; take n bits starting sh bits in */
    return (uint32_t)((v << (16 + sh)) >> (64 - n));
}

static uint32_t get(Bits *b, int n)
{
    uint32_t v = peek(b, n);
    b->pos += n;
    return v;
}

static int sext10(uint32_t v)
{
    return (int)(v & 0x200 ? v - 0x400 : v);
}

/* MPEG-1 DC size codes {code, length}: luma (B.12), chroma (B.13), sizes 0..8 */
static const uint8_t kDcLuma[9][2] = {{4, 3}, {0, 2}, {1, 2}, {5, 3}, {6, 3}, {14, 4}, {30, 5}, {62, 6}, {126, 7}};
static const uint8_t kDcChroma[9][2] = {{0, 2}, {1, 2}, {2, 2}, {6, 3}, {14, 4}, {30, 5}, {62, 6}, {126, 7},
                                          {254, 8}};

/* v3: the DC difference of a block of component c (0 Y, 1 Cb, 2 Cr), added to its predictor */
static int dc_v3(Bits *b, int c, int *pred)
{
    const uint8_t (*t)[2] = c == 0 ? kDcLuma : kDcChroma;
    int n, diff = 0;
    for (n = 0; n < 9; n++)
        if (peek(b, t[n][1]) == t[n][0])
            break;
    if (n == 9)
        return 0x7FFF;
    b->pos += t[n][1];
    if (n > 0) {
        int v = (int)get(b, n);
        diff = v >= 1 << (n - 1) ? v : v - (1 << n) + 1;
    }
    pred[c] += diff * 4;
    return pred[c];
}

/* one 8 x 8 block into out (stride), as signed values around 0; returns 0 on a bad stream. pred: NULL for v2,
 * else the v3 DC predictors and c the component */
static int block(Bits *b, int q, float *out, int stride, int *pred, int c)
{
    float co[64], tmp[64];
    int i = 0, k, x, y, dc;

    memset(co, 0, sizeof co);
    dc = pred ? dc_v3(b, c, pred) : sext10(get(b, 10));
    if (dc == 0x7FFF)
        return 0;
    co[0] = (float)(dc * kQuant[0]);
    for (;;) {
        uint32_t e = sVlc[peek(b, 16)];
        int run, level, len = e & 0x3F;
        if (e & V_EOB) {
            b->pos += 2;
            break;
        }
        if (e & V_ESC) {
            b->pos += 6;
            run = (int)get(b, 6);
            level = sext10(get(b, 10));
        } else {
            if (len == 0 || b->pos > b->n * 8)
                return 0;
            b->pos += len;
            run = (e >> 8) & 0xFF;
            level = (int)(e >> 16);
            if (get(b, 1))
                level = -level;
        }
        i += run + 1;
        if (i > 63)
            return 0;
        k = kZigzag[i];
        co[k] = (float)(level >= 0 ? (level * q * kQuant[k] + 4) >> 3 : -((-level * q * kQuant[k] + 4) >> 3));
    }
    /* IDCT: tmp = C^T * F, out = tmp * C */
    for (y = 0; y < 8; y++)
        for (x = 0; x < 8; x++) {
            float s = 0;
            for (k = 0; k < 8; k++)
                s += sCos[k][y] * co[k * 8 + x];
            tmp[y * 8 + x] = s;
        }
    for (y = 0; y < 8; y++)
        for (x = 0; x < 8; x++) {
            float s = 0;
            for (k = 0; k < 8; k++)
                s += tmp[y * 8 + k] * sCos[k][x];
            out[y * stride + x] = s;
        }
    return 1;
}

static uint8_t clamp8(float v) { return v < 0 ? 0 : v > 255 ? 255 : (uint8_t)(v + 0.5f); }

static int u16at(const uint8_t *p) { return p[0] | p[1] << 8; }
static long u32at(const uint8_t *p) { return (long)(p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24); }

static int is_video(const uint8_t *s) {
    return !(s[SUBHEADER + 2] & SUBMODE_AUDIO) && u16at(s + VIDEO_HEADER) == VIDEO_MAGIC;
}

int str_open(StrMovie *m, const uint8_t *d, size_t n) {
    int i, cap = 0, want = -1, have = 0, chunks = 0, start = 0;

    tables_init();
    memset(m, 0, sizeof(*m));
    m->d = d;
    m->n = n;
    m->sectors = (int)(n / STR_SECTOR);
    m->audio_file = m->audio_channel = -1;
    for (i = 0; i < m->sectors; i++) {
        const uint8_t *s = d + (size_t)i * STR_SECTOR, *h = s + VIDEO_HEADER;
        int chunk, frame;

        if (s[SUBHEADER + 2] & SUBMODE_AUDIO) {
            if (m->audio_file < 0) {
                m->audio_file = s[SUBHEADER];
                m->audio_channel = s[SUBHEADER + 1];
            }
            continue;
        }
        if (!is_video(s)) {
            continue;
        }
        chunk = u16at(h + 4);
        frame = (int)u32at(h + 8);
        if (frame != want) {   /* a new frame (a lost chunk drops the one before) */
            want = frame;
            have = 0;
            start = i;
            chunks = u16at(h + 6);
            if (chunks <= 0 || chunks > MAX_CHUNKS) {
                str_close(m);
                return 0;
            }
            if (m->width == 0) {
                m->width = u16at(h + 16);
                m->height = u16at(h + 18);
            }
        }
        if (chunk < chunks) {
            have |= 1 << chunk;
        }
        if (have == (1 << chunks) - 1) {
            if (m->frames == cap) {
                cap = cap * 2 + 256;
                m->first_sector = realloc(m->first_sector, sizeof(int) * (size_t)cap);
                m->due_sector = realloc(m->due_sector, sizeof(int) * (size_t)cap);
            }
            m->first_sector[m->frames] = start;
            m->due_sector[m->frames++] = i + 1;
            have = 0;
            want = -1;
        }
    }
    if (m->frames == 0 || m->width <= 0 || m->height <= 0 || m->width % MACROBLOCK != 0) {
        str_close(m);
        return 0;
    }
    return 1;
}

void str_close(StrMovie *m) {
    free(m->first_sector);
    free(m->due_sector);
    memset(m, 0, sizeof(*m));
}

int str_frame(const StrMovie *m, int i, uint8_t *y, uint8_t *cb, uint8_t *cr) {
    int w = m->width, h = m->height, ph = (h + MACROBLOCK - 1) & ~(MACROBLOCK - 1), cw = w / 2;
    int k, chunks = 0, size = 0, q, ver, mx, my, x, row, pred[3] = {0, 0, 0}, *pp, ok = 1;
    uint8_t *bs = calloc(MAX_CHUNKS, CHUNK);
    float *Y = calloc((size_t)w * ph, sizeof(float)), *Cb = calloc((size_t)cw * ph / 2, sizeof(float)),
          *Cr = calloc((size_t)cw * ph / 2, sizeof(float));
    Bits b;

    for (k = m->first_sector[i]; k < m->due_sector[i]; k++) {
        const uint8_t *s = m->d + (size_t)k * STR_SECTOR, *hd = s + VIDEO_HEADER;
        int chunk;

        if (!is_video(s) || u32at(hd + 8) != u32at(m->d + (size_t)m->first_sector[i] * STR_SECTOR + VIDEO_HEADER + 8)) {
            continue;
        }
        chunk = u16at(hd + 4);
        chunks = u16at(hd + 6);
        size = (int)u32at(hd + 12);
        if (chunk < MAX_CHUNKS) {
            memcpy(bs + (size_t)chunk * CHUNK, hd + 32, CHUNK);
        }
    }
    if (size > chunks * CHUNK) {
        size = chunks * CHUNK;
    }
    ver = size >= 8 ? u16at(bs + 6) : 0;
    if (size < 8 || u16at(bs + 2) != FRAME_MAGIC || (ver != 2 && ver != 3)) {
        ok = 0;
    }
    pp = ver == 3 ? pred : NULL;
    q = u16at(bs + 4);
    b.d = bs + 8;
    b.n = size - 8;
    b.pos = 0;
    for (mx = 0; ok && mx < w / MACROBLOCK; mx++) {
        for (my = 0; ok && my < ph / MACROBLOCK; my++) {
            ok = block(&b, q, Cr + my * 8 * cw + mx * 8, cw, pp, 2) && block(&b, q, Cb + my * 8 * cw + mx * 8, cw, pp, 1);
            for (x = 0; ok && x < 4; x++) {
                ok = block(&b, q, Y + (my * MACROBLOCK + (x >> 1) * 8) * w + mx * MACROBLOCK + (x & 1) * 8, w, pp, 0);
            }
        }
    }
    if (ok) {
        for (row = 0; row < h; row++) {
            for (x = 0; x < w; x++) {
                y[row * w + x] = clamp8(Y[row * w + x] + LEVEL_SHIFT);
            }
        }
        for (row = 0; row < (h + 1) / 2; row++) {
            for (x = 0; x < cw; x++) {
                cb[row * cw + x] = clamp8(Cb[row * cw + x] + LEVEL_SHIFT);
                cr[row * cw + x] = clamp8(Cr[row * cw + x] + LEVEL_SHIFT);
            }
        }
    }
    free(bs);
    free(Y);
    free(Cb);
    free(Cr);
    return ok;
}
