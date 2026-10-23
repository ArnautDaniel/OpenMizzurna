\ area 11's rules on the game's data (scripts/rules/area011.fs): the inn's room - a scene the first time in, the
\ crate, the thing that can only be taken once found
USING: tester engine actors messages clock areas keys matthew townsfolk items events window story-flags talk crate
    rules.area011 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  3000 0 do  reply  started? 0= if  leave  then  loop ;
: in-the-room ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps ;
prologue-done flag-set

testing the first time in: its scene, once
T{ kathys-room-seen flag-clear  in-the-room  started?  played-out  kathys-room-seen flag? -> true true }T
T{ in-the-room  started? -> false }T
testing the thing: there until taken; □ before it's found does nothing; found, □ takes it
T{ got-epoch-pills flag-clear  0 pending-scenes session!  in-the-room  the-find here? -> true }T
T{ the-find take-it -> false }T
T{ the-find look-at-it drop  played-out  the-find take-it drop  played-out  got-epoch-pills flag?  the-find-item has-item?
   the-find here? -> true true false }T

test-summary
