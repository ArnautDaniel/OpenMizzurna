\ ovl001.fs - area 001 overlay (CD.BIN 001_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl001
USING: events events.main ;

\ (looped back to: declared first)
defer L-80103B30
defer L-80103BB0

\ $801035FC
: entry-801035FC
    3 frames wait
    6 object-anim
    end-script
    ;

\ $80103624
: entry-80103624
    10 frames wait
    text-hold
    222 3 say-mode
    end-script
    ;

\ $80103640
: entry-80103640
    10 frames wait
    text-hold
    223 3 say-mode
    end-script
    ;

\ $8010365C
: entry-8010365C
    10 frames wait
    text-hold
    224 0 choice
    main-exe $8004E594 native
    end-script
    ;

\ $80103680
: entry-80103680
    10 frames wait
    text-hold
    238 0 choice
    main-exe $8004E5B8 native
    end-script
    ;

\ $801036A4
: entry-801036A4
    10 frames wait
    text-hold
    239 0 choice
    main-exe $8004E5B8 native
    end-script
    ;

\ $801036C8
: entry-801036C8
    10 frames wait
    text-hold
    225 0 choice
    main-exe $8004E5F4 native
    10 frames wait
    end-script
    ;

\ $801036F4
: entry-801036F4
    10 frames wait
    text-hold
    226 3 say-mode
    10 frames wait
    end-script
    ;

\ $80103718
: entry-80103718
    10 frames wait
    text-hold
    236 3 say-mode
    40 frames wait
    end-script
    ;

\ $8010373C
: entry-8010373C
    10 frames wait
    text-hold
    237 3 say-mode
    40 frames wait
    end-script
    ;

\ $801037A8
: entry-801037A8
    10 frames wait
    text-hold
    229 3 say-mode
    10 frames wait
    end-script
    ;

\ $801037CC
: entry-801037CC
    10 frames wait
    text-hold
    230 3 say-mode
    10 frames wait
    end-script
    ;

\ $801037F0
: entry-801037F0
    10 frames wait
    text-hold
    231 3 say-mode
    10 frames wait
    end-script
    ;

\ $80103814
: entry-80103814
    10 frames wait
    text-hold
    232 3 say-mode
    10 frames wait
    end-script
    ;

\ $80103838
: entry-80103838
    10 frames wait
    text-hold
    233 3 say-mode
    40 frames wait
    end-script
    ;

\ $8010385C
: entry-8010385C
    10 frames wait
    text-hold
    234 3 say-mode
    40 frames wait
    end-script
    ;

\ $801038A4
: entry-801038A4
    -566 -1652 -782 346239 -4080 285238 594 camera-set
    nozz 0 0 face
    nozz 240 7 talk
    -818 -2360 -2618 346106 -4080 285488 3852 camera-set
    matthew 2 0 face
    matthew 241 4 talk
    5 frames wait
    3 screen-effect
    25 frames wait
    1 set-scene
    2 screen-effect
    -584 -2792 -932 348247 -4080 284957 -228 camera-set
    matthew 14 3 through-door
    wait-effect
    end-script
    ;

: L-80103C14
    end-script
    ;

: L-80103988
    xa-stop
    5 frames wait
    16 music
    259 screen-effect
    wait-effect
    matthew 348180 0 287578 set-pos
    matthew 0 0 0 set-rot
    -554 -1904 -2318 348246 -4080 287680 1372 camera-set
    end-script
    ;

: L-801039F4
    matthew 507 0 anim
    2 screen-effect
    wait-effect
    matthew wait-anim
    matthew 509 0 anim
    matthew wait-anim
    events.main:sub-80096D74
    end-script
    ;

: L-80103978
    55 frames wait
    text-close
    end-script
    ;

: L-80103ADC
    2 memory-card
    text-close
    1 result= if  goto L-80103BB0  then
    221 3 say-mode
    ' L-80103978 start-event
    goto L-801039F4
    ;

: L-80103B10
    217 0 choice
    0 result= if  goto L-80103ADC  then
    goto L-80103BB0
    ;

: L-80103B70
    214 2 choice
    2 result= if  goto L-80103B30  then
    2 frames wait
    text-close
    2 frames wait
    1 result= if  goto L-801039F4  then
    goto L-80103B10
    ;

: L-80103A34
    0 memory-card
    text-close
    1 result= if  goto L-80103BB0  then
    219 3 say-mode
    ' L-80103978 start-event
    goto L-801039F4
    ;

: L-80103A68
    215 0 choice
    0 result= if  goto L-80103A34  then
    goto L-80103BB0
    ;

: L-80103A88
    1 memory-card
    text-close
    1 result= if  goto L-80103BB0  then
    220 3 say-mode
    ' L-80103978 start-event
    goto L-801039F4
    ;

: L-80103ABC
    216 0 choice
    0 result= if  goto L-80103A88  then
    goto L-80103BB0
    ;

:noname
    213 2 choice
    2 result= if  goto L-80103B70  then
    2 frames wait
    text-close
    2 frames wait
    0 result= if  goto L-80103A68  then
    goto L-80103ABC
    ; is L-80103B30

:noname
    211 0 choice
    1 result= if  goto L-801039F4  then
    212 say
    40 frames wait
    text-hold
    goto L-80103B30
    ; is L-80103BB0

: L-80103BFC
    L-80103988
    ovl001 $80103390 native
    goto L-80103BB0
    ;

: L-80103BE4
    L-80103988
    ovl001 $801033B0 native
    goto L-80103BB0
    ;

\ $80103C18
: entry-80103C18
    matthew 348131 287216 0 face-point
    matthew 209 4 talk
    210 2 choice
    2 result= if  goto L-80103C14  then
    0 result= if  goto L-80103BFC  then
    goto L-80103BE4
    ;

\ $80103C60
: wake-by-the-bed
    L-80103988
    goto L-801039F4
    ;

\ $80103C70
: entry-80103C70
    37 5 say-mode-wait
    end-script
    ;

\ $80103C80
: entry-80103C80
    matthew 347961 0 284945 set-pos
    matthew 0 2048 0 set-rot
    -260 -1800 -1368 348243 -4020 284649 1468 camera-set
    matthew 36 38 0 anim-then
    matthew wait-anim
    matthew 51 1 anim
    38 2 say-mode-wait
    39 2 say-mode-wait
    end-script
    ;

\ $80103D0C
: entry-80103D0C
    11 screen-effect
    wait-effect
    xa-stop
    27 -1 347635 284407 2048 0 warp
    ;

\ $80103D38
: entry-80103D38
    -788 -2774 -1556 347643 -4080 286903 3872 camera-set
    matthew 347171 0 287387 set-pos
    matthew 3072 0 turn-to
    matthew 62 0 anim
    matthew wait-anim
    242 say
    wait-text
    end-script
    ;

\ $80103FE4
: new-game-opening
    -1 music
    257 screen-effect
    matthew 348180 0 287578 set-pos
    matthew 0 0 0 set-rot
    20 frames wait
    60 2 caption-for
    text-close
    61 2 caption-for
    text-close
    2 1 sequence-play
    20 frames wait
    -76 -1640 -752 348570 -3924 286978 -1550 camera-set
    347814 -3924 286942 5 camera-move
    80 frames wait
    8 screen-effect
    matthew 504 0 anim
    wait-effect
    matthew wait-anim
    2 sequence-stop
    62 matthew say-by
    matthew 505 1 anim
    wait-text
    0 picture-show
    63 3 say-mode-wait
    picture-hide
    matthew 64 0 talk
    0 picture-show
    65 3 say-mode-wait
    picture-hide
    matthew 506 0 anim
    matthew wait-anim
    66 matthew say-by
    matthew 507 508 1 anim-then
    wait-text
    matthew 508 1 anim
    0 picture-show
    67 3 say-mode-wait
    picture-hide
    matthew 68 0 talk
    matthew 348330 0 287618 set-pos
    -482 -1112 -2048 348330 -4080 287618 0 camera-set
    matthew 509 510 1 anim-then
    55 frames wait
    0 picture-show
    69 3 say-mode-wait
    picture-hide
    70 matthew say-by
    matthew 511 512 1 anim-then
    wait-text
    matthew 512 1 anim
    0 picture-show
    71 3 say-mode-wait
    picture-hide
    matthew 72 0 talk
    0 picture-show
    73 3 say-mode-wait
    picture-hide
    matthew 513 514 anim-at  matthew wait-anim
    matthew 74 0 talk
    matthew 75 0 talk
    0 picture-show
    76 3 say-mode-wait
    picture-hide
    matthew 77 0 talk
    0 picture-show
    78 3 say-mode-wait
    picture-hide
    matthew 348420 0 287558 set-pos
    -282 64382 -128 348180 -4140 287368 64240 camera-set
    matthew 515 0 anim
    80 frames wait
    105 sound
    matthew 160 actor ignore-b
    matthew wait-anim
    matthew 526 1 anim
    205 206 cd-pictures
    79 5 say-mode-wait
    80 5 say-mode-wait
    81 5 say-mode-wait
    82 5 say-mode-wait
    83 5 say-mode-wait
    84 5 say-mode-wait
    0 cd-picture
    matthew 516 0 anim
    30 frames wait
    matthew 4294967295 actor ignore-b
    160 actor 347967 0 287556 set-pos
    160 actor -4260 set-y
    160 actor 1024 -910 0 set-rot
    matthew wait-anim
    matthew 287538 set-z
    matthew 68 1 anim
    85 5 say-mode-wait
    camera-follow
    matthew 67 0 anim
    matthew wait-anim
    12 music
    end-script
    ;

\ $80104438
: entry-80104438
    nozz as-this-one
    -470 -2210 -1160 347488 -4080 287360 -448 camera-set
    this-one 14 3 through-door
    matthew 2 200 face
    this-one 0 200 face
    this-one wait-turned
    nozz 81 0 anim
    nozz wait-anim
    this-one 96 5 talk
    this-one 87 0 anim
    this-one wait-anim
    10 frames wait
    5 frames wait
    nozz 347171 287387 walk-to
    nozz wait-stop
    this-one 14 1 through-door
    nozz 0 0 0 set-pos
    end-script
    ;

\ $80104514
: entry-80104514
    morgan as-this-one
    matthew 346901 0 287566 set-pos
    morgan 345547 0 285294 set-pos
    morgan -4080 set-y
    morgan 0 0 0 set-rot
    nozz 346344 0 285214 set-pos
    nozz -4080 set-y
    nozz 0 -920 0 set-rot
    -608 -656 -626 346908 -4076 287542 -1228 camera-set
    matthew 345848 287272 0 face-point
    matthew 81 0 anim
    matthew wait-anim
    5 frames wait
    matthew 30 walk-forward
    24 frames wait
    matthew -70 -24 turn-by
    22 frames wait
    -730 63118 -2306 345609 -4080 285716 63084 camera-set
    2 frames wait
    97 morgan 2 say-timed-by
    morgan 59 60 1 anim-then
    45 frames wait
    matthew idle
    3 frames wait
    matthew 59 61 0 anim-then
    15 frames wait
    morgan 61 0 anim
    morgan wait-anim
    -610 -234 -602 345249 -4008 285716 -2452 camera-set
    10 frames wait
    10472 1 say-mode-wait
    10473 1 say-mode-wait
    -730 63118 -2306 345609 -4080 285716 63084 camera-set
    matthew 99 7 talk
    this-one 100 5 talk
    matthew 101 7 talk
    this-one 102 7 talk
    morgan 88 0 anim-at  morgan wait-anim
    matthew 103 3 talk
    this-one 104 7 talk
    matthew 105 2 talk
    -522 -218 -512 345870 -4056 285518 1452 camera-set
    10 frames wait
    matthew 0 1 anim
    this-one 15 1 anim
    10474 1 say-mode-wait
    10475 1 say-mode-wait
    10476 1 say-mode-wait
    nozz 106 5 talk
    -522 64094 -176 345582 -4080 285524 1644 camera-set
    this-one 107 0 talk
    matthew 108 5 talk
    this-one 16 0 1 anim-then
    this-one 109 0 talk
    matthew 83 84 anim-at  matthew wait-anim
    10 frames wait
    matthew 85 0 anim
    matthew wait-anim
    matthew 110 5 talk
    nozz 111 6 talk
    matthew 112 3 talk
    17 music
    this-one 113 7 talk
    this-one 115 3 talk
    matthew 116 5 talk
    this-one 117 3 talk
    nozz 118 5 talk
    matthew 119 1 talk
    this-one 120 3 talk
    morgan 14 15 1 anim-then
    nozz 121 5 talk
    10 frames wait
    morgan 16 0 1 anim-then
    this-one 122 0 talk
    matthew 123 4 talk
    -410 63534 -778 345582 -4080 285524 3100 camera-set
    this-one 124 7 talk
    morgan 345042 285254 walk-to
    this-one wait-stop
    this-one 15 3 through-door
    this-one 346467 283920 walk-to
    matthew 2 -150 face
    matthew 125 5 talk
    nozz 126 6 talk
    this-one wait-stop
    end-script
    ;

\ $801049F8
: entry-801049F8
    james 187 actor-model
    james as-this-one
    this-one 345624 0 286159 set-pos
    this-one 0 1 anim
    this-one 0 3220 0 set-rot
    nozz 345454 0 286409 set-pos
    nozz 0 1 anim
    nozz 0 5592 0 set-rot
    matthew 6 0 face
    -668 -3068 -992 345304 -3900 285845 4990 camera-set
    map-sync
    nozz 0 0 face
    nozz wait-turned
    nozz 127 5 talk
    this-one 0 0 face
    this-one 128 6 talk
    matthew 129 7 talk
    this-one 130 3 talk
    matthew 131 4 talk
    this-one 132 7 talk
    10 frames wait
    3 screen-effect
    wait-effect
    0 set-scene
    153 actor 187 actor-model
    153 actor as-this-one
    this-one 348447 0 288026 set-pos
    this-one 1 1 set-floor
    this-one 57 1 anim
    this-one 0 0 0 set-rot
    matthew 348977 0 287906 set-pos
    matthew 0 1 anim
    matthew 0 -642 0 set-rot
    -368 -1994 -392 348665 -4104 288014 304 camera-set
    map-sync
    2 screen-effect
    wait-effect
    10 frames wait
    this-one 58 35 0 anim-then
    this-one wait-anim
    this-one 33 0 1 anim-then
    5 frames wait
    this-one 133 0 6 talk-to
    this-one 134 6 6 talk-to
    matthew 135 3 talk
    this-one 136 7 6 talk-to
    matthew 137 6 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 0 200 face
    3 frames wait
    this-one 0 1 anim
    this-one wait-turned
    this-one 138 7 6 talk-to
    this-one 139 2 6 talk-to
    this-one 140 0 6 talk-to
    this-one 85 0 anim-at  this-one wait-anim
    this-one 141 5 6 talk-to
    10 frames wait
    matthew 142 3 talk
    this-one 143 6 6 talk-to
    matthew 144 5 talk
    this-one 145 3 6 talk-to
    matthew 146 7 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 147 0 6 talk-to
    this-one 148 0 6 talk-to
    matthew 149 4 talk
    this-one 16 0 anim-at  this-one wait-anim
    this-one 150 6 6 talk-to
    this-one 151 7 6 talk-to
    matthew 152 6 talk
    this-one 87 0 anim
    this-one wait-anim
    10 frames wait
    this-one 349152 287264 walk-to  this-one wait-stop
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80104E10
: entry-80104E10
    nozz as-this-one
    153 5 say-mode-wait
    matthew 81 0 anim
    154 5 say-mode-wait
    matthew wait-anim
    matthew 44 46 0 anim-then
    matthew wait-anim
    matthew 51 1 anim
    155 5 say-mode-wait
    matthew 141 0 anim-at  matthew wait-anim
    -548 -1796 -728 346091 -3984 285448 4612 camera-set
    matthew 345893 285418 walk-to
    matthew wait-stop
    matthew 2 0 face
    matthew 59 60 anim-at  matthew wait-anim
    this-one 156 5 talk
    matthew 157 0 talk
    matthew 158 0 talk
    matthew 61 0 anim-at  matthew wait-anim
    this-one 159 4 talk
    matthew 160 5 talk
    this-one 161 6 talk
    matthew 162 7 talk
    this-one 163 3 talk
    matthew 164 6 talk
    this-one 165 0 talk
    matthew 134 137 0 anim-then
    matthew wait-anim
    24 picture-show
    166 3 say-mode-wait
    picture-hide
    10 frames wait
    end-script
    ;

\ $80104FA0
: entry-80104FA0
    -374 -2300 -836 349363 -4080 287148 5508 camera-set
    12 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80104FDC
: entry-80104FDC
    -722 -2552 -860 348917 -3768 287540 1180 camera-set
    13 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80105018
: entry-80105018
    -470 -8 -1622 349787 -4068 287871 3059 camera-set
    13 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80105054
: entry-80105054
    -632 -3710 -1220 348572 -4008 286043 -182 camera-set
    12 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80105090
: entry-80105090
    -632 -3710 -1220 348572 -4008 286043 -182 camera-set
    14 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $801050CC
: entry-801050CC
    -482 -1688 -1154 346769 -4068 287506 4882 camera-set
    14 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

: L-80105108
    27 -1 344881 284957 2560 0 warp
    ;

\ $80105124
: entry-80105124
    -482 -1904 -824 345080 -4032 285428 2314 camera-set
    xa-stop
    15 door-animate
    events.main:sub-80096CF0
    ovl001 $801032B0 native-result
    1 result= if  goto L-80105108  then
    27 -1 344881 284957 2560 -2146388268 warp
    ;

\ $80105198
: entry-80105198
    1 5 say-mode-wait
    end-script
    ;

\ $801051A8
: entry-801051A8
    2 5 say-mode-wait
    end-script
    ;

\ $801051B8
: entry-801051B8
    matthew 348085 0 287663 set-pos
    matthew 44 46 0 anim-then
    matthew wait-anim
    matthew 51 1 anim
    205 206 cd-pictures
    10 frames wait
    3 5 say-mode-wait
    0 cd-picture
    matthew 47 0 anim
    matthew wait-anim
    matthew 348138 -4080 287722 set-pos
    end-script
    ;

\ $80105248
: entry-80105248
    matthew 348085 0 287663 set-pos
    matthew 44 46 0 anim-then
    matthew wait-anim
    matthew 51 1 anim
    205 206 cd-pictures
    10 frames wait
    4 5 say-mode-wait
    0 cd-picture
    matthew 47 0 anim
    matthew wait-anim
    matthew 348138 -4080 287722 set-pos
    end-script
    ;

\ $801052E8
: entry-801052E8
    matthew 348453 0 288060 set-pos
    matthew 0 0 0 set-rot
    -482 -1436 -176 348583 -4056 288191 286 camera-set
    matthew 32 34 anim-at  matthew wait-anim
    105 sound
    matthew 56 57 anim-at  matthew wait-anim
    106 sound
    6 5 say-mode-wait
    matthew 58 141 0 anim-then
    matthew wait-anim
    matthew 35 33 0 anim-then
    matthew wait-anim
    end-script
    ;

\ $801053A0
: entry-801053A0
    7 5 say-mode-wait
    8 5 say-mode-wait
    end-script
    ;

\ $801053BC
: entry-801053BC
    -1 music
    -428 -1220 -896 348879 -4080 288077 3392 camera-set
    matthew 40 0 anim
    matthew wait-anim
    108 sound
    matthew 41 0 anim
    matthew wait-anim
    20 frames wait
    1 xa-play
    20 frames wait
    9 5 say-mode-wait
    xa-stop
    20 frames wait
    end-script
    ;

\ $80105458
: entry-80105458
    23 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $80105470
: entry-80105470
    24 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $80105488
: entry-80105488
    matthew 347961 0 284945 set-pos
    matthew 0 2048 0 set-rot
    -260 -1800 -1368 348243 -4020 284649 1468 camera-set
    matthew 36 38 0 anim-then
    matthew wait-anim
    matthew 51 1 anim
    26 5 say-mode-wait
    5 frames wait
    106 sound
    matthew 56 57 anim-at  matthew wait-anim
    27 2 say-mode-wait
    28 2 say-mode-wait
    29 2 say-mode-wait
    30 2 say-mode-wait
    31 2 say-mode-wait
    32 2 say-mode-wait
    33 2 say-mode-wait
    34 2 say-mode-wait
    matthew 58 0 anim
    matthew wait-anim
    35 5 say-mode-wait
    5 frames wait
    matthew 55 0 anim
    matthew wait-anim
    7 picture-show
    36 3 say-mode-wait
    picture-hide
    5 frames wait
    end-script
    ;

\ $801055E4
: entry-801055E4
    59 5 say-mode-wait
    end-script
    ;

\ $801055F4
: entry-801055F4
    40 5 say-mode-wait
    end-script
    ;

\ $80105604
: entry-80105604
    41 5 say-mode-wait
    end-script
    ;

\ $80105614
: entry-80105614
    42 5 say-mode-wait
    end-script
    ;

\ $80105624
: entry-80105624
    43 5 say-mode-wait
    end-script
    ;

\ $80105634
: entry-80105634
    44 5 say-mode-wait
    end-script
    ;

\ $80105644
: entry-80105644
    45 5 say-mode-wait
    end-script
    ;

\ $80105654
: entry-80105654
    46 5 say-mode-wait
    end-script
    ;

\ $80105684
: entry-80105684
    49 5 say-mode-wait
    end-script
    ;

\ $80105694
: entry-80105694
    nozz as-this-one
    this-one 0 200 face
    this-one wait-turned
    this-one 50 5 talk
    matthew 2 0 face
    this-one wait-turned
    matthew 51 6 talk
    this-one 52 5 talk
    matthew 53 4 talk
    10 frames wait
    end-script
    ;

\ $80105700
: entry-80105700
    54 5 say-mode-wait
    55 5 say-mode-wait
    56 5 say-mode-wait
    matthew 36 37 0 anim-then
    matthew wait-anim
    matthew 134 137 0 anim-then
    matthew wait-anim
    10 picture-show
    57 3 say-mode-wait
    picture-hide
    10 frames wait
    end-script
    ;

\ $80105778
: entry-80105778
    3 screen-effect
    wait-effect
    119 ambient-start
    50 frames wait
    119 ambient-stop
    2 screen-effect
    wait-effect
    end-script
    ;

\ $801057AC
: entry-801057AC
    3 screen-effect
    wait-effect
    118 ambient-start
    50 frames wait
    118 ambient-stop
    2 screen-effect
    wait-effect
    end-script
    ;

: L-801057E0
    25 frames wait
    107 sound
    end-script
    ;

: L-801057F4
    7 frames wait
    107 sound
    end-script
    ;

\ $80105808
: entry-80105808
    -344 -1862 -428 349421 -4044 288121 544 camera-set
    matthew 349421 0 288121 set-pos
    matthew 0 1028 0 set-rot
    ' L-801057E0 start-event
    ovl001 $801032C8 native
    matthew 500 502 anim-at  matthew wait-anim
    10 frames wait
    20 5 say-mode-wait
    21 5 say-mode-wait
    10 frames wait
    ovl001 $801032E8 native
    ' L-801057F4 start-event
    matthew 501 0 anim
    matthew wait-anim
    ovl001 $80103308 native
    end-script
    ;

: L-801058C8
    12 0 say-mode-wait
    13 0 say-mode-wait
    14 0 say-mode-wait
    15 0 say-mode-wait
    16 0 say-mode-wait
    17 0 say-mode-wait
    1 picture-show
    18 3 say-mode-wait
    picture-hide
    10 frames wait
    ovl001 $801032E8 native
    ' L-801057F4 start-event
    matthew 501 0 anim
    matthew wait-anim
    ovl001 $80103308 native
    ovl001 $80103370 native
    end-script
    ;

: L-80105968
    19 5 say-mode-wait
    10 frames wait
    ovl001 $801032E8 native
    ' L-801057F4 start-event
    matthew 501 0 anim
    matthew wait-anim
    ovl001 $80103308 native
    end-script
    ;

\ $801059AC
: entry-801059AC
    -344 -1862 -428 349421 -4044 288121 544 camera-set
    matthew 349421 0 288121 set-pos
    matthew 0 1028 0 set-rot
    ' L-801057E0 start-event
    ovl001 $801032C8 native
    matthew 500 502 anim-at  matthew wait-anim
    10 frames wait
    10 5 say-mode-wait
    11 0 choice
    0 result= if  goto L-801058C8  then
    1 result= if  goto L-80105968  then
    end-script
    ;

\ $80105A50
: entry-80105A50
    58 5 say-mode-wait
    end-script
    ;
