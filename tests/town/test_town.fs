\ the town (docs/subsystems/town.md): its tiles' places, without the game's data
USING: tester engine actors messages town ;

testing a tile's centre: (col, row) at col * 19980 + 9990, (row + 1) * 19980 + 9990 (src: func_8001D374)
T{ 0 0 tile-centre  17 13 tile-centre -> 9990 29970 349650 289710 }T

testing the lamps' times of day (src: func_800243F0)
T{ lamps-start lamps-on lamps-out -> 961 1021 1381 }T

test-summary
