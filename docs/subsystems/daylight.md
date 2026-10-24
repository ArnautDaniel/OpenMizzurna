# Daylight

## Purpose

The day's light and the weather (src: `clock.c`: func_800234B4, func_80023D70, scene_lights_apply,
func_800252A4; the scenes' light records, `area.c` scene_lights). What the lit models are drawn in.

## Rules (`scripts/daylight.fs`)

- **The hour's light** (the executable's tables by the hour: `hour-light-1` .. `-3` and their `-way`s,
  `hour-ambient`, `hour-fog`): three lights - each its colour and the way it shines (the game's axes, y down) - an
  ambient (at least 63 63 75) and the fog's colour. A minute's value lies between this hour's entry and the next's.
  When it isn't clear each colour is grey: the mean of its three.
- **Easing**: the colours (lights, ambient, fog) step 1 toward the hour's a frame; a jump in time
  (`time-set`) or a new area sets them at once (src: clock_set).
- **Outdoors** (the town): the hour's lights, the ambient (x16 of 4096), and the fog.
- **Indoors**: the scene's light record (the area's light table, 34 bytes a scene: three lights, an ambient, a
  level, a mode). The hour's values are scaled to the level (100: as they are). Mode 2: as outdoors, fog too.
  Else: each light the way of the brighter of the scene's and the hour's; each colour the brighter one (mode 0:
  always the hour's); the ambient x10; no fog.
- **The weather**: clear, overcast or snowing; each minute, when its time is up (90 - 217 minutes), a new one by
  chance - unless held (`set-weather ( w hold )`, src: func_80025384). It snows on the morning of 12/25 and clears
  on the last evening (12/31 from 20:00). Fact: `the-weather`.

## The engine's light

`light-way ( i x y z )`, `light-color ( i r g b )` (256 full), `ambient-light ( r g b )` (4096 full),
`fog-color ( r g b )` (128: the texture as it is), `fog! ( flag )`. A lit model's vertex colour is multiplied by
the ambient plus each light by how much the face turns to it (at most 255 / 128); in fog it then moves toward
the fog's colour by the PlayStation's depth cue: 1.25 - 12.5 h / depth (h the projection, 400 in the town; the
depth in model units) - clear to 4000, all fog from 20000. Unlit models: neither.

## Also

A shot's flash (`muzzle-flash`: three bright lights for a frame, src shootout_flash_lights). The sky and the snow
are their own pages (sky.md, snow.md). The original's scripted ambient fade (func_80025330) has no caller.
