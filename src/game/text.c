/* The game's text and fonts for the scripts (engine words; docs/forth.md, docs/subsystems/window.md).
 *
 * Text: the library's text files (data/text.h: `#id` then the message's lines), a message by bank and id:
 *   bank 0..32  an area's (cdbin/NNN_4/4.txt), 33 the common one (cdbin/033_4/4.txt);
 *   bank 34     the list menus' labels (cdbin/033_4/6.txt: in the list font's own codes - drawn with font 1);
 *   bank 200+   an event pack's (cdbin/NNN_T.txt, the pack by its CD.BIN entry).
 * The game's fonts (data/mfo.h: cdbin/033_4/5/3 the dialogue font, 5/4 the list font) drawn as the game drew
 * them, a glyph its own width (the 2021 patch's proportional font), lines 16 pixels apart, in one of the font's
 * palettes (its colours). */
#include "engine.h"

#include "../core/files.h"
#include "../data/png.h"
#include "../render/render.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define PRIM(name) static void name(Forth *f, Word *w)
#define POP() forth_pop(f)
#define PUSH(x) forth_push(f, (Cell)(x))

#define AREA_BANKS 33          /* areas 0..32 */
#define COMMON_BANK 33
#define LIST_BANK 34           /* the list menus' labels */
#define PACK_BANKS 200         /* from here: an event pack's, by its CD.BIN entry */
#define ENTRY_KINDS 5          /* CD.BIN: 5 entries a group */
#define MAX_FILES 128

/* ---- text files ---- */
typedef struct TextFile {
    int bank;
    char *data;      /* the file */
    long *at;        /* message id -> its first line's offset (-1: none) */
    int ids;
} TextFile;

static TextFile sFiles[MAX_FILES];
static int sNFiles;
static char *sMessage;   /* the last message asked for, unescaped */

static int bank_path(int bank, char *path, size_t n) {
    if (bank >= 0 && bank <= COMMON_BANK) {
        snprintf(path, n, "cdbin/%03d_4/4.txt", bank);
    } else if (bank == LIST_BANK) {
        snprintf(path, n, "cdbin/%03d_4/6.txt", COMMON_BANK);
    } else if (bank >= PACK_BANKS) {
        snprintf(path, n, "cdbin/%03d_%d.txt", bank / ENTRY_KINDS, bank % ENTRY_KINDS);
    } else {
        return 0;
    }
    return 1;
}

static TextFile *text_file(int bank) {
    char path[64];
    size_t size;
    char *d, *p;
    TextFile *t;
    int i;

    for (i = 0; i < sNFiles; i++) {
        if (sFiles[i].bank == bank) {
            return &sFiles[i];
        }
    }
    if (!bank_path(bank, path, sizeof(path)) || sNFiles == MAX_FILES || (d = (char *)files_read(path, &size)) == NULL) {
        return NULL;
    }
    t = &sFiles[sNFiles++];
    t->bank = bank;
    t->data = d;
    t->ids = 0;
    t->at = NULL;
    for (p = d; *p; p = strchr(p, '\n') ? strchr(p, '\n') + 1 : p + strlen(p)) {
        if (p[0] == '#') {
            int id = atoi(p + 1);
            if (id >= t->ids) {
                int was = t->ids;
                t->ids = id + 64;
                t->at = realloc(t->at, sizeof(long) * (size_t)t->ids);
                for (i = was; i < t->ids; i++) {
                    t->at[i] = -1;
                }
            }
            t->at[id] = (strchr(p, '\n') ? strchr(p, '\n') + 1 : p + strlen(p)) - d;
        }
    }
    return t;
}

/* message `id` of `bank`: its lines joined by newlines, escapes undone (data/text.h); NULL if none */
static const char *text_message(int bank, int id, size_t *len) {
    TextFile *t = text_file(bank);
    const char *p;
    size_t n = 0;

    if (t == NULL || id < 0 || id >= t->ids || t->at[id] < 0) {
        return NULL;
    }
    free(sMessage);
    sMessage = malloc(strlen(t->data + t->at[id]) + 1);
    for (p = t->data + t->at[id]; *p && !(p[0] == '#' && (p == t->data || p[-1] == '\n')); p++) {
        if (p[0] == '\\' && p[1] != 0 && p[1] != '\n') {   /* \\ \{ \} \# */
            sMessage[n++] = *++p;
        } else {
            sMessage[n++] = *p;
        }
    }
    while (n > 0 && sMessage[n - 1] == '\n') {
        n--;
    }
    sMessage[n] = 0;
    *len = n;
    return sMessage;
}

