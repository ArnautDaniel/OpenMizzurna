# The map

The town map screen (src: `mapscreen.c`). `scripts/map.fs`, the actor `map`.

- **Opening.** Choosing the map in the item ring opens it. The world stands still while it is up (`ui-lock`).
- **The picture.** The town's map (page $17 in its first palette: `ui/map.png`, 256 × 240) is drawn at
  (−160, −96) to (160, 96), with the ✕ EXIT hint.
- **The places**, 22 (src: D_8009A520 `map-places`, D_8009A538 `map-dots`, D_8009A590 `map-icons`). Each
  has:
  - a dot at (x − 154, 100 − y);
  - a red line from the dot to its icon;
  - its icon (`ui/map-icon-<k>.png`).

  A place flagged $40 is shown only once the map knows it (story-flags.fs's `map-known`, set by the events'
  `map-reveal`); the rest always are.
- **His arrow.** A triangle at his place (x / 1998 − 150, 104 − z / 2497), turned with his heading. While he
  drives it is the car's. Its red pulses between $10 and $7F, 6 a game frame.
- **The hand.**
  - The d-pad moves it 2 a game frame (8 with △ or ○ held) within (−144, −96)..(136, 88).
  - R1 and L1 jump it to the next and the one before of the places shown, onto the icon.
  - Over an icon, the place's name (CD.BIN 034_3's text: text bank 1000 + the entry) shows in a dark bar at
    (−8, 64).
- **✕** closes it.
- Others read the fact `map-up?`.

## Status

**Built:** the screen, the places, the hand, the names, his arrow.

**To come:** ovl000's own map (the station's: the other palette and the other known set, D_800A34EC) with
that area's rules.
