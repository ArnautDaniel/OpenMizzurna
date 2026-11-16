\ mods/menu.fs - the mods menu (the title's MODS, there when mods/ has any; docs/modding.md): the mods in their load
\ order, each ON or OFF, and why one asked for didn't load this run. The title's pad drives it: up / down pick one,
\ ○ turns it on or off, left / right move it in the order, ✕ goes back - writing mods/order.txt if anything changed.
\ It applies when OpenMizzurna next starts (a mod's scripts load with the game's). The list is the engine's
\ (src/game/mods.c: mods-menu-count, mods-menu-mod ...). Drawn on the original's 320 x 240 screen (screen.fs).
IN: mods.menu
USING: engine screen ;

<PRIVATE
variable at         \ the row under the cursor
variable saved      \ order.txt written this visit: 0 not, 1 written, -1 couldn't be
: rows ( -- n )  mods-menu-count ;

\ ---- where things go: the original's screen, from its centre; the rows in the font at half the screen's scale (a
\ mod's reason is long) ----
-144 constant left-x   -104 constant title-y   -80 constant first-row-y   10 constant row-h
24 constant name-indent   8 constant cursor-gap   120 constant why-indent   72 constant reason-y   88 constant foot-y
100 constant note-y
$000000C0 constant backdrop        \ over the title's picture, so the rows read
$FFFFFFFF constant chosen-colour   $A8A0B0FF constant row-colour
$A0F0A0FF constant on-colour       $808080FF constant off-colour   $F0A060FF constant why-colour
: in-font ( -- )  0 font  0 font-palette  scale pen-scale ;
: in-small-font ( -- )  0 font  0 font-palette  scale 2/ 1 max pen-scale ;
: say ( addr len x y rgba -- )  pen-color  >screen font-text ;
: row-y ( i -- y )  row-h *  first-row-y + ;
: row ( i -- )
    {: i :}  i mods-menu-mod {: a n on :}
    on if  s" ON"  on-colour  else  s" OFF"  off-colour  then  {: c :}  left-x i row-y c say
    a n  left-x name-indent +  i row-y  i at @ = if  chosen-colour  else  row-colour  then  say
    i mods-menu-why nip if  s" not loaded"  left-x why-indent +  i row-y  why-colour say  then ;
: reason ( -- )   \ why the chosen mod didn't load this run, in full
    rows 0= if  exit  then  at @ mods-menu-why  left-x reason-y  why-colour say ;
PRIVATE>

: open-mods ( -- )  0 at !  0 saved ! ;
: pick-mod ( delta -- )  at @ +  0 max  rows 1- 0 max min  at ! ;   \ the cursor up (-1) or down (1)
: move-mod ( delta -- )  rows if  at @ swap mods-menu-move at !  else  drop  then ;   \ the mod up or down the order
: switch-mod ( -- )  rows if  at @ mods-menu-switch  then ;
: leave-mods ( -- done? )   \ back: order.txt written if anything changed; not written, it stays (and says so)
    mods-menu-changed? if  mods-menu-save if  1  else  -1  then  saved !  then
    saved @ 0< 0= ;
: draw-mods ( -- )
    backdrop pen-color  screen-left screen-top >screen  screen-w scale *  screen-h scale *  draw-rect
    in-font
    s" MODS"  left-x title-y  chosen-colour say
    in-small-font
    rows 0 ?do  i row  loop
    s" >"  left-x cursor-gap -  at @ row-y  chosen-colour say
    reason
    s" ○ on/off   left/right: order   ✕ back"  left-x foot-y  row-colour say
    mods-menu-changed? if  s" the changes apply at the next start"  left-x note-y  why-colour say  then
    saved @ 0< if  s" mods/order.txt can't be written"  left-x note-y row-h +  why-colour say  then ;
