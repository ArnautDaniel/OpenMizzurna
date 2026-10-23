\ ovl005.fs - area 005 overlay (CD.BIN 005_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl005
USING: events ;

\ $80105264
: entry-80105264
    wolf as-this-one
    -686 -470 -674 114535 168 59940 3940 camera-set
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 5 rumble
    114541 168 59784 600 camera-move
    1 frames wait
    114601 168 59844 600 camera-move
    1 frames wait
    114517 168 59946 600 camera-move
    1 frames wait
    -686 -470 -674 114535 168 59940 3940 camera-set
    10 frames wait
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 5 rumble
    114541 168 59784 600 camera-move
    1 frames wait
    114601 168 59844 600 camera-move
    1 frames wait
    114517 168 59946 600 camera-move
    1 frames wait
    -686 -470 -674 114535 168 59940 3940 camera-set
    10 frames wait
    this-one 82 0 anim
    this-one wait-anim
    15 frames wait
    end-script
    ;

\ $8010543C
: entry-8010543C
    wolf as-this-one
    -686 -470 -674 112939 -1008 59562 988 camera-set
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 5 rumble
    112849 -1044 59460 600 camera-move
    1 frames wait
    112447 -984 59616 600 camera-move
    1 frames wait
    112453 -1128 59502 600 camera-move
    1 frames wait
    -686 -470 -674 112939 -1008 59562 988 camera-set
    10 frames wait
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 5 rumble
    112849 -1044 59460 600 camera-move
    1 frames wait
    112447 -984 59616 600 camera-move
    1 frames wait
    112453 -1128 59502 600 camera-move
    1 frames wait
    -686 -470 -674 112939 -1008 59562 988 camera-set
    10 frames wait
    this-one 82 0 anim
    this-one wait-anim
    15 frames wait
    end-script
    ;

\ $80105614
: entry-80105614
    99 actor as-this-one
    -686 -434 -788 114865 -1260 55974 1540 camera-set
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 10 rumble
    180 8 rumble
    114865 -1164 56088 600 camera-move
    1 frames wait
    114775 -1248 55878 600 camera-move
    1 frames wait
    115057 -1284 55914 600 camera-move
    1 frames wait
    -686 -434 -788 114865 -1260 55974 1540 camera-set
    10 frames wait
    this-one 1391 0 anim
    this-one wait-anim
    115 sound
    15 frames wait
    end-script
    ;

\ $80105748
: entry-80105748
    wolf as-this-one
    -686 -470 -686 116401 -2556 60048 4096 camera-set
    this-one 1377 0 anim
    this-one wait-anim
    this-one 1354 1 anim
    113 sound
    14 screen-effect
    256 5 rumble
    180 8 rumble
    116263 -2556 60264 600 camera-move
    1 frames wait
    116509 -2664 60312 600 camera-move
    1 frames wait
    116425 -2496 60252 600 camera-move
    1 frames wait
    -686 -470 -686 116401 -2556 60048 4096 camera-set
    6 frames wait
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 5 rumble
    180 8 rumble
    116263 -2556 60264 600 camera-move
    1 frames wait
    116509 -2664 60312 600 camera-move
    1 frames wait
    116425 -2496 60252 600 camera-move
    1 frames wait
    -686 -470 -686 116401 -2556 60048 4096 camera-set
    6 frames wait
    this-one 1377 0 anim
    this-one wait-anim
    113 sound
    this-one 1354 1 anim
    14 screen-effect
    256 5 rumble
    180 8 rumble
    116263 -2556 60264 600 camera-move
    1 frames wait
    116509 -2664 60312 600 camera-move
    1 frames wait
    116425 -2496 60252 600 camera-move
    1 frames wait
    -686 -470 -686 116401 -2556 60048 4096 camera-set
    6 frames wait
    this-one 1343 0 anim
    this-one wait-anim
    this-one 1345 0 anim
    this-one wait-anim
    6 frames wait
    end-script
    ;

\ $80105D80
: entry-80105D80
    wolf as-this-one
    -686 -470 -674 115753 168 56496 3940 camera-set
    114823 120 56496 100 camera-move
    wait-camera-move
    4060 10 camera-angle
    wait-camera-turn
    this-one 113887 60538 5 50 walk-to-ex
    114475 120 60240 100 camera-move
    wait-camera-move
    this-one 111664 -1200 60612 set-pos
    this-one -1200 set-y
    this-one 0 6142 0 set-rot
    91 actor 111680 0 60310 set-pos
    91 actor -1200 set-y
    91 actor 0 6142 0 set-rot
    91 actor 0 1 anim
    99 actor 115620 0 55212 set-pos
    99 actor -1200 set-y
    99 actor 0 -470 0 set-rot
    99 actor 0 1 anim
    3268 -100 camera-angle
    wait-camera-turn
    -554 50 camera-height-to
    wait-camera-height
    this-one 1351 0 anim
    this-one anim-hold
    91 actor 111783 59541 walk-to
    this-one wait-anim
    this-one 111570 59548 walk-to
    3076 -10 camera-angle
    wait-camera-turn
    114013 120 61236 50 camera-move
    wait-camera-move
    111559 -1200 61236 50 camera-move
    91 actor 113742 58920 walk-to
    this-one 113578 59598 walk-to
    wait-camera-move
    -686 50 camera-height-to
    1564 -50 camera-angle
    wait-camera-turn
    91 actor -1024 -200 turn-to
    -956 30 camera-height-to
    91 actor 131 1 anim
    30 frames wait
    this-one -1024 -200 turn-to
    -686 30 camera-height-to
    wait-camera-height
    988 -15 camera-angle
    111559 -1200 59490 50 camera-move
    wait-camera-move
    this-one 1353 1354 1 anim-then
    30 frames wait
    end-script
    ;

: L-801059FC
    wolf as-this-one
    this-one 1388 0 anim
    this-one anim-hold
    91 actor 1389 0 anim
    91 actor anim-hold
    this-one wait-anim
    end-script
    ;

: L-80105A3C
    wolf as-this-one
    this-one 10 0 anim
    this-one anim-hold
    91 actor 10 0 anim
    91 actor anim-hold
    this-one wait-anim
    L-801059FC
    L-801059FC
    L-801059FC
    this-one 13 0 anim
    this-one anim-hold
    91 actor 13 0 anim
    91 actor anim-hold
    this-one wait-anim
    this-one 113857 57261 walk-to
    91 actor 113857 57451 walk-to
    this-one wait-stop
    this-one 114090 57261 walk-to
    91 actor 114090 57451 walk-to
    this-one wait-stop
    this-one 10 0 anim
    this-one anim-hold
    91 actor 10 0 anim
    91 actor anim-hold
    this-one wait-anim
    L-801059FC
    L-801059FC
    L-801059FC
    L-801059FC
    L-801059FC
    L-801059FC
    this-one 13 0 anim
    this-one anim-hold
    91 actor 13 0 anim
    91 actor anim-hold
    this-one wait-anim
    this-one 4096 0 turn-to
    this-one 1351 0 anim
    this-one anim-hold
    this-one wait-anim
    91 actor 116050 61035 walk-to
    this-one 116409 60920 walk-to
    this-one wait-stop
    91 actor wait-stop
    91 actor 111976 -4000 60276 set-pos
    91 actor -4000 set-y
    this-one 2048 0 turn-to
    end-script
    ;

\ $8010602C
: entry-8010602C
    wolf as-this-one
    -686 -470 -686 111559 -1200 59490 988 camera-set
    this-one 113576 58974 walk-to
    this-one wait-stop
    91 actor 2048 0 turn-to
    this-one 2048 0 turn-to
    ' L-80105A3C start-event
    113599 -1200 59472 50 camera-move
    wait-camera-move
    2044 50 camera-angle
    wait-camera-turn
    113599 -1608 57354 50 camera-move
    wait-camera-move
    1024 -50 camera-angle
    wait-camera-turn
    169 actor 115620 0 55212 set-pos
    -536 30 camera-height-to
    wait-camera-height
    99 actor 169 actor ignore-a
    10 frames wait
    1540 60 camera-angle
    -788 60 camera-height-to
    wait-camera-height
    30 frames wait
    give-control
    end-script
    ;

: L-801064A4
    99 actor as-this-one
    -686 -434 -788 114271 -1452 56706 1540 camera-set
    this-one 113 0 anim
    15 sound
    this-one wait-anim
    this-one 114 1 anim
    60 frames wait
    end-script
    ;

\ $80106144
: entry-80106144
    1 set-scene
    L-801064A4
    wolf as-this-one
    -686 -470 -788 113599 -1608 57354 1540 camera-set
    1024 -50 camera-angle
    -536 50 camera-height-to
    wait-camera-height
    wait-camera-turn
    -686 20 camera-height-to
    116251 -2604 57348 50 camera-move
    wait-camera-move
    0 -60 camera-angle
    wait-camera-turn
    60 frames wait
    end-script
    ;

: L-80105C30
    wolf as-this-one
    this-one 115151 61284 5 50 walk-to-ex
    this-one wait-stop
    this-one 3072 0 turn-to
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
    this-one 112601 60460 5 50 walk-to-ex
    this-one wait-stop
    end-script
    ;

\ $801061DC
: entry-801061DC
    ' L-80105C30 start-event
    -686 -470 -686 116251 -2604 57348 4096 camera-set
    3064 -10 camera-angle
    116287 -2604 61344 50 camera-move
    wait-camera-move
    115435 -2604 61332 50 camera-move
    wait-camera-move
    112603 -3864 61314 50 camera-move
    wait-camera-move
    1936 -80 camera-angle
    wait-camera-turn
    75 frames wait
    end-script
    ;

\ $80106274
: entry-80106274
    -308 -2486 -650 115402 0 57150 1782 camera-set
    matthew 115342 0 57326 set-pos
    matthew 0 set-y
    matthew 0 4036 0 set-rot
    matthew 108 0 anim
    13 sound
    matthew wait-anim
    matthew 109 1 anim
    75 frames wait
    end-script
    ;

\ $80106300
: entry-80106300
    -218 -4394 -800 111559 -1212 59565 -1138 camera-set
    matthew 111541 0 59565 set-pos
    matthew -1200 set-y
    matthew 0 1046 0 set-rot
    matthew 108 0 anim
    13 sound
    matthew wait-anim
    matthew 109 1 anim
    75 frames wait
    end-script
    ;

\ $8010638C
: entry-8010638C
    -518 -4286 -1652 115099 -1780 55408 3722 camera-set
    matthew 113671 0 57334 set-pos
    matthew -1780 set-y
    matthew 0 1596 0 set-rot
    matthew 108 0 anim
    13 sound
    matthew wait-anim
    matthew 109 1 anim
    75 frames wait
    end-script
    ;

\ $80106418
: entry-80106418
    -464 -2846 -932 116264 -2800 59743 -2174 camera-set
    matthew 116288 -2800 57355 set-pos
    matthew -2800 set-y
    matthew 0 0 0 set-rot
    matthew 108 0 anim
    13 sound
    matthew wait-anim
    matthew 109 1 anim
    75 frames wait
    end-script
    ;
