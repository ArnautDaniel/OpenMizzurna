/* The models in the library (modelconv.h). */
#include "modelconv.h"

#include "cdbin.h"
#include "container.h"
#include "modelgltf.h"
#include "png.h"
#include "tmd.h"
#include "town.h"
#include "vramctx.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* the executable's tables the game loads models and textures by (read here, by the import, only) */
#define EXE_HEADER 0x800
#define EXE_LOAD_ADDRESS 0x80010000u
#define KIND_MODELS 0x800959C8u        /* u8 a character kind: its model's CD.BIN entry, from CHARACTER_ENTRIES */
#define KIND_TEXTURES 0x80095A88u      /* u8 a kind: its texture's entry (0: none, its textures stay loaded) */
#define KINDS 0xC0
#define CHARACTER_ENTRIES 0x16E
#define AREA_CHARACTER_SHEET 0x80080550u   /* s16 an area: the CD.BIN entry of its character sheet */
#define SLOT_PALETTE_PLACE 0x8009590Cu     /* s16 x, y a character slot: where its palette goes */
#define SLOT_PALETTE_WORD 0x800958E0u      /* u16 a slot: the palette word its polygons get */
#define AREAS 33                           /* areas 0..32 (33 is the common data's group) */
#define COMMON_GROUP 33
#define SLOT_PAGE_WORD 0x15                /* slot 0's texture page (src: npc_model_stream, slot / 7 + 0x15) */
#define SLOT_X 0x140                       /* slot 0's image in video memory */
#define SLOT_Y 0x100
#define PAGE_KEEP_BITS 0x1E0               /* the patch keeps a polygon's blend mode and depth */
#define PALETTE_KEEP_BITS 0x3C1            /* and its palette's column and row within the block */
#define BAND_KEEP 0x1F                     /* and its v within the slot's 32-line band */
#define AREA_PROPS_PART 3                  /* NNN_4's part 3: the area's models */

/* the order the game uploads the common images in (src: func_800192DC, container 033_3's part 0) */
static const int kCommonImages[] = {3, 1, 4, 5, 0, 2};

static const uint8_t *exe_at(const uint8_t *exe, size_t n, uint32_t address, size_t bytes) {
    size_t at = EXE_HEADER + (address - EXE_LOAD_ADDRESS);
    return at + bytes <= n ? exe + at : NULL;
}

static int le16s(const uint8_t *p) { return (int16_t)(p[0] | p[1] << 8); }

/* a part of an entry, by a path of part numbers; NULL if it isn't there */
static const uint8_t *part_of(const uint8_t *d, size_t n, const int *path, int depth, size_t *size) {
    Container c;
    int i;

    for (i = 0; i < depth; i++) {
        if (d == NULL || !container_parse(&c, d, n) || path[i] >= c.n) {
            return NULL;
        }
        d = c.parts[path[i]].data;
        n = c.parts[path[i]].size;
    }
    *size = n;
    return d;
}

/* ---- the models ---- */
enum { MODEL_AREA, MODEL_STREAMED, MODEL_OTHER };

typedef struct Found {
    char id[64];          /* "001_4.3.1", "073_1" */
    char path[64];        /* its glTF, less ".gltf": "models/001_4.3.1", "town/03-05/terrain" */
    char comment[64];     /* where it's from */
    int terrain;          /* a TMD on its own (a town tile's ground) */
    const uint8_t *data;
    size_t size;
    int kind;
    int area;             /* MODEL_AREA: its area */
    int texture_entry;    /* MODEL_STREAMED: its texture's entry */
} Found;

typedef struct Models {
    Found *list;
    int n, cap;
} Models;

static void add(Models *ms, const char *id, const uint8_t *d, size_t n, int kind, int area, int texture_entry) {
    Found *f;

    if (ms->n == ms->cap) {
        ms->cap = ms->cap * 2 + 64;
        ms->list = realloc(ms->list, sizeof(Found) * (size_t)ms->cap);
    }
    f = &ms->list[ms->n++];
    snprintf(f->id, sizeof(f->id), "%s", id);
    snprintf(f->path, sizeof(f->path), "models/%s", id);
    snprintf(f->comment, sizeof(f->comment), "CD.BIN %s", id);
    f->terrain = 0;
    f->data = d;
    f->size = n;
    f->kind = kind;
    f->area = area;
    f->texture_entry = texture_entry;
}

static void walk(Models *ms, const uint8_t *d, size_t n, const char *id, int group, int top_part, int depth,
                 const int *texture_of) {
    Container c;
    int i;

    if (model_is(d, n)) {
        int entry = -1, kind = MODEL_OTHER, area = -1, texture = -1;

        if (depth == 0) {
            sscanf(id, "%d_%d", &group, &entry);
            entry = group * CDBIN_KINDS + entry;
            if (texture_of[entry] > 0) {
                kind = MODEL_STREAMED;
                texture = texture_of[entry];
            }
        } else if (group < AREAS && strstr(id, "_4.3.") != NULL && top_part == AREA_PROPS_PART) {
            kind = MODEL_AREA;
            area = group;
        }
        add(ms, id, d, n, kind, area, texture);
        return;
    }
    if (depth < 4 && container_parse(&c, d, n)) {
        for (i = 0; i < c.n; i++) {
            char sub[80];
            snprintf(sub, sizeof(sub), "%s.%d", id, i);
            walk(ms, c.parts[i].data, c.parts[i].size, sub, group, depth == 0 ? i : top_part, depth + 1, texture_of);
        }
    }
}

