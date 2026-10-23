/* What the PlayStation's video memory held while a model was drawn - the images a context loads, where the game
 * put them - and textures cut from it. Only the import uses this (it turns the game's texture pages and palettes
 * into plain textures); the engine never sees video memory.
 *
 * A texture page is 256 x 256 texels of 4 bits (indices into a 16-colour palette), 8 bits (256 colours) or 16 bits
 * (colours); its word says which page of video memory and which depth, a palette word where the palette is. */
#ifndef VRAMCTX_H
#define VRAMCTX_H

#include <stddef.h>
#include <stdint.h>

#define VRAM_WIDTH 1024    /* 16-bit units */
#define VRAM_HEIGHT 512
#define PAGE_TEXELS 256

typedef struct VramContext {
    uint16_t vram[VRAM_HEIGHT][VRAM_WIDTH];
} VramContext;

void vram_clear(VramContext *v);
/* a TIM where it says it goes; or with its pixels and palette put elsewhere (as the game uploads some) */
int vram_load_tim(VramContext *v, const uint8_t *tim, size_t n);
int vram_load_tim_at(VramContext *v, const uint8_t *tim, size_t n, int x, int y, int palette_x, int palette_y);

/* the page a texture page word names, through a palette, as RGBA (PAGE_TEXELS x PAGE_TEXELS); the texture's
 * name ("page5-8bit-palette0-244", "page21-16bit") in name */
void vram_page_rgba(const VramContext *v, uint16_t texture_page, uint16_t palette, uint8_t *rgba);
void vram_texture_name(uint16_t texture_page, uint16_t palette, char *name, size_t n);

#endif
