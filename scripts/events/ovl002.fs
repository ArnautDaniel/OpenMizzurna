\ ovl002.fs - area 002 overlay (CD.BIN 002_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl002
USING: events events.main ;

\ $80096DC8
: entry-80096DC8
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    end-script
    ;

\ $80104978
: entry-80104978
    257 1 call-pack-script
    end-script
    ;

\ $80104988
: entry-80104988
    257 2 call-pack-script
    end-script
    ;

: L-80104998
    5 frames wait
    3 screen-effect
    25 frames wait
    27 -1 104641 355004 -2552 0 warp
    ;

\ $801049CC
: entry-801049CC
    94 actor as-this-one
    -470 -2492 -1298 103389 0 359610 4594 camera-set
    events.main:sub-80096D74
    this-one 0 0 face
    -722 -2492 -866 105420 0 360120 2716 camera-set
    matthew 94 0 face
    this-one 518 7 talk
    events.main:sub-80096E74
    matthew 519 0 talk
    matthew 520 6 talk
    goto L-80104998
    ;

: L-80104A50
    this-one 518 7 talk
    matthew 519 0 talk
    events.main:sub-80096E74
    matthew 520 6 talk
    goto L-80104998
    ;

\ $80104A6C
: entry-80104A6C
    94 actor as-this-one
    -488 -2492 -56 98588 0 354380 752 camera-set
    this-one 101420 0 356324 set-pos
    this-one 101348 355425 walk-to
    this-one wait-stop
    this-one 0 200 face
    this-one wait-turned
    goto L-80104A50
    ;

\ $80104ACC
: entry-80104ACC
    94 actor as-this-one
    -470 -2492 -704 101710 0 350425 -8194 camera-set
    this-one 101567 359665 walk-to
    this-one wait-stop
    this-one 0 -200 face
    this-one wait-turned
    goto L-80104A50
    ;

\ $80104B1C
: entry-80104B1C
    94 actor as-this-one
    -734 -2126 -1004 99226 0 360629 -2096 camera-set
    this-one 18 0 through-door
    this-one 0 0 face
    goto L-80104A50
    ;

\ $80104B64
: entry-80104B64
    518 3 say-mode-wait
    matthew 519 0 talk
    events.main:sub-80096E74
    matthew 520 6 talk
    goto L-80104998
    ;

\ $80104B88
: entry-80104B88
    108 actor as-this-one
    -824 -1838 -3344 98673 0 356959 2010 camera-set
    this-one 20 0 through-door
    this-one 0 0 face
    goto L-80104A50
    ;

\ $80104BD0
: entry-80104BD0
    94 actor as-this-one
    -626 -2330 -1058 103306 0 353983 -2026 camera-set
    this-one 19 0 through-door
    this-one 0 0 face
    goto L-80104A50
    ;

\ $80106BA0
: entry-80106BA0
    -536 -2528 -704 99253 -560 352564 5 camera-set
    matthew 353619 set-z
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
    2 0 -1 -1 0 -2146407724 warp
    ;

: L-80104C18
    94 actor as-this-one
    this-one 100699 355986 walk-to
    this-one wait-stop
    this-one 0 0 face
    this-one 523 6 talk
    matthew 94 0 face
    matthew 522 7 talk
    matthew 99505 -1200 353622 set-pos
    goto entry-80106BA0
    ;

\ $80104C68
: entry-80104C68
    -560 -2492 -1316 100396 -1200 356261 -4482 camera-set
    goto L-80104C18
    ;

\ $80104C90
: entry-80104C90
    -680 -2492 -728 98818 -1200 354581 4728 camera-set
    goto L-80104C18
    ;

\ $80104CB8
: entry-80104CB8
    -470 -1700 -560 101889 -1200 359933 -1970 camera-set
    goto L-80104C18
    ;

: L-80104CE0
    this-one 0 0 face
    this-one 523 6 talk
    matthew 94 0 face
    matthew 522 7 talk
    3 screen-effect
    wait-effect
    0 set-scene
    matthew 99505 -1200 353622 set-pos
    2 screen-effect
    goto entry-80106BA0
    ;

\ $80104D34
: entry-80104D34
    -518 -2018 -500 98143 -1200 360217 1472 camera-set
    94 actor as-this-one
    this-one 22 0 through-door
    goto L-80104CE0
    ;

\ $80104D70
: entry-80104D70
    -512 -2438 -404 101038 -1200 350179 7320 camera-set
    94 actor as-this-one
    this-one 25 0 through-door
    goto L-80104CE0
    ;

\ $80104DAC
: entry-80104DAC
    -920 -2186 -1220 104183 -1200 350225 2906 camera-set
    94 actor as-this-one
    this-one 23 0 through-door
    goto L-80104CE0
    ;

\ $80104DE8
: entry-80104DE8
    -440 -2006 -176 104190 -1200 351576 3090 camera-set
    94 actor as-this-one
    this-one 24 0 through-door
    goto L-80104CE0
    ;

\ $80104E24
: entry-80104E24
    -842 -110 -4316 98863 -1200 356710 -1032 camera-set
    94 actor as-this-one
    this-one 21 0 through-door
    goto L-80104CE0
    ;

\ $80106A1C
: entry-80106A1C
    -566 -2594 -1436 98707 0 353815 6126 camera-set
    matthew 353800 set-z
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
    29 0 -1 -1 0 -2146408104 warp
    ;

\ $80104E60
: entry-80104E60
    entry-80106A1C
    94 actor as-this-one
    this-one 99735 0 355886 set-pos
    this-one -2044 0 turn-to
    this-one 72 1 anim
    -470 -2492 -398 99505 -1200 353622 -32 camera-set
    events.main:sub-80096D74
    this-one 521 7 talk
    matthew 522 0 talk
    matthew 180 300 turn-by
    matthew wait-turned
    goto entry-80106BA0
    ;

\ $80104EE0
: entry-80104EE0
    -470 -1952 -992 103511 0 359260 3434 camera-set
    matthew 45 7 talk
    176 actor 46 8 talk
    matthew 47 1 talk
    end-script
    ;

\ $80104F2C
: entry-80104F2C
    108 actor 103381 0 358978 set-pos
    108 actor 0 420 0 set-rot
    108 actor 745 1 anim
    108 actor 2 1 set-floor
    end-script
    ;

\ $80104F78
: entry-80104F78
    matthew 0 1 anim
    10 frames wait
    end-script
    ;

\ $80104F90
: entry-80104F90
    events.main:sub-80096EB4
    matthew 94 actor face-each-other
    94 actor 7 1 talk
    94 actor -98 0 turn-to
    94 actor 92 1 anim
    end-script
    ;

\ $80104FD0
: entry-80104FD0
    matthew 62 0 anim
    matthew wait-anim
    526 matthew say-by-wait
    527 matthew say-by-wait
    end-script
    ;

\ $80105204
: entry-80105204
    15 say-wait-close
    end-script
    ;

\ $80105210
: entry-80105210
    16 5 say-mode-wait
    end-script
    ;

\ $80105274
: entry-80105274
    267 0 call-pack-script
    end-script
    ;

\ $80105284
: entry-80105284
    268 0 call-pack-script
    ovl002 $8002CC24 native
    20 frames wait
    end-script
    ;

\ $801052A4
: entry-801052A4
    255 0 call-pack-script
    give-control
    this-one 19 0 1 anim-then
    this-one 98656 349770 walk-to
    this-one wait-stop
    this-one 0 1024 0 set-rot
    this-one 70 72 1 anim-then
    end-script
    ;

\ $80105308
: entry-80105308
    255 1 call-pack-script
    end-script
    ;

\ $80105318
: entry-80105318
    256 0 call-pack-script
    end-script
    ;

\ $80105328
: entry-80105328
    256 1 call-pack-script
    end-script
    ;

\ $80105338
: entry-80105338
    255 2 call-pack-script
    end-script
    ;

\ $80105348
: entry-80105348
    266 0 call-pack-script
    end-script
    ;

\ $80105358
: entry-80105358
    266 1 call-pack-script
    end-script
    ;

\ $80105368
: entry-80105368
    124 sound
    10 frames wait
    matthew 81 0 anim
    matthew wait-anim
    103 5 say-mode-wait
    10 frames wait
    end-script
    ;

\ $801053A4
: entry-801053A4
    101 actor as-this-one
    isabella 183 actor-model
    matthew 103564 0 359455 set-pos
    matthew 0 350 0 set-rot
    matthew 0 1 anim
    matthew 30 0 set-floor
    isabella 105360 0 360959 set-pos
    isabella 1240 set-y
    isabella 0 6624 0 set-rot
    isabella 746 1 anim
    isabella 30 0 set-floor
    this-one 102818 0 360534 set-pos
    169 actor 102818 0 360534 set-pos
    169 actor 30 0 set-floor
    this-one 0 1 anim
    this-one 30 0 set-floor
    james 102818 0 360534 set-pos
    154 actor 102818 0 360534 set-pos
    154 actor 30 0 set-floor
    james 755 1 anim
    james 30 0 set-floor
    108 actor 0 0 0 set-pos
    barbara 0 0 0 set-pos
    -536 -2456 -470 103564 1200 359455 2510 camera-set
    matthew 104 1 talk
    20 frames wait
    101 actor 169 actor ignore-a
    james 154 actor ignore-a
    -326 -1736 -398 105400 1200 360931 504 camera-set
    105 0 say-mode-wait
    isabella 747 751 anim-at  isabella wait-anim
    matthew 103984 0 361160 set-pos
    -1082 -620 -1160 103342 1764 361333 1214 camera-set
    matthew 104564 361260 walk-to
    matthew wait-stop
    matthew 105204 360955 walk-to
    matthew 106 0 talk
    matthew wait-stop
    -488 -1196 -272 105204 1248 361045 1420 camera-set
    isabella 752 746 anim-at  isabella wait-anim
    107 0 say-mode-wait
    isabella 748 749 anim-at  isabella wait-anim
    matthew 17 18 anim-at  matthew wait-anim
    108 32767 actor 2 say-timed-by
    this-one 104898 0 361054 set-pos
    this-one 0 1220 0 set-rot
    -596 -1268 -344 105066 1200 360955 2740 camera-set
    matthew 0 1 anim
    matthew 101 -200 face
    2 frames wait
    this-one 159 116 1 anim-then
    8 frames wait
    14 music
    110 sound
    matthew 113 109 1 anim-then
    15 frames wait
    this-one 104788 0 361194 set-pos
    matthew 105424 0 360465 set-pos
    -416 -2276 -494 105030 1200 360805 484 camera-set
    isabella 750 751 1 anim-then
    this-one 118 0 anim-at  this-one wait-anim
    matthew 110 30 anim-at  matthew wait-anim
    matthew 109 0 talk
    this-one 110 3 talk
    matthew 31 0 anim-at  matthew wait-anim
    matthew 111 5 talk
    this-one 754 755 1 anim-then
    117 sound
    this-one 112 0 talk
    matthew 113 0 talk
    this-one 114 0 talk
    this-one 115 0 talk
    matthew 116 5 talk
    117 101 actor 1 say-timed-by-wait
    isabella 746 1 anim
    116 sound
    101 actor 725 726 anim-at  101 actor wait-anim
    this-one 118 0 talk
    james 103454 0 359575 set-pos
    james 0 630 0 set-rot
    -452 -548 -398 104034 1224 360049 -1508 camera-set
    map-sync
    matthew 119 0 talk
    -560 -2132 -542 104992 1200 360711 2628 camera-set
    james 756 0 anim-at  james wait-anim
    james 104574 359995 walk-to
    this-one 120 0 talk
    james wait-stop
    james 101 -200 face
    james wait-turned
    james 754 755 anim-at  james wait-anim
    this-one 121 0 talk
    -566 -692 -566 104704 1188 359709 -84 camera-set
    -372 -5 camera-angle
    this-one 103708 360834 2 30 walk-to-ex
    5 frames wait
    james -910 -30 turn-to
    this-one wait-stop
    this-one 103648 359874 5 60 walk-to-ex
    matthew 122 0 talk
    this-one wait-stop
    101 actor 4294967295 actor ignore-a
    this-one 0 0 0 set-pos
    169 actor 0 0 0 set-pos
    wait-camera-turn
    -470 -2492 -506 105955 1200 360673 5006 camera-set
    matthew 105075 360433 walk-to
    james 0 1 anim
    james 123 0 talk
    matthew 6 -200 face
    matthew wait-turned
    matthew 87 0 anim-at  matthew wait-anim
    3 screen-effect
    wait-effect
    matthew 105265 0 360838 set-pos
    matthew 0 8678 0 set-rot
    james 105094 0 361115 set-pos
    james 0 5356 0 set-rot
    -524 -1196 -182 105199 1200 360997 3206 camera-set
    2 screen-effect
    wait-effect
    matthew 729 0 anim-at  matthew wait-anim
    matthew 124 7 talk
    isabella 125 0 talk
    matthew 17 18 anim-at  matthew wait-anim
    matthew 126 0 talk
    isabella 747 746 1 anim-then
    17 music
    isabella 127 0 talk
    matthew 19 0 1 anim-then
    james 128 0 talk
    matthew 129 0 talk
    12 music
    isabella 747 746 1 anim-then
    isabella 130 0 talk
    matthew 131 5 talk
    isabella 747 746 1 anim-then
    isabella 132 0 talk
    isabella 747 746 1 anim-then
    isabella 133 0 talk
    matthew 134 5 talk
    15 frames wait
    isabella 748 749 anim-at  isabella wait-anim
    isabella 135 0 talk
    matthew 136 0 talk
    isabella 137 0 talk
    isabella 138 0 talk
    10 frames wait
    isabella 139 0 talk
    matthew 140 5 talk
    isabella 141 0 talk
    james 142 0 talk
    james 14 15 anim-at  james wait-anim
    james 143 0 talk
    matthew 17 18 1 anim-then
    matthew 144 0 talk
    isabella 145 0 talk
    3 screen-effect
    wait-effect
    james 4294967295 actor ignore-a
    154 actor 0 0 0 set-pos
    27 -1 -1 -1 -1 -2146386896 warp
    ;

\ $80105ED8
: entry-80105ED8
    107 actor as-this-one
    matthew 146 7 talk
    this-one 147 0 talk
    matthew 148 5 talk
    this-one 149 0 talk
    matthew 150 4 talk
    this-one 151 0 talk
    matthew 152 5 talk
    this-one 153 0 talk
    this-one 154 0 talk
    this-one 155 0 talk
    matthew 156 6 talk
    this-one 157 0 talk
    matthew 158 3 talk
    this-one 159 0 talk
    this-one 160 0 talk
    end-script
    ;

\ $80105F98
: entry-80105F98
    matthew 161 7 talk
    107 actor 162 0 talk
    matthew 163 4 talk
    107 actor 164 0 talk
    end-script
    ;

\ $80105FD4
: entry-80105FD4
    matthew 165 7 talk
    samuel 166 0 talk
    matthew 167 4 talk
    end-script
    ;

\ $80106000
: entry-80106000
    samuel as-this-one
    matthew 168 7 talk
    this-one 169 0 talk
    matthew 170 5 talk
    this-one 171 0 talk
    matthew 172 7 talk
    this-one 173 0 talk
    matthew 174 3 talk
    this-one 175 0 talk
    end-script
    ;

\ $8010606C
: entry-8010606C
    samuel as-this-one
    matthew 176 5 talk
    this-one 177 0 talk
    matthew 178 7 talk
    this-one 179 0 talk
    matthew 180 3 talk
    this-one 181 0 talk
    matthew 182 2 talk
    end-script
    ;

\ $801060CC
: entry-801060CC
    samuel as-this-one
    matthew 183 7 talk
    this-one 184 0 talk
    matthew 185 5 talk
    this-one 186 0 talk
    matthew 187 7 talk
    this-one 188 0 talk
    matthew 189 3 talk
    this-one 190 0 talk
    matthew 191 2 talk
    this-one 192 0 talk
    matthew 193 7 talk
    this-one 194 0 talk
    matthew 195 4 talk
    end-script
    ;

\ $80106174
: entry-80106174
    lidds 196 0 talk
    matthew 197 6 talk
    end-script
    ;

\ $80106194
: entry-80106194
    barbara as-this-one
    this-one 198 0 talk
    matthew 199 4 talk
    this-one 200 0 talk
    matthew 201 7 talk
    this-one 202 0 talk
    this-one 203 0 talk
    this-one 204 0 talk
    this-one 205 0 talk
    this-one 206 0 talk
    matthew 207 4 talk
    this-one 208 0 talk
    matthew 209 3 talk
    this-one 210 0 talk
    end-script
    ;

\ $8010623C
: entry-8010623C
    barbara as-this-one
    matthew 211 5 talk
    this-one 212 0 talk
    this-one 213 0 talk
    this-one 214 0 talk
    matthew 215 4 talk
    this-one 216 0 talk
    matthew 217 7 talk
    this-one 218 0 talk
    end-script
    ;

\ $801062A8
: entry-801062A8
    morgan as-this-one
    matthew 219 5 talk
    this-one 220 0 talk
    matthew 221 7 talk
    this-one 222 0 talk
    matthew 223 5 talk
    this-one 224 0 talk
    matthew 225 4 talk
    this-one 226 0 talk
    end-script
    ;

\ $80106314
: entry-80106314
    cohen as-this-one
    matthew 227 5 talk
    this-one 228 0 talk
    matthew 229 6 talk
    this-one 230 0 talk
    matthew 231 7 talk
    this-one 232 0 talk
    matthew 233 4 talk
    end-script
    ;

\ $80106374
: entry-80106374
    matthew 234 5 talk
    10 actor 235 0 talk
    matthew 236 2 talk
    matthew 237 7 talk
    10 actor 238 0 talk
    matthew 239 4 talk
    end-script
    ;

\ $801063C8
: entry-801063C8
    james as-this-one
    matthew 17 5 talk
    this-one 18 0 talk
    matthew 19 7 talk
    this-one 20 0 talk
    matthew 21 6 talk
    this-one 22 0 talk
    matthew 23 2 talk
    this-one 24 0 talk
    end-script
    ;

\ $80106434
: entry-80106434
    james as-this-one
    this-one 25 0 talk
    matthew 26 6 talk
    this-one 27 0 talk
    matthew 28 1 talk
    this-one 29 0 talk
    matthew 30 3 talk
    this-one 31 0 talk
    end-script
    ;

\ $80106494
: entry-80106494
    james as-this-one
    matthew 32 5 talk
    this-one 33 0 talk
    this-one 34 0 talk
    matthew 35 7 talk
    this-one 36 0 talk
    matthew 37 4 talk
    this-one 38 0 talk
    matthew 39 5 talk
    this-one 40 0 talk
    matthew 41 6 talk
    this-one 42 0 talk
    matthew 43 7 talk
    this-one 44 0 talk
    end-script
    ;

\ $8010653C
: entry-8010653C
    -548 -2738 -716 100603 12 358889 2856 camera-set
    18 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80106578
: entry-80106578
    -512 -2738 -716 99979 0 358772 1373 camera-set
    18 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $801065B4
: entry-801065B4
    -512 -2738 -716 102061 60 352321 239 camera-set
    19 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $801065F0
: entry-801065F0
    -512 -2738 -716 102555 0 352348 -1397 camera-set
    19 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $8010662C
: entry-8010662C
    -500 -2414 -428 98835 24 354934 505 camera-set
    20 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80106668
: entry-80106668
    -320 -2486 -1124 99171 -60 355737 1571 camera-set
    20 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $801066A4
: entry-801066A4
    -380 -3968 -416 99122 -1200 355942 -768 camera-set
    21 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $801066E0
: entry-801066E0
    -512 -2774 -1166 99427 -1200 355905 1495 camera-set
    21 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $8010671C
: entry-8010671C
    -458 -2450 -284 101012 -1200 358534 31220 camera-set
    22 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80106758
: entry-80106758
    -422 -2480 -842 100457 -1200 359223 1292 camera-set
    22 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80106794
: entry-80106794
    -476 -2666 -242 101906 -1200 352040 1790 camera-set
    23 door-animate
    events.main:sub-80096CF0
    4 set-scene
    end-script
    ;

\ $801067D0
: entry-801067D0
    -512 -2414 -446 102291 -1200 350193 -804 camera-set
    23 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $8010680C
: entry-8010680C
    -476 -2666 -242 101906 -1200 352040 1790 camera-set
    24 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $80106848
: entry-80106848
    xa-stop
    -512 -3386 -440 102425 -1200 351745 3008 camera-set
    24 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80106888
: entry-80106888
    -512 -2162 -410 101066 -1092 351074 2476 camera-set
    25 door-animate
    events.main:sub-80096CF0
    5 set-scene
    end-script
    ;

\ $801068C4
: entry-801068C4
    -422 -2738 -428 100627 -1200 351041 -3484 camera-set
    25 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80106900
: entry-80106900
    -470 -2630 -200 103465 1200 359017 -8 camera-set
    2 set-scene
    end-script
    ;

\ $8010692C
: entry-8010692C
    -470 -2630 -200 103465 1200 359017 -8 camera-set
    1 set-scene
    end-script
    ;

\ $80106D1C
: entry-80106D1C
    -566 -2594 -1436 98707 1200 353815 6126 camera-set
    matthew 353359 set-z
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
    2 0 -1 -1 0 -2146407336 warp
    ;

\ $80106E98
: entry-80106E98
    -536 -2528 -704 99253 640 352564 5 camera-set
    matthew 353619 set-z
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
    30 1 -1 -1 0 -2146406956 warp
    ;

: L-801072A8
    5 frames wait
    3 screen-effect
    wait-effect
    29 0 -1 -1 0 -2146406052 warp
    ;

: L-801072DC
    5 frames wait
    3 screen-effect
    wait-effect
    30 1 -1 -1 0 -2146405912 warp
    ;

: L-80106F50
    matthew 100471 0 353994 set-pos
    matthew 0 set-y
    matthew 0 0 0 set-rot
    -554 -2924 -164 100710 84 354270 1404 camera-set
    1 1 elevator-doors
    20 frames wait
    matthew 100471 354614 walk-to
    matthew wait-stop
    end-script
    ;

\ $80107310
: entry-80107310
    matthew 100908 0 354642 set-pos
    matthew 0 2048 0 set-rot
    -554 -2924 -164 100710 84 354270 1404 camera-set
    matthew 62 0 anim
    matthew wait-anim
    1 1 elevator-doors
    matthew 100471 354564 walk-to
    matthew wait-stop
    matthew 100471 353994 walk-to
    matthew wait-stop
    matthew 0 0 turn-to
    30 frames wait
    0 2 choice
    0 result= if  goto L-801072A8  then
    1 result= if  goto L-801072DC  then
    2 result= if  goto L-80106F50  then
    end-script
    ;

: L-80107274
    5 frames wait
    3 screen-effect
    wait-effect
    2 0 -1 -1 0 -2146406192 warp
    ;

: L-80106FD0
    matthew 100471 0 353994 set-pos
    matthew -1200 set-y
    matthew 0 0 0 set-rot
    -554 -2924 -164 100710 -1116 354270 1404 camera-set
    1 1 elevator-doors
    20 frames wait
    matthew 100471 354614 walk-to
    matthew wait-stop
    end-script
    ;

\ $801073EC
: entry-801073EC
    matthew 100908 0 354642 set-pos
    matthew 0 2048 0 set-rot
    -554 -2924 -164 100710 -1116 354270 1404 camera-set
    matthew 62 0 anim
    matthew wait-anim
    1 1 elevator-doors
    matthew 100471 354564 walk-to
    matthew wait-stop
    matthew 100471 353994 walk-to
    matthew wait-stop
    matthew 0 0 turn-to
    30 frames wait
    1 2 choice
    0 result= if  goto L-80107274  then
    1 result= if  goto L-801072DC  then
    2 result= if  goto L-80106FD0  then
    end-script
    ;

: L-80107050
    matthew 100471 0 353994 set-pos
    matthew 1200 set-y
    matthew 0 0 0 set-rot
    -554 -2924 -164 100710 1284 354270 1404 camera-set
    1 1 elevator-doors
    20 frames wait
    matthew 100471 354614 walk-to
    matthew wait-stop
    end-script
    ;

\ $801074C8
: entry-801074C8
    matthew 100908 0 354642 set-pos
    matthew 0 2048 0 set-rot
    -554 -2924 -164 100710 1284 354270 1404 camera-set
    matthew 62 0 anim
    matthew wait-anim
    1 1 elevator-doors
    matthew 100471 354564 walk-to
    matthew wait-stop
    matthew 100471 353994 walk-to
    matthew wait-stop
    matthew 0 0 turn-to
    30 frames wait
    2 2 choice
    0 result= if  goto L-801072A8  then
    1 result= if  goto L-80107274  then
    2 result= if  goto L-80107050  then
    end-script
    ;

\ $801075C0
: entry-801075C0
    4 5 say-mode-wait
    5 5 say-mode-wait
    end-script
    ;

\ $801075DC
: entry-801075DC
    12 5 say-mode-wait
    end-script
    ;

\ $801075EC
: entry-801075EC
    13 5 say-mode-wait
    end-script
    ;

\ $801075FC
: entry-801075FC
    14 5 say-mode-wait
    end-script
    ;

\ $801077E8
: entry-801077E8
    258 0 call-pack-script
    end-script
    ;

\ $801077F8
: entry-801077F8
    258 1 call-pack-script
    end-script
    ;
