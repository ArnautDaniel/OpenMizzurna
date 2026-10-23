\ the town on the game's data: its tiles streamed around the camera, its street lamps
USING: tester engine actors messages clock areas daylight town town.private events ;

: the-areas ( -- id )  s" areas" actor-named ;
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: step ( -- )  broadcast tick  deliver ;
: steps ( n -- )  0 ?do  step  loop ;
: all-lamps ( -- n )  0  #lamps 0 do  i lamp-lit? -  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;   \ (the new game the library starts: its opening stopped)
settled

testing in the town, the 6 x 6 tiles around the camera are loaded; out of it, none
T{ the-town 0 the-areas send enter-area  deliver  344771 -4000 285059 s" camera" actor-named send camera-pivot  deliver
   step  tiles-loaded -> 36 }T
T{ camera-tile -> 17 13 }T

testing the lamps: lit from 17:01 to 23:00, unlit by day, coming on between 16:01 and 17:00
T{ 25 20 0 at-time  step  all-lamps -> #lamps }T
T{ 25 12 0 at-time  step  all-lamps -> 0 }T
T{ 25 16 30 at-time  20 steps  all-lamps -> #lamps }T

testing its billboards (the library's text): tile 17,13 has 12 about its origin; the town 8 sprites
T{ 17 13 billboards #records  17 13 tile-origin  sprites #records -> 12 349650 289710 8 }T
T{ 17 13 billboards 0 record  dup at-x nth  over bottom nth  rot sprite nth -> 2620 -4072 1 }T

testing their light: a sixth of the first light's sum, at least 40 (src: D_800A3848)
T{ 0 true s" daylight" actor-named send set-weather  deliver  26 12 0 at-time  billboard-grey -> 114 }T
T{ 26 22 0 at-time  billboard-grey -> 40 }T

testing leaving the town lets its tiles go
T{ 1 0 the-areas send enter-area  deliver  deliver  tiles-loaded -> 0 }T

test-summary
