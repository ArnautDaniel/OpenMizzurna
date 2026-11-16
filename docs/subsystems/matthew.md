# Matthew

## Purpose

The player (src: `player.c`, `player_misc.c`): his body in the world, his figure (`models/073_1.gltf`: kind
0's model, CD.BIN 073_1, in the area's textures), walking and running, looking and acting.

## Now

- His body is a kernel body (`body-place`): the events place and turn him (`figure-place`, `figure-turn`
  from `set-pos` / `set-rot`). A height of 0 means "on his floor" (the scripts' `set_pos` gives 0 and the
  original's floor holds the figure; heights they give otherwise, `set-y`, are the world's).
- A scene walks him too (src: actor_nav_step, move_anim): `walk-to` / `run-to` toward a point at a pace a game
  frame in its motion (`figure-walk`; standing at the end), `walk-forward` straight on until `idle`
  (`figure-forward`, `figure-idle`); `turn-to`, `turn-by`, `face` turn him a speed a game frame, its sign the
  way (`figure-turning`; 0 at once). `wait-stop` / `wait-turned` wait on the facts `matthew-walking?` /
  `matthew-turned?` (a walk he's stuck on is given up after 75 game frames, as for everyone). On a stool: `take-diner-seat` takes it (the townsfolk keep the stools), `sit-diner-seat`
  puts him on it.
- On a stool the pad isn't his: `figure-on-foot` (false) stops his walking (src: the pad's field mode; the
  counter: `docs/subsystems/counter.md`); the fact `matthew-on-foot?`.
- Pushing a crate (`figure-pushes`): it stays 0xB4 ahead as he walks, its own motions, no running; his perch
  (`figure-perch`: on his feet, on a crate, on a stool) - `docs/subsystems/crate.md`.
- Facts: `matthew-there?`, `matthew-now ( -- x y z yaw )`, `matthew-was ( -- x y z )` (before his last step).

## Motions (`scripts/figures.fs`)

An animation id names a motion: below 193 the common set (`cdbin/033_4/1/<id>.motion.txt`), from an area's
first id (`area-motion-first`: 300, 500 ... ) its own (`cdbin/NNN_4/1/<id - first>.motion.txt`; src:
func_80020464). The original advanced a figure's motion a frame each game frame, so motions play at the game's
rate (`game-rate`, docs/subsystems/areas.md: 15 a second indoors, 30 in the town but 15 while an event holds it).
Each frame poses the figure (`draw-figure`: every part's turn in place of its rest turn, the root moved by the
frame's move) - the engine's posing (`scene3d_draw_posed`).

A figure's motions chain as the original's (`playing`, figures.fs; src: the Actor's anim, anim_queued, anim_loop,
anim_next, anim_done, played by func_8002032C): when the one playing wraps, the one queued after it (if any)
plays, looping as the first did; one that doesn't loop, with none queued, is **done** (`wait-anim` ends) and its
next plays on and on, or with none its last frame holds. The one playing asked for again goes on where it is (src:
the frame starts over only when the motion changes). With `anim-hold` its last root move is kept as his new
place, turned by his heading. The events' commands (src: actor_command_default):

| cmd | the original | |
|---|---|---|
| `anim` $03 | `actor_anim(a, id, loop, 0)` | id, then standing (motion 0) |
| `anim-then` $04 | `actor_anim_then(a, id, then, loop, 0)` | id once, then `then` looping as asked (or once, then standing) |
| `anim-once` $05 | `actor_anim(a, id, 0, -1)` | once, its last frame held |
| `anim-then-loop` $06 | `actor_anim_then(a, id, then, 0, -1)` | once, then `then` once, its last frame held (despite the name) |
| `anim-at` $13 | `actor_anim(a, id, 0, next)` | once, then next on and on (a talk's gesture too) |

Matthew's are `figure-anim-then` / `figure-anim` / `figure-hold` (the fact `anim-done?`).

## A mod's way of walking (the walk-intent hook)

`walk-intent ( -- heading pace )` (a hook: docs/modding.md) is asked each step he's the player's on foot. The
game's answer is `by-the-pad`: the tank controls below. A mod's may give a heading (0 .. 4095) and a pace
(`standing-pace`, `walking-pace`, `running-pace`): he turns toward it the shorter way (`way-between`), a third
of what is left a game frame, 32 to 320 (`steer-to`: the turn
motions while he stands) and walks or runs by the game's own speeds and motions (`pace-by`, shared with the pad)
and walls. The sample: `examples/mods/stick-walk` (the left stick, the way it points on the screen).

## Walking (src: player_control_foot, player_update)

While he is the player's (`player-control`: the events' control ops and their starts and ends), the pad
moves him by the original's per-frame numbers, each step covering as many of its frames as it ran in that
time (`game-frames`: 15 a second indoors, ~30 in the town - he was twice as quick in the town, as the clock -
and 15 there while an event holds the game):

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

## Floors and crawling

A scene's `set-floor` on him (`figure-floor ( floor )`, src: actor_set_floor) sets his floor - the one his walls
and height are read from - and puts him at its height; one going along with him goes to it too. Crawling
(`figure-crawl ( flag )`, src: ovl019's duct crawl): while a scene runs the pad drives him tank-like - up ahead
0x28 a frame, down back 0x1E, left / right turn 0x80 - and a move that would put a wall at his place or one or two
of his lengths (0x78) ahead is undone, turn and all; motions 0x1006 moving, 0x1005 still. Fact:
`matthew-crawling?`. Unseen (`figure-shown ( flag )`, src: actors[0].visible): not drawn, as while a shoot-out
aims; seen again on entering an area. His perch 4: in the car (driving.md). A step that would put him on a solid
billboard (a tree, a lamp post: `solid-billboard?`, src: player_update's billboard_blocked) is not taken.

## Unstuck

Standing where he can't be with control on foot - in a wall, or someone's place (a person on his floor within his
radius and theirs, a car or prop of 26 - 60) - he's moved to the nearest free place, 25 at a time out to 475, x
first on some frames, z on the others (src: cobj.c's unstick; `taken?`, events.fs). (`tests/library/test_blocking.fs`.)

## Elsewhere

Looking and acting (look.md), doors (doors.md), the floors' walls, zones and heights (floors.fs), the car
(driving.md), what else stops him - the town's cars and props, the police cordon (town.md).
