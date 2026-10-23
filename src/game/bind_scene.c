/* The 3D words for the scripts (render/scene3d.h; docs/forth.md): models by library path, drawn this frame at
 * a place in the game's world units, through the game's camera. */
#include "engine.h"

#include "../core/files.h"
#include "../data/motion.h"
#include "../render/scene3d.h"

#include <stdlib.h>

#include <stdio.h>
#include <string.h>

#define PRIM(name) static void name(Forth *f, Word *w)
#define POP() forth_pop(f)
#define PUSH(x) forth_push(f, (Cell)(x))
#define MAX_MODELS 512

static struct {
    char path[160];
    Model3D *m;
} sModels[MAX_MODELS];
static int sNModels;

PRIM(p_model) {   /* ( addr len -- model ) a model from the library (0: none there) */
    Cell len = POP();
    const char *s = (const char *)POP();
    char path[160];
    int i;

    snprintf(path, sizeof(path), "%.*s", (int)len, s);
    for (i = 0; i < sNModels; i++) {
        if (strcmp(sModels[i].path, path) == 0) {
            PUSH(sModels[i].m != NULL ? i + 1 : 0);
            return;
        }
    }
    if (sNModels == MAX_MODELS) {
        forth_error(f, "model: too many models");
    }
    snprintf(sModels[sNModels].path, sizeof(sModels[sNModels].path), "%s", path);
    sModels[sNModels].m = model3d_load(path);
    sNModels++;
    PUSH(sModels[sNModels - 1].m != NULL ? sNModels : 0);
}
PRIM(p_draw_model) {   /* ( model x y z yaw -- ) this frame, at a world place, turned */
    Cell yaw = POP(), z = POP(), y = POP(), x = POP(), m = POP();
    float pos[3] = {(float)x, (float)y, (float)z};

    if (m >= 1 && m <= sNModels) {
        scene3d_draw(sModels[m - 1].m, pos, (float)yaw, NULL, 0);
    }
}
PRIM(p_draw_model_shown) {   /* ( model x y z yaw addr n -- ) only the parts whose byte (of n) isn't 0 */
    Cell n = POP(), addr = POP(), yaw = POP(), z = POP(), y = POP(), x = POP(), m = POP();
    float pos[3] = {(float)x, (float)y, (float)z};

    if (m >= 1 && m <= sNModels) {
        scene3d_draw(sModels[m - 1].m, pos, (float)yaw, (const unsigned char *)addr, (int)n);
    }
}
PRIM(p_camera) {   /* ( x y z yaw dist eye-y look-y h -- ) the game's camera (scene3d.h) */
    Cell h = POP(), look_y = POP(), eye_y = POP(), dist = POP(), yaw = POP(), z = POP(), y = POP(), x = POP();
    float target[3] = {(float)x, (float)y, (float)z};

    scene3d_camera(target, (float)yaw, (float)dist, (float)eye_y, (float)look_y, (float)h);
}
#define MAX_MOTIONS 512
#define MAX_PARTS 64
static struct {
    char path[160];
    Motion m;
    int ok;
} sMotions[MAX_MOTIONS];
static int sNMotions;

PRIM(p_motion) {   /* ( addr len -- motion ) a character's motion from the library (its .motion.txt; 0: none) */
    Cell len = POP();
    const char *s = (const char *)POP();
    char path[160], why[128];
    size_t n;
    uint8_t *text;
    int i;

    snprintf(path, sizeof(path), "%.*s", (int)len, s);
    for (i = 0; i < sNMotions; i++) {
        if (strcmp(sMotions[i].path, path) == 0) {
            PUSH(sMotions[i].ok ? i + 1 : 0);
            return;
        }
    }
    if (sNMotions == MAX_MOTIONS) {
        forth_error(f, "motion: too many motions");
    }
    snprintf(sMotions[sNMotions].path, sizeof(sMotions[sNMotions].path), "%s", path);
    text = files_read(path, &n);
    sMotions[sNMotions].ok = text != NULL && motion_from_text(&sMotions[sNMotions].m, (const char *)text, n, why, sizeof(why));
    free(text);
    sNMotions++;
    PUSH(sMotions[sNMotions - 1].ok ? sNMotions : 0);
}
static Motion *motion_arg(Cell id) { return id >= 1 && id <= sNMotions && sMotions[id - 1].ok ? &sMotions[id - 1].m : NULL; }
PRIM(p_motion_frames) { Motion *m = motion_arg(POP()); PUSH(m != NULL ? m->frames : 0); }   /* ( motion -- n ) */
PRIM(p_motion_move) {   /* ( motion frame -- dx dy dz ) the root's move that frame (dy upward) */
    Cell frame = POP();
    Motion *m = motion_arg(POP());

    if (m == NULL || frame < 0 || frame >= m->frames) {
        PUSH(0), PUSH(0), PUSH(0);
        return;
    }
    PUSH(m->move[frame][0]), PUSH(m->move[frame][1]), PUSH(m->move[frame][2]);
}
PRIM(p_draw_figure) {   /* ( model motion frame x y z yaw -- ) a figure posed in a frame of a motion */
    Cell yaw = POP(), z = POP(), y = POP(), x = POP(), frame = POP(), mo = POP(), m = POP();
    float pos[3] = {(float)x, (float)y, (float)z}, move[3];
    Motion *mt = motion_arg(mo);
    short turns[MAX_PARTS][3];
    int k, parts;

    if (m < 1 || m > sNModels) {
        return;
    }
    if (mt == NULL || mt->frames <= 0) {
        scene3d_draw(sModels[m - 1].m, pos, (float)yaw, NULL, 0);
        return;
    }
    frame = ((frame % mt->frames) + mt->frames) % mt->frames;
    parts = mt->parts < MAX_PARTS ? mt->parts : MAX_PARTS;
    for (k = 0; k < parts; k++) {
        const int16_t *t = mt->turns + ((size_t)frame * mt->parts + k) * 3;
        turns[k][0] = t[0], turns[k][1] = t[1], turns[k][2] = t[2];
    }
    for (k = 0; k < 3; k++) {
        move[k] = (float)mt->move[frame][k];
    }
    scene3d_draw_posed(sModels[m - 1].m, pos, (float)yaw, (const short (*)[3])turns, parts, move);
}
PRIM(p_texture_area) { scene3d_texture_area((int)POP()); }   /* ( area -- ) */

void bind_scene(Forth *f) {
    forth_prim(f, "model", p_model);
    forth_prim(f, "draw-model", p_draw_model);
    forth_prim(f, "draw-model-shown", p_draw_model_shown);
    forth_prim(f, "camera", p_camera);
    forth_prim(f, "texture-area", p_texture_area);
    forth_prim(f, "motion", p_motion);
    forth_prim(f, "motion-frames", p_motion_frames);
    forth_prim(f, "motion-move", p_motion_move);
    forth_prim(f, "draw-figure", p_draw_figure);
}
