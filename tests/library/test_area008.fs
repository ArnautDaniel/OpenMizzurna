\ area 8's rules on the game's data (scripts/rules/area008.fs): the junkyard's bus - the seat looked at twice, then
\ searched; the figure on the bus at its point in the story (another kind's model), talking to it
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk
    fight rules.area008 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  3000 0 do  reply  started? 0= if  leave  then  loop ;
: on-the-bus-now ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps ;
prologue-done flag-set

testing the seat: looked at once, not yet searchable; twice, □ searches it, once
T{ seat-searched flag-clear  on-the-bus-now  events-stop  0 0 look-check  0 0 act-check -> the-seat nothing-to-do }T
T{ the-seat look-do played-out  the-seat look-do played-out  0 0 act-check -> search-the-seat }T
T{ search-the-seat act-do  played-out  seat-searched flag?  0 0 look-check -> true nothing-here }T

testing at its point in the story a figure is on the bus, in another kind's model; talking to it
T{ $01C story@ $20000000 or $01C story!  on-the-bus-now  events-stop  on-the-bus here?  on-the-bus kind-of -> true 1 }T
T{ on-the-bus talks-here  started?  $01C story@ $40000000 and 0<> -> true true true }T

testing the two-player fight's picking (minigame 43): each pad its pick, ○ settles it; then the fight
: tap ( key -- )  dup true key-hold  2 steps  false key-hold  2 steps ;
T{ events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps
   43 minigame  4 steps  minigame-running?  0 picker# picked @ -> true 0 }T
T{ pad-up tap  pad-up tap  pad2-down tap  0 picker# picked @  1 picker# picked @ -> 2 35 }T
T{ pad-circle tap  pad2-circle tap  0 picker# chosen @ 0<>  1 picker# chosen @ 0<> -> true true }T
T{ 60 steps  fighting? -> true }T
testing enough: it's over
T{ vs-over  events-stop  minigame-running? -> false }T

test-summary
