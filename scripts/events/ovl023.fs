\ ovl023.fs - area 023 overlay (CD.BIN 023_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl023
USING: events events.main ;

\ $801024F0
: entry-801024F0
    take-control
    172 -2738 -896 36440 -1200 275392 -5104 camera-set
    34 say-wait-close
    matthew 37555 275483 run-to
    matthew wait-stop
    -350 -2738 -1652 37244 -1200 275566 -4900 camera-set
    matthew -1462 -200 turn-to
    matthew 150 152 1 anim-then
    2 frames wait
    35 matthew say-by-wait
    9 screen-effect
    wait-effect
    29 0 101236 356311 2476 -2146416288 warp
    ;

\ $801025A8
: arrive-801025A8
    cohen 37436 -1200 275318 set-pos
    cohen -2260 0 turn-to
    cohen 23 2 set-floor
    cohen 109 1 anim
    2 screen-effect
    matthew 38751 0 277050 set-pos
    matthew -5126 0 turn-to
    -638 -758 -644 38648 0 276991 -1082 camera-set
    matthew 81 1 anim
    40 frames wait
    matthew 30 6 talk
    matthew 17 18 1 anim-then
    -2090 -40 camera-angle
    wait-camera-turn
    10 frames wait
    -266 40 camera-angle
    matthew 19 0 anim
    matthew wait-anim
    matthew 14 15 1 anim-then
    wait-camera-turn
    31 5 say-mode-wait
    -1538 -20 camera-angle
    wait-camera-turn
    8 frames wait
    111 sound
    16 frames wait
    107 sound
    -1298 100 camera-angle
    -248 100 camera-height-to
    wait-camera-height
    -512 -362 -338 38751 0 277194 -5342 camera-set
    32 say-wait-close
    end-script
    ;

\ $8010271C
: entry-8010271C
    -512 -2378 -1022 37158 -1200 275135 -1716 camera-set
    matthew 166 actor ignore-a
    matthew 42 0 anim
    matthew wait-anim
    matthew 0 1 anim
    23 picture-show
    9 5 say-mode-wait
    10 5 say-mode-wait
    11 5 say-mode-wait
    12 3 say-mode-wait
    picture-hide
    matthew 4294967295 actor ignore-a
    166 actor 0 0 0 set-pos
    end-script
    ;

\ $801027C0
: entry-801027C0
    matthew 150 151 anim-at  matthew wait-anim
    5 matthew say-by-wait
    6 matthew say-by-wait
    matthew 152 0 1 anim-then
    165 actor 39115 -1200 279163 set-pos
    165 actor 23 2 set-floor
    matthew 165 actor ignore-a
    end-script
    ;

\ $80102824
: entry-80102824
    0 matthew say-by-wait
    -26 -3530 -680 35962 -1188 275643 3192 camera-set
    17 music
    1 matthew say-by-wait
    2 matthew say-by-wait
    3 matthew say-by-wait
    end-script
    ;

\ $80102880
: entry-80102880
    -656 -2594 -1256 39088 -1200 275691 -1518 camera-set
    matthew 39088 0 275691 set-pos
    matthew -990 0 turn-to
    matthew 4900 0 anim
    matthew wait-anim
    7 matthew say-by-wait
    end-script
    ;

\ $801028DC
: entry-801028DC
    -656 -2594 -1256 39088 -1200 275691 -1518 camera-set
    matthew 39088 0 275691 set-pos
    matthew -990 0 turn-to
    matthew 4900 0 anim
    matthew wait-anim
    101 0 sequence-play
    8 matthew say-by-wait
    matthew 4294967295 actor ignore-a
    end-script
    ;

\ $80102980
: entry-80102980
    4 say-wait-close
    end-script
    ;

\ $8010298C
: arrive-8010298C
    cohen as-this-one
    matthew 36889 0 277706 set-pos
    matthew 0 1024 0 set-rot
    matthew 74 1 anim
    this-one 37654 0 276891 set-pos
    this-one 0 1 anim
    this-one 0 0 face
    -350 -3350 -1310 37513 24 277424 1616 camera-set
    2 screen-effect
    wait-effect
    10 frames wait
    this-one 13 7 talk
    matthew 14 0 talk
    this-one 87 0 anim
    this-one wait-anim
    this-one 37654 277481 walk-to
    this-one wait-stop
    matthew 75 72 1 anim-then
    this-one 0 3072 0 set-rot
    this-one 70 72 1 anim-then
    20 frames wait
    this-one 15 0 talk
    this-one 16 8 talk
    this-one 17 0 talk
    this-one 73 74 1 anim-then
    this-one 18 0 talk
    matthew 19 8 talk
    this-one 75 72 1 anim-then
    this-one 20 0 talk
    this-one 21 8 talk
    matthew 22 8 talk
    this-one 23 0 talk
    10 frames wait
    matthew 24 8 talk
    this-one 73 74 1 anim-then
    this-one 25 0 talk
    matthew 26 8 talk
    this-one 75 72 1 anim-then
    this-one 27 0 talk
    this-one 28 8 talk
    10 frames wait
    29 5 say-mode-wait
    10 frames wait
    11 screen-effect
    wait-effect
    5 frames wait
    27 -1 39657 277167 3072 -2146387956 warp
    ;

\ $80102C48
: entry-80102C48
    -836 -5762 -1112 37737 0 277087 5782 camera-set
    86 door-animate
    events.main:sub-80096CF0
    27 -1 39209 277195 1024 0 warp
    ;

\ $80102C98
: entry-80102C98
    82 -5222 -728 38516 -1200 276520 1368 camera-set
    87 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80102CD4
: entry-80102CD4
    -314 -5180 -1364 39220 -1200 277112 3200 camera-set
    87 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80102D10
: entry-80102D10
    matthew 2028 0 turn-to
    matthew 62 0 anim
    matthew wait-anim
    33 5 say-mode-wait
    end-script
    ;

\ $80102D3C
: entry-80102D3C
    end-script
    ;

\ $80102D40
: entry-80102D40
    -422 -2234 -1544 39040 -1200 275498 3074 camera-set
    89 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $80102D7C
: entry-80102D7C
    89 door-animate-alt
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80102D98
: entry-80102D98
    matthew 35849 0 278811 set-pos
    matthew 0 2048 0 set-rot
    -536 -2738 -104 35859 0 278851 2039 camera-set
    matthew 10 0 anim
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
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    -2546 -872 -3020 37197 36 275557 3575 camera-set
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
    matthew 13 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 23 2 set-floor
    end-script
    ;

\ $80102F18
: entry-80102F18
    matthew 35856 -1200 276660 set-pos
    matthew 0 0 0 set-rot
    -2546 -872 -3020 37197 36 275557 3575 camera-set
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    0 set-scene
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
    -536 -2738 -104 35859 0 278851 2039 camera-set
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
    matthew 23 1 set-floor
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' arrive-801025A8 $801025A8 23 overlay-word!
' arrive-8010298C $8010298C 23 overlay-word!
