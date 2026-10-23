/* CD.BIN's entries in the library: each entry's parts as files, converted where their kind has a converter.
 *
 *   a container       a folder (`cdbin/033_4/`) with index.txt naming its parts in order, each part in it
 *   a TIM image       N.png (+ N.clut.png, N.tim.txt: data/tim.h)
 *   a text block      N.txt (data/text.h)
 *   anything else     N.<kind> as it is: .vab (a sound bank's header), .sep (sequences), .spl (a model),
 *                     .exe (a program), .bin (not known yet)
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

/* entry `name` ("033_4") into the library under cdbin/; 0 on failure (why says) */
int cdconv_entry(const char *name, const uint8_t *d, size_t n, CdconvWriteFn write, void *ctx, char *why, size_t nwhy);

/* the library's form of entry `name` against the entry itself: every part there, the images and containers
 * making the same bytes again, the text the same characters. 0 on a difference (why says where) */
int cdconv_check(const char *name, const uint8_t *d, size_t n, CdconvReadFn read, void *ctx, char *why, size_t nwhy);

#endif
