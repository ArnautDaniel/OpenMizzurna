/* Sequences and MIDI files (seqmidi.h). */
#include "seqmidi.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define SEP_HEADER 6         /* "pQES", u16 version */
#define SEQ_HEADER 13        /* id, ticks, tempo (3), rhythm, bytes */
#define META 0xFF
#define META_TEMPO 0x51
#define META_END 0x2F
#define META_TIME_SIGNATURE 0x58
#define MIDI_CLOCKS_A_CLICK 24
#define MIDI_32NDS_A_QUARTER 8
#define INDEX_FORMAT "om-sep 1"

static uint32_t be(const uint8_t *p, int n) {
    uint32_t v = 0;
    while (n--) {
        v = v << 8 | *p++;
    }
    return v;
}

typedef struct Buf {
    uint8_t *d;
    size_t n, cap;
} Buf;

static void put(Buf *b, const void *d, size_t n) {
    if (b->n + n > b->cap) {
        b->cap = (b->n + n) * 2 + 64;
        b->d = realloc(b->d, b->cap);
    }
    memcpy(b->d + b->n, d, n);
    b->n += n;
}
static void put1(Buf *b, unsigned v) { uint8_t c = (uint8_t)v; put(b, &c, 1); }
static void putbe(Buf *b, uint32_t v, int n) { while (n--) put1(b, v >> (n * 8) & 0xFF); }

/* the sequences' places */
static int sequence_at(const uint8_t *d, size_t n, int i, size_t *at) {
    size_t o = SEP_HEADER;
    int k;

    if (n < SEP_HEADER || memcmp(d, "pQES", 4) != 0) {
        return 0;
    }
    for (k = 0; o + SEQ_HEADER <= n; k++) {
        size_t len = be(d + o + 9, 4);
        if (o + SEQ_HEADER + len > n) {
            return 0;
        }
        if (k == i) {
            *at = o;
            return 1;
        }
        o += SEQ_HEADER + len;
    }
    return 0;
}

int sep_sequences(const uint8_t *d, size_t n) {
    size_t at;
    int k = 0;

    while (sequence_at(d, n, k, &at)) {
        k++;
    }
    return k;
}

/* the length of a variable-length number at p */
static size_t varlen_size(const uint8_t *p, const uint8_t *end) {
    size_t k = 0;
    while (p + k < end && (p[k] & 0x80)) {
        k++;
    }
    return k + 1;
}

/* the running status while scanning (carried_after_tempo) */
static unsigned sRun;
static void run_status_set(unsigned s) { sRun = s; }
static unsigned run_status_get(void) { return sRun; }

/* a channel message's data bytes */
static int data_bytes(unsigned status) {
    unsigned kind = status & 0xF0;
    return kind == 0xC0 || kind == 0xD0 ? 1 : 2;
}

/* the tempo changes (counted from 0) after which the original carries running status on, as a list "a b c"
 * (index.txt keeps it: the MIDI file says the status again, and only those come back without it) */
static void carried_after_tempo(const uint8_t *d, size_t n, int i, char *out, size_t nout) {
    size_t at, k = 0;
    const uint8_t *ev, *end;
    int tempos = 0, after = 0;
    size_t o = 0;

    out[0] = 0;
    if (!sequence_at(d, n, i, &at)) {
        return;
    }
    ev = d + at + SEQ_HEADER;
    end = ev + be(d + at + 9, 4);
    while (ev + k < end) {
        unsigned status;

        k += varlen_size(ev + k, end);
        if (ev + k >= end) {
            break;
        }
        status = ev[k];
        if (status == META) {
            if (ev[k + 1] != META_TEMPO) {
                break;
            }
            k += 5;
            after = 1;
            tempos++;
            continue;
        }
        if (!(status & 0x80) && after && o + 16 < nout) {
            o += (size_t)snprintf(out + o, nout - o, " %d", tempos - 1);
        }
        if (status & 0x80) {
            k += 1 + (size_t)data_bytes(status);
            run_status_set(status);
        } else {
            k += (size_t)data_bytes(run_status_get());
        }
        after = 0;
    }
}

