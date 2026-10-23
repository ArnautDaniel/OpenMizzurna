\ ovl011.fs - area 011 overlay (CD.BIN 011_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl011
USING: events events.main ;

\ $8010243C
: entry-8010243C
    11 music
    matthew 466924 0 411843 set-pos
    matthew 0 2048 0 set-rot
    matthew 0 1 anim
    -476 -2414 -680 467008 24 411825 2004 camera-set
    10 frames wait
    matthew 81 0 anim
    matthew wait-anim
    -470 -578 -680 467296 96 411897 1836 camera-set
    10 frames wait
    7 32767 actor say-by-wait
    8 32767 actor say-by-wait
    -1844 -1370 -3650 466924 0 411843 2028 camera-set
    matthew 466844 411223 walk-to
    9 32767 actor say-by-wait
    matthew wait-stop
    matthew 81 0 anim
    10 32767 actor say-by-wait
    matthew wait-anim
    end-script
    ;

\ $80102560
: entry-80102560
    -224 -3170 -788 466940 0 411410 3463 camera-set
    45 door-animate
    events.main:sub-80096CF0
    27 -1 -1 -1 0 0 warp
    ;

\ $801025B0
: entry-801025B0
    -440 -2270 -950 466771 12 410072 1624 camera-set
    46 door-animate
    events.main:sub-80096CF0
    matthew 467080 0 409994 set-pos
    1 set-scene
    end-script
    ;

\ $80102600
: entry-80102600
    -386 -2234 -734 467152 0 409994 -896 camera-set
    46 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $8010263C
: entry-8010263C
    3 5 say-mode-wait
    4 5 say-mode-wait
    end-script
    ;

\ $80102658
: entry-80102658
    matthew 48 49 0 anim-then
    matthew wait-anim
    matthew 51 1 anim
    10 frames wait
    5 5 say-mode-wait
    10 frames wait
    matthew 55 0 anim
    matthew wait-anim
    10 frames wait
    27 picture-show
    6 3 say-mode-wait
    picture-hide
    10 frames wait
    end-script
    ;

\ $801026DC
: entry-801026DC
    matthew 467227 0 410092 set-pos
    matthew 0 0 0 set-rot
    matthew 0 1 anim
    -590 -614 -1022 467299 0 410127 -606 camera-set
    40 frames wait
    end-script
    ;
