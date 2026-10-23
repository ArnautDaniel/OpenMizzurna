\ area 6's rules on the game's data (scripts/rules/area006.fs): Astin Farm - ○ before a thing examines it, □ takes
\ it; taking the note early means someone's coming (the closet then, not the door); the warning when he heads home
USING: tester engine actors messages clock areas keys matthew townsfolk items events window story-flags talk
    rules.area006 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  3000 0 do  reply  started? 0= if  leave  then  loop ;
: press ( key -- )  dup true key-hold  2 steps  false key-hold  2 steps ;
: in-the-room ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps ;
: before-the-note ( -- )   \ facing it, a step away
    $16026 0 $BEC9 200 - s" matthew" actor-named send figure-place  0 0 0 s" matthew" actor-named send figure-turn
    deliver  2 steps ;
prologue-done flag-set

testing the note is there until taken; ○ before it: "What's this?"; □ takes it
T{ 26 12 0 at-time  got-mels-note flag-clear  in-the-room  events-stop  the-note here? -> true }T
T{ before-the-note  pad-circle press  started?  played-out -> true }T
T{ before-the-note  pad-square press  played-out  got-mels-note flag?  note-item has-item?  the-note here? -> true true false }T
testing taken on the 26th, someone's coming: the way out says hide; the closet hides him
T{ must-hide @ -> 1 }T
T{ the-way-out  started?  played-out  must-hide @ -> true 1 }T
T{ the-closet  started?  must-hide @ -> true 0 }T

test-summary
