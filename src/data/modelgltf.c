/* Models as glTF (modelgltf.h). */
#include "modelgltf.h"

#include "cgltf.h"
#include "vramctx.h"

#include <math.h>
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define NORMAL_ONE 4096.0f   /* the game's normals: 4096 = 1 */
#define COLOR_ONE 128.0f     /* its colours: 0x80 = 1 */
#define PAGE_BLEND(word) ((word) >> 5 & 3)
#define PAGE_WITHOUT_BLEND 0x19F
#define MAX_MATERIALS 1024

static const char *kBlendNames[4] = {"half", "add", "subtract", "quarter"};

/* ---- what a polygon is drawn with ---- */
typedef struct MaterialKey {
    int textured;
    uint16_t page, palette;   /* page without its blend bits */
    int blend;                /* -1 opaque, else the page's blend mode */
    int lit, double_sided;
} MaterialKey;

int model_lamp(uint16_t palette) {
    int x = palette & 0x3F, y = palette >> 6;
    return y == LAMP_ROW && x < LAMPS ? x : -1;
}

static MaterialKey key_of(const ModelPolygon *p) {
    MaterialKey k;

    memset(&k, 0, sizeof(k));
    k.textured = p->textured;
    if (p->textured) {
        k.page = p->texture_page & PAGE_WITHOUT_BLEND;
        k.palette = p->palette;
    }
    k.blend = p->semi_transparent ? (int)PAGE_BLEND(p->texture_page) : -1;
    k.lit = p->lit;
    k.double_sided = p->double_sided;
    return k;
}

static int same_key(const MaterialKey *a, const MaterialKey *b) { return memcmp(a, b, sizeof(*a)) == 0; }

/* ---- the corners, in glTF's space ---- */
typedef struct Corner {
    float pos[3], normal[3], color[4], uv[2];
} Corner;

static void corner(const ModelObject *o, const ModelPolygon *p, int k, Corner *c) {
    const int16_t *v = o->vertices[p->vertex[k] < o->nvertices ? p->vertex[k] : 0];

    c->pos[0] = v[0] / MODEL_UNITS;
    c->pos[1] = -v[1] / MODEL_UNITS;   /* (half a turn about x: y and z change sign) */
    c->pos[2] = -v[2] / MODEL_UNITS;
    if (p->normal[k] >= 0 && p->normal[k] < o->nnormals) {
        const int16_t *nm = o->normals[p->normal[k]];
        c->normal[0] = nm[0] / NORMAL_ONE;
        c->normal[1] = -nm[1] / NORMAL_ONE;
        c->normal[2] = -nm[2] / NORMAL_ONE;
    } else {
        c->normal[0] = 0.0f;
        c->normal[1] = 1.0f;
        c->normal[2] = 0.0f;
    }
    c->color[0] = p->color[k][0] / COLOR_ONE;
    c->color[1] = p->color[k][1] / COLOR_ONE;
    c->color[2] = p->color[k][2] / COLOR_ONE;
    c->color[3] = 1.0f;
    c->uv[0] = p->uv[k][0] / (float)PAGE_TEXELS;
    c->uv[1] = p->uv[k][1] / (float)PAGE_TEXELS;
}

/* a polygon's triangles: glTF faces are counter-clockwise; the game's corners run the other way (a quad's as a
 * strip: 0 1 2, 1 3 2) */
static int triangles(const ModelPolygon *p, int *idx) {
    idx[0] = 0; idx[1] = 2; idx[2] = 1;
    if (p->corners == 4) {
        idx[3] = 1; idx[4] = 2; idx[5] = 3;
        return 2;
    }
    return 1;
}

/* an object's materials in the order their polygons first use them */
static int object_materials(const ModelObject *o, MaterialKey *keys) {
    int n = 0, i, k;

    for (i = 0; i < o->npolygons; i++) {
        MaterialKey key = key_of(&o->polygons[i]);
        for (k = 0; k < n && !same_key(&keys[k], &key); k++) {
        }
        if (k == n && n < MAX_MATERIALS) {
            keys[n++] = key;
        }
    }
    return n;
}

