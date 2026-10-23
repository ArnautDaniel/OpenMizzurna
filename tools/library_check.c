/* library_check <disc.cue> <library folder>: every CD.BIN entry of the disc against the library's form of it
 * (data/cdconv.c): the images and containers making the same bytes again, the text the same characters, the
 * rest the same bytes. Then counts what the library holds. */
#include "../src/data/cdbin.h"
#include "../src/data/cdconv.h"
#include "../src/data/disc.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

static const char *sLibrary;
static int sImages, sTexts, sFiles;

static uint8_t *read_library(void *ctx, const char *path, size_t *n) {
    char full[1400];
    FILE *f;
    long len;
    uint8_t *d;
    const char *dot = strrchr(path, '.');

    (void)ctx;
    snprintf(full, sizeof(full), "%s/%s", sLibrary, path);
    f = fopen(full, "rb");
    if (f == NULL) {
        return NULL;
    }
    fseek(f, 0, SEEK_END);
    len = ftell(f);
    fseek(f, 0, SEEK_SET);
    d = malloc((size_t)len + 1);
    *n = fread(d, 1, (size_t)len, f);
    d[*n] = 0;
    fclose(f);
    sFiles++;
    if (dot != NULL && strcmp(dot, ".png") == 0 && strstr(path, ".clut.") == NULL) {
        sImages++;
    }
    if (dot != NULL && strcmp(dot, ".txt") == 0 && strstr(path, "index.txt") == NULL && strstr(path, ".tim.") == NULL) {
        sTexts++;
    }
    return d;
}

int main(int argc, char **argv) {
    char why[1024];
    Disc *d = argc == 3 ? disc_open(argv[1], why, sizeof(why)) : NULL;
    size_t exe_n = 0, cd_n = 0;
    uint8_t *exe, *cd;
    Cdbin t;
    int i, bad = 0, checked = 0;

    if (d == NULL) {
        fprintf(stderr, "library_check: %s\n", argc == 3 ? why : "usage: library_check <disc.cue> <library folder>");
        return 1;
    }
    sLibrary = argv[2];
    exe = disc_read_all(d, "SLPS017.83", &exe_n);
    cd = disc_read_all(d, "CD.BIN", &cd_n);
    if (exe == NULL || cd == NULL || !cdbin_table(&t, exe, exe_n, cd_n)) {
        fprintf(stderr, "library_check: no CD.BIN table\n");
        return 1;
    }
    for (i = 0; i < t.n; i++) {
        size_t n = 0;
        uint8_t *entry = cdbin_entry(&t, cd, cd_n, i, &n);
        char name[16];

        if (entry == NULL) {
            continue;
        }
        snprintf(name, sizeof(name), "%03d_%d", i / CDBIN_KINDS, i % CDBIN_KINDS);
        if (!cdconv_check(name, entry, n, read_library, NULL, why, sizeof(why))) {
            printf("%s: %s\n", name, why);
            bad++;
        }
        checked++;
        free(entry);
    }
    printf("%d entries checked, %d differ; %d files read: %d images, %d text files\n", checked, bad, sFiles, sImages, sTexts);
    return bad != 0;
}
