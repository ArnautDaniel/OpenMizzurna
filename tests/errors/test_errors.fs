\ the errors actor (docs/subsystems/errors.md): a failure written to the log and the actor that failed asked to
\ start over - a few times in a while, then left as it is; the window and the effects put themselves right
USING: tester engine actors messages errors effects window ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
message poke ( -- )
variable starts
behaviour fragile
  on poke ( -- )  0 @ drop ;   \ (an error: a null address)
  on start-over ( -- )  1 starts +! ;
end-behaviour
state: no-state end-state
fragile no-state s" fragile" spawn constant fr  deliver
: fail ( -- )  fr send poke  deliver  deliver ;   \ (the failure, then the errors actor's start-over)

testing a failure: the actor that failed is asked to start over
T{ fail  starts @  fr errors-start-overs -> 1 1 }T
testing failing again in a while: three start-overs, then it is left as it is
T{ fail fail fail  starts @ -> 3 }T
testing a while later it is asked again
T{ start-over-window steps  fail  starts @ -> 4 }T

testing the window starts over shut, the clock let go
T{ true window-up !  s" window" actor-named send start-over  deliver  window-up? -> false }T
testing the effects start over with none running (nothing waits for one)
T{ fade kind !  10 frames-left !  effect-running?  s" effects" actor-named send start-over  deliver  effect-running? -> true false }T

test-summary
