/* The town (town.h). */
#include "town.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define EXE_HEADER 0x800
#define EXE_LOAD_ADDRESS 0x80010000u
#define TOWN_GRID 0x80094BACu   /* s16 sector, s16 bytes a cell, row by row (src: map_tile_grid) */
#define SECTOR 2048
#define TILE_HEADER 8   /* u32 parts, u32 0; then u32 offset, size a part (each part on a 4-byte boundary) */
#define PART_ENTRY 8
#define PART_ALIGN 4

static const char *kRawParts[TOWN_TILE_PARTS] = {NULL, NULL, "obstacles.bin", "zones.bin", NULL};

/* ---- a tile's billboards as text (town.h): {s32 count, x, z} then {s16 x, z, bottom, sprite, half width, top} ---- */
#define BILLBOARDS_HEAD 12
#define BILLBOARD_SIZE 12
#define BILLBOARD_LINE 96

static int s16_at(const uint8_t *p) { return (int16_t)(p[0] | p[1] << 8); }
static int32_t s32_at(const uint8_t *p) { return (int32_t)((uint32_t)p[0] | (uint32_t)p[1] << 8 | (uint32_t)p[2] << 16 | (uint32_t)p[3] << 24); }
static void put16(uint8_t *p, int v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); }
static void put32(uint8_t *p, int32_t v) { put16(p, v & 0xFFFF); put16(p + 2, (int)((uint32_t)v >> 16)); }

static int billboards_laid_out(const uint8_t *d, size_t n) {
    return n >= BILLBOARDS_HEAD && s32_at(d) >= 0 && n == BILLBOARDS_HEAD + (size_t)s32_at(d) * BILLBOARD_SIZE;
}

static char *billboards_text(const uint8_t *d, size_t n, size_t *len) {
    int count = s32_at(d), i;
    size_t cap = (size_t)(count + 4) * BILLBOARD_LINE, k = 0;
    char *t = malloc(cap);

    (void)n;
    k += (size_t)snprintf(t + k, cap - k, "om-billboards 1\norigin %d %d\nbillboards %d\n", s32_at(d + 4), s32_at(d + 8), count);
    for (i = 0; i < count; i++) {
        const uint8_t *b = d + BILLBOARDS_HEAD + i * BILLBOARD_SIZE;
        k += (size_t)snprintf(t + k, cap - k, "billboard %d at %d %d from %d to %d half-width %d sprite %d\n", i,
                              s16_at(b), s16_at(b + 2), s16_at(b + 4), s16_at(b + 10), s16_at(b + 8), s16_at(b + 6));
    }
    *len = k;
    return t;
}

static uint8_t *billboards_from_text(const char *s, size_t n, size_t *len) {
    char *t = malloc(n + 1), *line;
    uint8_t *d = NULL;
    int count = -1, x = 0, z = 0, seen = 0;

    memcpy(t, s, n);
    t[n] = 0;
    for (line = strtok(t, "\n"); line != NULL; line = strtok(NULL, "\n")) {
        int i, bx, bz, lo, hi, hw, sp;

        if (sscanf(line, "origin %d %d", &x, &z) == 2) {
            continue;
        }
        if (sscanf(line, "billboards %d", &count) == 1 && count >= 0 && d == NULL) {
            d = calloc(1, BILLBOARDS_HEAD + (size_t)count * BILLBOARD_SIZE);
        } else if (d != NULL && sscanf(line, "billboard %d at %d %d from %d to %d half-width %d sprite %d", &i, &bx, &bz,
                                       &lo, &hi, &hw, &sp) == 7 && i >= 0 && i < count) {
            uint8_t *b = d + BILLBOARDS_HEAD + i * BILLBOARD_SIZE;
            put16(b, bx), put16(b + 2, bz), put16(b + 4, lo), put16(b + 6, sp), put16(b + 8, hw), put16(b + 10, hi);
            seen++;
        }
    }
    free(t);
    if (d == NULL || seen != count) {
        free(d);
        return NULL;
    }
    put32(d, count), put32(d + 4, x), put32(d + 8, z);
    *len = BILLBOARDS_HEAD + (size_t)count * BILLBOARD_SIZE;
    return d;
}

const uint8_t *town_tile(const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n, int col, int row,
                         size_t *size) {
    size_t at = EXE_HEADER + (TOWN_GRID - EXE_LOAD_ADDRESS) + (size_t)(row * TOWN_COLUMNS + col) * 4, start;
    const uint8_t *g;

    if (exe == NULL || map == NULL || at + 4 > exe_n) {
        return NULL;
    }
    g = exe + at;
    start = (size_t)(uint16_t)(g[0] | g[1] << 8) * SECTOR;
    *size = (uint16_t)(g[2] | g[3] << 8);
    return start + *size <= map_n ? map + start : NULL;
}

static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }

int town_tile_parts(const uint8_t *tile, size_t n, const uint8_t *part[TOWN_TILE_PARTS], size_t size[TOWN_TILE_PARTS]) {
    size_t pos = TILE_HEADER + TOWN_TILE_PARTS * PART_ENTRY;
    int p;

    if (n < pos || le32(tile) != TOWN_TILE_PARTS || le32(tile + 4) != 0) {
        return 0;
    }
    for (p = 0; p < TOWN_TILE_PARTS; p++) {
        size_t offset = le32(tile + TILE_HEADER + p * PART_ENTRY);

        size[p] = le32(tile + TILE_HEADER + p * PART_ENTRY + 4);
        if (offset != pos || offset + size[p] > n) {
            return 0;
        }
        part[p] = tile + offset;
        pos = (offset + size[p] + PART_ALIGN - 1) & ~(size_t)(PART_ALIGN - 1);
    }
    return part[TOWN_TILE_PARTS - 1] + size[TOWN_TILE_PARTS - 1] == tile + n;   /* (the last ends the tile) */
}

