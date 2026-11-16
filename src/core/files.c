/* The game's files (files.h): the layers over the library (the mods'), then the library. */
#include "files.h"

#include "../platform/library.h"
#include "zip.h"

#include <stdio.h>
#include <stdlib.h>

#define LAYERS 64   /* (as many as the mods: mods.c's MODS) */

typedef struct Layer {
    char dir[1024];    /* a folder's library/, ending in "/"; in a zip, the prefix of its library/ */
    const Zip *zip;    /* NULL: a folder */
} Layer;

static Layer sLayers[LAYERS];
static int sNLayers;

void files_layer_add(const char *dir) {
    if (sNLayers < LAYERS) {
        snprintf(sLayers[sNLayers].dir, sizeof(sLayers[0].dir), "%s/", dir);
        sLayers[sNLayers++].zip = NULL;
    }
}

void files_layer_add_zip(const Zip *z, const char *prefix) {
    if (sNLayers < LAYERS) {
        snprintf(sLayers[sNLayers].dir, sizeof(sLayers[0].dir), "%slibrary/", prefix);
        sLayers[sNLayers++].zip = z;
    }
}

void files_layers_clear(void) { sNLayers = 0; }

static uint8_t *read_whole(FILE *f, size_t *size) {
    long n;
    uint8_t *data;

    fseek(f, 0, SEEK_END);
    n = ftell(f);
    fseek(f, 0, SEEK_SET);
    data = n >= 0 ? malloc((size_t)n + 1) : NULL;
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

static FILE *open_in(const char *dir, const char *path) {
    char full[1400];

    snprintf(full, sizeof(full), "%s%s", dir, path);
    return fopen(full, "rb");
}

/* the layer's file: its index in the zip (>= 0), or 1 for a folder's (its FILE in *f); -1 none */
static int find_in_layer(const Layer *l, const char *path, FILE **f) {
    char name[1400];

    if (l->zip != NULL) {
        snprintf(name, sizeof(name), "%s%s", l->dir, path);
        return zip_find(l->zip, name);
    }
    *f = open_in(l->dir, path);
    return *f != NULL ? 1 : -1;
}

uint8_t *files_read(const char *path, size_t *size) {
    FILE *f = NULL;
    int i, k;

    for (i = sNLayers - 1; i >= 0; i--) {
        if ((k = find_in_layer(&sLayers[i], path, &f)) >= 0) {
            return sLayers[i].zip != NULL ? zip_read(sLayers[i].zip, k, size) : read_whole(f, size);
        }
    }
    f = open_in(library_dir(), path);
    return f != NULL ? read_whole(f, size) : NULL;
}

int files_exist(const char *path) {   /* (a zipped mod's file: found in its directory, not unpacked) */
    FILE *f = NULL;
    int i;

    for (i = sNLayers - 1; i >= 0; i--) {
        if (find_in_layer(&sLayers[i], path, &f) >= 0) {
            if (f != NULL) {
                fclose(f);
            }
            return 1;
        }
    }
    f = open_in(library_dir(), path);
    if (f == NULL) {
        return 0;
    }
    fclose(f);
    return 1;
}
