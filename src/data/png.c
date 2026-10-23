/* PNG files (png.h): RGBA through stb; indexed written and read here (on stb's zlib). */
#include "png.h"

#include "stb_image.h"
#include "stb_image_write.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* stb_image_write's deflate (defined there, not declared in its header) */
unsigned char *stbi_zlib_compress(unsigned char *data, int data_len, int *out_len, int quality);

#define COLOR_TYPE_INDEXED 3
#define ZLIB_QUALITY 8

typedef struct Buffer {
    uint8_t *data;
    size_t n, cap;
} Buffer;

static void put(Buffer *b, const void *data, size_t n) {
    if (b->n + n > b->cap) {
        b->cap = (b->n + n) * 2 + 64;
        b->data = realloc(b->data, b->cap);
    }
    memcpy(b->data + b->n, data, n);
    b->n += n;
}

static void put_to_buffer(void *ctx, void *data, int n) { put(ctx, data, (size_t)n); }

uint8_t *png_encode_rgba(const uint8_t *rgba, int w, int h, size_t *n) {
    Buffer b = {0};

    if (!stbi_write_png_to_func(put_to_buffer, &b, w, h, 4, rgba, w * 4)) {
        free(b.data);
        return NULL;
    }
    *n = b.n;
    return b.data;
}

static uint32_t crc_of(const uint8_t *p, size_t n, uint32_t crc) {
    static uint32_t table[256];
    size_t i;

    if (table[1] == 0) {
        for (i = 0; i < 256; i++) {
            uint32_t c = (uint32_t)i;
            int k;
            for (k = 0; k < 8; k++) {
                c = c & 1 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
            }
            table[i] = c;
        }
    }
    crc = ~crc;
    for (i = 0; i < n; i++) {
        crc = table[(crc ^ p[i]) & 0xFF] ^ (crc >> 8);
    }
    return ~crc;
}

static void put32be(uint8_t *p, uint32_t v) { p[0] = (uint8_t)(v >> 24); p[1] = (uint8_t)(v >> 16); p[2] = (uint8_t)(v >> 8); p[3] = (uint8_t)v; }
static uint32_t be32(const uint8_t *p) { return (uint32_t)p[0] << 24 | p[1] << 16 | p[2] << 8 | p[3]; }

static void chunk(Buffer *b, const char *type, const uint8_t *data, size_t n) {
    uint8_t head[8], tail[4];
    uint32_t crc;

    put32be(head, (uint32_t)n);
    memcpy(head + 4, type, 4);
    crc = crc_of(head + 4, 4, 0);
    crc = crc_of(data, n, crc);
    put32be(tail, crc);
    put(b, head, 8);
    if (n > 0) {
        put(b, data, n);
    }
    put(b, tail, 4);
}

uint8_t *png_encode_indexed(const uint8_t *idx, int w, int h, int depth, const uint8_t *palette, size_t *n) {
    static const uint8_t kSignature[8] = {0x89, 'P', 'N', 'G', '\r', '\n', 0x1A, '\n'};
    int colors = 1 << depth, row_bytes = (w * depth + 7) / 8, x, y, zlen = 0;
    uint8_t ihdr[13], plte[256 * 3], trns[256], *rows, *z;
    Buffer b = {0};

    put32be(ihdr, (uint32_t)w);
    put32be(ihdr + 4, (uint32_t)h);
    ihdr[8] = (uint8_t)depth;
    ihdr[9] = COLOR_TYPE_INDEXED;
    ihdr[10] = ihdr[11] = ihdr[12] = 0;   /* deflate, adaptive filtering, no interlace */
    for (x = 0; x < colors; x++) {
        memcpy(plte + x * 3, palette + x * 4, 3);
        trns[x] = palette[x * 4 + 3];
    }
    rows = calloc((size_t)(row_bytes + 1) * h, 1);
    for (y = 0; y < h; y++) {
        uint8_t *r = rows + (size_t)y * (row_bytes + 1) + 1;   /* (filter 0: none) */
        for (x = 0; x < w; x++) {
            uint8_t v = idx[(size_t)y * w + x];
            if (depth == 8) {
                r[x] = v;
            } else {
                r[x / 2] |= (uint8_t)((v & 0xF) << (x % 2 == 0 ? 4 : 0));   /* (the left pixel in the high bits) */
            }
        }
    }
    z = stbi_zlib_compress(rows, (row_bytes + 1) * h, &zlen, ZLIB_QUALITY);
    free(rows);
    if (z == NULL) {
        return NULL;
    }
    put(&b, kSignature, 8);
    chunk(&b, "IHDR", ihdr, 13);
    chunk(&b, "PLTE", plte, (size_t)colors * 3);
    chunk(&b, "tRNS", trns, (size_t)colors);
    chunk(&b, "IDAT", z, (size_t)zlen);
    chunk(&b, "IEND", NULL, 0);
    free(z);
    *n = b.n;
    return b.data;
}

