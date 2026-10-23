\ game.fs - where the game starts (after prelude.fs): the actors that make up the game are spawned here, as
\ their subsystems are built (docs/subsystems.md: the map).
IN: game
USING: engine actors keys messages clock effects window items areas townsfolk matthew camera talk look story
    events.checks events.schedules rules.area001 ;

spawn-clock drop  spawn-effects drop  spawn-window drop  spawn-items drop  spawn-townsfolk drop  spawn-look drop  spawn-areas drop  spawn-matthew drop  spawn-camera drop  spawn-story drop

\ until the title exists: with the game's library there, a new game at once; without (the tests), a word on
\ the screen
: library? ( -- flag )  s" exe/SLPS_017.83" library-file nip 0> ;
: hello ( -- )
    $E8E8F0FF pen-color  3 pen-scale
    s" OpenMizzurna" 40 40 draw-text
    $8888A0FF pen-color  2 pen-scale
    s" ` opens the console" 40 90 draw-text ;
: begin-game ( -- )
    library? if  s" story" actor-named send new-game  else  ['] hello on-draw  then ;
begin-game
