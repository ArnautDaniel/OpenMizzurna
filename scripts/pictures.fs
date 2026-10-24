\ pictures.fs - the pictures with the text (docs/subsystems/pictures.md; src: ccdtim.c): the small picture
\ beside the message box (an item got, the phone: pictures/<n>.png, 48 x 48) and the big one above it (a scene's
\ photograph: two CD.BIN pictures, its halves, 180 x 120). The events show and hide them; an event's end hides
\ the small one, and the big one it showed.
IN: pictures
USING: engine actors messages screen ;

variable small   -1 small !        \ the small picture shown (-1: none)
variable left-half   variable right-half   \ the big one's CD.BIN entries (0: none)
: picture-up? ( -- flag )  small @ 0< 0= ;
: big-picture-up? ( -- flag )  left-half @ 0<> ;

<PRIVATE
28 constant #pictures
-136 constant small-x   40 constant small-y   48 constant small-size   \ src: func_8003E5E8 (-112 less half, 64 less half)
-90 constant big-x   -80 constant big-y   90 constant half-w   120 constant half-h   \ src: func_800156A4
5 constant entry-kinds   600 constant #entries
create small-images #pictures cells allot   small-images #pictures cells erase
create entry-images #entries cells allot   entry-images #entries cells erase
: small-image ( n -- image )   \ pictures/<n>.png, loaded once
    {: n :}  small-images n cells + @ ?dup if  exit  then
    $clear  s" pictures/" $+  n n>str $+  s" .png" $+  $@ image  dup small-images n cells + ! ;
: entry-image ( entry -- image )   \ cdbin/NNN_K.png, loaded once
    {: e :}  entry-images e cells + @ ?dup if  exit  then
    $clear  s" cdbin/" $+  e entry-kinds / three-digits  s" _" $+  e entry-kinds mod n>str $+  s" .png" $+
    $@ image  dup entry-images e cells + ! ;
: half ( entry x -- )  {: e x :}  e entry-image  x big-y half-w half-h sprite ;
: draw-pictures ( -- )
    $FFFFFFFF pen-color
    big-picture-up? if  left-half @ big-x half  right-half @ ?dup if  big-x half-w + half  then  then
    picture-up? if  small @ small-image  small-x small-y small-size small-size sprite  then ;
' draw-pictures on-draw
PRIVATE>

state: pictures-state end-state
behaviour picture-box
  on show-picture ( n -- )  dup 0< over #pictures >= or if  drop -1  then  small ! ;
  on hide-picture ( -- )  -1 small ! ;
  on show-big-picture ( left right -- )  right-half !  left-half ! ;
end-behaviour

: spawn-pictures ( -- id )  picture-box pictures-state s" pictures" spawn ;