/* the town's tiles: their ground and objects, props of the town's area */
static void find_town(Models *ms, const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n) {
    const uint8_t *part[TOWN_TILE_PARTS];
    size_t size[TOWN_TILE_PARTS], n;
    char folder[32];
    int c, r, p;

    for (r = 0; r < TOWN_ROWS; r++) {
        for (c = 0; c < TOWN_COLUMNS; c++) {
            const uint8_t *d = town_tile(exe, exe_n, map, map_n, c, r, &n);

            if (d == NULL || !town_tile_parts(d, n, part, size)) {
                continue;
            }
            town_folder(c, r, folder, sizeof(folder));
            for (p = TOWN_TERRAIN; p <= TOWN_OBJECTS; p++) {
                Found *f;

                add(ms, folder, part[p], size[p], MODEL_AREA, TOWN_AREA, -1);
                f = &ms->list[ms->n - 1];
                snprintf(f->path, sizeof(f->path), "%s/%s", folder, p == TOWN_TERRAIN ? "terrain" : "objects");
                snprintf(f->comment, sizeof(f->comment), "MAP.BIN tile %d, %d: its %s", c, r,
                         p == TOWN_TERRAIN ? "ground" : "objects");
                f->terrain = p == TOWN_TERRAIN;
            }
        }
    }
}

static int read_found(const Found *f, Model *m) {
    return f->terrain ? model_read_tmd(m, f->data, f->size) : model_read(m, f->data, f->size);
}

static void find_models(Models *ms, const uint8_t *const *entries, const size_t *sizes, int n, const uint8_t *exe,
                        size_t exe_n) {
    const uint8_t *models = exe_at(exe, exe_n, KIND_MODELS, KINDS), *textures = exe_at(exe, exe_n, KIND_TEXTURES, KINDS);
    int *texture_of = calloc((size_t)n, sizeof(int)), i;

    for (i = 0; models != NULL && textures != NULL && i < KINDS; i++) {
        int entry = CHARACTER_ENTRIES + models[i];
        if (textures[i] != 0 && entry < n) {
            texture_of[entry] = CHARACTER_ENTRIES + textures[i];
        }
    }
    memset(ms, 0, sizeof(*ms));
    for (i = 0; i < n; i++) {
        char id[16];
        if (entries[i] == NULL) {
            continue;
        }
        snprintf(id, sizeof(id), "%03d_%d", i / CDBIN_KINDS, i % CDBIN_KINDS);
        walk(ms, entries[i], sizes[i], id, i / CDBIN_KINDS, -1, 0, texture_of);
    }
    free(texture_of);
}

/* ---- video memory ---- */
/* every street lamp's palette lit or not (src: func_800243F0's palette copies) */
static void lamps_set(VramContext *v, int lit) {
    int i;

    for (i = 0; i < LAMPS; i++) {
        memmove(&v->vram[LAMP_ROW][i * 16], &v->vram[LAMP_ROW][lit ? LAMP_LIT_X : LAMP_UNLIT_X], 16 * sizeof(uint16_t));
    }
}

static void area_context(VramContext *v, int area, const uint8_t *const *entries, const size_t *sizes, int n,
                         const uint8_t *exe, size_t exe_n) {
    size_t size;
    const uint8_t *d;
    int path[2] = {0, 0}, i, sheet;
    const uint8_t *sheets = exe_at(exe, exe_n, AREA_CHARACTER_SHEET, (size_t)(area + 1) * 2);
    int common = COMMON_GROUP * CDBIN_KINDS + 3, own = area * CDBIN_KINDS + 3;

    vram_clear(v);
    for (i = 0; i < (int)(sizeof(kCommonImages) / sizeof(kCommonImages[0])); i++) {
        path[1] = kCommonImages[i];
        d = common < n && entries[common] ? part_of(entries[common], sizes[common], path, 2, &size) : NULL;
        if (d != NULL) {
            vram_load_tim(v, d, size);
        }
    }
    path[1] = 0;   /* (the area's sheet: part 0's first part; the town's part 0 is its sheet itself) */
    d = own < n && entries[own] ? part_of(entries[own], sizes[own], path, 2, &size) : NULL;
    if (d == NULL && own < n && entries[own]) {
        d = part_of(entries[own], sizes[own], path, 1, &size);
    }
    if (d != NULL) {
        vram_load_tim(v, d, size);
    }
    sheet = sheets != NULL ? le16s(sheets + area * 2) : 0;
    if (sheet > 0 && sheet < n && entries[sheet] != NULL) {
        vram_load_tim(v, entries[sheet], sizes[sheet]);
    }
    if (area == TOWN_AREA) {
        lamps_set(v, 0);   /* (the lamps as by day) */
    }
}

/* a streamed character: its texture in slot 0, its polygons pointed there (src: func_8003BF38, func_8003D0F0) */
static int streamed_context(VramContext *v, Model *m, const uint8_t *tim, size_t tim_n, const uint8_t *exe, size_t exe_n) {
    const uint8_t *place = exe_at(exe, exe_n, SLOT_PALETTE_PLACE, 4), *word = exe_at(exe, exe_n, SLOT_PALETTE_WORD, 2);
    uint16_t palette;
    int i, k, c;

    if (place == NULL || word == NULL) {
        return 0;
    }
    vram_clear(v);
    if (!vram_load_tim_at(v, tim, tim_n, SLOT_X, SLOT_Y, le16s(place), le16s(place + 2))) {
        return 0;
    }
    palette = (uint16_t)(word[0] | word[1] << 8);
    for (i = 0; i < m->nobjects; i++) {
        for (k = 0; k < m->objects[i].npolygons; k++) {
            ModelPolygon *p = &m->objects[i].polygons[k];
            if (!p->textured) {
                continue;
            }
            p->texture_page = (uint16_t)((p->texture_page & PAGE_KEEP_BITS) | SLOT_PAGE_WORD);
            p->palette = (uint16_t)((p->palette & PALETTE_KEEP_BITS) | palette);
            for (c = 0; c < p->corners; c++) {
                p->uv[c][1] &= BAND_KEEP;
            }
        }
    }
    return 1;
}

