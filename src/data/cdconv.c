/* CD.BIN's entries in the library (cdconv.h). */
#include "cdconv.h"

#include "container.h"
#include "seqmidi.h"
#include "vab.h"
#include "text.h"
#include "tim.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define INDEX_FORMAT "om-container 1"
#define MODEL_MAGIC 0x88FA   /* a model (SPL: its parts, then a TMD) starts with this u16 */

typedef enum { PART_EMPTY, PART_FOLDER, PART_IMAGE, PART_TEXT, PART_BANK, PART_SEQUENCES, PART_RAW } PartKind;
static const char *kKindNames[] = {"empty", "folder", "image", "text", "bank", "sequences", "raw"};

static PartKind kind_of(const uint8_t *d, size_t n) {
    Tim t;

    if (n == 0) {
        return PART_EMPTY;
    }
    if (container_is(d, n)) {
        return PART_FOLDER;
    }
    if (tim_parse(&t, d, n) && t.size == n) {
        return PART_IMAGE;
    }
    if (n >= 8 && (memcmp(d, "MDD", 4) == 0 || memcmp(d, "MDT", 4) == 0)) {
        return PART_TEXT;
    }
    if (vab_header_size(d, n) == n) {
        return PART_BANK;
    }
    if (sep_sequences(d, n) > 0) {
        return PART_SEQUENCES;
    }
    return PART_RAW;
}

/* a raw part's file extension, by what it starts with */
static const char *raw_extension(const uint8_t *d, size_t n) {
    if (n >= 4 && memcmp(d, "pBAV", 4) == 0) {
        return "vab";
    }
    if (n >= 4 && memcmp(d, "pQES", 4) == 0) {
        return "sep";
    }
    if (n >= 8 && memcmp(d, "PS-X EXE", 8) == 0) {
        return "exe";
    }
    if (n >= 2 && (d[0] | d[1] << 8) == MODEL_MAGIC) {
        return "spl";
    }
    return "bin";
}

static int write_text(const char *path, const uint8_t *block, size_t n, const char *comment, CdconvWriteFn write,
                      void *ctx, char *why, size_t nwhy) {
    TextBlock b;
    char *s;
    size_t len;
    int ok;

    if (!text_decode(&b, block, n)) {
        snprintf(why, nwhy, "%s: a damaged text block", path);
        return 0;
    }
    s = text_to_utf8(&b, comment, &len);
    ok = write(ctx, path, (const uint8_t *)s, len);
    free(s);
    text_free(&b);
    return ok;
}

/* part `base` (a path without extension); its file name as index.txt names it in *file */
static int convert_part(const char *base, const char *what, const uint8_t *d, size_t n, CdconvWriteFn write, void *ctx,
                        char *file, size_t nfile, char *why, size_t nwhy) {
    const char *slash = strrchr(base, '/'), *leaf = slash != NULL ? slash + 1 : base;
    char path[600];

    switch (kind_of(d, n)) {
    case PART_EMPTY:
        snprintf(file, nfile, "-");
        return 1;
    case PART_FOLDER: {
        Container c;
        char index[16384], sub[600], subfile[64];
        int i, k;

        container_parse(&c, d, n);
        k = snprintf(index, sizeof(index), "%s\nparts %d\ntail %zu\n", INDEX_FORMAT, c.n, c.tail);
        for (i = 0; i < c.n; i++) {
            char sub_what[300];

            snprintf(sub, sizeof(sub), "%s/%d", base, i);
            snprintf(sub_what, sizeof(sub_what), "%s, part %d", what, i);
            if (!convert_part(sub, sub_what, c.parts[i].data, c.parts[i].size, write, ctx, subfile, sizeof(subfile), why, nwhy)) {
                return 0;
            }
            k += snprintf(index + k, sizeof(index) - (size_t)k, "part %d %s %s\n", i,
                          kKindNames[kind_of(c.parts[i].data, c.parts[i].size)], subfile);
        }
        snprintf(path, sizeof(path), "%s/index.txt", base);
        snprintf(file, nfile, "%s/", leaf);
        return write(ctx, path, (const uint8_t *)index, (size_t)k);
    }
    case PART_IMAGE: {
        Tim t;

        tim_parse(&t, d, n);
        snprintf(file, nfile, "%s.png", leaf);
        if (!tim_convert(&t, base, write, ctx)) {
            snprintf(why, nwhy, "%s: can't write the image", base);
            return 0;
        }
        return 1;
    }
    case PART_TEXT:
        snprintf(path, sizeof(path), "%s.txt", base);
        snprintf(file, nfile, "%s.txt", leaf);
        return write_text(path, d, n, what, write, ctx, why, nwhy);
    case PART_BANK: {
        size_t len;
        char *text = vab_header_text(d, n, what, &len);
        int ok;

        snprintf(path, sizeof(path), "%s.bank.txt", base);
        snprintf(file, nfile, "%s.bank.txt", leaf);
        ok = text != NULL && write(ctx, path, (const uint8_t *)text, len);
        free(text);
        return ok;
    }
    case PART_SEQUENCES: {
        int i, count = sep_sequences(d, n), ok;
        size_t len;
        char *index = sep_index_text(d, n, what, &len);

        snprintf(path, sizeof(path), "%s.sep/index.txt", base);
        snprintf(file, nfile, "%s.sep/", leaf);
        ok = write(ctx, path, (const uint8_t *)index, len);
        free(index);
        for (i = 0; ok && i < count; i++) {
            uint8_t *mid = sep_to_midi(d, n, i, &len);

            snprintf(path, sizeof(path), "%s.sep/%d.mid", base, i);
            ok = mid != NULL && write(ctx, path, mid, len);
            free(mid);
        }
        return ok;
    }
    case PART_RAW:
        snprintf(path, sizeof(path), "%s.%s", base, raw_extension(d, n));
        snprintf(file, nfile, "%s.%s", leaf, raw_extension(d, n));
        return write(ctx, path, d, n);
    }
    return 0;
}

