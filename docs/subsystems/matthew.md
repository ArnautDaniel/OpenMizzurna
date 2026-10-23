# Matthew

## Purpose

The player (src: `player.c`, `player_misc.c`): his body in the world, his figure (`models/073_1.gltf`: kind
0's model, CD.BIN 073_1, in the area's textures), walking and running, looking and acting.

## Now

- His body is a kernel body (`body-place`): the events place and turn him (`figure-place`, `figure-turn`
  from `set-pos` / `set-rot`). A height of 0 means "on his floor" (the scripts' `set_pos` gives 0 and the
  original's floor holds the figure; heights they give otherwise, `set-y`, are the world's).
- Facts: `matthew-there?`, `matthew-now ( -- x y z yaw )`.

## Motions (`scripts/figures.fs`)

An animation id names a motion: below 193 the common set (`cdbin/033_4/1/<id>.motion.txt`), from an area's
first id (`area-motion-first`: 300, 500 ... ) its own (`cdbin/NNN_4/1/<id - first>.motion.txt`; src:
func_80020464). The original advanced a figure's motion a frame each game frame, so motions play at 15 frames
a second indoors and 30 in the town. Each frame poses the figure (`draw-figure`: every part's turn in place of
its rest turn, the root moved by the frame's move) - the engine's posing (`scene3d_draw_posed`). A motion
loops or ends; at its end the next one plays, or (a reading: the original's `anim_next`) its last frame
holds; with `anim-hold` its last root move is kept as his new place, turned by his heading. The events' `anim`,
`anim-then`, `anim-hold` and `wait-anim` play Matthew's (`figure-anim`, `figure-hold`; the fact `anim-done?`).

## To come

Walking with the pad by the original's speeds and turning, the
floors' walls and zones, looking and acting.
