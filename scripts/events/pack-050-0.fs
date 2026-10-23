\ pack-050-0.fs - event pack 050_0 (CD.BIN 250): 6 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-050-0
USING: events ;

\ $80010020
: script-0
    matthew 0 1 anim
    -470 -1750 -600 0 0 0 0 camera-set
    cohen camera-at
    matthew cohen camera-face
    4 0 camera-turn
    cohen 3 1 anim
    cohen 0 0 face
    cohen 0 1 anim
    15000 cohen say-by-wait
    matthew camera-at
    cohen matthew camera-face
    15001 matthew say-by-wait
    15002 matthew say-by-wait
    cohen camera-at
    matthew cohen camera-face
    15003 cohen say-by-wait
    15004 cohen say-by-wait
    11 screen-effect
    nozz 15513 -1202 422792 set-pos
    nozz 0 -767 0 set-rot
    nozz 29 30 1 anim-then
    matthew 19668 -1054 417540 set-pos
    cohen 19569 -1080 417895 set-pos
    matthew 18107 421463 run-to
    cohen 16062 422843 5 50 walk-to-ex
    camera-follow
    10 screen-effect
    35 frames wait
    camera-fixed
    -572 -3602 -680 15642 -1202 423231 -10846 camera-set
    44 frames wait
    100 38 camera-turn
    20 frames wait
    matthew 15641 423442 run-to
    cohen 16493 422903 5 50 walk-to-ex
    cohen wait-stop
    cohen 2 0 face
    15005 cohen say-by-wait
    15006 nozz say-by-wait
    15007 cohen say-by-wait
    matthew 2 -300 face
    15008 matthew say-by-wait
    15009 nozz say-by-wait
    15010 nozz say-by-wait
    15011 cohen say-by-wait
    15012 cohen say-by-wait
    15013 nozz say-by-wait
    3 screen-effect
    wait-effect
    -572 -3602 -680 24147 -314 410202 -2923 camera-set
    matthew 24622 -120 410209 set-pos
    matthew 0 -6189 0 set-rot
    cohen 25781 -14 408848 set-pos
    cohen 0 -633 0 set-rot
    morgan 25247 -72 408515 set-pos
    morgan 0 -8197 0 set-rot
    nozz 24620 -138 409332 set-pos
    nozz 0 -8174 0 set-rot
    nozz 0 1 anim
    60 frames wait
    2 screen-effect
    15014 nozz say-by-wait
    15015 nozz say-by-wait
    end-script
    ;

\ $80010350
: script-1
    camera-follow
    matthew 15800 424473 walk-to
    this-one wait-stop
    camera-fixed
    matthew 0 2982 0 set-rot
    matthew 0 1 anim
    153 actor 26 1 set-floor
    153 actor 16751 -1202 422057 set-pos
    153 actor as-this-one
    153 actor 1 actor-model
    -560 -3620 -404 15056 -1202 423933 2958 camera-set
    this-one 15978 423770 walk-to
    this-one wait-stop
    this-one 0 200 face
    this-one wait-turned
    153 actor 15016 7 talk
    matthew 153 -200 face
    matthew wait-turned
    this-one 3032 -100 turn-to
    this-one wait-turned
    15017 morgan say-by-wait
    matthew 15018 7 talk
    this-one 0 200 face
    this-one wait-turned
    153 actor 15019 5 talk
    20 frames wait
    11 screen-effect
    wait-effect
    20 frames wait
    -572 -2450 -644 15863 -1202 433295 -1420 camera-set
    matthew 15887 -1202 432958 set-pos
    matthew 0 -410 0 set-rot
    153 actor 15744 -1202 433335 set-pos
    153 actor -182 0 turn-to
    10 screen-effect
    wait-effect
    this-one 63 0 anim
    this-one anim-hold
    3 object-anim
    this-one wait-anim
    this-one 0 -200 face
    this-one wait-turned
    15020 morgan say-by
    wait-text
    this-one 10679 435980 walk-to
    matthew 15744 433335 walk-to
    matthew wait-stop
    matthew -182 0 turn-to
    matthew 63 0 anim
    matthew anim-hold
    3 object-anim
    matthew wait-anim
    matthew 15350 434071 walk-to
    matthew wait-stop
    camera-follow
    matthew 10897 435276 walk-to
    matthew wait-stop
    camera-fixed
    3 screen-effect
    wait-effect
    11 music
    matthew 26 1 set-floor
    8 set-scene
    matthew 9405 -806 435451 set-pos
    matthew 0 -1232 0 set-rot
    153 actor 8849 -770 435754 set-pos
    153 actor 0 -1393 0 set-rot
    153 actor as-this-one
    153 actor 1 actor-model
    -512 -2738 -716 9405 -806 435451 -1232 camera-set
    153 actor 1 ride
    camera-follow
    20 frames wait
    269 screen-effect
    matthew 6766 434413 walk-to
    matthew wait-stop
    camera-fixed
    153 actor unride
    153 actor 6546 434663 walk-to
    this-one wait-stop
    this-one 2535 0 turn-to
    matthew 15021 1 talk
    end-script
    ;

