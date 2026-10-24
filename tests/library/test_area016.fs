\ area 16's rules on the game's data (scripts/rules/area016.fs, crate.fs): the crate (○ takes hold, □ climbs), the
\ poster (first look, then its line), the key rack without a companion, the main door without the key, the
\ teacher's hours, closing time (once), the building's halves by where he is
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags items talk crate
    rules.area016 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  10000 0 do  reply  started? 0= if  leave  then  loop ;
: press ( key -- )  dup true key-hold  2 steps  false key-hold  2 steps ;
: in-the-library ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps  played-out ;
: put-him ( x z -- )  0 swap s" matthew" actor-named send figure-place  deliver ;
: before-the-crate ( -- )   \ 200 short of it (-z), facing it (+z)
    the-crate-slot person-now drop nip {: x z :}  x z 200 - put-him  0 0 0 s" matthew" actor-named send figure-turn
    deliver  2 steps ;
prologue-done flag-set

testing on arriving by day (no teacher yet): the crate in its place
T{ 26 12 0 at-time  in-the-library  started?  the-crate-slot here?  teacher-here @ -> false true false }T
testing ○ before the crate takes hold, ○ lets go; □ climbs up, □ down
T{ before-the-crate  pad-circle press  200 steps  the-crate holding? -> true }T
T{ pad-circle press  300 steps  the-crate holding? -> false }T
T{ before-the-crate  pad-square press  300 steps  the-crate on-the-crate?  matthew-perch -> true 1 }T
T{ pad-square press  300 steps  the-crate on-the-crate?  matthew-perch -> false 0 }T

testing the poster: its first look, then its line; the key rack alone: its line, the key not takeable
T{ 0 11 look-check  dup the-poster =  swap look-do  started?  poster-seen @ 0<> -> true true true }T
T{ played-out  0 0 look-check look-do  started?  key-takeable @ -> true false }T
T{ played-out  0 0 act-check -> nothing-to-do }T
testing the main door without the key
T{ main-door  started?  played-out -> true }T

testing the teacher: in at 14:04, out at 18:01
T{ 26 14 4 at-time  2 steps  teacher-here @ 0<> -> true }T
T{ played-out  26 18 1 at-time  2 steps  teacher-here @ 0<> -> false }T
testing closing time: shown out, once
T{ played-out  26 18 30 at-time  2 steps  closing-done @ 0<> -> true }T
T{ played-out  26 18 31 at-time  2 steps  started? -> false }T

testing the halves: far (past the middle) shows object 2, near object 1
: halves-shown ( -- near far )  models-shown 1+ c@  models-shown 2 + c@ ;
: at-z ( z -- )  matthew-now drop nip drop swap put-him  2 steps ;
T{ 26 12 0 at-time  in-the-library  $23668 100 + at-z  halves-shown -> 0 1 }T
T{ $23668 100 - at-z  halves-shown -> 1 0 }T

testing the script's question at $8010317C: whether Liz is at her first place, an answer of 0 or 1 (src: func_8002D620)
: asked ( -- n )  this-area ovl $8010317C find-native execute ;
T{ asked  dup 0=  swap 1 = or -> true }T
T{ asked  lidds lidds-first-place at-record? 1 and = -> true }T

test-summary
