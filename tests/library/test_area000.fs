\ area 0's rules on the game's data (scripts/rules/area000.fs): the police station - what ○ and □ find there, the
\ desk officer's first talk, a scene on arriving, the station closing at 22:00
USING: tester keys engine actors messages clock areas floors matthew events story-flags talk look rules.area000 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: in-station ( scene -- )  events-stop  this-area swap s" areas" actor-named send enter-area  deliver  2 steps ;
: started? ( -- flag )  events-running @ 0<> ;
: placed ( -- )   \ in the middle of the cabinet's zone (not behind the counter: that sends him back)
    3 0 0 1 zone-centre {: x z :}  x 0 z s" matthew" actor-named send figure-place  deliver ;

testing ○ on floor 1 by the zone: the board (0), the cabinet (3); by the check: the window, the files once seen
T{ 25 10 0 at-time  0 in-station  events-stop  placed  1 floor ! -> }T
T{ 0 $FF look-check  3 $FF look-check  4 $FF look-check -> the-board the-cabinet nothing-here }T
T{ 0 $0E look-check  0 $09 look-check -> out-the-window nothing-here }T
T{ 1 computer-seen !  0 $09 look-check  0 computer-seen ! -> the-files }T
T{ 2 floor !  0 $FF look-check  1 floor ! -> nothing-here }T

testing □: the board once seen, the cabinet looked at twice, the computer until it's been used
T{ 0 $FF act-check  1 board-seen !  0 $FF act-check -> do-nothing study-the-board }T
T{ 3 $FF act-check  2 cabinet-looks !  3 $FF act-check -> do-nothing open-the-cabinet }T
T{ 0 9 act-check -> the-computer }T

testing the desk officer: his first talk is a scene, and marks it
T{ events-stop  0 $080 story!  desk-officer talk-with  deliver  started?  $080 story@ -> true 1 }T

testing arriving with the wolf's word set: its scene, once
T{ events-stop  $044 0 story-bit flag-set  station-wolf-seen flag-clear  0 in-station
   started?  station-wolf-seen flag? -> true true }T
T{ events-stop  0 in-station  started? -> false }T

testing walking behind the front desk (zone 0): stopped short of it, and told so
: desk ( -- x z )  0 0 0 1 zone-centre ;
: outside? ( x z -- flag )  2dup 1 blocked? 0=  -rot 1 zone-hit drop 0<> and ;   \ open floor, not the desk's zone
: open-step ( dx dz -- x z true | false )   \ the first open spot out from the desk that way, 100 at a time
    {: dx dz :}  20 1 do  desk dz i * + swap dx i * + swap  2dup outside? if  true unloop exit  then  2drop  loop
    false ;
: start-spot ( -- x z heading )   \ out to the side the floor is open, and facing back toward the desk
    100 0 open-step if  1024 3 *  exit  then   -100 0 open-step if  1024  exit  then
    0 100 open-step if  2048  exit  then        0 -100 open-step if  0  exit  then   0 0 0 ;
: walk-in ( -- )   \ up held until a scene starts (at most 300 steps)
    pad-up true key-hold  300 0 do  1 steps  started? if  leave  then  loop  pad-up false key-hold ;
: beside-the-desk ( -- )
    start-spot {: x z h :}  x 0 z s" matthew" actor-named send figure-place  0 h 0 s" matthew" actor-named send figure-turn
    deliver ;
T{ events-stop  25 10 0 at-time  0 in-station  events-stop  beside-the-desk  walk-in  started?  at-zone drop counter-zone = -> true false }T

testing 22:00: the station closes, whichever room he is in
T{ 25 22 5 at-time  0 in-station  started?  closing-done @ -> true 1 }T

test-summary
