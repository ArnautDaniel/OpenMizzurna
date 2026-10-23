/* Snow (engine words; docs/subsystems/snow.md; src: snow.c - its flakes' loops 0x8003C90C, 0x8003CB48). Up to 512
 * flakes, falling a game frame at a time and drawn as small sprites facing the camera:
 *   around the camera (the town: kind 0) - a wrapping cube of 8192 world units about it, the flakes from 2048 to
 *   4095 above the ground (the camera's pivot) falling 14 - 17 a frame, swaying by the wobble table;
 *   from emitters (a scene's windows: kind 1) - up to 4 places, each its own cube of 4096 turned by its heading,
 *   every 4th flake its.
 * A landed flake lies 6 frames and starts again on top while the live count lasts. The scripts set the scene's kind,
 * its emitters and the count (it eases toward 300 when it snows: daylight.fs). */
#include "engine.h"

#include "../core/files.h"
#include "../render/scene3d.h"

#include <math.h>
#include <stdlib.h>
#include <string.h>

#define PRIM(name) static void name(Forth *f, Word *w)
#define POP() forth_pop(f)
#define PUSH(x) forth_push(f, (Cell)(x))

#define FLAKES 512
#define EMITTERS 4
#define AROUND 0                 /* the scene's kinds */
#define FROM_EMITTERS 1
#define WORLD_CUBE 0x1FFF        /* around the camera: 8192 */
#define LOCAL_CUBE 0xFFF         /* an emitter's: 4096 */
#define HALF_WORLD 4096
#define HIGH 0x7FF               /* the flakes start up to this far above ... */
#define HIGHER 0x800             /* ... and new ones this much more */
#define FALL 14                  /* a frame, and up to 3 more */
#define DRIFT 5                  /* and across, with the sway */
#define LYING 6                  /* frames a landed flake lies */
#define LANDED 0x20              /* its sprites' second band */
#define WOBBLE_STEPS 32
#define SPRITE_PIXELS 8          /* on the original's screen */
#define SHEET_W 256.0f           /* the snow's sheet (sky/snow.png) */
#define SHEET_H 64.0f
#define FAR 8192                 /* nearer than this they're drawn; the sprite's column by nearness (32 a step) */
#define SHADE 0.5f               /* src: 0x40 of 0x80, added to the picture */
#define PS_ADD 2                 /* (scene3d's blends: the PlayStation's add) */

typedef struct Flake {
    float x, y, z;
    int phase, state;
} Flake;

static Flake sFlakes[FLAKES];
static int sLive, sKind = -1, sEmitters;
static float sEmitter[EMITTERS][3];   /* x, z, heading */
static uint32_t sRand = 0x1234567u;

static int next_rand(void) {
    sRand = sRand * 1103515245u + 12345u;
    return (int)(sRand >> 8 & 0xFFFF);
}

static void spawn(Flake *k, int cube, int higher) {
    k->x = (float)(next_rand() & cube);
    k->y = (float)(-(next_rand() & HIGH) - higher);
    k->z = (float)(next_rand() & cube);
    k->phase = next_rand() & (WOBBLE_STEPS - 1);
    k->state = 0;
}

static int cube_of(void) { return sKind == FROM_EMITTERS ? LOCAL_CUBE : WORLD_CUBE; }

/* a scene's snow: the live flakes in the air, the rest lying idle (src: func_8003AE04) */
static void scene(int kind) {
    int i;

    sKind = kind;
    for (i = 0; i < FLAKES; i++) {
        if (i < sLive) {
            spawn(&sFlakes[i], cube_of(), 0);
        } else {
            sFlakes[i].state = LANDED;
            sFlakes[i].phase = LYING;
        }
    }
}

/* a frame of their fall (src: the flakes' loop: the sway, landing, starting again while the count lasts) */
static void step(Flake *k, int budget, const signed char *wob) {
    if (k->state != 0 && k->phase == LYING) {
        return;
    }
    k->phase++;
    if (k->y < 0) {
        k->phase &= WOBBLE_STEPS - 1;
        k->x += DRIFT + (wob ? wob[k->phase * 2] : 0);
        k->y += FALL + (next_rand() & 3);
        k->z += DRIFT + (wob ? wob[k->phase * 2 + 1] : 0);
        if (sKind == FROM_EMITTERS) {
            k->x = (float)((int)k->x & LOCAL_CUBE);
            k->z = (float)((int)k->z & LOCAL_CUBE);
        }
        if (k->y >= 0) {
            k->y = 0;
            k->state = LANDED;
            k->phase = 0;
        }
    } else if (k->phase >= LYING && budget > 0) {
        spawn(k, cube_of(), 0);
        k->y = -HIGH;
    }
}

