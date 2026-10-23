\ pack-049-0.fs - event pack 049_0 (CD.BIN 245): 2 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-049-0
USING: events ;

\ $80010010
: script-0
    morgan as-this-one
    nozz 0 0 0 set-pos
    4 actor 0 0 0 set-pos
    60 actor 0 0 0 set-pos
    28 actor 105701 0 351029 set-pos
    28 actor 0 -70 0 set-rot
    31 actor 108857 0 353256 set-pos
    31 actor 0 2310 0 set-rot
    32 actor 107127 0 354681 set-pos
    32 actor 0 70 0 set-rot
    58 actor 103643 0 348801 set-pos
    58 actor 0 1260 0 set-rot
    30 actor 351185 0 278719 set-pos
    30 actor 0 1024 0 set-rot
    matthew 344765 0 284889 set-pos
    matthew 0 6566 0 set-rot
    matthew 0 1 anim
    this-one 346965 0 283149 set-pos
    this-one 0 -140 0 set-rot
    this-one 30 1 anim
    27 actor 347335 0 283689 set-pos
    27 actor 0 3036 0 set-rot
    -852 -1842 -664 343997 -3664 284435 582 camera-set
    map-sync
    2 screen-effect
    wait-effect
    matthew 81 0 anim
    matthew wait-anim
    matthew 17 19 0 anim-then
    1062 16 camera-angle
    20 frames wait
    matthew 1 -200 face
    this-one 15000 0 talk
    wait-camera-turn
    30 actor -1 340 move-forward
    this-one 15001 0 talk
    140 sound
    this-one 31 0 anim-at  this-one wait-anim
    this-one 0 -200 face
    this-one 15002 5 talk
    matthew 345125 284479 walk-to
    this-one 15003 6 talk
    matthew 15004 0 talk
    matthew wait-stop
    129 actor 335055 0 284609 set-pos
    matthew 347425 284269 walk-to
    10 frames wait
    this-one 347465 283159 walk-to
    this-one wait-stop
    this-one -80 0 turn-to
    this-one 5701 0 anim
    this-one anim-hold
    5 frames wait
    116 sound
    33 frames wait
    117 sound
    matthew wait-stop
    30 actor idle
    30 actor 0 0 0 set-pos
    127 actor 334780 0 275509 set-pos
    34 actor 341405 0 268139 set-pos
    34 actor 0 0 0 set-rot
    -1140 -2562 -1252 351017 -3700 282851 -882 camera-set
    129 actor 335055 290129 walk-to
    matthew 0 1994 0 set-rot
    matthew -4000 set-y
    matthew 5703 0 anim
    matthew anim-hold
    5 frames wait
    116 sound
    33 frames wait
    117 sound
    matthew wait-anim
    matthew 0 0 0 set-pos
    5 frames wait
    morgan 0 0 0 set-pos
    131 actor 333940 0 273049 set-pos
    131 actor 0 1024 0 set-rot
    47 actor 333940 0 271729 set-pos
    47 actor 0 3072 0 set-rot
    27 actor as-this-one
    10 frames wait
    this-one 350205 283689 -1 120 walk-to-ex
    this-one wait-stop
    10 frames wait
    127 actor 5 70 move-forward
    -1242 -24 camera-angle
    this-one -1 120 move-forward
    10 frames wait
    this-one 2500 -120 turn-to
    10 frames wait
    this-one -1 240 move-forward
    10 frames wait
    this-one 3072 200 turn-to
    this-one wait-turned
    this-one -1 220 move-forward
    34 actor -1 360 move-forward
    5 frames wait
    this-one idle
    40 frames wait
    this-one -1 220 move-forward
    10 frames wait
    34 actor idle
    34 actor 0 0 0 set-pos
    this-one -1 320 move-forward
    10 frames wait
    this-one -1 520 move-forward
    60 frames wait
    9 screen-effect
    wait-effect
    129 actor wait-stop
    129 actor 0 0 0 set-pos
    127 actor idle
    127 actor 0 0 0 set-pos
    this-one idle
    27 actor as-this-one
    -942 -1536 -820 111375 -24 344823 7740 camera-set
    map-sync
    10 frames wait
    8 screen-effect
    wait-effect
    10 frames wait
    15005 3 caption-for
    text-close
    167 actor 27 actor-model
    27 actor 107313 0 343946 set-pos
    27 actor 0 4192 0 set-rot
    27 actor -1 180 move-forward
    10 frames wait
    27 actor -1 160 move-forward
    20 frames wait
    27 actor -1 120 move-forward
    27 actor 5032 100 turn-to
    10 frames wait
    27 actor idle
    10 frames wait
    morgan as-this-one
    58 actor 0 0 0 set-pos
    -1008 -2622 -922 109221 300 349053 7812 camera-set
    5 frames wait
    116 sound
    33 frames wait
    117 sound
    15 frames wait
    morgan 108836 0 350026 set-pos
    morgan 0 3080 0 set-rot
    10 frames wait
    107313 300 349053 20 camera-move
    morgan 106996 350076 walk-to
    morgan wait-stop
    15 frames wait
    116 sound
    20 frames wait
    117 sound
    morgan 1610 -200 turn-to
    wait-camera-move
    matthew 108956 0 348526 set-pos
    matthew 107876 348816 walk-to
    matthew wait-stop
    matthew 107546 349536 walk-to
    matthew wait-stop
    this-one 15006 0 talk
    this-one 15007 7 talk
    this-one 15008 2 talk
    matthew 15009 7 talk
    matthew 85 0 1 anim-then
    matthew 15010 3 talk
    matthew 15011 7 talk
    this-one 15012 6 talk
    matthew 15013 4 talk
    matthew 15014 2 talk
    10 frames wait
    morgan 85 0 anim
    morgan wait-anim
    matthew 15015 0 talk
    matthew 15016 7 talk
    this-one 15017 7 talk
    matthew 15018 4 talk
    this-one 15019 5 talk
    matthew 15020 3 talk
    matthew 87 0 anim
    matthew wait-anim
    10 frames wait
    9 screen-effect
    wait-effect
    end-script
    ;

