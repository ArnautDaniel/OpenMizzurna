# Flare

A flare's glow on the ground at someone's feet (src: snow.c `ground_flare_draw`; minigame 38 in area 24, 40 in area
5). `scripts/flare.fs`, the actor `flare`.

## Messages

- `flare-on ( slot )`: lights it at their feet.
- Area 24's minigame 38 lights it on Barton; area 5's minigame 40 on slot 101 (both code minigames,
  `minigame-code!`).
- Entering another area puts it out.

## Rules

- **The picture** is the area's own, `flares/<area>.png`. The importer cuts it from that area's textures: page
  (0x380, 0), palette (0x320, 0xFE), u 0x50, v 0x10, 24 × 24.
- **Drawing.** It's a square lying flat at their height, added onto the scene (the page's blend 2). It's lifted 8
  above the floor: the original sorted polygons by draw order and had no depth buffer.
- **Growing.** Its half size is 256 · sin(t), with t rising 0x10 a frame of the original's up to a quarter turn
  (0x400). It then stays at that size.

## Status

Built (`tests/library/test_flare.fs`).
