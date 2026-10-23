\ rows.fs - the words the schedules are written in (scripts/events/schedules.fs): kept apart so their names
\ (until, stop, place, route ...) don't hide the language's elsewhere.
IN: townsfolk.rows
USING: clock townsfolk ;

: schedule ( slot -- )  person-of dup writing !  here swap first-stop !  0 writing @ #stops ! ;
: stop ( i -- )   \ (rows are written in order: i is the next)
    drop  here stop-row allot  dup stop-row erase  writing-row !  1 writing @ #stops +! ;
: row ( -- addr )  writing-row @ ;
: until ( day hour minute -- )  date>time row stop-until ! ;
: record ( k -- )  row stop-record ! ;
: in-area ( area floor -- )  row stop-floor !  row stop-area ! ;
: place ( x z -- )  row stop-z !  row stop-x ! ;
: facing ( heading -- )  row stop-heading ! ;
: pose ( motion -- )  row stop-pose ! ;
: with-partner ( -- )  partnered row stop-placed ! ;
: diner-seat ( -- )  at-diner row stop-placed ! ;
: club-seat ( -- )  at-club row stop-placed ! ;
: variant ( n -- )  row stop-variant ! ;
: route ( address -- )  row stop-route ! ;
: end-stop ( -- ) ;
: end-schedule ( -- ) ;

