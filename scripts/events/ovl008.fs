\ ovl008.fs - area 008 overlay (CD.BIN 008_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl008
USING: events events.main ;

\ $80103030
: entry-80103030
    matthew 44 0 anim
    matthew wait-anim
    matthew 46 anim-once
    matthew wait-anim
    2 say-wait-close
    end-script
    ;

\ $80103058
: entry-80103058
    matthew 44 0 anim
    matthew wait-anim
    matthew 46 anim-once
    matthew wait-anim
    3 say-wait-close
    matthew 47 0 anim
    matthew wait-anim
    end-script
    ;

\ $80103090
: entry-80103090
    matthew 44 0 anim
    matthew wait-anim
    matthew 46 anim-once
    matthew wait-anim
    4 say-wait-close
    5 say-wait-close
    187 188 cd-pictures
    6 say-wait-close
    7 say-wait-close
    0 cd-picture
    end-script
    ;

\ $801030E4
: entry-801030E4
    matthew 81 0 anim
    8 say-wait-close
    end-script
    ;

\ $801030FC
: entry-801030FC
    11 screen-effect
    wait-effect
    0 set-scene
    matthew 105042 0 207167 set-pos
    72 time-skip
    26 minigame
    wait-minigame
    27 -1 108147 200581 2558 0 warp
    ;

\ $80103150
: entry-80103150
    11 screen-effect
    wait-effect
    0 set-scene
    matthew 107276 0 209681 set-pos
    matthew 3538 0 turn-to
    10 screen-effect
    wait-effect
    end-script
    ;

\ $801031B0
: entry-801031B0
    28 minigame
    wait-minigame
    27 -1 107919 198884 -3470 0 warp
    ;

\ $801031D8
: entry-801031D8
    -542 -5402 -536 105996 -2 218303 4250 camera-set
    39 door-animate
    events.main:sub-80096CF0
    27 -1 106793 199299 5631 0 warp
    ;

\ $80103228
: entry-80103228
    11 screen-effect
    wait-effect
    matthew 107690 0 209166 set-pos
    matthew -2528 0 turn-to
    1 set-scene
    10 screen-effect
    wait-effect
    end-script
    ;

\ $801032C8
: entry-801032C8
    157 actor as-this-one
    matthew 107357 0 209649 set-pos
    matthew 3564 0 turn-to
    157 actor 107591 0 210276 set-pos
    157 actor 2198 0 turn-to
    157 actor 124 1 anim
    -480 -2238 -1548 107015 0 209857 936 camera-set
    map-sync
    10 screen-effect
    wait-effect
    matthew 14 0 anim
    matthew wait-anim
    matthew 107462 209932 walk-to
    107064 -38 210622 32 camera-move
    1908 30 camera-angle
    matthew wait-stop
    matthew 157 0 face
    matthew 120 121 1 anim-then
    wait-camera-turn
    wait-camera-move
    10 frames wait
    157 actor 106 1900 1 anim-then
    2 frames wait
    255 3 rumble
    256 3 rumble
    2 frames wait
    matthew 129 0 anim
    matthew 9 0 talk
    matthew 10 0 talk
    11 2 say-mode-wait
    11 screen-effect
    wait-effect
    end-script
    ;

\ $801034A0
: entry-801034A0
    153 actor 1 actor-model
    102 actor 7 actor-model
    153 actor as-this-one
    -320 -2002 -876 106418 0 207243 1608 camera-set
    153 actor 106233 0 207085 set-pos
    matthew 106348 0 207299 set-pos
    102 actor 107153 0 208255 set-pos
    matthew 153 actor face-each-other
    this-one 13 1 talk
    matthew 14 7 talk
    this-one 15 7 talk
    matthew 16 7 talk
    this-one 17 7 talk
    matthew 18 4 talk
    this-one 19 2 talk
    this-one 20 2 talk
    matthew 21 7 talk
    this-one 22 2 talk
    102 actor 106693 207615 walk-to
    102 actor wait-stop
    102 actor 153 actor face-each-other
    this-one 23 1 talk
    102 actor 24 7 talk
    this-one 25 3 talk
    this-one 26 7 talk
    102 actor 27 4 talk
    matthew 28 1 talk
    this-one 29 7 talk
    this-one 30 7 talk
    matthew 31 7 talk
    this-one 32 7 talk
    matthew 33 6 talk
    11 screen-effect
    wait-effect
    153 actor 0 0 0 set-pos
    102 actor 0 0 0 set-pos
    end-script
    ;
