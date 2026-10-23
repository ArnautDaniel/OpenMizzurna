\ ovl029.fs - area 029 overlay (CD.BIN 029_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl029
USING: events ;

\ $80104960
: arrive-80104960
    ovl029 $801045EC native
    257 0 call-pack-script
    end-script
    ;

\ $80106958
: arrive-80106958
    matthew 99505 0 353110 set-pos
    matthew 0 0 0 set-rot
    matthew -960 set-y
    -254 -1478 -2012 99514 -1200 353682 1704 camera-set
    10 screen-effect
    matthew 10 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 13 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 29 2 set-floor
    end-script
    ;

\ $8010715C
: arrive-8010715C
    matthew 100471 0 353994 set-pos
    matthew -1200 set-y
    matthew 0 0 0 set-rot
    -554 -2924 -164 100710 -1116 354270 1404 camera-set
    2 screen-effect
    wait-effect
    1 1 elevator-doors
    20 frames wait
    matthew 100471 354614 walk-to
    matthew wait-stop
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' arrive-80104960 $80104960 29 overlay-word!
' arrive-80106958 $80106958 29 overlay-word!
' arrive-8010715C $8010715C 29 overlay-word!
