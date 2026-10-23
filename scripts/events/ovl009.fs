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
