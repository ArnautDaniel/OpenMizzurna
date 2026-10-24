/* Sound banks (vab.h).
 *
 * The header's layout (Sony's VAB):
 *   32 bytes      "pBAV", u32 version, u32 id, u32 total size (header + body), u16 reserved, u16 programs used,
 *                 u16 tones, u16 samples, u8 master volume, u8 master pan, u8 attributes x2, u32 reserved
 *   128 x 16      programs: u8 tones, volume, priority, mode, pan, reserved; u16 attribute; u32 reserved x2
 *   used x 16 x 32  tones, 16 slots a used program: u8 priority, mode, volume, pan, centre key, fine tune (1/128
 *                 of a semitone), lowest key, highest key, vibrato width, time, portamento width, time, bend
 *                 down, up, reserved x2; u16 envelope words x2 (the sound processor's ADSR); s16 program,
 *                 sample; s16 reserved x4
 *   256 x u16     the samples' sizes in 8-byte units (entry 0 unused)
 * The text names every field; anything all zero (an unused program or tone slot, the reserved fields) is left
 * out and comes back as zero. */
#include "vab.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define VH_HEADER 32
#define VH_PROGRAM 16
#define VH_TONE 32
#define VH_SIZE_UNIT 8
#define TEXT_FORMAT "om-bank 1"

/* the envelope words (the sound processor's ADSR) */
#define ADSR1_ATTACK_EXPONENTIAL 0x8000
#define ADSR1_ATTACK_SHIFT(a) ((a) >> 10 & 0x1F)
#define ADSR1_ATTACK_STEP(a) ((a) >> 8 & 0x3)
#define ADSR1_DECAY_SHIFT(a) ((a) >> 4 & 0xF)
#define ADSR1_SUSTAIN_LEVEL(a) ((a) & 0xF)
#define ADSR2_SUSTAIN_EXPONENTIAL 0x8000
#define ADSR2_SUSTAIN_DECREASE 0x4000
#define ADSR2_UNUSED_BIT 0x2000
#define ADSR2_SUSTAIN_SHIFT(a) ((a) >> 8 & 0x1F)
#define ADSR2_SUSTAIN_STEP(a) ((a) >> 6 & 0x3)
#define ADSR2_RELEASE_EXPONENTIAL 0x20
#define ADSR2_RELEASE_SHIFT(a) ((a) & 0x1F)

static uint16_t le16(const uint8_t *p) { return (uint16_t)(p[0] | p[1] << 8); }
static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }
static void put16(uint8_t *p, unsigned v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); }
static void put32(uint8_t *p, uint32_t v) { p[0] = (uint8_t)v; p[1] = (uint8_t)(v >> 8); p[2] = (uint8_t)(v >> 16); p[3] = (uint8_t)(v >> 24); }

static int used_programs(const uint8_t *vh) { return le16(vh + 18); }
static size_t tones_at(void) { return VH_HEADER + VAB_PROGRAMS * VH_PROGRAM; }
static size_t sizes_at(const uint8_t *vh) { return tones_at() + (size_t)used_programs(vh) * VAB_TONES_PER_PROGRAM * VH_TONE; }

size_t vab_header_size(const uint8_t *vh, size_t n) {
    size_t size;

    if (n < VH_HEADER || memcmp(vh, "pBAV", 4) != 0) {
        return 0;
    }
    size = sizes_at(vh) + VAB_SAMPLES * 2;
    return size <= n ? size : 0;
}

size_t vab_body_size(const uint8_t *vh) {
    size_t total = le32(vh + 12), own = sizes_at(vh) + VAB_SAMPLES * 2;
    return total > own ? total - own : 0;
}

int vab_sample_count(const uint8_t *vh) { return le16(vh + 22); }
size_t vab_sample_bytes(const uint8_t *vh, int i) { return (size_t)le16(vh + sizes_at(vh) + i * 2) * VH_SIZE_UNIT; }

/* ---- the header as text ---- */
typedef struct Out {
    char *s;
    size_t n, cap;
} Out;

static void outf(Out *o, const char *fmt, ...) __attribute__((format(printf, 2, 3)));
#include <stdarg.h>
static void outf(Out *o, const char *fmt, ...) {
    va_list ap;
    int k;

    for (;;) {
        va_start(ap, fmt);
        k = vsnprintf(o->s + o->n, o->cap - o->n, fmt, ap);
        va_end(ap);
        if (k >= 0 && o->n + (size_t)k < o->cap) {
            o->n += (size_t)k;
            return;
        }
        o->cap = o->cap * 2 + (size_t)k + 256;
        o->s = realloc(o->s, o->cap);
    }
}

