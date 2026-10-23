\ rows.fs - the words the doors are written in (scripts/events/doors.fs): kept apart so their names (door,
\ room, place ...) don't hide the language's elsewhere.
IN: doors.rows
USING: doors ;

variable writing   variable writing-side
: door ( n -- )
    door-of dup writing !  dup door-row erase
    no-room over near-side side-room !  no-room swap far-side side-room !
    0 writing-side ! ;
: in-area ( area -- )  writing @ door-area ! ;
: room ( room -- )   \ the next side
    writing @  writing-side @ if  far-side  else  near-side  then  dup writing-side !  side-room ! ;
: side ( -- addr )  writing-side @ ;
: spot ( n -- )  side side-spot ! ;
: place ( x z -- )  side side-z !  side side-x ! ;
: facing ( heading -- )  side side-heading ! ;
: way ( 0|1 -- )  side side-way ! ;
: end-door ( -- ) ;
