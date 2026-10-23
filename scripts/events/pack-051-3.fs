\ pack-051-3.fs - event pack 051_3 (CD.BIN 258): 2 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-051-3
USING: events ;

\ $80010010
: script-0
    barbara as-this-one
    barbara 15000 7 talk
    matthew 15001 0 talk
    -350 -2462 -2960 105137 0 359246 9208 camera-set
    matthew 105386 359204 walk-to
    matthew wait-stop
    matthew 90 -200 face
    matthew wait-turned
    barbara 15002 7 talk
    matthew 15003 7 talk
    barbara 15004 7 talk
    -380 -2492 -788 106134 0 359624 980 camera-set
    matthew 15005 1 talk
    barbara 15006 7 talk
    matthew 15007 6 talk
    barbara 15008 7 talk
    matthew 15009 7 talk
    barbara 15010 7 talk
    matthew 15011 7 talk
    barbara 15012 7 talk
    matthew 15013 5 talk
    barbara 15014 7 talk
    15015 5 say-mode-wait
    matthew 180 200 turn-by
    barbara 15016 7 talk
    matthew wait-turned
    15017 5 say-mode-wait
    barbara 15018 7 talk
    matthew 15019 0 talk
    barbara 15020 7 talk
    matthew 90 200 face
    matthew wait-turned
    matthew 15021 1 talk
    barbara 15022 7 talk
    matthew 15023 7 talk
    barbara 15024 7 talk
    matthew 15025 2 talk
    barbara 15026 7 talk
    matthew 15027 7 talk
    barbara 15028 7 talk
    matthew 15029 3 talk
    matthew 3036 0 turn-by
    end-script
    ;

\ $80010294
: script-1
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    15030 barbara say-by-wait
    15031 matthew say-by-wait
    15032 barbara say-by-wait
    15033 matthew say-by-wait
    15034 barbara say-by-wait
    matthew 25 0 through-door
    11 screen-effect
    wait-effect
    5 set-scene
    barbara 2 2 set-floor
    lidds 2 2 set-floor
    barbara 98786 -1200 350590 set-pos
    barbara 0 1444 0 set-rot
    lidds 98643 -1200 349787 set-pos
    lidds 0 1033 0 set-rot
    lidds 72 1 anim
    lidds as-this-one
    -476 -2438 -998 98935 -1200 350310 4839 camera-set
    25 frames wait
    10 screen-effect
    wait-effect
    matthew 99208 350720 walk-to
    matthew wait-stop
    matthew 90 -200 face
    matthew wait-turned
    matthew 15035 5 talk
    barbara 15036 7 talk
    this-one 71 0 anim
    this-one wait-anim
    this-one 90 -200 face
    this-one wait-turned
    lidds 15037 5 talk
    matthew 15038 6 talk
    barbara 15039 7 talk
    lidds 15040 3 talk
    this-one 98721 350262 walk-to
    barbara 15041 7 talk
    this-one wait-stop
    lidds 15042 4 talk
    this-one 100966 351027 walk-to
    barbara 15043 7 talk
    this-one wait-stop
    this-one 0 -200 face
    this-one wait-turned
    lidds 15044 7 talk
    this-one 25 5 through-door
    lidds place-at-stop
    -476 -1514 -572 98965 -1200 350558 3945 camera-set
    matthew 15045 7 talk
    barbara 15046 7 talk
    matthew 15047 7 talk
    barbara 15048 7 talk
    matthew 15049 7 talk
    barbara 15050 7 talk
    matthew 15051 7 talk
    barbara 15052 7 talk
    matthew 15053 7 talk
    barbara 15054 0 talk
    matthew 15055 1 talk
    barbara 15056 0 talk
    matthew 15057 7 talk
    barbara 15058 0 talk
    matthew 15059 7 talk
    barbara 15060 7 talk
    15061 5 say-mode-wait
    barbara 15062 7 talk
    matthew 15063 7 talk
    5 frames wait
    11 screen-effect
    wait-effect
    0 set-scene
    -470 -2492 -560 101230 -1200 351210 1024 camera-set
    matthew 101230 -1200 351210 set-pos
    matthew 0 1024 0 set-rot
    25 frames wait
    10 screen-effect
    wait-effect
    end-script
    ;

\ its scripts by number
' script-0 0 258 pack-script!
' script-1 1 258 pack-script!