static int all_zero(const uint8_t *p, size_t n) {
    size_t i;
    for (i = 0; i < n; i++) {
        if (p[i] != 0) {
            return 0;
        }
    }
    return 1;
}

char *vab_header_text(const uint8_t *vh, size_t n, const char *comment, size_t *len) {
    Out o = {NULL, 0, 0};
    int p, b, s;

    if (vab_header_size(vh, n) == 0) {
        return NULL;
    }
    outf(&o, "\\ %s\n%s\n", comment, TEXT_FORMAT);
    outf(&o, "version %u\nid %u\nsize %u\n", le32(vh + 4), le32(vh + 8), le32(vh + 12));
    outf(&o, "programs %u\ntones %u\nsamples %u\n", le16(vh + 18), le16(vh + 20), le16(vh + 22));
    outf(&o, "master-volume %u\nmaster-pan %u\nattributes %u %u\n", vh[24], vh[25], vh[26], vh[27]);
    if (le16(vh + 16) || le32(vh + 28)) {
        outf(&o, "reserved %u %u\n", le16(vh + 16), le32(vh + 28));
    }
    outf(&o, "\\ program <n>: tones, volume, priority, mode, pan, attribute\n");
    for (p = 0; p < VAB_PROGRAMS; p++) {
        const uint8_t *e = vh + VH_HEADER + p * VH_PROGRAM;
        if (all_zero(e, VH_PROGRAM)) {
            continue;
        }
        outf(&o, "program %d  tones %u  volume %u  priority %u  mode %u  pan %u  attribute %u", p, e[0], e[1], e[2], e[3],
             e[4], le16(e + 6));
        if (e[5] || le32(e + 8) || le32(e + 12)) {
            outf(&o, "  reserved %u %u %u", e[5], le32(e + 8), le32(e + 12));
        }
        outf(&o, "\n");
    }
    outf(&o, "\\ tone <block> <slot>: the used programs' tones, 16 slots each\n");
    for (b = 0; b < used_programs(vh); b++) {
        for (s = 0; s < VAB_TONES_PER_PROGRAM; s++) {
            const uint8_t *t = vh + tones_at() + ((size_t)b * VAB_TONES_PER_PROGRAM + s) * VH_TONE;
            unsigned a1 = le16(t + 16), a2 = le16(t + 18);

            if (all_zero(t, VH_TONE)) {
                continue;
            }
            outf(&o, "tone %d %d  program %d  sample %d  keys %u %u  centre %u  fine %u  volume %u  pan %u  priority %u  mode %u",
                 b, s, (int16_t)le16(t + 20), (int16_t)le16(t + 22), t[6], t[7], t[4], t[5], t[2], t[3], t[0], t[1]);
            outf(&o, "  bend %u %u  vibrato %u %u  portamento %u %u", t[12], t[13], t[8], t[9], t[10], t[11]);
            outf(&o, "  attack %s %u %u  decay %u  sustain-level %u  sustain %s %s %u %u  release %s %u",
                 a1 & ADSR1_ATTACK_EXPONENTIAL ? "exponential" : "linear", ADSR1_ATTACK_SHIFT(a1), ADSR1_ATTACK_STEP(a1),
                 ADSR1_DECAY_SHIFT(a1), ADSR1_SUSTAIN_LEVEL(a1),
                 a2 & ADSR2_SUSTAIN_EXPONENTIAL ? "exponential" : "linear", a2 & ADSR2_SUSTAIN_DECREASE ? "decrease" : "increase",
                 ADSR2_SUSTAIN_SHIFT(a2), ADSR2_SUSTAIN_STEP(a2), a2 & ADSR2_RELEASE_EXPONENTIAL ? "exponential" : "linear",
                 ADSR2_RELEASE_SHIFT(a2));
            if (a2 & ADSR2_UNUSED_BIT) {
                outf(&o, "  envelope-bit13");
            }
            if (t[14] || t[15] || !all_zero(t + 24, 8)) {
                outf(&o, "  reserved %u %u %u %u %u %u", t[14], t[15], le16(t + 24), le16(t + 26), le16(t + 28), le16(t + 30));
            }
            outf(&o, "\n");
        }
    }
    outf(&o, "\\ sample-size <n> <bytes>\n");
    for (s = 0; s < VAB_SAMPLES; s++) {
        if (le16(vh + sizes_at(vh) + s * 2)) {
            outf(&o, "sample-size %d %zu\n", s, vab_sample_bytes(vh, s));
        }
    }
    *len = o.n;
    return o.s;
}

