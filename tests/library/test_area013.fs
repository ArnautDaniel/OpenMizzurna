\ area 13's rules on the game's data (scripts/rules/area013.fs): the park - the old man there by the hour, meeting
\ him (his character pack's talk), his lessons by the one reached (a minigame that's a script), the lights
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk
    rules.area013 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  5000 0 do  reply  started? 0= if  leave  then  loop ;
: in-the-park ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver
    $41000 0 $50000 s" matthew" actor-named send figure-place  deliver  2 steps ;
prologue-done flag-set

testing at 15:00 he's here (his hours 14:05 - 17:00); at 18:00 not
T{ 26 15 0 at-time  in-the-park  he's-here @  played-out -> true }T
T{ 26 18 0 at-time  in-the-park  he's-here @  played-out -> false }T
testing by day, having heard of him: meeting him (his pack's talk), once; then a lesson's offer
T{ 26 15 0 at-time  in-the-park  played-out  1 heard-of-him story!  met-him flag-clear
   the-old-man talks-here  started?  played-out  met-him flag? -> true true true }T
testing a lesson: the one reached plays (a script); learned, the next
T{ 0 lesson !  39 minigame  started?  played-out  lesson-learned  lesson @ -> true 1 }T
testing by day on floor 1, its first row of lights; by night its fourth
T{ 26 12 0 at-time  in-the-park  played-out  2 steps  lights-row @ -> 0 }T
T{ 26 22 0 at-time  2 steps  lights-row @ -> 3 }T

test-summary
