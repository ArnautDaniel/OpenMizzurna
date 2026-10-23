/* Containers: how CD.BIN's entries group their parts - {u32 count, u32 0, {u32 offset, u32 size} x count}, the
 * parts after, each starting on a 4-byte boundary; a part may be a container itself. The library keeps a
 * container as a folder (its parts as files, index.txt naming them); container_build makes the bytes again. */
#ifndef CONTAINER_H
#define CONTAINER_H

#include <stddef.h>
#include <stdint.h>

#define CONTAINER_MAX_PARTS 511

typedef struct ContainerPart {
    const uint8_t *data;
    size_t size;
} ContainerPart;

typedef struct Container {
    ContainerPart parts[CONTAINER_MAX_PARTS];
    int n;
    size_t tail;   /* zero bytes after the last part's end (some are padded, some stop unaligned) */
} Container;

/* is it a container (laid out as above, every part inside it)? */
int container_is(const uint8_t *d, size_t n);
/* its parts (pointing into d); 0 if it isn't one */
int container_parse(Container *c, const uint8_t *d, size_t n);
/* the bytes of a container of these parts (malloc'd), laid out as the game's are */
uint8_t *container_build(const ContainerPart *parts, int n, size_t tail, size_t *size);

#endif
