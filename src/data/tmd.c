/* The game's models (tmd.h). */
#include "tmd.h"

#include <stdlib.h>
#include <string.h>

#define SPL_MAGIC 0x88FA
#define SPL_PART 16
#define TMD_ID 0x41
#define TMD_OBJECT 28
#define TMD_HEADER 12

/* a polygon's mode byte */
#define MODE_POLYGON 1        /* bits 5..7 */
#define MODE_GOURAUD 0x10
#define MODE_QUAD 0x08
#define MODE_TEXTURED 0x04
#define MODE_SEMI 0x02
/* and its flag byte */
#define FLAG_UNLIT 0x01
#define FLAG_DOUBLE_SIDED 0x02
#define FLAG_GRADATION 0x04   /* a colour a corner */

static uint16_t le16(const uint8_t *p) { return (uint16_t)(p[0] | p[1] << 8); }
static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }

int model_is(const uint8_t *d, size_t n) {
    size_t parts, t;

    if (n < 4 || le16(d) != SPL_MAGIC) {
        return 0;
    }
    parts = le16(d + 2);
    t = 4 + parts * SPL_PART;
    return t + TMD_HEADER <= n && le32(d + t) == TMD_ID;
}

static int read_polygon(ModelPolygon *p, const uint8_t *pk, size_t avail, size_t *len) {
    unsigned ilen = pk[1], k, w = 0, halves = 0, i;
    const uint8_t *words = pk + 4;
    uint16_t half[32];

    memset(p, 0, sizeof(*p));
    p->flag = pk[2];
    p->mode = pk[3];
    *len = 4 + (size_t)ilen * 4;
    if (*len > avail || p->mode >> 5 != MODE_POLYGON || ilen > 16) {
        return 0;
    }
    p->corners = p->mode & MODE_QUAD ? 4 : 3;
    p->textured = (p->mode & MODE_TEXTURED) != 0;
    p->gouraud = (p->mode & MODE_GOURAUD) != 0;
    p->semi_transparent = (p->mode & MODE_SEMI) != 0;
    p->lit = !(p->flag & FLAG_UNLIT);
    p->double_sided = (p->flag & FLAG_DOUBLE_SIDED) != 0;
    if (p->textured) {   /* a word a corner: u, v, then the palette (corner 0) and the texture page (corner 1) */
        for (k = 0; k < (unsigned)p->corners; k++, w++) {
            uint32_t x = le32(words + w * 4);
            p->uv[k][0] = (uint8_t)x;
            p->uv[k][1] = (uint8_t)(x >> 8);
            if (k == 0) {
                p->palette = (uint16_t)(x >> 16);
            } else if (k == 1) {
                p->texture_page = (uint16_t)(x >> 16);
            }
        }
    }
    if (!(p->lit && p->textured)) {   /* colours: one, or one a corner (gradation, or unlit gouraud) */
        p->colors = (p->flag & FLAG_GRADATION) || (p->gouraud && !p->lit) ? p->corners : 1;
        for (k = 0; k < (unsigned)p->colors; k++, w++) {
            memcpy(p->color[k], words + w * 4, 3);
        }
        for (k = (unsigned)p->colors; k < (unsigned)p->corners; k++) {
            memcpy(p->color[k], p->color[0], 3);
        }
    } else {
        for (k = 0; k < (unsigned)p->corners; k++) {   /* (lit and textured: the texture as it is) */
            p->color[k][0] = p->color[k][1] = p->color[k][2] = 0x80;
        }
    }
    for (i = w; i < ilen; i++) {
        half[halves++] = le16(words + i * 4);
        half[halves++] = le16(words + i * 4 + 2);
    }
    for (k = 0; k < 4; k++) {
        p->normal[k] = -1;
    }
    if (p->lit && p->gouraud) {
        for (k = 0; k < (unsigned)p->corners; k++) {
            p->normal[k] = half[2 * k];
            p->vertex[k] = half[2 * k + 1];
        }
    } else if (p->lit) {
        for (k = 0; k < (unsigned)p->corners; k++) {
            p->normal[k] = half[0];
            p->vertex[k] = half[1 + k];
        }
    } else {
        for (k = 0; k < (unsigned)p->corners; k++) {
            p->vertex[k] = half[k];
        }
    }
    return 1;
}

