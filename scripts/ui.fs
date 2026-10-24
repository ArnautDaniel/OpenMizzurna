\ ui.fs - the screens over the field (docs/subsystems/ring.md; src: cui.c ui_lock, ui_busy): while one is up
\ (the item ring; the phone, the notebook and the map to come) the world stands still - the clock, Matthew, the
\ townsfolk, the cars, the camera - and the events go on. The fact ui-busy? is the ui actor's; a screen sends it
\ ui-lock when it opens and ui-unlock when it closes.
IN: ui
USING: engine actors messages ;

variable locks   \ the screens up
: ui-busy? ( -- flag )  locks @ 0> ;

<PRIVATE
: the-clock ( -- id )  s" clock" actor-named ;
PRIVATE>

state: ui-state end-state
behaviour the-ui
  on ui-lock ( -- )  ui-busy? 0= if  menu-open the-clock send hold-clock  then  1 locks +! ;
  on ui-unlock ( -- )
    ui-busy? 0= if  exit  then
    -1 locks +!  ui-busy? 0= if  menu-open the-clock send release-clock  then ;
end-behaviour

: spawn-ui ( -- id )  the-ui ui-state s" ui" spawn ;
