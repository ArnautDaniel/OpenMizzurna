/* cdbin_check <disc.cue> <folder of NNN_K.bin>: every CD.BIN entry unpacked by src/data/cdbin.c compared with
 * a folder of the same entries unpacked by another tool (the decomp's tools/cdbin.py) */
#include "../src/data/cdbin.h"
#include "../src/data/disc.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char **argv) {
    char why[256];
    Disc *d = argc == 3 ? disc_open(argv[1], why, sizeof(why)) : NULL;
    size_t exe_n = 0, cd_n = 0;
    uint8_t *exe, *cd;
    Cdbin t;
    int i, same = 0, bad = 0;

    if (d == NULL) {
        fprintf(stderr, "cdbin_check: %s\n", argc == 3 ? why : "usage: cdbin_check <disc.cue> <folder>");
        return 1;
    }
    exe = disc_read_all(d, "SLPS017.83", &exe_n);
    cd = disc_read_all(d, "CD.BIN", &cd_n);
    if (exe == NULL || cd == NULL || !cdbin_table(&t, exe, exe_n, cd_n)) {
        fprintf(stderr, "cdbin_check: no CD.BIN table\n");
        return 1;
    }
    for (i = 0; i < t.n; i++) {
        size_t n = 0, m = 0;
        uint8_t *mine = cdbin_entry(&t, cd, cd_n, i, &n), *theirs;
        char path[1024];
        FILE *f;

        snprintf(path, sizeof(path), "%s/%03d_%d.bin", argv[2], i / CDBIN_KINDS, i % CDBIN_KINDS);
        f = fopen(path, "rb");
        if (f == NULL) {
            if (mine != NULL) {
                printf("%03d_%d: %zu bytes here, none there\n", i / CDBIN_KINDS, i % CDBIN_KINDS, n);
                bad++;
            }
            free(mine);
            continue;
        }
        fseek(f, 0, SEEK_END);
        m = (size_t)ftell(f);
        fseek(f, 0, SEEK_SET);
        theirs = malloc(m + 1);
        m = fread(theirs, 1, m, f);
        fclose(f);
        if (mine == NULL || n != m || memcmp(mine, theirs, n) != 0) {
            printf("%03d_%d: differs (%zu here, %zu there)\n", i / CDBIN_KINDS, i % CDBIN_KINDS, n, m);
            bad++;
        } else {
            same++;
        }
        free(mine);
        free(theirs);
    }
    printf("%d entries in the table: %d the same, %d different\n", t.n, same, bad);
    return bad != 0;
}
