/* Reading .zip files (zip.h): the central directory read once; a file's data read and inflated (stb_image's
 * zlib) when asked for. */
#define _FILE_OFFSET_BITS 64
#include "zip.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <strings.h>

#include "../third_party/stb_image.h"

/* the zip format's records (PKWARE's APPNOTE): each starts with its signature */
#define END_SIGNATURE 0x06054b50u          /* the end of the central directory */
#define DIRECTORY_SIGNATURE 0x02014b50u    /* a file's entry in the central directory */
#define LOCAL_SIGNATURE 0x04034b50u        /* a file's own header, before its data */
#define END_SIZE 22                        /* the end record, without its comment */
#define COMMENT_MOST 65535                 /* the end record's comment at most */
#define DIRECTORY_ENTRY_SIZE 46            /* a central directory entry, before its name */
#define LOCAL_HEADER_SIZE 30               /* a local header, before its name and extra field */
#define FLAG_ENCRYPTED 1
#define METHOD_STORED 0
#define METHOD_DEFLATED 8
#define ZIP64_COUNT 0xFFFFu                /* a count or offset saying "see the zip64 record" (not read) */
#define ZIP64_OFFSET 0xFFFFFFFFu
#define SIZE_MOST 0x7FFFFFFFu              /* a file's sizes, packed and not: under 2 GB (stb's inflate counts in ints) */

typedef struct ZipEntry {
    char *name;
    uint32_t at;      /* its local header */
    uint32_t packed, size;
    int method;
} ZipEntry;

struct Zip {
    char path[1024];
    ZipEntry *e;
    int n;
};

static uint16_t le16(const uint8_t *p) { return (uint16_t)(p[0] | p[1] << 8); }
static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }

static Zip *refuse(Zip *z, FILE *f, uint8_t *buf, const char *path, const char *why) {
    fprintf(stderr, "zip: %s: %s\n", path, why);
    free(buf);
    if (f != NULL) {
        fclose(f);
    }
    zip_close(z);
    return NULL;
}

Zip *zip_open(const char *path) {
    FILE *f = fopen(path, "rb");
    uint8_t *buf = NULL, *p, *end;
    long size, tail, i;
    uint32_t cd_at, cd_size;
    int count, k;
    Zip *z;

    if (f == NULL) {
        return NULL;
    }
    if ((z = calloc(1, sizeof(*z))) == NULL) {
        fclose(f);
        return NULL;
    }
    snprintf(z->path, sizeof(z->path), "%s", path);
    fseek(f, 0, SEEK_END);
    size = ftell(f);
    tail = size < END_SIZE + COMMENT_MOST ? size : END_SIZE + COMMENT_MOST;
    buf = malloc((size_t)tail + 1);
    if (buf == NULL || fseek(f, size - tail, SEEK_SET) != 0 || fread(buf, 1, (size_t)tail, f) != (size_t)tail) {
        return refuse(z, f, buf, path, "can't be read");
    }
    for (i = tail - END_SIZE; i >= 0 && le32(buf + i) != END_SIGNATURE; i--) {
    }
    if (i < 0) {
        return refuse(z, f, buf, path, "isn't a zip");
    }
    count = le16(buf + i + 10);
    cd_size = le32(buf + i + 12);
    cd_at = le32(buf + i + 16);
    if (count == ZIP64_COUNT || cd_at == ZIP64_OFFSET || (long)cd_at + (long)cd_size > size) {
        return refuse(z, f, buf, path, "is a zip64 or damaged (not read)");
    }
    free(buf);
    buf = malloc(cd_size + 1u);
    if (buf == NULL || fseek(f, (long)cd_at, SEEK_SET) != 0 || fread(buf, 1, cd_size, f) != cd_size) {
        return refuse(z, f, buf, path, "its directory can't be read");
    }
    fclose(f);
    f = NULL;
    if ((z->e = calloc((size_t)count + 1, sizeof(ZipEntry))) == NULL) {
        return refuse(z, f, buf, path, "is too big to read");
    }
    p = buf;
    end = buf + cd_size;
    for (k = 0; k < count; k++) {
        int nlen, j;
        ZipEntry *e;

        if (p + DIRECTORY_ENTRY_SIZE > end || le32(p) != DIRECTORY_SIGNATURE) {
            return refuse(z, f, buf, path, "its directory is damaged");
        }
        nlen = le16(p + 28);
        if (p + DIRECTORY_ENTRY_SIZE + nlen > end) {
            return refuse(z, f, buf, path, "its directory is damaged");
        }
        if ((le16(p + 8) & FLAG_ENCRYPTED) != 0) {
            return refuse(z, f, buf, path, "is encrypted");
        }
        e = &z->e[z->n];
        e->method = le16(p + 10);
        e->packed = le32(p + 20);
        e->size = le32(p + 24);
        e->at = le32(p + 42);
        if (nlen > 0 && p[DIRECTORY_ENTRY_SIZE + nlen - 1] != '/') {   /* (a folder's own entry: left out) */
            if ((e->name = malloc((size_t)nlen + 1)) == NULL) {
                return refuse(z, f, buf, path, "is too big to read");
            }
            memcpy(e->name, p + DIRECTORY_ENTRY_SIZE, (size_t)nlen);
            e->name[nlen] = 0;
            for (j = 0; j < nlen; j++) {
                e->name[j] = e->name[j] == '\\' ? '/' : e->name[j];
            }
            z->n++;
        }
        p += DIRECTORY_ENTRY_SIZE + nlen + le16(p + 30) + le16(p + 32);
    }
    free(buf);
    return z;
}

