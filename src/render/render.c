/* The renderer (render.h). The 2D layer: every rectangle, image and glyph of a frame becomes a quad in one
 * vertex list; a run of quads with the same texture is one draw. The font is a texture made at start-up from
 * font.h (glyph i at column i, 6 x 8 cells), and plain rectangles use a 1 x 1 white texture, so every quad
 * goes through the same shader. */
#include "render.h"

#include "../platform/gl.h"
#include "font.h"
#include "stb_image_write.h"

#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define GLYPH_W 6   /* a 5-pixel glyph and a column of space */
#define GLYPH_H 8   /* 7 rows and one of space */
#define MAX_QUADS 65536

typedef struct Vertex2D {
    float x, y, u, v;
    uint32_t rgba;   /* 0xRRGGBBAA, read as 4 normalized bytes (so stored as ABGR in memory) */
} Vertex2D;

typedef struct Batch {
    uint32_t texture;
    int first, count;   /* vertices */
} Batch;

static struct {
    uint32_t program, vao, vbo;
    int loc_screen;
    GpuTexture white, font;
    Vertex2D *verts;
    int nverts;
    Batch batches[4096];
    int nbatches;
    int w, h;
    uint8_t *last_frame;   /* for render_screenshot: read back at render_end when asked */
    int want_shot;
} R;

static const char *kVertex2D =
    "#version 460 core\n"
    "layout(location = 0) in vec2 pos;\n"
    "layout(location = 1) in vec2 uv;\n"
    "layout(location = 2) in vec4 color;\n"
    "layout(location = 0) uniform vec2 screen;   /* the window size in pixels */\n"
    "out vec2 v_uv;\n"
    "out vec4 v_color;\n"
    "void main() {\n"
    "    gl_Position = vec4(pos.x / screen.x * 2.0 - 1.0, 1.0 - pos.y / screen.y * 2.0, 0.0, 1.0);\n"
    "    v_uv = uv;\n"
    "    v_color = color;\n"
    "}\n";

static const char *kFragment2D =
    "#version 460 core\n"
    "in vec2 v_uv;\n"
    "in vec4 v_color;\n"
    "layout(binding = 0) uniform sampler2D image;\n"
    "out vec4 color;\n"
    "void main() {\n"
    "    color = texture(image, v_uv) * v_color;\n"
    "}\n";

static uint32_t compile(GLenum kind, const char *src) {
    GLuint s = glCreateShader(kind);
    GLint ok = 0;

    glShaderSource(s, 1, &src, NULL);
    glCompileShader(s);
    glGetShaderiv(s, GL_COMPILE_STATUS, &ok);
    if (!ok) {
        char log[1024];

        glGetShaderInfoLog(s, sizeof(log), NULL, log);
        fprintf(stderr, "render: a shader didn't compile:\n%s\n", log);
    }
    return s;
}

static uint32_t link(const char *vs, const char *fs) {
    GLuint p = glCreateProgram(), v = compile(GL_VERTEX_SHADER, vs), f = compile(GL_FRAGMENT_SHADER, fs);
    GLint ok = 0;

    glAttachShader(p, v);
    glAttachShader(p, f);
    glLinkProgram(p);
    glGetProgramiv(p, GL_LINK_STATUS, &ok);
    if (!ok) {
        char log[1024];

        glGetProgramInfoLog(p, sizeof(log), NULL, log);
        fprintf(stderr, "render: a program didn't link:\n%s\n", log);
    }
    glDetachShader(p, v);
    glDetachShader(p, f);
    glDeleteShader(v);
    glDeleteShader(f);
    return p;
}