int model_read(Model *m, const uint8_t *d, size_t n) {
    size_t t, base;
    int i, k;

    memset(m, 0, sizeof(*m));
    if (!model_is(d, n)) {
        return 0;
    }
    m->nparts = le16(d + 2);
    m->parts = calloc((size_t)m->nparts + 1, sizeof(ModelPart));
    for (i = 0; i < m->nparts; i++) {
        const uint8_t *e = d + 4 + i * SPL_PART;
        ModelPart *p = &m->parts[i];

        p->parent = (int16_t)le16(e);
        p->place[0] = (int16_t)le16(e + 2);
        p->place[1] = (int16_t)le16(e + 4);
        p->place[2] = (int16_t)le16(e + 6);
        p->turn[1] = (int16_t)le16(e + 8);   /* (stored ry, rx, rz) */
        p->turn[0] = (int16_t)le16(e + 10);
        p->turn[2] = (int16_t)le16(e + 12);
        p->flags = le16(e + 14);
    }
    t = 4 + (size_t)m->nparts * SPL_PART;
    m->nobjects = (int)le32(d + t + 8);
    base = t + TMD_HEADER;   /* (offsets count from the object table) */
    if (base + (size_t)m->nobjects * TMD_OBJECT > n) {
        model_free(m);
        return 0;
    }
    m->objects = calloc((size_t)m->nobjects + 1, sizeof(ModelObject));
    for (i = 0; i < m->nobjects; i++) {
        const uint8_t *e = d + base + i * TMD_OBJECT;
        ModelObject *o = &m->objects[i];
        size_t vt = base + le32(e), nt = base + le32(e + 8), pt = base + le32(e + 16);

        o->nvertices = (int)le32(e + 4);
        o->nnormals = (int)le32(e + 12);
        o->npolygons = (int)le32(e + 20);
        o->scale = (int32_t)le32(e + 24);
        if (vt + (size_t)o->nvertices * 8 > n || nt + (size_t)o->nnormals * 8 > n || pt > n) {
            model_free(m);
            return 0;
        }
        o->vertices = calloc((size_t)o->nvertices + 1, sizeof(*o->vertices));
        o->normals = calloc((size_t)o->nnormals + 1, sizeof(*o->normals));
        o->polygons = calloc((size_t)o->npolygons + 1, sizeof(ModelPolygon));
        for (k = 0; k < o->nvertices; k++) {
            o->vertices[k][0] = (int16_t)le16(d + vt + k * 8);
            o->vertices[k][1] = (int16_t)le16(d + vt + k * 8 + 2);
            o->vertices[k][2] = (int16_t)le16(d + vt + k * 8 + 4);
        }
        for (k = 0; k < o->nnormals; k++) {
            o->normals[k][0] = (int16_t)le16(d + nt + k * 8);
            o->normals[k][1] = (int16_t)le16(d + nt + k * 8 + 2);
            o->normals[k][2] = (int16_t)le16(d + nt + k * 8 + 4);
        }
        for (k = 0; k < o->npolygons; k++) {
            size_t len;
            if (!read_polygon(&o->polygons[k], d + pt, n - pt, &len)) {
                model_free(m);
                return 0;
            }
            pt += len;
        }
    }
    return 1;
}

void model_free(Model *m) {
    int i;

    for (i = 0; m->objects != NULL && i < m->nobjects; i++) {
        free(m->objects[i].vertices);
        free(m->objects[i].normals);
        free(m->objects[i].polygons);
    }
    free(m->objects);
    free(m->parts);
    memset(m, 0, sizeof(*m));
}
