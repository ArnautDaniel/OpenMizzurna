/* The renderer: OpenGL 4.6 core (direct state access), drawing into the window's own framebuffer.
 *
 * This is OpenMizzurna's own renderer, not the PlayStation's GPU: the game's pictures (images, models,
 * the town) arrive here as modern textures and meshes once their subsystems are built. The first layer
 * is 2D, in window pixels from the top left: rectangles, images and text (a 5 x 7 pixel font), batched
 * and drawn in the order given at render_end. */
#ifndef RENDER_H
#define RENDER_H

#include <stdint.h>

typedef struct GpuTexture {
    uint32_t id;
    int w, h;
} GpuTexture;

int render_init(void);
void render_shutdown(void);

/* a frame: begun with the window's size and a clear colour, ended (drawn) before the swap */
void render_begin(int window_w, int window_h, float r, float g, float b);
void render_end(void);

/* textures from RGBA pixels (row 0 at the top) */
GpuTexture render_texture_rgba(const uint8_t *rgba, int w, int h);
void render_texture_update(GpuTexture t, const uint8_t *rgba);
void render_texture_free(GpuTexture t);

/* 2D, in window pixels; rgba 0xRRGGBBAA tints (0xFFFFFFFF: as it is) */
void render_rect(float x, float y, float w, float h, uint32_t rgba);
void render_image(GpuTexture t, float x, float y, float w, float h, uint32_t rgba);
void render_image_uv(GpuTexture t, float x, float y, float w, float h, float u0, float v0, float u1, float v1,
                     uint32_t rgba);
/* how the 2D drawn from now on meets the picture (until render_end): RENDER_BLEND_ALPHA as usual, or the
 * PlayStation's add and subtract (the screen's effects: fades to white and to black) */
enum { RENDER_BLEND_ALPHA, RENDER_BLEND_ADD, RENDER_BLEND_SUBTRACT, RENDER_BLEND_PS_ADD, RENDER_BLEND_PS_HALF,
       RENDER_BLEND_PS_SOLID };
/* (the PlayStation's semi-transparent images: a texel marked to blend - alpha 128 - is added to the picture
 * (PS_ADD) or averaged with it (PS_HALF); one marked solid - 255 - covers it; 0 isn't drawn. Drawn opaque
 * (PS_SOLID), every texel but 0 covers it, as the PlayStation's opaque sprites) */

/* the 2D drawn from now on goes behind the 3D (the sky) or over it, as usual (until render_end) */
void render_behind(int behind);
void render_blend(int mode);
/* the 2D drawn from now on only within this window rectangle (w 0: everywhere), until render_end */
void render_clip(int x, int y, int w, int h);
/* a four-cornered shape, a colour a corner (corners as the PlayStation's G4: 0 1 / 2 3) */
void render_quad4(const float xy[8], const uint32_t rgba[4]);
/* a picture on four corners (0 1 / 2 3: its top left, top right, bottom left, bottom right), tinted */
void render_image_quad(GpuTexture t, const float xy[8], uint32_t rgba);
/* the same with a part of it (uv 0..1) */
void render_image_quad_uv(GpuTexture t, const float xy[8], float u0, float v0, float u1, float v1, uint32_t rgba);
void render_text(float x, float y, float scale, uint32_t rgba, const char *s, int n);
float render_text_width(float scale, int chars);
float render_line_height(float scale);

/* a shader program from GLSL sources (errors printed) */
uint32_t render_program(const char *vs, const char *fs);

/* the last drawn frame as a PNG; 0 on failure */
int render_screenshot(const char *path);

#endif
