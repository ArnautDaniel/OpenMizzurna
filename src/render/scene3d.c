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
#define MAX_TEXTURES 1024
#define FLOATS_PER_VERTEX 12            /* position 3, normal 3, colour 4, uv 2 */

enum { BLEND_NONE, BLEND_HALF, BLEND_ADD, BLEND_SUBTRACT, BLEND_QUARTER };

typedef struct Material {
    char texture[96];   /* its name ("page9-4bit-palette288-247"), or "" for none */
    int area_set;       /* 1: the area's set; 0: the model's own (uri) */
    char uri[160];      /* the model's own: its library path */
    int blend, lit, double_sided;
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
    int nnodes;
};

#define MAX_SHOWN 256
#define MAX_POSED 64
typedef struct Draw {
    Model3D *m;
    float world[16];
    int posed;                          /* a figure in a motion's frame: its nodes' transforms below */
    float node[MAX_POSED][16];
    unsigned char shown[MAX_SHOWN];   /* a byte a node (0: hidden) */
    int nshown;                       /* (nodes past it are shown) */
} Draw;

typedef struct Tex {
    char path[200];
    GpuTexture t;
    int missing;
} Tex;

static struct {
    uint32_t program;
    int loc_mvp, loc_model, loc_lit, loc_textured, loc_pass, loc_semi;
    Draw draws[MAX_DRAWS];
    int ndraws;
    Tex tex[MAX_TEXTURES];
    int ntex;
    int area;
    float view[16], eye[3];
    float fovy;
    int have_camera;
} S;

static const char *kVS =
    "#version 460 core\n"
    "layout(location = 0) in vec3 a_pos;\n"
    "layout(location = 1) in vec3 a_normal;\n"
    "layout(location = 2) in vec4 a_color;\n"
    "layout(location = 3) in vec2 a_uv;\n"
    "uniform mat4 mvp;\n"
    "uniform mat4 model;\n"
    "uniform int lit;\n"
    "out vec4 v_color;\n"
    "out vec2 v_uv;\n"
    "void main() {\n"
    "    vec3 n = normalize(mat3(model) * a_normal);\n"
    /* (until the areas' lights: a soft light from above and in front, and ambient) */
    "    float light = lit != 0 ? 0.55 + 0.45 * max(dot(n, normalize(vec3(0.3, 0.8, 0.5))), 0.0) : 1.0;\n"
    "    v_color = vec4(a_color.rgb * light, a_color.a);\n"
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
static GpuTexture texture_at(const char *path, int *ok) {
    size_t n;
    uint8_t *file, *rgba;
    int i, w = 0, h = 0;

    for (i = 0; i < S.ntex; i++) {
        if (strcmp(S.tex[i].path, path) == 0) {
            *ok = !S.tex[i].missing;
            return S.tex[i].t;
        }
    }
    if (S.ntex == MAX_TEXTURES) {
        *ok = 0;
        return S.tex[0].t;
    }
    file = files_read(path, &n);
    rgba = file != NULL ? png_read_rgba(file, n, &w, &h) : NULL;
    free(file);
    snprintf(S.tex[S.ntex].path, sizeof(S.tex[S.ntex].path), "%s", path);
    S.tex[S.ntex].missing = rgba == NULL;
    if (rgba != NULL) {
        S.tex[S.ntex].t = render_texture_rgba(rgba, w, h);
        free(rgba);
    }
    *ok = !S.tex[S.ntex].missing;
    return S.tex[S.ntex++].t;
}

