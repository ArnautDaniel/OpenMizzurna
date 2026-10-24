/* The 3D layer (scene3d.h). */
#include "scene3d.h"

#include "render.h"

#include "../core/files.h"
#include "../data/png.h"
#include "../platform/gl.h"
#include "cgltf.h"

#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define WORLD_TO_GLTF (1.0f / 512.0f)   /* world units are 2 model units; 256 model units a glTF unit */
#define TURN 4096.0f
#define SCREEN_HALF_HEIGHT 120.0f       /* the PlayStation's screen: 240 high */
#define NEAR_PLANE 0.02f
#define FAR_PLANE 2000.0f
#define MAX_DRAWS 1024
#define TEXTURES_AT_FIRST 256
#define FLOATS_PER_VERTEX 12            /* position 3, normal 3, colour 4, uv 2 */

enum { BLEND_NONE, BLEND_HALF, BLEND_ADD, BLEND_SUBTRACT, BLEND_QUARTER };

typedef struct Material {
    char texture[96];   /* its name ("page9-4bit-palette288-247"), or "" for none */
    int area_set;       /* 1: the area's set; 0: the model's own (uri) */
    char uri[160];      /* the model's own: its library path */
    int blend, lit, double_sided;
    int lamp;           /* a town street lamp's light (its lit texture when the lamp is on); -1: none */
} Material;

typedef struct Primitive {
    int first, count;   /* vertices */
    int node, material;
} Primitive;

struct Model3D {
    uint32_t vao, vbo;
    Primitive *prims;
    int nprims;
    Material *materials;
    int nmaterials;
    float (*node_world)[16];   /* each node's rest transform in the model */
    float (*node_local)[16];   /* and on its parent (its translation and rest turn) */
    int *parent;               /* -1: a root */
    float *part_scale;         /* each part's place scale (its extras: 2^scale) - a motion's root move is in it */
    short (*rest_turn)[3];     /* each part's rest turn, the game's (its extras' "turn") - objects' turns add to it */
    int nnodes;
    float *positions;          /* every vertex's place in its node (x y z): the ground's height is found in them */
    int nverts;
};

#define MAX_SHOWN 256
#define POOL_AT_FIRST 4096          /* node matrices for posed draws, grown as a frame needs */
typedef struct Draw {
    Model3D *m;
    float world[16];
    int posed;                          /* a figure in a motion's frame, or objects moved: its nodes' transforms */
    int node;                           /* (its first in the frame's pool of node matrices) */
    unsigned char shown[MAX_SHOWN];   /* a byte a node (0: hidden) */
    int nshown;                       /* (nodes past it are shown) */
} Draw;

#define MAX_QUADS 4096
typedef struct Quad {
    GpuTexture t;
    float corner[4][3];   /* glTF's space: 0 1 the top, 2 3 the bottom (the PlayStation's order) */
    float shade;          /* 1.0: the texture as it is */
    int blend;
    float uv[4];          /* the part of the texture across it: u0 v0 u1 v1 */
} Quad;

typedef struct Tex {
    char path[200];
    GpuTexture t;
    int missing;
    uint32_t hash;
} Tex;

static struct {
    uint32_t program;
    int loc_mvp, loc_model, loc_modelview, loc_lit, loc_textured, loc_pass, loc_semi;
    int loc_light_way, loc_light_color, loc_ambient, loc_fog, loc_fog_color, loc_fog_cue;
    Scene3DLight light;
    int light_set;
    Draw draws[MAX_DRAWS];
    int ndraws;
    Tex *tex;
    int ntex, captex;
    Quad *quads;
    int nquads;
    float (*pool)[16];   /* this frame's posed draws' node matrices */
    int npool, cappool;
    uint32_t quad_vao, quad_vbo;
    int area;
    unsigned char lamp_lit[SCENE3D_LAMPS];
    int anim_shown[SCENE3D_ANIMS];   /* each of the area's texture animations' frame shown, + 1 (0: none) */
    float view[16], eye[3];
    float fovy, projection_h;
    int have_camera;
} S;

static const char *kVS =
    "#version 460 core\n"
    "#define GTE_MAX 1.9921875\n"   /* (the lighting's result: 255 over the colours' 128) */
    "#define MODEL_UNITS 256.0\n"   /* (glTF's units in the models') */
    "layout(location = 0) in vec3 a_pos;\n"
    "layout(location = 1) in vec3 a_normal;\n"
    "layout(location = 2) in vec4 a_color;\n"
    "layout(location = 3) in vec2 a_uv;\n"
    "uniform mat4 mvp;\n"
    "uniform mat4 model;\n"
    "uniform mat4 modelview;\n"
    "uniform int lit;\n"
    "uniform vec3 light_way[3];\n"     /* the way each light shines (glTF's axes) */
    "uniform vec3 light_color[3];\n"   /* 1.0: full */
    "uniform vec3 ambient;\n"
    "uniform int fog;\n"
    "uniform vec3 fog_color;\n"        /* in the vertex colours' units: 1.0 the texture as it is */
    "uniform vec3 fog_cue;\n"          /* the PlayStation's depth cue: a, b, the projection's h */
    "out vec4 v_color;\n"
    "out vec2 v_uv;\n"
    "void main() {\n"
    "    vec3 n = normalize(mat3(model) * a_normal);\n"
    "    vec3 c = a_color.rgb;\n"
    "    if (lit != 0) {\n"
    "        vec3 l = ambient;\n"
    "        for (int i = 0; i < 3; i++) l += max(dot(n, -light_way[i]), 0.0) * light_color[i];\n"
    "        c = min(c * l, vec3(GTE_MAX));\n"
    "        if (fog != 0) {\n"   /* how far into the fog: b + a h / depth (the depth in the models' units) */
    "            float depth = max(-(modelview * vec4(a_pos, 1.0)).z * MODEL_UNITS, 1.0);\n"
    "            c = mix(c, fog_color, clamp(fog_cue.y + fog_cue.x * fog_cue.z / depth, 0.0, 1.0));\n"
    "        }\n"
    "    }\n"
    "    v_color = vec4(c, a_color.a);\n"
    "    v_uv = a_uv;\n"
    "    gl_Position = mvp * vec4(a_pos, 1.0);\n"
    "}\n";

