/* The game library (library.h, docs/library.md).
 *
 * The import reads the disc image (data/disc.c), checks it is the English Mizzurna Falls (its executable's
 * CRC), and writes library.new/: CD.BIN's entries unpacked and converted (cdbin/, data/cdconv.c: containers as
 * folders, images as PNG, text as UTF-8), the other files as they are, and manifest.txt. When all is written it takes the place of library/. Formats are converted
 * kind by kind as the subsystems need them (the manifest's "convert" lines say which); until then a kind is
 * kept as the disc has it. */
#include "library.h"

#include "../data/cdbin.h"
#include "../data/cdconv.h"
#include "../data/disc.h"
#include "../data/exetables.h"
#include "../data/flacenc.h"
#include "../data/modelconv.h"
#include "../data/xa.h"
#include "../render/render.h"
#include "platform.h"

#include <SDL3/SDL.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define LIBRARY_FORMAT "openmizzurna-library 1"
#define ENGLISH_EXE_CRC 0x6FAD64B1u   /* SLPS_017.83 with the 2021 English translation */
#define MOVE_ASIDE_SUFFIX ".old"

/* ---- where ---- */
static char sHome[1024], sLibrary[1100];

const char *library_home(void) {
    if (sHome[0] == 0) {
        const char *env = getenv("OM_HOME"), *base = SDL_GetBasePath();

        if (env != NULL) {
            snprintf(sHome, sizeof(sHome), "%s%s", env, env[0] && env[strlen(env) - 1] == '/' ? "" : "/");
        } else {
            snprintf(sHome, sizeof(sHome), "%s", base != NULL ? base : "./");
        }
    }
    return sHome;
}

const char *library_dir(void) {
    snprintf(sLibrary, sizeof(sLibrary), "%slibrary/", library_home());
    return sLibrary;
}

/* ---- files ---- */
static uint32_t crc32_of(const uint8_t *p, size_t n) {
    static uint32_t table[256];
    uint32_t crc = 0xFFFFFFFFu;
    size_t i;

    if (table[1] == 0) {
        for (i = 0; i < 256; i++) {
            uint32_t c = (uint32_t)i;
            int k;

            for (k = 0; k < 8; k++) {
                c = c & 1 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
            }
            table[i] = c;
        }
    }
    for (i = 0; i < n; i++) {
        crc = table[(crc ^ p[i]) & 0xFF] ^ (crc >> 8);
    }
    return crc ^ 0xFFFFFFFFu;
}

static int make_parents(const char *path) {
    char dir[1100];
    char *slash;

    snprintf(dir, sizeof(dir), "%s", path);
    slash = strrchr(dir, '/');
    if (slash == NULL) {
        return 1;
    }
    *slash = 0;
    return SDL_CreateDirectory(dir);
}

static SDL_EnumerationResult remove_one(void *user, const char *dir, const char *name) {
    char path[1200];
    SDL_PathInfo info;

    (void)user;
    snprintf(path, sizeof(path), "%s%s", dir, name);
    if (SDL_GetPathInfo(path, &info) && info.type == SDL_PATHTYPE_DIRECTORY) {
        strcat(path, "/");
        SDL_EnumerateDirectory(path, remove_one, NULL);
    }
    SDL_RemovePath(path);
    return SDL_ENUM_CONTINUE;
}

static void remove_tree(const char *dir) {   /* (dir ends with '/') */
    SDL_EnumerateDirectory(dir, remove_one, NULL);
    SDL_RemovePath(dir);
}

/* ---- the import ---- */
typedef struct Import {
    char disc[1024];
    /* the progress, read by the window while the import thread writes it */
    SDL_AtomicInt done_bytes_k, total_bytes_k;   /* in KB */
    char current[300];
    SDL_AtomicInt finished;   /* 1 ok, -1 failed */
    SDL_AtomicInt cancel;
    char why[1500];
    FILE *manifest;
} Import;

static int write_file(Import *im, const char *root, const char *rel, const uint8_t *data, size_t size) {
    char path[1200];
    FILE *f;
    int ok;

    snprintf(path, sizeof(path), "%s%s", root, rel);
    if (!make_parents(path) || (f = fopen(path, "wb")) == NULL) {
        snprintf(im->why, sizeof(im->why), "can't write %s", path);
        return 0;
    }
    ok = fwrite(data, 1, size, f) == size;
    ok = fclose(f) == 0 && ok;
    if (!ok) {
        snprintf(im->why, sizeof(im->why), "can't write %s (the disc full?)", path);
        return 0;
    }
    fprintf(im->manifest, "file %zu %s\n", size, rel);
    SDL_AddAtomicInt(&im->done_bytes_k, (int)(size / 1024));
    return 1;
}

