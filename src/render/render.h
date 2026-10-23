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
void render_text(float x, float y, float scale, uint32_t rgba, const char *s, int n);
float render_text_width(float scale, int chars);
float render_line_height(float scale);

/* the last drawn frame as a PNG; 0 on failure */
int render_screenshot(const char *path);

#endif
