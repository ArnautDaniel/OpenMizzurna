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
