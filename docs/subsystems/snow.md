# Snow

## Purpose

The snow falling (src: `snow.c`: func_8003AE04, func_8003B0C0, the flakes' loops 0x8003C90C / 0x8003CB48;
`clock.c` func_80025224).

## Rules

- **How a scene snows** (the area's part 6, `cdbin/NNN_4/6.bin`: 28 bytes a scene - s16 kind, count, then x z
  heading for each emitter): kind 0 around the camera (the town), kind 1 from emitters (a scene's windows: up to
  four places from the area's origin, each its own 4096 cube turned by its heading, every fourth flake its), else
  none.
- **The flakes** (up to 512; the engine's `src/game/snow.c`): around the camera a wrapping cube of 8192 world
  units; they start 0 - 2047 above the ground (the camera's pivot; new ones 2048 more), fall 14 - 17 a game frame
  and drift 5 and a sway (`snow-wobble`: 32 steps); landed, a flake lies 6 frames and starts again on top while
  the live count lasts.
- **How many**: 300 while it snows (daylight.fs: the weather), easing one a game frame; a jump in time sets it at
  once.
- **Drawn**: 8 x 8 sprites of the original's screen (`sky/snow.png`, cut at the import), a column by how near
  (nearer, bigger flakes), a row by their fall or lying; half grey, added to the picture.

Engine: `snow-scene ( kind )`, `snow-emitter ( i x z heading )`, `snow-flakes ( n )`, `snow-live`, `snow-step`
(a game frame), `draw-snow ( image x y z )` (about the camera's pivot); scene3d's `scene3d_sprite`.

## Notes

The flakes meet the scene by its depth (the original sorted them into its ordering table by distance - where a
flake meets the ground the two can differ by a pixel).
