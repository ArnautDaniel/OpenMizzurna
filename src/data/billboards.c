/* An area's billboards in the library (billboards.h). */
#include "billboards.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define SETS 16
#define SET_SIZE 4
#define HEAD (SETS * SET_SIZE)
#define ENTRY_SIZE 12
#define AREA_GROUPS 33
#define BILLBOARDS_PART 5
#define LINE 112

static int s16_at(const uint8_t *p) { return (int16_t)(p[0] | p[1] << 8); }
static void put16(uint8_t *p, int v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); }

int billboards_part(const char *base, const uint8_t *d, size_t n) {
    int group, part, i;
    char rest[8];

    if (sscanf(base, "cdbin/%d_4/%d%1s", &group, &part, rest) != 2 || group >= AREA_GROUPS || part != BILLBOARDS_PART ||
        n < HEAD || (n - HEAD) % ENTRY_SIZE != 0) {
        return 0;
    }
    for (i = 0; i < SETS; i++) {   /* (each set's entries within the part, on an entry's boundary) */
        int count = s16_at(d + i * SET_SIZE), at = s16_at(d + i * SET_SIZE + 2) * 2;
        if (count < 0 || (count > 0 && (at < HEAD || (at - HEAD) % ENTRY_SIZE != 0 || at + (size_t)count * ENTRY_SIZE > n))) {
            return 0;
        }
    }
    return 1;
}

char *billboards_text(const uint8_t *d, size_t n, const char *comment, size_t *len) {
    size_t entries = (n - HEAD) / ENTRY_SIZE, cap = (entries + SETS + 4) * LINE + strlen(comment), k = 0, i;
    char *t = malloc(cap);

    k += (size_t)snprintf(t + k, cap - k, "# %s\nom-area-billboards 1\n", comment);
    for (i = 0; i < SETS; i++) {
        k += (size_t)snprintf(t + k, cap - k, "set %zu count %d at %d\n", i, s16_at(d + i * SET_SIZE),
                              s16_at(d + i * SET_SIZE + 2));
    }
    k += (size_t)snprintf(t + k, cap - k, "entries %zu\n", entries);
    for (i = 0; i < entries; i++) {
        const uint8_t *b = d + HEAD + i * ENTRY_SIZE;
        k += (size_t)snprintf(t + k, cap - k, "billboard %zu at %d %d from %d to %d half-width %d sprite %d\n", i,
                              s16_at(b), s16_at(b + 2), s16_at(b + 4), s16_at(b + 10), s16_at(b + 8), s16_at(b + 6));
    }
    *len = k;
    return t;
}

uint8_t *billboards_from_text(const char *s, size_t n, size_t *len) {
    char *t = malloc(n + 1), *line;
    uint8_t *d = NULL, head[HEAD];
    int entries = -1, seen = 0, sets = 0;

    memcpy(t, s, n);
    t[n] = 0;
    memset(head, 0, sizeof(head));
    for (line = strtok(t, "\n"); line != NULL; line = strtok(NULL, "\n")) {
        int i, a, b, c, e, g, h;

        if (sscanf(line, "set %d count %d at %d", &i, &a, &b) == 3 && i >= 0 && i < SETS) {
            put16(head + i * SET_SIZE, a), put16(head + i * SET_SIZE + 2, b);
            sets++;
        } else if (sscanf(line, "entries %d", &entries) == 1 && entries >= 0 && d == NULL) {
            d = calloc(1, HEAD + (size_t)entries * ENTRY_SIZE);
        } else if (d != NULL && sscanf(line, "billboard %d at %d %d from %d to %d half-width %d sprite %d", &i, &a, &b, &c,
                                       &e, &g, &h) == 7 && i >= 0 && i < entries) {
            uint8_t *p = d + HEAD + i * ENTRY_SIZE;
            put16(p, a), put16(p + 2, b), put16(p + 4, c), put16(p + 6, h), put16(p + 8, g), put16(p + 10, e);
            seen++;
        }
    }
    free(t);
    if (d == NULL || sets != SETS || seen != entries) {
        free(d);
        return NULL;
    }
    memcpy(d, head, HEAD);
    *len = HEAD + (size_t)entries * ENTRY_SIZE;
    return d;
}
