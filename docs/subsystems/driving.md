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
- **Leaving the town** ends the drive.
- **Drawn:** the fuel gauge, a strip at (−144, 30) with its needle easing toward the fuel; the speed (a sixth of
  it) at (−128, 74). The gauge's and the digits' pictures are to come.

## Status

Built (`tests/library/test_driving.fs`). To come:
- the road blocks (`car_road_block`: 18 tiles' zones, before 12/28 03:00);
- other people and cars in the way;
- the pad's rumble (its scrapes and crash sound, its engine held and pitched by the speed: sound.md);
- a passenger getting in (`into-the-car-with`);
- the car's lean and wheels (`func_80109430`);
- unsticking it (`func_80106370`);
- the boat.
