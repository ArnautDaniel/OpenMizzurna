\ the townsfolk's schedules: where the clock puts them (docs/subsystems/townsfolk.md)
USING: tester engine actors messages clock townsfolk events.schedules ;

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

test-summary
