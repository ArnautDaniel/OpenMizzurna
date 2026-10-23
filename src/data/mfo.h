/* The game's fonts ("MFO": the dialogue font 033_4/5/3, the list font 033_4/5/4) and their form in the library.
 *
 * An MFO: "MFO\0", u8 cell width, height, two bytes not known yet (12 16; 8 16), u32 the palettes' bytes
 * (from +0x10 to the glyph table), u16 glyphs, u16 palettes; per palette {u32 offset after the table, u32 0},
 * then the palettes (16 15-bit colours); per glyph {u32 its bitmap's offset after the table, u8 0, u8 width,
 * u8 height, u8 flags (2)}, then the bitmaps: 4 bits a pixel, the left pixel in the low nibble, (width+1)/2
 * bytes a row. Glyph i is text code 0xA000 + i: ASCII from ' ' (data/text.h).
 *
 * In the library a font `N` becomes:
 *   N.font.png       the glyphs, 16 to a row, each at the top left of its cell: an indexed PNG (palette 0)
 *   N.font.clut.png  the palettes, a row each (colours as TIMs': data/tim.h)
 *   N.font.txt       om-font 1 / cell / header / palettes / glyphs, each: its width, height, flags, offset
 * The font's bytes come back from them. */
#ifndef MFO_H
#define MFO_H

#include <stddef.h>
#include <stdint.h>

#define MFO_FIRST_CODE 0xA000
#define MFO_ATLAS_COLUMNS 16

int mfo_is(const uint8_t *d, size_t n);

typedef int (*MfoWriteFn)(void *ctx, const char *path, const uint8_t *data, size_t n);
typedef uint8_t *(*MfoReadFn)(void *ctx, const char *path, size_t *n);

int mfo_convert(const uint8_t *d, size_t n, const char *name, const char *what, MfoWriteFn write, void *ctx);
uint8_t *mfo_rebuild(const char *name, MfoReadFn read, void *ctx, size_t *size);

#endif
