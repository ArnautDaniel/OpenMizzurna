\ area 18's rules on the game's data (scripts/rules/area018.fs): Ringwald's Gas Station - ○ at the counter talks
\ to Robert while he's behind it, his lines by his stops, closing time sending Matthew out to the town
USING: tester engine actors messages clock areas keys matthew townsfolk events window talk rules.area018 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
412000 constant in-the-shop-x   169400 constant in-the-shop-z
: in-station ( scene -- )
    events-stop  this-area swap s" areas" actor-named send enter-area  deliver
    in-the-shop-x 0 in-the-shop-z s" matthew" actor-named send figure-place  deliver  2 steps ;

testing ○ at the counter: Robert behind it (12/25 10:00, his stop 0) - talking to him; at 13:00 he's out
T{ 25 10 0 at-time  0 in-station  events-stop  the-counter 0 look-check  0 0 look-check -> talk-to-robert nothing-here }T
T{ 25 13 0 at-time  the-counter 0 look-check -> nothing-here }T
testing his lines by his stop: at stop 0 the noticeboard; at stop 1 (the church) none here - his own
T{ 25 10 0 at-time  events-stop  robert talks-here  started? -> true true }T
T{ events-stop  25 12 30 at-time  robert stop-of  robert talks-here -> 1 false }T

testing 22:00 - 22:59: the attendant sends him out, back to the town before the station
: answer ( -- )
    window-up? stage @ finished = stage @ page-full = or and pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  2000 0 do  answer  started? 0= if  leave  then  loop ;
T{ 26 22 10 at-time  0 in-station  started?  closing-done @ -> true 1 }T
T{ played-out  20 steps  area @  matthew-now drop nip -> the-town 411712 169863 }T
testing at 23:00 (Robert's "open 24 hours"): no one sends him out
T{ 26 23 0 at-time  0 in-station  started? -> false }T

test-summary