/* ---- sound banks' bodies ---- */
typedef struct Headers {
    const uint8_t *vh[256];
    int group[256];
    int n;
} Headers;

static void find_headers(Headers *h, const uint8_t *d, size_t n, int group) {
    Container c;
    int i;

    if (vab_header_size(d, n) == n && h->n < 256) {
        h->vh[h->n] = d;
        h->group[h->n++] = group;
    } else if (container_parse(&c, d, n)) {
        for (i = 0; i < c.n; i++) {
            find_headers(h, c.parts[i].data, c.parts[i].size, group);
        }
    }
}

void cdconv_pair_bodies(const uint8_t *const *entries, const size_t *sizes, int n, int kinds_per_group,
                        const uint8_t **header_of) {
    Headers *h = calloc(1, sizeof(*h));
    int i, k;

    for (i = 0; i < n; i++) {
        if (entries[i] != NULL) {
            find_headers(h, entries[i], sizes[i], i / kinds_per_group);
        }
    }
    for (i = 0; i < n; i++) {
        header_of[i] = NULL;
        if (entries[i] == NULL || kind_of(entries[i], sizes[i]) != PART_RAW) {
            continue;
        }
        for (k = 0; k < h->n; k++) {
            if (vab_body_size(h->vh[k]) == sizes[i] &&
                (header_of[i] == NULL || h->group[k] == i / kinds_per_group)) {
                header_of[i] = h->vh[k];
            }
        }
    }
    free(h);
}

int cdconv_body(const char *name, const uint8_t *vb, size_t n, const uint8_t *vh, CdconvWriteFn write, void *ctx,
                char *why, size_t nwhy) {
    char path[300], line[160];
    char *list = malloc(64 * 1024);
    size_t at = 0, len;
    int i, k, ok = 1;

    k = snprintf(list, 64 * 1024, "\\ the samples of a sound bank: CD.BIN entry %s\nom-samples 1\nrate %d\nsamples %d\n",
                 name, VAB_RATE, vab_sample_count(vh));
    for (i = 1; ok && i <= vab_sample_count(vh); i++) {
        size_t bytes = vab_sample_bytes(vh, i);
        long frames, loop;
        int16_t *pcm;
        uint8_t *wav;

        if (at + bytes > n) {
            snprintf(why, nwhy, "%s: sample %d runs past the body", name, i);
            ok = 0;
            break;
        }
        pcm = vab_decode_sample(vb + at, bytes, &frames, &loop);
        wav = vab_wav(pcm, frames, VAB_RATE, loop, &len);
        snprintf(path, sizeof(path), "cdbin/%s.samples/%d.wav", name, i);
        ok = write(ctx, path, wav, len);
        snprintf(line, sizeof(line), "sample %d bytes %zu frames %ld loop %ld\n", i, bytes, frames, loop);
        k += snprintf(list + k, 64 * 1024 - (size_t)k, "%s", line);
        free(pcm);
        free(wav);
        at += bytes;
    }
    if (ok) {
        snprintf(path, sizeof(path), "cdbin/%s.samples/samples.txt", name);
        ok = write(ctx, path, (const uint8_t *)list, (size_t)k);
    }
    free(list);
    return ok;
}

