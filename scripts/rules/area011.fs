\ area011.fs - area 11's rules: the Lakeside Inn's room (src: ovl011.c; decomp/docs/areas/area11.md;
\ port/src/game/ovl011.c): the crate (crate.fs), one thing to find and take (its handler: src actor_handler_141), a
\ scene the first time in. The room's looks are the rows'.
IN: rules.area011
USING: engine actors messages story-flags items townsfolk events look talk areas crate ;

11 constant this-area
16 constant the-crate-slot   141 constant the-find   18 constant the-find-item
$72337 constant crate-x   $64526 constant crate-z
create the-crate  crate-place allot
the-crate the-crate-slot crate-x 0 crate-z this-area 1 crate!

: entered ( -- )
    16 map-reveal  the-crate forget-crate
    got-epoch-pills flag? 0= if
        the-find crate-x 0 crate-z set-pos  the-find 0 $2BC 0 set-rot  the-find this-area 1 set-floor  then
    kathys-room-seen flag? 0= if  kathys-room-seen flag-set  ['] events.ovl011:entry-8010243C start-event  then ;
: scene-hook ( scene -- )  drop ;
: each-frame ( -- )  the-crate crate-step ;

\ ---- the crate: ○ takes hold, □ climbs ----
: push-it ( slot -- true )  drop the-crate crate-push  true ;
: climb-it ( slot -- true )  drop the-crate crate-climb  true ;
' push-it the-crate-slot examined-by!   ' climb-it the-crate-slot taken-by!

\ ---- the thing: ○ finds it (and then it can be taken), □ takes it ----
2 constant found-it   \ (src: the session's +0x64 bit 1)
: found? ( -- flag )  pending-scenes session@ found-it and 0<> ;
: look-at-find ( slot -- true )
    drop  ['] events.ovl011:entry-8010263C start-event  pending-scenes session@ found-it or pending-scenes session!  true ;
: take-find ( slot -- handled? )   \ once found, until taken
    found? 0=  got-epoch-pills flag? or if  drop false exit  then
    0 0 0 set-pos  ['] events.ovl011:entry-80102658 start-event
    the-find-item s" items" actor-named send give-item  got-epoch-pills flag-set  true ;
' look-at-find the-find examined-by!   ' take-find the-find taken-by!

this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
