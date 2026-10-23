\ ovl019.fs - area 019 overlay (CD.BIN 019_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl019
USING: events events.main ;

\ $801034A0
: entry-801034A0
    11 screen-effect
    wait-effect
    32 scene-reset
    0 set-scene
    matthew 32 3 set-floor
    matthew 312261 -2300 306044 set-pos
    matthew 2048 0 turn-to
    ovl019 $80102234 native
    map-sync
    2 frames wait
    12 screen-effect
    5 5 effect-level
    1 frames wait
    8 8 effect-level
    1 frames wait
    3 3 effect-level
    1 frames wait
    0 0 effect-level
    6 frames wait
    4 4 effect-level
    1 frames wait
    6 6 effect-level
    1 frames wait
    3 3 effect-level
    1 frames wait
    0 0 effect-level
    2 frames wait
    4 4 effect-level
    1 frames wait
    8 8 effect-level
    1 frames wait
    13 screen-effect
    matthew 308726 305651 300 wait-near
    ovl019 $80102234 native
    matthew 4101 1 anim
    8 16 effect-level
    12 screen-effect
    wait-effect
    matthew 202 0 talk
    16 0 effect-level
    12 screen-effect
    wait-effect
    end-script
    ;

\ $8010369C
: entry-8010369C
    matthew 4116 0 anim
    matthew wait-anim
    matthew 4100 0 anim
    10 frames wait
    10 frames wait
    259 screen-effect
    wait-effect
    15 5 say-mode-wait
    16 5 say-mode-wait
    2 screen-effect
    wait-effect
    end-script
    ;

\ $801036FC
: entry-801036FC
    matthew 4116 0 anim
    matthew wait-anim
    matthew 4100 0 anim
    10 frames wait
    41 minigame
    wait-minigame
    19 scene-reset
    5 set-scene
    matthew 19 2 set-floor
    matthew 308875 -1200 305877 set-pos
    -620 -1244 256 308875 -1200 305877 3722 camera-set
    matthew 4104 0 anim
    2 screen-effect
    matthew wait-anim
    matthew 19 2 set-floor
    wait-effect
    end-script
    ;

\ $801037A4
: entry-801037A4
    matthew 32 0 anim
    matthew wait-anim
    matthew 33 0 anim
    matthew wait-anim
    11 say-wait-close
    end-script
    ;

\ $801037D0
: entry-801037D0
    5 say-wait-close
    6 say-wait-close
    end-script
    ;

\ $801037E4
: entry-801037E4
    7 say-wait-close
    8 say-wait-close
    end-script
    ;

\ $801037F8
: entry-801037F8
    9 say-wait-close
    end-script
    ;

\ $80103804
: entry-80103804
    74 door-animate-alt
    55 frames wait
    11 screen-effect
    wait-effect
    1 set-scene
    22 minigame
    end-script
    ;

\ $80103834
: entry-80103834
    events.main:sub-80096E34
    matthew 62 0 anim
    matthew wait-anim
    17 matthew say-by-wait
    end-script
    ;

\ $8010385C
: entry-8010385C
    169 actor 306695 0 305649 set-pos
    169 actor 19 2 set-floor
    -518 -3278 -1250 307430 -996 307419 1392 camera-set
    307430 -996 306483 10 camera-move
    matthew 81 0 anim
    200 5 say-mode-wait
    wait-camera-move
    -500 -2594 -2582 307322 -996 306159 3840 camera-set
    matthew 307542 305933 walk-to
    matthew wait-stop
    matthew 3774 200 turn-to
    matthew wait-turned
    matthew 82 0 anim
    201 5 say-mode-wait
    matthew 17 18 anim-at  matthew wait-anim
    -500 -2594 -2582 307322 -996 306159 -1968 camera-set
    matthew 307082 305708 walk-to
    matthew wait-stop
    matthew 48 50 anim-at  matthew wait-anim
    10 frames wait
    193 194 cd-pictures
    20 frames wait
    203 5 say-mode-wait
    204 5 say-mode-wait
    0 cd-picture
    matthew 49 0 anim-at  matthew wait-anim
    end-script
    ;

\ $801039DC
: entry-801039DC
    nozz as-this-one
    2 set-scene
    this-one 18 0 talk
    matthew 19 0 talk
    this-one 20 0 talk
    3 screen-effect
    wait-effect
    this-one 314401 0 303058 set-pos
    this-one 0 100 0 set-rot
    this-one 0 1 anim
    this-one 19 2 set-floor
    matthew 314553 0 303510 set-pos
    matthew 0 2170 0 set-rot
    matthew 0 1 anim
    matthew 19 2 set-floor
    -386 -2198 -314 314517 -1200 302970 20012 camera-set
    10 frames wait
    2 screen-effect
    wait-effect
    this-one 21 6 talk
    matthew 22 0 talk
    this-one 23 7 talk
    matthew 24 7 talk
    10 frames wait
    this-one 25 3 talk
    matthew 26 7 talk
    this-one 27 2 talk
    20 frames wait
    15 music
    this-one 85 0 anim-at  this-one wait-anim
    this-one 28 5 talk
    matthew 29 7 talk
    this-one 30 4 talk
    matthew 31 5 talk
    this-one 32 7 talk
    matthew 33 5 talk
    this-one 34 0 talk
    this-one 35 5 talk
    matthew 36 0 talk
    this-one 37 6 talk
    matthew 38 0 talk
    this-one 39 5 talk
    matthew 40 7 talk
    this-one 41 3 talk
    10 frames wait
    matthew 42 0 talk
    this-one 43 3 talk
    matthew 44 7 talk
    10 frames wait
    this-one 45 3 talk
    this-one 46 5 talk
    matthew 47 0 talk
    48 5 say-mode-wait
    10 frames wait
    this-one 49 0 talk
    matthew 50 0 talk
    this-one 51 7 talk
    matthew 87 0 anim
    matthew wait-anim
    20 frames wait
    3 screen-effect
    wait-effect
    1 set-scene
    -404 -2306 -572 307243 -1200 304894 1029 camera-set
    20 frames wait
    2 screen-effect
    wait-effect
    matthew 75 2 through-door
    matthew 3072 0 turn-to
    end-script
    ;

\ $80103D1C
: entry-80103D1C
    1 set-scene
    isabella as-this-one
    matthew 307279 0 305150 set-pos
    matthew 0 0 0 set-rot
    matthew 0 1 anim
    matthew 19 2 set-floor
    -1202 -122 -2318 307179 -1080 305038 4102 camera-set
    20 frames wait
    54 5 say-mode-wait
    matthew 66 68 anim-at  matthew wait-anim
    20 frames wait
    55 5 say-mode-wait
    20 frames wait
    3 screen-effect
    wait-effect
    4 set-scene
    this-one 307914 0 306036 set-pos
    this-one 19 2 set-floor
    -392 -578 -446 307347 -1200 305422 24 camera-set
    20 frames wait
    0 12 effect-level
    12 screen-effect
    30 frames wait
    this-one 307914 307356 walk-to
    15 frames wait
    52 5 say-mode-wait
    99 actor 307569 0 305036 set-pos
    99 actor 0 -700 0 set-rot
    99 actor 116 1 anim
    99 actor 19 2 set-floor
    isabella wait-stop
    this-one 4105 4106 1 anim-then
    30 frames wait
    3 screen-effect
    wait-effect
    1 set-scene
    matthew 0 1 anim
    -554 -80 -554 306811 -1212 304846 504 camera-set
    2 screen-effect
    wait-effect
    -1 music
    53 32767 actor 1 say-timed-by
    matthew 14 0 anim
    matthew wait-anim
    matthew 99 60 face
    768 20 camera-angle
    99 actor 106 0 anim
    10 frames wait
    113 sound
    5 screen-effect
    wait-effect
    4 set-scene
    this-one 307914 0 306756 set-pos
    this-one 0 2310 0 set-rot
    this-one 0 1 anim
    this-one 19 2 set-floor
    99 actor 307704 0 306206 set-pos
    99 actor 0 280 0 set-rot
    99 actor 0 1 anim
    99 actor 19 2 set-floor
    -662 -2054 -212 307560 -984 306394 -94 camera-set
    30 frames wait
    4 screen-effect
    5 frames wait
    5 screen-effect
    wait-effect
    259 screen-effect
    wait-effect
    56 0 say-mode-wait
    15 frames wait
    57 0 say-mode-wait
    58 0 say-mode-wait
    59 0 say-mode-wait
    15 frames wait
    60 0 say-mode-wait
    61 0 say-mode-wait
    20 frames wait
    5 screen-effect
    wait-effect
    isabella 0 0 0 set-pos
    99 actor 0 0 0 set-pos
    60 frames wait
    120 ambient-start
    -734 -2288 -328 308856 -1572 306160 -2038 camera-set
    4 screen-effect
    wait-effect
    5 screen-effect
    wait-effect
    4 screen-effect
    15 frames wait
    5 screen-effect
    wait-effect
    4 screen-effect
    5 frames wait
    62 4 say-mode-wait
    20 frames wait
    11 music
    -392 -1608 -328 308820 -1332 307450 -1918 camera-set
    -1150 8 camera-angle
    40 frames wait
    63 5 say-mode-wait
    wait-camera-turn
    5 screen-effect
    wait-effect
    matthew 308783 0 307618 set-pos
    matthew 0 3024 0 set-rot
    matthew 68 1 anim
    4 screen-effect
    wait-effect
    -710 -974 -2552 308713 -1200 307508 780 camera-set
    20 frames wait
    64 5 say-mode-wait
    65 5 say-mode-wait
    10 frames wait
    matthew 67 0 anim
    matthew wait-anim
    10 frames wait
    end-script
    ;

\ $801042AC
: entry-801042AC
    isabella as-this-one
    isabella 182 actor-model
    4 set-scene
    matthew 306408 0 307729 set-pos
    matthew 0 -1028 0 set-rot
    matthew 0 1 anim
    matthew 19 2 set-floor
    this-one 310188 0 306049 set-pos
    this-one 0 1 anim
    this-one 19 2 set-floor
    -554 -1154 -2120 306300 -924 307621 -434 camera-set
    10 frames wait
    matthew 44 0 anim
    matthew wait-anim
    matthew 164 actor ignore-a
    matthew 46 51 anim-at  matthew wait-anim
    207 208 cd-pictures
    20 frames wait
    66 5 say-mode-wait
    120 ambient-stop
    67 5 say-mode-wait
    20 frames wait
    -1 music
    116 sound
    0 cd-picture
    10 frames wait
    matthew 47 0 anim
    matthew wait-anim
    matthew 4294967295 actor ignore-a
    164 actor 306176 0 307812 set-pos
    164 actor -1450 set-y
    164 actor 1024 -910 0 set-rot
    matthew 45 0 anim
    matthew wait-anim
    matthew 17 0 anim
    matthew wait-anim
    matthew 19 0 1 anim-then
    matthew -2918 -60 turn-to
    -1868 -422 -2078 307428 228 306079 1198 camera-set
    this-one 78 5 through-door
    -812 -1550 -1052 306048 -900 307621 1342 camera-set
    this-one 14 0 anim
    this-one wait-anim
    this-one 0 120 face
    this-one wait-turned
    68 isabella say-by-wait
    this-one 307750 307620 walk-to
    598 -10 camera-angle
    306120 -900 307117 10 camera-move
    20 frames wait
    69 matthew 3 say-timed-by
    this-one wait-stop
    this-one 306960 307790 walk-to
    matthew 898 -60 turn-to
    this-one wait-stop
    this-one 0 0 turn-to
    this-one 44 45 0 anim-then
    this-one wait-anim
    this-one 350 60 turn-to
    this-one wait-turned
    this-one 44 45 0 anim-then
    this-one wait-anim
    5 music
    20 frames wait
    70 isabella 2 say-timed-by
    this-one 86 0 anim
    this-one wait-anim
    20 frames wait
    matthew 4105 4106 anim-at  matthew wait-anim
    matthew 71 0 talk
    20 frames wait
    this-one 17 18 anim-at  this-one wait-anim
    20 frames wait
    this-one 72 0 talk
    matthew 4107 0 anim
    this-one 19 0 anim-at  this-one wait-anim
    this-one 73 0 talk
    this-one 17 0 anim
    this-one wait-anim
    this-one 0 0 face
    -728 -1226 -806 307236 -888 306937 -458 camera-set
    30 frames wait
    this-one 74 7 talk
    matthew 75 7 talk
    this-one 17 19 0 anim-then
    this-one wait-anim
    this-one 76 3 talk
    matthew 77 7 talk
    -194 5 camera-angle
    this-one 17 0 anim
    this-one wait-anim
    this-one 1040 -120 turn-to
    this-one wait-turned
    this-one 307380 307790 walk-to
    this-one wait-stop
    20 frames wait
    this-one 78 0 talk
    matthew 79 4 talk
    matthew 80 3 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 81 0 talk
    this-one 16 0 anim-at  this-one wait-anim
    this-one 82 0 talk
    matthew 83 0 talk
    this-one 4105 4106 1 anim-then
    30 frames wait
    this-one 84 0 talk
    10 frames wait
    52 5 say-mode-wait
    10 frames wait
    end-script
    ;

: L-80104A38
    isabella 99 0 talk
    ['] events.main:sub-80099518 start-event
    end-script
    ;

: L-80104A54
    isabella 103 0 talk
    ['] events.main:sub-80099518 start-event
    end-script
    ;

: L-80104820
    isabella as-this-one
    matthew 104 0 talk
    30 frames wait
    this-one 105 4 talk
    -1112 -986 -1712 306588 -576 307633 262 camera-set
    5 music
    matthew 141 0 anim-at  matthew wait-anim
    matthew 4294967295 actor ignore-a
    164 actor 306176 0 307812 set-pos
    164 actor -1450 set-y
    164 actor 1024 -910 0 set-rot
    matthew 89 -200 face
    this-one 17 18 anim-at  this-one wait-anim
    this-one 106 0 talk
    20 frames wait
    this-one 107 0 talk
    10 frames wait
    matthew 108 7 talk
    this-one 19 0 anim-at  this-one wait-anim
    this-one 109 3 talk
    matthew 110 6 talk
    -2066 20 camera-dist-to
    30 frames wait
    this-one -1915 -200 turn-to
    this-one wait-turned
    this-one 306908 307279 1 15 walk-to-ex
    this-one 111 0 talk
    20 frames wait
    this-one 112 0 talk
    matthew 113 7 talk
    this-one 114 3 talk
    this-one 115 0 talk
    this-one 116 0 talk
    this-one 117 0 talk
    this-one 118 3 talk
    this-one 119 4 talk
    this-one 120 0 talk
    this-one 121 7 talk
    this-one 122 0 talk
    matthew 123 7 talk
    50 frames wait
    104 1 sequence-play
    end-script
    ;

: L-80104A70
    isabella as-this-one
    matthew 93 0 talk
    this-one 94 0 talk
    matthew 95 0 talk
    this-one 96 0 talk
    this-one 97 0 talk
    this-one 98 0 talk
    92 2 choice
    0 result= if  goto L-80104A38  then
    1 result= if  goto L-80104A54  then
    2 result= if  goto L-80104820  then
    end-script
    ;

: L-80104AF4
    isabella as-this-one
    matthew 100 0 talk
    this-one 101 0 talk
    this-one 102 0 talk
    92 2 choice
    0 result= if  goto L-80104A38  then
    1 result= if  goto L-80104A54  then
    2 result= if  goto L-80104820  then
    end-script
    ;

\ $80104B54
: entry-80104B54
    isabella as-this-one
    4 set-scene
    matthew 306408 0 307729 set-pos
    matthew 0 -1028 0 set-rot
    matthew 0 1 anim
    matthew 19 2 set-floor
    this-one 307408 0 307559 set-pos
    this-one 0 1 anim
    this-one 19 2 set-floor
    -554 -1154 -2120 306300 -924 307621 -434 camera-set
    10 frames wait
    matthew 44 0 anim
    matthew wait-anim
    matthew 164 actor ignore-a
    matthew 46 51 anim-at  matthew wait-anim
    20 frames wait
    this-one 85 0 talk
    this-one 306748 307529 walk-to
    -950 -2342 -752 306738 -696 307567 742 camera-set
    10 frames wait
    matthew 86 0 talk
    this-one 87 0 talk
    matthew 141 0 anim-at  matthew wait-anim
    matthew 17 18 anim-at  matthew wait-anim
    matthew -1588 -200 turn-to
    matthew 88 0 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 89 0 talk
    20 frames wait
    matthew 90 0 talk
    20 frames wait
    this-one 16 0 anim-at  this-one wait-anim
    this-one 91 7 talk
    matthew 19 0 anim-at  matthew wait-anim
    matthew 133 51 anim-at  matthew wait-anim
    10 frames wait
    92 2 choice
    0 result= if  goto L-80104A70  then
    1 result= if  goto L-80104AF4  then
    2 result= if  goto L-80104820  then
    end-script
    ;

\ $80104D88
: entry-80104D88
    isabella as-this-one
    this-one 124 0 talk
    matthew 125 0 talk
    this-one 126 1 talk
    this-one 4105 4106 anim-at  this-one wait-anim
    end-script
    ;

\ $80104DC8
: entry-80104DC8
    isabella as-this-one
    4 set-scene
    matthew 306421 0 307431 set-pos
    matthew 0 -1024 0 set-rot
    matthew 0 1 anim
    matthew 19 2 set-floor
    -446 -1982 -698 306665 -1200 307513 -2776 camera-set
    10 frames wait
    matthew 44 0 260 anim-then
    104 sequence-stop
    matthew 45 0 260 anim-then
    matthew 89 200 face
    matthew 127 0 talk
    matthew 89 0 face
    this-one 128 0 talk
    matthew 129 7 talk
    this-one 130 0 talk
    matthew 131 0 talk
    this-one 132 0 talk
    matthew 133 7 talk
    this-one 4107 0 anim-at  this-one wait-anim
    this-one 134 1 talk
    10 frames wait
    9 screen-effect
    wait-effect
    matthew 307165 -1200 305175 set-pos
    matthew -7024 0 turn-to
    1 set-scene
    2 screen-effect
    wait-effect
    isabella 184 actor-model
    end-script
    ;

: L-80105284
    isabella as-this-one
    10 frames wait
    matthew 157 7 talk
    10 frames wait
    matthew 158 6 talk
    this-one 4107 0 anim-at  this-one wait-anim
    isabella 14 15 anim-at  isabella wait-anim
    this-one 159 0 talk
    isabella 16 0 anim-at  isabella wait-anim
    this-one 160 0 talk
    this-one 306728 307649 walk-to
    306873 -432 307761 10 camera-move
    matthew 161 6 talk
    this-one wait-stop
    isabella 4105 4106 anim-at  isabella wait-anim
    matthew 306961 307581 walk-to
    matthew wait-stop
    this-one 4107 0 anim-at  this-one wait-anim
    this-one 14 15 anim-at  this-one wait-anim
    this-one 0 200 face
    2 frames wait
    this-one 0 1 anim
    20 frames wait
    this-one 162 0 talk
    20 frames wait
    matthew 182 7 talk
    30 frames wait
    this-one 306888 307619 1 15 walk-to-ex
    this-one wait-stop
    this-one 1139 200 turn-to
    this-one wait-turned
    this-one 4105 4106 anim-at  this-one wait-anim
    20 50 rumble
    20 frames wait
    matthew 181 0 talk
    20 frames wait
    this-one 183 0 talk
    20 frames wait
    this-one 184 0 talk
    matthew 185 0 talk
    this-one 186 0 talk
    this-one 4107 0 anim-at  this-one wait-anim
    this-one 306708 307629 2 15 walk-to-ex
    this-one wait-stop
    this-one 195 7 talk
    this-one 196 4 talk
    20 frames wait
    matthew 197 7 talk
    20 frames wait
    9 screen-effect
    wait-effect
    matthew 307091 0 304936 set-pos
    matthew 0 1048 0 set-rot
    -554 -2894 -1172 307060 -1200 304891 1052 camera-set
    1 set-scene
    8 screen-effect
    wait-effect
    ovl019 $801029E8 native
    end-script
    ;

: L-80104F5C
    isabella as-this-one
    10 frames wait
    matthew 157 7 talk
    10 frames wait
    matthew 158 6 talk
    this-one 4107 0 anim-at  this-one wait-anim
    isabella 14 15 anim-at  isabella wait-anim
    this-one 159 0 talk
    isabella 16 0 anim-at  isabella wait-anim
    this-one 160 0 talk
    this-one 306728 307649 walk-to
    306873 -432 307761 10 camera-move
    matthew 161 6 talk
    this-one wait-stop
    isabella 4105 4106 anim-at  isabella wait-anim
    this-one 162 0 talk
    20 frames wait
    this-one 163 0 talk
    isabella 4107 0 anim-at  isabella wait-anim
    this-one 0 200 face
    this-one wait-turned
    this-one 164 4 talk
    this-one 165 3 talk
    matthew 166 5 talk
    this-one 167 7 talk
    this-one 168 7 talk
    10 frames wait
    matthew 169 4 talk
    this-one 306728 307769 walk-to
    this-one wait-stop
    this-one 44 46 0 anim-then
    this-one wait-anim
    this-one 141 0 anim
    this-one wait-anim
    this-one 306958 307649 walk-to
    this-one wait-stop
    this-one 0 -200 face
    this-one wait-turned
    this-one 59 60 anim-at  this-one wait-anim
    this-one 171 0 talk
    matthew 59 61 0 anim-then
    15 frames wait
    this-one 61 0 0 anim-then
    matthew wait-anim
    matthew 133 51 1 anim-then
    this-one 172 0 talk
    this-one 173 3 talk
    this-one 174 7 talk
    matthew 175 0 talk
    matthew 55 0 anim-at  matthew wait-anim
    7 picture-show
    180 3 say-mode-wait
    picture-hide
    this-one 176 6 talk
    matthew 177 7 talk
    this-one 178 4 talk
    10 frames wait
    this-one 179 7 talk
    10 frames wait
    9 screen-effect
    wait-effect
    matthew 307091 0 304936 set-pos
    matthew 0 1048 0 set-rot
    -554 -2894 -1172 307060 -1200 304891 1052 camera-set
    1 set-scene
    8 screen-effect
    wait-effect
    ovl019 $801029C8 native
    end-script
    ;

\ $80105540
: entry-80105540
    isabella as-this-one
    10 frames wait
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    30 frames wait
    135 32767 actor say-by-wait
    20 frames wait
    matthew 136 0 talk
    20 frames wait
    this-one 137 0 talk
    10 frames wait
    matthew 138 0 talk
    this-one 139 0 talk
    -482 -1526 -464 306202 -1116 304693 4780 camera-set
    matthew 74 1 through-door
    3 screen-effect
    wait-effect
    4 set-scene
    matthew 307881 0 306831 set-pos
    matthew 0 3072 0 set-rot
    matthew 0 1 anim
    matthew 19 2 set-floor
    this-one 307228 0 306599 set-pos
    this-one 0 0 0 set-rot
    this-one 161 1 anim
    this-one 19 2 set-floor
    -302 -1472 -644 307637 -1356 306931 -1576 camera-set
    this-one 184 actor-model
    -1 music
    30 frames wait
    2 screen-effect
    wait-effect
    10 frames wait
    matthew 140 7 talk
    10 frames wait
    this-one 141 0 talk
    matthew 142 4 talk
    20 frames wait
    this-one 143 0 talk
    this-one 163 0 anim
    this-one wait-anim
    this-one 0 200 face
    matthew 144 0 talk
    this-one 145 1 talk
    20 frames wait
    matthew 146 0 talk
    this-one 17 18 anim-at  this-one wait-anim
    this-one 147 0 talk
    104 1 sequence-play
    20 frames wait
    this-one 0 0 anim
    this-one -948 0 turn-to
    this-one 4105 4106 anim-at  this-one wait-anim
    matthew 14 15 anim-at  matthew wait-anim
    -616 14 camera-angle
    this-one 4107 0 anim-at  this-one wait-anim
    this-one 306868 306599 walk-to
    this-one wait-stop
    this-one 306488 307379 walk-to
    this-one wait-stop
    this-one -948 0 turn-to
    this-one 44 46 0 anim-then
    this-one wait-anim
    104 sequence-stop
    this-one 100 101 1 anim-then
    35 frames wait
    this-one 102 101 1 anim-then
    this-one 148 0 talk
    matthew 0 1 anim
    -374 -572 -356 307505 -1356 307171 5496 camera-set
    30 frames wait
    this-one 149 0 talk
    matthew 81 0 anim
    matthew wait-anim
    this-one 306548 0 307559 set-pos
    matthew 0 3702 0 set-rot
    -932 -1730 -2840 307413 -1200 306645 4224 camera-set
    this-one 307028 307769 walk-to
    this-one wait-stop
    this-one 4128 -200 turn-to
    this-one wait-turned
    this-one 150 0 talk
    10 frames wait
    matthew 151 0 talk
    this-one 4105 4106 anim-at  this-one wait-anim
    this-one 152 0 talk
    matthew 307801 307401 walk-to
    matthew wait-stop
    matthew 307261 307641 walk-to
    this-one 153 0 talk
    6 music
    20 frames wait
    this-one 154 0 talk
    matthew 155 0 talk
    10 frames wait
    this-one 156 0 talk
    -1292 -1172 -1922 307053 -432 307761 5856 camera-set
    10 frames wait
    5 24 prompt
    0 result= if  goto L-80105284  then
    1 result= if  goto L-80104F5C  then
    end-script
    ;

\ $80105A58
: entry-80105A58
    nozz as-this-one
    this-one 206 0 talk
    matthew 207 7 talk
    this-one 208 0 talk
    matthew 209 3 talk
    this-one 210 0 talk
    matthew 211 7 talk
    this-one 212 0 talk
    matthew 213 6 talk
    this-one 214 0 talk
    matthew 215 1 talk
    this-one 216 0 talk
    matthew 217 6 talk
    end-script
    ;

\ $80105B3C
: entry-80105B3C
    -434 -2090 -926 306482 0 304828 -426 camera-set
    71 door-animate
    events.main:sub-80096CF0
    27 -1 305879 304896 3072 0 warp
    ;

\ $80105B8C
: entry-80105B8C
    -440 -2630 -122 311192 0 304696 2965 camera-set
    72 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80105BC8
: entry-80105BC8
    -524 -2480 -1346 311160 0 304328 4131 camera-set
    72 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80105C04
: entry-80105C04
    -404 -2306 -572 307243 -1200 304894 1029 camera-set
    73 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80105C40
: entry-80105C40
    -452 -3308 -770 311747 -1044 304312 347 camera-set
    73 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80105C7C
: entry-80105C7C
    77 door-animate-alt
    events.main:sub-80096CF0
    27 -1 -1 -1 3072 0 warp
    ;

\ $80105CAC
: arrive-80105CAC
    matthew 311774 0 303234 set-pos
    matthew 0 0 0 set-rot
    matthew -960 set-y
    -32 -4682 -266 311048 -1200 303667 -654 camera-set
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
    matthew 19 2 set-floor
    end-script
    ;

\ $80105D70
: entry-80105D70
    -398 -2726 -752 310982 -24 303501 2780 camera-set
    matthew 303770 set-z
    matthew 0 2048 0 set-rot
    matthew 10 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    11 screen-effect
    matthew 11 0 anim
    9 frames wait
    19 0 -1 -1 0 -2146411348 warp
    ;

\ $80105EEC
: entry-80105EEC
    -32 -4682 -266 311048 -1200 303667 -654 camera-set
    matthew 303636 set-z
    matthew 0 2048 0 set-rot
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    11 screen-effect
    matthew 7 0 anim
    9 frames wait
    31 1 -1 -1 0 -2146410968 warp
    ;

\ $80105FA4
: entry-80105FA4
    -248 -2702 -296 311546 96 303456 7800 camera-set
    matthew 303762 set-z
    matthew 0 2048 0 set-rot
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 9 0 anim
    matthew anim-hold
    matthew wait-anim
    end-script
    ;

\ $8010603C
: entry-8010603C
    -476 -1730 -1448 311720 0 303879 2042 camera-set
    matthew 303269 set-z
    matthew 0 0 0 set-rot
    matthew 10 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 13 0 anim
    matthew anim-hold
    matthew wait-anim
    end-script
    ;

\ $801060D4
: entry-801060D4
    -404 -2306 -572 307243 -1200 304894 1029 camera-set
    74 door-animate
    events.main:sub-80096CF0
    4 set-scene
    end-script
    ;

\ $80106110
: entry-80106110
    -566 -2414 -1364 307079 -1200 305822 -1808 camera-set
    74 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80106188
: entry-80106188
    -332 -3422 -1448 314674 -1200 304283 3920 camera-set
    75 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $801061C4
: entry-801061C4
    -404 -2306 -572 307243 -1200 304894 1029 camera-set
    76 door-animate
    events.main:sub-80096CF0
    6 set-scene
    end-script
    ;

\ $80106200
: entry-80106200
    -494 -4178 -602 309978 -1200 305846 10552 camera-set
    76 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80106278
: entry-80106278
    -392 -2990 -638 308050 -1200 306228 3271 camera-set
    78 door-animate
    events.main:sub-80096CF0
    4 set-scene
    ovl019 $80102A04 native
    end-script
    ;

\ $801062BC
: entry-801062BC
    -476 -4112 -590 313317 -1200 303721 3522 camera-set
    79 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $801062F8
: entry-801062F8
    -512 -2774 -194 313949 -1200 303712 1178 camera-set
    79 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80106428
: entry-80106428
    -464 -1238 -920 306613 12 305897 4400 camera-set
    12 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $80106460
: entry-80106460
    -464 -1238 -920 306613 12 305897 4400 camera-set
    13 5 say-mode-wait
    14 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' L-80104820 $80104820 19 overlay-word!
' L-80104A38 $80104A38 19 overlay-word!
' L-80104A54 $80104A54 19 overlay-word!
' arrive-80105CAC $80105CAC 19 overlay-word!
