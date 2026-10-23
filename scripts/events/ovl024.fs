\ ovl024.fs - area 024 overlay (CD.BIN 024_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl024
USING: events events.main ;

\ $8010393C
: entry-8010393C
    matthew 24 1 set-floor
    matthew 592303 -25840 130360 set-pos
    1 set-scene
    95 actor as-this-one
    -782 -1658 -1292 592577 -25840 130496 13290 camera-set
    this-one 92 0 through-door
    matthew 95 0 face
    95 actor 0 0 face
    this-one 223 0 talk
    matthew 224 7 talk
    5 frames wait
    3 screen-effect
    25 frames wait
    27 -1 590706 132226 -1034 0 warp
    ;

: L-80103A78
    this-one 173 4 talk
    this-one 174 6 talk
    end-script
    ;

\ $80103A94
: entry-80103A94
    95 actor as-this-one
    -842 -2702 76 591203 -25840 132183 2378 camera-set
    this-one 160 4 talk
    13 8 prompt
    1 result= if  goto L-80103A78  then
    this-one 161 1 talk
    matthew 162 7 talk
    this-one 163 7 talk
    matthew 164 1 talk
    this-one 165 7 talk
    matthew 83 84 anim-at  matthew wait-anim
    166 5 say-mode-wait
    this-one 167 6 talk
    168 5 say-mode-wait
    this-one 169 5 talk
    matthew 85 0 anim-at  matthew wait-anim
    matthew 170 1 talk
    this-one 171 3 talk
    matthew 172 7 talk
    ovl024 $801036D4 native
    end-script
    ;

\ $80103B9C
: entry-80103B9C
    95 actor as-this-one
    -1292 -4070 -2672 594307 -25840 131450 3508 camera-set
    95 actor 591736 132255 walk-to
    this-one wait-stop
    matthew 95 0 face
    95 actor 95 0 face
    this-one 223 0 talk
    matthew 224 7 talk
    5 frames wait
    3 screen-effect
    25 frames wait
    27 -1 590706 132226 -1034 0 warp
    ;

: L-80103C3C
    -470 -1832 -926 595178 -25552 132268 4904 camera-set
    end-script
    ;

\ $80103CCC
: entry-80103CCC
    1 screen-effect
    -1 music
    matthew 0 4096 0 set-rot
    0 set-scene
    morgan 594430 0 132558 set-pos
    morgan -25930 set-y
    morgan 0 -1038 0 set-rot
    morgan 5116 1 anim
    father-barton 0 0 0 set-pos
    samuel 594430 0 132045 set-pos
    samuel -25955 set-y
    samuel 0 -1038 0 set-rot
    samuel 5100 1 anim
    steve 594429 0 132281 set-pos
    steve -25935 set-y
    steve 0 -1038 0 set-rot
    steve 5105 1 anim
    10 actor 594063 0 133563 set-pos
    10 actor 0 6208 0 set-rot
    10 actor 0 1 anim
    nozz 593712 0 132681 set-pos
    nozz 0 1152 0 set-rot
    nozz 72 1 anim
    cohen 593712 0 132917 set-pos
    cohen 0 1216 0 set-rot
    cohen 72 1 anim
    -470 -2330 -1490 594224 -25852 132358 4352 camera-set
    map-sync
    30 frames wait
    116 ambient-start
    30 frames wait
    2 screen-effect
    wait-effect
    morgan 5117 5113 anim-at  morgan wait-anim
    102 0 sequence-play
    20 frames wait
    7 morgan say-by-wait
    10 frames wait
    116 ambient-stop
    10 frames wait
    nozz 8 8 talk
    9 morgan say-by
    wait-text
    L-80103C3C
    samuel 5101 5102 anim-at  samuel wait-anim
    10 frames wait
    0 1 say-mode-wait
    1 1 say-mode-wait
    2 1 say-mode-wait
    samuel 5104 5102 1 anim-then
    10 samuel say-by-wait
    116 ambient-start
    -470 -2330 -1490 594224 -25852 132358 4352 camera-set
    cohen 11 8 talk
    samuel 5104 5102 1 anim-then
    12 samuel say-by-wait
    samuel 5103 5100 anim-at  samuel wait-anim
    nozz 13 8 talk
    116 ambient-stop
    -470 -2330 -1490 594224 -25852 132358 4352 camera-set
    L-80103C3C
    14 samuel say-by-wait
    samuel 5101 5102 anim-at  samuel wait-anim
    samuel 5104 5102 1 anim-then
    15 samuel say-by-wait
    nozz 16 8 talk
    17 samuel say-by-wait
    samuel 5104 5102 1 anim-then
    18 samuel say-by-wait
    116 ambient-start
    samuel 5103 5100 anim-at  samuel wait-anim
    30 frames wait
    116 ambient-stop
    -470 -2330 -1490 594224 -25852 132358 4352 camera-set
    morgan 5120 5113 anim-at  morgan wait-anim
    -584 -938 -716 594271 -25840 132480 592 camera-set
    19 morgan say-by-wait
    20 morgan say-by-wait
    21 morgan say-by-wait
    22 morgan say-by-wait
    morgan 5115 5116 anim-at  morgan wait-anim
    23 morgan say-by
    wait-text
    594235 -25840 132119 50 camera-move
    steve 5106 5107 anim-at  steve wait-anim
    24 steve say-by
    wait-text
    25 steve say-by
    wait-text
    26 morgan say-by
    wait-text
    steve 5108 5109 1 anim-then
    27 steve say-by
    wait-text
    steve 5110 5111 1 anim-then
    28 steve say-by
    wait-text
    morgan 5117 5113 anim-at  morgan wait-anim
    29 morgan say-by-wait
    30 morgan say-by-wait
    31 morgan say-by-wait
    32 morgan say-by-wait
    33 morgan say-by-wait
    34 morgan say-by-wait
    116 ambient-start
    9 screen-effect
    wait-effect
    1 set-scene
    -512 -1154 -680 593701 -26000 130667 3184 camera-set
    map-sync
    10 frames wait
    116 ambient-stop
    2 screen-effect
    wait-effect
    10 frames wait
    35 matthew say-by
    wait-text
    36 matthew say-by
    wait-text
    10 actor place-at-stop
    father-barton place-at-stop
    end-script
    ;

\ $801042FC
: entry-801042FC
    matthew 0 4096 0 set-rot
    0 set-scene
    -470 -2330 -1490 594224 -25852 132358 4352 camera-set
    30 frames wait
    1 set-scene
    -512 -1154 -680 593701 -26000 130667 3184 camera-set
    wait-text
    end-script
    ;

\ $80104370
: entry-80104370
    11 screen-effect
    wait-effect
    matthew 0 4096 0 set-rot
    0 set-scene
    -470 -2330 -1490 594224 -25852 132358 4352 camera-set
    map-sync
    10 screen-effect
    wait-effect
    30 frames wait
    1 set-scene
    -464 -1884 -930 593812 -25840 130690 4096 camera-set
    42 matthew say-by-wait
    end-script
    ;

\ $80104408
: entry-80104408
    -674 -2918 -1184 594318 -25840 131739 -537 camera-set
    morgan camera-at
    37 morgan say-by-wait
    3 screen-effect
    30 frames wait
    matthew 593919 0 130596 set-pos
    matthew 0 3839 0 set-rot
    1 set-scene
    2 screen-effect
    end-script
    ;

\ $80104580
: entry-80104580
    events.main:sub-80096E34
    matthew 62 0 anim
    matthew wait-anim
    4 matthew say-by-wait
    end-script
    ;

: L-801057A4
    father-barton 4294967295 actor ignore-a
    102 actor 15 actor-model
    102 actor as-this-one
    1 set-scene
    matthew 596525 0 130626 set-pos
    matthew 0 2048 0 set-rot
    matthew 0 1 anim
    this-one 595325 0 130206 set-pos
    this-one 24 1 set-floor
    -926 -830 -2858 596525 -25840 130626 2072 camera-set
    this-one 140 0 talk
    2384 36 camera-angle
    this-one 596225 130206 5 60 walk-to-ex
    matthew 2538 200 turn-to
    this-one wait-stop
    this-one 0 -200 face
    this-one wait-turned
    20 frames wait
    matthew 141 0 talk
    this-one 142 1 talk
    matthew 14 15 anim-at  matthew wait-anim
    20 frames wait
    matthew 143 0 talk
    20 frames wait
    9 screen-effect
    wait-effect
    27 -1 -1 -1 -1 -2146386980 warp
    ;

\ $801045A8
: entry-801045A8
    95 door-animate
    events.main:sub-80096CF0
    1 set-scene
    ovl024 $80103708 native
    goto L-801057A4
    ;

\ $801045D0
: entry-801045D0
    ovl024 $80103740 native
    matthew 5154 0 anim
    30 frames wait
    11 screen-effect
    wait-effect
    6 set-scene
    matthew 596762 0 132674 set-pos
    matthew 24 1 set-floor
    -506 -1874 -1832 596834 -25840 132674 -1500 camera-set
    10 screen-effect
    wait-effect
    matthew 49 0 anim
    matthew wait-anim
    end-script
    ;

\ $8010465C
: entry-8010465C
    father-barton as-this-one
    this-one 593629 0 132765 set-pos
    this-one 0 1024 0 set-rot
    this-one 5135 1 anim
    matthew 591124 0 132270 set-pos
    matthew 0 1024 0 set-rot
    matthew 0 1 anim
    -1518 -1988 -326 595378 -25828 132276 1032 camera-set
    20 frames wait
    32767 actor 43 0 talk
    32767 actor 44 0 talk
    -264 -4478 -1226 592204 -25828 132858 -816 camera-set
    matthew 592984 132270 walk-to
    this-one 45 0 talk
    matthew wait-stop
    matthew 14 0 face
    this-one 5136 73 0 anim-then
    this-one wait-anim
    this-one 74 1 anim
    matthew 46 0 talk
    this-one 47 0 talk
    matthew 48 3 talk
    this-one 75 71 0 anim-then
    this-one wait-anim
    this-one 49 0 talk
    this-one 50 0 talk
    this-one 51 0 talk
    matthew 52 7 talk
    this-one 53 0 talk
    this-one 54 0 talk
    this-one 55 0 talk
    this-one 56 0 talk
    matthew 57 3 talk
    end-script
    ;

\ $80104878
: entry-80104878
    father-barton as-this-one
    matthew 591132 0 132242 set-pos
    matthew 0 1024 0 set-rot
    matthew 0 1 anim
    136 actor 593252 0 132362 set-pos
    136 actor 0 -700 0 set-rot
    136 actor 0 1 anim
    137 actor 592802 0 132712 set-pos
    137 actor 0 1024 0 set-rot
    137 actor 72 1 anim
    -1310 -1694 -1520 592086 -25036 132128 -824 camera-set
    58 5 say-mode-wait
    616 36 camera-angle
    wait-camera-turn
    59 5 say-mode-wait
    this-one 591612 0 131262 set-pos
    this-one 0 0 0 set-rot
    this-one 0 1 anim
    -1358 -2492 -1556 593028 -24940 132368 -920 camera-set
    this-one 60 0 talk
    matthew 14 15 anim-at  matthew wait-anim
    this-one 591642 131822 walk-to
    20 frames wait
    matthew 61 0 talk
    this-one 14 15 anim-at  this-one wait-anim
    5 frames wait
    this-one 0 1 anim
    this-one 136 200 face
    this-one 62 0 talk
    matthew 16 0 anim-at  matthew wait-anim
    matthew 63 3 talk
    137 actor 71 0 anim-at  137 actor wait-anim
    137 actor 592682 132242 walk-to
    137 actor wait-stop
    matthew 591462 0 132602 set-pos
    matthew 0 1444 0 set-rot
    this-one 0 43 0 set-rot
    -2798 -1016 -3848 592776 -24916 132188 -920 camera-set
    10 frames wait
    this-one 64 4 talk
    137 actor 591672 132242 walk-to
    136 actor 591992 132422 walk-to
    137 actor wait-stop
    matthew 65 5 talk
    137 actor 14 16 anim-at  137 actor wait-anim
    this-one 66 0 talk
    137 actor 67 7 talk
    136 actor 68 6 talk
    136 actor 69 3 talk
    matthew 70 1 talk
    this-one 71 4 talk
    this-one 72 0 talk
    matthew 73 0 talk
    137 actor 17 18 anim-at  137 actor wait-anim
    136 actor 74 7 talk
    137 actor 75 0 talk
    137 actor 19 0 anim-at  137 actor wait-anim
    137 actor 591115 132170 walk-to
    137 actor wait-stop
    137 actor 90 0 through-door
    -506 -914 -410 591636 -25912 131708 2056 camera-set
    136 actor 0 0 0 set-pos
    137 actor 0 0 0 set-pos
    this-one 17 18 anim-at  this-one wait-anim
    matthew 76 0 talk
    this-one 19 0 anim-at  this-one wait-anim
    end-script
    ;

\ $80104C6C
: entry-80104C6C
    matthew 591143 0 132212 set-pos
    matthew 0 1024 0 set-rot
    matthew 0 1 anim
    -512 -5696 -2228 591215 -25840 132314 2980 camera-set
    10 frames wait
    matthew 592123 132212 walk-to
    matthew wait-stop
    matthew 77 1 talk
    matthew 81 0 anim
    matthew wait-anim
    78 5 say-mode-wait
    matthew 81 0 anim
    matthew wait-anim
    end-script
    ;

: L-80104D2C
    92 32767 actor 2 say-timed-by-wait
    text-hold
    93 32767 actor 3 say-timed-by-wait
    text-close
    matthew 141 0 anim
    matthew wait-anim
    matthew 14 0 anim
    matthew wait-anim
    matthew -1716 200 turn-to
    matthew 15 1 anim
    end-script
    ;

\ $80104DA0
: entry-80104DA0
    father-barton as-this-one
    3 set-scene
    this-one 593573 0 127988 set-pos
    this-one 24 1 set-floor
    matthew 595543 0 128338 set-pos
    matthew 0 -3336 0 set-rot
    matthew 0 1 anim
    -392 -596 -1520 595647 -25636 128234 -2262 camera-set
    matthew 40 0 anim
    matthew wait-anim
    matthew 42 51 anim-at  matthew wait-anim
    195 196 cd-pictures
    79 32767 actor say-by-wait
    80 32767 actor say-by-wait
    18 music
    81 32767 actor say-by-wait
    82 32767 actor say-by-wait
    matthew 56 57 anim-at  matthew wait-anim
    121 sound
    30 frames wait
    83 2 say-mode-wait
    84 2 say-mode-wait
    85 2 say-mode-wait
    86 2 say-mode-wait
    87 2 say-mode-wait
    13 music
    88 2 say-mode-wait
    89 2 say-mode-wait
    90 2 say-mode-wait
    91 2 say-mode-wait
    matthew 58 51 1 anim-then
    ' L-80104D2C start-event
    0 cd-picture
    3 frames wait
    -626 -1790 -644 595185 -25768 128210 -3366 camera-set
    596409 -25768 128786 14 camera-move
    wait-camera-move
    5 frames wait
    this-one 595073 0 128228 set-pos
    this-one 0 -3336 0 set-rot
    this-one 24 1 anim
    -1124 -1292 -968 594568 -25336 128043 -1238 camera-set
    14 music
    this-one 25 0 anim
    5 frames wait
    594388 -25408 128001 15 camera-move
    256 10 rumble
    255 10 rumble
    this-one wait-anim
    5 screen-effect
    wait-effect
    wait-camera-move
    5 set-scene
    148 actor 593918 -24640 131561 set-pos
    148 actor 0 0 0 set-rot
    matthew 594088 0 131571 set-pos
    matthew 0 1024 0 set-rot
    matthew 5121 1 anim
    this-one 594888 0 131591 set-pos
    170 actor 594888 0 131591 set-pos
    this-one 0 3072 0 set-rot
    this-one 68 1 anim
    matthew 24 0 set-floor
    this-one 24 0 set-floor
    148 actor 24 0 set-floor
    170 actor 24 0 set-floor
    -1214 262 -1460 593880 -23932 131575 3088 camera-set
    30 frames wait
    4 screen-effect
    5 frames wait
    5 screen-effect
    5 frames wait
    4 screen-effect
    5 frames wait
    5 screen-effect
    5 frames wait
    4 screen-effect
    wait-effect
    94 32767 actor say-by-wait
    5 screen-effect
    5 frames wait
    4 screen-effect
    5 frames wait
    95 32767 actor say-by-wait
    20 frames wait
    96 32767 actor say-by-wait
    father-barton 170 actor ignore-a
    -1268 5 camera-height-to
    wait-camera-height
    10 frames wait
    matthew 97 0 talk
    matthew 594078 0 131571 set-pos
    matthew 0 1034 0 set-rot
    -122 -650 -1112 594012 -24628 131643 712 camera-set
    matthew 5126 5121 1 anim-then
    98 5 say-mode-wait
    matthew 5126 5121 1 anim-then
    -422 -1400 -254 593976 -24484 131505 832 camera-set
    matthew 99 0 talk
    100 32767 actor say-by-wait
    matthew 101 0 talk
    this-one 102 0 talk
    matthew 103 0 talk
    this-one 104 0 talk
    this-one 105 0 talk
    matthew 5122 5121 1 anim-then
    matthew 106 0 talk
    this-one 107 0 talk
    this-one 108 0 talk
    4 music
    this-one 69 72 1 anim-then
    this-one 109 0 talk
    matthew 5122 5121 1 anim-then
    matthew 110 0 talk
    this-one 73 74 1 anim-then
    this-one 111 0 talk
    matthew 112 0 talk
    this-one 113 0 talk
    this-one 114 0 talk
    this-one 75 72 1 anim-then
    this-one 115 0 talk
    matthew 5126 5121 1 anim-then
    matthew 116 0 talk
    matthew 5122 5121 1 anim-then
    matthew 117 0 talk
    this-one 118 0 talk
    -476 -170 -404 594426 -24328 131205 -632 camera-set
    matthew 5122 5121 1 anim-then
    matthew 119 0 talk
    matthew 5126 5121 1 anim-then
    matthew 120 0 talk
    -956 -2882 -2042 593838 -23680 131931 -742 camera-set
    this-one 71 0 anim
    this-one 121 0 talk
    matthew 5122 5121 1 anim-then
    this-one wait-anim
    this-one 594538 131571 walk-to
    matthew 122 0 talk
    this-one wait-stop
    -1 music
    this-one 5140 5141 anim-at  this-one wait-anim
    10 frames wait
    matthew 123 0 talk
    -1448 -2522 -1196 594978 -23716 131217 1034 camera-set
    594978 -23716 131883 6 camera-move
    this-one 124 0 talk
    this-one 125 0 talk
    this-one 126 0 talk
    wait-camera-move
    -404 -1406 22 594150 -24604 131601 1594 camera-set
    matthew 127 0 talk
    this-one 128 0 talk
    this-one 129 0 talk
    this-one 130 0 talk
    this-one 131 0 talk
    this-one 132 0 talk
    -476 -860 -518 593982 -24640 131457 850 camera-set
    17 music
    this-one 5131 anim-once
    this-one anim-hold
    15 frames wait
    133 matthew 1 say-timed-by-wait
    text-close
    111 sound
    14 screen-effect
    30 frames wait
    134 5 say-mode-wait
    -200 -1106 -1394 594432 -24352 131535 2914 camera-set
    6 music
    38 minigame
    80 frames wait
    9 screen-effect
    wait-effect
    -1994 -2092 -2000 594144 -22972 131589 3058 camera-set
    30 frames wait
    2 screen-effect
    wait-effect
    135 5 say-mode-wait
    136 5 say-mode-wait
    137 5 say-mode-wait
    17 minigame
    wait-minigame
    3 screen-effect
    wait-effect
    -560 -1982 -20 594148 -24640 131432 -8918 camera-set
    2 screen-effect
    wait-effect
    10 frames wait
    matthew 81 0 anim
    139 5 say-mode-wait
    matthew wait-anim
    10 frames wait
    end-script
    ;

\ $80105914
: entry-80105914
    father-barton as-this-one
    matthew 144 5 talk
    this-one 145 0 talk
    this-one 146 0 talk
    matthew 147 7 talk
    matthew 148 7 talk
    this-one 149 0 talk
    this-one 150 4 talk
    3 screen-effect
    wait-effect
    101 0 sequence-play
    matthew 593820 -25840 132344 set-pos
    matthew 0 3072 0 set-rot
    matthew 81 0 anim
    -716 -5276 -2054 593100 -25528 131912 6884 camera-set
    40 frames wait
    100 0 sequence-play
    this-one 593850 -25840 132044 set-pos
    this-one 0 0 face
    30 frames wait
    2 screen-effect
    wait-effect
    matthew 17 0 anim
    matthew wait-anim
    matthew 14 -200 face
    this-one 59 60 1 anim-then
    this-one 151 0 talk
    this-one 152 0 talk
    matthew 59 61 0 anim-then
    20 frames wait
    this-one 61 0 anim
    this-one wait-anim
    matthew 133 51 anim-at  matthew wait-anim
    10 frames wait
    matthew 153 0 talk
    matthew 55 137 0 anim-then
    matthew wait-anim
    6 picture-show
    159 3 say-mode-wait
    picture-hide
    this-one 154 7 talk
    this-one 135 60 1 anim-then
    15 frames wait
    this-one 155 0 talk
    matthew 156 6 talk
    this-one 157 0 talk
    matthew 158 0 talk
    matthew 59 61 0 anim-then
    20 frames wait
    this-one 61 0 anim
    this-one wait-anim
    matthew 87 0 anim
    matthew wait-anim
    10 frames wait
    end-script
    ;

\ $80105BA0
: entry-80105BA0
    -1 music
    james 187 actor-model
    153 actor 187 actor-model
    153 actor as-this-one
    5 set-scene
    matthew 594031 0 130985 set-pos
    matthew 0 0 0 set-rot
    matthew 0 1 anim
    matthew 24 0 set-floor
    this-one 593991 0 131595 set-pos
    this-one 0 3072 0 set-rot
    this-one 0 1 anim
    this-one 24 0 set-floor
    -494 -2792 -1562 593671 -24640 131489 -1476 camera-set
    matthew 175 0 talk
    this-one 17 18 anim-at  this-one wait-anim
    this-one 176 0 6 talk-to
    matthew 594021 131245 walk-to
    matthew 177 0 talk
    this-one 178 0 6 talk-to
    this-one 19 0 anim-at  this-one wait-anim
    this-one 179 3 6 talk-to
    matthew 17 18 anim-at  matthew wait-anim
    matthew 180 0 talk
    10 frames wait
    191 192 cd-pictures
    20 frames wait
    this-one 181 0 6 talk-to
    matthew 182 0 talk
    this-one 183 0 6 talk-to
    this-one 184 0 6 talk-to
    matthew 19 0 anim-at  matthew wait-anim
    matthew 185 0 talk
    this-one 0 -200 face
    15 music
    this-one 186 0 6 talk-to
    matthew 187 2 talk
    0 cd-picture
    20 frames wait
    matthew 85 0 anim-at  matthew wait-anim
    matthew 188 5 talk
    this-one 189 3 6 talk-to
    matthew 17 18 anim-at  matthew wait-anim
    matthew 190 0 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 191 0 6 talk-to
    this-one 16 0 anim-at  this-one wait-anim
    this-one 192 4 6 talk-to
    this-one 594172 131065 walk-to
    this-one wait-stop
    this-one 97 5 through-door
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80105E8C
: entry-80105E8C
    -1118 -4820 -1964 591804 -25840 132316 -789 camera-set
    90 door-animate
    events.main:sub-80096CF0
    27 -1 -1 -1 -1 0 warp
    ;

\ $80105EDC
: entry-80105EDC
    -614 -3236 -1394 593953 -25840 131008 1481 camera-set
    92 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80105F18
: entry-80105F18
    -350 -1508 -128 596870 -25780 130458 7164 camera-set
    92 door-animate
    events.main:sub-80096CF0
    0 set-scene
    ovl024 $801033DC native
    end-script
    ;

\ $80105F5C
: entry-80105F5C
    -512 -2738 -182 593836 -25852 130703 -763 camera-set
    matthew 593663 0 130664 set-pos
    matthew 0 4047 0 set-rot
    matthew 62 0 anim
    matthew wait-anim
    39 matthew say-by-wait
    40 matthew say-by-wait
    end-script
    ;

\ $80105FD4
: entry-80105FD4
    -464 -2090 -1580 592175 -25840 130481 3078 camera-set
    93 door-animate
    events.main:sub-80096CF0
    27 -1 -1 -1 -1 0 warp
    ;

\ $80106024
: entry-80106024
    -350 -1508 -128 596870 -25780 130458 7164 camera-set
    94 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80106060
: entry-80106060
    -1370 -1388 -1532 593403 -24976 127464 0 camera-set
    94 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $8010609C
: entry-8010609C
    -350 -1508 -128 596870 -25780 130458 7164 camera-set
    95 door-animate
    events.main:sub-80096CF0
    6 set-scene
    end-script
    ;

\ $801060D8
: entry-801060D8
    -596 -3674 -176 596240 -25840 131347 6238 camera-set
    95 door-animate
    events.main:sub-80096CF0
    1 set-scene
    end-script
    ;

\ $80106114
: entry-80106114
    -1370 -1388 -1532 593403 -24976 127464 0 camera-set
    96 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $80106150
: entry-80106150
    -260 -3674 -740 593995 -25840 128681 7534 camera-set
    96 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $8010618C
: entry-8010618C
    -194 -2900 -1724 594013 -24700 130618 3056 camera-set
    97 door-animate
    events.main:sub-80096CF0
    5 set-scene
    end-script
    ;

\ $801061C8
: entry-801061C8
    -314 -3206 -824 594067 -24640 131362 2466 camera-set
    97 door-animate
    events.main:sub-80096CF0
    4 set-scene
    end-script
    ;

\ $80106204
: entry-80106204
    matthew 596339 0 130658 set-pos
    matthew 0 3072 0 set-rot
    4 set-scene
    -836 -2198 -1904 596035 -25312 130736 4128 camera-set
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
    -320 -2198 58 594387 -24724 130652 -3111 camera-set
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
    matthew 24 0 set-floor
    end-script
    ;

\ $80106384
: entry-80106384
    4 set-scene
    matthew 594230 0 130678 set-pos
    matthew 0 1024 0 set-rot
    -320 -2198 58 594387 -24724 130652 -3111 camera-set
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
    -836 -2198 -1904 596035 -25312 130736 4128 camera-set
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 13 0 anim
    matthew anim-hold
    matthew wait-anim
    1 set-scene
    matthew 24 1 set-floor
    end-script
    ;

\ $8010650C
: entry-8010650C
    191 192 cd-pictures
    50 frames wait
    0 cd-picture
    end-script
    ;

\ $8010652C
: entry-8010652C
    3 5 say-mode-wait
    end-script
    ;
