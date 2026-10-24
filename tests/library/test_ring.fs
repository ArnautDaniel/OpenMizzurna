\ the item ring and showing items on the game's data (scripts/ring.fs, scripts/showing.fs): △ opens it with Matthew
\ the player's, the world standing still; left and right turn it; the waist bag opens the key items, ✕ goes back
\ and closes; ○ on the diary reads it. Shown an item, a character answers by the items they react to, their
\ reaction marked seen; a few answer their own way
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags items ui window floors
    showing ring ring.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: give ( item -- )  s" items" actor-named send give-item  deliver ;
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
: turn-key ( key -- )  dup true key-hold  4 steps  false key-hold  20 steps ;   \ (held a game frame: 15 a second here)
prologue-done flag-set
25 10 0 at-time
0 into  events-stop  true broadcast player-control  deliver
3 0 0 1 zone-centre 0 swap s" matthew" actor-named send figure-place  deliver   \ (in the station, by its cabinet)
9 give  14 give  7 give  13 give   \ the lighter, the diary; the tape, the badge

testing △ opens it: the tools (the phone, the lighter, the diary, the waist bag) and two key items; the world stands
T{ pad-triangle tap  ring-up?  ui-busy?  #tools @  #keys @  sel @ -> true true 4 2 0 }T
T{ clock-time  200 steps  clock-time = -> true }T
testing right turns to the next, in four game frames; left back, and round to the last
T{ pad-right turn-key  sel @  turn @  name-msg @ -> 1 0 6 }T
T{ pad-left turn-key  pad-left turn-key  sel @ -> 3 }T
testing ○ on the waist bag: the key items, from their first; ✕ back to the tools
T{ pad-circle tap  key-list? @  sel @  item-now -> true 0 7 }T
T{ pad-cross tap  key-list? @  ring-up? -> false true }T
testing ✕ closes it: the world goes on
T{ pad-cross tap  ring-up?  ui-busy? -> false false }T
testing ○ on the diary reads it (an event)
T{ pad-triangle tap  pad-right turn-key  pad-right turn-key  item-now  pad-circle tap  ring-up?  events-running @ 0> -> 14 false true }T
T{ events-stop  true broadcast player-control  deliver -> }T

testing a character's answer: the items they react to (the item + 1), not others; the photo Morgan's own way
T{ morgan 7 answer-to  morgan 6 answer-to  morgan 12 answer-to -> 8 0 $65 }T
testing shown: their reaction plays, marked seen (the session's word for them)
T{ morgan 8 show-to  deliver  events-running @ 0>  morgan seen-word session@ 8 rshift 1 and -> true 1 }T
T{ events-stop  true broadcast player-control  deliver -> }T
testing Samuel takes the pills only at the hospital; the deputy knows the badge; Winona, Emma's key
T{ samuel 18 answer-to  deputy 13 answer-to  winona 5 answer-to -> 0 1 $64 }T

test-summary
