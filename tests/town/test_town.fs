\ the town (docs/subsystems/town.md): its tiles' places, without the game's data
USING: tester engine actors messages areas town ;

testing a tile's centre: (col, row) at col * 19980 + 9990, (row + 1) * 19980 + 9990 (src: func_8001D374)
T{ 0 0 tile-centre  17 13 tile-centre -> 9990 29970 349650 289710 }T

testing which tile a point is in, and the map's edges
T{ 344771 285059 tile-of  17 13 tile-centre tile-of -> 17 13 17 13 }T
T{ 0 0 on-map?  31 23 on-map?  32 0 on-map?  0 -1 on-map? -> true true false false }T

testing the lamps' times of day (src: func_800243F0)
T{ lamps-start lamps-on lamps-out -> 961 1021 1381 }T

test-summary
