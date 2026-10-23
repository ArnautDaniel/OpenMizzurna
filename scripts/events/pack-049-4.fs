\ pack-049-4.fs - event pack 049_4 (CD.BIN 249): 5 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-049-4
USING: events ;

\ $8001001C
: script-0
    -1 music
    -560 -2982 -1752 46025 0 414944 3624 camera-set
    matthew 46046 0 415920 set-pos
    matthew 1 0 face
    matthew 0 1 anim
    matthew 60 run
    15 frames wait
    matthew idle
    15000 nozz say-by
    nozz 17 18 1 anim-then
    matthew 29 30 1 anim-then
    morgan 0 160 face
    wait-text
    10 actor 47514 0 415782 set-pos
    10 actor 0 0 face
    10 actor 70 run
    5 frames wait
    matthew 31 0 anim
    21 frames wait
    10 actor idle
    10 actor 29 30 1 anim-then
    16 sound
    10 frames wait
    16 sound
    10 frames wait
    16 sound
    10 frames wait
    15001 10 actor say-by
    wait-text
    15002 matthew say-by
    matthew 82 88 0 anim-then
    wait-text
    15003 morgan say-by
    morgan 89 86 0 anim-then
    wait-text
    -902 -882 -750 45641 636 414897 -4752 camera-set
    nozz 19 0 anim
    nozz wait-anim
    morgan 17 18 1 anim-then
    nozz 88 89 0 anim-then
    15004 nozz say-by-wait
    15005 morgan say-by
    morgan 19 0 anim
    morgan wait-anim
    morgan 86 0 anim
    wait-text
    nozz 88 81 0 anim-then
    15006 nozz say-by-wait
    morgan 83 84 1 anim-then
    15007 morgan say-by-wait
    matthew 82 88 0 anim-then
    15008 matthew say-by-wait
    15007 morgan say-by-wait
    morgan 85 86 0 anim-then
    15009 morgan say-by-wait
    15010 nozz say-by
    nozz 88 87 0 anim-then
    nozz 89 0 anim
    wait-text
    9 screen-effect
    wait-effect
    10 frames wait
    cohen 8 goto-stop
    cohen 34480 0 409500 set-pos
    cohen 26 1 set-floor
    cohen 0 set-y
    cohen 36480 408492 0 face-point
    cohen 20 walk-forward
    -1076 -4596 -666 37305 180 409177 2539 camera-set
    map-sync
    8 screen-effect
    wait-effect
    60 frames wait
    3 screen-effect
    wait-effect
    11 music
    nozz 8 goto-stop
    morgan 12 goto-stop
    nozz 36250 0 406942 set-pos
    nozz 35384 407764 0 face-point
    nozz 5546 1 anim
    172 actor 36250 0 406942 set-pos
    matthew 35201 0 407077 set-pos
    matthew 2 0 face
    matthew 17 18 1 anim-then
    morgan 35384 0 407764 set-pos
    morgan 0 0 face
    morgan 83 84 1 anim-then
    7 set-scene
    -1112 -1212 -1320 34644 312 406924 4858 camera-set
    map-sync
    nozz 172 actor ignore-a
    2 screen-effect
    wait-effect
    10 actor 54415 0 416640 set-pos
    10 actor 1538 0 turn-to
    cohen idle
    matthew 19 0 anim
    matthew wait-anim
    -458 774 -516 35941 0 407027 -1809 camera-set
    cohen 100 0 through-door
    cohen 36475 0 408052 set-pos
    15011 nozz say-by-wait
    -1112 -1212 -1320 34644 312 406924 4858 camera-set
    nozz 5547 0 anim
    cohen 36250 407642 150 face-point
    cohen 36250 407642 walk-to
    matthew 35691 407137 150 face-point
    matthew 35691 407137 walk-to
    morgan 85 0 anim
    morgan wait-anim
    morgan 17 18 1 anim-then
    nozz wait-anim
    nozz 5538 1 anim
    nozz 3 120 face
    matthew wait-stop
    matthew 3 0 face
    cohen 0 120 face
    cohen 81 0 anim
    15012 cohen say-by-wait
    morgan 19 0 anim
    morgan wait-anim
    morgan 83 84 1 anim-then
    15013 morgan say-by-wait
    cohen 15014 3 talk
    matthew 82 88 0 anim-then
    15015 matthew say-by-wait
    morgan 85 88 0 anim-then
    15016 morgan say-by-wait
    nozz 1 -120 face
    15017 nozz say-by-wait
    morgan 17 18 1 anim-then
    cohen 17 18 anim-at  cohen wait-anim
    cohen 15018 3 talk
    matthew 86 0 anim
    matthew wait-anim
    30 frames wait
    3 screen-effect
    wait-effect
    0 set-scene
    cohen 36770 0 409062 set-pos
    cohen 36330 409132 0 face-point
    cohen 5538 1 anim
    24 actor 36770 0 409062 set-pos
    nozz 36230 0 409132 set-pos
    nozz 3 0 face
    nozz 5538 1 anim
    matthew 36521 0 409647 set-pos
    matthew 3 0 face
    morgan 36044 0 409844 set-pos
    morgan 3 0 face
    -1190 -3792 -1824 37557 480 409789 2731 camera-set
    map-sync
    cohen 24 actor ignore-a
    2 screen-effect
    wait-effect
    morgan 19 0 anim
    morgan wait-anim
    15019 cohen say-by
    cohen 5541 5538 1 anim-then
    wait-text
    15020 morgan say-by
    morgan 14 15 1 anim-then
    nozz 1 -80 face
    wait-text
    15021 nozz say-by
    nozz 5541 5538 1 anim-then
    wait-text
    nozz 34980 410692 -80 face-point
    nozz 5531 30 move-forward
    35 frames wait
    morgan 34980 410692 80 face-point
    morgan 16 0 anim
    morgan wait-anim
    morgan 34242 411178 walk-to
    30 frames wait
    15022 cohen say-by
    morgan 0 80 face
    cohen 5541 5538 1 anim-then
    wait-text
    matthew 87 0 anim
    matthew wait-anim
    15023 cohen say-by
    wait-text
    nozz idle
    morgan wait-stop
    nozz 4294967295 actor ignore-a
    morgan 56330 0 509132 set-pos
    nozz 56330 0 509132 set-pos
    172 actor 0 0 0 set-pos
    end-script
    ;

