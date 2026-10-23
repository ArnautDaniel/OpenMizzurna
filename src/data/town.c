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

static const char *kRawParts[TOWN_TILE_PARTS] = {NULL, NULL, "obstacles.bin", "zones.bin", "billboards.bin"};

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
