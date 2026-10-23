/* A model as glTF 2.0 (the library's form; data/tmd.h is what it is made from).
 *
 * The game's space (x right, y down, z forward) turned half a turn about x into glTF's (y up); 256 of the game's
 * units are 1.0. A node per part, placed and turned as the part is at rest (its exact place and turn in the node's
 * extras: "om": {"place": [x, y, z], "turn": [rx, ry, rz], "scale": s, "flags": f}); objects past the parts hang
 * from the scene's root. A mesh per object: its polygons as triangles, grouped by material - texture, blending,
 * lit or not, one- or two-sided. Vertex colours (COLOR_0) are the game's with 0x80 as 1.0, so they multiply the
 * texture as on the PlayStation and may go past 1.
 *
 * Materials name their texture in extras: "om": {"texture-set": "area" | "own" | "none", "texture": name,
 * "blend": "half" | "add" | "subtract" | "quarter"}; "area" is the set of the area being played (the image the
 * glTF names is a default for viewers), "own" the model's. Unlit polygons use KHR_materials_unlit. */
#ifndef MODELGLTF_H
#define MODELGLTF_H

#include <stddef.h>
#include <stdint.h>

#include "tmd.h"

#define MODEL_UNITS 256.0f   /* the game's units in 1.0 */
#define MODEL_MAX_TEXTURES 512

typedef struct GltfOut {
    char *json;
    size_t json_size;
    uint8_t *bin;
    size_t bin_size;
    char textures[MODEL_MAX_TEXTURES][64];   /* the textures its materials name (page / palette names: data/vramctx.h) */
    int ntextures;
} GltfOut;

/* the model as glTF: `set` the materials' texture set ("area" or "own"), `uri` each texture's default image
 * (relative to the glTF; NULL: none written), `bin_uri` the binary buffer's file name */
/* the town's street lamps (src: func_800243F0): each lamp's light is drawn through a 16-colour palette of video
 * memory's last row (lamp i's at 16 i), which the game fills with the lit or the unlit colours as the lamps go on
 * and off; the import cuts its textures both ways (textures/lamps/: lit) and a material says its lamp */
#define LAMPS 38
#define LAMP_ROW 255
#define LAMP_UNLIT_X 0x2A0   /* where the town's sheet keeps the two palettes, on that row */
#define LAMP_LIT_X 0x2B0
int model_lamp(uint16_t palette);   /* the lamp a palette is (-1: none) */

int model_gltf(const Model *m, const char *set, const char *(*uri)(void *ctx, const char *texture), void *uri_ctx,
               const char *bin_uri, const char *comment, GltfOut *out);
void model_gltf_free(GltfOut *out);

/* the check: the glTF read back (cgltf) holds this model - every part's place and turn, every polygon's corners,
 * normals, colours and texture coordinates, in order. 0 on a difference (why says) */
int model_gltf_check(const Model *m, const char *json, size_t json_size, const uint8_t *bin, size_t bin_size,
                     char *why, size_t nwhy);

#endif