/* the page / palette words a texture name stands for (the reverse of vram_texture_name) */
static int texture_words(const char *name, uint16_t *page, uint16_t *palette) {
    int index, depth, px = 0, py = 0;

    if (sscanf(name, "page%d-%dbit-palette%d-%d", &index, &depth, &px, &py) == 4 ||
        sscanf(name, "page%d-%dbit", &index, &depth) == 2) {
        *page = (uint16_t)((index & 0xF) | (index >> 4 & 1) << 4 | (depth == 4 ? 0 : depth == 8 ? 1 : 2) << 7);
        *palette = (uint16_t)((px / 16) | py << 6);
        return 1;
    }
    return 0;
}

/* ---- writing and checking ---- */
typedef struct Job {
    ModelconvWriteFn write;
    ModelconvReadFn read;
    void *ctx;
    char *why;
    size_t nwhy;
    int models, textures, failed;
    uint8_t *page;   /* PAGE_TEXELS^2 RGBA */
} Job;

static uint64_t hash_of(const uint8_t *d, size_t n) {
    uint64_t h = 1469598103934665603ull;
    size_t i;
    for (i = 0; i < n; i++) {
        h = (h ^ d[i]) * 1099511628211ull;
    }
    return h;
}

/* a texture page written to (or checked against) path; the page is in j->page */
static void image_file(Job *j, const char *path, const uint8_t *image, int iw, int ih) {
    size_t n;

    if (j->failed) {
        return;
    }
    if (j->write != NULL) {
        uint8_t *png = png_encode_rgba(image, iw, ih, &n);
        if (png == NULL || !j->write(j->ctx, path, png, n)) {
            snprintf(j->why, j->nwhy, "can't write %s", path);
            j->failed = 1;
        }
        free(png);
    } else {
        uint8_t *file = j->read(j->ctx, path, &n), *rgba;
        int w = 0, h = 0;

        rgba = file != NULL ? png_read_rgba(file, n, &w, &h) : NULL;
        if (rgba == NULL || w != iw || h != ih || memcmp(rgba, image, (size_t)w * h * 4) != 0) {
            snprintf(j->why, j->nwhy, "%s isn't the texture video memory held", path);
            j->failed = 1;
        }
        free(rgba);
        free(file);
    }
    j->textures++;
}

static void image_file_text(Job *j, const char *path, const char *text, size_t n) {   /* (a text beside the images) */
    size_t got = 0;
    uint8_t *file;

    if (j->failed) {
        return;
    }
    if (j->write != NULL) {
        if (!j->write(j->ctx, path, (const uint8_t *)text, n)) {
            snprintf(j->why, j->nwhy, "can't write %s", path);
            j->failed = 1;
        }
        return;
    }
    file = j->read(j->ctx, path, &got);
    if (file == NULL || got != n || memcmp(file, text, n) != 0) {
        snprintf(j->why, j->nwhy, "%s isn't what the import writes", path);
        j->failed = 1;
    }
    free(file);
}

static void texture_file(Job *j, const char *path) { image_file(j, path, j->page, PAGE_TEXELS, PAGE_TEXELS); }

/* the sky's pieces (src: func_8001E2E4, func_8001E5FC, sky.c): where in video memory each image is - its
 * page (depth, x, y: the texture page word the game made) and palette - and its rectangle there */
typedef struct SkyPiece {
    const char *name;
    int depth, page_x, page_y, palette_x, palette_y, u, v, w, h;
} SkyPiece;
static const SkyPiece kSky[] = {
    {"clouds", 4, 0x340, 0, 0x340, 0xF4, 0, 0, 256, 240},
    {"horizon", 4, 0x2C0, 0, 0x2D0, 0xFE, 0, 0, 256, 64},
    {"sun", 8, 0x2C0, 0x40, 0x2C0, 0xFF, 0x00, 0x40, 0x40, 0x40},
    {"flare-1", 4, 0x2E0, 0x78, 0x2D0, 0xF8, 0x80, 0x78, 8, 8}, {"flare-2", 4, 0x2E0, 0x78, 0x2D0, 0xF8, 0x80, 0x78, 8, 8},
    {"flare-3", 4, 0x2E0, 0x78, 0x2D0, 0xF8, 0x80, 0x78, 8, 8}, {"flare-4", 4, 0x2E0, 0x78, 0x2D0, 0xF8, 0x80, 0x78, 8, 8},
    {"flare-5", 4, 0x2E2, 0x78, 0x2D0, 0xF9, 0x88, 0x78, 8, 8}, {"flare-6", 4, 0x2E2, 0x78, 0x2D0, 0xF9, 0x88, 0x78, 8, 8},
    {"flare-7", 4, 0x2E4, 0x78, 0x2E0, 0xFE, 0x90, 0x78, 8, 8}, {"flare-8", 4, 0x2EC, 0x58, 0x2C0, 0xFC, 0xB0, 0x58, 16, 16},
    {"flare-9", 4, 0x2E6, 0x78, 0x2D0, 0xFB, 0x98, 0x78, 8, 8}, {"flare-10", 4, 0x2E8, 0x58, 0x2D0, 0xFC, 0xA0, 0x58, 8, 8},
    {"flare-11", 4, 0x2E0, 0x40, 0x2C0, 0xF8, 0x80, 0x40, 32, 32}, {"flare-12", 4, 0x2EC, 0x68, 0x2C0, 0xFD, 0xB0, 0x68, 16, 16},
    {"flare-13", 4, 0x2EE, 0x40, 0x2C0, 0xFE, 0xB8, 0x40, 8, 16}, {"flare-14", 4, 0x2EE, 0x40, 0x2C0, 0xFE, 0xB8, 0x40, 8, 16},
    {"moon-1", 4, 0x2E0, 0x60, 0x2C0, 0xF9, 0x80, 0x60, 24, 24},   /* (its phases: to the 27th, to the 30th, the 31st) */
    {"moon-2", 4, 0x2E0, 0x60, 0x2C0, 0xF9, 0x98, 0x60, 24, 24},
    {"moon-3", 4, 0x2E0, 0x60, 0x2C0, 0xF9, 0xA0, 0x40, 24, 24},
    {"snow", 4, 0x380, 0, 0x350, 0xFE, 0, 0, 256, 64},   /* (the flakes: a column by nearness, a row by their fall) */
};

