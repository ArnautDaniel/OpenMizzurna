/* CD.BIN's table and entries (cdbin.h). */
#include "cdbin.h"

#include <stdlib.h>
#include <string.h>

#define EXE_HEADER 0x800                /* a PS-X EXE's header, before the code it loads */
#define EXE_LOAD_ADDRESS 0x80010000u    /* where SLPS_017.83 is loaded */
#define TABLE_ADDRESS 0x8009391Cu       /* the CD.BIN table in it (src: cd_file_table) */
#define CD_SECTOR 2048                  /* the table counts in data sectors */
#define PACK_MAGIC "1BIN"
#define PACK_HEADER 8                   /* the magic, then the unpacked size (big-endian) */

static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }
static uint32_t be32(const uint8_t *p) { return (uint32_t)p[0] << 24 | p[1] << 16 | p[2] << 8 | p[3]; }

int cdbin_table(Cdbin *t, const uint8_t *exe, size_t exe_size, size_t cdbin_size) {
    size_t at = EXE_HEADER + (TABLE_ADDRESS - EXE_LOAD_ADDRESS);

    memset(t, 0, sizeof(*t));
    if (exe_size < at + 8 || memcmp(exe, "PS-X EXE", 8) != 0) {
        return 0;
    }
    /* the table ends at its first all-zero entry after the first one, or where it would leave CD.BIN */
    for (; t->n < CDBIN_MAX_ENTRIES && at + 8 <= exe_size; at += 8) {
        uint32_t sector = le32(exe + at), size = le32(exe + at + 4);

        if ((t->n > 0 && sector == 0 && size == 0) || (uint64_t)sector * CD_SECTOR + size > cdbin_size) {
            break;
        }
        t->entries[t->n].offset = sector * CD_SECTOR;
        t->entries[t->n].size = size;
        t->n++;
    }
    return t->n > 0;
}

/* "1BIN": control bytes - c > 0: c bytes as they are; c < 0: 2 - c bytes copied from (the next byte + 1) back in
 * the output; 0: the end (src: cd_lz_decompress) */
uint8_t *cdbin_unpack(const uint8_t *packed, size_t n, size_t *size) {
    size_t want, in = PACK_HEADER, out = 0;
    uint8_t *dst;

    if (n < PACK_HEADER || memcmp(packed, PACK_MAGIC, 4) != 0) {
        return NULL;
    }
    want = be32(packed + 4);
    dst = malloc(want + 1);
    while (in < n) {
        int c = (int8_t)packed[in++];

        if (c == 0) {
            break;
        }
        if (c > 0) {
            if (in + (size_t)c > n || out + (size_t)c > want) {
                break;
            }
            memcpy(dst + out, packed + in, (size_t)c);
            in += (size_t)c;
            out += (size_t)c;
        } else {
            size_t back, count = (size_t)(2 - c), k;

            if (in >= n) {
                break;
            }
            back = (size_t)packed[in++] + 1;
            if (back > out || out + count > want) {
                break;
            }
            for (k = 0; k < count; k++, out++) {   /* (byte by byte: a copy may overlap what it makes) */
                dst[out] = dst[out - back];
            }
        }
    }
    if (out != want) {
        free(dst);
        return NULL;
    }
    *size = want;
    return dst;
}

uint8_t *cdbin_entry(const Cdbin *t, const uint8_t *cdbin, size_t cdbin_size, int i, size_t *size) {
    const CdbinEntry *e;
    uint8_t *copy;

    if (i < 0 || i >= t->n || t->entries[i].size == 0) {
        return NULL;
    }
    e = &t->entries[i];
    if ((uint64_t)e->offset + e->size > cdbin_size) {
        return NULL;
    }
    if (e->size >= PACK_HEADER && memcmp(cdbin + e->offset, PACK_MAGIC, 4) == 0) {
        return cdbin_unpack(cdbin + e->offset, e->size, size);
    }
    copy = malloc(e->size);
    memcpy(copy, cdbin + e->offset, e->size);
    *size = e->size;
    return copy;
}
