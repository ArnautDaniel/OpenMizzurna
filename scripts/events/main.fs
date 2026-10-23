\ main.fs - the main executable's scripts (started from its C, called by others).
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.main
USING: events ;

\ (looped back to: declared first)
defer L-80099064

: L-80097090
    this-one 101888 358656 walk-to  this-one wait-stop
    end-script
    ;

: L-8009709C
    this-one 101648 355120 walk-to  this-one wait-stop
    end-script
    ;

: L-80097118
    this-one 104592 358768 walk-to  this-one wait-stop
    L-80097090
    goto L-8009709C
    ;

: L-80081AA8
    this-one 100480 354688 walk-to  this-one wait-stop
    10 frames wait
    1 1 elevator-doors
    this-one 100480 353968 walk-to  this-one wait-stop
    100 frames wait
    this-one 29 2 set-floor
    2 1 elevator-doors
    this-one 100448 355136 walk-to  this-one wait-stop
    this-one 101664 355040 walk-to  this-one wait-stop
    end-script
    ;

: L-80097318
    this-one 101392 351232 walk-to  this-one wait-stop
    this-one 101200 351200 walk-to  this-one wait-stop
    this-one 25 0 through-door
    end-script
    ;

\ $80081B54
: route-80081B54
    barbara as-this-one
    L-80097118
    L-80081AA8
    L-80097318
    this-one 98784 350576 walk-to  this-one wait-stop
    this-one 1444 -40 turn-to
    this-one wait-turned
    this-one arrive
    end-script
    ;

: L-800970A8
    this-one 101664 355040 walk-to  this-one wait-stop
    end-script
    ;

: L-80097338
    this-one 100928 350944 walk-to  this-one wait-stop
    this-one 25 5 through-door
    goto L-800970A8
    ;

: L-80081B00
    this-one 100496 354656 walk-to  this-one wait-stop
    20 frames wait
    2 1 elevator-doors
    this-one 100496 353888 walk-to  this-one wait-stop
    100 frames wait
    this-one 2 1 set-floor
    1 1 elevator-doors
    this-one 100560 354944 walk-to  this-one wait-stop
    goto L-8009709C
    ;

: L-800971D4
    L-80097090
    this-one 104592 358768 walk-to  this-one wait-stop
    end-script
    ;

\ $80081B94
: route-80081B94
    barbara as-this-one
    L-80097338
    L-80081B00
    L-800971D4
    this-one 105456 359776 walk-to  this-one wait-stop
    this-one -2039 70 turn-to
    this-one wait-turned
    this-one arrive
    end-script
    ;

\ $80081BD4
: route-80081BD4
    barbara as-this-one
    L-80097338
    L-80081B00
    this-one 102032 356272 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80081BFC
: route-80081BFC
    barbara as-this-one
    L-8009709C
    L-80081AA8
    L-80097318
    this-one 98784 350576 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80081E88
    this-one 90 190 turn-by
    this-one 4 0 anim
    this-one wait-turned
    this-one 346352 287376 walk-to  this-one wait-stop
    this-one 345824 287248 walk-to  this-one wait-stop
    this-one 345056 285248 walk-to  this-one wait-stop
    this-one 15 3 through-door
    this-one 344528 284080 walk-to  this-one wait-stop
    end-script
    ;

: L-80096D88
    this-one 0 65 act-command
    this-one wait-stop
    this-one 0 75 act-command
    50 frames wait
    this-one 0 66 act-command
    this-one wait-anim
    end-script
    ;

: L-80097FD4
    25 frames wait
    this-one 117 116 1 anim-then
    18 frames wait
    this-one 105 116 1 anim-then
    25 frames wait
    this-one 106 116 1 anim-then
    48 frames wait
    this-one 373984 284144 walk-to  this-one wait-stop
    this-one 374048 312928 walk-to  this-one wait-stop
    this-one 372224 312912 walk-to  this-one wait-stop
    L-80096D88
    end-script
    ;

: L-80097628
    this-one 1024 0 turn-to
    this-one 30 walk-forward
    43 frames wait
    this-one idle
    this-one 589888 131856 walk-to  this-one wait-stop
    this-one 590880 132288 walk-to  this-one wait-stop
    this-one 91 -2 through-door
    this-one 591424 132272 walk-to  this-one wait-stop
    end-script
    ;

\ $80081ED4
: route-80081ED4
    nozz as-this-one
    L-80081E88
    L-80097FD4
    nozz 7 360 attach
    this-one wait-follow
    L-80097628
    nozz arrive
    end-script
    ;

: L-80097600
    this-one 593696 132208 walk-to  this-one wait-stop
    this-one 591104 132272 walk-to  this-one wait-stop
    this-one 90 0 through-door
    L-80096D88
    end-script
    ;

: L-80097F0C
    this-one 344944 284944 walk-to  this-one wait-stop
    this-one 15 -2 through-door
    end-script
    ;

: L-80081E64
    L-80097F0C
    this-one 344832 285104 walk-to  this-one wait-stop
    this-one 346576 287312 walk-to  this-one wait-stop
    this-one 346336 285200 walk-to  this-one wait-stop
    end-script
    ;

\ $80081F0C
: route-80081F0C
    nozz as-this-one
    L-80097600
    nozz 171 360 attach
    L-80081E64
    nozz arrive
    end-script
    ;

: L-80097670
    this-one 54896 408800 walk-to  this-one wait-stop
    this-one 57984 411792 walk-to  this-one wait-stop
    this-one 56800 415408 walk-to  this-one wait-stop
    this-one 54400 416096 walk-to  this-one wait-stop
    end-script
    ;

: L-80097694
    L-80097670
    this-one 52736 415872 walk-to  this-one wait-stop
    this-one 45296 415808 walk-to  this-one wait-stop
    this-one 44416 415760 walk-to  this-one wait-stop
    end-script
    ;

: L-800976F8
    this-one 41296 415408 walk-to  this-one wait-stop
    this-one 36240 407632 walk-to  this-one wait-stop
    end-script
    ;

\ $80081F38
: route-80081F38
    nozz as-this-one
    L-80081E88
    L-80096D88
    nozz 170 360 attach
    this-one wait-follow
    L-80097694
    L-800976F8
    end-script
    ;

: L-8009770C
    this-one 41744 415872 walk-to  this-one wait-stop
    this-one 52688 415856 walk-to  this-one wait-stop
    this-one 54592 416496 walk-to  this-one wait-stop
    this-one 57728 413136 walk-to  this-one wait-stop
    this-one 56976 410464 walk-to  this-one wait-stop
    this-one 54992 408128 walk-to  this-one wait-stop
    this-one 56160 406960 walk-to  this-one wait-stop
    end-script
    ;

: L-80097F24
    this-one 373312 314016 walk-to  this-one wait-stop
    this-one 344416 314016 walk-to  this-one wait-stop
    this-one 344288 284224 walk-to  this-one wait-stop
    this-one 20 22 1 anim-then
    150 frames wait
    this-one 117 116 1 anim-then
    18 frames wait
    this-one 105 116 1 anim-then
    25 frames wait
    this-one 106 116 1 anim-then
    48 frames wait
    this-one 83 84 1 anim-then
    30 frames wait
    this-one 85 0 anim
    30 frames wait
    this-one 344832 285104 walk-to  this-one wait-stop
    end-script
    ;

\ $80081FB4
: route-80081FB4
    nozz as-this-one
    L-8009770C
    L-80096D88
    nozz 13 360 attach
    this-one wait-follow
    L-80097F24
    L-80081E64
    nozz arrive
    end-script
    ;

: L-80097994
    this-one 249760 136000 walk-to  this-one wait-stop
    this-one 1 -2 through-door
    this-one 249696 135088 walk-to  this-one wait-stop
    end-script
    ;

: L-80081E40
    L-80097994
    this-one 249904 134752 walk-to  this-one wait-stop
    this-one 4888 -260 turn-to
    this-one wait-turned
    end-script
    ;

\ $80081FF4
: route-80081FF4
    nozz as-this-one
    L-80081E88
    nozz 7 9 44 lift
    this-one wait-follow
    L-80081E40
    nozz arrive
    end-script
    ;

: L-80097968
    this-one 249616 135536 walk-to  this-one wait-stop
    this-one 1 0 through-door
    end-script
    ;

: L-8009823C
    this-one 304720 312336 walk-to  this-one wait-stop
    this-one 305024 305776 walk-to  this-one wait-stop
    this-one 305920 305776 walk-to  this-one wait-stop
    this-one 2099 200 turn-to
    this-one wait-turned
    this-one 1024 -200 turn-to
    this-one wait-turned
    this-one 71 -2 through-door
    this-one 307232 304864 walk-to  this-one wait-stop
    end-script
    ;

: L-800982CC
    this-one 311248 304752 walk-to  this-one wait-stop
    this-one 72 0 through-door
    this-one 311088 303872 walk-to  this-one wait-stop
    this-one 303770 set-z
    this-one 0 2048 0 set-rot
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 311774 0 303234 set-pos
    this-one 0 0 0 set-rot
    this-one -960 set-y
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 13 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 19 2 set-floor
    this-one 311573 0 303735 set-pos
    this-one 311296 304384 walk-to  this-one wait-stop
    this-one 73 0 through-door
    end-script
    ;

: L-80081DD4
    L-8009823C
    L-800982CC
    this-one 314560 304624 walk-to  this-one wait-stop
    this-one 75 1 through-door
    this-one 314464 303296 walk-to  this-one wait-stop
    end-script
    ;

\ $80082028
: route-80082028
    nozz as-this-one
    L-80097968
    nozz 9 0 44 lift
    this-one wait-follow
    L-80081DD4
    nozz arrive
    end-script
    ;

: L-800983C0
    this-one 311152 304656 walk-to  this-one wait-stop
    this-one 73 1 through-door
    this-one 311728 303744 walk-to  this-one wait-stop
    this-one 303636 set-z
    this-one 0 2048 0 set-rot
    this-one 6 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 310939 0 303286 set-pos
    this-one 0 0 0 set-rot
    this-one -320 set-y
    this-one 6 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 8 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 31 1 set-floor
    this-one 311232 304352 walk-to  this-one wait-stop
    this-one 72 1 through-door
    this-one 307232 304864 walk-to  this-one wait-stop
    end-script
    ;

: L-8009828C
    this-one 306240 304976 walk-to  this-one wait-stop
    this-one 71 0 through-door
    this-one 0 200 turn-to
    this-one wait-turned
    this-one 305712 308528 walk-to  this-one wait-stop
    end-script
    ;

: L-80081E04
    this-one 314464 303296 walk-to  this-one wait-stop
    this-one 314784 304384 walk-to  this-one wait-stop
    this-one 75 2 through-door
    L-800983C0
    L-8009828C
    this-one 0 1 76 act-command
    this-one wait-stop
    end-script
    ;

: L-80098A38
    this-one 374112 314160 walk-to  this-one wait-stop
    end-script
    ;

\ $8008205C
: route-8008205C
    nozz as-this-one
    L-80081E04
    L-80098A38
    L-80096D88
    nozz 10 360 attach
    this-one wait-follow
    goto L-80097694
    ;

\ $80082094
: route-80082094
    nozz as-this-one
    L-800976F8
    nozz arrive
    end-script
    ;

\ $800820AC
: route-800820AC
    nozz as-this-one
    L-8009770C
    L-80096D88
    nozz 13 360 attach
    this-one wait-follow
    L-80097F24
    L-80081E64
    nozz arrive
    end-script
    ;

\ $800820EC
: route-800820EC
    nozz as-this-one
    this-one 346640 287440 walk-to  this-one wait-stop
    this-one 346944 287456 walk-to  this-one wait-stop
    this-one 14 3 through-door
    this-one 347712 286336 walk-to  this-one wait-stop
    this-one 349264 285680 walk-to  this-one wait-stop
    nozz arrive
    end-script
    ;

\ $80082128
: route-80082128
    nozz as-this-one
    this-one 347232 287280 walk-to  this-one wait-stop
    this-one 14 1 through-door
    this-one 346288 285216 walk-to  this-one wait-stop
    this-one -1024 0 turn-to
    nozz arrive
    end-script
    ;

\ $80082160
: route-80082160
    nozz as-this-one
    L-80081E88
    L-80097FD4
    nozz 16 360 attach
    this-one wait-follow
    L-80081E40
    nozz arrive
    end-script
    ;

: L-8009854C
    this-one 2048 0 turn-to
    this-one 30 walk-forward
    30 frames wait
    this-one idle
    this-one 42528 276288 walk-to  this-one wait-stop
    this-one 40272 277072 walk-to  this-one wait-stop
    this-one -981 0 turn-to
    this-one 39152 277008 walk-to  this-one wait-stop
    end-script
    ;

: L-80098594
    this-one 86 -2 through-door
    this-one 37552 277056 walk-to  this-one wait-stop
    end-script
    ;

: L-800985C4
    L-8009854C
    goto L-80098594
    ;

\ $80082198
: route-80082198
    nozz as-this-one
    L-80097968
    nozz 54 360 attach
    this-one wait-follow
    L-800985C4
    nozz arrive
    end-script
    ;

: L-800985AC
    this-one 38720 277040 walk-to  this-one wait-stop
    this-one 86 0 through-door
    end-script
    ;

: L-80098534
    this-one 39520 277152 walk-to  this-one wait-stop
    this-one 45840 279040 walk-to  this-one wait-stop
    goto L-80096D88
    ;

: L-800985D4
    L-800985AC
    goto L-80098534
    ;

\ $800821C8
: route-800821C8
    nozz as-this-one
    L-800985D4
    nozz 11 360 attach
    this-one wait-follow
    L-80097694
    nozz arrive
    end-script
    ;

\ $800821F8
: route-800821F8
    nozz as-this-one
    L-80081E04
    nozz 0 7 40 lift
    this-one wait-follow
    L-80081E64
    nozz arrive
    end-script
    ;

\ $8008222C
: route-8008222C
    nozz as-this-one
    L-80081E88
    nozz 7 0 44 lift
    this-one wait-follow
    L-80081DD4
    nozz arrive
    end-script
    ;

: L-800827F8
    this-one 71 0 anim
    this-one wait-anim
    L-800985AC
    goto L-80098534
    ;

\ $80082854
: route-80082854
    cohen as-this-one
    L-800827F8
    cohen 1 360 attach
    this-one wait-follow
    L-80097628
    this-one 593744 132448 walk-to  this-one wait-stop
    this-one 593728 132928 walk-to  this-one wait-stop
    this-one 20 22 1 anim-then
    900 frames wait
    cohen arrive
    end-script
    ;

\ $800828AC
: route-800828AC
    cohen as-this-one
    L-800827F8
    cohen 11 360 attach
    this-one wait-follow
    L-80097694
    L-800976F8
    cohen arrive
    end-script
    ;

\ $800828E4
: route-800828E4
    cohen as-this-one
    L-800827F8
    this-one 11 360 attach
    this-one wait-follow
    this-one arrive
    end-script
    ;

\ $80096D74
: sub-80096D74
    this-one 67 0 anim
    this-one wait-anim
    end-script
    ;

: L-800827C0
    L-8009854C
    L-80098594
    this-one 37632 278096 walk-to  this-one wait-stop
    this-one 0 6897 0 set-rot
    this-one 68 1 anim
    end-script
    ;

\ $8008290C
: route-8008290C
    cohen as-this-one
    sub-80096D74
    L-80097600
    cohen 2 360 attach
    this-one wait-follow
    L-800827C0
    cohen arrive
    end-script
    ;

\ $80082964
: route-80082964
    cohen as-this-one
    L-8009770C
    L-80096D88
    cohen 12 360 attach
    this-one wait-follow
    L-800827C0
    cohen arrive
    end-script
    ;

\ $800829BC
: route-800829BC
    cohen as-this-one
    L-800976F8
    cohen arrive
    end-script
    ;

\ $800829D4
: route-800829D4
    cohen arrive
    end-script
    ;

\ $800829DC
: route-800829DC
    cohen as-this-one
    this-one 45 360 attach
    this-one wait-follow
    L-800827C0
    this-one arrive
    end-script
    ;

: L-80097C04
    this-one 380864 140752 walk-to  this-one wait-stop
    end-script
    ;

: L-80097C10
    this-one 1024 0 turn-to
    this-one 57 -2 through-door
    this-one 382112 140976 walk-to  this-one wait-stop
    end-script
    ;

: L-80097C54
    this-one 383072 141120 walk-to  this-one wait-stop
    this-one 58 0 through-door
    this-one 383728 141360 walk-to  this-one wait-stop
    end-script
    ;

: L-80082818
    L-80097C04
    L-80097C10
    L-80097C54
    this-one 383728 141360 walk-to  this-one wait-stop
    this-one 384896 142448 walk-to  this-one wait-stop
    this-one 385744 147696 walk-to  this-one wait-stop
    this-one 388144 146704 walk-to  this-one wait-stop
    end-script
    ;

\ $80082A04
: route-80082A04
    cohen as-this-one
    L-8009770C
    L-80096D88
    cohen 60 360 attach
    this-one wait-follow
    L-80082818
    this-one arrive
    end-script
    ;

: L-80097C34
    this-one 383392 140944 walk-to  this-one wait-stop
    this-one 58 1 through-door
    this-one 382112 140976 walk-to  this-one wait-stop
    end-script
    ;

: L-80097C74
    this-one 381328 140576 walk-to  this-one wait-stop
    this-one 56 0 through-door
    end-script
    ;

\ $80082A3C
: route-80082A3C
    cohen as-this-one
    this-one 383680 146976 walk-to  this-one wait-stop
    L-80097C34
    L-80097C74
    L-80096D88
    cohen 59 360 attach
    this-one wait-follow
    L-80097694
    L-800976F8
    cohen arrive
    end-script
    ;

: L-80096D04
    this-one 63 0 anim
    this-one anim-hold
    this-one wait-anim
    end-script
    ;

: L-80096EF4
    this-one 20 22 1 anim-then
    end-script
    ;

: L-80082A8C
    this-one 48 50 1 anim-then
    120 frames wait
    this-one 49 0 anim
    this-one wait-anim
    end-script
    ;

\ $80096D1C
: sub-80096D1C
    this-one 64 0 anim
    this-one anim-hold
    this-one wait-anim
    end-script
    ;

: L-80082AB8
    cohen as-this-one
    this-one 36321 0 407507 set-pos
    this-one 36464 408064 walk-to  this-one wait-stop
    this-one 0 0 turn-to
    this-one wait-turned
    100 door-anim
    L-80096D04
    this-one 36336 409936 walk-to  this-one wait-stop
    this-one 31952 410112 walk-to  this-one wait-stop
    L-80096EF4
    180 frames wait
    L-80082A8C
    this-one 30880 416368 walk-to  this-one wait-stop
    L-80082A8C
    this-one 30112 418320 walk-to  this-one wait-stop
    L-80082A8C
    this-one 31120 420944 walk-to  this-one wait-stop
    L-80082A8C
    this-one 32096 423648 walk-to  this-one wait-stop
    L-80082A8C
    this-one 30624 425072 walk-to  this-one wait-stop
    L-80082A8C
    this-one 31552 426512 walk-to  this-one wait-stop
    L-80082A8C
    this-one 32960 425392 walk-to  this-one wait-stop
    L-80096EF4
    240 frames wait
    this-one 32528 426944 walk-to  this-one wait-stop
    this-one 2305 0 turn-to
    this-one wait-turned
    L-80082A8C
    this-one 32960 425392 walk-to  this-one wait-stop
    L-80082A8C
    this-one 37216 431184 walk-to  this-one wait-stop
    L-80082A8C
    this-one 40144 432016 walk-to  this-one wait-stop
    this-one 40672 431408 walk-to  this-one wait-stop
    this-one 2474 0 turn-to
    this-one wait-turned
    L-80082A8C
    this-one 26801 0 turn-to
    this-one wait-turned
    this-one 34160 431520 walk-to  this-one wait-stop
    this-one 24400 430624 walk-to  this-one wait-stop
    this-one 21616 428192 walk-to  this-one wait-stop
    this-one 16224 424496 walk-to  this-one wait-stop
    L-80096EF4
    240 frames wait
    this-one 19424 418928 walk-to  this-one wait-stop
    this-one 18992 414800 walk-to  this-one wait-stop
    this-one 21472 412224 walk-to  this-one wait-stop
    this-one 25824 408928 walk-to  this-one wait-stop
    L-80096EF4
    240 frames wait
    this-one 29408 409216 walk-to  this-one wait-stop
    this-one 36336 409936 walk-to  this-one wait-stop
    L-80096EF4
    240 frames wait
    this-one 36400 408448 walk-to  this-one wait-stop
    100 door-anim
    sub-80096D1C
    this-one 36320 407504 walk-to  this-one wait-stop
    end-script
    ;

\ $80082CB8
: route-80082CB8
    cohen as-this-one
    L-80082AB8
    L-8009770C
    L-80096D88
    cohen 12 360 attach
    this-one wait-follow
    L-800827C0
    cohen arrive
    end-script
    ;

: L-80097980
    this-one 249744 138768 walk-to  this-one wait-stop
    this-one 249776 135984 walk-to  this-one wait-stop
    end-script
    ;

: L-80097A34
    L-80097980
    goto L-80097994
    ;

\ $80082CF8
: route-80082CF8
    cohen as-this-one
    L-800827F8
    cohen 14 360 attach
    this-one wait-follow
    L-80097A34
    this-one 250432 135040 walk-to  this-one wait-stop
    cohen arrive
    end-script
    ;

: L-800979F4
    L-80097968
    this-one 250224 137760 walk-to  this-one wait-stop
    goto L-80096D88
    ;

\ $80082D30
: route-80082D30
    cohen as-this-one
    L-800979F4
    cohen 54 360 attach
    this-one wait-follow
    L-800827C0
    cohen arrive
    end-script
    ;

\ $80082D60
: route-80082D60
    cohen as-this-one
    L-800827F8
    cohen 11 360 attach
    this-one wait-follow
    L-80097694
    this-one 40944 414800 walk-to  this-one wait-stop
    this-one 37600 411920 walk-to  this-one wait-stop
    this-one 30496 416928 walk-to  this-one wait-stop
    this-one 30128 411440 walk-to  this-one wait-stop
    this-one 32016 426272 walk-to  this-one wait-stop
    cohen arrive
    end-script
    ;

: L-800976B8
    this-one 45296 415808 walk-to  this-one wait-stop
    this-one 52736 415872 walk-to  this-one wait-stop
    this-one 54400 416096 walk-to  this-one wait-stop
    this-one 56800 415408 walk-to  this-one wait-stop
    this-one 57984 411792 walk-to  this-one wait-stop
    this-one 54896 408800 walk-to  this-one wait-stop
    this-one 55168 405504 walk-to  this-one wait-stop
    goto L-80096D88
    ;

: L-80082DB8
    cohen as-this-one
    this-one 30128 411440 walk-to  this-one wait-stop
    this-one 30496 416928 walk-to  this-one wait-stop
    this-one 37600 411920 walk-to  this-one wait-stop
    this-one 40944 414800 walk-to  this-one wait-stop
    this-one 44416 415760 walk-to  this-one wait-stop
    goto L-800976B8
    ;

: L-80098048
    this-one 329824 269392 walk-to  this-one wait-stop
    this-one 82 -2 through-door
    end-script
    ;

: L-80096F80
    this-one 0 57 act-command
    this-one wait-stop
    this-one 0 3071 0 set-rot
    this-one 4554 0 anim
    this-one wait-anim
    this-one sit-diner-seat
    end-script
    ;

\ $80082DF0
: route-80082DF0
    L-80082DB8
    cohen 172 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    cohen arrive
    end-script
    ;

\ $80082E28
: route-80082E28
    L-80082DB8
    cohen 12 360 attach
    this-one wait-follow
    L-800827C0
    cohen arrive
    end-script
    ;

\ $80096FB0
: sub-80096FB0
    this-one 4508 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 0 1 anim
    this-one leave-diner-seat
    end-script
    ;

: L-80098060
    this-one 329552 269488 walk-to  this-one wait-stop
    this-one 82 0 through-door
    end-script
    ;

\ $80082E50
: route-80082E50
    cohen as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 157 360 attach
    this-one wait-follow
    L-80097C04
    L-80097C10
    L-80097C54
    this-one 383728 141360 walk-to  this-one wait-stop
    this-one 384896 142448 walk-to  this-one wait-stop
    this-one 385744 147696 walk-to  this-one wait-stop
    this-one 388144 146704 walk-to  this-one wait-stop
    cohen arrive
    end-script
    ;

: L-800979B4
    this-one 249696 133904 walk-to  this-one wait-stop
    this-one 3 0 through-door
    this-one 249536 133440 walk-to  this-one wait-stop
    end-script
    ;

: L-800978D8
    this-one 247552 133200 walk-to  this-one wait-stop
    this-one 4 1 through-door
    end-script
    ;

: L-800979D4
    L-80097980
    L-80097994
    L-800979B4
    goto L-800978D8
    ;

\ $80082EC0
: route-80082EC0
    cohen as-this-one
    this-one 383680 146976 walk-to  this-one wait-stop
    L-80097C34
    L-80097C74
    L-80096D88
    cohen 158 360 attach
    this-one wait-follow
    L-800979D4
    this-one 248352 131744 walk-to  this-one wait-stop
    cohen arrive
    end-script
    ;

: L-80097874
    this-one 4 2 through-door
    this-one 249536 133440 walk-to  this-one wait-stop
    end-script
    ;

: L-800978F0
    this-one 249616 133744 walk-to  this-one wait-stop
    this-one 3 1 through-door
    this-one 249696 135088 walk-to  this-one wait-stop
    end-script
    ;

: L-80097A0C
    L-80097874
    L-800978F0
    L-80097968
    this-one 250224 137760 walk-to  this-one wait-stop
    goto L-80096D88
    ;

\ $80082F10
: route-80082F10
    cohen as-this-one
    L-80097A0C
    cohen 54 360 attach
    this-one wait-follow
    L-800827C0
    cohen arrive
    end-script
    ;

\ $80082F40
: route-80082F40
    cohen as-this-one
    L-800827F8
    cohen 11 360 attach
    this-one wait-follow
    L-80097694
    this-one 35216 410256 walk-to  this-one wait-stop
    this-one 25376 408880 walk-to  this-one wait-stop
    this-one 23216 411552 walk-to  this-one wait-stop
    cohen arrive
    end-script
    ;

\ $80082F88
: route-80082F88
    cohen as-this-one
    this-one 25632 408768 walk-to  this-one wait-stop
    this-one 35936 410560 walk-to  this-one wait-stop
    this-one 44416 415760 walk-to  this-one wait-stop
    L-800976B8
    cohen 12 360 attach
    this-one wait-follow
    L-800827C0
    cohen arrive
    end-script
    ;

\ $80082FD0
: route-80082FD0
    cohen as-this-one
    L-80082DB8
    cohen 60 360 attach
    this-one wait-follow
    L-80082818
    cohen arrive
    end-script
    ;

: L-800970B4
    this-one 106784 356016 walk-to  this-one wait-stop
    this-one 104800 356688 walk-to  this-one wait-stop
    end-script
    ;

: L-800970C8
    10 frames wait
    this-one 104624 357920 walk-to  this-one wait-stop
    10 frames wait
    this-one 104592 358768 walk-to  this-one wait-stop
    end-script
    ;

\ $80083000
: route-80083000
    cohen as-this-one
    L-80097A0C
    cohen 136 360 attach
    this-one wait-follow
    L-800970B4
    L-800970C8
    this-one 103824 359072 walk-to  this-one wait-stop
    cohen arrive
    end-script
    ;

: L-800970EC
    this-one 104672 358240 walk-to  this-one wait-stop
    10 frames wait
    this-one 104736 356928 walk-to  this-one wait-stop
    10 frames wait
    this-one 104752 356512 walk-to  this-one wait-stop
    end-script
    ;

\ $80083040
: route-80083040
    cohen as-this-one
    L-800970EC
    L-80096D88
    cohen 45 360 attach
    this-one wait-follow
    L-800827C0
    cohen arrive
    end-script
    ;