static const char *kFS =
    "#version 460 core\n"
    "in vec4 v_color;\n"
    "in vec2 v_uv;\n"
    "layout(binding = 0) uniform sampler2D image;\n"
    "uniform int textured;\n"
    "uniform int pass;\n"      /* 0 the solid texels, 1 the blending ones */
    "uniform int semi;\n"      /* the polygon is semi-transparent */
    "out vec4 color;\n"
    "void main() {\n"
    "    vec4 t = textured != 0 ? texture(image, v_uv) : vec4(1.0);\n"
    "    if (t.a < 0.25) discard;\n"                                     /* the colour 0: never drawn */
    "    bool blends = semi != 0 && (textured == 0 || t.a < 0.75);\n"   /* a texel marked to blend */
    "    if (blends != (pass == 1)) discard;\n"
    "    color = vec4(min(t.rgb * v_color.rgb, vec3(1.0)), 1.0);\n"
    "}\n";

/* ---- matrices (column-major, as GL) ---- */
static void mat_identity(float *m) {
    memset(m, 0, sizeof(float) * 16);
    m[0] = m[5] = m[10] = m[15] = 1;
}

static void mat_mul(float *out, const float *a, const float *b) {
    float r[16];
    int i, j, k;

    for (i = 0; i < 4; i++) {
        for (j = 0; j < 4; j++) {
            float s = 0;
            for (k = 0; k < 4; k++) {
                s += a[k * 4 + j] * b[i * 4 + k];
            }
            r[i * 4 + j] = s;
        }
    }
    memcpy(out, r, sizeof(r));
}

static void world_point(const float w[3], float *out) {   /* the game's world to glTF's space */
    out[0] = w[0] * WORLD_TO_GLTF;
    out[1] = -w[1] * WORLD_TO_GLTF;
    out[2] = -w[2] * WORLD_TO_GLTF;
}

static void look_at(float *m, const float eye[3], const float at[3]) {
    float f[3] = {at[0] - eye[0], at[1] - eye[1], at[2] - eye[2]}, up[3] = {0, 1, 0}, s[3], u[3], len;
    int i;

    len = sqrtf(f[0] * f[0] + f[1] * f[1] + f[2] * f[2]);
    for (i = 0; i < 3; i++) {
        f[i] /= len > 0 ? len : 1;
    }
    s[0] = f[1] * up[2] - f[2] * up[1];
    s[1] = f[2] * up[0] - f[0] * up[2];
    s[2] = f[0] * up[1] - f[1] * up[0];
    len = sqrtf(s[0] * s[0] + s[1] * s[1] + s[2] * s[2]);
    for (i = 0; i < 3; i++) {
        s[i] /= len > 0 ? len : 1;
    }
    u[0] = s[1] * f[2] - s[2] * f[1];
    u[1] = s[2] * f[0] - s[0] * f[2];
    u[2] = s[0] * f[1] - s[1] * f[0];
    mat_identity(m);
    m[0] = s[0], m[4] = s[1], m[8] = s[2];
    m[1] = u[0], m[5] = u[1], m[9] = u[2];
    m[2] = -f[0], m[6] = -f[1], m[10] = -f[2];
    m[12] = -(s[0] * eye[0] + s[1] * eye[1] + s[2] * eye[2]);
    m[13] = -(u[0] * eye[0] + u[1] * eye[1] + u[2] * eye[2]);
    m[14] = f[0] * eye[0] + f[1] * eye[1] + f[2] * eye[2];
}

static void perspective(float *m, float fovy, float aspect) {
    float t = 1.0f / tanf(fovy / 2);

    memset(m, 0, sizeof(float) * 16);
    m[0] = t / aspect;
    m[5] = t;
    m[10] = (FAR_PLANE + NEAR_PLANE) / (NEAR_PLANE - FAR_PLANE);
    m[11] = -1;
    m[14] = 2 * FAR_PLANE * NEAR_PLANE / (NEAR_PLANE - FAR_PLANE);
}

/* ---- textures ---- */
/* every texture asked for, by its library path (one that isn't there too: asked again, it isn't), found by a hash
 * of the path; kept for the run (the town's hundreds: a few hundred megabytes of video memory at most) */
static uint32_t path_hash(const char *s) {
    uint32_t h = 2166136261u;   /* (FNV-1a) */

    while (*s) {
        h = (h ^ (uint8_t)*s++) * 16777619u;
    }
    return h;
}

static GpuTexture texture_at(const char *path, int *ok) {
    size_t n;
    uint8_t *file, *rgba;
    int i, w = 0, h = 0;
    uint32_t hash = path_hash(path);
    Tex *t;

    for (i = 0; i < S.ntex; i++) {
        if (S.tex[i].hash == hash && strcmp(S.tex[i].path, path) == 0) {
            *ok = !S.tex[i].missing;
            return S.tex[i].t;
        }
    }
    if (S.ntex == S.captex) {
        S.captex = S.captex ? S.captex * 2 : TEXTURES_AT_FIRST;
        S.tex = realloc(S.tex, sizeof(Tex) * (size_t)S.captex);
    }
    file = files_read(path, &n);
    rgba = file != NULL ? png_read_rgba(file, n, &w, &h) : NULL;
    free(file);
    t = &S.tex[S.ntex++];
    memset(t, 0, sizeof(*t));
    snprintf(t->path, sizeof(t->path), "%s", path);
    t->hash = hash;
    t->missing = rgba == NULL;
    if (rgba != NULL) {
        t->t = render_texture_rgba(rgba, w, h);
        free(rgba);
    }
    *ok = !t->missing;
    return t->t;
}

