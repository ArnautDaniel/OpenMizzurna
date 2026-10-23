/* The game's motions (animations), and their form in the library: plain text, a line a frame.
 *
 * A character's motion (the 193 shared ones, 033_4 part 1; an area's own, NNN_4 part 1): for each frame, each part's
 * turn (rx, ry, rz; 4096 a full turn, applied as the part's rest turn is) and the root's move that frame (dx, dy,
 * dz; dy upward). The game stores it packed - a value per part and axis, or "hold this one n frames" (src:
 * func_8001CE94) - and the text has every frame's values.
 *     om-motion 1 / parts <n> / frames <n> /
 *     frame <i> move <dx> <dy> <dz> turns <rx ry rz> ... (a triple a part)
 *
 * A prop's motion (033_4 part 0: the areas' moving objects): for each frame, where the object is from its rest
 * place (side, height, forward - along its rest heading) and its turn (rx, ry, rz) added to its rest turn (src:
 * func_8001E0F8, func_8001DFB4); `stride` turns are stored a frame and the first is used.
 *     om-prop-motion 1 / frames <n> / stride <n> /
 *     frame <i> place <side> <height> <forward> turns <rx ry rz> ... (stride triples) */
#ifndef MOTION_H
#define MOTION_H

#include <stddef.h>
#include <stdint.h>

typedef struct Motion {
    int prop;            /* a prop's motion (else a character's) */
    int parts;           /* a character's: parts (turns a frame); a prop's: its stride */
    int frames;
    int16_t (*move)[3];  /* a frame: the root's move (a character's) or the place (a prop's) */
    int16_t *turns;      /* frames x parts x 3 */
} Motion;

int motion_decode(Motion *m, const uint8_t *d, size_t n, int prop);
void motion_free(Motion *m);
char *motion_text(const Motion *m, const char *comment, size_t *len);
int motion_from_text(Motion *m, const char *s, size_t n, char *why, size_t nwhy);
int motion_same(const Motion *a, const Motion *b);

#endif
