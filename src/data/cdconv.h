/* CD.BIN's entries in the library: each entry's parts as files, converted where their kind has a converter.
 *
 *   a container       a folder (`cdbin/033_4/`) with index.txt naming its parts in order, each part in it
 *   a TIM image       N.png (+ N.clut.png, N.tim.txt: data/tim.h)
 *   a text block      N.txt (data/text.h)
 *   a sound bank's header   N.bank.txt (data/vab.h); its body (another entry) M.samples/ - WAVs
 *   sequences         N.sep/ - MIDI files (data/seqmidi.h)
 *   anything else     N.<kind> as it is: .spl (a model), .exe (a program), .bin (not known yet)
 * An event pack (its script, then its text block) is kept as it is and its text also written as N.txt: the
 * scripts are converted to Forth once, at development time, and the engine reads only the text.
 *
 * index.txt:   om-container 1 / parts <n> / tail <zero bytes after the last> / part <i> <kind> <file> ... */
#ifndef CDCONV_H
#define CDCONV_H

#include <stddef.h>
#include <stdint.h>

typedef int (*CdconvWriteFn)(void *ctx, const char *path, const uint8_t *data, size_t n);
typedef uint8_t *(*CdconvReadFn)(void *ctx, const char *path, size_t *n);

/* sound banks' bodies: for each entry, the bank header its samples go with (NULL: not a body). A header names its
 * body by size; one in the same group (an area's NNN_1 for its NNN_2) is preferred. `entries`: every entry's
 * bytes (NULL when empty) */
void cdconv_pair_bodies(const uint8_t *const *entries, const size_t *sizes, int n, int kinds_per_group,
                        const uint8_t **header_of);

/* entry `name` into the library as a sound bank's body (the samples its header lists) */
int cdconv_body(const char *name, const uint8_t *vb, size_t n, const uint8_t *vh, CdconvWriteFn write, void *ctx,
                char *why, size_t nwhy);
int cdconv_check_body(const char *name, const uint8_t *vb, size_t n, const uint8_t *vh, CdconvReadFn read, void *ctx,
                      char *why, size_t nwhy);

/* entry `name` ("033_4") into the library under cdbin/; 0 on failure (why says) */
int cdconv_entry(const char *name, const uint8_t *d, size_t n, CdconvWriteFn write, void *ctx, char *why, size_t nwhy);

/* the library's form of entry `name` against the entry itself: every part there, the images and containers
 * making the same bytes again, the text the same characters. 0 on a difference (why says where) */
int cdconv_check(const char *name, const uint8_t *d, size_t n, CdconvReadFn read, void *ctx, char *why, size_t nwhy);

#endif
