\ the areas' every-frame hooks on the game's data (areas.fs's area-every-frame!, the rules' - src func_800185A4):
\ a frame of the original's each; the fans turning (area 3, 12, 21) and area 3's object on its side from the 26th
\ 15:00; the church's later set (area 24, from the 25th 14:00 on entering); the fire's flames running while it
\ burns (area 5); the strips (10, 23), the waterfall and the cabin's water (26) and its window patched; the
\ hospital's windows by the hour (2, 29); the town's water a frame every other
USING: tester engine actors messages clock areas events story-flags ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
prologue-done flag-set
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: into ( area scene -- )  events-stop  s" areas" actor-named send enter-area  deliver  2 steps  events-stop  deliver ;
: up? ( object -- flag )  models-shown + c@ 0<> ;
: slot-turned ( object -- slot | 0 )
    #turned 0 do  i turned# to-object @ over = if  drop i turned# unloop exit  then  loop  drop 0 ;
: turn-of ( object -- ry | -1 )  slot-turned ?dup if  to-ry @  else  -1  then ;   \ about the vertical (-1: not turned)
: tilt-of ( object -- rz | -1 )  slot-turned ?dup if  to-rz @  else  -1  then ;

testing area 3: the fans turn a step a frame of the original's; before the 26th 15:00 object 4 isn't turned
T{ 25 12 0 at-time  3 0 into  8 turn-of  10 steps  8 turn-of <>  4 turn-of -> true -1 }T
testing from the 26th 15:00 it's on its side
T{ 26 15 30 at-time  3 0 into  4 steps  4 tilt-of -> $400 }T
testing area 12's and 21's turning ones
T{ 12 0 into  4 steps  9 turn-of 0< 0=  $B turn-of 0< 0= -> true true }T
T{ 21 0 into  4 steps  6 turn-of 0< 0=  8 turn-of 0< 0= -> true true }T

testing the church's sets: before the 25th 14:00 the first, entering later the later one
T{ 25 12 0 at-time  24 0 into  2 steps  $14 up?  $15 up? -> true false }T
T{ 25 15 0 at-time  24 0 into  2 steps  $14 up?  $15 up? -> false true }T

testing area 5's fire: not burning in scene 0, hidden and still; burning, shown and its flames running
T{ 5 0 into  0 events:fire-lit !  4 steps  10 up?  0 texture-frame@ -> false -1 }T
T{ 1 events:fire-lit !  4 steps  10 up?  0 texture-frame@ 0< 0= -> true true }T
T{ 0 texture-frame@  2 steps  0 texture-frame@ <> -> true }T

testing area 10's strip in scene 1, area 23's in scene 0; none elsewhere
T{ 10 0 into  4 steps  0 texture-frame@ -> -1 }T
T{ 10 1 into  4 steps  0 texture-frame@ 0 5 within -> true }T
T{ 23 0 into  4 steps  0 texture-frame@ 0 5 within -> true }T

testing area 26: the waterfall outside, the water in the cabin; the window lit, then dark
T{ 26 0 into  4 steps  0 texture-frame@ 0 16 within  1 texture-frame@ -> true -1 }T
T{ 26 7 into  4 steps  1 texture-frame@ 0 5 within -> true }T
T{ rules.area026:window-lit  2 texture-frame@  3 texture-frame@ -> 0 -1 }T
T{ rules.area026:window-dark  3 texture-frame@ -> 0 }T

testing the hospital's windows: the day's before 16:00, the evening's after (area 2 and 29)
T{ 25 12 0 at-time  2 0 into  2 steps  0 texture-frame@ -> 0 }T
T{ 25 17 0 at-time  2 steps  0 texture-frame@ -> 1 }T
T{ 29 0 into  2 steps  0 texture-frame@ -> 1 }T

testing the town's water: a frame every other of the town's (30 a second)
T{ 27 0 into  2 steps  0 texture-frame@ 0 6 within -> true }T

testing a scene past the area's (area 4 has one): nothing shown, no reverb, nothing read past its table
T{ 4 0 into  0 0 shown?  1 0 shown?  1 1 shown? -> true false false }T
T{ 4 3 into  scene-reverb  0 up?  1 up? -> 0 false false }T
testing the town's metadata (4 bytes: none): no objects of its own
T{ 27 0 into  objects -> 0 }T

test-summary