/* ---- and back ---- */

/* the next word of a line (or NULL) */
static char *word(char **at) {
    char *w;

    while (**at == ' ' || **at == '\t') {
        (*at)++;
    }
    if (**at == 0) {
        return NULL;
    }
    w = *at;
    while (**at && **at != ' ' && **at != '\t') {
        (*at)++;
    }
    if (**at) {
        *(*at)++ = 0;
    }
    return w;
}

static int number(char **at, long *v) {
    char *w = word(at), *end;

    if (w == NULL) {
        return 0;
    }
    *v = strtol(w, &end, 10);
    return *end == 0;
}

static int numbers(char **at, long *v, int n) {
    int i;
    for (i = 0; i < n; i++) {
        if (!number(at, &v[i])) {
            return 0;
        }
    }
    return 1;
}

static int mode_word(char **at, const char *one, const char *zero, int *bit) {
    char *w = word(at);

    if (w == NULL) {
        return 0;
    }
    *bit = strcmp(w, one) == 0;
    return *bit || strcmp(w, zero) == 0;
}

static int tone_line(uint8_t *vh, char *at, char *why, size_t nwhy) {
    long b, s, v[6] = {0};
    uint8_t *t;
    unsigned a1 = 0, a2 = 0;
    char *key;
    int bit = 0;

    if (!number(&at, &b) || !number(&at, &s) || b < 0 || b >= used_programs(vh) || s < 0 || s >= VAB_TONES_PER_PROGRAM) {
        snprintf(why, nwhy, "a tone line wants a block and a slot");
        return 0;
    }
    t = vh + tones_at() + ((size_t)b * VAB_TONES_PER_PROGRAM + (size_t)s) * VH_TONE;
    while ((key = word(&at)) != NULL) {
        int ok = 1;

        if (strcmp(key, "program") == 0) { ok = number(&at, v); put16(t + 20, (unsigned)v[0]); }
        else if (strcmp(key, "sample") == 0) { ok = number(&at, v); put16(t + 22, (unsigned)v[0]); }
        else if (strcmp(key, "keys") == 0) { ok = numbers(&at, v, 2); t[6] = (uint8_t)v[0]; t[7] = (uint8_t)v[1]; }
        else if (strcmp(key, "centre") == 0) { ok = number(&at, v); t[4] = (uint8_t)v[0]; }
        else if (strcmp(key, "fine") == 0) { ok = number(&at, v); t[5] = (uint8_t)v[0]; }
        else if (strcmp(key, "volume") == 0) { ok = number(&at, v); t[2] = (uint8_t)v[0]; }
        else if (strcmp(key, "pan") == 0) { ok = number(&at, v); t[3] = (uint8_t)v[0]; }
        else if (strcmp(key, "priority") == 0) { ok = number(&at, v); t[0] = (uint8_t)v[0]; }
        else if (strcmp(key, "mode") == 0) { ok = number(&at, v); t[1] = (uint8_t)v[0]; }
        else if (strcmp(key, "bend") == 0) { ok = numbers(&at, v, 2); t[12] = (uint8_t)v[0]; t[13] = (uint8_t)v[1]; }
        else if (strcmp(key, "vibrato") == 0) { ok = numbers(&at, v, 2); t[8] = (uint8_t)v[0]; t[9] = (uint8_t)v[1]; }
        else if (strcmp(key, "portamento") == 0) { ok = numbers(&at, v, 2); t[10] = (uint8_t)v[0]; t[11] = (uint8_t)v[1]; }
        else if (strcmp(key, "attack") == 0) {
            ok = mode_word(&at, "exponential", "linear", &bit) && numbers(&at, v, 2);
            a1 |= (bit ? ADSR1_ATTACK_EXPONENTIAL : 0) | (unsigned)(v[0] & 0x1F) << 10 | (unsigned)(v[1] & 3) << 8;
        } else if (strcmp(key, "decay") == 0) { ok = number(&at, v); a1 |= (unsigned)(v[0] & 0xF) << 4; }
        else if (strcmp(key, "sustain-level") == 0) { ok = number(&at, v); a1 |= (unsigned)(v[0] & 0xF); }
        else if (strcmp(key, "sustain") == 0) {
            int down = 0;
            ok = mode_word(&at, "exponential", "linear", &bit) && mode_word(&at, "decrease", "increase", &down) &&
                 numbers(&at, v, 2);
            a2 |= (bit ? ADSR2_SUSTAIN_EXPONENTIAL : 0) | (down ? ADSR2_SUSTAIN_DECREASE : 0) |
                  (unsigned)(v[0] & 0x1F) << 8 | (unsigned)(v[1] & 3) << 6;
        } else if (strcmp(key, "release") == 0) {
            ok = mode_word(&at, "exponential", "linear", &bit) && number(&at, v);
            a2 |= (bit ? ADSR2_RELEASE_EXPONENTIAL : 0) | (unsigned)(v[0] & 0x1F);
        } else if (strcmp(key, "envelope-bit13") == 0) { a2 |= ADSR2_UNUSED_BIT; }
        else if (strcmp(key, "reserved") == 0) {
            ok = numbers(&at, v, 6);
            t[14] = (uint8_t)v[0]; t[15] = (uint8_t)v[1];
            put16(t + 24, (unsigned)v[2]); put16(t + 26, (unsigned)v[3]); put16(t + 28, (unsigned)v[4]); put16(t + 30, (unsigned)v[5]);
        } else {
            snprintf(why, nwhy, "a tone has no field \"%s\"", key);
            return 0;
        }
        if (!ok) {
            snprintf(why, nwhy, "a tone's %s wants other values", key);
            return 0;
        }
    }
    put16(t + 16, a1);
    put16(t + 18, a2);
    return 1;
}

