/* CD-XA audio (xa.h). Sector: subheader at 16 {file, channel, submode (4: audio), coding (bit 0 stereo, bit 2
 * 18.9 kHz, bit 4 8-bit samples)}; at 24, 18 sound groups of 128 bytes: the 8 units' filter and shift at bytes
 * 4..11, then 28 words of 4 bytes (unit u's 4-bit sample i in byte 16 + 4 i + u / 2, low nibble for even u).
 * Stereo: even units left, odd right. */
#include "xa.h"

#include <stdlib.h>
#include <string.h>

#define SUBHEADER 16
#define SUBMODE_AUDIO 0x04
#define CODING_STEREO 0x01
#define CODING_HALF_RATE 0x04
#define CODING_8BIT 0x10
#define GROUPS 18
#define GROUP_BYTES 128
#define UNITS 8
#define UNIT_SAMPLES 28
#define DATA_AT 24
#define RATE_FULL 37800
#define RATE_HALF 18900

static const int kK0[4] = {0, 60, 115, 98};
static const int kK1[4] = {0, 0, -52, -55};

uint32_t xa_channels(const uint8_t *d, size_t n, int file) {
    uint32_t mask = 0;
    size_t o;

    for (o = 0; o + XA_SECTOR <= n; o += XA_SECTOR) {
        const uint8_t *s = d + o + SUBHEADER;
        if (s[0] == file && (s[2] & SUBMODE_AUDIO) && s[1] < 32) {
            mask |= 1u << s[1];
        }
    }
    return mask;
}

static void unit(const uint8_t *g, int u, int *hist, int16_t *out, int stride) {
    int param = g[4 + u], shift = param & 0xF, f = param >> 4 & 3, i;

    for (i = 0; i < UNIT_SAMPLES; i++) {
        int nibble = g[16 + 4 * i + u / 2] >> (u % 2 * 4) & 0xF;
        int v = (int16_t)(nibble << 12) >> shift;

        v += (hist[0] * kK0[f] + hist[1] * kK1[f] + 32) >> 6;
        v = v < -32768 ? -32768 : v > 32767 ? 32767 : v;
        hist[1] = hist[0];
        hist[0] = v;
        out[i * stride] = (int16_t)v;
    }
}

int xa_decode(XaStream *s, const uint8_t *d, size_t n, int file, int channel) {
    size_t o, cap = 0;
    int hist[2][2] = {{0, 0}, {0, 0}};

    memset(s, 0, sizeof(*s));
    for (o = 0; o + XA_SECTOR <= n; o += XA_SECTOR) {
        const uint8_t *sub = d + o + SUBHEADER;
        int stereo, g, u;

        if (sub[0] != file || sub[1] != channel || !(sub[2] & SUBMODE_AUDIO) || (sub[3] & CODING_8BIT)) {
            continue;
        }
        stereo = sub[3] & CODING_STEREO;
        if (s->sectors == 0) {
            s->channels = stereo ? 2 : 1;
            s->rate = sub[3] & CODING_HALF_RATE ? RATE_HALF : RATE_FULL;
        }
        if ((size_t)(s->frames + GROUPS * UNITS * UNIT_SAMPLES) * 2 > cap) {
            cap = cap * 2 + (size_t)GROUPS * UNITS * UNIT_SAMPLES * 2;
            s->pcm = realloc(s->pcm, cap * sizeof(int16_t));
        }
        for (g = 0; g < GROUPS; g++) {
            const uint8_t *grp = d + o + DATA_AT + g * GROUP_BYTES;
            if (s->channels == 2) {
                for (u = 0; u < UNITS; u += 2) {
                    unit(grp, u, hist[0], s->pcm + s->frames * 2, 2);
                    unit(grp, u + 1, hist[1], s->pcm + s->frames * 2 + 1, 2);
                    s->frames += UNIT_SAMPLES;
                }
            } else {
                for (u = 0; u < UNITS; u++) {
                    unit(grp, u, hist[0], s->pcm + s->frames, 1);
                    s->frames += UNIT_SAMPLES;
                }
            }
        }
        s->sectors++;
    }
    return s->sectors > 0;
}

void xa_free(XaStream *s) {
    free(s->pcm);
    memset(s, 0, sizeof(*s));
}
