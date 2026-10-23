\ effects.fs - the screen's effects (docs/subsystems/effects.md; src: effect.c screen_effect /
\ screen_effect_update): fades to and from black or white, wipes, the iris and a tint flash, each a row of
\ the executable's table: its kind, a level that starts somewhere and steps each frame for so many frames,
\ how it meets the picture (the PlayStation's subtract: towards black; add: towards white), its colours.
\ The events wait for one to finish (wait-effect: the fact effect-running?).
IN: effects
USING: engine actors messages screen ;

0 constant no-effect   1 constant fade   2 constant wipe-in   3 constant wipe-out   4 constant iris-effect
5 constant tint
$100 constant under-window      \ (effect + 256: drawn under the message window - the opening's black)
$40 constant subtract-blend   $20 constant add-blend
13 constant soft-iris   $60 constant soft-iris-solid
15e fconstant effect-frames     \ its frames a second (the events ran at 15)
16 constant iris-open   4 constant iris-ring   16 constant iris-pixels

variable kind   variable level   variable step   variable frames-left   variable blend-word
variable color-a   variable color-b   variable solid   variable under   fvariable due
variable iris   variable iris-target   variable r-in   variable r-out
: effect-running? ( -- flag )  frames-left @ 0<> ;   \ (a fact: the events' wait-effect)

: row ( n i -- b )  swap 8 * +  >r s" screen-effects" r> exe-u8 ;   \ byte i of effect n's row
: s8 ( b -- n )  dup $80 and if  256 -  then ;
: start ( n -- )   \ src: screen_effect
    dup under-window and 0<> under !  $FF and
    dup 0 row kind !  dup 1 row s8 level !  dup 2 row s8 step !  dup 3 row 2 + frames-left !
    dup 4 row blend-word !  dup 5 row color-a !  dup 6 row color-b !
    soft-iris = if  soft-iris-solid  else  255  then  solid !  0e due f!
    kind @ no-effect = if  0 frames-left !  then ;   \ (effect 0, or no table: nothing runs, nothing to wait for)

: random ( n -- 0..n-1 )  ticks 1103515245 * 12345 + 16 rshift swap mod ;
: radii ( -- )   \ the iris's ring, by its level (or flickering: level 1)
    level @ 1 = if  8 random 116 + r-out !  8 random 52 + r-in !  exit  then
    iris @ dup iris-ring >= if  iris-ring - iris-pixels *  else  drop 0  then r-in !
    iris @ iris-pixels * r-out ! ;
: frame ( -- )   \ one of the effect's frames
    frames-left @ if  frames-left @ 3 >= if  step @ level +!  then  -1 frames-left +!  then
    kind @ case
        fade of  level @ 0= if  no-effect kind !  0 frames-left !  then  endof
        wipe-in of  frames-left @ 0= if  no-effect kind !  then  endof
        tint of  frames-left @ 0= if  no-effect kind !  then  endof
        iris-effect of
            level @ 0= if
                iris @ iris-target @ < if  2 frames-left !  1 iris +!
                else  iris @ iris-target @ > if  2 frames-left !  -1 iris +!
                else  iris @ iris-open = if  no-effect kind !  0 frames-left !  then  then  then
            then  radii  endof
    endcase ;
: run ( -- )   \ the effect's frames this step
    kind @ no-effect = if  exit  then
    due f@  effect-frames dt f* f+  begin  fdup 1e f>= while  frame  1e f-  repeat  due f! ;

\ ---- drawing ----
: grey ( c -- rgba )  dup 24 lshift over 16 lshift or swap 8 lshift or $FF or ;
: cyan ( c -- rgba )  dup 16 lshift swap 8 lshift or $FF or ;   \ (no red: subtracted, a red flash)
: >pen ( -- )   \ how it meets the picture
    blend-word @ case  subtract-blend of  2 pen-blend  endof  add-blend of  1 pen-blend  endof  0 pen-blend  endcase ;
: corner ( x y -- x' y' )  >screen ;
: shape ( x0 y0 x1 y1 x2 y2 x3 y3 c0 c1 c2 c3 -- )   \ on the original's screen
    {: x0 y0 x1 y1 x2 y2 x3 y3 c0 c1 c2 c3 :}
    x0 y0 corner  x1 y1 corner  x2 y2 corner  x3 y3 corner  c0 c1 c2 c3 draw-quad ;
: whole ( rgba -- )  >r screen-left screen-top screen-right screen-top screen-left screen-bottom screen-right
    screen-bottom r@ r@ r@ r> shape ;
: fade-level ( -- c )  level @ 0 max 32 min  >r s" effect-fade-levels" r> exe-u8 ;
: tint-level ( -- c )  level @ 1+ 0 max 4 min  >r s" effect-tint-levels" r> exe-u8 ;
: edge ( -- )   \ a wipe's soft edge (a slanted band: colour a to b)
    level @ $160 - screen-top  level @ $100 - screen-top  level @ $100 - screen-bottom  level @ $A0 - screen-bottom
    color-a @ grey color-b @ grey color-a @ grey color-b @ grey shape ;
: sine ( k -- s )  15 and  >r s" effect-iris-sines" r> exe-s32 ;   \ the iris's table: 0x10000 = 1
variable ix  variable iy  variable ox  variable oy  variable fx  variable fy     \ the last segment's corners
variable nix variable niy variable nox variable noy variable nfx variable nfy    \ and this one's
: scaled ( s r -- n )  * 16 arshift ;
: segment ( k -- )
    dup sine  swap 4 + sine   ( s c )
    over r-in @ scaled nix !  dup r-in @ scaled niy !  over r-out @ scaled nox !  dup r-out @ scaled noy !
    256 scaled nfy !  256 scaled nfx !
    ix @ iy @ ox @ oy @ nix @ niy @ nox @ noy @  color-a @ grey color-b @ grey color-a @ grey color-b @ grey shape
    ox @ oy @ fx @ fy @ nox @ noy @ nfx @ nfy @  solid @ grey dup dup dup shape
    nix @ ix !  niy @ iy !  nox @ ox !  noy @ oy !  nfx @ fx !  nfy @ fy ! ;
: iris-ring ( -- )   \ 16 segments: shaded from the inner ring to the outer, solid out past the screen
    0 ix !  r-in @ iy !  0 ox !  r-out @ oy !  0 fx !  256 fy !
    17 1 do  i segment  loop ;
: draw-effect ( -- )
    >pen
    kind @ case
        fade of  fade-level grey whole  endof
        wipe-in of  edge  level @ $100 - screen-top screen-right screen-top level @ $A0 - screen-bottom screen-right
            screen-bottom solid @ grey dup dup dup shape  endof
        wipe-out of  edge  screen-left screen-top level @ $160 - screen-top screen-left screen-bottom level @ $100 -
            screen-bottom solid @ grey dup dup dup shape  endof
        iris-effect of  iris-ring  endof
        tint of  tint-level cyan whole  endof
    endcase
    0 pen-blend ;
: draw-effect-under ( -- )  under @ if  draw-effect  then ;
: draw-effect-over ( -- )  under @ 0= if  draw-effect  then ;
' draw-effect-under on-draw

state: effects-state end-state
behaviour screen-effects
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  run ;
  on screen-effect ( n -- )  start ;
  on effect-level ( from to -- )  iris-target !  iris !  radii ;
end-behaviour

: spawn-effects ( -- id )  screen-effects effects-state s" effects" spawn ;
