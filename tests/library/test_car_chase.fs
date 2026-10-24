\ the car chase on the game's data (scripts/car-chase.fs, area 27's minigame 21): Matthew put in his car, then the
\ bike sets off and the camera sweeps; the chase - each knock slows his car, shakes the bike; the third brings it
\ down, the crash, it's over - won: on to the station. Its time up, or out of the car, they've got away
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags traffic
    driving driving.private car-chase car-chase.private sound ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: replying? ( -- flag )
    window-up?  window:stage @ window:finished =  window:stage @ window:page-full = or  window:stage @ window:asking = or  and ;
: reply ( -- )  replying? pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  5000 0 do  reply  started? 0= if  leave  then  loop ;
: until-stage ( s -- )  3000 0 do  dup stage @ = if  leave  then  1 steps  loop  drop ;
: in-town ( -- )  events-stop  28 12 0 date>time s" clock" actor-named send set-time  deliver
    the-town 0 s" areas" actor-named send enter-area  deliver  2 steps ;
: to-the-bike ( -- )   \ his car moved up to the bike
    bike drop  his-car-now drop {: bx bz cx cz :}  bx cx - 200 -  bz cz -  s" driving" actor-named send car-nudge  deliver ;
prologue-done flag-set

testing begun: his car at its start, him put in it (his scene), then the bike off and the sweep
T{ in-town  21 minigame  deliver  2 steps  stage @  started?  minigame-running? -> getting-in true true }T
T{ played-out  2 steps  driving?  stage @  the-bike car-driving? -> true sweeping true }T
testing the sweep over: the chase, three knocks to go
T{ chasing until-stage  stage @  knocks @ -> chasing 3 }T
testing a knock: his car slowed, the bike shaken a while
T{ to-the-bike  2 steps  knocks @  timer @ 0>  last-sound @  the-bike car-motion-now -> 2 true knock-sound bike-wobbles }T
testing the third: the crash, the rider down, over
T{ 0 timer !  1 knocks !  to-the-bike  2 steps  stage @  the-bike car-motion-now -> crashing bike-falls }T
T{ off until-stage  stage @  minigame-running? -> off false }T
testing won (its native after the wait): out of the car, on to the station
T{ start-clock  won  started?  driving?  chase-on? -> true false false }T
T{ played-out  area @ -> 0 }T

testing its time up: they've got away - their line, the rider back to their day
T{ in-town  21 minigame  deliver  played-out  start-clock  1 chase-left !  2 steps  chase-on?  stage @  started? -> false off true }T
T{ played-out  mel stop-of -> rider-got-away }T
testing out of the car while it's on: got away too
T{ in-town  21 minigame  deliver  played-out  start-clock  2 steps  step-out  deliver  2 steps  chase-on? -> false }T
T{ played-out -> }T

test-summary
