\ game.fs - where the game starts (after prelude.fs): the actors that make up the game are spawned here, as
\ their subsystems are built (docs/subsystems.md: the map).
IN: game
USING: engine actors keys messages clock events events.main fight shootout tail dog-chase walkers flare driving car-chase effects window list-menu knock items areas daylight sky snow billboards town traffic townsfolk routes matthew camera talk look story
    events.checks events.schedules events.doors events.car-routes rules.area000 rules.area001 rules.area002 rules.area003 rules.area005 rules.area006 rules.area007 rules.area008 rules.area009 rules.area010 rules.area011 rules.area012 rules.area013 rules.area016 rules.area017 rules.area018 rules.area019 rules.area021 rules.area023 rules.area024 rules.area026 rules.area027 rules.area028 ;

spawn-clock drop  spawn-effects drop  spawn-window drop  spawn-list-menu drop  spawn-knock drop  spawn-items drop  spawn-daylight drop  spawn-sky drop  spawn-snow drop  spawn-town drop  spawn-traffic drop  spawn-townsfolk drop  spawn-routes drop  spawn-look drop  spawn-areas drop  spawn-matthew drop  spawn-camera drop  spawn-story drop  spawn-fight drop  spawn-shootout drop  spawn-tail drop  spawn-dog-chase drop  spawn-walkers drop  spawn-flare drop  spawn-driving drop  spawn-car-chase drop

\ the main executable's minigames that are a script alone (src: minigame_main_start): Winona's ride to the school
\ (6 / 8 she comes along, off her schedule; 7 / 9 not now), 23 a warp
' events.main:exe-80096950 6 minigame-script!   ' events.main:exe-80096978 7 minigame-script!
' events.main:exe-800969B4 8 minigame-script!   ' events.main:exe-800969DC 9 minigame-script!

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
