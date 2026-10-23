\ ovl016.fs - area 016 overlay (CD.BIN 016_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl016
USING: events events.main ;

: L-80103260
    this-one 0 0 face
    matthew 109 0 face
    this-one 156 7 talk
    events.main:sub-80096E74
    matthew 157 0 talk
    matthew 158 6 talk
    5 frames wait
    3 screen-effect
    25 frames wait
    27 -1 379951 140909 494 0 warp
    ;

\ $801032D8
: entry-801032D8
    matthew 380034 -240 150722 set-pos
    109 actor as-this-one
    this-one 16 1 set-floor
    -482 -2594 -554 380485 -240 150719 4014 camera-set
    this-one 381396 -240 150471 set-pos
    this-one 381339 150889 walk-to
    this-one wait-stop
    this-one 60 0 through-door
    goto L-80103260
    ;

\ $80103358
: entry-80103358
    109 actor as-this-one
    this-one 16 1 set-floor
    -656 -2774 -1814 384951 -240 151204 2248 camera-set
    this-one 381675 -240 148026 set-pos
    this-one 383075 148844 walk-to
    this-one wait-stop
    this-one 59 0 through-door
    goto L-80103260
    ;

\ $801033C4
: entry-801033C4
    109 actor as-this-one
    this-one 16 1 set-floor
    -1142 -3890 -3326 385275 -240 142711 1024 camera-set
    this-one 71 0 anim
    this-one wait-anim
    goto L-80103260
    ;

\ $80103418
: entry-80103418
    109 actor as-this-one
    this-one 16 1 set-floor
    -1142 -3890 -3326 387504 -240 146144 1684 camera-set
    this-one 71 0 anim
    this-one wait-anim
    goto L-80103260
    ;

\ $8010346C
: entry-8010346C
    109 actor as-this-one
    this-one 16 1 set-floor
    -548 -2990 -440 382262 -240 151680 2048 camera-set
    this-one 383084 -240 141663 set-pos
    this-one 382468 141495 walk-to
    this-one wait-stop
    goto L-80103260
    ;

\ $801034CC
: entry-801034CC
    109 actor as-this-one
    this-one 16 1 set-floor
    -548 -2990 -440 381942 -240 140062 226 camera-set
    this-one 383762 -240 140477 set-pos
    this-one 383400 140921 walk-to
    this-one wait-stop
    this-one 58 1 through-door
    this-one 382737 141593 walk-to
    this-one wait-stop
    goto L-80103260
    ;

\ $80103548
: entry-80103548
    109 actor as-this-one
    this-one 16 1 set-floor
    -548 -2990 -440 381788 -240 143292 1587 camera-set
    this-one 383762 -240 140477 set-pos
    this-one 383400 140921 walk-to
    this-one wait-stop
    this-one 58 1 through-door
    this-one 382381 140752 walk-to
    this-one wait-stop
    goto L-80103260
    ;

\ $801035C4
: entry-801035C4
    -374 -1094 -1130 389609 -240 140578 5582 camera-set
    matthew 389577 140566 walk-to
    matthew wait-stop
    matthew 0 2048 0 set-rot
    matthew 70 0 anim
    matthew wait-anim
    matthew 72 1 anim
    96 say
    wait-text
    matthew 3502 1 anim
    97 say
    wait-text
    98 say
    wait-text
    99 2 say-mode-wait
    100 2 say-mode-wait
    101 2 say-mode-wait
    102 2 say-mode-wait
    103 2 say-mode-wait
    104 2 say-mode-wait
    105 2 say-mode-wait
    matthew 71 0 anim
    events.main:sub-80096E74
    matthew wait-anim
    106 say
    wait-text
    107 say
    wait-text
    matthew 389011 141022 walk-to
    matthew wait-stop
    end-script
    ;

\ $80103B2C
: entry-80103B2C
    110 actor as-this-one
    this-one 385357 0 149374 set-pos
    this-one 16 1 set-floor
    this-one 0 0 turn-to
    this-one 3502 1 anim
    end-script
    ;

\ $80103B6C
: entry-80103B6C
    110 actor as-this-one
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80103B88
: entry-80103B88
    110 actor as-this-one
    goto entry-80103B2C
    ;

\ $80103B98
: entry-80103B98
    110 actor as-this-one
    goto entry-80103B6C
    ;

\ $80103BA8
: entry-80103BA8
    110 actor as-this-one
    -656 -2738 -2318 385611 -240 149278 -500 camera-set
    matthew 74 7 talk
    this-one 75 6 talk
    matthew 76 4 talk
    this-one 77 1 talk
    matthew 81 1 anim
    15 frames wait
    matthew 0 1 anim
    this-one 78 0 talk
    matthew 79 5 talk
    this-one 80 2 talk
    this-one 81 1 talk
    matthew 82 7 talk
    this-one 83 4 talk
    this-one 84 1 talk
    matthew 85 3 talk
    this-one 86 0 talk
    matthew 87 7 talk
    this-one 88 5 talk
    matthew 89 5 talk
    this-one 90 6 talk
    matthew 91 1 talk
    this-one 92 2 talk
    this-one 93 0 talk
    matthew 94 1 talk
    goto entry-80103B2C
    ;

\ $80103C4C
: entry-80103C4C
    110 actor as-this-one
    matthew this-one face-each-other
    this-one 95 1 talk
    goto entry-80103B2C
    ;

: L-80103C6C
    -422 -1946 -2198 385840 -239 148365 2090 camera-set
    matthew 385825 0 148940 set-pos
    matthew 2168 0 turn-to
    winona 385696 0 148365 set-pos
    winona 1730 0 turn-to
    8 screen-effect
    wait-effect
    winona 57 0 talk
    matthew 58 6 talk
    winona 59 7 talk
    11 screen-effect
    wait-effect
    0 set-scene
    -548 -2990 -440 382209 -240 148726 1197 camera-set
    matthew 382447 -240 148610 set-pos
    winona 381981 -240 148163 set-pos
    matthew winona face-each-other
    10 screen-effect
    wait-effect
    winona as-this-one
    this-one 69 7 talk
    matthew 70 7 talk
    this-one 71 7 talk
    matthew 72 7 talk
    this-one 73 7 talk
    winona unride
    winona 382149 146660 walk-to
    winona wait-stop
    winona 0 0 0 set-pos
    main-exe $80047B7C native
    end-script
    ;

\ $80103DE0
: arrive-80103DE0
    ovl016 $8010316C native
    ovl016 $8010317C native-result
    0 result= if  goto L-80103C6C  then
    2 screen-effect
    16 minigame
    wait-minigame
    main-exe $80047B7C native
    end-script
    ;

\ $80103E1C
: entry-80103E1C
    52 say-wait-close
    end-script
    ;

\ $80103E28
: entry-80103E28
    53 say-wait-close
    end-script
    ;

\ $80103E34
: entry-80103E34
    54 winona say-by-wait
    12 minigame
    end-script
    ;

\ $80103E4C
: entry-80103E4C
    17 picture-show
    15 5 say-mode-wait
    16 5 say-mode-wait
    17 5 say-mode-wait
    18 5 say-mode-wait
    19 1 say-mode-wait
    20 5 say-mode-wait
    picture-hide
    end-script
    ;

\ $80103EA4
: entry-80103EA4
    17 picture-show
    21 5 say-mode-wait
    22 5 say-mode-wait
    19 1 say-mode-wait
    22 5 say-mode-wait
    23 5 say-mode-wait
    picture-hide
    end-script
    ;

\ $80103EF0
: entry-80103EF0
    155 5 say-mode-wait
    end-script
    ;

\ $80103F00
: entry-80103F00
    matthew 62 0 1 anim-then
    10 frames wait
    55 say-wait-close
    56 say-wait-close
    end-script
    ;

\ $80103FD8
: entry-80103FD8
    matthew 24 1 talk
    109 actor 25 8 talk
    end-script
    ;

\ $80103FF0
: entry-80103FF0
    109 actor 26 8 talk
    end-script
    ;

\ $80104038
: entry-80104038
    -236 -2594 -1526 389978 -240 140374 1778 camera-set
    11 say-wait-close
    end-script
    ;

\ $80104064
: entry-80104064
    winona 60 1 talk
    end-script
    ;

: L-80104078
    this-one 162 1 talk
    this-one 163 7 talk
    ovl016 $8010319C native
    end-script
    ;

\ $8010408C
: entry-8010408C
    winona as-this-one
    this-one 159 3 talk
    160 1 choice
    0 result= if  goto L-80104078  then
    this-one 161 6 talk
    end-script
    ;

\ $801040B8
: entry-801040B8
    lidds as-this-one
    matthew 381571 0 140542 set-pos
    matthew 0 1 anim
    matthew 0 765 0 set-rot
    this-one 382101 0 142392 set-pos
    this-one 0 1 anim
    -470 -2264 -62 382521 -240 141076 -3594 camera-set
    matthew 382351 140742 walk-to
    15 frames wait
    27 32767 actor say-by-wait
    this-one 382411 141342 walk-to
    matthew 382411 141342 -80 face-point
    matthew wait-turned
    this-one wait-stop
    matthew 89 0 anim
    28 matthew say-by-wait
    -560 -680 -620 382377 -264 141292 -3906 camera-set
    10464 1 say-mode-wait
    10465 1 say-mode-wait
    10466 1 say-mode-wait
    -470 -2264 -62 382521 -240 141076 -3594 camera-set
    29 this-one say-by
    this-one 86 0 anim
    wait-text
    30 matthew say-by
    wait-text
    20 frames wait
    31 matthew say-by
    matthew 88 0 anim
    wait-text
    32 this-one say-by
    this-one 86 0 anim
    wait-text
    33 this-one say-by
    this-one 90 0 anim
    wait-text
    34 matthew say-by
    matthew 89 0 anim
    wait-text
    35 this-one say-by
    this-one 90 0 anim
    wait-text
    36 matthew say-by
    matthew 88 0 anim
    wait-text
    37 this-one say-by
    this-one 83 84 1 anim-then
    wait-text
    38 this-one say-by
    wait-text
    40 this-one say-by
    this-one 85 86 0 anim-then
    wait-text
    41 this-one say-by
    this-one 90 0 anim
    wait-text
    this-one 3 0 anim
    this-one 382331 142982 -120 face-point
    this-one wait-turned
    this-one 382461 148402 walk-to
    30 frames wait
    main-exe $8002D5F4 native
    end-script
    ;

\ $801043D0
: entry-801043D0
    matthew 108 7 talk
    lidds 109 0 talk
    matthew 110 3 talk
    lidds 111 0 talk
    matthew 112 6 talk
    end-script
    ;

\ $80104418
: entry-80104418
    -548 -4040 -644 381667 -156 141615 -127 camera-set
    57 door-animate
    events.main:sub-80096CF0
    27 -1 380858 141096 3072 0 warp
    ;

\ $80104464
: entry-80104464
    -548 -4040 -644 381667 -156 141615 -127 camera-set
    56 door-animate
    events.main:sub-80096CF0
    27 -1 380858 141096 3072 0 warp
    ;

\ $801044B4
: entry-801044B4
    -494 -2198 -164 382581 -180 141576 1481 camera-set
    58 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $801044F0
: entry-801044F0
    -404 -2738 -410 383660 -312 140963 3948 camera-set
    58 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $8010452C
: entry-8010452C
    -422 -2012 -266 382477 -240 150216 5724 camera-set
    59 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80104568
: entry-80104568
    -494 -3170 -1058 383813 -239 149098 2724 camera-set
    59 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $801045A4
: entry-801045A4
    -512 -2882 -638 381678 -238 150942 3824 camera-set
    60 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $801045E0
: entry-801045E0
    -434 -2738 -278 380422 -238 150598 4096 camera-set
    60 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $8010461C
: entry-8010461C
    -434 -2738 -278 380422 -238 150598 4096 camera-set
    61 door-animate
    events.main:sub-80096CF0
    17 0 -1 -1 -1 0 warp
    ;

\ $80104708
: entry-80104708
    13 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $80104720
: entry-80104720
    14 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $80104738
: entry-80104738
    12 5 say-mode-wait
    5 frames wait
    end-script
    ;

\ $80104750
: entry-80104750
    0 5 say-mode-wait
    end-script
    ;

\ $80104760
: entry-80104760
    2 5 say-mode-wait
    end-script
    ;

\ $80104770
: entry-80104770
    3 5 say-mode-wait
    end-script
    ;

\ $80104780
: entry-80104780
    1 5 say-mode-wait
    end-script
    ;

\ $80104790
: entry-80104790
    4 5 say-mode-wait
    end-script
    ;

\ $801047A0
: entry-801047A0
    5 5 say-mode-wait
    end-script
    ;

\ $801047B0
: entry-801047B0
    6 5 say-mode-wait
    end-script
    ;

\ $801047C0
: entry-801047C0
    7 5 say-mode-wait
    end-script
    ;

\ $801047D0
: entry-801047D0
    8 5 say-mode-wait
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' arrive-80103DE0 $80103DE0 16 overlay-word!
