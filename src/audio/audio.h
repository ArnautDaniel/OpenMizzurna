/* The game's sound, on SDL3 audio (docs/subsystems/sound.md): what the original's sound calls intend, not its
 * sound chip. Banks (the library's N.bank.txt and M.samples/) hold programs - each a set of tones playing a sample
 * over a range of keys, with a volume, pan, tuning and envelope; voices play a tone's sample at a key's pitch with
 * the tone's envelope (the sound chip's attack / decay / sustain / release rules, so the banks' values keep their
 * timing); sequences (the library's N.sep/, as the original's bytes) play on a small sequencer; a generic reverb
 * stands for the chip's; and the XA streams (the library's xa/TSG/<channel>.flac) play beside them. Everything is
 * mixed in float at 44.1 kHz. All calls are safe from the game's thread (the mixer runs on SDL's). */
#ifndef AUDIO_H
#define AUDIO_H

#include <stdint.h>

#define AU_RATE 44100     /* the mixer's rate, and the banks' samples' (a sample plays at it on its centre key) */
#define AU_VOICES 24      /* the sound chip's voices */
#define AU_BANKS 8        /* banks open at once (the game's: common, area, music, ...) */
#define AU_SEPS 8         /* sequence sets open at once */
#define AU_SEQS 16        /* sequences in a set */

enum { AU_OWN_EFFECT, AU_OWN_LOOP, AU_OWN_SEQ };   /* who keyed a voice on: a one-shot, a held loop, a sequence */

/* start the mixer (no device: everything still works, nothing is heard); 0 if no device */
int au_init(int want_device);
void au_quit(void);

/* a bank into slot (its header's text and its samples' folder, library paths); 0 on failure */
int au_bank_load(int slot, const char *bank_text, const char *samples);
void au_bank_free(int slot);

/* key on: tone `tone` of the program, or every tone covering the key when tone < 0; volume 0..1 each side;
 * returns the first voice or -1 */
int au_key_on(int owner, int bank, int program, int tone, int key, int fine, float left, float right);
void au_key_off(int voice);                      /* into its release */
void au_loop_off(int bank, int program, int key);   /* every held loop of that program and key, into release */
void au_voice_pitch(int voice, int key, int fine);
void au_voice_volume(int voice, float left, float right);
int au_voice_on(int voice);                       /* still sounding */
int au_voices_on(void);                           /* how many sound */
void au_all_off(void);
void au_master(float volume);
void au_reverb(int type, float depth);           /* the original's reverb types (0 off .. 9) */

/* sequence sets: a SEP (its folder in the library) played on a bank; returns the set or -1 */
int au_sep_load(const char *folder, int bank);
void au_sep_free(int sep);
void au_seq_play(int sep, int seq, int loops, float volume);   /* loops 0: for ever */
void au_seq_stop(int sep, int seq);
void au_seq_fade(int sep, int seq, int out, float seconds);    /* out: to silence, else back to full */
int au_seq_playing(int sep, int seq);

/* an XA stream: the library's xa/TSG/<channel>.flac at a volume, to its end */
void au_xa_play(int channel, float volume);
/* any FLAC of the library the same way (a movie's sound) */
void au_flac_play(const char *path, float volume);
void au_xa_stop(void);
int au_xa_playing(void);

/* advance the mixer by frames with no device (tests): voices, sequences and the stream move on */
void au_run(int frames);
/* the same, the output written to a WAV file (a development aid: hearing it headless); 0 if it can't be written */
int au_record(const char *path, int frames);

#endif
