/* An area's billboards (NNN_4's part 5; src: billboard_init, func_8003B2DC) and their form in the library: plain
 * text. 16 sets - a scene's each: how many and where their entries start (in halfwords from the part's start) - then
 * the entries, 12 bytes each: x, z (world units from the area's origin), bottom, sprite, half width, top.
 *     om-area-billboards 1 / set <scene> count <n> at <halfword> / entries <n> /
 *     billboard <i> at <x> <z> from <bottom> to <top> half-width <w> sprite <s>
 * Known by where it is (every NNN_4's part 5 laid out so). */
#ifndef BILLBOARDS_H
#define BILLBOARDS_H

#include <stddef.h>
#include <stdint.h>

int billboards_part(const char *base, const uint8_t *d, size_t n);   /* it is an area's billboards */
char *billboards_text(const uint8_t *d, size_t n, const char *comment, size_t *len);
uint8_t *billboards_from_text(const char *s, size_t n, size_t *len);

#endif
