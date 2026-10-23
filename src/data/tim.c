/* TIM images and their library form (tim.h). */
#include "tim.h"

#include "png.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define TIM_MAGIC 0x10
#define FLAG_DEPTH 0x7          /* 0: 4 bits, 1: 8, 2: 16, 3: 24 */
#define FLAG_HAS_CLUT 0x8
#define BLOCK_HEADER 12         /* a block's bytes (u32), then x, y, w, h (u16) */
#define COLOR_BLEND_BIT 0x8000
#define SIDECAR_FORMAT "om-tim 1"

static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }
static uint16_t le16(const uint8_t *p) { return (uint16_t)(p[0] | p[1] << 8); }
static void put32(uint8_t *p, uint32_t v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); p[2] = (uint8_t)(v >> 16); p[3] = (uint8_t)(v >> 24); }
static void put16(uint8_t *p, uint16_t v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); }

static const int kDepths[4] = {4, 8, 16, 24};

/* pixels across from the image block's width in 16-bit units */
static int pixel_width(int depth, int units) {
    return depth == 4 ? units * 4 : depth == 8 ? units * 2 : depth == 16 ? units : units * 2 / 3;
}

int tim_parse(Tim *t, const uint8_t *d, size_t n) {
    uint32_t flags, len;
    size_t at = 8;

    memset(t, 0, sizeof(*t));
    if (n < 8 + BLOCK_HEADER || le32(d) != TIM_MAGIC) {
        return 0;
    }
    flags = le32(d + 4);
    if ((flags & FLAG_DEPTH) > 3 || (flags & ~(uint32_t)(FLAG_DEPTH | FLAG_HAS_CLUT)) != 0) {
        return 0;
    }
    t->depth = kDepths[flags & FLAG_DEPTH];
    t->has_clut = (flags & FLAG_HAS_CLUT) != 0;
    if (t->has_clut) {
        len = le32(d + at);
        if (len < BLOCK_HEADER || at + len > n) {
            return 0;
        }
        t->clut_x = le16(d + at + 4);
        t->clut_y = le16(d + at + 6);
        t->clut_w = le16(d + at + 8);
        t->clut_h = le16(d + at + 10);
        if (BLOCK_HEADER + (size_t)t->clut_w * t->clut_h * 2 != len) {
            return 0;
        }
        t->clut = d + at + BLOCK_HEADER;
        at += len;
    }
    if (at + BLOCK_HEADER > n) {
        return 0;
    }
    len = le32(d + at);
    if (len < BLOCK_HEADER || at + len > n) {
        return 0;
    }
    t->pix_x = le16(d + at + 4);
    t->pix_y = le16(d + at + 6);
    t->pix_w = le16(d + at + 8);
    t->pix_h = le16(d + at + 10);
    if (BLOCK_HEADER + (size_t)t->pix_w * t->pix_h * 2 != len) {
        return 0;
    }
    t->pixels = d + at + BLOCK_HEADER;
    t->width = pixel_width(t->depth, t->pix_w);
    t->height = t->pix_h;
    t->size = at + len;
    return t->width > 0 && t->height > 0;
}

void tim_color_rgba(uint16_t c, uint8_t *rgba) {
    int r = c & 0x1F, g = c >> 5 & 0x1F, b = c >> 10 & 0x1F;

    rgba[0] = (uint8_t)(r << 3 | r >> 2);
    rgba[1] = (uint8_t)(g << 3 | g >> 2);
    rgba[2] = (uint8_t)(b << 3 | b >> 2);
    rgba[3] = c == 0 ? TIM_ALPHA_CLEAR : (c & COLOR_BLEND_BIT) ? TIM_ALPHA_BLEND : TIM_ALPHA_SOLID;
}

uint16_t tim_rgba_color(const uint8_t *rgba) {
    uint16_t c;

    if (rgba[3] < TIM_ALPHA_BLEND / 2) {
        return 0;
    }
    c = (uint16_t)(rgba[0] >> 3 | (rgba[1] >> 3) << 5 | (rgba[2] >> 3) << 10);
    return rgba[3] < (TIM_ALPHA_BLEND + TIM_ALPHA_SOLID) / 2 ? (uint16_t)(c | COLOR_BLEND_BIT) : c;
}

