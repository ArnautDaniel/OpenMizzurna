\ door-sounds.fs - a door's sounds (the events' op $176; src: cevcon.c func_80041D24): by its kind, one of the main
\ executable's little scripts of sounds and waits, on a thread of its own (the last one's cut short) - Matthew
\ stays the player's. The third kind's sound is the area's (the executable's `door-sounds`, by the content area;
\ none there: silent), twice.
IN: door-sounds
USING: engine actors messages areas events events.main ;

<PRIVATE
-2 constant main-thread   \ (src: g_main_thread: no figure's)
2 constant area-kind   10 constant frames-before   2 constant frames-between
variable area-door
: area-door-sound ( -- )   \ src: D_800967CC with its two sounds patched
    frames-before frames wait  area-door @ sound  frames-between frames wait  area-door @ sound ;
: script-of ( kind -- xt | 0 )
    case
        0 of  ['] exe-8009672C  endof   6 of  ['] exe-80096758  endof   7 of  ['] exe-8009677C  endof
        1 of  ['] exe-800967A0  endof   4 of  ['] exe-80096704  endof   5 of  ['] exe-80096718  endof
        area-kind of  ['] area-door-sound  endof   3 of  ['] exe-800967F0  endof
        0 swap
    endcase ;
: a-door ( kind -- )
    dup area-kind = if  s" door-sounds" area @ content exe-u8 dup area-door !  0= if  drop exit  then  then
    script-of ?dup if  main-thread start-route  then ;
' a-door is door-sound
PRIVATE>
