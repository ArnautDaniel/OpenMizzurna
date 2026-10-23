/* The models in the library (docs/library.md, "Models and their textures"): every model of CD.BIN as glTF
 * (data/modelgltf.h) in models/, and the textures their materials name - each area's set in textures/area-NNN/
 * (cut from what video memory held in that area: data/vramctx.h), a streamed character's own beside it.
 *
 * Which images a model is drawn with is the game's loading (src: load_area, func_80019918, npc_model_stream):
 *   an area's props (NNN_4, part 3): the common images (033_3), the area's sheet (NNN_3), its character sheet
 *       (a CD.BIN entry per area, from the executable's table);
 *   a streamed character (a kind with a texture entry - the executable's kind tables): its texture image put in a
 *       character slot, its polygons pointed at the slot (src: func_8003D0F0);
 *   any other model (the cast whose textures stay loaded, props of no area): the area set of where it's drawn. */
#ifndef MODELCONV_H
#define MODELCONV_H

#include <stddef.h>
#include <stdint.h>

typedef int (*ModelconvWriteFn)(void *ctx, const char *path, const uint8_t *data, size_t n);
typedef uint8_t *(*ModelconvReadFn)(void *ctx, const char *path, size_t *n);

/* entries: every CD.BIN entry unpacked (NULL when empty); exe: the executable. 0 on failure (why says) */
int modelconv_write(const uint8_t *const *entries, const size_t *sizes, int n, const uint8_t *exe, size_t exe_n,
                    ModelconvWriteFn write, void *ctx, char *why, size_t nwhy);
/* the library's models and textures against the disc's: the glTF holding each model, every texture the same
 * texels; counts in *models, *textures */
int modelconv_check(const uint8_t *const *entries, const size_t *sizes, int n, const uint8_t *exe, size_t exe_n,
                    ModelconvReadFn read, void *ctx, int *models, int *textures, char *why, size_t nwhy);

#endif
