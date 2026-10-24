\ driving on the game's data (scripts/driving.fs): ○ at his car gets Matthew in (its scene), unseen, the car his
\ to drive; ✕ speeds it up and on, burning fuel; left / right steer it while it moves; □ brakes; ○ near still gets
\ him out at a door; with an empty tank it goes no faster than 120
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags traffic talk
    floors billboards town.private driving driving.private sound ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: played-out ( -- )  3000 0 do  1 steps  started? 0= if  leave  then  loop ;
: press-for ( key n -- )  over true key-hold  steps  false key-hold ;
$3C02C constant car-x   $21933 constant car-z   $30 constant car-heading   \ (src: its place at the start)
: by-the-car ( -- )   \ in the town, his car parked, Matthew at its driver's side facing it
    events-stop  the-town 0 s" areas" actor-named send enter-area  deliver  2 steps
    his-car car-x 0 car-z car-heading s" traffic" actor-named send car-place  deliver
    car-x car-z car-heading door-side door-forward beside {: x z :}
    x 0 z s" matthew" actor-named send figure-place  0 car-heading across + 0 s" matthew" actor-named send figure-turn
    deliver  2 steps ;
: car-x-now ( -- x )  his-car car-now 2drop drop ;
: car-z-now ( -- z )  his-car car-now drop nip nip ;
prologue-done flag-set

testing ○ at the car: his scene, then at the wheel - unseen, in the car
T{ by-the-car  car-reach?  get-in  started? -> true true true }T
T{ played-out  driving?  matthew-perch -> true perch-in-car }T
testing the engine: its tones held while he drives
T{ sound.private:engine-voices @ 0< -> false }T
testing ✕: it speeds up and moves on
T{ car-z-now  pad-cross 30 press-for  speed @ 0>  car-z-now rot <> -> true true }T
testing burning fuel: a unit each 201 frames of ✕
T{ fuel @  pad-cross 450 press-for  fuel @ - 0> -> true }T
testing steering: right turns it while it moves (from its place again)
: in-again ( -- )  driving? 0= if  by-the-car  get-in drop  played-out  then
    his-car car-x 0 car-z car-heading s" traffic" actor-named send car-place  deliver
    car-heading ch !  car-x cx !  car-z cz !  0e velocity f!  0 steer ! ;
: steer-right ( n -- )  pad-cross true key-hold  pad-right true key-hold  steps  pad-cross false key-hold  pad-right false key-hold ;
T{ in-again  his-car car-now nip nip nip  10 steer-right  his-car car-now nip nip nip = -> false }T
testing its pose as he steers: the front wheels turned ten times the steer, leaning against it, the wheels on
: pose-follows? ( -- turned? leans? spun? )
    his-car car-wheels-now {: spun turned lean :}  turned steer @ 10 * =  lean steer @ negate =  spun 0<> ;
T{ pose-follows? -> true true true }T
testing □: it slows to a standstill
T{ pad-square 120 press-for  20 steps  speed @ abs 9 <= -> true }T
testing ○ near still: out at a door, seen, on his feet
T{ pad-circle 2 press-for  4 steps  driving?  matthew-perch  matthew-on-foot? -> false 0 true }T
testing an empty tank: no faster than 120
T{ in-again  0 fuel !  pad-cross 12 press-for  speed @ 120 <= speed @ 0> and -> true }T
testing a wall ahead: at length it stands against it, the car where it was before the hit
: car-xz ( -- x z )  car-x-now car-z-now ;
T{ fill-up  in-again  pad-cross true key-hold  600 steps  car-xz  4 steps  car-xz  pad-cross false key-hold  rot = -rot = and -> true }T
T{ fill-up  fuel @ -> full-tank }T

testing the town's solid billboards (src: billboard_blocked): one of the car's tile blocks at its foot, not 400 off
: solid-row? ( row -- flag )  sprite nth solid-kinds < ;
: a-solid-one ( -- x z )   \ the first billboard of the car's tile with a solid sprite
    car-x car-z tile-of {: c r :}  c r billboards {: t :}  c r tile-origin {: ox oz :}  -1
    t #records 0 do  t i record solid-row? if  drop i leave  then  loop  {: k :}
    t k record {: b :}  ox b at-x nth +  oz b at-z nth + ;
T{ by-the-car  a-solid-one 2dup solid-billboard?  -rot 400 + solid-billboard? -> true false }T

testing a passenger: in his car at its second seat; out with him, beside that seat, following on foot again
: winona-along ( -- )  winona 1 s" townsfolk" actor-named send person-ride  deliver ;
T{ by-the-car  winona-along  with-passenger  deliver  winona riding?  following? -> true true }T
T{ step-out  deliver  2 steps  winona riding?  companion-mode @  winona here? -> false 1 true }T
T{ winona s" townsfolk" actor-named send person-unride  deliver -> }T

test-summary