\ $80083078
: route-80083078
    cohen as-this-one
    L-800827F8
    cohen 11 360 attach
    this-one wait-follow
    L-80097694
    this-one 44416 415152 walk-to  this-one wait-stop
    cohen arrive
    end-script
    ;

: L-80098914
    this-one 92224 53120 walk-to  this-one wait-stop
    sub-80096D1C
    this-one 95456 53584 walk-to  this-one wait-stop
    end-script
    ;

: L-80098930
    L-80098914
    this-one 98624 51568 walk-to  this-one wait-stop
    goto L-80096D88
    ;

\ $80096D34
: sub-80096D34
    this-one 70 0 anim
    this-one wait-anim
    this-one 72 1 anim
    end-script
    ;

\ $80083794
: route-80083794
    dennis as-this-one
    L-80098930
    this-one 48 360 attach
    this-one wait-follow
    L-80097628
    this-one 592736 132224 walk-to  this-one wait-stop
    this-one 592736 131376 walk-to  this-one wait-stop
    this-one 998 -220 turn-to
    this-one wait-turned
    sub-80096D34
    this-one arrive
    end-script
    ;

: L-800988E8
    this-one 98688 51792 walk-to  this-one wait-stop
    this-one 97008 53120 walk-to  this-one wait-stop
    this-one 92464 53248 walk-to  this-one wait-stop
    L-80096D04
    this-one 90624 53264 walk-to  this-one wait-stop
    end-script
    ;

\ $800837EC
: route-800837EC
    dennis as-this-one
    this-one 592736 132224 walk-to  this-one wait-stop
    L-80097600
    this-one 124 360 attach
    this-one wait-follow
    this-one 129 360 attach
    this-one wait-follow
    L-800988E8
    this-one arrive
    end-script
    ;

: L-80097DC4
    this-one 563904 218192 walk-to  this-one wait-stop
    this-one 1054 200 turn-to
    200 frames wait
    this-one 565600 220480 walk-to  this-one wait-stop
    this-one 1116 200 turn-to
    200 frames wait
    this-one 568272 225744 walk-to  this-one wait-stop
    this-one 572192 224800 walk-to  this-one wait-stop
    this-one 2052 200 turn-to
    200 frames wait
    this-one 579520 223440 walk-to  this-one wait-stop
    this-one 579392 219792 walk-to  this-one wait-stop
    this-one 1042 -200 turn-to
    200 frames wait
    this-one 580032 215232 walk-to  this-one wait-stop
    this-one 3017 200 turn-to
    200 frames wait
    this-one 583888 212656 walk-to  this-one wait-stop
    this-one 477 -200 turn-to
    200 frames wait
    this-one 581440 209152 walk-to  this-one wait-stop
    this-one 569568 209680 walk-to  this-one wait-stop
    this-one 4186 200 turn-to
    200 frames wait
    this-one 561840 212192 walk-to  this-one wait-stop
    this-one 565344 214320 walk-to  this-one wait-stop
    end-script
    ;

\ $80083834
: route-80083834
    dennis as-this-one
    L-80098930
    this-one 118 360 attach
    this-one wait-follow
    L-80097DC4
    this-one arrive
    end-script
    ;

\ $80083864
: route-80083864
    dennis as-this-one
    L-80098930
    this-one 107 360 attach
    this-one wait-follow
    L-80097A34
    L-800979B4
    this-one 250240 132192 walk-to  this-one wait-stop
    this-one 249398 840 133667 set-pos
    this-one arrive
    end-script
    ;

: L-80097A44
    this-one 132475 set-z
    this-one 0 -2035 0 set-rot
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    3 frames wait
    this-one 250143 567 131210 set-pos
    this-one 567 set-y
    this-one 0 0 0 set-rot
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 13 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 0 1 set-floor
    this-one 249536 133440 walk-to  this-one wait-stop
    end-script
    ;

\ $800838E4
: route-800838E4
    dennis as-this-one
    this-one 249312 132368 walk-to  this-one wait-stop
    L-80097A44
    L-800978F0
    L-800979F4
    this-one 160 360 attach
    this-one wait-follow
    L-800988E8
    this-one arrive
    end-script
    ;

\ $8008392C
: route-8008392C
    dennis as-this-one
    L-80096D88
    this-one 28 360 attach
    this-one wait-follow
    this-one 129 360 attach
    this-one wait-follow
    L-800988E8
    this-one arrive
    end-script
    ;

: L-8008355C
    this-one 103184 53856 walk-to  this-one wait-stop
    this-one 104656 59200 walk-to  this-one wait-stop
    this-one 971 200 turn-to
    20 frames wait
    this-one 48 50 1 anim-then
    200 frames wait
    this-one 49 0 anim
    this-one wait-anim
    this-one 104384 61744 walk-to  this-one wait-stop
    this-one 1038 200 turn-to
    180 frames wait
    this-one 104480 56448 walk-to  this-one wait-stop
    this-one 112352 53408 walk-to  this-one wait-stop
    this-one 1114 0 turn-to
    50 frames wait
    this-one 65 0 anim
    50 frames wait
    this-one 112464 51328 walk-to  this-one wait-stop
    this-one 1034 -200 turn-to
    160 frames wait
    this-one 112128 49872 walk-to  this-one wait-stop
    this-one 115792 49232 walk-to  this-one wait-stop
    this-one 113584 46352 walk-to  this-one wait-stop
    this-one 112432 51264 walk-to  this-one wait-stop
    this-one 5250 200 turn-to
    160 frames wait
    this-one 65 0 anim
    50 frames wait
    this-one 32 -2 through-door
    this-one 113817 0 52532 set-pos
    end-script
    ;

\ $8008396C
: route-8008396C
    dennis as-this-one
    L-80098914
    L-8008355C
    this-one arrive
    end-script
    ;

\ $8008398C
: route-8008398C
    dennis as-this-one
    this-one 108094 0 57819 set-pos
    this-one 1994 0 turn-to
    sub-80096D1C
    L-800988E8
    this-one arrive
    end-script
    ;

: L-80083680
    this-one 108094 0 57819 set-pos
    this-one 1994 0 turn-to
    sub-80096D1C
    goto L-80096D88
    ;

: L-800836AC
    L-8009823C
    L-800982CC
    this-one 310416 305248 walk-to  this-one wait-stop
    this-one 76 1 through-door
    this-one 310736 305888 walk-to  this-one wait-stop
    this-one 81 1 anim
    100 frames wait
    this-one 311936 305968 walk-to  this-one wait-stop
    this-one 81 1 anim
    100 frames wait
    this-one 309632 305936 walk-to  this-one wait-stop
    this-one 81 1 anim
    100 frames wait
    this-one 310176 307008 walk-to  this-one wait-stop
    this-one 151 1 anim
    200 frames wait
    end-script
    ;

\ $800839C8
: route-800839C8
    dennis as-this-one
    L-80083680
    this-one 53 360 attach
    this-one wait-follow
    L-800836AC
    this-one arrive
    end-script
    ;

: L-80083744
    this-one 153 0 anim
    this-one wait-anim
    this-one 81 1 anim
    100 frames wait
    this-one 310288 305376 walk-to  this-one wait-stop
    this-one 76 6 through-door
    L-800983C0
    L-8009828C
    goto L-80096D88
    ;

\ $800839F8
: route-800839F8
    dennis as-this-one
    L-80083744
    this-one 139 360 attach
    this-one wait-follow
    L-800988E8
    this-one arrive
    end-script
    ;

: L-80097B74
    this-one 333296 437072 walk-to  this-one wait-stop
    this-one 40 -2 through-door
    end-script
    ;

\ $80083A28
: route-80083A28
    dennis as-this-one
    L-80083680
    this-one 146 360 attach
    this-one wait-follow
    L-80097B74
    this-one 334400 441472 walk-to  this-one wait-stop
    this-one 4780 200 turn-to
    this-one wait-turned
    this-one arrive
    end-script
    ;

: L-80097B8C
    this-one 333184 437120 walk-to  this-one wait-stop
    this-one 40 0 through-door
    end-script
    ;

: L-80097BA4
    L-80097B8C
    goto L-80096D88
    ;

: L-80097028
    this-one 110256 200224 walk-to  this-one wait-stop
    this-one 106880 199072 walk-to  this-one wait-stop
    this-one 39 -2 through-door
    this-one 1 1 1 set-pos
    end-script
    ;

\ $80083A70
: route-80083A70
    dennis as-this-one
    L-80097BA4
    this-one 144 360 attach
    this-one wait-follow
    100 frames wait
    this-one 88 360 attach
    this-one wait-follow
    L-80097028
    this-one arrive
    end-script
    ;

\ $80083AB8
: route-80083AB8
    dennis as-this-one
    this-one 147 360 attach
    this-one wait-follow
    L-800988E8
    this-one arrive
    end-script
    ;

: L-80097808
    this-one 463696 293168 walk-to  this-one wait-stop
    this-one 464352 288192 walk-to  this-one wait-stop
    this-one -7202 0 turn-to
    L-80096D04
    this-one 466308 0 288205 set-pos
    end-script
    ;

\ $80083AE0
: route-80083AE0
    dennis as-this-one
    L-80098930
    this-one 161 360 attach
    this-one wait-follow
    L-80097808
    this-one arrive
    end-script
    ;

: L-80097840
    this-one 464740 0 288218 set-pos
    this-one -8816 0 turn-to
    sub-80096D1C
    this-one 463696 293168 walk-to  this-one wait-stop
    goto L-80096D88
    ;

\ $80083B10
: route-80083B10
    dennis as-this-one
    L-80097840
    this-one 140 360 attach
    this-one wait-follow
    L-800836AC
    this-one arrive
    end-script
    ;

: L-80097780
    this-one 468288 292784 walk-to  this-one wait-stop
    end-script
    ;

: L-8009778C
    this-one -2093 0 turn-to
    this-one 48 -2 through-door
    this-one 468288 290448 walk-to  this-one wait-stop
    end-script
    ;

: L-80096FD8
    this-one 0 68 act-command
    this-one wait-stop
    this-one 0 3071 0 set-rot
    this-one 4554 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one sit-club-seat
    end-script
    ;

: L-800977D0
    L-80097780
    L-8009778C
    this-one -1 1 69 act-command
    goto L-80096FD8
    ;

\ $80083B40
: route-80083B40
    dennis as-this-one
    L-80083744
    this-one 141 360 attach
    this-one wait-follow
    L-800977D0
    this-one arrive
    end-script
    ;

: L-8009700C
    this-one 2708 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one leave-club-seat
    end-script
    ;

: L-800977B0
    this-one 468304 292400 walk-to  this-one wait-stop
    this-one 48 0 through-door
    this-one 469344 294304 walk-to  this-one wait-stop
    end-script
    ;

: L-800977F0
    L-8009700C
    this-one 468080 290384 walk-to  this-one wait-stop
    goto L-800977B0
    ;

\ $80083B70
: route-80083B70
    dennis as-this-one
    L-800977F0
    L-80096D88
    this-one 129 360 attach
    this-one wait-follow
    L-800988E8
    this-one arrive
    end-script
    ;

\ $80083D60
: route-80083D60
    dennis as-this-one
    L-80098930
    this-one 53 360 attach
    this-one wait-follow
    L-800836AC
    this-one arrive
    end-script
    ;

: L-80084110
    this-one 593728 130976 walk-to  this-one wait-stop
    this-one 92 0 through-door
    this-one 592160 130480 walk-to  this-one wait-stop
    this-one 93 1 through-door
    this-one 590288 130528 walk-to  this-one wait-stop
    L-80096D88
    end-script
    ;

: L-80097520
    L-800970B4
    L-800970C8
    goto L-80097118
    ;

: L-800971E8
    this-one 98784 355072 walk-to  this-one wait-stop
    this-one 98704 353728 walk-to  this-one wait-stop
    this-one 353800 set-z
    this-one 0 2048 0 set-rot
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one 99505 -640 352611 set-pos
    this-one -640 set-y
    this-one 0 0 0 set-rot
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 13 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 29 2 set-floor
    this-one 99504 354912 walk-to  this-one wait-stop
    goto L-800970A8
    ;

: L-80097538
    this-one 101168 358832 walk-to  this-one wait-stop
    this-one 22 0 through-door
    end-script
    ;

\ $800841A4
: route-800841A4
    father-barton as-this-one
    L-80084110
    this-one 36 360 attach
    this-one wait-follow
    L-80097520
    L-800971E8
    L-80097538
    this-one 99520 358576 walk-to  this-one wait-stop
    this-one 99472 359216 walk-to  this-one wait-stop
    this-one -1058 -230 turn-to
    this-one wait-turned
    400 frames wait
    this-one 99968 359504 walk-to  this-one wait-stop
    this-one 2525 300 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

: L-800974E8
    this-one 100992 358560 walk-to  this-one wait-stop
    this-one 22 1 through-door
    goto L-800970A8
    ;

: L-800973B4
    this-one 100064 355248 walk-to  this-one wait-stop
    this-one 99296 353664 walk-to  this-one wait-stop
    this-one 353619 set-z
    this-one 0 2048 0 set-rot
    this-one 6 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 98672 -640 352611 set-pos
    this-one -640 set-y
    this-one 0 0 0 set-rot
    this-one 6 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 8 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 2 1 set-floor
    this-one 99200 355120 walk-to  this-one wait-stop
    goto L-8009709C
    ;

: L-80097550
    L-800971D4
    goto L-800970EC
    ;

: L-8008414C
    L-80097628
    this-one 594096 132304 walk-to  this-one wait-stop
    10 frames wait
    this-one 5133 0 anim
    this-one wait-anim
    this-one 594048 133408 walk-to  this-one wait-stop
    this-one 2286 250 turn-to
    this-one wait-turned
    end-script
    ;

\ $80084228
: route-80084228
    father-barton as-this-one
    sub-80096D74
    this-one 100688 359424 walk-to  this-one wait-stop
    this-one 100720 358816 walk-to  this-one wait-stop
    L-800974E8
    L-800973B4
    L-80097550
    L-80096D88
    this-one 4 360 attach
    this-one wait-follow
    L-8008414C
    this-one arrive
    end-script
    ;

\ $80084288
: route-80084288
    father-barton as-this-one
    L-80084110
    this-one 41 360 attach
    this-one wait-follow
    L-80097A34
    this-one 250128 134832 walk-to  this-one wait-stop
    this-one 811 -230 turn-to
    this-one wait-turned
    this-one arrive
    end-script
    ;

: L-80084190
    this-one 464112 292832 walk-to  this-one wait-stop
    this-one 463136 291104 walk-to  this-one wait-stop
    end-script
    ;

\ $800842D0
: route-800842D0
    father-barton as-this-one
    L-800979F4
    this-one 122 360 attach
    this-one wait-follow
    L-80084190
    this-one arrive
    end-script
    ;

: L-80096F24
    this-one 81 0 anim
    50 frames wait
    end-script
    ;

\ $80084300
: route-80084300
    father-barton as-this-one
    this-one 463424 292960 walk-to  this-one wait-stop
    L-80096F24
    L-80096D88
    this-one 123 360 attach
    this-one wait-follow
    L-8008414C
    this-one arrive
    end-script
    ;

\ $80084340
: route-80084340
    father-barton as-this-one
    L-80084110
    this-one 86 360 attach
    this-one wait-follow
    this-one 131056 309376 walk-to  this-one wait-stop
    this-one 131136 317728 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097748
    this-one 131056 309376 walk-to  this-one wait-stop
    end-script
    ;

\ $80084378
: route-80084378
    father-barton as-this-one
    L-80097748
    L-80096D88
    this-one 85 360 attach
    this-one wait-follow
    L-8008414C
    this-one arrive
    end-script
    ;

\ $800843B0
: route-800843B0
    father-barton as-this-one
    L-80084110
    this-one 124 360 attach
    this-one wait-follow
    L-80084190
    this-one arrive
    end-script
    ;

\ $800843E0
: route-800843E0
    father-barton as-this-one
    L-80084110
    this-one 86 360 attach
    this-one wait-follow
    this-one 131056 309376 walk-to  this-one wait-stop
    this-one 131840 317600 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-800846A0
    this-one 247435 0 132925 set-pos
    this-one 4 2 through-door
    this-one 1076 200 turn-to
    this-one 3 walk-forward
    5 frames wait
    this-one 30 walk-forward
    this-one 251358 133362 200 wait-near
    this-one idle
    this-one 1780 110 turn-to
    this-one wait-turned
    this-one 6 1 through-door
    this-one 30 walk-forward
    30 frames wait
    this-one idle
    end-script
    ;

\ $8008472C
: route-8008472C
    hudson as-this-one
    this-one 71 0 anim
    this-one wait-anim
    this-one 90 90 turn-by
    this-one wait-turned
    this-one 30 walk-forward
    13 frames wait
    this-one -65 -110 turn-by
    this-one 249394 132396 200 wait-near
    this-one idle
    L-80097A44
    L-800978D8
    this-one 30 walk-forward
    25 frames wait
    this-one idle
    this-one 1 0 face
    110 frames wait
    this-one 0 0 turn-to
    this-one 30 walk-forward
    25 frames wait
    this-one idle
    L-800846A0
    this-one arrive
    end-script
    ;

: L-800978B8
    this-one 251216 132880 walk-to  this-one wait-stop
    this-one 6 3 through-door
    this-one 249536 133440 walk-to  this-one wait-stop
    end-script
    ;

: L-80084654
    this-one 248432 132512 walk-to  this-one wait-stop
    this-one 611 -100 turn-to
    this-one wait-turned
    this-one 248552 0 132763 set-pos
    this-one 0 1024 0 set-rot
    this-one 70 72 1 anim-then
    end-script
    ;

\ $800847F0
: route-800847F0
    hudson as-this-one
    L-800978B8
    L-800978D8
    L-80084654
    this-one arrive
    end-script
    ;

: L-8008463C
    sub-80096D74
    this-one 247456 132896 walk-to  this-one wait-stop
    goto L-80097A0C
    ;

\ $80084818
: route-80084818
    hudson as-this-one
    L-8008463C
    this-one 23 360 attach
    this-one arrive
    end-script
    ;

\ $8008483C
: route-8008483C
    hudson as-this-one
    L-8008463C
    this-one 24 360 attach
    this-one arrive
    end-script
    ;

\ $80084860
: route-80084860
    hudson as-this-one
    this-one 55 partner-command
    this-one wait-follow
    L-800979D4
    L-80084654
    this-one arrive
    end-script
    ;

\ $8008488C
: route-8008488C
    hudson as-this-one
    this-one 30 partner-command
    this-one wait-follow
    L-80098048
    this-one 328816 268848 walk-to  this-one wait-stop
    this-one 329024 267632 walk-to  this-one wait-stop
    this-one 3144 0 turn-to
    goto sub-80096D34
    ;

: L-80097D9C
    this-one 327008 69936 walk-to  this-one wait-stop
    this-one 328016 69968 walk-to  this-one wait-stop
    this-one 55 -2 through-door
    this-one 329024 69984 walk-to  this-one wait-stop
    end-script
    ;

\ $800848CC
: route-800848CC
    hudson as-this-one
    L-8008463C
    this-one 51 360 attach
    this-one wait-follow
    L-80097D9C
    this-one arrive
    end-script
    ;

: L-80097D68
    this-one 328533 0 70044 set-pos
    this-one -823 0 turn-to
    this-one 55 0 through-door
    this-one 327200 70032 walk-to  this-one wait-stop
    end-script
    ;

\ $800848FC
: route-800848FC
    hudson as-this-one
    L-80097D68
    L-80096D88
    this-one 127 360 attach
    this-one wait-follow
    L-800979D4
    L-80084654
    this-one arrive
    end-script
    ;

\ $8008493C
: route-8008493C
    hudson as-this-one
    L-8008463C
    this-one 40 360 attach
    this-one wait-follow
    this-one arrive
    end-script
    ;

\ $80084964
: route-80084964
    hudson as-this-one
    this-one 175632 423104 walk-to  this-one wait-stop
    this-one 175616 421520 walk-to  this-one wait-stop
    L-80096D88
    this-one 44 360 attach
    this-one wait-follow
    L-800979D4
    L-80084654
    this-one arrive
    end-script
    ;

: L-800849AC
    hudson as-this-one
    L-80096D88
    this-one 109 360 attach
    this-one wait-follow
    132 frames wait
    this-one 148 360 attach
    this-one wait-follow
    this-one 596336 58688 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800849F4
: route-800849F4
    hudson as-this-one
    this-one 328832 268880 walk-to  this-one wait-stop
    L-80098060
    goto L-800849AC
    ;

\ $80084A14
: route-80084A14
    hudson as-this-one
    L-80096D88
    this-one 149 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $80084A54
: route-80084A54
    hudson as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 109 360 attach
    this-one wait-follow
    100 frames wait
    this-one 23 360 attach
    this-one arrive
    end-script
    ;

\ $80084AA0
: route-80084AA0
    hudson as-this-one
    this-one 55 partner-command
    this-one wait-follow
    this-one 51 360 attach
    this-one wait-follow
    L-80097D9C
    this-one arrive
    end-script
    ;

\ $80084AD4
: route-80084AD4
    hudson as-this-one
    L-8008463C
    this-one 148 360 attach
    this-one wait-follow
    this-one 596336 58688 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80084B04
: route-80084B04
    hudson as-this-one
    this-one 55 partner-command
    this-one wait-follow
    this-one 54 360 attach
    this-one wait-follow
    30 frames wait
    this-one 11 360 attach
    this-one wait-follow
    L-80097694
    this-one 44000 415200 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80084B58
: route-80084B58
    hudson as-this-one
    L-800976B8
    this-one 26 360 attach
    this-one wait-follow
    L-800979D4
    L-80084654
    this-one arrive
    end-script
    ;

: L-800984E8
    sub-80096D74
    this-one 307824 306592 walk-to  this-one wait-stop
    this-one 307808 306080 walk-to  this-one wait-stop
    this-one 306960 305392 walk-to  this-one wait-stop
    this-one 74 4 through-door
    L-800983C0
    goto L-8009828C
    ;

: L-800982BC
    this-one 305552 311808 walk-to  this-one wait-stop
    goto L-80096D88
    ;

: L-80098524
    L-800984E8
    goto L-800982BC
    ;

\ $80084E28
: route-80084E28
    isabella as-this-one
    L-80098524
    this-one 145 360 attach
    this-one wait-follow
    L-80097028
    this-one arrive
    end-script
    ;

: L-800984AC
    L-8009823C
    L-800982CC
    this-one 307280 305136 walk-to  this-one wait-stop
    this-one 74 1 through-door
    this-one 306704 306592 walk-to  this-one wait-stop
    this-one 307424 306592 walk-to  this-one wait-stop
    goto sub-80096D34
    ;

\ $80084E58
: route-80084E58
    isabella as-this-one
    this-one 142 360 attach
    this-one wait-follow
    L-800984AC
    this-one arrive
    end-script
    ;

\ $80084E80
: route-80084E80
    isabella as-this-one
    L-80098524
    this-one 141 360 attach
    this-one wait-follow
    L-80097808
    this-one arrive
    end-script
    ;

\ $80084EB0
: route-80084EB0
    isabella as-this-one
    isabella 184 actor-model
    L-80097840
    this-one 140 360 attach
    this-one wait-follow
    L-800984AC
    this-one arrive
    end-script
    ;

: L-800985E4
    this-one 11 0 anim
    this-one anim-hold
    this-one wait-anim
    end-script
    ;

: L-800985FC
    this-one 0 2560 0 set-rot
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    L-800985E4
    L-800985E4
    L-800985E4
    L-800985E4
    L-800985E4
    this-one 12 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 13 2 set-floor
    end-script
    ;

: L-8009866C
    this-one 0 2560 0 set-rot
    this-one 10 0 anim
    this-one anim-hold
    this-one wait-anim
    L-800985E4
    L-800985E4
    L-800985E4
    L-800985E4
    L-800985E4
    this-one 12 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 13 3 set-floor
    end-script
    ;

: L-80098824
    this-one 278512 98080 walk-to  this-one wait-stop
    L-800985FC
    this-one 275024 94736 walk-to  this-one wait-stop
    L-8009866C
    this-one 271712 91232 walk-to  this-one wait-stop
    end-script
    ;

\ $80084EEC
: route-80084EEC
    isabella as-this-one
    L-80098524
    this-one 163 360 attach
    this-one wait-follow
    L-80098824
    this-one arrive
    end-script
    ;

: L-800986DC
    this-one 7 0 anim
    this-one anim-hold
    this-one wait-anim
    end-script
    ;

: L-800986F4
    this-one 0 512 0 set-rot
    this-one 6 0 anim
    this-one anim-hold
    this-one wait-anim
    L-800986DC
    L-800986DC
    L-800986DC
    L-800986DC
    L-800986DC
    this-one 8 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 13 2 set-floor
    end-script
    ;

: L-80098764
    this-one 0 512 0 set-rot
    this-one 6 0 anim
    this-one anim-hold
    this-one wait-anim
    L-800986DC
    L-800986DC
    L-800986DC
    L-800986DC
    L-800986DC
    this-one 8 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 13 1 set-floor
    end-script
    ;

: L-800987D4
    this-one 273680 93552 walk-to  this-one wait-stop
    L-800986F4
    this-one 277328 96784 walk-to  this-one wait-stop
    L-80098764
    end-script
    ;

\ $80084F1C
: route-80084F1C
    isabella as-this-one
    L-800987D4
    L-80096D88
    this-one 162 360 attach
    this-one wait-follow
    L-800984AC
    this-one arrive
    end-script
    ;

\ $80084F54
: route-80084F54
    isabella as-this-one
    L-80098524
    this-one 143 360 attach
    this-one wait-follow
    this-one 336672 429680 walk-to  this-one wait-stop
    this-one 353072 431088 walk-to  this-one wait-stop
    this-one 378624 422416 walk-to  this-one wait-stop
    this-one 379600 424608 walk-to  this-one wait-stop
    this-one 391968 427296 walk-to  this-one wait-stop
    this-one 395904 432176 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097BB4
    this-one 333840 439472 walk-to  this-one wait-stop
    this-one 42 0 through-door
    end-script
    ;

\ $80084FAC
: route-80084FAC
    isabella as-this-one
    this-one 391968 427296 walk-to  this-one wait-stop
    this-one 379600 424608 walk-to  this-one wait-stop
    this-one 378624 422416 walk-to  this-one wait-stop
    this-one 353072 431088 walk-to  this-one wait-stop
    this-one 336672 429680 walk-to  this-one wait-stop
    L-80097B74
    L-80097BB4
    this-one 335232 440960 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097BCC
    this-one 333792 439904 walk-to  this-one wait-stop
    this-one 41 0 through-door
    end-script
    ;

: L-80097BE4
    L-80097BCC
    this-one 333184 437120 walk-to  this-one wait-stop
    this-one 40 0 through-door
    end-script
    ;

: L-800989CC
    this-one 335104 275152 walk-to  this-one wait-stop
    end-script
    ;

: L-800989B4
    this-one 344528 274976 walk-to  this-one wait-stop
    end-script
    ;

\ $80084FFC
: route-80084FFC
    isabella as-this-one
    L-80097BE4
    L-80096D88
    this-one 169 360 attach
    this-one wait-follow
    L-800989CC
    L-800989B4
    L-80097F0C
    this-one 345088 285696 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097EF4
    this-one 345040 285296 walk-to  this-one wait-stop
    this-one 15 3 through-door
    end-script
    ;

\ $8008504C
: route-8008504C
    isabella as-this-one
    L-80097EF4
    L-800989B4
    L-80096D88
    this-one 178 360 attach
    this-one wait-follow
    L-800984AC
    this-one arrive
    end-script
    ;

\ $8008508C
: route-8008508C
    isabella arrive
    end-script
    ;

: L-800971AC
    this-one 102448 352144 walk-to  this-one wait-stop
    this-one 19 3 through-door
    this-one 101600 352320 walk-to  this-one wait-stop
    L-8009709C
    end-script
    ;

