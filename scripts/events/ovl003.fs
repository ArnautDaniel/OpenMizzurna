\ ovl003.fs - area 003 overlay (CD.BIN 003_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl003
USING: events events.main ;

\ $80102CB4
: entry-80102CB4
    matthew 97 anim-once
    matthew 309 190 0 3 260 act-command
    matthew 191 0 talk
    3 screen-effect
    wait-effect
    matthew 3 1 set-floor
    17 actor unride
    17 actor 0 1 anim
    17 actor 3 1 set-floor
    17 actor 176464 0 423881 set-pos
    matthew 175525 0 423599 set-pos
    17 actor 0 0 face
    matthew 17 0 face
    0 set-scene
    -566 -2342 -1130 175297 -4240 423966 1024 camera-set
    40 frames wait
    2 screen-effect
    17 actor 192 7 talk
    matthew 193 7 talk
    35 frames wait
    matthew 2008 200 turn-to
    matthew wait-turned
    27 door-animate
    48 frames wait
    3 screen-effect
    17 actor place-at-stop
    27 -1 175679 423034 2142 0 warp
    ;

: L-80103DC4
    matthew 176228 0 423750 set-pos
    matthew 0 1 anim
    17 actor 178427 0 423072 set-pos
    17 actor 0 3072 0 set-rot
    17 actor 161 1 anim
    winona 178654 0 423348 set-pos
    winona 0 -1790 0 set-rot
    winona 0 1 anim
    steve 176708 0 424040 set-pos
    steve 0 0 0 set-rot
    -494 -2252 -1562 176264 -4264 423852 1152 camera-set
    steve 178118 424040 walk-to
    matthew 177808 423720 walk-to
    10 frames wait
    178208 -4264 423852 30 camera-move
    15 frames wait
    winona 90 80 turn-by
    winona 4 0 anim
    99 winona say-by
    wait-text
    steve wait-stop
    steve 17 120 face
    matthew wait-stop
    matthew 17 18 anim-at  matthew wait-anim
    10 frames wait
    -488 -848 -446 178208 -4264 423852 -288 camera-set
    0 1 say-mode-wait
    1 1 say-mode-wait
    2 1 say-mode-wait
    10 frames wait
    96 steve say-by
    wait-text
    matthew 19 14 0 anim-then
    matthew wait-anim
    matthew 15 1 anim
    97 matthew say-by
    wait-text
    steve 903 904 1 anim-then
    20 frames wait
    98 steve say-by
    wait-text
    wait-camera-move
    -638 -1724 -1304 179246 -3556 422514 -2681 camera-set
    20 frames wait
    100 matthew say-by
    matthew 178658 0 423990 set-pos
    matthew 16 0 anim
    matthew 17 0 face
    wait-text
    101 17 actor say-by
    wait-text
    3 1 say-mode-wait
    4 1 say-mode-wait
    102 matthew say-by
    wait-text
    103 matthew say-by
    wait-text
    20 frames wait
    steve 0 0 face
    steve 0 0 anim
    104 17 actor say-by
    wait-text
    105 17 actor say-by
    wait-text
    -578 -1562 -1160 177854 -4288 423396 -3281 camera-set
    106 steve say-by
    steve 86 0 anim
    wait-text
    107 matthew say-by
    wait-text
    108 matthew say-by
    matthew 12 120 face
    matthew 90 0 anim
    wait-text
    109 steve say-by
    steve 87 0 anim
    wait-text
    110 matthew say-by
    matthew 89 0 anim
    wait-text
    -1 music
    111 17 actor say-by
    17 actor 164 0 anim
    17 actor wait-anim
    17 actor 168 169 1 anim-then
    winona 17 -120 face
    matthew 17 18 1 anim-then
    wait-text
    112 steve say-by
    steve 14 15 1 anim-then
    wait-text
    113 steve say-by
    steve 88 0 anim
    17 actor 170 175 0 anim-then
    17 actor wait-anim
    17 actor 161 1 anim
    matthew 19 0 anim
    wait-text
    114 steve say-by
    steve 89 0 anim
    wait-text
    115 matthew say-by
    matthew 89 0 anim
    wait-text
    end-script
    ;

\ $80102DEC
: arrive-80102DEC
    -386 -3146 -410 175754 -4324 423891 1328 camera-set
    steve as-this-one
    this-one 176210 0 424023 set-pos
    this-one 0 2647 0 set-rot
    2 screen-effect
    matthew 175505 0 423544 set-pos
    matthew 0 1492 0 set-rot
    10 frames wait
    matthew -90 -200 turn-by
    matthew 15 walk-forward
    5 frames wait
    this-one -30 -200 turn-by
    7 frames wait
    matthew idle
    goto L-80103DC4
    ;

\ $80102EC0
: entry-80102EC0
    -476 -1778 -1370 178664 -5554 423573 -727 camera-set
    14 matthew say-by-wait
    end-script
    ;

\ $80102EF0
: entry-80102EF0
    -476 -1778 -1370 178664 -5554 423573 -727 camera-set
    13 matthew say-by-wait
    end-script
    ;

\ $80102F20
: entry-80102F20
    17 actor as-this-one
    this-one 177 0 talk
    128 17 actor say-by
    17 actor 89 0 anim
    wait-text
    events.main:sub-80096D74
    17 actor 1 ride
    end-script
    ;

: L-80102FC8
    153 actor 3 1 set-floor
    153 actor 177734 0 424838 set-pos
    153 actor -5600 set-y
    153 actor 3072 0 turn-to
    153 actor 6 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 7 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 7 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 7 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 7 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 7 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 7 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 7 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 7 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 9 0 anim
    153 actor anim-hold
    153 actor wait-anim
    153 actor 3 1 set-floor
    153 actor 175253 424350 walk-to
    end-script
    ;

: L-8010316C
    steve as-this-one
    153 actor 1 actor-model
    102 actor 7 actor-model
    matthew 177715 0 423692 set-pos
    matthew 0 5102 0 set-rot
    matthew 0 1 anim
    matthew 3 1 set-floor
    this-one 178286 0 423797 set-pos
    this-one 0 -937 0 set-rot
    this-one 0 1 anim
    17 actor 0 0 0 set-pos
    0 set-scene
    20 frames wait
    -458 -2882 -248 177653 -4240 423360 2462 camera-set
    10 screen-effect
    wait-effect
    10 frames wait
    this-one 67 7 talk
    matthew 68 4 talk
    this-one 14 15 anim-at  this-one wait-anim
    10 frames wait
    this-one 69 0 talk
    10 frames wait
    this-one 16 0 anim-at  this-one wait-anim
    this-one 70 7 talk
    matthew 71 3 talk
    this-one 72 5 talk
    matthew 73 7 talk
    matthew 74 3 talk
    matthew 75 4 talk
    this-one 76 2 talk
    20 frames wait
    this-one 85 0 anim-at  this-one wait-anim
    15 music
    this-one 77 5 talk
    matthew 78 6 talk
    this-one 79 4 talk
    10 frames wait
    this-one 80 3 talk
    matthew 81 4 talk
    this-one 907 908 anim-at  this-one wait-anim
    10 frames wait
    this-one 82 0 talk
    10 frames wait
    matthew 83 0 talk
    111 sound
    steve 909 0 anim
    matthew 3352 -200 turn-to
    85 matthew 1 say-timed-by-wait
    text-hold
    86 steve 1 say-timed-by-wait
    text-close
    this-one 176726 424157 5 60 walk-to-ex
    10 frames wait
    9 screen-effect
    wait-effect
    matthew 178278 0 422787 set-pos
    matthew 0 245 0 set-rot
    matthew 0 1 anim
    this-one 176826 0 423167 set-pos
    this-one 0 4120 0 set-rot
    this-one 15 1 anim
    102 actor 176848 0 423447 set-pos
    102 actor 0 1960 0 set-rot
    102 actor 0 1 anim
    102 actor 3 1 set-floor
    17 actor 178427 0 423072 set-pos
    17 actor 0 3072 0 set-rot
    17 actor 161 1 anim
    17 actor 3 1 set-floor
    90 time-skip
    6 music
    -674 -326 -716 175902 -4240 424503 1637 camera-set
    map-sync
    80 frames wait
    8 screen-effect
    wait-effect
    177270 -4240 424503 11 camera-move
    102 actor 90 0 anim-at  102 actor wait-anim
    this-one 16 0 anim-at  this-one wait-anim
    87 matthew 2 say-timed-by
    this-one 87 0 anim
    this-one wait-anim
    20 frames wait
    88 17 actor 2 say-timed-by-wait
    text-close
    this-one 175196 -4240 423737 set-pos
    this-one 0 5450 0 set-rot
    wait-camera-move
    20 frames wait
    -476 -830 -464 177014 -4180 423969 2911 camera-set
    89 steve 2 say-timed-by
    102 actor 27 0 through-door
    ['] L-80102FC8 start-event
    102 actor 0 0 0 set-pos
    steve 17 18 anim-at  steve wait-anim
    3295 10 camera-angle
    wait-camera-turn
    steve 175196 424037 walk-to
    85 frames wait
    153 actor 87 0 anim-at  153 actor wait-anim
    153 actor 90 3 talk
    this-one 91 7 talk
    153 actor 17 18 anim-at  153 actor wait-anim
    10 frames wait
    153 actor 92 0 talk
    this-one 93 4 talk
    153 actor 19 0 anim-at  153 actor wait-anim
    153 actor 94 0 talk
    this-one 95 7 talk
    153 actor 87 0 anim-at  153 actor wait-anim
    2983 -10 camera-angle
    153 actor 175553 423580 walk-to
    this-one 14 0 1 anim-then
    this-one 1540 60 turn-to
    153 actor wait-stop
    153 actor 27 0 through-door
    10 frames wait
    this-one 178796 0 423797 set-pos
    -482 -1604 -776 178382 -4180 423111 1351 camera-set
    17 actor 163 0 anim-at  17 actor wait-anim
    17 actor 0 -200 face
    17 actor wait-anim
    matthew 47 7 talk
    17 actor 48 4 talk
    matthew 49 5 talk
    this-one 178496 423377 walk-to
    17 actor 50 3 talk
    17 actor 51 2 talk
    17 actor 85 0 anim
    17 actor wait-anim
    17 actor 52 3 talk
    matthew 53 1 talk
    17 actor 907 908 1 anim-then
    17 actor 54 0 talk
    this-one 17 18 1 anim-then
    this-one 55 0 talk
    matthew 83 84 1 anim-then
    30 frames wait
    17 actor 909 0 1 anim-then
    this-one 19 0 anim
    this-one wait-anim
    matthew 85 0 1 anim-then
    matthew 56 5 talk
    17 actor 57 4 talk
    17 actor 1 ride
    matthew 0 3072 0 set-rot
    end-script
    ;

\ $80103978
: entry-80103978
    steve as-this-one
    this-one 0 0 face
    this-one 60 5 talk
    matthew 12 0 face
    matthew 61 4 talk
    17 actor 62 0 talk
    this-one 63 4 talk
    10 frames wait
    11 screen-effect
    wait-effect
    17 actor unride
    17 actor 0 0 0 set-pos
    goto L-8010316C
    ;

\ $80103A0C
: entry-80103A0C
    steve as-this-one
    this-one 64 0 talk
    17 actor 65 5 talk
    17 actor 66 4 talk
    10 frames wait
    11 screen-effect
    wait-effect
    17 actor unride
    goto L-8010316C
    ;

\ $80103A68
: arrive-80103A68
    steve as-this-one
    matthew 177008 0 424047 set-pos
    matthew 0 1715 0 set-rot
    matthew 0 1 anim
    this-one 176986 0 423467 set-pos
    this-one 0 1180 0 set-rot
    this-one 0 1 anim
    17 actor 177166 0 423407 set-pos
    17 actor 0 0 0 set-rot
    17 actor 908 1 anim
    -572 -3020 -1268 177132 -4048 423587 2775 camera-set
    map-sync
    10 screen-effect
    wait-effect
    20 frames wait
    matthew 90 0 anim
    this-one 36 anim-once
    this-one wait-anim
    20 frames wait
    17 actor 909 0 anim
    17 actor wait-anim
    this-one 37 anim-once
    17 actor 17 18 1 anim-then
    this-one wait-anim
    matthew 47 5 talk
    17 actor 19 0 anim
    17 actor wait-anim
    17 actor 48 4 talk
    this-one 17 18 1 anim-then
    matthew 49 5 talk
    17 actor 50 3 talk
    17 actor 51 2 talk
    17 actor 85 0 anim
    this-one 19 86 0 anim-then
    17 actor wait-anim
    17 actor 52 3 talk
    matthew 53 1 talk
    17 actor 907 908 1 anim-then
    17 actor 54 0 talk
    this-one 17 18 1 anim-then
    this-one 55 0 talk
    matthew 83 84 1 anim-then
    30 frames wait
    17 actor 909 0 1 anim-then
    this-one 19 0 anim
    this-one wait-anim
    this-one 36 anim-once
    10 frames wait
    matthew 85 0 1 anim-then
    matthew 56 5 talk
    this-one 37 0 1 anim-then
    17 actor 57 4 talk
    17 actor 1 ride
    matthew 0 3072 0 set-rot
    26 actor 182904 0 416008 set-pos
    26 actor 0 -1024 0 set-rot
    end-script
    ;

\ $801042F8
: entry-801042F8
    17 actor 178427 0 423072 set-pos
    17 actor 0 3072 0 set-rot
    17 actor 68 1 anim
    matthew 178359 0 422813 set-pos
    matthew 17 0 face
    -398 -2270 -1292 178519 -4240 422897 707 camera-set
    116 matthew say-by
    matthew 89 0 anim
    wait-text
    17 actor 164 167 anim-at  17 actor wait-anim
    17 actor 171 172 anim-at  17 actor wait-anim
    117 17 actor say-by
    wait-text
    118 17 actor say-by
    wait-text
    119 17 actor say-by
    wait-text
    120 matthew say-by
    matthew 89 0 anim
    wait-text
    17 actor 173 167 anim-at  17 actor wait-anim
    118 17 actor say-by
    wait-text
    122 matthew say-by
    matthew 86 0 anim
    wait-text
    123 matthew say-by
    matthew 90 0 anim
    wait-text
    124 matthew say-by
    matthew 89 0 anim
    wait-text
    125 17 actor say-by
    wait-text
    126 17 actor say-by
    wait-text
    matthew 87 0 anim
    matthew wait-anim
    17 actor 166 0 anim
    17 actor wait-anim
    17 actor 0 -80 face
    128 17 actor say-by
    17 actor 89 0 anim
    wait-text
    129 17 actor say-by
    17 actor 87 0 anim
    wait-text
    17 actor 1 ride
    end-script
    ;

\ $80104530
: entry-80104530
    17 actor as-this-one
    matthew 176659 0 423064 set-pos
    matthew 0 2624 0 set-rot
    matthew 0 1 anim
    this-one 176549 0 422674 set-pos
    this-one 0 3072 0 set-rot
    this-one 0 1 anim
    -278 -1622 -2030 176553 -4240 422834 2676 camera-set
    10 frames wait
    this-one 59 60 1 anim-then
    30 frames wait
    this-one 61 0 anim
    this-one wait-anim
    this-one 40 0 anim
    this-one wait-anim
    109 sound
    this-one 41 0 anim
    this-one wait-anim
    matthew 130 7 talk
    this-one 131 0 talk
    this-one 40 0 anim
    this-one wait-anim
    105 sound
    this-one 41 0 anim
    this-one wait-anim
    109 sound
    20 frames wait
    this-one 165 0 anim
    108 sound
    17 actor wait-anim
    this-one 167 1 anim
    10 frames wait
    -1 music
    4 xa-play
    50 frames wait
    matthew 132 7 talk
    10 frames wait
    this-one 79 72 1 anim-then
    this-one 133 0 talk
    10 frames wait
    matthew 134 2 talk
    40 frames wait
    this-one 73 74 1 anim-then
    matthew 85 0 anim
    matthew wait-anim
    matthew 135 6 talk
    30 frames wait
    this-one 75 72 1 anim-then
    this-one 136 0 talk
    20 frames wait
    this-one 80 68 1 anim-then
    matthew 83 84 1 anim-then
    137 5 say-mode-wait
    10 frames wait
    138 5 say-mode-wait
    matthew 85 0 anim
    matthew wait-anim
    30 frames wait
    this-one 139 0 talk
    10 frames wait
    xa-stop
    ['] events.main:sub-800995C0 start-event
    end-script
    ;

\ $80104834
: entry-80104834
    steve as-this-one
    matthew 140 5 talk
    this-one 141 0 talk
    matthew 142 7 talk
    this-one 143 0 talk
    matthew 144 6 talk
    this-one 145 0 talk
    end-script
    ;

\ $80104888
: entry-80104888
    matthew 146 7 talk
    steve 147 0 talk
    matthew 148 4 talk
    steve 149 0 talk
    end-script
    ;

\ $801048C4
: entry-801048C4
    17 actor as-this-one
    matthew 150 7 talk
    this-one 151 0 talk
    matthew 152 4 talk
    this-one 153 0 talk
    matthew 154 3 talk
    this-one 155 0 talk
    end-script
    ;

\ $80104960
: entry-80104960
    17 actor as-this-one
    this-one 166 7 talk
    matthew 167 7 talk
    this-one 168 3 talk
    end-script
    ;

\ $801049F0
: entry-801049F0
    -512 -2486 -362 178898 -5600 425070 6166 camera-set
    28 door-animate
    events.main:sub-80096CF0
    1 set-scene
    17 actor 178279 -1360 424892 set-pos
    17 actor 1 ride
    end-script
    ;

\ $80104A4C
: entry-80104A4C
    17 actor unride
    17 actor 178279 -1360 424892 set-pos
    17 actor 0 1028 0 set-rot
    -590 -5474 -476 180290 -5588 425052 5028 camera-set
    28 door-animate
    events.main:sub-80096CF0
    2 set-scene
    17 actor 578279 -1360 424892 set-pos
    end-script
    ;

\ $80104ACC
: entry-80104ACC
    17 actor unride
    xa-stop
    17 actor place-at-stop
    -470 -1292 -2216 175644 -4144 423399 5841 camera-set
    27 door-animate
    events.main:sub-80096CF0
    27 -1 175699 423178 2048 0 warp
    ;

: L-80104B30
    -488 -1538 -1610 178490 -5624 424042 1204 camera-set
    17 actor 178536 -1360 423752 set-pos
    17 actor 29 3 through-door
    17 actor 1 ride
    end-script
    ;

\ $80104B84
: entry-80104B84
    -566 -2630 -368 178635 -5600 423255 3852 camera-set
    29 door-animate
    events.main:sub-80096CF0
    1 set-scene
    17 actor unride
    ['] L-80104B30 start-event
    matthew 30 walk-forward
    10 frames wait
    matthew idle
    end-script
    ;

: L-80104BEC
    -284 -1382 -1676 178530 -5902 423662 -519 camera-set
    17 actor 178625 -1360 424058 set-pos
    17 actor 29 1 through-door
    17 actor 178242 423587 walk-to
    17 actor wait-stop
    17 actor 0 -80 face
    end-script
    ;

\ $80104C5C
: entry-80104C5C
    17 actor 178925 -1360 424105 set-pos
    17 actor 0 2746 0 set-rot
    -446 -2306 -224 178275 -5564 424180 2806 camera-set
    29 door-animate
    events.main:sub-80096CF0
    3 set-scene
    17 actor unride
    ['] L-80104BEC start-event
    matthew 30 walk-forward
    10 frames wait
    matthew idle
    end-script
    ;

: L-80104CEC
    17 actor 175387 424838 walk-to
    17 actor wait-stop
    17 actor 1024 80 turn-to
    17 actor wait-turned
    17 actor 10 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 11 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 11 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 11 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 11 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 11 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 11 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 11 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 11 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 13 0 anim
    17 actor anim-hold
    17 actor wait-anim
    matthew 3 2 set-floor
    17 actor 3 2 set-floor
    17 actor 1 ride
    end-script
    ;

\ $80104E8C
: entry-80104E8C
    17 actor unride
    17 actor 174650 0 424878 set-pos
    matthew 175387 0 424838 set-pos
    matthew 0 1024 0 set-rot
    -398 -2054 -32 174978 -4240 424494 5136 camera-set
    matthew 10 0 anim
    matthew anim-hold
    ['] L-80104CEC start-event
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    1 set-scene
    -434 -2126 -8 177888 -5588 424702 -702 camera-set
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 13 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 30 walk-forward
    15 frames wait
    matthew idle
    end-script
    ;

: L-80105058
    17 actor 177734 424838 walk-to
    17 actor wait-stop
    17 actor 3072 80 turn-to
    17 actor wait-turned
    17 actor 6 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 7 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 7 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 7 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 7 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 7 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 7 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 7 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 7 0 anim
    17 actor anim-hold
    17 actor wait-anim
    17 actor 9 0 anim
    17 actor anim-hold
    17 actor wait-anim
    matthew 3 1 set-floor
    17 actor 3 1 set-floor
    17 actor 1 ride
    end-script
    ;

\ $801051F8
: entry-801051F8
    17 actor unride
    17 actor 178238 -1360 424878 set-pos
    matthew 177734 -1360 424872 set-pos
    matthew 0 3072 0 set-rot
    -434 -2126 -8 177888 -5588 424702 -702 camera-set
    matthew 6 0 anim
    matthew anim-hold
    ['] L-80105058 start-event
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    0 set-scene
    -398 -2054 -32 174978 -4240 424494 5136 camera-set
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 9 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 30 walk-forward
    15 frames wait
    matthew idle
    end-script
    ;

\ $801053C4
: entry-801053C4
    17 actor as-this-one
    matthew 32 0 talk
    matthew 33 7 talk
    this-one 34 0 talk
    this-one 35 4 talk
    matthew 36 0 talk
    end-script
    ;

\ $8010540C
: entry-8010540C
    17 actor as-this-one
    matthew 37 0 talk
    this-one 38 0 talk
    matthew 39 0 talk
    this-one 40 0 talk
    matthew 41 0 talk
    this-one 42 0 talk
    matthew 43 0 talk
    matthew 44 0 talk
    this-one 45 0 talk
    end-script
    ;

\ $80105484
: entry-80105484
    -476 -1778 -1370 178664 -5554 423573 -727 camera-set
    matthew 907 908 anim-at  matthew wait-anim
    10 matthew say-by
    wait-text
    matthew 909 0 anim-at  matthew wait-anim
    end-script
    ;

\ $801054D8
: entry-801054D8
    -476 -1778 -1370 178664 -5554 423573 -727 camera-set
    matthew 907 908 anim-at  matthew wait-anim
    11 matthew say-by
    wait-text
    12 matthew say-by
    wait-text
    matthew 909 0 anim-at  matthew wait-anim
    end-script
    ;

: L-8010553C
    picture-hide
    matthew 39 0 anim
    matthew wait-anim
    matthew 4294967295 actor ignore-a
    matthew 37 0 anim
    1 frames wait
    19 actor 179011 -5900 423740 set-pos
    19 actor -5900 set-y
    matthew wait-anim
    matthew 40 42 0 anim-then
    matthew wait-anim
    matthew 51 1 anim
    18 matthew say-by
    wait-text
    19 17 actor say-by
    17 actor 83 84 1 anim-then
    30 frames wait
    wait-text
    20 17 actor say-by
    17 actor 85 86 0 anim-then
    wait-text
    21 matthew say-by
    wait-text
    22 17 actor say-by
    17 actor 88 0 anim
    wait-text
    23 matthew say-by
    wait-text
    24 matthew say-by
    wait-text
    matthew 55 0 anim
    matthew wait-anim
    6 picture-show
    5 3 say-mode-wait
    picture-hide
    ovl003 $80102A80 native
    end-script
    ;

: L-801056AC
    picture-hide
    matthew 39 0 anim
    matthew wait-anim
    matthew 4294967295 actor ignore-a
    matthew 37 0 anim
    1 frames wait
    19 actor 179011 -5900 423740 set-pos
    19 actor -5900 set-y
    matthew wait-anim
    end-script
    ;

\ $80105710
: entry-80105710
    matthew 3 2 set-floor
    matthew 179022 -1360 423577 set-pos
    matthew 0 1 anim
    matthew 0 0 0 set-rot
    17 actor 3 2 set-floor
    17 actor 178242 -1360 423587 set-pos
    17 actor 0 0 face
    -476 -1778 -1370 178664 -5554 423573 -727 camera-set
    matthew 36 0 anim
    matthew wait-anim
    matthew 19 actor ignore-a
    matthew 38 51 anim-at  matthew wait-anim
    15 32767 actor say-by
    wait-text
    5 frames wait
    16 32767 actor say-by
    3 frames wait
    wait-text
    6 picture-show
    17 0 choice
    0 result= if  goto L-8010553C  then
    1 result= if  goto L-801056AC  then
    end-script
    ;

\ $8010583C
: entry-8010583C
    7 5 say-mode-wait
    end-script
    ;

\ $8010584C
: entry-8010584C
    25 5 say-mode-wait
    end-script
    ;

\ $8010585C
: entry-8010585C
    matthew 178689 0 425496 set-pos
    matthew 0 3086 0 set-rot
    matthew 0 1 anim
    -476 -974 -968 178639 -5588 425486 2438 camera-set
    matthew 40 42 0 anim-then
    matthew wait-anim
    matthew 51 1 anim
    10 frames wait
    18 picture-show
    26 5 say-mode-wait
    27 5 say-mode-wait
    picture-hide
    3 screen-effect
    wait-effect
    1 set-scene
    matthew 178749 0 424798 set-pos
    matthew 0 3230 0 set-rot
    matthew 0 1 anim
    17 actor 178371 0 424845 set-pos
    17 actor 0 1105 0 set-rot
    17 actor 0 1 anim
    -470 -2120 -2372 178579 -5432 424960 3704 camera-set
    2 screen-effect
    wait-effect
    matthew 59 60 anim-at  matthew wait-anim
    matthew 28 0 talk
    17 actor 29 0 talk
    matthew 30 0 talk
    17 actor 31 0 talk
    matthew 61 0 anim-at  matthew wait-anim
    matthew 134 137 0 anim-then
    matthew wait-anim
    18 picture-show
    6 3 say-mode-wait
    picture-hide
    matthew 87 0 anim-at  matthew wait-anim
    17 actor 1 ride
    end-script
    ;

\ $80105A60
: entry-80105A60
    8 5 say-mode-wait
    end-script
    ;

\ $80105A70
: entry-80105A70
    9 5 say-mode-wait
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' arrive-80102DEC $80102DEC 3 overlay-word!
' arrive-80103A68 $80103A68 3 overlay-word!
