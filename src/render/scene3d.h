/* The 3D layer: the game's models (glTF from the library: data/modelgltf.h) drawn through the game's camera.
 *
 * Everything here takes the game's own units and axes - world units (twice a model's), y downward, angles of
 * 4096 to the turn - and turns them into glTF's (model units / 256, y up) itself, so the scripts speak the
 * game's numbers (the events' positions, the scenes' cameras).
 *
 * The camera is the original's (src: camera_apply_view): a target point, the eye `dist` from it along the
 * heading `yaw` and `eye_y` above it (negative: up), looking at the point `look_y` above the target; the
 * projection the PlayStation's: a screen 240 high seen from `h` (500 indoors, 400 in the town), so the
 * vertical field of view is 2 atan(120 / h); a wider window sees more to the sides.
 *
 * Materials as the converter wrote them: a texture from the area's set (textures/area-NNN/, else
 * textures/common/: scene3d_texture_area) or the model's own; vertex colours with 1.0 the texture as it is;
 * lit or not; alpha 0 never drawn; a semi-transparent polygon's texels marked to blend (alpha 128) blended by
 * the PlayStation's four modes (half, add, subtract, quarter), drawn after the solid ones. */
#ifndef SCENE3D_H
#define SCENE3D_H

typedef struct Model3D Model3D;

/* a model (a library path: "models/001_4.3.0.gltf"); NULL if it isn't there or won't load */
Model3D *model3d_load(const char *path);
void model3d_free(Model3D *m);

/* whose texture set "area" materials use (the area being played) */
void scene3d_texture_area(int area);

/* the light the lit models are drawn in (the game's: src clock.c, the scenes' light records): three lights, each
 * the way it shines (the game's axes: y downward) and its colour (256 full); the ambient (4096 full, the
 * PlayStation's); and in the town the fog - the PlayStation's depth cue, its colour (128: a texture as it is) */
typedef struct Scene3DLight {
    float way[3][3], color[3][3];
    float ambient[3];
    int fog;
    float fog_color[3];
} Scene3DLight;
void scene3d_light(const Scene3DLight *l);

/* the town's street lamps, lit or not: a material that is a lamp's light (its glTF extras' "lamp") shows its lit
 * texture (textures/lamps/) while its lamp is */
#define SCENE3D_LAMPS 38
void scene3d_lamp(int lamp, int lit);

/* an area's texture animations (the original's video memory copies): while animation a shows frame f, a material
 * of the area's set with a texture in textures/area-NNN/anim-a/f/ shows that; -1 none (entering an area: none) */
#define SCENE3D_ANIMS 4
void scene3d_texture_frame(int anim, int frame);
int scene3d_texture_frame_shown(int anim);   /* (-1 none) */

/* the camera, in world units (see above) */
void scene3d_camera(const float target[3], float yaw, float dist, float eye_y, float look_y, float projection_h);

/* draw a model this frame at a world position, turned by yaw about the vertical; shown: a byte a part (its
 * node: 0 hidden), NULL all of them */
void scene3d_draw(Model3D *m, const float pos[3], float yaw, const unsigned char *shown, int nshown);

/* the ground under (x, z) (world units) in a model standing at pos, unturned: the highest of its polygons there -
 * its height to *y (world units, y downward); 0 if none is under it */
int scene3d_ground(const Model3D *m, const float pos[3], float x, float z, float *y);

/* a figure in a frame of a motion: each node's turn (rx, ry, rz; nodes past nturns keep their rest turn) and
 * the root's move this frame (model units: side, up, forward) */
void scene3d_draw_posed(Model3D *m, const float pos[3], float yaw, const short (*turns)[3], int nturns,
                        const float root_move[3]);
/* where one of its nodes (its origin) falls on a screen w x h pixels, with the camera as it stands: 0 if behind
 * the camera */
int scene3d_node_on_screen(Model3D *m, const float pos[3], float yaw, const short (*turns)[3], int nturns,
                           const float root_move[3], int node, int w, int h, float out[2]);

/* a textured quad this frame (the billboards): its corners in world units - 0 1 along the top, 2 3 the bottom - the
 * whole texture across it, shaded (1.0: as it is); blend: 0 solid, else its texels marked to blend do so by the
 * PlayStation's mode (1 half, 2 add, 3 subtract, 4 quarter) */
#include "render.h"
void scene3d_quad(GpuTexture t, const float corner[4][3], float shade, int blend);
/* a small square facing the camera (the snow) at a world place, `pixels` across on the original's screen (240
 * high), a part of the texture (u0 v0 u1 v1) across it */
void scene3d_sprite(GpuTexture t, const float pos[3], float pixels, const float uv[4], float shade, int blend);

/* a model with some of its objects moved this frame (the areas' animated props: doors ...): each pose an object
 * (its node), its place from where it rests (side, height, forward - along its rest heading; model units, height
 * downward) and its turn added to its rest turn (the game's) */
typedef struct Scene3DObjectPose {
    int object;
    float place[3];
    short turn[3];
} Scene3DObjectPose;
void scene3d_draw_objects(Model3D *m, const float pos[3], float yaw, const unsigned char *shown, int nshown,
                          const Scene3DObjectPose *poses, int nposes);

/* draw what was asked this frame (render_end calls it, before the 2D) */
void scene3d_flush(int w, int h);

#endif