/* the corners of an object's triangles of one material, in order (malloc'd); their count */
static Corner *material_corners(const ModelObject *o, const MaterialKey *key, int *n) {
    Corner *c = malloc(sizeof(Corner) * ((size_t)o->npolygons * 6 + 1));
    int i, t, k, idx[6];

    *n = 0;
    for (i = 0; i < o->npolygons; i++) {
        const ModelPolygon *p = &o->polygons[i];
        MaterialKey pk = key_of(p);

        if (!same_key(&pk, key)) {
            continue;
        }
        for (t = triangles(p, idx) * 3, k = 0; k < t; k++) {
            corner(o, p, idx[k], &c[(*n)++]);
        }
    }
    return c;
}

/* the part's turn (rx, ry, rz) as the game's matrix (Ry Rx Rz), in glTF's space, as a quaternion (x y z w) */
static void part_rotation(const ModelPart *part, double q[4]) {
    double a[3], cx, sx, cy, sy, cz, sz, r[3][3], g[3][3], tr;
    static const double kFlip[3] = {1, -1, -1};
    int i, j;

    for (i = 0; i < 3; i++) {
        a[i] = part->turn[i] * 2.0 * M_PI / TURN;
    }
    cx = cos(a[0]); sx = sin(a[0]); cy = cos(a[1]); sy = sin(a[1]); cz = cos(a[2]); sz = sin(a[2]);
    r[0][0] = cy * cz + sy * sx * sz; r[0][1] = -cy * sz + sy * sx * cz; r[0][2] = sy * cx;
    r[1][0] = cx * sz;                r[1][1] = cx * cz;                 r[1][2] = -sx;
    r[2][0] = -sy * cz + cy * sx * sz; r[2][1] = sy * sz + cy * sx * cz; r[2][2] = cy * cx;
    for (i = 0; i < 3; i++) {
        for (j = 0; j < 3; j++) {
            g[i][j] = r[i][j] * kFlip[i] * kFlip[j];
        }
    }
    tr = g[0][0] + g[1][1] + g[2][2];
    if (tr > 0) {
        double s = sqrt(tr + 1.0) * 2;
        q[3] = s / 4; q[0] = (g[2][1] - g[1][2]) / s; q[1] = (g[0][2] - g[2][0]) / s; q[2] = (g[1][0] - g[0][1]) / s;
    } else if (g[0][0] > g[1][1] && g[0][0] > g[2][2]) {
        double s = sqrt(1.0 + g[0][0] - g[1][1] - g[2][2]) * 2;
        q[3] = (g[2][1] - g[1][2]) / s; q[0] = s / 4; q[1] = (g[0][1] + g[1][0]) / s; q[2] = (g[0][2] + g[2][0]) / s;
    } else if (g[1][1] > g[2][2]) {
        double s = sqrt(1.0 + g[1][1] - g[0][0] - g[2][2]) * 2;
        q[3] = (g[0][2] - g[2][0]) / s; q[0] = (g[0][1] + g[1][0]) / s; q[1] = s / 4; q[2] = (g[1][2] + g[2][1]) / s;
    } else {
        double s = sqrt(1.0 + g[2][2] - g[0][0] - g[1][1]) * 2;
        q[3] = (g[1][0] - g[0][1]) / s; q[0] = (g[0][2] + g[2][0]) / s; q[1] = (g[1][2] + g[2][1]) / s; q[2] = s / 4;
    }
}

static int32_t part_scale(const Model *m, int i) { return i < m->nobjects ? m->objects[i].scale : 0; }

/* where a part sits on its parent, in glTF's space: the game stores y upward here (unlike its vertices) */
static void part_translation(const Model *m, int i, float t[3]) {
    float s = ldexpf(1.0f, part_scale(m, i));
    const ModelPart *p = &m->parts[i];

    t[0] = p->place[0] * s / MODEL_UNITS;
    t[1] = p->place[1] * s / MODEL_UNITS;
    t[2] = -p->place[2] * s / MODEL_UNITS;
}

