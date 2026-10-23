/* The disc image and its ISO 9660 file system (disc.h). */
#include "disc.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <strings.h>

/* where a raw MODE2 sector's parts are */
#define SECTOR_SUBHEADER 16      /* file, channel, submode, coding (twice) */
#define SECTOR_USER_DATA 24      /* a form 1 sector's 2048 bytes */
#define SUBMODE_FORM2 0x20       /* the subheader's submode: a form 2 sector (audio, video) */

/* ISO 9660 */
#define ISO_DESCRIPTOR_SECTOR 16 /* the primary volume descriptor */
#define ISO_ROOT_RECORD 156      /* the root directory's record, in the descriptor */
#define ISO_DIR_FLAG 0x02        /* a record's flags: a directory */
#define XA_ATTR_FORM2 0x1000     /* CD-XA attributes (in a record's system use area): form 2 sectors */
#define XA_ATTR_INTERLEAVED 0x2000

#define MAX_ENTRIES 512
#define MAX_DEPTH 8

struct Disc {
    FILE *bin;
    uint64_t sectors;
    DiscEntry entries[MAX_ENTRIES];
    int n;
};

static int read_sector(Disc *d, uint32_t lba, uint8_t *raw) {
    if (lba >= d->sectors || fseeko(d->bin, (off_t)lba * DISC_SECTOR_RAW, SEEK_SET) != 0) {
        return 0;
    }
    return fread(raw, 1, DISC_SECTOR_RAW, d->bin) == DISC_SECTOR_RAW;
}

static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }
static uint16_t be16(const uint8_t *p) { return (uint16_t)(p[0] << 8 | p[1]); }

/* a directory record's CD-XA attributes, if it has them ("XA" in its system use area) */
static unsigned xa_attributes(const uint8_t *rec) {
    unsigned len = rec[0], name_len = rec[32], sua = 33 + name_len + (name_len % 2 == 0);

    if (sua + 14 <= len && rec[sua + 6] == 'X' && rec[sua + 7] == 'A') {
        return be16(rec + sua + 4);
    }
    return 0;
}

static void walk(Disc *d, uint32_t lba, uint32_t size, const char *prefix, int depth) {
    uint32_t sector, sectors = (size + DISC_SECTOR_DATA - 1) / DISC_SECTOR_DATA;
    uint8_t raw[DISC_SECTOR_RAW];

    if (depth > MAX_DEPTH) {
        return;
    }
    for (sector = 0; sector < sectors; sector++) {
        const uint8_t *data = raw + SECTOR_USER_DATA;
        unsigned pos = 0;

        if (!read_sector(d, lba + sector, raw)) {
            return;
        }
        while (pos < DISC_SECTOR_DATA && data[pos] != 0) {
            const uint8_t *rec = data + pos;
            unsigned name_len = rec[32];
            char name[128], path[256];

            pos += rec[0];
            if (name_len == 1 && (rec[33] == 0 || rec[33] == 1)) {   /* (. and ..) */
                continue;
            }
            snprintf(name, sizeof(name), "%.*s", (int)name_len, (const char *)rec + 33);
            if (strchr(name, ';') != NULL) {
                *strchr(name, ';') = 0;
            }
            snprintf(path, sizeof(path), "%s%s", prefix, name);
            if (rec[25] & ISO_DIR_FLAG) {
                char sub[260];

                snprintf(sub, sizeof(sub), "%s/", path);
                walk(d, le32(rec + 2), le32(rec + 10), sub, depth + 1);
            } else if (d->n < MAX_ENTRIES) {
                DiscEntry *e = &d->entries[d->n++];
                unsigned xa = xa_attributes(rec);

                snprintf(e->path, sizeof(e->path), "%s", path);
                e->lba = le32(rec + 2);
                e->size = le32(rec + 10);
                e->raw = (xa & (XA_ATTR_FORM2 | XA_ATTR_INTERLEAVED)) != 0;
            }
        }
    }
}

/* the .bin a .cue names (FILE "name" BINARY), beside the .cue */
static int cue_bin(const char *cue, char *bin, size_t n, char *why, size_t nwhy) {
    FILE *f = fopen(cue, "r");
    char line[1024];
    const char *slash = strrchr(cue, '/');
    int dir_len = slash != NULL ? (int)(slash - cue + 1) : 0;

    if (f == NULL) {
        snprintf(why, nwhy, "can't read %s", cue);
        return 0;
    }
    while (fgets(line, sizeof(line), f) != NULL) {
        char *q1 = strchr(line, '"'), *q2 = q1 != NULL ? strchr(q1 + 1, '"') : NULL;

        if (strstr(line, "FILE") != NULL && q2 != NULL) {
            if (strstr(line, "BINARY") == NULL) {
                snprintf(why, nwhy, "the .cue's track isn't a raw (BINARY) image");
                fclose(f);
                return 0;
            }
            snprintf(bin, n, "%.*s%.*s", dir_len, cue, (int)(q2 - q1 - 1), q1 + 1);
            fclose(f);
            return 1;
        }
    }
    fclose(f);
    snprintf(why, nwhy, "the .cue names no image file");
    return 0;
}

