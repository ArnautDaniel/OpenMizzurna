\ the rowing boat on the game's data (scripts/boat.fs): begun (the lake bar's script's native) he sits in it facing
\ backwards, the pad the boat's; ✕ pulls it ahead, L1 turns it; by a boathouse ○'s prompt shows; ended, he's
\ on foot again
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags ui boat boat.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
prologue-done flag-set
25 12 0 date>time s" clock" actor-named send set-time  deliver
$524DF constant lake-x   $6C294 constant lake-z   \ (by the west boathouse)
: to-the-lake ( -- )  27 -1 lake-x lake-z 0 0 warp ;
' to-the-lake start-event  60 steps  events-stop  true broadcast player-control  deliver
the-boat lake-x 0 lake-z s" townsfolk" actor-named send extra-place  deliver

testing begun: rowing, the field without the pad, the boat where it was put
T{ boat-begin  deliver  rowing?  field-pad?  bx @  bz @ -> true false lake-x lake-z }T
testing by the west boathouse: ○'s prompt
T{ 2 steps  boat-prompt @ -> landing-prompt }T
testing ✕ pulls it ahead (if the water lets it), his place the boat's; L1 turns it
T{ bh @  pad-l1 true key-hold  10 steps  pad-l1 false key-hold  bh @ <> -> true }T
T{ pad-cross true key-hold  20 steps  pad-cross false key-hold  velocity f@ f0>  matthew-now drop rot drop nip  bz @ = -> true true }T
testing ended: on foot, the pad the field's again
T{ boat-end  deliver  rowing?  field-pad?  boat-prompt @ -> false true -1 }T

test-summary
