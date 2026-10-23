/* The game's files (files.h). */
#include "files.h"

#include "../platform/library.h"

#include <stdio.h>
#include <stdlib.h>

static FILE *open_in_library(const char *path) {
    char full[1400];

    snprintf(full, sizeof(full), "%s%s", library_dir(), path);
    return fopen(full, "rb");
}

uint8_t *files_read(const char *path, size_t *size) {
    FILE *f = open_in_library(path);
    long n;
    uint8_t *data;

    if (f == NULL) {
        return NULL;
    }
    fseek(f, 0, SEEK_END);
    n = ftell(f);
    fseek(f, 0, SEEK_SET);
    data = malloc((size_t)n + 1);
    if (data == NULL || fread(data, 1, (size_t)n, f) != (size_t)n) {
        free(data);
        fclose(f);
        return NULL;
    }
    fclose(f);
    data[n] = 0;
    *size = (size_t)n;
    return data;
}

int files_exist(const char *path) {
    FILE *f = open_in_library(path);

    if (f == NULL) {
        return 0;
    }
    fclose(f);
    return 1;
}
