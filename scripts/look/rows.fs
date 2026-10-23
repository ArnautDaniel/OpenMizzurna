\ rows.fs - the words the look and act tables are written in (scripts/events/checks.fs): kept apart so their
\ names (row, zone, script, prompt ...) don't hide others elsewhere.
IN: look.rows
USING: events look ;

: last-of ( addr -- addr' )   \ the link to append at
    begin  dup @ while  @  repeat ;
: row ( area table -- )   \ a new row at the end of that area's table; the words below fill it
    here check-row allot {: a t r :}  r check-row erase  r building !  r  a t table-list last-of ! ;
: zone ( n -- )  building @ row-zone ! ;
: facing ( turn -- )  building @ row-facing ! ;   \ (4096 a turn)
: either-side ( degrees -- )  building @ row-within ! ;   \ (how far the heading may be off)
: states ( bits -- )  building @ row-states ! ;
: ask-area ( -- )  ask-the-area building @ row-kind ! ;
: script ( "word" -- )  ' building @ row-target !  run-script building @ row-kind ! ;
: native-at ( scope address -- )  building @ row-target !  building @ row-scope !  run-native building @ row-kind ! ;
: hint ( n -- )  building @ row-prompt ! ;
: end-row ( -- ) ;