/* the index of pixel i (4 bits: the low nibble first) */
static uint8_t index_at(const Tim *t, size_t i) {
    return t->depth == 8 ? t->pixels[i] : (uint8_t)(t->pixels[i / 2] >> (i % 2 ? 4 : 0) & 0xF);
}

int tim_convert(const Tim *t, const char *name, TimWriteFn write, void *ctx) {
    char path[512], text[256];
    size_t i, n = (size_t)t->width * t->height, len = 0;
    uint8_t *png = NULL;
    int ok;

    if (t->depth <= 8) {
        uint8_t *idx = malloc(n), palette[256 * 4];
        int colors = 1 << t->depth, k;

        for (i = 0; i < n; i++) {
            idx[i] = index_at(t, i);
        }
        for (k = 0; k < colors; k++) {   /* (the first row of the block; grey ramp if it has none) */
            if (t->has_clut && k < t->clut_w && t->clut_h > 0) {
                tim_color_rgba(le16(t->clut + k * 2), palette + k * 4);
            } else {
                uint8_t v = (uint8_t)(k * 255 / (colors - 1));
                palette[k * 4] = palette[k * 4 + 1] = palette[k * 4 + 2] = v;
                palette[k * 4 + 3] = TIM_ALPHA_SOLID;
            }
        }
        png = png_encode_indexed(idx, t->width, t->height, t->depth, palette, &len);
        free(idx);
    } else {
        uint8_t *rgba = malloc(n * 4);

        for (i = 0; i < n; i++) {
            if (t->depth == 16) {
                tim_color_rgba(le16(t->pixels + i * 2), rgba + i * 4);
            } else {
                memcpy(rgba + i * 4, t->pixels + i * 3, 3);
                rgba[i * 4 + 3] = TIM_ALPHA_SOLID;
            }
        }
        png = png_encode_rgba(rgba, t->width, t->height, &len);
        free(rgba);
    }
    snprintf(path, sizeof(path), "%s.png", name);
    ok = png != NULL && write(ctx, path, png, len);
    free(png);
    if (ok && t->has_clut) {
        size_t colors = (size_t)t->clut_w * t->clut_h;
        uint8_t *rgba = malloc(colors * 4);

        for (i = 0; i < colors; i++) {
            tim_color_rgba(le16(t->clut + i * 2), rgba + i * 4);
        }
        png = png_encode_rgba(rgba, t->clut_w, t->clut_h, &len);
        free(rgba);
        snprintf(path, sizeof(path), "%s.clut.png", name);
        ok = png != NULL && write(ctx, path, png, len);
        free(png);
    }
    if (ok) {
        int k = snprintf(text, sizeof(text), "%s\ndepth %d\npixels %u %u %u %u\n", SIDECAR_FORMAT, t->depth,
                         t->pix_x, t->pix_y, t->pix_w, t->pix_h);
        if (t->has_clut) {
            k += snprintf(text + k, sizeof(text) - (size_t)k, "clut %u %u %u %u\n", t->clut_x, t->clut_y, t->clut_w, t->clut_h);
        }
        snprintf(path, sizeof(path), "%s.tim.txt", name);
        ok = write(ctx, path, (const uint8_t *)text, (size_t)k);
    }
    return ok;
}

