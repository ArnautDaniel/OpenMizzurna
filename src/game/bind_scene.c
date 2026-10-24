/* The 3D words for the scripts (render/scene3d.h; docs/forth.md): models by library path, drawn this frame at
 * a place in the game's world units, through the game's camera. */
#include "engine.h"

#include "../core/files.h"
#include "../data/motion.h"
#include "../render/scene3d.h"

#include <stdlib.h>

#include <math.h>
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
        if (sModels[i].path[0] != 0 && strcmp(sModels[i].path, path) == 0) {
            PUSH(sModels[i].m != NULL ? i + 1 : 0);
            return;
        }
    }
    for (i = 0; i < sNModels && sModels[i].path[0] != 0; i++) {   /* (a slot forgotten, or a new one) */
    }
    if (i == MAX_MODELS) {
        forth_error(f, "model: too many models");
    }
    snprintf(sModels[i].path, sizeof(sModels[i].path), "%s", path);
    sModels[i].m = model3d_load(path);
    if (i == sNModels) {
        sNModels++;
    }
    PUSH(sModels[i].m != NULL ? i + 1 : 0);
}
PRIM(p_forget_model) {   /* ( model -- ) let it go: its memory freed, its number another's later (the town's tiles) */
    Cell m = POP();

    if (m >= 1 && m <= sNModels && sModels[m - 1].path[0] != 0) {
        model3d_free(sModels[m - 1].m);
        sModels[m - 1].m = NULL;
        sModels[m - 1].path[0] = 0;
    }
}
PRIM(p_draw_model) {   /* ( model x y z yaw -- ) this frame, at a world place, turned */
    Cell yaw = POP(), z = POP(), y = POP(), x = POP(), m = POP();
    float pos[3] = {(float)x, (float)y, (float)z};

    if (m >= 1 && m <= sNModels) {
        scene3d_draw(sModels[m - 1].m, pos, (float)yaw, NULL, 0);
    }
}
PRIM(p_ground_height) {   /* ( model x z cx cz -- y true | false ) the ground under x z in the model standing at
                            * cx 0 cz (the town's tiles): its height, world units */
    Cell cz = POP(), cx = POP(), z = POP(), x = POP(), m = POP();
    float pos[3] = {(float)cx, 0, (float)cz}, y;

    if (m >= 1 && m <= sNModels && scene3d_ground(sModels[m - 1].m, pos, (float)x, (float)z, &y)) {
        PUSH((Cell)lroundf(y));
        PUSH(-1);
    } else {
        PUSH(0);
    }
}
#define MAX_POSES 64
static Scene3DObjectPose sPoses[MAX_POSES];   /* the objects the next draw-model-shown moves */
static int sNPoses;
PRIM(p_pose_object) {   /* ( object side height forward rx ry rz -- ) for the next draw-model-shown: an object moved */
    Cell rz = POP(), ry = POP(), rx = POP(), fwd = POP(), height = POP(), side = POP(), obj = POP();

    if (sNPoses < MAX_POSES) {
        Scene3DObjectPose *p = &sPoses[sNPoses++];
        p->object = (int)obj;
        p->place[0] = (float)side, p->place[1] = (float)height, p->place[2] = (float)fwd;
        p->turn[0] = (short)rx, p->turn[1] = (short)ry, p->turn[2] = (short)rz;
    }
}
PRIM(p_draw_model_shown) {   /* ( model x y z yaw addr n -- ) only the parts whose byte (of n) isn't 0 (and the objects
                              * posed since: pose-object) */
    Cell n = POP(), addr = POP(), yaw = POP(), z = POP(), y = POP(), x = POP(), m = POP();
    float pos[3] = {(float)x, (float)y, (float)z};

    if (m >= 1 && m <= sNModels) {
        if (sNPoses > 0) {
            scene3d_draw_objects(sModels[m - 1].m, pos, (float)yaw, (const unsigned char *)addr, (int)n, sPoses, sNPoses);
        } else {
            scene3d_draw(sModels[m - 1].m, pos, (float)yaw, (const unsigned char *)addr, (int)n);
        }
    }
    sNPoses = 0;
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
PRIM(p_motion_turn) {   /* ( motion frame -- rx ry rz ) a prop motion's turn that frame (a character's: its root's) */
    Cell frame = POP();
    Motion *m = motion_arg(POP());

    if (m == NULL || frame < 0 || frame >= m->frames) {
        PUSH(0), PUSH(0), PUSH(0);
        return;
    }
    PUSH(m->turns[(size_t)frame * m->parts * 3]), PUSH(m->turns[(size_t)frame * m->parts * 3 + 1]);
    PUSH(m->turns[(size_t)frame * m->parts * 3 + 2]);
}
static int pose_of(Motion *mt, Cell frame, short (*turns)[3], float move[3]) {   /* a motion's frame: its parts */
    int k, parts;

    frame = ((frame % mt->frames) + mt->frames) % mt->frames;
    parts = mt->parts < MAX_PARTS ? mt->parts : MAX_PARTS;
    for (k = 0; k < parts; k++) {
        const int16_t *t = mt->turns + ((size_t)frame * mt->parts + k) * 3;
        turns[k][0] = t[0], turns[k][1] = t[1], turns[k][2] = t[2];
    }
    for (k = 0; k < 3; k++) {
        move[k] = (float)mt->move[frame][k];
    }
    return parts;
}
PRIM(p_draw_figure) {   /* ( model motion frame x y z yaw -- ) a figure posed in a frame of a motion */
    Cell yaw = POP(), z = POP(), y = POP(), x = POP(), frame = POP(), mo = POP(), m = POP();
    float pos[3] = {(float)x, (float)y, (float)z}, move[3];
    Motion *mt = motion_arg(mo);
    short turns[MAX_PARTS][3];
    int parts;

    if (m < 1 || m > sNModels) {
        return;
    }
    if (mt == NULL || mt->frames <= 0) {
        scene3d_draw(sModels[m - 1].m, pos, (float)yaw, NULL, 0);
        return;
    }
    parts = pose_of(mt, frame, turns, move);
    scene3d_draw_posed(sModels[m - 1].m, pos, (float)yaw, (const short (*)[3])turns, parts, move);
}
PRIM(p_part_on_screen) {   /* ( model motion frame x y z yaw part -- px py true | false ) where a part of the figure,
                            * posed so, falls on the window (its pixels), with this frame's camera */
    Cell part = POP(), yaw = POP(), z = POP(), y = POP(), x = POP(), frame = POP(), mo = POP(), m = POP();
    float pos[3] = {(float)x, (float)y, (float)z}, move[3], at[2];
    Motion *mt = motion_arg(mo);
    short turns[MAX_PARTS][3];
    int parts = 0, seen;

    if (m < 1 || m > sNModels) {
        PUSH(0);
        return;
    }
    if (mt != NULL && mt->frames > 0) {
        parts = pose_of(mt, frame, turns, move);
    }
    seen = scene3d_node_on_screen(sModels[m - 1].m, pos, (float)yaw, (const short (*)[3])turns, parts,
                                  parts > 0 ? move : NULL, (int)part, gEngine.width, gEngine.height, at);
    if (!seen) {
        PUSH(0);
        return;
    }
    PUSH((Cell)at[0]), PUSH((Cell)at[1]), PUSH(-1);
}
PRIM(p_texture_area) { scene3d_texture_area((int)POP()); }   /* ( area -- ) */
/* the light (scene3d.h): set a piece at a time, given whole each time */
static Scene3DLight sLight;
PRIM(p_light_way) {   /* ( i x y z -- ) the way light i shines (the game's axes) */
    Cell z = POP(), y = POP(), x = POP(), i = POP();
    if (i >= 0 && i < 3) {
        sLight.way[i][0] = (float)x, sLight.way[i][1] = (float)y, sLight.way[i][2] = (float)z;
        scene3d_light(&sLight);
    }
}
PRIM(p_light_color) {   /* ( i r g b -- ) its colour (256 full) */
    Cell b = POP(), g = POP(), r = POP(), i = POP();
    if (i >= 0 && i < 3) {
        sLight.color[i][0] = (float)r, sLight.color[i][1] = (float)g, sLight.color[i][2] = (float)b;
        scene3d_light(&sLight);
    }
}
PRIM(p_ambient_light) {   /* ( r g b -- ) the ambient (4096 full) */
    Cell b = POP(), g = POP(), r = POP();
    sLight.ambient[0] = (float)r, sLight.ambient[1] = (float)g, sLight.ambient[2] = (float)b;
    scene3d_light(&sLight);
}
PRIM(p_fog_color) {   /* ( r g b -- ) the fog's colour (128: a texture as it is) */
    Cell b = POP(), g = POP(), r = POP();
    sLight.fog_color[0] = (float)r, sLight.fog_color[1] = (float)g, sLight.fog_color[2] = (float)b;
    scene3d_light(&sLight);
}
PRIM(p_fog_store) { sLight.fog = POP() != 0; scene3d_light(&sLight); }   /* ( flag -- ) fog or none */
PRIM(p_lamp) { Cell lit = POP(); scene3d_lamp((int)POP(), lit != 0); }   /* ( lamp flag -- ) a street lamp on or off */
PRIM(p_texture_frame) { Cell frame = POP(); scene3d_texture_frame((int)POP(), (int)frame); }   /* ( anim frame -- ) -1 none */
PRIM(p_texture_frame_shown) { PUSH(scene3d_texture_frame_shown((int)POP())); }   /* ( anim -- frame ) -1 none */

void bind_scene(Forth *f) {
    forth_prim(f, "model", p_model);
    forth_prim(f, "forget-model", p_forget_model);
    forth_prim(f, "draw-model", p_draw_model);
    forth_prim(f, "ground-height", p_ground_height);
    forth_prim(f, "draw-model-shown", p_draw_model_shown);
    forth_prim(f, "camera", p_camera);
    forth_prim(f, "texture-area", p_texture_area);
    forth_prim(f, "lamp", p_lamp);
    forth_prim(f, "texture-frame", p_texture_frame);
    forth_prim(f, "texture-frame@", p_texture_frame_shown);
    forth_prim(f, "light-way", p_light_way);
    forth_prim(f, "light-color", p_light_color);
    forth_prim(f, "ambient-light", p_ambient_light);
    forth_prim(f, "fog-color", p_fog_color);
    forth_prim(f, "fog!", p_fog_store);
    forth_constant(f, "#lamps", SCENE3D_LAMPS);
    forth_prim(f, "motion", p_motion);
    forth_prim(f, "motion-frames", p_motion_frames);
    forth_prim(f, "motion-move", p_motion_move);
    forth_prim(f, "motion-turn", p_motion_turn);
    forth_prim(f, "pose-object", p_pose_object);
    forth_prim(f, "draw-figure", p_draw_figure);
    forth_prim(f, "part-on-screen", p_part_on_screen);
}
