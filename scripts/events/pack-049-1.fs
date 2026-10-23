\ pack-049-1.fs - event pack 049_1 (CD.BIN 246): 2 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-049-1
USING: events ;

\ $80010010
: script-0
    james as-this-one
    26 actor 0 0 0 set-pos
    matthew 107115 0 53442 set-pos
    matthew 0 628 0 set-rot
    matthew 0 1 anim
    this-one 107390 0 53677 set-pos
    this-one 27 1 set-floor
    154 actor 107390 0 53677 set-pos
    this-one 0 2730 0 set-rot
    this-one 15 1 anim
    32 actor 106985 0 52257 set-pos
    32 actor 0 1680 0 set-rot
    -1314 -8610 -322 106672 -200 58765 940 camera-set
    map-sync
    106672 -200 54121 18 camera-move
    8 screen-effect
    wait-effect
    wait-camera-move
    matthew 15000 0 talk
    this-one 15001 0 talk
    -582 -2202 -628 107032 -128 53739 1516 camera-set
    james 154 actor ignore-a
    this-one 15002 0 talk
    matthew 15003 4 talk
    james 16 0 anim-at  james wait-anim
    this-one 15004 0 talk
    james 133 51 anim-at  james wait-anim
    matthew 15005 0 talk
    this-one 15006 0 talk
    james 141 0 anim-at  james wait-anim
    matthew 59 61 0 anim-then
    james 59 61 0 anim-then
    12 frames wait
    this-one 4294967295 actor ignore-a
    matthew 154 actor ignore-a
    matthew 15007 0 talk
    this-one 15008 5 talk
    matthew 15009 4 talk
    matthew 134 0 anim
    matthew wait-anim
    matthew 4294967295 actor ignore-a
    154 actor 0 0 0 set-pos
    matthew 137 0 0 anim-then
    matthew wait-anim
    this-one 15010 7 talk
    10 frames wait
    this-one 104870 57157 5 60 walk-to-ex
    15 frames wait
    matthew 0 -282 0 set-rot
    -684 -2724 -142 107655 -176 53082 -260 camera-set
    this-one wait-stop
    this-one 104870 58597 5 60 walk-to-ex
    this-one wait-stop
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80010334
: script-1
    morgan as-this-one
    32 actor 0 0 0 set-pos
    34 actor 0 0 0 set-pos
    this-one 107810 0 52247 set-pos
    this-one 0 3150 0 set-rot
    this-one 15 1 anim
    152 actor 106155 0 54042 set-pos
    152 actor 0 1 anim
    10 actor 106445 0 54712 set-pos
    10 actor 0 1 anim
    10 actor 0 -770 0 set-rot
    108 actor 107235 0 54702 set-pos
    108 actor 0 1 anim
    108 actor 0 700 0 set-rot
    131 actor 105985 0 55032 set-pos
    131 actor 0 420 0 set-rot
    58 actor 108825 0 56152 set-pos
    58 actor 0 -840 0 set-rot
    -1104 -4614 -574 107320 -32 55893 412 camera-set
    map-sync
    8 screen-effect
    wait-effect
    152 actor 109335 55002 walk-to
    10 actor 40 42 0 anim-then
    10 actor wait-anim
    10 actor 51 1 anim
    108 actor 10 200 face
    108 actor wait-turned
    10 actor 141 0 anim-at  10 actor wait-anim
    131 actor -1 60 move-forward
    10 actor 108 200 face
    10 actor wait-turned
    131 actor -840 -42 turn-to
    10 actor 89 0 anim-at  10 actor wait-anim
    10 actor 106985 54722 walk-to
    10 actor wait-stop
    131 actor idle
    131 actor 0 0 0 set-pos
    152 actor 0 0 0 set-pos
    3 screen-effect
    wait-effect
    59 actor 106155 0 53302 set-pos
    59 actor 0 -700 0 set-rot
    -714 -618 -682 107632 160 51501 -140 camera-set
    map-sync
    2 screen-effect
    wait-effect
    10 actor 90 0 anim-at  10 actor wait-anim
    108 actor 87 0 anim-at  108 actor wait-anim
    108 actor 108615 55482 walk-to
    10 actor 5740 5741 anim-at  10 actor wait-anim
    108 actor wait-stop
    108 actor 0 0 0 set-pos
    hudson 106575 0 52442 set-pos
    20 frames wait
    10 actor 5742 0 anim-at  10 actor wait-anim
    10 actor 4000 -200 turn-to
    hudson 107235 52442 walk-to
    this-one 16 0 1 anim-then
    hudson wait-stop
    hudson 89 0 anim-at  hudson wait-anim
    hudson 15011 7 talk
    hudson 15012 3 talk
    this-one 15013 5 talk
    this-one 87 0 anim-at  this-one wait-anim
    3 screen-effect
    wait-effect
    matthew 95345 0 51712 set-pos
    matthew 0 -142 0 set-rot
    matthew 0 1 anim
    94 actor 95375 0 51892 set-pos
    94 actor 0 1890 0 set-rot
    94 actor 0 1 anim
    50 actor 93775 0 54022 set-pos
    50 actor 0 -770 0 set-rot
    26 actor 99525 0 59512 set-pos
    26 actor 0 -2122 0 set-rot
    this-one 96725 0 51882 set-pos
    -876 -1410 -736 96220 160 51357 3604 camera-set
    map-sync
    2 screen-effect
    wait-effect
    94 actor 88 0 anim-at  94 actor wait-anim
    matthew 87 0 anim-at  matthew wait-anim
    94 actor 86 0 anim-at  94 actor wait-anim
    matthew 89 0 anim-at  matthew wait-anim
    94 actor 17 18 anim-at  94 actor wait-anim
    this-one 95815 52012 walk-to
    matthew 14 15 1 anim-then
    -816 -1914 -736 95716 172 52293 4780 camera-set
    94 actor 0 1 anim
    94 actor 840 -200 turn-to
    26 actor 99525 50272 -1 240 walk-to-ex
    94 actor wait-turned
    94 actor 87 0 anim-at  94 actor wait-anim
    94 actor 94835 52922 walk-to
    94 actor wait-stop
    94 actor 0 0 0 set-pos
    50 actor 0 0 0 set-pos
    winona 96975 0 51412 set-pos
    this-one 0 0 face
    59 actor 98885 0 51472 set-pos
    59 actor 0 0 0 set-rot
    -690 -1548 -658 95867 -92 51922 4562 camera-set
    matthew 0 1 anim
    matthew 1 200 face
    59 actor 98885 54352 -1 80 walk-to-ex
    this-one 15014 5 talk
    matthew 15015 1 talk
    this-one 15016 7 talk
    matthew 15017 5 talk
    this-one 15018 4 talk
    59 actor 98885 65032 -1 240 walk-to-ex
    matthew 15019 0 talk
    winona 15020 0 talk
    winona 95655 51552 5 60 walk-to-ex
    matthew -3012 200 turn-to
    winona wait-stop
    winona 29 30 anim-at  winona wait-anim
    matthew 15021 0 talk
    winona 31 0 anim-at  winona wait-anim
    59 actor wait-stop
    59 actor 0 0 0 set-pos
    58 actor 99725 0 51532 set-pos
    58 actor 0 0 0 set-rot
    winona 130 131 anim-at  winona wait-anim
    winona 15022 0 talk
    matthew 87 0 anim-at  matthew wait-anim
    this-one 15023 3 talk
    matthew 17 18 anim-at  matthew wait-anim
    winona 132 0 1 anim-then
    matthew 15024 0 talk
    58 actor 99725 68812 -1 180 walk-to-ex
    matthew 15025 0 talk
    this-one 15026 7 talk
    matthew 0 1 anim
    matthew 1 -200 face
    matthew 15027 0 talk
    this-one 15028 3 talk
    matthew 15029 5 talk
    58 actor wait-stop
    58 actor 0 0 0 set-pos
    10 actor 98585 0 51772 set-pos
    matthew 15030 1 talk
    this-one 15031 5 talk
    matthew 1154 200 turn-to
    matthew 15032 0 talk
    winona 15033 6 talk
    this-one 15034 5 talk
    matthew 15035 0 talk
    -510 -1158 -484 95705 -200 51772 5486 camera-set
    10 actor 95945 51772 5 60 walk-to-ex
    10 frames wait
    winona 386 200 turn-to
    15 frames wait
    this-one 17 18 1 anim-then
    10 actor 15036 0 talk
    10 actor wait-stop
    10 actor 1 200 face
    this-one -2514 -200 turn-to
    this-one 0 1 anim
    this-one 15037 0 talk
    10 actor 15038 5 talk
    this-one 15039 1 talk
    winona 15040 0 talk
    matthew 15041 1 talk
    this-one 15042 4 talk
    9 screen-effect
    wait-effect
    167 actor 27 actor-model
    matthew 100515 0 50695 set-pos
    matthew 0 -130 0 set-rot
    26 actor 100234 0 49756 set-pos
    26 actor 0 -449 0 set-rot
    morgan 0 0 0 set-pos
    167 actor 99645 0 55812 set-pos
    10 actor 0 0 0 set-pos
    winona 0 0 0 set-pos
    59 actor 101165 0 54612 set-pos
    59 actor 0 2048 0 set-rot
    91 actor 0 0 0 set-pos
    -528 -2052 -394 100695 0 50551 -226 camera-set
    map-sync
    8 screen-effect
    wait-effect
    10 frames wait
    167 actor 99645 73682 -1 240 walk-to-ex
    167 actor wait-stop
    167 actor 0 0 0 set-pos
    10 actor 101665 0 52872 set-pos
    10 actor 0 3072 0 set-rot
    map-sync
    15043 5 say-mode-wait
    15044 5 say-mode-wait
    15045 5 say-mode-wait
    20 frames wait
    11 screen-effect
    wait-effect
    26 0 56971 410905 0 0 warp
    ;

\ its scripts by number
' script-0 0 246 pack-script!
' script-1 1 246 pack-script!