int cdconv_check_body(const char *name, const uint8_t *vb, size_t n, const uint8_t *vh, CdconvReadFn read, void *ctx,
                      char *why, size_t nwhy) {
    char path[300];
    size_t at = 0;
    int i;

    for (i = 1; i <= vab_sample_count(vh); i++) {
        size_t bytes = vab_sample_bytes(vh, i), len;
        long frames, loop, wframes, wloop;
        int16_t *pcm, *wpcm;
        uint8_t *wav;
        int same;

        if (at + bytes > n) {
            snprintf(why, nwhy, "%s: sample %d runs past the body", name, i);
            return 0;
        }
        pcm = vab_decode_sample(vb + at, bytes, &frames, &loop);
        snprintf(path, sizeof(path), "cdbin/%s.samples/%d.wav", name, i);
        wav = read(ctx, path, &len);
        wpcm = wav != NULL ? vab_wav_read(wav, len, &wframes, &wloop) : NULL;
        same = wpcm != NULL && wframes == frames && wloop == loop && memcmp(pcm, wpcm, sizeof(int16_t) * (size_t)frames) == 0;
        free(pcm);
        free(wpcm);
        free(wav);
        if (!same) {
            snprintf(why, nwhy, "%s doesn't hold the sample's sound", path);
            return 0;
        }
        at += bytes;
    }
    return 1;
}

static int pack_text(const uint8_t *block, size_t n) {
    TextBlock b;

    if (!text_decode(&b, block, n)) {
        return 0;
    }
    text_free(&b);
    return 1;
}

int cdconv_entry(const char *name, const uint8_t *d, size_t n, CdconvWriteFn write, void *ctx, char *why, size_t nwhy) {
    char base[300], what[300], file[64];
    long at;

    snprintf(base, sizeof(base), "cdbin/%s", name);
    snprintf(what, sizeof(what), "CD.BIN entry %s", name);
    if (!convert_part(base, what, d, n, write, ctx, file, sizeof(file), why, nwhy)) {
        return 0;
    }
    /* an event pack: its text too (a block that decodes: "MDD" may also turn up inside other data) */
    at = text_find_block(d, n);
    if (kind_of(d, n) == PART_RAW && at > 0 && pack_text(d + at, n - (size_t)at)) {
        char path[320], comment[400];

        snprintf(path, sizeof(path), "%s.txt", base);
        snprintf(comment, sizeof(comment), "%s, the text block at +0x%lX (an event pack's)", what, at);
        return write_text(path, d + at, n - (size_t)at, comment, write, ctx, why, nwhy);
    }
    return 1;
}

/* ---- the check ---- */
static int same_text(const char *path, const uint8_t *block, size_t n, CdconvReadFn read, void *ctx, char *why, size_t nwhy) {
    TextBlock a, b;
    size_t len;
    uint8_t *s = read(ctx, path, &len);
    char err[200];
    int i, ok;

    if (s == NULL) {
        snprintf(why, nwhy, "%s is missing", path);
        return 0;
    }
    if (!text_decode(&a, block, n) || !text_from_utf8(&b, (const char *)s, len, err, sizeof(err))) {
        snprintf(why, nwhy, "%s: %s", path, "doesn't read back");
        free(s);
        text_free(&a);
        return 0;
    }
    free(s);
    ok = a.n == b.n;
    for (i = 0; ok && i < a.n; i++) {
        ok = a.messages[i].n == b.messages[i].n &&
             memcmp(a.messages[i].codes, b.messages[i].codes, sizeof(uint16_t) * (size_t)a.messages[i].n) == 0;
        if (!ok) {
            snprintf(why, nwhy, "%s: message %d differs", path, i);
        }
    }
    if (ok == 0 && a.n != b.n) {
        snprintf(why, nwhy, "%s: %d messages, %d in the block", path, b.n, a.n);
    }
    text_free(&a);
    text_free(&b);
    return ok;
}

