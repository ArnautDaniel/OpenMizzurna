# Matthew

## Purpose

The player (src: `player.c`, `player_misc.c`): his body in the world, his figure (`models/073_1.gltf`: kind
0's model, CD.BIN 073_1, in the area's textures), walking and running, looking and acting.

## Now

- His body is a kernel body (`body-place`): the events place and turn him (`figure-place`, `figure-turn`
  from `set-pos` / `set-rot`). A height of 0 means "on his floor" (the scripts' `set_pos` gives 0 and the
  original's floor holds the figure; heights they give otherwise, `set-y`, are the world's).
- Facts: `matthew-there?`, `matthew-now ( -- x y z yaw )`.

## To come

His motions (the library's motion files), walking with the pad by the original's speeds and turning, the
floors' walls and zones, looking and acting.
