/* The movies in the library: each STR (data/str.h) as
 *   movies/NAME.m2v   its pictures as MPEG-2 video (an elementary stream: ffmpeg, mpv, VLC play it), encoded once
 *                     at import with libavcodec at a fine fixed quantiser - the one conversion that isn't exact:
 *                     the check holds every frame to MOVIE_MIN_PSNR of the STR's own decoding;
 *   movies/NAME.flac  its sound (the XA sectors decoded), exact;
 *   movies/NAME.txt   om-movie 1 / size <w> <h> / rate <frames a second> / frames <n> / delay <seconds before
 *                     frame 0 shows> / sound <file> (none: silent).
 * The engine plays the .m2v and the .flac together; a mod's movie is any MPEG-1/2 video and a FLAC. Built
 * without libavcodec, the import can't encode and keeps the STR as it is (movieconv_available says). */
#ifndef MOVIECONV_H
#define MOVIECONV_H

#include <stddef.h>
#include <stdint.h>

#define MOVIE_RATE 15         /* frames a second: 10 sectors a frame at 150 sectors a second */
#define MOVIE_MIN_PSNR 38.0   /* dB: the encoded pictures against the STR's */

typedef int (*MovieWriteFn)(void *ctx, const char *path, const uint8_t *data, size_t n);
typedef uint8_t *(*MovieReadFn)(void *ctx, const char *path, size_t *n);

int movieconv_available(void);
/* movie `name` ("OPENING") from the STR's whole sectors; 0 on failure (why says) */
int movieconv_write(const char *name, const uint8_t *str, size_t n, MovieWriteFn write, void *ctx, char *why,
                    size_t nwhy);
/* the library's against the STR: the frames' count and pictures (the worst frame's PSNR in *psnr), the sound
 * sample for sample, the description */
int movieconv_check(const char *name, const uint8_t *str, size_t n, MovieReadFn read, void *ctx, double *psnr,
                    char *why, size_t nwhy);

#endif
