\ the real-drive sample mod (examples/mods/real-drive; the car-handling, car-moving, engine-note and follow-yaw
\ hooks): its engine's torque and gears, the drag, and the tyres' simulation settling - not spinning up - at speed
USING: tester engine mods camera driving driving.private real-drive ;

: hundredths ( F: x -- n )  100e f* fround f>s ;
0 settings game-driving !  0 settings manual-gears !   \ (a run before may have kept other settings)

testing the hooks taken
T{ ' car-handling hook-taken?  ' car-moving hook-taken?  ' engine-note hook-taken?  ' follow-yaw hook-taken?
   -> true true true true }T

testing the torque curve: its points, between them, past its ends
T{ 4500e torque hundredths  1000e torque hundredths  7000e torque hundredths  3500e torque hundredths
   500e torque hundredths  8000e torque hundredths -> 100 60 80 91 60 80 }T

testing the gears: up past the red line's edge with the gas, down low; never past 5th or below 1st
: in-gear ( n F: revs t -- )  throttle f!  revs f!  gear !  0 shifting ! ;
T{ 2 6700e 1e in-gear  auto-shift  gear @  shifting @ 0> -> 3 true }T
T{ 2 6700e 0e in-gear  auto-shift  gear @ -> 2 }T
T{ 3 2000e 0e in-gear  auto-shift  gear @ -> 2 }T
T{ 5 6900e 1e in-gear  auto-shift  gear @  1 1000e 0e in-gear  auto-shift  gear @ -> 5 1 }T
testing no drive while it shifts, nor past the red line
T{ 2 4500e 1e in-gear  3 shifting !  engine-push f0=  0 shifting !  7000e revs f!  engine-push f0= -> true true }T
T{ 1 4500e 1e in-gear  engine-push hundredths -> 420 }T

testing the drag: against the way it moves, none still
T{ 0e throttle f!  300e drag f0<  -300e drag f0>  0e drag f0= -> true true true }T

testing the tyres at speed: the turn settles near the grip's limit, not spinning up
: settle ( F: u lock -- lateral-g )
    lock f!  {: F: u :}  0e sideways f!  0e yaw-rate f!  0e push-was f!
    200 0 do  4 0 do  u 0e 0.25e turning-step  loop  loop  u yaw-rate f@ f* g f/ ;
T{ 700e 0.03e settle  fdup 0.5e f>  tyre-grip 1.1e f* f< -> true true }T
T{ 700e 0.1e settle  tyre-grip 1.1e f* f<  sideways f@ fabs 200e f< -> true true }T   \ (its lock at that speed, all of it)
T{ 150e 0.4e settle  fdup 0.5e f>  tyre-grip 1.1e f* f< -> true true }T

testing Game in the options: the game's own driving (not at the wheel here: the game's way either way)
T{ 1 driving-change  game's?  1 driving-change  game's? -> true false }T

test-summary
