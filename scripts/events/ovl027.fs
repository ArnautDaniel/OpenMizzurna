\ ovl027.fs - area 027 overlay (CD.BIN 027_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl027
USING: events events.main ;

\ $80096BE4
: entry-80096BE4
    winona 54 1 talk
    end-script
    ;

\ $80109E54
: entry-80109E54
    106 actor as-this-one
    106 actor idle
    106 actor 0 0 face
    this-one 64 5 talk
    matthew 65 7 talk
    this-one 66 7 talk
    this-one 67 1 talk
    matthew 68 3 talk
    this-one 69 5 talk
    106 actor 578366 219816 walk-to
    20 frames wait
    70 2 say-mode-wait
    71 2 say-mode-wait
    end-script
    ;

\ $80109F04
: entry-80109F04
    winona 147 7 talk
    matthew 148 7 talk
    end-script
    ;

\ $80109F5C
: entry-80109F5C
    -600 -2850 -1024 329874 -4080 269309 3524 camera-set
    matthew 329850 -4082 269377 set-pos
    matthew 3072 0 turn-to
    106 actor 329539 -4080 269282 set-pos
    106 actor 1024 0 turn-to
    matthew 330396 269461 2 20 walk-to-ex
    106 actor 82 0 through-door
    106 actor 331741 268121 walk-to
    15 frames wait
    matthew 17 18 1 anim-then
    106 actor wait-stop
    end-script
    ;

: L-80109BB4
    matthew this-one camera-face
    45 0 camera-turn
    matthew this-one face-each-other
    this-one 180 0 turn-by
    this-one 177 0 anim
    this-one anim-hold
    matthew 176 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew this-one face-each-other
    end-script
    ;

: L-80109B84
    matthew this-one face-each-other
    matthew 91 92 1 anim-then
    this-one 94 95 1 anim-then
    end-script
    ;

: L-80109ADC
    45 frames wait
    0 12 effect-level
    40 frames wait
    12 0 effect-level
    15 frames wait
    -600 -2850 -700 0 0 0 0 camera-set
    matthew camera-at
    45 frames wait
    matthew 84 84 1 anim-then
    4 screen-effect
    wait-effect
    180 64 camera-turn
    matthew 99 0 talk
    wait-camera-turn
    matthew 85 0 1 anim-then
    matthew 100 0 talk
    end-script
    ;

\ $8010A178
: entry-8010A178
    112 actor as-this-one
    L-80109BB4
    L-80109B84
    this-one 102 0 talk
    matthew 103 0 talk
    this-one 104 0 talk
    matthew 105 0 talk
    this-one 106 0 talk
    L-80109B84
    matthew 107 0 talk
    this-one 108 0 talk
    matthew 109 0 talk
    this-one 110 0 talk
    this-one 111 0 talk
    this-one 112 0 talk
    matthew 113 0 talk
    this-one 114 0 talk
    matthew 115 0 talk
    this-one 116 0 talk
    matthew 117 0 talk
    this-one 118 0 talk
    matthew 119 0 talk
    16 0 effect-level
    15 screen-effect
    18 frames wait
    112 actor 0 0 0 set-pos
    -600 -2850 -700 116013 0 199722 -1414 camera-set
    L-80109ADC
    3 map-reveal
    end-script
    ;

\ $8010A240
: entry-8010A240
    153 actor 7 actor-model
    10 actor as-this-one
    -312 -2402 -1884 107289 -200 199727 2338 camera-set
    10 actor 106787 -200 199196 set-pos
    matthew 107419 -200 199613 set-pos
    153 actor 108883 -200 197954 set-pos
    matthew 10 actor face-each-other
    matthew 59 60 1 anim-then
    this-one 120 7 talk
    matthew 121 0 talk
    this-one 122 7 talk
    matthew 123 0 talk
    this-one 124 7 talk
    153 actor 107424 198992 walk-to  153 actor wait-stop
    153 actor 125 7 talk
    this-one 126 7 talk
    matthew 127 1 talk
    153 actor 128 5 talk
    matthew 129 6 talk
    this-one 130 7 talk
    matthew 131 4 talk
    153 actor 132 0 talk
    11 screen-effect
    wait-effect
    153 actor 0 0 0 set-pos
    8 0 106284 218754 2554 0 warp
    ;

\ $8010A358
: entry-8010A358
    153 actor 1 actor-model
    102 actor 7 actor-model
    153 actor 92199 0 53156 set-pos
    153 actor 0 1024 0 set-rot
    102 actor 92199 0 53156 set-pos
    102 actor 0 1024 0 set-rot
    -608 -3066 -164 92845 -200 53196 -1214 camera-set
    153 actor as-this-one
    2 object-anim
    153 actor 64 0 anim
    153 actor anim-hold
    this-one wait-anim
    153 actor 93075 52820 walk-to
    153 actor wait-stop
    102 actor as-this-one
    2 object-anim
    102 actor 64 0 anim
    102 actor anim-hold
    this-one wait-anim
    102 actor 93075 53340 walk-to
    morgan as-this-one
    matthew 133 0 talk
    this-one 134 0 talk
    102 actor 135 0 talk
    this-one 136 0 talk
    matthew 137 0 talk
    this-one 138 0 talk
    matthew 139 0 talk
    this-one 140 0 talk
    matthew 141 0 talk
    this-one 142 0 talk
    matthew 143 0 talk
    this-one 144 0 talk
    this-one 145 0 talk
    matthew 146 0 talk
    153 actor 96031 53419 walk-to
    102 actor 93031 53419 walk-to
    10 actor 93031 53419 walk-to
    30 frames wait
    11 screen-effect
    wait-effect
    153 actor idle
    153 actor 0 0 0 set-pos
    102 actor idle
    102 actor 0 0 0 set-pos
    10 actor idle
    10 actor 0 0 0 set-pos
    10 screen-effect
    wait-effect
    end-script
    ;

\ $8010A510
: entry-8010A510
    events.main:sub-80096E34
    matthew 62 0 anim
    matthew wait-anim
    0 matthew say-by-wait
    end-script
    ;

\ $8010A538
: entry-8010A538
    118 -1370 -2450 0 0 0 1761 camera-set
    winona camera-at
    winona 10022 3 talk
    end-script
    ;

\ $8010A574
: entry-8010A574
    118 -1370 -2450 0 0 0 1761 camera-set
    winona camera-at
    winona 151 1 talk
    end-script
    ;

: L-8010A5B0
    -1122 -7062 14 39110 -360 277133 2842 camera-set
    37 2 caption-for-nowait
    86 door-animate
    goto events.main:sub-80096CF0
    ;

\ $8010A5EC
: entry-8010A5EC
    cohen 0 ending-schedule
    101 5 say-mode-wait
    L-8010A5B0
    23 0 38762 277187 3072 -2146425432 warp
    ;

\ $8010A624
: entry-8010A624
    L-8010A5B0
    23 0 38762 277187 3072 0 warp
    ;

\ $8010A654
: entry-8010A654
    steve as-this-one
    this-one door-release
    -504 -3180 -984 175746 -4240 423252 -3176 camera-set
    this-one 79 1 talk
    3 screen-effect
    5 frames wait
    this-one 180 300 turn-by
    wait-effect
    3 0 -1 -1 -1 -2146423316 warp
    ;

\ $8010A6D0
: entry-8010A6D0
    17 actor as-this-one
    this-one door-release
    this-one 80 3 talk
    this-one 716 717 1 anim-then
    81 17 actor say-by
    20 frames wait
    this-one 82 7 talk
    3 screen-effect
    wait-effect
    this-one place-at-stop
    matthew 175758 0 423741 set-pos
    matthew 239 0 turn-to
    3 0 -1 -1 -1 0 warp
    ;

\ $8010A760
: entry-8010A760
    steve as-this-one
    this-one door-release
    this-one 10018 4 talk
    7 screen-effect
    5 frames wait
    this-one place-at-stop
    matthew 175758 0 423741 set-pos
    matthew 239 0 turn-to
    3 0 -1 -1 -1 0 warp
    ;

\ $8010A7C4
: entry-8010A7C4
    261 9 call-pack-script
    21 0 327798 267846 0 -2146415092 warp
    ;

\ $8010A810
: entry-8010A810
    121 actor as-this-one
    -996 -2850 -700 409623 -4000 166871 163 camera-set
    this-one 67 0 through-door
    this-one 411312 169408 walk-to  this-one wait-stop
    -996 -2850 -700 408187 -4000 168748 1910 camera-set
    this-one 409739 0 168210 set-pos
    this-one 408704 168288 walk-to  this-one wait-stop
    this-one 26 0 face
    this-one 60 6 talk
    this-one 408528 168768 walk-to  this-one wait-stop
    -600 -2850 -268 409384 -4000 170051 -5534 camera-set
    this-one 91 92 1 anim-then
    55 frames wait
    this-one 93 0 1 anim-then
    -600 -2634 -700 409281 -4000 168249 -3052 camera-set
    this-one 61 7 talk
    ovl027 $80106D04 native
    give-control
    this-one 411840 169776 walk-to  this-one wait-stop
    this-one 67 -2 through-door
    this-one 0 0 0 set-pos
    end-script
    ;

: L-8010A94C
    11 screen-effect
    wait-effect
    main-exe $8001A2C8 native
    103 actor 0 0 0 set-pos
    end-script
    ;

\ $8010A974
: entry-8010A974
    L-8010A94C
    9 2 336683 442607 2560 0 warp
    ;

\ $8010A998
: entry-8010A998
    L-8010A94C
    10 0 465755 462785 314 0 warp
    ;

\ $8010A9BC
: entry-8010A9BC
    62 say-wait-close
    end-script
    ;

\ $8010A9C8
: entry-8010A9C8
    -996 -5154 -430 144154 0 302339 3574 camera-set
    28 2 caption-for
    text-close
    11 screen-effect
    wait-effect
    7 0 -1 -1 -1 0 warp
    ;

\ $8010AA20
: entry-8010AA20
    -1578 -8448 -1684 54662 0 403395 4122 camera-set
    39 2 caption-for
    text-close
    11 screen-effect
    wait-effect
    26 0 56705 410553 544 0 warp
    ;

\ $8010AA78
: entry-8010AA78
    events.main:sub-80096E34
    32 door-animate
    events.main:sub-80096CF0
    5 0 112815 51313 -3192 -2146417132 warp
    ;

\ $8010AAAC
: entry-8010AAAC
    -2010 -9612 -376 464406 0 415091 -2781 camera-set
    32 2 caption-for-nowait
    45 door-animate
    events.main:sub-80096CF0
    11 0 466924 411843 2048 0 warp
    ;

\ $8010B470
: entry-8010B470
    106 actor 72 3 talk
    end-script
    ;

\ $8010B5E0
: entry-8010B5E0
    matthew 62 0 1 anim-then
    10 frames wait
    149 say-wait-close
    150 say-wait-close
    end-script
    ;

\ $8010B60C
: entry-8010B60C
    winona as-this-one
    this-one 57 7 talk
    this-one 58 7 talk
    this-one 59 7 talk
    main-exe $80047B7C native
    end-script
    ;

\ $8010B644
: arrive-8010B644
    camera-follow
    2 screen-effect
    wait-effect
    ovl027 $80106DA4 native
    end-script
    ;

\ $8010B660
: entry-8010B660
    337 matthew say-by-wait
    -1 music
    end-script
    ;

\ $8010B678
: entry-8010B678
    0 0 250063 135476 2048 -2146408856 warp
    ;

\ $8010B694
: arrive-8010B694
    map-sync
    103 actor 402 set-y
    2 screen-effect
    main-exe $8001A290 native
    end-script
    ;

\ $8010B6D4
: arrive-8010B6D4
    245 0 call-pack-script
    2 0 104609 357908 3400 -2146414048 warp
    ;

\ $8010B700
: arrive-8010B700
    245 1 call-pack-script
    -1 music
    -930 -8250 -280 250361 -336 135015 -2258 camera-set
    30 frames wait
    2 screen-effect
    wait-effect
    20 3 caption-for
    text-close
    3 screen-effect
    wait-effect
    main-exe $80047620 native
    end-script
    ;

\ $8010B770
: entry-8010B770
    247 0 call-pack-script
    -1122 -7062 14 39110 -360 277133 2842 camera-set
    matthew 0 0 0 set-pos
    cohen 0 0 0 set-pos
    2 screen-effect
    wait-effect
    37 3 caption-for
    text-close
    3 screen-effect
    wait-effect
    23 0 36889 277706 1024 -2146424436 warp
    ;

\ $8010B81C
: entry-8010B81C
    247 2 call-pack-script
    give-control
    this-one 92589 53201 walk-to
    this-one wait-stop
    this-one 0 3072 0 set-rot
    events.main:sub-80096D1C
    this-one place-at-stop
    21 map-reveal
    end-script
    ;

\ $8010B864
: entry-8010B864
    247 3 call-pack-script
    11 screen-effect
    wait-effect
    3 0 -1 -1 -1 -2146420120 warp
    ;

\ $8010B89C
: entry-8010B89C
    247 4 call-pack-script
    end-script
    ;

\ $8010B8AC
: arrive-8010B8AC
    261 2 call-pack-script
    21 0 329445 269064 1024 -2146411968 warp
    ;

\ $8010B8D8
: arrive-8010B8D8
    269 3 call-pack-script
    ovl027 $8010656C native
    2 map-reveal
    end-script
    ;

\ $8010B8F8
: entry-8010B8F8
    248 0 call-pack-script
    end-script
    ;

\ $8010B908
: entry-8010B908
    winona unride
    winona 411283 -4000 168749 set-pos
    matthew 410793 -4000 168339 set-pos
    matthew 0 1 anim
    matthew 15 0 face
    winona 0 1 anim
    winona 0 0 face
    -1060 -1114 -1060 411381 -3352 168313 -697 camera-set
    winona 0 7 talk
    matthew 1 6 talk
    winona 2 4 talk
    winona 83 84 1 anim-then
    30 frames wait
    winona 3 0 talk
    winona 85 0 anim
    winona wait-anim
    matthew 4 5 talk
    winona 87 0 anim
    winona 5 7 talk
    matthew 6 5 talk
    winona 86 0 anim
    winona wait-anim
    15 music
    winona 7 6 talk
    winona 8 7 talk
    matthew 9 7 talk
    winona 10 2 talk
    11 winona say-by-wait
    matthew 12 4 talk
    13 winona say-by-wait
    winona 85 86 0 anim-then
    wait-text
    matthew 14 7 talk
    -858 -5154 -490 410944 -4000 169021 646 camera-set
    20 frames wait
    winona 87 0 anim-at  winona wait-anim
    winona 19 6 talk
    winona 411823 169789 80 face-point
    winona 411865 169730 walk-to
    winona wait-stop
    winona 1024 200 turn-to
    winona wait-turned
    winona 67 -2 through-door
    winona 1 0 set-stop
    winona place-at-stop
    14 map-reveal
    end-script
    ;

\ $8010BB78
: arrive-8010BB78
    248 4 call-pack-script
    end-script
    ;

\ $8010BB88
: entry-8010BB88
    248 5 call-pack-script
    give-control
    winona wait-stop
    winona 67 -2 through-door
    winona 7 goto-stop
    end-script
    ;

\ $8010BBBC
: entry-8010BBBC
    248 2 call-pack-script
    end-script
    ;

\ $8010BBCC
: entry-8010BBCC
    256 2 call-pack-script
    end-script
    ;

\ $8010BBDC
: arrive-8010BBDC
    253 2 call-pack-script
    end-script
    ;

\ $8010BBEC
: entry-8010BBEC
    248 6 call-pack-script
    end-script
    ;

\ $8010BBFC
: entry-8010BBFC
    248 7 call-pack-script
    ovl027 $80106950 native
    wait-minigame
    ovl027 $801060BC native
    end-script
    ;

\ $8010BC20
: arrive-8010BC20
    252 3 call-pack-script
    end-script
    ;

\ $8010BC30
: arrive-8010BC30
    246 0 call-pack-script
    end-script
    ;

\ $8010BC50
: entry-8010BC50
    261 3 call-pack-script
    end-script
    ;

: L-8010BC60
    main-exe $8002E988 native
    end-script
    ;

\ $8010BC6C
: entry-8010BC6C
    261 6 call-pack-script
    1 result= if  goto L-8010BC60  then
    end-script
    ;

\ $8010BC88
: entry-8010BC88
    261 7 call-pack-script
    end-script
    ;

\ $8010BC98
: entry-8010BC98
    261 8 call-pack-script
    end-script
    ;

\ $8010BD0C
: entry-8010BD0C
    -870 -14838 -9128 90102 600 52491 -4826 camera-set
    27 2 caption-for-nowait
    36 door-animate
    events.main:sub-80096CF0
    6 0 89561 48037 0 0 warp
    ;

\ $8010BD88
: entry-8010BD88
    -1536 -8208 -1552 108799 0 352668 -4612 camera-set
    24 2 caption-for
    text-close
    11 screen-effect
    wait-effect
    2 0 104609 357908 0 0 warp
    ;

\ $8010BE04
: entry-8010BE04
    -1464 -13458 -1402 108826 -224 196713 -1332 camera-set
    29 2 caption-for-nowait
    38 door-animate
    events.main:sub-80096CF0
    8 0 106391 218783 2560 0 warp
    ;

\ $8010BE90
: entry-8010BE90
    -1062 -5052 -592 176007 -4310 421658 288 camera-set
    22 2 caption-for-nowait
    27 door-animate
    events.main:sub-80096CF0
    3 0 175642 423776 1024 0 warp
    ;

\ $8010BF0C
: entry-8010BF0C
    -1668 -11190 -250 249941 -132 134912 2300 camera-set
    20 2 caption-for-nowait
    0 door-animate
    events.main:sub-80096CF0
    0 0 249749 135314 2048 0 warp
    ;

\ $8010BF88
: entry-8010BF88
    -1464 -9228 -682 277782 -80 98926 2733 camera-set
    31 2 caption-for
    text-close
    11 screen-effect
    wait-effect
    13 0 278140 98409 2560 -2146422032 warp
    ;

\ $8010C004
: entry-8010C004
    -1374 -6954 -448 305375 -4058 305652 5536 camera-set
    35 2 caption-for-nowait
    71 door-animate
    events.main:sub-80096CF0
    31 0 306376 304878 1024 0 warp
    ;

\ $8010C080
: entry-8010C080
    -2604 -12300 -388 328357 -4202 269244 2744 camera-set
    36 2 caption-for-nowait
    82 door-animate
    events.main:sub-80096CF0
    21 0 329479 269371 3072 0 warp
    ;

\ $8010C0DC
: entry-8010C0DC
    -804 -1770 -436 334050 -3938 273681 2591 camera-set
    48 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $8010C114
: entry-8010C114
    -1068 -3354 -412 334963 -4010 272545 -2539 camera-set
    48 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $8010C18C
: entry-8010C18C
    -1056 -6342 -1588 334156 -34 437631 3896 camera-set
    30 2 caption-for-nowait
    40 door-animate
    events.main:sub-80096CF0
    9 0 332995 437418 -484 0 warp
    ;

\ $8010C208
: entry-8010C208
    -672 -4326 -136 345175 -4082 284381 4368 camera-set
    21 2 caption-for-nowait
    15 door-animate
    events.main:sub-80096CF0
    1 3 345196 285243 4586 0 warp
    ;

\ $8010C284
: entry-8010C284
    -282 -4542 -1468 345444 -4082 305108 -3177 camera-set
    45 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $8010C574
: entry-8010C574
    -900 -4620 -508 376433 216 139708 4750 camera-set
    34 2 caption-for-nowait
    56 door-animate
    events.main:sub-80096CF0
    16 0 381374 140564 1024 0 warp
    ;

\ $8010C5F0
: entry-8010C5F0
    -834 -3714 -376 411323 -3988 170252 5480 camera-set
    23 2 caption-for-nowait
    67 door-animate
    events.main:sub-80096CF0
    18 0 412146 169822 1024 0 warp
    ;

\ $8010C66C
: entry-8010C66C
    46 5 say-mode-wait
    matthew 468515 -8320 177984 set-pos
    matthew 0 1024 0 set-rot
    -528 -582 -1276 468575 -8320 177844 4403 camera-set
    matthew 48 50 anim-at  matthew wait-anim
    47 5 say-mode-wait
    matthew 49 0 anim-at  matthew wait-anim
    5 frames wait
    end-script
    ;

\ $8010C718
: entry-8010C718
    -1200 -5718 -250 462909 48 296760 -2694 camera-set
    33 2 caption-for-nowait
    48 door-animate
    events.main:sub-80096CF0
    12 0 468307 292409 2048 0 warp
    ;

\ $8010C7B4
: entry-8010C7B4
    -2388 -10698 26 591626 -25660 129647 688 camera-set
    38 2 caption-for-nowait
    90 door-animate
    events.main:sub-80096CF0
    24 0 591187 132255 1024 0 warp
    ;

\ $8010C810
: entry-8010C810
    -2388 -10698 26 591626 -25660 129647 688 camera-set
    38 2 caption-for-nowait
    93 door-animate
    events.main:sub-80096CF0
    24 1 592169 130486 1024 0 warp
    ;

\ the words the main executable's scripts go to here, by address
' arrive-8010B644 $8010B644 27 overlay-word!
' arrive-8010B694 $8010B694 27 overlay-word!
' arrive-8010B6D4 $8010B6D4 27 overlay-word!
' arrive-8010B700 $8010B700 27 overlay-word!
' arrive-8010B8AC $8010B8AC 27 overlay-word!
' arrive-8010B8D8 $8010B8D8 27 overlay-word!
' arrive-8010BB78 $8010BB78 27 overlay-word!
' arrive-8010BBDC $8010BBDC 27 overlay-word!
' arrive-8010BC20 $8010BC20 27 overlay-word!
' arrive-8010BC30 $8010BC30 27 overlay-word!
