\ pack-051-2.fs - event pack 051_2 (CD.BIN 257): 3 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-051-2
USING: events ;

\ $80010014
: script-0
    2 screen-effect
    153 actor as-this-one
    matthew 29 2 set-floor
    153 actor 5 actor-model
    this-one 100740 -1200 355487 set-pos
    this-one 4420 0 turn-to
    this-one 29 2 set-floor
    -470 -2492 -560 101476 -1200 355704 3582 camera-set
    matthew 15000 7 talk
    this-one 15001 6 talk
    matthew 15002 1 talk
    this-one 15003 3 talk
    -470 -2492 -1226 101618 -1200 356075 -1204 camera-set
    matthew 15004 1 talk
    this-one 15005 7 talk
    20 frames wait
    this-one 100530 354486 walk-to
    this-one wait-stop
    4 frames wait
    2 1 elevator-doors
    this-one 100502 353894 walk-to
    this-one wait-stop
    27 frames wait
    this-one 0 0 0 set-pos
    end-script
    ;

\ $8001012C
: script-1
    cohen as-this-one
    matthew 1376 100 turn-to
    matthew wait-turned
    this-one 738 739 1 anim-then
    matthew 15006 6 talk
    matthew 103207 351542 walk-to
    this-one 15007 0 talk
    matthew 15008 7 talk
    this-one 15009 0 talk
    matthew 15010 7 talk
    this-one 15011 0 talk
    matthew 15012 1 talk
    this-one 15013 0 talk
    this-one 15014 0 talk
    matthew 15015 6 talk
    this-one 15016 0 talk
    matthew 15017 7 talk
    this-one 15018 0 talk
    matthew 15019 0 talk
    10 frames wait
    this-one 15020 0 talk
    matthew 15021 6 talk
    this-one 15022 0 talk
    10 frames wait
    this-one 15023 0 talk
    matthew 103437 351429 walk-to
    matthew wait-stop
    matthew 42 0 anim
    matthew wait-anim
    matthew 15024 0 talk
    this-one 15025 0 talk
    19 picture-show
    15026 3 say-mode-wait
    picture-hide
    end-script
    ;

\ $800101FC
: script-2
    matthew 103437 351429 walk-to
    matthew wait-stop
    -494 -2294 -608 103437 -1200 351429 2641 camera-set
    matthew 43 0 1 anim-then
    15027 matthew say-by-wait
    15028 cohen say-by-wait
    matthew 15029 1 talk
    15030 cohen say-by-wait
    matthew 15031 1 talk
    15032 cohen say-by-wait
    matthew 15033 1 talk
    15034 cohen say-by-wait
    matthew 15035 1 talk
    15036 cohen say-by-wait
    15037 cohen say-by-wait
    matthew 15038 1 talk
    15039 cohen say-by-wait
    matthew 15040 1 talk
    15041 cohen say-by-wait
    matthew 15042 1 talk
    15043 cohen say-by-wait
    matthew 15044 1 talk
    15045 cohen say-by-wait
    matthew 15046 1 talk
    15047 cohen say-by-wait
    matthew 15048 1 talk
    15049 cohen say-by-wait
    matthew 15050 1 talk
    15051 cohen say-by-wait
    matthew 15052 1 talk
    15053 cohen say-by-wait
    matthew 15054 1 talk
    15055 cohen say-by-wait
    matthew 15056 1 talk
    15057 cohen say-by-wait
    matthew 15058 1 talk
    15059 cohen say-by-wait
    matthew 15060 1 talk
    15061 cohen say-by-wait
    matthew 15062 1 talk
    end-script
    ;

\ its scripts by number
' script-0 0 257 pack-script!
' script-1 1 257 pack-script!
' script-2 2 257 pack-script!