\ $80085388
: route-80085388
    james as-this-one
    sub-80096D74
    L-800971AC
    L-800971D4
    this-one 105056 360112 walk-to  this-one wait-stop
    this-one 0 2083 0 set-rot
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $800853D0
: route-800853D0
    james as-this-one
    this-one 104592 358768 walk-to  this-one wait-stop
    L-80097118
    L-800971E8
    L-80097318
    this-one 99440 350128 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097130
    this-one 101568 358720 walk-to  this-one wait-stop
    this-one 5004 0 turn-to
    300 frames wait
    goto L-8009709C
    ;

: L-8009717C
    this-one 101552 353296 walk-to  this-one wait-stop
    this-one 102160 352208 walk-to  this-one wait-stop
    this-one 1186 0 turn-to
    this-one wait-turned
    this-one 19 0 through-door
    end-script
    ;

: L-80085364
    L-8009717C
    this-one 103760 352080 walk-to  this-one wait-stop
    this-one 400 0 turn-to
    goto sub-80096D34
    ;

\ $80085408
: route-80085408
    james as-this-one
    sub-80096D74
    this-one 99760 351024 walk-to  this-one wait-stop
    60 frames wait
    L-80097338
    this-one 101280 357344 walk-to  this-one wait-stop
    this-one -976 -250 turn-to
    this-one wait-turned
    100 frames wait
    this-one 101168 355520 walk-to  this-one wait-stop
    this-one 100048 355424 walk-to  this-one wait-stop
    this-one 100016 355984 walk-to  this-one wait-stop
    this-one -749 -200 turn-to
    this-one wait-turned
    250 frames wait
    L-800973B4
    L-80097130
    L-80085364
    this-one arrive
    end-script
    ;

\ $800854A0
: route-800854A0
    james as-this-one
    sub-80096D74
    this-one 104592 358768 walk-to  this-one wait-stop
    L-80097118
    L-80085364
    this-one arrive
    end-script
    ;

: L-80097560
    L-800971E8
    this-one 101200 355936 walk-to  this-one wait-stop
    this-one -646 0 turn-to
    100 frames wait
    this-one 101312 355280 walk-to  this-one wait-stop
    this-one 99072 355408 walk-to  this-one wait-stop
    this-one 21 0 through-door
    this-one 99168 357584 walk-to  this-one wait-stop
    this-one 31 0 turn-to
    end-script
    ;

\ $800854D0
: route-800854D0
    james as-this-one
    L-800971AC
    L-80097560
    this-one arrive
    end-script
    ;

: L-800975B8
    this-one 99040 355776 walk-to  this-one wait-stop
    this-one 21 2 through-door
    goto L-800970A8
    ;

\ $800854F0
: route-800854F0
    james as-this-one
    L-800975B8
    this-one 101184 356608 walk-to  this-one wait-stop
    this-one -930 -200 turn-to
    this-one wait-turned
    this-one 101664 355040 walk-to  this-one wait-stop
    L-800973B4
    L-80097550
    L-80096D88
    this-one 100 360 attach
    this-one wait-follow
    L-80097780
    L-8009778C
    this-one 466848 290448 walk-to  this-one wait-stop
    this-one 467024 290992 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80085570
: route-80085570
    james as-this-one
    L-800977B0
    L-80096D88
    this-one 114 360 attach
    this-one wait-follow
    this-one 391632 416896 walk-to  this-one wait-stop
    this-one 30 -2 through-door
    this-one 392129 0 414862 set-pos
    this-one arrive
    end-script
    ;

\ $800855C4
: route-800855C4
    james as-this-one
    L-80096D88
    this-one 132 360 attach
    this-one wait-follow
    L-80097520
    L-80097560
    this-one arrive
    end-script
    ;

\ $800855FC
: route-800855FC
    james as-this-one
    james 6 actor-model
    L-800975B8
    L-800973B4
    L-80085364
    this-one arrive
    end-script
    ;

\ $80085630
: route-80085630
    james as-this-one
    L-800971AC
    L-80097550
    L-80096D88
    james 187 actor-model
    this-one 133 360 attach
    this-one wait-follow
    L-80097C04
    L-80097C10
    L-80097C54
    this-one 383712 147584 walk-to  this-one wait-stop
    this-one 388720 147760 walk-to  this-one wait-stop
    this-one -15 -250 turn-to
    this-one wait-turned
    this-one arrive
    end-script
    ;

\ $800856AC
: route-800856AC
    james as-this-one
    this-one 389424 146992 walk-to  this-one wait-stop
    this-one 389472 144480 walk-to  this-one wait-stop
    this-one 383776 144448 walk-to  this-one wait-stop
    this-one 383936 141712 walk-to  this-one wait-stop
    L-80097C34
    L-80097C74
    L-80096D88
    this-one 130 360 attach
    this-one wait-follow
    L-80097520
    L-80085364
    this-one arrive
    end-script
    ;

\ $80085714
: route-80085714
    james as-this-one
    L-80096D88
    this-one 174 360 attach
    this-one wait-follow
    L-80097F0C
    this-one 345536 285280 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008574C
: route-8008574C
    james as-this-one
    L-80097EF4
    L-80096D88
    this-one 170 360 attach
    this-one wait-follow
    L-80097694
    this-one 44624 415952 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097754
    this-one 3072 0 turn-to
    this-one 30 walk-forward
    50 frames wait
    this-one idle
    this-one 129424 310720 walk-to  this-one wait-stop
    end-script
    ;

\ $8008578C
: route-8008578C
    james as-this-one
    L-8009770C
    L-80096D88
    this-one 175 360 attach
    this-one wait-follow
    L-80097754
    this-one 122976 310336 walk-to  this-one wait-stop
    this-one 122880 312576 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800857D4
: route-800857D4
    james as-this-one
    L-80097748
    L-80096D88
    this-one 85 360 attach
    this-one wait-follow
    L-80097628
    this-one 593040 132256 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80085814
: route-80085814
    james as-this-one
    L-80097600
    this-one 124 360 attach
    this-one wait-follow
    this-one 129 360 attach
    this-one wait-follow
    this-one 102416 53168 walk-to  this-one wait-stop
    this-one 112608 51280 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008585C
: route-8008585C
    james as-this-one
    this-one 112617 0 51293 set-pos
    this-one 102416 53168 walk-to  this-one wait-stop
    L-80096D88
    this-one 176 360 attach
    this-one wait-follow
    L-80097694
    this-one 44624 415952 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097D10
    sub-80096D74
    this-one 383728 150336 walk-to  this-one wait-stop
    this-one 383376 148816 walk-to  this-one wait-stop
    this-one 59 2 through-door
    this-one 382304 148528 walk-to  this-one wait-stop
    this-one 382112 140976 walk-to  this-one wait-stop
    end-script
    ;

\ $80086704
: route-80086704
    lidds as-this-one
    L-80097D10
    L-80097C74
    L-80096D88
    this-one 31 360 attach
    this-one wait-follow
    L-80097628
    this-one 592736 132224 walk-to  this-one wait-stop
    this-one 592752 131808 walk-to  this-one wait-stop
    this-one 1009 -150 turn-to
    this-one wait-turned
    sub-80096D34
    this-one arrive
    end-script
    ;

: L-800975D4
    this-one 98640 350224 walk-to  this-one wait-stop
    this-one 98640 349776 walk-to  this-one wait-stop
    this-one 1033 -200 turn-to
    this-one wait-turned
    sub-80096D34
    end-script
    ;

\ $8008676C
: route-8008676C
    lidds as-this-one
    this-one 592736 132224 walk-to  this-one wait-stop
    L-80097600
    this-one 36 360 attach
    this-one wait-follow
    L-80097520
    L-800971E8
    L-80097318
    L-800975D4
    this-one arrive
    end-script
    ;

\ $800867BC
: route-800867BC
    lidds as-this-one
    this-one 98643 0 349787 set-pos
    this-one 29 2 set-floor
    sub-80096D74
    L-80097338
    L-800973B4
    L-800971D4
    L-800970EC
    L-80096D88
    this-one 61 360 attach
    this-one wait-follow
    this-one 278320 98576 walk-to  this-one wait-stop
    L-800985FC
    this-one 274432 95168 walk-to  this-one wait-stop
    L-8009866C
    this-one 269280 93008 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-800866C8
    this-one 337360 65456 walk-to  this-one wait-stop
    this-one 3086 0 turn-to
    goto L-80096D04
    ;

\ $80086850
: route-80086850
    lidds as-this-one
    L-800987D4
    L-80096D88
    this-one 62 360 attach
    this-one wait-follow
    L-800866C8
    this-one arrive
    end-script
    ;

: L-800866E4
    this-one 337014 0 65459 set-pos
    sub-80096D1C
    goto L-80096D88
    ;

: L-80097C8C
    this-one 382304 148528 walk-to  this-one wait-stop
    this-one 383072 148656 walk-to  this-one wait-stop
    this-one 59 0 through-door
    end-script
    ;

: L-80097CAC
    this-one 383696 150384 walk-to  this-one wait-stop
    this-one 384160 150352 walk-to  this-one wait-stop
    this-one -2063 0 turn-to
    this-one 72 1 anim
    this-one 384165 0 150357 set-pos
    end-script
    ;

: L-80097D48
    L-80097C04
    L-80097C10
    L-80097C8C
    goto L-80097CAC
    ;

\ $80086888
: route-80086888
    lidds as-this-one
    L-800866E4
    L-80096D88
    this-one 98 360 attach
    this-one wait-follow
    L-80097D48
    this-one arrive
    end-script
    ;

\ $800868C0
: route-800868C0
    lidds as-this-one
    L-80097D10
    L-80097C74
    L-80096D88
    this-one 125 360 attach
    this-one wait-follow
    this-one 131056 309376 walk-to  this-one wait-stop
    this-one 132544 317264 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80086908
: route-80086908
    lidds as-this-one
    L-80097748
    L-80096D88
    this-one 126 360 attach
    this-one wait-follow
    L-80097D48
    this-one arrive
    end-script
    ;

\ $80086940
: route-80086940
    lidds arrive
    end-script
    ;

\ $80086948
: route-80086948
    lidds as-this-one
    L-80097D10
    L-80097C54
    this-one 385040 142832 walk-to  this-one wait-stop
    this-one 388224 142992 walk-to  this-one wait-stop
    this-one 388432 144832 walk-to  this-one wait-stop
    this-one 383488 144656 walk-to  this-one wait-stop
    this-one 383600 146096 walk-to  this-one wait-stop
    this-one 387552 146080 walk-to  this-one wait-stop
    this-one 387536 147456 walk-to  this-one wait-stop
    this-one 383520 147328 walk-to  this-one wait-stop
    L-80096EF4
    300 frames wait
    this-one 389568 146864 walk-to  this-one wait-stop
    this-one 389440 144512 walk-to  this-one wait-stop
    this-one 388528 144080 walk-to  this-one wait-stop
    this-one 388560 141680 walk-to  this-one wait-stop
    this-one 389600 141424 walk-to  this-one wait-stop
    this-one 2102 200 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $800869F4
: route-800869F4
    lidds as-this-one
    sub-80096D74
    this-one 388704 141616 walk-to  this-one wait-stop
    this-one 387472 143520 walk-to  this-one wait-stop
    L-80096EF4
    300 frames wait
    this-one 386256 142784 walk-to  this-one wait-stop
    this-one 384208 142704 walk-to  this-one wait-stop
    L-80097C34
    L-80097C8C
    L-80097CAC
    this-one arrive
    end-script
    ;

\ $80086A54
: route-80086A54
    lidds as-this-one
    L-80097D10
    L-80097C74
    L-80096D88
    this-one 130 360 attach
    this-one wait-follow
    L-80097520
    L-800971E8
    L-80097318
    L-800975D4
    this-one arrive
    end-script
    ;

\ $80086AAC
: route-80086AAC
    lidds as-this-one
    L-800866E4
    L-80096D88
    this-one 134 360 attach
    this-one wait-follow
    this-one 278320 98576 walk-to  this-one wait-stop
    L-800985FC
    this-one 274432 95168 walk-to  this-one wait-stop
    L-8009866C
    this-one 269280 93008 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80086B04
: route-80086B04
    lidds as-this-one
    L-800987D4
    L-80096D88
    this-one 135 360 attach
    this-one wait-follow
    L-80097D48
    this-one arrive
    end-script
    ;

\ $80086B3C
: route-80086B3C
    lidds as-this-one
    sub-80096D74
    L-80097338
    L-800973B4
    L-800971D4
    L-800970EC
    L-80096D88
    this-one 137 360 attach
    this-one wait-follow
    L-80097A34
    this-one 250144 135360 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80086B9C
: route-80086B9C
    lidds as-this-one
    L-800979F4
    this-one 138 360 attach
    this-one wait-follow
    L-800866C8
    this-one arrive
    end-script
    ;

\ $80086BCC
: route-80086BCC
    lidds as-this-one
    L-80097D10
    L-80097C74
    L-80096D88
    this-one 158 360 attach
    this-one wait-follow
    this-one 160 360 attach
    this-one wait-follow
    this-one 98224 53072 walk-to  this-one wait-stop
    this-one 94800 54992 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80086C24
: route-80086C24
    lidds as-this-one
    this-one 98224 53072 walk-to  this-one wait-stop
    L-80096D88
    this-one 105 360 attach
    this-one wait-follow
    this-one 157 360 attach
    this-one wait-follow
    L-80097D48
    this-one arrive
    end-script
    ;

\ $80086C6C
: route-80086C6C
    lidds as-this-one
    L-80097D10
    L-80097C74
    L-80096D88
    this-one 158 360 attach
    this-one wait-follow
    this-one 18 360 attach
    this-one wait-follow
    L-80097F0C
    this-one 345616 286240 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80086CC4
: route-80086CC4
    lidds as-this-one
    L-80097EF4
    L-80096D88
    this-one 177 360 attach
    this-one wait-follow
    L-80097520
    this-one 101968 355440 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800870B4
: route-800870B4
    mel as-this-one
    L-800978B8
    L-800978F0
    L-80097968
    L-80096D88
    this-one 24 400 attach
    mel arrive
    end-script
    ;

\ $80087160
: route-80087160
    mel as-this-one
    this-one 30 partner-command
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    mel arrive
    end-script
    ;

: L-80087050
    mel as-this-one
    this-one 567232 214256 walk-to  this-one wait-stop
    this-one 572352 218608 walk-to  this-one wait-stop
    this-one 570768 215088 walk-to  this-one wait-stop
    this-one 572352 218608 walk-to  this-one wait-stop
    this-one 578896 218800 walk-to  this-one wait-stop
    this-one 579200 223904 walk-to  this-one wait-stop
    this-one 572720 224112 walk-to  this-one wait-stop
    this-one 572992 221440 walk-to  this-one wait-stop
    this-one 6635 0 turn-to
    this-one 22 1 anim
    end-script
    ;

\ $80087194
: route-80087194
    mel as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 20 400 attach
    this-one wait-follow
    L-80087050
    mel arrive
    end-script
    ;

\ $800871D4
: route-800871D4
    mel as-this-one
    L-80096D88
    this-one 28 400 attach
    this-one wait-follow
    L-80097780
    L-8009778C
    this-one -1 1 69 act-command
    L-80096FD8
    this-one arrive
    end-script
    ;

: L-8008721C
    mel as-this-one
    L-80096D88
    this-one 63 400 attach
    this-one wait-follow
    this-one 29 400 attach
    this-one arrive
    end-script
    ;

\ $80087268
: route-80087268
    mel as-this-one
    L-800977F0
    goto L-8008721C
    ;

: L-80098850
    this-one 95024 47696 walk-to  this-one wait-stop
    this-one 89584 47872 walk-to  this-one wait-stop
    this-one -16 0 turn-to
    end-script
    ;

: L-80098870
    L-80098850
    this-one 36 -2 through-door
    this-one 89568 48816 walk-to  this-one wait-stop
    end-script
    ;

\ $80087280
: route-80087280
    mel as-this-one
    this-one 30 partner-command
    this-one wait-follow
    this-one 64 400 attach
    this-one wait-follow
    L-80098870
    this-one arrive
    end-script
    ;

: L-80098890
    this-one 95024 47696 walk-to  this-one wait-stop
    this-one 97408 53200 walk-to  this-one wait-stop
    L-80096EF4
    200 frames wait
    goto L-80096D88
    ;

: L-800988B8
    this-one 89491 0 48245 set-pos
    this-one 6128 0 turn-to
    this-one 36 0 through-door
    goto L-80098890
    ;

\ $800872B4
: route-800872B4
    mel as-this-one
    L-800988B8
    L-80096D88
    this-one 102 400 attach
    this-one wait-follow
    this-one 174064 422432 walk-to  this-one wait-stop
    this-one 378 0 turn-to
    L-80096EF4
    100 frames wait
    this-one 174656 425984 walk-to  this-one wait-stop
    this-one -1278 -300 turn-to
    this-one wait-turned
    10 frames wait
    this-one 1042 -300 turn-to
    this-one wait-turned
    this-one 177008 426320 walk-to  this-one wait-stop
    this-one 2058 0 turn-to
    this-one arrive
    end-script
    ;

\ $8008734C
: route-8008734C
    mel as-this-one
    this-one 178496 426208 walk-to  this-one wait-stop
    10 frames wait
    this-one 17 18 1 anim-then
    20 frames wait
    this-one 179957 426096 run-to
    this-one wait-stop
    this-one 179855 419963 run-to
    this-one wait-stop
    this-one 177120 419238 run-to
    this-one wait-stop
    L-80096D88
    this-one 103 400 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $800873E4
: route-800873E4
    mel as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 20 400 attach
    this-one wait-follow
    L-80087050
    this-one arrive
    end-script
    ;

\ $80087424
: route-80087424
    mel as-this-one
    L-80096D88
    this-one 112 400 attach
    this-one wait-follow
    this-one 38 400 attach
    this-one arrive
    end-script
    ;

\ $80087458
: route-80087458
    mel as-this-one
    this-one 39 partner-command
    this-one wait-follow
    this-one 113 400 attach
    this-one wait-follow
    this-one 170257 426218 run-to
    this-one wait-stop
    this-one 0 0 0 set-pos
    this-one arrive
    end-script
    ;

: L-800874A4
    mel as-this-one
    L-80096D88
    this-one 20 400 attach
    this-one wait-follow
    L-80087050
    this-one arrive
    end-script
    ;

\ $800874D4
: route-800874D4
    mel as-this-one
    this-one 328848 269584 walk-to  this-one wait-stop
    L-80098060
    goto L-800874A4
    ;

\ $800874F4
: route-800874F4
    mel as-this-one
    this-one 168994 0 425555 set-pos
    this-one 169232 417392 walk-to  this-one wait-stop
    this-one 175200 417488 walk-to  this-one wait-stop
    L-80096D88
    this-one 104 400 attach
    this-one wait-follow
    L-80098870
    this-one arrive
    end-script
    ;

\ $80087544
: route-80087544
    mel as-this-one
    L-80096D88
    this-one 101 400 attach
    this-one wait-follow
    this-one arrive
    end-script
    ;

: L-8008756C
    mel as-this-one
    this-one 128 400 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $800875A4
: route-800875A4
    mel as-this-one
    L-80096D88
    goto L-8008756C
    ;

\ $800875BC
: route-800875BC
    mel as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 74 400 attach
    this-one wait-follow
    L-80097780
    L-8009778C
    this-one -1 1 69 act-command
    L-80096FD8
    this-one arrive
    end-script
    ;

\ $80087614
: route-80087614
    mel as-this-one
    L-800977F0
    L-80096D88
    this-one 129 400 attach
    this-one wait-follow
    L-80098870
    this-one arrive
    end-script
    ;

\ $8008764C
: route-8008764C
    mel as-this-one
    L-800988B8
    this-one 105 400 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $8008768C
: route-8008768C
    mel as-this-one
    L-800988B8
    this-one 105 400 attach
    this-one wait-follow
    L-80098048
    this-one 328864 271648 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-800981A8
    this-one 564864 215360 walk-to  this-one wait-stop
    this-one 571152 215216 walk-to  this-one wait-stop
    this-one 573056 223664 walk-to  this-one wait-stop
    this-one 576240 223616 walk-to  this-one wait-stop
    this-one 2067 0 turn-to
    this-one 104 -2 through-door
    this-one 1 1 1 set-pos
    end-script
    ;

\ $800876C4
: route-800876C4
    mel as-this-one
    L-800988B8
    this-one 118 400 attach
    this-one wait-follow
    L-800981A8
    this-one arrive
    end-script
    ;

\ $800876F4
: route-800876F4
    mel arrive
    end-script
    ;

: L-80097B54
    this-one 249312 132368 walk-to  this-one wait-stop
    L-80097A44
    L-800978F0
    goto L-800979F4
    ;

\ $80087720
: route-80087720
    mel as-this-one
    L-80097B54
    this-one 24 400 attach
    this-one arrive
    end-script
    ;

\ $80087744
: route-80087744
    mel as-this-one
    this-one 30 partner-command
    this-one wait-follow
    this-one 77 400 attach
    this-one wait-follow
    this-one 471833 412438 run-to
    this-one wait-stop
    this-one 473434 409699 run-to
    this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80087790
: route-80087790
    mel arrive
    end-script
    ;

\ $80087798
: route-80087798
    mel as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 109 400 attach
    this-one wait-follow
    L-80097A34
    this-one 249072 135104 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800877E0
: route-800877E0
    mel as-this-one
    L-800979F4
    this-one 17 400 attach
    this-one wait-follow
    this-one 304720 308336 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80087810
: route-80087810
    mel as-this-one
    L-80096D88
    this-one 141 400 attach
    this-one wait-follow
    this-one 106 400 attach
    this-one wait-follow
    this-one 565696 223264 walk-to  this-one wait-stop
    this-one 575312 224480 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80087858
: route-80087858
    mel as-this-one
    this-one 565696 223264 walk-to  this-one wait-stop
    L-80096D88
    this-one 28 360 attach
    this-one wait-follow
    10 frames wait
    this-one 129 360 attach
    this-one wait-follow
    L-800988E8
    this-one arrive
    end-script
    ;

\ $800878D8
: route-800878D8
    mel as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 74 400 attach
    this-one wait-follow
    L-80097780
    L-8009778C
    this-one arrive
    end-script
    ;

\ $80096E34
: sub-80096E34
    -500 -1544 -1326 0 0 0 0 camera-set
    matthew camera-at
    matthew camera-heading
    5 0 camera-turn
    end-script
    ;

\ $80087B60
: exe-80087B60
    sub-80096E34
    111 actor 10021 0 talk
    end-script
    ;

: L-80087DA4
    this-one 0 0 turn-to
    sub-80096D74
    this-one 3 0 anim
    this-one -90 -90 turn-by
    this-one wait-turned
    this-one 30 walk-forward
    13 frames wait
    this-one 75 110 turn-by
    this-one 247392 132943 200 wait-near
    this-one idle
    end-script
    ;

: L-8009788C
    this-one 251328 133360 walk-to  this-one wait-stop
    this-one 6 1 through-door
    this-one 30 walk-forward
    30 frames wait
    this-one idle
    end-script
    ;

\ $80087EC8
: route-80087EC8
    morgan as-this-one
    L-80087DA4
    L-80097874
    L-8009788C
    morgan arrive
    end-script
    ;

: L-80087E0C
    this-one 247984 130832 walk-to  this-one wait-stop
    this-one 248416 130992 walk-to  this-one wait-stop
    this-one 248376 0 131107 set-pos
    this-one 0 0 0 set-rot
    goto sub-80096D34
    ;

\ $80087EF0
: route-80087EF0
    morgan as-this-one
    L-800978B8
    L-800978D8
    L-80087E0C
    this-one arrive
    end-script
    ;

: L-80087F18
    L-80087DA4
    L-80097874
    L-800978F0
    this-one 249680 135184 walk-to  this-one wait-stop
    this-one 14 15 anim-at  this-one wait-anim
    10 frames wait
    this-one 16 0 anim
    this-one wait-anim
    L-80097968
    L-80096D88
    end-script
    ;

: L-80087E84
    morgan as-this-one
    L-80097628
    this-one 10 walk-forward
    5 frames wait
    this-one idle
    this-one 591334 0 132369 set-pos
    450 frames wait
    morgan arrive
    end-script
    ;

\ $80087F74
: route-80087F74
    morgan as-this-one
    L-80087F18
    this-one 3 360 attach
    this-one wait-follow
    L-80087E84
    end-script
    ;

\ $80087FA0
: route-80087FA0
    morgan as-this-one
    this-one 594528 132848 walk-to  this-one wait-stop
    this-one 593760 132832 walk-to  this-one wait-stop
    L-80097600
    morgan 8 360 attach
    this-one wait-follow
    this-one 57728 411472 walk-to  this-one wait-stop
    this-one 57184 413984 walk-to  this-one wait-stop
    this-one 54736 416352 walk-to  this-one wait-stop
    morgan arrive
    end-script
    ;

\ $80087FF0
: route-80087FF0
    morgan as-this-one
    this-one 52928 415856 walk-to  this-one wait-stop
    L-800976F8
    morgan arrive
    end-script
    ;

\ $80088010
: route-80088010
    morgan as-this-one
    L-8009770C
    L-80096D88
    morgan 12 360 attach
    this-one wait-follow
    this-one 45856 277888 walk-to  this-one wait-stop
    morgan arrive
    end-script
    ;

: L-80087E44
    morgan as-this-one
    L-80097980
    L-80097994
    this-one 250496 135312 walk-to  this-one wait-stop
    100 frames wait
    L-800979B4
    L-800978D8
    goto L-80087E0C
    ;

\ $80088048
: route-80088048
    morgan as-this-one
    L-800985AC
    L-80098534
    morgan 14 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $80088080
: route-80088080
    morgan as-this-one
    L-80087F18
    this-one 18 360 attach
    this-one wait-follow
    this-one 342448 287536 walk-to  this-one wait-stop
    this-one 342560 286320 walk-to  this-one wait-stop
    this-one 345728 284336 walk-to  this-one wait-stop
    this-one 347600 284304 walk-to  this-one wait-stop
    this-one 349280 286016 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800880D0
: route-800880D0
    morgan as-this-one
    this-one 347600 284304 walk-to  this-one wait-stop
    this-one 345728 284336 walk-to  this-one wait-stop
    this-one 342560 286320 walk-to  this-one wait-stop
    L-80096D88
    this-one 19 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

: L-8009811C
    this-one 467632 177792 walk-to  this-one wait-stop
    this-one 5086 0 turn-to
    this-one 468544 177840 walk-to  this-one wait-stop
    this-one 99 -2 through-door
    this-one 470000 177904 walk-to  this-one wait-stop
    end-script
    ;

\ $80088118
: route-80088118
    morgan as-this-one
    L-80087F18
    this-one 21 360 attach
    this-one wait-follow
    L-8009811C
    morgan arrive
    end-script
    ;

: L-80098150
    this-one 468800 177808 walk-to  this-one wait-stop
    this-one 99 0 through-door
    this-one 468080 177824 walk-to  this-one wait-stop
    goto L-80096D88
    ;

\ $80088148
: route-80088148
    morgan as-this-one
    L-80098150
    this-one 25 360 attach
    this-one wait-follow
    L-80097694
    this-one 44448 415296 walk-to  this-one wait-stop
    morgan arrive
    end-script
    ;

\ $80088180
: route-80088180
    morgan as-this-one
    L-800976F8
    morgan arrive
    end-script
    ;

\ $80088198
: route-80088198
    morgan as-this-one
    L-8009770C
    L-80096D88
    morgan 26 360 attach
    this-one wait-follow
    L-80097A34
    L-800979B4
    L-8009788C
    this-one arrive
    end-script
    ;

: L-80098174
    this-one 364352 289872 walk-to  this-one wait-stop
    this-one 0 200 turn-to
    this-one wait-turned
    end-script
    ;

\ $800881E0
: route-800881E0
    morgan as-this-one
    L-80087F18
    this-one 49 360 attach
    this-one wait-follow
    L-80098174
    this-one arrive
    end-script
    ;

: L-80098190
    this-one 364165 0 289848 set-pos
    goto L-80096D88
    ;

\ $800882E8
: route-800882E8
    morgan as-this-one
    L-80098190
    this-one 57 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $80088318
: route-80088318
    morgan as-this-one
    L-80098890
    this-one 107 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

