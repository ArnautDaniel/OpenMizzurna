/* The town's roads in the library (roads.h). */
#include "roads.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define NODE_SIZE 16
#define POINT_SIZE 8
#define ROADS_GROUP 33   /* the common container, 033_4 */
#define ROADS_ENTRY 5
#define NODES_PART 1
#define POINTS_PART 2
#define LINE_MAX_CHARS 96

static int32_t s32_at(const uint8_t *p) { return (int32_t)((uint32_t)p[0] | (uint32_t)p[1] << 8 | (uint32_t)p[2] << 16 | (uint32_t)p[3] << 24); }
static int u16_at(const uint8_t *p) { return p[0] | p[1] << 8; }
static int s16_at(const uint8_t *p) { return (int16_t)u16_at(p); }
static void put_s32(uint8_t *p, int32_t v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); p[2] = (uint8_t)(v >> 16); p[3] = (uint8_t)(v >> 24); }
static void put_16(uint8_t *p, int v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); }

RoadsKind roads_kind(const char *base, size_t n) {
    int group, entry, part;
    char rest[8];

    if (sscanf(base, "cdbin/%d_4/%d/%d%1s", &group, &entry, &part, rest) != 3 || group != ROADS_GROUP ||
        entry != ROADS_ENTRY || n == 0) {
        return ROADS_NONE;
    }
    if (part == NODES_PART && n % NODE_SIZE == 0) {
        return ROADS_NODES;
    }
    if (part == POINTS_PART && n % POINT_SIZE == 0) {
        return ROADS_POINTS;
    }
    return ROADS_NONE;
}

char *roads_text(RoadsKind kind, const uint8_t *d, size_t n, const char *comment, size_t *len) {
    size_t size = kind == ROADS_NODES ? NODE_SIZE : POINT_SIZE, count = n / size, i, k = 0, cap;
    char *s;

    cap = (count + 4) * LINE_MAX_CHARS + strlen(comment);
    s = malloc(cap);
    if (kind == ROADS_NODES) {
        k += (size_t)snprintf(s + k, cap - k, "# %s\nom-road-nodes 1\nnodes %zu\n", comment, count);
        for (i = 0; i < count; i++) {
            const uint8_t *p = d + i * size;
            k += (size_t)snprintf(s + k, cap - k, "node %zu at %d %d %d points %d first %d\n", i, s32_at(p),
                                  s32_at(p + 4), s32_at(p + 8), u16_at(p + 12), u16_at(p + 14));
        }
    } else {
        k += (size_t)snprintf(s + k, cap - k, "# %s\nom-road-points 1\npoints %zu\n", comment, count);
        for (i = 0; i < count; i++) {
            const uint8_t *p = d + i * size;
            k += (size_t)snprintf(s + k, cap - k, "point %zu step %d %d %d length %d\n", i, s16_at(p), s16_at(p + 2),
                                  s16_at(p + 4), u16_at(p + 6));
        }
    }
    *len = k;
    return s;
}

uint8_t *roads_from_text(RoadsKind kind, const char *s, size_t n, size_t *len) {
    char *t = malloc(n + 1), *line;
    size_t size = kind == ROADS_NODES ? NODE_SIZE : POINT_SIZE, count = 0, seen = 0;
    uint8_t *d = NULL;
    int ok = 0;

    memcpy(t, s, n);
    t[n] = 0;
    for (line = strtok(t, "\n"); line != NULL; line = strtok(NULL, "\n")) {
        size_t i;
        int a, b, c, e, f;

        if (line[0] == '#') {
            continue;
        }
        if (sscanf(line, kind == ROADS_NODES ? "nodes %zu" : "points %zu", &count) == 1) {
            d = calloc(count, size);
            ok = 1;
        } else if (d != NULL && kind == ROADS_NODES &&
                   sscanf(line, "node %zu at %d %d %d points %d first %d", &i, &a, &b, &c, &e, &f) == 6 && i < count) {
            uint8_t *p = d + i * size;
            put_s32(p, a), put_s32(p + 4, b), put_s32(p + 8, c), put_16(p + 12, e), put_16(p + 14, f);
            seen++;
        } else if (d != NULL && kind == ROADS_POINTS &&
                   sscanf(line, "point %zu step %d %d %d length %d", &i, &a, &b, &c, &e) == 5 && i < count) {
            uint8_t *p = d + i * size;
            put_16(p, a), put_16(p + 2, b), put_16(p + 4, c), put_16(p + 6, e);
            seen++;
        }
    }
    free(t);
    if (!ok || seen != count) {
        free(d);
        return NULL;
    }
    *len = count * size;
    return d;
}