void zip_close(Zip *z) {
    int i;

    if (z == NULL) {
        return;
    }
    for (i = 0; i < z->n; i++) {
        free(z->e[i].name);
    }
    free(z->e);
    free(z);
}

int zip_count(const Zip *z) { return z != NULL ? z->n : 0; }
const char *zip_name(const Zip *z, int i) { return z != NULL && i >= 0 && i < z->n ? z->e[i].name : NULL; }

int zip_find(const Zip *z, const char *name) {
    char want[1024];
    int i;

    if (z == NULL) {
        return -1;
    }
    snprintf(want, sizeof(want), "%s", name);
    for (i = 0; want[i] != 0; i++) {
        want[i] = want[i] == '\\' ? '/' : want[i];
    }
    for (i = 0; i < z->n; i++) {
        if (strcasecmp(z->e[i].name, want) == 0) {
            return i;
        }
    }
    return -1;
}

uint8_t *zip_read(const Zip *z, int i, size_t *size) {
    const ZipEntry *e;
    uint8_t head[LOCAL_HEADER_SIZE], *packed = NULL, *out = NULL;
    FILE *f;

    if (z == NULL || i < 0 || i >= z->n || (f = fopen(z->path, "rb")) == NULL) {
        return NULL;
    }
    e = &z->e[i];
    if (e->packed >= SIZE_MOST || e->size >= SIZE_MOST) {
        fprintf(stderr, "zip: %s: %s is too big to read\n", z->path, e->name);
        fclose(f);
        return NULL;
    }
    if (fseek(f, (long)e->at, SEEK_SET) != 0 || fread(head, 1, LOCAL_HEADER_SIZE, f) != LOCAL_HEADER_SIZE ||
        le32(head) != LOCAL_SIGNATURE ||
        fseek(f, (long)e->at + LOCAL_HEADER_SIZE + le16(head + 26) + le16(head + 28), SEEK_SET) != 0 ||
        (packed = malloc((size_t)e->packed + 1)) == NULL || fread(packed, 1, e->packed, f) != e->packed) {
        fprintf(stderr, "zip: %s: %s can't be read\n", z->path, e->name);
        free(packed);
        fclose(f);
        return NULL;
    }
    fclose(f);
    if (e->method == METHOD_STORED && e->packed == e->size) {
        out = packed;
        packed = NULL;
    } else if (e->method == METHOD_DEFLATED && (out = malloc((size_t)e->size + 1)) != NULL &&
               stbi_zlib_decode_noheader_buffer((char *)out, (int)e->size, (const char *)packed, (int)e->packed) !=
                   (int)e->size) {
        free(out);
        out = NULL;
    }
    free(packed);
    if (out == NULL) {
        fprintf(stderr, "zip: %s: %s isn't stored or deflated as OpenMizzurna reads (method %d)\n", z->path, e->name,
                e->method);
        return NULL;
    }
    out[e->size] = 0;
    if (size != NULL) {
        *size = e->size;
    }
    return out;
}
