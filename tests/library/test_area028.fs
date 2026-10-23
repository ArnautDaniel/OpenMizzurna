\ area 28's rules on the game's data (scripts/rules/area028.fs): the closed door's first knock, then the later
\ ones (again each visit); the scene's call that marks them caught
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk
    rules.area028 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  10000 0 do  reply  started? 0= if  leave  then  loop ;
: in-the-cabin ( -- )  events-stop  this-area 1 s" areas" actor-named send enter-area  deliver  2 steps ;
prologue-done flag-set

testing the closed door: the first knock, then the later ones; again the first on another visit
T{ in-the-cabin  the-closed-door  started?  knocked @ 0<> -> true true }T
T{ played-out  the-closed-door  started?  played-out -> true }T
T{ in-the-cabin  knocked @ -> 0 }T
testing the scene's call: caught
T{ hideout-robbers-caught flag-clear  caught  hideout-robbers-caught flag? -> true }T

test-summary