/* ---- writing ---- */
typedef struct Text {
    char *s;
    size_t n, cap;
} Text;

static void tf(Text *t, const char *fmt, ...) __attribute__((format(printf, 2, 3)));
static void tf(Text *t, const char *fmt, ...) {
    va_list ap;
    int k;

    for (;;) {
        va_start(ap, fmt);
        k = vsnprintf(t->s + t->n, t->cap - t->n, fmt, ap);
        va_end(ap);
        if (k >= 0 && t->n + (size_t)k < t->cap) {
            t->n += (size_t)k;
            return;
        }
        t->cap = t->cap * 2 + (size_t)k + 4096;
        t->s = realloc(t->s, t->cap);
    }
}

typedef struct Bin {
    uint8_t *d;
    size_t n, cap;
} Bin;

static size_t bin_put(Bin *b, const void *d, size_t n) {
    size_t at = b->n;
    if (b->n + n + 4 > b->cap) {
        b->cap = (b->n + n + 4) * 2;
        b->d = realloc(b->d, b->cap);
    }
    memcpy(b->d + b->n, d, n);
    b->n += n;
    while (b->n % 4) {
        b->d[b->n++] = 0;
    }
    return at;
}

/* the accessors and views, collected as written (one view each) */
typedef struct Accessors {
    Text views, accessors;
    int n;
} Accessors;

static int accessor(Accessors *a, Bin *b, const float *data, int count, int width, const char *type, int minmax) {
    size_t at = bin_put(b, data, sizeof(float) * (size_t)count * width);

    tf(&a->views, "%s{\"buffer\":0,\"byteOffset\":%zu,\"byteLength\":%zu}", a->n ? "," : "", at,
       sizeof(float) * (size_t)count * width);
    tf(&a->accessors, "%s{\"bufferView\":%d,\"componentType\":5126,\"count\":%d,\"type\":\"%s\"", a->n ? "," : "", a->n,
       count, type);
    if (minmax) {
        float lo[3] = {1e30f, 1e30f, 1e30f}, hi[3] = {-1e30f, -1e30f, -1e30f};
        int i, k;
        for (i = 0; i < count; i++) {
            for (k = 0; k < width; k++) {
                float v = data[(size_t)i * width + k];
                lo[k] = v < lo[k] ? v : lo[k];
                hi[k] = v > hi[k] ? v : hi[k];
            }
        }
        tf(&a->accessors, ",\"min\":[%.9g,%.9g,%.9g],\"max\":[%.9g,%.9g,%.9g]", lo[0], lo[1], lo[2], hi[0], hi[1], hi[2]);
    }
    tf(&a->accessors, "}");
    return a->n++;
}

