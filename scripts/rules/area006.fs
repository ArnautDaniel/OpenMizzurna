\ area006.fs - area 6's rules: Astin Farm (src: ovl006.c; decomp/docs/areas/area06.md; port/src/game/ovl006.c): the
\ two things in the room he can take (their handlers: src actor_handler_140, actor_handler_162), the warning when
\ the room's owner heads home, the closet and the door out (the look rows' C), the closet's doors.
IN: rules.area006
USING: engine actors messages clock story-flags items townsfolk events look talk areas ;

6 constant this-area
140 constant the-diary   162 constant the-note   14 constant diary-item   16 constant note-item
$018 5 story-bit constant diary-here-now   \ (src: +0x18 bit 5: set in area 28)
5 constant at-his-home   \ (Mel's record 5)
: give ( item -- )  s" items" actor-named send give-item ;
: park ( slot -- )  0 0 0 set-pos ;

\ ---- this visit (src: D_80102AF8 hide-counter, D_80102AFC the warning said) ----
variable must-hide   variable warned

\ ---- entering (0x80101ED0): the things in their places, or gone ----
: entered ( -- )
    2 map-reveal  0 warned !  0 must-hide !
    the-note this-area 1 set-floor
    emma-diary-found-missing flag?  got-emmas-diary flag? 0= and  diary-here-now flag? and if
        the-diary $15D00 $136 negate $C292 set-pos  the-diary $5A $1A4 negate 0 set-rot
    else  the-diary park  then
    got-mels-note flag? if  the-note park  else  the-note $16026 0 $BEC9 set-pos  then ;
: scene-hook ( scene -- )  drop ;

\ ---- each frame (0x801021F0): its owner on the way home - the warning, and out ----
: heading-home? ( -- flag )   \ src: func_8002E204: travelling, the next stop at home
    mel person-of {: p :}  p travelling @ 0= if  false exit  then
    p p at-stop @ 1+ p #stops @ 1- min row-of stop-record @ at-his-home = ;
: each-frame ( -- )
    warned @ 0=  heading-home? and if  1 warned !  ['] events.ovl006:entry-80102270 start-event  then ;

\ ---- the things: ○ examines, □ takes ----
: what's-this ( slot -- true )  drop ['] events.ovl006:entry-801022BC start-event  true ;
: take-diary ( slot -- true )
    park  ['] events.ovl006:entry-801022CC start-event  diary-item give  got-emmas-diary flag-set  true ;
27 constant the-27th   20 constant still-out   \ (before the 27th, his stop before 20: out on the road)
: someone-coming ( -- )   \ 0x80102124: before the 27th he'll be coming: hide (src: and event_lock - to come)
    clock-time time>day the-27th < if  1 must-hide +!  then ;
: take-note ( slot -- true )
    park
    clock-time time>day the-27th <  mel person-of at-stop @ still-out < and if  ['] events.ovl006:entry-80102418
    else  ['] events.ovl006:entry-8010231C  then  start-event
    note-item give  got-mels-note flag-set  someone-coming  true ;
' what's-this the-diary examined-by!   ' take-diary the-diary taken-by!
' what's-this the-note examined-by!    ' take-note the-note taken-by!

\ ---- the C its rows and events call ----
: closet-doors-open ( -- )  7 1 do  i object-anim  loop ;     \ 0x80102094: the closet's doors (objects' 1 .. 6)
: closet-doors-shut ( -- )  13 7 do  i object-anim  loop ;    \ 0x801020DC: (7 .. 12)
: the-closet ( -- )   \ 0x80102164: a place to hide when it's needed
    must-hide @ if  0 must-hide !  ['] events.ovl006:entry-80102618  else  ['] events.ovl006:entry-801022AC  then
    start-event ;
: the-way-out ( -- )   \ 0x801021A8: not with someone coming
    must-hide @ if  ['] events.ovl006:entry-8010230C  else  ['] events.ovl006:entry-801025C8  then  start-event ;
this-area ovl $80102094 ' closet-doors-open native:   this-area ovl $801020DC ' closet-doors-shut native:
this-area ovl $80102164 ' the-closet native:   this-area ovl $801021A8 ' the-way-out native:

this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
