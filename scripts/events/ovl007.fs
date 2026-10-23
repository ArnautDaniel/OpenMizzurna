\ ovl007.fs - area 007 overlay (CD.BIN 007_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl007
USING: events ;

: L-80102EB0
    10 frames wait
    26 32767 actor 3 say-timed-by-wait
    text-hold
    27 32767 actor 3 say-timed-by-wait
    text-hold
    28 32767 actor 3 say-timed-by-wait
    text-hold
    10 frames wait
    29 32767 actor 2 say-timed-by-wait
    text-hold
    end-script
    ;

: L-80102F14
    father-barton as-this-one
    129 actor 14 15 1 anim-then
    10 frames wait
    lidds 132443 317347 walk-to
    lidds wait-stop
    lidds 110 0 face
    lidds 90 0 anim
    110 actor 1706 87 0 anim-then
    this-one 131953 317727 walk-to
    20 frames wait
    129 actor 130403 317017 walk-to
    5 frames wait
    10 frames wait
    this-one 89 0 anim
    this-one wait-anim
    lidds 88 0 anim
    110 actor 87 0 anim
    110 actor wait-anim
    10 frames wait
    110 actor 130423 317277 walk-to
    lidds 90 0 anim
    10 frames wait
    this-one 87 0 anim
    this-one wait-anim
    lidds 130343 317347 walk-to
    20 frames wait
    this-one 14 15 1 anim-then
    end-script
    ;

: L-8010306C
    father-barton as-this-one
    matthew 131143 0 316747 set-pos
    matthew 0 480 0 set-rot
    matthew 0 1 anim
    142 actor 125563 0 320057 set-pos
    142 actor -400 set-y
    142 actor 0 140 0 set-rot
    142 actor 1713 1 anim
    winona 131373 0 316687 set-pos
    winona 0 190 0 set-rot
    winona 0 1 anim
    lidds 132553 0 317277 set-pos
    lidds 0 -490 0 set-rot
    lidds 0 1 anim
    this-one 131143 0 317737 set-pos
    this-one 0 770 0 set-rot
    this-one 57 1 anim
    110 actor 132283 0 317277 set-pos
    110 actor 0 -350 0 set-rot
    110 actor 1705 1 anim
    111 actor 132733 0 316357 set-pos
    111 actor 0 -490 0 set-rot
    111 actor 0 1 anim
    129 actor 131853 0 317017 set-pos
    129 actor 0 0 0 set-rot
    129 actor 0 1 anim
    -710 -764 -644 125581 312 320233 1896 camera-set
    map-sync
    2 screen-effect
    wait-effect
    142 actor 1714 1715 0 anim-then
    142 actor wait-anim
    1416 -10 camera-angle
    142 actor 1715 1715 0 anim-then
    142 actor wait-anim
    142 actor 1714 0 anim
    142 actor wait-anim
    100 0 sequence-play
    142 actor 1716 1717 1 anim-then
    5 frames wait
    142 actor 125563 -460 320747 -1 120 walk-path
    142 actor wait-stop
    142 actor 0 0 0 set-pos
    100 sequence-stop
    -500 -2816 -452 132823 48 317239 2088 camera-set
    map-sync
    131509 48 317239 10 camera-move
    ' L-80102EB0 start-event
    100 frames wait
    this-one 58 51 0 anim-then
    this-one wait-anim
    this-one 55 1703 0 anim-then
    this-one wait-anim
    10 frames wait
    -536 -2492 -620 132343 72 316135 1440 camera-set
    winona 14 15 1 anim-then
    30 frames wait
    winona 30 0 talk
    matthew 14 15 1 anim-then
    matthew 31 0 talk
    winona 32 0 talk
    matthew 33 0 talk
    winona 16 0 anim
    winona wait-anim
    winona 34 2 talk
    winona 85 0 anim
    winona wait-anim
    winona 14 15 1 anim-then
    111 actor 20 walk-forward
    111 actor -45 -100 turn-by
    30 frames wait
    winona 16 0 1 anim-then
    winona 132133 316157 120 face-point
    ' L-80102F14 start-event
    10 frames wait
    111 actor idle
    winona 35 0 talk
    111 actor 15 80 face
    111 actor wait-turned
    -524 -2384 -356 131623 84 316465 312 camera-set
    111 actor 36 0 talk
    matthew 16 0 anim
    matthew 111 120 face
    111 actor 89 0 anim
    111 actor wait-anim
    111 actor 37 0 talk
    20 frames wait
    winona 38 6 talk
    111 actor 39 7 talk
    111 actor 14 15 1 anim-then
    30 frames wait
    111 actor 40 0 talk
    matthew 41 5 talk
    111 actor 16 0 anim
    111 actor wait-anim
    111 actor 42 3 talk
    this-one 131803 317037 walk-to
    111 actor 14 16 0 anim-then
    111 actor wait-anim
    111 actor 43 7 talk
    111 actor 132133 315547 walk-to
    winona 44 5 talk
    129 actor 0 0 0 set-pos
    110 actor 0 0 0 set-pos
    lidds -2146932472 ending-schedule
    -470 -2168 -650 131959 84 316495 -480 camera-set
    this-one 45 0 talk
    winona 17 18 anim-at  winona wait-anim
    winona 46 0 talk
    this-one 47 3 talk
    matthew 14 15 anim-at  matthew wait-anim
    matthew 48 0 talk
    this-one 49 0 talk
    20 frames wait
    matthew 16 0 anim-at  matthew wait-anim
    father-barton 14 15 anim-at  father-barton wait-anim
    this-one 50 0 talk
    father-barton 16 0 anim-at  father-barton wait-anim
    this-one 51 0 talk
    winona -2146887836 ending-schedule
    111 actor wait-stop
    111 actor 0 0 0 set-pos
    end-script
    ;

\ $801027D8
: entry-801027D8
    father-barton as-this-one
    this-one -1865 290 turn-to
    -512 -2738 -716 132307 0 317403 -954 camera-set
    this-one 79 1 talk
    this-one 80 0 talk
    20 frames wait
    3 screen-effect
    80 frames wait
    goto L-8010306C
    ;

\ $80102844
: entry-80102844
    11 screen-effect
    wait-effect
    27 -1 -1 -1 -1 0 warp
    ;

\ $801028AC
: entry-801028AC
    101 actor as-this-one
    matthew 138316 0 308736 set-pos
    matthew 0 1500 0 set-rot
    -548 -2954 -788 138316 0 308736 5723 camera-set
    mel camera-at
    20 frames wait
    1 matthew 1 say-timed-by-wait
    text-hold
    20 frames wait
    matthew 81 0 anim
    2 matthew 2 say-timed-by-wait
    text-hold
    matthew wait-anim
    9 screen-effect
    wait-effect
    10 frames wait
    matthew 131072 0 307462 set-pos
    matthew 0 2048 0 set-rot
    matthew 18 1 anim
    this-one 136312 0 307722 set-pos
    this-one 0 1 anim
    40 actor 134852 0 308902 set-pos
    40 actor 0 -840 0 set-rot
    mel 134902 0 308732 set-pos
    mel 0 2180 0 set-rot
    mel 0 1 anim
    -968 -188 -950 132562 812 306568 8670 camera-set
    text-close
    11 music
    8 screen-effect
    wait-effect
    20 frames wait
    mel 17 18 1 anim-then
    20 frames wait
    mel 19 0 anim
    mel wait-anim
    mel 133942 308322 walk-to
    5 frames wait
    -758 -2204 -884 130978 -100 306460 8646 camera-set
    mel wait-stop
    mel 83 84 1 anim-then
    50 frames wait
    this-one 3 0 talk
    8814 8 camera-angle
    this-one 134572 308142 walk-to
    mel 85 17 0 anim-then
    mel -3051 -200 turn-to
    this-one wait-stop
    mel 4 0 talk
    -458 -3536 -146 134182 -100 308698 -170 camera-set
    this-one 5 1 talk
    mel 4 0 talk
    mel 6 5 talk
    this-one 7 6 talk
    mel 8 5 talk
    this-one 9 6 talk
    this-one 83 84 1 anim-then
    20 frames wait
    this-one 85 0 anim
    this-one wait-anim
    this-one 10 5 talk
    15 music
    mel 11 7 talk
    this-one 12 3 talk
    mel 13 5 talk
    this-one 14 1 talk
    this-one 15 5 talk
    mel 16 0 talk
    this-one 17 6 talk
    mel 18 7 talk
    this-one 86 0 anim
    this-one wait-anim
    this-one 134932 308002 walk-to
    this-one wait-stop
    this-one 14 15 anim-at  this-one wait-anim
    this-one 19 0 talk
    this-one 16 0 anim-at  this-one wait-anim
    -758 -2204 -884 130726 -100 306280 8742 camera-set
    this-one 136852 307942 walk-to
    this-one wait-stop
    mel 1700 1701 1 anim-then
    20 frames wait
    mel 1702 0 anim
    mel wait-anim
    matthew 83 84 1 anim-then
    mel 2048 200 turn-to
    mel 14 15 1 anim-then
    30 frames wait
    matthew 85 0 anim
    matthew wait-anim
    mel 16 0 anim
    mel wait-anim
    mel 134862 308822 walk-to
    mel wait-stop
    mel 0 3168 0 set-rot
    mel 1707 1708 0 anim-then
    mel wait-anim
    mel 1710 1711 1 anim-then
    ovl007 $801026C8 native
    40 actor -1 40 move-forward
    5 frames wait
    40 actor -180 -90 turn-by
    5 frames wait
    40 actor -1 80 move-forward
    30 frames wait
    camera-follow
    40 actor idle
    this-one 0 0 0 set-pos
    ovl007 $801026EC native
    mel -2146929300 ending-schedule
    20 frames wait
    end-script
    ;

\ $801037C4
: entry-801037C4
    father-barton as-this-one
    matthew 139694 0 308470 set-pos
    matthew 0 -5073 0 set-rot
    matthew 0 1 anim
    winona 137977 0 309129 set-pos
    winona 0 1260 0 set-rot
    winona 0 1 anim
    -548 -3098 -1094 139514 0 308434 -5073 camera-set
    10 frames wait
    winona 139164 308720 walk-to
    winona 21 0 talk
    winona wait-stop
    matthew 15 0 face
    matthew 22 5 talk
    winona 23 4 talk
    matthew 24 7 talk
    winona 25 7 talk
    20 frames wait
    3 screen-effect
    wait-effect
    goto L-8010306C
    ;

\ $801038FC
: entry-801038FC
    father-barton as-this-one
    matthew 71 7 talk
    this-one 72 0 talk
    matthew 73 6 talk
    this-one 74 0 talk
    this-one 75 0 talk
    matthew 76 3 talk
    this-one 77 0 talk
    this-one 78 0 talk
    end-script
    ;

\ $80103968
: entry-80103968
    -314 -1946 -932 119463 -40 306581 2431 camera-set
    matthew 48 50 anim-at  matthew wait-anim
    0 5 say-mode-wait
    matthew 49 0 anim-at  matthew wait-anim
    5 frames wait
    end-script
    ;

\ $80103A88
: entry-80103A88
    156 actor as-this-one
    matthew 119737 0 306262 set-pos
    matthew 0 7802 0 set-rot
    matthew 0 1 anim
    -512 -2168 -716 119471 0 306552 9404 camera-set
    this-one 1706 0 anim
    this-one wait-anim
    156 actor 52 0 talk
    matthew 53 7 talk
    this-one 150 151 1 anim-then
    156 actor 54 0 talk
    matthew 55 7 talk
    156 actor 56 0 talk
    matthew 57 7 talk
    156 actor 58 0 talk
    this-one 153 0 anim
    this-one wait-anim
    this-one 0 -200 face
    this-one wait-turned
    156 actor 59 5 talk
    matthew 60 7 talk
    156 actor 61 6 talk
    156 actor 62 7 talk
    matthew 63 7 talk
    156 actor 64 7 talk
    156 actor 65 7 talk
    156 actor 66 7 talk
    matthew 67 7 talk
    156 actor 68 7 talk
    this-one 119894 307949 walk-to
    this-one wait-stop
    156 actor 0 0 0 set-pos
    end-script
    ;
