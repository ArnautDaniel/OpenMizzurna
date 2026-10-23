/* Sound banks (Sony's VAB): a header (VH) of programs - the instruments, each a set of tones that play a
 * sample over a range of keys with a volume, pan, tuning and envelope - and a body (VB) of the samples, in the
 * PlayStation's ADPCM.
 *
 * In the library:
 *   N.bank.txt      the header as text, every field named (the bytes come back from it)
 *   M.samples/      the body (another CD.BIN entry): <i>.wav for sample i (1 up; mono, 16-bit, 44100 Hz - a
 *                   tone plays its sample at that rate on its centre key), a loop as the WAV's smpl chunk, and
 *                   samples.txt listing them
 * A header names its body by size: the header's total size less its own is the body's. */
#ifndef VAB_H
#define VAB_H

#include <stddef.h>
#include <stdint.h>

#define VAB_PROGRAMS 128
#define VAB_TONES_PER_PROGRAM 16
#define VAB_SAMPLES 256
#define VAB_RATE 44100   /* the sound processor's rate: a sample plays at it on its tone's centre key */

/* a header's size from its counts; 0 if it isn't a VAB header */
size_t vab_header_size(const uint8_t *vh, size_t n);
/* the size of the body it goes with */
size_t vab_body_size(const uint8_t *vh);
int vab_sample_count(const uint8_t *vh);
/* sample i's bytes in the body (1 up) */
size_t vab_sample_bytes(const uint8_t *vh, int i);

/* the header as text (malloc'd), and back (the header's bytes, malloc'd); NULL on a malformed text (why says) */
char *vab_header_text(const uint8_t *vh, size_t n, const char *comment, size_t *len);
uint8_t *vab_header_from_text(const char *s, size_t n, size_t *size, char *why, size_t nwhy);

/* one sample's ADPCM decoded: its PCM (malloc'd), its length, and its loop (-1 none; else the loop starts at
 * loop_start and repeats from there after the last sample) */
int16_t *vab_decode_sample(const uint8_t *adpcm, size_t n, long *frames, long *loop_start);

/* a WAV file (malloc'd) of mono 16-bit PCM at `rate`, with a loop (loop_start < 0: none) */
uint8_t *vab_wav(const int16_t *pcm, long frames, int rate, long loop_start, size_t *size);
/* a WAV's samples (malloc'd) and loop; NULL if it isn't one of ours */
int16_t *vab_wav_read(const uint8_t *wav, size_t n, long *frames, long *loop_start);

#endif
