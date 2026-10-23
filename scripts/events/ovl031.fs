\ ovl031.fs - area 031 overlay (CD.BIN 031_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl031
USING: events ;

\ $80105E28
: arrive-80105E28
    -398 -2726 -752 310982 -24 303501 2780 camera-set
    matthew 310939 0 303286 set-pos
    matthew 0 0 0 set-rot
    matthew -320 set-y
    10 screen-effect
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 8 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 31 1 set-floor
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' arrive-80105E28 $80105E28 31 overlay-word!
