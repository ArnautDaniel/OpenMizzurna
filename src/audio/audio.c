/* The mixer (audio.h): banks from the library, 24 voices with the sound chip's envelope rules, a generic reverb,
 * the sequencer (seq.c) and the XA stream (xa.c) mixed in, on an SDL3 audio stream. */
#include "mixer.h"

#include "../core/files.h"
#include "../data/vab.h"

#include <SDL3/SDL.h>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

AuBank au_banks[AU_BANKS];
AuVoice au_voices[AU_VOICES];

static SDL_Mutex *sLock;
static SDL_AudioStream *sStream;
static float sMaster = 1.0f;
static unsigned sAge;

void au_lock(void) { if (sLock) SDL_LockMutex(sLock); }
void au_unlock(void) { if (sLock) SDL_UnlockMutex(sLock); }

/* ---- banks: the header's text back to its bytes (data/vab.c), then its programs and tones ---- */
#define VH_HEADER 32
#define VH_PROGRAM 16
#define VH_TONE 32
#define VH_SAMPLES 22         /* u16 */
#define VH_VOLUME 24
#define VH_PAN 25
#define TONE_REVERB 4         /* the tone's mode: reverb on */

static uint16_t le16(const uint8_t *p) { return (uint16_t)(p[0] | p[1] << 8); }

static void read_programs(AuBank *b, const uint8_t *vh) {
    int p, k = 0, t;

    b->volume = vh[VH_VOLUME];
    b->pan = vh[VH_PAN];
    for (p = 0; p < AU_PROGRAMS; p++) {
        const uint8_t *pa = vh + VH_HEADER + p * VH_PROGRAM;
        AuProgram *pr = &b->program[p];

        if (pa[0] == 0) {
            continue;
        }
        pr->ntones = pa[0] > AU_TONES ? AU_TONES : pa[0];
        pr->volume = pa[1];
        pr->pan = pa[4];
        for (t = 0; t < pr->ntones; t++) {
            const uint8_t *ta = vh + VH_HEADER + AU_PROGRAMS * VH_PROGRAM + (k * AU_TONES + t) * VH_TONE;
            AuTone *to = &pr->tone[t];

            to->priority = ta[0];
            to->reverb = (ta[1] & TONE_REVERB) != 0;
            to->volume = ta[2];
            to->pan = ta[3];
            to->centre = ta[4];
            to->fine = ta[5];
            to->low = ta[6];
            to->high = ta[7];
            to->bend_down = ta[12];
            to->bend_up = ta[13];
            to->adsr1 = le16(ta + 16);
            to->adsr2 = le16(ta + 18);
            to->sample = (int16_t)le16(ta + 22);
        }
        k++;
    }
}

int au_bank_load(int slot, const char *bank_text, const char *samples) {
    size_t n = 0, vhn = 0;
    uint8_t *text, *vh;
    char why[256], path[300];
    AuBank *b;
    int i, count;

    if (slot < 0 || slot >= AU_BANKS || (text = files_read(bank_text, &n)) == NULL) {
        return 0;
    }
    vh = vab_header_from_text((const char *)text, n, &vhn, why, sizeof(why));
    free(text);
    if (vh == NULL) {
        return 0;
    }
    count = le16(vh + VH_SAMPLES);
    b = calloc(1, sizeof(*b));   /* (read whole before the mixer sees it: no disk while it waits) */
    if (b == NULL) {
        free(vh);
        return 0;
    }
    read_programs(b, vh);
    for (i = 1; i <= count && i < AU_SAMPLES; i++) {   /* (sample 0 is none) */
        uint8_t *wav;
        size_t wn = 0;

        snprintf(path, sizeof(path), "%s/%d.wav", samples, i);
        if ((wav = files_read(path, &wn)) != NULL) {
            b->sample[i].pcm = vab_wav_read(wav, wn, &b->sample[i].length, &b->sample[i].loop);
            free(wav);
        }
    }
    b->used = 1;
    au_bank_free(slot);
    au_lock();
    au_banks[slot] = *b;
    au_unlock();
    free(b);
    free(vh);
    return 1;
}

