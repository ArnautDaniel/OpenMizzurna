\ ovl010.fs - area 010 overlay (CD.BIN 010_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl010
USING: events ;

\ $80102680
: entry-80102680
    matthew 6144 0 turn-to
    matthew 6 7 1 anim-then
    10 frames wait
    3 screen-effect
    wait-effect
    103 actor 465805 0 461889 set-pos
    103 actor -3094 0 turn-to
    27 -1 337119 443028 -498 -2146388332 warp
    ;

\ $801026E8
: entry-801026E8
    -1286 -3206 -1868 472900 1 467143 -496 camera-set
    61 say-wait-close
    end-script
    ;
