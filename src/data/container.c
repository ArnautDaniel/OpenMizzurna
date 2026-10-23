/* Containers (container.h). */
#include "container.h"

#include <stdlib.h>
#include <string.h>

#define HEADER 8      /* count, then a zero word */
#define PART_ENTRY 8  /* offset, size */
#define ALIGN 4

static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }
static void put32(uint8_t *p, uint32_t v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); p[2] = (uint8_t)(v >> 16); p[3] = (uint8_t)(v >> 24); }
static size_t aligned(size_t n) { return (n + ALIGN - 1) & ~(size_t)(ALIGN - 1); }

/* the game's own check is none (it knows which entries are containers); this recognises one by its layout:
 * every part where the one before it ends (rounded up to 4), within the data, the rest zeros */
int container_is(const uint8_t *d, size_t n) {
    uint32_t count, i;
    size_t pos, k;

    if (n < HEADER + PART_ENTRY) {
        return 0;
    }
    count = le32(d);
    if (count == 0 || count > CONTAINER_MAX_PARTS || le32(d + 4) != 0 || HEADER + (size_t)count * PART_ENTRY > n) {
        return 0;
    }
    pos = HEADER + (size_t)count * PART_ENTRY;
    for (i = 0; i < count; i++) {
        uint32_t offset = le32(d + HEADER + i * PART_ENTRY), size = le32(d + HEADER + i * PART_ENTRY + 4);

        if (offset != pos || (uint64_t)offset + size > n) {
            return 0;
        }
        for (k = offset + size; k < aligned(offset + size) && k < n; k++) {
            if (d[k] != 0) {
                return 0;
            }
        }
        pos = aligned(offset + size);
    }
    for (k = pos; k < n; k++) {
        if (d[k] != 0) {
            return 0;
        }
    }
    return 1;
}

int container_parse(Container *c, const uint8_t *d, size_t n) {
    uint32_t i;
    size_t end = 0;

    memset(c, 0, sizeof(*c));
    if (!container_is(d, n)) {
        return 0;
    }
    c->n = (int)le32(d);
    for (i = 0; i < (uint32_t)c->n; i++) {
        uint32_t offset = le32(d + HEADER + i * PART_ENTRY);

        c->parts[i].data = d + offset;
        c->parts[i].size = le32(d + HEADER + i * PART_ENTRY + 4);
        end = offset + c->parts[i].size;   /* (the last part's own end: a container may stop there, unpadded) */
    }
    c->tail = n > end ? n - end : 0;
    return 1;
}

uint8_t *container_build(const ContainerPart *parts, int n, size_t tail, size_t *size) {
    size_t pos = HEADER + (size_t)n * PART_ENTRY, total = pos;
    uint8_t *out;
    int i;

    for (i = 0; i < n; i++) {   /* (each part starts aligned; the total ends where the last part does, then the tail) */
        total = (i > 0 ? aligned(total) : total) + parts[i].size;
    }
    total += tail;
    out = calloc(1, total);
    put32(out, (uint32_t)n);
    for (i = 0; i < n; i++) {
        put32(out + HEADER + i * PART_ENTRY, (uint32_t)pos);
        put32(out + HEADER + i * PART_ENTRY + 4, (uint32_t)parts[i].size);
        memcpy(out + pos, parts[i].data, parts[i].size);
        pos = aligned(pos + parts[i].size);
    }
    *size = total;
    return out;
}
