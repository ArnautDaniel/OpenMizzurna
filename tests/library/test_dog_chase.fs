\ the dog chase on the game's data (scripts/dog-chase.fs, area 8's minigame 26; the d-pad's headings the area's
\ table): off the bus with the seat searched, its scene, then the chase - full health; the d-pad runs him; left
\ standing, the dog comes and bites (30); out of health, game over and ○ from the start; at the yard's far side
\ he's got away: its end, out to the town
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk
    dog-chase dog-chase.private rules.area008 sound ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: replying? ( -- flag )
    window-up?  window:stage @ window:finished =  window:stage @ window:page-full = or  window:stage @ window:asking = or  and ;
: reply ( -- )  replying? pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  20000 0 do  reply  started? 0= if  leave  then  loop ;
: until-chasing ( -- )  5000 0 do  reply  chasing? if  leave  then  loop ;
: until-bitten ( -- )  3000 0 do  1 steps  health @ full-health < if  leave  then  loop ;
: until-him ( stage -- )  3000 0 do  1 steps  dup him @ = if  leave  then  loop  drop ;
: press-for ( key n -- )  over true key-hold  steps  false key-hold ;
prologue-done flag-set  seat-searched flag-set  $01C story@ $20000000 invert and $01C story!

testing off the bus, the seat searched: its scene, then the chase from the start
T{ events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps
   off-the-bus  until-chasing  chasing?  health @  him @  dog @ -> true 120 free waiting }T
testing the d-pad: right runs him that way, facing it
T{ his-x @  pad-right 20 press-for  his-x @ <  facing-goal @ -> true 1024 }T
testing left standing: the dog comes and bites
T{ until-bitten  health @  dog @  last-sound @ -> 90 backing-off bite-sound }T
testing out of health: down, game over; ○ from the start, full health
T{ 1 health !  free him !  his-x @ dog-x !  his-z @ 50 + dog-z !  charging dog !  down until-him  game-over until-him  him @ -> game-over }T
T{ pad-circle 2 press-for  free until-him  health @  his-x @ -> 120 his-start-x }T
testing at the yard's far side: got away - its end, out to the town
T{ 106496 his-x !  218986 his-z !  played-out  chasing?  area @ -> false the-town }T

test-summary
