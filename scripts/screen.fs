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
: channel ( rgba shift -- n )  rshift $FF and ;
: blend-channel ( a b shift t n -- c )   \ (from a's channel to b's, t of n of the way)
    {: a b sh t n :}  a sh channel  b sh channel over -  t * n /  + ;
: bar-shade ( top bottom t n -- rgba )   \ a colour t of n of the way down from top to bottom (alpha opaque)
    {: a b t n :}  a b 24 t n blend-channel 24 lshift  a b 16 t n blend-channel 16 lshift or
    a b 8 t n blend-channel 8 lshift or  $FF or ;
: shaded ( x y w h top bottom -- )   \ a rectangle shaded top to bottom (src: a POLY_G4's two colours)
    {: x y w h top bot :}  scale {: k :}
    h 0 ?do  top bot i h bar-shade pen-color  x y i + >screen  w k *  k  draw-rect  loop ;
\ a health bar (src: hp_bar_draw): its frame `full` + 3 across from y + 8 to y + 15, the health from x + 2 to
\ x + health + 1 and y + 9 to y + 14, each shaded down; the name above (ui/fight-names.png, 80 x 8 each, ten down;
\ -1 none)
$A0A0A0FF constant bar-back-top   $202020FF constant bar-back-bottom
$40E040FF constant bar-fill-top   $204020FF constant bar-fill-bottom
80 constant bar-name-w   8 constant bar-name-h   -1 constant no-name
0 constant matthew-name   2 constant wolf-name   8 constant bear-name   9 constant gang-name
: draw-bar-name ( x y name -- )
    {: x y n :}  n 0< if  exit  then
    s" ui/fight-names.png" image {: im :}  $FFFFFFFF pen-color  blend-ps-solid pen-blend
    im  0 n bar-name-h *  bar-name-w bar-name-h  x y >screen  bar-name-w scale *  bar-name-h scale *  draw-image-part
    blend-usual pen-blend ;
: health-bar ( x y health full name -- )
    {: x y v full n :}
    x y n draw-bar-name
    x y 8 +  full 3 +  7  bar-back-top bar-back-bottom shaded
    v 0> if  x 2 +  y 9 +  v 1-  5  bar-fill-top bar-fill-bottom shaded  then ;
\ the face buttons' icons (the UI's: ui/button-*.png, 16 x 16) - the targets, the keypad, the phone
16 constant button-size
: button-icon ( addr len -- image )  $clear  s" ui/button-" $+  $+  s" .png" $+  $@ image ;
: triangle-icon ( -- image )  s" triangle" button-icon ;   : square-icon ( -- image )  s" square" button-icon ;
: cross-icon ( -- image )  s" cross" button-icon ;         : circle-icon ( -- image )  s" circle" button-icon ;
: icon-at ( image px py -- )   \ centred on a point in window pixels, opaque
    {: im px py :}  button-size scale * {: w :}  $FFFFFFFF pen-color  blend-ps-solid pen-blend
    im  px w 2/ -  py w 2/ -  w w draw-image  blend-usual pen-blend ;
\ the game over (src: gameover_panel_draw: the screen black, the banner, "CONTINUE" below; ui/game-over*.png)
$000000FF constant game-over-backdrop
-128 constant banner-x   -56 constant banner-y   256 constant banner-w   64 constant banner-h
-40 constant over-caption-x   28 constant over-caption-y   80 constant over-caption-w   16 constant over-caption-h
: draw-game-over ( -- )
    game-over-backdrop pen-color  screen-left screen-top >screen  screen-w scale *  screen-h scale *  draw-rect
    $FFFFFFFF pen-color
    s" ui/game-over.png" image  banner-x banner-y banner-w banner-h sprite
    s" ui/game-over-caption.png" image  over-caption-x over-caption-y over-caption-w over-caption-h sprite ;
