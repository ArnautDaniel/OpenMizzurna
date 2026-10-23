# The camera

## Purpose

How the game is seen (src: `camera.c`, the events' camera ops). Following, it stands where Matthew stood and
faces as he faced two of the original's frames ago (`camera_follow`'s lag: a chase camera behind his back),
at the scene's eye height, look height and distance (the area metadata's camera table; the town's own:
-700, -600, -2850). An event's `camera-set` fixes it where the event wants until `camera-follow`.

The view itself is the engine's (`camera`: render/scene3d.h): the original's target, heading, distance and
heights, and its projection - 500 indoors, 400 in the town, on a 240-high screen.

## API

- `camera-place ( look-y dist eye-y x y z yaw -- )` (the events' camera_set's order), `camera-follow`,
  `camera-fixed`.
- It listens to `area-entered` and `scene-shown`: behind Matthew again, at the scene's profile.

## Status

**Built**: following, fixed shots. **To come**: the events' tweens (`camera-move`, `camera-turn`,
`camera-height-to`, `camera-dist-to` and their waits), the town's and the car's follow.
