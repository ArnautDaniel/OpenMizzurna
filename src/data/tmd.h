/* The game's models, read for the import: an SPL (Mizzurna Falls's own: a hierarchy of parts, each placed and
 * turned relative to its parent) followed by a TMD (Sony's: objects of vertices, normals and polygons; object i is
 * part i's). Read into plain structures; the PlayStation's texture page and palette words are kept as they are
 * (only the import uses them, to cut the textures - data/vramctx.h).
 *
 * SPL: u16 0x88FA, u16 parts, then per part: s16 parent (-1 the root), s16 x, y, z (its place), s16 ry, rx, rz (its
 *      turn: 4096 a full turn, applied as Ry Rx Rz), u16 flags.
 * TMD: u32 0x41, u32 flags, u32 objects; per object: u32 vertices' offset, count, normals' offset, count,
 *      polygons' offset, count, s32 scale (the part's place is scaled by 2^scale); vertices and normals s16 x, y, z
 *      (normals 4096 = 1); polygons: u8 output length, input length (words), flags, mode, then the words. */
#ifndef TMD_H
#define TMD_H

#include <stddef.h>
#include <stdint.h>

#define TURN 4096   /* the game's angles: a full turn */

typedef struct ModelPart {
    int16_t parent;
    int16_t place[3];    /* x, y, z (the game's: y down) */
    int16_t turn[3];     /* rx, ry, rz */
    uint16_t flags;
} ModelPart;

typedef struct ModelPolygon {
    int corners;             /* 3 or 4 (a quad's corners go 0 1 2 3 as a strip: triangles 0 1 2 and 1 3 2) */
    uint16_t vertex[4];
    int normal[4];           /* -1: none (unlit) */
    uint8_t uv[4][2];
    uint8_t color[4][3];     /* 128 = full (the colour multiplies the texture as 0x80 = 1) */
    int colors;              /* how many of color are its own (1: flat, else one a corner) */
    uint16_t texture_page;   /* the PlayStation's words: page, colour depth, blend mode */
    uint16_t palette;
    uint8_t mode, flag;      /* the TMD's */
    int textured, gouraud, lit, semi_transparent, double_sided;
} ModelPolygon;

typedef struct ModelObject {
    int16_t (*vertices)[3];
    int nvertices;
    int16_t (*normals)[3];
    int nnormals;
    ModelPolygon *polygons;
    int npolygons;
    int32_t scale;
} ModelObject;

typedef struct Model {
    ModelPart *parts;
    int nparts;
    ModelObject *objects;
    int nobjects;
} Model;

/* is it a model (an SPL with a TMD)? */
int model_is(const uint8_t *d, size_t n);
/* read one; 0 if it isn't one or is damaged */
int model_read(Model *m, const uint8_t *d, size_t n);
/* a TMD on its own (no parts: the town's terrain) */
int model_read_tmd(Model *m, const uint8_t *d, size_t n);
void model_free(Model *m);

#endif
