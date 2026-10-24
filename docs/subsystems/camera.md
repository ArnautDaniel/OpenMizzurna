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

## The events' moves (src: camera_tween_update, nav_step)

Each of the original's frames (15 a second while an event runs): the pivot travels toward a point by its speed
(across by the heading to it; up and down by the pitch against x alone, as the original), the heading steps
toward its goal the shorter way (step 0: at once), the eye height and distance step to theirs.
`camera-move-to`, `camera-turn-to`, `camera-height-to`, `camera-dist-to`, `camera-face`, `camera-pivot`;
facts `camera-moving?` ... for the events' waits.

Low behind him (`camera-behind ( look-y dist eye-y )`, src: ovl019's crawl): at his place, turning as he turns,
until a scene places the camera or he is the player's again.

**Built**: following (on foot, in the town, behind the car: `camera-driving`), fixed shots, the moves (seen: the
opening's slow pan along the bed), `camera-face` between two figures.
