/* The 3D words for the scripts (render/scene3d.h; docs/forth.md): models by library path, drawn this frame at
 * a place in the game's world units, through the game's camera. */
#include "engine.h"

#include "../render/scene3d.h"

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
        scene3d_draw(sModels[m - 1].m, pos, (float)yaw);
    }
}
PRIM(p_camera) {   /* ( x y z yaw dist eye-y look-y h -- ) the game's camera (scene3d.h) */
    Cell h = POP(), look_y = POP(), eye_y = POP(), dist = POP(), yaw = POP(), z = POP(), y = POP(), x = POP();
    float target[3] = {(float)x, (float)y, (float)z};

    scene3d_camera(target, (float)yaw, (float)dist, (float)eye_y, (float)look_y, (float)h);
}
PRIM(p_texture_area) { scene3d_texture_area((int)POP()); }   /* ( area -- ) */

void bind_scene(Forth *f) {
    forth_prim(f, "model", p_model);
    forth_prim(f, "draw-model", p_draw_model);
    forth_prim(f, "camera", p_camera);
    forth_prim(f, "texture-area", p_texture_area);
}
