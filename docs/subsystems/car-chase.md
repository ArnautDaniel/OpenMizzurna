# Car chase

Chasing the bike through the town in Matthew's car (area 27's minigame 21; src: ovl027 `func_801025E0` states
20–26, `carchase_timer_start`, `carchase_end`, `chase_lost`, `func_80106980`;
`decomp/docs/research/minigames/README.md` §4). `scripts/car-chase.fs`, the actor `car-chase`.

## Words, messages and facts

- Minigame 21 (a code minigame, `minigame-begun`: its script's `wait-minigame` waits for its end) begins it.
  Minigame 3 aborts it.
- Area 27's natives:
  - 0x80106950 starts its clock, 2700 frames;
  - 0x801060BC is the won end. Unless they've got away, it gets him out of the car and plays the scene on
    (`entry-8010B678`). Mel goes to stop 0x21, and Dennis, Wolf and Rolein to their stops for the day (the
    town's table `chase-after-stops`). His car goes back to its place by the station.
- Fact: `chase-on?` (the clock running).
- From driving: `put-in-his-car`, `his-car-now`, `step-out`, and the messages `drive-hold`, `car-bumped`,
  `car-nudge`, `left-the-car`. From traffic: `car-speed`.

## Rules

- **Timing.** A step is a frame of the original's (30 a second in the town).
- **Getting in.** His car is placed at (0x70504, 0x649DA), heading 0, and he's put in it (his scene). The bike
  (slot 40) stands at the start of road 0x75.
- **The sweep.** Once he's in, the bike drives road 0x75 at 430, with Mel on it (off her day, motion 0x1655). For
  80 frames the camera sweeps from the bike toward the car, swinging out over its first 32. The car is held.
- **The chase.**
  - The camera: eye −0x600, distance −0x2000, its pivot 4096 ahead of the car along its heading.
  - Within 1700 of the bike: a knock. His car drops to a sixteenth of its speed, the bike is shaken (Mel 0x1666)
    and runs at 530 for 150 frames.
  - The third knock: Mel falls (0x1660), the bike stops, the car is held.
- **The crash.** For 30 frames the bike slides on 0x85 a frame, Mel 128 a frame (her heading + 0x100), and the car
  128 a frame. The camera watches Mel from the car's side. When her motion ends: a fade (effect 11). Then it's
  over: the car's his again and the camera follows.
- **Got away.** When the clock runs out, or he gets out of the car while it runs: their line (`entry-8010B660`),
  the chase ends, the bike stops, and Mel goes to stop 0x16.
- **Leaving the town** aborts it.

## Status

Built (`tests/library/test_car_chase.fs`): with the knocks' and the crash's sounds and rumble, the music stopping,
the bike's own motions (a knock: $1663; the fall: $1664 then $1665 - traffic's `car-anim`), the rider on it at its
place, height and way.
