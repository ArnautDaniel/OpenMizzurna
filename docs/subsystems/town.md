# Town

## Purpose

The town: the streamed outdoors, area 27 (src: `map_stream.c`, ovl027's tile renderer func_80106F1C, `clock.c`
func_800243F0). 32 x 24 tiles of 19980 world units; tile (col, row)'s centre is (col 19980 + 9990,
(row + 1) 19980 + 9990). Each tile: its ground and its buildings and props (the library's
`town/CC-RR/terrain.gltf`, `objects.gltf`, from the town's texture set).

## Now (`scripts/town.fs`)

- Entering the town (`enter-area` 27: `areas.fs` loads its texture set, no shell): the tiles around the camera
  are loaded - the 6 x 6 window the original streamed, its corner by which half of its tile the camera is in -
  and the rest let go (`forget-model`); leaving it, all go. (The original read one tile a frame from the disc;
  here they load at once.)
- Drawn: the tiles within two of the camera's (the original's 5 x 5; it also left out those behind the camera by
  its heading - a wider window sees them).
- The cars are drawn (`traffic.fs`: their kinds' models, while on the roads), and the townsfolk in the town.
- The billboards: the trees, lamp posts, bins standing in a tile (`town/CC-RR/billboards.txt`: each its place from
  the tile's origin, its bottom and top, half width, sprite), upright pictures turned to face the camera's heading
  (src: billboard_draw); their sprites (`town/sprites.txt`, `town/sprites/<i>.png`) are drawn in the first light's
  colour, a sixth of its sum and at least 40 of 128 (src: D_800A3848; some at full brightness), their texels
  marked to blend averaged with what is behind. Engine: `records` (the texts' rows), `draw-billboard`.
- Solid billboards (`solid-billboard? ( x z )`, floors.fs's deferred word; src: billboard_blocked / billboard_hit):
  one whose sprite is under the area's count of solid kinds (the executable's `billboard-solid-kinds`, by area:
  the town's 8) within 128 of the point - the tile's there in the town, the scene's set indoors. Matthew's steps
  and the car stop at them.
- The street lamps (38): off by day; from 16:01 they come on a few at a time (a lamp switches 7 frames in 8), from
  17:01 all are on; from 23:01 they go off a few at a time, and stay off until the evening. A lamp's windows and
  lights show their lit textures (the engine's `lamp`). Facts: `lamp-lit? ( i -- flag )`, `tiles-loaded`,
  `camera-tile`, `tile-centre`.

- On foot (docs/subsystems/matthew.md): the tiles' obstacles and zones, the ground's height (`ground-at`); the
  townsfolk walking there follow the ground too.

- The tiles' moving objects (src: ovl027 func_801075AC, func_801078E8): 45 animation slots, each an object of
  its tile and a common props' motion (the executable's `town-door-motions`; the tiles with slots: the town
  code's `town-animated-tiles`). A door's town side swings its slot (`town-door-swing`, from the events' door
  ops): once through, then still. The signs that keep moving (func_801076C4): two turning $1E a frame (16, 12
  and 20, 7), one swinging (17, 13: about x and z between ±$50, bobbing, hung at $EB0).
  `tests/library/test_town.fs`.
- The water's texture animation, the town's per-frame rules and people (areas.md, rules/area027.fs).

- What stops him (`tests/library/test_blocking.fs`): the cars and props of actors 26 - 60 on the roads (src:
  player_probe_blocked: his radius and theirs, theirs halved to their sides - events.fs's `in-the-way?`); the
  police cordon's two road tiles (922, 1022: column + row x 100) on foot but in their zone 0 until the 28th
  03:00 (src: foot_road_block); the car's road blocks (driving.md).

(The cars have no headlights in the original: nothing lights them.)
