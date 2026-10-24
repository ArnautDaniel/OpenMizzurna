\ the notebook on the game's data (scripts/notebook.fs): chosen in the ring it opens over the field, the world
\ standing still; ○ on the characters shows the people he knows, one at a time; on the phone numbers, those he
\ knows; ✕ back, and closed. A talk makes a person known, a number learnt is known. And the speakers' portraits
\ (window.fs): a kind on the area's sheet has one
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags ui window talk
    notebook notebook.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
prologue-done flag-set
0 into  events-stop  true broadcast player-control  deliver

testing chosen in the ring: the menu, the world standing still
T{ 1 -1 broadcast ring-chosen  deliver  notebook-up?  ui-busy?  page @  row @ -> true true on-the-menu 0 }T
testing ○ on the characters: the eight known at a new game, the first shown; down the next, up back
T{ pad-circle tap  page @  #known @  at @ -> people-page 8 0 }T
T{ pad-down tap  at @  pad-up tap  at @ -> 1 0 }T
testing ✕ to the menu; down, ○: the five numbers known at a new game
T{ pad-cross tap  page @  pad-down tap  row @  pad-circle tap  page @  #known @ -> on-the-menu 1 numbers-page 5 }T
testing ✕ twice: closed, the world going on
T{ pad-cross tap  pad-cross tap  notebook-up?  ui-busy? -> false false }T
testing learnt: a number, a person (talking to Samuel: his page)
T{ 6 number-known?  6 s" notebook" actor-named send learn-number  deliver  6 number-known? -> false true }T
T{ 6 bio-known?  samuel met  deliver  6 bio-known? -> false true }T

testing the portraits: in the station Morgan's, not Samuel's (his kind isn't on its sheet); at the hospital his
T{ 1 on-sheet?  5 on-sheet?  2 area !  5 on-sheet?  0 area ! -> true false true }T

test-summary
