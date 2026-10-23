\ ovl013.fs - area 013 overlay (CD.BIN 013_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl013
USING: events events.main ;

\ (looped back to: declared first)
defer L-8010387C

\ $801028FC
: entry-801028FC
    171 actor as-this-one
    this-one 268544 0 91092 set-pos
    this-one 13 3 set-floor
    this-one 1044 0 turn-to
    this-one 167 1 anim
    end-script
    ;

\ $8010293C
: entry-8010293C
    171 actor as-this-one
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80102958
: entry-80102958
    171 actor as-this-one
    goto entry-801028FC
    ;

\ $80102968
: entry-80102968
    171 actor as-this-one
    goto entry-8010293C
    ;

\ $801029F0
: entry-801029F0
    11 screen-effect
    wait-effect
    39 minigame
    end-script
    ;

: L-801029D8
    this-one 108 1 talk
    events.main:sub-80096D34
    end-script
    ;

\ $80102A08
: entry-80102A08
    171 actor as-this-one
    this-one 105 0 talk
    matthew 105 0 talk
    events.main:sub-80096D74
    this-one 106 5 talk
    107 1 choice
    0 result= if  goto L-801029D8  then
    11 screen-effect
    wait-effect
    39 minigame
    end-script
    ;

\ $80102A6C
: entry-80102A6C
    171 actor 10021 0 talk
    end-script
    ;

\ $80102A80
: entry-80102A80
    11 screen-effect
    10 frames wait
    27 -1 278626 98702 512 0 warp
    ;

: L-80102AAC
    winona 48 4 talk
    matthew 86 0 anim
    winona 49 7 talk
    winona 50 6 talk
    matthew 51 3 talk
    10 frames wait
    9 screen-effect
    wait-effect
    27 -1 278480 98750 512 -2146387080 warp
    ;

: L-80102B2C
    winona 31 0 talk
    winona 2908 0 anim-at  winona wait-anim
    -602 -1850 -224 272411 -1776 90581 3434 camera-set
    matthew 2900 2901 1 anim-then
    winona 2903 2904 1 anim-then
    winona anim-hold
    20 50 rumble
    20 frames wait
    5 music
    -710 -554 -1034 272411 -1776 90581 1994 camera-set
    30 frames wait
    39 winona say-by
    wait-text
    10 frames wait
    40 matthew say-by
    wait-text
    41 32767 actor 1 say-timed-by-wait
    text-hold
    42 32767 actor 1 say-timed-by-wait
    text-hold
    43 32767 actor 1 say-timed-by-wait
    text-hold
    44 32767 actor 1 say-timed-by-wait
    text-close
    matthew 34 0 talk
    matthew 35 0 talk
    winona 36 0 talk
    matthew 2902 0 anim
    winona 2905 0 anim
    winona anim-hold
    -710 -1148 -440 272411 -1776 90707 3122 camera-set
    matthew wait-anim
    matthew 272367 90767 2 20 walk-to-ex
    matthew wait-stop
    winona 45 4 talk
    matthew 46 7 talk
    winona 47 7 talk
    20 frames wait
    ['] L-80102AAC start-event
    end-script
    ;

: L-80102D30
    winona 31 0 talk
    matthew 32 7 talk
    winona 2908 0 anim-at  winona wait-anim
    winona 33 0 talk
    matthew 34 3 talk
    matthew 35 4 talk
    winona 36 7 talk
    winona 37 3 talk
    matthew 38 0 talk
    ['] L-80102AAC start-event
    end-script
    ;

\ $80102DCC
: entry-80102DCC
    -1 music
    matthew 13 3 set-floor
    matthew 272823 -1920 93361 set-pos
    matthew 15 0 face
    winona 13 3 set-floor
    winona 271715 -1920 90306 set-pos
    winona 0 1536 0 set-rot
    winona 0 1 anim
    2 set-scene
    -122 -3812 -728 273206 -2460 91820 -639 camera-set
    272018 -2460 90392 20 camera-move
    matthew 272183 91061 walk-to
    30 frames wait
    winona 272025 90776 walk-to
    30 frames wait
    0 winona say-by
    wait-text
    matthew wait-stop
    matthew 15 80 face
    1 matthew say-by
    matthew 88 0 anim
    wait-text
    2 winona say-by
    winona 89 0 anim
    wait-text
    20 frames wait
    winona 83 84 1 anim-then
    30 frames wait
    3 matthew say-by
    matthew 90 0 anim
    wait-text
    winona 85 4 0 anim-then
    winona 90 80 turn-by
    4 winona say-by
    wait-text
    5 matthew say-by
    matthew 90 0 anim
    wait-text
    4 winona say-by
    wait-text
    6 winona say-by
    wait-text
    7 matthew say-by
    matthew 82 0 anim
    wait-text
    8 winona say-by
    winona 86 0 anim
    wait-text
    9 matthew say-by
    matthew 90 0 anim
    wait-text
    -578 -2210 -506 272333 -1836 90629 3218 camera-set
    winona 272465 90456 walk-to
    20 frames wait
    matthew 17 18 1 anim-then
    10 winona say-by
    winona 86 0 anim
    wait-text
    11 matthew say-by
    winona 83 84 1 anim-then
    wait-text
    12 winona say-by
    wait-text
    13 matthew say-by
    matthew 19 0 anim
    matthew 15 -80 face
    wait-text
    winona 85 0 anim
    14 5 say-mode-wait
    15 winona say-by
    winona 86 0 anim
    wait-text
    16 winona say-by
    winona 0 -80 face
    winona 90 0 anim
    wait-text
    17 winona say-by
    winona 89 0 anim
    wait-text
    18 winona say-by
    winona 90 0 anim
    wait-text
    19 winona say-by
    winona 86 0 anim
    wait-text
    11 matthew say-by
    wait-text
    21 winona say-by
    winona 89 0 anim
    wait-text
    22 winona say-by
    winona 86 0 anim
    wait-text
    11 matthew say-by
    wait-text
    winona 2906 2907 anim-at  winona wait-anim
    24 winona say-by
    wait-text
    25 winona say-by
    wait-text
    26 winona say-by
    wait-text
    27 winona say-by
    wait-text
    28 winona say-by
    wait-text
    matthew 20 walk-forward
    23 frames wait
    matthew idle
    23 matthew say-by
    wait-text
    5 24 prompt
    0 result= if  goto L-80102B2C  then
    1 result= if  goto L-80102D30  then
    end-script
    ;

\ $801032F0
: entry-801032F0
    matthew 278140 0 98409 set-pos
    matthew 0 2560 0 set-rot
    2 set-scene
    220 -7526 -152 276997 -1248 97200 2572 camera-set
    2 screen-effect
    wait-effect
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
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 12 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 13 2 set-floor
    end-script
    ;

\ $80103424
: entry-80103424
    matthew 276912 -960 97231 set-pos
    matthew 0 512 0 set-rot
    2 set-scene
    220 -7526 -152 276997 -1248 97200 2572 camera-set
    matthew 6 0 anim
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
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 8 0 anim
    matthew anim-hold
    matthew wait-anim
    11 screen-effect
    10 frames wait
    27 -1 278626 98702 512 0 warp
    ;

\ $80103568
: entry-80103568
    matthew 274734 -960 94946 set-pos
    matthew 0 2560 0 set-rot
    2 set-scene
    220 -7526 -152 276997 -1248 97200 2572 camera-set
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
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 12 0 anim
    matthew anim-hold
    matthew wait-anim
    2 set-scene
    matthew 13 3 set-floor
    ovl013 $80102568 native
    end-script
    ;

\ $801036A0
: entry-801036A0
    2 set-scene
    matthew 273511 -1920 93700 set-pos
    matthew 0 512 0 set-rot
    220 -7526 -152 276997 -1248 97200 2572 camera-set
    matthew 6 0 anim
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
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 8 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 13 2 set-floor
    1 set-scene
    end-script
    ;

: L-80103830
    this-one 81 5 talk
    end-script
    ;

: L-80103838
    -480 -1106 -460 270694 -1920 90639 3296 camera-set
    this-one 83 3 talk
    82 0 choice
    0 result= if  goto L-8010387C  then
    this-one 81 5 talk
    end-script
    ;

:noname
    11 screen-effect
    wait-effect
    171 actor 8 matthew 0 fight
    13 music
    -608 -2802 -652 271176 -1920 90263 4128 camera-set
    this-one 0 0 anim
    this-one 270806 -1920 90557 set-pos
    matthew 0 0 anim
    matthew 271430 -1920 90557 set-pos
    this-one matthew face-each-other
    10 screen-effect
    wait-effect
    main-exe $80037928 native-result
    0 result= if  goto L-80103838  then
    ovl013 $8010274C native
    this-one 84 7 talk
    end-script
    ; is L-8010387C

\ $80103938
: entry-80103938
    171 actor as-this-one
    this-one 0 1 anim
    this-one 270806 -1920 90557 set-pos
    matthew 271430 -1920 90557 set-pos
    this-one 13 3 set-floor
    -608 -2802 -652 271176 -1920 90263 4128 camera-set
    this-one matthew face-each-other
    10 screen-effect
    wait-effect
    this-one 52 5 talk
    matthew 53 1 talk
    80 0 choice
    1 result= if  goto L-80103830  then
    this-one 54 7 talk
    matthew 55 1 talk
    this-one 56 5 talk
    matthew 57 7 talk
    this-one 58 0 talk
    this-one 117 116 anim-at  this-one wait-anim
    this-one 59 0 talk
    this-one 105 116 anim-at  this-one wait-anim
    this-one 60 0 talk
    this-one 157 116 anim-at  this-one wait-anim
    this-one 61 0 talk
    this-one 147 116 anim-at  this-one wait-anim
    this-one 62 0 talk
    this-one 107 116 anim-at  this-one wait-anim
    this-one 63 0 talk
    this-one 158 116 anim-at  this-one wait-anim
    this-one 64 0 talk
    goto L-8010387C
    ;

\ $80103A64
: entry-80103A64
    171 actor as-this-one
    this-one 0 1 anim
    this-one 270806 -1920 90557 set-pos
    matthew 271430 -1920 90557 set-pos
    this-one 13 3 set-floor
    -608 -2802 -652 271176 -1920 90263 4128 camera-set
    this-one matthew face-each-other
    10 screen-effect
    wait-effect
    this-one 65 0 talk
    80 0 choice
    1 result= if  goto L-80103830  then
    this-one 66 0 talk
    this-one 106 116 anim-at  this-one wait-anim
    this-one 67 0 talk
    this-one 139 116 anim-at  this-one wait-anim
    this-one 68 0 talk
    this-one 144 116 anim-at  this-one wait-anim
    this-one 69 0 talk
    this-one 146 116 anim-at  this-one wait-anim
    this-one 70 0 talk
    goto L-8010387C
    ;

\ $80103B54
: entry-80103B54
    171 actor as-this-one
    this-one 0 1 anim
    this-one 270806 -1920 90557 set-pos
    matthew 271430 -1920 90557 set-pos
    this-one 13 3 set-floor
    -608 -2802 -652 271176 -1920 90263 4128 camera-set
    this-one matthew face-each-other
    10 screen-effect
    wait-effect
    this-one 71 4 talk
    80 0 choice
    1 result= if  goto L-80103830  then
    this-one 72 5 talk
    matthew 73 3 talk
    this-one 74 7 talk
    this-one 75 7 talk
    this-one 117 0 anim
    this-one wait-anim
    this-one 105 0 anim
    4 frames wait
    this-one 107 0 anim
    8 frames wait
    this-one 106 0 anim
    this-one wait-anim
    this-one 76 7 talk
    goto L-8010387C
    ;

: L-80103C4C
    11 screen-effect
    wait-effect
    8 0 -1 -1 -1 -2146421132 warp
    ;

\ $80103C74
: entry-80103C74
    171 actor as-this-one
    this-one 0 1 anim
    this-one 270806 -1920 90557 set-pos
    matthew 271430 -1920 90557 set-pos
    this-one 13 3 set-floor
    -608 -2802 -652 271176 -1920 90263 4128 camera-set
    this-one matthew face-each-other
    10 screen-effect
    wait-effect
    this-one 77 7 talk
    78 0 choice
    0 result= if  goto L-80103C4C  then
    79 0 choice
    0 result= if  goto L-8010387C  then
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' entry-801032F0 $801032F0 13 overlay-word!