static GpuTexture material_texture(const Material *mt, int *ok) {
    char path[200];
    GpuTexture t;

    *ok = 0;
    if (!mt->texture[0]) {
        return t = (GpuTexture){0, 0, 0};
    }
    if (mt->lamp >= 0 && mt->lamp < SCENE3D_LAMPS && S.lamp_lit[mt->lamp]) {
        snprintf(path, sizeof(path), "textures/lamps/%s.png", mt->texture);
        t = texture_at(path, ok);
        if (*ok) {
            return t;
        }
    }
    if (mt->area_set) {
        int a;
        for (a = 0; a < SCENE3D_ANIMS; a++) {   /* (a frame of one of the area's texture animations) */
            if (S.anim_shown[a] > 0) {
                snprintf(path, sizeof(path), "textures/area-%03d/anim-%d/%d/%s.png", S.area, a, S.anim_shown[a] - 1,
                         mt->texture);
                t = texture_at(path, ok);
                if (*ok) {
                    return t;
                }
            }
        }
        snprintf(path, sizeof(path), "textures/area-%03d/%s.png", S.area, mt->texture);
        t = texture_at(path, ok);
        if (*ok) {
            return t;
        }
        snprintf(path, sizeof(path), "textures/common/%s.png", mt->texture);
        return texture_at(path, ok);
    }
    return texture_at(mt->uri, ok);
}

/* ---- models ---- */
static void read_extras(const cgltf_data *data, const cgltf_extras *e, char *out, size_t n) {
    cgltf_size size = n;

    out[0] = 0;
    if (cgltf_copy_extras_json(data, e, out, &size) != cgltf_result_success) {
        out[0] = 0;
    }
}

static int json_int(const char *json, const char *key, int none) {   /* "key": n */
    char pat[64];
    const char *p;

    snprintf(pat, sizeof(pat), "\"%s\":", key);
    p = strstr(json, pat);
    return p != NULL ? atoi(p + strlen(pat)) : none;
}

static void json_string(const char *json, const char *key, char *out, size_t n) {   /* "key": "value" */
    char pat[64];
    const char *p;
    size_t k = 0;

    out[0] = 0;
    snprintf(pat, sizeof(pat), "\"%s\"", key);
    p = strstr(json, pat);
    if (p == NULL || (p = strchr(p + strlen(pat), '"')) == NULL) {
        return;
    }
    for (p++; *p && *p != '"' && k + 1 < n; p++) {
        out[k++] = *p;
    }
    out[k] = 0;
}

