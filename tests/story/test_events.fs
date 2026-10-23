\ the events' language and the converted scripts (docs/subsystems/story.md)
USING: tester engine actors messages clock events events.ovl001 ;

testing frames are an event's: 15 a second
T{ 15 frames -> 1e seconds }T
T{ 20 frames -> 40 }T

testing goto is a tail jump: a long chain doesn't grow the return stack
variable hops
defer hop
: hop-on ( -- )  1 hops +!  hops @ 100000 < if  goto hop  then ;
' hop-on is hop
T{ 0 hops !  hop  hops @ -> 100000 }T

\ watching the clock: whether Matthew is held (the clock's no-control reason)
: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;   \ (as the engine steps: tasks, then the actors)
: running ( -- n )  events-running @ ;

testing an event runs as a task, holding Matthew until it ends
: little-event ( -- )  matthew 7 as-this-one drop  30 frames wait  42 result!  end-script  99 result! ;
T{ ' little-event start-event  running -> 1 }T
T{ 30 steps  running -> 1 }T
T{ 40 steps  running -> 0 }T

testing the new-game opening runs to its end: its waits are 285 frames (19 s); the ops not built yet pass
T{ 0 unbuilt-ops !  ' new-game-opening start-event  running -> 1 }T
T{ 19e seconds 5 - steps  running -> 1 }T
T{ 10 steps  running  unbuilt-ops @ 0> -> 0 true }T

test-summary