: L-80087D44
    sub-80096D74
    this-one 3 0 anim
    this-one -90 -90 turn-by
    this-one wait-turned
    this-one 30 walk-forward
    13 frames wait
    this-one 75 110 turn-by
    this-one 247392 132943 200 wait-near
    this-one idle
    goto L-80097A0C
    ;

\ $80088348
: route-80088348
    morgan as-this-one
    L-80087D44
    this-one 108 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $80088388
: route-80088388
    morgan as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 109 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $800883C8
: route-800883C8
    morgan as-this-one
    L-80098060
    L-80096D88
    this-one 109 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

: L-80097EB4
    this-one 175616 421232 walk-to  this-one wait-stop
    this-one 175648 423280 walk-to  this-one wait-stop
    this-one 27 -2 through-door
    end-script
    ;

\ $80088400
: route-80088400
    morgan as-this-one
    L-80087D44
    this-one 40 360 attach
    this-one wait-follow
    L-80097EB4
    this-one 177120 423600 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097ED4
    this-one 175584 423504 walk-to  this-one wait-stop
    this-one 27 0 through-door
    this-one 175680 422016 walk-to  this-one wait-stop
    end-script
    ;

: L-80088438
    morgan as-this-one
    L-80097ED4
    L-80096D88
    this-one 44 360 attach
    this-one wait-follow
    this-one 23 360 attach
    this-one arrive
    end-script
    ;

\ $80088474
: route-80088474
    morgan as-this-one
    L-80097ED4
    goto L-80088438
    ;

\ $8008848C
: route-8008848C
    morgan as-this-one
    this-one 55 partner-command
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $800884B0
: route-800884B0
    morgan as-this-one
    L-80098150
    this-one 22 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $800884E0
: route-800884E0
    morgan as-this-one
    L-80087D44
    this-one 108 360 attach
    this-one wait-follow
    L-80098048
    this-one 328400 271120 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80088518
: route-80088518
    morgan as-this-one
    L-80087D44
    this-one 160 360 attach
    this-one wait-follow
    L-80098850
    this-one 65 1 anim
    50 frames wait
    this-one 0 1 anim
    50 frames wait
    this-one 92624 47520 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80088578
: route-80088578
    morgan as-this-one
    L-80098150
    this-one 22 360 attach
    this-one wait-follow
    10 frames wait
    this-one 108 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $800885D0
: route-800885D0
    morgan as-this-one
    L-80087D44
    this-one 122 360 attach
    this-one wait-follow
    50 frames wait
    this-one 106 360 attach
    this-one wait-follow
    L-80097DC4
    this-one 576704 214640 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80088620
: route-80088620
    morgan as-this-one
    this-one 567312 214480 walk-to  this-one wait-stop
    L-80096D88
    this-one 28 360 attach
    this-one wait-follow
    50 frames wait
    this-one 164 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $80088670
: route-80088670
    morgan as-this-one
    L-80087D44
    this-one 160 360 attach
    this-one wait-follow
    this-one 99184 50160 walk-to  this-one wait-stop
    this-one 97312 52496 walk-to  this-one wait-stop
    this-one 92656 53184 walk-to  this-one wait-stop
    this-one 65 0 anim
    30 frames wait
    this-one 65 0 anim
    50 frames wait
    this-one 65 0 anim
    200 frames wait
    this-one 96032 52288 walk-to  this-one wait-stop
    this-one 93824 46672 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80098948
    this-one 92736 53216 walk-to  this-one wait-stop
    300 frames wait
    this-one 95008 52224 walk-to  this-one wait-stop
    this-one 94976 48592 walk-to  this-one wait-stop
    this-one 91712 46976 walk-to  this-one wait-stop
    this-one 84768 47744 walk-to  this-one wait-stop
    this-one 85056 49552 walk-to  this-one wait-stop
    300 frames wait
    this-one 84976 59600 walk-to  this-one wait-stop
    this-one 93712 59616 walk-to  this-one wait-stop
    this-one 99312 50048 walk-to  this-one wait-stop
    goto L-80096D88
    ;

\ $800886FC
: route-800886FC
    morgan as-this-one
    L-80098948
    this-one 107 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $8008872C
: route-8008872C
    morgan as-this-one
    L-80087D44
    this-one 122 360 attach
    this-one wait-follow
    L-80097780
    L-8009778C
    this-one -1 1 69 act-command
    L-80096FD8
    this-one arrive
    end-script
    ;

\ $80088774
: route-80088774
    morgan as-this-one
    L-800977F0
    L-80096D88
    this-one 164 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $800887AC
: route-800887AC
    morgan as-this-one
    L-80097ED4
    L-80096D88
    this-one 173 360 attach
    this-one wait-follow
    L-80097694
    this-one arrive
    end-script
    ;

: L-800887E4
    L-80087D44
    this-one 136 360 attach
    this-one wait-follow
    L-80097520
    goto L-800971E8
    ;

\ $8008880C
: route-8008880C
    morgan as-this-one
    L-800887E4
    L-80097538
    this-one 100544 359424 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80088834
: route-80088834
    morgan as-this-one
    L-800974E8
    L-800973B4
    L-80097550
    L-80096D88
    this-one 137 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $8008887C
: route-8008887C
    morgan as-this-one
    L-80096D88
    this-one 161 360 attach
    this-one wait-follow
    this-one 464080 292880 walk-to  this-one wait-stop
    this-one 464064 289056 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800888B4
: route-800888B4
    morgan as-this-one
    this-one 464080 292880 walk-to  this-one wait-stop
    L-80096D88
    this-one 164 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $800888EC
: route-800888EC
    morgan as-this-one
    L-8009770C
    L-80096D88
    morgan 26 360 attach
    this-one wait-follow
    L-80087E44
    this-one arrive
    end-script
    ;

\ $80088924
: route-80088924
    morgan as-this-one
    L-80087D44
    this-one 18 360 attach
    this-one wait-follow
    50 frames wait
    this-one 170 360 attach
    this-one wait-follow
    L-80097670
    this-one arrive
    end-script
    ;

\ $80088B04
: route-80088B04
    morgan as-this-one
    L-80098150
    this-one 22 360 attach
    this-one wait-follow
    50 frames wait
    this-one 40 360 attach
    this-one wait-follow
    L-80097EB4
    this-one 177120 423600 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80098C74
    this-one 379824 69616 walk-to  this-one wait-stop
    end-script
    ;

: L-8008B334
    this-one 377933 64799 run-to
    this-one wait-stop
    this-one 373522 64683 run-to
    this-one wait-stop
    this-one 373658 73264 run-to
    this-one wait-stop
    this-one 377876 73339 run-to
    this-one wait-stop
    end-script
    ;

: L-80096F08
    this-one 29 30 1 anim-then
    50 frames wait
    end-script
    ;

: L-8008B378
    L-8008B334
    L-8008B334
    L-8008B334
    L-80096F08
    500 frames wait
    L-80096EF4
    200 frames wait
    end-script
    ;

\ $8008B3B4
: route-8008B3B4
    65 actor as-this-one
    L-80098C74
    L-80096EF4
    400 frames wait
    L-8008B378
    L-8008B378
    L-8008B378
    L-80098C74
    L-80096EF4
    100 frames wait
    this-one arrive
    end-script
    ;

: L-80098BCC
    this-one 380064 100368 walk-to  this-one wait-stop
    end-script
    ;

: L-80098C38
    this-one 380080 59472 walk-to  this-one wait-stop
    end-script
    ;

: L-80098C2C
    this-one 365216 59472 walk-to  this-one wait-stop
    end-script
    ;

: L-80098CB4
    this-one 365154 100382 run-to
    this-one wait-stop
    end-script
    ;

: L-80098CA0
    this-one 340089 100326 run-to
    this-one wait-stop
    end-script
    ;

: L-80098C08
    this-one 340160 59488 walk-to  this-one wait-stop
    end-script
    ;

: L-80098BFC
    this-one 325216 59520 walk-to  this-one wait-stop
    end-script
    ;

: L-80098B90
    this-one 325184 100320 walk-to  this-one wait-stop
    end-script
    ;

: L-80098C8C
    this-one 280149 100288 run-to
    this-one wait-stop
    end-script
    ;

: L-80098B84
    this-one 320144 100288 walk-to  this-one wait-stop
    end-script
    ;

: L-80098BF0
    this-one 320176 59504 walk-to  this-one wait-stop
    end-script
    ;

: L-80098CC8
    this-one 340167 59492 run-to
    this-one wait-stop
    end-script
    ;

: L-80098BA8
    this-one 345152 100368 walk-to  this-one wait-stop
    end-script
    ;

: L-80098C50
    this-one 345184 125344 walk-to  this-one wait-stop
    end-script
    ;

: L-80098C5C
    this-one 380112 125392 walk-to  this-one wait-stop
    end-script
    ;

: L-8008B128
    L-80098C38
    L-80098C2C
    L-80098CB4
    L-80098CA0
    L-80096F08
    L-80096EF4
    500 frames wait
    L-80098C08
    L-80098BFC
    L-80098B90
    L-80098C8C
    L-80096F08
    L-80096EF4
    500 frames wait
    L-80098B84
    L-80096EF4
    200 frames wait
    L-80098BF0
    L-80098CC8
    L-80098CA0
    L-80098BA8
    L-80098C50
    L-80098C5C
    L-80098BCC
    end-script
    ;

\ $8008B40C
: route-8008B40C
    65 actor as-this-one
    L-80098BCC
    L-80096EF4
    500 frames wait
    L-8008B128
    L-80098C74
    this-one 376368 69696 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008B44C
: route-8008B44C
    65 actor as-this-one
    this-one 376368 69696 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-800989C0
    this-one 322704 284064 walk-to  this-one wait-stop
    end-script
    ;

: L-80098B08
    this-one 322734 0 287201 set-pos
    this-one 322720 285312 walk-to  this-one wait-stop
    40 frames wait
    L-800989C0
    end-script
    ;

: L-80098A20
    this-one 344320 284384 walk-to  this-one wait-stop
    end-script
    ;

: L-800989A8
    this-one 373920 275232 walk-to  this-one wait-stop
    end-script
    ;

: L-80098A08
    this-one 374064 245264 walk-to  this-one wait-stop
    end-script
    ;

: L-800989FC
    this-one 344144 245296 walk-to  this-one wait-stop
    end-script
    ;

: L-80098AB0
    this-one 344328 284391 run-to
    this-one wait-stop
    end-script
    ;

: L-80098A2C
    this-one 344176 314144 walk-to  this-one wait-stop
    end-script
    ;

: L-80098A44
    this-one 335104 314176 walk-to  this-one wait-stop
    end-script
    ;

: L-80098A68
    this-one 335120 284224 walk-to  this-one wait-stop
    end-script
    ;

: L-80098A88
    this-one 335109 275161 run-to
    this-one wait-stop
    end-script
    ;

: L-800989F0
    this-one 335152 245264 walk-to  this-one wait-stop
    end-script
    ;

: L-800989E4
    this-one 305152 245312 walk-to  this-one wait-stop
    end-script
    ;

: L-800989D8
    this-one 305232 275264 walk-to  this-one wait-stop
    end-script
    ;

: L-80098AC4
    this-one 305211 284257 run-to
    this-one wait-stop
    end-script
    ;

: L-80089CE0
    L-800989A8
    L-80096EF4
    200 frames wait
    L-80098A08
    L-800989FC
    L-800989B4
    L-80096EF4
    400 frames wait
    L-80098AB0
    L-80098A2C
    L-80098A44
    L-80098A68
    L-80096EF4
    400 frames wait
    L-80098A88
    L-800989F0
    L-800989E4
    L-80096EF4
    200 frames wait
    L-800989D8
    L-80096EF4
    200 frames wait
    L-80098AC4
    L-80096F08
    L-80098A68
    L-80096EF4
    200 frames wait
    L-80098AB0
    L-80096F08
    goto L-800989B4
    ;

: L-80098A14
    this-one 374096 284208 walk-to  this-one wait-stop
    end-script
    ;

\ $8008B464
: route-8008B464
    66 actor as-this-one
    L-80098B08
    L-80098A20
    L-800989B4
    L-80089CE0
    L-800989A8
    L-80096EF4
    200 frames wait
    L-80098A14
    this-one 365024 284032 walk-to  this-one wait-stop
    this-one 363072 284992 walk-to  this-one wait-stop
    this-one 361632 289664 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80098A74
    this-one 344538 274981 run-to
    this-one wait-stop
    end-script
    ;

: L-80089DD0
    L-80098A88
    this-one 335520 261664 walk-to  this-one wait-stop
    L-80096EF4
    400 frames wait
    this-one 334704 263872 walk-to  this-one wait-stop
    this-one 327680 264128 walk-to  this-one wait-stop
    L-80096EF4
    100 frames wait
    this-one 48 50 1 anim-then
    600 frames wait
    this-one 335040 267728 walk-to  this-one wait-stop
    L-80098A88
    L-80096F08
    L-80096EF4
    100 frames wait
    L-800989D8
    L-800989E4
    L-80096EF4
    700 frames wait
    L-80098AC4
    L-80096F08
    L-80098A68
    L-80096EF4
    100 frames wait
    this-one 344512 285776 walk-to  this-one wait-stop
    this-one -5123 -300 turn-to
    L-80096F08
    this-one 26 27 1 anim-then
    600 frames wait
    L-80098A68
    L-80098AB0
    L-80096EF4
    100 frames wait
    L-80098A74
    L-80096EF4
    100 frames wait
    end-script
    ;

: L-80098AD8
    this-one 322736 285296 walk-to  this-one wait-stop
    this-one 0 0 turn-to
    L-80096D04
    this-one 322374 0 285236 set-pos
    end-script
    ;

\ $8008B4CC
: route-8008B4CC
    66 actor as-this-one
    this-one 359424 284448 walk-to  this-one wait-stop
    L-80098A20
    L-80098A74
    L-80089DD0
    L-80098A20
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

: L-80098A9C
    this-one 374100 284215 run-to
    this-one wait-stop
    end-script
    ;

\ $8008B514
: route-8008B514
    66 actor as-this-one
    L-80098B08
    L-80098A9C
    L-80096F08
    L-80096EF4
    200 frames wait
    this-one 360560 288960 walk-to  this-one wait-stop
    this-one 360704 292800 walk-to  this-one wait-stop
    this-one -15299 250 turn-to
    this-one wait-turned
    200 frames wait
    this-one 360721 294070 run-to
    this-one wait-stop
    this-one 367969 293904 run-to
    this-one wait-stop
    this-one 367555 284474 run-to
    this-one wait-stop
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

: L-80098A5C
    this-one 305200 284256 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A13C
    L-80098A20
    L-80098A5C
    L-80096EF4
    500 frames wait
    L-800989D8
    L-800989CC
    L-80096EF4
    200 frames wait
    L-80098A74
    goto L-80098A2C
    ;

: L-80098A50
    this-one 305232 314160 walk-to  this-one wait-stop
    end-script
    ;

\ $8008B5B4
: route-8008B5B4
    66 actor as-this-one
    L-80098B08
    L-80098A20
    L-80098A2C
    L-8008A13C
    L-80098A50
    L-80098A5C
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

: L-8008A760
    this-one 32 33 0 anim-then
    this-one wait-anim
    this-one 32 33 0 anim-then
    this-one wait-anim
    60 frames wait
    end-script
    ;

: L-8008A724
    this-one 223776 41696 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A730
    this-one 212672 39280 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A794
    this-one 65 0 anim
    this-one wait-anim
    this-one 65 0 anim
    this-one wait-anim
    end-script
    ;

: L-8008A7B8
    this-one 82 0 anim
    this-one wait-anim
    this-one 82 0 anim
    this-one wait-anim
    end-script
    ;

: L-8008A754
    this-one 201632 417120 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A73C
    this-one 189536 41712 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A748
    this-one 175376 436240 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A7DC
    this-one 225415 -4080 422226 set-pos
    this-one 2048 0 turn-to
    sub-80096D1C
    this-one 224368 418064 walk-to  this-one wait-stop
    this-one 1024 0 turn-to
    L-8008A760
    L-8008A724
    L-80096EF4
    60 frames wait
    this-one 230448 417376 walk-to  this-one wait-stop
    this-one 0 0 turn-to
    this-one 48 50 1 anim-then
    120 frames wait
    this-one 49 0 anim
    this-one wait-anim
    this-one 230976 412528 walk-to  this-one wait-stop
    L-80096EF4
    60 frames wait
    this-one 231136 393136 walk-to  this-one wait-stop
    L-8008A730
    this-one 211584 397712 walk-to  this-one wait-stop
    this-one 0 0 turn-to
    L-8008A794
    L-80096EF4
    60 frames wait
    this-one 212064 396640 walk-to  this-one wait-stop
    this-one 213552 397712 walk-to  this-one wait-stop
    this-one 0 0 turn-to
    L-8008A7B8
    60 frames wait
    L-8008A730
    L-80096EF4
    180 frames wait
    this-one 201760 393184 walk-to  this-one wait-stop
    L-8008A754
    L-80096EF4
    180 frames wait
    L-8008A73C
    L-80096EF4
    120 frames wait
    this-one 190048 418032 walk-to  this-one wait-stop
    this-one 1024 0 turn-to
    L-8008A760
    L-8008A73C
    this-one 167872 417168 walk-to  this-one wait-stop
    this-one 167680 436272 walk-to  this-one wait-stop
    L-80096EF4
    120 frames wait
    L-8008A748
    this-one 175760 430896 walk-to  this-one wait-stop
    L-8008A794
    L-80096EF4
    120 frames wait
    L-8008A748
    this-one 201792 436352 walk-to  this-one wait-stop
    L-8008A754
    L-80096EF4
    120 frames wait
    L-8008A724
    this-one 225408 421856 walk-to  this-one wait-stop
    this-one 0 0 turn-to
    L-80096D04
    end-script
    ;

\ $8008B604
: route-8008B604
    67 actor as-this-one
    L-8008A7DC
    this-one arrive
    end-script
    ;

\ $8008B61C
: route-8008B61C
    68 actor as-this-one
    this-one 358400 63968 walk-to  this-one wait-stop
    L-80096D88
    this-one 69 360 attach
    this-one wait-follow
    this-one 37 360 attach
    this-one arrive
    end-script
    ;

\ $8008B658
: route-8008B658
    68 actor as-this-one
    this-one 30 partner-command
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

: L-80098090
    this-one 409008 169184 walk-to  this-one wait-stop
    this-one 3167 0 turn-to
    end-script
    ;

\ $8008B68C
: route-8008B68C
    68 actor as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 34 360 attach
    this-one wait-follow
    L-80098090
    100 frames wait
    L-80096D88
    this-one 90 360 attach
    this-one arrive
    end-script
    ;

\ $8008B6E8
: route-8008B6E8
    68 actor as-this-one
    this-one 91 partner-command
    this-one wait-follow
    this-one 110 360 attach
    this-one wait-follow
    L-80098174
    this-one arrive
    end-script
    ;

\ $8008B71C
: route-8008B71C
    68 actor as-this-one
    L-80098190
    this-one 111 360 attach
    this-one wait-follow
    this-one 584112 118064 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008B74C
: route-8008B74C
    68 actor as-this-one
    L-80096D88
    this-one 9 360 attach
    this-one wait-follow
    this-one 70 360 attach
    this-one wait-follow
    this-one 357616 63920 walk-to  this-one wait-stop
    L-80096D04
    this-one 356720 63904 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008B79C
: route-8008B79C
    69 actor as-this-one
    this-one 468464 412400 walk-to  this-one wait-stop
    this-one 471568 412384 walk-to  this-one wait-stop
    this-one 471520 414816 walk-to  this-one wait-stop
    this-one 1092 200 turn-to
    300 frames wait
    L-80096D88
    this-one 78 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

: L-80098078
    this-one 411824 169888 walk-to  this-one wait-stop
    this-one 67 -2 through-door
    end-script
    ;

\ $8008B808
: route-8008B808
    69 actor as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 34 360 attach
    this-one wait-follow
    L-80098078
    this-one 412880 169600 walk-to  this-one wait-stop
    this-one 512 0 turn-to
    this-one arrive
    end-script
    ;

: L-800980A8
    this-one 412112 169664 walk-to  this-one wait-stop
    this-one 67 0 through-door
    end-script
    ;

\ $8008B85C
: route-8008B85C
    69 actor as-this-one
    L-800980A8
    L-80096D88
    this-one 90 360 attach
    this-one arrive
    end-script
    ;

\ $8008B888
: route-8008B888
    69 actor as-this-one
    this-one 91 partner-command
    this-one wait-follow
    this-one 89 360 attach
    this-one wait-follow
    L-80098A38
    L-80098A2C
    this-one 345280 313104 walk-to  this-one wait-stop
    sub-80096D1C
    this-one 346064 312384 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008B8DC
: route-8008B8DC
    69 actor as-this-one
    L-80098A2C
    L-8008A13C
    L-80098A38
    L-80096D88
    this-one 93 360 attach
    this-one wait-follow
    this-one 278080 98816 walk-to  this-one wait-stop
    L-800985FC
    this-one 274432 95328 walk-to  this-one wait-stop
    L-8009866C
    this-one 271520 91968 walk-to  this-one wait-stop
    400 frames wait
    this-one 269376 93296 walk-to  this-one wait-stop
    400 frames wait
    this-one 270112 93952 walk-to  this-one wait-stop
    400 frames wait
    this-one 270832 88768 walk-to  this-one wait-stop
    640 frames wait
    this-one -57 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008B990
: route-8008B990
    69 actor as-this-one
    this-one 273760 93424 walk-to  this-one wait-stop
    L-800986F4
    this-one 277152 96896 walk-to  this-one wait-stop
    L-80098764
    L-80096D88
    this-one 92 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $8008B9F0
: route-8008B9F0
    69 actor as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 74 360 attach
    this-one wait-follow
    L-80097780
    L-8009778C
    this-one -1 1 69 act-command
    L-80096FD8
    this-one arrive
    end-script
    ;

\ $8008BA48
: route-8008BA48
    69 actor as-this-one
    L-800977F0
    L-80096D88
    this-one 94 360 attach
    this-one wait-follow
    this-one 468416 412112 walk-to  this-one wait-stop
    this-one 2108 0 turn-to
    L-80096D04
    this-one 468528 411552 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008BA9C
: route-8008BA9C
    70 actor as-this-one
    this-one 218688 148832 walk-to  this-one wait-stop
    L-80096D88
    this-one 119 360 attach
    this-one wait-follow
    this-one 7 360 attach
    this-one wait-follow
    L-80097628
    this-one 592752 132320 walk-to  this-one wait-stop
    this-one 592752 131488 walk-to  this-one wait-stop
    this-one 988 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008BB08
: route-8008BB08
    70 actor as-this-one
    sub-80096D74
    this-one 592752 132320 walk-to  this-one wait-stop
    L-80097600
    this-one 15 360 attach
    this-one wait-follow
    L-80098090
    800 frames wait
    L-80096D88
    this-one 121 360 attach
    this-one wait-follow
    this-one 461152 415952 walk-to  this-one wait-stop
    this-one 461024 430448 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008BB78
: route-8008BB78
    70 actor as-this-one
    this-one 461152 415952 walk-to  this-one wait-stop
    L-80096D88
    this-one 78 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

: L-80096F3C
    this-one 17 18 1 anim-then
    30 frames wait
    end-script
    ;

: L-80098D28
    L-80098A38
    L-80098A2C
    this-one 344272 304816 walk-to  this-one wait-stop
    L-80096F3C
    this-one 345296 304368 walk-to  this-one wait-stop
    400 frames wait
    this-one 344240 303536 walk-to  this-one wait-stop
    L-80098A20
    L-80096EF4
    200 frames wait
    L-800989B4
    this-one 346368 271488 walk-to  this-one wait-stop
    L-80096D04
    700 frames wait
    this-one 346368 271488 walk-to  this-one wait-stop
    this-one 344416 269424 walk-to  this-one wait-stop
    this-one 344464 257504 walk-to  this-one wait-stop
    this-one 352592 255296 walk-to  this-one wait-stop
    this-one 6151 250 turn-to
    L-80096EF4
    100 frames wait
    this-one 360336 254832 walk-to  this-one wait-stop
    this-one 5485 0 turn-to
    100 frames wait
    this-one 361488 264528 walk-to  this-one wait-stop
    this-one 357264 264448 walk-to  this-one wait-stop
    L-80096F3C
    this-one 357264 263872 walk-to  this-one wait-stop
    this-one 46 250 turn-to
    goto sub-80096D34
    ;

\ $8008BBC0
: route-8008BBC0
    70 actor as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 35 360 attach
    this-one wait-follow
    this-one 374512 310576 walk-to  this-one wait-stop
    L-80098A38
    L-80098D28
    this-one arrive
    end-script
    ;

: L-80098E24
    sub-80096D74
    this-one 360048 269792 walk-to  this-one wait-stop
    this-one 360288 275280 walk-to  this-one wait-stop
    L-800989A8
    L-80098A38
    end-script
    ;

\ $8008BC10
: route-8008BC10
    70 actor as-this-one
    L-80098E24
    L-80096D88
    this-one 37 360 attach
    this-one arrive
    end-script
    ;

\ $8008BC3C
: route-8008BC3C
    70 actor as-this-one
    this-one 65 partner-command
    this-one wait-follow
    L-80098174
    this-one arrive
    end-script
    ;

\ $8008BC60
: route-8008BC60
    70 actor as-this-one
    L-80098190
    this-one 66 360 attach
    this-one wait-follow
    this-one 120 360 attach
    this-one wait-follow
    this-one 218688 148832 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008BCA0
: route-8008BCA0
    71 actor as-this-one
    this-one 191632 408016 walk-to  this-one wait-stop
    L-80096D88
    this-one 46 360 attach
    this-one wait-follow
    this-one 37 360 attach
    this-one arrive
    end-script
    ;

\ $8008BCDC
: route-8008BCDC
    71 actor as-this-one
    this-one 65 partner-command
    this-one wait-follow
    L-80098174
    this-one arrive
    end-script
    ;

\ $8008BD00
: route-8008BD00
    71 actor as-this-one
    L-80098190
    this-one 66 360 attach
    this-one wait-follow
    this-one 37 360 attach
    this-one arrive
    end-script
    ;

\ $8008BD34
: route-8008BD34
    71 actor as-this-one
    this-one 30 partner-command
    this-one wait-follow
    this-one 334944 269824 walk-to  this-one wait-stop
    this-one 334896 261376 walk-to  this-one wait-stop
    this-one 90 120 turn-by
    this-one wait-turned
    400 frames wait
    this-one 90 120 turn-by
    this-one wait-turned
    this-one 335104 275024 walk-to  this-one wait-stop
    100 frames wait
    this-one 335056 284288 walk-to  this-one wait-stop
    this-one 1033 200 turn-to
    100 frames wait
    this-one 344048 284352 walk-to  this-one wait-stop
    this-one 344768 285040 walk-to  this-one wait-stop
    this-one 82 -2 through-door
    this-one 345456 288160 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008BDD8
: route-8008BDD8
    71 actor as-this-one
    this-one 345136 28512 walk-to  this-one wait-stop
    this-one 15 3 through-door
    this-one 335249 274296 run-to
    this-one wait-stop
    L-80096F08
    this-one 334304 270912 walk-to  this-one wait-stop
    L-80096D88
    this-one 67 360 attach
    this-one wait-follow
    L-80097628
    this-one 593184 132064 walk-to  this-one wait-stop
    this-one 593200 133008 walk-to  this-one wait-stop
    this-one 965 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008BE60
: route-8008BE60
    71 actor as-this-one
    sub-80096D74
    this-one 593184 132064 walk-to  this-one wait-stop
    L-80097600
    this-one 9 360 attach
    this-one wait-follow
    this-one 37 360 attach
    this-one arrive
    end-script
    ;

\ $8008BEA4
: route-8008BEA4
    71 actor as-this-one
    this-one 39 partner-command
    this-one wait-follow
    this-one 68 360 attach
    this-one wait-follow
    this-one 191632 408016 walk-to  this-one wait-stop
    this-one 191504 406256 walk-to  this-one wait-stop
    L-80096D04
    this-one 190912 404544 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008BEF0
