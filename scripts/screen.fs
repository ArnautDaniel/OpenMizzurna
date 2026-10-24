\ screen.fs - the original's screen: 320 x 240 with its origin at the centre (the PlayStation's 2D:
\ the windows, the screen's effects), scaled to the window by a whole factor and centred.
IN: screen
USING: engine ;

320 constant screen-w   240 constant screen-h
-160 constant screen-left   160 constant screen-right   -120 constant screen-top   120 constant screen-bottom
: scale ( -- n )  screen-size screen-h / swap screen-w / min 1 max ;
: >screen ( x y -- x' y' )   \ the original's point to the window's pixels
    scale * screen-size nip 2/ +  swap  scale * screen-size drop 2/ +  swap ;

\ ---- the 2D's blending (pen-blend), and the PlayStation's opaque sprites ----
0 constant blend-usual   1 constant blend-add   2 constant blend-subtract
3 constant blend-ps-add   4 constant blend-ps-half   5 constant blend-ps-solid
: sprite ( image x y w h -- )   \ a picture of the library's at the original's place and size, opaque as its
                                \ sprites (a texel marked to blend covers too)
    {: im x y w h :}  blend-ps-solid pen-blend  im x y >screen  w scale *  h scale *  draw-image  blend-usual pen-blend ;

\ ---- what the minigames draw alike ----
\ a health bar (src: hp_bar_draw): its frame `full` + 3 across, 8 high, at x and y + 8; the health filled in it
$302030E0 constant bar-back   $E0B040FF constant bar-fill
: health-bar ( x y health full -- )
    {: x y v full :}  scale {: k :}
    bar-back pen-color  x y 8 + >screen  full 3 + k *  8 k *  draw-rect
    v 0> if  bar-fill pen-color  x 2 + y 9 + >screen  v k *  6 k *  draw-rect  then ;
\ the face buttons' colours (the targets, the keypad: their icons to come)
$40C080FF constant triangle-ink   $E080C0FF constant square-ink   $6080E0FF constant cross-ink   $E05050FF constant circle-ink
\ the game over (src: gameover_panel_draw: its picture to come)
$FFFFFFFF constant game-over-ink   -40 constant game-over-x   -8 constant game-over-y
: draw-game-over ( -- )  game-over-ink pen-color  scale 2 * pen-scale  s" GAME OVER" game-over-x game-over-y >screen draw-text ;
