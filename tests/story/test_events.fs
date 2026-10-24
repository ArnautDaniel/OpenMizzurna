\ the events' language and the converted scripts (docs/subsystems/story.md)
USING: tester engine actors messages keys clock window events events.ovl001 ;

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

\ a player who presses ○ whenever a line has finished
: answer ( -- )  window-up? stage @ finished = and if  pad-circle true key-hold  then  1 steps  pad-circle false key-hold ;
: play ( n -- )  0 ?do  answer  loop ;

testing the new-game opening waits for the player's ○ on its lines, then runs to its end, nothing in it unbuilt
T{ 0 unbuilt-ops !  ' new-game-opening start-event  60e seconds steps  running  window-up? -> 1 true }T
T{ 120e seconds play  running  window-up?  unbuilt-ops @ -> 0 false 0 }T   \ (every op it uses built)

test-summary
