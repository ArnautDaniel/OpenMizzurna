\ ovl028.fs - area 028 overlay (CD.BIN 028_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl028
USING: events ;

: L-8010211C
    101 actor as-this-one
    153 actor 1 actor-model
    0 set-scene
    matthew 575309 0 221353 set-pos
    matthew 0 3408 0 set-rot
    matthew 116 1 anim
    this-one 575628 0 221821 set-pos
    169 actor 575628 0 221821 set-pos
    this-one 0 2950 0 set-rot
    this-one 0 1 anim
    mel 574878 0 221051 set-pos
    mel 0 8282 0 set-rot
    mel 0 1 anim
    106 actor 574969 0 221693 set-pos
    106 actor 0 1338 0 set-rot
    106 actor 27 1 anim
    -800 -2618 -350 575866 240 221507 13256 camera-set
    map-sync
    this-one 169 actor ignore-a
    2 screen-effect
    wait-effect
    matthew 118 0 1 anim-then
    106 actor 23 0 talk
    this-one 24 3 talk
    this-one 575278 221821 walk-to
    this-one 25 0 talk
    this-one wait-stop
    this-one 137 0 anim
    this-one wait-stop
    matthew 81 0 anim
    mel 101 0 face
    mel 26 5 talk
    this-one 17 18 anim-at  this-one wait-anim
    this-one 27 0 talk
    matthew 14 15 anim-at  matthew wait-anim
    matthew 28 0 talk
    this-one 0 1 anim
    this-one 0 -200 face
    this-one wait-turned
    14 music
    this-one 5903 5904 anim-at  this-one wait-anim
    this-one 29 0 talk
    105 sound
    5 frames wait
    mel 30 5 talk
    10 ambient-start
    20 frames wait
    matthew 16 0 1 anim-then
    this-one 5906 5907 1 anim-then
    mel 81 0 anim
    mel 31 0 talk
    matthew 32 0 talk
    this-one 33 0 talk
    this-one 5908 5905 0 anim-then
    this-one 34 0 talk
    this-one 575228 221951 walk-to
    mel 35 5 talk
    this-one 17 18 1 anim-then
    this-one 36 0 talk
    101 actor 106 0 through-door
    153 actor 574229 0 220363 set-pos
    153 actor 0 0 0 set-rot
    mel 575098 221471 walk-to
    mel wait-stop
    matthew 91 92 1 anim-then
    mel 0 0 face
    mel 94 95 1 anim-then
    10 ambient-stop
    37 matthew say-by-wait
    mel 38 0 talk
    -674 -3044 -644 574750 240 221285 12632 camera-set
    575038 240 221987 20 camera-move
    108 sound
    153 actor 146 0 1 anim-then
    153 actor 574229 0 220613 -1 50 walk-path
    153 actor wait-stop
    153 actor 0 0 face
    153 actor 39 5 talk
    153 actor 574989 221133 walk-to
    265 screen-effect
    wait-effect
    this-one 4294967295 actor ignore-a
    40 32767 actor say-by-wait
    27 -1 566286 212145 5402 -2146387752 warp
    ;

\ $80102600
: entry-80102600
    1 set-scene
    101 actor as-this-one
    matthew 575469 0 222363 set-pos
    matthew 0 2048 0 set-rot
    matthew 0 1 anim
    this-one 574348 0 221641 set-pos
    this-one 0 2048 0 set-rot
    this-one 68 1 anim
    mel 573958 0 220961 set-pos
    mel 0 1024 0 set-rot
    mel 0 1 anim
    -614 -932 -1022 575491 12 222347 2342 camera-set
    10 frames wait
    matthew 94 95 anim-at  matthew wait-anim
    3 32767 actor say-by-wait
    10 frames wait
    3 screen-effect
    wait-effect
    0 set-scene
    -566 -1982 -1088 574918 0 221477 6920 camera-set
    2 screen-effect
    wait-effect
    mel 4 3 talk
    this-one 5 0 talk
    mel 101 -200 face
    mel 6 5 talk
    101 actor 69 72 anim-at  101 actor wait-anim
    this-one 7 0 talk
    this-one 79 72 1 anim-then
    this-one 8 0 talk
    mel 9 6 talk
    this-one 10 0 talk
    3 screen-effect
    wait-effect
    1 set-scene
    matthew 0 1 anim
    -350 -1160 -296 574791 -12 222657 968 camera-set
    2 screen-effect
    wait-effect
    matthew 17 0 anim
    12 matthew 1 say-timed-by-wait
    matthew 598 -200 turn-to
    106 actor 105 1 through-door
    106 actor 0 -200 face
    106 actor wait-turned
    106 actor 11 5 talk
    106 actor 575119 222343 walk-to
    5 frames wait
    matthew 575119 222343 2 20 walk-to-ex
    5 frames wait
    259 screen-effect
    wait-effect
    106 actor wait-stop
    0 set-scene
    matthew 575529 0 221603 set-pos
    matthew 0 2668 0 set-rot
    matthew 0 1 anim
    this-one 575268 0 221121 set-pos
    this-one 0 7858 0 set-rot
    this-one 18 1 anim
    mel 574878 0 221111 set-pos
    mel 0 4750 0 set-rot
    mel 0 1 anim
    106 actor 574819 0 221563 set-pos
    106 actor 0 1048 0 set-rot
    106 actor 0 1 anim
    -1 music
    20 frames wait
    -146 -3380 -2486 574864 12 221051 10928 camera-set
    this-one 13 0 talk
    10 frames wait
    2 screen-effect
    wait-effect
    this-one 19 0 anim
    this-one wait-anim
    mel 14 5 talk
    this-one 17 18 anim-at  this-one wait-anim
    this-one 15 0 talk
    mel 14 15 anim-at  mel wait-anim
    mel 16 0 talk
    this-one 19 0 anim
    this-one wait-anim
    mel 0 1 anim
    mel 106 -200 face
    this-one 14 15 anim-at  this-one wait-anim
    106 actor 101 200 face
    106 actor wait-turned
    106 actor 17 5 talk
    mel 5900 5901 1 anim-then
    mel 18 0 talk
    106 actor 19 1 talk
    mel 20 0 talk
    mel 5902 0 1 anim-then
    106 actor 14 15 anim-at  106 actor wait-anim
    106 actor 21 0 talk
    106 actor 16 0 anim-at  106 actor wait-anim
    106 actor 0 -200 face
    106 actor 22 5 talk
    3 screen-effect
    wait-effect
    matthew 575529 0 221563 set-pos
    106 actor 574819 0 221563 set-pos
    matthew 0 106 actor 3 fight
    ' L-8010211C start-event
    ovl028 $80102064 native
    end-script
    ;

\ $80102BFC
: entry-80102BFC
    0 32767 actor say-by-wait
    end-script
    ;

\ $80102C0C
: entry-80102C0C
    matthew 576323 0 222842 set-pos
    matthew 0 1024 0 set-rot
    -464 -1880 -1472 576373 12 222814 1316 camera-set
    matthew 65 0 anim
    matthew wait-anim
    1 32767 actor say-by-wait
    end-script
    ;

\ $80102C78
: entry-80102C78
    matthew 576323 0 222842 set-pos
    matthew 0 1024 0 set-rot
    -464 -1880 -1472 576373 12 222814 1316 camera-set
    matthew 65 0 anim
    matthew wait-anim
    2 32767 actor say-by-wait
    end-script
    ;
