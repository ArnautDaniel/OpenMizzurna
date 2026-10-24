\ the last of the events' ops on the game's data (scripts/events.fs): the prompt (□'s and ○'s icons beside the
\ box until one is pressed: ○ 1, □ 0); the hospital's elevator doors (four of its objects' motions, in its
\ content areas only); a talk's gestures (a motion and what follows; none sitting but the seated ones; Barbara
\ her own); a labourer off along their stop's route
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
prologue-done flag-set
25 12 0 date>time s" clock" actor-named send set-time  deliver

testing the prompt: shown, the event waiting; ○ answers 1 and it's gone
variable answer
: asks ( -- )  13 8 prompt  thread thread-result slot @ answer ! ;
T{ -1 answer !  ' asks start-event  2 steps  prompt-left @  prompt-right @  prompt-shown? -> 13 8 true }T
T{ pad-circle tap  2 steps  answer @  prompt-shown? -> 1 false }T
testing □ answers 0
T{ -1 answer !  ' asks start-event  2 steps  pad-square tap  2 steps  answer @ -> 0 }T

testing a gesture: Matthew's raised hand - its motion, then the next looping
T{ 1 into  events-stop  deliver  matthew raised-hand gesture  deliver  matthew-anim-now -> 83 84 }T
testing a plain one: once, its last frame held; none at all sitting (but the seated ones)
T{ matthew 3 gesture  deliver  matthew-anim-now -> 86 -1 }T
T{ 2 s" matthew" actor-named send figure-perch  deliver  matthew 3 gesture  deliver  matthew-anim-now
   0 s" matthew" actor-named send figure-perch  deliver -> 86 -1 }T

testing the elevator doors: nothing outside the hospital
T{ 1 1 elevator-doors  unbuilt-ops @ -> unbuilt-ops @ }T

test-summary
