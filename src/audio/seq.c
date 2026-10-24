/* The sequencer (audio.h's au_sep_* / au_seq_*): a SEP - the library's N.sep/ made back into the original's bytes
 * (data/seqmidi.c) - holds sequences of MIDI events with Sony's own header and habits: a tempo change is FF 51 and
 * three bytes (no length byte), FF 2F ends the data, and a loop is NRPN 99 = 20 with data entry n (its start: n
 * times, 0 or 127 for ever) and 99 = 30 (back to its start). Time runs by the output's samples. */
#include "mixer.h"

#include "../core/files.h"
#include "../data/seqmidi.h"

#include <stdlib.h>
#include <string.h>

#define SEP_HEADER 6        /* "pQES", u16 version */
#define SEQ_HEADER 13       /* u16 id, u16 ticks a quarter, u24 tempo, u16 rhythm, u32 bytes */
#define DEFAULT_TEMPO 500000
#define LOOP_START 20       /* the NRPN's values */
#define LOOP_END 30
#define FOREVER 127
#define CC_VOLUME 7
#define CC_PAN 10
#define CC_EXPRESSION 11
#define CC_DATA 6
#define CC_NRPN 99
#define BEND_CENTRE 8192
#define EVENTS_A_BLOCK 4096  /* (a guard against a sequence that never waits) */

typedef struct Channel {
    int program, volume, pan, expression, bend;
} Channel;

typedef struct Seq {
    uint8_t *d;
    int length, per_quarter;
    double tempo, wait;
    int playing, at, loops, forever;
    int loop_at, loop_count, nrpn;
    uint8_t running;
    Channel ch[16];
    float volume, fade, fade_step;
} Seq;

typedef struct Sep {
    int used, bank, n;
    Seq s[AU_SEQS];
} Sep;

static Sep sSeps[AU_SEPS];

static int be(const uint8_t *p, int n) {
    int v = 0;
    while (n--) {
        v = v << 8 | *p++;
    }
    return v;
}

static Seq *seq_of(int sep, int seq) {
    if (sep < 0 || sep >= AU_SEPS || !sSeps[sep].used || seq < 0 || seq >= sSeps[sep].n) {
        return NULL;
    }
    return &sSeps[sep].s[seq];
}

static uint8_t *read_file(void *ctx, const char *path, size_t *n) {
    (void)ctx;
    return files_read(path, n);
}

int au_sep_load(const char *folder, int bank) {
    size_t n = 0;
    char why[256];
    uint8_t *d = sep_from_folder(folder, read_file, NULL, &n, why, sizeof(why));
    int id, i, at = SEP_HEADER;

    if (d == NULL) {
        return -1;
    }
    au_lock();
    for (id = 0; id < AU_SEPS && sSeps[id].used; id++) {
    }
    if (id == AU_SEPS) {
        au_unlock();
        free(d);
        return -1;
    }
    memset(&sSeps[id], 0, sizeof(sSeps[id]));
    sSeps[id].used = 1;
    sSeps[id].bank = bank;
    for (i = 0; i < AU_SEQS && (size_t)at + SEQ_HEADER <= n; i++) {
        Seq *s = &sSeps[id].s[i];
        int len = be(d + at + 9, 4);

        if ((size_t)(at + SEQ_HEADER + len) > n) {
            break;
        }
        s->per_quarter = be(d + at + 2, 2);
        s->tempo = be(d + at + 4, 3);
        s->length = len;
        s->d = calloc((size_t)len + 4, 1);
        memcpy(s->d, d + at + SEQ_HEADER, (size_t)len);
        s->volume = 1;
        s->fade = 1;
        at += SEQ_HEADER + len;
    }
    sSeps[id].n = i;
    au_unlock();
    free(d);
    return id;
}

static void voices_off(int sep, int seq, int channel, int key) {
    int i;

    for (i = 0; i < AU_VOICES; i++) {
        AuVoice *v = &au_voices[i];
        if (v->on && v->owner == AU_OWN_SEQ && v->sep == sep && (seq < 0 || v->seq == seq) &&
            (channel < 0 || v->channel == channel) && (key < 0 || v->key == key)) {
            v->phase = ENV_RELEASE;
        }
    }
}

void au_sep_free(int sep) {
    int i;

    au_lock();
    if (sep >= 0 && sep < AU_SEPS && sSeps[sep].used) {
        for (i = 0; i < AU_VOICES; i++) {
            if (au_voices[i].on && au_voices[i].owner == AU_OWN_SEQ && au_voices[i].sep == sep) {
                au_voices[i].on = 0;
            }
        }
        for (i = 0; i < sSeps[sep].n; i++) {
            free(sSeps[sep].s[i].d);
        }
        memset(&sSeps[sep], 0, sizeof(sSeps[sep]));
    }
    au_unlock();
}

void sq_reset(void) {
    int i;
    for (i = 0; i < AU_SEPS; i++) {
        au_sep_free(i);
    }
}

static int varlen(Seq *s) {
    int v = 0, c;
    do {
        if (s->at >= s->length) {
            return 0;
        }
        c = s->d[s->at++];
        v = v << 7 | (c & 0x7F);
    } while (c & 0x80);
    return v;
}

static void rewind_seq(Seq *s) {
    int c;

    s->at = 0;
    s->running = 0;
    s->loop_at = -1;
    s->nrpn = -1;
    for (c = 0; c < 16; c++) {
        s->ch[c] = (Channel){0, 127, 64, 127, 0};
    }
    s->wait = varlen(s);
}

