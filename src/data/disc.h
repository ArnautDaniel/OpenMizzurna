/* The player's disc image, read for the import: a raw CD image (.cue and its .bin, or a lone .bin: 2352-byte
 * sectors, MODE2) and the ISO 9660 file system on it.
 *
 * Mizzurna Falls's disc mixes two kinds of file. Most are data (CD-XA form 1: 2048 bytes of a sector's 2352 are
 * the file's). The music (TSG.XA) and the movies (*.STR) are interleaved streams whose sectors carry their own
 * headers (form 2 audio and video): those are read whole, 2352 bytes a sector, as the game's streaming read
 * them. A 2048-byte .iso has lost those sectors' extra bytes, so it can't hold the game. */
#ifndef DISC_H
#define DISC_H

#include <stddef.h>
#include <stdint.h>

#define DISC_SECTOR_RAW 2352    /* a whole sector: sync, header, subheader, data, error correction */
#define DISC_SECTOR_DATA 2048   /* a data (form 1) sector's file bytes */

typedef struct DiscEntry {
    char path[300];     /* "SLPS_017.83", "MOVIE/OPENING.STR" ... ('/' between folders, no ";1") */
    uint32_t lba;       /* its first sector */
    uint32_t size;      /* its size as the directory gives it (data bytes) */
    int raw;            /* read whole sectors (an XA / STR stream) */
} DiscEntry;

typedef struct Disc Disc;

/* open a disc image (.cue or .bin); NULL with the reason in why */
Disc *disc_open(const char *path, char *why, size_t nwhy);
void disc_close(Disc *d);

int disc_count(const Disc *d);
const DiscEntry *disc_entry(const Disc *d, int i);
const DiscEntry *disc_find(const Disc *d, const char *path);   /* case doesn't matter; NULL if none */

/* the bytes a file holds on disc: data bytes, or whole raw sectors for a stream */
uint64_t disc_file_bytes(const DiscEntry *e);
/* read a file into buf (disc_file_bytes of it); 0 on a read error */
int disc_read(Disc *d, const DiscEntry *e, uint8_t *buf);
/* the whole file (malloc'd), or NULL; its size in *size */
uint8_t *disc_read_all(Disc *d, const char *path, size_t *size);

#endif
