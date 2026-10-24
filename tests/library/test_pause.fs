\ Start on the game's data (scripts/pause.fs): after the prologue it pauses - the world held, the events' tasks
\ standing still - and goes on again; before the prologue's end, with no event running, it skips the rest of it
USING: tester engine actors messages clock areas keys matthew events story-flags ui pause ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
25 12 0 date>time s" clock" actor-named send set-time  deliver
prologue-done flag-set
1 into  events-stop  true broadcast player-control  deliver

testing Start: paused, the world held; an event's wait stands still
variable woke
: sleeper ( -- )  30 forth:wait  true woke ! ;
T{ false woke !  ' sleeper forth:spawn drop  pad-start tap  paused?  ui-busy?  60 steps  woke @ -> true true false }T
testing Start again: going on, the wait over in time
T{ pad-start tap  paused?  ui-busy?  60 steps  woke @ -> false false true }T
testing before the prologue's end, no event running: Start skips the rest (its script, the prologue then over)
T{ prologue-done flag-clear  events-stop  deliver  pad-start tap  events-running @ 0>  paused? -> true false }T
T{ 200 steps  prologue-done flag? -> true }T

test-summary
