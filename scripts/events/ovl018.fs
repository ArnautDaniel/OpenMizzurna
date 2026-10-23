\ ovl018.fs - area 018 overlay (CD.BIN 018_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl018
USING: events events.main ;

: L-80102168
    matthew this-one face-each-other
    this-one 15 5 talk
    matthew 16 0 talk
    3 screen-effect
    wait-effect
    this-one 0 0 0 set-pos
    27 -1 411712 169863 3072 0 warp
    ;

\ $801021C4
: entry-801021C4
    121 actor as-this-one
    this-one 68 0 through-door
    goto L-80102168
    ;

\ $801021E0
: entry-801021E0
    121 actor as-this-one
    -872 -2378 -2066 413227 -4000 171027 -5976 camera-set
    this-one 67 -2 through-door
    goto L-80102168
    ;

\ $80102260
: entry-80102260
    -440 -2342 -1982 412106 -4000 169810 -1011 camera-set
    67 door-animate
    events.main:sub-80096CF0
    27 -1 411712 169863 3072 0 warp
    ;

\ $801022B0
: entry-801022B0
    -548 -3566 -698 413031 -3928 168625 -22707 camera-set
    68 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $801022EC
: entry-801022EC
    -344 -2624 -1904 413731 -4000 168509 -20485 camera-set
    68 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;
