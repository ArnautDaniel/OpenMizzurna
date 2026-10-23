\ ovl006.fs - area 006 overlay (CD.BIN 006_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl006
USING: events events.main ;

\ $80102270
: entry-80102270
    events.main:sub-80096E74
    33 5 say-mode-wait
    34 5 say-mode-wait
    27 -1 89762 47320 646 0 warp
    ;

\ $801022AC
: entry-801022AC
    1 5 say-mode-wait
    end-script
    ;

\ $801022BC
: entry-801022BC
    32 5 say-mode-wait
    end-script
    ;

\ $801022CC
: entry-801022CC
    15 picture-show
    28 5 say-mode-wait
    29 5 say-mode-wait
    30 3 say-mode-wait
    31 5 say-mode-wait
    picture-hide
    end-script
    ;

\ $8010230C
: entry-8010230C
    11 5 say-mode-wait
    end-script
    ;

\ $8010231C
: entry-8010231C
    matthew 90167 0 48632 set-pos
    matthew 0 4146 0 set-rot
    matthew 0 1 anim
    -332 -2396 -1184 90641 156 48804 4746 camera-set
    matthew 48 50 anim-at  matthew wait-anim
    matthew wait-anim
    10 frames wait
    matthew 49 51 anim-at  matthew wait-anim
    matthew wait-anim
    203 204 cd-pictures
    20 frames wait
    5 5 say-mode-wait
    6 5 say-mode-wait
    20 frames wait
    0 cd-picture
    10 frames wait
    8 picture-show
    10 3 say-mode-wait
    picture-hide
    162 actor 0 0 0 set-pos
    end-script
    ;

\ $801025C8
: entry-801025C8
    -404 -2918 -896 89464 -12 48126 2450 camera-set
    36 door-animate
    events.main:sub-80096CF0
    27 -1 -1 -1 1024 0 warp
    ;

\ $80102618
: entry-80102618
    153 actor as-this-one
    153 actor 13 actor-model
    -524 -2288 -410 89602 60 49488 7740 camera-set
    matthew 89598 0 49518 set-pos
    matthew 0 0 0 set-rot
    matthew 0 1 anim
    10 frames wait
    matthew 12 0 talk
    matthew 1500 0 anim
    ovl006 $80102094 native
    matthew wait-anim
    matthew 0 0 0 set-pos
    10 frames wait
    3 screen-effect
    wait-effect
    0 12 effect-level
    1 frames wait
    12 screen-effect
    -458 -1736 -590 89611 0 48405 2264 camera-set
    10 frames wait
    this-one 6 1 set-floor
    this-one 36 -2 through-door
    this-one 81 0 anim
    this-one wait-anim
    this-one 90620 48470 walk-to
    10 frames wait
    90163 0 48405 10 camera-move
    1808 -10 camera-angle
    this-one wait-stop
    this-one 44 0 anim
    this-one wait-anim
    this-one 46 55 0 anim-then
    this-one wait-anim
    this-one 104 101 1 anim-then
    102 sequence-stop
    30 frames wait
    11 music
    this-one 13 0 13 talk-to
    this-one 102 101 1 anim-then
    this-one 14 0 13 talk-to
    this-one 102 101 1 anim-then
    this-one 15 0 13 talk-to
    this-one 16 0 13 talk-to
    this-one 102 101 1 anim-then
    this-one 17 0 13 talk-to
    this-one 102 101 1 anim-then
    this-one 19 0 13 talk-to
    this-one 103 45 0 anim-then
    this-one wait-anim
    this-one 20 3 13 talk-to
    10 frames wait
    this-one 90110 48470 walk-to
    this-one wait-stop
    10 frames wait
    this-one 14 15 anim-at  this-one wait-anim
    10 frames wait
    this-one 0 1 anim
    this-one 3850 200 turn-to
    this-one wait-turned
    this-one 89810 49310 walk-to
    this-one wait-stop
    dennis 21 0 talk
    this-one 22 0 13 talk-to
    dennis 23 0 talk
    89623 0 48405 10 camera-move
    2168 10 camera-angle
    this-one 17 18 anim-at  this-one wait-anim
    this-one 24 0 13 talk-to
    dennis 25 0 talk
    this-one 26 0 13 talk-to
    this-one 19 0 anim-at  this-one wait-anim
    10 frames wait
    this-one 89810 48490 walk-to
    this-one wait-stop
    this-one 89410 48055 walk-to
    this-one wait-stop
    this-one 36 0 through-door
    this-one 0 0 0 set-pos
    12 0 effect-level
    30 frames wait
    2 screen-effect
    wait-effect
    -440 -2354 -824 89731 0 49593 3776 camera-set
    matthew 89598 0 49778 set-pos
    matthew 0 2048 0 set-rot
    matthew 1501 0 anim
    matthew anim-hold
    ovl006 $801020DC native
    matthew wait-anim
    end-script
    ;
