/* PNG files for the library: RGBA (any image), and indexed (4 or 8 bits a pixel with a palette) for the game's
 * palette images - an indexed PNG keeps each pixel's palette index exactly, and any image viewer shows it in its
 * palette. Reading takes any PNG for RGBA; indexed reading wants an indexed PNG. */
#ifndef PNG_H
#define PNG_H

#include <stddef.h>
#include <stdint.h>

/* a PNG file's bytes (malloc'd), its size in *n; NULL on failure */
uint8_t *png_encode_rgba(const uint8_t *rgba, int w, int h, size_t *n);
/* idx: one byte a pixel (0..15 for depth 4); palette: (1 << depth) RGBA colours */
uint8_t *png_encode_indexed(const uint8_t *idx, int w, int h, int depth, const uint8_t *palette, size_t *n);

/* the image as RGBA (malloc'd), NULL if it isn't a PNG */
uint8_t *png_read_rgba(const uint8_t *file, size_t n, int *w, int *h);
/* an indexed PNG's indices (malloc'd, a byte a pixel) and its bit depth; NULL if it isn't one */
uint8_t *png_read_indexed(const uint8_t *file, size_t n, int *w, int *h, int *depth);

#endif
