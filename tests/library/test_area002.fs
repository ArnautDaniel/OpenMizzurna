\ area 2's rules on the game's data (scripts/rules/area002.fs): Gruber Hospital - kept inside before the prologue
\ ends, out to the town after; closing time; knocking at Barbara's door and her cigarettes; the attack
USING: tester engine actors messages clock areas floors keys matthew townsfolk items events window story-flags talk
    rules.area002 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: in-hospital ( scene -- )  events-stop  this-area swap s" areas" actor-named send enter-area  deliver  2 steps ;
: at-zone-of ( index -- )   \ standing in the middle of that zone of floor 1
    0 0 1 zone-centre {: x z :}  x 0 z s" matthew" actor-named send figure-place  deliver ;
: answer ( -- )
    window-up? stage @ finished = stage @ page-full = or and pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  2000 0 do  answer  started? 0= if  leave  then  loop ;
4 at-zone-of   \ (Matthew placed: he has a body)

testing before the prologue ends he can't walk out: at the way out (zone 11) he stays in the hospital
T{ prologue-done flag-clear  25 10 0 at-time  0 in-hospital  events-stop  4 at-zone-of  5 steps
   the-exit at-zone-of  5 steps  started?  area @ -> false this-area }T
testing after it the way out takes him to the town, before the hospital
T{ prologue-done flag-set  0 in-hospital  events-stop  4 at-zone-of  5 steps  the-exit at-zone-of  5 steps
   played-out  20 steps  area @  matthew-now drop nip -> the-town $19942 $56C66 }T

testing closing time at 21:55 - 22:59: shown out, once a visit
T{ 26 22 0 at-time  0 in-hospital  started?  closing-done @ -> true 1 }T

testing □ at Barbara's door knocks (and ○ there then says who's in); with her cigarettes on the 28th she takes them
T{ events-stop  26 12 0 at-time  0 in-hospital  events-stop  0 $49 act-check -> knock-barbara }T
T{ knock-barbara act-do  started?  knocked-barbara @  0 $49 look-check -> true 1 barbaras-door }T
T{ events-stop  0 in-hospital  events-stop  barbara-invited flag-set  gave-barbara-cigarettes flag-clear
   cigarettes s" items" actor-named send give-item  deliver  28 16 0 at-time
   knock-barbara act-do  deliver  started?  gave-barbara-cigarettes flag?  cigarettes has-item? -> true true false }T

testing the attack: ready, he comes in - its scene; Isabella and Winona away, Wolf, James, Samuel to their places
T{ events-stop  26 12 0 at-time  hospital-attack-scene-seen flag-clear  $020 story@ $1000 or $020 story!
   0 in-hospital  started?  attacked?  isabella person-of travelling @  wolf stop-of -> true true true $3E }T

test-summary
