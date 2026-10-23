\ ovl009.fs - area 009 overlay (CD.BIN 009_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl009
USING: events events.main ;

\ $80103C20
: entry-80103C20
    matthew 494 0 turn-to
    matthew 6 7 1 anim-then
    10 frames wait
    3 screen-effect
    wait-effect
    103 actor 337119 0 443028 set-pos
    103 actor -498 0 turn-to
    27 -1 337119 443028 -498 -2146388332 warp
    ;

\ $80103C88
: entry-80103C88
    winona 2 1 talk
    end-script
    ;

\ $80103C9C
: entry-80103C9C
    0 say-wait-close
    end-script
    ;

\ $80103CA8
: entry-80103CA8
    winona as-this-one
    2 set-scene
    matthew 334164 0 440437 set-pos
    matthew 0 1 anim
    matthew 0 510 0 set-rot
    this-one 335664 0 441737 set-pos
    this-one 0 1 anim
    this-one 0 1120 0 set-rot
    -284 -4970 -950 335130 12 440601 10384 camera-set
    matthew 3 6 talk
    this-one 14 15 anim-at  this-one wait-anim
    matthew 335324 441357 walk-to
    40 frames wait
    -266 -2702 -200 335550 -60 441177 11056 camera-set
    matthew 4 5 talk
    this-one 16 0 anim-at  this-one wait-anim
    this-one 5 0 talk
    matthew 6 0 talk
    15 frames wait
    this-one 7 0 talk
    matthew 8 0 talk
    10 frames wait
    this-one 9 3 talk
    matthew 10 7 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 11 0 talk
    matthew 12 5 talk
    this-one 0 1 anim
    this-one 0 200 face
    this-one wait-turned
    this-one 13 4 talk
    this-one 335484 441567 walk-to
    this-one wait-stop
    this-one 59 60 anim-at  this-one wait-anim
    matthew 59 61 0 anim-then
    15 frames wait
    this-one 61 0 anim
    this-one wait-anim
    matthew 133 51 anim-at  matthew wait-anim
    197 198 cd-pictures
    15 frames wait
    matthew 14 0 talk
    10 frames wait
    this-one 15 4 talk
    0 cd-picture
    10 frames wait
    -536 -2378 -1160 335766 132 440847 12208 camera-set
    10 frames wait
    this-one 336004 441817 walk-to
    matthew 16 0 talk
    10 frames wait
    this-one 17 0 talk
    10 frames wait
    matthew 18 0 talk
    10 frames wait
    matthew 141 0 anim
    matthew wait-anim
    matthew 19 6 talk
    10 frames wait
    this-one 20 4 talk
    10 frames wait
    this-one 14 15 1 anim-then
    matthew 134 137 0 anim-then
    matthew wait-anim
    20 picture-show
    24 3 say-mode-wait
    picture-hide
    this-one 0 1 anim
    this-one 0 200 face
    this-one 21 0 talk
    10 frames wait
    this-one 22 6 talk
    10 frames wait
    matthew 23 4 talk
    end-script
    ;

\ $8010405C
: entry-8010405C
    97 actor as-this-one
    matthew 25 7 talk
    this-one 26 0 talk
    matthew 27 5 talk
    this-one 28 0 talk
    matthew 29 3 talk
    this-one 30 0 talk
    this-one 31 0 talk
    matthew 32 5 talk
    this-one 33 0 talk
    end-script
    ;

\ $801040D4
: entry-801040D4
    97 actor as-this-one
    this-one 34 0 talk
    this-one 35 0 talk
    matthew 36 1 talk
    this-one 37 0 talk
    matthew 38 5 talk
    this-one 39 0 talk
    matthew 40 3 talk
    this-one 41 0 talk
    matthew 42 7 talk
    this-one 43 0 talk
    matthew 44 1 talk
    this-one 45 0 talk
    end-script
    ;

\ $80104170
: entry-80104170
    -590 -3566 -572 332693 132 436837 6142 camera-set
    40 door-animate
    events.main:sub-80096CF0
    27 -1 333362 436968 1536 0 warp
    ;

\ $801041C0
: entry-801041C0
    -728 -2054 -932 333313 216 439567 -3334 camera-set
    42 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $801041FC
: entry-801041FC
    -116 -6374 -1802 333300 3 439157 2766 camera-set
    41 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80104238
: entry-80104238
    -590 -3566 -572 332693 132 436837 6142 camera-set
    43 door-animate
    events.main:sub-80096CF0
    1 set-scene
    matthew 332305 0 437074 set-pos
    end-script
    ;

\ $80104284
: entry-80104284
    -638 -236 -2264 332216 0 436947 509 camera-set
    43 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $80104358
: entry-80104358
    the-prop @ as-this-one
    this-one 332555 0 437885 set-pos
    97 actor this-one ignore-a
    97 actor 1024 0 turn-to
    97 actor 2125 0 anim
    97 actor wait-anim
    97 actor 4294967295 actor ignore-a
    ovl009 $80103934 native
    97 actor 2126 0 anim
    97 actor wait-anim
    83 97 actor say-by
    wait-text
    end-script
    ;

\ $801043E8
: entry-801043E8
    the-prop @ as-this-one
    97 actor 1024 0 turn-to
    97 actor this-one ignore-a
    97 actor 2126 0 anim
    97 actor wait-anim
    97 actor 4294967295 actor ignore-a
    this-one 0 0 0 set-pos
    end-script
    ;

\ $80104448
: entry-80104448
    the-prop @ as-this-one
    matthew 2128 0 anim
    matthew wait-anim
    matthew this-one ignore-a
    matthew 2108 0 anim
    matthew wait-anim
    matthew 2108 0 anim
    matthew wait-anim
    matthew 4294967295 actor ignore-a
    ovl009 $80103934 native
    matthew 2109 0 anim
    matthew wait-anim
    matthew 119 1 anim
    end-script
    ;

\ $80104508
: entry-80104508
    153 actor 1 actor-model
    102 actor 10 actor-model
    177 actor 58 actor-model
    153 actor as-this-one
    matthew 0 0 anim
    177 actor 334909 0 436120 set-pos
    177 actor 9 1 set-floor
    matthew 334051 0 436969 set-pos
    matthew 2176 0 turn-to
    153 actor 333608 0 437040 set-pos
    153 actor 2030 0 turn-to
    102 actor 333223 0 436991 set-pos
    102 actor 1576 0 turn-to
    26 actor 0 0 0 set-pos
    177 actor 332800 433498 0 50 walk-to-ex
    -448 -1682 -620 334846 0 433537 3872 camera-set
    map-sync
    1 2 sequence-play
    8 screen-effect
    wait-effect
    177 actor wait-stop
    this-one 46 2 talk
    matthew 47 7 talk
    this-one 48 7 talk
    0 0 sequence-play
    20 frames wait
    153 actor 334047 437545 walk-to
    20 frames wait
    153 actor idle
    153 actor 104 anim-once
    5 frames wait
    0 sequence-stop
    15 frames wait
    this-one 49 0 talk
    1 sequence-stop
    102 actor as-this-one
    matthew 333556 436987 walk-to
    10 frames wait
    102 actor 0 -256 face
    this-one wait-turned
    matthew 50 0 talk
    this-one 51 7 talk
    matthew 52 1 talk
    this-one 53 7 talk
    matthew 54 2 talk
    this-one 55 7 talk
    matthew 56 4 talk
    153 actor as-this-one
    153 actor 103 0 anim
    this-one wait-anim
    153 actor 2548 256 turn-to
    this-one wait-turned
    matthew 153 256 face
    102 actor 153 -256 face
    matthew wait-turned
    matthew 57 7 talk
    -672 -1362 -1260 333679 0 436374 4064 camera-set
    15 music
    this-one 58 2 talk
    matthew 59 1 talk
    this-one 60 7 talk
    13 music
    matthew 61 0 talk
    this-one 62 7 talk
    matthew 63 7 talk
    this-one 64 7 talk
    11 screen-effect
    wait-effect
    26 actor 334778 0 435796 set-pos
    153 actor 0 0 0 set-pos
    102 actor 0 0 0 set-pos
    177 actor 0 0 0 set-pos
    ovl009 $80102344 native
    27 -1 -1 -1 -1 0 warp
    ;

\ the words the main executable's scripts go to here, by address
' entry-80104508 $80104508 9 overlay-word!
