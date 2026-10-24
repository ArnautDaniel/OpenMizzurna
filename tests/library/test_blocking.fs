\ what stops Matthew and his car, on the game's data (events.fs's in-the-way?, matthew.fs, driving.fs): a car or
\ prop of actors 26 - 60 on the roads there (within his radius and theirs, theirs halved to their sides); the police cordon's
\ two road tiles on foot but in their zone 0 until the 28th 03:00; the car's road blocks by tile and zone
USING: tester engine actors messages clock areas floors matthew matthew.private townsfolk traffic events story-flags
    driving driving.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
prologue-done flag-set
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: into ( area scene -- )  events-stop  s" areas" actor-named send enter-area  deliver  2 steps  events-stop  deliver ;
30 constant someone   300000 constant spot-x   300000 constant spot-z
: put ( x z -- )  {: x z :}  someone x 0 z 0 s" traffic" actor-named send car-place  deliver ;
: reach ( -- r )  matthew radius-of  someone radius-of + ;

testing a car standing there is in his way; well away, not
T{ 25 12 0 at-time  the-town 0 into  spot-x spot-z put  spot-x spot-z in-the-way?  spot-x 5000 + spot-z in-the-way? -> true false }T
testing ahead of them (their facing) at his radius and theirs less a little: in the way; to their side, not
T{ spot-x  spot-z reach + 2 -  in-the-way?  spot-x reach + 2 -  spot-z  in-the-way? -> true false }T
testing indoors, cars aren't
T{ 1 0 into  spot-x spot-z in-the-way? -> false }T

testing the cordon's tiles: 922 and 1022 (column + row x 100)
22 constant cordon-col   9 constant cordon-row
: tile-mid ( col row -- x z )  tile-size * tile-size 2/ +  tile-size +  swap tile-size * tile-size 2/ +  swap ;
T{ cordon-col cordon-row tile-mid tile-key  cordon-col cordon-row 1+ tile-mid tile-key -> 922 1022 }T
testing on foot there, outside zone 0, until the 28th 03:00 (in the town)
T{ 26 12 0 at-time  the-town 0 into  cordon-col cordon-row tile-mid  2dup 1 zone-at 0<>  -rot cordoned? = -> true }T
T{ 28 4 0 at-time  cordon-col cordon-row tile-mid cordoned? -> false }T
testing elsewhere in the town, not
T{ 26 12 0 at-time  17 13 tile-mid cordoned? -> false }T
testing the car's road blocks: a front's tile blocks in its zone 0, elsewhere not
T{ 3 2 tile-mid tile-key  17 13 tile-mid road-blocked? -> 203 false }T
T{ 14 3 tile-mid road-blocked?  14 3 tile-mid car-zone 0= = -> true }T
T{ 1 0 into -> }T

test-summary