static int program_line(uint8_t *vh, char *at, char *why, size_t nwhy) {
    long p, v[3];
    uint8_t *e;
    char *key;

    if (!number(&at, &p) || p < 0 || p >= VAB_PROGRAMS) {
        snprintf(why, nwhy, "a program line wants its number (0..127)");
        return 0;
    }
    e = vh + VH_HEADER + p * VH_PROGRAM;
    while ((key = word(&at)) != NULL) {
        int ok = 1;

        if (strcmp(key, "tones") == 0) { ok = number(&at, v); e[0] = (uint8_t)v[0]; }
        else if (strcmp(key, "volume") == 0) { ok = number(&at, v); e[1] = (uint8_t)v[0]; }
        else if (strcmp(key, "priority") == 0) { ok = number(&at, v); e[2] = (uint8_t)v[0]; }
        else if (strcmp(key, "mode") == 0) { ok = number(&at, v); e[3] = (uint8_t)v[0]; }
        else if (strcmp(key, "pan") == 0) { ok = number(&at, v); e[4] = (uint8_t)v[0]; }
        else if (strcmp(key, "attribute") == 0) { ok = number(&at, v); put16(e + 6, (unsigned)v[0]); }
        else if (strcmp(key, "reserved") == 0) {
            ok = numbers(&at, v, 3);
            e[5] = (uint8_t)v[0];
            put32(e + 8, (uint32_t)v[1]);
            put32(e + 12, (uint32_t)v[2]);
        } else {
            snprintf(why, nwhy, "a program has no field \"%s\"", key);
            return 0;
        }
        if (!ok) {
            snprintf(why, nwhy, "a program's %s wants a number", key);
            return 0;
        }
    }
    return 1;
}

