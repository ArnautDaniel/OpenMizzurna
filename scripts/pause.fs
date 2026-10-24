\ pause.fs - Start (docs/subsystems/pause.md; src: pad_map.c pad_update's Start, cui.c ui_update, field.c,
\ cevcon.c event_threads_update): after the prologue it pauses the game - the world and the events stand still,
\ "PAUSE" in the middle of the screen - and again goes on; before the prologue's end, with no event running, it
\ skips the rest of the prologue (the main executable's script $80096598). Not while a screen is up. Others
\ read the fact paused?.
IN: pause
USING: engine actors messages keys screen ui story-flags events events.main ;

variable paused
: paused? ( -- flag )  paused @ 0<> ;

<PRIVATE
variable start-was
: pause-on ( -- )  true paused !  s" ui" actor-named send ui-lock  true forth:tasks-paused! ;
: pause-off ( -- )  false paused !  s" ui" actor-named send ui-unlock  false forth:tasks-paused! ;
: start ( -- )
    prologue-done flag? 0= if
        events-running @ 0=  ui-busy? 0= and if  ['] events.main:exe-80096598 start-event  then  exit  then
    paused? if  pause-off exit  then
    ui-busy? 0= if  pause-on  then ;
: start-step ( -- )
    pad-start key-down? dup start-was @ 0= and  swap start-was !  if  start  then ;

\ "PAUSE" (src: func_800168D0: 56 x 16 at the screen's middle, from the clock's sheet)
variable label-image
-28 constant label-x   -4 constant label-y   56 constant label-w   16 constant label-h   $40 constant label-v
: draw-pause ( -- )
    paused? 0= if  exit  then
    label-image @ 0= if  s" ui/clock.png" image label-image !  then
    $FFFFFFFF pen-color  blend-ps-solid pen-blend
    label-image @ 0 label-v label-w label-h  label-x label-y >screen  label-w scale *  label-h scale *  draw-image-part
    blend-usual pen-blend ;
' draw-pause on-draw
PRIVATE>

state: pause-state end-state
behaviour the-pause
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  start-step ;
end-behaviour

: spawn-pause ( -- id )  the-pause pause-state s" pause" spawn ;