\ $80010704
: script-2
    153 actor as-this-one
    153 actor 1 actor-model
    153 actor 15022 7 talk
    matthew 15023 7 talk
    153 actor 15024 7 talk
    matthew 15025 7 talk
    153 actor 15026 7 talk
    matthew 15027 2 talk
    153 actor 15028 7 talk
    matthew 15029 0 talk
    this-one 0 -200 face
    this-one wait-turned
    153 actor 15030 7 talk
    0 cd-picture
    matthew 153 200 face
    matthew wait-turned
    matthew 15031 7 talk
    153 actor 15032 5 talk
    matthew 15033 6 talk
    153 actor 15034 7 talk
    0 screen-effect
    153 actor 1 actor-model
    102 actor 3 actor-model
    3 screen-effect
    wait-effect
    matthew 26 1 set-floor
    0 set-scene
    matthew 17205 -1202 430764 set-pos
    -572 -3602 -680 17205 -1202 430764 1868 camera-set
    153 actor 26 1 set-floor
    153 actor 17039 -770 430143 set-pos
    153 actor 0 -1393 0 set-rot
    102 actor 26 1 set-floor
    102 actor 17618 -1202 426365 set-pos
    102 actor 0 4030 0 set-rot
    102 actor 0 1 anim
    153 actor as-this-one
    15 frames wait
    camera-follow
    this-one 17690 427764 walk-to
    matthew 17934 428197 walk-to
    2 screen-effect
    wait-effect
    matthew wait-stop
    camera-fixed
    153 actor 15035 7 talk
    -572 -3602 -680 16575 -1202 426672 3012 camera-set
    this-one 17213 426750 walk-to
    matthew 17740 427172 walk-to
    this-one wait-stop
    153 actor 102 -200 face
    this-one 15036 7 3 talk-to
    this-one wait-turned
    153 actor 15037 7 talk
    matthew wait-stop
    matthew 15038 7 talk
    153 actor 17 18 1 anim-then
    153 actor 15039 0 talk
    153 actor 19 0 anim
    matthew 15040 7 talk
    this-one 15041 7 3 talk-to
    153 actor 0 -300 face
    this-one wait-turned
    153 actor 15042 5 talk
    matthew 15043 4 talk
    153 actor 102 200 face
    this-one wait-turned
    153 actor 15044 7 talk
    this-one 15045 7 3 talk-to
    15 frames wait
    11 screen-effect
    wait-effect
    102 actor 0 0 0 set-pos
    153 actor 0 0 0 set-pos
    camera-follow
    matthew 44367 2 415827 set-pos
    matthew 0 -3142 0 set-rot
    20 frames wait
    10 screen-effect
    wait-effect
    camera-fixed
    end-script
    ;

\ $80010B34
: script-3
    winona as-this-one
    winona matthew face-each-other
    this-one 15046 0 talk
    matthew 15047 7 talk
    this-one 15048 7 talk
    matthew 15049 6 talk
    this-one 135 60 anim-at  this-one wait-anim
    matthew 59 61 0 anim-then
    15 frames wait
    winona 61 0 anim
    matthew 133 51 anim-at  matthew wait-anim
    15 frames wait
    matthew 15051 0 talk
    this-one 15052 7 talk
    10 frames wait
    end-script
    ;

\ $80010BB8
: script-4
    20 frames wait
    matthew 15053 0 talk
    this-one 15054 3 talk
    matthew 141 0 anim-at  matthew wait-anim
    matthew 15055 0 talk
    this-one 15056 2 talk
    matthew 15057 2 talk
    winona 85 0 anim
    this-one 15058 0 talk
    end-script
    ;

\ $80010BF8
: script-5
    10 frames wait
    matthew 85 0 anim
    matthew 15059 0 talk
    this-one 15060 4 talk
    matthew 134 137 0 anim-then
    matthew wait-anim
    13 picture-show
    15061 3 say-mode-wait
    picture-hide
    10 frames wait
    end-script
    ;

\ its scripts by number
' script-0 0 250 pack-script!
' script-1 1 250 pack-script!
' script-2 2 250 pack-script!
' script-3 3 250 pack-script!
' script-4 4 250 pack-script!
' script-5 5 250 pack-script!