void au_bank_free(int slot) {
    int i;

    if (slot < 0 || slot >= AU_BANKS) {
        return;
    }
    au_lock();
    for (i = 0; i < AU_VOICES; i++) {
        if (au_voices[i].on && au_voices[i].bank == slot) {
            au_voices[i].on = 0;
        }
    }
    for (i = 0; i < AU_SAMPLES; i++) {
        free(au_banks[slot].sample[i].pcm);
    }
    memset(&au_banks[slot], 0, sizeof(au_banks[slot]));
    au_unlock();
}

/* ---- voices ---- */
static int voice_for(int priority) {   /* a free one; else the quietest releasing; else the oldest least important */
    int i, best = -1, quietest = 0x10000;

    for (i = 0; i < AU_VOICES; i++) {
        if (!au_voices[i].on) {
            return i;
        }
    }
    for (i = 0; i < AU_VOICES; i++) {
        if (au_voices[i].phase == ENV_RELEASE && au_voices[i].level < quietest) {
            best = i;
            quietest = au_voices[i].level;
        }
    }
    if (best >= 0) {
        return best;
    }
    for (i = 0; i < AU_VOICES; i++) {
        AuVoice *v = &au_voices[i];

        if (v->t->priority > priority) {
            continue;
        }
        if (best < 0 || v->t->priority < au_voices[best].t->priority ||
            (v->t->priority == au_voices[best].t->priority && v->age < au_voices[best].age)) {
            best = i;
        }
    }
    return best;
}

int au_key_on_locked(int owner, int sep, int seq, int channel, int bank, int program, int tone, int key, int fine,
                     float left, float right) {
    AuProgram *p;
    int t, first = -1;

    if (bank < 0 || bank >= AU_BANKS || !au_banks[bank].used || program < 0 || program >= AU_PROGRAMS) {
        return -1;
    }
    p = &au_banks[bank].program[program];
    for (t = 0; t < p->ntones; t++) {
        const AuTone *to = &p->tone[t];
        const AuSample *s;
        AuVoice *v;
        int i;

        if (tone >= 0 ? t != tone : (key < to->low || key > to->high)) {
            continue;
        }
        if (to->sample <= 0 || to->sample >= AU_SAMPLES) {
            continue;
        }
        s = &au_banks[bank].sample[to->sample];
        if (s->length == 0 || s->pcm == NULL || (i = voice_for(to->priority)) < 0) {
            continue;
        }
        v = &au_voices[i];
        memset(v, 0, sizeof(*v));
        v->on = 1;
        v->owner = owner, v->sep = sep, v->seq = seq, v->channel = channel;
        v->bank = bank, v->program = program, v->tone = t, v->key = key;
        v->pitch = key * 128 + fine;
        v->left = left, v->right = right;
        v->t = to, v->s = s;
        v->age = sAge++;
        if (first < 0) {
            first = i;
        }
    }
    return first;
}

int au_key_on(int owner, int bank, int program, int tone, int key, int fine, float left, float right) {
    int v;

    au_lock();
    v = au_key_on_locked(owner, -1, -1, -1, bank, program, tone, key, fine, left, right);
    au_unlock();
    return v;
}

void au_key_off(int i) {
    au_lock();
    if (i >= 0 && i < AU_VOICES && au_voices[i].on) {
        au_voices[i].phase = ENV_RELEASE;
    }
    au_unlock();
}

void au_loop_off(int bank, int program, int key) {
    int i;

    au_lock();
    for (i = 0; i < AU_VOICES; i++) {
        AuVoice *v = &au_voices[i];

        if (v->on && v->owner == AU_OWN_LOOP && v->bank == bank && v->program == program && v->key == key) {
            v->phase = ENV_RELEASE;
        }
    }
    au_unlock();
}

void au_voice_pitch(int i, int key, int fine) {
    au_lock();
    if (i >= 0 && i < AU_VOICES) {
        au_voices[i].pitch = key * 128 + fine;
    }
    au_unlock();
}

void au_voice_volume(int i, float left, float right) {
    au_lock();
    if (i >= 0 && i < AU_VOICES) {
        au_voices[i].left = left, au_voices[i].right = right;
    }
    au_unlock();
}

int au_voice_on(int i) { return i >= 0 && i < AU_VOICES && au_voices[i].on; }

