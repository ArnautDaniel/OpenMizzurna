\ game.fs - where the game starts (after prelude.fs): the actors that make up the game are spawned here, as
\ their subsystems are built (docs/subsystems.md: the map).
IN: game
USING: engine actors keys messages clock window areas matthew camera story ;

spawn-clock drop  spawn-window drop  spawn-areas drop  spawn-matthew drop  spawn-camera drop  spawn-story drop

\ until the title exists: a word on the screen
: hello ( -- )
    $E8E8F0FF pen-color  3 pen-scale
    s" OpenMizzurna" 40 40 draw-text
    $8888A0FF pen-color  2 pen-scale
    s" ` opens the console" 40 90 draw-text ;
' hello on-draw
