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
- The street lamps (38): off by day; from 16:01 they come on a few at a time (a lamp switches 7 frames in 8), from
  17:01 all are on; from 23:01 they go off a few at a time, and stay off until the evening. A lamp's windows and
  lights show their lit textures (the engine's `lamp`). Facts: `lamp-lit? ( i -- flag )`, `tiles-loaded`,
  `camera-tile`, `tile-centre`.

- On foot (docs/subsystems/matthew.md): the tiles' obstacles and zones, the ground's height (`ground-at`); the
  townsfolk walking there follow the ground too.

## To come

Snow, the moving
signs and the tiles' object animations, the police cordon coming down (12/25 - 12/28), the doors into the buildings (and the zones' scripts), the cars and props in his way (src: player_probe_blocked), the
two road tiles closed on foot until 12/28 03:00, driving, the
cars' headlights at night.