static int check_part(const char *base, const uint8_t *d, size_t n, CdconvReadFn read, void *ctx, char *why, size_t nwhy) {
    char path[600];
    size_t len = 0;
    uint8_t *got;
    int ok;

    switch (kind_of(d, n)) {
    case PART_EMPTY:
        return 1;
    case PART_FOLDER: {
        Container c;
        size_t rebuilt_n;
        uint8_t *rebuilt, *index;
        int i, parts = -1;
        size_t tail = 0;

        container_parse(&c, d, n);
        rebuilt = container_build(c.parts, c.n, c.tail, &rebuilt_n);   /* (the layout: the same bytes) */
        ok = rebuilt_n == n && memcmp(rebuilt, d, n) == 0;
        free(rebuilt);
        if (!ok) {
            snprintf(why, nwhy, "%s: the container doesn't lay out as the game's", base);
            return 0;
        }
        snprintf(path, sizeof(path), "%s/index.txt", base);
        index = read(ctx, path, &len);
        if (index == NULL || sscanf((char *)index, INDEX_FORMAT "\nparts %d\ntail %zu", &parts, &tail) != 2 ||
            parts != c.n || tail != c.tail) {
            free(index);
            snprintf(why, nwhy, "%s is missing or says another layout", path);
            return 0;
        }
        free(index);
        for (i = 0; i < c.n; i++) {
            snprintf(path, sizeof(path), "%s/%d", base, i);
            if (!check_part(path, c.parts[i].data, c.parts[i].size, read, ctx, why, nwhy)) {
                return 0;
            }
        }
        return 1;
    }
    case PART_IMAGE: {
        Tim t;

        tim_parse(&t, d, n);
        got = tim_rebuild(base, (TimReadFn)read, ctx, &len);
        ok = got != NULL && len == n && memcmp(got, d, n) == 0;
        free(got);
        if (!ok) {
            snprintf(why, nwhy, "%s: the image doesn't make the same TIM again", base);
        }
        return ok;
    }
    case PART_TEXT:
        snprintf(path, sizeof(path), "%s.txt", base);
        return same_text(path, d, n, read, ctx, why, nwhy);
    case PART_BANK: {
        char err[256] = "";
        uint8_t *text;

        snprintf(path, sizeof(path), "%s.bank.txt", base);
        text = read(ctx, path, &len);
        got = text != NULL ? vab_header_from_text((const char *)text, len, &len, err, sizeof(err)) : NULL;
        free(text);
        ok = got != NULL && len == n && memcmp(got, d, n) == 0;
        free(got);
        if (!ok) {
            snprintf(why, nwhy, "%s doesn't make the same header again %s", path, err);
        }
        return ok;
    }
    case PART_SEQUENCES: {
        char err[256] = "";

        snprintf(path, sizeof(path), "%s.sep", base);
        got = sep_from_folder(path, (SepReadFn)read, ctx, &len, err, sizeof(err));
        ok = got != NULL && len == n && memcmp(got, d, n) == 0;
        free(got);
        if (!ok) {
            snprintf(why, nwhy, "%s doesn't make the same sequences again %s", path, err);
        }
        return ok;
    }
    case PART_RAW:
        snprintf(path, sizeof(path), "%s.%s", base, raw_extension(d, n));
        got = read(ctx, path, &len);
        ok = got != NULL && len == n && memcmp(got, d, n) == 0;
        free(got);
        if (!ok) {
            snprintf(why, nwhy, "%s differs", path);
        }
        return ok;
    }
    return 0;
}

int cdconv_check(const char *name, const uint8_t *d, size_t n, CdconvReadFn read, void *ctx, char *why, size_t nwhy) {
    char base[300];
    long at;

    snprintf(base, sizeof(base), "cdbin/%s", name);
    if (!check_part(base, d, n, read, ctx, why, nwhy)) {
        return 0;
    }
    at = text_find_block(d, n);
    if (kind_of(d, n) == PART_RAW && at > 0 && pack_text(d + at, n - (size_t)at)) {
        char path[320];

        snprintf(path, sizeof(path), "%s.txt", base);
        return same_text(path, d + at, n - (size_t)at, read, ctx, why, nwhy);
    }
    return 1;
}
