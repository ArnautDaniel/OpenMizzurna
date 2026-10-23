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
- The street lamps (38): off by day; from 16:01 they come on a few at a time (a lamp switches 7 frames in 8), from
  17:01 all are on; from 23:01 they go off a few at a time, and stay off until the evening. A lamp's windows and
  lights show their lit textures (the engine's `lamp`). Facts: `lamp-lit? ( i -- flag )`, `tiles-loaded`,
  `camera-tile`, `tile-centre`.

## To come

The sky (its colour by the hour, the stars, clouds, the horizon strip, sun and moon: `sky.c`), the billboards (the trees: each tile's `billboards.bin`), snow, the moving
signs and the tiles' object animations, the police cordon coming down (12/25 - 12/28), the ground's height for
people (the terrain), Matthew walking there (the tiles' obstacles and zones), the doors into the buildings, the
cars' headlights at night.
