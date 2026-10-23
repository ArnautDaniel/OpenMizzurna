\ screen.fs - the original's screen: 320 x 240 with its origin at the centre (the PlayStation's 2D:
\ the windows, the screen's effects), scaled to the window by a whole factor and centred.
IN: screen
USING: engine ;

320 constant screen-w   240 constant screen-h
-160 constant screen-left   160 constant screen-right   -120 constant screen-top   120 constant screen-bottom
: scale ( -- n )  screen-size screen-h / swap screen-w / min 1 max ;
: >screen ( x y -- x' y' )   \ the original's point to the window's pixels
    scale * screen-size nip 2/ +  swap  scale * screen-size drop 2/ +  swap ;