: route-8008BEF0
    71 actor as-this-one
    this-one 191632 408016 walk-to  this-one wait-stop
    L-80096D88
    this-one 46 360 attach
    this-one wait-follow
    this-one 68 360 attach
    this-one wait-follow
    this-one 191632 408016 walk-to  this-one wait-stop
    this-one 191504 406256 walk-to  this-one wait-stop
    L-80096D04
    this-one 190912 404544 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008BF50
: route-8008BF50
    72 actor as-this-one
    L-80098C74
    L-80096D88
    this-one 71 360 attach
    this-one wait-follow
    this-one 73 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $8008BFA8
: route-8008BFA8
    72 actor as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 74 360 attach
    this-one wait-follow
    this-one 471216 293072 walk-to  this-one wait-stop
    this-one 471088 288160 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008BFF0
: route-8008BFF0
    72 actor as-this-one
    this-one 471216 293072 walk-to  this-one wait-stop
    L-80096D88
    this-one 63 360 attach
    this-one wait-follow
    this-one 29 360 attach
    this-one arrive
    end-script
    ;

\ $8008C02C
: route-8008C02C
    72 actor as-this-one
    this-one 30 partner-command
    this-one wait-follow
    this-one 20 360 attach
    this-one wait-follow
    this-one 564976 219968 walk-to  this-one wait-stop
    this-one 585888 220048 walk-to  this-one wait-stop
    this-one 585920 209424 walk-to  this-one wait-stop
    L-80096D88
    this-one 75 360 attach
    this-one wait-follow
    this-one 589696 130368 walk-to  this-one wait-stop
    this-one 591808 130432 walk-to  this-one wait-stop
    sub-80096D1C
    this-one 594128 130224 walk-to  this-one wait-stop
    this-one 596832 130320 walk-to  this-one wait-stop
    this-one 596608 130704 walk-to  this-one wait-stop
    600 frames wait
    5 object-anim
    150 frames wait
    this-one 589696 130368 walk-to  this-one wait-stop
    L-80096D88
    this-one 15 360 attach
    this-one wait-follow
    this-one 412592 174432 walk-to  this-one wait-stop
    this-one 415088 174432 walk-to  this-one wait-stop
    this-one 90 0 turn-by
    400 frames wait
    this-one 412592 174432 walk-to  this-one wait-stop
    L-80096D88
    this-one 43 360 attach
    this-one wait-follow
    this-one 175760 421680 walk-to  this-one wait-stop
    this-one 175728 423328 walk-to  this-one wait-stop
    200 frames wait
    this-one 175760 421680 walk-to  this-one wait-stop
    L-80096D88
    this-one 104 360 attach
    this-one wait-follow
    this-one 99392 49520 walk-to  this-one wait-stop
    this-one 98544 52720 walk-to  this-one wait-stop
    L-80098948
    this-one 118 360 attach
    this-one wait-follow
    this-one 570352 210528 walk-to  this-one wait-stop
    this-one -4045 -200 turn-to
    200 frames wait
    this-one 575376 210480 walk-to  this-one wait-stop
    this-one 582224 219712 walk-to  this-one wait-stop
    this-one 582224 221872 walk-to  this-one wait-stop
    200 frames wait
    this-one 581456 208064 walk-to  this-one wait-stop
    this-one 563968 210272 walk-to  this-one wait-stop
    L-80096D88
    this-one 112 360 attach
    this-one wait-follow
    this-one 72 360 attach
    this-one wait-follow
    this-one 377856 62624 walk-to  this-one wait-stop
    this-one 1024 0 turn-to
    L-80096D04
    this-one 376368 69696 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-8008A1C4
    this-one 334575 -4080 260850 set-pos
    this-one 1024 0 turn-to
    goto L-80096D04
    ;

: L-8008A2D8
    this-one 70 72 1 anim-then
    300 frames wait
    this-one 71 0 anim
    30 frames wait
    end-script
    ;

: L-8008A32C
    this-one 83 84 1 anim-then
    180 frames wait
    this-one 85 0 anim
    30 frames wait
    end-script
    ;

: L-8008AC14
    L-800989B4
    L-800989CC
    L-80096EF4
    180 frames wait
    L-800989F0
    L-800989E4
    L-800989D8
    L-80096EF4
    180 frames wait
    L-80098A5C
    L-80098A50
    L-80098A44
    L-80096EF4
    180 frames wait
    L-80098A68
    L-80096EF4
    180 frames wait
    L-800989CC
    L-800989B4
    this-one 344432 257520 walk-to  this-one wait-stop
    this-one 354032 258176 walk-to  this-one wait-stop
    this-one 354864 259984 walk-to  this-one wait-stop
    this-one 2048 300 turn-to
    this-one wait-turned
    L-8008A2D8
    this-one 350064 259664 walk-to  this-one wait-stop
    this-one 3241 300 turn-to
    this-one wait-turned
    50 frames wait
    L-8008A32C
    50 frames wait
    this-one 350064 264832 walk-to  this-one wait-stop
    180 frames wait
    this-one 359488 263904 walk-to  this-one wait-stop
    this-one 0 -300 turn-to
    this-one wait-turned
    L-8008A2D8
    this-one 360112 272240 walk-to  this-one wait-stop
    L-80096EF4
    180 frames wait
    L-800989A8
    L-8008A32C
    50 frames wait
    L-80098A08
    L-800989FC
    L-80096EF4
    180 frames wait
    L-800989B4
    180 frames wait
    L-80098A20
    end-script
    ;

: L-8008A1A4
    this-one 334912 260848 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A1B0
    this-one 3072 0 turn-to
    goto L-80096D04
    ;

\ $8008C22C
: route-8008C22C
    74 actor as-this-one
    L-8008A1C4
    L-80098A68
    L-80098A20
    L-8008AC14
    L-80098A68
    L-8008A1A4
    L-8008A1B0
    this-one arrive
    end-script
    ;

: L-8008A0A4
    L-80096F24
    L-80098A14
    L-80096F24
    L-80098A38
    L-80096F24
    L-80098A2C
    L-80096F24
    L-80098A44
    L-80096F24
    L-80098A68
    L-80096F24
    L-800989CC
    L-80096F24
    L-800989B4
    L-80096F24
    L-80098A20
    L-80096F24
    L-80098A68
    goto L-80096F24
    ;

\ $8008C274
: route-8008C274
    74 actor as-this-one
    L-8008A1C4
    L-80098A68
    L-8008A0A4
    L-8008A1A4
    L-8008A1B0
    this-one arrive
    end-script
    ;

: L-80089F08
    L-800989FC
    300 frames wait
    this-one 344512 256048 walk-to  this-one wait-stop
    this-one 3051 0 turn-to
    500 frames wait
    L-800989B4
    300 frames wait
    this-one 344512 256048 walk-to  this-one wait-stop
    this-one 3051 0 turn-to
    500 frames wait
    this-one 87 1 anim
    200 frames wait
    L-800989FC
    this-one 3051 0 turn-to
    500 frames wait
    L-800989F0
    this-one 335120 256096 walk-to  this-one wait-stop
    this-one 3051 0 turn-to
    this-one 87 1 anim
    300 frames wait
    this-one 332832 256112 walk-to  this-one wait-stop
    this-one 87 1 anim
    300 frames wait
    this-one 335104 256992 walk-to  this-one wait-stop
    this-one 335296 260848 walk-to  this-one wait-stop
    500 frames wait
    this-one 335120 265040 walk-to  this-one wait-stop
    this-one 330400 266032 walk-to  this-one wait-stop
    this-one 330752 275072 walk-to  this-one wait-stop
    L-800989D8
    500 frames wait
    L-800989CC
    500 frames wait
    this-one 330528 274320 walk-to  this-one wait-stop
    this-one 330176 269376 walk-to  this-one wait-stop
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    3500 frames wait
    sub-80096FB0
    L-80098060
    this-one 330464 269216 walk-to  this-one wait-stop
    this-one 330704 265776 walk-to  this-one wait-stop
    this-one 334816 265504 walk-to  this-one wait-stop
    L-800989F0
    500 frames wait
    L-800989FC
    goto L-800989B4
    ;

\ $8008C2AC
: route-8008C2AC
    75 actor as-this-one
    L-80098B08
    L-80098A20
    L-800989B4
    L-80089F08
    L-80098A20
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

\ $8008C2F4
: route-8008C2F4
    75 actor as-this-one
    L-80098B08
    L-80098A68
    L-8008A0A4
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

: L-80098C80
    this-one 325152 69872 walk-to  this-one wait-stop
    end-script
    ;

: L-80098B9C
    this-one 340080 100320 walk-to  this-one wait-stop
    end-script
    ;

: L-80098B6C
    this-one 280144 100288 walk-to  this-one wait-stop
    end-script
    ;

: L-80098B78
    this-one 285248 100352 walk-to  this-one wait-stop
    end-script
    ;

: L-80098BD8
    this-one 285200 77312 walk-to  this-one wait-stop
    end-script
    ;

: L-80098BE4
    this-one 297776 59568 walk-to  this-one wait-stop
    end-script
    ;

: L-8008B1EC
    L-80098BA8
    L-80098C50
    L-80098C5C
    L-80096EF4
    100 frames wait
    L-80098C50
    L-80098BA8
    L-80098B6C
    L-80096EF4
    200 frames wait
    L-80098B78
    L-80098BD8
    L-80098BE4
    L-80098C08
    L-80098B9C
    end-script
    ;

\ $8008C32C
: route-8008C32C
    77 actor as-this-one
    L-80098C80
    L-80098B90
    L-80098B9C
    L-8008B1EC
    L-80098B90
    L-80098C80
    this-one 329024 69984 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-8008B268
    this-one 278640 98304 walk-to  this-one wait-stop
    L-800985FC
    this-one 274960 94704 walk-to  this-one wait-stop
    L-8009866C
    this-one 273760 90512 walk-to  this-one wait-stop
    200 frames wait
    this-one 270384 88720 walk-to  this-one wait-stop
    this-one 86 0 anim
    200 frames wait
    this-one 268800 89840 walk-to  this-one wait-stop
    200 frames wait
    this-one 268544 91296 walk-to  this-one wait-stop
    this-one 5125 300 turn-to
    sub-80096D34
    400 frames wait
    this-one 270528 91296 walk-to  this-one wait-stop
    400 frames wait
    this-one 269120 93024 walk-to  this-one wait-stop
    50 frames wait
    this-one 273216 94000 walk-to  this-one wait-stop
    L-800986F4
    this-one 276752 97376 walk-to  this-one wait-stop
    L-80098764
    L-80098B6C
    end-script
    ;

\ $8008C374
: route-8008C374
    77 actor as-this-one
    L-80098C80
    L-80098B90
    L-80098B6C
    L-8008B268
    L-80098B90
    L-80098C80
    this-one 329024 69984 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-8008A18C
    L-80098D28
    500 frames wait
    goto L-80098E24
    ;

\ $8008C3BC
: route-8008C3BC
    78 actor as-this-one
    L-80098B08
    L-80098A20
    L-80098A2C
    L-8008A13C
    L-80098A38
    L-8008A18C
    L-80098A14
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

\ $8008C414
: route-8008C414
    78 actor as-this-one
    L-80098B08
    L-80098A20
    L-800989B4
    L-80089CE0
    L-80098A20
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

\ $8008C45C
: route-8008C45C
    79 actor as-this-one
    L-8008A7DC
    this-one arrive
    end-script
    ;

\ $8008C474
: route-8008C474
    80 actor as-this-one
    this-one 216936 0 117907 set-pos
    this-one 220608 119568 walk-to  this-one wait-stop
    this-one 220464 140640 walk-to  this-one wait-stop
    this-one 246272 140144 walk-to  this-one wait-stop
    L-80097A34
    this-one 249072 134224 walk-to  this-one wait-stop
    this-one 2035 -200 turn-to
    100 frames wait
    L-80096EF4
    100 frames wait
    L-80097968
    this-one 9 7 34 lift
    this-one wait-follow
    this-one 345664 285456 walk-to  this-one wait-stop
    this-one 345104 288160 walk-to  this-one wait-stop
    L-80096EF4
    400 frames wait
    this-one 345904 287088 walk-to  this-one wait-stop
    this-one 44 anim-once
    300 frames wait
    this-one 45 0 1 anim-then
    this-one 345840 288192 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008C54C
: route-8008C54C
    80 actor as-this-one
    this-one 345136 285264 walk-to  this-one wait-stop
    this-one 344192 284416 walk-to  this-one wait-stop
    this-one 373824 284288 walk-to  this-one wait-stop
    this-one 374208 314032 walk-to  this-one wait-stop
    this-one 344112 314192 walk-to  this-one wait-stop
    this-one 344288 284560 walk-to  this-one wait-stop
    this-one 344448 275248 walk-to  this-one wait-stop
    this-one 374160 275216 walk-to  this-one wait-stop
    this-one 353168 284400 walk-to  this-one wait-stop
    this-one 90 200 turn-by
    this-one wait-turned
    500 frames wait
    this-one -90 -200 turn-by
    this-one wait-turned
    this-one 344256 285024 walk-to  this-one wait-stop
    this-one 90 200 turn-by
    this-one wait-turned
    500 frames wait
    this-one -90 -200 turn-by
    this-one wait-turned
    this-one 344336 284608 walk-to  this-one wait-stop
    this-one 90 200 turn-by
    L-80096EF4
    300 frames wait
    this-one 334400 28416 walk-to  this-one wait-stop
    this-one wait-stop
    this-one -90 -200 turn-by
    this-one wait-turned
    300 frames wait
    this-one 334979 275254 run-to
    this-one wait-stop
    this-one 335280 261776 walk-to  this-one wait-stop
    this-one 90 200 turn-by
    this-one wait-turned
    500 frames wait
    this-one -90 -200 turn-by
    this-one wait-turned
    this-one 334912 252704 walk-to  this-one wait-stop
    this-one 90 200 turn-by
    this-one wait-turned
    500 frames wait
    this-one -90 -200 turn-by
    this-one wait-turned
    this-one 335056 245440 walk-to  this-one wait-stop
    this-one 332720 245344 walk-to  this-one wait-stop
    this-one 90 200 turn-by
    this-one wait-turned
    500 frames wait
    this-one -90 -200 turn-by
    this-one wait-turned
    this-one 316560 245184 walk-to  this-one wait-stop
    this-one 90 200 turn-by
    this-one wait-turned
    500 frames wait
    this-one -90 -200 turn-by
    this-one wait-turned
    this-one 305200 245520 walk-to  this-one wait-stop
    this-one 305264 275072 walk-to  this-one wait-stop
    L-80096EF4
    300 frames wait
    300 frames wait
    this-one 305328 284336 walk-to  this-one wait-stop
    this-one 344640 284400 walk-to  this-one wait-stop
    this-one 7 9 34 lift
    this-one wait-follow
    L-80097994
    this-one 249072 134224 walk-to  this-one wait-stop
    L-80096EF4
    100 frames wait
    L-80097968
    this-one 246384 140320 walk-to  this-one wait-stop
    this-one 231280 140080 walk-to  this-one wait-stop
    this-one 225152 117984 walk-to  this-one wait-stop
    L-80096EF4
    300 frames wait
    this-one 215776 117968 walk-to  this-one wait-stop
    this-one 216928 11776 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008C7C8
: route-8008C7C8
    80 actor as-this-one
    this-one 216936 0 117907 set-pos
    this-one 220608 119568 walk-to  this-one wait-stop
    L-80096D88
    this-one 119 360 attach
    this-one wait-follow
    50 frames wait
    this-one 93 360 attach
    this-one wait-follow
    50 frames wait
    this-one 97 360 attach
    this-one wait-follow
    L-80098090
    50 frames wait
    L-80096D88
    this-one 90 460 attach
    this-one arrive
    end-script
    ;

\ $8008C85C
: route-8008C85C
    80 actor as-this-one
    this-one 91 partner-command
    this-one wait-follow
    this-one 89 360 attach
    this-one wait-follow
    50 frames wait
    this-one 120 360 attach
    this-one wait-follow
    this-one 215776 117968 walk-to  this-one wait-stop
    this-one 216928 11776 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008C8B0
: route-8008C8B0
    81 actor as-this-one
    this-one 174896 409472 walk-to  this-one wait-stop
    L-80096D88
    this-one 46 360 attach
    this-one wait-follow
    this-one 7 360 attach
    this-one wait-follow
    L-80097628
    this-one 592288 132368 walk-to  this-one wait-stop
    this-one 592256 133248 walk-to  this-one wait-stop
    this-one -3058 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008C91C
: route-8008C91C
    81 actor as-this-one
    sub-80096D74
    this-one 592224 132240 walk-to  this-one wait-stop
    L-80097600
    this-one 15 360 attach
    this-one wait-follow
    L-80098078
    this-one 413664 171296 walk-to  this-one wait-stop
    this-one 57 1 anim
    this-one arrive
    end-script
    ;

\ $8008C970
: route-8008C970
    81 actor as-this-one
    this-one 413568 169824 walk-to  this-one wait-stop
    L-800980A8
    L-80096D88
    this-one 32 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $8008C9C0
: route-8008C9C0
    81 actor as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 29 360 attach
    this-one arrive
    end-script
    ;

\ $8008C9F4
: route-8008C9F4
    81 actor as-this-one
    this-one 55 partner-command
    this-one wait-follow
    L-80097A34
    this-one 250416 135248 walk-to  this-one wait-stop
    100 frames wait
    this-one 249264 134000 walk-to  this-one wait-stop
    this-one 2090 0 turn-to
    this-one arrive
    end-script
    ;

\ $8008CA3C
: route-8008CA3C
    81 actor as-this-one
    L-80097968
    L-80096D88
    this-one 18 360 attach
    this-one wait-follow
    this-one 344896 285024 walk-to  this-one wait-stop
    this-one 535 0 turn-to
    sub-80096D1C
    this-one 345072 287712 walk-to  this-one wait-stop
    this-one -95 0 turn-to
    this-one arrive
    end-script
    ;

\ $8008CA9C
: route-8008CA9C
    81 actor as-this-one
    L-80097EF4
    L-80096D88
    this-one 76 360 attach
    this-one wait-follow
    L-80098090
    this-one arrive
    end-script
    ;

\ $8008CAD4
: route-8008CAD4
    81 actor as-this-one
    L-80096D88
    this-one 32 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $8008CB14
: route-8008CB14
    81 actor as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 67 360 attach
    this-one wait-follow
    L-80097628
    this-one 593184 132064 walk-to  this-one wait-stop
    this-one 593200 133008 walk-to  this-one wait-stop
    this-one 965 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008CB78
: route-8008CB78
    81 actor as-this-one
    this-one 593184 132064 walk-to  this-one wait-stop
    L-80097600
    this-one 36 360 attach
    this-one wait-follow
    L-800970B4
    L-800970C8
    40 frames wait
    this-one 105488 360080 walk-to  this-one wait-stop
    this-one 1978 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008CBDC
: route-8008CBDC
    81 actor as-this-one
    L-800970EC
    L-80096D88
    this-one 80 360 attach
    this-one wait-follow
    this-one 174896 409472 walk-to  this-one wait-stop
    this-one 174912 406576 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008CC1C
: route-8008CC1C
    82 actor as-this-one
    this-one 47 380 attach
    this-one wait-follow
    this-one 76 380 attach
    this-one wait-follow
    this-one 90 380 attach
    this-one arrive
    end-script
    ;

\ $8008CC58
: route-8008CC58
    83 actor as-this-one
    this-one 211615 0 431747 set-pos
    this-one 211616 434816 walk-to  this-one wait-stop
    L-80096D88
    this-one 81 360 attach
    this-one wait-follow
    this-one 38 360 attach
    this-one arrive
    end-script
    ;

\ $8008CCA4
: route-8008CCA4
    83 actor as-this-one
    this-one 39 partner-command
    this-one wait-follow
    this-one 79 360 attach
    this-one wait-follow
    L-800970B4
    L-800970C8
    40 frames wait
    this-one 105488 360080 walk-to  this-one wait-stop
    this-one 1978 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008CD04
: route-8008CD04
    83 actor as-this-one
    L-80097118
    L-8009717C
    40 frames wait
    L-800971AC
    L-800971D4
    L-800970EC
    L-80096D88
    this-one 83 360 attach
    this-one wait-follow
    this-one arrive
    end-script
    ;

\ $8008CD5C
: route-8008CD5C
    83 actor as-this-one
    L-80096D88
    this-one 85 360 attach
    this-one wait-follow
    L-80097628
    this-one 593184 132064 walk-to  this-one wait-stop
    this-one 593200 133008 walk-to  this-one wait-stop
    this-one 965 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008CDB0
: route-8008CDB0
    83 actor as-this-one
    sub-80096D74
    this-one 593184 132064 walk-to  this-one wait-stop
    L-80097600
    this-one 41 360 attach
    this-one wait-follow
    this-one 23 360 attach
    this-one arrive
    end-script
    ;

\ $8008CDF4
: route-8008CDF4
    83 actor as-this-one
    this-one 55 partner-command
    this-one wait-follow
    this-one 42 360 attach
    this-one wait-follow
    L-80098090
    this-one arrive
    end-script
    ;

\ $8008CE28
: route-8008CE28
    83 actor as-this-one
    L-80096D88
    this-one 89 360 attach
    this-one wait-follow
    this-one 38 360 attach
    this-one arrive
    end-script
    ;

\ $8008CE5C
: route-8008CE5C
    83 actor as-this-one
    this-one 39 partner-command
    this-one wait-follow
    this-one 37 360 attach
    this-one arrive
    end-script
    ;

\ $8008CE84
: route-8008CE84
    83 actor as-this-one
    this-one 39 partner-command
    this-one wait-follow
    this-one 82 360 attach
    this-one wait-follow
    this-one 211616 434816 walk-to  this-one wait-stop
    this-one 211600 431744 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008CEC0
: route-8008CEC0
    84 actor as-this-one
    this-one 112985 0 351335 set-pos
    L-800970B4
    L-800970C8
    L-80096EF4
    200 frames wait
    this-one 102432 359136 walk-to  this-one wait-stop
    this-one 60 0 turn-to
    L-80096EF4
    100 frames wait
    L-80097090
    L-8009709C
    this-one 1056 0 turn-to
    L-80096F24
    L-8009709C
    L-800971E8
    this-one 101600 350704 walk-to  this-one wait-stop
    L-80096F24
    this-one 101440 356864 walk-to  this-one wait-stop
    L-80096F24
    this-one 101168 356816 walk-to  this-one wait-stop
    L-80096EF4
    100 frames wait
    L-800970A8
    L-800973B4
    L-80096F24
    L-80097550
    this-one 112976 351280 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80096D54
    this-one 150 0 anim
    this-one wait-anim
    this-one 151 1 anim
    end-script
    ;

\ $8008CFB8
: route-8008CFB8
    84 actor as-this-one
    this-one 112985 0 351335 set-pos
    L-80096D88
    this-one 83 360 attach
    this-one wait-follow
    L-80097754
    this-one 126640 311184 walk-to  this-one wait-stop
    this-one 125296 312880 walk-to  this-one wait-stop
    this-one 3072 -190 turn-to
    100 frames wait
    L-80096D54
    this-one arrive
    end-script
    ;

\ $8008D024
: route-8008D024
    84 actor as-this-one
    this-one 153 0 anim
    this-one wait-anim
    this-one 126640 311184 walk-to  this-one wait-stop
    L-80096D88
    this-one 84 360 attach
    this-one wait-follow
    L-80097A34
    this-one 249600 134144 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008D074
: route-8008D074
    84 actor as-this-one
    L-800979F4
    this-one 49 360 attach
    this-one wait-follow
    L-80098174
    this-one arrive
    end-script
    ;

: L-8008ADA8
    L-80096D04
    180 frames wait
    this-one 180 0 turn-by
    L-80096D04
    end-script
    ;

: L-8008A38C
    this-one 97 98 1 anim-then
    120 frames wait
    this-one 99 0 anim
    30 frames wait
    end-script
    ;

: L-8008AFA8
    L-80098A2C
    this-one 345200 313056 walk-to  this-one wait-stop
    this-one 1493 0 turn-to
    L-8008ADA8
    L-80098A2C
    L-80096EF4
    180 frames wait
    L-80098A44
    this-one 334944 312096 walk-to  this-one wait-stop
    this-one 3072 0 turn-to
    L-8008ADA8
    L-8008A38C
    this-one 334944 304368 walk-to  this-one wait-stop
    this-one 3072 0 turn-to
    L-8008ADA8
    L-80096EF4
    180 frames wait
    this-one 344112 297296 walk-to  this-one wait-stop
    this-one 1024 0 turn-to
    L-8008A32C
    this-one 344832 297264 walk-to  this-one wait-stop
    L-8008ADA8
    this-one 344112 297296 walk-to  this-one wait-stop
    L-80096EF4
    180 frames wait
    L-80098A20
    L-80098A68
    this-one 314064 284208 walk-to  this-one wait-stop
    this-one 314064 284688 walk-to  this-one wait-stop
    this-one 0 0 turn-to
    L-8008ADA8
    L-8008A38C
    L-80098A5C
    this-one 305104 293968 walk-to  this-one wait-stop
    this-one 308784 297056 walk-to  this-one wait-stop
    this-one 1024 0 turn-to
    L-8008ADA8
    L-80096EF4
    60 frames wait
    this-one 305280 301520 walk-to  this-one wait-stop
    L-80098A50
    L-80098A44
    L-8008A38C
    L-80098A2C
    goto L-80098A38
    ;

\ $8008D0A4
: route-8008D0A4
    84 actor as-this-one
    L-80098190
    this-one 66 360 attach
    this-one wait-follow
    L-80098A38
    L-8008AFA8
    this-one 37 360 attach
    this-one arrive
    end-script
    ;

\ $8008D0E8
: route-8008D0E8
    84 actor as-this-one
    this-one 39 partner-command
    this-one wait-follow
    L-80098A38
    L-80098D28
    this-one arrive
    end-script
    ;

\ $8008D114
: route-8008D114
    84 actor as-this-one
    L-80098E24
    L-80096D88
    this-one 79 360 attach
    this-one wait-follow
    this-one 112976 351280 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008D14C
: route-8008D14C
    86 actor as-this-one
    L-80098C74
    L-80098BCC
    L-80098B6C
    this-one 277840 99072 walk-to  this-one wait-stop
    L-800985FC
    this-one 274352 95312 walk-to  this-one wait-stop
    L-8009866C
    this-one 273728 91408 walk-to  this-one wait-stop
    this-one 3016 0 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008D1B0
: route-8008D1B0
    86 actor as-this-one
    sub-80096D74
    this-one 273968 93216 walk-to  this-one wait-stop
    L-800986F4
    this-one 277344 96752 walk-to  this-one wait-stop
    L-80098764
    L-80098B6C
    L-80098B78
    L-80098BD8
    L-80098BE4
    L-80098C38
    L-80098C74
    this-one 376368 69696 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-8008A308
    this-one 87 0 anim
    this-one wait-anim
    this-one 87 0 anim
    this-one wait-anim
    end-script
    ;

: L-8008A3BC
    L-800989B4
    this-one 360208 273280 walk-to  this-one wait-stop
    this-one 360064 266528 walk-to  this-one wait-stop
    L-80096F08
    60 frames wait
    this-one 355056 263936 walk-to  this-one wait-stop
    this-one 0 300 turn-to
    this-one wait-turned
    L-8008A2D8
    this-one 353136 264080 walk-to  this-one wait-stop
    this-one 350256 261168 walk-to  this-one wait-stop
    this-one 350064 259664 walk-to  this-one wait-stop
    this-one 3241 300 turn-to
    this-one wait-turned
    50 frames wait
    L-8008A32C
    this-one 362768 255536 walk-to  this-one wait-stop
    this-one 2017 300 turn-to
    this-one wait-turned
    50 frames wait
    L-8008A32C
    this-one 366144 258512 walk-to  this-one wait-stop
    this-one 5025 300 turn-to
    this-one wait-turned
    30 frames wait
    L-8008A308
    60 frames wait
    this-one 359456 260000 walk-to  this-one wait-stop
    this-one 2048 -300 turn-to
    this-one wait-turned
    L-8008A2D8
    this-one 367360 256944 walk-to  this-one wait-stop
    this-one 373936 257088 walk-to  this-one wait-stop
    L-80098A08
    L-80096F08
    120 frames wait
    this-one 354768 245440 walk-to  this-one wait-stop
    60 frames wait
    this-one 354784 259984 walk-to  this-one wait-stop
    this-one 2048 300 turn-to
    this-one wait-turned
    L-8008A2D8
    this-one 351024 257664 walk-to  this-one wait-stop
    60 frames wait
    this-one 344240 257568 walk-to  this-one wait-stop
    60 frames wait
    L-800989FC
    60 frames wait
    L-800989F0
    L-8008A308
    goto L-800989CC
    ;

