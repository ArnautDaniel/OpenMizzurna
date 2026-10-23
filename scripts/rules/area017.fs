\ area017.fs - area 17's rules: the high school's main building (src: ovl017.c; decomp/docs/areas/area17.md;
\ port/src/game/ovl017.c): with a companion along she leads the way up (on her own thread: he stays the
\ player's), the locker she waits at, the desk after it, the way out with or without her, closing time by floor
\ and where he is.
IN: rules.area017
USING: engine actors messages clock story-flags items townsfolk events look talk areas matthew ;

17 constant this-area
6 constant the-locker-key   8 constant the-floppy   10 constant the-pocket-camera   26 constant the-main-key
129 constant the-locker-check   4 constant the-desk-zone
$58FC2 constant at-the-locker-x   $23E86 constant at-the-locker-z   \ where her walk ends
: give ( item -- )  s" items" actor-named send give-item ;
: take ( item -- )  s" items" actor-named send take-item ;
: with-him? ( -- flag )  winona off-schedule? ;   \ (src: her stop -1)

\ ---- this visit (src: D_801039A8 the desk done, D_801039AC the locker done, D_801032E0 closing done) ----
variable desk-done   variable locker-done   variable closing-done
: leading? ( -- flag )  winona route-thread 0< 0= ;   \ her walk up (src: minigame 13 running)
: entered ( -- )
    14 map-reveal  0 desk-done !  0 locker-done !  0 closing-done !
    winona going-along? if  ['] events.ovl017:entry-801025D8 winona start-route  then ;

\ ---- each frame (0x80102278): closing time, 17:59 - 22:59, once: shown out from where he is ----
create closing-floor-1  ' events.ovl017:entry-80103720 , ' events.ovl017:entry-80103468 , ' events.ovl017:entry-80103380 ,
  ' events.ovl017:entry-80103550 , ' events.ovl017:entry-80103638 , ' events.ovl017:entry-80103808 ,
create closing-floor-2  ' events.ovl017:entry-80103760 , ' events.ovl017:entry-801034A8 , ' events.ovl017:entry-801033C0 ,
  ' events.ovl017:entry-80103590 , ' events.ovl017:entry-80103678 , ' events.ovl017:entry-80103848 ,
create closing-floor-3  ' events.ovl017:entry-801037A0 , ' events.ovl017:entry-801034E8 , ' events.ovl017:entry-80103400 ,
  ' events.ovl017:entry-801035D0 , ' events.ovl017:entry-801036B8 , ' events.ovl017:entry-80103888 ,
create closing-floors  closing-floor-1 , closing-floor-2 , closing-floor-3 ,
3 constant the-classroom
enum: wings  front-hall  west-corridor  east-corridor  far-east-wing  far-west-wing  far-middle ;
$23C86 constant front-hall-z   $2437F constant corridors-z   $5A640 constant corridor-split-x
$5C47F constant far-east-x   $58FD3 constant far-west-x
: wing ( -- n )   \ where he is: each floor laid out alike
    matthew-now drop nip {: x z :}
    z front-hall-z <= if  front-hall exit  then
    z corridors-z <= if  x corridor-split-x <= if  west-corridor  else  east-corridor  then  exit  then
    x far-east-x > if  far-east-wing exit  then
    x far-west-x <= if  far-west-wing exit  then
    far-middle ;
: closing ( -- )
    closing-done @  leading? or  0 17 59 22 59 between? 0= or if  exit  then
    scene @ {: sc :}
    sc the-classroom = if  ['] events.ovl017:entry-801038C8
    else  sc 3 u< 0= if  exit  then  sc cells closing-floors + @  wing cells + @  then
    start-event  1 closing-done ! ;

\ ---- ○ (0x801020AC / 0x80102194): the locker she waits at; the desk, with the camera ----
enum: look-action  nothing-here  open-the-locker  sit-at-the-desk ;
: she-waits-there? ( -- flag )
    winona person-now drop nip {: x z :}  with-him?  x at-the-locker-x = and  z at-the-locker-z = and ;
: look-check ( zone check -- action )
    {: z c :}
    c the-locker-check = if  locker-done @ 0=  she-waits-there? and if  open-the-locker  else  nothing-here  then  exit  then
    z the-desk-zone =  desk-done @ 0= and  with-him? and  the-pocket-camera has-item? and if  sit-at-the-desk exit  then
    nothing-here ;
: look-do ( action -- )
    case
        open-the-locker of
            1 locker-done !  14 minigame  the-locker-key take  the-floppy give  the-pocket-camera give  endof
        sit-at-the-desk of
            1 desk-done !  the-main-key take  ['] events.ovl017:entry-80102B58 start-event  endof
        drop run-rows
    endcase ;
: act-check ( zone check -- action )  2drop 0 ;
: act-do ( action -- )  drop run-rows ;

\ ---- the C its rows call ----
: the-way-out ( -- )   \ 0x80102068: not without her; else out to the library
    with-him? if  ['] events.ovl017:entry-80102B4C  else  ['] events.ovl017:entry-80102BFC  then  start-event ;
this-area ovl $80102068 ' the-way-out native:
' events.ovl017:entry-80102874 14 minigame-script!   ' events.ovl017:entry-80102A00 15 minigame-script!

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
this-area ' entered 0 area-hooks!
this-area ' closing area-frame!
