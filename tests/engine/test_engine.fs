\ the engine's own words, headless (openmizzurna --test)
USING: tester engine keys actors ;

testing keys held by scripts
T{ pad-circle key-down? -> false }T
T{ pad-circle true key-hold  pad-circle key-down? -> true }T
T{ pad-circle false key-hold  pad-circle key-down? -> false }T

testing the screen and the pen
T{ screen-size  0> swap 0> and -> true }T
T{ 2 pen-scale  char-size  0> swap 0> and -> true }T

testing an actor's frame
variable ticked
behaviour counting
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  1 ticked +! ;
end-behaviour
state: no-state end-state
T{ counting no-state s" counter" spawn  drop  deliver  actors-frame actors-frame  ticked @ -> 2 }T

testing real time
T{ 1e seconds -> 30 }T
T{ 0.5e seconds -> 15 }T
T{ 0.001e seconds -> 1 }T
T{ 0e seconds -> 0 }T
T{ dt 30e f* 1e f- fabs 1e-9 f< -> true }T

test-summary
