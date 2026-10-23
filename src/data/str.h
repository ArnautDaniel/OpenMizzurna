/* The game's movies: STR files (OPENING, PROLOGUE, M_DIGEST, M_BADEND, M_FINEND), read for the import from
 * their whole 2352-byte sectors. Video and sound sectors are interleaved; the drive reads 150 sectors a second
 * (double speed), and a frame is due when the sector that completes it has been read (the sound, every
 * sector's worth, plays alongside - data/xa.h decodes it).
 *
 * A video sector: its subheader without the audio bit; at 24 a 32-byte header {u16 0x0160, u16 0x8001,
 * u16 chunk, u16 chunks, u32 frame, u32 frame bytes, u16 width, u16 height, ...}, then 2016 bytes of the frame.
 * A frame (the MDEC's bitstream, versions 2 and 3): {u16 codes, u16 0x3800, u16 quantiser scale, u16 version},
 * then bits read from the top of little-endian 16-bit words. Macroblocks of 16 x 16 column by column (top to
 * bottom, then left to right; a height not a multiple of 16 padded), each six 8 x 8 blocks: Cr, Cb, then Y
 * top-left, top-right, bottom-left, bottom-right. A block: its DC (v2: 10 bits signed; v3: an MPEG-1 DC size
 * code and that many bits of difference from the last Y / Cb / Cr DC, times 4), then MPEG-1's coefficient codes
 * up to the end code "10" (the escape "000001": a 6-bit run, a 10-bit signed level). Dequantised as the MDEC
 * does - DC x 2, AC (level x scale x matrix + 4) / 8 - and an 8 x 8 inverse DCT; the colours are YCbCr as JPEG's
 * (full range, BT.601). */
#ifndef STR_H
#define STR_H

#include <stddef.h>
#include <stdint.h>

#define STR_SECTOR 2352
#define STR_SECTORS_PER_SECOND 150   /* the drive at double speed */

typedef struct StrMovie {
    const uint8_t *d;
    size_t n;
    int sectors;
    int width, height;
    int frames;
    int *first_sector;   /* a frame: its first sector */
    int *due_sector;     /* and the sector completing it (the frame is due once it is read) */
    int audio_file, audio_channel;   /* the sound's (-1: silent) */
} StrMovie;

/* the movie's frames (their sectors) and its sound's channel; 0 if it isn't a movie */
int str_open(StrMovie *m, const uint8_t *d, size_t n);
void str_close(StrMovie *m);
/* frame i as 8-bit full-range YCbCr 4:2:0 planes (width x height; chroma width/2 x (height+1)/2); 0 on a
 * damaged frame */
int str_frame(const StrMovie *m, int i, uint8_t *y, uint8_t *cb, uint8_t *cr);

#endif
