# Dog chase

Running from the junkyard's dog to the yard's far side (area 8's minigame 26; src: ovl008 `dog_reset`,
`dog_update`, `area_minigame_start` / `update`; `decomp/docs/research/minigames/README.md` §5).
`scripts/dog-chase.fs`, the actor `dog-chase`.

## Messages and facts

- `dog-chase-start`: from the start.
- `dog-chase-stop`: it's over, and the camera follows him again.
- Fact: `chasing?`.
- Area 8's minigame 26 (`rules/area008.fs`, `the-dog-chase`) runs on the minigame's thread: its scene
  (`entry-801032C8`), the chase while its script waits for Matthew within 400 of (106496, 218986)
  (`entry-80103428`), then its end (`entry-8010344C`). The script that started it (getting off the bus) waits
  for it, then goes on to the town.

## Rules

- **Timing.** A step is a frame of the original's (15 a second indoors).
- **The start.** Health 120. Matthew at (0x19F58, 0x336FA); the dog (slot 157) at (0x1A312, 0x339DC), heading
  0xA00, waiting 10 frames.
- **Matthew.** He runs where the d-pad points: the area's table `dog-directions` gives a heading for each
  combination, and he moves 60 a frame (motion 5; standing, motion 0). His facing turns toward it at 0x200 a
  frame. A wall puts him back.
- **The dog.**
  - Waiting: turns toward him at 0x100 a frame. Once lined up it charges, 1 in 4 or at once if he's within
    0x200.
  - Charging: turns 0x30 a frame and speeds up 50 a frame to 150. More than 0x300 off his line, it skids:
    slowing 50 a frame, then waiting again.
- **A bite** (within 0x80 while he's free):
  - 30 off his health, and the flash (effect 14);
  - he staggers for 17 frames (motion 0x772);
  - the dog backs off for 17 frames (0x773), turns away (0x400) and waits 8;
  - he's pushed to 0x100 from it.
- **Out of health.** He falls (0x6C; effect 9), then game over (effect 2). ○ or Start fades out (effect 3) and
  it begins again from the start.
- **The camera** is fixed over him: yaw 0, distance −0xDE2, eye −0xA00, look −0x200.
- **Drawn:** his health bar at (−140, −80), out of 120.

## Status

Built: the chase, the bite, the game over and retry, the camera, the bar (`tests/library/test_dog_chase.fs`).

To come:
- the bite's sound and the pad's rumble;
- the chase's music;
- the barking ambience.
