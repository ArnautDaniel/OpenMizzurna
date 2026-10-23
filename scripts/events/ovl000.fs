\ ovl000.fs - area 000 overlay (CD.BIN 000_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl000
USING: events events.main ;

\ $80096DC8
: entry-80096DC8
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    end-script
    ;

\ $80096DE4
: entry-80096DE4
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    10019 say-wait-close
    end-script
    ;

: L-80104DE4
    matthew 448 6 talk
    8 actor 449 0 talk
    matthew 450 0 talk
    5 frames wait
    3 screen-effect
    25 frames wait
    8 actor place-at-stop
    27 -1 249754 136458 0 0 warp
    ;

\ $80104E44
: entry-80104E44
    matthew 8 0 face
    -512 -1568 -812 251069 -360 135134 1320 camera-set
    8 actor 447 0 talk
    -812 -2396 -1310 250455 -360 134035 3464 camera-set
    goto L-80104DE4
    ;

\ $80104EA8
: entry-80104EA8
    -734 -2432 -746 252660 -360 133506 7054 camera-set
    8 actor 3 0 through-door
    8 actor 0 0 face
    matthew 8 0 face
    8 actor 447 0 talk
    goto L-80104DE4
    ;

\ $80104F08
: entry-80104F08
    -1404 -2036 -2906 248804 -360 131235 -544 camera-set
    8 actor 4 1 through-door
    8 actor 0 0 face
    matthew 8 0 face
    8 actor 447 0 talk
    goto L-80104DE4
    ;

: L-80104F68
    5 frames wait
    3 screen-effect
    25 frames wait
    27 -1 249754 136458 0 0 warp
    ;

\ $80104F9C
: entry-80104F9C
    10 actor as-this-one
    10 actor 249686 133520 walk-to
    this-one wait-stop
    -782 -2906 -1412 249142 840 132574 964 camera-set
    10 actor 249860 132896 walk-to
    this-one wait-stop
    10 actor 0 0 face
    this-one 451 7 talk
    matthew 10 0 face
    matthew 452 0 talk
    goto L-80104F68
    ;

\ $8010501C
: entry-8010501C
    10 actor as-this-one
    this-one 5 1 through-door
    -942 -1660 -2894 246765 -360 132608 3864 camera-set
    10 actor 0 0 face
    this-one 451 7 talk
    matthew 10 0 face
    matthew 452 0 talk
    goto L-80104F68
    ;

\ $80105088
: entry-80105088
    8 actor as-this-one
    -776 -1856 -1202 249893 -360 134203 4756 camera-set
    this-one 304 8 talk
    matthew 8 0 face
    this-one 305 8 talk
    matthew 2038 0 turn-to
    end-script
    ;

\ $801050E4
: entry-801050E4
    10 actor as-this-one
    10 actor 249686 133520 walk-to
    this-one wait-stop
    -782 -2906 -1412 249142 840 132574 964 camera-set
    10 actor 249860 132896 walk-to
    this-one wait-stop
    10 actor 0 0 face
    this-one 453 7 talk
    this-one 454 6 talk
    matthew 455 1 talk
    3 screen-effect
    wait-effect
    0 set-scene
    matthew 250063 -360 134951 set-pos
    matthew 6072 0 turn-to
    matthew 0 1 set-floor
    2 screen-effect
    wait-effect
    ovl000 $8010388C native
    end-script
    ;

\ $801051B0
: entry-801051B0
    -470 -2200 -1160 250218 59 131332 3720 camera-set
    matthew 132228 set-z
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
    5 set-scene
    -470 -2200 -1160 249120 810 132169 984 camera-set
    matthew 249376 0 131800 set-pos
    matthew 0 0 0 set-rot
    matthew 520 set-y
    matthew wait-anim
    ovl000 $80103810 native
    10 screen-effect
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 8 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 0 0 set-floor
    end-script
    ;

\ $80105320
: entry-80105320
    -470 -2200 -1160 249120 810 132169 984 camera-set
    matthew 132371 set-z
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
    1 set-scene
    -470 -2200 -1160 250218 59 131332 3720 camera-set
    matthew 250163 0 131740 set-pos
    matthew 0 0 0 set-rot
    matthew -120 set-y
    matthew wait-anim
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
    matthew 0 1 set-floor
    end-script
    ;

\ $80105488
: entry-80105488
    matthew 251299 0 133238 set-pos
    matthew 0 2048 0 set-rot
    -518 -2264 -662 250969 -360 133382 1040 camera-set
    matthew 62 0 anim
    matthew wait-anim
    30 matthew say-by-wait
    end-script
    ;

\ $8010550C
: entry-8010550C
    mel 456 1 talk
    end-script
    ;

\ $801056A4
: entry-801056A4
    matthew 247353 0 132986 set-pos
    matthew 0 2048 0 set-rot
    matthew 0 1 anim
    camera-follow
    morgan 248376 0 131107 set-pos
    morgan 0 0 0 set-rot
    morgan 301 1 anim
    39 morgan 2 say-timed-by
    matthew 30 walk-forward
    35 frames wait
    matthew 1 -70 face
    camera-fixed
    -206 -2260 -1358 248195 -300 130584 6348 camera-set
    31 frames wait
    matthew idle
    40 matthew say-by
    wait-text
    morgan 303 0 anim
    morgan wait-anim
    morgan 304 1 anim
    41 morgan say-by
    morgan 305 0 anim
    morgan wait-anim
    morgan 301 1 anim
    wait-text
    42 matthew say-by
    matthew 87 0 anim
    wait-text
    43 morgan say-by
    morgan 303 0 anim
    morgan wait-anim
    morgan 304 1 anim
    wait-text
    44 matthew say-by
    matthew 83 0 anim
    matthew 86 0 anim
    wait-text
    morgan 305 0 anim
    morgan wait-anim
    45 morgan say-by
    morgan 306 0 anim
    morgan wait-anim
    morgan 307 1 anim
    wait-text
    46 matthew say-by
    matthew 83 0 anim
    matthew 88 0 anim
    wait-text
    47 morgan say-by
    wait-text
    48 matthew say-by
    matthew 88 0 anim
    wait-text
    morgan 308 301 0 anim-then
    morgan wait-anim
    morgan 303 304 1 anim-then
    49 morgan say-by
    wait-text
    50 matthew say-by
    matthew 87 0 anim
    matthew 82 0 anim
    wait-text
    51 morgan say-by
    wait-text
    -512 -1848 -1044 247698 -432 132207 -503 camera-set
    hudson 247490 0 133325 set-pos
    hudson 0 2048 0 set-rot
    hudson 0 1 set-floor
    hudson 4 1 through-door
    morgan 305 301 1 anim-then
    hudson 0 2048 0 set-rot
    hudson 30 walk-forward
    30 frames wait
    -1196 1440 -1404 248329 324 130970 1826 camera-set
    matthew 17 18 1 anim-then
    hudson idle
    hudson 248350 131551 walk-to
    52 morgan 2 say-timed-by
    hudson wait-stop
    hudson 1 130 face
    -1484 -1692 -1152 248047 348 132440 -214 camera-set
    10 frames wait
    3 1 say-mode-wait
    4 1 say-mode-wait
    5 1 say-mode-wait
    hudson 53 3 talk
    -1196 1440 -1404 248329 324 130970 1826 camera-set
    54 morgan say-by
    wait-text
    55 hudson say-by
    hudson 89 0 anim
    wait-text
    56 morgan say-by
    wait-text
    hudson 14 15 1 anim-then
    20 frames wait
    matthew 19 17 0 anim-then
    matthew wait-anim
    matthew 18 1 anim
    57 morgan say-by
    wait-text
    hudson 16 0 anim
    hudson wait-anim
    58 hudson say-by
    nozz 89 0 anim
    wait-text
    15 music
    59 matthew say-by
    matthew 19 0 anim
    matthew wait-anim
    wait-text
    60 morgan say-by
    wait-text
    hudson 61 6 talk
    hudson 62 7 talk
    63 morgan say-by
    wait-text
    64 hudson say-by
    nozz 89 0 anim
    wait-text
    morgan 303 304 1 anim-then
    65 morgan say-by
    wait-text
    15 music
    morgan 66 0 talk
    67 matthew say-by
    wait-text
    -1196 1440 -1404 248329 324 130970 1826 camera-set
    morgan 305 301 0 anim-then
    morgan wait-anim
    hudson 247409 132937 walk-to
    morgan 302 71 0 anim-then
    morgan wait-anim
    morgan 0 3072 0 set-rot
    morgan 30 walk-forward
    20 frames wait
    morgan idle
    morgan 247409 131686 walk-to
    morgan 0 100 face
    30 frames wait
    matthew 1 150 face
    hudson 1 -180 face
    68 morgan say-by
    wait-text
    hudson 180 180 turn-by
    10 frames wait
    hudson 4 2 through-door
    hudson 257435 0 133222 set-pos
    morgan 247409 132937 walk-to
    morgan wait-stop
    morgan 4 2 through-door
    morgan 257435 0 133222 set-pos
    morgan arrive
    hudson arrive
    end-script
    ;

\ $80105E64
: entry-80105E64
    153 actor as-this-one
    this-one 14 actor-model
    matthew 250063 0 135476 set-pos
    matthew 0 1 anim
    matthew 0 2170 0 set-rot
    this-one 249590 0 133702 set-pos
    this-one 0 1 anim
    this-one 0 1 set-floor
    -254 -3488 -974 249403 -360 134526 2823 camera-set
    this-one 3 1 through-door
    this-one 249600 134572 walk-to
    matthew 69 7 talk
    matthew 70 5 talk
    this-one 71 4 14 talk-to
    matthew 72 3 talk
    this-one 17 18 1 anim-then
    this-one 73 0 14 talk-to
    this-one 74 0 14 talk-to
    matthew 75 0 talk
    this-one 19 0 anim
    this-one wait-anim
    this-one 76 5 14 talk-to
    matthew 77 3 talk
    this-one 78 0 14 talk-to
    matthew 79 6 talk
    30 frames wait
    this-one 87 0 anim
    this-one wait-anim
    this-one 249638 135550 walk-to
    this-one wait-stop
    this-one 1 0 through-door
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80106048
: entry-80106048
    mel as-this-one
    5 set-scene
    matthew 0 0 set-floor
    this-one 0 0 set-floor
    matthew 252287 0 132572 set-pos
    matthew 0 1 anim
    matthew 0 2048 0 set-rot
    this-one 252287 0 132032 set-pos
    this-one 0 1 anim
    this-one 0 0 0 set-rot
    -512 -1574 -368 252251 840 132212 2492 camera-set
    this-one 312 313 1 anim-then
    this-one 80 0 talk
    matthew 81 3 talk
    this-one 314 0 anim
    this-one wait-anim
    this-one 82 5 talk
    this-one 83 1 talk
    matthew 84 6 talk
    this-one 85 5 talk
    this-one 86 1 talk
    this-one 87 5 talk
    matthew 88 6 talk
    15 music
    this-one 89 3 talk
    matthew 90 7 talk
    this-one 91 5 talk
    matthew 92 7 talk
    this-one 93 5 talk
    matthew 94 5 talk
    this-one 312 313 1 anim-then
    this-one 95 0 talk
    this-one 314 0 anim
    this-one wait-anim
    end-script
    ;

: L-80106264
    dennis 249543 135216 walk-to
    dennis wait-stop
    dennis 14 15 1 anim-then
    end-script
    ;

: L-80106294
    dennis 249638 135550 walk-to
    dennis wait-stop
    dennis 1 0 through-door
    end-script
    ;

\ $801062C0
: entry-801062C0
    153 actor 1 actor-model
    153 actor as-this-one
    matthew 250063 0 135476 set-pos
    matthew 0 1 anim
    matthew 0 2170 0 set-rot
    this-one 249590 0 133702 set-pos
    this-one 0 1 anim
    this-one 0 1 set-floor
    dennis 249573 0 134826 set-pos
    dennis 0 1 set-floor
    dennis 0 1 anim
    dennis 0 1110 0 set-rot
    mel 249833 0 134696 set-pos
    mel 0 1 set-floor
    mel 0 1 anim
    mel 0 -965 0 set-rot
    -254 -3488 -974 249403 -360 134526 2823 camera-set
    map-sync
    this-one 3 1 through-door
    mel 96 3 talk
    dennis 97 5 talk
    mel 98 1 talk
    dennis 99 7 talk
    ['] L-80106264 start-event
    mel 14 15 1 anim-then
    mel 100 0 talk
    mel 0 1 anim
    mel 0 0 face
    mel 312 313 1 anim-then
    mel 101 0 talk
    dennis 13 200 face
    dennis 0 1 anim
    dennis 103 5 talk
    10 frames wait
    ['] L-80106294 start-event
    mel 314 0 1 anim-then
    mel 104 0 talk
    mel 249663 135296 walk-to
    matthew 14 15 1 anim-then
    mel wait-stop
    mel 14 15 1 anim-then
    mel 102 0 talk
    mel 249638 135550 walk-to
    mel wait-stop
    mel 1 0 through-door
    this-one 249590 134712 walk-to
    matthew 16 0 anim
    matthew wait-anim
    mel -2146928900 ending-schedule
    dennis -2146944844 ending-schedule
    matthew 105 5 talk
    this-one 106 3 talk
    matthew 107 1 talk
    this-one 83 84 1 anim-then
    20 frames wait
    this-one 85 0 anim-at  this-one wait-anim
    this-one 108 6 talk
    10 frames wait
    this-one 249638 135550 walk-to
    this-one wait-stop
    this-one 1 0 through-door
    153 actor 0 0 0 set-pos
    end-script
    ;

\ $80106668
: arrive-80106668
    -1 music
    153 actor as-this-one
    this-one 1 actor-model
    102 actor 15 actor-model
    matthew 250303 0 135056 set-pos
    matthew 0 1 set-floor
    matthew 0 1 anim
    matthew 0 1120 0 set-rot
    102 actor 249583 0 135896 set-pos
    153 actor 0 1 set-floor
    102 actor 0 1 set-floor
    -128 -2576 -758 250303 -492 134880 2631 camera-set
    8 screen-effect
    wait-effect
    10 frames wait
    matthew 87 0 anim
    matthew wait-anim
    10 frames wait
    matthew 17 0 anim
    matthew wait-anim
    matthew -560 -200 turn-to
    -578 -1976 -1202 250159 -360 134958 3495 camera-set
    102 actor 0 -2 through-door
    102 actor 0 -200 face
    this-one 249590 0 133702 set-pos
    this-one 0 1 anim
    102 actor 109 0 15 talk-to
    matthew 110 6 talk
    102 actor 111 7 15 talk-to
    matthew 112 4 talk
    102 actor 83 84 anim-at  102 actor wait-anim
    10 frames wait
    matthew 17 0 anim
    matthew wait-anim
    -254 -3488 -974 249403 -360 134526 2823 camera-set
    matthew 255 -200 face
    102 actor 85 0 anim
    102 actor 153 200 face
    this-one 3 1 through-door
    matthew 113 5 talk
    this-one 114 4 talk
    10 frames wait
    this-one 17 18 anim-at  this-one wait-anim
    this-one 115 0 talk
    10 frames wait
    3 screen-effect
    wait-effect
    13 music
    20 frames wait
    102 actor 247909 0 134926 set-pos
    102 actor 0 1024 0 set-rot
    102 actor 169 1 anim
    this-one 248140 0 134562 set-pos
    this-one 0 0 0 set-rot
    this-one 0 1 anim
    matthew 248323 0 135086 set-pos
    matthew 0 2144 0 set-rot
    matthew 0 1 anim
    -758 -2540 -770 248989 -228 134912 2964 camera-set
    2 screen-effect
    wait-effect
    10 frames wait
    102 actor 117 0 talk
    -842 -1532 -2948 248419 -276 135122 2364 camera-set
    this-one 87 0 anim
    this-one wait-anim
    this-one 116 7 talk
    matthew 118 7 talk
    this-one 119 7 talk
    matthew 120 6 talk
    this-one 121 5 talk
    this-one 122 7 talk
    this-one 123 7 talk
    this-one 17 18 anim-at  this-one wait-anim
    this-one 124 0 talk
    102 actor 125 0 15 talk-to
    this-one 19 0 anim
    this-one wait-anim
    this-one 126 3 talk
    matthew 127 1 talk
    this-one 128 2 talk
    matthew 129 7 talk
    this-one 85 0 anim
    this-one 130 0 talk
    matthew 131 7 talk
    this-one 132 0 talk
    matthew 133 7 talk
    this-one 134 4 talk
    matthew 135 5 talk
    this-one 136 3 talk
    this-one 137 7 talk
    matthew 83 84 1 anim-then
    10 frames wait
    102 actor 170 167 1 anim-then
    20 frames wait
    matthew 85 0 anim-at  matthew wait-anim
    matthew 138 7 talk
    this-one 139 3 talk
    matthew 140 7 talk
    this-one 141 4 talk
    102 actor 166 0 anim-at  102 actor wait-anim
    -776 -1928 412 248287 -396 134366 2120 camera-set
    153 actor 248752 134736 walk-to  153 actor wait-stop
    153 actor 10 0 through-door
    153 actor 0 0 0 set-pos
    102 actor 248752 134736 walk-to  102 actor wait-stop
    102 actor 10 0 through-door
    102 actor 0 0 0 set-pos
    ovl000 $801037B8 native
    end-script
    ;

\ $80106C5C
: entry-80106C5C
    mel as-this-one
    5 set-scene
    matthew 0 0 set-floor
    this-one 0 0 set-floor
    matthew 252330 0 132539 set-pos
    matthew 0 1 anim
    matthew 0 2048 0 set-rot
    this-one 252287 0 132032 set-pos
    this-one 0 1 anim
    this-one 0 0 0 set-rot
    -494 -1394 -710 252438 876 131963 1880 camera-set
    this-one 312 313 1 anim-then
    this-one 142 0 talk
    matthew 143 7 talk
    this-one 314 0 anim
    this-one 144 0 talk
    matthew 145 7 talk
    this-one 146 0 talk
    matthew 147 7 talk
    this-one 148 5 talk
    matthew 149 7 talk
    this-one 150 1 talk
    10 frames wait
    matthew 151 7 talk
    10 frames wait
    15 music
    this-one 152 3 talk
    matthew 153 5 talk
    this-one 154 3 talk
    matthew 155 0 talk
    this-one 156 5 talk
    matthew 157 7 talk
    this-one 158 5 talk
    10 frames wait
    end-script
    ;

\ $80106E30
: entry-80106E30
    8 actor as-this-one
    this-one 251023 0 135208 set-pos
    this-one 0 3072 0 set-rot
    this-one 72 1 anim
    matthew 250443 0 135188 set-pos
    matthew 0 1024 0 set-rot
    matthew 0 1 anim
    -536 -1316 -776 250563 -360 135044 577 camera-set
    10 frames wait
    matthew 136 51 anim-at  matthew wait-anim
    matthew 159 0 talk
    this-one 160 0 talk
    this-one 71 0 anim
    this-one wait-anim
    this-one 59 60 anim-at  this-one wait-anim
    this-one 161 0 talk
    matthew 141 59 0 anim-then
    matthew wait-anim
    matthew 674 0 turn-to
    -470 -1892 -758 251343 -360 135404 -1568 camera-set
    this-one 61 0 anim-at  this-one wait-anim
    this-one 250933 135418 walk-to
    this-one wait-stop
    this-one 251223 135568 walk-to
    this-one wait-stop
    this-one 0 0 turn-to
    this-one 59 0 anim
    this-one wait-anim
    this-one 60 1 anim
    113 sound
    30 frames wait
    this-one 61 0 anim
    this-one wait-anim
    this-one -20 -200 turn-by
    this-one wait-turned
    this-one 40 0 anim
    this-one wait-anim
    108 sound
    this-one 41 0 anim
    this-one wait-anim
    this-one 20 200 turn-by
    this-one wait-turned
    this-one 83 84 1 anim-then
    50 frames wait
    this-one 162 0 talk
    matthew 163 7 talk
    8 actor 85 0 anim-at  8 actor wait-anim
    this-one 0 -200 face
    this-one 164 3 talk
    matthew 165 5 talk
    8 actor 14 15 anim-at  8 actor wait-anim
    this-one 166 0 talk
    8 actor 16 0 anim-at  8 actor wait-anim
    this-one 167 7 talk
    matthew 168 7 talk
    this-one 169 4 talk
    matthew 83 84 anim-at  matthew wait-anim
    20 frames wait
    matthew 85 0 anim-at  matthew wait-anim
    matthew 170 7 talk
    this-one 87 0 anim
    this-one wait-anim
    9 screen-effect
    wait-effect
    this-one 251023 0 135208 set-pos
    this-one 0 3072 0 set-rot
    this-one 72 1 anim
    matthew 250443 0 135188 set-pos
    matthew 0 1024 0 set-rot
    matthew 0 1 anim
    -536 -1316 -776 250563 -360 135044 577 camera-set
    8 screen-effect
    wait-effect
    this-one 171 0 talk
    end-script
    ;

: L-80107248
    102 actor 249638 135550 walk-to
    102 actor wait-stop
    102 actor 1 0 through-door
    102 actor 0 0 0 set-pos
    end-script
    ;

\ $80107288
: entry-80107288
    153 actor 1 actor-model
    102 actor 21 actor-model
    153 actor as-this-one
    matthew 250063 0 135476 set-pos
    matthew 0 1 anim
    matthew 0 2170 0 set-rot
    this-one 249590 0 133702 set-pos
    this-one 0 1 set-floor
    this-one 0 1 anim
    102 actor 249573 0 134226 set-pos
    102 actor 0 1 set-floor
    102 actor 0 1 anim
    102 actor 0 1110 0 set-rot
    -668 -1568 -1346 250253 -360 134745 -552 camera-set
    10 frames wait
    matthew 238 0 talk
    102 actor 249573 135306 walk-to
    102 actor wait-stop
    102 actor 14 15 anim-at  102 actor wait-anim
    10 frames wait
    102 actor 146 0 talk
    102 actor 16 0 anim-at  102 actor wait-anim
    ['] L-80107248 start-event
    -1296 -24 camera-angle
    matthew 249943 134936 walk-to
    this-one 3 1 through-door
    102 actor 0 0 0 set-pos
    this-one 249883 134566 walk-to
    this-one wait-stop
    matthew 239 5 talk
    this-one 240 4 talk
    this-one 241 7 talk
    this-one 83 84 anim-at  this-one wait-anim
    this-one 242 0 talk
    this-one 243 0 talk
    this-one 244 0 talk
    this-one 245 0 talk
    matthew 246 6 talk
    this-one 85 0 anim-at  this-one wait-anim
    this-one 247 4 talk
    this-one 248 7 talk
    this-one 249 6 talk
    this-one 250 3 talk
    matthew 251 7 talk
    this-one 252 4 talk
    this-one 253 3 talk
    matthew 254 1 talk
    matthew 255 5 talk
    this-one 256 4 talk
    this-one 257 3 talk
    this-one 87 0 anim-at  this-one wait-anim
    this-one 249638 135550 walk-to
    this-one wait-stop
    this-one 1 0 through-door
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80107584
: entry-80107584
    matthew 250414 -360 135176 set-pos
    matthew 0 1024 0 set-rot
    -398 -1454 -1544 250682 -348 135141 408 camera-set
    matthew 258 0 talk
    8 actor 259 8 talk
    matthew 260 7 talk
    -362 -446 -356 250904 -348 135141 648 camera-set
    10 frames wait
    0 1 say-mode-wait
    1 1 say-mode-wait
    2 1 say-mode-wait
    8 actor 261 8 talk
    -398 -1454 -1544 250682 -348 135141 408 camera-set
    matthew 262 7 talk
    8 actor 263 8 talk
    matthew 264 7 talk
    8 actor 265 8 talk
    8 actor 266 8 talk
    end-script
    ;

\ $801076CC
: entry-801076CC
    8 actor as-this-one
    matthew 250414 -360 135176 set-pos
    matthew 0 1024 0 set-rot
    -398 -1454 -1544 250682 -348 135141 408 camera-set
    matthew 267 7 talk
    this-one 268 0 talk
    matthew 269 5 talk
    this-one 270 0 talk
    matthew 271 4 talk
    this-one 272 0 talk
    end-script
    ;

\ $80107768
: entry-80107768
    matthew 250414 -360 135176 set-pos
    matthew 0 1024 0 set-rot
    -398 -1454 -1544 250682 -348 135141 408 camera-set
    matthew 273 7 talk
    8 actor 274 8 talk
    matthew 275 5 talk
    8 actor 276 8 talk
    matthew 277 3 talk
    8 actor 278 8 talk
    8 actor 279 8 talk
    8 actor 280 8 talk
    matthew 281 7 talk
    end-script
    ;

\ $80107844
: entry-80107844
    8 actor as-this-one
    this-one 283 8 talk
    this-one 284 8 talk
    matthew 285 7 talk
    this-one 286 8 talk
    this-one 287 8 talk
    end-script
    ;

\ $8010788C
: entry-8010788C
    8 actor as-this-one
    this-one 288 8 talk
    matthew 289 3 talk
    this-one 290 8 talk
    matthew 291 7 talk
    this-one 292 8 talk
    matthew 293 6 talk
    this-one 294 8 talk
    matthew 295 1 talk
    this-one 296 8 talk
    end-script
    ;

\ $80107970
: entry-80107970
    -470 -2200 -1160 249634 -360 135381 0 camera-set
    1 door-animate
    events.main:sub-80096CF0
    27 -1 249754 136458 0 0 warp
    ;

\ $801079C0
: entry-801079C0
    -380 -2900 -740 249785 -360 134408 6738 camera-set
    3 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $801079FC
: entry-801079FC
    -380 -1210 -1598 251166 -396 132988 3564 camera-set
    6 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80107A38
: entry-80107A38
    -470 -64 -1718 246627 -408 132988 -6 camera-set
    5 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80107A74
: entry-80107A74
    -470 -2200 -1160 249634 -360 135381 0 camera-set
    0 door-animate
    events.main:sub-80096CF0
    27 -1 249754 136458 0 0 warp
    ;

\ $80107AC4
: entry-80107AC4
    -434 -1600 -1682 247327 -252 133041 -556 camera-set
    4 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80107B00
: entry-80107B00
    -530 -2834 -224 250492 852 132648 1035 camera-set
    7 door-animate
    events.main:sub-80096CF0
    end-script
    ;

\ $80107B34
: entry-80107B34
    -530 -2834 -224 250492 852 132648 1035 camera-set
    11 door-animate
    events.main:sub-80096CF0
    end-script
    ;

\ $80107B68
: entry-80107B68
    -596 -1978 -206 249830 -180 133800 -4596 camera-set
    3 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80107BA4
: entry-80107BA4
    -650 -1018 -2342 247371 -432 133195 -2936 camera-set
    4 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80107BE0
: entry-80107BE0
    -656 -1300 -3776 251679 1272 132868 1316 camera-set
    6 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $80107C1C
: entry-80107C1C
    -482 -1792 -1874 246870 -204 133111 1524 camera-set
    5 door-animate
    events.main:sub-80096CF0
    4 set-scene
    end-script
    ;

\ $80107C58
: entry-80107C58
    -488 -1510 -1706 248889 -348 134662 463 camera-set
    2 door-animate
    events.main:sub-80096CF0
    end-script
    ;

\ $80107C8C
: entry-80107C8C
    -758 -1052 -2084 248680 -360 134527 4580 camera-set
    10 door-animate
    events.main:sub-80096CF0
    end-script
    ;

\ $80107CC0
: entry-80107CC0
    -44 -890 -398 249087 -552 135314 7645 camera-set
    6 say
    wait-text
    end-script
    ;

\ $80107CF0
: entry-80107CF0
    -44 -890 -398 249087 -552 135314 7645 camera-set
    7 say
    wait-text
    end-script
    ;

\ $80107D20
: entry-80107D20
    -44 -890 -398 249087 -552 135314 7645 camera-set
    107 sound
    8 say
    wait-text
    end-script
    ;

\ $80107D58
: entry-80107D58
    -44 -890 -398 249087 -552 135314 7645 camera-set
    107 sound
    9 say
    wait-text
    end-script
    ;

\ $80107D90
: entry-80107D90
    -44 -890 -398 249087 -552 135314 7645 camera-set
    107 sound
    10 say
    wait-text
    end-script
    ;

\ $80107DC8
: entry-80107DC8
    11 8 actor say-by
    wait-text
    end-script
    ;

\ $80107DDC
: entry-80107DDC
    21 say
    wait-text
    22 say
    wait-text
    end-script
    ;

\ $80107DF8
: entry-80107DF8
    25 say
    wait-text
    end-script
    ;

\ $80107E08
: entry-80107E08
    26 say
    wait-text
    end-script
    ;

\ $80107E18
: entry-80107E18
    27 say
    wait-text
    28 say
    wait-text
    end-script
    ;

\ $80107E34
: entry-80107E34
    29 say
    wait-text
    end-script
    ;

\ $80107E44
: entry-80107E44
    23 say
    wait-text
    24 say
    wait-text
    end-script
    ;

\ $80107E64
: entry-80107E64
    31 say
    wait-text
    end-script
    ;

\ $80107E74
: entry-80107E74
    matthew 248399 -360 135275 set-pos
    matthew 104 0 face
    -284 -2540 -680 248849 -360 135143 693 camera-set
    matthew 48 50 anim-at  matthew wait-anim
    matthew 32 0 talk
    matthew 49 0 1 anim-then
    107 sound
    matthew 104 actor ignore-a
    wait-text
    matthew 8 150 face
    matthew wait-turned
    33 8 actor say-by
    matthew 52 53 1 anim-then
    wait-text
    34 matthew say-by
    wait-text
    35 8 actor say-by
    matthew 54 51 1 anim-then
    wait-text
    36 matthew say-by
    matthew 55 0 1 anim-then
    matthew 4294967295 actor ignore-a
    104 actor 0 0 0 set-pos
    wait-text
    107 sound
    2 picture-show
    37 3 say-mode-wait
    picture-hide
    10 frames wait
    end-script
    ;

\ $80107FF0
: entry-80107FF0
    -434 -1096 -1010 250558 -456 134763 -3484 camera-set
    12 8 actor say-by
    wait-text
    13 matthew say-by
    wait-text
    14 8 actor say-by
    wait-text
    end-script
    ;

\ $80108044
: entry-80108044
    -434 -1096 -1010 250558 -456 134763 -3484 camera-set
    15 8 actor say-by
    wait-text
    16 matthew say-by
    wait-text
    17 8 actor say-by
    wait-text
    end-script
    ;

\ $80108098
: entry-80108098
    -434 -1096 -1010 250558 -456 134763 -3484 camera-set
    18 matthew say-by
    wait-text
    19 8 actor say-by
    wait-text
    end-script
    ;

\ $80108278
: entry-80108278
    326 2 say-mode-wait
    327 2 say-mode-wait
    328 2 say-mode-wait
    329 2 say-mode-wait
    end-script
    ;

\ $801082AC
: entry-801082AC
    330 2 say-mode-wait
    331 2 say-mode-wait
    332 2 say-mode-wait
    333 2 say-mode-wait
    end-script
    ;

\ $801082E0
: entry-801082E0
    358 2 say-mode-wait
    359 2 say-mode-wait
    360 2 say-mode-wait
    end-script
    ;

\ $80108308
: entry-80108308
    358 2 say-mode-wait
    359 2 say-mode-wait
    end-script
    ;

\ $80108324
: entry-80108324
    306 3 say-mode-wait
    307 3 say-mode-wait
    308 3 say-mode-wait
    309 3 say-mode-wait
    end-script
    ;

\ $80108358
: entry-80108358
    334 2 say-mode-wait
    335 2 say-mode-wait
    336 2 say-mode-wait
    end-script
    ;

\ $80108380
: entry-80108380
    337 2 say-mode-wait
    338 2 say-mode-wait
    339 2 say-mode-wait
    340 2 say-mode-wait
    end-script
    ;

\ $801083B4
: entry-801083B4
    355 2 say-mode-wait
    356 2 say-mode-wait
    357 2 say-mode-wait
    end-script
    ;

\ $801083DC
: entry-801083DC
    314 2 say-mode-wait
    315 2 say-mode-wait
    316 2 say-mode-wait
    317 2 say-mode-wait
    end-script
    ;

\ $80108410
: entry-80108410
    318 2 say-mode-wait
    319 2 say-mode-wait
    320 2 say-mode-wait
    321 2 say-mode-wait
    end-script
    ;

\ $80108444
: entry-80108444
    341 2 say-mode-wait
    342 2 say-mode-wait
    343 2 say-mode-wait
    end-script
    ;

\ $8010846C
: entry-8010846C
    344 2 say-mode-wait
    345 2 say-mode-wait
    346 2 say-mode-wait
    end-script
    ;

\ $80108494
: entry-80108494
    347 2 say-mode-wait
    end-script
    ;

\ $801084A4
: entry-801084A4
    348 2 say-mode-wait
    349 2 say-mode-wait
    350 2 say-mode-wait
    end-script
    ;

\ $801084CC
: entry-801084CC
    322 2 say-mode-wait
    323 2 say-mode-wait
    324 2 say-mode-wait
    end-script
    ;

\ $801084F4
: entry-801084F4
    351 2 say-mode-wait
    352 2 say-mode-wait
    end-script
    ;

\ $80108510
: entry-80108510
    353 2 say-mode-wait
    354 2 say-mode-wait
    end-script
    ;

\ $8010852C
: entry-8010852C
    310 2 say-mode-wait
    311 2 say-mode-wait
    312 2 say-mode-wait
    313 2 say-mode-wait
    end-script
    ;

\ $80108560
: entry-80108560
    363 2 say-mode-wait
    364 2 say-mode-wait
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' entry-80106048 $80106048 0 overlay-word!
' arrive-80106668 $80106668 0 overlay-word!