int model_gltf(const Model *m, const char *set, const char *(*uri)(void *ctx, const char *texture), void *uri_ctx,
               const char *bin_uri, const char *comment, GltfOut *out) {
    Text nodes = {0}, meshes = {0}, materials = {0}, textures = {0}, images = {0}, json = {0};
    Accessors acc = {{0}, {0}, 0};
    Bin bin = {0};
    MaterialKey keys[MAX_MATERIALS], all[MAX_MATERIALS];
    int nall = 0, i, k, nnodes = m->nparts > m->nobjects ? m->nparts : m->nobjects, nmeshes = 0, roots = 0;
    int *mesh_of = calloc((size_t)nnodes + 1, sizeof(int));

    memset(out, 0, sizeof(*out));
    /* meshes: one per object with polygons */
    for (i = 0; i < m->nobjects; i++) {
        const ModelObject *o = &m->objects[i];
        int nk = object_materials(o, keys);

        mesh_of[i] = -1;
        if (nk == 0) {
            continue;
        }
        tf(&meshes, "%s{\"name\":\"object%d\",\"primitives\":[", nmeshes ? "," : "", i);
        for (k = 0; k < nk; k++) {
            int n, c, material, pos, nrm = -1, col, uv = -1;
            Corner *cs = material_corners(o, &keys[k], &n);
            float *buf = malloc(sizeof(float) * 4 * ((size_t)n + 1));

            for (material = 0; material < nall && !same_key(&all[material], &keys[k]); material++) {
            }
            if (material == nall) {
                if (nall == MAX_MATERIALS) {
                    material = 0;   /* (never so many: the largest model has ~100) */
                } else {
                    all[nall++] = keys[k];
                }
            }
            for (c = 0; c < n; c++) memcpy(buf + c * 3, cs[c].pos, sizeof(float) * 3);
            pos = accessor(&acc, &bin, buf, n, 3, "VEC3", 1);
            if (keys[k].lit) {
                for (c = 0; c < n; c++) memcpy(buf + c * 3, cs[c].normal, sizeof(float) * 3);
                nrm = accessor(&acc, &bin, buf, n, 3, "VEC3", 0);
            }
            for (c = 0; c < n; c++) memcpy(buf + c * 4, cs[c].color, sizeof(float) * 4);
            col = accessor(&acc, &bin, buf, n, 4, "VEC4", 0);
            if (keys[k].textured) {
                for (c = 0; c < n; c++) memcpy(buf + c * 2, cs[c].uv, sizeof(float) * 2);
                uv = accessor(&acc, &bin, buf, n, 2, "VEC2", 0);
            }
            tf(&meshes, "%s{\"attributes\":{\"POSITION\":%d,\"COLOR_0\":%d", k ? "," : "", pos, col);
            if (nrm >= 0) tf(&meshes, ",\"NORMAL\":%d", nrm);
            if (uv >= 0) tf(&meshes, ",\"TEXCOORD_0\":%d", uv);
            tf(&meshes, "},\"material\":%d}", material);
            free(cs);
            free(buf);
        }
        tf(&meshes, "]}");
        mesh_of[i] = nmeshes++;
    }
    /* materials, and the textures they name */
    for (k = 0; k < nall; k++) {
        const MaterialKey *mk = &all[k];
        char name[64] = "", tex[64];
        int t;

        if (mk->textured) {
            vram_texture_name(mk->page, mk->palette, tex, sizeof(tex));
            for (t = 0; t < out->ntextures && strcmp(out->textures[t], tex) != 0; t++) {
            }
            if (t == out->ntextures && out->ntextures < MODEL_MAX_TEXTURES) {
                snprintf(out->textures[out->ntextures++], sizeof(out->textures[0]), "%s", tex);
                tf(&images, "%s{\"uri\":\"%s\"}", t ? "," : "", uri != NULL ? uri(uri_ctx, tex) : "");
                tf(&textures, "%s{\"source\":%d,\"sampler\":0}", t ? "," : "", t);
            }
            snprintf(name, sizeof(name), "%s", tex);
            tf(&materials, "%s{\"name\":\"%s%s%s%s\",\"pbrMetallicRoughness\":{\"baseColorTexture\":{\"index\":%d},"
               "\"metallicFactor\":0}", k ? "," : "", name, mk->blend >= 0 ? "-" : "", mk->blend >= 0 ? kBlendNames[mk->blend] : "",
               mk->lit ? "" : "-unlit", t);
        } else {
            tf(&materials, "%s{\"name\":\"colour%s%s%s\",\"pbrMetallicRoughness\":{\"metallicFactor\":0}", k ? "," : "",
               mk->blend >= 0 ? "-" : "", mk->blend >= 0 ? kBlendNames[mk->blend] : "", mk->lit ? "" : "-unlit");
        }
        tf(&materials, ",\"alphaMode\":\"%s\"%s", mk->blend >= 0 ? "BLEND" : mk->textured ? "MASK" : "OPAQUE",
           mk->double_sided ? ",\"doubleSided\":true" : "");
        if (!mk->lit) {
            tf(&materials, ",\"extensions\":{\"KHR_materials_unlit\":{}}");
        }
        tf(&materials, ",\"extras\":{\"om\":{\"texture-set\":\"%s\"", mk->textured ? set : "none");
        if (mk->textured) tf(&materials, ",\"texture\":\"%s\"", name);
        if (mk->textured && model_lamp(mk->palette) >= 0) tf(&materials, ",\"lamp\":%d", model_lamp(mk->palette));
        if (mk->blend >= 0) tf(&materials, ",\"blend\":\"%s\"", kBlendNames[mk->blend]);
        tf(&materials, "}}}");
    }
    /* nodes: the parts (and objects past them) */
    for (i = 0; i < nnodes; i++) {
        double q[4] = {0, 0, 0, 1};
        float t[3] = {0, 0, 0};
        int child, first = 1;

        tf(&nodes, "%s{\"name\":\"part%d\"", i ? "," : "", i);
        if (i < m->nparts) {
            const ModelPart *p = &m->parts[i];
            part_translation(m, i, t);
            part_rotation(p, q);
            tf(&nodes, ",\"translation\":[%.9g,%.9g,%.9g],\"rotation\":[%.9g,%.9g,%.9g,%.9g]", t[0], t[1], t[2], q[0], q[1], q[2], q[3]);
            tf(&nodes, ",\"extras\":{\"om\":{\"place\":[%d,%d,%d],\"turn\":[%d,%d,%d],\"scale\":%d,\"flags\":%u}}",
               p->place[0], p->place[1], p->place[2], p->turn[0], p->turn[1], p->turn[2], part_scale(m, i), p->flags);
            for (child = 0; child < m->nparts; child++) {
                if (m->parts[child].parent == i && child != i) {
                    tf(&nodes, "%s%d", first ? ",\"children\":[" : ",", child);
                    first = 0;
                }
            }
            if (!first) tf(&nodes, "]");
        }
        if (i < m->nobjects && mesh_of[i] >= 0) {
            tf(&nodes, ",\"mesh\":%d", mesh_of[i]);
        }
        tf(&nodes, "}");
    }
    tf(&json, "{\"asset\":{\"version\":\"2.0\",\"generator\":\"OpenMizzurna's import\",\"extras\":{\"om\":\"%s\"}},", comment);
    tf(&json, "\"extensionsUsed\":[\"KHR_materials_unlit\"],\"scene\":0,\"scenes\":[{\"nodes\":[");
    for (i = 0; i < nnodes; i++) {
        if (i >= m->nparts || m->parts[i].parent < 0 || m->parts[i].parent >= m->nparts || m->parts[i].parent == i) {
            tf(&json, "%s%d", roots++ ? "," : "", i);
        }
    }
    tf(&json, "]}],\"nodes\":[%s],\"meshes\":[%s]", nodes.s ? nodes.s : "", meshes.s ? meshes.s : "");
    if (nall) tf(&json, ",\"materials\":[%s]", materials.s);
    if (out->ntextures) {
        tf(&json, ",\"textures\":[%s],\"images\":[%s],\"samplers\":[{\"magFilter\":9728,\"minFilter\":9728}]", textures.s, images.s);
    }
    if (acc.n) {
        tf(&json, ",\"accessors\":[%s],\"bufferViews\":[%s]", acc.accessors.s, acc.views.s);
    }
    tf(&json, ",\"buffers\":[{\"uri\":\"%s\",\"byteLength\":%zu}]}\n", bin_uri, bin.n);
    free(nodes.s); free(meshes.s); free(materials.s); free(textures.s); free(images.s);
    free(acc.views.s); free(acc.accessors.s); free(mesh_of);
    out->json = json.s;
    out->json_size = json.n;
    out->bin = bin.d;
    out->bin_size = bin.n;
    return 1;
}