uint8_t *vab_header_from_text(const char *s, size_t n, size_t *size, char *why, size_t nwhy) {
    char *copy = malloc(n + 1), *line, *next;
    uint8_t head[VH_HEADER] = {'p', 'B', 'A', 'V'}, *vh = NULL;
    int lineno = 0, have_format = 0;

    memcpy(copy, s, n);
    copy[n] = 0;
    for (line = copy; line != NULL; line = next) {
        char *at = line, *key;
        long v[2] = {0};
        int ok = 1;

        next = strchr(line, '\n');
        if (next != NULL) {
            *next++ = 0;
        }
        lineno++;
        if (line[0] == '\\' || line[0] == 0) {
            continue;
        }
        if (strcmp(line, TEXT_FORMAT) == 0) {
            have_format = 1;
            continue;
        }
        key = word(&at);
        if (key == NULL) {   /* (a line of spaces) */
            continue;
        }
        if (strcmp(key, "version") == 0) { ok = number(&at, v); put32(head + 4, (uint32_t)v[0]); }
        else if (strcmp(key, "id") == 0) { ok = number(&at, v); put32(head + 8, (uint32_t)v[0]); }
        else if (strcmp(key, "size") == 0) { ok = number(&at, v); put32(head + 12, (uint32_t)v[0]); }
        else if (strcmp(key, "programs") == 0) { ok = number(&at, v); put16(head + 18, (unsigned)v[0]); }
        else if (strcmp(key, "tones") == 0) { ok = number(&at, v); put16(head + 20, (unsigned)v[0]); }
        else if (strcmp(key, "samples") == 0) { ok = number(&at, v); put16(head + 22, (unsigned)v[0]); }
        else if (strcmp(key, "master-volume") == 0) { ok = number(&at, v); head[24] = (uint8_t)v[0]; }
        else if (strcmp(key, "master-pan") == 0) { ok = number(&at, v); head[25] = (uint8_t)v[0]; }
        else if (strcmp(key, "attributes") == 0) { ok = numbers(&at, v, 2); head[26] = (uint8_t)v[0]; head[27] = (uint8_t)v[1]; }
        else if (strcmp(key, "reserved") == 0) { ok = numbers(&at, v, 2); put16(head + 16, (unsigned)v[0]); put32(head + 28, (uint32_t)v[1]); }
        else {
            if (vh == NULL) {   /* (the counts are known now: the header can be laid out) */
                *size = sizes_at(head) + VAB_SAMPLES * 2;
                vh = calloc(1, *size);
                memcpy(vh, head, VH_HEADER);
            }
            if (strcmp(key, "program") == 0) {
                ok = program_line(vh, at, why, nwhy);
            } else if (strcmp(key, "tone") == 0) {
                ok = tone_line(vh, at, why, nwhy);
            } else if (strcmp(key, "sample-size") == 0) {
                ok = numbers(&at, v, 2) && v[0] >= 0 && v[0] < VAB_SAMPLES;
                if (ok) {
                    put16(vh + sizes_at(vh) + v[0] * 2, (unsigned)(v[1] / VH_SIZE_UNIT));
                } else {
                    snprintf(why, nwhy, "sample-size wants a sample and its bytes");
                }
            } else {
                snprintf(why, nwhy, "no field \"%s\"", key);
                ok = 0;
            }
            if (!ok) {
                char where[64];
                snprintf(where, sizeof(where), " (line %d)", lineno);
                strncat(why, where, nwhy - strlen(why) - 1);
                free(vh);
                free(copy);
                return NULL;
            }
            continue;
        }
        if (!ok) {
            snprintf(why, nwhy, "line %d: %s wants a number", lineno, key);
            free(vh);
            free(copy);
            return NULL;
        }
    }
    free(copy);
    if (!have_format || vh == NULL) {
        snprintf(why, nwhy, have_format ? "no programs" : "not a bank text (" TEXT_FORMAT ")");
        free(vh);
        return NULL;
    }
    return vh;
}

/* ---- samples ---- */
#define ADPCM_BLOCK 16
#define ADPCM_SAMPLES 28
#define FLAG_END 0x1
#define FLAG_REPEAT 0x2
#define FLAG_LOOP_START 0x4
static const int kFilter[5][2] = {{0, 0}, {60, 0}, {115, -52}, {98, -55}, {122, -60}};

