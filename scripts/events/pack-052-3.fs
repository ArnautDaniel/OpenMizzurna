\ pack-052-3.fs - event pack 052_3 (CD.BIN 263): 1 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-052-3
USING: events ;

\ $8001000C
: script-0
    james 6 actor-model
    james as-this-one
    matthew 32754 0 426896 set-pos
    matthew 0 -700 0 set-rot
    matthew 0 1 anim
    91 actor 32784 0 427096 set-pos
    91 actor 0 -1980 0 set-rot
    91 actor 109 1 anim
    91 actor -760 set-y
    cohen 32364 0 427446 set-pos
    24 actor 32364 0 427446 set-pos
    cohen 0 1570 0 set-rot
    cohen 0 1 anim
    cohen unride
    this-one 36164 0 427336 set-pos
    this-one 0 3040 0 set-rot
    this-one 5555 1 anim
    154 actor 34091 0 428424 set-pos
    -2012 -338 -2996 32922 -512 427274 6808 camera-set
    map-sync
    cohen 24 actor ignore-a
    5 frames wait
    cohen 81 0 anim
    matthew 48 50 1 anim-then
    15000 matthew say-by-wait
    cohen wait-anim
    cohen 32364 427026 walk-to
    cohen wait-stop
    cohen 1540 0 turn-to
    cohen 150 151 anim-at  cohen wait-anim
    matthew 15001 0 talk
    15 frames wait
    cohen 15003 0 talk
    15 frames wait
    cohen 153 0 anim
    cohen wait-anim
    -338 -2336 -494 33078 -716 427328 6856 camera-set
    cohen 14 15 anim-at  cohen wait-anim
    10 frames wait
    cohen 32324 428276 -200 face-point
    cohen 0 1 anim
    cohen wait-turned
    cohen 32324 428276 walk-to
    20 frames wait
    7408 60 camera-angle
    cohen wait-stop
    cohen 81 0 anim
    cohen wait-anim
    cohen 15004 0 talk
    matthew 153 0 anim-at  matthew wait-anim
    matthew 3 200 face
    matthew 15005 0 talk
    cohen 14 15 anim-at  cohen wait-anim
    cohen 6 220 face
    5 frames wait
    cohen 0 1 anim
    15006 cohen 1 say-timed-by-wait
    text-close
    -1 music
    138 sound
    cohen 113 114 1 anim-then
    matthew 94 0 anim
    cohen 31454 -800 428596 -1 70 walk-path
    5 frames wait
    15007 matthew 1 say-timed-by-wait
    text-close
    matthew 81 0 anim
    15008 32767 actor 2 say-timed-by
    matthew wait-anim
    matthew 14 0 1 anim-then
    15 frames wait
    matthew 730 200 turn-to
    matthew wait-turned
    15009 32767 actor 2 say-timed-by
    20 frames wait
    742 -5006 -908 36210 -788 427184 994 camera-set
    39306 -860 427430 160 camera-move
    10 frames wait
    james 154 actor ignore-a
    text-close
    14 music
    15010 32767 actor 2 say-timed-by
    this-one 5556 0 anim
    this-one wait-anim
    10 frames wait
    -1886 -362 -2354 36030 340 427334 1000 camera-set
    this-one 5594 0 anim
    15 frames wait
    text-close
    16 sound
    12 sound
    15011 32767 actor 1 say-timed-by
    this-one wait-anim
    matthew 32934 0 426696 set-pos
    -776 -5180 -1070 35262 -368 426416 1312 camera-set
    this-one 33964 427316 walk-to
    35262 -368 425942 2 camera-move
    20 frames wait
    15012 matthew 2 say-timed-by
    13 music
    this-one wait-stop
    matthew 15013 5 talk
    this-one 15014 3 talk
    this-one 15015 7 talk
    matthew 15016 1 talk
    this-one 87 0 anim
    this-one wait-anim
    this-one 15017 6 talk
    10 frames wait
    matthew 15018 3 talk
    this-one 15019 6 talk
    matthew 17 19 0 anim-then
    matthew wait-anim
    matthew 15020 5 talk
    wait-camera-move
    -626 -2534 -446 34284 -860 427812 538 camera-set
    15 frames wait
    this-one 15021 7 talk
    this-one 14 15 anim-at  this-one wait-anim
    10 frames wait
    this-one 15022 0 talk
    end-script
    ;

\ its scripts by number
' script-0 0 263 pack-script!