Model3D *model3d_load(const char *path) {
    size_t n, bn;
    char *json = (char *)files_read(path, &n), dir[200], bin[260], extras[512], set[16], blend[16];
    cgltf_options options;
    cgltf_data *data = NULL;
    Model3D *m;
    float *verts;
    int nverts = 0, cap = 0;
    size_t i, k, v;
    uint8_t *binary;
    const char *slash;

    if (json == NULL) {
        return NULL;
    }
    memset(&options, 0, sizeof(options));
    if (cgltf_parse(&options, json, n, &data) != cgltf_result_success || data->buffers_count != 1) {
        free(json);
        return NULL;
    }
    slash = strrchr(path, '/');
    snprintf(dir, sizeof(dir), "%.*s", slash ? (int)(slash - path + 1) : 0, path);
    snprintf(bin, sizeof(bin), "%s%s", dir, data->buffers[0].uri ? data->buffers[0].uri : "");
    binary = files_read(bin, &bn);
    if (binary == NULL || bn < data->buffers[0].size) {
        free(binary);
        cgltf_free(data);
        free(json);
        return NULL;
    }
    data->buffers[0].data = binary;
    data->buffers[0].data_free_method = cgltf_data_free_method_none;

    m = calloc(1, sizeof(*m));
    m->nmaterials = (int)data->materials_count;
    m->materials = calloc(data->materials_count + 1, sizeof(Material));
    for (i = 0; i < data->materials_count; i++) {
        const cgltf_material *src = &data->materials[i];
        Material *mt = &m->materials[i];

        read_extras(data, &src->extras, extras, sizeof(extras));
        json_string(extras, "texture-set", set, sizeof(set));
        json_string(extras, "texture", mt->texture, sizeof(mt->texture));
        json_string(extras, "blend", blend, sizeof(blend));
        mt->area_set = strcmp(set, "area") == 0;
        mt->lamp = json_int(extras, "lamp", -1);
        if (strcmp(set, "none") == 0) {
            mt->texture[0] = 0;
        }
        if (!mt->area_set && mt->texture[0] && src->pbr_metallic_roughness.base_color_texture.texture &&
            src->pbr_metallic_roughness.base_color_texture.texture->image &&
            src->pbr_metallic_roughness.base_color_texture.texture->image->uri) {
            snprintf(mt->uri, sizeof(mt->uri), "%s%s", dir, src->pbr_metallic_roughness.base_color_texture.texture->image->uri);
        }
        mt->blend = strcmp(blend, "half") == 0 ? BLEND_HALF : strcmp(blend, "add") == 0 ? BLEND_ADD :
                    strcmp(blend, "subtract") == 0 ? BLEND_SUBTRACT : strcmp(blend, "quarter") == 0 ? BLEND_QUARTER :
                    BLEND_NONE;
        mt->lit = !src->unlit;
        mt->double_sided = src->double_sided;
    }
    m->nnodes = (int)data->nodes_count;
    m->node_world = calloc(data->nodes_count + 1, sizeof(*m->node_world));
    m->node_local = calloc(data->nodes_count + 1, sizeof(*m->node_local));
    m->parent = calloc(data->nodes_count + 1, sizeof(int));
    m->part_scale = calloc(data->nodes_count + 1, sizeof(float));
    m->rest_turn = calloc(data->nodes_count + 1, sizeof(*m->rest_turn));
    for (i = 0; i < data->nodes_count; i++) {
        read_extras(data, &data->nodes[i].extras, extras, sizeof(extras));
        m->part_scale[i] = 1.0f;
        if (strstr(extras, "\"scale\":") != NULL) {
            m->part_scale[i] = ldexpf(1.0f, atoi(strstr(extras, "\"scale\":") + 8));
        }
        if (strstr(extras, "\"turn\":[") != NULL) {
            int t[3] = {0, 0, 0};
            sscanf(strstr(extras, "\"turn\":[") + 8, "%d,%d,%d", &t[0], &t[1], &t[2]);
            m->rest_turn[i][0] = (short)t[0], m->rest_turn[i][1] = (short)t[1], m->rest_turn[i][2] = (short)t[2];
        }
        cgltf_node_transform_world(&data->nodes[i], m->node_world[i]);
        cgltf_node_transform_local(&data->nodes[i], m->node_local[i]);
        m->parent[i] = data->nodes[i].parent ? (int)(data->nodes[i].parent - data->nodes) : -1;
    }
    verts = NULL;
    for (i = 0; i < data->nodes_count; i++) {
        const cgltf_mesh *mesh = data->nodes[i].mesh;

        for (k = 0; mesh != NULL && k < mesh->primitives_count; k++) {
            const cgltf_primitive *p = &mesh->primitives[k];
            const cgltf_accessor *pos = NULL, *nor = NULL, *col = NULL, *uv = NULL;
            size_t a, count;
            Primitive *pr;

            for (a = 0; a < p->attributes_count; a++) {
                const cgltf_attribute *at = &p->attributes[a];
                if (at->type == cgltf_attribute_type_position) pos = at->data;
                else if (at->type == cgltf_attribute_type_normal) nor = at->data;
                else if (at->type == cgltf_attribute_type_color) col = at->data;
                else if (at->type == cgltf_attribute_type_texcoord) uv = at->data;
            }
            if (pos == NULL || p->type != cgltf_primitive_type_triangles) {
                continue;
            }
            count = p->indices ? p->indices->count : pos->count;
            if (nverts + (int)count > cap) {
                cap = (nverts + (int)count) * 2 + 1024;
                verts = realloc(verts, sizeof(float) * FLOATS_PER_VERTEX * (size_t)cap);
            }
            for (v = 0; v < count; v++) {
                size_t idx = p->indices ? cgltf_accessor_read_index(p->indices, v) : v;
                float *o = verts + (size_t)(nverts + (int)v) * FLOATS_PER_VERTEX;

                memset(o, 0, sizeof(float) * FLOATS_PER_VERTEX);
                o[6] = o[7] = o[8] = o[9] = 1;
                cgltf_accessor_read_float(pos, idx, o, 3);
                if (nor) cgltf_accessor_read_float(nor, idx, o + 3, 3);
                if (col) cgltf_accessor_read_float(col, idx, o + 6, col->type == cgltf_type_vec4 ? 4 : 3);
                if (uv) cgltf_accessor_read_float(uv, idx, o + 10, 2);
            }
            m->prims = realloc(m->prims, sizeof(Primitive) * (size_t)(m->nprims + 1));
            pr = &m->prims[m->nprims++];
            pr->first = nverts;
            pr->count = (int)count;
            pr->node = (int)(&data->nodes[i] - data->nodes);
            pr->material = p->material ? (int)(p->material - data->materials) : -1;
            nverts += (int)count;
        }
    }
    glCreateBuffers(1, &m->vbo);
    glNamedBufferStorage(m->vbo, (GLsizeiptr)(sizeof(float) * FLOATS_PER_VERTEX * (size_t)(nverts > 0 ? nverts : 1)),
                         verts, 0);
    glCreateVertexArrays(1, &m->vao);
    glVertexArrayVertexBuffer(m->vao, 0, m->vbo, 0, sizeof(float) * FLOATS_PER_VERTEX);
    for (i = 0; i < 4; i++) {
        static const int size[4] = {3, 3, 4, 2}, offset[4] = {0, 3, 6, 10};
        glEnableVertexArrayAttrib(m->vao, (GLuint)i);
        glVertexArrayAttribFormat(m->vao, (GLuint)i, size[i], GL_FLOAT, GL_FALSE, (GLuint)(sizeof(float) * offset[i]));
        glVertexArrayAttribBinding(m->vao, (GLuint)i, 0);
    }
    m->nverts = nverts;
    m->positions = malloc(sizeof(float) * 3 * (size_t)(nverts > 0 ? nverts : 1));
    for (v = 0; v < (size_t)nverts; v++) {
        memcpy(m->positions + v * 3, verts + v * FLOATS_PER_VERTEX, sizeof(float) * 3);
    }
    free(verts);
    free(binary);
    cgltf_free(data);
    free(json);
    return m;
}

void model3d_free(Model3D *m) {
    if (m == NULL) {
        return;
    }
    glDeleteBuffers(1, &m->vbo);
    glDeleteVertexArrays(1, &m->vao);
    free(m->positions);
    free(m->rest_turn);
    free(m->prims);
    free(m->materials);
    free(m->node_world);
    free(m->node_local);
    free(m->parent);
    free(m->part_scale);
    free(m);
}

/* ---- the frame ---- */
void scene3d_texture_area(int area) {
    int a;
    S.area = area;
    for (a = 0; a < SCENE3D_ANIMS; a++) {
        S.anim_shown[a] = 0;
    }
}
void scene3d_texture_frame(int anim, int frame) {
    if (anim >= 0 && anim < SCENE3D_ANIMS) {
        S.anim_shown[anim] = frame < 0 ? 0 : frame + 1;
    }
}
int scene3d_texture_frame_shown(int anim) {
    return anim >= 0 && anim < SCENE3D_ANIMS ? S.anim_shown[anim] - 1 : -1;
}
void scene3d_lamp(int lamp, int lit) {
    if (lamp >= 0 && lamp < SCENE3D_LAMPS) {
        S.lamp_lit[lamp] = (unsigned char)(lit != 0);
    }
}

