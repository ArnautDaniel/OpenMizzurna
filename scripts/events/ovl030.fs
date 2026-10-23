\ ovl030.fs - area 030 overlay (CD.BIN 030_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl030
USING: events ;

\ $80106DD4
: arrive-80106DD4
    matthew 98591 0 352784 set-pos
    matthew 0 0 0 set-rot
    matthew 880 set-y
    -512 -2738 -716 98962 1200 354217 2338 camera-set
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
    matthew 30 0 set-floor
    end-script
    ;

\ $801071E8
: arrive-801071E8
    matthew 100471 0 353994 set-pos
    matthew 1200 set-y
    matthew 0 0 0 set-rot
    -554 -2924 -164 100710 1284 354270 1404 camera-set
    2 screen-effect
    wait-effect
    1 1 elevator-doors
    20 frames wait
    matthew 100471 354614 walk-to
    matthew wait-stop
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' arrive-80106DD4 $80106DD4 30 overlay-word!
' arrive-801071E8 $801071E8 30 overlay-word!
