\ pack-050-2.fs - event pack 050_2 (CD.BIN 252): 5 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-050-2
USING: events ;

\ $8001001C
: script-0
    58 actor 26 1 set-floor
    58 actor 57101 2 415865 set-pos
    58 actor 0 -210 0 set-rot
    108 actor 26 1 set-floor
    108 actor 58439 0 414057 set-pos
    108 actor 0 11762 0 set-rot
    108 actor 5581 1 anim
    james 26 1 set-floor
    james 6 actor-model
    james 57370 0 414381 set-pos
    james 0 987 0 set-rot
    james 114 1 anim
    -494 -2756 -752 56705 0 410553 544 camera-set
    matthew 56705 0 410553 set-pos
    matthew 0 set-y
    matthew 0 544 0 set-rot
    matthew 0 1 anim
    james as-this-one
    camera-follow
    matthew 15000 0 talk
    matthew 57383 414088 5 70 walk-to-ex
    matthew wait-stop
    camera-fixed
    -152 -1154 -524 57425 2 414200 -814 camera-set
    matthew 0 27 0 set-rot
    matthew 150 151 1 anim-then
    this-one 15001 0 talk
    matthew 15002 0 talk
    this-one 15003 0 talk
    matthew 15004 0 talk
    this-one 15005 0 talk
    matthew 15006 0 talk
    matthew 153 0 anim
    matthew wait-anim
    end-script
    ;

\ $80010234
: script-1
    153 actor as-this-one
    153 actor 3 actor-model
    153 actor 26 1 set-floor
    153 actor 40174 -770 412792 set-pos
    24 actor 40174 -770 412792 set-pos
    153 actor 0 -3345 0 set-rot
    153 actor 5538 1 anim
    91 actor 32784 0 427096 set-pos
    91 actor 0 -1980 0 set-rot
    91 actor 109 1 anim
    91 actor -760 set-y
    153 actor as-this-one
    camera-follow
    matthew 42227 415365 5 70 walk-to-ex
    matthew wait-stop
    camera-fixed
    -572 -1406 -680 40316 2 412975 -1659 camera-set
    153 actor 24 actor ignore-a
    this-one 0 -300 face
    this-one wait-turned
    this-one 15007 0 3 talk-to
    -572 -1730 -680 40257 2 413021 -975 camera-set
    matthew 40371 413287 5 70 walk-to-ex
    matthew wait-stop
    matthew 15008 1 talk
    this-one 15009 6 3 talk-to
    matthew 15010 1 talk
    this-one 15011 0 3 talk-to
    matthew 15012 7 talk
    this-one 15013 0 3 talk-to
    this-one 0 -200 face
    this-one wait-turned
    153 actor 59 60 1 anim-then
    matthew 15014 7 talk
    matthew 40245 413135 walk-to
    matthew wait-stop
    matthew 59 60 0 anim-then
    matthew wait-anim
    153 actor 4294967295 actor ignore-a
    matthew 24 actor ignore-a
    matthew 61 0 anim
    this-one 61 0 anim
    matthew wait-anim
    this-one 15015 7 3 talk-to
    153 actor 35611 414291 5 70 walk-to-ex
    this-one wait-stop
    153 actor 0 0 0 set-pos
    end-script
    ;

\ $800104D4
: script-2
    james as-this-one
    matthew 32754 0 426896 set-pos
    matthew 0 -700 0 set-rot
    matthew 5525 1 anim
    91 actor 32784 0 427096 set-pos
    91 actor 0 -1980 0 set-rot
    91 actor 109 1 anim
    91 actor -760 set-y
    -506 -2336 -230 33078 -716 426788 7336 camera-set
    matthew 5545 5546 anim-at  matthew wait-anim
    matthew 15016 0 talk
    91 actor 15017 0 talk
    matthew 15018 0 talk
    matthew 5547 5538 anim-at  matthew wait-anim
    20 frames wait
    15019 32767 actor 2 say-timed-by
    10 frames wait
    14 music
    matthew 5502 0 anim
    matthew anim-hold
    -716 -1412 -572 32682 -632 426674 6064 camera-set
    35 frames wait
    -506 -2336 -230 33078 -716 426788 7336 camera-set
    15020 matthew 1 say-timed-by
    matthew 600 0 turn-to
    matthew wait-turned
    15 frames wait
    -842 -1892 -710 31992 -224 426764 6448 camera-set
    matthew 760 0 turn-to
    20 frames wait
    138 sound
    5 frames wait
    james 34091 0 428424 set-pos
    james 0 -1400 0 set-rot
    matthew wait-anim
    matthew 114 1 anim
    -362 -1910 -908 32064 -380 427154 4552 camera-set
    matthew 15021 0 talk
    this-one 32611 427444 walk-to
    this-one wait-stop
    matthew 15022 0 talk
    15023 2 caption-for
    text-close
    this-one 5582 5583 anim-at  this-one wait-anim
    15024 matthew 2 say-timed-by-wait
    text-close
    138 sound
    14 screen-effect
    wait-effect
    257 screen-effect
    wait-effect
    50 frames wait
    15025 2 caption-for
    text-close
    15026 2 caption-for
    text-close
    15027 2 caption-for
    text-close
    1132 -104 -3062 31530 -212 426770 4864 camera-set
    31530 976 426770 10 camera-move
    2 screen-effect
    wait-effect
    259 screen-effect
    wait-effect
    30 frames wait
    15028 2 caption-for
    text-close
    15 frames wait
    5 screen-effect
    wait-effect
    259 screen-effect
    wait-effect
    15029 2 caption-for
    text-close
    20 frames wait
    3 screen-effect
    wait-effect
    261 screen-effect
    wait-effect
    15 frames wait
    15030 3 caption-for
    text-close
    5 screen-effect
    wait-effect
    20 frames wait
    4 screen-effect
    wait-effect
    259 screen-effect
    wait-effect
    15032 2 caption-for
    text-close
    15033 2 caption-for
    text-close
    20 frames wait
    end-script
    ;