uint8_t *sep_to_midi(const uint8_t *d, size_t n, int i, size_t *size) {
    size_t at, len, k = 0;
    const uint8_t *ev, *end;
    unsigned ticks, tempo, rhythm, run = 0;
    Buf track = {0}, file = {0};
    int after_tempo = 0;

    if (!sequence_at(d, n, i, &at)) {
        return NULL;
    }
    ticks = be(d + at + 2, 2);
    tempo = be(d + at + 4, 3);
    rhythm = be(d + at + 7, 2);
    len = be(d + at + 9, 4);
    ev = d + at + SEQ_HEADER;
    end = ev + len;
    /* the header's tempo and time signature, first */
    put(&track, "\x00\xFF\x51\x03", 4);
    putbe(&track, tempo, 3);
    put(&track, "\x00\xFF\x58\x04", 4);
    put1(&track, rhythm >> 8);
    put1(&track, rhythm & 0xFF);
    put1(&track, MIDI_CLOCKS_A_CLICK);
    put1(&track, MIDI_32NDS_A_QUARTER);
    while (ev + k < end) {
        size_t dl = varlen_size(ev + k, end);
        unsigned status;

        put(&track, ev + k, dl);   /* the delta, as it is */
        k += dl;
        if (ev + k >= end) {
            break;
        }
        status = ev[k];
        if (status == META) {
            unsigned type = ev[k + 1];

            if (type == META_TEMPO) {   /* Sony's has no length byte */
                put(&track, "\xFF\x51\x03", 3);
                put(&track, ev + k + 2, 3);
                k += 5;
                after_tempo = 1;
                continue;
            }
            put(&track, ev + k, (size_t)(end - (ev + k)));   /* (the end, FF 2F 00) */
            break;
        }
        if (status & 0x80) {
            run = status;
            put(&track, ev + k, 1 + (size_t)data_bytes(status));
            k += 1 + (size_t)data_bytes(status);
        } else {
            if (after_tempo) {   /* (a MIDI file's meta event ends running status: say it again) */
                put1(&track, run);
            }
            put(&track, ev + k, (size_t)data_bytes(run));
            k += (size_t)data_bytes(run);
        }
        after_tempo = 0;
    }
    put(&file, "MThd", 4);
    putbe(&file, 6, 4);
    putbe(&file, 0, 2);   /* format 0 */
    putbe(&file, 1, 2);
    putbe(&file, ticks, 2);
    put(&file, "MTrk", 4);
    putbe(&file, (uint32_t)track.n, 4);
    put(&file, track.d, track.n);
    free(track.d);
    *size = file.n;
    return file.d;
}

char *sep_index_text(const uint8_t *d, size_t n, const char *comment, size_t *size) {
    int count = sep_sequences(d, n), i, k;
    char *s = malloc((size_t)count * 600 + 512), carried[512];
    size_t at;

    k = sprintf(s, "\\ %s\n%s\nversion %u\nsequences %d\n", comment, INDEX_FORMAT, be(d + 4, 2), count);
    for (i = 0; i < count; i++) {
        sequence_at(d, n, i, &at);
        carried_after_tempo(d, n, i, carried, sizeof(carried));
        k += sprintf(s + k, "sequence %d id %u%s%s\n", i, be(d + at, 2), carried[0] ? " running-status-after-tempo" : "", carried);
    }
    *size = (size_t)k;
    return s;
}

/* a MIDI file's track back to Sony's events (the first two events - the header's tempo and time signature -
 * into the header) */
static int listed(const char *list, int v) {
    const char *p = list;
    char *end;

    while (p != NULL && *p) {
        long x = strtol(p, &end, 10);
        if (end == p) {
            break;
        }
        if (x == v) {
            return 1;
        }
        p = end;
    }
    return 0;
}

