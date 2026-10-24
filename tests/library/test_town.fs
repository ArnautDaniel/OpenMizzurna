\ the town on the game's data: its tiles streamed around the camera, its street lamps, its moving signs and the
\ doors' animation slots (a door's town side swings its slot, once through)
USING: tester engine actors messages clock keys areas floors daylight matthew billboards town town.private events doors ;

: the-areas ( -- id )  s" areas" actor-named ;
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: step ( -- )  broadcast tick  deliver ;
: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;   \ (a tick of the game's)
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

testing the ground under a point: the loaded tile's terrain, its highest polygon (src: func_8001C264)
T{ 344771 285059 ground-at -> -4082 true }T

testing on foot: the tile's obstacles block, from its centre (src: point_blocked); off the map is blocked
T{ 344771 285059 1 blocked?  349650 289710 1 blocked?  344771 284200 1 blocked?  -100 -100 1 blocked?
   -> false true false true }T
T{ 344771 285059 1 zone-hit -> 0 0 }T

testing Matthew walking in the town: along the street, on the ground; into the shop's wall, not
: matthew-at ( x z heading -- )
    0 swap 0 s" matthew" actor-named send figure-turn  0 swap s" matthew" actor-named send figure-place  deliver ;
: walked ( ticks -- x y z )   \ up held so long
    pad-up true key-hold  steps  pad-up false key-hold  1 steps  matthew-now drop ;
: along-the-street? ( x y z -- west? south? on-ground? )
    {: x y z :}  x 344771 <  z 285059 <  y -4082 -4000 1+ within ;
T{ true broadcast player-control  deliver  344771 285059 2560 matthew-at  30 walked along-the-street?
   -> true true true }T
T{ 344771 285059 0 matthew-at  30 walked nip nip  285300 < -> true }T

testing its billboards (the library's text): tile 17,13 has 12 about its origin; the town 8 sprites
T{ 17 13 billboards #records  17 13 tile-origin  sprites #records -> 12 349650 289710 8 }T
T{ 17 13 billboards 0 record  dup at-x nth  over bottom nth  rot sprite nth -> 2620 -4072 1 }T

testing their light: a sixth of the first light's sum, at least 40 (src: D_800A3848)
T{ 0 true s" daylight" actor-named send set-weather  deliver  26 12 0 at-time  billboard-grey -> 114 }T
T{ 26 22 0 at-time  billboard-grey -> 40 }T

testing the signs keep moving: the turning ones' turn, the swinging one's
T{ sign-turn @  swing-x sw-now @  4 steps  swing-x sw-now @ <>  swap sign-turn @ <> -> true true }T
testing a door's town side: its animation slot plays once through, then still
: a-town-door ( -- n )  #doors 0 do  i town-room door-spot 0< 0= if  i unloop exit  then  loop  -1 ;
T{ a-town-door 0< 0= -> true }T
T{ a-town-door door-opens  deliver  a-town-door town-room door-spot slot-motion@ 0<> -> true }T
T{ 300 steps  a-town-door town-room door-spot slot-motion@ -> 0 }T
testing the events' object op in the town: its slot (op $156)
T{ 5 object-anim  deliver  5 slot-motion@ 0<> -> true }T
testing leaving the town lets its tiles go
T{ 1 0 the-areas send enter-area  deliver  deliver  tiles-loaded -> 0 }T

test-summary
