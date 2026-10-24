\ list-menu.fs - the list menu (docs/subsystems/list-menu.md; src: cui.c func_80047D70, func_80047F30, its form
\ D_800995D4): a board of up to seven labels on the right of the screen - the diner's order, Club Wolves' - a
\ cursor moved with up and down, ○ or □ picks, ✕ cancels. The labels are the list bank's (34: CD.BIN 033_4 part
\ 6), drawn in the list font. Its state is its own; others read the facts menu-up? and menu-answer.
IN: list-menu
USING: engine actors messages keys screen ;

\ ---- facts ----
variable menu-up
-2 constant still-open   -1 constant cancelled
variable menu-answer   still-open menu-answer !   \ the row picked (0..), cancelled, or still open
: menu-up? ( -- flag )  menu-up @ 0<> ;

<PRIVATE
\ ---- the form (the original's screen, the origin at its centre) ----
0 constant form-x   -84 constant form-y          \ src: the form's place (0, -0x54)
144 constant board-w   168 constant board-h      \ its board: a quad, corners shaded, half see-through
46 constant title-label   47 constant special-label   \ "MENU", "DAILY SPECIAL"
56 constant title-x   4 constant title-y
16 constant rows-x   24 constant rows-y   16 constant row-h
6 constant special-row   120 constant special-y  \ the seventh row, under its own heading
34 constant list-bank   1 constant list-font   0 constant list-palette
0 constant cursor-x                             \ (left of the rows: the cursor sprite)
\ src: the quad's corner colours, half see-through (the PlayStation's half: half the screen, half the colour)
$AAAA6E80 constant board-top-left   $647D0F80 constant board-top-right
$87D26E80 constant board-bottom-left   $12693480 constant board-bottom-right
7 constant #rows
create labels  #rows cells allot
variable #labels   variable cursor
variable up-was   variable down-was   variable pick-was   variable cancel-was

: row-y ( row -- y )   \ the seventh row sits under the special's heading
    dup special-row = if  drop special-y row-h +  else  row-h * rows-y +  then ;
: open ( ids.. -- )   \ seven label ids, the first on top (0: none)
    0 #labels !  #rows 0 do  #rows 1- i - cells labels + !  loop
    #rows 0 do  labels i cells + @ 0<> if  1 #labels +!  then  loop
    0 cursor !  still-open menu-answer !  true menu-up !
    pad-up key-down? up-was !  pad-down key-down? down-was !
    pad-circle key-down? pad-square key-down? or pick-was !  pad-cross key-down? cancel-was ! ;
: close ( answer -- )  menu-answer !  false menu-up !  menu-answer @ broadcast list-closed ;
: press-edge ( flag var -- pressed? )  {: f v :}  f v @ 0= and  f v ! ;
: menu-step ( -- )   \ src: func_80047F30
    menu-up? 0= if  exit  then
    pad-circle key-down? pad-square key-down? or pick-was press-edge if  cursor @ close exit  then
    pad-cross key-down? cancel-was press-edge if  cancelled close exit  then
    pad-up key-down? up-was press-edge if  cursor @ 1- dup 0< if  drop #labels @ 1-  then  cursor !  then
    pad-down key-down? down-was press-edge if  cursor @ 1+ dup #labels @ >= if  drop 0  then  cursor !  then ;

\ ---- drawing ----
: at ( x y -- x' y' )  form-y + swap form-x + swap >screen ;
: board ( -- )
    0 0 at  board-w 0 at  0 board-h at  board-w board-h at
    board-top-left board-top-right board-bottom-left board-bottom-right draw-quad ;
: label ( id x y -- )  {: id x y :}  list-bank id text-of  x y at font-text ;
\ the cursor: the UI's sprite (ui/cursor.png, 16 x 16, src: func_80047D70's D_800F6B48), opaque
16 constant cursor-size   variable cursor-image
: the-cursor ( -- )
    cursor-image @ 0= if  s" ui/cursor.png" image cursor-image !  then
    $FFFFFFFF pen-color  blend-ps-solid pen-blend
    cursor-image @  0 0 cursor-size cursor-size  cursor-x cursor @ row-y at  cursor-size scale *  dup  draw-image-part
    blend-usual pen-blend ;
: draw-menu ( -- )
    menu-up? 0= if  exit  then
    board
    list-font font  list-palette font-palette  $FFFFFFFF pen-color  scale pen-scale
    title-label title-x title-y label
    #labels @ #rows = if  special-label rows-x special-y label  then
    #rows 0 do  labels i cells + @ ?dup if  rows-x i row-y label  then  loop
    the-cursor ;
' draw-menu on-draw
PRIVATE>

state: list-menu-state end-state
behaviour the-list-menu
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  menu-step ;
  on list-open ( id0 id1 id2 id3 id4 id5 id6 -- )  open ;
end-behaviour

: spawn-list-menu ( -- id )  the-list-menu list-menu-state s" list-menu" spawn ;
