/* disc_ls <disc.cue> [folder]: the disc's files (raw: read as whole sectors); with a folder of the same files
 * extracted, each compared byte for byte (the developers' check of src/data/disc.c) */
#include "../src/data/disc.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char **argv) {
    char why[256];
    Disc *d;
    int i, bad = 0;

    if (argc < 2 || (d = disc_open(argv[1], why, sizeof(why))) == NULL) {
        fprintf(stderr, "disc_ls: %s\n", argc < 2 ? "usage: disc_ls <disc.cue> [folder]" : why);
        return 1;
    }
    for (i = 0; i < disc_count(d); i++) {
        const DiscEntry *e = disc_entry(d, i);
        uint64_t n = disc_file_bytes(e);
        printf("%-28s lba %6u  %10llu bytes%s", e->path, e->lba, (unsigned long long)n, e->raw ? "  (raw sectors)" : "");
        if (argc > 2) {
            char path[1024];
            FILE *f;
            uint8_t *mine = malloc(n), *theirs = malloc(n + 1);
            const char *name = strrchr(e->path, '/') ? strrchr(e->path, '/') + 1 : e->path;
            size_t got = 0;

            snprintf(path, sizeof(path), "%s/%s", argv[2], name);
            f = fopen(path, "rb");
            if (f != NULL) {
                got = fread(theirs, 1, n + 1, f);
                fclose(f);
            }
            if (f == NULL) {
                printf("  (not in the folder)");
            } else if (!disc_read(d, e, mine) || got != n || memcmp(mine, theirs, n) != 0) {
                printf("  DIFFERS (%zu bytes there)", got);
                bad = 1;
            } else {
                printf("  same");
            }
            free(mine);
            free(theirs);
        }
        printf("\n");
    }
    disc_close(d);
    return bad;
}