\ $800108F4
: script-3
    26 actor 0 0 0 set-pos
    101 actor as-this-one
    131 actor 107334 0 350493 set-pos
    131 actor 0 2170 0 set-rot
    matthew 104594 0 356493 set-pos
    matthew 0 5740 0 set-rot
    matthew 0 1 anim
    153 actor 189 actor-model
    153 actor 106274 0 351353 set-pos
    153 actor 0 1260 0 set-rot
    153 actor 27 1 set-floor
    153 actor 0 1 anim
    this-one 107594 0 349053 set-pos
    this-one 27 1 set-floor
    this-one 0 5740 0 set-rot
    this-one 0 1 anim
    -600 -2850 -700 105350 300 355239 5630 camera-set
    20 frames wait
    15034 5 say-mode-wait
    -768 -2598 -574 106682 288 350445 5486 camera-set
    153 actor 106754 350573 walk-to
    153 actor wait-stop
    153 actor 88 0 anim
    153 actor wait-anim
    10 frames wait
    -768 -1194 -862 107294 252 349617 5582 camera-set
    this-one 87 0 anim
    15 music
    15035 5 say-mode-wait
    153 actor 0 0 0 set-pos
    10 frames wait
    -696 -2058 -382 107222 252 350301 5486 camera-set
    10 frames wait
    this-one 107974 350293 walk-to
    this-one wait-stop
    this-one -980 200 turn-to
    this-one wait-turned
    this-one 5701 0 anim
    116 sound
    40 frames wait
    117 sound
    this-one wait-anim
    this-one 0 0 0 set-pos
    10 frames wait
    -600 -2850 -700 105350 300 355239 5630 camera-set
    10 frames wait
    131 actor 107274 341313 -1 240 walk-to-ex
    5 frames wait
    matthew 104774 355743 5 60 walk-to-ex
    matthew wait-stop
    15036 5 say-mode-wait
    10 frames wait
    131 actor wait-stop
    131 actor 0 0 0 set-pos
    26 actor 108212 0 358605 set-pos
    26 actor 0 2048 0 set-rot
    end-script
    ;

\ $80010C50
: script-4
    morgan as-this-one
    this-one 0 0 face
    matthew 1 0 face
    this-one 15037 0 talk
    matthew 15038 7 talk
    this-one 15039 0 talk
    matthew 15040 5 talk
    matthew 15041 3 talk
    matthew 15042 7 talk
    this-one 15039 0 talk
    matthew 15044 5 talk
    this-one 15045 0 talk
    matthew 15046 7 talk
    this-one 15047 5 talk
    matthew 15048 7 talk
    15 frames wait
    this-one 15049 3 talk
    this-one 15050 7 talk
    matthew 15051 1 talk
    this-one 15052 4 talk
    this-one 15053 5 talk
    matthew 15054 0 talk
    this-one 15055 6 talk
    this-one 15056 5 talk
    this-one 15057 7 talk
    matthew 87 0 anim
    matthew wait-anim
    this-one 135 60 1 anim-then
    this-one 15058 0 talk
    matthew 59 60 anim-at  matthew wait-anim
    this-one 61 0 anim
    this-one wait-anim
    matthew 15059 0 talk
    this-one 15060 7 talk
    matthew 15061 0 talk
    this-one 15062 6 talk
    matthew 15063 0 talk
    matthew 61 0 anim
    matthew wait-anim
    matthew 134 137 0 anim-then
    matthew wait-anim
    14 picture-show
    15064 3 say-mode-wait
    picture-hide
    this-one place-at-stop
    end-script
    ;

\ its scripts by number
' script-0 0 252 pack-script!
' script-1 1 252 pack-script!
' script-2 2 252 pack-script!
' script-3 3 252 pack-script!
' script-4 4 252 pack-script!
