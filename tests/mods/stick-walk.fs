\ the stick-walk sample mod (examples/mods/stick-walk; the walk-intent hook): where the stick - or here the d-pad -
\ points on the screen, with the camera's facing kept while it's pushed; Tank in the options gives the game's way
USING: tester engine keys actors mods matthew camera stick-walk ;

: hold ( key flag -- )  key-hold ;
0 setting !   \ (stick: a run before may have kept Tank)

testing the hook taken
T{ ' walk-intent hook-taken? -> true }T

testing nothing pushed: standing, nothing held
T{ walk-intent nip -> standing-pace }T

testing right: a quarter turn right of the camera's facing, walking
1000 yaw !
T{ pad-right true hold  walk-intent  pad-right false hold -> 1000 1024 + walking-pace }T

testing the camera's facing kept while pushed: it turns, the way doesn't
T{ pad-up true hold  walk-intent 2drop  3000 yaw !  walk-intent  pad-up false hold -> 1000 walking-pace }T
T{ walk-intent nip  pad-up true hold  walk-intent  pad-up false hold -> standing-pace 3000 walking-pace }T

testing ✕ held: running; down: toward the camera
T{ pad-down true hold  pad-cross true hold  walk-intent  pad-cross false hold  pad-down false hold
   -> 3000 2048 + 4095 and running-pace }T

testing Tank: the game's own way (the pad's tank controls)
T{ 1 movement-change  walk-intent nip  1 movement-change  tank? -> by-the-pad false }T

test-summary
