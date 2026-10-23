\ area 9's rules on the game's data (scripts/rules/area009.fs): the lake - sitting at the hut's counter, waving
\ the counterman over, ordering a drink from the list and drinking it
USING: tester engine actors messages clock areas keys matthew townsfolk events window list-menu story-flags talk
    counter rules.area009 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: replying ( n -- )  0 ?do  reply  loop ;
: press ( key -- )  dup true key-hold  2 steps  false key-hold  2 steps ;
: held ( key n -- )  over true key-hold  steps  false key-hold ;
: until-the-menu ( -- )  200 0 do  reply  menu-up? if  leave  then  loop ;
: at-the-hut ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps ;
: before-stool ( seat -- )   \ across the counter from it, facing it
    at-lake swap seat-spot drop nip nip {: z :}  $51340 0 z s" matthew" actor-named send figure-place
    0 3072 0 s" matthew" actor-named send figure-turn  deliver  2 steps ;
prologue-done flag-set

testing □ before a free stool sits him down; □ waves the counterman over
T{ 26 12 0 at-time  at-the-hut  events-stop  1 before-stool  pad-square press  60 steps
   at-the-counter?  matthew seat-of -> true at-lake 1 }T
T{ pad-square 10 held  300 steps  barman-here -> 1 }T
testing □ orders: the list of two; ○ picks the first; served, □ drinks; one drink this visit
T{ pad-square 3 held  until-the-menu  menu-up? -> true }T
T{ pad-circle press  400 replying  pad-square 3 held  600 replying  drinks @  events-running @ -> 1 0 }T

test-summary
