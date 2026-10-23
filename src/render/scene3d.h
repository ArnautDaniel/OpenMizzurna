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

/* the camera, in world units (see above) */
void scene3d_camera(const float target[3], float yaw, float dist, float eye_y, float look_y, float projection_h);

/* draw a model this frame at a world position, turned by yaw about the vertical; shown: a byte a part (its
 * node: 0 hidden), NULL all of them */
void scene3d_draw(Model3D *m, const float pos[3], float yaw, const unsigned char *shown, int nshown);

/* draw what was asked this frame (render_end calls it, before the 2D) */
void scene3d_flush(int w, int h);

#endif
