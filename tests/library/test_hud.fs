\ the HUD's facts on the game's data (scripts/look.fs's prompts, the ring's, hud.fs): what □ and ○ would do, an
\ icon each - a thing's answer first (talked to, examined, taken), else the row's hint, else the area's check
\ through its table; nothing with no control
USING: tester engine actors messages clock areas keys floors matthew townsfolk events story-flags items ui look
    ring ring.private hud ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
prologue-done flag-set
25 12 0 date>time s" clock" actor-named send set-time  deliver   \ (no scene in the station then)

testing the answers' icons (src: D_80096128): talking 7, taking 6, examining 3
T{ answer-talk answer-prompt  answer-take answer-prompt  answer-examine answer-prompt -> 7 6 3 }T
testing the order: an answer, else the zone's hint, else the check's, else the area's check (the station's 4th: 7)
T{ 1 5 9 4 first-prompt  0 5 9 4 first-prompt  0 -1 9 4 first-prompt -> 7 5 9 }T
T{ 0 into  0 -1 -1 4 first-prompt  0 -1 -1 0 first-prompt -> 7 -1 }T
testing the areas with a table: the station's, not the warehouse's
T{ 0 has-prompts?  5 has-prompts? -> true false }T
testing with control, by the station's cabinet: something to do with ○ (none without control)
1024 constant toward-the-cabinet   \ (its zone's centre is in a wall: he stands just off it, facing it)
: in-zone? ( x z index -- flag )  {: x z k :}  x z 1 zone-hit drop k = ;
: fits? ( x z index -- flag )  {: x z k :}  x z k in-zone?  x z 1 blocked? 0= and ;
: free-in-zone ( index -- x z )   \ a point of that zone (floor 1) not in a wall: out from its centre
    {: k :}  k 0 0 1 zone-centre {: cx cz :}
    1000 25 do
        cx i + cz k fits? if  cx i + cz unloop exit  then   cx i - cz k fits? if  cx i - cz unloop exit  then
        cx cz i + k fits? if  cx cz i + unloop exit  then   cx cz i - k fits? if  cx cz i - unloop exit  then
    25 +loop  cx cz ;
T{ events-stop  true broadcast player-control  deliver
   3 free-in-zone 0 swap s" matthew" actor-named send figure-place  0 toward-the-cabinet 0 s" matthew" actor-named send figure-turn
   deliver  2 steps  look-prompt @ 0< 0= -> true }T
T{ false broadcast player-control  deliver  2 steps  look-prompt @  act-prompt @ -> -1 -1 }T
testing the ring's: ○ reads whenever there's an item; □ with no one to show it to, none
T{ true broadcast player-control  deliver  pad-triangle tap  ring-up?  ring-look-prompt @  ring-act-prompt @ -> true 3 -1 }T
T{ pad-cross tap  ring-up? -> false }T

test-summary
