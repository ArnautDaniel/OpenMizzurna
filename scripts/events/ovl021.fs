\ ovl021.fs - area 021 overlay (CD.BIN 021_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl021
USING: events events.main ;

\ $80104AFC
: entry-80104AFC
    matthew as-this-one
    this-one 326845 -4080 270511 set-pos
    rolein 327557 269934 walk-to
    rolein wait-stop
    rolein -1024 0 turn-to
    rolein 4561 0 anim
    rolein wait-anim
    rolein this-one ignore-a
    ovl021 $8010383C native
    rolein wait-stop
    rolein 1024 0 turn-to
    rolein 4559 anim-once
    rolein wait-anim
    rolein 4294967295 actor ignore-a
    1 frames wait
    ovl021 $8010389C native
    rolein 4560 0 anim
    rolein wait-anim
    246 rolein say-by
    wait-text
    end-script
    ;

\ $80104BE0
: entry-80104BE0
    matthew as-this-one
    ovl021 $8010383C native
    rolein wait-stop
    rolein 1024 0 turn-to
    rolein this-one ignore-a
    rolein 4516 0 anim
    rolein wait-anim
    rolein 4294967295 actor ignore-a
    this-one 326845 -4080 270511 set-pos
    end-script
    ;

\ $80104C50
: entry-80104C50
    matthew as-this-one
    matthew 4517 0 anim
    matthew wait-anim
    matthew 4563 0 anim
    matthew wait-anim
    matthew 4563 anim-once
    matthew wait-anim
    matthew 4564 0 anim
    matthew wait-anim
    matthew 119 1 anim
    end-script
    ;

\ $80104CB8
: entry-80104CB8
    matthew as-this-one
    matthew 4517 0 anim
    matthew wait-anim
    matthew this-one ignore-a
    matthew 4563 0 anim
    matthew wait-anim
    matthew 4563 anim-once
    matthew wait-anim
    matthew 4294967295 actor ignore-a
    matthew 4564 0 anim
    1 frames wait
    ovl021 $8010389C native
    matthew wait-anim
    matthew 119 1 anim
    end-script
    ;

\ $80104D4C
: entry-80104D4C
    matthew 22 0 face
    rolein 0 0 face
    -620 -1622 -1004 0 0 0 3422 camera-set
    rolein camera-at
    rolein 305 3 talk
    -512 -2738 -716 328570 -4080 270139 1802 camera-set
    matthew 306 6 talk
    rolein 307 0 talk
    5 frames wait
    3 screen-effect
    25 frames wait
    27 -1 329845 269364 1024 0 warp
    ;

\ $80104E0C
: arrive-80104E0C
    rolein as-this-one
    138 actor 0 0 0 set-pos
    139 actor 0 0 0 set-pos
    -836 -2738 -968 327772 -3780 268247 -210 camera-set
    rolein 327699 0 268320 set-pos
    rolein 0 0 face
    matthew 22 0 face
    40 frames wait
    8 screen-effect
    this-one 6 7 talk
    matthew 7 7 talk
    this-one 8 0 talk
    matthew 9 6 talk
    this-one 10 7 talk
    give-control
    rolein 327913 269469 walk-to
    this-one wait-stop
    rolein 1024 230 turn-to
    end-script
    ;

\ $80105024
: entry-80105024
    camera-follow
    rolein 327834 0 270334 set-pos
    rolein 0 1 anim
    rolein 0 1024 0 set-rot
    matthew 329550 0 269335 set-pos
    matthew 0 3072 0 set-rot
    matthew 328901 269379 walk-to
    matthew wait-stop
    matthew 328739 270144 walk-to
    matthew wait-stop
    matthew 328354 270184 walk-to
    camera-fixed
    -1430 -302 -1760 329362 -4524 270030 -895 camera-set
    matthew wait-stop
    matthew 3072 120 turn-to
    matthew wait-turned
    matthew 5 take-diner-seat
    matthew 4554 0 anim
    matthew anim-hold
    10 frames wait
    112 sound
    matthew wait-anim
    -1430 -302 -1760 329362 -4524 270030 -895 camera-set
    mel 329844 0 269454 set-pos
    mel 0 3072 0 set-rot
    matthew 11 9 talk
    rolein 12 6 talk
    -1382 -302 -1340 328174 -3192 270198 -775 camera-set
    3 1 say-mode-wait
    4 1 say-mode-wait
    5 1 say-mode-wait
    rolein 13 5 talk
    -1430 -302 -1760 329362 -4524 270030 -895 camera-set
    matthew 14 9 talk
    rolein 15 7 talk
    matthew 16 12 talk
    rolein 17 6 talk
    matthew 18 11 talk
    rolein 19 5 talk
    matthew 20 9 talk
    rolein 21 3 talk
    22 matthew say-by
    wait-text
    -1496 -446 -1526 330646 -3192 269298 -1015 camera-set
    329248 -3192 269346 18 camera-move
    mel 82 -2 through-door
    mel 30 walk-forward
    27 frames wait
    mel idle
    -554 -1976 -782 328138 -3972 270022 3536 camera-set
    matthew 4509 4510 1 anim-then
    mel 3 take-diner-seat
    mel 328350 0 269675 set-pos
    mel 0 3072 0 set-rot
    mel 4507 0 anim
    mel anim-hold
    10 frames wait
    112 sound
    mel wait-anim
    mel 119 1 anim
    rolein 327744 270074 walk-to
    rolein 13 -120 face
    rolein 23 7 talk
    mel 20 40 turn-by
    -536 -1890 -698 329068 -3948 268864 1520 camera-set
    0 1 say-mode-wait
    1 1 say-mode-wait
    2 1 say-mode-wait
    mel 24 0 talk
    -554 -1976 -782 328138 -3972 270022 3536 camera-set
    rolein 25 7 talk
    mel 26 0 talk
    rolein 27 7 talk
    mel 28 0 talk
    rolein 29 7 talk
    mel 30 0 talk
    rolein 31 7 talk
    mel 32 0 talk
    matthew -45 -80 turn-by
    matthew 33 0 talk
    mel 4512 4513 1 anim-then
    mel 34 0 talk
    rolein 17 18 1 anim-then
    matthew 35 0 talk
    mel 36 0 talk
    matthew 37 0 talk
    mel 38 0 talk
    mel 4514 119 1 anim-then
    rolein 19 0 anim
    rolein wait-anim
    rolein 39 7 talk
    mel 40 0 talk
    rolein 41 7 talk
    mel 42 0 talk
    mel 43 0 talk
    matthew 44 0 talk
    15 music
    mel 45 0 talk
    matthew 46 0 talk
    mel 4512 4513 1 anim-then
    mel 47 0 talk
    mel 4514 119 1 anim-then
    mel 48 0 talk
    -1790 -572 -2036 329572 -3252 270420 -1327 camera-set
    mel as-this-one
    events.main:sub-80096FB0
    mel 4 0 1 anim-then
    mel 180 120 turn-by
    mel wait-turned
    mel 30 walk-forward
    5 frames wait
    mel idle
    rolein 49 7 talk
    mel 17 18 1 anim-then
    mel 50 0 talk
    mel 19 0 anim
    mel 329492 269508 walk-to
    mel wait-stop
    mel 0 1024 0 set-rot
    mel 82 0 through-door
    rolein 51 7 talk
    matthew 4511 119 1 anim-then
    matthew 52 0 talk
    rolein 0 -80 face
    rolein 53 7 talk
    matthew 45 40 turn-by
    2 map-reveal
    ovl021 $8010497C native
    end-script
    ;

: L-801057E0
    mel as-this-one
    this-one 329039 269719 walk-to
    this-one wait-stop
    this-one 329492 269508 walk-to
    this-one wait-stop
    this-one 82 0 through-door
    this-one -2146929500 ending-schedule
    end-script
    ;

\ $80105828
: entry-80105828
    -1 music
    mel as-this-one
    mel leave-diner-seat
    matthew 329379 0 269299 set-pos
    matthew 0 3072 0 set-rot
    this-one 328809 0 270019 set-pos
    rolein 328629 0 269799 set-pos
    rolein 13 0 face
    rolein 0 1 anim
    this-one 22 0 face
    -746 -1994 -1196 329319 -4080 268975 -400 camera-set
    map-sync
    10 frames wait
    rolein 54 1 talk
    matthew 14 15 1 anim-then
    this-one 55 5 talk
    rolein 56 3 talk
    matthew 16 0 1 anim-then
    this-one 17 18 anim-at  this-one wait-anim
    this-one 57 0 talk
    this-one 19 0 anim
    this-one wait-anim
    this-one 58 5 talk
    ['] L-801057E0 start-event
    10 frames wait
    matthew 329009 269279 walk-to
    matthew wait-stop
    matthew 22 0 face
    matthew 14 15 1 anim-then
    70 frames wait
    matthew 16 0 1 anim-then
    matthew 59 7 talk
    rolein 60 3 talk
    matthew 61 1 talk
    rolein 62 2 talk
    10 frames wait
    ['] events.main:sub-8009954C start-event
    end-script
    ;

: L-80106070
    80 138 actor 2 say-timed-by-wait
    text-hold
    text-close
    10 frames wait
    81 rolein 1 say-timed-by-wait
    text-hold
    82 morgan 1 say-timed-by-wait
    text-hold
    83 matthew 1 say-timed-by-wait
    text-hold
    text-close
    35 frames wait
    84 139 actor 2 say-timed-by-wait
    text-hold
    85 138 actor 2 say-timed-by-wait
    text-hold
    text-close
    10 frames wait
    86 138 actor 2 say-timed-by-wait
    text-hold
    87 morgan 2 say-timed-by-wait
    text-hold
    88 139 actor 2 say-timed-by-wait
    text-hold
    89 138 actor 2 say-timed-by-wait
    text-hold
    90 morgan 1 say-timed-by-wait
    text-close
    45 frames wait
    91 138 actor 2 say-timed-by-wait
    text-hold
    92 rolein 2 say-timed-by-wait
    text-hold
    93 138 actor 2 say-timed-by-wait
    text-hold
    text-close
    26 frames wait
    94 morgan 2 say-timed-by-wait
    text-hold
    95 139 actor 1 say-timed-by-wait
    text-hold
    96 138 actor 3 say-timed-by-wait
    text-hold
    97 matthew 2 say-timed-by-wait
    text-hold
    98 morgan 3 say-timed-by-wait
    text-close
    70 frames wait
    99 138 actor 2 say-timed-by-wait
    text-close
    20 frames wait
    100 139 actor 2 say-timed-by-wait
    text-hold
    101 138 actor 2 say-timed-by-wait
    text-hold
    102 morgan 3 say-timed-by-wait
    text-hold
    103 morgan 1 say-timed-by-wait
    text-hold
    104 139 actor 2 say-timed-by-wait
    text-close
    10 frames wait
    105 matthew 1 say-timed-by-wait
    text-hold
    morgan 14 15 1 anim-then
    106 morgan 1 say-timed-by-wait
    text-hold
    107 rolein 2 say-timed-by-wait
    text-close
    end-script
    ;

: L-80105CD8
    138 actor 329445 0 269064 set-pos
    138 actor 0 3072 0 set-rot
    138 actor 15 1 anim
    -566 -2414 -932 329367 -4080 269303 1201 camera-set
    20 frames wait
    10 ambient-start
    2 screen-effect
    wait-effect
    138 actor 118 0 talk
    morgan 139 0 face
    238 -2090 -2108 329187 -4080 270521 913 camera-set
    10 ambient-stop
    119 139 actor say-by
    139 actor 329005 269784 4538 20 walk-to-ex
    139 actor wait-stop
    139 actor 4520 0 anim
    139 actor wait-anim
    139 actor 329525 269244 5 60 walk-to-ex
    1 frames wait
    text-close
    morgan 328867 269666 5 60 walk-to-ex
    9 screen-effect
    wait-effect
    27 -1 329845 269364 1024 -2146387796 warp
    ;

: L-80105E40
    matthew 114 0 talk
    matthew 4541 anim-once
    138 actor 328985 0 268964 set-pos
    138 actor 0 2132 0 set-rot
    138 actor 4519 1 anim
    139 actor 115 0 talk
    139 actor 4532 4534 0 anim-then
    10 frames wait
    matthew 4542 0 anim
    139 actor wait-anim
    139 actor 4519 1 anim
    139 actor 116 0 talk
    -524 -1874 -782 328819 -4080 269415 4293 camera-set
    138 actor 4521 4522 1 anim-then
    138 actor 117 0 talk
    30 frames wait
    3 screen-effect
    wait-effect
    goto L-80105CD8
    ;

: L-80105F5C
    2 13 prompt
    0 result= if  goto events.main:sub-8009949C  then
    1 result= if  goto L-80105E40  then
    end-script
    ;

: L-80105F84
    matthew 111 0 talk
    rolein 112 0 talk
    morgan 113 0 talk
    goto L-80105F5C
    ;

: L-80105FC0
    2 23 prompt
    0 result= if  goto events.main:sub-8009949C  then
    1 result= if  goto L-80105F84  then
    end-script
    ;

: L-80105FE8
    matthew 108 5 talk
    139 actor 4527 4519 1 anim-then
    139 actor 109 0 talk
    morgan 106 0 talk
    rolein 110 0 talk
    goto L-80105FC0
    ;

\ $80106308
: entry-80106308
    138 actor as-this-one
    morgan 328367 0 271126 set-pos
    morgan 119 1 anim
    morgan 0 3072 0 set-rot
    matthew 328531 0 270801 set-pos
    matthew 0 1 anim
    matthew 1 0 face
    this-one 329165 0 269234 set-pos
    147 actor 329165 0 269234 set-pos
    this-one 0 2482 0 set-rot
    rolein 327834 0 270194 set-pos
    rolein 0 1 anim
    rolein 0 1024 0 set-rot
    139 actor 329385 0 269484 set-pos
    139 actor 4519 1 anim
    139 actor 0 3500 0 set-rot
    -296 -1988 -770 327849 -4164 271005 11315 camera-set
    morgan -30 -40 turn-by
    morgan 4509 4510 1 anim-then
    20 frames wait
    morgan 65 0 talk
    rolein 327924 271054 1 20 walk-to-ex
    matthew -1 6 talk
    rolein wait-stop
    rolein 1 200 face
    rolein wait-turned
    rolein 4515 4516 0 anim-then
    morgan 4511 119 1 anim-then
    morgan 66 0 talk
    rolein 4515 4516 0 anim-then
    rolein 67 0 talk
    rolein 68 0 talk
    rolein 69 0 talk
    morgan 70 9 talk
    rolein 71 7 talk
    rolein 327834 270194 110 face-point
    rolein wait-turned
    rolein 327834 269444 1 20 walk-to-ex
    matthew 17 18 1 anim-then
    morgan 4509 4510 1 anim-then
    20 frames wait
    morgan 72 0 talk
    matthew 73 0 talk
    20 frames wait
    matthew 19 0 anim
    matthew wait-anim
    matthew 74 7 talk
    morgan 75 0 talk
    morgan 4511 119 1 anim-then
    morgan 76 0 talk
    matthew 77 7 talk
    morgan 78 0 talk
    morgan 4509 4510 1 anim-then
    morgan 79 0 talk
    5 frames wait
    morgan 4511 4505 0 anim-then
    morgan wait-anim
    morgan 4509 119 1 anim-then
    5 frames wait
    matthew 14 16 0 anim-then
    15 frames wait
    morgan 2033 -200 turn-to
    matthew 180 200 turn-by
    20 frames wait
    14 music
    5 frames wait
    118 sound
    2 frames wait
    120 sound
    2 frames wait
    117 ambient-start
    2 frames wait
    -476 -1628 -806 328903 -4080 269601 1517 camera-set
    this-one 4518 4519 1 anim-then
    this-one 147 actor ignore-b
    139 actor 4543 4519 1 anim-then
    116 sound
    ['] L-80106070 start-event
    200 20 camera-turn
    rolein 138 0 face
    matthew 0 1 anim
    matthew 138 -180 face
    morgan 0 0 anim
    morgan 328817 270926 walk-to
    morgan 139 180 face
    117 ambient-stop
    10 frames wait
    116 sound
    rolein 4551 4552 1 anim-then
    10 frames wait
    10 music
    this-one 4527 4519 1 anim-then
    90 frames wait
    matthew 17 18 1 anim-then
    139 actor 4524 4525 1 anim-then
    30 frames wait
    this-one 4521 4522 1 anim-then
    matthew 19 90 0 anim-then
    morgan 14 87 0 anim-then
    30 frames wait
    this-one 4523 4519 1 anim-then
    139 actor 4526 4519 1 anim-then
    wait-camera-turn
    55 frames wait
    -890 -1616 -3092 328711 -4080 269829 1464 camera-set
    40 frames wait
    139 actor 329005 269784 4537 30 walk-to-ex
    139 actor wait-stop
    139 actor 4519 1 anim
    139 actor 1 0 face
    this-one 328915 269064 4537 20 walk-to-ex
    this-one wait-stop
    this-one 2482 -200 turn-to
    this-one 4543 4519 1 anim-then
    rolein 327894 0 269264 set-pos
    rolein 0 1024 0 set-rot
    60 frames wait
    this-one 329165 0 269234 set-pos
    this-one 328415 269354 4537 30 walk-to-ex
    -542 -1112 -494 328009 -4008 269187 -3384 camera-set
    this-one wait-stop
    this-one 3072 0 turn-to
    this-one 4528 0 anim
    this-one wait-anim
    this-one 4294967295 actor ignore-b
    147 actor 328095 0 269284 set-pos
    147 actor -4490 set-y
    this-one 4529 0 anim
    this-one wait-anim
    this-one 4519 1 anim
    rolein 4553 0 anim
    rolein wait-anim
    rolein 17 19 0 anim-then
    this-one 4527 4519 1 anim-then
    70 frames wait
    rolein 4548 0 anim
    10 frames wait
    -620 -2738 -428 328999 -4080 271521 4173 camera-set
    morgan 328747 270666 walk-to
    morgan wait-stop
    morgan 88 0 anim
    30 frames wait
    139 actor 328905 270184 4537 30 walk-to-ex
    139 actor wait-stop
    139 actor 4519 1 anim
    morgan 17 19 0 anim-then
    morgan wait-anim
    matthew 88 0 260 anim-then
    morgan 14 15 1 anim-then
    30 frames wait
    morgan 16 0 anim
    50 frames wait
    -170 -2702 -860 327667 -4080 268305 2421 camera-set
    this-one 4527 4519 1 anim-then
    rolein 4549 4550 0 anim-then
    rolein wait-anim
    this-one 4530 0 anim
    this-one wait-anim
    this-one 147 actor ignore-b
    this-one 4531 0 anim
    this-one wait-anim
    this-one 4521 4522 1 anim-then
    139 actor 328995 0 270334 set-pos
    morgan 328867 0 270766 set-pos
    morgan 0 -2081 0 set-rot
    matthew 328611 0 270671 set-pos
    matthew 0 1657 0 set-rot
    40 frames wait
    -512 -1586 -530 329107 -4080 270429 5301 camera-set
    139 actor 4524 4526 0 anim-then
    139 actor wait-anim
    139 actor 4519 1 anim
    20 frames wait
    matthew 17 19 0 anim-then
    matthew wait-anim
    20 frames wait
    139 actor 328755 270434 4537 30 walk-to-ex
    139 actor wait-stop
    139 actor 0 -587 0 set-rot
    139 actor 4519 1 anim
    150 frames wait
    2 23 prompt
    0 result= if  goto events.main:sub-8009949C  then
    1 result= if  goto L-80105FE8  then
    end-script
    ;

\ $80106DF0
: entry-80106DF0
    63 32767 actor say-by-wait
    matthew 81 0 anim
    matthew wait-anim
    morgan 328367 0 271126 set-pos
    morgan 4510 1 anim
    morgan 0 2722 0 set-rot
    -200 -1988 -914 328101 -4116 271473 12131 camera-set
    morgan 64 0 talk
    10 frames wait
    goto entry-80106308
    ;

\ $80106E8C
: entry-80106E8C
    rolein 128 0 talk
    matthew 129 2 talk
    rolein 130 0 talk
    end-script
    ;

\ $80106EBC
: entry-80106EBC
    rolein as-this-one
    this-one 131 0 talk
    matthew 132 7 talk
    this-one 133 0 talk
    matthew 134 6 talk
    this-one 135 0 talk
    matthew 136 4 talk
    this-one 137 0 talk
    this-one 138 0 talk
    2 map-reveal
    7 map-reveal
    end-script
    ;

\ $80106F38
: entry-80106F38
    rolein as-this-one
    this-one 139 0 talk
    matthew 140 7 talk
    this-one 141 0 talk
    matthew 142 4 talk
    this-one 143 0 talk
    matthew 144 5 talk
    this-one 145 0 talk
    end-script
    ;

\ $80106F98
: entry-80106F98
    rolein as-this-one
    this-one 146 0 talk
    matthew 147 7 talk
    this-one 148 0 talk
    matthew 149 6 talk
    this-one 150 0 talk
    end-script
    ;

\ $80106FE0
: entry-80106FE0
    rolein as-this-one
    this-one 151 0 talk
    matthew 152 3 talk
    this-one 153 0 talk
    matthew 154 4 talk
    this-one 155 0 talk
    end-script
    ;

\ $80107028
: entry-80107028
    rolein 156 0 talk
    matthew 157 7 talk
    rolein 158 0 talk
    end-script
    ;

\ $80107058
: entry-80107058
    matthew 159 7 talk
    rolein 160 0 talk
    rolein 161 0 talk
    matthew 162 6 talk
    end-script
    ;

\ $80107094
: entry-80107094
    rolein as-this-one
    this-one 163 0 talk
    matthew 164 4 talk
    this-one 165 0 talk
    matthew 166 7 talk
    this-one 167 0 talk
    matthew 168 5 talk
    end-script
    ;

\ $801070E8
: entry-801070E8
    rolein as-this-one
    this-one 169 0 talk
    matthew 170 7 talk
    this-one 171 0 talk
    matthew 172 6 talk
    matthew 173 5 talk
    this-one 174 0 talk
    end-script
    ;

\ $80107184
: entry-80107184
    rolein as-this-one
    this-one 180 0 talk
    this-one 181 0 talk
    matthew 182 4 talk
    this-one 183 0 talk
    matthew 184 5 talk
    this-one 185 0 talk
    end-script
    ;

\ $801071D8
: entry-801071D8
    rolein as-this-one
    matthew 186 5 talk
    this-one 187 0 talk
    matthew 188 5 talk
    this-one 189 0 talk
    this-one 190 0 talk
    matthew 191 1 talk
    this-one 192 0 talk
    matthew 193 5 talk
    this-one 194 0 talk
    matthew 195 1 talk
    this-one 196 0 talk
    matthew 197 5 talk
    this-one 198 0 talk
    matthew 199 5 talk
    this-one 200 0 talk
    matthew 201 7 talk
    this-one 202 0 talk
    this-one 203 0 talk
    matthew 204 4 talk
    this-one 205 0 talk
    this-one 206 0 talk
    15 music
    207 5 say-mode-wait
    end-script
    ;

\ $801072F4
: entry-801072F4
    matthew 208 7 talk
    rolein 209 0 talk
    matthew 210 5 talk
    rolein 211 0 talk
    matthew 212 0 talk
    rolein 213 0 talk
    matthew 214 7 talk
    rolein 215 0 talk
    matthew 216 6 talk
    rolein 217 0 talk
    matthew 218 4 talk
    rolein 219 0 talk
    220 5 say-mode-wait
    221 5 say-mode-wait
    end-script
    ;

\ $801073B8
: entry-801073B8
    222 32767 actor say-by-wait
    223 32767 actor say-by-wait
    224 32767 actor say-by-wait
    225 32767 actor say-by-wait
    226 32767 actor say-by-wait
    227 32767 actor say-by-wait
    end-script
    ;

\ $80107404
: entry-80107404
    228 32767 actor say-by-wait
    end-script
    ;

\ $80107414
: entry-80107414
    -674 -1442 -2390 329443 -4080 269329 5120 camera-set
    82 door-animate
    events.main:sub-80096CF0
    27 -1 329845 269364 1024 0 warp
    ;

\ $80107464
: entry-80107464
    -386 -2954 -1742 327381 -4080 267486 2722 camera-set
    84 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $801074A0
: entry-801074A0
    -296 -2516 -968 327038 -4200 267833 880 camera-set
    84 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' arrive-80104E0C $80104E0C 21 overlay-word!
