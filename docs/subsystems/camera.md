# The camera

## Purpose

How the game is seen (src: `camera.c`, the events' camera ops). Following, it stands where Matthew stood and
faces as he faced two of the original's frames ago (`camera_follow`'s lag: a chase camera behind his back),
at the scene's eye height, look height and distance (the area metadata's camera table; the town's own:
-700, -600, -2850; -900 eye height behind the car). Following, it takes that profile **every step**
(`camera_follow` writes `g_camera_profile` into the camera each frame), so a shot's distance or heights
(`camera-set`, `camera-dist-to`, `camera-height-to`) last only while it isn't following: a shot set while
Matthew is the player's is overridden at once, and nothing an event leaves outlives the following's return.
An event's `camera-set` fixes it where the event wants while the following is off (the event has control),
until `camera-follow`.

The view itself is the engine's (`camera`: render/scene3d.h): the original's target, heading, distance and
heights, and its projection - 500 indoors, 400 in the town, on a 240-high screen.

## API

- `camera-place ( look-y dist eye-y x y z yaw -- )` (the events' camera_set's order), `camera-follow`,
  `camera-fixed`.
- It listens to `area-entered` and `scene-shown`: behind Matthew again, at the scene's profile.

## Status

## The events' moves (src: camera_tween_update, nav_step)

Each of the original's frames (15 a second while an event runs), before the following (which, when on,
draws over them): the pivot travels toward a point by its speed (across by the heading to it; up and down by
the pitch against x alone, as the original); the heading steps **the step's way** - a positive step turns it
up, a negative one down, whichever way is shorter - and on the frame its goal lies between where it was a step
back and where it is, it is put on the goal (a goal it starts on: once round; the original's quirk with a
negative step, a goal under 2048 and a turn down through 0 that hasn't reached it - the goal raised past reach,
a turn without end - is kept, and no event of the game's meets it); the eye height and distance step to theirs.
The events' `camera-angle` / `camera-turn` with step 0 set the heading at once (src: ops $142, $11C), the
degrees of `camera-turn` turned to 4096ths as C does (toward 0).
`camera-move-to`, `camera-turn-to`, `camera-height-to`, `camera-dist-to`, `camera-face`, `camera-pivot`;
facts `camera-moving?` ... for the events' waits.

Low behind him (`camera-behind ( look-y dist eye-y )`, src: ovl019's crawl): at his place, turning as he turns,
until a scene places the camera or he is the player's again. (Nothing sends it now.)

The door with a fixed camera (`door-animate-alt`, op $13A, src: func_80045008): look -470, distance -812, eye
-1802, heading -693, the pivot where Matthew stands - set whether or not he went through.

**Built**: following (on foot, in the town, behind the car: `camera-driving`), fixed shots, the moves (seen: the
opening's slow pan along the bed), `camera-face` between two figures.