/* the town's billboard sprites (027_4's part 7: 12 bytes a sprite, {s16 w, h, u, v, texture page, palette}; the
 * page's top bit: drawn at full brightness; src billboard_draw): town/sprites.txt - each sprite's size, whether
 * it is drawn at full brightness, how its texels marked to blend do (1 half, 2 add, 3 subtract, 4 quarter) - and
 * town/sprites/<i>.png */
#define SPRITES_PART 7
#define SPRITE_SIZE 12
#define FULL_BRIGHT 0x8000
#define PAGE_BLEND_SHIFT 5

static void area_sprites(Job *j, const VramContext *v, const uint8_t *const *entries, const size_t *sizes, int n,
                         int area, const char *list, const char *folder) {
    int own = area * CDBIN_KINDS + 4, path[1] = {SPRITES_PART}, i, y, count;
    size_t size = 0, k = 0, cap;
    const uint8_t *d = own < n && entries[own] ? part_of(entries[own], sizes[own], path, 1, &size) : NULL;
    uint8_t *image;
    char *text, file[64];

    if (d == NULL || size < SPRITE_SIZE) {
        return;
    }
    image = malloc((size_t)PAGE_TEXELS * PAGE_TEXELS * 4);
    count = (int)(size / SPRITE_SIZE);
    cap = (size_t)(count + 4) * 96;
    text = malloc(cap);
    k += (size_t)snprintf(text + k, cap - k, "om-sprites 1\nsprites %d\n", count);
    for (i = 0; i < count && !j->failed; i++) {
        const uint8_t *e = d + i * SPRITE_SIZE;
        int w = le16s(e), h = le16s(e + 2), u = le16s(e + 4), vv = le16s(e + 6);
        uint16_t page = (uint16_t)(e[8] | e[9] << 8), palette = (uint16_t)(e[10] | e[11] << 8);

        if (w <= 0 || h <= 0 || u + w > PAGE_TEXELS || w > PAGE_TEXELS || h > PAGE_TEXELS) {
            w = h = 1, u = vv = 0;   /* (an entry that isn't a picture: a point, so its rows keep their numbers) */
        }
        k += (size_t)snprintf(text + k, cap - k, "sprite %d size %d %d full-bright %d blend %d\n", i, w, h,
                              (page & FULL_BRIGHT) != 0, (page >> PAGE_BLEND_SHIFT & 3) + 1);
        vram_page_rgba(v, (uint16_t)(page & ~FULL_BRIGHT), palette, j->page);
        for (y = 0; y < h; y++) {
            memcpy(image + (size_t)y * w * 4, j->page + ((size_t)((vv + y) & 0xFF) * PAGE_TEXELS + (size_t)u) * 4, (size_t)w * 4);
        }
        snprintf(file, sizeof(file), "%s/%d.png", folder, i);
        image_file(j, file, image, w, h);
    }
    image_file_text(j, list, text, k);
    free(text);
    free(image);
}

/* a flare's glow on the ground (src: snow.c ground_flare_draw), in the areas that light one: in each one's own
 * textures, as flares/<area>.png */
static const SkyPiece kGroundFlare = {"ground-flare", 4, 0x380, 0, 0x320, 0xFE, 0x50, 0x10, 24, 24};
static const int kFlareAreas[] = {5, 24};

static void piece_image(Job *j, const VramContext *v, const SkyPiece *p, const char *path) {
    uint8_t *image = malloc((size_t)PAGE_TEXELS * PAGE_TEXELS * 4);
    uint16_t page = (uint16_t)((p->page_x / 64 & 0xF) | (p->page_y / 256 & 1) << 4 | (p->depth == 4 ? 0 : 1) << 7);
    uint16_t palette = (uint16_t)(p->palette_x / 16 | p->palette_y << 6);
    int y;

    vram_page_rgba(v, page, palette, j->page);
    for (y = 0; y < p->h; y++) {
        memcpy(image + (size_t)y * p->w * 4, j->page + ((size_t)(p->v + y) * PAGE_TEXELS + (size_t)p->u) * 4, (size_t)p->w * 4);
    }
    image_file(j, path, image, p->w, p->h);
    free(image);
}

/* the item ring's pictures (src: ring.c: CD.BIN 034_4, a bare TMD of a flat card an item, each its own corner of
 * the UI's textures): items/<k>.png, the card's texture rectangle cut out */
#define ITEM_CARDS_ENTRY (34 * CDBIN_KINDS + 4)
#define TMD_OBJECTS_AT 12
#define TMD_OBJECT 28
static void item_pictures(Job *j, const VramContext *v, const uint8_t *const *entries, const size_t *sizes, int n) {
    const uint8_t *d = ITEM_CARDS_ENTRY < n ? entries[ITEM_CARDS_ENTRY] : NULL;
    size_t size = ITEM_CARDS_ENTRY < n ? sizes[ITEM_CARDS_ENTRY] : 0;
    uint8_t *image;
    uint32_t count, o;

    if (d == NULL || size < TMD_OBJECTS_AT || d[0] != 0x41) {
        return;
    }
    count = (uint32_t)(d[8] | d[9] << 8 | d[10] << 16 | (uint32_t)d[11] << 24);
    image = malloc((size_t)PAGE_TEXELS * PAGE_TEXELS * 4);
    for (o = 0; o < count && !j->failed && TMD_OBJECTS_AT + (o + 1) * TMD_OBJECT <= size; o++) {
        const uint8_t *ob = d + TMD_OBJECTS_AT + o * TMD_OBJECT;
        uint32_t at = TMD_OBJECTS_AT + (uint32_t)(ob[16] | ob[17] << 8 | ob[18] << 16 | (uint32_t)ob[19] << 24);
        const uint8_t *pk, *w;
        int u0, v0, u1, v1, y, iw, ih;
        uint16_t palette, page;
        char path[64];

        if (at + 4 + 7 * 4 > size) {
            continue;
        }
        pk = d + at;
        w = pk + 4;   /* a textured quad: u v palette, u v page, u v, u v (then the colour) */
        u0 = w[0], v0 = w[1], palette = (uint16_t)(w[2] | w[3] << 8);
        page = (uint16_t)(w[6] | w[7] << 8);
        u1 = w[12], v1 = w[13];
        iw = u1 - u0 + 1, ih = v1 - v0 + 1;
        if (iw <= 0 || ih <= 0 || u0 + iw > PAGE_TEXELS || v0 + ih > PAGE_TEXELS) {
            continue;
        }
        vram_page_rgba(v, page, palette, j->page);
        for (y = 0; y < ih; y++) {
            memcpy(image + (size_t)y * iw * 4, j->page + ((size_t)(v0 + y) * PAGE_TEXELS + (size_t)u0) * 4, (size_t)iw * 4);
        }
        snprintf(path, sizeof(path), "items/%u.png", o);
        image_file(j, path, image, iw, ih);
    }
    free(image);
}