static int midi_back(const uint8_t *m, size_t n, Buf *out, unsigned id, const char *carried, char *why, size_t nwhy) {
    const uint8_t *t, *end;
    unsigned ticks, tempo = 0, rhythm = 0, run = 0;
    size_t k = 0;
    Buf ev = {0};
    int after_tempo = 0, tempos = 0;

    if (n < 22 || memcmp(m, "MThd", 4) != 0 || memcmp(m + 14, "MTrk", 4) != 0) {
        snprintf(why, nwhy, "not a MIDI file");
        return 0;
    }
    ticks = be(m + 12, 2);
    t = m + 22;
    end = t + be(m + 18, 4);
    if (end > m + n || end - t < 16 || memcmp(t, "\x00\xFF\x51\x03", 4) != 0 || memcmp(t + 7, "\x00\xFF\x58\x04", 4) != 0) {
        snprintf(why, nwhy, "a sequence's MIDI file starts with its tempo and time signature");
        return 0;
    }
    tempo = be(t + 4, 3);
    rhythm = t[11] << 8 | t[12];
    t += 15;
    while (t + k < end) {
        size_t dl = varlen_size(t + k, end);
        unsigned status;

        put(&ev, t + k, dl);
        k += dl;
        if (t + k >= end) {
            break;
        }
        status = t[k];
        if (status == META) {
            if (t[k + 1] == META_TEMPO && t[k + 2] == 3) {
                put(&ev, "\xFF\x51", 2);
                put(&ev, t + k + 3, 3);
                k += 6;
                after_tempo = 1;
                tempos++;
                continue;
            }
            put(&ev, t + k, (size_t)(end - (t + k)));
            break;
        }
        if (status & 0x80) {
            if (!(after_tempo && status == run && listed(carried, tempos - 1))) {   /* (said again only for the MIDI file) */
                put1(&ev, status);
            }
            run = status;
            k++;
        }
        put(&ev, t + k, (size_t)data_bytes(run));
        k += (size_t)data_bytes(run);
        after_tempo = 0;
    }
    putbe(out, id, 2);
    putbe(out, ticks, 2);
    putbe(out, tempo, 3);
    putbe(out, rhythm, 2);
    putbe(out, (uint32_t)ev.n, 4);
    put(out, ev.d, ev.n);
    free(ev.d);
    return 1;
}

uint8_t *sep_from_folder(const char *folder, SepReadFn read, void *ctx, size_t *size, char *why, size_t nwhy) {
    char path[600];
    size_t n = 0;
    uint8_t *index;
    unsigned version, count, i;
    Buf out = {0};
    const char *at;

    snprintf(path, sizeof(path), "%s/index.txt", folder);
    index = read(ctx, path, &n);
    at = index != NULL ? strstr((char *)index, INDEX_FORMAT) : NULL;
    if (at == NULL || sscanf(at, INDEX_FORMAT "\nversion %u\nsequences %u", &version, &count) != 2) {
        snprintf(why, nwhy, "%s is missing or malformed", path);
        free(index);
        return NULL;
    }
    put(&out, "pQES", 4);
    putbe(&out, version, 2);
    for (i = 0; i < count; i++) {
        char key[64], carried[512] = "";
        const char *line, *eol, *tag;
        unsigned id = 0;
        uint8_t *mid;
        size_t mn;
        int ok;

        snprintf(key, sizeof(key), "\nsequence %u id ", i);
        line = strstr((char *)index, key);
        if (line == NULL || sscanf(line + strlen(key), "%u", &id) != 1) {
            snprintf(why, nwhy, "%s: no sequence %u", path, i);
            free(index);
            free(out.d);
            return NULL;
        }
        eol = strchr(line + 1, '\n');
        tag = strstr(line + 1, "running-status-after-tempo");
        if (tag != NULL && (eol == NULL || tag < eol)) {
            size_t len = (size_t)((eol != NULL ? eol : tag + strlen(tag)) - (tag + 26));
            snprintf(carried, sizeof(carried), "%.*s", (int)len, tag + 26);
        }
        snprintf(path, sizeof(path), "%s/%u.mid", folder, i);
        mid = read(ctx, path, &mn);
        ok = mid != NULL && midi_back(mid, mn, &out, id, carried, why, nwhy);
        free(mid);
        if (!ok) {
            free(index);
            free(out.d);
            return NULL;
        }
    }
    free(index);
    *size = out.n;
    return out.d;
}
