\ the townsfolk's schedules: where the clock puts them (docs/subsystems/townsfolk.md)
USING: tester engine actors messages clock townsfolk talk doors events.schedules events.doors ;
2 constant nozz   \ (the slot)

testing the converted schedules: Nozz (slot 2) has rows, his first at his counter in area 1
T{ nozz scheduled?  nozz person-of #stops @ 0>  nozz person-of 0 row-of stop-area @ -> true true 1 }T
T{ nozz person-of 0 row-of stop-record @ -> behind-the-counter }T

testing a person's stop is the first whose time hasn't come
: nozz-at ( day hour minute -- stop )  date>time  nozz person-of swap stop-for ;
T{ 25 9 0 nozz-at -> 0 }T
T{ 25 11 39 nozz-at -> 0 }T
T{ 25 11 40 nozz-at -> 1 }T
T{ 31 23 59 nozz-at  nozz person-of #stops @ 1- = -> true }T

testing the clock moves them: at 12/25 12:00 Nozz has left his counter
T{ 25 12 0 date>time s" clock" actor-named send set-time  deliver  deliver  nozz stop-of  nozz-behind-counter? -> 1 false }T

testing the minute's rules (while Matthew is the player's): at his stop's minute he departs - travelling, no
testing stop - and still on his way at the next row's minute, he is put at that stop (src: sched_depart, sched_skip)
: the-townsfolk ( -- id )  s" townsfolk" actor-named ;
: minute-at ( day hour minute -- )  date>time the-townsfolk send minute-passed  deliver  deliver ;
T{ 25 11 39 date>time s" clock" actor-named send set-time  deliver  deliver  nozz stop-of -> 0 }T
T{ true the-townsfolk send player-control  deliver  25 11 39 minute-at  nozz stop-of -> 0 }T
T{ 25 11 40 minute-at  nozz stop-of  nozz walking? -> -1 false }T
T{ 25 13 41 minute-at  nozz stop-of -> -1 }T
T{ 25 13 42 minute-at  nozz stop-of  nozz person-of person-area @ -> 1 24 }T

testing a door: Nozz's shop's back door (15) into its room 3, a step from the door, facing out; to the town
: way-fields ( way -- area x z heading way )  {: w :}  w out-area @  w out-x @  w out-z @  w out-heading @  w out-way @ ;
T{ 15 3 false door-way way-fields -> 1 345042 285254 2560 1 }T
T{ 15 town-room false door-way out-area @ -> the-town }T
T{ 15 7 false door-way -> 0 }T

testing going through it: a person comes out there, stepping out
T{ nozz 15 3 false -1 the-townsfolk send person-through-door  deliver  nozz person-now
   -> 345042 0 285254 2560 }T

testing their cars in the rows: at his counter Nozz's car waits at car route 7's start; Morgan's stop 18 has him
testing riding along route 23 at 360 (docs/subsystems/traffic.md)
1 constant morgan
T{ nozz person-of 0 row-of stop-car @ -> 7 }T
: row-riding ( row -- placed route speed )  {: r :}  r stop-placed @  r stop-x @  r stop-z @ ;
T{ morgan person-of 18 row-of row-riding -> partnered 23 360 }T

testing going straight ahead (cmd $0A): along their heading at their speed, until idle
: nozz-x-z ( -- x z )  nozz person-now drop nip ;
T{ nozz 1000 2000 the-townsfolk send person-place  nozz 0 the-townsfolk send person-turn  deliver  nozz-x-z -> 1000 2000 }T
T{ nozz 30 -1 the-townsfolk send person-forward  deliver  broadcast tick  deliver  nozz-x-z  2000 > -> 1000 true }T
: stays? ( -- flag )  nozz-x-z  broadcast tick  deliver  nozz-x-z  rot = -rot = and ;
T{ nozz the-townsfolk send person-idle  deliver  stays? -> true }T

testing a line by stop: a table of pairs ended by -1 (src: func_800429C8)
create lines  0 , 168 ,  2 , 169 ,  9 , 171 ,  -1 ,
T{ lines 0 a-line-by-stop  lines 9 a-line-by-stop  lines 5 a-line-by-stop -> 168 171 -1 }T

test-summary