typedef struct ImportWrite {
    Import *im;
    const char *root;
} ImportWrite;

static int import_write(void *ctx, const char *path, const uint8_t *data, size_t n) {
    ImportWrite *w = ctx;
    return write_file(w->im, w->root, path, data, n);
}

/* an XA file's audio channels as FLACs (xa/<name>/<channel>.flac) and index.txt (data/xa.h) */
#define XA_AUDIO_FILE 1   /* the file number the game's XA audio sectors carry */
static int convert_xa(Import *im, const char *root, const char *name, const uint8_t *d, size_t n) {
    uint32_t mask = xa_channels(d, n, XA_AUDIO_FILE);
    char rel[128], index[4096], tag[64];
    int c, k = snprintf(index, sizeof(index), "\\ the audio channels of %s.XA\nom-xa 1\n", name);

    for (c = 0; c < 32; c++) {
        XaStream st;
        uint8_t *flac;
        size_t len;
        const char *tags[1];
        int ok;

        if (!(mask & 1u << c) || !xa_decode(&st, d, n, XA_AUDIO_FILE, c)) {
            continue;
        }
        snprintf(tag, sizeof(tag), "XACHANNEL=%d", c);
        tags[0] = tag;
        flac = flacenc(st.pcm, st.frames, st.channels, st.rate, tags, 1, &len);
        snprintf(rel, sizeof(rel), "xa/%s/%d.flac", name, c);
        ok = flac != NULL && write_file(im, root, rel, flac, len);
        k += snprintf(index + k, sizeof(index) - (size_t)k, "channel %d rate %d channels %d frames %ld sectors %d\n", c,
                      st.rate, st.channels, st.frames, st.sectors);
        free(flac);
        xa_free(&st);
        if (!ok) {
            return 0;
        }
    }
    snprintf(rel, sizeof(rel), "xa/%s/index.txt", name);
    return write_file(im, root, rel, (const uint8_t *)index, (size_t)k);
}

/* the executable SYSTEM.CNF boots ("BOOT = cdrom:\SLPS017.83;1" -> "SLPS017.83") */
static int boot_name(Disc *d, char *name, size_t n) {
    size_t len = 0;
    uint8_t *cnf = disc_read_all(d, "SYSTEM.CNF", &len);
    char *at, *end;

    if (cnf == NULL) {
        return 0;
    }
    at = strstr((char *)cnf, "cdrom:");
    if (at == NULL) {
        free(cnf);
        return 0;
    }
    at += 6;
    while (*at == '\\' || *at == '/') {
        at++;
    }
    end = at;
    while (*end && *end != ';' && *end != '\r' && *end != '\n') {
        end++;
    }
    snprintf(name, n, "%.*s", (int)(end - at), at);
    free(cnf);
    return 1;
}

/* the disc's other files, kept as they are until their kinds are converted: where each goes */
static const char *kept_as(const char *name) {
    const char *dot = strrchr(name, '.');

    if (dot != NULL && SDL_strcasecmp(dot, ".STR") == 0) {
        return "movies/";
    }
    return "disc/";
}