uint8_t *tim_rebuild(const char *name, TimReadFn read, void *ctx, size_t *size) {
    char path[512];
    size_t n = 0, cn = 0, len, i;
    uint8_t *side, *png, *out = NULL;
    unsigned depth = 0, px, py, pw, ph, cx = 0, cy = 0, cw = 0, ch = 0;
    int has_clut, w = 0, h = 0, d = 0, flag;
    const char *clut_line;

    snprintf(path, sizeof(path), "%s.tim.txt", name);
    side = read(ctx, path, &n);
    if (side == NULL || strncmp((char *)side, SIDECAR_FORMAT, strlen(SIDECAR_FORMAT)) != 0 ||
        sscanf((char *)side, SIDECAR_FORMAT "\ndepth %u\npixels %u %u %u %u", &depth, &px, &py, &pw, &ph) != 5) {
        free(side);
        return NULL;
    }
    clut_line = strstr((char *)side, "\nclut ");
    has_clut = clut_line != NULL && sscanf(clut_line, "\nclut %u %u %u %u", &cx, &cy, &cw, &ch) == 4;
    free(side);
    for (flag = 0; flag < 4 && kDepths[flag] != (int)depth; flag++) {
    }
    if (flag == 4) {
        return NULL;
    }
    len = 8 + (has_clut ? BLOCK_HEADER + (size_t)cw * ch * 2 : 0) + BLOCK_HEADER + (size_t)pw * ph * 2;
    out = calloc(1, len);
    put32(out, TIM_MAGIC);
    put32(out + 4, (uint32_t)flag | (has_clut ? FLAG_HAS_CLUT : 0));
    i = 8;
    if (has_clut) {
        int cw_read, ch_read;
        uint8_t *rgba;

        snprintf(path, sizeof(path), "%s.clut.png", name);
        png = read(ctx, path, &cn);
        rgba = png != NULL ? png_read_rgba(png, cn, &cw_read, &ch_read) : NULL;
        free(png);
        if (rgba == NULL || cw_read != (int)cw || ch_read != (int)ch) {
            free(rgba);
            free(out);
            return NULL;
        }
        put32(out + i, (uint32_t)(BLOCK_HEADER + cw * ch * 2));
        put16(out + i + 4, (uint16_t)cx);
        put16(out + i + 6, (uint16_t)cy);
        put16(out + i + 8, (uint16_t)cw);
        put16(out + i + 10, (uint16_t)ch);
        for (n = 0; n < (size_t)cw * ch; n++) {
            put16(out + i + BLOCK_HEADER + n * 2, tim_rgba_color(rgba + n * 4));
        }
        free(rgba);
        i += BLOCK_HEADER + (size_t)cw * ch * 2;
    }
    put32(out + i, (uint32_t)(BLOCK_HEADER + pw * ph * 2));
    put16(out + i + 4, (uint16_t)px);
    put16(out + i + 6, (uint16_t)py);
    put16(out + i + 8, (uint16_t)pw);
    put16(out + i + 10, (uint16_t)ph);
    i += BLOCK_HEADER;
    snprintf(path, sizeof(path), "%s.png", name);
    png = read(ctx, path, &n);
    if (png == NULL) {
        free(out);
        return NULL;
    }
    if (depth <= 8) {
        uint8_t *idx = png_read_indexed(png, n, &w, &h, &d);
        size_t k;

        if (idx == NULL || w != pixel_width((int)depth, (int)pw) || h != (int)ph || d != (int)depth) {
            free(idx);
            free(png);
            free(out);
            return NULL;
        }
        for (k = 0; k < (size_t)w * h; k++) {
            if (depth == 8) {
                out[i + k] = idx[k];
            } else {
                out[i + k / 2] |= (uint8_t)((idx[k] & 0xF) << (k % 2 ? 4 : 0));
            }
        }
        free(idx);
    } else {
        uint8_t *rgba = png_read_rgba(png, n, &w, &h);
        size_t k;

        if (rgba == NULL || w != pixel_width((int)depth, (int)pw) || h != (int)ph) {
            free(rgba);
            free(png);
            free(out);
            return NULL;
        }
        for (k = 0; k < (size_t)w * h; k++) {
            if (depth == 16) {
                put16(out + i + k * 2, tim_rgba_color(rgba + k * 4));
            } else {
                memcpy(out + i + k * 3, rgba + k * 4, 3);
            }
        }
        free(rgba);
    }
    free(png);
    *size = len;
    return out;
}
