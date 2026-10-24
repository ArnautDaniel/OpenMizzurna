# Rumble

The pads' motors (src: frame.c `pad_read_port`'s motors, the events' op $155, the minigames' rumbles).
`scripts/rumble.fs`, the actor `rumble`; the engine's `pad-rumble ( port small? large -- )` (SDL's gamepad
rumble, renewed each tick).

- A pad has two motors: the small one, on or off, and the large one at a strength from 0 to 255. Each runs for so
  many of the original's pad frames (60 a second; −1 for ever, 0 off) and then stops.
- **Messages:** `rumble-small ( port frames )`, `rumble-large ( port strength frames )`, `rumble-stop`.
  `shake ( small-frames strength large-frames )` shakes the first pad.
- **Op $155** (`rumble ( strength frames )`): 0x100 runs the small motor, anything else the large one at that
  strength.
- **Who shakes it:**
  - a fighter hurt: their own pad, by the damage × 8; a knock-out at full;
  - a shot in the shoot-outs;
  - the dog's bite;
  - the car chase's knocks;
  - the car's scrapes (by the speed) and its crashes;
  - the rope's pulls and its last one.
- **Ports.** Port 0 is the first gamepad. Port 1 is the second: its buttons are keys of their own (scripts/
  keys.fs's `pad2-*`: I K J L the d-pad, O ○, U ✕, Y □, P △, 7 L1, 9 R1, H Start, G Select), so the keyboard can
  play it too.
