\ pack-049-3.fs - event pack 049_3 (CD.BIN 248): 8 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-049-3
USING: events ;

\ $80010028
: script-0
    winona unride
    winona 411283 -4000 168749 set-pos
    matthew 410793 -4000 168339 set-pos
    matthew 0 1 anim
    matthew 15 0 face
    winona 0 1 anim
    winona 0 0 face
    robert 412123 -4000 169789 set-pos
    robert 0 1 anim
    robert 0 3072 0 set-rot
    -1060 -1114 -1060 411381 -3352 168313 -697 camera-set
    winona 15000 7 talk
    matthew 15001 6 talk
    winona 15002 4 talk
    winona 83 84 1 anim-then
    30 frames wait
    winona 15003 0 talk
    winona 85 0 anim
    winona wait-anim
    matthew 15004 5 talk
    winona 87 0 anim
    winona 15005 7 talk
    matthew 15006 5 talk
    winona 86 0 anim
    winona wait-anim
    15 music
    winona 15007 6 talk
    winona 15008 7 talk
    matthew 15009 7 talk
    winona 15010 2 talk
    15011 winona say-by-wait
    matthew 15012 4 talk
    15013 winona say-by-wait
    winona 85 86 0 anim-then
    wait-text
    matthew 15014 7 talk
    120 30 camera-turn
    409905 -3364 168421 36 camera-move
    30 frames wait
    robert 67 0 through-door
    robert 3 0 anim
    robert 15 -80 face
    winona 4 0 anim
    winona 93 80 face
    matthew 15015 7 talk
    robert 15016 4 talk
    robert 15017 5 talk
    winona 15018 4 talk
    robert 4 0 anim
    robert -90 -80 turn-by
    robert wait-turned
    robert 67 -2 through-door
    robert place-at-stop
    winona 3 0 anim
    winona 0 -90 face
    winona wait-turned
    winona 15019 6 talk
    winona 411823 169789 80 face-point
    winona 411865 169730 walk-to
    winona wait-stop
    winona 1024 200 turn-to
    winona wait-turned
    winona 67 -2 through-door
    winona 1 0 set-stop
    winona place-at-stop
    14 map-reveal
    end-script
    ;

\ $80010388
: script-3
    winona 278909 0 98747 set-pos
    winona -80 set-y
    matthew -80 set-y
    winona 0 0 face
    winona 1 ride
    winona 27 1 set-floor
    winona 0 ending-schedule
    matthew 27 1 set-floor
    2 screen-effect
    wait-effect
    end-script
    ;

\ $800103EC
: script-5
    winona unride
    winona 410943 -4000 168489 set-pos
    matthew 410583 -4000 168039 set-pos
    matthew 0 1 anim
    matthew 15 0 face
    winona 0 1 anim
    winona 0 0 face
    -1060 -2806 -1006 410805 -3328 169033 -169 camera-set
    winona 15020 7 talk
    matthew 15021 7 talk
    matthew 17 18 anim-at  matthew wait-anim
    winona 15022 0 talk
    10 frames wait
    matthew 19 0 anim-at  matthew wait-anim
    winona 15023 6 talk
    matthew 87 0 260 anim-then
    winona 15024 7 talk
    matthew 89 0 260 anim-then
    winona 411823 169729 walk-to
    20 frames wait
    end-script
    ;

\ $80010514
: script-1
    3 frames wait
    text-hold
    15032 137 actor 1 say-timed-by-wait
    text-hold
    end-script
    ;