static GpuTexture material_texture(const Material *mt, int *ok) {
    char path[200];
    GpuTexture t;

    *ok = 0;
    if (!mt->texture[0]) {
        return t = (GpuTexture){0, 0, 0};
    }
    if (mt->area_set) {
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
    for (i = 0; i < data->nodes_count; i++) {
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
    free(m->prims);
    free(m->materials);
    free(m->node_world);
    free(m->node_local);
    free(m->parent);
    free(m);
}

/* ---- the frame ---- */
void scene3d_texture_area(int area) { S.area = area; }

void scene3d_camera(const float target[3], float yaw, float dist, float eye_y, float look_y, float projection_h) {
    float a = yaw * 2.0f * (float)M_PI / TURN;
    float eye_w[3] = {target[0] + dist * sinf(a), target[1] + eye_y, target[2] + dist * cosf(a)};
    float at_w[3] = {target[0], target[1] + look_y, target[2]}, eye[3], at[3];

    world_point(eye_w, eye);
    world_point(at_w, at);
    look_at(S.view, eye, at);
    memcpy(S.eye, eye, sizeof(eye));
    S.fovy = 2.0f * atanf(SCREEN_HALF_HEIGHT / (projection_h > 0 ? projection_h : 500.0f));
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

static Draw *new_draw(Model3D *m, const float pos[3], float yaw) {
    Draw *d;
    float p[3], a = -yaw * 2.0f * (float)M_PI / TURN;   /* (the game's turn about its y, which points down) */

    if (m == NULL || S.ndraws == MAX_DRAWS) {
        return NULL;
    }
    d = &S.draws[S.ndraws++];
    d->m = m;
    d->posed = 0;
    d->nshown = 0;
    world_point(pos, p);
    mat_identity(d->world);
    d->world[0] = cosf(a), d->world[2] = -sinf(a);
    d->world[8] = sinf(a), d->world[10] = cosf(a);
    d->world[12] = p[0], d->world[13] = p[1], d->world[14] = p[2];
    return d;
}

void scene3d_draw_posed(Model3D *m, const float pos[3], float yaw, const short (*turns)[3], int nturns,
                        const float root_move[3]) {
    Draw *d = new_draw(m, pos, yaw);
    int i;

    if (d == NULL || m->nnodes > MAX_POSED) {
        return;
    }
    d->posed = 1;
    for (i = 0; i < m->nnodes; i++) {   /* (the converter writes parents before their children) */
        float local[16], r[16];

        memcpy(local, m->node_local[i], sizeof(local));
        if (i < nturns) {   /* its turn this frame, in place of its rest turn */
            turn_matrix(turns[i], r);
            memcpy(r + 12, local + 12, sizeof(float) * 3);
            memcpy(local, r, sizeof(local));
        }
        if (m->parent[i] < 0 && root_move != NULL) {   /* the root moves by the frame (model units, y up) */
            local[12] += root_move[0] / 256.0f;
            local[13] += root_move[1] / 256.0f;
            local[14] -= root_move[2] / 256.0f;
        }
        if (m->parent[i] >= 0 && m->parent[i] < i) {
            mat_mul(d->node[i], d->node[m->parent[i]], local);
        } else {
            memcpy(d->node[i], local, sizeof(local));
        }
    }
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

void scene3d_flush(int w, int h) {
    float proj[16], vp[16];
    int pass, i, k;

    if (S.ndraws == 0 || !S.have_camera) {
        S.ndraws = 0;
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
    }
    perspective(proj, S.fovy, (float)w / (float)(h > 0 ? h : 1));
    mat_mul(vp, proj, S.view);
    glUseProgram(S.program);
    glEnable(GL_DEPTH_TEST);
    glDepthFunc(GL_LEQUAL);
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
                float model[16], mvp[16];
                int has_tex = 0;
                GpuTexture t = mt != NULL ? material_texture(mt, &has_tex) : (GpuTexture){0, 0, 0};

                if ((pass == 1 && (mt == NULL || mt->blend == BLEND_NONE)) ||
                    (p->node < S.draws[i].nshown && !S.draws[i].shown[p->node])) {
                    continue;
                }
                mat_mul(model, S.draws[i].world, S.draws[i].posed ? S.draws[i].node[p->node] : m->node_world[p->node]);
                mat_mul(mvp, vp, model);
                glProgramUniformMatrix4fv(S.program, S.loc_mvp, 1, GL_FALSE, mvp);
                glProgramUniformMatrix4fv(S.program, S.loc_model, 1, GL_FALSE, model);
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
    }
    glDisable(GL_CULL_FACE);
    glDisable(GL_DEPTH_TEST);
    glDepthMask(GL_TRUE);
    glBlendEquation(GL_FUNC_ADD);
    S.ndraws = 0;
}