void scene3d_camera(const float target[3], float yaw, float dist, float eye_y, float look_y, float projection_h) {
    float a = yaw * 2.0f * (float)M_PI / TURN;
    float eye_w[3] = {target[0] + dist * sinf(a), target[1] + eye_y, target[2] + dist * cosf(a)};
    float at_w[3] = {target[0], target[1] + look_y, target[2]}, eye[3], at[3];

    world_point(eye_w, eye);
    world_point(at_w, at);
    look_at(S.view, eye, at);
    memcpy(S.eye, eye, sizeof(eye));
    S.projection_h = projection_h > 0 ? projection_h : 500.0f;
    S.fovy = 2.0f * atanf(SCREEN_HALF_HEIGHT / S.projection_h);
    S.have_camera = 1;
}

/* a game turn (rx, ry, rz: Ry Rx Rz, as the converter: data/modelgltf.c part_rotation) in glTF's space */
static void turn_matrix(const short t[3], float *m) {
    double a[3], cx, sx, cy, sy, cz, sz, r[3][3];
    static const double kFlip[3] = {1, -1, -1};
    int i, j;

    for (i = 0; i < 3; i++) {
        a[i] = t[i] * 2.0 * M_PI / TURN;
    }
    cx = cos(a[0]); sx = sin(a[0]); cy = cos(a[1]); sy = sin(a[1]); cz = cos(a[2]); sz = sin(a[2]);
    r[0][0] = cy * cz + sy * sx * sz; r[0][1] = -cy * sz + sy * sx * cz; r[0][2] = sy * cx;
    r[1][0] = cx * sz;                r[1][1] = cx * cz;                 r[1][2] = -sx;
    r[2][0] = -sy * cz + cy * sx * sz; r[2][1] = sy * sz + cy * sx * cz; r[2][2] = cy * cx;
    mat_identity(m);
    for (i = 0; i < 3; i++) {
        for (j = 0; j < 3; j++) {
            m[j * 4 + i] = (float)(r[i][j] * kFlip[i] * kFlip[j]);
        }
    }
}

static void place_matrix(const float pos[3], float yaw, float *world) {   /* standing at pos, turned by yaw */
    float p[3], a = -yaw * 2.0f * (float)M_PI / TURN;   /* (the game's turn about its y, which points down) */

    world_point(pos, p);
    mat_identity(world);
    world[0] = cosf(a), world[2] = -sinf(a);
    world[8] = sinf(a), world[10] = cosf(a);
    world[12] = p[0], world[13] = p[1], world[14] = p[2];
}

static Draw *new_draw(Model3D *m, const float pos[3], float yaw) {
    Draw *d;

    if (m == NULL || S.ndraws == MAX_DRAWS) {
        return NULL;
    }
    d = &S.draws[S.ndraws++];
    d->m = m;
    d->posed = 0;
    d->nshown = 0;
    place_matrix(pos, yaw, d->world);
    return d;
}

static float (*pool_take(int n, int *first))[16] {   /* n node matrices from this frame's pool */
    if (S.npool + n > S.cappool) {
        while (S.npool + n > S.cappool) {
            S.cappool = S.cappool ? S.cappool * 2 : POOL_AT_FIRST;
        }
        S.pool = realloc(S.pool, sizeof(*S.pool) * (size_t)S.cappool);
    }
    *first = S.npool;
    S.npool += n;
    return S.pool + *first;
}

static void chain(const Model3D *m, float (*node)[16], int i, const float *local) {   /* parent's, then its own */
    if (m->parent[i] >= 0 && m->parent[i] < i) {
        mat_mul(node[i], node[m->parent[i]], local);
    } else {
        memcpy(node[i], local, sizeof(float) * 16);
    }
}

static void pose_nodes(const Model3D *m, const short (*turns)[3], int nturns, const float root_move[3],
                       float (*node)[16]) {
    int i;

    for (i = 0; i < m->nnodes; i++) {   /* (the converter writes parents before their children) */
        float local[16], r[16];

        memcpy(local, m->node_local[i], sizeof(local));
        if (i < nturns) {   /* its turn this frame, in place of its rest turn */
            turn_matrix(turns[i], r);
            memcpy(r + 12, local + 12, sizeof(float) * 3);
            memcpy(local, r, sizeof(local));
        }
        if (m->parent[i] < 0 && root_move != NULL) {   /* the root moves by the frame (in its part's scale, y up) */
            local[12] += root_move[0] * m->part_scale[i] / 256.0f;
            local[13] += root_move[1] * m->part_scale[i] / 256.0f;
            local[14] -= root_move[2] * m->part_scale[i] / 256.0f;
        }
        chain(m, node, i, local);
    }
}

void scene3d_draw_posed(Model3D *m, const float pos[3], float yaw, const short (*turns)[3], int nturns,
                        const float root_move[3]) {
    Draw *d = new_draw(m, pos, yaw);

    if (d == NULL) {
        return;
    }
    d->posed = 1;
    pose_nodes(m, turns, nturns, root_move, pool_take(m->nnodes, &d->node));
}

int scene3d_node_on_screen(Model3D *m, const float pos[3], float yaw, const short (*turns)[3], int nturns,
                           const float root_move[3], int which, int w, int h, float out[2]) {
    float (*node)[16], world[16], at[16], v[4], t;
    int k;

    if (m == NULL || which < 0 || which >= m->nnodes || !S.have_camera || w <= 0 || h <= 0) {
        return 0;
    }
    node = malloc(sizeof(*node) * (size_t)m->nnodes);
    if (node == NULL) {
        return 0;
    }
    pose_nodes(m, turns, nturns, root_move, node);
    place_matrix(pos, yaw, world);
    mat_mul(at, world, node[which]);
    free(node);
    for (k = 0; k < 3; k++) {   /* its origin, in the camera's space */
        v[k] = S.view[k] * at[12] + S.view[4 + k] * at[13] + S.view[8 + k] * at[14] + S.view[12 + k];
    }
    if (v[2] >= 0) {   /* behind the camera */
        return 0;
    }
    t = tanf(S.fovy / 2);
    out[0] = ((v[0] / -v[2]) / (t * (float)w / (float)h) + 1) * 0.5f * (float)w;
    out[1] = (1 - (v[1] / -v[2]) / t) * 0.5f * (float)h;
    return 1;
}