\ $8008D220
: route-8008D220
    87 actor as-this-one
    L-800989B4
    L-800989CC
    L-8008A3BC
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

\ $8008D258
: route-8008D258
    87 actor as-this-one
    L-80098B08
    L-80098A5C
    L-800989D8
    L-800989A8
    L-80098A14
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

\ $8008D2A0
: route-8008D2A0
    87 actor as-this-one
    L-80098B08
    L-80098A68
    L-8008A0A4
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

\ $8008D2D8
: route-8008D2D8
    87 actor as-this-one
    L-80098B08
    L-80098A68
    L-800989F0
    L-800989FC
    this-one arrive
    end-script
    ;

\ $8008D308
: route-8008D308
    113 actor as-this-one
    this-one 358400 63968 walk-to  this-one wait-stop
    L-80096D88
    this-one 69 360 attach
    this-one wait-follow
    this-one 37 360 attach
    this-one arrive
    end-script
    ;

\ $8008D344
: route-8008D344
    113 actor as-this-one
    this-one 30 partner-command
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $8008D378
: route-8008D378
    113 actor as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 34 360 attach
    this-one wait-follow
    L-80098090
    100 frames wait
    L-80096D88
    this-one 90 360 attach
    this-one arrive
    end-script
    ;

\ $8008D3D4
: route-8008D3D4
    113 actor as-this-one
    this-one 91 partner-command
    this-one wait-follow
    this-one 110 360 attach
    this-one wait-follow
    L-80098174
    this-one arrive
    end-script
    ;

\ $8008D408
: route-8008D408
    113 actor as-this-one
    L-80098190
    this-one 111 360 attach
    this-one wait-follow
    this-one 584112 118064 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008D438
: route-8008D438
    113 actor as-this-one
    L-80096D88
    this-one 9 360 attach
    this-one wait-follow
    this-one 70 360 attach
    this-one wait-follow
    this-one 357616 63920 walk-to  this-one wait-stop
    L-80096D04
    this-one 356720 63904 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008D488
: route-8008D488
    114 actor as-this-one
    L-80098B08
    L-80098A20
    L-80098A2C
    L-8008A13C
    L-80098A38
    L-8008A18C
    L-80098A14
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

\ $8008D4E0
: route-8008D4E0
    114 actor as-this-one
    L-80098B08
    L-80098A20
    L-800989B4
    L-80089CE0
    L-80098A20
    L-800989C0
    L-80098AD8
    this-one arrive
    end-script
    ;

\ $8008D528
: route-8008D528
    116 actor as-this-one
    this-one 216895 0 131704 set-pos
    this-one 217824 131760 walk-to  this-one wait-stop
    L-80096D88
    this-one 119 360 attach
    this-one wait-follow
    L-80098A38
    L-8008AFA8
    200 frames wait
    L-80096D88
    this-one 38 360 attach
    this-one wait-follow
    this-one arrive
    end-script
    ;

: L-8008ADD0
    L-80098A14
    this-one 354256 284416 walk-to  this-one wait-stop
    this-one 0 0 turn-to
    L-8008ADA8
    L-80096EF4
    180 frames wait
    L-80098A20
    L-800989B4
    this-one 346352 271488 walk-to  this-one wait-stop
    this-one 1577 0 turn-to
    L-8008ADA8
    L-80096EF4
    180 frames wait
    L-800989B4
    L-800989CC
    this-one 334880 253760 walk-to  this-one wait-stop
    this-one 3072 0 turn-to
    L-8008ADA8
    L-80096EF4
    180 frames wait
    this-one 344480 255632 walk-to  this-one wait-stop
    this-one 351472 255680 walk-to  this-one wait-stop
    L-80096EF4
    180 frames wait
    this-one 351408 254864 walk-to  this-one wait-stop
    this-one 2048 0 turn-to
    L-8008ADA8
    this-one 351680 266176 walk-to  this-one wait-stop
    L-80096EF4
    180 frames wait
    this-one 350592 267456 walk-to  this-one wait-stop
    this-one 7659 0 turn-to
    L-8008ADA8
    this-one 351680 266176 walk-to  this-one wait-stop
    L-8008A38C
    this-one 365760 265392 walk-to  this-one wait-stop
    this-one 1024 0 turn-to
    L-8008ADA8
    this-one 365728 255952 walk-to  this-one wait-stop
    this-one 3072 0 turn-to
    L-80096EF4
    180 frames wait
    this-one 372656 256064 walk-to  this-one wait-stop
    this-one 372640 260384 walk-to  this-one wait-stop
    L-80096EF4
    180 frames wait
    this-one 370704 260448 walk-to  this-one wait-stop
    L-8008ADA8
    this-one 372640 260384 walk-to  this-one wait-stop
    L-8008A38C
    L-800989A8
    L-80096EF4
    180 frames wait
    L-80098A14
    L-80098A38
    end-script
    ;

\ $8008D598
: route-8008D598
    116 actor as-this-one
    this-one 39 partner-command
    this-one wait-follow
    L-80098A38
    L-8008ADD0
    L-80096D88
    this-one 82 360 attach
    this-one wait-follow
    this-one 211584 431728 walk-to  this-one wait-stop
    this-one 2048 0 turn-to
    this-one arrive
    end-script
    ;

\ $8008D5F0
: route-8008D5F0
    116 actor as-this-one
    this-one 211595 0 431739 set-pos
    L-80096D88
    this-one 81 360 attach
    this-one wait-follow
    50 frames wait
    this-one 120 360 attach
    this-one wait-follow
    this-one 217824 131760 walk-to  this-one wait-stop
    this-one 216880 13168 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-8008A2B0
    this-one 344700 -4080 301022 set-pos
    this-one 3072 0 turn-to
    L-80096D04
    end-script
    ;

: L-8008A1E8
    this-one 334848 295264 walk-to  this-one wait-stop
    end-script
    ;

\ $8008D650
: route-8008D650
    117 actor as-this-one
    L-8008A2B0
    L-8008A1E8
    L-80098A68
    this-one 7 1 76 act-command
    this-one wait-stop
    this-one 7 3 34 lift
    this-one wait-follow
    L-80098B9C
    L-80098B6C
    this-one 279152 97984 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008D6B0
: route-8008D6B0
    117 actor as-this-one
    L-80098B6C
    200 frames wait
    L-8008B268
    this-one 279152 97984 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097910
    this-one 249072 134768 walk-to  this-one wait-stop
    this-one 2 0 through-door
    this-one 247872 135184 walk-to  this-one wait-stop
    this-one 1034 0 turn-to
    goto sub-80096D34
    ;

\ $8008D6E0
: route-8008D6E0
    117 actor as-this-one
    this-one 2 1 76 act-command
    this-one wait-stop
    this-one 2 9 34 lift
    this-one wait-follow
    L-80097994
    L-80097910
    this-one arrive
    end-script
    ;

: L-80097940
    sub-80096D74
    this-one 248768 134784 walk-to  this-one wait-stop
    this-one 10 0 through-door
    this-one 249696 135088 walk-to  this-one wait-stop
    end-script
    ;

: L-80098B34
    this-one 330608 273648 walk-to  this-one wait-stop
    this-one 330256 269376 walk-to  this-one wait-stop
    goto L-80098048
    ;

\ $8008D720
: route-8008D720
    117 actor as-this-one
    L-80097940
    L-80097968
    this-one 9 7 34 lift
    this-one wait-follow
    L-800989B4
    L-800989CC
    L-80098B34
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

: L-80098B4C
    L-80098060
    this-one 330288 269936 walk-to  this-one wait-stop
    this-one 330592 274640 walk-to  this-one wait-stop
    goto L-800989CC
    ;

: L-8008A240
    this-one 344528 2912 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A28C
    this-one 344320 301008 walk-to  this-one wait-stop
    end-script
    ;

: L-8008A298
    this-one 1024 0 turn-to
    L-80096D04
    end-script
    ;

\ $8008D77C
: route-8008D77C
    117 actor as-this-one
    sub-80096FB0
    L-80098B4C
    L-80096EF4
    100 frames wait
    L-80096F24
    L-80098A68
    L-80096F24
    L-8008A240
    L-8008A28C
    L-8008A298
    this-one arrive
    end-script
    ;

: L-8008A9FC
    this-one 66 67 0 anim-then
    this-one wait-anim
    this-one 66 67 0 anim-then
    this-one wait-anim
    L-80096F08
    60 frames wait
    this-one 97 98 1 anim-then
    30 frames wait
    this-one 99 0 anim
    this-one wait-anim
    end-script
    ;

: L-8008AA60
    L-80098A9C
    this-one 396419 282873 run-to
    this-one wait-stop
    L-8008A9FC
    this-one 396419 282873 run-to
    this-one wait-stop
    this-one 403327 315937 run-to
    this-one wait-stop
    this-one 432395 320527 run-to
    this-one wait-stop
    this-one 459250 302546 run-to
    this-one wait-stop
    this-one 468888 302366 run-to
    this-one wait-stop
    L-8008A9FC
    this-one 468038 312693 run-to
    this-one wait-stop
    this-one 445787 349188 run-to
    this-one wait-stop
    this-one 450420 372810 run-to
    this-one wait-stop
    this-one 471236 393649 run-to
    this-one wait-stop
    this-one 492899 407220 run-to
    this-one wait-stop
    this-one 494082 419969 run-to
    this-one wait-stop
    L-8008A9FC
    this-one 478220 418113 run-to
    this-one wait-stop
    this-one 453474 417308 run-to
    this-one wait-stop
    this-one 443016 413552 run-to
    this-one wait-stop
    this-one 406566 424599 run-to
    this-one wait-stop
    this-one 377462 417968 run-to
    this-one wait-stop
    this-one 347445 428259 run-to
    this-one wait-stop
    L-8008A9FC
    this-one 331874 418077 run-to
    this-one wait-stop
    this-one 330500 406132 run-to
    this-one wait-stop
    this-one 341937 383297 run-to
    this-one wait-stop
    this-one 342111 319768 run-to
    this-one wait-stop
    L-8008A9FC
    this-one 344182 314152 run-to
    this-one wait-stop
    L-80098AB0
    L-8008A9FC
    end-script
    ;

\ $8008D7DC
: route-8008D7DC
    117 actor as-this-one
    L-8008A2B0
    L-80098A20
    L-8008AA60
    L-8008A28C
    L-8008A298
    this-one arrive
    end-script
    ;

\ $8008D814
: route-8008D814
    119 actor as-this-one
    this-one 470345 0 192204 set-pos
    this-one 467952 191968 walk-to  this-one wait-stop
    L-80096D88
    this-one 167 360 attach
    this-one wait-follow
    L-80097628
    this-one 592224 132288 walk-to  this-one wait-stop
    this-one 592208 133312 walk-to  this-one wait-stop
    this-one 1046 190 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $8008D880
: route-8008D880
    119 actor as-this-one
    sub-80096D74
    this-one 592224 132288 walk-to  this-one wait-stop
    L-80097600
    this-one 15 360 attach
    this-one wait-follow
    L-80098090
    this-one arrive
    end-script
    ;

\ $8008D8C0
: route-8008D8C0
    119 actor as-this-one
    L-80096D88
    this-one 89 360 attach
    this-one wait-follow
    L-80098D28
    this-one arrive
    end-script
    ;

\ $8008D8F0
: route-8008D8F0
    119 actor as-this-one
    L-80098E24
    L-80096D88
    this-one 79 360 attach
    this-one wait-follow
    L-800970B4
    L-800970C8
    this-one 103632 359136 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008D938
: route-8008D938
    119 actor as-this-one
    L-80097118
    L-800971E8
    this-one 101184 356096 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-800987F8
    this-one 278112 98768 walk-to  this-one wait-stop
    L-800985FC
    this-one 274336 95392 walk-to  this-one wait-stop
    L-8009866C
    this-one 269232 93168 walk-to  this-one wait-stop
    end-script
    ;

\ $8008D960
: route-8008D960
    119 actor as-this-one
    L-800970A8
    L-800973B4
    L-80097550
    L-80096D88
    this-one 61 360 attach
    this-one wait-follow
    L-800987F8
    this-one arrive
    end-script
    ;

\ $8008D9A8
: route-8008D9A8
    119 actor as-this-one
    L-800987D4
    L-80096D88
    this-one 99 360 attach
    this-one wait-follow
    L-80097A34
    this-one 249376 134432 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008D9E8
: route-8008D9E8
    119 actor as-this-one
    L-800979F4
    this-one 21 360 attach
    this-one wait-follow
    this-one 469504 192240 walk-to  this-one wait-stop
    this-one 470336 192240 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-8008A35C
    this-one 32 34 1 anim-then
    60 frames wait
    this-one 35 0 anim
    30 frames wait
    end-script
    ;

: L-8008A554
    this-one 374320 258592 walk-to  this-one wait-stop
    360 frames wait
    this-one 370736 260384 walk-to  this-one wait-stop
    this-one 3072 -300 turn-to
    this-one wait-turned
    L-80096D04
    60 frames wait
    this-one 1024 0 turn-to
    this-one wait-turned
    L-80096D04
    this-one 371280 259792 walk-to  this-one wait-stop
    this-one 371376 256032 walk-to  this-one wait-stop
    this-one 363872 255312 walk-to  this-one wait-stop
    this-one 2048 0 turn-to
    this-one wait-turned
    this-one 65 0 anim
    this-one wait-anim
    this-one 65 0 anim
    this-one wait-anim
    this-one 65 0 anim
    this-one wait-anim
    L-8008A35C
    this-one 359376 256176 walk-to  this-one wait-stop
    this-one 2048 0 turn-to
    this-one wait-turned
    L-80096EF4
    60 frames wait
    this-one 357840 254816 walk-to  this-one wait-stop
    L-8008A32C
    this-one 358768 254416 walk-to  this-one wait-stop
    this-one 2048 0 turn-to
    this-one wait-turned
    L-80096D04
    60 frames wait
    this-one 0 0 turn-to
    this-one wait-turned
    L-80096D04
    L-80096EF4
    120 frames wait
    this-one 357168 259968 walk-to  this-one wait-stop
    this-one 2048 0 turn-to
    this-one wait-turned
    L-8008A2D8
    L-8008A38C
    this-one 351472 255808 walk-to  this-one wait-stop
    this-one 351408 254912 walk-to  this-one wait-stop
    L-80096D04
    60 frames wait
    this-one 0 0 turn-to
    this-one wait-turned
    L-80096D04
    this-one 349712 256256 walk-to  this-one wait-stop
    this-one 344240 256608 walk-to  this-one wait-stop
    L-800989B4
    L-800989A8
    L-80096EF4
    120 frames wait
    goto L-80098A08
    ;

: L-8008A24C
    this-one 1024 0 turn-to
    L-80096D04
    end-script
    ;

\ $8008DA20
: route-8008DA20
    120 actor as-this-one
    this-one 341744 156448 walk-to  this-one wait-stop
    L-800989FC
    L-80098A08
    L-8008A554
    L-80098A14
    L-80098A20
    L-8008A240
    L-8008A24C
    this-one arrive
    end-script
    ;

: L-8008A264
    this-one 345000 -4080 292099 set-pos
    this-one 3072 0 turn-to
    L-80096D04
    end-script
    ;

\ $8008DA70
: route-8008DA70
    120 actor as-this-one
    L-8008A264
    L-80098A2C
    L-80098A38
    L-8008A18C
    L-80098A08
    L-800989FC
    this-one 341744 156448 walk-to  this-one wait-stop
    L-80098C50
    this-one arrive
    end-script
    ;

: L-8008A1F4
    this-one 334160 295264 walk-to  this-one wait-stop
    this-one 3072 0 turn-to
    L-80096D04
    end-script
    ;

\ $8008DAC0
: route-8008DAC0
    123 actor as-this-one
    L-80098A38
    L-8008A18C
    L-80098A44
    L-8008A1E8
    L-8008A1F4
    this-one arrive
    end-script
    ;

: L-8008A214
    this-one 333606 -4080 295264 set-pos
    this-one 1024 0 turn-to
    L-80096D04
    goto L-8008A1E8
    ;

\ $8008DAF8
: route-8008DAF8
    123 actor as-this-one
    L-8008A214
    L-80098A44
    L-80098A38
    L-8008ADD0
    L-80098A44
    L-8008A1E8
    L-8008A1F4
    this-one arrive
    end-script
    ;

\ $8008DB40
: route-8008DB40
    123 actor as-this-one
    L-8008A214
    L-8008A240
    L-80098A20
    L-8008AA60
    L-80098A68
    L-8008A1E8
    L-8008A1F4
    this-one arrive
    end-script
    ;

\ $8008DB88
: route-8008DB88
    123 actor as-this-one
    L-8008A214
    L-80098A68
    L-80098A5C
    L-80098A50
    this-one arrive
    end-script
    ;

: L-8008FB2C
    this-one 412288 168048 walk-to  this-one wait-stop
    this-one 412992 167984 walk-to  this-one wait-stop
    this-one 413024 169616 walk-to  this-one wait-stop
    L-800980A8
    goto L-80096D88
    ;

\ $8008FB54
: route-8008FB54
    robert as-this-one
    L-8008FB2C
    this-one 6 360 attach
    this-one wait-follow
    L-80097628
    this-one 593712 132304 walk-to  this-one wait-stop
    this-one 593712 131456 walk-to  this-one wait-stop
    sub-80096D34
    this-one arrive
    end-script
    ;

: L-8008FB00
    L-80098078
    this-one 413024 169616 walk-to  this-one wait-stop
    this-one 412992 167984 walk-to  this-one wait-stop
    this-one 412288 168048 walk-to  this-one wait-stop
    this-one 412464 169056 walk-to  this-one wait-stop
    end-script
    ;

\ $8008FBA4
: route-8008FBA4
    robert as-this-one
    L-80097600
    this-one 15 360 attach
    this-one wait-follow
    L-8008FB00
    this-one arrive
    end-script
    ;

\ $8008FBDC
: route-8008FBDC
    robert as-this-one
    L-8008FB2C
    this-one 96 360 attach
    this-one wait-follow
    winona wait-attached
    this-one 97 360 attach
    this-one wait-follow
    L-8008FB00
    this-one arrive
    end-script
    ;

\ $8008FC30
: route-8008FC30
    robert as-this-one
    L-8008FB2C
    winona wait-attached
    this-one 96 360 attach
    this-one wait-follow
    this-one 97 360 attach
    this-one wait-follow
    L-8008FB00
    this-one arrive
    end-script
    ;

\ $8008FC84
: route-8008FC84
    robert as-this-one
    L-8008FB2C
    this-one 96 360 attach
    this-one wait-follow
    this-one 99 360 attach
    this-one wait-follow
    L-80097A34
    this-one 249136 134000 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008FCD8
: route-8008FCD8
    robert as-this-one
    L-80097968
    L-80096D88
    this-one 42 360 attach
    this-one wait-follow
    L-8008FB00
    this-one arrive
    end-script
    ;

\ $8008FD18
: route-8008FD18
    robert as-this-one
    L-8008FB2C
    this-one 165 360 attach
    this-one wait-follow
    this-one 337424 436800 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008FD50
: route-8008FD50
    robert as-this-one
    L-80096D88
    winona wait-attached
    this-one 153 360 attach
    this-one wait-follow
    60 frames wait
    this-one 166 360 attach
    this-one wait-follow
    L-8008FB00
    this-one arrive
    end-script
    ;

\ $8008FDAC
: route-8008FDAC
    robert as-this-one
    L-8008FB2C
    this-one 168 360 attach
    this-one wait-follow
    winona wait-attached
    this-one 154 360 attach
    this-one wait-follow
    this-one 337424 436800 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008FE00
: route-8008FE00
    robert as-this-one
    L-80096D88
    winona wait-attached
    this-one 169 360 attach
    this-one wait-follow
    60 frames wait
    this-one 34 360 attach
    this-one wait-follow
    L-8008FB00
    this-one arrive
    end-script
    ;

\ $8008FE5C
: route-8008FE5C
    robert as-this-one
    L-8008FB2C
    this-one 168 360 attach
    this-one wait-follow
    winona wait-attached
    this-one 166 360 attach
    this-one wait-follow
    50 frames wait
    this-one 43 360 attach
    this-one wait-follow
    L-80097EB4
    this-one 177744 423312 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008FED4
: route-8008FED4
    robert as-this-one
    L-80097ED4
    this-one 155 360 attach
    this-one wait-follow
    L-8008FB00
    this-one arrive
    end-script
    ;

\ $8008FF0C
: route-8008FF0C
    robert as-this-one
    L-8008FB2C
    this-one 121 360 attach
    this-one wait-follow
    winona wait-attached
    this-one 179 360 attach
    this-one wait-follow
    this-one 112496 353760 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8008FF60
: route-8008FF60
    robert as-this-one
    L-80096D88
    winona wait-attached
    this-one 100 360 attach
    this-one wait-follow
    80 frames wait
    this-one 63 360 attach
    this-one wait-follow
    50 frames wait
    this-one 34 360 attach
    this-one wait-follow
    L-8008FB00
    this-one arrive
    end-script
    ;

: L-80090110
    L-80098060
    this-one 335104 269456 walk-to  this-one wait-stop
    goto L-800989CC
    ;

: L-80090138
    this-one 404992 276960 walk-to  this-one wait-stop
    this-one 408160 309280 walk-to  this-one wait-stop
    this-one 431744 313376 walk-to  this-one wait-stop
    this-one 466784 293568 walk-to  this-one wait-stop
    goto L-800977D0
    ;

\ $80090160
: route-80090160
    rolein as-this-one
    this-one 188 actor-model
    L-80090110
    L-80090138
    this-one arrive
    end-script
    ;

: L-80098E9C
    this-one 430208 313920 walk-to  this-one wait-stop
    this-one 409120 310592 walk-to  this-one wait-stop
    this-one 404992 276960 walk-to  this-one wait-stop
    goto L-800989CC
    ;

: L-80090128
    this-one 335104 269456 walk-to  this-one wait-stop
    goto L-80098048
    ;

\ $80090188
: route-80090188
    rolein as-this-one
    L-800977F0
    L-80098E9C
    L-80090128
    this-one arrive
    end-script
    ;

\ $800901B0
: route-800901B0
    rolein as-this-one
    this-one 188 actor-model
    L-80090110
    L-800989B4
    L-80098A20
    100 frames wait
    this-one 7 1 76 act-command
    this-one wait-stop
    this-one 7 2 65 lift
    this-one wait-follow
    L-80098824
    this-one arrive
    end-script
    ;

\ $80090210
: route-80090210
    rolein as-this-one
    L-800987D4
    this-one 2 1 76 act-command
    this-one wait-stop
    this-one 2 7 65 lift
    this-one wait-follow
    L-80098A68
    L-800989CC
    L-80090138
    this-one arrive
    end-script
    ;

: L-80090260
    this-one arrive
    end-script
    ;

: L-80098CDC
    L-80098A20
    L-80098A2C
    this-one 341952 319744 walk-to  this-one wait-stop
    this-one 342080 383888 walk-to  this-one wait-stop
    this-one 332672 402512 walk-to  this-one wait-stop
    this-one 330192 424480 walk-to  this-one wait-stop
    this-one 342512 432528 walk-to  this-one wait-stop
    this-one 374736 423728 walk-to  this-one wait-stop
    this-one 378768 423568 walk-to  this-one wait-stop
    end-script
    ;

\ $80090268
: route-80090268
    rolein as-this-one
    this-one 188 actor-model
    main-exe $80030F84 native-result
    0 result= if  goto L-80090260  then
    L-80090110
    L-800989B4
    L-80098A20
    L-80098CDC
    this-one 382368 423280 walk-to  this-one wait-stop
    this-one 408480 429184 walk-to  this-one wait-stop
    this-one 446752 416704 walk-to  this-one wait-stop
    this-one 459264 417280 walk-to  this-one wait-stop
    this-one 463856 413408 walk-to  this-one wait-stop
    this-one 468496 412176 walk-to  this-one wait-stop
    this-one 47 -2 through-door
    this-one arrive
    end-script
    ;

: L-80098E58
    this-one 459264 417280 walk-to  this-one wait-stop
    this-one 495360 417648 walk-to  this-one wait-stop
    this-one 492384 406624 walk-to  this-one wait-stop
    this-one 458048 382144 walk-to  this-one wait-stop
    this-one 447120 363072 walk-to  this-one wait-stop
    this-one 447840 345184 walk-to  this-one wait-stop
    this-one 467136 315664 walk-to  this-one wait-stop
    this-one 469920 302720 walk-to  this-one wait-stop
    end-script
    ;

\ $800902F0
: route-800902F0
    rolein as-this-one
    this-one 327913 0 269469 set-pos
    main-exe $80030F84 native-result
    0 result= if  goto route-80090160  then
    this-one 45 0 through-door
    this-one 466915 0 412173 set-pos
    L-80098E58
    L-800977D0
    this-one arrive
    end-script
    ;

: L-8009062C
    sub-80096D74
    this-one 98704 358640 walk-to  this-one wait-stop
    goto L-800974E8
    ;

\ $80090674
: route-80090674
    samuel as-this-one
    samuel 186 actor-model
    L-8009062C
    this-one 100664 355540 walk-to
    this-one wait-stop
    40 frames wait
    this-one 100074 355600 walk-to
    this-one wait-stop
    this-one 100173 356954 walk-to
    this-one wait-stop
    this-one 3049 -250 turn-to
    this-one arrive
    end-script
    ;

\ $800906DC
: route-800906DC
    samuel as-this-one
    this-one 100105 354976 walk-to
    this-one wait-stop
    this-one 101671 355041 walk-to
    this-one wait-stop
    L-800973B4
    L-80097550
    L-80096D88
    this-one 4 360 attach
    this-one wait-follow
    L-80097628
    this-one 593222 132384 walk-to
    this-one wait-stop
    this-one 593183 132896 walk-to
    this-one wait-stop
    sub-80096D34
    this-one arrive
    end-script
    ;

: L-80097154
    this-one 100400 358784 walk-to  this-one wait-stop
    this-one 18 0 through-door
    end-script
    ;

: L-8009716C
    L-80097090
    goto L-80097154
    ;

: L-80090660
    L-8009716C
    this-one 98608 358512 walk-to  this-one wait-stop
    end-script
    ;

\ $80090764
: route-80090764
    samuel as-this-one
    L-80097600
    samuel 5 actor-model
    this-one 36 360 attach
    this-one wait-follow
    L-80097520
    L-80090660
    this-one arrive
    end-script
    ;

: L-80097504
    this-one 100160 358848 walk-to  this-one wait-stop
    this-one 18 1 through-door
    goto L-80097090
    ;

\ $800907A8
: route-800907A8
    samuel as-this-one
    L-80097504
    this-one 102208 359392 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80090644
    L-80097538
    this-one 98816 358528 walk-to  this-one wait-stop
    this-one 98800 359264 walk-to  this-one wait-stop
    end-script
    ;

\ $800907C8
: route-800907C8
    samuel as-this-one
    L-80097090
    L-8009709C
    30 frames wait
    L-80097130
    L-800971E8
    L-80090644
    this-one arrive
    end-script
    ;

\ $80090808
: route-80090808
    samuel as-this-one
    samuel 186 actor-model
    sub-80096D74
    this-one 98704 358640 walk-to  this-one wait-stop
    L-800974E8
    L-800973B4
    L-80097550
    L-80096D88
    this-one 116 360 attach
    this-one wait-follow
    this-one 222144 407568 walk-to  this-one wait-stop
    L-80096D04
    this-one 222192 406064 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8009087C
