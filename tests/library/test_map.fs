\ the town map on the game's data (scripts/map.fs): chosen in the ring it opens, the world standing still; the
\ places shown (some only once the map knows them); R1 and L1 jump the hand to the next and the one before, its
\ name under it; the d-pad moves it; ✕ closes
USING: tester engine actors messages clock areas keys matthew events story-flags ui map map.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
prologue-done flag-set
0 map-known !
0 into  events-stop  true broadcast player-control  deliver

testing chosen in the ring: open, the world standing still, the hand in the middle
T{ 2 -1 broadcast ring-chosen  deliver  map-up?  ui-busy?  hand-x @  hand-y @ -> true true 0 0 }T
testing the places: Barrows Forest always; Cohen's lodge only once the map knows it
T{ 1 shown?  0 shown?  1 map-known !  0 shown?  0 map-known ! -> true false true }T
testing R1: to the next place shown, the hand on its icon, its name under it; L1 back round to the last shown
T{ pad-r1 tap  chosen @  under-hand -> 1 1 }T
T{ pad-l1 tap  chosen @  pad-l1 tap  chosen @ -> 13 9 }T
testing the d-pad moves the hand 2 a game frame
T{ hand-x @  pad-right true key-hold  8 steps  pad-right false key-hold  hand-x @ swap -  dup 0>  swap 2 mod -> true 0 }T
testing ✕ closes it
T{ pad-cross tap  map-up?  ui-busy? -> false false }T

test-summary
