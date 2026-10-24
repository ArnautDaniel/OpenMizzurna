\ the shoot-out on the game's data (scripts/shootout.fs; the rounds' rows and camera paths are the areas' tables):
\ begun, full health and the first row's targets up; the right button clears its target and, its time up, a hit;
\ a row let go, a miss - the scene, then Matthew's health down; ten hits win a round; out of health, game over
\ and ○ from the start; the last round won, it's over (the forest's: the bear down); the warehouse's script
\ waits on it; a shot's flash
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags shootout
    shootout.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: replying? ( -- flag )
    window-up?  window:stage @ window:finished =  window:stage @ window:page-full = or  window:stage @ window:asking = or  and ;
: reply ( -- )  replying? pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: press ( key -- )  dup true key-hold  5 steps  false key-hold  5 steps ;
: until-stage ( s -- )  2000 0 do  stage @ over = if  leave  then  reply  loop  drop ;
: until-aiming ( -- )  aiming until-stage ;
: until-not-aiming ( -- )  400 0 do  1 steps  stage @ aiming <> if  leave  then  loop ;
: until-row ( -- )   \ a row's targets just up
    400 0 do  1 steps  stage @ aiming =  targets-shown? and  row-left @ row @ 2 row-u8 1- >= and if  leave  then  loop ;
: until-over ( -- )  3000 0 do  reply  shooting? 0= if  leave  then  loop ;
: clear-targets ( -- )  #targets 0 do  i target# up @ if  i target# button @ button-key press  then  loop ;
: first-target ( -- t )  0 target# ;
: in-the-cabin ( -- )
    events-stop  26 7 s" areas" actor-named send enter-area  deliver  2 steps
    rules.area026:the-gun rules.area026:cabin-gun-x rules.area026:gun-y rules.area026:cabin-gun-z
    s" townsfolk" actor-named send extra-place  deliver   \ (the gun on the floor, as the run into it leaves it)
    rules.area026:the-shootout s" shootout" actor-named send shootout-start  deliver ;
prologue-done flag-set  rules.area026:bear-down flag-clear

testing begun: on, full health, the first row's target up (its button the table's: △), two shots
T{ in-the-cabin  until-row  shooting?  round @  health @  pools @  shots @  first-target button @ -> true 0 100 90 2 triangle }T
testing the gun on the floor hidden with him while aiming
T{ rules.area026:the-gun here? -> false }T
testing the wrong button: a shot spent, the target still up
T{ pad-cross press  shots @  first-target up @ 0<> -> 1 true }T
testing a shot's flash: the scene lit by the three bright lights for a frame, its own after
T{ s" daylight" actor-named send muzzle-flash  deliver  daylight.private:flash-due @  1 steps  daylight.private:flash-due @
   -> true false }T
testing the right one: the target down; its time up, a hit (3 off the bear)
T{ pad-triangle press  first-target up @  hits @ -> false 0 }T
T{ 40 steps  hits @  pools @ -> 1 87 }T
testing a row let go: a miss, its scene; then the swipe's damage (18)
variable before
T{ until-row  missed-scene until-stage  stage @  health @ before ! -> missed-scene }T
T{ until-aiming  before @ health @ - -> 18 }T
testing ten hits: the round won, its scene, then the second round
T{ until-row  9 hits !  clear-targets  until-not-aiming  stage @ -> won-scene }T
T{ until-aiming  round @  hits @ -> 1 0 }T
testing out of health: the scene, game over; ○ from the start, full health
T{ 1 health !  until-row  game-over until-stage  stage @  shooting? -> game-over true }T
T{ pad-circle press  stage @  round @  health @  pools @ -> aiming 0 100 90 }T
testing the last round won: its scene, then over - the bear down, the hunters to the cabin
T{ 2 round !  until-row  9 hits !  clear-targets  until-not-aiming  stage @  rules.area026:the-gun here? -> final-scene true }T
T{ until-over  shooting?  rules.area026:bear-down flag?  rules.area026:hunt @ -> false true rules.area026:after-the-shootout }T
T{ settled -> }T

testing the warehouse's: its script's question answered while it's on
T{ events-stop  5 1 s" areas" actor-named send enter-area  deliver  2 steps
   rules.area005:the-shootout s" shootout" actor-named send shootout-start  deliver  rules.area005:shootout-on -> 1 }T
T{ false shootout-going !  rules.area005:shootout-on -> 0 }T

test-summary