: route-8009087C
    samuel as-this-one
    this-one 222048 407136 walk-to  this-one wait-stop
    sub-80096D1C
    L-80096D88
    samuel 5 actor-model
    this-one 115 360 attach
    this-one wait-follow
    L-80097520
    L-800971E8
    L-80090644
    this-one arrive
    end-script
    ;

\ $800908D8
: route-800908D8
    samuel as-this-one
    L-8009062C
    L-800973B4
    L-800971D4
    this-one 104192 360128 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090908
: route-80090908
    samuel as-this-one
    L-80097118
    L-800971E8
    this-one 101216 355984 walk-to  this-one wait-stop
    200 frames wait
    this-one 101200 355392 walk-to  this-one wait-stop
    this-one 99952 355536 walk-to  this-one wait-stop
    this-one 100320 357744 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090950
: route-80090950
    samuel as-this-one
    this-one 99952 355536 walk-to  this-one wait-stop
    L-800970A8
    L-80090644
    this-one arrive
    end-script
    ;

\ $80090978
: route-80090978
    samuel as-this-one
    L-8009062C
    L-800973B4
    L-80090660
    this-one arrive
    end-script
    ;

\ $800909A0
: route-800909A0
    samuel as-this-one
    L-80097154
    this-one 98608 358512 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800909C0
: route-800909C0
    samuel as-this-one
    L-80097504
    L-8009709C
    L-800971E8
    L-80090644
    this-one arrive
    end-script
    ;

\ $800909F0
: route-800909F0
    samuel as-this-one
    L-80097118
    L-800971E8
    this-one 101536 353104 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090A18
: route-80090A18
    samuel as-this-one
    L-800970A8
    L-80090644
    this-one arrive
    end-script
    ;

\ $80090A38
: route-80090A38
    samuel as-this-one
    L-8009062C
    this-one 100208 355264 walk-to  this-one wait-stop
    this-one 100064 355776 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090A60
: route-80090A60
    samuel as-this-one
    this-one 100352 355328 walk-to  this-one wait-stop
    L-800970A8
    L-80097318
    this-one 100416 350752 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090A90
: route-80090A90
    samuel as-this-one
    L-80097338
    L-800973B4
    L-800971D4
    this-one 105088 359696 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090AC0
: route-80090AC0
    samuel as-this-one
    L-80097118
    L-800971E8
    L-80090644
    this-one arrive
    end-script
    ;

\ $80090D60
: route-80090D60
    17 actor arrive
    end-script
    ;

\ $80090E0C
: route-80090E0C
    steve as-this-one
    L-80097ED4
    L-80096D88
    this-one 5 360 attach
    this-one wait-follow
    L-80097628
    this-one 593216 132544 walk-to  this-one wait-stop
    this-one 593200 131328 walk-to  this-one wait-stop
    this-one 976 -250 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $80090E68
: route-80090E68
    steve as-this-one
    L-80097600
    this-one 41 360 attach
    this-one wait-follow
    L-80097A34
    this-one 250368 134944 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090EA0
: route-80090EA0
    steve as-this-one
    L-80097968
    L-80096D88
    this-one 40 360 attach
    this-one wait-follow
    L-80097EB4
    this-one 178112 424032 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090EE0
: route-80090EE0
    steve as-this-one
    L-80097ED4
    L-80096D88
    this-one 44 360 attach
    this-one wait-follow
    L-80097A34
    this-one 250368 134944 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80090F20
: route-80090F20
    steve as-this-one
    L-80097ED4
    L-80096D88
    this-one 151 360 attach
    this-one wait-follow
    L-80098D28
    350 frames wait
    this-one 352608 264352 walk-to  this-one wait-stop
    this-one 350016 260016 walk-to  this-one wait-stop
    this-one 3036 200 turn-to
    this-one wait-turned
    L-80096EF4
    350 frames wait
    this-one 40 0 anim
    150 frames wait
    this-one 41 0 anim
    this-one wait-anim
    this-one arrive
    end-script
    ;

: L-80096F58
    this-one 14 15 1 anim-then
    30 frames wait
    end-script
    ;

\ $80090FB4
: route-80090FB4
    steve as-this-one
    this-one 352496 258192 walk-to  this-one wait-stop
    this-one 357840 258176 walk-to  this-one wait-stop
    L-80096F58
    this-one 6002 260 turn-to
    this-one wait-turned
    100 frames wait
    this-one 81 1 anim
    80 frames wait
    this-one 361056 259920 walk-to  this-one wait-stop
    this-one 360928 264032 walk-to  this-one wait-stop
    this-one 357264 263840 walk-to  this-one wait-stop
    this-one 46 250 turn-to
    sub-80096D34
    400 frames wait
    L-80098E24
    L-80096EF4
    100 frames wait
    L-80096D88
    this-one 73 360 attach
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $80091084
: route-80091084
    steve as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 109 360 attach
    this-one wait-follow
    L-80097A34
    this-one 250368 134944 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80098108
    L-80098078
    this-one 412720 170912 walk-to  this-one wait-stop
    end-script
    ;

\ $800910CC
: route-800910CC
    steve as-this-one
    L-80097968
    L-80096D88
    this-one 42 360 attach
    this-one wait-follow
    L-80098108
    this-one arrive
    end-script
    ;

\ $80091104
: route-80091104
    steve as-this-one
    L-80096D88
    this-one 43 360 attach
    this-one wait-follow
    L-80097EB4
    this-one 178112 424032 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8009113C
: route-8009113C
    steve as-this-one
    L-80097ED4
    L-80096D88
    this-one 152 360 attach
    this-one wait-follow
    this-one 466960 293216 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091174
: route-80091174
    steve as-this-one
    L-80096D88
    this-one 150 360 attach
    this-one wait-follow
    L-80097EB4
    this-one 178112 424032 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800911AC
: route-800911AC
    steve as-this-one
    sub-80096FB0
    L-80098060
    L-80096D88
    this-one 67 360 attach
    this-one wait-follow
    L-80097628
    this-one 593216 132544 walk-to  this-one wait-stop
    this-one 593200 131328 walk-to  this-one wait-stop
    this-one 976 -250 turn-to
    sub-80096D34
    this-one arrive
    end-script
    ;

\ $80091210
: route-80091210
    steve as-this-one
    L-80097600
    this-one 15 360 attach
    this-one wait-follow
    L-80098090
    300 frames wait
    L-80096D88
    this-one 43 360 attach
    this-one wait-follow
    L-80097EB4
    this-one 178112 424032 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-800980C0
    L-80098078
    this-one 413632 170128 walk-to  this-one wait-stop
    this-one 3084 0 turn-to
    sub-80096D34
    end-script
    ;

\ $800914FC
: route-800914FC
    winona as-this-one
    this-one 178624 423856 walk-to  this-one wait-stop
    this-one 175584 423504 walk-to  this-one wait-stop
    this-one 27 0 through-door
    this-one 175696 419344 walk-to  this-one wait-stop
    this-one 4 1 76 act-command
    this-one wait-stop
    this-one 4 6 44 lift
    this-one wait-follow
    L-800980C0
    this-one arrive
    end-script
    ;

: L-800980E8
    sub-80096D74
    this-one 412080 169648 walk-to  this-one wait-stop
    this-one 67 0 through-door
    end-script
    ;

\ $80091558
: route-80091558
    winona as-this-one
    L-800980E8
    this-one 6 1 76 act-command
    this-one wait-stop
    this-one 6 5 44 lift
    this-one wait-follow
    L-80098048
    this-one -1 take-diner-seat
    L-80096F80
    this-one arrive
    end-script
    ;

\ $800915A8
: route-800915A8
    winona as-this-one
    sub-80096FB0
    L-80098060
    this-one 5 1 76 act-command
    this-one wait-stop
    this-one 5 9 44 lift
    L-80097994
    this-one 250448 135232 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800915F4
: route-800915F4
    winona as-this-one
    L-80097968
    this-one 9 2 44 lift
    this-one wait-follow
    L-800987F8
    this-one arrive
    end-script
    ;

\ $80091628
: route-80091628
    winona as-this-one
    L-800987D4
    robert wait-follow
    this-one 30 1 80 act-command
    300 frames wait
    this-one wait-follow
    20 frames wait
    L-800980C0
    this-one arrive
    end-script
    ;

\ $8009166C
: route-8009166C
    winona as-this-one
    L-800980E8
    this-one 30 1 80 act-command
    500 frames wait
    this-one wait-follow
    L-800987F8
    this-one 268480 92032 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800916A8
: route-800916A8
    winona as-this-one
    L-800987D4
    this-one 2 1 76 act-command
    this-one wait-stop
    this-one 2 6 44 lift
    this-one wait-follow
    L-800980C0
    this-one arrive
    end-script
    ;

: L-800914B0
    this-one 346240 284320 walk-to  this-one wait-stop
    this-one 347696 284528 walk-to  this-one wait-stop
    this-one 347712 284912 walk-to  this-one wait-stop
    this-one 348880 285696 walk-to  this-one wait-stop
    end-script
    ;

\ $800916E8
: route-800916E8
    winona as-this-one
    L-800980E8
    this-one 6 1 76 act-command
    this-one wait-stop
    this-one 6 7 38 lift
    this-one wait-follow
    L-800914B0
    this-one arrive
    end-script
    ;

: L-800914D4
    this-one 347712 284912 walk-to  this-one wait-stop
    this-one 347696 284528 walk-to  this-one wait-stop
    this-one 346240 284320 walk-to  this-one wait-stop
    this-one 7 1 76 act-command
    this-one wait-stop
    end-script
    ;

\ $80091728
: route-80091728
    winona as-this-one
    L-800914D4
    this-one 7 1 44 lift
    this-one wait-follow
    this-one 131056 309376 walk-to  this-one wait-stop
    this-one 131360 316672 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091764
: route-80091764
    winona as-this-one
    this-one 134432 308336 walk-to  this-one wait-stop
    this-one 1 1 76 act-command
    this-one wait-stop
    this-one 1 2 44 lift
    this-one wait-follow
    L-800987F8
    this-one arrive
    end-script
    ;

\ $800917A4
: route-800917A4
    winona as-this-one
    L-800987D4
    this-one 2 1 76 act-command
    this-one wait-stop
    this-one 2 9 44 lift
    this-one wait-follow
    L-80097994
    this-one 250448 135232 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800917EC
: route-800917EC
    winona as-this-one
    L-80097968
    this-one 9 4 44 lift
    this-one wait-follow
    this-one 175696 418016 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091820
: route-80091820
    winona as-this-one
    L-80097EB4
    this-one 176160 424304 walk-to  this-one wait-stop
    this-one 1451 200 turn-to
    this-one wait-turned
    this-one arrive
    end-script
    ;

\ $80091850
: route-80091850
    winona as-this-one
    L-80097ED4
    this-one 4 1 76 act-command
    this-one wait-stop
    this-one 4 6 44 lift
    this-one wait-follow
    L-800980C0
    this-one arrive
    end-script
    ;

\ $80091890
: route-80091890
    winona as-this-one
    L-800980E8
    this-one 30 1 91 act-command
    this-one wait-stop
    this-one 30 1 80 act-command
    300 frames wait
    this-one wait-follow
    L-800987F8
    this-one arrive
    end-script
    ;

\ $800918D0
: route-800918D0
    winona as-this-one
    L-800980E8
    this-one 6 1 76 act-command
    this-one wait-stop
    this-one 6 7 44 lift
    this-one wait-follow
    L-80098CDC
    this-one 382464 425104 walk-to  this-one wait-stop
    this-one 378928 431472 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091920
: route-80091920
    winona as-this-one
    this-one 376272 425360 walk-to  this-one wait-stop
    this-one 379152 423440 walk-to  this-one wait-stop
    this-one 373952 423600 walk-to  this-one wait-stop
    this-one 348080 432816 walk-to  this-one wait-stop
    this-one 336752 430448 walk-to  this-one wait-stop
    L-80097B74
    this-one 334608 441408 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80091968
    this-one 383584 145792 walk-to  this-one wait-stop
    L-80096F3C
    this-one 383584 147168 walk-to  this-one wait-stop
    L-80096F3C
    this-one 384384 147360 walk-to  this-one wait-stop
    L-80096F3C
    L-80096F58
    this-one 386448 147296 walk-to  this-one wait-stop
    L-80096F3C
    L-80096F58
    this-one 388368 147232 walk-to  this-one wait-stop
    L-80096F3C
    L-80096F58
    this-one 389472 147200 walk-to  this-one wait-stop
    this-one 389472 146416 walk-to  this-one wait-stop
    L-80096F3C
    this-one -90 -200 turn-by
    this-one wait-turned
    this-one 44 anim-once
    25 frames wait
    this-one 45 0 anim
    this-one wait-anim
    this-one 90 200 turn-by
    this-one wait-turned
    this-one 389472 145824 walk-to  this-one wait-stop
    L-80096F58
    50 frames wait
    this-one 90 200 turn-by
    this-one wait-turned
    this-one 388640 145824 walk-to  this-one wait-stop
    L-80096F3C
    L-80096F58
    this-one 386672 145808 walk-to  this-one wait-stop
    L-80096F3C
    L-80096F58
    this-one 384720 145808 walk-to  this-one wait-stop
    L-80096F3C
    L-80096F58
    this-one 385552 145760 walk-to  this-one wait-stop
    this-one 385552 144688 walk-to  this-one wait-stop
    this-one 386432 144704 walk-to  this-one wait-stop
    L-80096F3C
    this-one 387296 144064 walk-to  this-one wait-stop
    this-one 2080 200 turn-to
    this-one wait-turned
    this-one 32 anim-once
    45 frames wait
    this-one 33 0 anim
    this-one wait-anim
    this-one 388544 144880 walk-to  this-one wait-stop
    this-one 54 -200 turn-to
    this-one wait-turned
    this-one 32 anim-once
    45 frames wait
    this-one 33 0 anim
    this-one wait-anim
    L-80096EF4
    250 frames wait
    this-one 383744 144512 walk-to  this-one wait-stop
    L-80096EF4
    250 frames wait
    end-script
    ;

\ $80091B54
: route-80091B54
    winona as-this-one
    L-80097B8C
    this-one 30 1 91 act-command
    this-one wait-stop
    this-one 30 1 80 act-command
    500 frames wait
    this-one wait-follow
    L-80097C04
    L-80097C10
    L-80097C54
    this-one 383648 144608 walk-to  this-one wait-stop
    L-80096F3C
    L-80091968
    L-80091968
    this-one arrive
    end-script
    ;

: L-80098C68
    this-one 367280 125392 walk-to  this-one wait-stop
    end-script
    ;

\ $80091BC4
: route-80091BC4
    winona as-this-one
    L-80097C34
    L-80097C74
    L-80098C68
    L-80098C50
    L-80098BA8
    L-80098C8C
    L-80096F08
    this-one 2 1 76 act-command
    this-one wait-stop
    this-one 2 9 44 lift
    this-one wait-follow
    L-80097994
    this-one 250448 135232 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091C3C
: route-80091C3C
    winona as-this-one
    L-80097968
    this-one 9 2 44 lift
    this-one wait-follow
    L-800987F8
    this-one arrive
    end-script
    ;

\ $80091C70
: route-80091C70
    winona as-this-one
    L-800987D4
    L-80098B6C
    L-80096F24
    L-80098B78
    L-80098BD8
    L-80098BE4
    this-one 337568 59488 walk-to  this-one wait-stop
    this-one 337360 65488 walk-to  this-one wait-stop
    this-one 3010 0 turn-to
    this-one 65 0 anim
    40 frames wait
    this-one 65 0 anim
    20 frames wait
    this-one 65 0 anim
    40 frames wait
    this-one 82 0 anim
    100 frames wait
    this-one 340016 69808 walk-to  this-one wait-stop
    this-one 3 1 76 act-command
    this-one wait-stop
    this-one 3 6 44 lift
    this-one wait-follow
    L-800980C0
    this-one arrive
    end-script
    ;

: L-80097CF8
    L-80097C04
    L-80097C10
    goto L-80097C54
    ;

\ $80091D4C
: route-80091D4C
    winona as-this-one
    L-800980E8
    this-one 6 1 76 act-command
    this-one wait-stop
    this-one 6 3 44 lift
    this-one wait-follow
    L-80098C50
    L-80098C68
    L-80097CF8
    this-one 384288 144832 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-80097CE8
    L-80097C34
    goto L-80097C74
    ;

\ $80091DA4
: route-80091DA4
    winona as-this-one
    L-80097CE8
    robert wait-follow
    this-one 30 1 91 act-command
    this-one wait-stop
    this-one 30 1 80 act-command
    300 frames wait
    this-one wait-follow
    L-80097B74
    this-one 334608 441408 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091DF4
: route-80091DF4
    winona as-this-one
    L-80097B8C
    this-one 30 1 91 act-command
    this-one wait-stop
    this-one 30 1 80 act-command
    500 frames wait
    this-one wait-follow
    this-one 335328 271296 walk-to  this-one wait-stop
    L-80098A68
    L-80098A20
    L-80097F0C
    this-one 345888 285856 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091E54
: route-80091E54
    winona as-this-one
    L-80097EF4
    this-one 7 9 48 lift
    this-one wait-follow
    L-80097994
    this-one 250448 135232 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091E90
: route-80091E90
    winona as-this-one
    L-80097968
    this-one 9 3 45 lift
    this-one wait-follow
    L-80098C50
    L-80098C68
    L-80097CF8
    this-one 384288 144832 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091EDC
: route-80091EDC
    winona as-this-one
    L-80097CE8
    robert wait-follow
    this-one 30 1 91 act-command
    this-one wait-stop
    this-one 30 1 80 act-command
    300 frames wait
    this-one wait-follow
    20 frames wait
    L-800980C0
    this-one arrive
    end-script
    ;

\ $80091F2C
: route-80091F2C
    winona as-this-one
    L-800980E8
    this-one 6 1 76 act-command
    this-one wait-stop
    this-one 6 2 44 lift
    this-one wait-follow
    L-800987F8
    this-one arrive
    end-script
    ;

\ $80091F6C
: route-80091F6C
    winona as-this-one
    L-800987D4
    this-one 2 1 76 act-command
    this-one wait-stop
    this-one 2 9 44 lift
    this-one wait-follow
    L-80097994
    this-one 250448 135232 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80091FB4
: route-80091FB4
    winona as-this-one
    L-80097968
    this-one 9 7 45 lift
    this-one wait-follow
    L-80098CDC
    this-one 382368 423280 walk-to  this-one wait-stop
    this-one 408480 429184 walk-to  this-one wait-stop
    this-one 446752 416704 walk-to  this-one wait-stop
    this-one 459264 417280 walk-to  this-one wait-stop
    this-one 463488 413360 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80092010
: route-80092010
    winona as-this-one
    robert wait-follow
    this-one 30 1 91 act-command
    this-one wait-stop
    this-one 30 1 80 act-command
    300 frames wait
    this-one wait-follow
    L-80097520
    L-800971E8
    this-one 101392 349856 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80092060
: route-80092060
    winona as-this-one
    L-800970A8
    L-800973B4
    L-80097550
    this-one 30 1 91 act-command
    this-one wait-stop
    this-one 30 1 80 act-command
    300 frames wait
    this-one wait-follow
    L-800977D0
    this-one arrive
    end-script
    ;

: L-800920B0
    this-one 10046 7 talk
    this-one 1 ride
    this-one restore-state
    this-one reset
    main-exe $80032CE4 native
    end-script
    ;

: L-800920D0
    10045 1 choice
    0 result= if  goto L-800920B0  then
    matthew 10047 7 talk
    this-one 10048 6 talk
    40 frames wait
    this-one 10049 1 talk
    this-one restore-state
    end-script
    ;

\ $80092104
: exe-80092104
    winona as-this-one
    this-one save-state
    this-one 10043 7 talk
    this-one 10044 1 talk
    goto L-800920D0
    ;

\ $80092120
: exe-80092120
    winona as-this-one
    this-one save-state
    this-one 10050 1 talk
    goto L-800920D0
    ;

: L-8009245C
    this-one 466576 289392 walk-to  this-one wait-stop
    this-one 466576 290176 walk-to  this-one wait-stop
    this-one 468288 290448 walk-to  this-one wait-stop
    L-800977B0
    goto L-80096D88
    ;

\ $80092484
: route-80092484
    wolf as-this-one
    this-one 189 actor-model
    L-8009245C
    this-one 88 360 attach
    this-one wait-follow
    L-80097028
    this-one arrive
    end-script
    ;

: L-80092430
    L-80097780
    L-8009778C
    this-one 466496 290032 walk-to  this-one wait-stop
    this-one 466592 289152 walk-to  this-one wait-stop
    this-one 469040 289168 walk-to  this-one wait-stop
    end-script
    ;

\ $800924BC
: route-800924BC
    wolf as-this-one
    L-80096D88
    this-one 87 360 attach
    this-one wait-follow
    L-80092430
    this-one arrive
    end-script
    ;

\ $800924EC
: route-800924EC
    wolf as-this-one
    this-one 189 actor-model
    L-8009245C
    this-one 140 360 attach
    this-one wait-follow
    L-800984AC
    this-one 307984 306368 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $8009252C
: route-8009252C
    wolf as-this-one
    L-80098524
    this-one 141 360 attach
    this-one wait-follow
    L-80092430
    this-one arrive
    end-script
    ;

\ $8009255C
: route-8009255C
    wolf as-this-one
    this-one 142 360 attach
    this-one wait-follow
    L-800984AC
    this-one 307984 306368 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

: L-800923F4
    L-80097B74
    this-one 332336 439392 walk-to  this-one wait-stop
    this-one 1932 -240 turn-to
    this-one wait-turned
    L-80096EF4
    250 frames wait
    this-one 335520 440560 walk-to  this-one wait-stop
    end-script
    ;

\ $8009258C
: route-8009258C
    wolf as-this-one
    L-80098524
    this-one 143 360 attach
    this-one wait-follow
    L-800923F4
    this-one arrive
    end-script
    ;

\ $800925BC
: route-800925BC
    wolf as-this-one
    this-one 333536 439648 walk-to  this-one wait-stop
    L-80097BA4
    this-one 144 360 attach
    this-one wait-follow
    this-one 469840 292608 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $800925F4
: route-800925F4
    wolf as-this-one
    L-80092430
    this-one arrive
    end-script
    ;

\ $80092634
: route-80092634
    wolf as-this-one
    this-one 50 0 through-door
    this-one 469040 289168 walk-to  this-one wait-stop
    this-one arrive
    end-script
    ;

\ $80092694
: route-80092694
    wolf as-this-one
    L-80098524
    this-one 139 360 attach
    this-one wait-follow
    this-one 102720 53728 walk-to  this-one wait-stop
    this-one 112624 51328 walk-to  this-one wait-stop
    this-one 32 -2 through-door
    this-one 113817 0 52532 set-pos
    this-one arrive
    end-script
    ;

\ $800926E8
: route-800926E8
    wolf as-this-one
    L-80083680
    this-one 146 360 attach
    this-one wait-follow
    L-800923F4
    this-one arrive
    end-script
    ;

: L-800923D4
    this-one 464272 293280 walk-to  this-one wait-stop
    this-one 464336 288336 walk-to  this-one wait-stop
    this-one 53 -2 through-door
    end-script
    ;

\ $80092718
: route-80092718
    wolf as-this-one
    this-one 333536 439648 walk-to  this-one wait-stop
    L-80097BA4
    this-one 144 360 attach
    this-one wait-follow
    L-800923D4
    this-one arrive
    end-script
    ;

\ $80092750
: route-80092750
    wolf as-this-one
    L-800984E8
    this-one 305120 309248 walk-to  this-one wait-stop
    L-80096F24
    this-one 305344 300576 walk-to  this-one wait-stop
    L-80096F24
    this-one 311648 300704 walk-to  this-one wait-stop
    L-80096F24
    L-80096F3C
    this-one 316256 300528 walk-to  this-one wait-stop
    L-80096F24
    this-one 316304 304224 walk-to  this-one wait-stop
    L-80096F24
    L-80096F3C
    this-one 316416 314048 walk-to  this-one wait-stop
    L-80096F24
    this-one 311488 313360 walk-to  this-one wait-stop
    this-one 310800 308832 walk-to  this-one wait-stop
    L-80096F24
    this-one 305440 308976 walk-to  this-one wait-stop
    this-one 305328 312592 walk-to  this-one wait-stop
    L-80096D88
    this-one 141 360 attach
    this-one wait-follow
    L-800923D4
    this-one arrive
    end-script
    ;

: L-80096138
    -474 -2210 -1106 0 0 0 260 camera-set
    end-script
    ;

\ $80096178
: exe-80096178
    matthew as-this-one
    this-one save-state
    L-80096138
    matthew 0 1 anim
    this-one camera-at
    this-one 0 0 face
    this-one 0 0 talk
    this-one restore-state
    end-script
    ;

\ $800961D4
: exe-800961D4
    matthew as-this-one
    this-one freeze
    L-80096138
    matthew 0 1 anim
    this-one camera-at
    this-one 0 0 face
    this-one 0 0 talk
    this-one unfreeze
    end-script
    ;

\ $80096230
: exe-80096230
    matthew as-this-one
    this-one save-state
    this-one 0 0 talk
    this-one restore-state
    end-script
    ;

\ $8009625C
: exe-8009625C
    matthew as-this-one
    this-one freeze
    L-80096138
    this-one camera-at
    this-one 0 0 talk
    this-one unfreeze
    end-script
    ;

\ $80096298
: exe-80096298
    matthew as-this-one
    L-80096138
    this-one camera-at
    this-one 0 0 talk
    end-script
    ;

\ $800963FC
: exe-800963FC
    118 -1370 -2450 0 0 0 1761 camera-set
    winona camera-at
    winona 10052 1 talk
    winona arrive
    end-script
    ;

\ $8009643C
: exe-8009643C
    118 -1370 -2450 0 0 0 1761 camera-set
    winona camera-at
    winona 10055 2 talk
    matthew 10056 7 talk
    winona 10057 6 talk
    main-exe $80047B7C native
    end-script
    ;

\ $80096598
: exe-80096598
    9 screen-effect
    wait-effect
    main-exe $80047620 native
    end-script
    ;

\ $80096A34
: exe-80096A34
    matthew as-this-one
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    3 frames wait
    matthew 2 -20 move-forward
    16 frames wait
    matthew idle
    this-one 0 0 through-door-back
    this-one 10013 1 talk
    this-one door-hold
    end-script
    ;

\ $80096A94
: exe-80096A94
    matthew as-this-one
    matthew 0 1 anim
    matthew this-one camera-face
    4 0 camera-turn
    this-one 0 6 talk
    this-one 0 0 through-door
    this-one place-at-stop
    main-exe $80047614 native
    end-script
    ;

\ $80096AE8
: exe-80096AE8
    dennis as-this-one
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    3 frames wait
    matthew 2 -20 move-forward
    16 frames wait
    matthew idle
    this-one 92274 -200 53152 set-pos
    this-one -200 set-y
    this-one 5056 0 turn-to
    2 object-anim
    sub-80096D1C
    this-one 10016 5 talk
    this-one door-hold
    end-script
    ;

\ $80096B70
: exe-80096B70
    dennis as-this-one
    matthew 0 1 anim
    matthew this-one camera-face
    4 0 camera-turn
    this-one 10011 1 talk
    this-one 92627 -200 53149 set-pos
    this-one 3058 0 turn-to
    3 object-anim
    sub-80096D1C
    this-one place-at-stop
    main-exe $80047614 native
    end-script
    ;

\ $80096BE4
: exe-80096BE4
    winona 54 1 talk
    end-script
    ;

\ $80096BF8
: exe-80096BF8
    winona 55 1 talk
    end-script
    ;

\ $80096C0C
: exe-80096C0C
    sub-80096E34
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    0 matthew say-by-wait
    end-script
    ;

: L-80096DC8
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    end-script
    ;

