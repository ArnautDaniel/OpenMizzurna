\ the story actor: a new game, the prologue's end (docs/subsystems/story.md)
USING: tester engine actors messages keys clock story-flags window events story ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
variable last-time   variable minutes   variable over
behaviour story-listener
  on spawned ( -- )  self subscribe time-set  self subscribe minute-passed  self subscribe prologue-over ;
  on time-set ( t -- )  last-time ! ;
  on minute-passed ( t -- )  last-time !  1 minutes +! ;
  on prologue-over ( -- )  true over ! ;
end-behaviour
state: no-state end-state
story-listener no-state s" story-test" spawn drop  deliver

testing the flags: bits of the block, by name
T{ story-clear  took-lighter flag? -> false }T
T{ took-lighter flag-set  took-lighter flag?  $008 story@ -> true 1 }T
T{ took-lighter flag-toggle  took-lighter flag? -> false }T
T{ 3 got-barbaras-cigarettes story+!  got-barbaras-cigarettes story@ -> 3 }T

testing a new game: 12/25 09:00, the flags clear, the opening playing, the clock held
T{ prologue-done flag-set  s" story" actor-named send new-game  deliver
   last-time @  prologue-done flag?  events-running @ -> new-game-time false 1 }T
: answer ( -- )  window-up? stage @ finished = and if  pad-circle true key-hold  then  1 steps  pad-circle false key-hold ;
: play ( n -- )  0 ?do  answer  loop ;
T{ 0 minutes !  120e seconds play  events-running @  minutes @ -> 0 0 }T

testing the prologue's end (src: the script $80096598): 12/25 10:00, the flag set, the clock running
T{ ' events.main:exe-80096598 start-event  10 steps
   last-time @  prologue-done flag?  over @ -> 25 10 0 date>time true true }T
T{ 0 minutes !  5e seconds steps  minutes @ -> 1 }T

test-summary
