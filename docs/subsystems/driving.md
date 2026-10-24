# Driving

Matthew driving his car (slot 26) in the town (src: ovl027 `car_update`, cevcon.c `func_8004175C`,
`func_8004301C`, `car_exit`, `car_can_exit`, hud.c `fuel_gauge_draw`; `decomp/README.md` *Driving*).
`scripts/driving.fs`, the actor `driving`.

## Facts and words

- `driving?`; `fuel` (0–120, a new game starts full).
- `fill-up`: the gas station's full service (area 27's native 0x80106D04).
- ○ at the car (in the town, before him within reach: look's `circle-first!`) puts him beside the driver's door
  (−0x1D6, −0x64 from the car, facing across it) and plays his scene (`into-the-car`, main 0x800965B0). Its native
  (0x8004301C) puts him at the wheel.
- Messages used:
  - `car-place` (traffic): the car where he has it;
  - `camera-driving` (the camera): eye −900 behind the car;
  - `figure-shown`, `figure-on-foot` and `figure-perch 4` (Matthew): unseen, the pad the car's, in the car.
  
  Look and act stand aside while he's in the car.

## Rules

- **Timing.** A step is a frame of the original's (30 a second in the town).
- **Steering.** Left / right steer 8 a frame, to min(speed / 5, (800 − speed) / 12). Let go, it eases back 4 a
  frame. It turns the car only while it moves.
- **The pedals.**
  - ✕ adds 0.4 a frame, to 8 (2 with an empty tank), and uses a unit of fuel every 201 frames.
  - □ takes 0.4 off, down to −2.
  - Neither: it coasts down 0.1 a frame; between −0.4 and 0.1 it stops; below −0.4 it comes back by 0.4.
  - Speed is that × 60 units a frame.
- **Walls.** Ahead (a point a speed's length on): an obstacle of the town's floor or a solid billboard. The car
  tries 0x20 more each way, up to 0x200, moves that way and steers 0x32 toward it. After the move its box's two
  corners (±300) at the new point and the half-step point are tested; a hit stops it where it was.
- **Getting out.** ○ when |speed| ≤ 9 puts him at the first of the car's 6 seat points that's free, seen, on his
  feet. The car stays where it is.
- **A gas pump** (the town's rules, `rules/area027.fs`: src func_801069F4): not full (under 110), nearly still,
  facing along the pumps, inside one of the two pumps' boxes - ○'s prompt is the fill one, and ○ stops the car
  and plays the fill scene instead of getting out (`car-at-pump`).
- **Road blocks** (src: car_road_block): by the tile (column + row x 100) and the zone under the car (its two
  corners): building fronts in zone 0 (314, 612, 623, 723, 823, 1028, 1202, 1407, 2018, 2019, 2023), others in
  zone 1 (512, 1317, 1902, 2008), the police cordon's (922, 1022) in zone 0 until the 28th 03:00 - looking
  ahead where it is, moving where it would be; blocked, it stops. (`tests/library/test_blocking.fs`.)
- **Leaving the town** ends the drive.
- **Drawn** (from the HUD's sheet, `ui/clock.png`): the fuel gauge's scale at (−144, 30) with its marker
  easing a unit a frame toward the fuel; the speed's three digits (a sixth of it, jittering from 80 up) at
  (−128, 74) and "Km/h", over a dark backdrop.
- **With a passenger** (native $8004305C): at the wheel, and the one along with him rides in the car
  (`person-ride` mode 2).

## Status

Built (`tests/library/test_driving.fs`, the pumps `test_town_frame.fs`): with the scrapes' and crashes' rumble
and sounds, the engine's tones, a passenger (in at the second seat, out with him), and unsticking (src:
func_80106370: standing in a wall or someone's place - people and cars don't stop it otherwise - moving, or every
32 frames, it goes to the nearest free spot 100 at a time out to 1400, x then z), and its pose (src:
func_80109430, func_8001A2F4: it leans against the steer; its wheels - the model's parts 3 - 6, 1 - 4 on the
smaller cars - turn on by half the speed a frame, the front two turned ten times the steer, the other way
backing; traffic's `car-pose`, `car-wheels-now`); on the ground's slope it's tilted with it (src: func_8001C264;
`car-tilt`), and a jolt - its tilt changing by 0x33, or flat and its height so - bumps the pad (src: car_bump).

The boat is boat.md's.
