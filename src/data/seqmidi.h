/* Sequences (Sony's SEP: one or more SEQ sequences - MIDI events with Sony's own header) and their form in the
 * library: Standard MIDI files.
 *
 * A SEP: "pQES", u16 version, then each sequence: u16 id, u16 ticks a quarter note, u24 tempo (microseconds a
 * quarter note), u16 rhythm (time signature: numerator, then the denominator's power of 2), u32 bytes, and its
 * events (big-endian throughout). The events are MIDI's, but for the tempo change (FF 51 and three bytes, no length
 * byte) and running status carrying across a tempo change. Loops are MIDI controller messages (NRPN 99 = 20 with
 * data entry n: the loop's start, n its count; 99 = 30: its end) and are kept as they are.
 *
 * In the library a SEP `N` becomes a folder N.sep/: index.txt (the version, each sequence's id) and <i>.mid for
 * sequence i - a type-0 MIDI file whose first two events set the header's tempo and time signature. */
#ifndef SEQMIDI_H
#define SEQMIDI_H

#include <stddef.h>
#include <stdint.h>

/* how many sequences a SEP holds; 0 if it isn't one */
int sep_sequences(const uint8_t *d, size_t n);
/* sequence i as a MIDI file (malloc'd) */
uint8_t *sep_to_midi(const uint8_t *d, size_t n, int i, size_t *size);
/* index.txt's text (malloc'd) */
char *sep_index_text(const uint8_t *d, size_t n, const char *comment, size_t *size);

/* the SEP made again from index.txt and its MIDI files (read through `read`, paths "<folder>/<i>.mid")
 * (malloc'd); NULL on a malformed file (why says) */
typedef uint8_t *(*SepReadFn)(void *ctx, const char *path, size_t *n);
uint8_t *sep_from_folder(const char *folder, SepReadFn read, void *ctx, size_t *size, char *why, size_t nwhy);

#endif