int au_voices_on(void) {
    int i, n = 0;

    for (i = 0; i < AU_VOICES; i++) {
        n += au_voices[i].on != 0;
    }
    return n;
}

void au_all_off(void) {
    int i;

    au_lock();
    for (i = 0; i < AU_VOICES; i++) {
        au_voices[i].on = 0;
    }
    au_unlock();
}

void au_master(float volume) {
    au_lock();
    sMaster = volume;
    au_unlock();
}

/* ---- the envelope: a step each output sample, by the sound chip's rate rules ---- */
#define ENV_TOP 0x7FFF
#define ENV_RATE_SPLIT 11     /* shifts above it slow the steps, below it enlarge them */
#define ENV_EXP_KNEE 0x6000   /* an exponential attack slows above it */
#define SUSTAIN_UNIT 0x800    /* the sustain level's steps */

static void envelope_step(AuVoice *v) {
    uint16_t a1 = v->t->adsr1, a2 = v->t->adsr2;
    int expo, down, shift, step, cycles, delta;

    switch (v->phase) {
    case ENV_ATTACK:
        expo = a1 >> 15, down = 0, shift = (a1 >> 10) & 31, step = 7 - ((a1 >> 8) & 3);
        break;
    case ENV_DECAY:
        expo = 1, down = 1, shift = (a1 >> 4) & 15, step = -8;
        break;
    case ENV_SUSTAIN:
        expo = a2 >> 15, down = (a2 >> 14) & 1, shift = (a2 >> 8) & 31;
        step = down ? -8 + ((a2 >> 6) & 3) : 7 - ((a2 >> 6) & 3);
        break;
    default:
        expo = (a2 >> 5) & 1, down = 1, shift = a2 & 31, step = -8;
        break;
    }
    cycles = 1 << (shift > ENV_RATE_SPLIT ? shift - ENV_RATE_SPLIT : 0);
    delta = step * (1 << (shift < ENV_RATE_SPLIT ? ENV_RATE_SPLIT - shift : 0));
    if (expo && !down && v->level > ENV_EXP_KNEE) {
        cycles *= 4;
    }
    if (expo && down) {
        delta = delta * v->level >> 15;
    }
    if (++v->count < cycles) {
        return;
    }
    v->count = 0;
    v->level += delta;
    v->level = v->level > ENV_TOP ? ENV_TOP : v->level < 0 ? 0 : v->level;
    if (v->phase == ENV_ATTACK && v->level >= ENV_TOP) {
        v->phase = ENV_DECAY;
    } else if (v->phase == ENV_DECAY && v->level <= ((a1 & 15) + 1) * SUSTAIN_UNIT) {
        v->phase = ENV_SUSTAIN;
    } else if (v->phase == ENV_RELEASE && v->level == 0) {
        v->on = 0;
    }
}

#define PAN_CENTRE 64
#define PAN_RIGHT 127
static void pan_gain(int p, float *l, float *r) {
    p = p < 0 ? 0 : p > PAN_RIGHT ? PAN_RIGHT : p;
    *l = p <= PAN_CENTRE ? 1.0f : (float)(PAN_RIGHT - p) / (PAN_RIGHT - PAN_CENTRE);
    *r = p >= PAN_CENTRE ? 1.0f : (float)p / PAN_CENTRE;
}

/* ---- the reverb: a small room (combs and allpasses) or an echo, by the original's reverb types ---- */
static const struct { float room, damp, echo_seconds, echo_feedback; } kReverb[10] = {
    {0, 0, 0, 0},           /* off */
    {0.70f, 0.40f, 0, 0},   /* room */
    {0.72f, 0.35f, 0, 0},   /* studio, small */
    {0.78f, 0.30f, 0, 0},   /* studio, medium */
    {0.82f, 0.30f, 0, 0},   /* studio, large */
    {0.88f, 0.25f, 0, 0},   /* hall */
    {0.92f, 0.20f, 0, 0},   /* space echo */
    {0, 0, 0.40f, 0.50f},   /* echo */
    {0, 0, 0.40f, 0.00f},   /* delay */
    {0.75f, 0.05f, 0, 0},   /* pipe */
};
static const int kCombs[4] = {1116, 1188, 1277, 1356};
static const int kAllpasses[2] = {556, 441};
#define STEREO_SPREAD 23
static struct {
    int type;
    float depth;
    float comb[2][4][1400], comb_low[2][4];
    int comb_at[2][4];
    float allpass[2][2][600];
    int allpass_at[2][2];
    float echo[2][AU_RATE / 2];
    int echo_at;
} sReverb;