/* the item ring's 2D pieces (src: ring.c func_8003D6E0, func_8003EA28: the UI's textures, palettes $3F6D / $3F70):
 * its titles (the tools, the key items) and its hint (✕ EXIT); the face buttons; as ui/<name>.png */
static const SkyPiece kUiPieces[] = {
    {"ring-tools", 4, 0x2C0, 0, 0x2D0, 0xFD, 0xB0, 0x90, 48, 16},       /* "TOOL" */
    {"ring-items", 4, 0x2C0, 0, 0x2D0, 0xFD, 0xB0, 0x80, 48, 16},       /* "ITEM": the key items */
    {"exit", 4, 0x2C0, 0, 0x2D0, 0xFD, 0xB0, 0xA0, 40, 16},             /* "EXIT" */
    {"button-cross", 4, 0x300, 0, 0x300, 0xFD, 0x80, 0xB0, 16, 16},
    /* the phone's digits (src: ftel.c: D_800A34E4's columns; a blank one below) */
    {"button-circle", 4, 0x300, 0, 0x300, 0xFD, 0x90, 0xB0, 16, 16},
    {"button-square", 4, 0x300, 0, 0x300, 0xFD, 0x70, 0xB0, 16, 16},
    {"button-triangle", 4, 0x300, 0, 0x300, 0xFD, 0x60, 0xB0, 16, 16},
    {"digit-blank", 4, 0x300, 0, 0x300, 0xFD, 0x60, 0xC0, 16, 16},
    /* the notebook's (src: fhbook.c: the menu's cursor, func_8003E930; the pages' arrows) */
    {"hand", 4, 0x2C0, 0, 0x2D0, 0xFD, 0x60, 0xB0, 24, 16},
    {"more-above", 4, 0x300, 0, 0x300, 0xFD, 0xA0, 0xB0, 16, 16},
    {"more-below", 4, 0x300, 0, 0x300, 0xFD, 0xA0, 0xC0, 16, 16},
};

static void ui_images(Job *j, const VramContext *v) {
    size_t i;

    for (i = 0; i < sizeof(kUiPieces) / sizeof(kUiPieces[0]) && !j->failed; i++) {
        char path[64];

        snprintf(path, sizeof(path), "ui/%s.png", kUiPieces[i].name);
        piece_image(j, v, &kUiPieces[i], path);
    }
}

/* the small pictures beside the message box (src: ccdtim.c func_8003E5E8: D_80095BD4's rectangles, D_80095C44's
 * palettes): the phone's (0), the items got, the people on the line - 48 x 48 each, 0 .. 14 on the UI page at
 * (384, 256), 15 .. 27 on the one at (960, 256); as pictures/<n>.png */
static const uint8_t kPictureAt[][2] = {   /* u, v */
    {0, 64}, {48, 64}, {96, 64}, {144, 64}, {192, 64}, {0, 112}, {48, 112}, {96, 112}, {144, 112}, {192, 112},
    {0, 160}, {48, 160}, {96, 160}, {144, 160}, {192, 160},
    {144, 80}, {192, 80}, {0, 128}, {48, 128}, {0, 32}, {48, 32}, {96, 32}, {144, 32}, {192, 32}, {0, 80}, {48, 80},
    {96, 80}, {96, 128},
};
static const uint16_t kPicturePalette[] = {
    0x7418, 0x7419, 0x741A, 0x7458, 0x7459, 0x745A, 0x7498, 0x7499, 0x749A, 0x74D8, 0x74D9, 0x74DA, 0x7518, 0x7519,
    0x751A, 0x6CBC, 0x6CBD, 0x6CBE, 0x6CBF, 0x6C3C, 0x6C3D, 0x6C3E, 0x6C3F, 0x6C7C, 0x6C7D, 0x6C7E, 0x6C7F, 0x6CFC,
};
#define PICTURE_SIZE 48
#define FIRST_PAGE_PICTURES 15

static void picture_images(Job *j, const VramContext *v) {
    size_t i;

    for (i = 0; i < sizeof(kPictureAt) / sizeof(kPictureAt[0]) && !j->failed; i++) {
        uint16_t pal = kPicturePalette[i];
        SkyPiece p = {"picture", 4, i < FIRST_PAGE_PICTURES ? 0x180 : 0x3C0, 0x100, (pal & 0x3F) * 16, pal >> 6,
                      kPictureAt[i][0], kPictureAt[i][1], PICTURE_SIZE, PICTURE_SIZE};
        char path[64];

        snprintf(path, sizeof(path), "pictures/%u.png", (unsigned)i);
        piece_image(j, v, &p, path);
    }
}

