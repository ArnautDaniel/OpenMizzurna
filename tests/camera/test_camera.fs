\ the camera (docs/subsystems/camera.md): the events' turns go the step's way; following, it keeps the scene's
\ heights and distance, whatever a shot or a move left
USING: tester engine actors messages camera ;

: turn ( from goal step -- yaw )   \ the heading after the turn, stepped to its end (src: camera_tween_update)
    {: from g st :}  from yaw !  g turn-goal !  st turn-step !  true turning !
    begin  turning @ while  turn-toward  repeat  yaw @ ;
: turn-frames ( from goal step -- n )   \ how many of the original's frames it takes
    {: from g st :}  from yaw !  g turn-goal !  st turn-step !  true turning !  0
    begin  turning @ while  turn-toward  1+  repeat ;

testing a turn goes the step's way, not the shorter one
T{ 100 300 50 turn -> 300 }T
T{ 100 300 50 turn-frames -> 6 }T                   \ (on the goal, it's taken the frame after)
T{ 300 100 -50 turn  300 100 -50 turn-frames -> 100 5 }T
T{ 300 100 50 turn-frames -> 79 }T                  \ up, the long way round
T{ 2100 2200 -50 turn-frames -> 81 }T               \ down, the long way round
T{ 4000 100 50 turn  4000 100 50 turn-frames -> 100 5 }T    \ up through the full turn
T{ 100 4000 -50 turn  100 4000 -50 turn-frames -> 4000 5 }T  \ down through it
T{ 10384 622 40 turn -> 622 }T                       \ a shot's heading past a turn: taken as its part of one
T{ 100 300 0 turn -> 300 }T                          \ step 0: at once

testing following brings the scene's profile back over a shot's distance and heights
T{ -700 profile-eye-y !  -600 profile-look-y !  -2000 profile-dist !
   -1802 eye-y !  -116 look-y !  -6374 dist !  as-profiled  eye-y @ look-y @ dist @ -> -700 -600 -2000 }T

test-summary
