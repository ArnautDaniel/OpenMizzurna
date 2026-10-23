/* CD-XA audio: the music and voices of TSG.XA (and the movies' sound), interleaved on the disc - each sector
 * belongs to a file number and channel (its subheader), and a stream is one channel's sectors in order, ADPCM
 * coded (4 bits a sample, 18 groups of 128 bytes a sector).
 *
 * In the library TSG.XA becomes a FLAC a channel (xa/TSG/<channel>.flac), with its rate and channels; the game
 * plays a channel from its start to its end. */
#ifndef XA_H
#define XA_H

#include <stddef.h>
#include <stdint.h>

#define XA_SECTOR 2352

typedef struct XaStream {
    int16_t *pcm;      /* interleaved when stereo */
    long frames;
    int channels;      /* 1 or 2 */
    int rate;          /* 37800 or 18900 */
    int sectors;       /* the stream's own sectors */
} XaStream;

/* the channels (bitmask) of file `file` that have audio in a raw XA file (whole sectors) */
uint32_t xa_channels(const uint8_t *d, size_t n, int file);
/* one channel's audio decoded; 0 if it has none */
int xa_decode(XaStream *s, const uint8_t *d, size_t n, int file, int channel);
void xa_free(XaStream *s);

#endif