void au_reverb(int type, float depth) {
    au_lock();
    if (type != sReverb.type) {
        memset(&sReverb, 0, sizeof(sReverb));
    }
    sReverb.type = type < 0 || type > 9 ? 0 : type;
    sReverb.depth = depth;
    au_unlock();
}

int au_reverb_type(void) { return sReverb.type; }

static void reverb_run(const float *in, float *out, int frames) {
    int i, c, k, type = sReverb.type;

    if (type == 0) {
        return;
    }
    for (i = 0; i < frames; i++) {
        for (c = 0; c < 2; c++) {
            float x = in[i * 2 + c] * 0.5f, y = 0;

            if (kReverb[type].echo_seconds > 0) {
                int len = (int)(kReverb[type].echo_seconds * AU_RATE);
                float *e = &sReverb.echo[c][sReverb.echo_at % len];
                y = *e;
                *e = x + y * kReverb[type].echo_feedback;
            } else {
                for (k = 0; k < 4; k++) {
                    int len = kCombs[k] + c * STEREO_SPREAD;
                    float *p = &sReverb.comb[c][k][sReverb.comb_at[c][k]], o = *p;
                    sReverb.comb_low[c][k] = o * (1 - kReverb[type].damp) + sReverb.comb_low[c][k] * kReverb[type].damp;
                    *p = x + sReverb.comb_low[c][k] * kReverb[type].room;
                    if (++sReverb.comb_at[c][k] >= len) {
                        sReverb.comb_at[c][k] = 0;
                    }
                    y += o;
                }
                y *= 0.25f;
                for (k = 0; k < 2; k++) {
                    int len = kAllpasses[k] + c * STEREO_SPREAD;
                    float *p = &sReverb.allpass[c][k][sReverb.allpass_at[c][k]], b = *p;
                    *p = y + b * 0.5f;
                    y = b - y;
                    if (++sReverb.allpass_at[c][k] >= len) {
                        sReverb.allpass_at[c][k] = 0;
                    }
                }
            }
            out[i * 2 + c] += y * sReverb.depth * 2.0f;
        }
        if (++sReverb.echo_at >= AU_RATE * 4) {
            sReverb.echo_at = 0;
        }
    }
}

/* ---- mixing ---- */
#define BLOCK 128
#define VOICE_HEADROOM 0.5f
#define SEMITONE 128
#define OCTAVE (12 * SEMITONE)

static void mix_block(float *out, int frames) {
    static float send[BLOCK * 2];
    int i, n;

    memset(out, 0, sizeof(float) * 2 * (size_t)frames);
    memset(send, 0, sizeof(float) * 2 * (size_t)frames);
    sq_advance(frames);
    for (i = 0; i < AU_VOICES; i++) {
        AuVoice *v = &au_voices[i];
        AuBank *b;
        AuProgram *p;
        float gl, gr, pl, pr, g;
        double step;
        int bend = 0;

        if (!v->on) {
            continue;
        }
        b = &au_banks[v->bank];
        p = &b->program[v->program];
        if (v->owner == AU_OWN_SEQ) {
            sq_voice_gain(v, &gl, &gr, &bend);
        } else {
            gl = v->left, gr = v->right;
        }
        pan_gain(v->t->pan + p->pan - PAN_CENTRE, &pl, &pr);
        g = (v->t->volume / 127.0f) * (p->volume / 127.0f) * (b->volume / 127.0f) * VOICE_HEADROOM;
        gl *= pl * g, gr *= pr * g;
        step = pow(2.0, (double)(v->pitch + bend - (v->t->centre * SEMITONE + v->t->fine)) / OCTAVE);
        for (n = 0; n < frames && v->on; n++) {
            const AuSample *s = v->s;
            long at = (long)v->at, next = at + 1;
            float f = (float)(v->at - (double)at), x, e;

            if (next >= s->length) {
                next = s->loop >= 0 ? s->loop : at;
            }
            x = (s->pcm[at] + (s->pcm[next] - s->pcm[at]) * f) * (1.0f / 32768.0f);
            envelope_step(v);
            e = v->level * (1.0f / ENV_TOP) * x;
            out[n * 2] += e * gl, out[n * 2 + 1] += e * gr;
            if (v->t->reverb) {
                send[n * 2] += e * gl, send[n * 2 + 1] += e * gr;
            }
            v->at += step;
            if (v->at >= (double)s->length) {
                if (s->loop < 0) {
                    v->on = 0;
                } else {
                    v->at = (double)s->loop + fmod(v->at - (double)s->length, (double)(s->length - s->loop));
                }
            }
        }
    }
    reverb_run(send, out, frames);
    xa_mix(out, frames);
    for (n = 0; n < frames * 2; n++) {
        float x = out[n] * sMaster;
        out[n] = x < -1 ? -1 : x > 1 ? 1 : x;
    }
}

