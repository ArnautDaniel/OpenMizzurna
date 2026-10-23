/* Video memory for the import (vramctx.h). */
#include "vramctx.h"

#include "tim.h"

#include <stdio.h>
#include <string.h>

#define PAGE_X(word) (((word) & 0xF) * 64)          /* 64 16-bit units a page */
#define PAGE_Y(word) (((word) >> 4 & 1) * 256)
#define PAGE_DEPTH(word) ((word) >> 7 & 3)          /* 0: 4 bits, 1: 8 bits, 2: 16 bits */
#define PALETTE_X(word) (((word) & 0x3F) * 16)
#define PALETTE_Y(word) ((word) >> 6 & 0x1FF)

void vram_clear(VramContext *v) { memset(v->vram, 0, sizeof(v->vram)); }

static void blit(VramContext *v, int x, int y, int w, int h, const uint8_t *data) {
    int r, c;

    for (r = 0; r < h; r++) {
        for (c = 0; c < w; c++) {
            if (y + r < VRAM_HEIGHT && x + c < VRAM_WIDTH) {
                const uint8_t *p = data + ((size_t)r * w + c) * 2;
                v->vram[y + r][x + c] = (uint16_t)(p[0] | p[1] << 8);
            }
        }
    }
}

int vram_load_tim_at(VramContext *v, const uint8_t *tim, size_t n, int x, int y, int palette_x, int palette_y) {
    Tim t;

    if (!tim_parse(&t, tim, n)) {
        return 0;
    }
    blit(v, x, y, t.pix_w, t.pix_h, t.pixels);
    if (t.has_clut) {
        blit(v, palette_x, palette_y, t.clut_w, t.clut_h, t.clut);
    }
    return 1;
}

int vram_load_tim(VramContext *v, const uint8_t *tim, size_t n) {
    Tim t;

    if (!tim_parse(&t, tim, n)) {
        return 0;
    }
    return vram_load_tim_at(v, tim, n, t.pix_x, t.pix_y, t.clut_x, t.clut_y);
}

void vram_page_rgba(const VramContext *v, uint16_t page, uint16_t palette, uint8_t *rgba) {
    int px = PAGE_X(page), py = PAGE_Y(page), depth = PAGE_DEPTH(page), cx = PALETTE_X(palette),
        cy = PALETTE_Y(palette), x, y;

    for (y = 0; y < PAGE_TEXELS; y++) {
        const uint16_t *row = v->vram[(py + y) % VRAM_HEIGHT];
        for (x = 0; x < PAGE_TEXELS; x++) {
            uint16_t c;
            if (depth == 0) {
                int i = row[(px + x / 4) % VRAM_WIDTH] >> (x % 4 * 4) & 0xF;
                c = v->vram[cy][(cx + i) % VRAM_WIDTH];
            } else if (depth == 1) {
                int i = row[(px + x / 2) % VRAM_WIDTH] >> (x % 2 * 8) & 0xFF;
                c = v->vram[cy][(cx + i) % VRAM_WIDTH];
            } else {
                c = row[(px + x) % VRAM_WIDTH];
            }
            tim_color_rgba(c, rgba + ((size_t)y * PAGE_TEXELS + x) * 4);
        }
    }
}

void vram_texture_name(uint16_t page, uint16_t palette, char *name, size_t n) {
    static const char *kDepth[4] = {"4bit", "8bit", "16bit", "16bit"};
    int index = (page & 0xF) | (page >> 4 & 1) << 4;

    if (PAGE_DEPTH(page) >= 2) {
        snprintf(name, n, "page%d-%s", index, kDepth[PAGE_DEPTH(page)]);
    } else {
        snprintf(name, n, "page%d-%s-palette%d-%d", index, kDepth[PAGE_DEPTH(page)], PALETTE_X(palette), PALETTE_Y(palette));
    }
}