\ $800108EC
: script-1
    cohen 32315 0 415525 set-pos
    cohen 0 80 0 set-rot
    cohen 5538 1 anim
    camera-follow
    matthew 32209 415160 walk-to
    matthew wait-stop
    camera-fixed
    matthew 3 0 face
    -1022 3048 -564 32369 450 415480 -19427 camera-set
    10 frames wait
    cohen 5543 1 anim
    10 frames wait
    15024 matthew say-by
    matthew 88 0 anim
    wait-text
    cohen 5544 0 anim
    cohen wait-anim
    cohen 5542 5543 1 anim-then
    15025 cohen say-by
    wait-text
    15026 matthew say-by
    matthew 89 0 anim
    wait-text
    15027 cohen say-by
    cohen 5544 5538 1 anim-then
    cohen 0 -120 face
    wait-text
    15028 cohen say-by
    wait-text
    end-script
    ;

\ $80010A28
: script-2
    matthew 30965 0 416675 set-pos
    matthew 0 -670 0 set-rot
    cohen 30695 0 415905 set-pos
    cohen 5538 1 anim
    cohen 0 -160 0 set-rot
    -466 120 -2190 30852 0 416635 -2170 camera-set
    matthew 48 0 anim
    matthew wait-anim
    matthew 50 1 anim
    10 frames wait
    15030 matthew say-by
    wait-text
    cohen 30695 416525 walk-to
    matthew 49 0 anim
    cohen wait-stop
    cohen 5545 0 anim
    cohen wait-anim
    cohen 5546 1 anim
    10 frames wait
    15031 cohen say-by
    wait-text
    cohen 5547 0 anim
    cohen wait-anim
    cohen 5538 0 anim
    15032 cohen say-by
    wait-text
    end-script
    ;

\ $80010B58
: script-3
    cohen 31093 -800 426081 set-pos
    cohen 0 840 0 set-rot
    cohen 5543 1 anim
    matthew 31203 0 424551 set-pos
    -590 -1404 -402 29833 -656 425331 -3302 camera-set
    20 frames wait
    matthew 31153 425591 walk-to
    matthew wait-stop
    matthew 3 0 face
    10 frames wait
    cohen 5544 5538 anim-at
    cohen wait-anim
    15033 cohen say-by
    wait-text
    matthew 45 70 turn-by
    10 frames wait
    matthew 81 0 anim
    matthew wait-anim
    15035 cohen say-by
    wait-text
    15036 cohen say-by
    wait-text
    cohen 30203 426081 130 face-point
    100 -1260 -882 30961 -1424 425769 -4838 camera-set
    cohen wait-turned
    15038 cohen say-by
    matthew 30203 426081 -130 face-point
    wait-text
    end-script
    ;

\ $80010CAC
: script-4
    matthew 32504 -800 426679 set-pos
    matthew 0 6933 0 set-rot
    cohen 32313 -800 427751 set-pos
    cohen 5538 1 anim
    cohen 0 2302 0 set-rot
    -722 -1306 -2016 32408 -800 427093 6455 camera-set
    matthew 32 34 0 anim-then
    matthew wait-anim
    cohen 32313 426931 walk-to
    15040 matthew say-by
    matthew 35 33 0 anim-then
    cohen wait-stop
    cohen 5538 1 anim
    wait-text
    matthew wait-anim
    -494 686 -336 32756 -692 427243 8639 camera-set
    matthew 14 15 1 anim-then
    15041 matthew say-by
    wait-text
    15042 cohen say-by
    wait-text
    104 0 sequence-play
    3 frames wait
    cohen -120 -130 turn-by
    6 frames wait
    matthew -120 -100 turn-by
    matthew 16 0 anim
    -722 -1306 -2016 32408 -800 427093 6455 camera-set
    15047 cohen say-by
    matthew 81 0 anim
    wait-text
    109 sound
    cohen 5542 5543 1 anim-then
    15043 matthew say-by
    wait-text
    cohen 5544 5538 1 anim-then
    15044 cohen say-by
    wait-text
    cohen 5542 5543 1 anim-then
    15045 cohen say-by
    wait-text
    end-script
    ;

\ its scripts by number
' script-0 0 249 pack-script!
' script-1 1 249 pack-script!
' script-2 2 249 pack-script!
' script-3 3 249 pack-script!
' script-4 4 249 pack-script!
