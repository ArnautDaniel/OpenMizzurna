/* A movie of the library (data/movieconv.h: movies/NAME.m2v, NAME.flac, NAME.txt): its pictures decoded a frame
 * at a time with libavcodec into a texture, at its rate (from its delay on), its sound played as the stream
 * beside it. One at a time. Built without libavcodec, none opens. */
#ifndef MOVIE_H
#define MOVIE_H

#include "../render/render.h"

#define MOVIE_ENDED -2   /* movie_advance: its last picture shown */
#define MOVIE_BEFORE -1  /* movie_advance: its first not yet due */

int movie_open(const char *name, float volume);   /* 0: no such movie (or no decoder) */
int movie_advance(double seconds);                /* on by that long: the picture now (or the above) */
GpuTexture movie_texture(void);                   /* the picture now (id 0: none yet) */
void movie_close(void);

#endif