void scene3d_draw_objects(Model3D *m, const float pos[3], float yaw, const unsigned char *shown, int nshown,
                          const Scene3DObjectPose *poses, int nposes) {
    Draw *d = new_draw(m, pos, yaw);
    float (*node)[16];
    int i, k;

    if (d == NULL) {
        return;
    }
    d->nshown = shown != NULL ? (nshown < MAX_SHOWN ? nshown : MAX_SHOWN) : 0;
    if (d->nshown > 0) {
        memcpy(d->shown, shown, (size_t)d->nshown);
    }
    node = pool_take(m->nnodes, &d->node);
    d->posed = 1;
    for (i = 0; i < m->nnodes; i++) {
        float local[16];

        memcpy(local, m->node_local[i], sizeof(local));
        for (k = 0; k < nposes; k++) {
            const Scene3DObjectPose *p = &poses[k];
            short t[3];
            float r[16], a, side, fwd;

            if (p->object != i) {
                continue;
            }
            /* its turn: the rest turn and the frame's; its place: the frame's, along its rest heading (src: func_8001E0F8) */
            t[0] = (short)(m->rest_turn[i][0] + p->turn[0]);
            t[1] = (short)(m->rest_turn[i][1] + p->turn[1]);
            t[2] = (short)(m->rest_turn[i][2] + p->turn[2]);
            turn_matrix(t, r);
            memcpy(r + 12, local + 12, sizeof(float) * 3);
            a = m->rest_turn[i][1] * 2.0f * (float)M_PI / TURN;
            side = p->place[0], fwd = p->place[2];
            r[12] += (fwd * sinf(a) + side * sinf(a + (float)M_PI / 2)) * m->part_scale[i] / 256.0f;
            r[13] -= p->place[1] * m->part_scale[i] / 256.0f;
            r[14] -= (fwd * cosf(a) + side * cosf(a + (float)M_PI / 2)) * m->part_scale[i] / 256.0f;
            memcpy(local, r, sizeof(local));
        }
        chain(m, node, i, local);
    }
}

/* the ground under (x, z) in a model standing at pos, unturned (src: func_80046504: the highest polygon) */
int scene3d_ground(const Model3D *m, const float pos[3], float x, float z, float *y) {
    float base[3], p[3] = {x, 0, z}, g[3], best = 0;
    int k, t, found = 0;

    if (m == NULL || m->positions == NULL) {
        return 0;
    }
    world_point(pos, base);
    world_point(p, g);
    for (k = 0; k < m->nprims; k++) {
        const Primitive *pr = &m->prims[k];
        const float *mw = m->node_world[pr->node];

        for (t = 0; t + 2 < pr->count; t += 3) {
            float v[3][3], d, l0, l1, l2, gy;
            int c, j;

            for (c = 0; c < 3; c++) {
                const float *q = m->positions + (size_t)(pr->first + t + c) * 3;
                for (j = 0; j < 3; j++) {
                    v[c][j] = mw[j] * q[0] + mw[4 + j] * q[1] + mw[8 + j] * q[2] + mw[12 + j] + base[j];
                }
            }
            /* (barycentric across x and z: glTF's y is the height) */
            d = (v[1][2] - v[2][2]) * (v[0][0] - v[2][0]) + (v[2][0] - v[1][0]) * (v[0][2] - v[2][2]);
            if (fabsf(d) < 1e-12f) {
                continue;
            }
            l0 = ((v[1][2] - v[2][2]) * (g[0] - v[2][0]) + (v[2][0] - v[1][0]) * (g[2] - v[2][2])) / d;
            l1 = ((v[2][2] - v[0][2]) * (g[0] - v[2][0]) + (v[0][0] - v[2][0]) * (g[2] - v[2][2])) / d;
            l2 = 1 - l0 - l1;
            if (l0 < 0 || l1 < 0 || l2 < 0) {
                continue;
            }
            gy = l0 * v[0][1] + l1 * v[1][1] + l2 * v[2][1];
            if (!found || gy > best) {
                best = gy;
                found = 1;
            }
        }
    }
    if (found) {
        *y = -best / WORLD_TO_GLTF;
    }
    return found;
}

void scene3d_draw(Model3D *m, const float pos[3], float yaw, const unsigned char *shown, int nshown) {
    Draw *d = new_draw(m, pos, yaw);

    if (d == NULL) {
        return;
    }
    d->nshown = shown != NULL ? (nshown < MAX_SHOWN ? nshown : MAX_SHOWN) : 0;
    if (d->nshown > 0) {
        memcpy(d->shown, shown, (size_t)d->nshown);
    }
}

static void set_blend(int mode) {
    switch (mode) {
    case BLEND_ADD:
        glBlendEquation(GL_FUNC_ADD);
        glBlendFunc(GL_ONE, GL_ONE);
        break;
    case BLEND_SUBTRACT:
        glBlendEquation(GL_FUNC_REVERSE_SUBTRACT);
        glBlendFunc(GL_ONE, GL_ONE);
        break;
    case BLEND_QUARTER:
        glBlendEquation(GL_FUNC_ADD);
        glBlendColor(0, 0, 0, 0.25f);
        glBlendFunc(GL_CONSTANT_ALPHA, GL_ONE);
        break;
    default:   /* half */
        glBlendEquation(GL_FUNC_ADD);
        glBlendColor(0, 0, 0, 0.5f);
        glBlendFunc(GL_CONSTANT_ALPHA, GL_CONSTANT_ALPHA);
        break;
    }
}

/* ---- the light (scene3d.h) ---- */
#define FOG_CUE_A (-0xC80 / 256.0f)          /* src: func_80023D70's GsFOGPARAM: dqa (8.8) */
#define FOG_CUE_B ((float)0x01400000 / (1 << 24))   /* and dqb (8.24) */
#define VERTEX_UNIT 128.0f                  /* a vertex colour that leaves the texture as it is */
#define LIGHT_UNIT 256.0f                   /* a light's colour at full */
#define AMBIENT_UNIT 4096.0f                /* the PlayStation's 1.0 */