/* ---- fonts ---- */
enum { DIALOGUE_FONT, LIST_FONT, FONTS };
#define MAX_GLYPHS 256
#define MAX_PALETTES 16
#define LINE_GAP 16            /* src: the windows' line pitch (func_80035C20 ..., 0x10) */
#define PALETTE_COLORS 16

typedef struct GameFont {
    int tried, ok;
    int cell_w, cell_h, glyphs, palettes, atlas_w, atlas_h;
    uint8_t width[MAX_GLYPHS], height[MAX_GLYPHS];
    uint8_t *index;                     /* the atlas's palette indices */
    uint8_t clut[MAX_PALETTES][PALETTE_COLORS][4];
    GpuTexture tex[MAX_PALETTES];       /* made as each palette is first drawn in */
} GameFont;

static GameFont sFonts[FONTS];
static const char *kFontNames[FONTS] = {"cdbin/033_4/5/3", "cdbin/033_4/5/4"};
static int sFont, sPalette;

static GameFont *font_get(int which) {
    GameFont *g = &sFonts[which];
    char path[96], *txt, *line;
    size_t n;
    uint8_t *file, *clut;
    int cw = 0, ch = 0, depth = 0, i;

    if (g->tried) {
        return g->ok ? g : NULL;
    }
    g->tried = 1;
    snprintf(path, sizeof(path), "%s.font.txt", kFontNames[which]);
    txt = (char *)files_read(path, &n);
    snprintf(path, sizeof(path), "%s.font.png", kFontNames[which]);
    file = files_read(path, &n);
    g->index = file != NULL ? png_read_indexed(file, n, &g->atlas_w, &g->atlas_h, &depth) : NULL;
    free(file);
    snprintf(path, sizeof(path), "%s.font.clut.png", kFontNames[which]);
    file = files_read(path, &n);
    clut = file != NULL ? png_read_rgba(file, n, &cw, &ch) : NULL;
    free(file);
    if (txt == NULL || g->index == NULL || clut == NULL || cw != PALETTE_COLORS) {
        free(txt);
        free(clut);
        return NULL;
    }
    g->palettes = ch < MAX_PALETTES ? ch : MAX_PALETTES;
    memcpy(g->clut, clut, (size_t)g->palettes * PALETTE_COLORS * 4);
    free(clut);
    for (line = txt; line && *line; line = strchr(line, '\n') ? strchr(line, '\n') + 1 : NULL) {
        int a, b, c;
        if (sscanf(line, "cell %d %d", &a, &b) == 2) {
            g->cell_w = a, g->cell_h = b;
        } else if (sscanf(line, "glyph %d width %d height %d", &i, &a, &c) == 3 && i >= 0 && i < MAX_GLYPHS) {
            g->width[i] = (uint8_t)a;
            g->height[i] = (uint8_t)c;
            if (i + 1 > g->glyphs) {
                g->glyphs = i + 1;
            }
        }
    }
    free(txt);
    g->ok = g->cell_w > 0 && g->glyphs > 0;
    return g->ok ? g : NULL;
}

static GpuTexture font_texture(GameFont *g, int palette) {
    if (palette < 0 || palette >= g->palettes) {
        palette = 0;
    }
    if (g->tex[palette].id == 0) {
        uint8_t *rgba = malloc((size_t)g->atlas_w * g->atlas_h * 4);
        size_t i;

        for (i = 0; i < (size_t)g->atlas_w * g->atlas_h; i++) {
            memcpy(rgba + i * 4, g->clut[palette][g->index[i] & 15], 4);
            if (rgba[i * 4 + 3] != 0) {
                rgba[i * 4 + 3] = 255;   /* (the font's colours are solid; the value 0 is clear) */
            }
        }
        g->tex[palette] = render_texture_rgba(rgba, g->atlas_w, g->atlas_h);
        free(rgba);
    }
    return g->tex[palette];
}

