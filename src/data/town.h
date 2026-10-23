/* The town (the streamed outdoors, area 27): MAP.BIN, a grid of tiles the game streams in around the camera
 * (src: map_stream.c). The executable's grid (32 x 24 cells of {s16 first sector, s16 bytes}) says where each
 * tile is in MAP.BIN; a tile is a container of five parts:
 *   0 its ground (a TMD on its own, no parts)      town/CC-RR/terrain.gltf
 *   1 its objects (a model: SPL + TMD)              town/CC-RR/objects.gltf
 *   2 obstacle boxes, 3 trigger zones (region lists; read with the areas' when the movement subsystem is built)
 *     4 billboard placements                        town/CC-RR/obstacles.bin, zones.bin, billboards.bin, as they are
 * Tile (col, row)'s centre is (col * 19980 + 9990, (row + 1) * 19980 + 9990) in world units (src: func_8001D374);
 * its models are in model units, half of world units, like every model. town/index.txt lists the tiles.
 * (MAP.BIN's sectors past each tile's end, and a tile's padding between its parts, hold leftovers of the disc's
 * making - never read by the game, not kept.) */
#ifndef TOWN_H
#define TOWN_H

#include <stddef.h>
#include <stdint.h>

#define TOWN_COLUMNS 32
#define TOWN_ROWS 24
#define TOWN_TILE 19980         /* a tile's side, world units */
#define TOWN_AREA 27            /* the area the town is (its textures are that area's) */
#define TOWN_TILE_PARTS 5
enum { TOWN_TERRAIN, TOWN_OBJECTS, TOWN_OBSTACLES, TOWN_ZONES, TOWN_BILLBOARDS };

/* tile (col, row)'s bytes in MAP.BIN; NULL if the grid or MAP.BIN is short */
const uint8_t *town_tile(const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n, int col, int row,
                         size_t *size);
/* its five parts (pointing into tile); 0 if it isn't laid out as a tile (the container layout, data/container.h,
 * its padding aside) */
int town_tile_parts(const uint8_t *tile, size_t n, const uint8_t *part[TOWN_TILE_PARTS], size_t size[TOWN_TILE_PARTS]);
/* its folder in the library ("town/03-05") */
void town_folder(int col, int row, char *out, size_t n);

typedef int (*TownWriteFn)(void *ctx, const char *path, const uint8_t *data, size_t n);
typedef uint8_t *(*TownReadFn)(void *ctx, const char *path, size_t *n);

/* the index and each tile's region and billboard parts (the models: data/modelconv.c); 0 on failure */
int town_write(const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n, TownWriteFn write, void *ctx,
               char *why, size_t nwhy);
/* and the check: every tile its five parts and nothing else, the parts kept the same bytes, the index the same;
 * tiles checked in *tiles */
int town_check(const uint8_t *exe, size_t exe_n, const uint8_t *map, size_t map_n, TownReadFn read, void *ctx,
               int *tiles, char *why, size_t nwhy);

#endif
