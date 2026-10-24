/* The sound's engine words (audio/audio.h; docs/subsystems/sound.md). Volumes are the original's 0..127. */
#include "engine.h"

#include "../audio/audio.h"

#include <stdio.h>

#define PRIM(name) static void name(Forth *f, Word *w)
#define POP() forth_pop(f)
#define PUSH(x) forth_push(f, (Cell)(x))
#define FULL 127.0f
#define MS 1000.0f

static void path_of(Forth *f, char *buf, size_t size) {   /* ( addr len -- ) */
    Cell n = POP();
    const char *s = (const char *)POP();
    snprintf(buf, size, "%.*s", (int)n, s);
}

PRIM(p_bank_load) {   /* ( text len samples len slot -- flag ) a bank: its header's text, its samples' folder */
    Cell slot = POP();
    char text[200], samples[200];
    path_of(f, samples, sizeof(samples));
    path_of(f, text, sizeof(text));
    PUSH(au_bank_load((int)slot, text, samples) ? -1 : 0);
}
PRIM(p_bank_free) { au_bank_free((int)POP()); }   /* ( slot -- ) */
PRIM(p_key_on) {   /* ( owner bank program tone key volume -- voice | -1 ) tone -1: every tone covering the key */
    Cell vol = POP(), key = POP(), tone = POP(), prog = POP(), bank = POP(), owner = POP();
    PUSH(au_key_on((int)owner, (int)bank, (int)prog, (int)tone, (int)key, 0, vol / FULL, vol / FULL));
}
PRIM(p_key_off) { au_key_off((int)POP()); }   /* ( voice -- ) into its release */
PRIM(p_loop_off) {   /* ( bank program key -- ) every held loop of it, into release */
    Cell key = POP(), prog = POP();
    au_loop_off((int)POP(), (int)prog, (int)key);
}
PRIM(p_voice_pitch) {   /* ( voice key fine -- ) fine: 1/128 of a semitone */
    Cell fine = POP(), key = POP();
    au_voice_pitch((int)POP(), (int)key, (int)fine);
}
PRIM(p_voice_volume) { Cell v = POP(); au_voice_volume((int)POP(), v / FULL, v / FULL); }   /* ( voice volume -- ) */
PRIM(p_voice_on) { PUSH(au_voice_on((int)POP()) ? -1 : 0); }   /* ( voice -- flag ) */
PRIM(p_voices_on) { PUSH(au_voices_on()); }                    /* ( -- n ) */
PRIM(p_all_off) { au_all_off(); }
PRIM(p_master) { au_master(POP() / FULL); }                    /* ( volume -- ) */
PRIM(p_reverb) { Cell depth = POP(); au_reverb((int)POP(), depth / FULL); }   /* ( type depth -- ) */
PRIM(p_sep_load) {   /* ( folder len bank -- sep | -1 ) */
    Cell bank = POP();
    char folder[200];
    path_of(f, folder, sizeof(folder));
    PUSH(au_sep_load(folder, (int)bank));
}
PRIM(p_sep_free) { au_sep_free((int)POP()); }   /* ( sep -- ) */
PRIM(p_seq_play) {   /* ( sep seq loops volume -- ) loops 0: for ever */
    Cell vol = POP(), loops = POP(), seq = POP();
    au_seq_play((int)POP(), (int)seq, (int)loops, vol / FULL);
}
PRIM(p_seq_stop) { Cell seq = POP(); au_seq_stop((int)POP(), (int)seq); }   /* ( sep seq -- ) */
PRIM(p_seq_fade) {   /* ( sep seq out? ms -- ) out: to silence, else back to full */
    Cell ms = POP(), out = POP(), seq = POP();
    au_seq_fade((int)POP(), (int)seq, out != 0, ms / MS);
}
PRIM(p_seq_playing) { Cell seq = POP(); PUSH(au_seq_playing((int)POP(), (int)seq) ? -1 : 0); }   /* ( sep seq -- flag ) */
PRIM(p_xa_start) { Cell vol = POP(); au_xa_play((int)POP(), vol / FULL); }   /* ( channel volume -- ) */
PRIM(p_xa_end) { au_xa_stop(); }
PRIM(p_xa_playing) { PUSH(au_xa_playing() ? -1 : 0); }   /* ( -- flag ) */
PRIM(p_audio_record) {   /* ( path len frames -- flag ) the mixer on, its output into a WAV file (development) */
    Cell frames = POP();
    char path[300];
    path_of(f, path, sizeof(path));
    PUSH(au_record(path, (int)frames) ? -1 : 0);
}
PRIM(p_audio_run) { au_run((int)POP()); }   /* ( frames -- ) the mixer on by that many 44.1 kHz frames (no device: tests) */

void bind_audio(Forth *f) {
    static const struct { const char *name; void (*fn)(Forth *, Word *); } k[] = {
        {"bank-load", p_bank_load}, {"bank-free", p_bank_free}, {"key-on", p_key_on}, {"key-off", p_key_off},
        {"loop-off", p_loop_off}, {"voice-pitch", p_voice_pitch}, {"voice-volume", p_voice_volume},
        {"voice-on?", p_voice_on}, {"voices-on", p_voices_on}, {"all-voices-off", p_all_off},
        {"sound-volume", p_master}, {"reverb", p_reverb}, {"sep-load", p_sep_load}, {"sep-free", p_sep_free},
        {"seq-play", p_seq_play}, {"seq-stop", p_seq_stop}, {"seq-fade", p_seq_fade}, {"seq-playing?", p_seq_playing},
        {"xa-start", p_xa_start}, {"xa-end", p_xa_end}, {"xa-playing?", p_xa_playing}, {"audio-run", p_audio_run}, {"audio-record", p_audio_record},
    };
    size_t i;

    for (i = 0; i < sizeof(k) / sizeof(k[0]); i++) {
        forth_prim(f, k[i].name, k[i].fn);
    }
    forth_constant(f, "effect-voice", AU_OWN_EFFECT);
    forth_constant(f, "loop-voice", AU_OWN_LOOP);
    forth_constant(f, "audio-rate", AU_RATE);
}
