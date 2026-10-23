\ ovl005.fs - area 005 overlay (CD.BIN 005_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl005
USING: events ;

\ (looped back to: declared first)
defer L-80104FB0

:noname
    20 frames wait
    ovl005 $8010224C native-result
    1 result= if  goto L-80104FB0  then
    end-script
    ; is L-80104FB0

: L-80104FD0
    wolf as-this-one
    1 set-scene
    matthew 115674 0 54597 set-pos
    matthew 0 -4676 0 set-rot
    matthew 0 1 anim
    91 actor 114319 0 60468 set-pos
    91 actor 0 2950 0 set-rot
    91 actor 0 1 anim
    this-one 114519 0 60428 set-pos
    this-one 0 -1200 0 set-rot
    this-one 0 1 anim
    -614 -608 -602 115032 12 54993 -2732 camera-set
    2 screen-effect
    wait-effect
    10 frames wait
    matthew 1300 1301 1 anim-then
    30 frames wait
    -1232 -1604 -1160 110970 516 52377 172 camera-set
    114864 516 52377 30 camera-move
    wait-camera-move
    10 frames wait
    -704 -650 -800 114433 0 58026 -4036 camera-set
    this-one 1351 0 anim
    this-one anim-hold
    91 actor 20 walk-forward
    109 32767 actor 2 say-timed-by-wait
    91 actor idle
    this-one 17 0 anim
    this-one wait-anim
    this-one 2048 -200 turn-to
    this-one wait-turned
    matthew 114954 0 53947 set-pos
    matthew 114774 55927 5 60 walk-to-ex
    -686 -470 -674 114913 168 52116 -4132 camera-set
    114709 168 56064 150 camera-move
    15 frames wait
    this-one 1353 1354 1 anim-then
    wait-camera-move
    this-one 1908 -200 turn-to
    115753 168 56496 120 camera-move
    -4252 -15 camera-angle
    wait-camera-move
    wait-camera-turn
    ovl005 $8010222C native
    goto L-80104FB0
    ;

: L-801030D0
    matthew 4294967295 actor ignore-a
    wolf 189 actor-model
    wolf as-this-one
    2 set-scene
    matthew 112834 0 60429 set-pos
    matthew -4000 set-y
    matthew 0 1 anim
    matthew 0 2048 0 set-rot
    this-one 113200 0 56218 set-pos
    this-one -4000 set-y
    this-one 0 770 0 set-rot
    this-one 24 1 anim
    91 actor 113650 0 55958 set-pos
    91 actor 0 -1540 0 set-rot
    91 actor -4000 set-y
    91 actor 1309 1 anim
    -716 -5240 -392 111952 -3820 60972 -470 camera-set
    matthew 81 0 anim-at  matthew wait-anim
    -1892 -3644 -1616 113404 -2512 59652 -1352 camera-set
    113404 -2512 56484 36 camera-move
    8 frames wait
    matthew 113254 57890 5 60 walk-to-ex
    matthew wait-stop
    matthew 113254 57130 walk-to
    16 sound
    15 frames wait
    16 sound
    15 frames wait
    16 sound
    15 frames wait
    16 sound
    15 frames wait
    16 sound
    matthew 66 0 talk
    this-one 67 0 talk
    this-one 0 -200 face
    this-one 25 0 anim-at  this-one wait-anim
    this-one 1353 1354 1 anim-then
    matthew 1300 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 1301 1 anim
    115 sound
    10 frames wait
    115 sound
    10 frames wait
    115 sound
    10 frames wait
    115 sound
    20 frames wait
    this-one 68 0 talk
    matthew 69 0 talk
    this-one 1355 0 anim-at  this-one wait-anim
    this-one 29 30 anim-at  this-one wait-anim
    matthew 1302 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 0 1 anim
    morgan 0 310 act-command
    this-one 71 0 talk
    matthew 72 7 talk
    this-one 73 0 talk
    this-one 31 0 anim-at  this-one wait-anim
    this-one 134 137 0 anim-then
    this-one 74 0 talk
    this-one wait-anim
    this-one 1318 0 anim
    wolf 4294967295 actor ignore-a
    170 actor 0 0 0 set-pos
    -1946 -980 -1850 113194 -2608 56298 -1976 camera-set
    10 frames wait
    14 music
    75 32767 actor 1 say-timed-by-wait
    text-hold
    text-close
    20 frames wait
    this-one wait-anim
    this-one 29 30 anim-at  this-one wait-anim
    16 sound
    15 frames wait
    16 sound
    15 frames wait
    16 sound
    15 frames wait
    16 sound
    30 frames wait
    this-one 31 0 anim-at  this-one wait-anim
    matthew 76 0 talk
    13 music
    this-one 77 3 talk
    -1856 -3140 -1550 113194 -2608 56694 -368 camera-set
    179 actor 113200 0 56218 set-pos
    179 actor -4000 set-y
    matthew 78 5 talk
    this-one 79 3 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 80 0 talk
    matthew 81 1 talk
    this-one 16 0 anim-at  this-one wait-anim
    this-one 82 0 talk
    matthew 83 5 talk
    -2210 -1250 -2126 113224 -2344 56718 -1736 camera-set
    this-one 84 5 talk
    matthew 85 1 talk
    this-one 86 3 talk
    this-one 87 7 talk
    matthew 88 5 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 89 0 talk
    this-one 90 0 talk
    -2270 -1034 -2558 113386 -2272 56502 -2024 camera-set
    this-one 91 0 talk
    this-one 16 0 anim-at  this-one wait-anim
    this-one 92 6 talk
    this-one 1331 0 anim
    this-one wait-anim
    wolf 179 actor ignore-a
    this-one 1332 1334 anim-at  this-one wait-anim
    10 frames wait
    3 screen-effect
    wait-effect
    matthew 113060 0 57160 set-pos
    this-one 113060 0 56178 set-pos
    wolf 7 matthew 0 fight
    this-one 113300 0 54888 set-pos
    this-one 0 -630 0 set-rot
    this-one 109 1 anim
    this-one -4000 set-y
    91 actor 113650 0 55958 set-pos
    91 actor 0 -1540 0 set-rot
    91 actor -4000 set-y
    91 actor 1309 1 anim
    matthew 113180 0 55618 set-pos
    matthew -4000 set-y
    matthew 0 1890 0 set-rot
    matthew 30 1 anim
    -728 -1928 -3722 113362 -3088 55368 -2168 camera-set
    2 screen-effect
    wait-effect
    10 frames wait
    this-one 93 0 talk
    matthew 31 0 anim-at  matthew wait-anim
    matthew 17 18 anim-at  matthew wait-anim
    matthew 0 1 anim
    matthew 113420 55648 walk-to
    matthew wait-stop
    matthew 113480 0 55688 set-pos
    matthew 0 560 0 set-rot
    -1250 -1640 -2030 113434 -3112 55800 -3176 camera-set
    matthew 150 151 anim-at  matthew wait-anim
    matthew 94 0 talk
    this-one 110 0 1 anim-then
    91 actor 1310 1311 anim-at  91 actor wait-anim
    10 frames wait
    95 91 actor 1 say-timed-by-wait
    text-hold
    97 32767 actor 1 say-timed-by-wait
    text-hold
    this-one 113300 0 54938 set-pos
    this-one 0 0 0 set-rot
    96 matthew 1 say-timed-by
    matthew 113530 0 55488 set-pos
    matthew 0 560 0 set-rot
    matthew 151 1 anim
    -1160 -1220 -938 113500 -3064 56124 -2072 camera-set
    17 music
    matthew 153 0 1 anim-then
    this-one 1331 1332 0 anim-then
    matthew 21 200 face
    this-one wait-anim
    this-one 1331 3 anim
    this-one 1340 0 anim
    this-one anim-hold
    10 frames wait
    114 sound
    this-one wait-anim
    this-one 1341 1 anim
    this-one 98 0 talk
    this-one 99 0 talk
    matthew 113190 0 55858 set-pos
    matthew 0 2100 0 set-rot
    matthew 0 1 anim
    -1520 -1298 -1970 113170 -3004 55290 -3872 camera-set
    matthew 113190 55758 walk-to
    91 actor 1312 1309 anim-at  91 actor wait-anim
    91 actor 101 0 talk
    matthew 17 18 anim-at  matthew wait-anim
    matthew 102 0 talk
    this-one 103 0 talk
    matthew 19 0 anim-at  matthew wait-anim
    matthew 104 5 talk
    91 actor 105 0 talk
    114 sound
    107 wolf 1 say-timed-by
    -1148 -3278 -1340 113170 -3004 55290 -4280 camera-set
    10 frames wait
    this-one 1342 0 anim
    this-one anim-hold
    10 frames wait
    15 sound
    130 sound
    14 sound
    matthew 113190 55158 5 60 walk-to-ex
    this-one wait-anim
    matthew wait-stop
    30 frames wait
    matthew 17 18 anim-at  matthew wait-anim
    matthew 0 1 anim
    matthew 91 -200 face
    matthew wait-turned
    matthew 113490 55698 walk-to
    15 frames wait
    9 screen-effect
    wait-effect
    matthew wait-stop
    this-one 0 0 0 set-pos
    27 -1 -1 -1 -1 -2146386880 warp
    ;

: L-80103C60
    wolf as-this-one
    matthew 114709 0 52048 set-pos
    matthew 0 680 0 set-rot
    matthew 30 1 anim
    this-one 115629 0 53598 set-pos
    this-one 5 1 set-floor
    this-one 0 -2048 0 set-rot
    this-one 0 1 anim
    91 actor 115409 0 53728 set-pos
    91 actor 0 5880 0 set-rot
    91 actor 1309 1 anim
    101 actor 115429 0 52479 set-pos
    101 actor 5 1 set-floor
    101 actor 0 0 0 set-rot
    101 actor 109 1 anim
    -2588 -1976 -2990 114581 2220 52548 -3296 camera-set
    2 screen-effect
    wait-effect
    10 frames wait
    101 actor 36 0 talk
    matthew 31 0 1 anim-then
    this-one 37 6 talk
    101 actor 115429 0 52268 set-pos
    101 actor 1319 1320 anim-at  101 actor wait-anim
    101 actor 38 0 talk
    this-one 39 3 talk
    15 frames wait
    101 actor 40 0 talk
    this-one 41 0 talk
    -2522 -1340 -2432 115391 2256 52254 -3800 camera-set
    10 frames wait
    101 actor 42 0 talk
    this-one 43 0 talk
    101 actor 44 0 talk
    101 actor 45 0 talk
    this-one 46 0 talk
    matthew 114329 0 52288 set-pos
    matthew 0 540 0 set-rot
    this-one 115509 52978 walk-to
    101 actor 47 0 talk
    this-one wait-stop
    101 actor 48 0 talk
    this-one 1306 0 anim
    49 101 actor 1 say-timed-by
    this-one wait-anim
    this-one 1307 1308 0 anim-then
    3 frames wait
    113 sound
    14 music
    101 actor 1321 1322 1 anim-then
    this-one wait-anim
    matthew 50 0 talk
    this-one 51 0 talk
    10 frames wait
    -2450 -2294 -2324 115739 2160 53262 -1448 camera-set
    this-one 115509 53478 walk-to
    matthew 52 0 talk
    this-one 53 0 talk
    this-one wait-stop
    169 actor 115429 0 52479 set-pos
    this-one 0 -200 face
    this-one wait-turned
    54 wolf 2 say-timed-by
    this-one 1353 1354 1 anim-then
    13 frames wait
    matthew 1300 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 1301 1 anim
    -2450 -2186 -2972 115451 2160 53262 -2600 camera-set
    101 actor 1323 0 anim
    101 actor 169 actor ignore-a
    22 frames wait
    114 sound
    this-one 1381 1382 1 anim-then
    101 actor wait-anim
    101 actor 1324 1 anim
    15 frames wait
    this-one 55 0 talk
    101 actor 56 0 talk
    this-one 1383 0 1 anim-then
    15 frames wait
    this-one 2067 -200 turn-to
    this-one wait-turned
    101 actor 4294967295 actor ignore-a
    169 actor 0 0 0 set-pos
    57 wolf 2 say-timed-by
    this-one 115509 53158 walk-to
    this-one wait-stop
    this-one 1306 1307 anim-at  this-one wait-anim
    7 frames wait
    -2822 -908 -2414 115751 2184 53076 -3296 camera-set
    113 sound
    10 frames wait
    40 minigame
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    58 wolf 2 say-timed-by
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    1 set-357c
    102 0 sequence-play
    5 frames wait
    10 music
    118 ambient-start
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    -2432 -1328 -2156 114653 2184 52692 -1448 camera-set
    113 sound
    59 matthew 2 say-timed-by-wait
    text-hold
    text-close
    10 ambient-start
    -2414 -932 -2318 114869 2196 52512 544 camera-set
    matthew 18 1 anim
    60 wolf 2 say-timed-by
    this-one 1308 0 anim-at  this-one wait-anim
    this-one 14 15 anim-at  this-one wait-anim
    15 frames wait
    -2594 -2840 -3290 114853 2016 52116 -1208 camera-set
    61 matthew 2 say-timed-by-wait
    text-hold
    text-close
    -2702 -638 -2270 115487 2160 53118 -3872 camera-set
    this-one 1306 1307 anim-at  this-one wait-anim
    62 wolf 2 say-timed-by
    7 frames wait
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    113 sound
    10 frames wait
    63 matthew 1 say-timed-by-wait
    text-hold
    text-close
    this-one 115599 0 53688 set-pos
    this-one 0 -1300 0 set-rot
    this-one 0 1 anim
    154 actor 114689 0 51945 set-pos
    -2414 -896 -2372 114581 2196 52620 -3608 camera-set
    64 wolf 2 say-timed-by
    matthew 0 1 anim
    10 ambient-stop
    this-one 1313 1315 0 anim-then
    91 actor 1314 1316 0 anim-then
    30 frames wait
    1 object-anim
    30 frames wait
    117 sound
    this-one wait-anim
    91 actor 0 0 0 set-pos
    this-one 0 0 0 set-pos
    -2924 -1064 -3686 114077 2196 52620 -2408 camera-set
    matthew 114529 51995 walk-to
    matthew wait-stop
    matthew 48 50 anim-at  matthew wait-anim
    matthew 154 actor ignore-a
    matthew 49 51 anim-at  matthew wait-anim
    15 frames wait
    matthew 141 0 anim-at  matthew wait-anim
    -2618 -2792 -2354 114545 2196 52620 -3728 camera-set
    matthew 115729 54095 5 60 walk-to-ex
    10 frames wait
    9 screen-effect
    wait-effect
    matthew wait-stop
    L-80104FD0
    101 actor 0 0 0 set-pos
    118 ambient-stop
    goto L-801030D0
    ;

\ $80104614
: arrive-80104614
    wolf 189 actor-model
    wolf as-this-one
    91 actor 114919 0 53638 set-pos
    91 actor 0 2590 0 set-rot
    91 actor 1309 1 anim
    this-one 115709 0 54018 set-pos
    this-one 0 4200 0 set-rot
    this-one 5 1 set-floor
    this-one 0 1 anim
    150 actor 116059 0 53688 set-pos
    101 actor 116059 0 53688 set-pos
    101 actor 5 1 set-floor
    101 actor 0 630 0 set-rot
    101 actor 0 1 anim
    -1112 -1496 -1832 113403 -792 53496 390 camera-set
    113403 -792 49950 12 camera-move
    8 screen-effect
    matthew 112899 0 51318 set-pos
    matthew 0 5142 0 set-rot
    matthew 0 1 anim
    65 frames wait
    this-one 14 15 anim-at  this-one wait-anim
    101 actor 150 actor ignore-a
    101 actor 1325 1325 0 anim-then
    101 actor wait-anim
    this-one 16 0 anim-at  this-one wait-anim
    101 actor 115879 53628 walk-to
    101 actor wait-stop
    101 actor 115879 53078 walk-to
    101 actor wait-stop
    101 actor 1024 0 turn-to
    101 actor 1325 1325 0 anim-then
    101 actor wait-anim
    101 actor 115579 53078 walk-to
    101 actor wait-stop
    101 actor 115579 52298 walk-to
    101 actor wait-stop
    101 actor 1024 0 turn-to
    101 actor 1325 1325 0 anim-then
    101 actor wait-anim
    101 actor 2048 0 turn-to
    101 actor 1325 1325 0 anim-then
    101 actor wait-anim
    117 sound
    101 actor 14 15 anim-at  101 actor wait-anim
    wait-camera-move
    388 -794 406 114597 -792 51498 -1122 camera-set
    10 frames wait
    matthew 0 1 talk
    10 frames wait
    -458 -704 -446 112335 96 51966 5142 camera-set
    matthew 113579 51798 5 60 walk-to-ex
    101 actor 1 0 talk
    matthew 17 18 1 anim-then
    10 frames wait
    4950 -24 camera-angle
    wait-camera-turn
    10 frames wait
    -2216 -512 -2786 114647 1764 53376 544 camera-set
    18 music
    matthew 2 0 talk
    91 actor 1310 1311 anim-at  91 actor wait-anim
    10 frames wait
    91 actor 3 0 talk
    matthew 0 1 anim
    matthew 114169 52528 walk-to
    101 actor 115039 52298 walk-to
    101 actor 0 1 anim
    101 actor 4294967295 actor ignore-a
    150 actor 115699 0 52368 set-pos
    -602 -2756 -1490 113831 0 52950 1103 camera-set
    matthew 4 5 talk
    91 actor 1312 1309 1 anim-then
    101 actor 5 6 talk
    this-one 115069 53318 walk-to
    matthew 592 200 turn-to
    matthew 6 0 talk
    7 music
    this-one 7 0 talk
    matthew 8 1 talk
    this-one wait-stop
    this-one 1343 1344 anim-at  this-one wait-anim
    this-one 9 0 talk
    matthew 10 5 talk
    this-one 1345 0 anim-at  this-one wait-anim
    101 actor 14 15 1 anim-then
    this-one 17 18 anim-at  this-one wait-anim
    this-one 11 0 talk
    101 actor 12 0 talk
    this-one 19 0 1 anim-then
    this-one 13 0 talk
    matthew 14 5 talk
    -428 -2180 -230 114587 108 52482 215 camera-set
    this-one 1348 1349 1 anim-then
    101 actor 15 0 talk
    this-one 16 0 talk
    this-one 1350 0 anim-at  this-one wait-anim
    this-one 17 3 talk
    matthew 18 5 talk
    this-one 1343 1344 anim-at  this-one wait-anim
    this-one 19 0 talk
    101 actor 16 0 anim-at  101 actor wait-anim
    -302 -884 -548 116069 132 53922 1415 camera-set
    15 frames wait
    101 actor 20 6 talk
    matthew 101 0 face
    154 actor 114169 0 52528 set-pos
    150 actor 0 0 0 set-pos
    10 frames wait
    -2768 -146 -2786 114911 2148 52500 -1184 camera-set
    matthew 137 0 anim
    matthew wait-anim
    matthew 154 actor ignore-a
    matthew 1353 1354 1 anim-then
    matthew 21 0 talk
    this-one 22 0 talk
    101 actor 23 0 talk
    matthew 24 0 talk
    this-one -1350 0 turn-to
    170 actor 115069 0 53318 set-pos
    -2468 -1568 -2438 114569 2208 54348 -2168 camera-set
    this-one 1345 0 anim-at  this-one wait-anim
    this-one 134 0 anim
    this-one wait-anim
    this-one 137 0 anim
    this-one wait-anim
    this-one 170 actor ignore-a
    this-one 1303 1304 1 anim-then
    115 sound
    this-one 25 0 talk
    91 actor 1310 1311 1 anim-then
    91 actor 26 0 talk
    matthew 1359 1360 anim-at  matthew wait-anim
    matthew 27 0 talk
    101 actor 28 5 talk
    matthew 1361 1354 anim-at  matthew wait-anim
    matthew 29 0 talk
    matthew 4294967295 actor ignore-a
    154 actor 114409 0 52408 set-pos
    154 actor -530 set-y
    154 actor 0 1120 0 set-rot
    154 actor 1330 0 anim
    154 actor anim-hold
    matthew 1329 0 1 anim-then
    154 actor wait-anim
    154 actor 0 0 0 set-pos
    101 actor 30 6 talk
    this-one 31 0 talk
    101 actor 114679 52298 walk-to
    101 actor 32 0 talk
    matthew 33 0 talk
    101 actor 34 6 talk
    101 actor 35 7 talk
    10 frames wait
    3 screen-effect
    wait-effect
    matthew 113759 0 52118 set-pos
    101 actor 114679 0 52118 set-pos
    101 actor 6 matthew 0 fight
    13 music
    goto L-80103C60
    ;

\ $80105264
: entry-80105264
    wolf as-this-one
    -686 -470 -674 114535 168 59940 3940 camera-set
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 5 rumble
    114541 168 59784 600 camera-move
    1 frames wait
    114601 168 59844 600 camera-move
    1 frames wait
    114517 168 59946 600 camera-move
    1 frames wait
    -686 -470 -674 114535 168 59940 3940 camera-set
    10 frames wait
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 5 rumble
    114541 168 59784 600 camera-move
    1 frames wait
    114601 168 59844 600 camera-move
    1 frames wait
    114517 168 59946 600 camera-move
    1 frames wait
    -686 -470 -674 114535 168 59940 3940 camera-set
    10 frames wait
    this-one 82 0 anim
    this-one wait-anim
    15 frames wait
    end-script
    ;

\ $8010543C
: entry-8010543C
    wolf as-this-one
    -686 -470 -674 112939 -1008 59562 988 camera-set
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 5 rumble
    112849 -1044 59460 600 camera-move
    1 frames wait
    112447 -984 59616 600 camera-move
    1 frames wait
    112453 -1128 59502 600 camera-move
    1 frames wait
    -686 -470 -674 112939 -1008 59562 988 camera-set
    10 frames wait
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 5 rumble
    112849 -1044 59460 600 camera-move
    1 frames wait
    112447 -984 59616 600 camera-move
    1 frames wait
    112453 -1128 59502 600 camera-move
    1 frames wait
    -686 -470 -674 112939 -1008 59562 988 camera-set
    10 frames wait
    this-one 82 0 anim
    this-one wait-anim
    15 frames wait
    end-script
    ;

\ $80105614
: entry-80105614
    99 actor as-this-one
    -686 -434 -788 114865 -1260 55974 1540 camera-set
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 8 rumble
    114865 -1164 56088 600 camera-move
    1 frames wait
    114775 -1248 55878 600 camera-move
    1 frames wait
    115057 -1284 55914 600 camera-move
    1 frames wait
    -686 -434 -788 114865 -1260 55974 1540 camera-set
    10 frames wait
    this-one 1391 0 anim
    this-one wait-anim
    115 sound
    15 frames wait
    end-script
    ;

\ $80105748
: entry-80105748
    wolf as-this-one
    -686 -470 -686 116401 -2556 60048 4096 camera-set
    this-one 1377 0 anim
    this-one wait-anim
    this-one 1354 1 anim
    113 sound
    14 screen-effect
    256 5 rumble
    180 8 rumble
    116263 -2556 60264 600 camera-move
    1 frames wait
    116509 -2664 60312 600 camera-move
    1 frames wait
    116425 -2496 60252 600 camera-move
    1 frames wait
    -686 -470 -686 116401 -2556 60048 4096 camera-set
    6 frames wait
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 5 rumble
    180 8 rumble
    116263 -2556 60264 600 camera-move
    1 frames wait
    116509 -2664 60312 600 camera-move
    1 frames wait
    116425 -2496 60252 600 camera-move
    1 frames wait
    -686 -470 -686 116401 -2556 60048 4096 camera-set
    6 frames wait
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 5 rumble
    180 8 rumble
    116263 -2556 60264 600 camera-move
    1 frames wait
    116509 -2664 60312 600 camera-move
    1 frames wait
    116425 -2496 60252 600 camera-move
    1 frames wait
    -686 -470 -686 116401 -2556 60048 4096 camera-set
    6 frames wait
    this-one 1343 0 anim
    this-one wait-anim
    this-one 1345 0 anim
    this-one wait-anim
    6 frames wait
    end-script
    ;

\ $80105D80
: entry-80105D80
    wolf as-this-one
    -686 -470 -674 115753 168 56496 3940 camera-set
    114823 120 56496 100 camera-move
    wait-camera-move
    4060 10 camera-angle
    wait-camera-turn
    this-one 113887 60538 5 50 walk-to-ex
    114475 120 60240 100 camera-move
    wait-camera-move
    this-one 111664 -1200 60612 set-pos
    this-one -1200 set-y
    this-one 0 6142 0 set-rot
    91 actor 111680 0 60310 set-pos
    91 actor -1200 set-y
    91 actor 0 6142 0 set-rot
    91 actor 0 1 anim
    99 actor 115620 0 55212 set-pos
    99 actor -1200 set-y
    99 actor 0 -470 0 set-rot
    99 actor 0 1 anim
    3268 -100 camera-angle
    wait-camera-turn
    -554 50 camera-height-to
    wait-camera-height
    this-one 1351 0 anim
    this-one anim-hold
    91 actor 111783 59541 walk-to
    this-one wait-anim
    this-one 111570 59548 walk-to
    3076 -10 camera-angle
    wait-camera-turn
    114013 120 61236 50 camera-move
    wait-camera-move
    111559 -1200 61236 50 camera-move
    91 actor 113742 58920 walk-to
    this-one 113578 59598 walk-to
    wait-camera-move
    -686 50 camera-height-to
    1564 -50 camera-angle
    wait-camera-turn
    91 actor -1024 -200 turn-to
    -956 30 camera-height-to
    91 actor 131 1 anim
    30 frames wait
    this-one -1024 -200 turn-to
    -686 30 camera-height-to
    wait-camera-height
    988 -15 camera-angle
    111559 -1200 59490 50 camera-move
    wait-camera-move
    this-one 1353 1354 1 anim-then
    30 frames wait
    end-script
    ;

: L-801059FC
    wolf as-this-one
    this-one 1388 0 anim
    this-one anim-hold
    91 actor 1389 0 anim
    91 actor anim-hold
    this-one wait-anim
    end-script
    ;

: L-80105A3C
    wolf as-this-one
    this-one 10 0 anim
    this-one anim-hold
    91 actor 10 0 anim
    91 actor anim-hold
    this-one wait-anim
    L-801059FC
    L-801059FC
    L-801059FC
    this-one 13 0 anim
    this-one anim-hold
    91 actor 13 0 anim
    91 actor anim-hold
    this-one wait-anim
    this-one 113857 57261 walk-to
    91 actor 113857 57451 walk-to
    this-one wait-stop
    this-one 114090 57261 walk-to
    91 actor 114090 57451 walk-to
    this-one wait-stop
    this-one 10 0 anim
    this-one anim-hold
    91 actor 10 0 anim
    91 actor anim-hold
    this-one wait-anim
    L-801059FC
    L-801059FC
    L-801059FC
    L-801059FC
    L-801059FC
    L-801059FC
    this-one 13 0 anim
    this-one anim-hold
    91 actor 13 0 anim
    91 actor anim-hold
    this-one wait-anim
    this-one 4096 0 turn-to
    this-one 1351 0 anim
    this-one anim-hold
    this-one wait-anim
    91 actor 116050 61035 walk-to
    this-one 116409 60920 walk-to
    this-one wait-stop
    91 actor wait-stop
    91 actor 111976 -4000 60276 set-pos
    91 actor -4000 set-y
    this-one 2048 0 turn-to
    end-script
    ;

\ $8010602C
: entry-8010602C
    wolf as-this-one
    -686 -470 -686 111559 -1200 59490 988 camera-set
    this-one 113576 58974 walk-to
    this-one wait-stop
    91 actor 2048 0 turn-to
    this-one 2048 0 turn-to
    ['] L-80105A3C start-event
    113599 -1200 59472 50 camera-move
    wait-camera-move
    2044 50 camera-angle
    wait-camera-turn
    113599 -1608 57354 50 camera-move
    wait-camera-move
    1024 -50 camera-angle
    wait-camera-turn
    169 actor 115620 0 55212 set-pos
    -536 30 camera-height-to
    wait-camera-height
    99 actor 169 actor ignore-a
    10 frames wait
    1540 60 camera-angle
    -788 60 camera-height-to
    wait-camera-height
    30 frames wait
    give-control
    end-script
    ;

: L-801064A4
    99 actor as-this-one
    -686 -434 -788 114271 -1452 56706 1540 camera-set
    this-one 113 0 anim
    15 sound
    this-one wait-anim
    this-one 114 1 anim
    60 frames wait
    end-script
    ;

\ $80106144
: entry-80106144
    1 set-scene
    L-801064A4
    wolf as-this-one
    -686 -470 -788 113599 -1608 57354 1540 camera-set
    1024 -50 camera-angle
    -536 50 camera-height-to
    wait-camera-height
    wait-camera-turn
    -686 20 camera-height-to
    116251 -2604 57348 50 camera-move
    wait-camera-move
    0 -60 camera-angle
    wait-camera-turn
    60 frames wait
    end-script
    ;

: L-80105C30
    wolf as-this-one
    this-one 115151 61284 5 50 walk-to-ex
    this-one wait-stop
    this-one 3072 0 turn-to
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 13 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 112601 60460 5 50 walk-to-ex
    this-one wait-stop
    end-script
    ;

\ $801061DC
: entry-801061DC
    ['] L-80105C30 start-event
    -686 -470 -686 116251 -2604 57348 4096 camera-set
    3064 -10 camera-angle
    116287 -2604 61344 50 camera-move
    wait-camera-move
    115435 -2604 61332 50 camera-move
    wait-camera-move
    112603 -3864 61314 50 camera-move
    wait-camera-move
    1936 -80 camera-angle
    wait-camera-turn
    75 frames wait
    end-script
    ;

\ $80106274
: entry-80106274
    -308 -2486 -650 115402 0 57150 1782 camera-set
    matthew 115342 0 57326 set-pos
    matthew 0 set-y
    matthew 0 4036 0 set-rot
    matthew 108 0 anim
    13 sound
    matthew wait-anim
    matthew 109 1 anim
    75 frames wait
    end-script
    ;

\ $80106300
: entry-80106300
    -218 -4394 -800 111559 -1212 59565 -1138 camera-set
    matthew 111541 0 59565 set-pos
    matthew -1200 set-y
    matthew 0 1046 0 set-rot
    matthew 108 0 anim
    13 sound
    matthew wait-anim
    matthew 109 1 anim
    75 frames wait
    end-script
    ;

\ $8010638C
: entry-8010638C
    -518 -4286 -1652 115099 -1780 55408 3722 camera-set
    matthew 113671 0 57334 set-pos
    matthew -1780 set-y
    matthew 0 1596 0 set-rot
    matthew 108 0 anim
    13 sound
    matthew wait-anim
    matthew 109 1 anim
    75 frames wait
    end-script
    ;

\ $80106418
: entry-80106418
    -464 -2846 -932 116264 -2800 59743 -2174 camera-set
    matthew 116288 -2800 57355 set-pos
    matthew -2800 set-y
    matthew 0 0 0 set-rot
    matthew 108 0 anim
    13 sound
    matthew wait-anim
    matthew 109 1 anim
    75 frames wait
    end-script
    ;

\ the words the main executable's scripts go to here, by address
' arrive-80104614 $80104614 5 overlay-word!
