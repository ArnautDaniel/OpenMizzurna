\ pack-052-4.fs - event pack 052_4 (CD.BIN 264): 1 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-052-4
USING: events ;

\ $8001000C
: script-0
    matthew 15001 5 talk
    10 frames wait
    this-one 15002 3 talk
    0 cd-picture
    10 frames wait
    matthew 15003 1 talk
    this-one 15004 3 talk
    this-one 15005 6 talk
    this-one 15006 3 talk
    10 frames wait
    -548 -3956 -392 34086 -644 427110 466 camera-set
    -3000 3 camera-angle
    matthew 15007 5 talk
    matthew 15008 1 talk
    this-one 15009 3 talk
    matthew 15010 5 talk
    this-one 15011 6 talk
    matthew 15012 5 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 15013 0 talk
    this-one 15014 0 talk
    this-one 16 0 anim-at  this-one wait-anim
    wait-camera-turn
    -830 -932 -716 33978 -596 427578 184 camera-set
    this-one 15015 7 talk
    this-one 15016 3 talk
    this-one 15017 6 talk
    matthew 15018 0 talk
    this-one 15020 0 talk
    matthew 15021 0 talk
    matthew 940 0 turn-to
    this-one 34314 0 427306 set-pos
    this-one 0 0 face
    178 actor 32934 0 426696 set-pos
    -1562 -2348 -1244 34266 304 426572 3400 camera-set
    this-one 5554 5555 anim-at  this-one wait-anim
    -1 music
    this-one 15022 0 talk
    matthew 15023 6 talk
    this-one 15024 0 talk
    10 frames wait
    matthew 15025 0 talk
    wait-camera-move
    -1646 -1760 -1352 32436 232 426542 2800 camera-set
    15 frames wait
    matthew 136 0 anim
    matthew 178 actor ignore-a
    matthew wait-anim
    matthew 51 1 anim
    25 frames wait
    this-one 15026 0 talk
    matthew 15027 0 talk
    10 frames wait
    this-one 15028 0 talk
    matthew 15029 0 talk
    10 frames wait
    this-one 15030 0 talk
    matthew 15031 0 talk
    -1598 -1538 -1652 33258 304 426992 1048 camera-set
    15 frames wait
    this-one 15032 0 talk
    15033 matthew 1 say-timed-by
    10 frames wait
    this-one 2690 -200 turn-to
    this-one wait-turned
    james 5608 5555 1 anim-then
    5 frames wait
    138 sound
    matthew 5600 0 anim
    matthew 4294967295 actor ignore-a
    178 actor 0 0 0 set-pos
    5 frames wait
    15034 matthew 1 say-timed-by
    matthew wait-anim
    matthew 5601 1 anim
    5 frames wait
    matthew 5602 0 anim
    matthew wait-anim
    this-one 15035 0 talk
    matthew 15036 0 talk
    -1658 -1136 -1550 34410 304 427388 616 camera-set
    cohen 5598 1 anim
    15037 james 2 say-timed-by-wait
    text-close
    18 music
    109 sound
    15039 matthew 1 say-timed-by
    this-one 5600 5601 anim-at  this-one wait-anim
    -278 -1676 -200 31339 -740 428682 -810 camera-set
    10 frames wait
    15038 cohen 1 say-timed-by
    cohen 5599 5581 anim-at  cohen wait-anim
    -422 -2972 -266 35688 -884 427338 1036 camera-set
    this-one 5602 0 anim
    this-one wait-anim
    this-one 3 200 face
    this-one 5554 5555 anim-at  this-one wait-anim
    46 -1118 -1448 32545 -788 427200 -1218 camera-set
    91 actor 110 0 anim
    15039 james 1 say-timed-by
    28 frames wait
    -422 -2972 -266 35688 -884 427338 1036 camera-set
    this-one 5555 1 anim
    this-one 91 0 face
    91 actor 6 0 face
    matthew 91 0 face
    91 actor 131 1 anim
    this-one 5560 5561 anim-at  this-one wait-anim
    -422 -1244 -272 32814 -884 427128 -860 camera-set
    135 sound
    -590 40 camera-dist-to
    wait-camera-dist
    -314 -1454 16 32814 -908 427020 -1220 camera-set
    91 actor 32274 427136 2 20 walk-to-ex
    matthew -794 -10 turn-to
    15040 matthew 2 say-timed-by
    91 actor wait-stop
    91 actor 6703 300 turn-to
    91 actor wait-turned
    135 sound
    91 actor 5 70 move-forward
    matthew 88 0 anim
    15040 matthew 2 say-timed-by
    matthew wait-anim
    -404 -2426 -200 32562 -920 427386 -2876 camera-set
    this-one 5556 0 anim
    cohen 31934 0 428596 set-pos
    cohen 114 1 anim
    cohen 3140 0 turn-to
    cohen 4294967295 actor ignore-a
    24 actor 31844 0 428506 set-pos
    24 actor 5320 0 turn-to
    24 actor 51 1 anim
    15041 james 2 say-timed-by-wait
    text-close
    this-one 29424 427236 5 70 walk-to-ex
    matthew 14 15 1 anim-then
    10 frames wait
    15042 james 1 say-timed-by-wait
    text-close
    matthew 32214 427066 5 60 walk-to-ex
    15043 cohen 1 say-timed-by-wait
    text-close
    matthew 14 15 anim-at  matthew wait-anim
    matthew 32034 428206 5 60 walk-to-ex
    15 frames wait
    -806 -554 -1538 31710 -656 428304 -3452 camera-set
    matthew wait-stop
    matthew 150 151 anim-at  matthew wait-anim
    15044 matthew 1 say-timed-by-wait
    text-hold
    15045 cohen 1 say-timed-by-wait
    text-hold
    15046 matthew 1 say-timed-by-wait
    text-hold
    15047 cohen 2 say-timed-by-wait
    text-hold
    15048 cohen 2 say-timed-by-wait
    text-close
    -428 -2288 -104 32466 -800 427810 1708 camera-set
    matthew 24 actor ignore-a
    matthew 49 0 anim-at  matthew wait-anim
    matthew 17 18 anim-at  matthew wait-anim
    91 actor idle
    3 screen-effect
    wait-effect
    end-script
    ;

\ its scripts by number
' script-0 0 264 pack-script!
