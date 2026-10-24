/* The mixer's own state, shared by its parts (audio.c, seq.c, xa.c); called with the lock held. */
#ifndef MIXER_H
#define MIXER_H

#include "audio.h"

#define AU_PROGRAMS 128
#define AU_TONES 16
#define AU_SAMPLES 256

typedef struct AuTone {
    uint8_t priority, reverb, volume, pan, centre, fine, low, high, bend_down, bend_up;
    uint16_t adsr1, adsr2;   /* the sound chip's envelope words, as the bank has them */
    int sample;
} AuTone;

typedef struct AuProgram {
    int ntones;
    uint8_t volume, pan;
    AuTone tone[AU_TONES];
} AuProgram;

typedef struct AuSample {
    int16_t *pcm;
    long length;
    long loop;   /* where it repeats from, or -1: once */
} AuSample;

typedef struct AuBank {
    int used;
    uint8_t volume, pan;
    AuProgram program[AU_PROGRAMS];
    AuSample sample[AU_SAMPLES];
} AuBank;

typedef struct AuVoice {
    int on;
    int owner, sep, seq, channel;   /* a sequence's: its set, sequence and MIDI channel */
    int bank, program, tone, key;
    int pitch;                      /* in 1/128 semitones (key * 128 + fine) */
    float left, right;              /* key-on volumes 0..1 (a sequence's: the velocity, in left) */
    const AuTone *t;
    const AuSample *s;
    double at;
    int phase, level, count;        /* the envelope: attack, decay, sustain, release; its level (0..0x7FFF) */
    unsigned age;
} AuVoice;

enum { ENV_ATTACK, ENV_DECAY, ENV_SUSTAIN, ENV_RELEASE };

extern AuBank au_banks[AU_BANKS];
extern AuVoice au_voices[AU_VOICES];

void au_lock(void);
void au_unlock(void);
int au_key_on_locked(int owner, int sep, int seq, int channel, int bank, int program, int tone, int key, int fine,
                     float left, float right);

/* seq.c */
void sq_advance(int frames);
void sq_voice_gain(const AuVoice *v, float *left, float *right, int *bend);
void sq_reset(void);

/* xa.c */
void xa_mix(float *out, int frames);

#endif
