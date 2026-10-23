\ the townsfolk's schedules: where the clock puts them (docs/subsystems/townsfolk.md)
USING: tester engine actors messages clock townsfolk talk events.schedules ;

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

testing a line by stop: a table of pairs ended by -1 (src: func_800429C8)
create lines  0 , 168 ,  2 , 169 ,  9 , 171 ,  -1 ,
T{ lines 0 a-line-by-stop  lines 9 a-line-by-stop  lines 5 a-line-by-stop -> 168 171 -1 }T

test-summary
