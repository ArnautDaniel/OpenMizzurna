\ map.fs - the town map (docs/subsystems/map.md; src: mapscreen.c): chosen in the ring it opens over the field,
\ the world standing still (ui.fs): the town's picture, its places marked (a dot, a line to an icon; some only
\ once the map knows them: story-flags.fs's map-known), Matthew's arrow (his car's while he drives) pulsing red.
\ The hand moves with the d-pad (faster with △ or ○ held), L1 and R1 jump it to the place before and after; over
\ a place its name shows. ✕ closes. The police computer is the same screen (src: ovl000's FPolice.C): the
\ map in its other palette, the places of the case files it holds; □ on one plays its file (the list waits).
\ Its state is its own; others read map-up?.
IN: map
USING: engine actors messages keys screen ui story-flags figures matthew driving events ;

variable map-open
: map-up? ( -- flag )  map-open @ 0<> ;
variable computer   \ (the police computer's: src D_800A34F0)
variable computer-places   variable file-opener   variable file-playing   -1 file-playing !

<PRIVATE
22 constant #places
$1F constant icon-bits   $40 constant only-once-known
1000 34 5 * + 3 + constant place-names   \ (the text of CD.BIN 034_3: a place each)
: place-flags ( i -- n )  s" map-places" rot exe-u8 ;
: shown? ( i -- flag )   \ src: map_known (the computer: its own places alone)
    computer @ if  1 swap lshift computer-places @ and 0<> exit  then
    dup place-flags only-once-known and 0= if  drop true exit  then
    1 swap lshift map-known @ and 0<> ;
: dot-x ( i -- x )  s" map-dots" rot 2* exe-s16 ;
: dot-y ( i -- y )  s" map-dots" rot 2* 1+ exe-s16 ;
: icon-x ( i -- x )  s" map-icons" rot 2* exe-s16 ;
: icon-y ( i -- y )  s" map-icons" rot 2* 1+ exe-s16 ;

\ ---- the hand ----
variable hand-x   variable hand-y   variable chosen   \ (chosen: the place L1 / R1 jumped to)
-144 constant left-edge   136 constant right-edge   -96 constant top-edge   88 constant bottom-edge
2 constant slow   8 constant fast   7 constant into-the-icon   16 constant icon-size
fvariable due
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: clamp ( n lo hi -- n' )  rot min max ;
: move-hand ( -- )   \ a frame (src: cursor_move)
    pad-triangle key-down? pad-circle key-down? or if  fast  else  slow  then {: d :}
    pad-up key-down? if  hand-y @ d - top-edge bottom-edge clamp hand-y !  then
    pad-down key-down? if  hand-y @ d + top-edge bottom-edge clamp hand-y !  then
    pad-right key-down? if  hand-x @ d + left-edge right-edge clamp hand-x !  then
    pad-left key-down? if  hand-x @ d - left-edge right-edge clamp hand-x !  then ;
: any-shown? ( -- flag )  false  #places 0 do  i shown? or  loop ;
: jump ( way -- )   \ src: map_cursor_cycle: to the next place shown that way
    any-shown? 0= if  drop exit  then  {: w :}
    begin  chosen @ w + #places + #places mod  dup chosen !  shown? until
    chosen @ icon-x into-the-icon + hand-x !  chosen @ icon-y into-the-icon + hand-y ! ;
: under-hand ( -- place | -1 )   \ src: marker_at
    -1  #places 0 do
        i shown? if
            hand-x @ i icon-x - icon-size u<  hand-y @ i icon-y - icon-size u< and if  drop i leave  then
        then
    loop ;

\ ---- his arrow, pulsing (src: func_8004CB38) ----
variable red   variable fading
$6F constant first-red   6 constant red-step   $10 constant faintest   $7F constant reddest
1998 constant units-across   2497 constant units-down   150 constant map-left   104 constant map-top
: pulse ( -- )   \ a frame
    fading @ if  red @ red-step - dup faintest < if  drop faintest  false fading !  then  red !  exit  then
    red @ red-step + dup reddest > if  drop reddest  true fading !  then  red ! ;
: where-he-is ( -- x z heading )   \ (in the car: the car)
    driving? if  his-car-now exit  then  matthew-now {: x y z h :}  x z h ;

\ ---- opening, a frame, closing ----
variable cross-was   variable l1-was   variable r1-was   variable square-was
: edge ( flag var -- pressed? )  {: f v :}  f v @ 0= and  f v ! ;
: open-map ( -- )
    false computer !  -1 file-playing !
    true map-open !  0 hand-x !  0 hand-y !  0 chosen !  first-red red !  false fading !
    s" ui" actor-named send ui-lock  0e due f!
    pad-cross key-down? cross-was !  pad-l1 key-down? l1-was !  pad-r1 key-down? r1-was ! ;
: close-map ( -- )  false map-open !  false computer !  s" ui" actor-named send ui-unlock ;
: open-computer ( places opener -- )   \ src: ovl000 func_80103B6C
    file-opener !  computer-places !  open-map  true computer !  pad-square key-down? square-was ! ;
: playing-file? ( -- flag )   \ a case file's scene on: the list waits
    file-playing @ dup 0< if  drop false exit  then  event-alive? dup 0= if  -1 file-playing !  then ;
: map-step ( -- )
    pad-cross key-down? cross-was edge  pad-l1 key-down? l1-was edge  pad-r1 key-down? r1-was edge
    pad-square key-down? square-was edge  {: x l r q :}
    map-up? 0= if  exit  then
    playing-file? if  exit  then
    x if  close-map exit  then
    computer @  q and if  under-hand dup 0< 0= if  file-opener @ execute file-playing !  exit  then  drop  then
    steps-due 0 ?do  move-hand  pulse  loop
    r if  1 jump  else  l if  -1 jump  then  then ;

\ ---- drawing (the original's screen) ----
-160 constant map-x   -96 constant map-y   320 constant map-w   192 constant map-h
-154 constant dot-left   100 constant dot-top   8 constant dot-size   4 constant dot-middle
$F03030FF constant line-color
96 constant hint-x   80 constant hint-y   16 constant button-w   40 constant hint-w
-8 constant bar-x   64 constant bar-y   160 constant bar-w   16 constant bar-h   66 constant name-y   $20202080 constant bar-color
24 constant hand-w
variable map-image   variable other-map-image   variable dot-image   variable hand-image   variable exit-image   variable cross-image
create icon-images #places cells allot   icon-images #places cells erase
: ui-image ( var addr len -- image )
    {: v a n :}  v @ ?dup if  exit  then  $clear  s" ui/" $+  a n $+  s" .png" $+  $@ image  dup v ! ;
: icon-image ( k -- image )
    {: k :}  icon-images k cells + @ ?dup if  exit  then
    $clear  s" ui/map-icon-" $+  k n>str $+  s" .png" $+  $@ image  dup icon-images k cells + ! ;
: dot-at ( i -- x y )  dup dot-x dot-left +  swap dot-y negate dot-top + ;
: thick-line ( x0 y0 x1 y1 color -- )   \ a line a screen pixel thick (scaled), in window pixels
    {: x0 y0 x1 y1 c :}  x1 x0 - abs  y1 y0 - abs > if  0 scale  else  scale 0  then {: dx dy :}
    x0 y0 x1 y1 x0 dx + y0 dy + x1 dx + y1 dy + c c c c draw-quad ;
: line ( i -- )   \ from the dot's middle to the icon's (src: the marker's LINE_F2)
    {: i :}  i dot-at dot-middle + swap dot-middle + swap >screen  i icon-x 8 + i icon-y 8 + >screen
    line-color thick-line ;
: draw-places ( -- )
    $FFFFFFFF pen-color
    #places 0 do  i shown? if  dot-image s" map-dot" ui-image  i dot-at dot-size dot-size sprite  then  loop
    #places 0 do  i shown? if
        i line  $FFFFFFFF pen-color
        i place-flags icon-bits and icon-image  i icon-x i icon-y icon-size icon-size sprite
    then  loop ;
: draw-arrow ( -- )   \ src: func_8004CB38: a triangle at his place, turned with his heading
    where-he-is {: x z h :}
    x units-across / map-left -  z units-down / negate map-top +  {: px py :}
    h 4095 and negate s>f 4096e f/ 2e f* pi f* {: F: a :}
    a fsin a fcos {: F: s F: c :}
    px s -8e f* f>s +  py c -8e f* f>s +  >screen
    px s 8e f* c 7e f* f- f>s +  py c 8e f* s 7e f* f+ f>s +  >screen
    px s 8e f* c 6e f* f+ f>s +  py c 8e f* s 6e f* f- f>s +  >screen
    {: x0 y0 x1 y1 x2 y2 :}  red @ 24 lshift $202000 or $FF or {: ink :}
    x0 y0 x1 y1 x2 y2 x2 y2 ink ink ink ink draw-quad ;
\ the computer's hint: □ CHECK (src: police_sprites, at (0, 80): the label from the HUD's sheet)
0 constant check-x   80 constant check-y   $38 constant check-u   $40 constant check-v   56 constant check-w
variable square-image   variable sheet-image
: draw-check ( -- )
    $FFFFFFFF pen-color  square-image s" button-square" ui-image  check-x check-y button-w button-w sprite
    sheet-image s" clock" ui-image {: im :}  blend-ps-solid pen-blend
    im check-u check-v check-w button-w  check-x button-w + check-y >screen  check-w scale *  button-w scale *  draw-image-part
    blend-usual pen-blend ;
: draw-name ( -- )
    under-hand dup 0< if  drop exit  then  {: i :}
    bar-color pen-color  bar-x bar-y >screen  bar-w scale *  bar-h scale *  draw-rect
    0 font  0 font-palette  $FFFFFFFF pen-color  scale pen-scale
    place-names i text-of  2dup font-width {: w :}  bar-w w - 2/ bar-x + name-y >screen font-text ;
: draw-map ( -- )
    map-up? 0= if  exit  then
    $FFFFFFFF pen-color
    computer @ if  other-map-image s" map-other" ui-image  else  map-image s" map" ui-image  then  map-x map-y map-w map-h sprite
    playing-file? if  draw-places exit  then   \ (a file's scene: the places only - src func_80103CBC)
    cross-image s" button-cross" ui-image  hint-x hint-y button-w button-w sprite
    exit-image s" exit" ui-image  hint-x button-w + hint-y hint-w button-w sprite
    draw-places  computer @ if  draw-check  else  draw-arrow  then
    $FFFFFFFF pen-color  hand-image s" hand" ui-image  hand-x @ hand-y @ hand-w icon-size sprite
    draw-name ;
' draw-map on-draw
PRIVATE>

state: map-state end-state
behaviour the-map
  on spawned ( -- )  self subscribe tick  self subscribe ring-chosen ;
  on tick ( -- )  map-step ;
  on ring-chosen ( item who -- )   \ src: ui_update's choice 2
    0< swap 2 = and  map-up? 0= and if  open-map  then ;
  on computer-open ( places opener -- )  map-up? if  2drop exit  then  open-computer ;
end-behaviour

: spawn-map ( -- id )  the-map map-state s" map" spawn ;