void au_run(int frames) {
    float buf[BLOCK * 2];

    while (frames > 0) {
        int k = frames > BLOCK ? BLOCK : frames;
        au_lock();
        mix_block(buf, k);
        au_unlock();
        frames -= k;
    }
}

int au_record(const char *path, int frames) {
    static const uint8_t header[44] = {'R', 'I', 'F', 'F', 0, 0, 0, 0, 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ', 16, 0, 0, 0,
                                       1, 0, 2, 0, 0x44, 0xAC, 0, 0, 0x10, 0xB1, 2, 0, 4, 0, 16, 0,
                                       'd', 'a', 't', 'a', 0, 0, 0, 0};   /* 16-bit stereo at 44.1 kHz */
    float buf[BLOCK * 2];
    int16_t pcm[BLOCK * 2];
    uint8_t h[44];
    uint32_t bytes = (uint32_t)frames * 4, riff = bytes + 36;
    FILE *f = fopen(path, "wb");
    int i;

    if (f == NULL) {
        return 0;
    }
    memcpy(h, header, sizeof(h));
    memcpy(h + 4, &riff, 4);
    memcpy(h + 40, &bytes, 4);
    fwrite(h, 1, sizeof(h), f);
    while (frames > 0) {
        int k = frames > BLOCK ? BLOCK : frames;
        au_lock();
        mix_block(buf, k);
        au_unlock();
        for (i = 0; i < k * 2; i++) {
            pcm[i] = (int16_t)(buf[i] * 32767);
        }
        fwrite(pcm, 4, (size_t)k, f);
        frames -= k;
    }
    fclose(f);
    return 1;
}

static void SDLCALL feed(void *user, SDL_AudioStream *st, int wanted, int total) {
    float buf[BLOCK * 2];
    (void)user, (void)total;

    while (wanted > 0) {
        int frames = wanted / (int)(2 * sizeof(float));
        frames = frames > BLOCK ? BLOCK : frames < 1 ? 1 : frames;
        au_lock();
        mix_block(buf, frames);
        au_unlock();
        SDL_PutAudioStreamData(st, buf, frames * (int)(2 * sizeof(float)));
        wanted -= frames * (int)(2 * sizeof(float));
    }
}

int au_init(int want_device) {
    SDL_AudioSpec spec = {SDL_AUDIO_F32, 2, AU_RATE};

    if (sLock == NULL) {
        sLock = SDL_CreateMutex();
    }
    if (!want_device || sStream != NULL) {
        return sStream != NULL;
    }
    if (!SDL_InitSubSystem(SDL_INIT_AUDIO)) {
        return 0;
    }
    sStream = SDL_OpenAudioDeviceStream(SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK, &spec, feed, NULL);
    if (sStream == NULL) {
        fprintf(stderr, "audio: %s\n", SDL_GetError());
        return 0;
    }
    SDL_ResumeAudioStreamDevice(sStream);
    return 1;
}

void au_quit(void) {
    int i;

    if (sStream != NULL) {
        SDL_DestroyAudioStream(sStream);
        sStream = NULL;
    }
    au_xa_stop();
    for (i = 0; i < AU_BANKS; i++) {
        au_bank_free(i);
    }
    sq_reset();
}