/* the speakers' portraits (src: msgbox.c func_80014DF8 / func_80014D18, area.c func_80019918): sheets of 64 x 80
 * cells (8-bit, from VRAM x 0x280, y 0x100: six to a page, three down a column pair), each cell a character kind
 * with its palette by a table of {s16 kind, palette x, y} x 24. An area's sheet is one picture of all 24
 * (D_80080550); a few areas' sets patch cells over it, and the common set patches in the main characters (the
 * notebook's). As portraits/<kind>.png, each kind once. */
#define PORTRAIT_CELLS 24
#define PORTRAIT_W 64
#define PORTRAIT_H 80
#define PORTRAIT_X 0x280
#define PORTRAIT_Y 0x100
#define PORTRAIT_COMMON_SET 0x80080594   /* 9 s16: the common set's CD.BIN entries */
#define PORTRAIT_COMMON_SET_N 9
#define PORTRAIT_AREA_SET 0x80080550     /* s16 by area: its own sheet */
typedef struct PortraitSheet {
    uint32_t table;   /* its cells' table */
    uint32_t set;     /* the patches loaded over the area's sheet (s16 x n; 0: the common set) */
    int n, area;      /* the area whose sheet is under them */
} PortraitSheet;
static const PortraitSheet kPortraitSheets[] = {
    {0x800802AC, 0, 0, 0},             /* the common set over the station's sheet (the notebook's) */
    {0x8007FF4C, 0, -1, 0},            /* the areas' (the station's sheet alone) */
    {0x800800FC, 0, -1, 19},
    {0x8008018C, 0x800805D0, 5, 2},    /* the hospital's */
    {0x8008021C, 0x800805A8, 4, 12},   /* the club's */
    {0x8007FFDC, 0x800805B0, 4, 21},   /* the diner's */
    {0x8008006C, 0x800805DC, 8, 26},   /* the forest's */
};

static void load_entry(VramContext *v, const uint8_t *const *entries, const size_t *sizes, int n, int e) {
    if (e > 0 && e < n && entries[e] != NULL) {
        vram_load_tim(v, entries[e], sizes[e]);
    }
}

static void portrait_images(Job *j, VramContext *v, const uint8_t *const *entries, const size_t *sizes, int n,
                            const uint8_t *exe, size_t exe_n) {
    const uint8_t *common = exe_at(exe, exe_n, PORTRAIT_COMMON_SET, PORTRAIT_COMMON_SET_N * 2);
    uint8_t done[256] = {0}, *image = malloc((size_t)PORTRAIT_W * PORTRAIT_H * 4);
    size_t s;
    int i, y;

    for (s = 0; s < sizeof(kPortraitSheets) / sizeof(kPortraitSheets[0]) && common != NULL && !j->failed; s++) {
        const PortraitSheet *ps = &kPortraitSheets[s];
        const uint8_t *table = exe_at(exe, exe_n, ps->table, PORTRAIT_CELLS * 6);
        const uint8_t *set = ps->set ? exe_at(exe, exe_n, ps->set, (size_t)ps->n * 2) : NULL;
        const uint8_t *own = exe_at(exe, exe_n, PORTRAIT_AREA_SET, (size_t)(ps->area + 1) * 2);

        if (table == NULL) {
            continue;
        }
        vram_clear(v);
        if (own != NULL) {
            load_entry(v, entries, sizes, n, le16s(own + ps->area * 2));
        }
        if (set != NULL) {
            for (i = 0; i < ps->n; i++) {
                load_entry(v, entries, sizes, n, le16s(set + i * 2));
            }
        } else if (ps->n == 0) {
            for (i = 0; i < PORTRAIT_COMMON_SET_N; i++) {
                load_entry(v, entries, sizes, n, le16s(common + i * 2));
            }
        }
        for (i = 0; i < PORTRAIT_CELLS && !j->failed; i++) {
            int kind = le16s(table + i * 6) & 0xFF, px = le16s(table + i * 6 + 2), py = le16s(table + i * 6 + 4);
            uint16_t page = (uint16_t)((((i / 6) * 64 + PORTRAIT_X) / 64) | (PORTRAIT_Y / 256) << 4 | 1 << 7);
            int u = ((i / 3) & 1) * PORTRAIT_W, vv = (i % 3) * PORTRAIT_H;
            char path[64];

            if (done[kind]) {
                continue;
            }
            done[kind] = 1;
            vram_page_rgba(v, page, (uint16_t)(px / 16 | py << 6), j->page);
            for (y = 0; y < PORTRAIT_H; y++) {
                memcpy(image + (size_t)y * PORTRAIT_W * 4, j->page + ((size_t)(vv + y) * PAGE_TEXELS + (size_t)u) * 4,
                       (size_t)PORTRAIT_W * 4);
            }
            snprintf(path, sizeof(path), "portraits/%d.png", kind);
            image_file(j, path, image, PORTRAIT_W, PORTRAIT_H);
        }
    }
    free(image);
}

static void sky_images(Job *j, const VramContext *v) {
    uint8_t *image = malloc((size_t)PAGE_TEXELS * PAGE_TEXELS * 4);
    size_t i;
    int y;

    for (i = 0; i < sizeof(kSky) / sizeof(kSky[0]) && !j->failed; i++) {
        const SkyPiece *p = &kSky[i];
        uint16_t page = (uint16_t)((p->page_x / 64 & 0xF) | (p->page_y / 256 & 1) << 4 | (p->depth == 4 ? 0 : 1) << 7);
        uint16_t palette = (uint16_t)(p->palette_x / 16 | p->palette_y << 6);
        char path[64];

        vram_page_rgba(v, page, palette, j->page);
        for (y = 0; y < p->h; y++) {
            memcpy(image + (size_t)y * p->w * 4, j->page + ((size_t)(p->v + y) * PAGE_TEXELS + (size_t)p->u) * 4, (size_t)p->w * 4);
        }
        snprintf(path, sizeof(path), "sky/%s.png", p->name);
        image_file(j, path, image, p->w, p->h);
    }
    free(image);
}