void au_seq_play(int sep, int seq, int loops, float volume) {
    Seq *s;

    au_lock();
    if ((s = seq_of(sep, seq)) != NULL) {
        rewind_seq(s);
        s->playing = 1;
        s->forever = loops == 0;
        s->loops = loops;
        s->volume = volume;
        s->fade = 1;
        s->fade_step = 0;
    }
    au_unlock();
}

void au_seq_stop(int sep, int seq) {
    Seq *s;

    au_lock();
    if ((s = seq_of(sep, seq)) != NULL) {
        s->playing = 0;
        voices_off(sep, seq, -1, -1);
    }
    au_unlock();
}

void au_seq_fade(int sep, int seq, int out, float seconds) {
    Seq *s;

    au_lock();
    if ((s = seq_of(sep, seq)) != NULL && seconds > 0) {
        s->fade_step = (out ? -1.0f : 1.0f) / (seconds * AU_RATE);
    }
    au_unlock();
}

int au_seq_playing(int sep, int seq) {
    Seq *s = seq_of(sep, seq);
    return s != NULL && s->playing;
}

static void event(int sep, int seq, Seq *s) {   /* the one at its place */
    int st, c, a, b;

    if (s->at >= s->length) {
        st = 0xFF, a = 0x2F;
        goto meta;
    }
    st = s->d[s->at];
    if (st & 0x80) {
        s->at++;
    } else {
        st = s->running;
    }
    if (st == 0xFF) {
        a = s->d[s->at++];
    meta:
        if (a == 0x51) {   /* the tempo */
            s->tempo = be(s->d + s->at, 3);
            s->at += 3;
        } else {           /* the end: again, or over */
            if (s->forever || --s->loops > 0) {
                rewind_seq(s);
                return;
            }
            s->playing = 0;
            voices_off(sep, seq, -1, -1);
            return;
        }
        s->wait += varlen(s);
        return;
    }
    s->running = (uint8_t)st;
    c = st & 15;
    switch (st & 0xF0) {
    case 0x80:
    case 0x90:
        a = s->d[s->at++];
        b = s->d[s->at++];
        voices_off(sep, seq, c, a);
        if ((st & 0xF0) == 0x90 && b != 0) {
            au_key_on_locked(AU_OWN_SEQ, sep, seq, c, sSeps[sep].bank, s->ch[c].program, -1, a, 0, b / 127.0f, 0);
        }
        break;
    case 0xA0:
        s->at += 2;
        break;
    case 0xB0:
        a = s->d[s->at++];
        b = s->d[s->at++];
        if (a == CC_VOLUME) {
            s->ch[c].volume = b;
        } else if (a == CC_PAN) {
            s->ch[c].pan = b;
        } else if (a == CC_EXPRESSION) {
            s->ch[c].expression = b;
        } else if (a == CC_NRPN) {
            s->nrpn = b;
            if (b == LOOP_END && s->loop_at >= 0 && (s->loop_count == 0 || --s->loop_count > 0)) {
                s->at = s->loop_at;   /* (at the time before the loop's first event) */
            }
        } else if (a == CC_DATA && s->nrpn == LOOP_START) {
            s->loop_count = b == FOREVER ? 0 : b;
            s->loop_at = s->at;
        }
        break;
    case 0xC0:
        s->ch[c].program = s->d[s->at++];
        break;
    case 0xD0:
        s->at++;
        break;
    case 0xE0:
        a = s->d[s->at++];
        b = s->d[s->at++];
        s->ch[c].bend = (b << 7 | a) - BEND_CENTRE;
        break;
    default:
        s->at++;
        break;
    }
    s->wait += varlen(s);
}

void sq_advance(int frames) {
    int i, j;

    for (i = 0; i < AU_SEPS; i++) {
        if (!sSeps[i].used) {
            continue;
        }
        for (j = 0; j < sSeps[i].n; j++) {
            Seq *s = &sSeps[i].s[j];
            int guard = EVENTS_A_BLOCK;

            if (!s->playing) {
                continue;
            }
            if (s->fade_step != 0) {
                s->fade += s->fade_step * (float)frames;
                if (s->fade <= 0) {
                    s->fade = 0, s->fade_step = 0;
                } else if (s->fade >= 1) {
                    s->fade = 1, s->fade_step = 0;
                }
            }
            s->wait -= frames * (double)s->per_quarter * 1e6 / (s->tempo > 0 ? s->tempo : DEFAULT_TEMPO) / AU_RATE;
            while (s->playing && s->wait <= 0 && guard-- > 0) {
                event(i, j, s);
            }
        }
    }
}

void sq_voice_gain(const AuVoice *v, float *l, float *r, int *bend) {
    Seq *s = seq_of(v->sep, v->seq);
    Channel *c;
    float g;
    int p;

    if (s == NULL) {
        *l = *r = 0;
        return;
    }
    c = &s->ch[v->channel];
    g = v->left * (c->volume / 127.0f) * (c->expression / 127.0f) * s->fade * s->volume;
    p = c->pan;
    *l = g * (p <= 64 ? 1.0f : (127 - p) / 63.0f);
    *r = g * (p >= 64 ? 1.0f : p / 64.0f);
    *bend = c->bend * 128 * (c->bend >= 0 ? v->t->bend_up : v->t->bend_down) / BEND_CENTRE;
}