/* where a flake is in the world */
static void place(const Flake *k, int i, const float pivot[3], float out[3]) {
    if (sKind == FROM_EMITTERS) {
        const float *e = sEmitter[i % EMITTERS];
        float a = e[2] * 2.0f * (float)M_PI / 4096.0f, c = cosf(a), s = sinf(a);
        out[0] = e[0] + c * k->x + s * k->z;
        out[2] = e[1] - s * k->x + c * k->z;
    } else {   /* (wrapped to within half the cube of the camera) */
        int dx = (((int)k->x - (int)pivot[0]) & WORLD_CUBE), dz = (((int)k->z - (int)pivot[2]) & WORLD_CUBE);
        out[0] = pivot[0] + (float)(dx >= HALF_WORLD ? dx - WORLD_CUBE - 1 : dx);
        out[2] = pivot[2] + (float)(dz >= HALF_WORLD ? dz - WORLD_CUBE - 1 : dz);
    }
    out[1] = pivot[1] + k->y;
}

PRIM(p_snow_scene) { scene((int)POP()); }   /* ( kind -- ) 0 around the camera, 1 from emitters, else none */
PRIM(p_snow_emitter) {   /* ( i x z heading -- ) an emitter of the scene's: where (world), its heading */
    Cell h = POP(), z = POP(), x = POP(), i = POP();
    if (i >= 0 && i < EMITTERS) {
        sEmitter[i][0] = (float)x, sEmitter[i][1] = (float)z, sEmitter[i][2] = (float)h;
        if (i >= sEmitters) {
            sEmitters = (int)i + 1;
        }
    }
}
PRIM(p_snow_flakes) {   /* ( n -- ) the live flakes: the new ones start high above (src: func_8003B0C0) */
    Cell n = POP();
    int i;

    n = n < 0 ? 0 : n > FLAKES ? FLAKES : n;
    for (i = sLive; i < n; i++) {
        spawn(&sFlakes[i], cube_of(), HIGHER);
    }
    sLive = (int)n;
}
PRIM(p_snow_step) {   /* ( -- ) a game frame of their fall */
    static signed char *wob;
    static int tried;
    int i;

    if (!tried) {   /* (the executable's table, from the library) */
        size_t n = 0;
        tried = 1;
        wob = (signed char *)files_read("exe/snow-wobble.bin", &n);
        if (wob != NULL && n < WOBBLE_STEPS * 2) {
            free(wob);
            wob = NULL;
        }
    }
    if (sKind != AROUND && sKind != FROM_EMITTERS) {
        return;
    }
    for (i = 0; i < FLAKES; i++) {
        if (sKind == FROM_EMITTERS && i % EMITTERS >= sEmitters) {
            continue;
        }
        step(&sFlakes[i], sLive - i, wob);
    }
}
PRIM(p_draw_snow) {   /* ( image x y z -- ) this frame, about the camera's pivot */
    Cell pz = POP(), py = POP(), px = POP(), image = POP();
    float pivot[3] = {(float)px, (float)py, (float)pz};
    GpuTexture t = engine_image(image);
    int i;

    if (t.id == 0 || (sKind != AROUND && sKind != FROM_EMITTERS)) {
        return;
    }
    for (i = 0; i < FLAKES; i++) {
        const Flake *k = &sFlakes[i];
        float at[3], uv[4], dx, dz, near;
        int column, row;

        if ((k->state != 0 && k->phase == LYING) || (sKind == FROM_EMITTERS && i % EMITTERS >= sEmitters)) {
            continue;
        }
        place(k, i, pivot, at);
        dx = at[0] - pivot[0], dz = at[2] - pivot[2];
        near = sqrtf(dx * dx + dz * dz);
        if (near >= FAR) {
            continue;
        }
        column = ((int)near >> 7) & ~7;   /* (src: the depth / 32, / 4, by 8: nearer, bigger flakes) */
        row = (k->phase & 6) * 4 + k->state;
        uv[0] = column / SHEET_W, uv[1] = row / SHEET_H;
        uv[2] = (column + SPRITE_PIXELS) / SHEET_W, uv[3] = (row + SPRITE_PIXELS) / SHEET_H;
        scene3d_sprite(t, at, SPRITE_PIXELS, uv, SHADE, PS_ADD);
    }
}
PRIM(p_snow_live) { PUSH(sLive); }   /* ( -- n ) */

void bind_snow(Forth *f) {
    forth_prim(f, "snow-scene", p_snow_scene);
    forth_prim(f, "snow-emitter", p_snow_emitter);
    forth_prim(f, "snow-flakes", p_snow_flakes);
    forth_prim(f, "snow-step", p_snow_step);
    forth_prim(f, "draw-snow", p_draw_snow);
    forth_prim(f, "snow-live", p_snow_live);
}
