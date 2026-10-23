/* library_check <disc.cue> <library folder>: every CD.BIN entry of the disc against the library's form of it
 * (data/cdconv.c): the images and containers making the same bytes again, the text the same characters, the
 * rest the same bytes. Then counts what the library holds. */
#include "../src/data/cdbin.h"
#include "../src/data/cdconv.h"
#include "../src/data/disc.h"
#include "../src/data/modelconv.h"
#include "../src/data/xa.h"
#include "../src/third_party/dr_flac.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

static const char *sLibrary;
static int sImages, sTexts, sFiles, sBodies, sXa, sModels, sTextures;

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
    {
        uint8_t **entries = calloc((size_t)t.n, sizeof(uint8_t *));
        size_t *sizes = calloc((size_t)t.n, sizeof(size_t));
        const uint8_t **header_of = calloc((size_t)t.n, sizeof(uint8_t *));

        for (i = 0; i < t.n; i++) {
            entries[i] = cdbin_entry(&t, cd, cd_n, i, &sizes[i]);
        }
        cdconv_pair_bodies((const uint8_t *const *)entries, sizes, t.n, CDBIN_KINDS, header_of);
        for (i = 0; i < t.n; i++) {
            char name[16];
            int ok;

            if (entries[i] == NULL) {
                continue;
            }
            snprintf(name, sizeof(name), "%03d_%d", i / CDBIN_KINDS, i % CDBIN_KINDS);
            ok = header_of[i] != NULL ? cdconv_check_body(name, entries[i], sizes[i], header_of[i], read_library, NULL, why, sizeof(why))
                                      : cdconv_check(name, entries[i], sizes[i], read_library, NULL, why, sizeof(why));
            if (!ok) {
                printf("%s: %s\n", name, why);
                bad++;
            }
            if (header_of[i] != NULL) {
                sBodies++;
            }
            checked++;
        }
        {
            int models = 0, textures = 0;
            if (!modelconv_check((const uint8_t *const *)entries, sizes, t.n, exe, exe_n, read_library, NULL, &models,
                                 &textures, why, sizeof(why))) {
                printf("models: %s\n", why);
                bad++;
            }
            sModels = models;
            sTextures = textures;
        }
        for (i = 0; i < t.n; i++) {
            free(entries[i]);
        }
        free(entries);
        free(sizes);
        free(header_of);
    }
    /* the XA music: every channel's FLAC the same samples as the XA decoded */
    {
        size_t xa_n = 0;
        uint8_t *xa = disc_read_all(d, "TSG.XA", &xa_n);
        uint32_t mask = xa != NULL ? xa_channels(xa, xa_n, 1) : 0;
        int c;

        for (c = 0; c < 32; c++) {
            XaStream st;
            char path[64];
            size_t fn;
            uint8_t *flac;
            unsigned int channels, rate;
            drflac_uint64 frames;
            drflac_int16 *pcm;

            if (!(mask & 1u << c) || !xa_decode(&st, xa, xa_n, 1, c)) {
                continue;
            }
            snprintf(path, sizeof(path), "xa/TSG/%d.flac", c);
            flac = read_library(NULL, path, &fn);
            pcm = flac != NULL ? drflac_open_memory_and_read_pcm_frames_s16(flac, fn, &channels, &rate, &frames, NULL) : NULL;
            if (pcm == NULL || (long)frames != st.frames || (int)channels != st.channels || (int)rate != st.rate ||
                memcmp(pcm, st.pcm, sizeof(int16_t) * (size_t)st.frames * channels) != 0) {
                printf("%s: not the same sound as TSG.XA's channel %d\n", path, c);
                bad++;
            }
            sXa++;
            drflac_free(pcm, NULL);
            free(flac);
            xa_free(&st);
        }
        free(xa);
    }
    printf("%d entries checked, %d differ; %d files read: %d images, %d text files, %d sound bank bodies, %d XA tracks, %d models, %d model textures\n",
           checked, bad, sFiles, sImages, sTexts, sBodies, sXa, sModels, sTextures);
    return bad != 0;
}