Disc *disc_open(const char *path, char *why, size_t nwhy) {
    char bin[1100];
    const char *dot = strrchr(path, '.');
    Disc *d;
    uint8_t raw[DISC_SECTOR_RAW];
    long long bytes;
    static const uint8_t kSync[12] = {0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0};

    if (dot != NULL && strcasecmp(dot, ".cue") == 0) {
        if (!cue_bin(path, bin, sizeof(bin), why, nwhy)) {
            return NULL;
        }
    } else {
        snprintf(bin, sizeof(bin), "%s", path);
    }
    d = calloc(1, sizeof(*d));
    d->bin = fopen(bin, "rb");
    if (d->bin == NULL) {
        snprintf(why, nwhy, "can't read %s", bin);
        free(d);
        return NULL;
    }
    fseeko(d->bin, 0, SEEK_END);
    bytes = (long long)ftello(d->bin);
    d->sectors = (uint64_t)bytes / DISC_SECTOR_RAW;
    if (bytes % DISC_SECTOR_RAW != 0 || !read_sector(d, 0, raw) || memcmp(raw, kSync, sizeof(kSync)) != 0) {
        snprintf(why, nwhy, bytes % DISC_SECTOR_DATA == 0
                 ? "this is a 2048-byte .iso: the music and movies need a raw image (.cue / .bin, 2352-byte sectors)"
                 : "not a raw CD image (.cue / .bin)");
        disc_close(d);
        return NULL;
    }
    if (!read_sector(d, ISO_DESCRIPTOR_SECTOR, raw) || raw[SECTOR_USER_DATA] != 1 ||
        memcmp(raw + SECTOR_USER_DATA + 1, "CD001", 5) != 0) {
        snprintf(why, nwhy, "no ISO 9660 file system on the disc");
        disc_close(d);
        return NULL;
    }
    {
        const uint8_t *root = raw + SECTOR_USER_DATA + ISO_ROOT_RECORD;
        walk(d, le32(root + 2), le32(root + 10), "", 0);
    }
    return d;
}

void disc_close(Disc *d) {
    if (d != NULL) {
        if (d->bin != NULL) {
            fclose(d->bin);
        }
        free(d);
    }
}

int disc_count(const Disc *d) { return d->n; }
const DiscEntry *disc_entry(const Disc *d, int i) { return i >= 0 && i < d->n ? &d->entries[i] : NULL; }

const DiscEntry *disc_find(const Disc *d, const char *path) {
    int i;

    for (i = 0; i < d->n; i++) {
        if (strcasecmp(d->entries[i].path, path) == 0) {
            return &d->entries[i];
        }
    }
    return NULL;
}

uint64_t disc_file_bytes(const DiscEntry *e) {
    uint64_t sectors = (e->size + DISC_SECTOR_DATA - 1) / DISC_SECTOR_DATA;
    return e->raw ? sectors * DISC_SECTOR_RAW : e->size;
}

int disc_read(Disc *d, const DiscEntry *e, uint8_t *buf) {
    uint64_t sectors = (e->size + DISC_SECTOR_DATA - 1) / DISC_SECTOR_DATA, s;
    uint8_t raw[DISC_SECTOR_RAW];

    for (s = 0; s < sectors; s++) {
        if (!read_sector(d, e->lba + (uint32_t)s, raw)) {
            return 0;
        }
        if (e->raw) {
            memcpy(buf + s * DISC_SECTOR_RAW, raw, DISC_SECTOR_RAW);
        } else {
            uint64_t at = s * DISC_SECTOR_DATA, n = e->size - at < DISC_SECTOR_DATA ? e->size - at : DISC_SECTOR_DATA;
            memcpy(buf + at, raw + SECTOR_USER_DATA, n);
        }
    }
    return 1;
}

uint8_t *disc_read_all(Disc *d, const char *path, size_t *size) {
    const DiscEntry *e = disc_find(d, path);
    uint8_t *buf;

    if (e == NULL) {
        return NULL;
    }
    buf = malloc(disc_file_bytes(e) + 1);
    if (!disc_read(d, e, buf)) {
        free(buf);
        return NULL;
    }
    buf[disc_file_bytes(e)] = 0;
    *size = disc_file_bytes(e);
    return buf;
}
