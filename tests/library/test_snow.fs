\ the snow on the game's data (docs/subsystems/snow.md)
USING: tester engine actors messages clock areas events daylight snow ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
the-town 0 s" areas" actor-named send enter-area  deliver

testing snowing on the morning of 12/25: a jump in time there, and all 300 flakes are falling
T{ 25 9 0 date>time s" clock" actor-named send set-time  deliver  deliver  snow-live -> 300 }T

testing it clears: one flake fewer a game frame (the town's 30 a second: one a step)
T{ clear true s" daylight" actor-named send set-weather  deliver  10 steps  snow-live -> 290 }T

test-summary
