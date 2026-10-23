\ area008.fs - area 8's rules: the junkyard and its bus (src: ovl008.c; decomp/docs/areas/area08.md;
\ port/src/game/ovl008.c): a figure on the bus at one point of the story, the seat to look at and then search,
\ getting off the bus, the minigame that is a script alone, the dog chase (minigame 26: dog-chase.fs). (Minigame 43,
\ the two-player viewer: to come.)
IN: rules.area008
USING: engine actors messages clock story-flags townsfolk events look talk areas dog-chase ;

8 constant this-area
153 constant on-the-bus   1 constant morgans-kind
$008 16 story-bit constant seat-searched   \ (src: +0x08 bit 16)
: bus-visit ( -- n )  $01C story@ $60000000 and ;   \ (src: +0x1C bits 29 - 30)

\ ---- this visit (src: D_80103844 the seat looked at, D_80103840 searched) ----
variable seat-looks   variable searched

: entered ( -- )
    3 map-reveal  0 seat-looks !  0 searched !
    bus-visit $20000000 = if
        on-the-bus $19EF9 0 $328ED set-pos  on-the-bus 0 $200 0 set-rot  on-the-bus morgans-kind actor-model  then ;
: scene-hook ( scene -- )  drop ;

\ ---- ○ the seat (0x80101FA0 / 0x80101FF4), □ searching it (0x80102068 / 0x801020B4) ----
enum: look-action  nothing-here  2 the-seat ;
: look-check ( zone check -- action )
    nip 0= if  seat-searched flag? if  nothing-here  else  the-seat  then  else  nothing-here  then ;
: look-do ( action -- )
    the-seat = if
        seat-looks @ if  ['] events.ovl008:entry-80103058  else  ['] events.ovl008:entry-80103030  then  start-event
        1 seat-looks +!  exit  then
    run-rows ;
enum: act-action  nothing-to-do  search-the-seat ;
: act-check ( zone check -- action )  nip 0=  seat-looks @ 2 >= and if  search-the-seat  else  nothing-to-do  then ;
: act-do ( action -- )
    search-the-seat = if
        searched @ if  ['] events.ovl008:entry-801030E4 start-event exit  then
        ['] events.ovl008:entry-80103090 start-event  seat-searched flag-set  1 searched !  exit  then
    run-rows ;

\ ---- talking here (0x801021A4): the figure on the bus ----
: talks-here ( slot -- handled? )
    on-the-bus <> if  false exit  then
    ['] events.ovl008:entry-801031B0 start-event  $01C story@ $40000000 or $01C story!  true ;
' talks-here this-area area-talk!

\ ---- the C its events call ----
: off-the-bus ( -- )   \ 0x80102140
    seat-searched flag?  $01C story@ 29 rshift 1 and 0= and if  ['] events.ovl008:entry-801030FC
    else  ['] events.ovl008:entry-80103150  then  start-event ;
this-area ovl $80102140 ' off-the-bus native:
' events.ovl008:entry-801034A0 28 minigame-script!   \ (src: area_minigame_start's 28)
: the-dog-chase ( -- )   \ minigame 26 (src: area_minigame_start / update): its scene, the chase until he's at
    events.ovl008:entry-801032C8                         \ the yard's far side (the scene's wait), then its end
    s" dog-chase" actor-named send dog-chase-start
    events.ovl008:entry-80103428
    s" dog-chase" actor-named send dog-chase-stop
    events.ovl008:entry-8010344C ;
' the-dog-chase 26 minigame-script!

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
this-area ' entered ' scene-hook area-hooks!