uint8_t *png_read_rgba(const uint8_t *file, size_t n, int *w, int *h) {
    int comp;
    return stbi_load_from_memory(file, (int)n, w, h, &comp, 4);
}

static int paeth(int a, int b, int c) {
    int p = a + b - c, pa = abs(p - a), pb = abs(p - b), pc = abs(p - c);
    return pa <= pb && pa <= pc ? a : pb <= pc ? b : c;
}

uint8_t *png_read_indexed(const uint8_t *file, size_t n, int *w, int *h, int *depth) {
    size_t pos = 8, zn = 0;
    uint8_t *zdata = NULL, *idx = NULL;
    int width = 0, height = 0, bits = 0, type = -1, raw_n = 0, row_bytes, x, y;
    char *raw;
    uint8_t *prev, *cur;

    if (n < 8 || memcmp(file, "\x89PNG\r\n\x1A\n", 8) != 0) {
        return NULL;
    }
    while (pos + 12 <= n) {
        uint32_t len = be32(file + pos);
        const uint8_t *type_at = file + pos + 4, *data = file + pos + 8;

        if (pos + 12 + len > n) {
            break;
        }
        if (memcmp(type_at, "IHDR", 4) == 0 && len >= 13) {
            width = (int)be32(data);
            height = (int)be32(data + 4);
            bits = data[8];
            type = data[9];
        } else if (memcmp(type_at, "IDAT", 4) == 0) {
            zdata = realloc(zdata, zn + len);
            memcpy(zdata + zn, data, len);
            zn += len;
        } else if (memcmp(type_at, "IEND", 4) == 0) {
            break;
        }
        pos += 12 + len;
    }
    if (type != COLOR_TYPE_INDEXED || (bits != 4 && bits != 8) || width <= 0 || height <= 0 || zdata == NULL) {
        free(zdata);
        return NULL;
    }
    raw = stbi_zlib_decode_malloc((const char *)zdata, (int)zn, &raw_n);
    free(zdata);
    row_bytes = (width * bits + 7) / 8;
    if (raw == NULL || raw_n < (row_bytes + 1) * height) {
        free(raw);
        return NULL;
    }
    idx = malloc((size_t)width * height);
    prev = calloc((size_t)row_bytes, 1);
    for (y = 0; y < height; y++) {
        uint8_t filter = (uint8_t)raw[(size_t)y * (row_bytes + 1)];
        cur = (uint8_t *)raw + (size_t)y * (row_bytes + 1) + 1;
        for (x = 0; x < row_bytes; x++) {   /* (a pixel is one byte or less: the "left" byte is the one before) */
            int a = x > 0 ? cur[x - 1] : 0, b = prev[x], c = x > 0 ? prev[x - 1] : 0;
            switch (filter) {
            case 1: cur[x] = (uint8_t)(cur[x] + a); break;
            case 2: cur[x] = (uint8_t)(cur[x] + b); break;
            case 3: cur[x] = (uint8_t)(cur[x] + (a + b) / 2); break;
            case 4: cur[x] = (uint8_t)(cur[x] + paeth(a, b, c)); break;
            default: break;
            }
        }
        for (x = 0; x < width; x++) {
            idx[(size_t)y * width + x] = bits == 8 ? cur[x] : (uint8_t)(cur[x / 2] >> (x % 2 == 0 ? 4 : 0) & 0xF);
        }
        memcpy(prev, cur, (size_t)row_bytes);
    }
    free(prev);
    free(raw);
    *w = width;
    *h = height;
    *depth = bits;
    return idx;
}