void scene3d_light(const Scene3DLight *l) {
    S.light = *l;
    S.light_set = 1;
}

static void light_uniforms(void) {
    float way[9], color[9], amb[3], fog[3];
    int i, k;

    if (!S.light_set) {   /* (no light given: a soft one from above and in front, and ambient) */
        memset(&S.light, 0, sizeof(S.light));
        S.light.way[0][0] = -0.3f, S.light.way[0][1] = 0.8f, S.light.way[0][2] = 0.5f;
        for (k = 0; k < 3; k++) {
            S.light.color[0][k] = 0.45f * LIGHT_UNIT;
            S.light.ambient[k] = 0.55f * AMBIENT_UNIT;
        }
    }
    for (i = 0; i < 3; i++) {
        float w[3] = {S.light.way[i][0], S.light.way[i][1], S.light.way[i][2]}, g[3], len;

        world_point(w, g);   /* (a direction: the axes turned as the points') */
        len = sqrtf(g[0] * g[0] + g[1] * g[1] + g[2] * g[2]);
        for (k = 0; k < 3; k++) {
            way[i * 3 + k] = len > 0 ? g[k] / len : 0;
            color[i * 3 + k] = S.light.color[i][k] / LIGHT_UNIT;
        }
    }
    for (k = 0; k < 3; k++) {
        amb[k] = S.light.ambient[k] / AMBIENT_UNIT;
        fog[k] = S.light.fog_color[k] / VERTEX_UNIT;
    }
    glProgramUniform3fv(S.program, S.loc_light_way, 3, way);
    glProgramUniform3fv(S.program, S.loc_light_color, 3, color);
    glProgramUniform3fv(S.program, S.loc_ambient, 1, amb);
    glProgramUniform1i(S.program, S.loc_fog, S.light.fog);
    glProgramUniform3fv(S.program, S.loc_fog_color, 1, fog);
    glProgramUniform3f(S.program, S.loc_fog_cue, FOG_CUE_A, FOG_CUE_B, S.projection_h);
}

static Quad *quad_new(GpuTexture t, float shade, int blend) {
    Quad *q;

    if (S.quads == NULL) {
        S.quads = malloc(sizeof(Quad) * MAX_QUADS);
    }
    if (S.nquads == MAX_QUADS || t.id == 0) {
        return NULL;
    }
    q = &S.quads[S.nquads++];
    q->t = t;
    q->shade = shade;
    q->blend = blend;
    q->uv[0] = 0, q->uv[1] = 0, q->uv[2] = 1, q->uv[3] = 1;
    return q;
}

void scene3d_quad(GpuTexture t, const float corner[4][3], float shade, int blend) {
    Quad *q = quad_new(t, shade, blend);
    int k;

    if (q == NULL) {
        return;
    }
    for (k = 0; k < 4; k++) {
        world_point(corner[k], q->corner[k]);
    }
}

void scene3d_sprite(GpuTexture t, const float pos[3], float pixels, const float uv[4], float shade, int blend) {
    float p[3], depth, half;
    const float *v = S.view;
    float right[3] = {v[0], v[4], v[8]}, up[3] = {v[1], v[5], v[9]};
    Quad *q;
    int k, j;

    if (!S.have_camera) {
        return;
    }
    world_point(pos, p);
    depth = -(v[2] * p[0] + v[6] * p[1] + v[10] * p[2] + v[14]);
    if (depth <= 0 || (q = quad_new(t, shade, blend)) == NULL) {
        return;
    }
    half = pixels * 0.5f * depth / S.projection_h;   /* (the original's pixels: its screen 240 high seen from h) */
    for (k = 0; k < 4; k++) {
        float sx = (k & 1) ? half : -half, sy = (k & 2) ? -half : half;
        for (j = 0; j < 3; j++) {
            q->corner[k][j] = p[j] + right[j] * sx + up[j] * sy;
        }
    }
    memcpy(q->uv, uv, sizeof(q->uv));
}

/* the quads' vertices this frame, two triangles each (0 1 2, 1 3 2), into their buffer */
static void quads_upload(void) {
    static const int kOrder[6] = {0, 1, 2, 1, 3, 2};
    float *v = malloc(sizeof(float) * FLOATS_PER_VERTEX * 6 * (size_t)S.nquads), *p = v;
    int i, k;

    if (S.quad_vao == 0) {
        static const int size[4] = {3, 3, 4, 2}, offset[4] = {0, 3, 6, 10};
        glCreateBuffers(1, &S.quad_vbo);
        glNamedBufferStorage(S.quad_vbo, (GLsizeiptr)(sizeof(float) * FLOATS_PER_VERTEX * 6 * MAX_QUADS), NULL,
                             GL_DYNAMIC_STORAGE_BIT);
        glCreateVertexArrays(1, &S.quad_vao);
        glVertexArrayVertexBuffer(S.quad_vao, 0, S.quad_vbo, 0, sizeof(float) * FLOATS_PER_VERTEX);
        for (i = 0; i < 4; i++) {
            glEnableVertexArrayAttrib(S.quad_vao, (GLuint)i);
            glVertexArrayAttribFormat(S.quad_vao, (GLuint)i, size[i], GL_FLOAT, GL_FALSE, (GLuint)(sizeof(float) * offset[i]));
            glVertexArrayAttribBinding(S.quad_vao, (GLuint)i, 0);
        }
    }
    for (i = 0; i < S.nquads; i++) {
        for (k = 0; k < 6; k++) {
            const float *c = S.quads[i].corner[kOrder[k]];
            float sh = S.quads[i].shade;
            const float *uv = S.quads[i].uv;
            float vert[FLOATS_PER_VERTEX] = {c[0], c[1], c[2], 0, 1, 0, sh, sh, sh, 1,
                                             uv[(kOrder[k] & 1) ? 2 : 0], uv[(kOrder[k] & 2) ? 3 : 1]};
            memcpy(p, vert, sizeof(vert));
            p += FLOATS_PER_VERTEX;
        }
    }
    glNamedBufferSubData(S.quad_vbo, 0, (GLsizeiptr)((char *)p - (char *)v), v);
    free(v);
}