void model_gltf_free(GltfOut *out) {
    free(out->json);
    free(out->bin);
    memset(out, 0, sizeof(*out));
}

/* ---- the check ---- */
static int same_floats(const cgltf_accessor *a, const float *want, int count, int width) {
    float v[4];
    int i;

    if (a == NULL || (int)a->count != count) {
        return 0;
    }
    for (i = 0; i < count; i++) {
        if (!cgltf_accessor_read_float(a, (cgltf_size)i, v, (cgltf_size)width) ||
            memcmp(v, want + (size_t)i * width, sizeof(float) * (size_t)width) != 0) {
            return 0;
        }
    }
    return 1;
}

static const cgltf_accessor *attribute(const cgltf_primitive *p, cgltf_attribute_type type) {
    cgltf_size i;
    for (i = 0; i < p->attributes_count; i++) {
        if (p->attributes[i].type == type) {
            return p->attributes[i].data;
        }
    }
    return NULL;
}

int model_gltf_check(const Model *m, const char *json, size_t json_size, const uint8_t *bin, size_t bin_size,
                     char *why, size_t nwhy) {
    cgltf_options options;
    cgltf_data *data = NULL;
    int i, k, ok = 1, mesh = 0;

    memset(&options, 0, sizeof(options));
    if (cgltf_parse(&options, json, json_size, &data) != cgltf_result_success || data->buffers_count != 1 ||
        data->buffers[0].size != bin_size) {
        snprintf(why, nwhy, "not glTF cgltf reads");
        if (data) cgltf_free(data);
        return 0;
    }
    data->buffers[0].data = (void *)bin;
    data->buffers[0].data_free_method = cgltf_data_free_method_none;
    if (cgltf_validate(data) != cgltf_result_success) {
        snprintf(why, nwhy, "glTF that doesn't validate");
        cgltf_free(data);
        return 0;
    }
    for (i = 0; ok && i < m->nparts; i++) {   /* the parts: their exact place and turn */
        char extras[512] = "", want[256];
        cgltf_size n = sizeof(extras);
        const ModelPart *p = &m->parts[i];

        if ((size_t)i >= data->nodes_count) {
            ok = 0;
            break;
        }
        cgltf_copy_extras_json(data, &data->nodes[i].extras, extras, &n);
        snprintf(want, sizeof(want), "\"place\":[%d,%d,%d],\"turn\":[%d,%d,%d]", p->place[0], p->place[1], p->place[2],
                 p->turn[0], p->turn[1], p->turn[2]);
        if (strstr(extras, want) == NULL) {
            snprintf(why, nwhy, "part %d's place or turn differs", i);
            ok = 0;
        }
    }
    for (i = 0; ok && i < m->nobjects; i++) {   /* the objects: every corner of every polygon */
        const ModelObject *o = &m->objects[i];
        MaterialKey keys[MAX_MATERIALS];
        int nk = object_materials(o, keys);
        const cgltf_mesh *me;

        if (nk == 0) {
            continue;
        }
        if ((size_t)mesh >= data->meshes_count || data->meshes[mesh].primitives_count != (cgltf_size)nk) {
            snprintf(why, nwhy, "object %d's mesh is missing or has other materials", i);
            ok = 0;
            break;
        }
        me = &data->meshes[mesh++];
        for (k = 0; ok && k < nk; k++) {
            int n, c;
            Corner *cs = material_corners(o, &keys[k], &n);
            float *buf = malloc(sizeof(float) * 4 * ((size_t)n + 1));
            const cgltf_primitive *pr = &me->primitives[k];

            for (c = 0; c < n; c++) memcpy(buf + c * 3, cs[c].pos, sizeof(float) * 3);
            ok = same_floats(attribute(pr, cgltf_attribute_type_position), buf, n, 3);
            if (ok && keys[k].lit) {
                for (c = 0; c < n; c++) memcpy(buf + c * 3, cs[c].normal, sizeof(float) * 3);
                ok = same_floats(attribute(pr, cgltf_attribute_type_normal), buf, n, 3);
            }
            if (ok) {
                for (c = 0; c < n; c++) memcpy(buf + c * 4, cs[c].color, sizeof(float) * 4);
                ok = same_floats(attribute(pr, cgltf_attribute_type_color), buf, n, 4);
            }
            if (ok && keys[k].textured) {
                for (c = 0; c < n; c++) memcpy(buf + c * 2, cs[c].uv, sizeof(float) * 2);
                ok = same_floats(attribute(pr, cgltf_attribute_type_texcoord), buf, n, 2);
            }
            if (!ok) {
                snprintf(why, nwhy, "object %d, material %d: the corners differ", i, k);
            }
            free(cs);
            free(buf);
        }
    }
    cgltf_free(data);
    return ok;
}
