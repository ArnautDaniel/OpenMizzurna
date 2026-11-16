\ options.fs - the options (docs/subsystems/options.md): F1 anywhere - the title, a screen, the pause - opens a
\ menu over the game, not one of the original's. While it's up the world stands as under the pause and the
\ game reads no keys (capture-keys!: the menu reads them itself). ↑ ↓ choose a row, ← → change it, F1, Esc or
\ ✕ close it. Its rows: the volume, the vibration, fullscreen - and any a mod adds (add-option).
IN: options
USING: engine actors messages keys screen ui settings pause ;

\ ---- the rows: a label, what it shows now, how ← → change it ----
16 constant #options-max   24 constant label-max
begin-structure option
  label-max +field option-label   field: option-label-len
  field: option-value   holds: ( -- addr len )   \ what it is now, as shown
  field: option-change  holds: ( dir -- )        \ ← -1, → 1
end-structure
create options  #options-max option * allot   variable #options
: option# ( i -- addr )  option * options + ;
: add-option ( label len value-xt change-xt -- )   \ a row more, at the bottom (a mod's too: its label is copied)
    #options @ #options-max >= abort" add-option: no room for more options"
    #options @ option# {: a n v c o :}  n label-max min {: n' :}
    a o option-label n' move  n' o option-label-len !  v o option-value !  c o option-change !
    1 #options +! ;

<PRIVATE
: on-off ( flag -- addr len )  if  s" On"  else  s" Off"  then ;
create volume-text  2 allot
: volume-shown ( -- addr len )   \ "7" .. "10" (two characters at most)
    volume 10 < if  volume [char] 0 + volume-text c!  volume-text 1  else  s" 10"  then ;
: volume-change ( dir -- )  volume + volume! ;
: vibration-shown ( -- addr len )  vibration? on-off ;
: vibration-change ( dir -- )  drop  vibration? 0= vibration! ;
: fullscreen-shown ( -- addr len )  fullscreen? on-off ;
: fullscreen-change ( dir -- )  drop  fullscreen? 0= fullscreen-setting! ;
PRIVATE>
s" Volume" ' volume-shown ' volume-change add-option
s" Vibration" ' vibration-shown ' vibration-change add-option
s" Fullscreen" ' fullscreen-shown ' fullscreen-change add-option

\ ---- up and down ----
variable options-up   variable row
: options-up? ( -- flag )  options-up @ 0<> ;   \ (a fact)
<PRIVATE
key: F1 constant options-key   key: Escape constant escape-key
: open-options ( -- )
    true options-up !  0 row !  true capture-keys!
    s" ui" actor-named send ui-lock  true forth:tasks-paused! ;
: close-options ( -- )
    false options-up !  false capture-keys!
    s" ui" actor-named send ui-unlock  paused? 0= if  false forth:tasks-paused!  then ;   \ (the pause keeps its own)
: changed ( dir -- )  row @ option# option-change @ execute ;
: wrapped ( n -- n' )  #options @ + #options @ mod ;
: options-keys ( -- )
    options-key menu-key-pressed?  escape-key menu-key-pressed? or  pad-cross menu-key-pressed? or if
        close-options exit  then
    pad-up menu-key-pressed? if  row @ 1- wrapped row !  then
    pad-down menu-key-pressed? if  row @ 1+ wrapped row !  then
    pad-left menu-key-pressed? if  -1 changed  then
    pad-right menu-key-pressed? if  1 changed  then ;
: options-step ( -- )
    options-up? if  options-keys exit  then
    options-key menu-key-pressed? if  open-options  then ;

\ ---- drawing: a panel in the middle of the window, the chosen row marked ----
$000000C0 constant panel-color   $FFFFFFFF constant text-color   $FFD040FF constant chosen-color
2 constant text-scale   48 constant panel-columns   4 constant panel-margin
: line-y ( i -- y )  {: i :}  char-size nip  i 2 + *  ;
: draw-options ( -- )
    options-up? 0= if  exit  then
    text-scale pen-scale  char-size {: cw ch :}
    panel-columns cw *  #options @ 3 + ch * {: w h :}
    screen-size {: sw sh :}  sw w - 2/ {: x0 :}  sh h - 2/ {: y0 :}
    blend-usual pen-blend  panel-color pen-color  x0 panel-margin -  y0 panel-margin -
    w panel-margin 2* +  h panel-margin 2* +  draw-rect
    text-color pen-color  s" OPTIONS" x0 y0 draw-text
    #options @ 0 ?do
        i row @ = if  chosen-color  else  text-color  then pen-color
        i option# {: o :}  y0 i line-y + {: y :}
        o option-label o option-label-len @  x0 cw 2* +  y draw-text
        o option-value @ execute  x0 cw label-max * +  y draw-text
    loop
    text-color pen-color  s" Up Down: choose  Left Right: change  F1: close"  x0  y0 h + ch - draw-text ;
' draw-options on-draw
PRIVATE>

state: options-state end-state
behaviour the-options
  on spawned ( -- )  self subscribe tick  self subscribe held-tick ;
  on tick ( -- )  options-step ;
  on held-tick ( -- )  options-step ;
end-behaviour

: spawn-options ( -- id )  the-options options-state s" options" spawn ;