static void quads_draw(int pass, const float *vp) {
    static const float kIdentity[16] = {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1};
    int i;

    glBindVertexArray(S.quad_vao);
    glProgramUniformMatrix4fv(S.program, S.loc_mvp, 1, GL_FALSE, vp);
    glProgramUniformMatrix4fv(S.program, S.loc_model, 1, GL_FALSE, kIdentity);
    glProgramUniformMatrix4fv(S.program, S.loc_modelview, 1, GL_FALSE, S.view);
    glProgramUniform1i(S.program, S.loc_lit, 0);
    glProgramUniform1i(S.program, S.loc_textured, 1);
    glDisable(GL_CULL_FACE);
    for (i = 0; i < S.nquads; i++) {
        glProgramUniform1i(S.program, S.loc_semi, S.quads[i].blend != BLEND_NONE);
        if (pass == 1) {
            if (S.quads[i].blend == BLEND_NONE) {
                continue;
            }
            set_blend(S.quads[i].blend);
        }
        glBindTextureUnit(0, S.quads[i].t.id);
        glDrawArrays(GL_TRIANGLES, i * 6, 6);
    }
}

void scene3d_flush(int w, int h) {
    float proj[16], vp[16];
    int pass, i, k;

    if ((S.ndraws == 0 && S.nquads == 0) || !S.have_camera) {
        S.ndraws = 0;
        S.nquads = 0;
        S.npool = 0;
        return;
    }
    if (S.program == 0) {
        S.program = render_program(kVS, kFS);
        S.loc_mvp = glGetUniformLocation(S.program, "mvp");
        S.loc_model = glGetUniformLocation(S.program, "model");
        S.loc_lit = glGetUniformLocation(S.program, "lit");
        S.loc_textured = glGetUniformLocation(S.program, "textured");
        S.loc_pass = glGetUniformLocation(S.program, "pass");
        S.loc_semi = glGetUniformLocation(S.program, "semi");
        S.loc_modelview = glGetUniformLocation(S.program, "modelview");
        S.loc_light_way = glGetUniformLocation(S.program, "light_way");
        S.loc_light_color = glGetUniformLocation(S.program, "light_color");
        S.loc_ambient = glGetUniformLocation(S.program, "ambient");
        S.loc_fog = glGetUniformLocation(S.program, "fog");
        S.loc_fog_color = glGetUniformLocation(S.program, "fog_color");
        S.loc_fog_cue = glGetUniformLocation(S.program, "fog_cue");
    }
    light_uniforms();
    perspective(proj, S.fovy, (float)w / (float)(h > 0 ? h : 1));
    mat_mul(vp, proj, S.view);
    glUseProgram(S.program);
    glEnable(GL_DEPTH_TEST);
    glDepthFunc(GL_LEQUAL);
    if (S.nquads > 0) {
        quads_upload();
    }
    for (pass = 0; pass < 2; pass++) {   /* the solid texels, then the blending ones over them */
        glProgramUniform1i(S.program, S.loc_pass, pass);
        glDepthMask(pass == 0 ? GL_TRUE : GL_FALSE);
        if (pass == 0) {
            glDisable(GL_BLEND);
        } else {
            glEnable(GL_BLEND);
        }
        for (i = 0; i < S.ndraws; i++) {
            Model3D *m = S.draws[i].m;

            glBindVertexArray(m->vao);
            for (k = 0; k < m->nprims; k++) {
                const Primitive *p = &m->prims[k];
                const Material *mt = p->material >= 0 ? &m->materials[p->material] : NULL;
                float model[16], mvp[16], modelview[16];
                int has_tex = 0;
                GpuTexture t = mt != NULL ? material_texture(mt, &has_tex) : (GpuTexture){0, 0, 0};

                if ((pass == 1 && (mt == NULL || mt->blend == BLEND_NONE)) ||
                    (p->node < S.draws[i].nshown && !S.draws[i].shown[p->node])) {
                    continue;
                }
                mat_mul(model, S.draws[i].world, S.draws[i].posed ? S.pool[S.draws[i].node + p->node] : m->node_world[p->node]);
                mat_mul(mvp, vp, model);
                glProgramUniformMatrix4fv(S.program, S.loc_mvp, 1, GL_FALSE, mvp);
                glProgramUniformMatrix4fv(S.program, S.loc_model, 1, GL_FALSE, model);
                mat_mul(modelview, S.view, model);
                glProgramUniformMatrix4fv(S.program, S.loc_modelview, 1, GL_FALSE, modelview);
                glProgramUniform1i(S.program, S.loc_lit, mt == NULL || mt->lit);
                glProgramUniform1i(S.program, S.loc_textured, has_tex);
                glProgramUniform1i(S.program, S.loc_semi, mt != NULL && mt->blend != BLEND_NONE);
                if (has_tex) {
                    glBindTextureUnit(0, t.id);
                }
                if (pass == 1) {
                    set_blend(mt->blend);
                }
                if (mt != NULL && mt->double_sided) {
                    glDisable(GL_CULL_FACE);
                } else {
                    glEnable(GL_CULL_FACE);
                    glCullFace(GL_BACK);
                }
                glDrawArrays(GL_TRIANGLES, p->first, p->count);
            }
        }
        if (S.nquads > 0) {
            quads_draw(pass, vp);
        }
    }
    glDisable(GL_CULL_FACE);
    glDisable(GL_DEPTH_TEST);
    glDepthMask(GL_TRUE);
    glBlendEquation(GL_FUNC_ADD);
    S.ndraws = 0;
    S.nquads = 0;
    S.npool = 0;
}
