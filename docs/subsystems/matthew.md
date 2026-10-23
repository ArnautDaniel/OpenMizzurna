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

## Walking (src: player_control_foot, player_update)

While he is the player's (`player-control`: the events' control ops and their starts and ends), the pad
moves him by the original's per-frame numbers, each step covering as many of its frames as it ran in that
time (15 a second indoors, ~30 in the town: he was twice as quick in the town, as the clock - faithful):

| | original (a frame) |
|---|---|
| walk (Up) | 60 units |
| run (Up + ✕) | builds 4 a frame to 38; from 7, +3 speed a step past 6 (to 156) |
| step back (Down, standing) | -40 |
| turn (Left / Right) | builds: standing +16 to 100, walking +10 to 50, running +6 to 30 (4096 a turn) |
| L1 / R1 | a quarter turn |
| a blocked step | tried 48 at a time to 384 either side, his heading easing 50 that way; else undone |
| standing 120 frames | the fidget (motions 20 then 22) |

Motions: stand 0, walk 1, back 2, turning 3 / 4, run 5. The walls are his floor's obstacles
(`floors.fs`: `blocked?`).

In the town the walls are the obstacles of the tile he steps into (off the map: a wall), and his height is the
ground's under him: the highest of the tile's terrain polygons there (src: point_blocked, func_8001C264; where
there is none he keeps his height). The town's tiles stream around the camera as he walks.

## To come

Looking and acting (○: the zones and the areas' rules), doors, the floors' zones and heights, the car with the pad by the original's speeds and turning, the
floors' walls and zones, looking and acting.
