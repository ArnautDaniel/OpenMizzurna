\ ovl012.fs - area 012 overlay (CD.BIN 012_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl012
USING: events events.main ;

\ (looped back to: declared first)
defer entry-80106954

\ $80099120
: entry-80099120
    give-control
    99 actor 50 0 through-door
    99 actor 466718 0 289033 set-pos
    99 actor 0 0 turn-to
    end-script
    ;

\ $80099154
: entry-80099154
    give-control
    99 actor 52 0 through-door
    99 actor 0 0 0 set-pos
    end-script
    ;

\ $8009917C
: entry-8009917C
    give-control
    112 actor 48 -2 through-door
    112 actor 468387 0 292373 set-pos
    112 actor 467728 291616 walk-to  112 actor wait-stop
    112 actor 465632 291744 walk-to  112 actor wait-stop
    112 actor 465088 291520 walk-to  112 actor wait-stop
    112 actor 3072 0 turn-to
    112 actor 0 1 anim
    ovl012 $80104F0C native
    end-script
    ;

\ $800991DC
: entry-800991DC
    give-control
    112 actor 465632 291744 walk-to  112 actor wait-stop
    112 actor 467728 291616 walk-to  112 actor wait-stop
    112 actor 468384 292368 walk-to  112 actor wait-stop
    112 actor 0 0 0 set-pos
    end-script
    ;

\ $80105094
: entry-80105094
    matthew as-this-one
    this-one 467137 0 289865 set-pos
    wolf this-one ignore-a
    wolf 0 0 turn-to
    wolf 2715 0 anim
    wolf wait-anim
    wolf 4294967295 actor ignore-a
    ovl012 $801037E8 native
    wolf 2716 0 anim
    wolf wait-anim
    298 wolf say-by
    wait-text
    end-script
    ;

\ $80105124
: entry-80105124
    matthew as-this-one
    wolf 0 0 turn-to
    wolf this-one ignore-a
    wolf 2716 0 anim
    wolf wait-anim
    wolf 4294967295 actor ignore-a
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80105184
: entry-80105184
    matthew as-this-one
    matthew 2717 0 anim
    matthew wait-anim
    matthew this-one ignore-a
    matthew 2733 0 anim
    matthew wait-anim
    matthew 2733 0 anim
    matthew wait-anim
    matthew 4294967295 actor ignore-a
    ovl012 $801037E8 native
    matthew 2734 0 anim
    matthew wait-anim
    matthew 119 1 anim
    end-script
    ;

\ $80105210
: entry-80105210
    matthew as-this-one
    matthew 2717 0 anim
    matthew wait-anim
    matthew 2733 0 anim
    matthew wait-anim
    matthew 2733 0 anim
    matthew wait-anim
    ovl012 $801037E8 native
    matthew 2734 0 anim
    matthew wait-anim
    matthew 119 1 anim
    end-script
    ;

: L-8010528C
    matthew 342 6 talk
    5 frames wait
    3 screen-effect
    25 frames wait
    8 actor place-at-stop
    27 -1 468285 292835 0 0 warp
    ;

\ $801052D0
: entry-801052D0
    341 say-wait-close
    events.main:sub-80096E74
    goto L-8010528C
    ;

\ $8010533C
: entry-8010533C
    99 actor as-this-one
    this-one 343 0 talk
    end-script
    ;

: L-80105498
    34 mel 3 say-timed-by-wait
    text-hold
    text-close
    end-script
    ;

: L-8010535C
    mel as-this-one
    matthew 467918 0 290224 set-pos
    matthew 0 724 0 set-rot
    matthew 116 1 anim
    this-one 468464 1 290414 set-pos
    this-one 0 -1234 0 set-rot
    this-one 24 1 anim
    -416 -1904 -968 467990 0 290240 1408 camera-set
    2 screen-effect
    wait-effect
    16 sound
    10 frames wait
    16 sound
    matthew 118 0 anim
    10 frames wait
    16 sound
    this-one 25 0 anim
    this-one wait-anim
    this-one 2738 2739 1 anim-then
    this-one 35 0 talk
    this-one 2740 0 anim
    this-one wait-anim
    -1 music
    mel -2146930096 ending-schedule
    end-script
    ;

: L-801054B4
    -68 -2186 -1220 468510 -48 289761 1765 camera-set
    matthew 106 0 anim
    7 frames wait
    116 sound
    121 sound
    mel 113 114 1 anim-then
    mel 468814 0 290204 -1 50 walk-path
    matthew wait-anim
    matthew 116 1 anim
    20 frames wait
    mel 115 0 anim
    ['] L-80105498 start-event
    mel wait-anim
    mel 116 1 anim
    3 screen-effect
    wait-effect
    matthew 467598 0 290240 set-pos
    mel 468598 0 290240 set-pos
    mel 2 matthew 0 fight
    ['] L-8010535C start-event
    end-script
    ;

: L-801055CC
    33 mel 3 say-timed-by-wait
    text-hold
    text-close
    end-script
    ;

: L-801055E8
    -68 -2636 -1106 468270 -96 289641 1813 camera-set
    ['] L-801055CC start-event
    mel 106 0 anim
    7 frames wait
    116 sound
    121 sound
    256 10 rumble
    200 10 rumble
    matthew 113 114 1 anim-then
    matthew 467770 0 290204 -1 50 walk-path
    mel wait-anim
    mel 116 1 anim
    20 frames wait
    matthew 115 0 anim
    matthew wait-anim
    matthew 116 1 anim
    3 screen-effect
    wait-effect
    matthew 467598 0 290240 set-pos
    mel 468598 0 290240 set-pos
    mel 2 matthew 0 fight
    ['] L-8010535C start-event
    end-script
    ;

\ $80105718
: entry-80105718
    mel as-this-one
    this-one 468337 0 289865 set-pos
    this-one 0 2048 0 set-rot
    this-one 119 1 anim
    matthew 468032 0 290125 set-pos
    matthew 1651 0 turn-to
    matthew 0 1 anim
    101 actor 469492 0 290765 set-pos
    -674 -1262 -1178 468086 -12 290407 1858 camera-set
    20 frames wait
    this-one 11 0 talk
    this-one 13 10 talk
    this-one 2704 2717 0 anim-then
    this-one wait-anim
    this-one 2733 2734 0 anim-then
    this-one wait-anim
    this-one 119 1 anim
    matthew 468152 290025 walk-to
    14 101 actor say-by
    matthew 17 18 1 anim-then
    1634 -10 camera-angle
    101 actor 468682 290095 walk-to
    this-one 2709 2710 1 anim-then
    wait-text
    this-one leave-club-seat
    12 mel say-by
    wait-text
    101 actor 15 5 talk
    101 actor 16 3 talk
    this-one 2711 119 1 anim-then
    this-one 17 0 talk
    101 actor 18 3 talk
    15 music
    19 mel say-by
    this-one 2726 0 anim
    this-one anim-hold
    this-one wait-anim
    -1160 -452 -1346 469868 132 290029 3074 camera-set
    this-one 101 -150 face
    wait-text
    101 actor 20 1 talk
    101 actor 21 4 talk
    matthew 19 0 anim
    matthew 22 7 talk
    101 actor 14 15 1 anim-then
    23 101 actor say-by
    10 frames wait
    101 actor 16 89 0 anim-then
    wait-text
    101 actor 24 4 talk
    this-one 2738 2739 1 anim-then
    this-one 25 0 talk
    this-one 2740 0 anim
    101 actor 86 0 anim
    101 actor wait-anim
    101 actor 468402 290435 walk-to
    101 actor wait-stop
    101 actor 14 15 1 anim-then
    26 101 actor say-by
    wait-text
    101 actor 16 0 anim
    101 actor 468532 290925 walk-to
    this-one 468483 290204 walk-to
    this-one wait-stop
    this-one 17 0 anim
    this-one wait-anim
    matthew 468233 0 290204 set-pos
    matthew 13 0 face
    this-one 0 0 face
    101 actor 0 0 0 set-pos
    -380 -1718 -1472 468384 -12 290151 1645 camera-set
    this-one 27 7 talk
    101 actor 0 0 0 set-pos
    matthew 28 5 talk
    this-one 29 1 talk
    matthew 30 5 talk
    this-one 31 5 talk
    this-one 2736 0 anim
    this-one anim-hold
    matthew 2737 0 anim
    matthew anim-hold
    this-one 32 0 talk
    matthew wait-anim
    -590 -1820 -122 468546 -24 289737 1693 camera-set
    5 frames wait
    2 23 prompt
    0 result= if  goto L-801054B4  then
    1 result= if  goto L-801055E8  then
    end-script
    ;

: L-80105C18
    matthew 468124 0 289888 set-pos
    matthew 0 1 anim
    matthew 0 1938 0 set-rot
    wolf 468224 0 289227 set-pos
    wolf 0 4015 0 set-rot
    -170 -1274 -68 468276 -324 289794 2423 camera-set
    2 screen-effect
    wait-effect
    wolf 2718 2719 1 anim-then
    20 frames wait
    matthew 48 7 talk
    wolf 49 0 talk
    wolf 2721 2719 1 anim-then
    wolf 50 0 talk
    matthew 90 0 anim
    matthew wait-anim
    matthew 468084 289848 walk-to
    matthew wait-stop
    matthew 0 2048 0 set-rot
    matthew 2725 119 anim-at  matthew wait-anim
    matthew 467934 0 289848 set-pos
    matthew 0 2048 0 set-rot
    wolf 467934 0 289177 set-pos
    wolf 0 4015 0 set-rot
    -206 -1478 -728 468030 -324 289560 2519 camera-set
    matthew 2700 119 anim-at  matthew wait-anim
    51 wolf say-by
    wait-text
    52 matthew say-by
    wait-text
    -206 -1478 -728 467796 -108 288510 2183 camera-set
    0 1 say-mode-wait
    1 1 say-mode-wait
    2 1 say-mode-wait
    53 wolf say-by
    wait-text
    -206 -1478 -728 468030 -324 289560 2519 camera-set
    matthew 2700 119 anim-at  matthew wait-anim
    54 matthew say-by
    wait-text
    55 wolf say-by
    wolf 88 0 anim
    wait-text
    wolf 86 0 anim
    wolf wait-anim
    wolf 469129 289177 120 face-point
    5 frames wait
    wolf 30 walk-forward
    40 frames wait
    matthew 2709 2710 1 anim-then
    wolf idle
    wolf wait-stop
    wolf 467994 289177 walk-to
    wolf wait-stop
    wolf 467934 289297 walk-to
    matthew 2711 119 1 anim-then
    wolf wait-stop
    wolf 0 120 face
    wolf wait-turned
    wolf 2715 2716 0 anim-then
    56 matthew say-by
    wait-text
    57 wolf say-by
    wait-text
    matthew 2717 119 anim-at  matthew wait-anim
    58 matthew say-by
    matthew 2701 119 1 anim-then
    wait-text
    59 wolf say-by
    wait-text
    60 matthew say-by
    wait-text
    61 wolf say-by
    wait-text
    62 wolf say-by
    wait-text
    63 matthew say-by
    wait-text
    64 wolf say-by
    wait-text
    65 wolf say-by
    wait-text
    66 matthew say-by
    wait-text
    67 wolf say-by
    wait-text
    68 matthew say-by
    wait-text
    69 wolf say-by
    wait-text
    70 wolf say-by
    wait-text
    71 matthew say-by
    wait-text
    72 wolf say-by
    wait-text
    73 matthew say-by
    wait-text
    10 frames wait
    74 5 say-mode-wait
    ovl012 $80103360 native
    end-script
    ;

\ $801060E8
: entry-801060E8
    matthew 468319 0 292477 set-pos
    matthew 0 1 anim
    matthew 0 2048 0 set-rot
    wolf 467829 0 289177 set-pos
    wolf 0 1 anim
    wolf 0 4015 0 set-rot
    james 467038 0 291003 set-pos
    james 77 1 anim
    james 0 1070 0 set-rot
    -350 -1928 -296 468307 -84 292483 2048 camera-set
    36 james say-by-wait
    matthew 81 0 anim
    matthew wait-anim
    37 james say-by-wait
    matthew 14 0 anim
    matthew wait-anim
    28 20 camera-turn
    matthew 6 80 face
    matthew 4 0 anim
    matthew wait-anim
    -1340 -176 -1346 468403 588 288091 3926 camera-set
    matthew 467329 291327 walk-to
    467857 588 288091 10 camera-move
    55 frames wait
    matthew wait-stop
    -530 -932 -408 467419 48 291067 3268 camera-set
    matthew 6 80 face
    matthew 90 0 anim
    matthew wait-anim
    38 james say-by
    wait-text
    39 matthew say-by
    matthew 88 0 anim
    wait-text
    40 james say-by
    wait-text
    41 matthew say-by
    matthew 86 0 anim
    wait-text
    42 james say-by
    james 78 72 1 anim-then
    wait-text
    43 matthew say-by
    wait-text
    44 matthew say-by
    matthew 89 0 anim
    wait-text
    45 james say-by
    james 79 72 1 anim-then
    wait-text
    46 james say-by
    james 73 74 1 anim-then
    wait-text
    -512 -932 -504 467557 -60 289657 1660 camera-set
    47 james say-by
    james 72 1 anim
    wait-text
    10 frames wait
    3 screen-effect
    wait-effect
    goto L-80105C18
    ;

\ $80106414
: entry-80106414
    111 actor as-this-one
    this-one 465797 0 292173 set-pos
    this-one 0 1024 0 set-rot
    this-one 72 1 anim
    matthew 466287 0 292193 set-pos
    matthew 0 3072 0 set-rot
    matthew 0 1 anim
    -164 -1010 -2504 465989 12 292115 3761 camera-set
    matthew 70 72 1 anim-then
    matthew 75 8 talk
    this-one 73 74 anim-at  this-one wait-anim
    this-one 76 0 talk
    matthew 77 8 talk
    this-one 75 72 anim-at  this-one wait-anim
    this-one 78 0 talk
    matthew 79 8 talk
    this-one 80 0 talk
    matthew 81 8 talk
    this-one 82 0 talk
    matthew 83 8 talk
    this-one 73 74 anim-at  this-one wait-anim
    this-one 84 0 talk
    this-one 85 0 talk
    matthew 86 8 talk
    this-one 87 0 talk
    this-one 88 0 talk
    this-one 75 72 anim-at  this-one wait-anim
    this-one 89 0 talk
    this-one 90 0 talk
    matthew 91 0 talk
    this-one 92 0 talk
    this-one 93 0 talk
    matthew 94 0 talk
    this-one 73 74 anim-at  this-one wait-anim
    this-one 95 0 talk
    this-one 96 0 talk
    matthew 97 0 talk
    this-one 98 0 talk
    this-one 99 0 talk
    this-one 100 0 talk
    matthew 101 0 talk
    this-one 75 72 anim-at  this-one wait-anim
    15 music
    this-one 102 0 talk
    matthew 103 8 talk
    this-one 104 8 talk
    this-one 105 0 talk
    this-one 106 8 talk
    this-one 73 74 anim-at  this-one wait-anim
    this-one 107 0 talk
    2 1 sequence-play
    30 frames wait
    matthew 108 0 talk
    matthew 71 0 anim-at  matthew wait-anim
    -398 -1442 -866 465989 12 292115 3689 camera-set
    matthew 466287 291833 walk-to
    matthew wait-stop
    matthew 104 101 anim-at  matthew wait-anim
    2 sequence-stop
    matthew 109 0 talk
    110 phone-say
    this-one 75 72 anim-at  this-one wait-anim
    matthew 111 0 talk
    112 phone-say
    matthew 113 0 talk
    114 phone-say
    picture-hide
    matthew 115 0 talk
    this-one 73 74 anim-at  this-one wait-anim
    this-one 116 0 talk
    matthew 111 200 face
    matthew wait-turned
    matthew 117 3 talk
    this-one 118 0 talk
    matthew 119 7 talk
    this-one 120 0 talk
    this-one 121 0 talk
    17 music
    this-one 122 0 talk
    matthew 123 5 talk
    this-one 124 0 talk
    matthew 125 5 talk
    this-one 126 0 talk
    matthew 127 0 talk
    this-one 128 0 talk
    129 5 say-mode-wait
    matthew 466388 0 291782 set-pos
    end-script
    ;

\ $80106954
:noname
    give-control
    isabella as-this-one
    this-one 2765 2765 0 anim-then
    this-one wait-anim
    this-one 2767 2768 0 anim-then
    this-one wait-anim
    this-one 2765 2766 0 anim-then
    this-one wait-anim
    this-one 2766 2765 0 anim-then
    this-one wait-anim
    this-one 2767 2768 0 anim-then
    this-one wait-anim
    this-one 2763 1 anim
    goto entry-80106954
    ; is entry-80106954

\ $80106B14
: entry-80106B14
    give-control
    112 actor 465102 291528 walk-to
    isabella as-this-one
    this-one 81 0 anim
    this-one wait-anim
    this-one 465702 289094 walk-to
    this-one wait-stop
    this-one 1024 -200 turn-to
    this-one wait-turned
    10 frames wait
    this-one 8 0 anim
    this-one anim-hold
    this-one wait-anim
    10 frames wait
    this-one 466890 289090 walk-to
    this-one wait-stop
    this-one 52 0 through-door
    10 frames wait
    this-one 0 0 0 set-pos
    99 actor 466718 289033 walk-to
    99 actor wait-stop
    99 actor 0 200 turn-to
    99 actor wait-turned
    10 frames wait
    end-script
    ;

: L-80106C0C
    112 actor 465672 290608 walk-to
    112 actor wait-stop
    112 actor 2162 200 turn-to
    112 actor 97 98 1 anim-then
    10 frames wait
    112 actor 100 101 1 anim-then
    30 frames wait
    112 actor 103 0 anim
    112 actor wait-anim
    112 actor 130 132 0 anim-then
    112 actor wait-anim
    end-script
    ;

: L-80106CA4
    115 ambient-start
    113 ambient-start
    30 frames wait
    115 ambient-stop
    xa-stop
    james 466978 0 291043 set-pos
    james 77 1 anim
    james 0 930 0 set-rot
    99 actor 467136 0 289033 set-pos
    99 actor 0 0 0 set-rot
    99 actor 0 1 anim
    -530 -2294 -908 466401 96 290824 2880 camera-set
    map-sync
    10 frames wait
    113 ambient-stop
    20 frames wait
    james 153 0 talk
    matthew 154 4 talk
    james 78 72 1 anim-then
    james 155 0 talk
    matthew 156 7 talk
    james 157 0 talk
    112 actor 465412 290008 walk-to
    112 actor wait-stop
    112 actor 89 0 face
    112 actor 130 132 0 anim-then
    112 actor 152 0 talk
    wolf place-at-stop
    end-script
    ;

: L-80106E28
    isabella as-this-one
    this-one 2765 2765 0 anim-then
    this-one wait-anim
    this-one 2767 2768 0 anim-then
    this-one wait-anim
    this-one 2765 2766 0 anim-then
    this-one wait-anim
    this-one 2766 2765 0 anim-then
    this-one wait-anim
    this-one 2767 2768 0 anim-then
    this-one wait-anim
    this-one 2763 1 anim
    goto L-80106CA4
    ;

: L-80106F88
    300 frames wait
    -500 -1112 -434 465716 0 291263 2406 camera-set
    467000 0 291263 7 camera-move
    wait-camera-move
    10 frames wait
    -716 -854 -986 467127 204 291436 4272 camera-set
    50 frames wait
    matthew 17 18 anim-at  matthew wait-anim
    25 frames wait
    -572 -1148 -560 467616 24 289195 1702 camera-set
    70 frames wait
    -716 -854 -986 467127 204 291436 4272 camera-set
    15 frames wait
    matthew 19 0 anim-at  matthew wait-anim
    15 frames wait
    -656 -710 -1154 465141 96 289456 2616 camera-set
    end-script
    ;

: L-80106EC4
    19 frames wait
    3 6 caption-for
    text-close
    20 frames wait
    4 6 caption-for
    text-close
    6 frames wait
    5 6 caption-for
    text-close
    18 frames wait
    6 5 caption-for
    text-close
    3 frames wait
    7 5 caption-for
    text-close
    15 frames wait
    8 6 caption-for
    text-close
    5 frames wait
    9 6 caption-for
    text-close
    9 frames wait
    10 6 caption-for
    text-close
    end-script
    ;

\ $8010709C
: entry-8010709C
    isabella as-this-one
    james 187 actor-model
    james 466978 0 291043 set-pos
    james 77 1 anim
    james 0 930 0 set-rot
    matthew 467148 0 291283 set-pos
    matthew 0 1 anim
    matthew 0 2470 0 set-rot
    this-one 466718 0 288673 set-pos
    this-one 12 1 set-floor
    this-one 0 0 0 set-rot
    this-one 0 1 anim
    99 actor 466718 0 289033 set-pos
    99 actor 12 1 set-floor
    99 actor 0 0 0 set-rot
    99 actor 0 1 anim
    wolf 468104 0 289129 set-pos
    wolf 0 0 0 set-rot
    wolf 2744 1 anim
    112 actor 465102 0 291528 set-pos
    112 actor 12 1 set-floor
    112 actor 0 3072 0 set-rot
    112 actor 0 1 anim
    -704 -1190 -860 467342 228 290916 7608 camera-set
    10 frames wait
    james 139 0 talk
    matthew 140 6 talk
    james 78 72 1 anim-then
    james 141 0 talk
    matthew 142 7 talk
    james 143 8 talk
    matthew 144 7 talk
    james 76 77 1 anim-then
    james 145 0 talk
    matthew 146 3 talk
    james 78 72 1 anim-then
    james 147 0 talk
    -1 music
    115 ambient-start
    113 ambient-start
    15 frames wait
    -536 -1946 -956 467021 0 289444 2308 camera-set
    james 466968 0 290943 set-pos
    james 74 1 anim
    james 0 1700 0 set-rot
    99 actor 467136 289033 walk-to
    99 actor wait-stop
    115 ambient-stop
    99 actor 0 -200 turn-to
    this-one 50 0 through-door
    this-one 14 15 1 anim-then
    30 frames wait
    113 ambient-stop
    this-one 138 0 talk
    this-one 16 0 anim
    this-one wait-anim
    -662 -3884 -1058 465776 156 288660 2568 camera-set
    this-one 465860 289188 walk-to
    10 frames wait
    465056 156 289452 20 camera-move
    ['] L-80106C0C start-event
    this-one wait-stop
    10 frames wait
    this-one 13 0 anim
    this-one anim-hold
    this-one wait-anim
    10 frames wait
    this-one 465202 289544 walk-to
    this-one wait-stop
    this-one 512 200 turn-to
    this-one wait-turned
    102 0 sequence-play
    20 frames wait
    this-one 81 0 anim
    this-one wait-anim
    this-one 130 7 talk
    112 actor 130 132 0 anim-then
    112 actor 131 0 talk
    this-one 132 4 talk
    james 148 0 talk
    matthew 149 0 talk
    james 150 0 talk
    -674 -548 -974 465261 -24 289576 2880 camera-set
    10467 1 say-mode-wait
    10468 1 say-mode-wait
    james 151 0 talk
    112 actor 130 132 0 anim-then
    112 actor 152 0 talk
    this-one 87 0 anim
    this-one wait-anim
    99 actor 0 0 0 set-pos
    wolf 467488 0 289463 set-pos
    wolf 0 3170 0 set-rot
    wolf 2744 1 anim
    112 actor 464882 290568 walk-to
    45 frames wait
    this-one 2762 2763 anim-at  this-one wait-anim
    35 frames wait
    0 xa-play
    112 actor wait-stop
    112 actor 89 0 face
    matthew 0 1 anim
    ['] L-80106E28 start-event
    ['] L-80106F88 start-event
    ['] L-80106EC4 start-event
    end-script
    ;

: L-801069F0
    isabella as-this-one
    -464 -2552 -1214 465884 0 290167 -1488 camera-set
    this-one 81 0 anim
    this-one wait-anim
    this-one 465702 289094 walk-to
    this-one wait-stop
    this-one 1024 -200 turn-to
    this-one wait-turned
    10 frames wait
    466274 156 288912 20 camera-move
    this-one 8 0 anim
    this-one anim-hold
    this-one wait-anim
    10 frames wait
    this-one 466890 289090 walk-to
    this-one wait-stop
    this-one 52 0 through-door
    10 frames wait
    this-one 0 0 0 set-pos
    99 actor 466718 289033 walk-to
    99 actor wait-stop
    99 actor 0 200 turn-to
    99 actor wait-turned
    wolf place-at-stop
    10 frames wait
    end-script
    ;

\ $8010769C
: entry-8010769C
    -1 music
    isabella as-this-one
    this-one 466718 0 288673 set-pos
    this-one 12 1 set-floor
    this-one 0 0 0 set-rot
    this-one 0 1 anim
    99 actor 466718 0 289033 set-pos
    99 actor 0 0 0 set-rot
    99 actor 0 1 anim
    wolf 468104 0 289129 set-pos
    wolf 0 0 0 set-rot
    wolf 2744 1 anim
    112 actor 465102 0 291528 set-pos
    112 actor 0 3072 0 set-rot
    112 actor 0 1 anim
    map-sync
    10 frames wait
    115 ambient-start
    113 ambient-start
    202 5 say-mode-wait
    matthew 81 0 anim
    matthew wait-anim
    115 ambient-stop
    -536 -1946 -956 467021 0 289444 2308 camera-set
    15 frames wait
    113 ambient-stop
    99 actor 467136 289033 walk-to
    99 actor wait-stop
    99 actor 0 -200 turn-to
    this-one 50 0 through-door
    this-one 14 15 1 anim-then
    30 frames wait
    this-one 138 0 talk
    this-one 16 0 anim
    this-one wait-anim
    -662 -3884 -1058 465776 156 288660 2568 camera-set
    this-one 465860 289188 walk-to
    10 frames wait
    465056 156 289452 20 camera-move
    ['] L-80106C0C start-event
    this-one wait-stop
    10 frames wait
    this-one 13 0 anim
    this-one anim-hold
    this-one wait-anim
    10 frames wait
    this-one 465202 289544 walk-to
    this-one wait-stop
    this-one 512 200 turn-to
    this-one wait-turned
    this-one 81 0 anim
    this-one wait-anim
    102 0 sequence-play
    20 frames wait
    this-one 130 7 talk
    112 actor 130 132 0 anim-then
    112 actor 131 0 talk
    this-one 132 4 talk
    this-one 133 7 talk
    113 ambient-start
    112 actor 130 132 0 anim-then
    112 actor 134 0 talk
    this-one 14 15 anim-at  this-one wait-anim
    10 frames wait
    113 ambient-stop
    -734 -1292 -752 468260 156 289098 1416 camera-set
    wolf 2718 2719 1 anim-then
    40 frames wait
    wolf 2720 0 anim
    wolf wait-anim
    wolf 14 15 1 anim-then
    20 frames wait
    -464 -2552 -1214 465884 0 290167 -1488 camera-set
    20 frames wait
    this-one 16 0 anim-at  this-one wait-anim
    this-one wait-anim
    this-one 135 3 talk
    112 actor 97 98 1 anim-then
    10 frames wait
    112 actor 100 101 1 anim-then
    112 actor 138 0 talk
    10 frames wait
    112 actor 103 0 anim
    112 actor wait-anim
    this-one 136 3 talk
    112 actor 130 132 0 anim-then
    112 actor 137 0 talk
    this-one 138 4 talk
    this-one 81 0 anim
    this-one wait-anim
    this-one 465702 289094 walk-to
    this-one wait-stop
    this-one 1024 -200 turn-to
    this-one wait-turned
    10 frames wait
    ['] L-801069F0 start-event
    end-script
    ;

\ $80107BA8
: entry-80107BA8
    -1 music
    isabella as-this-one
    this-one 466718 0 288673 set-pos
    this-one 0 0 0 set-rot
    this-one 0 1 anim
    this-one 12 1 set-floor
    99 actor 466718 0 289033 set-pos
    99 actor 0 0 0 set-rot
    99 actor 0 1 anim
    wolf 468104 0 289129 set-pos
    wolf 0 0 0 set-rot
    wolf 2744 1 anim
    112 actor 465102 0 291528 set-pos
    112 actor 0 3072 0 set-rot
    112 actor 0 1 anim
    map-sync
    10 frames wait
    115 ambient-start
    113 ambient-start
    202 5 say-mode-wait
    matthew 81 0 anim
    matthew wait-anim
    115 ambient-stop
    -536 -1946 -956 467021 0 289444 2308 camera-set
    15 frames wait
    113 ambient-stop
    99 actor 467136 289033 walk-to
    99 actor wait-stop
    99 actor 0 -200 turn-to
    give-control
    this-one 50 0 through-door
    this-one 14 15 1 anim-then
    30 frames wait
    this-one 16 0 anim
    this-one wait-anim
    this-one 465860 289188 walk-to
    10 frames wait
    112 actor 465672 290608 walk-to
    112 actor wait-stop
    112 actor 2162 200 turn-to
    112 actor 97 98 1 anim-then
    10 frames wait
    112 actor 100 101 1 anim-then
    30 frames wait
    112 actor 103 0 anim
    112 actor wait-anim
    112 actor 130 132 0 anim-then
    112 actor wait-anim
    this-one wait-stop
    10 frames wait
    this-one 13 0 anim
    this-one anim-hold
    this-one wait-anim
    10 frames wait
    this-one 465202 289544 walk-to
    this-one wait-stop
    this-one 512 200 turn-to
    this-one wait-turned
    this-one 81 0 anim
    this-one wait-anim
    this-one 87 0 anim
    this-one wait-anim
    40 frames wait
    goto entry-80106954
    ;

\ $80107EC0
: entry-80107EC0
    112 actor as-this-one
    matthew 465759 0 289946 set-pos
    matthew 0 3244 0 set-rot
    matthew 0 1 anim
    this-one 465412 0 290008 set-pos
    this-one 2474 0 turn-to
    isabella 465202 0 289544 set-pos
    isabella 0 512 0 set-rot
    isabella 0 0 anim
    -518 -1868 -506 465623 72 289946 2480 camera-set
    this-one 158 0 talk
    this-one 159 7 talk
    this-one 160 7 talk
    matthew 161 0 talk
    this-one 0 0 face
    this-one wait-turned
    this-one 162 5 talk
    matthew 163 0 talk
    this-one 164 5 talk
    this-one 165 5 talk
    166 5 say-mode-wait
    this-one 167 5 talk
    matthew 168 0 talk
    this-one 169 7 talk
    matthew 170 6 talk
    this-one 171 7 talk
    this-one 2736 0 anim
    this-one anim-hold
    matthew 2737 0 anim
    matthew anim-hold
    matthew wait-anim
    3 screen-effect
    wait-effect
    matthew 465909 0 290240 set-pos
    112 actor 465194 0 290240 set-pos
    matthew 0 112 actor 4 fight
    -1 music
    this-one 465407 41 289780 set-pos
    this-one 0 1304 0 set-rot
    this-one 30 1 anim
    matthew 465725 41 289536 set-pos
    matthew 0 7670 0 set-rot
    matthew 0 0 anim
    -518 -2552 -1190 465765 0 290100 6566 camera-set
    2 screen-effect
    wait-effect
    10 frames wait
    this-one 31 0 anim-at  this-one wait-anim
    this-one 465197 290180 2 10 walk-to-ex
    10 frames wait
    matthew 90 0 anim
    this-one wait-stop
    this-one 465117 290960 5 50 walk-to-ex
    172 112 actor 2 say-timed-by
    this-one wait-stop
    this-one 0 0 0 set-pos
    10 frames wait
    matthew 86 0 anim
    10 frames wait
    isabella as-this-one
    this-one 0 200 face
    this-one wait-stop
    this-one 173 4 talk
    10 frames wait
    matthew 174 6 talk
    this-one 17 18 anim-at  this-one wait-anim
    this-one 175 0 talk
    10 frames wait
    matthew 176 7 talk
    matthew 177 7 talk
    this-one 19 0 1 anim-then
    this-one 178 0 talk
    matthew 179 7 talk
    this-one 14 15 anim-at  this-one wait-anim
    30 frames wait
    this-one 16 0 anim-at  this-one wait-anim
    this-one 180 7 talk
    this-one 14 15 anim-at  this-one wait-anim
    10 frames wait
    this-one 16 0 anim-at  this-one wait-anim
    this-one 181 3 talk
    10 frames wait
    ['] L-801069F0 start-event
    end-script
    ;

\ $80108324
: entry-80108324
    mel as-this-one
    this-one 468204 0 290315 set-pos
    this-one 0 2048 0 set-rot
    this-one 0 1 anim
    wolf 468214 0 289285 set-pos
    wolf 0 -140 0 set-rot
    wolf 2744 1 anim
    99 actor 467974 0 290175 set-pos
    99 actor 0 560 0 set-rot
    99 actor 0 1 anim
    matthew 468124 0 292375 set-pos
    matthew 0 2048 0 set-rot
    matthew 0 1 anim
    winona 468634 0 291325 set-pos
    winona 0 2240 0 set-rot
    winona 131 1 anim
    -530 -2480 -1160 468034 0 291853 -2772 camera-set
    10 frames wait
    this-one 182 0 talk
    -1 music
    matthew 183 6 talk
    winona 14 15 anim-at  winona wait-anim
    winona 0 200 face
    winona 0 1 anim
    winona wait-turned
    winona 184 0 talk
    matthew 468124 291055 walk-to
    15 frames wait
    468034 0 290197 25 camera-move
    10 frames wait
    winona 1 ride
    matthew 185 0 talk
    wait-camera-move
    wolf 2721 2719 1 anim-then
    wolf 186 0 talk
    99 actor 187 5 talk
    this-one 2738 2739 anim-at  this-one wait-anim
    this-one 188 0 talk
    this-one 189 0 talk
    this-one 2740 0 anim-at  this-one wait-anim
    wolf 2720 0 1 anim-then
    wolf 190 0 talk
    this-one 191 5 talk
    mel 2748 200 turn-to
    this-one wait-turned
    99 actor 192 0 talk
    153 actor 468304 0 292315 set-pos
    153 actor 0 2170 0 set-rot
    153 actor 0 1 anim
    99 actor 2736 0 anim
    99 actor anim-hold
    this-one 2737 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 193 0 talk
    this-one 91 92 1 anim-then
    99 actor 94 95 1 anim-then
    winona 194 0 talk
    100 0 sequence-play
    10 frames wait
    153 actor 1 actor-model
    153 actor 195 0 talk
    -506 -1832 -746 468034 0 291817 -3540 camera-set
    153 actor 468304 290815 walk-to
    20 frames wait
    468034 0 290557 20 camera-move
    20 frames wait
    this-one 93 0 1 anim-then
    99 actor 96 0 1 anim-then
    wolf 196 0 talk
    this-one 153 0 face
    153 actor 197 5 talk
    this-one 198 1 talk
    0 1 sequence-play
    25 frames wait
    153 actor 199 0 talk
    153 actor 104 101 1 anim-then
    10 frames wait
    0 sequence-stop
    105 sound
    153 actor 200 0 talk
    50 frames wait
    153 actor 201 0 talk
    17 music
    wolf 202 0 talk
    153 actor 203 0 talk
    153 actor 103 0 anim-at  153 actor wait-anim
    matthew 204 7 talk
    153 actor 14 15 anim-at  153 actor wait-anim
    153 actor 205 0 talk
    wolf 206 0 talk
    153 actor 16 0 anim-at  153 actor wait-anim
    153 actor 207 5 talk
    this-one 208 6 talk
    this-one 209 1 talk
    153 actor 0 200 turn-to
    153 actor wait-turned
    153 actor 210 0 talk
    153 actor 468304 292365 walk-to
    10 frames wait
    this-one 468304 292365 walk-to
    211 mel 1 say-timed-by
    10 frames wait
    matthew 718 -200 turn-to
    this-one wait-stop
    101 0 sequence-play
    this-one 0 0 0 set-pos
    153 actor 0 0 0 set-pos
    winona 212 5 talk
    matthew 213 4 talk
    winona 45 1 set-stop
    15 frames wait
    end-script
    ;

\ $80108984
: entry-80108984
    wolf 214 0 talk
    end-script
    ;

\ $80108998
: entry-80108998
    wolf 215 0 talk
    end-script
    ;

\ $801089AC
: entry-801089AC
    wolf as-this-one
    this-one 216 0 talk
    matthew 217 5 talk
    this-one 218 0 talk
    matthew 219 1 talk
    this-one 220 0 talk
    end-script
    ;

\ $801089F4
: entry-801089F4
    wolf as-this-one
    matthew 221 7 talk
    this-one 222 0 talk
    matthew 223 7 talk
    224 5 say-mode-wait
    this-one 225 0 talk
    matthew 226 3 talk
    this-one 227 0 talk
    matthew 228 5 talk
    this-one 229 0 talk
    matthew 230 7 talk
    this-one 231 0 talk
    matthew 232 7 talk
    this-one 233 0 talk
    matthew 234 2 talk
    this-one 235 0 talk
    matthew 236 5 talk
    this-one 237 0 talk
    matthew 238 7 talk
    this-one 239 0 talk
    end-script
    ;

\ $80108AE4
: entry-80108AE4
    mel 240 0 talk
    matthew 241 5 talk
    mel 242 0 talk
    end-script
    ;

\ $80108B5C
: entry-80108B5C
    james as-this-one
    this-one 248 0 talk
    matthew 249 4 talk
    this-one 250 0 talk
    matthew 251 7 talk
    this-one 252 0 talk
    matthew 253 4 talk
    this-one 254 0 talk
    this-one 255 0 talk
    matthew 256 2 talk
    this-one 257 0 talk
    end-script
    ;

\ $80108BE0
: entry-80108BE0
    james as-this-one
    matthew 258 7 talk
    this-one 259 0 talk
    this-one 260 0 talk
    matthew 261 2 talk
    this-one 262 0 talk
    this-one 263 0 talk
    matthew 264 4 talk
    end-script
    ;

\ $80108C40
: entry-80108C40
    99 actor as-this-one
    this-one 265 0 talk
    matthew 266 5 talk
    this-one 267 0 talk
    matthew 268 7 talk
    this-one 269 0 talk
    end-script
    ;

\ $80108C88
: entry-80108C88
    99 actor 270 0 talk
    271 5 say-mode-wait
    end-script
    ;

\ $80108CDC
: entry-80108CDC
    111 actor 275 0 talk
    276 5 say-mode-wait
    end-script
    ;

\ $80108CFC
: entry-80108CFC
    277 5 say-mode-wait
    end-script
    ;

\ $80108D0C
: entry-80108D0C
    112 actor as-this-one
    matthew 278 0 talk
    this-one 279 7 talk
    matthew 280 7 talk
    this-one 281 5 talk
    matthew 282 3 talk
    this-one 283 5 talk
    matthew 284 5 talk
    matthew 285 2 talk
    this-one 286 1 talk
    matthew 85 0 anim-at  matthew wait-anim
    matthew 287 5 talk
    this-one 288 7 talk
    end-script
    ;

\ $80108DC4
: entry-80108DC4
    -476 -2018 -1310 468282 0 292368 -66 camera-set
    48 door-animate
    events.main:sub-80096CF0
    27 -1 468285 292835 0 0 warp
    ;

\ $80108E14
: entry-80108E14
    -380 -2738 -662 469721 0 290916 1551 camera-set
    49 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80108E50
: entry-80108E50
    -314 -2414 -2426 469910 0 291116 -1035 camera-set
    49 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80108E8C
: entry-80108E8C
    -404 -2420 -506 470437 0 291607 222 camera-set
    51 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80108EC8
: entry-80108EC8
    -410 -2558 -1454 470366 0 291874 5864 camera-set
    51 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;