void town_folder(int col, int row, char *out, size_t n) {
    snprintf(out, n, "town/%02d-%02d", col, row);
}

static char *index_text(size_t *len) {
    size_t cap = 64 * 1024;
    char *s = malloc(cap);
    int k = 0, c, r;

    k += snprintf(s + k, cap - (size_t)k,
                  "\\ the town: MAP.BIN's tiles (data/town.h)\nom-town 1\ncolumns %d\nrows %d\ntile-size %d\n"
                  "\\ tile <col> <row> <folder> centre <x> <z> (world units: twice the models')\n",
                  TOWN_COLUMNS, TOWN_ROWS, TOWN_TILE);
    for (r = 0; r < TOWN_ROWS; r++) {
        for (c = 0; c < TOWN_COLUMNS; c++) {
            char folder[32];
            town_folder(c, r, folder, sizeof(folder));
            k += snprintf(s + k, cap - (size_t)k, "tile %d %d %s centre %d %d\n", c, r, folder + strlen("town/"),
                          c * TOWN_TILE + TOWN_TILE / 2, (r + 1) * TOWN_TILE + TOWN_TILE / 2);
        }
    }
    *len = (size_t)k;
    return s;
}

/* tile (c, r)'s parts; 0 (why says) if it isn't a tile */
static int tile_parts(const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n, int c, int r,
                      const uint8_t *part[TOWN_TILE_PARTS], size_t size[TOWN_TILE_PARTS], char *why, size_t nwhy) {
    size_t n;
    const uint8_t *d = town_tile(exe, exe_n, map, map_n, c, r, &n);

    if (d == NULL || !town_tile_parts(d, n, part, size)) {
        snprintf(why, nwhy, "MAP.BIN's tile %d, %d isn't a tile", c, r);
        return 0;
    }
    return 1;
}

static int run(const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n, TownWriteFn write, TownReadFn read,
               void *ctx, int *tiles, char *why, size_t nwhy) {
    const uint8_t *part[TOWN_TILE_PARTS];
    size_t size[TOWN_TILE_PARTS];
    char path[96], folder[32];
    size_t len, fn;
    char *index = index_text(&len);
    uint8_t *file;
    int c, r, p, ok;

    if (write != NULL) {
        ok = write(ctx, "town/index.txt", (const uint8_t *)index, len);
    } else {
        file = read(ctx, "town/index.txt", &fn);
        ok = file != NULL && fn == len && memcmp(file, index, len) == 0;
        free(file);
    }
    free(index);
    if (!ok) {
        snprintf(why, nwhy, "town/index.txt: %s", write != NULL ? "can't write it" : "missing or different");
        return 0;
    }
    for (r = 0; r < TOWN_ROWS; r++) {
        for (c = 0; c < TOWN_COLUMNS; c++) {
            if (!tile_parts(exe, exe_n, map, map_n, c, r, part, size, why, nwhy)) {
                return 0;
            }
            town_folder(c, r, folder, sizeof(folder));
            for (p = 0; p < TOWN_TILE_PARTS; p++) {
                if (kRawParts[p] == NULL) {
                    continue;
                }
                snprintf(path, sizeof(path), "%s/%s", folder, kRawParts[p]);
                if (write != NULL) {
                    ok = write(ctx, path, part[p], size[p]);
                } else {
                    file = read(ctx, path, &fn);
                    ok = file != NULL && fn == size[p] && memcmp(file, part[p], fn) == 0;
                    free(file);
                }
                if (!ok) {
                    snprintf(why, nwhy, "%s: %s", path, write != NULL ? "can't write it" : "missing or different");
                    return 0;
                }
            }
            {   /* the billboards: text, or as they are if they aren't laid out as billboards */
                const uint8_t *b = part[TOWN_BILLBOARDS];
                size_t bn = size[TOWN_BILLBOARDS], tn = 0;
                int text = billboards_laid_out(b, bn);

                snprintf(path, sizeof(path), "%s/%s", folder, text ? "billboards.txt" : "billboards.bin");
                if (write != NULL) {
                    char *t = text ? billboards_text(b, bn, &tn) : NULL;
                    ok = text ? write(ctx, path, (const uint8_t *)t, tn) : write(ctx, path, b, bn);
                    free(t);
                } else {
                    uint8_t *back = NULL;
                    file = read(ctx, path, &fn);
                    if (file != NULL && text) {
                        back = billboards_from_text((const char *)file, fn, &tn);
                        ok = back != NULL && tn == bn && memcmp(back, b, bn) == 0;
                    } else {
                        ok = file != NULL && fn == bn && memcmp(file, b, fn) == 0;
                    }
                    free(back);
                    free(file);
                }
                if (!ok) {
                    snprintf(why, nwhy, "%s: %s", path, write != NULL ? "can't write it" : "missing or different");
                    return 0;
                }
            }
            if (tiles != NULL) {
                (*tiles)++;
            }
        }
    }
    return 1;
}

int town_write(const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n, TownWriteFn write, void *ctx,
               char *why, size_t nwhy) {
    return run(exe, exe_n, map, map_n, write, NULL, ctx, NULL, why, nwhy);
}

int town_check(const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n, TownReadFn read, void *ctx,
               int *tiles, char *why, size_t nwhy) {
    *tiles = 0;
    return run(exe, exe_n, map, map_n, NULL, read, ctx, tiles, why, nwhy);
}
