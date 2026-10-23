/* The game's fonts (mfo.h). */
#include "mfo.h"

#include "png.h"
#include "tim.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define HEADER 0x10
#define PALETTE_ENTRY 8
#define PALETTE_COLORS 16
#define GLYPH_ENTRY 8

static uint16_t le16(const uint8_t *p) { return (uint16_t)(p[0] | p[1] << 8); }
static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }
static void put16(uint8_t *p, uint32_t v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); }
static void put32(uint8_t *p, uint32_t v) { put16(p, v); put16(p + 2, v >> 16); }

typedef struct Font {
    int cell_w, cell_h, unknown[2];
    uint32_t palette_bytes;
    int glyphs, palettes;
    size_t glyph_table, bitmaps;
} Font;

static int parse(Font *f, const uint8_t *d, size_t n) {
    int i;

    if (n < HEADER || memcmp(d, "MFO", 4) != 0) {
        return 0;
    }
    f->cell_w = d[4];
    f->cell_h = d[5];
    f->unknown[0] = d[6];
    f->unknown[1] = d[7];
    f->palette_bytes = le32(d + 8);
    f->glyphs = le16(d + 12);
    f->palettes = le16(d + 14);
    f->glyph_table = HEADER + f->palette_bytes;
    f->bitmaps = f->glyph_table + (size_t)f->glyphs * GLYPH_ENTRY;
    if (f->bitmaps > n || HEADER + (size_t)f->palettes * PALETTE_ENTRY > f->glyph_table) {
        return 0;
    }
    for (i = 0; i < f->palettes; i++) {
        const uint8_t *e = d + HEADER + i * PALETTE_ENTRY;
        if (HEADER + (size_t)f->palettes * PALETTE_ENTRY + le32(e) + PALETTE_COLORS * 2 > f->glyph_table) {
            return 0;
        }
    }
    for (i = 0; i < f->glyphs; i++) {
        const uint8_t *g = d + f->glyph_table + i * GLYPH_ENTRY;
        size_t row = (size_t)(g[5] + 1) / 2;
        if ((g[7] & 5) != 0 || f->bitmaps + le32(g) + row * g[6] > n || g[5] > f->cell_w || g[6] > f->cell_h) {
            return 0;   /* (only 4-bit glyphs within their cells: the game's two) */
        }
    }
    return 1;
}

int mfo_is(const uint8_t *d, size_t n) {
    Font f;
    return parse(&f, d, n);
}

int mfo_convert(const uint8_t *d, size_t n, const char *name, const char *what, MfoWriteFn write, void *ctx) {
    Font f;
    int i, x, y, rows, w, ok;
    uint8_t *idx, *pal, *clut, *png;
    size_t len, cap = 65536;
    char path[320], *txt;
    int k;

    if (!parse(&f, d, n)) {
        return 0;
    }
    rows = (f.glyphs + MFO_ATLAS_COLUMNS - 1) / MFO_ATLAS_COLUMNS;
    w = MFO_ATLAS_COLUMNS * f.cell_w;
    idx = calloc((size_t)w * rows * f.cell_h, 1);
    pal = calloc(PALETTE_COLORS, 4);
    clut = calloc((size_t)PALETTE_COLORS * f.palettes, 4);
    for (i = 0; i < f.palettes; i++) {
        const uint8_t *c = d + HEADER + f.palettes * PALETTE_ENTRY + le32(d + HEADER + i * PALETTE_ENTRY);
        for (x = 0; x < PALETTE_COLORS; x++) {
            tim_color_rgba(le16(c + x * 2), clut + ((size_t)i * PALETTE_COLORS + x) * 4);
        }
    }
    memcpy(pal, clut, PALETTE_COLORS * 4);
    txt = malloc(cap);
    k = snprintf(txt, cap,
                 "\\ %s: a font (data/mfo.h)\nom-font 1\ncell %d %d\nheader %d %d\npalettes %d\nglyphs %d\nsize %zu\n"
                 "\\ glyph <i> width <w> height <h> flags <f> at <offset>\n",
                 what, f.cell_w, f.cell_h, f.unknown[0], f.unknown[1], f.palettes, f.glyphs, n);
    for (i = 0; i < f.palettes; i++) {
        k += snprintf(txt + k, cap - (size_t)k, "palette %d at %u\n", i, le32(d + HEADER + i * PALETTE_ENTRY));
    }
    for (i = 0; i < f.glyphs; i++) {
        const uint8_t *g = d + f.glyph_table + i * GLYPH_ENTRY, *bm = d + f.bitmaps + le32(g);
        int gw = g[5], gh = g[6], row = (gw + 1) / 2, ox = (i % MFO_ATLAS_COLUMNS) * f.cell_w,
            oy = (i / MFO_ATLAS_COLUMNS) * f.cell_h;

        for (y = 0; y < gh; y++) {
            for (x = 0; x < gw; x++) {
                uint8_t b = bm[y * row + x / 2];
                idx[(size_t)(oy + y) * w + ox + x] = x & 1 ? b >> 4 : b & 15;
            }
        }
        k += snprintf(txt + k, cap - (size_t)k, "glyph %d width %d height %d flags %d at %u\n", i, gw, gh, g[7], le32(g));
    }
    png = png_encode_indexed(idx, w, rows * f.cell_h, 4, pal, &len);
    snprintf(path, sizeof(path), "%s.font.png", name);
    ok = png != NULL && write(ctx, path, png, len);
    free(png);
    png = png_encode_rgba(clut, PALETTE_COLORS, f.palettes, &len);
    snprintf(path, sizeof(path), "%s.font.clut.png", name);
    ok = ok && png != NULL && write(ctx, path, png, len);
    free(png);
    snprintf(path, sizeof(path), "%s.font.txt", name);
    ok = ok && write(ctx, path, (const uint8_t *)txt, (size_t)k);
    free(txt);
    free(idx);
    free(pal);
    free(clut);
    return ok;
}