static int import_disc(Import *im) {
    char root[1200], final[1200], aside[1200], manifest[1300], exe_name[64], why[256];
    Disc *d = disc_open(im->disc, why, sizeof(why));
    size_t exe_n = 0, cd_n = 0;
    uint8_t *exe = NULL, *cd = NULL;
    Cdbin table;
    int i, ok = 0;
    long long total = 0;

    if (d == NULL) {
        snprintf(im->why, sizeof(im->why), "%s", why);
        return 0;
    }
    if (!boot_name(d, exe_name, sizeof(exe_name)) || (exe = disc_read_all(d, exe_name, &exe_n)) == NULL) {
        snprintf(im->why, sizeof(im->why), "not a PlayStation game disc (no SYSTEM.CNF naming its program)");
        goto out;
    }
    if (crc32_of(exe, exe_n) != ENGLISH_EXE_CRC) {
        snprintf(im->why, sizeof(im->why), strncmp(exe_name, "SLPS", 4) == 0 && strstr(exe_name, "017") != NULL
                 ? "this is Mizzurna Falls, but not the English version (the 2021 translation): only that is supported"
                 : "this isn't Mizzurna Falls (its program is %s)", exe_name);
        goto out;
    }
    cd = disc_read_all(d, "CD.BIN", &cd_n);
    if (cd == NULL || !cdbin_table(&table, exe, exe_n, cd_n)) {
        snprintf(im->why, sizeof(im->why), "the disc's CD.BIN is missing or damaged");
        goto out;
    }
    for (i = 0; i < disc_count(d); i++) {
        total += (long long)disc_file_bytes(disc_entry(d, i));
    }
    SDL_SetAtomicInt(&im->total_bytes_k, (int)(total / 1024));

    snprintf(final, sizeof(final), "%s", library_dir());
    snprintf(root, sizeof(root), "%.*s.new/", (int)strlen(final) - 1, final);
    snprintf(aside, sizeof(aside), "%.*s" MOVE_ASIDE_SUFFIX "/", (int)strlen(final) - 1, final);
    remove_tree(root);
    snprintf(manifest, sizeof(manifest), "%smanifest.txt", root);
    if (!SDL_CreateDirectory(root) || (im->manifest = fopen(manifest, "w")) == NULL) {
        snprintf(im->why, sizeof(im->why), "can't write the library in %s", root);
        goto out;
    }
    fprintf(im->manifest, "%s\ndisc %08X %s\nconvert cdbin folders 1\nconvert images png 1\nconvert text utf8 1\nconvert exe-tables files 1\nconvert banks text-wav 1\nconvert sequences midi 1\nconvert xa flac 1\nconvert models gltf 1\nconvert textures png 1\n",
            LIBRARY_FORMAT, crc32_of(exe, exe_n), exe_name);

    /* the executable: kept until the tables the game reads from it are files of their own */
    snprintf(im->current, sizeof(im->current), "%s", exe_name);
    if (!write_file(im, root, "exe/SLPS_017.83", exe, exe_n)) {
        goto out;
    }
    /* the tables named so far (data/exetables.c), a file each */
    for (i = 0; i < exetable_count(); i++) {
        const ExeTable *t = exetable_at(i);
        const uint8_t *bytes = exetable_bytes(t, exe, exe_n);
        char rel[128];

        snprintf(rel, sizeof(rel), "exe/%s.bin", t->name);
        if (bytes == NULL || !write_file(im, root, rel, bytes, t->size)) {
            goto out;
        }
    }
    /* CD.BIN's entries: unpacked, the sound banks' bodies paired with their headers, converted */
    {
        uint8_t **entries = calloc((size_t)table.n, sizeof(uint8_t *));
        size_t *sizes = calloc((size_t)table.n, sizeof(size_t));
        const uint8_t **header_of = calloc((size_t)table.n, sizeof(uint8_t *));
        int failed = 0;

        for (i = 0; i < table.n; i++) {
            entries[i] = cdbin_entry(&table, cd, cd_n, i, &sizes[i]);
            if (entries[i] == NULL && table.entries[i].size != 0) {
                snprintf(im->why, sizeof(im->why), "CD.BIN's entry %d is damaged", i);
                failed = 1;
            }
        }
        if (!failed) {
            cdconv_pair_bodies((const uint8_t *const *)entries, sizes, table.n, CDBIN_KINDS, header_of);
        }
        for (i = 0; !failed && i < table.n && !SDL_GetAtomicInt(&im->cancel); i++) {
            char rel[64], cwhy[512] = "";
            ImportWrite w = {im, root};
            int ok;

            if (entries[i] == NULL) {
                continue;
            }
            snprintf(rel, sizeof(rel), "%03d_%d", i / CDBIN_KINDS, i % CDBIN_KINDS);
            snprintf(im->current, sizeof(im->current), "CD.BIN %s", rel);
            ok = header_of[i] != NULL ? cdconv_body(rel, entries[i], sizes[i], header_of[i], import_write, &w, cwhy, sizeof(cwhy))
                                      : cdconv_entry(rel, entries[i], sizes[i], import_write, &w, cwhy, sizeof(cwhy));
            if (!ok) {
                if (cwhy[0]) {
                    snprintf(im->why, sizeof(im->why), "%s", cwhy);
                }
                failed = 1;
            }
        }
        if (!failed) {   /* the models, and the textures they're drawn with */
            ImportWrite w = {im, root};
            char mwhy[512] = "";

            snprintf(im->current, sizeof(im->current), "the models");
            if (!modelconv_write((const uint8_t *const *)entries, sizes, table.n, exe, exe_n, import_write, &w, mwhy, sizeof(mwhy))) {
                snprintf(im->why, sizeof(im->why), "%s", mwhy);
                failed = 1;
            }
        }
        for (i = 0; i < table.n; i++) {
            free(entries[i]);
        }
        free(entries);
        free(sizes);
        free(header_of);
        if (failed) {
            goto out;
        }
    }
    /* the rest of the disc */
    for (i = 0; i < disc_count(d) && !SDL_GetAtomicInt(&im->cancel); i++) {
        const DiscEntry *e = disc_entry(d, i);
        char rel[300];
        uint8_t *data;

        if (SDL_strcasecmp(e->path, exe_name) == 0 || SDL_strcasecmp(e->path, "CD.BIN") == 0 ||
            SDL_strcasecmp(e->path, "SYSTEM.CNF") == 0) {
            continue;
        }
        snprintf(im->current, sizeof(im->current), "%s", e->path);
        data = malloc(disc_file_bytes(e));
        if (data == NULL || !disc_read(d, e, data)) {
            free(data);
            snprintf(im->why, sizeof(im->why), "can't read %s from the disc", e->path);
            goto out;
        }
        if (SDL_strcasecmp(e->path, "TSG.XA") == 0) {
            ok = convert_xa(im, root, "TSG", data, disc_file_bytes(e));
        } else {
            snprintf(rel, sizeof(rel), "%s%s", kept_as(e->path), e->path);
            ok = write_file(im, root, rel, data, disc_file_bytes(e));
        }
        free(data);
        if (!ok) {
            goto out;
        }
        ok = 0;
    }
    if (SDL_GetAtomicInt(&im->cancel)) {
        snprintf(im->why, sizeof(im->why), "the import was cancelled");
        goto out;
    }
    fclose(im->manifest);
    im->manifest = NULL;
    /* the new library takes the old one's place */
    remove_tree(aside);
    if (SDL_GetPathInfo(final, NULL) && !SDL_RenamePath(final, aside)) {
        snprintf(im->why, sizeof(im->why), "can't move the old library aside (%s)", SDL_GetError());
        goto out;
    }
    if (!SDL_RenamePath(root, final)) {
        snprintf(im->why, sizeof(im->why), "can't put the library in place (%s)", SDL_GetError());
        goto out;
    }
    remove_tree(aside);
    ok = 1;
out:
    if (im->manifest != NULL) {
        fclose(im->manifest);
        im->manifest = NULL;
    }
    if (!ok) {
        remove_tree(root);
    }
    free(exe);
    free(cd);
    disc_close(d);
    return ok;
}

