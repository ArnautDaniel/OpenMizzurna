/* TIM images, the PlayStation's picture format, and their form in the library.
 *
 * A TIM is pixels (4 or 8 bits a pixel, indices into a palette; or 16 / 24 bits, colours) and, for the indexed
 * ones, a block of palettes (the CLUT: rows of colours); each block names where in video memory the game put it,
 * which is how models and sprites refer to a texture and its palette. Colours are 15-bit (5 bits each of red,
 * green, blue) and a flag (bit 15) marking texels that blend when drawn semi-transparent; the value 0 is
 * transparent.
 *
 * In the library a TIM `N` becomes:
 *   N.png       the image: an indexed PNG (4 / 8 bits) showing the first palette, or RGBA (16 / 24 bits)
 *   N.clut.png  the palette block, a colour a pixel (indexed images)
 *   N.tim.txt   what PNG can't say: the depth and where each block lived in video memory
 * Colours in RGBA: 5 bits to 8 (v << 3 | v >> 2), alpha 0 transparent (the value 0), 128 a blending texel
 * (bit 15), 255 otherwise - the exact 15-bit values come back from them. */
#ifndef TIM_H
#define TIM_H

#include <stddef.h>
#include <stdint.h>

#define TIM_ALPHA_CLEAR 0      /* the colour value 0: not drawn */
#define TIM_ALPHA_BLEND 128    /* bit 15 set: blends when the primitive is semi-transparent */
#define TIM_ALPHA_SOLID 255

typedef struct Tim {
    int depth;                       /* bits a pixel: 4, 8, 16 or 24 */
    int has_clut;
    uint16_t clut_x, clut_y, clut_w, clut_h;   /* the palette block in video memory (colours, rows) */
    const uint8_t *clut;             /* clut_w * clut_h 15-bit colours */
    uint16_t pix_x, pix_y, pix_w, pix_h;       /* the image in video memory (pix_w in 16-bit units) */
    const uint8_t *pixels;
    int width, height;               /* in pixels */
    size_t size;                     /* the TIM's bytes */
} Tim;

/* a TIM (pointing into d); 0 if it isn't one */
int tim_parse(Tim *t, const uint8_t *d, size_t n);

/* a 15-bit colour as RGBA, and back */
void tim_color_rgba(uint16_t c, uint8_t *rgba);
uint16_t tim_rgba_color(const uint8_t *rgba);

/* where converted files go and come from: library paths (`name` + ".png" ...) */
typedef int (*TimWriteFn)(void *ctx, const char *path, const uint8_t *data, size_t n);
typedef uint8_t *(*TimReadFn)(void *ctx, const char *path, size_t *n);

/* the library form of a TIM, written as `name`.png ...; 0 on failure */
int tim_convert(const Tim *t, const char *name, TimWriteFn write, void *ctx);
/* the TIM's bytes made again from its library form (malloc'd); NULL if a file is missing or wrong */
uint8_t *tim_rebuild(const char *name, TimReadFn read, void *ctx, size_t *size);

#endif
