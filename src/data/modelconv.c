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
static void texture_file(Job *j, const char *path) {
    size_t n;

    if (j->failed) {
        return;
    }
    if (j->write != NULL) {
        uint8_t *png = png_encode_rgba(j->page, PAGE_TEXELS, PAGE_TEXELS, &n);
        if (png == NULL || !j->write(j->ctx, path, png, n)) {
            snprintf(j->why, j->nwhy, "can't write %s", path);
            j->failed = 1;
        }
        free(png);
    } else {
        uint8_t *file = j->read(j->ctx, path, &n), *rgba;
        int w = 0, h = 0;

        rgba = file != NULL ? png_read_rgba(file, n, &w, &h) : NULL;
        if (rgba == NULL || w != PAGE_TEXELS || h != PAGE_TEXELS || memcmp(rgba, j->page, (size_t)w * h * 4) != 0) {
            snprintf(j->why, j->nwhy, "%s isn't the texture video memory held", path);
            j->failed = 1;
        }
        free(rgba);
        free(file);
    }
    j->textures++;
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
