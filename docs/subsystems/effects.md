# The screen's effects

## Purpose

Fades, wipes, the iris and a flash over the picture (src: `effect.c` screen_effect / screen_effect_update),
started by the events (`screen-effect`, `effect-level`; `wait-effect` waits for one to finish).

## The original's table (`screen-effects`: 16 rows of 8 bytes)

kind (1 fade, 2 wipe in, 3 wipe out, 4 iris, 5 tint), the level it starts at (s8), its step a frame (s8),
for how many frames, how it meets the picture (0x40 the PlayStation's subtract - towards black; 0x20 add -
towards white), two colours, its polygons. A level steps each frame while frames remain; then:

- **fade**: the level indexes a grey (`effect-fade-levels`, 0..32 to 0..255); at 0 it ends, otherwise it
  stays (effect 3 fades out and holds black until another).
- **wipes**: a slanted soft edge (colour a to b) at the level - 0x160 .. - 0xA0, solid to its right (2: it
  ends) or left (3: it stays).
- **iris**: 16 segments shaded from an inner ring to an outer, solid beyond; its size the iris level (0 shut,
  16 open; `effect-level from to` ramps it a step every 2 frames, open: it ends); level 1 flickers.
- **tint**: a quick flash, red (cyan subtracted) through `effect-tint-levels`.
- Effect n + 256 is drawn under the message window (the opening's black under "Prologue").

The original stepped it a game frame at a time; the events ran at 15 a second, and so do the effects here.

## Status

**Built** (`scripts/effects.fs`; the 2D layer's add / subtract blending and shaded quads: `pen-blend`,
`draw-quad`). Seen: the opening's black, its slow fade in.
