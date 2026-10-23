\ ovl004.fs - area 004 overlay (CD.BIN 004_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl004
USING: events events.main ;

: L-80101FC8
    matthew 366831 387582 walk-to
    matthew wait-stop
    matthew -2 0 turn-to
    end-script
    ;

\ $80101FE8
: arrive-80101FE8
    james as-this-one
    -512 -2738 -662 366584 0 386679 -7276 camera-set
    matthew 4 1 set-floor
    this-one 368241 0 386771 set-pos
    this-one -3088 0 turn-to
    this-one 4 1 set-floor
    this-one 32 anim-once
    2 screen-effect
    this-one 0 0 talk
    this-one 34 anim-once
    this-one wait-anim
    0 -2558 -4490 367068 0 387420 456 camera-set
    this-one 35 anim-once
    matthew 1 1 talk
    this-one 44 anim-once
    this-one wait-anim
    -512 -2738 -1292 367827 0 387232 -10256 camera-set
    this-one 46 anim-once
    this-one wait-anim
    this-one 47 anim-once
    this-one wait-anim
    this-one 45 anim-once
    this-one wait-anim
    matthew 367829 387483 walk-to
    this-one 368159 386358 walk-to
    this-one wait-stop
    this-one 48 50 1 anim-then
    matthew wait-stop
    matthew 81 1 anim
    20 frames wait
    this-one 49 0 1 anim-then
    this-one 3 0 talk
    matthew 6 280 face
    matthew 4 1 talk
    this-one 0 0 face
    this-one 56 57 1 anim-then
    this-one 5 0 talk
    this-one 6 0 talk
    this-one 7 0 talk
    this-one 8 0 talk
    this-one 9 0 talk
    -512 -2738 -284 368498 0 387246 676 camera-set
    matthew 10 1 talk
    this-one 58 0 1 anim-then
    9 frames wait
    this-one 11 5 talk
    -584 -2774 -716 364921 0 386981 1002 camera-set
    matthew 12 6 talk
    13 james say-by
    this-one 367536 386909 walk-to
    25 frames wait
    ['] L-80101FC8 start-event
    this-one wait-stop
    this-one 367540 387086 walk-to
    this-one wait-stop
    this-one 3074 -240 turn-to
    events.main:sub-80096D34
    14 james say-by-wait
    matthew 6 287 face
    matthew wait-turned
    -512 -1406 -680 367498 0 386906 -284 camera-set
    matthew 15 7 talk
    this-one 73 74 1 anim-then
    16 james say-by-wait
    matthew 17 3 talk
    18 james say-by-wait
    matthew 19 1 talk
    20 james say-by-wait
    21 james say-by-wait
    matthew 22 7 talk
    23 james say-by-wait
    matthew 24 1 talk
    25 james say-by-wait
    matthew 83 84 1 anim-then
    80 frames wait
    matthew 366870 386523 walk-to
    24 frames wait
    -584 -2774 -716 364921 0 386981 1002 camera-set
    this-one wait-stop
    this-one 76 77 1 anim-then
    matthew 17 18 1 anim-then
    54 frames wait
    matthew 6 -300 face
    matthew wait-turned
    matthew 26 7 talk
    27 james say-by-wait
    9 screen-effect
    wait-effect
    27 -1 390585 417928 -1078 0 warp
    ;

\ the words the main executable's scripts go to here, by address
' arrive-80101FE8 $80101FE8 4 overlay-word!
