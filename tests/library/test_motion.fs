\ a figure's motions chained as the original's (scripts/figures.fs; src: func_8002032C, func_8001FF34): the one
\ queued after it plays with the same looping; one that doesn't loop, with none queued, is done and plays its next
\ on and on, or holds its last frame; the one playing asked again goes on where it is
USING: tester engine areas figures ;

create p  playing allot
1 constant first-id   2 constant second-id   \ (the common motions: 033_4/1)
: advances ( n -- )  0 ?do  p advance drop  loop ;
: played-to-end ( -- )   \ until done, or round to the second motion
    10000 0 do  p advance drop  p done @  p anim @ second-id = or if  leave  then  loop ;

testing the common motions are there
T{ first-id motion-of 0<>  second-id motion-of 0<> -> true true }T

testing once, then the queued once, then done with its last frame held (cmd $06)
T{ first-id second-id false -1 p play-queued  p anim @  p done @ -> first-id false }T
T{ played-to-end  p anim @  p done @ -> second-id false }T   \ (the first played: the queued, not done)
: until-done ( -- )  10000 0 do  p advance drop  p done @ if  leave  then  loop ;
T{ until-done  p anim @  p done @  p frame  p clip @ motion-frames 1- = -> second-id true true }T

testing looping: the first once, then the queued round and round, never done (cmd $04 with its loop)
T{ first-id second-id true 0 p play-queued  played-to-end  p anim @  p done @ -> second-id false }T
T{ 500 advances  p anim @  p done @ -> second-id false }T

testing none queued, not looping: done, then the next on and on (cmd $03's standing, $13)
T{ first-id -1 false second-id p play-queued  played-to-end  p anim @  p done @  p looping @ -> second-id true true }T

testing the one playing asked again goes on where it is; once done, asked again it starts over
: asked-again-goes-on? ( -- flag )
    first-id -1 true -1 p play-queued  5 advances  p at f@ {: F: was :}
    first-id -1 true -1 p play-queued  p at f@ was f=  p at f@ 0e f> and ;
T{ asked-again-goes-on? -> true }T
T{ first-id -1 false -1 p play-queued  until-done  first-id -1 false -1 p play-queued  p frame  p done @ -> 0 false }T

test-summary