\ $80010538
: script-2
    father-barton as-this-one
    matthew 468475 0 294415 set-pos
    matthew 0 -1519 0 set-rot
    matthew 0 1 anim
    this-one 467255 0 293315 set-pos
    this-one 0 3150 0 set-rot
    this-one 0 1 anim
    136 actor 467025 0 293925 set-pos
    136 actor 0 -2800 0 set-rot
    136 actor 0 1 anim
    137 actor 466895 0 293295 set-pos
    137 actor 0 910 0 set-rot
    137 actor 0 1 anim
    -360 -3426 -1282 467455 0 293305 2368 camera-set
    30 frames wait
    137 actor 2736 0 anim
    137 actor anim-hold
    this-one 2737 0 anim
    this-one anim-hold
    137 actor 15026 0 talk
    137 actor 15027 1 talk
    this-one 15028 0 talk
    this-one 15029 7 talk
    137 actor 15030 5 talk
    this-one 15031 3 talk
    137 actor 106 0 anim
    ['] script-1 start-event
    7 frames wait
    -1 music
    149 sound
    this-one 467615 0 293295 -1 50 walk-path
    this-one 113 0 anim
    8 frames wait
    this-one 114 1 anim
    137 actor wait-anim
    137 actor 118 0 1 anim-then
    10 frames wait
    this-one 114 1 anim
    15033 matthew 1 say-timed-by-wait
    text-hold
    this-one 115 0 anim
    matthew 467705 293695 walk-to
    15034 matthew 1 say-timed-by-wait
    text-hold
    matthew wait-stop
    137 actor 0 0 face
    -564 -1986 -160 467309 60 293623 -2232 camera-set
    137 actor 15035 5 talk
    137 actor 467225 293445 walk-to
    137 actor wait-stop
    136 actor 15036 1 talk
    137 actor 14 15 1 anim-then
    137 actor 15037 0 talk
    136 actor 86 0 anim
    20 frames wait
    137 actor 466045 293565 walk-to
    matthew 15038 5 talk
    136 actor 466235 294175 walk-to
    matthew 467185 293645 walk-to
    this-one 15039 5 talk
    matthew 15040 0 talk
    this-one 15041 3 talk
    matthew 14 -200 face
    matthew 15042 7 talk
    this-one 0 0 face
    this-one 15043 5 talk
    matthew 15044 3 talk
    this-one 14 15 1 anim-then
    30 frames wait
    this-one 15045 0 talk
    this-one 15046 0 talk
    matthew 15047 0 talk
    this-one 16 0 1 anim-then
    matthew 15048 7 talk
    this-one 15049 4 talk
    this-one 467255 293315 walk-to
    this-one wait-stop
    this-one 0 0 face
    136 actor 0 0 0 set-pos
    137 actor 0 0 0 set-pos
    end-script
    ;

\ $80010964
: script-6
    155 actor as-this-one
    this-one 15050 5 talk
    matthew 15051 7 talk
    this-one 15052 7 talk
    matthew 15053 7 talk
    this-one 15054 5 talk
    matthew 15055 7 talk
    this-one 15056 3 talk
    this-one 15057 0 talk
    matthew 15058 0 talk
    this-one 15059 0 talk
    matthew 15060 5 talk
    this-one 15061 7 talk
    matthew 15062 7 talk
    15063 5 say-mode-wait
    this-one 15064 7 talk
    this-one 15065 3 talk
    this-one 15066 5 talk
    matthew 15067 7 talk
    15068 5 say-mode-wait
    this-one 15069 5 talk
    end-script
    ;

\ $800109D0
: script-7
    rolein as-this-one
    matthew 468484 0 412173 set-pos
    matthew 0 2048 0 set-rot
    matthew 0 1 anim
    this-one 468484 0 411763 set-pos
    mel 471604 0 413613 set-pos
    -504 -3498 -724 468652 60 411885 22236 camera-set
    matthew 65 0 anim
    10 frames wait
    119 sound
    2 frames wait
    119 sound
    matthew wait-anim
    matthew 65 0 anim
    10 frames wait
    119 sound
    2 frames wait
    119 sound
    matthew wait-anim
    matthew 468484 412473 2 20 walk-to-ex
    this-one 47 -2 through-door-back
    this-one 15070 0 talk
    matthew 15071 0 talk
    this-one 15072 3 talk
    matthew 15073 5 talk
    matthew 15074 6 talk
    this-one 15075 0 talk
    matthew 15076 5 talk
    this-one 15077 0 talk
    matthew 15078 5 talk
    matthew 15079 0 talk
    matthew 17 0 anim
    matthew wait-anim
    matthew 1024 -200 turn-to
    21756 -40 camera-angle
    10 frames wait
    matthew 1024 200 turn-to
    mel 471784 411153 5 70 walk-to-ex
    17 music
    matthew 15080 0 talk
    30 frames wait
    matthew 465644 412473 5 70 walk-to-ex
    matthew wait-stop
    3 screen-effect
    wait-effect
    10 music
    21 minigame
    2 screen-effect
    wait-effect
    25 frames wait
    15081 2 say-mode
    40 frames wait
    text-close
    15082 2 say-mode
    40 frames wait
    text-close
    end-script
    ;

\ its scripts by number
' script-0 0 248 pack-script!
' script-1 1 248 pack-script!
' script-2 2 248 pack-script!
' script-3 3 248 pack-script!
' script-3 4 248 pack-script!
' script-5 5 248 pack-script!
' script-6 6 248 pack-script!
' script-7 7 248 pack-script!
