/* The XA stream (audio.h's au_xa_*): a channel of TSG.XA as the library keeps it (xa/TSG/<channel>.flac, its own
 * rate), decoded a little at a time in the mixer and brought to the mixer's rate, to its end. */
#include "mixer.h"

#include "../core/files.h"
#include "dr_flac.h"

#include <stdio.h>
#include <stdlib.h>

#define CHUNK 1024   /* frames decoded at a time */

static struct {
    uint8_t *file;
    drflac *flac;
    float volume;
    float chunk[CHUNK * 2];
    int have, channels;
    double at, step;   /* where in the chunk, and the stream's frames an output frame */
} sXa;

static void xa_close(void) {
    if (sXa.flac != NULL) {
        drflac_close(sXa.flac);
    }
    free(sXa.file);
    sXa.flac = NULL;
    sXa.file = NULL;
    sXa.have = 0;
}

void au_xa_play(int channel, float volume) {
    char path[64];
    size_t n = 0;
    uint8_t *file;
    drflac *flac;

    snprintf(path, sizeof(path), "xa/TSG/%d.flac", channel);
    file = files_read(path, &n);
    flac = file != NULL ? drflac_open_memory(file, n, NULL) : NULL;
    au_lock();
    xa_close();
    if (flac != NULL) {
        sXa.file = file;
        sXa.flac = flac;
        sXa.volume = volume;
        sXa.channels = flac->channels;
        sXa.step = (double)flac->sampleRate / AU_RATE;
        sXa.at = 0;
        sXa.have = 0;
    } else {
        free(file);
    }
    au_unlock();
}

void au_xa_stop(void) {
    au_lock();
    xa_close();
    au_unlock();
}

int au_xa_playing(void) { return sXa.flac != NULL; }

static float frame_at(int k, int c) { return sXa.chunk[k * 2 + c]; }   /* (the chunk is stereo) */

void xa_mix(float *out, int frames) {
    int n;

    for (n = 0; n < frames && sXa.flac != NULL; n++) {
        int k = (int)sXa.at;
        float f, l, r;

        while (k + 1 >= sXa.have) {   /* the next chunk (keeping the last frame for the blend) */
            float keep[2] = {0, 0};
            drflac_uint64 got;
            float buf[CHUNK * 8];   /* (up to 8 channels) */

            if (sXa.have > 0) {
                keep[0] = frame_at(sXa.have - 1, 0), keep[1] = frame_at(sXa.have - 1, 1);
            }
            got = drflac_read_pcm_frames_f32(sXa.flac, CHUNK - 1, buf);
            if (got == 0) {
                xa_close();
                return;
            }
            sXa.chunk[0] = keep[0], sXa.chunk[1] = keep[1];
            for (drflac_uint64 i = 0; i < got; i++) {   /* (mono: both sides the same) */
                float a = buf[i * (drflac_uint64)sXa.channels];
                float b = sXa.channels > 1 ? buf[i * (drflac_uint64)sXa.channels + 1] : a;
                sXa.chunk[(i + 1) * 2] = a, sXa.chunk[(i + 1) * 2 + 1] = b;
            }
            sXa.at -= (sXa.have > 0 ? sXa.have - 1 : 0);
            sXa.have = (int)got + 1;
            k = (int)sXa.at;
        }
        f = (float)(sXa.at - k);
        l = frame_at(k, 0) + (frame_at(k + 1, 0) - frame_at(k, 0)) * f;
        r = frame_at(k, 1) + (frame_at(k + 1, 1) - frame_at(k, 1)) * f;
        out[n * 2] += l * sXa.volume;
        out[n * 2 + 1] += r * sXa.volume;
        sXa.at += sXa.step;
    }
}
