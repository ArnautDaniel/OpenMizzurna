\ the flare on the game's data (scripts/flare.fs; the glow is the area's picture): minigame 38 in the church lights
\ one at Barton's feet, growing to its full size and staying; leaving the area puts it out; area 5's is minigame 40
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags flare flare.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
prologue-done flag-set

testing minigame 38 in the church: lit at Barton's feet, small at first, its picture the area's
T{ 24 into  38 minigame  deliver  1 steps  lit @ 0<>  who @  half full-half <  picture 0<> -> true father-barton true true }T
testing grown: to its full size, and it stays
T{ 300 steps  grown @  half -> full-grown full-half }T
testing leaving the area: out
T{ 0 into  lit @ -> 0 }T
testing minigame 40 in the warehouse: lit at the carrier's feet
T{ 5 into  40 minigame  deliver  lit @ 0<>  who @ -> true 101 }T

test-summary
