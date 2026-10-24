\ area 10's rules on the game's data (scripts/rules/area010.fs, crate.fs): the crate - ○ takes hold and he pushes it
\ as he walks, ○ lets go; □ climbs it (his perch: on a crate) and □ climbs down; the window's peep or its scene
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk crate
    rules.area010 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  3000 0 do  reply  started? 0= if  leave  then  loop ;
: press ( key -- )  dup true key-hold  2 steps  false key-hold  2 steps ;
: at-the-cabin ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps ;
: before-the-crate ( -- )   \ 200 short of where it is now (its place before it's placed), facing it (+z)
    the-crate-slot here? if  the-crate-slot person-now drop nip  else  $72A92 $72171  then {: x z :}
    x 0 z 200 - s" matthew" actor-named send figure-place  0 0 0 s" matthew" actor-named send figure-turn
    deliver  2 steps ;
: where-the-crate-is ( -- z )  the-crate-slot person-now drop nip nip ;
prologue-done flag-set

testing the crate in its place; ○ before it takes hold
T{ at-the-cabin  events-stop  before-the-crate  4 steps  the-crate-slot here?  where-the-crate-is -> true $72171 }T
T{ before-the-crate  pad-circle press  200 steps  the-crate holding?  matthew-pushing? -> true true }T
testing walking pushes it ahead of him; ○ lets go
T{ pad-up true key-hold  20 steps  pad-up false key-hold  where-the-crate-is $72171 > -> true }T
T{ pad-circle press  300 steps  the-crate holding?  matthew-pushing?  matthew-perch -> false false 0 }T
testing □ climbs it: on top (his perch 1, his feet still); □ climbs down
T{ before-the-crate  pad-square press  300 steps  the-crate on-the-crate?  matthew-perch  matthew-on-foot? -> true 1 false }T
testing on top: □'s prompt to climb down (src: crate_area_update's 0x11)
T{ crate-busy @ area @ =  crate-act-prompt @ -> true $11 }T
T{ pad-square press  300 steps  the-crate on-the-crate?  matthew-perch  matthew-on-foot? -> false 0 true }T

testing the window: before its time, a peep; once known, the scene inside (a minigame that's a script), once
T{ inside-begun flag-clear  cabin-known flag-clear  the-cabin-window  started?  played-out  inside-begun flag? -> true false }T
T{ cabin-known flag-set  the-cabin-window  started?  inside-begun flag? -> true true }T

test-summary
