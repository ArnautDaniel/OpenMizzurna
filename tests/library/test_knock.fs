\ knocking on the game's data (scripts/knock.fs, rules/area027.fs): who answers which door when; the resident
\ stepping out, waiting at the door, going back in as he walks off; "no answer" and its reason; the visit to
\ James's house his invitation leads to
USING: tester engine actors messages clock areas floors keys doors matthew townsfolk events window story-flags talk
    knock rules.area027 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  3000 0 do  reply  started? 0= if  leave  then  loop ;
: by-door ( door -- )   \ in the street at that door, facing it
    town-room false door-way {: w :}  w out-x @ 0 w out-z @ s" matthew" actor-named send figure-place
    0 w out-heading @ 2048 + 4095 and 0 s" matthew" actor-named send figure-turn  deliver ;
: in-town ( -- )  events-stop  the-town 0 s" areas" actor-named send enter-area  deliver  2 steps ;
prologue-done flag-set

testing who answers: Steve at home on the 26th at 15:00; nobody at 12:00 (the 26th, before 11:00: no one); too
testing early before 7:00 and late from 20:45
T{ 26 15 0 at-time  in-town  events-stop  knock-reset  $1B knock -> steve }T
T{ 26 10 0 at-time  events-stop  $1B knock  started?  no-answer @ -> 0 true $2711 }T
T{ events-stop  26 21 0 at-time  $1B knock  no-answer @ -> 0 $2716 }T
T{ events-stop  26 6 0 at-time  $1B knock  no-answer @ -> 0 $2715 }T

testing he answers: out at the door, greeting him, waiting there; walking off, he goes back in
T{ events-stop  26 15 0 at-time  in-town  events-stop  $1B by-door  2 steps  knock-reset
   steve -2 $1B answer  played-out  steve at-a-door?  knock-idle? -> true false }T
: walk-off ( -- )  matthew-now drop {: x y z :}  x 800 + 0 z s" matthew" actor-named send figure-place  deliver ;
T{ walk-off  5 steps  played-out  steve at-a-door?  knock-idle? -> false true }T

testing James's invitation: his door takes Matthew in to the visit, then out to the town again
T{ events-stop  26 5 0 at-time  in-town  events-stop  james-door-scene-seen flag-clear  $178 story@ 1 or $178 story!
   $1E knock  james-door-scene-seen flag?  visited-james-house story@ -> 0 true 1 }T
T{ played-out  area @  matthew-now drop nip -> the-town 390585 417928 }T

test-summary
