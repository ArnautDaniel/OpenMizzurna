\ area010.fs - area 10's rules: the north-shore cabin (src: ovl010.c; decomp/docs/areas/area10.md;
\ port/src/game/ovl010.c): the crate (crate.fs: push it under the window, climb it), the window (the look row on
\ top: a scene once its time has come, else a peep), a later scene at the landing; both are minigames that are
\ scripts (their fight the events' op: fight.fs).
IN: rules.area010
USING: engine actors messages story-flags events look talk areas matthew crate ;

10 constant this-area
151 constant the-crate-slot
create the-crate  crate-place allot
the-crate the-crate-slot $72A92 0 $72171 this-area 1 crate!
$020 2 story-bit constant inside-begun   $020 3 story-bit constant landing-done   $01C 31 story-bit constant cabin-known
$71B16 constant landing-x   $7129C constant landing-z   240 constant landing-reach

: entered ( -- )  19 map-reveal  the-crate forget-crate ;
: scene-hook ( scene -- )  drop ;
: near-the-landing? ( -- flag )
    matthew-now drop {: x y z :}  x landing-x - dup *  z landing-z - dup * + s>f fsqrt f>s landing-reach < ;
: each-frame ( -- )   \ src: func_80102014
    the-crate crate-step
    inside-begun flag?  landing-done flag? 0= and  near-the-landing? and if
        landing-done flag-toggle  32 minigame  then ;   \ (src: and event_unlock - the lock: to come)

\ ---- the crate: ○ takes hold, □ climbs (src: the crate's handler, cmd 0x31 / 0x32: this area's push and climb) ----
: push-it ( slot -- true )  drop the-crate crate-push  true ;
: climb-it ( slot -- true )  drop the-crate crate-climb  true ;
' push-it the-crate-slot examined-by!   ' climb-it the-crate-slot taken-by!

\ ---- the C its rows and events call ----
: the-cabin-window ( -- )   \ 0x80101FA0: once its time has come, the scene inside (minigame 31); else a peep
    inside-begun flag? 0=  cabin-known flag? and if
        inside-begun flag-set  the-crate crate-cancel  31 minigame exit  then   \ (src: and event_lock)
    ['] events.ovl010:entry-801026E8 start-event ;
: off-the-crate ( -- )  the-crate crate-cancel ;   \ 0x801024AC
this-area ovl $80101FA0 ' the-cabin-window native:   this-area ovl $801024AC ' off-the-crate native:
' events.ovl010:entry-80102754 31 minigame-script!   ' events.ovl010:entry-80102D70 32 minigame-script!

\ each frame (src: func_80102570, before drawing): in scene 1 a five-frame texture animation (0); elsewhere it stays
\ where it was
0 constant strip-anim   5 constant strip-frames   1 constant strip-scene
variable strip-frame
: strip-drawn ( -- )
    scene @ strip-scene <> if  exit  then
    strip-anim strip-frame @ texture-frame  strip-frame @ 1+ strip-frames mod strip-frame ! ;
this-area ' strip-drawn area-every-frame!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