static int cut(Job *j, const VramContext *v, const char *name) {
    uint16_t page, palette;

    if (!texture_words(name, &page, &palette)) {
        return 0;
    }
    vram_page_rgba(v, page, palette, j->page);
    return 1;
}

/* a model's glTF written (or checked); texture_uri gives each texture's default image */
typedef const char *(*UriFn)(void *ctx, const char *texture);

static void model(Job *j, const Found *f, const Model *m, const char *set, UriFn uri, void *uctx) {
    GltfOut g;
    char gltf[200], bin[200], bin_uri[100];

    snprintf(bin_uri, sizeof(bin_uri), "%s.bin", strrchr(f->path, '/') + 1);
    snprintf(gltf, sizeof(gltf), "%s.gltf", f->path);
    snprintf(bin, sizeof(bin), "%s.bin", f->path);
    model_gltf(m, set, (const char *(*)(void *, const char *))uri, uctx, bin_uri, f->comment, &g);
    if (j->write != NULL) {
        if (!j->write(j->ctx, gltf, (const uint8_t *)g.json, g.json_size) || !j->write(j->ctx, bin, g.bin, g.bin_size)) {
            snprintf(j->why, j->nwhy, "can't write %s", gltf);
            j->failed = 1;
        }
    } else {
        size_t jn = 0, bn = 0;
        uint8_t *json = j->read(j->ctx, gltf, &jn), *data = j->read(j->ctx, bin, &bn);
        char err[256];

        if (json == NULL || data == NULL || !model_gltf_check(m, (const char *)json, jn, data, bn, err, sizeof(err))) {
            snprintf(j->why, j->nwhy, "%s: %s", gltf, json == NULL || data == NULL ? "missing" : err);
            j->failed = 1;
        }
        free(json);
        free(data);
    }
    model_gltf_free(&g);
    j->models++;
}

/* the texture names a model's materials use (as model_gltf names them: page and palette, blend bits aside) */
#define PAGE_WITHOUT_BLEND 0x19F
static int names_of(const Model *m, char (*names)[64], int max) {
    int n = 0, i, k, x;

    for (i = 0; i < m->nobjects; i++) {
        for (k = 0; k < m->objects[i].npolygons; k++) {
            const ModelPolygon *p = &m->objects[i].polygons[k];
            char name[64];

            if (!p->textured) {
                continue;
            }
            vram_texture_name(p->texture_page & PAGE_WITHOUT_BLEND, p->palette, name, sizeof(name));
            for (x = 0; x < n && strcmp(names[x], name) != 0; x++) {
            }
            if (x == n && n < max) {
                snprintf(names[n++], 64, "%s", name);
            }
        }
    }
    return n;
}

/* the areas' sets: every texture name any area uses, and per area the hash of its texture (0: not in its set) */
#define MAX_NAMES 2048
typedef struct Sets {
    char names[MAX_NAMES][64];
    int n;
    uint64_t hash[MAX_NAMES][AREAS];
    uint64_t common[MAX_NAMES];   /* the version most areas have: textures/common/ */
    int common_area[MAX_NAMES];   /* the first area with it (where it is cut from) */
    int area;                     /* the area a model's URIs are for */
    const char *up;               /* from the model's folder to the library's ("../") */
    char uri[128];
} Sets;

static int name_index(Sets *s, const char *name, int add) {
    int i;
    for (i = 0; i < s->n; i++) {
        if (strcmp(s->names[i], name) == 0) {
            return i;
        }
    }
    if (!add || s->n == MAX_NAMES) {
        return -1;
    }
    snprintf(s->names[s->n], 64, "%s", name);
    return s->n++;
}

static const char *area_uri(void *ctx, const char *texture) {
    Sets *s = ctx;
    int i = name_index(s, texture, 0);

    if (i >= 0 && s->hash[i][s->area] != 0 && s->hash[i][s->area] != s->common[i]) {
        snprintf(s->uri, sizeof(s->uri), "%stextures/area-%03d/%s.png", s->up, s->area, texture);
    } else {
        snprintf(s->uri, sizeof(s->uri), "%stextures/common/%s.png", s->up, texture);
    }
    return s->uri;
}

/* a streamed character's textures: in its own folder beside the glTF */
typedef struct OwnUri {
    char folder[96];
    char uri[192];
} OwnUri;

static const char *own_uri(void *ctx, const char *texture) {
    OwnUri *o = ctx;
    snprintf(o->uri, sizeof(o->uri), "%s%s.png", o->folder, texture);
    return o->uri;
}

