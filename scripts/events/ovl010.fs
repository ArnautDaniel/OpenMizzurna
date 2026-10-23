\ ovl010.fs - area 010 overlay (CD.BIN 010_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl010
USING: events events.main ;

\ $80102680
: entry-80102680
    matthew 6144 0 turn-to
    matthew 6 7 1 anim-then
    10 frames wait
    3 screen-effect
    wait-effect
    103 actor 465805 0 461889 set-pos
    103 actor -3094 0 turn-to
    27 -1 337119 443028 -498 -2146388332 warp
    ;

\ $801026E8
: entry-801026E8
    -1286 -3206 -1868 472900 1 467143 -496 camera-set
    61 say-wait-close
    end-script
    ;

\ $80102754
: entry-80102754
    3 screen-effect
    wait-effect
    isabella 184 actor-model
    1 set-scene
    1 frames wait
    camera-fixed
    isabella as-this-one
    -1024 -1042 -1388 472853 0 467541 3108 camera-set
    isabella 471290 0 467906 set-pos
    isabella 161 1 anim
    isabella 2048 0 turn-to
    100 actor 471290 0 467426 set-pos
    100 actor 0 0 turn-to
    map-sync
    8 screen-effect
    wait-effect
    100 actor 4 7 talk
    isabella 163 0 anim
    this-one wait-anim
    this-one 5 7 talk
    100 actor 6 5 talk
    this-one 7 1 talk
    100 actor 8 3 talk
    this-one 9 7 talk
    100 actor 10 7 talk
    this-one 11 7 talk
    100 actor 12 7 talk
    this-one 13 1 talk
    100 actor 14 7 talk
    this-one 15 0 talk
    100 actor 16 5 talk
    this-one 17 7 talk
    100 actor 18 7 talk
    100 actor 17 18 1 anim-then
    12 frames wait
    100 actor 19 0 talk
    100 actor 20 5 talk
    this-one 21 7 talk
    100 actor 22 0 talk
    100 actor 470730 466994 walk-to
    100 actor wait-stop
    100 actor 44 1 through-door
    8 frames wait
    matthew 23 0 talk
    isabella 17 18 anim-then-loop
    15 frames wait
    -512 -2738 -716 471882 0 467690 1024 camera-set
    isabella 0 1 anim
    matthew isabella face-each-other
    this-one 24 7 talk
    matthew 25 7 talk
    this-one 26 1 talk
    matthew 27 7 talk
    this-one 28 0 talk
    11 screen-effect
    wait-effect
    -544 -2066 -1004 471050 0 467416 -1040 camera-set
    isabella 2366 0 turn-to
    ovl010 $801024AC native
    matthew 0 set-y
    10 screen-effect
    wait-effect
    matthew 44 0 through-door
    isabella 471084 467564 walk-to
    matthew 470905 467412 walk-to
    matthew wait-stop
    this-one wait-stop
    matthew isabella face-each-other
    matthew 29 7 talk
    this-one 30 7 talk
    matthew 31 0 talk
    this-one 32 1 talk
    matthew 33 7 talk
    this-one 34 7 talk
    matthew 35 7 talk
    this-one 36 7 talk
    matthew 37 5 talk
    this-one 38 4 talk
    isabella 1 ride
    matthew 470730 466994 walk-to
    matthew wait-stop
    44 door-animate
    events.main:sub-80096CF0
    0 set-scene
    1 frames wait
    camera-fixed
    100 actor as-this-one
    matthew 470490 0 466534 set-pos
    isabella 470711 0 466566 set-pos
    100 actor 468987 0 466086 set-pos
    matthew 100 actor face-each-other
    isabella 100 0 face
    -512 -3506 -716 470765 0 466171 0 camera-set
    30 frames wait
    17 music
    this-one 39 0 talk
    -512 -128 camera-angle
    8 frames wait
    matthew 40 0 talk
    this-one 41 1 talk
    isabella 42 7 talk
    this-one 43 7 talk
    isabella 44 7 talk
    matthew 117 116 1 anim-then
    8 frames wait
    this-one 45 1 talk
    isabella 46 7 talk
    this-one 47 7 talk
    11 screen-effect
    wait-effect
    matthew 470545 0 466321 set-pos
    100 actor 469325 0 466321 set-pos
    matthew 100 actor face-each-other
    isabella unride
    isabella 2048 0 turn-to
    isabella 130 anim-once
    100 actor 5 matthew 0 fight
    100 actor 2306 1 anim
    matthew 468544 0 466089 set-pos
    isabella 468636 0 466709 set-pos
    100 actor 468006 0 466710 set-pos
    matthew 100 actor face-each-other
    isabella 100 0 face
    isabella 1 ride
    -352 -2738 -1420 468485 0 466459 -123 camera-set
    13 music
    10 screen-effect
    wait-effect
    this-one 48 0 talk
    matthew 49 7 talk
    end-script
    ;

\ $80102D70
: entry-80102D70
    isabella as-this-one
    isabella 10 1 set-floor
    isabella 0 0 anim
    100 actor 2306 1 anim
    100 actor 468006 0 466710 set-pos
    isabella unride
    matthew 465730 0 462458 set-pos
    isabella 465730 0 463058 set-pos
    matthew isabella face-each-other
    100 actor 89 0 face
    -480 -1170 -428 465487 0 462458 416 camera-set
    this-one 50 7 talk
    matthew 51 7 talk
    this-one 52 1 talk
    this-one 53 7 talk
    matthew 54 1 talk
    -1 music
    -576 -690 -620 465711 0 462714 0 camera-set
    465711 0 463194 32 camera-move
    this-one 55 7 talk
    wait-camera-move
    109 sound
    255 3 rumble
    256 3 rumble
    3 frames wait
    14 music
    100 actor 2309 1 anim
    isabella 2300 2301 1 anim-then
    20 frames wait
    -320 -690 -428 467291 0 464399 160 camera-set
    15 frames wait
    matthew 465730 0 462822 set-pos
    -192 -690 -620 465711 0 463194 0 camera-set
    8 frames wait
    -520 -1874 -1356 465951 0 463702 2528 camera-set
    8 frames wait
    -968 -2162 -76 465785 0 461631 2016 camera-set
    8 frames wait
    -32 -2450 -236 466453 0 463665 576 camera-set
    7 music
    matthew 150 151 1 anim-then
    matthew 56 0 talk
    100 actor 2310 2308 1 anim-then
    100 actor 57 0 talk
    matthew 152 151 1 anim-then
    matthew 58 0 talk
    isabella 2302 2301 1 anim-then
    this-one 59 0 talk
    matthew 152 151 1 anim-then
    matthew 60 0 talk
    11 screen-effect
    wait-effect
    100 actor 0 0 0 set-pos
    9 2 334051 436969 2176 -2146417400 warp
    ;