uint8_t *mfo_rebuild(const char *name, MfoReadFn read, void *ctx, size_t *size) {
    char path[320], *txt, *line, *next;
    size_t n = 0, len;
    uint8_t *file, *out = NULL, *idx = NULL, *clut = NULL;
    int cw = 0, ch = 0, u0 = 0, u1 = 0, palettes = 0, glyphs = 0, aw = 0, ah = 0, depth = 0, cl_w = 0, cl_h = 0;
    size_t glyph_table = 0, bitmaps = 0;
    int ok = 1;

    snprintf(path, sizeof(path), "%s.font.txt", name);
    file = read(ctx, path, &len);
    if (file == NULL) {
        return NULL;
    }
    txt = malloc(len + 1);
    memcpy(txt, file, len);
    txt[len] = 0;
    free(file);
    snprintf(path, sizeof(path), "%s.font.png", name);
    file = read(ctx, path, &len);
    idx = file != NULL ? png_read_indexed(file, len, &aw, &ah, &depth) : NULL;
    free(file);
    snprintf(path, sizeof(path), "%s.font.clut.png", name);
    file = read(ctx, path, &len);
    clut = file != NULL ? png_read_rgba(file, len, &cl_w, &cl_h) : NULL;
    free(file);
    if (idx == NULL || clut == NULL || cl_w != PALETTE_COLORS) {
        ok = 0;
    }
    for (line = txt; ok && line != NULL; line = next) {
        int i, a, b, c, at;
        unsigned off;
        size_t nn;

        next = strchr(line, '\n');
        if (next != NULL) {
            *next++ = 0;
        }
        if (sscanf(line, "cell %d %d", &a, &b) == 2) {
            cw = a, ch = b;
        } else if (sscanf(line, "header %d %d", &a, &b) == 2) {
            u0 = a, u1 = b;
        } else if (sscanf(line, "palettes %d", &a) == 1) {
            palettes = a;
        } else if (sscanf(line, "glyphs %d", &a) == 1) {
            glyphs = a;
        } else if (sscanf(line, "size %zu", &nn) == 1) {
            n = nn;
            out = calloc(n, 1);
            memcpy(out, "MFO", 4);
            out[4] = (uint8_t)cw, out[5] = (uint8_t)ch, out[6] = (uint8_t)u0, out[7] = (uint8_t)u1;
            put16(out + 12, (uint32_t)glyphs);
            put16(out + 14, (uint32_t)palettes);
        } else if (sscanf(line, "palette %d at %u", &i, &off) == 2) {
            size_t at2 = HEADER + (size_t)palettes * PALETTE_ENTRY + off;
            if (out == NULL || i >= cl_h || at2 + PALETTE_COLORS * 2 > n) {
                ok = 0;
                break;
            }
            put32(out + HEADER + i * PALETTE_ENTRY, off);
            for (at = 0; at < PALETTE_COLORS; at++) {
                put16(out + at2 + at * 2, tim_rgba_color(clut + ((size_t)i * PALETTE_COLORS + at) * 4));
            }
            if (at2 + PALETTE_COLORS * 2 > glyph_table) {
                glyph_table = at2 + PALETTE_COLORS * 2;   /* (the glyph table follows the last palette) */
            }
        } else if (sscanf(line, "glyph %d width %d height %d flags %d at %u", &i, &a, &b, &c, &off) == 5) {
            int x, y, row = (a + 1) / 2, ox = (i % MFO_ATLAS_COLUMNS) * cw, oy = (i / MFO_ATLAS_COLUMNS) * ch;
            uint8_t *g, *bm;

            if (out == NULL || i >= glyphs) {
                ok = 0;
                break;
            }
            if (bitmaps == 0) {
                put32(out + 8, (uint32_t)(glyph_table - HEADER));
                bitmaps = glyph_table + (size_t)glyphs * GLYPH_ENTRY;
            }
            g = out + glyph_table + i * GLYPH_ENTRY;
            if (bitmaps + off + (size_t)row * b > n || ox + a > aw || oy + b > ah) {
                ok = 0;
                break;
            }
            put32(g, off);
            g[5] = (uint8_t)a, g[6] = (uint8_t)b, g[7] = (uint8_t)c;
            bm = out + bitmaps + off;
            for (y = 0; y < b; y++) {
                for (x = 0; x < a; x++) {
                    uint8_t v = idx[(size_t)(oy + y) * aw + ox + x] & 15;
                    bm[y * row + x / 2] |= x & 1 ? (uint8_t)(v << 4) : v;
                }
            }
        }
    }
    free(txt);
    free(idx);
    free(clut);
    if (!ok || out == NULL) {
        free(out);
        return NULL;
    }
    *size = n;
    return out;
}