\ $80096C40
: exe-80096C40
    2 door-sound
    L-80096DC8
    63 say-wait-close
    -1776 -8064 -436 392730 228 419791 -1385 camera-set
    25 2 caption-for-nowait
    11 screen-effect
    wait-effect
    4 0 365809 387970 1672 -2146426904 warp
    ;

\ $80096CAC
: exe-80096CAC
    1 5 say-mode-wait
    end-script
    ;

: L-80096CBC
    55 frames wait
    11 screen-effect
    10 frames wait
    main-exe $80047C5C native
    10 screen-effect
    wait-effect
    matthew wait-anim
    end-script
    ;

\ $80096CF0
: sub-80096CF0
    ' L-80096CBC start-event
    65 frames wait
    end-script
    ;

\ $80096E74
: sub-80096E74
    -564 -1338 -700 0 0 0 0 camera-set
    matthew camera-at
    matthew camera-heading
    190 0 camera-turn
    end-script
    ;

\ $80096EB4
: sub-80096EB4
    -600 -2526 -1258 0 0 0 0 camera-set
    matthew camera-at
    matthew camera-heading
    90 0 camera-turn
    end-script
    ;

\ $80098EBC
: exe-80098EBC
    10000 0 choice
    main-exe $80047C90 native
    end-script
    ;

: L-80098ED4
    10504 say-wait-close
    picture-hide
    end-script
    ;

\ $80098EE4
: exe-80098EE4
    0 picture-show
    10502 say-wait-close
    10503 0 choice
    0 result= if  goto L-80098ED4  then
    10505 say-wait-close
    picture-hide
    main-exe $8004791C native
    26 actor 0 0 0 set-pos
    end-script
    ;

: L-80098F34
    10511 say-wait-close
    picture-hide
    end-script
    ;

: L-80098F44
    26 actor 349685 0 283687 set-pos
    26 actor -1072 0 turn-to
    goto L-80098F34
    ;

: L-80098F68
    26 actor 249781 0 140655 set-pos
    26 actor 1007 0 turn-to
    goto L-80098F34
    ;

: L-80098F8C
    26 actor 408056 0 163688 set-pos
    26 actor -82 0 turn-to
    goto L-80098F34
    ;

: L-80098FB0
    26 actor 112168 0 353607 set-pos
    26 actor -9244 0 turn-to
    goto L-80098F34
    ;

: L-80098FD4
    26 actor 584585 0 117565 set-pos
    26 actor -7 0 turn-to
    goto L-80098F34
    ;

: L-80098FF8
    26 actor 55993 0 404621 set-pos
    26 actor -8056 0 turn-to
    goto L-80098F34
    ;

: L-8009901C
    26 actor 101759 0 57326 set-pos
    26 actor -10220 0 turn-to
    goto L-80098F34
    ;

: L-80099040
    26 actor 464130 0 417929 set-pos
    26 actor -7198 0 turn-to
    goto L-80098F34
    ;

:noname
    10507 2 choice
    0 result= if  goto L-80098F44  then
    1 result= if  goto L-80098F68  then
    10508 2 choice
    0 result= if  goto L-80098F8C  then
    1 result= if  goto L-80098FB0  then
    10509 2 choice
    0 result= if  goto L-80098FD4  then
    1 result= if  goto L-80098FF8  then
    10510 2 choice
    0 result= if  goto L-8009901C  then
    1 result= if  goto L-80099040  then
    goto L-80099064
    ; is L-80099064

\ $800990FC
: exe-800990FC
    matthew wait-anim
    0 picture-show
    10502 say-wait-close
    10506 say-wait-close
    goto L-80099064
    ;

\ $80099210
: exe-80099210
    2 1 sequence-play
    30 frames wait
    matthew 81 0 anim
    matthew wait-anim
    matthew 104 0 anim
    matthew wait-anim
    2 sequence-stop
    matthew 102 1 anim
    10023 matthew say-by-wait
    0 picture-show
    10024 32767 actor say-by-wait
    picture-hide
    10025 matthew say-by-wait
    0 picture-show
    10026 32767 actor say-by-wait
    picture-hide
    10027 matthew say-by-wait
    0 picture-show
    10028 32767 actor say-by-wait
    picture-hide
    10029 matthew say-by-wait
    10030 matthew say-by-wait
    0 picture-show
    10031 32767 actor say-by-wait
    picture-hide
    10032 matthew say-by-wait
    matthew 103 0 anim-at  matthew wait-anim
    10 frames wait
    9 screen-effect
    wait-effect
    2 -1 -1 -1 0 -2146411136 warp
    ;

\ $80099350
: exe-80099350
    252 4 call-pack-script
    end-script
    ;

\ $80099360
: exe-80099360
    250 3 call-pack-script
    209 210 cd-pictures
    250 4 call-pack-script
    0 cd-picture
    250 5 call-pack-script
    end-script
    ;

\ $8009949C
: sub-8009949C
    139 actor 4535 4519 1 anim-then
    8 frames wait
    morgan 0 1 anim
    morgan 0 0 face
    matthew 111 0 1 anim-then
    109 sound
    12 sound
    256 10 rumble
    255 10 rumble
    20 frames wait
    $80106048 goto-overlay
    ;

\ $80099518
: sub-80099518
    92 2 choice
    0 result= if  $80104A38 goto-overlay  then
    1 result= if  $80104A54 goto-overlay  then
    2 result= if  $80104820 goto-overlay  then
    end-script
    ;

\ $8009954C
: sub-8009954C
    rolein as-this-one
    give-control
    this-one 85 0 anim-at  this-one wait-anim
    this-one 328752 267840 walk-to  this-one wait-stop
    this-one wait-stop
    this-one 327616 267840 walk-to  this-one wait-stop
    this-one wait-stop
    this-one 85 0 through-door
    this-one 327568 269280 walk-to  this-one wait-stop
    this-one wait-stop
    end-script
    ;

\ $800995C0
: sub-800995C0
    give-control
    700 frames wait
    xa-stop
    end-script
    ;

\ $800996E0
: exe-800996E0
    main-exe $8004C640 native
    matthew wait-anim
    picture-hide
    end-script
    ;

\ $800996F4
: exe-800996F4
    10459 5 say-mode-wait
    end-script
    ;

\ $80099704
: exe-80099704
    10460 phone-say
    10461 say-reset
    end-script
    ;

\ $80099718
: exe-80099718
    10462 5 say-mode-wait
    10463 5 say-mode-wait
    end-script
    ;

: L-8009A888
    20 frames wait
    10087 5 say-mode-wait
    10088 5 say-mode-wait
    main-exe $8004F2C8 native
    end-script
    ;

: L-8009A8B4
    10080 say
    wait-text
    10081 say
    wait-text
    10082 say
    wait-text
    10083 say
    wait-text
    10084 say
    wait-text
    10085 say
    wait-text
    10086 say
    wait-text
    main-exe $8004F31C native-result
    0 result= if  goto L-8009A888  then
    end-script
    ;

: L-8009A920
    20 frames wait
    10094 5 say-mode-wait
    10095 5 say-mode-wait
    10096 5 say-mode-wait
    main-exe $8004F2E4 native
    end-script
    ;

: L-8009A958
    10089 say
    wait-text
    10090 say
    wait-text
    10091 say
    wait-text
    10092 say
    wait-text
    10093 say
    wait-text
    main-exe $8004F334 native-result
    0 result= if  goto L-8009A920  then
    end-script
    ;

: L-8009A9AC
    20 frames wait
    10101 5 say-mode-wait
    main-exe $8004F300 native
    end-script
    ;

: L-8009A9CC
    10097 say
    wait-text
    10098 say
    wait-text
    10099 say
    wait-text
    10100 say
    wait-text
    main-exe $8004F34C native-result
    0 result= if  goto L-8009A9AC  then
    end-script
    ;

\ $8009AA14
: exe-8009AA14
    15 picture-show
    10078 32767 actor say-by-wait
    10079 2 choice
    picture-hide
    0 result= if  goto L-8009A8B4  then
    1 result= if  goto L-8009A958  then
    2 result= if  goto L-8009A9CC  then
    end-script
    ;

\ $8009AA60
: exe-8009AA60
    203 204 cd-pictures
    10499 say
    wait-text
    0 cd-picture
    end-script
    ;

\ $8009AA84
: exe-8009AA84
    203 204 cd-pictures
    10500 say
    wait-text
    0 cd-picture
    end-script
    ;

\ $8009AAA8
: exe-8009AAA8
    197 198 cd-pictures
    10501 say
    wait-text
    0 cd-picture
    end-script
    ;

\ $8009AACC
: exe-8009AACC
    209 210 cd-pictures
    10497 say
    wait-text
    10498 say
    wait-text
    0 cd-picture
    end-script
    ;

\ the townsfolk's routes, by address
' route-80081B54 $80081B54 main-word!
' route-80081B94 $80081B94 main-word!
' route-80081BD4 $80081BD4 main-word!
' route-80081BFC $80081BFC main-word!
' route-80081ED4 $80081ED4 main-word!
' route-80081F0C $80081F0C main-word!
' route-80081F38 $80081F38 main-word!
' route-80081FB4 $80081FB4 main-word!
' route-80081FF4 $80081FF4 main-word!
' route-80082028 $80082028 main-word!
' route-8008205C $8008205C main-word!
' route-80082094 $80082094 main-word!
' route-800820AC $800820AC main-word!
' route-800820EC $800820EC main-word!
' route-80082128 $80082128 main-word!
' route-80082160 $80082160 main-word!
' route-80082198 $80082198 main-word!
' route-800821C8 $800821C8 main-word!
' route-800821F8 $800821F8 main-word!
' route-8008222C $8008222C main-word!
' route-80082854 $80082854 main-word!
' route-800828AC $800828AC main-word!
' route-800828E4 $800828E4 main-word!
' route-8008290C $8008290C main-word!
' route-80082964 $80082964 main-word!
' route-800829BC $800829BC main-word!
' route-800829D4 $800829D4 main-word!
' route-800829DC $800829DC main-word!
' route-80082A04 $80082A04 main-word!
' route-80082A3C $80082A3C main-word!
' route-80082CB8 $80082CB8 main-word!
' route-80082CF8 $80082CF8 main-word!
' route-80082D30 $80082D30 main-word!
' route-80082D60 $80082D60 main-word!
' route-80082DF0 $80082DF0 main-word!
' route-80082E28 $80082E28 main-word!
' route-80082E50 $80082E50 main-word!
' route-80082EC0 $80082EC0 main-word!
' route-80082F10 $80082F10 main-word!
' route-80082F40 $80082F40 main-word!
' route-80082F88 $80082F88 main-word!
' route-80082FD0 $80082FD0 main-word!
' route-80083000 $80083000 main-word!
' route-80083040 $80083040 main-word!
' route-80083078 $80083078 main-word!
' route-80083794 $80083794 main-word!
' route-800837EC $800837EC main-word!
' route-80083834 $80083834 main-word!
' route-80083864 $80083864 main-word!
' route-800838E4 $800838E4 main-word!
' route-8008392C $8008392C main-word!
' route-8008396C $8008396C main-word!
' route-8008398C $8008398C main-word!
' route-800839C8 $800839C8 main-word!
' route-800839F8 $800839F8 main-word!
' route-80083A28 $80083A28 main-word!
' route-80083A70 $80083A70 main-word!
' route-80083AB8 $80083AB8 main-word!
' route-80083AE0 $80083AE0 main-word!
' route-80083B10 $80083B10 main-word!
' route-80083B40 $80083B40 main-word!
' route-80083B70 $80083B70 main-word!
' route-80083D60 $80083D60 main-word!
' route-800841A4 $800841A4 main-word!
' route-80084228 $80084228 main-word!
' route-80084288 $80084288 main-word!
' route-800842D0 $800842D0 main-word!
' route-80084300 $80084300 main-word!
' route-80084340 $80084340 main-word!
' route-80084378 $80084378 main-word!
' route-800843B0 $800843B0 main-word!
' route-800843E0 $800843E0 main-word!
' route-8008472C $8008472C main-word!
' route-800847F0 $800847F0 main-word!
' route-80084818 $80084818 main-word!
' route-8008483C $8008483C main-word!
' route-80084860 $80084860 main-word!
' route-8008488C $8008488C main-word!
' route-800848CC $800848CC main-word!
' route-800848FC $800848FC main-word!
' route-8008493C $8008493C main-word!
' route-80084964 $80084964 main-word!
' route-800849F4 $800849F4 main-word!
' route-80084A14 $80084A14 main-word!
' route-80084A54 $80084A54 main-word!
' route-80084AA0 $80084AA0 main-word!
' route-80084AD4 $80084AD4 main-word!
' route-80084B04 $80084B04 main-word!
' route-80084B58 $80084B58 main-word!
' route-80084E28 $80084E28 main-word!
' route-80084E58 $80084E58 main-word!
' route-80084E80 $80084E80 main-word!
' route-80084EB0 $80084EB0 main-word!
' route-80084EEC $80084EEC main-word!
' route-80084F1C $80084F1C main-word!
' route-80084F54 $80084F54 main-word!
' route-80084FAC $80084FAC main-word!
' route-80084FFC $80084FFC main-word!
' route-8008504C $8008504C main-word!
' route-8008508C $8008508C main-word!
' route-80085388 $80085388 main-word!
' route-800853D0 $800853D0 main-word!
' route-80085408 $80085408 main-word!
' route-800854A0 $800854A0 main-word!
' route-800854D0 $800854D0 main-word!
' route-800854F0 $800854F0 main-word!
' route-80085570 $80085570 main-word!
' route-800855C4 $800855C4 main-word!
' route-800855FC $800855FC main-word!
' route-80085630 $80085630 main-word!
' route-800856AC $800856AC main-word!
' route-80085714 $80085714 main-word!
' route-8008574C $8008574C main-word!
' route-8008578C $8008578C main-word!
' route-800857D4 $800857D4 main-word!
' route-80085814 $80085814 main-word!
' route-8008585C $8008585C main-word!
' route-80086704 $80086704 main-word!
' route-8008676C $8008676C main-word!
' route-800867BC $800867BC main-word!
' route-80086850 $80086850 main-word!
' route-80086888 $80086888 main-word!
' route-800868C0 $800868C0 main-word!
' route-80086908 $80086908 main-word!
' route-80086940 $80086940 main-word!
' route-80086948 $80086948 main-word!
' route-800869F4 $800869F4 main-word!
' route-80086A54 $80086A54 main-word!
' route-80086AAC $80086AAC main-word!
' route-80086B04 $80086B04 main-word!
' route-80086B3C $80086B3C main-word!
' route-80086B9C $80086B9C main-word!
' route-80086BCC $80086BCC main-word!
' route-80086C24 $80086C24 main-word!
' route-80086C6C $80086C6C main-word!
' route-80086CC4 $80086CC4 main-word!
' route-800870B4 $800870B4 main-word!
' route-80087160 $80087160 main-word!
' route-80087194 $80087194 main-word!
' route-800871D4 $800871D4 main-word!
' route-80087268 $80087268 main-word!
' route-80087280 $80087280 main-word!
' route-800872B4 $800872B4 main-word!
' route-8008734C $8008734C main-word!
' route-800873E4 $800873E4 main-word!
' route-80087424 $80087424 main-word!
' route-80087458 $80087458 main-word!
' route-800874D4 $800874D4 main-word!
' route-800874F4 $800874F4 main-word!
' route-80087544 $80087544 main-word!
' route-800875A4 $800875A4 main-word!
' route-800875BC $800875BC main-word!
' route-80087614 $80087614 main-word!
' route-8008764C $8008764C main-word!
' route-8008768C $8008768C main-word!
' route-800876C4 $800876C4 main-word!
' route-800876F4 $800876F4 main-word!
' route-80087720 $80087720 main-word!
' route-80087744 $80087744 main-word!
' route-80087790 $80087790 main-word!
' route-80087798 $80087798 main-word!
' route-800877E0 $800877E0 main-word!
' route-80087810 $80087810 main-word!
' route-80087858 $80087858 main-word!
' route-800878D8 $800878D8 main-word!
' route-80087EC8 $80087EC8 main-word!
' route-80087EF0 $80087EF0 main-word!
' route-80087F74 $80087F74 main-word!
' route-80087FA0 $80087FA0 main-word!
' route-80087FF0 $80087FF0 main-word!
' route-80088010 $80088010 main-word!
' route-80088048 $80088048 main-word!
' route-80088080 $80088080 main-word!
' route-800880D0 $800880D0 main-word!
' route-80088118 $80088118 main-word!
' route-80088148 $80088148 main-word!
' route-80088180 $80088180 main-word!
' route-80088198 $80088198 main-word!
' route-800881E0 $800881E0 main-word!
' route-800882E8 $800882E8 main-word!
' route-80088318 $80088318 main-word!
' route-80088348 $80088348 main-word!
' route-80088388 $80088388 main-word!
' route-800883C8 $800883C8 main-word!
' route-80088400 $80088400 main-word!
' route-80088474 $80088474 main-word!
' route-8008848C $8008848C main-word!
' route-800884B0 $800884B0 main-word!
' route-800884E0 $800884E0 main-word!
' route-80088518 $80088518 main-word!
' route-80088578 $80088578 main-word!
' route-800885D0 $800885D0 main-word!
' route-80088620 $80088620 main-word!
' route-80088670 $80088670 main-word!
' route-800886FC $800886FC main-word!
' route-8008872C $8008872C main-word!
' route-80088774 $80088774 main-word!
' route-800887AC $800887AC main-word!
' route-8008880C $8008880C main-word!
' route-80088834 $80088834 main-word!
' route-8008887C $8008887C main-word!
' route-800888B4 $800888B4 main-word!
' route-800888EC $800888EC main-word!
' route-80088924 $80088924 main-word!
' route-80088B04 $80088B04 main-word!
' route-8008B3B4 $8008B3B4 main-word!
' route-8008B40C $8008B40C main-word!
' route-8008B44C $8008B44C main-word!
' route-8008B464 $8008B464 main-word!
' route-8008B4CC $8008B4CC main-word!
' route-8008B514 $8008B514 main-word!
' route-8008B5B4 $8008B5B4 main-word!
' route-8008B604 $8008B604 main-word!
' route-8008B61C $8008B61C main-word!
' route-8008B658 $8008B658 main-word!
' route-8008B68C $8008B68C main-word!
' route-8008B6E8 $8008B6E8 main-word!
' route-8008B71C $8008B71C main-word!
' route-8008B74C $8008B74C main-word!
' route-8008B79C $8008B79C main-word!
' route-8008B808 $8008B808 main-word!
' route-8008B85C $8008B85C main-word!
' route-8008B888 $8008B888 main-word!
' route-8008B8DC $8008B8DC main-word!
' route-8008B990 $8008B990 main-word!
' route-8008B9F0 $8008B9F0 main-word!
' route-8008BA48 $8008BA48 main-word!
' route-8008BA9C $8008BA9C main-word!
' route-8008BB08 $8008BB08 main-word!
' route-8008BB78 $8008BB78 main-word!
' route-8008BBC0 $8008BBC0 main-word!
' route-8008BC10 $8008BC10 main-word!
' route-8008BC3C $8008BC3C main-word!
' route-8008BC60 $8008BC60 main-word!
' route-8008BCA0 $8008BCA0 main-word!
' route-8008BCDC $8008BCDC main-word!
' route-8008BD00 $8008BD00 main-word!
' route-8008BD34 $8008BD34 main-word!
' route-8008BDD8 $8008BDD8 main-word!
' route-8008BE60 $8008BE60 main-word!
' route-8008BEA4 $8008BEA4 main-word!
' route-8008BEF0 $8008BEF0 main-word!
' route-8008BF50 $8008BF50 main-word!
' route-8008BFA8 $8008BFA8 main-word!
' route-8008BFF0 $8008BFF0 main-word!
' route-8008C02C $8008C02C main-word!
' route-8008C22C $8008C22C main-word!
' route-8008C274 $8008C274 main-word!
' route-8008C2AC $8008C2AC main-word!
' route-8008C2F4 $8008C2F4 main-word!
' route-8008C32C $8008C32C main-word!
' route-8008C374 $8008C374 main-word!
' route-8008C3BC $8008C3BC main-word!
' route-8008C414 $8008C414 main-word!
' route-8008C45C $8008C45C main-word!
' route-8008C474 $8008C474 main-word!
' route-8008C54C $8008C54C main-word!
' route-8008C7C8 $8008C7C8 main-word!
' route-8008C85C $8008C85C main-word!
' route-8008C8B0 $8008C8B0 main-word!
' route-8008C91C $8008C91C main-word!
' route-8008C970 $8008C970 main-word!
' route-8008C9C0 $8008C9C0 main-word!
' route-8008C9F4 $8008C9F4 main-word!
' route-8008CA3C $8008CA3C main-word!
' route-8008CA9C $8008CA9C main-word!
' route-8008CAD4 $8008CAD4 main-word!
' route-8008CB14 $8008CB14 main-word!
' route-8008CB78 $8008CB78 main-word!
' route-8008CBDC $8008CBDC main-word!
' route-8008CC1C $8008CC1C main-word!
' route-8008CC58 $8008CC58 main-word!
' route-8008CCA4 $8008CCA4 main-word!
' route-8008CD04 $8008CD04 main-word!
' route-8008CD5C $8008CD5C main-word!
' route-8008CDB0 $8008CDB0 main-word!
' route-8008CDF4 $8008CDF4 main-word!
' route-8008CE28 $8008CE28 main-word!
' route-8008CE5C $8008CE5C main-word!
' route-8008CE84 $8008CE84 main-word!
' route-8008CEC0 $8008CEC0 main-word!
' route-8008CFB8 $8008CFB8 main-word!
' route-8008D024 $8008D024 main-word!
' route-8008D074 $8008D074 main-word!
' route-8008D0A4 $8008D0A4 main-word!
' route-8008D0E8 $8008D0E8 main-word!
' route-8008D114 $8008D114 main-word!
' route-8008D14C $8008D14C main-word!
' route-8008D1B0 $8008D1B0 main-word!
' route-8008D220 $8008D220 main-word!
' route-8008D258 $8008D258 main-word!
' route-8008D2A0 $8008D2A0 main-word!
' route-8008D2D8 $8008D2D8 main-word!
' route-8008D308 $8008D308 main-word!
' route-8008D344 $8008D344 main-word!
' route-8008D378 $8008D378 main-word!
' route-8008D3D4 $8008D3D4 main-word!
' route-8008D408 $8008D408 main-word!
' route-8008D438 $8008D438 main-word!
' route-8008D488 $8008D488 main-word!
' route-8008D4E0 $8008D4E0 main-word!
' route-8008D528 $8008D528 main-word!
' route-8008D598 $8008D598 main-word!
' route-8008D5F0 $8008D5F0 main-word!
' route-8008D650 $8008D650 main-word!
' route-8008D6B0 $8008D6B0 main-word!
' route-8008D6E0 $8008D6E0 main-word!
' route-8008D720 $8008D720 main-word!
' route-8008D77C $8008D77C main-word!
' route-8008D7DC $8008D7DC main-word!
' route-8008D814 $8008D814 main-word!
' route-8008D880 $8008D880 main-word!
' route-8008D8C0 $8008D8C0 main-word!
' route-8008D8F0 $8008D8F0 main-word!
' route-8008D938 $8008D938 main-word!
' route-8008D960 $8008D960 main-word!
' route-8008D9A8 $8008D9A8 main-word!
' route-8008D9E8 $8008D9E8 main-word!
' route-8008DA20 $8008DA20 main-word!
' route-8008DA70 $8008DA70 main-word!
' route-8008DAC0 $8008DAC0 main-word!
' route-8008DAF8 $8008DAF8 main-word!
' route-8008DB40 $8008DB40 main-word!
' route-8008DB88 $8008DB88 main-word!
' route-8008FB54 $8008FB54 main-word!
' route-8008FBA4 $8008FBA4 main-word!
' route-8008FBDC $8008FBDC main-word!
' route-8008FC30 $8008FC30 main-word!
' route-8008FC84 $8008FC84 main-word!
' route-8008FCD8 $8008FCD8 main-word!
' route-8008FD18 $8008FD18 main-word!
' route-8008FD50 $8008FD50 main-word!
' route-8008FDAC $8008FDAC main-word!
' route-8008FE00 $8008FE00 main-word!
' route-8008FE5C $8008FE5C main-word!
' route-8008FED4 $8008FED4 main-word!
' route-8008FF0C $8008FF0C main-word!
' route-8008FF60 $8008FF60 main-word!
' route-80090160 $80090160 main-word!
' route-80090188 $80090188 main-word!
' route-800901B0 $800901B0 main-word!
' route-80090210 $80090210 main-word!
' route-80090268 $80090268 main-word!
' route-800902F0 $800902F0 main-word!
' route-80090674 $80090674 main-word!
' route-800906DC $800906DC main-word!
' route-80090764 $80090764 main-word!
' route-800907A8 $800907A8 main-word!
' route-800907C8 $800907C8 main-word!
' route-80090808 $80090808 main-word!
' route-8009087C $8009087C main-word!
' route-800908D8 $800908D8 main-word!
' route-80090908 $80090908 main-word!
' route-80090950 $80090950 main-word!
' route-80090978 $80090978 main-word!
' route-800909A0 $800909A0 main-word!
' route-800909C0 $800909C0 main-word!
' route-800909F0 $800909F0 main-word!
' route-80090A18 $80090A18 main-word!
' route-80090A38 $80090A38 main-word!
' route-80090A60 $80090A60 main-word!
' route-80090A90 $80090A90 main-word!
' route-80090AC0 $80090AC0 main-word!
' route-80090D60 $80090D60 main-word!
' route-80090E0C $80090E0C main-word!
' route-80090E68 $80090E68 main-word!
' route-80090EA0 $80090EA0 main-word!
' route-80090EE0 $80090EE0 main-word!
' route-80090F20 $80090F20 main-word!
' route-80090FB4 $80090FB4 main-word!
' route-80091084 $80091084 main-word!
' route-800910CC $800910CC main-word!
' route-80091104 $80091104 main-word!
' route-8009113C $8009113C main-word!
' route-80091174 $80091174 main-word!
' route-800911AC $800911AC main-word!
' route-80091210 $80091210 main-word!
' route-800914FC $800914FC main-word!
' route-80091558 $80091558 main-word!
' route-800915A8 $800915A8 main-word!
' route-800915F4 $800915F4 main-word!
' route-80091628 $80091628 main-word!
' route-8009166C $8009166C main-word!
' route-800916A8 $800916A8 main-word!
' route-800916E8 $800916E8 main-word!
' route-80091728 $80091728 main-word!
' route-80091764 $80091764 main-word!
' route-800917A4 $800917A4 main-word!
' route-800917EC $800917EC main-word!
' route-80091820 $80091820 main-word!
' route-80091850 $80091850 main-word!
' route-80091890 $80091890 main-word!
' route-800918D0 $800918D0 main-word!
' route-80091920 $80091920 main-word!
' route-80091B54 $80091B54 main-word!
' route-80091BC4 $80091BC4 main-word!
' route-80091C3C $80091C3C main-word!
' route-80091C70 $80091C70 main-word!
' route-80091D4C $80091D4C main-word!
' route-80091DA4 $80091DA4 main-word!
' route-80091DF4 $80091DF4 main-word!
' route-80091E54 $80091E54 main-word!
' route-80091E90 $80091E90 main-word!
' route-80091EDC $80091EDC main-word!
' route-80091F2C $80091F2C main-word!
' route-80091F6C $80091F6C main-word!
' route-80091FB4 $80091FB4 main-word!
' route-80092010 $80092010 main-word!
' route-80092060 $80092060 main-word!
' route-80092484 $80092484 main-word!
' route-800924BC $800924BC main-word!
' route-800924EC $800924EC main-word!
' route-8009252C $8009252C main-word!
' route-8009255C $8009255C main-word!
' route-8009258C $8009258C main-word!
' route-800925BC $800925BC main-word!
' route-800925F4 $800925F4 main-word!
' route-80092634 $80092634 main-word!
' route-80092694 $80092694 main-word!
' route-800926E8 $800926E8 main-word!
' route-80092718 $80092718 main-word!
' route-80092750 $80092750 main-word!