GpuTexture render_texture_rgba(const uint8_t *rgba, int w, int h) {
    GpuTexture t = {0, w, h};

    glCreateTextures(GL_TEXTURE_2D, 1, &t.id);
    glTextureStorage2D(t.id, 1, GL_RGBA8, w, h);
    glTextureSubImage2D(t.id, 0, 0, 0, w, h, GL_RGBA, GL_UNSIGNED_BYTE, rgba);
    glTextureParameteri(t.id, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
    glTextureParameteri(t.id, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
    glTextureParameteri(t.id, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
    glTextureParameteri(t.id, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
    return t;
}

void render_texture_update(GpuTexture t, const uint8_t *rgba) {
    glTextureSubImage2D(t.id, 0, 0, 0, t.w, t.h, GL_RGBA, GL_UNSIGNED_BYTE, rgba);
}

void render_texture_free(GpuTexture t) {
    if (t.id != 0) {
        glDeleteTextures(1, &t.id);
    }
}

/* the font as a texture: FONT_COUNT glyphs side by side, white where a pixel is set */
static GpuTexture make_font(void) {
    int w = FONT_COUNT * GLYPH_W, h = GLYPH_H, g, x, y;
    uint8_t *px = calloc((size_t)w * h, 4);
    GpuTexture t;

    for (g = 0; g < FONT_COUNT; g++) {
        for (x = 0; x < 5; x++) {
            for (y = 0; y < 7; y++) {
                if (kFont5x7[g][x] >> y & 1) {
                    memset(px + ((size_t)y * w + g * GLYPH_W + x) * 4, 0xFF, 4);
                }
            }
        }
    }
    t = render_texture_rgba(px, w, h);
    free(px);
    return t;
}

int render_init(void) {
    static const uint8_t kWhite[4] = {0xFF, 0xFF, 0xFF, 0xFF};

    R.program = link(kVertex2D, kFragment2D);
    R.loc_screen = 0;   /* (its layout location) */
    R.verts = malloc(sizeof(Vertex2D) * 6 * MAX_QUADS);
    glCreateBuffers(1, &R.vbo);
    glNamedBufferData(R.vbo, sizeof(Vertex2D) * 6 * MAX_QUADS, NULL, GL_STREAM_DRAW);
    glCreateVertexArrays(1, &R.vao);
    glVertexArrayVertexBuffer(R.vao, 0, R.vbo, 0, sizeof(Vertex2D));
    glEnableVertexArrayAttrib(R.vao, 0);
    glEnableVertexArrayAttrib(R.vao, 1);
    glEnableVertexArrayAttrib(R.vao, 2);
    glVertexArrayAttribFormat(R.vao, 0, 2, GL_FLOAT, GL_FALSE, offsetof(Vertex2D, x));
    glVertexArrayAttribFormat(R.vao, 1, 2, GL_FLOAT, GL_FALSE, offsetof(Vertex2D, u));
    glVertexArrayAttribFormat(R.vao, 2, 4, GL_UNSIGNED_BYTE, GL_TRUE, offsetof(Vertex2D, rgba));
    glVertexArrayAttribBinding(R.vao, 0, 0);
    glVertexArrayAttribBinding(R.vao, 1, 0);
    glVertexArrayAttribBinding(R.vao, 2, 0);
    R.white = render_texture_rgba(kWhite, 1, 1);
    R.font = make_font();
    return 1;
}

void render_shutdown(void) {
    render_texture_free(R.white);
    render_texture_free(R.font);
    free(R.verts);
    free(R.last_frame);
    memset(&R, 0, sizeof(R));
}

void render_begin(int window_w, int window_h, float r, float g, float b) {
    const float clear[4] = {r, g, b, 1.0f};

    R.w = window_w;
    R.h = window_h;
    R.nverts = 0;
    R.nbatches = 0;
    glViewport(0, 0, window_w, window_h);
    glClearNamedFramebufferfv(0, GL_COLOR, 0, clear);
}

static uint32_t swap_rgba(uint32_t rgba) {   /* 0xRRGGBBAA to the bytes R G B A in memory */
    return (rgba >> 24) | (rgba >> 8 & 0xFF00) | (rgba << 8 & 0xFF0000) | (rgba << 24);
}

static void quad(uint32_t texture, float x, float y, float w, float h, float u0, float v0, float u1, float v1,
                 uint32_t rgba) {
    Vertex2D *v;
    uint32_t c = swap_rgba(rgba);

    if (R.nverts + 6 > 6 * MAX_QUADS) {
        return;
    }
    if (R.nbatches == 0 || R.batches[R.nbatches - 1].texture != texture) {
        if (R.nbatches == (int)(sizeof(R.batches) / sizeof(R.batches[0]))) {
            return;
        }
        R.batches[R.nbatches++] = (Batch){texture, R.nverts, 0};
    }
    v = R.verts + R.nverts;
    v[0] = (Vertex2D){x, y, u0, v0, c};
    v[1] = (Vertex2D){x + w, y, u1, v0, c};
    v[2] = (Vertex2D){x, y + h, u0, v1, c};
    v[3] = v[1];
    v[4] = (Vertex2D){x + w, y + h, u1, v1, c};
    v[5] = v[2];
    R.nverts += 6;
    R.batches[R.nbatches - 1].count += 6;
}

void render_rect(float x, float y, float w, float h, uint32_t rgba) {
    quad(R.white.id, x, y, w, h, 0, 0, 1, 1, rgba);
}

void render_image(GpuTexture t, float x, float y, float w, float h, uint32_t rgba) {
    quad(t.id, x, y, w, h, 0, 0, 1, 1, rgba);
}

void render_image_uv(GpuTexture t, float x, float y, float w, float h, float u0, float v0, float u1, float v1,
                     uint32_t rgba) {
    quad(t.id, x, y, w, h, u0, v0, u1, v1, rgba);
}

void render_text(float x, float y, float scale, uint32_t rgba, const char *s, int n) {
    float fw = (float)(FONT_COUNT * GLYPH_W), left = x;
    int i;

    for (i = 0; i < n; i++) {
        int c = (unsigned char)s[i];

        if (c == '\n') {
            x = left;
            y += render_line_height(scale);
            continue;
        }
        if (c < FONT_FIRST || c >= FONT_FIRST + FONT_COUNT) {
            c = '?';
        }
        c -= FONT_FIRST;
        quad(R.font.id, x, y, GLYPH_W * scale, GLYPH_H * scale, c * GLYPH_W / fw, 0, (c + 1) * GLYPH_W / fw, 1,
             rgba);
        x += GLYPH_W * scale;
    }
}

float render_text_width(float scale, int chars) { return (float)chars * GLYPH_W * scale; }
float render_line_height(float scale) { return GLYPH_H * scale + scale; }

void render_end(void) {
    int i;

    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
    glDisable(GL_DEPTH_TEST);
    glUseProgram(R.program);
    glProgramUniform2f(R.program, R.loc_screen, (float)R.w, (float)R.h);
    glNamedBufferSubData(R.vbo, 0, (GLsizeiptr)(sizeof(Vertex2D) * (size_t)R.nverts), R.verts);
    glBindVertexArray(R.vao);
    for (i = 0; i < R.nbatches; i++) {
        glBindTextureUnit(0, R.batches[i].texture);
        glDrawArrays(GL_TRIANGLES, R.batches[i].first, R.batches[i].count);
    }
    if (R.want_shot) {   /* (read back now: after the swap the back buffer is undefined) */
        free(R.last_frame);
        R.last_frame = malloc((size_t)R.w * R.h * 4);
        glReadnPixels(0, 0, R.w, R.h, GL_RGBA, GL_UNSIGNED_BYTE, R.w * R.h * 4, R.last_frame);
        R.want_shot = 0;
    }
}

static void write_to_file(void *ctx, void *data, int n) {
    fwrite(data, 1, (size_t)n, ctx);
}

/* the picture is read back at the next render_end: call it, run a frame, then call it again to write */
int render_screenshot(const char *path) {
    uint8_t *flipped;
    int y, ok;

    if (R.last_frame == NULL) {
        R.want_shot = 1;
        return 0;
    }
    flipped = malloc((size_t)R.w * R.h * 4);
    for (y = 0; y < R.h; y++) {   /* (GL's rows go up from the bottom) */
        memcpy(flipped + (size_t)y * R.w * 4, R.last_frame + (size_t)(R.h - 1 - y) * R.w * 4, (size_t)R.w * 4);
    }
    for (y = 0; y < R.w * R.h; y++) {   /* (opaque, as the screen shows it: blending leaves alpha below 1) */
        flipped[y * 4 + 3] = 255;
    }
    {
        FILE *out = fopen(path, "wb");

        ok = out != NULL && stbi_write_png_to_func(write_to_file, out, R.w, R.h, 4, flipped, R.w * 4) != 0;
        if (out != NULL) {
            ok = fclose(out) == 0 && ok;
        }
    }
    free(flipped);
    free(R.last_frame);
    R.last_frame = NULL;
    return ok;
}
