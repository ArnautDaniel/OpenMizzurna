# Sky

## Purpose

The sky behind the scene (src: `sky.c`, its quads set up in `hudprim.c` func_8001E2E4 / func_8001E5FC, its
colours and shades `clock.c` func_800248C0, func_80024E48, func_80024FAC). Drawn behind the 3D every frame an
area is played (indoors it shows only where the scene opens to it).

## Pictures (the library's `sky/`, cut at the import from the common images: `033_3/0/2`)

`clouds.png` (256 x 240), `horizon.png` (the mountains, 256 x 64), `sun.png` (64 x 64), `flare-1` .. `-14`,
`moon-1` .. `-3` (its phases). A texel marked to blend (alpha 128) is added to the picture behind it (the
horizon strip: averaged); a solid one (255) covers it - the PlayStation's semi-transparency (pen-blend 3 / 4).

## Rules (`scripts/sky.fs`), in the original's screen (320 x 240, the origin at the centre)

- **Sideways** it scrolls with the camera's heading: a full turn is 2048 pixels; the pieces wrap every 2048.
- **The horizon**: where the scene's horizon is, from the camera (up from the centre as it looks down:
  h (look-y - eye-y) / dist). (The original took it from the camera's pitch by a rough sum.)
- **Its colour** (`hour-sky-top`, `hour-sky-low`): the whole screen the top colour kept dim (red 20 - 70, green and
  blue 30 - 80); above the horizon the top colour, and from 120 above it down to 7 above it a gradient to the low
  colour. Not clear: each colour the sum of its three halved, kept as a byte (as the game did).
- **Stars**: 20, twinkling (a frame dim, a frame bright, bluish), from 16:00 to 8:00 when it's clear; repeated
  every 320 pixels across a wider window.
- **Clouds**: 8 pieces above the horizon and 8 mirrored above them, drifting a pixel every 8 frames, their shade
  by the hour (`hour-clouds`; halved when overcast, quartered in snow).
- **The horizon strip**: 8 pieces just above the horizon, shaded by the hour (`hour-horizon`).
- **The sun**: from 8:00 to 16:00 in clear weather, in the east before noon and the west after, its height by the
  hour (`sun-heights`, the hour's entry read as a byte, as the game did); and its 14 lens flares along the line
  from it through the screen's centre (`flare-distances`, `flare-sizes`), drawn over the scene.
- **The moon**: at night in clear weather, rising an hour later each day (`moon-heights`), its phase by the date.
- The colours and shades ease a step every other frame; a jump in time or a new area sets them at once.

## To come

Snow falling (snow.c), the lens flares' order among the overlays.
