\ the stick-walk sample mod (examples/mods/stick-walk; the walk-intent and follow-yaw hooks): the camera keeps its
\ own facing, turned by the right stick or L1 / R1; Matthew walks where the stick - or here the d-pad - points from
\ it; Tank in the options gives the game's way, its camera too
USING: tester engine keys actors mods figures matthew camera stick-walk ;

: hold ( key flag -- )  key-hold ;
0 setting !   \ (stick: a run before may have kept Tank)

testing the hooks taken
T{ ' walk-intent hook-taken?  ' follow-yaw hook-taken? -> true true }T

testing nothing pushed: standing
T{ walk-intent nip -> standing-pace }T

testing right: a quarter turn right of the camera's facing, walking; up: its way
1000 yaw !
T{ pad-right true hold  walk-intent  pad-right false hold -> 1000 1024 + walking-pace }T
T{ 3000 yaw !  pad-up true hold  walk-intent  pad-up false hold -> 3000 walking-pace }T

testing ✕ held: running; down: toward the camera
T{ pad-down true hold  pad-cross true hold  walk-intent  pad-cross false hold  pad-down false hold
   -> 3000 2048 + 4095 and running-pace }T

testing the camera's facing its own: behind him on a reset, then kept as he turns; R1 / L1 turn it
T{ 1000 true own-facing  2500 false own-facing -> 1000 1000 }T
T{ pad-r1 true hold  2500 false own-facing  pad-r1 false hold -> 1048 }T
T{ pad-l1 true hold  2500 false own-facing  pad-l1 false hold -> 1000 }T

testing the turn toward the stick's way: the shorter way round, either side of the half
T{ 100 4000 way-between  4000 100 way-between  0 1024 way-between  0 3072 way-between  0 2048 way-between
   -> -196 196 1024 -1024 -2048 }T

testing Tank: the game's own way (the pad's tank controls) and its camera (as he faced)
T{ 1 movement-change  walk-intent nip  2500 false own-facing  1 movement-change  tank? -> by-the-pad 2500 false }T

test-summary