static int run(Job *j, const uint8_t *const *entries, const size_t *sizes, int n, const uint8_t *exe, size_t exe_n,
               const uint8_t *map, size_t map_n) {
    Models ms;
    VramContext *v = malloc(sizeof(VramContext));
    Sets *sets = calloc(1, sizeof(Sets));
    OwnUri own;
    char (*names)[64] = calloc(MODEL_MAX_TEXTURES, 64);
    int i, a, k, nn;

    j->page = malloc((size_t)PAGE_TEXELS * PAGE_TEXELS * 4);
    find_models(&ms, entries, sizes, n, exe, exe_n);
    find_town(&ms, exe, exe_n, map, map_n);
    /* the streamed characters: each with its own textures */
    for (i = 0; i < ms.n && !j->failed; i++) {
        const Found *f = &ms.list[i];
        Model m;

        if (f->kind != MODEL_STREAMED || !read_found(f, &m)) {
            continue;
        }
        if (entries[f->texture_entry] != NULL &&
            streamed_context(v, &m, entries[f->texture_entry], sizes[f->texture_entry], exe, exe_n)) {
            char dir[160];

            snprintf(own.folder, sizeof(own.folder), "%s.textures/", f->id);
            model(j, f, &m, "own", own_uri, &own);
            nn = names_of(&m, names, MODEL_MAX_TEXTURES);
            for (k = 0; k < nn; k++) {
                snprintf(dir, sizeof(dir), "models/%s.textures/%s.png", f->id, names[k]);
                if (cut(j, v, names[k])) {
                    texture_file(j, dir);
                }
            }
        }
        model_free(&m);
    }
    /* the areas' sets: what each area's props use, and what the other models use, in every area */
    for (a = 0; a < AREAS; a++) {
        area_context(v, a, entries, sizes, n, exe, exe_n);
        for (i = 0; i < ms.n; i++) {
            const Found *f = &ms.list[i];
            Model m;

            if (f->kind == MODEL_STREAMED || (f->kind == MODEL_AREA && f->area != a) || !read_found(f, &m)) {
                continue;
            }
            nn = names_of(&m, names, MODEL_MAX_TEXTURES);
            for (k = 0; k < nn; k++) {
                int x = name_index(sets, names[k], 1);
                if (x >= 0 && sets->hash[x][a] == 0 && cut(j, v, names[k])) {
                    sets->hash[x][a] = hash_of(j->page, (size_t)PAGE_TEXELS * PAGE_TEXELS * 4) | 1;
                }
            }
            model_free(&m);
        }
    }
    for (i = 0; i < sets->n; i++) {   /* each texture's most common version is the common one */
        int best = 0, b;
        for (a = 0; a < AREAS; a++) {
            int count = 0;
            for (b = 0; b < AREAS; b++) {
                count += sets->hash[i][a] != 0 && sets->hash[i][b] == sets->hash[i][a];
            }
            if (count > best) {
                best = count;
                sets->common[i] = sets->hash[i][a];
                sets->common_area[i] = a;
            }
        }
    }
    /* the textures: the common versions, and each area's own where it differs; the models */
    for (a = 0; a < AREAS && !j->failed; a++) {
        area_context(v, a, entries, sizes, n, exe, exe_n);
        for (i = 0; i < sets->n && !j->failed; i++) {
            char path[200];

            if (sets->hash[i][a] == 0 || !cut(j, v, sets->names[i])) {
                continue;
            }
            if (sets->hash[i][a] != sets->common[i]) {
                snprintf(path, sizeof(path), "textures/area-%03d/%s.png", a, sets->names[i]);
                texture_file(j, path);
            } else if (sets->common_area[i] == a) {
                snprintf(path, sizeof(path), "textures/common/%s.png", sets->names[i]);
                texture_file(j, path);
            }
        }
        if (a != TOWN_AREA) {   /* an area's billboards' sprites (those with billboards: part 5) */
            char list[64], folder[64];
            snprintf(list, sizeof(list), "sprites/%03d.txt", a);
            snprintf(folder, sizeof(folder), "sprites/%03d", a);
            area_sprites(j, v, entries, sizes, n, a, list, folder);
        }
        for (i = 0; i < (int)(sizeof(kFlareAreas) / sizeof(kFlareAreas[0])); i++) {
            if (kFlareAreas[i] == a) {
                char path[64];
                snprintf(path, sizeof(path), "flares/%03d.png", a);
                piece_image(j, v, &kGroundFlare, path);
            }
        }
        if (a == TOWN_AREA) {   /* the lamps' textures lit */
            lamps_set(v, 1);
            for (i = 0; i < sets->n && !j->failed; i++) {
                uint16_t page, palette;
                char path[200];

                if (sets->hash[i][a] != 0 && texture_words(sets->names[i], &page, &palette) &&
                    model_lamp(palette) >= 0 && cut(j, v, sets->names[i])) {
                    snprintf(path, sizeof(path), "textures/lamps/%s.png", sets->names[i]);
                    texture_file(j, path);
                }
            }
            lamps_set(v, 0);
            sky_images(j, v);   /* (the sky's: the common images, as in the town) */
            item_pictures(j, v, entries, sizes, n);
            ui_images(j, v);
            picture_images(j, v);
            area_sprites(j, v, entries, sizes, n, TOWN_AREA, "town/sprites.txt", "town/sprites");
        }
        for (i = 0; i < ms.n && !j->failed; i++) {
            const Found *f = &ms.list[i];
            Model m;

            if (f->kind == MODEL_STREAMED || !read_found(f, &m)) {
                continue;
            }
            if ((f->kind == MODEL_AREA && f->area == a) || (f->kind == MODEL_OTHER && a == 1)) {
                sets->area = a;   /* (the others' default images: Matthew's area) */
                sets->up = strchr(f->path, '/') == strrchr(f->path, '/') ? "../" : "../../";
                model(j, f, &m, "area", area_uri, sets);
            }
            model_free(&m);
        }
    }
    if (!j->failed) {
        portrait_images(j, v, entries, sizes, n, exe, exe_n);   /* (its own contexts: after the areas') */
    }
    free(ms.list);
    free(v);
    free(sets);
    free(names);
    free(j->page);
    return !j->failed;
}

int modelconv_write(const uint8_t *const *entries, const size_t *sizes, int n, const uint8_t *exe, size_t exe_n,
                    const uint8_t *map, size_t map_n, ModelconvWriteFn write, void *ctx, char *why, size_t nwhy) {
    Job j;

    memset(&j, 0, sizeof(j));
    j.write = write;
    j.ctx = ctx;
    j.why = why;
    j.nwhy = nwhy;
    return run(&j, entries, sizes, n, exe, exe_n, map, map_n);
}

int modelconv_check(const uint8_t *const *entries, const size_t *sizes, int n, const uint8_t *exe, size_t exe_n,
                    const uint8_t *map, size_t map_n, ModelconvReadFn read, void *ctx, int *models, int *textures, char *why, size_t nwhy) {
    Job j;
    int ok;

    memset(&j, 0, sizeof(j));
    j.read = read;
    j.ctx = ctx;
    j.why = why;
    j.nwhy = nwhy;
    ok = run(&j, entries, sizes, n, exe, exe_n, map, map_n);
    *models = j.models;
    *textures = j.textures;
    return ok;
}