static int SDLCALL import_thread(void *data) {
    Import *im = data;
    SDL_SetAtomicInt(&im->finished, import_disc(im) ? 1 : -1);
    return 0;
}

/* ---- the library at start-up ---- */

/* whole: the manifest's format line, and every file it lists there at its size */
static int library_ok(char *why, size_t n) {
    char path[1200], line[1200];
    FILE *f;
    int first = 1, ok = 1;

    snprintf(path, sizeof(path), "%smanifest.txt", library_dir());
    f = fopen(path, "r");
    if (f == NULL) {
        snprintf(why, n, "no library yet");
        return 0;
    }
    while (ok && fgets(line, sizeof(line), f) != NULL) {
        long long size;
        char rel[1000], file[1200];
        SDL_PathInfo info;

        line[strcspn(line, "\r\n")] = 0;
        if (first) {
            first = 0;
            if (strcmp(line, LIBRARY_FORMAT) != 0) {
                snprintf(why, n, "the library was made by another version (%s)", line);
                ok = 0;
            }
        } else if (sscanf(line, "file %lld %999[^\n]", &size, rel) == 2) {
            snprintf(file, sizeof(file), "%s%s", library_dir(), rel);
            if (!SDL_GetPathInfo(file, &info) || (long long)info.size != size) {
                snprintf(why, n, "the library's %s is missing or damaged", rel);
                ok = 0;
            }
        }
    }
    fclose(f);
    return ok;
}

int library_import_from(const char *disc) {
    Import *im = calloc(1, sizeof(*im));
    int ok;

    snprintf(im->disc, sizeof(im->disc), "%s", disc);
    fprintf(stderr, "openmizzurna: importing %s into %s\n", disc, library_dir());
    ok = import_disc(im);
    if (!ok) {
        fprintf(stderr, "openmizzurna: the import failed: %s\n", im->why);
    } else {
        fprintf(stderr, "openmizzurna: the library is ready\n");
    }
    free(im);
    return ok;
}