int16_t *vab_decode_sample(const uint8_t *adpcm, size_t n, long *frames, long *loop_start) {
    int16_t *pcm = malloc(sizeof(int16_t) * (n / ADPCM_BLOCK + 1) * ADPCM_SAMPLES);
    int h1 = 0, h2 = 0, i;
    long out = 0, loop = -1;
    size_t o;
    int repeats = 0;

    for (o = 0; o + ADPCM_BLOCK <= n; o += ADPCM_BLOCK) {
        int shift = adpcm[o] & 0xF, filter = adpcm[o] >> 4, flags = adpcm[o + 1];

        if (filter > 4) {
            filter = 4;
        }
        if (flags & FLAG_LOOP_START) {
            loop = out;
        }
        for (i = 0; i < ADPCM_SAMPLES; i++) {
            int nibble = adpcm[o + 2 + i / 2] >> (i % 2 * 4) & 0xF;
            int v = (int16_t)(nibble << 12) >> shift;

            v += (h1 * kFilter[filter][0] + h2 * kFilter[filter][1] + 32) >> 6;
            v = v < -32768 ? -32768 : v > 32767 ? 32767 : v;
            pcm[out++] = (int16_t)v;
            h2 = h1;
            h1 = v;
        }
        if (flags & FLAG_END) {   /* (blocks after the end flag are never played) */
            repeats = (flags & FLAG_REPEAT) != 0;
            break;
        }
    }
    *frames = out;
    *loop_start = repeats ? (loop >= 0 ? loop : 0) : -1;
    return pcm;
}

/* ---- WAV ---- */
#define SMPL_LOOP_FORWARD 0

static void put_bytes(uint8_t **p, const void *d, size_t n) { memcpy(*p, d, n); *p += n; }

uint8_t *vab_wav(const int16_t *pcm, long frames, int rate, long loop_start, size_t *size) {
    size_t data = (size_t)frames * 2, smpl = loop_start >= 0 ? 8 + 36 + 24 : 0, total = 12 + 8 + 16 + 8 + data + smpl;
    uint8_t *w = calloc(1, total), *p = w, buf[64];
    long i;

    put_bytes(&p, "RIFF", 4);
    put32(p, (uint32_t)(total - 8)); p += 4;
    put_bytes(&p, "WAVEfmt ", 8);
    put32(p, 16); put16(p + 4, 1); put16(p + 6, 1); put32(p + 8, (uint32_t)rate); put32(p + 12, (uint32_t)rate * 2);
    put16(p + 16, 2); put16(p + 18, 16); p += 20;
    put_bytes(&p, "data", 4);
    put32(p, (uint32_t)data); p += 4;
    for (i = 0; i < frames; i++, p += 2) {
        put16(p, (uint16_t)pcm[i]);
    }
    if (loop_start >= 0) {   /* smpl: one forward loop, loop_start to the last sample */
        memset(buf, 0, sizeof(buf));
        put_bytes(&p, "smpl", 4);
        put32(p, 36 + 24); p += 4;
        put32(buf + 8, (uint32_t)(1000000000.0 / rate));   /* sample period, ns */
        put32(buf + 12, 60);                                  /* MIDI unity note */
        put32(buf + 28, 1);                                   /* one loop */
        put_bytes(&p, buf, 36);
        memset(buf, 0, 24);
        put32(buf + 4, SMPL_LOOP_FORWARD);
        put32(buf + 8, (uint32_t)loop_start);
        put32(buf + 12, (uint32_t)(frames - 1));
        put_bytes(&p, buf, 24);
    }
    *size = total;
    return w;
}

int16_t *vab_wav_read(const uint8_t *wav, size_t n, long *frames, long *loop_start) {
    size_t at = 12;
    int16_t *pcm = NULL;
    long count = 0;

    *loop_start = -1;
    if (n < 12 || memcmp(wav, "RIFF", 4) != 0 || memcmp(wav + 8, "WAVE", 4) != 0) {
        return NULL;
    }
    while (at + 8 <= n) {
        uint32_t len = le32(wav + at + 4);

        if (at + 8 + len > n) {
            break;
        }
        if (memcmp(wav + at, "fmt ", 4) == 0 && (len < 16 || le16(wav + at + 8) != 1 || le16(wav + at + 10) != 1 ||
                                                 le16(wav + at + 22) != 16)) {
            free(pcm);
            return NULL;   /* (ours: PCM, mono, 16-bit) */
        }
        if (memcmp(wav + at, "data", 4) == 0) {
            long i;
            count = (long)(len / 2);
            pcm = malloc(sizeof(int16_t) * (size_t)(count + 1));
            for (i = 0; i < count; i++) {
                pcm[i] = (int16_t)le16(wav + at + 8 + i * 2);
            }
        }
        if (memcmp(wav + at, "smpl", 4) == 0 && len >= 36 + 24 && le32(wav + at + 8 + 28) >= 1) {
            *loop_start = (long)le32(wav + at + 8 + 36 + 8);
        }
        at += 8 + len + (len & 1);
    }
    *frames = count;
    return pcm;
}
