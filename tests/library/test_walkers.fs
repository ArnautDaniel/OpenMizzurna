\ walkers on the game's data (scripts/walkers.fs; the town's footpaths, the common container's): one started at a
\ node, along its first edge toward its destination; stopped, it stands. The stalker (rules/area027.fs): minigame
\ 23 takes Matthew to the town and sets him off round his loop (0 -> 3 -> 9 -> 0); a leg done, the next; talked to,
\ caught - his scene, the walking over
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk walkers
    walkers.private rules.area027 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: replying? ( -- flag )
    window-up?  window:stage @ window:finished =  window:stage @ window:page-full = or  window:stage @ window:asking = or  and ;
: reply ( -- )  replying? pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  20000 0 do  reply  started? 0= if  leave  then  loop ;
: in-town ( -- )  events-stop  the-town 0 s" areas" actor-named send enter-area  deliver  2 steps ;
: x-of ( slot -- x )  person-now 2drop drop ;
: until-dest ( d -- )  5000 0 do  1 steps  the-stalker walker-of ?dup if  dest @ over = if  leave  then  then  loop  drop ;
prologue-done flag-set

testing started: at its node, walking its first edge
T{ in-town  the-stalker 0 3 100 5 s" walkers" actor-named send walker-start  deliver
   the-stalker walker-active?  the-stalker x-of  the-stalker walker-of from @ -> true 305900 0 }T
T{ the-stalker x-of  20 steps  the-stalker x-of = -> false }T
testing stopped: no longer walking
T{ the-stalker s" walkers" actor-named send walker-stop  deliver  the-stalker walker-active? -> false }T

testing minigame 23: to the town, then round his loop
T{ events-stop  0 stalker-due !  ' stalker-arrives start-event  played-out  2 steps
   area @  stalking @ 0<>  the-stalker walker-active?  the-stalker walker-of dest @ -> the-town true true 3 }T
testing a leg done: the next
T{ 9 until-dest  the-stalker walker-of dest @ -> 9 }T
testing talked to: caught - his scene, no longer walking, the flag
T{ stalker-caught flag-clear  the-stalker talks-to  deliver  started?  the-stalker walker-active?  stalker-caught flag? -> true true false true }T
T{ played-out  stalking @ -> 0 }T

test-summary
