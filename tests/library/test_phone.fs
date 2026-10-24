\ the phone on the game's data (scripts/phone.fs): chosen in the ring he takes it out and dials four face buttons;
\ the number rings out and whoever has it answers (from where he is, "right here"); a shoulder clears, or with
\ nothing keyed hangs up. A number answered by someone becomes known. The story's calls ring at their times;
\ chosen then, the call is answered
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags items ui window floors
    pictures ring phone phone.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: tap ( key -- )  dup true key-hold  4 steps  false key-hold  4 steps ;   \ (held a game frame)
: chosen ( -- )  0 -1 broadcast ring-chosen  deliver  1 steps ;
: free ( -- )  events-stop  true broadcast player-control  deliver ;
prologue-done flag-set
25 12 0 at-time
0 into  free
3 0 0 1 zone-centre 0 swap s" matthew" actor-named send figure-place  deliver   \ (in the station)

testing chosen: the dial up, the phone's picture, the world's pad taken; he takes it out, then keys
T{ chosen  phone-up?  step @  picture-up?  field-pad? -> true taking-out true false }T
T{ 60 steps  step @ -> keying }T
testing ○ □ □ ○: 40, the sheriff's department; it rings out, then the call: from the station, "right here"
T{ pad-circle tap  pad-square tap  pad-square tap  pad-circle tap  dialled  step @ -> 40 holding-up }T
T{ 200 steps  phone-up?  events-running @ 0>  field-pad? -> false true true }T
T{ free -> }T
testing a shoulder clears what's keyed; with nothing keyed, he hangs up and puts it away: no call
T{ chosen  60 steps  pad-cross tap  #digits @  pad-l1 tap  #digits @ -> 1 0 }T
T{ pad-l1 tap  step @  60 steps  phone-up?  events-running @ -> putting-away false 0 }T

testing the school's number by day on the 25th: their first answer, and the number known
T{ numbers-known @ school-known and  high-school call-number  events-running @ 0>  numbers-known @ school-known and 0<>
   $6B session-c@ -> 0 true true 1 }T
T{ free -> }T
testing a number no one has: the wrong number
T{ 1 call-number  events-running @ 0> -> true }T
T{ free -> }T

testing the 25th at 22:05 (not at the club): call 1 rings; chosen, it's answered - the day kept, on the line
T{ 25 22 5 at-time  4 steps  calling @  ringing? -> 1 true }T
T{ chosen  calling @  in-call?  call-1-day day-of-call -> on-the-line true 25 }T
testing the call over: he puts it away
T{ events-stop  deliver  10 steps  in-call?  calling @ -> false 0 }T
testing a time of day 0 is never reached (src: event_time_reached)
T{ 0 0 0 reached?  25 0 0 reached? -> false true }T

test-summary