/* the screen while waiting: a title, lines of text, and a bar (fraction < 0: none) */
static void screen(const char *title, const char *line1, const char *line2, float fraction) {
    int w, h;
    float s = 3.0f, x, y;

    platform_size(&w, &h);
    render_begin(w, h, 0.02f, 0.02f, 0.04f);
    x = 60.0f;
    y = (float)h * 0.35f;
    render_text(x, y, s, 0xE8E8F0FF, title, (int)strlen(title));
    y += render_line_height(s) * 2;
    render_text(x, y, 2.0f, 0xB0B0C0FF, line1, (int)strlen(line1));
    y += render_line_height(2.0f) * 1.5f;
    render_text(x, y, 2.0f, 0x8888A0FF, line2, (int)strlen(line2));
    if (fraction >= 0.0f) {
        float bw = (float)w - 2 * x;
        y += render_line_height(2.0f) * 2;
        render_rect(x, y, bw, 16.0f, 0x303040FF);
        render_rect(x, y, bw * (fraction > 1.0f ? 1.0f : fraction), 16.0f, 0xC0C8E0FF);
    }
    render_end();
    platform_swap();
}

typedef struct Pick {
    SDL_AtomicInt state;   /* 0 waiting, 1 chosen, -1 cancelled */
    char path[1024];
} Pick;

static void SDLCALL picked(void *user, const char *const *files, int filter) {
    Pick *p = user;

    (void)filter;
    if (files != NULL && files[0] != NULL) {
        snprintf(p->path, sizeof(p->path), "%s", files[0]);
        SDL_SetAtomicInt(&p->state, 1);
    } else {
        SDL_SetAtomicInt(&p->state, -1);
    }
}

/* the player picks their disc (the dialog), then it is imported with the progress shown: 1 done */
static int pick_and_import(const char *reason) {
    static const SDL_DialogFileFilter kFilters[] = {
        {"Mizzurna Falls disc image (.cue / .bin)", "cue;bin"},
        {"All files", "*"},
    };
    char said[1500];
    Input in;

    snprintf(said, sizeof(said), "%s", reason);
    for (;;) {
        Pick pick;
        Import *im;
        SDL_Thread *t;
        int result;

        memset(&pick, 0, sizeof(pick));
        SDL_ShowOpenFileDialog(picked, &pick, platform_window(), kFilters, 2, NULL, false);
        while (SDL_GetAtomicInt(&pick.state) == 0) {
            platform_poll(&in);
            if (in.quit) {
                return 0;
            }
            screen("Choose your Mizzurna Falls disc", said, "(a .cue / .bin image of the English version)", -1.0f);
            SDL_Delay(16);
        }
        if (SDL_GetAtomicInt(&pick.state) < 0) {
            return 0;
        }
        im = calloc(1, sizeof(*im));
        snprintf(im->disc, sizeof(im->disc), "%s", pick.path);
        t = SDL_CreateThread(import_thread, "import", im);
        while (SDL_GetAtomicInt(&im->finished) == 0) {
            int total = SDL_GetAtomicInt(&im->total_bytes_k);
            char line[300];

            platform_poll(&in);
            if (in.quit) {
                SDL_SetAtomicInt(&im->cancel, 1);
            }
            snprintf(line, sizeof(line), "%s", im->current);
            screen("Importing the disc", line, "(closing the window cancels)",
                   total > 0 ? (float)SDL_GetAtomicInt(&im->done_bytes_k) / (float)total : 0.0f);
            SDL_Delay(16);
        }
        SDL_WaitThread(t, NULL);
        result = SDL_GetAtomicInt(&im->finished);
        snprintf(said, sizeof(said), "%s", im->why);
        free(im);
        if (result > 0) {
            return 1;
        }
        if (in.quit) {
            return 0;
        }
    }
}

int library_choose(int headless) {
    char why[256];

    if (library_ok(why, sizeof(why))) {
        return 1;
    }
    if (headless) {
        fprintf(stderr, "openmizzurna: %s - import the disc first (openmizzurna --import <disc.cue>)\n", why);
        return 0;
    }
    if (!pick_and_import(why)) {
        fprintf(stderr, "openmizzurna: no library (%s)\n", why);
        return 0;
    }
    return library_ok(why, sizeof(why));
}
