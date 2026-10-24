# The Forth

The scripts are written in ClankForth (`external/clankforth`, a submodule shared with OpenHuey): its language is
[its docs/forth.md](../external/clankforth/docs/forth.md) - the model, vocabularies, writing words, the pitfalls.
A change to the language itself is a pull request there (github.com/ArnautDaniel/ClankForth), then the
submodule moved forward here. Below: what the game adds - the engine's words, the scripts, the tests.

## Engine words (src/game/bind_engine.c, vocabulary `engine`)

The engine's words so far; the game's (the world, figures, sound, the disc's data) join as their subsystems
are built, each listed here.

| Word | Stack | |
|---|---|---|
| `key:` | `"name" -- scancode` | `key: C`, `key: Left_Shift` (SDL key names, `_` for spaces) |
| `key-down?`, `key-pressed?` | `scancode -- flag` | held now / went down this tick |
| `key-hold` | `scancode flag --` | hold a key from a script (demos, tests) |
| `on-tick`, `off-tick` | `xt --` | run a word every tick |
| `ticks` | `-- n` | simulation steps since start (for tools; game logic uses real time) |
| `dt` | `F: -- seconds` | a step's length (1/30 by default) |
| `seconds` | `F: s -- steps` | real time as steps, for `wait` (`0.5e seconds wait`); at least one for any time |
| `step-blend` | `F: -- 0..1` | how far the display is between the last step and the next (drawing only) |
| `on-draw`, `off-draw` | `xt --` | run a word every frame to draw 2D |
| `pen-color`, `pen-scale` | `rgba --`, `n --` | colour (0xRRGGBBAA) and text size for the drawing words |
| `pen-blend` | `mode --` | 0 as usual, 1 add, 2 subtract; 3 / 4 the PlayStation's semi-transparency (add / half); 5 its opaque sprites (every texel but 0 covers). screen.fs names them `blend-usual` .. `blend-ps-solid`, and `sprite ( image x y w h )` draws a library picture at the original's place, opaque |
| `draw-text`, `draw-rect` | `addr len x y --`, `x y w h --` | in window pixels from the top left |
| `screen-size`, `char-size` | `-- w h` | |
| `hud` | `addr len --` | a line of text at the bottom of the screen (`0 0 hud` clears) |
| `clear-color` | `F: r g b --` | |
| `screenshot` | `addr len --` | save the next frame as a PNG |
| `screenshot-pending?` | `-- flag` | |
| `console!` | `flag --` | open or close the console |
| `hidden?` | `-- flag` | no window (tests, screenshots) |
| `text-of` | `bank id -- addr len` | a message of the game's text (docs/subsystems/window.md); `0 0` if none |
| `font` / `font-palette` | `n --` | the game's font to draw with (0 the dialogue font, 1 the list font) / its palette |
| `font-text` | `addr len x y --` | text in the game's font at the pen's scale and colour |
| `font-width` | `addr len -- px` | its widest line in the font's pixels; `font-line` the line pitch (16), `font-ready?` |
| `model` | `addr len -- model` | a model from the library (`s" models/001_4.3.0.gltf" model`); 0 if none |
| `draw-model` | `model x y z yaw --` | draw it this frame at a place in the game's world units, turned (4096 a turn) |
| `camera` | `x y z yaw dist eye-y look-y h --` | the game's camera (render/scene3d.h): its target, heading, distance, heights, projection |
| `texture-area` | `area --` | whose texture set the area models use |
| `draw-model-shown` | `model x y z yaw addr n --` | only the parts whose byte (of n) isn't 0 |
| `forget-model` | `model --` | let a model go: its memory freed, its number another's later (the town's tiles) |
| `lamp` / `#lamps` | `i flag --` | a town street lamp lit or not: its lights' materials show their lit textures (38) |
| `texture-frame` / `texture-frame@` | `anim frame --` / `anim -- frame` | one of the area's texture animations at that frame (`textures/area-NNN/anim-a/f/`; −1 none; entering an area: none; 4 an area) |
| `road-node` / `road-point` | `i -- x y z points first` / `i -- dx dy dz length` | the town's road graph (docs/subsystems/traffic.md) |
| `light-way` / `light-color` | `i x y z --` / `i r g b --` | light i's way (game axes) and colour (256 full): docs/subsystems/daylight.md |
| `ambient-light` / `fog-color` / `fog!` | `r g b --` / `r g b --` / `flag --` | the ambient (4096 full), the fog's colour (128: as it is), fog on |
| `image` / `image-size` / `draw-image` | `addr len -- image` / `image -- w h` / `image x y w h --` | a library picture drawn in 2D, tinted by the pen (a negative size flips) |
| `draw-image-quad` | `image x0 y0 x1 y1 x2 y2 x3 y3 --` | the same on four corners (0 1 / 2 3), as two triangles (the item ring's cards) |
| `draw-image-part` | `image u v w h x y w' h' --` | part of it (its texels u, v, w, h) into a rectangle (the HUD's sheets) |
| `draw-image-quad-part` | `image u v w h x0 y0 x1 y1 x2 y2 x3 y3 --` | part of it on four corners (the corner map, turned) |
| `clip-2d` | `x y w h --` | the 2D from now on only within that window rectangle (w 0: everywhere; the corner map's window) |
| `save-file` / `load-file` | `addr len name len -- flag` / `addr max name len -- n` | the player's own files, `<home>/saves/<name>` (load: -1 none) |
| `movie-open` / `movie-advance` / `movie-draw` / `movie-close` | `name len volume -- flag` / `-- frame` / `x y w h --` / `--` | a movie of the library: its pictures (a step on each advance: -1 not yet, -2 over) and its sound |
| `reboot` / `boot-mode` | `mode --` / `-- mode` | after this step the scripts start afresh (Forth and the actors), `boot-mode` that: 0 the title, 1 / 2 an ending's movie (the game over, an ending) |
| `behind` | `flag --` | the 2D from now on behind the 3D (the sky) or over it |
| `records` / `#records` / `record` | `path len word len -- table` / `table -- n` / `table i -- addr` | a library text's lines that start with the word: each line's numbers a row (a cell each) |
| `draw-billboard` | `image x z bottom top half yaw grey blend --` | an upright picture in the world facing the heading yaw (grey 128: as it is; blend: 0 solid, 1 half ...) |
| `pose-object` / `motion-turn` | `object side height forward rx ry rz --` / `motion frame -- rx ry rz` | an object moved in the next `draw-model-shown` (the areas' props); a prop motion's turn |
| `snow-scene` / `snow-emitter` / `snow-flakes` / `snow-step` / `draw-snow` | docs/subsystems/snow.md | the snowflakes |
| `random` | `n -- 0..n-1` | chance |
| `motion` / `motion-frames` / `motion-move` | `addr len -- motion` / `motion -- n` / `motion frame -- dx dy dz` | a character motion from the library and its frames |
| `draw-figure` | `model motion frame x y z yaw --` | a figure posed in a motion's frame |
| `ground-part` / `ground-tilt` | `model part x z cx cy cz -- y true \| false` / `heading -- rx rz` | the ground in one part of a model (an area's shell); a body's tilt on the last ground found (src: func_800466A4) |
| `figure-tag` / `draw-held` | `tag --` / `model tag node --` | the next figure drawn is tagged so (its slot); a model held at a node of the figure tagged so this frame (src: actor_hold) |
| `figure-tilt` | `rx rz --` | the next `draw-figure` tilted so (its root part turned about x and z; src: an actor's rot_x, rot_z) |
| `bank-load` ... `audio-record` | | the sound's words: docs/subsystems/sound.md |
| `draw-ground-quad` | `image x y z half grey blend --` | a picture lying flat in the world, centred there |
| `part-on-screen` | `model motion frame x y z yaw part -- px py true \| false` | where a part of the posed figure falls in the window, with this frame's camera |
| `n>str` / `$clear $+ $@` | `n -- addr len` / ... | a number's digits; a string built a piece at a time (prelude) |
| `exe-u8` / `exe-s8` / `exe-s16` / `exe-s32` | `addr len i -- n` | element i of a table of the executable (0 past its end or with none) |
| `library-file` | `addr len -- addr n` | a library file's bytes (read once, kept - as many as are asked for); `0 0` if none |
| `exe-table` | `addr len -- addr n` | a table of the executable by name (`s" music" exe-table`: the library's `exe/music.bin`, as `library-file`); `0 0` if none |

The pad's buttons as keys: `scripts/keys.fs` (`pad-circle`, `pad-cross`, ... the d-pad `pad-up` ...).

## The scripts

ClankForth's prelude (the language), `prelude.fs` (the game's few words for every script: `three-digits`,
`seconds`; `IN: forth`), then `game.fs` (`IN: game`), whose `USING:` pulls in the rest; each
file is a vocabulary of the same name. Each subsystem is its own file, with a folder of the same name when it
is bigger (the map: `docs/subsystems.md`). Shared: `keys.fs` (the pad), `messages.fs` (every message, declared
once).

The console (`` ` ``) evaluates whatever is typed, so any of this can be changed while the game runs: redefine
a word and the next tick uses it (words already compiled into others keep the old one; hooks are looked up by
execution token, so re-register with `on-tick` after a change, or go through a `defer`).

## Tests

`tester` (ClankForth's, built in: `USING: tester`): `T{ code -> results }T` runs the code and checks the stack
against the results; `testing name` labels what follows; `test-summary` prints the totals and sets the exit status.

- The language's own tests are ClankForth's; `ctest` runs them with ours.
- `tests/test_*.fs`: the actor kernel, run by the `forth` tool (`tools/forth-tool.c`: ClankForth's with the
  kernel; no game).
- `tests/<subsystem>/test_*.fs`: the game's, headless - `openmizzurna --test file.fs` loads the scripts,
  runs the file and exits with its status. `1 tick-tasks actors-frame` runs a tick,
  `deliver` delivers what is queued; `key: X true key-hold` holds a key. Run them with
  `SDL_AUDIO_DRIVER=dummy` (and `--hidden` for other headless runs).
- `tests/library/test_*.fs`: the game's on its data - run on the library `ctest` imports from the developers' disc
  (`OM_DEV_DISC`; without it they are left out).
- `ctest` runs both (later also the C checks of the library's conversions, `tests/*_check.c`).
