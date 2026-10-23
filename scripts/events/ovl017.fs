\ ovl017.fs - area 017 overlay (CD.BIN 017_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl017
USING: events events.main ;

\ $801025D8
: entry-801025D8
    winona as-this-one
    winona unride
    winona 379483 0 151015 set-pos
    winona 378515 147485 1 60 walk-to-ex
    this-one wait-stop
    winona 369492 147485 1 60 walk-to-ex
    this-one wait-stop
    winona 369492 149314 1 60 walk-to-ex
    this-one wait-stop
    winona 10 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 12 0 anim
    winona anim-hold
    this-one wait-anim
    winona 369000 150514 1 60 walk-to-ex
    this-one wait-stop
    winona -90 0 turn-by
    winona 10 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 12 0 anim
    winona anim-hold
    this-one wait-anim
    winona 17 2 set-floor
    winona 369492 149014 1 60 walk-to-ex
    this-one wait-stop
    winona 369492 149360 1 60 walk-to-ex
    this-one wait-stop
    winona 10 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 12 0 anim
    winona anim-hold
    this-one wait-anim
    winona 369000 150514 1 60 walk-to-ex
    this-one wait-stop
    winona -90 0 turn-by
    winona 10 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 11 0 anim
    winona anim-hold
    this-one wait-anim
    winona 12 0 anim
    winona anim-hold
    this-one wait-anim
    winona 17 3 set-floor
    winona 368621 147372 1 60 walk-to-ex
    this-one wait-stop
    winona 364482 147078 1 60 walk-to-ex
    this-one wait-stop
    winona -90 0 turn-by
    end-script
    ;

\ $80102874
: entry-80102874
    winona as-this-one
    -512 -2738 -588 364287 -2400 147058 2352 camera-set
    matthew 364645 -2400 146887 set-pos
    matthew 2352 0 turn-to
    this-one 1 7 talk
    363559 -2400 144832 128 camera-move
    wait-camera-move
    4 object-anim
    20 frames wait
    this-one 2 1 talk
    matthew 3 7 talk
    this-one 4 2 talk
    matthew 5 3 talk
    this-one 6 7 talk
    matthew 7 4 talk
    this-one 8 2 talk
    matthew 9 7 talk
    11 picture-show
    33 3 say-mode-wait
    picture-hide
    5 picture-show
    34 3 say-mode-wait
    picture-hide
    5 object-anim
    20 frames wait
    364287 -2400 147058 128 camera-move
    wait-camera-move
    this-one 10 7 talk
    winona 363331 146782 walk-to
    winona wait-stop
    winona 63 2 through-door
    winona 365790 -2400 144581 set-pos
    winona 2725 0 turn-to
    winona 84 1 anim
    end-script
    ;

\ $80102A00
: entry-80102A00
    -588 -714 -690 365664 -2400 145090 2186 camera-set
    matthew 365392 -2400 144579 set-pos
    matthew 15 0 face
    matthew 11 1 talk
    winona 12 7 talk
    matthew 13 4 talk
    winona 14 2 talk
    matthew 15 7 talk
    winona 16 7 talk
    matthew 19 3 talk
    winona 20 2 talk
    matthew 21 7 talk
    winona 22 6 talk
    matthew 23 2 talk
    winona 24 4 talk
    winona 25 7 talk
    matthew 26 7 talk
    winona 27 7 talk
    matthew 28 3 talk
    winona 1 ride
    end-script
    ;

\ $80102B4C
: entry-80102B4C
    29 say-wait-close
    end-script
    ;

\ $80102B58
: entry-80102B58
    15 minigame
    wait-minigame
    9 screen-effect
    wait-effect
    16 2 -1 -1 -1 -2146419232 warp
    ;

\ $80102BFC
: entry-80102BFC
    62 door-animate-alt
    events.main:sub-80096CF0
    16 3 -1 -1 1024 0 warp
    ;

\ $80102C2C
: entry-80102C2C
    -680 -4106 -152 363198 -2388 146790 6168 camera-set
    63 door-animate
    events.main:sub-80096CF0
    3 set-scene
    end-script
    ;

\ $80102C68
: entry-80102C68
    -494 -3674 -716 363421 -2400 146456 -328 camera-set
    63 door-animate
    events.main:sub-80096CF0
    2 set-scene
    end-script
    ;

\ $80102CA4
: entry-80102CA4
    -596 -2492 -1832 369631 0 149347 4102 camera-set
    matthew 149347 set-z
    matthew 0 0 0 set-rot
    matthew 10 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    11 screen-effect
    matthew 11 0 anim
    9 frames wait
    1 set-scene
    -254 -1478 -2012 369018 -1300 149410 -320 camera-set
    matthew 368935 0 150010 set-pos
    matthew -960 set-y
    matthew 0 2048 0 set-rot
    matthew wait-anim
    10 screen-effect
    matthew 10 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 13 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 17 2 set-floor
    end-script
    ;

\ $80102E0C
: entry-80102E0C
    -524 -2162 -2042 368885 -1200 149499 4094 camera-set
    matthew 149511 set-z
    matthew 0 0 0 set-rot
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    11 screen-effect
    matthew 7 0 anim
    9 frames wait
    0 set-scene
    -512 -2738 -716 369424 0 148991 4396 camera-set
    matthew 369554 0 149916 set-pos
    matthew -320 set-y
    matthew 0 2048 0 set-rot
    matthew wait-anim
    10 screen-effect
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 8 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 17 1 set-floor
    end-script
    ;

\ $80102F74
: entry-80102F74
    -596 -2492 -1832 369631 -1200 149347 4102 camera-set
    matthew 149347 set-z
    matthew 0 0 0 set-rot
    matthew 10 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    11 screen-effect
    matthew 11 0 anim
    9 frames wait
    2 set-scene
    -254 -1478 -2012 369018 -2500 149410 -320 camera-set
    matthew 368935 0 150010 set-pos
    matthew -2160 set-y
    matthew 0 2048 0 set-rot
    matthew wait-anim
    10 screen-effect
    matthew 10 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 11 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 13 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 17 3 set-floor
    end-script
    ;

\ $801030DC
: entry-801030DC
    -524 -2162 -2042 368885 -2400 149499 4094 camera-set
    matthew 149511 set-z
    matthew 0 0 0 set-rot
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    11 screen-effect
    matthew 7 0 anim
    9 frames wait
    1 set-scene
    -512 -2738 -716 369424 -1200 148991 4396 camera-set
    matthew 369554 0 149916 set-pos
    matthew -1520 set-y
    matthew 0 2048 0 set-rot
    matthew wait-anim
    10 screen-effect
    matthew 6 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 7 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 8 0 anim
    matthew anim-hold
    matthew wait-anim
    matthew 17 2 set-floor
    end-script
    ;

: L-801032E4
    this-one 0 0 face
    matthew 96 0 face
    this-one 30 7 talk
    events.main:sub-80096E74
    matthew 31 0 talk
    matthew 32 6 talk
    5 frames wait
    3 screen-effect
    wait-effect
    27 -1 371437 140480 0 -2146388412 warp
    ;

: L-80103358
    this-one 364078 0 149174 set-pos
    this-one 363897 147579 walk-to
    this-one wait-stop
    goto L-801032E4
    ;

\ $80103380
: entry-80103380
    96 actor as-this-one
    this-one 17 1 set-floor
    -512 -2738 -716 377725 0 147568 3072 camera-set
    goto L-80103358
    ;

\ $801033C0
: entry-801033C0
    96 actor as-this-one
    this-one 17 3 set-floor
    -512 -2738 -716 377725 -1200 147568 3072 camera-set
    goto L-80103358
    ;

\ $80103400
: entry-80103400
    96 actor as-this-one
    this-one 17 3 set-floor
    -512 -2738 -716 377725 -2400 147568 3072 camera-set
    goto L-80103358
    ;

: L-80103440
    this-one 378205 0 148699 set-pos
    this-one 378634 147534 walk-to
    this-one wait-stop
    goto L-801032E4
    ;

\ $80103468
: entry-80103468
    96 actor as-this-one
    this-one 17 1 set-floor
    -512 -2738 -716 363019 0 147344 1024 camera-set
    goto L-80103440
    ;

\ $801034A8
: entry-801034A8
    96 actor as-this-one
    this-one 17 2 set-floor
    -512 -2738 -716 363019 -1200 147344 1024 camera-set
    goto L-80103440
    ;

\ $801034E8
: entry-801034E8
    96 actor as-this-one
    this-one 17 3 set-floor
    -512 -2738 -716 363019 -2400 147344 1024 camera-set
    goto L-80103440
    ;

: L-80103528
    this-one 377449 0 148144 set-pos
    this-one 378624 147842 walk-to
    this-one wait-stop
    goto L-801032E4
    ;

\ $80103550
: entry-80103550
    96 actor as-this-one
    this-one 17 1 set-floor
    -512 -2738 -716 378815 0 151679 2048 camera-set
    goto L-80103528
    ;

\ $80103590
: entry-80103590
    96 actor as-this-one
    this-one 17 2 set-floor
    -512 -2738 -716 378815 -1200 151679 2048 camera-set
    goto L-80103528
    ;

\ $801035D0
: entry-801035D0
    96 actor as-this-one
    this-one 17 3 set-floor
    -512 -2738 -716 378815 -2400 151679 2048 camera-set
    goto L-80103528
    ;

: L-80103610
    this-one 364996 0 148107 set-pos
    this-one 364019 147805 walk-to
    this-one wait-stop
    goto L-801032E4
    ;

\ $80103638
: entry-80103638
    96 actor as-this-one
    this-one 17 1 set-floor
    -512 -2738 -716 363559 0 151630 2048 camera-set
    goto L-80103610
    ;

\ $80103678
: entry-80103678
    96 actor as-this-one
    this-one 17 2 set-floor
    -512 -2738 -716 363559 -1200 151630 2048 camera-set
    goto L-80103610
    ;

\ $801036B8
: entry-801036B8
    96 actor as-this-one
    this-one 17 3 set-floor
    -512 -2738 -716 363559 -2400 151630 2048 camera-set
    goto L-80103610
    ;

: L-801036F8
    this-one 369425 0 146791 set-pos
    this-one 370534 147263 walk-to
    this-one wait-stop
    goto L-801032E4
    ;

\ $80103720
: entry-80103720
    96 actor as-this-one
    this-one 17 1 set-floor
    -512 -2738 -716 371263 0 145694 0 camera-set
    goto L-801036F8
    ;

\ $80103760
: entry-80103760
    96 actor as-this-one
    this-one 17 2 set-floor
    -512 -2738 -716 371148 -1200 143556 0 camera-set
    goto L-801036F8
    ;

\ $801037A0
: entry-801037A0
    96 actor as-this-one
    this-one 17 3 set-floor
    -512 -2738 -716 371148 -2400 143556 0 camera-set
    goto L-801036F8
    ;

: L-801037E0
    this-one 364078 0 149174 set-pos
    this-one 363897 147579 walk-to
    this-one wait-stop
    goto L-801032E4
    ;

\ $80103808
: entry-80103808
    96 actor as-this-one
    this-one 17 1 set-floor
    -512 -2738 -716 370025 0 147272 11418 camera-set
    goto L-801037E0
    ;

\ $80103848
: entry-80103848
    96 actor as-this-one
    this-one 17 2 set-floor
    -512 -2738 -716 370025 -1200 147272 11418 camera-set
    goto L-801037E0
    ;

\ $80103888
: entry-80103888
    96 actor as-this-one
    this-one 17 3 set-floor
    -512 -2738 -716 370025 -2400 147272 11418 camera-set
    goto L-801037E0
    ;

\ $801038C8
: entry-801038C8
    96 actor as-this-one
    this-one 17 3 set-floor
    -512 -2738 -716 365366 -2400 143598 -388 camera-set
    this-one 63 2 through-door
    12 picture-show
    25 frames wait
    picture-hide
    goto L-801032E4
    ;
