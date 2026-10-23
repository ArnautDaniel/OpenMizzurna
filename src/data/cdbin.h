/* CD.BIN: almost all of the game's data, in entries. The game's executable holds the table of where each one
 * is (a sector and a size, as the original's CD loader used it); entry `group * 5 + kind` is a group's data of
 * one kind - for an area, its code, sound bank header, sound samples, textures and data (see docs/library.md).
 * Most entries are packed ("1BIN": an LZ the original unpacked as it loaded). */
#ifndef CDBIN_H
#define CDBIN_H

#include <stddef.h>
#include <stdint.h>

#define CDBIN_KINDS 5          /* entries per group */
#define CDBIN_MAX_ENTRIES 1024

typedef struct CdbinEntry {
    uint32_t offset, size;     /* in CD.BIN, bytes (size 0: an empty entry) */
} CdbinEntry;

typedef struct Cdbin {
    CdbinEntry entries[CDBIN_MAX_ENTRIES];
    int n;
} Cdbin;

/* the table read out of the executable (a PS-X EXE); 0 if it isn't there or doesn't fit a CD.BIN of that size */
int cdbin_table(Cdbin *t, const uint8_t *exe, size_t exe_size, size_t cdbin_size);
/* entry i of CD.BIN (unpacked if packed), malloc'd; NULL if it's empty or damaged. *size its size */
uint8_t *cdbin_entry(const Cdbin *t, const uint8_t *cdbin, size_t cdbin_size, int i, size_t *size);
/* unpack a "1BIN" block; NULL if damaged */
uint8_t *cdbin_unpack(const uint8_t *packed, size_t n, size_t *size);

#endif
