/* The town's roads, for the cars (src: carline.c), and their form in the library: plain text.
 *
 * The roads are a graph of nodes (033_4 part 5, part 1: 16 bytes a node) and the bends between them (part 2:
 * 8 bytes a point). A node: where it is (x, y, z: world units, y downward) and its run of points - the road
 * leaving it bends along `points` steps from point `first` on. A point: a step (dx, dy, dz) and its length.
 * The parts have no mark of their own: they are known by where they are.
 *     om-road-nodes 1 / nodes <n> / node <i> at <x> <y> <z> points <n> first <i>
 *     om-road-points 1 / points <n> / point <i> step <dx> <dy> <dz> length <n> */
#ifndef ROADS_H
#define ROADS_H

#include <stddef.h>
#include <stdint.h>

typedef enum { ROADS_NONE, ROADS_NODES, ROADS_POINTS } RoadsKind;

/* what a part is by where it is ("cdbin/033_4/5/1") and its size */
RoadsKind roads_kind(const char *base, size_t n);
/* the text of a part, and the part again from its text (NULL: not a roads text) */
char *roads_text(RoadsKind kind, const uint8_t *d, size_t n, const char *comment, size_t *len);
uint8_t *roads_from_text(RoadsKind kind, const char *s, size_t n, size_t *len);

#endif
