\ the police computer on the game's data (rules/area000.fs, map.fs): the map's other face listing the case files
\ the story has given it; □ on a place plays its file (the list waits), ✕ closes
USING: tester engine actors messages clock areas keys matthew events story-flags ui map map.private rules.area000 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
prologue-done flag-set
25 12 0 date>time s" clock" actor-named send set-time  deliver
0 into  events-stop  true broadcast player-control  deliver

testing opened: the computer, its listed files (not the map's), the world standing still
T{ the-computer-opens  deliver  map-up?  computer @  ui-busy?  1 shown?  0 shown? -> true true true true false }T
testing □ on a listed place: its file plays, the list waiting; over, the list again
T{ pad-r1 tap  chosen @  pad-square tap  file-playing @ 0< 0=  playing-file? -> 1 true true }T
T{ events-stop  deliver  2 steps  playing-file? -> false }T
testing ✕ closes it
T{ pad-cross tap  map-up?  computer @  ui-busy? -> false false false }T

test-summary