\ $800107BC
: script-1
    winona as-this-one
    matthew 104896 0 356324 set-pos
    matthew 0 1 anim
    matthew 0 2048 0 set-rot
    this-one 105966 0 355284 set-pos
    this-one 0 1 anim
    this-one 0 -630 0 set-rot
    32 actor 0 0 0 set-pos
    26 actor 106826 0 355284 set-pos
    26 actor 0 -3592 0 set-rot
    -600 -2562 -592 104834 0 355279 4072 camera-set
    map-sync
    2 screen-effect
    wait-effect
    30 frames wait
    15021 32767 actor say-by-wait
    matthew 17 18 anim-at  matthew wait-anim
    4744 20 camera-angle
    this-one 105756 355554 walk-to
    matthew 105466 355764 walk-to
    this-one wait-stop
    this-one 0 0 face
    matthew 15022 0 talk
    matthew wait-stop
    10 frames wait
    -534 -1152 -454 105986 0 355165 1704 camera-set
    15050 1 say-mode-wait
    15051 1 say-mode-wait
    15052 1 say-mode-wait
    10 frames wait
    this-one 15023 7 talk
    -666 -420 -628 105662 192 356161 2088 camera-set
    matthew 15024 4 talk
    this-one 15025 3 talk
    this-one 15026 7 talk
    matthew 15027 0 talk
    this-one 15028 7 talk
    matthew 15029 3 talk
    this-one 5743 5744 anim-at  this-one wait-anim
    10 frames wait
    this-one 15030 0 talk
    matthew 83 84 anim-at  matthew wait-anim
    15032 5 say-mode-wait
    15033 5 say-mode-wait
    matthew 85 0 anim-at  matthew wait-anim
    matthew 15031 6 talk
    this-one 15034 0 talk
    matthew 15035 7 talk
    this-one 5745 0 1 anim-then
    matthew 15036 6 talk
    this-one 86 0 anim
    this-one wait-anim
    this-one 15037 7 talk
    10 frames wait
    matthew 15038 4 talk
    -493 -1800 -358 106778 72 356377 2352 camera-set
    this-one 15039 6 talk
    matthew 17 18 1 anim-then
    matthew 15040 0 talk
    this-one 15041 4 talk
    matthew 19 0 1 anim-then
    matthew 15042 0 talk
    this-one 15043 3 talk
    matthew 15044 7 talk
    this-one 15045 4 talk
    10 frames wait
    265 screen-effect
    wait-effect
    10 frames wait
    15046 32767 actor say-by-wait
    15047 32767 actor say-by-wait
    15048 32767 actor say-by-wait
    15049 32767 actor say-by-wait
    end-script
    ;

\ its scripts by number
' script-0 0 245 pack-script!
' script-1 1 245 pack-script!
