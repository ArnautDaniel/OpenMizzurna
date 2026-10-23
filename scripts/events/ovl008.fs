\ ovl008.fs - area 008 overlay (CD.BIN 008_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl008
USING: events events.main ;

\ $80103030
: entry-80103030
    matthew 44 0 anim
    matthew wait-anim
    matthew 46 anim-once
    matthew wait-anim
    2 say-wait-close
    end-script
    ;

\ $80103058
: entry-80103058
    matthew 44 0 anim
    matthew wait-anim
    matthew 46 anim-once
    matthew wait-anim
    3 say-wait-close
    matthew 47 0 anim
    matthew wait-anim
    end-script
    ;

\ $80103090
: entry-80103090
    matthew 44 0 anim
    matthew wait-anim
    matthew 46 anim-once
    matthew wait-anim
    4 say-wait-close
    5 say-wait-close
    187 188 cd-pictures
    6 say-wait-close
    7 say-wait-close
    0 cd-picture
    end-script
    ;

\ $801030E4
: entry-801030E4
    matthew 81 0 anim
    8 say-wait-close
    end-script
    ;

\ $801030FC
: entry-801030FC
    11 screen-effect
    wait-effect
    0 set-scene
    matthew 105042 0 207167 set-pos
    72 time-skip
    26 minigame
    wait-minigame
    27 -1 108147 200581 2558 0 warp
    ;

\ $80103150
: entry-80103150
    11 screen-effect
    wait-effect
    0 set-scene
    matthew 107276 0 209681 set-pos
    matthew 3538 0 turn-to
    10 screen-effect
    wait-effect
    end-script
    ;

\ $801031B0
: entry-801031B0
    28 minigame
    wait-minigame
    27 -1 107919 198884 -3470 0 warp
    ;

\ $801031D8
: entry-801031D8
    -542 -5402 -536 105996 -2 218303 4250 camera-set
    39 door-animate
    events.main:sub-80096CF0
    27 -1 106793 199299 5631 0 warp
    ;

\ $80103228
: entry-80103228
    11 screen-effect
    wait-effect
    matthew 107690 0 209166 set-pos
    matthew -2528 0 turn-to
    1 set-scene
    10 screen-effect
    wait-effect
    end-script
    ;