/* the glyph a character is (data/text.h: ASCII from ' ', then the pad's symbols and the dash); -1 a newline */
static int glyph_of(const unsigned char *s, int n, int *used) {
    static const char *kSymbols[] = {"\xE2\x96\xB3", "\xE2\x97\x8B", "\xE2\x9C\x95", "\xE2\x96\xA1", "\xE2\x80\x94"};
    int k;

    *used = 1;
    if (s[0] == '\n') {
        return -1;
    }
    if (s[0] >= 0x20 && s[0] < 0x7F) {
        return s[0] - 0x20;
    }
    for (k = 0; k < 5; k++) {
        if (n >= 3 && memcmp(s, kSymbols[k], 3) == 0) {
            *used = 3;
            return 0x5F + k;
        }
    }
    while (*used < n && (s[*used] & 0xC0) == 0x80) {   /* (anything else: a '?') */
        (*used)++;
    }
    return '?' - 0x20;
}

/* the widest line, in the font's pixels */
static int font_width(GameFont *g, const char *s, int n) {
    int x = 0, best = 0, i = 0, used, c;

    while (i < n) {
        c = glyph_of((const unsigned char *)s + i, n - i, &used);
        i += used;
        if (c < 0) {
            x = 0;
        } else if (c < g->glyphs) {
            x += g->width[c];
        }
        best = x > best ? x : best;
    }
    return best;
}

static void font_draw(GameFont *g, const char *s, int n, float x, float y, float scale, uint32_t rgba) {
    GpuTexture t = font_texture(g, sPalette);
    float cx = x;
    int i = 0, used, c;

    while (i < n) {
        c = glyph_of((const unsigned char *)s + i, n - i, &used);
        i += used;
        if (c < 0) {
            cx = x;
            y += LINE_GAP * scale;
        } else if (c < g->glyphs) {
            int col = c % 16, row = c / 16;   /* (16 glyphs a row in the atlas: data/mfo.h) */
            float u = (float)(col * g->cell_w), v = (float)(row * g->cell_h);
            render_image_uv(t, cx, y, g->width[c] * scale, g->height[c] * scale, u / t.w, v / t.h,
                            (u + g->width[c]) / t.w, (v + g->height[c]) / t.h, rgba);
            cx += g->width[c] * scale;
        }
    }
}

/* ---- the words ---- */
PRIM(p_text_of) {   /* ( bank id -- addr len ) a message, 0 0 if there is none */
    Cell id = POP(), bank = POP();
    size_t len = 0;
    const char *s = text_message((int)bank, (int)id, &len);

    PUSH(s != NULL ? s : "");
    PUSH(s != NULL ? (Cell)len : 0);
}
PRIM(p_font) { Cell which = POP(); sFont = which >= 0 && which < FONTS ? (int)which : 0; }   /* ( n -- ) */
PRIM(p_font_palette) { sPalette = (int)POP(); }   /* ( n -- ) */
PRIM(p_font_text) {   /* ( addr len x y -- ) at the pen's scale, tinted by its colour */
    Cell y = POP(), x = POP(), len = POP();
    const char *s = (const char *)POP();
    GameFont *g = font_get(sFont);

    if (g != NULL) {
        font_draw(g, s, (int)len, (float)x, (float)y, (float)gEngine.pen_scale, gEngine.pen);
    }
}
PRIM(p_font_width) {   /* ( addr len -- pixels ) the widest line, in the font's pixels (unscaled) */
    Cell len = POP();
    const char *s = (const char *)POP();
    GameFont *g = font_get(sFont);

    PUSH(g != NULL ? font_width(g, s, (int)len) : 0);
}
PRIM(p_font_ready) { PUSH(font_get(sFont) != NULL ? -1 : 0); }   /* ( -- flag ) the font is in the library */

void bind_text(Forth *f) {
    static const struct { const char *name; Code code; } kWords[] = {
        {"text-of", p_text_of}, {"font", p_font}, {"font-palette", p_font_palette}, {"font-text", p_font_text},
        {"font-width", p_font_width}, {"font-ready?", p_font_ready},
    };
    size_t i;

    for (i = 0; i < sizeof(kWords) / sizeof(kWords[0]); i++) {
        forth_prim(f, kWords[i].name, kWords[i].code);
    }
    forth_constant(f, "font-line", LINE_GAP);
}
