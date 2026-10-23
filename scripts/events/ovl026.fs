\ ovl026.fs - area 026 overlay (CD.BIN 026_0): its scripts.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.ovl026
USING: events events.main ;

\ $801059B8
: entry-801059B8
    10 actor as-this-one
    -476 -3022 -582 53440 -300 415859 -822 camera-set
    this-one 81 0 anim
    this-one wait-anim
    37 32767 actor say-by-wait
    -476 -3022 -582 54298 0 415841 642 camera-set
    this-one 56358 417317 -120 face-point
    this-one 3 1 anim
    30 frames wait
    this-one 30 walk-forward
    6 frames wait
    give-control
    84 frames wait
    this-one 57065 418191 -80 face-point
    45 frames wait
    this-one idle
    end-script
    ;

\ $80105A7C
: entry-80105A7C
    give-control
    10 actor as-this-one
    this-one 54151 416162 -30 face-point
    60 frames wait
    this-one 30 walk-forward
    90 frames wait
    this-one idle
    this-one 54151 416162 walk-to
    30 frames wait
    this-one 54151 416162 walk-to
    this-one wait-stop
    this-one 54151 0 416162 set-pos
    this-one -60 -110 turn-by
    this-one wait-turned
    this-one 0 -2676 0 set-rot
    end-script
    ;

\ $80105B0C
: entry-80105B0C
    10 actor as-this-one
    -470 -1800 -600 53074 -12 416210 -3288 camera-set
    this-one idle
    this-one 0 -250 face
    9 frames wait
    this-one wait-turned
    38 32767 actor say-by-wait
    this-one 53881 416217 0 face-point
    this-one 85 run
    -470 -3042 -1086 52903 0 415927 -7624 camera-set
    give-control
    camera-fixed
    this-one 53881 416217 400 wait-near
    this-one idle
    end-script
    ;

\ $80105BBC
: entry-80105BBC
    10 actor as-this-one
    -474 -2210 -1106 0 0 0 260 camera-set
    this-one camera-at
    matthew 255 0 face
    matthew 0 1 anim
    this-one 0 0 face
    this-one 89 0 1 anim-then
    36 32767 actor say-by-wait
    3 screen-effect
    matthew 54394 0 415871 set-pos
    ovl026 $801020F8 native
    matthew this-one face-each-other
    camera-follow
    15 frames wait
    2 screen-effect
    end-script
    ;

\ $80105C70
: entry-80105C70
    matthew 0 1 anim
    -470 -1750 -600 0 0 0 0 camera-set
    cohen camera-at
    matthew cohen camera-face
    4 0 camera-turn
    cohen 3 1 anim
    cohen 0 -100 face
    cohen wait-turned
    cohen 0 1 anim
    40 cohen say-by-wait
    matthew 3 0 face
    end-script
    ;

\ $80105D04
: entry-80105D04
    matthew 0 1 anim
    -470 -1750 -600 0 0 0 0 camera-set
    cohen camera-at
    matthew cohen camera-face
    4 0 camera-turn
    cohen 3 1 anim
    cohen 0 0 face
    cohen 0 1 anim
    42 cohen say-by-wait
    end-script
    ;

\ $80105D84
: entry-80105D84
    250 0 call-pack-script
    give-control
    nozz 25514 409114 walk-to
    morgan 25514 409114 walk-to
    cohen 33346 409327 walk-to
    morgan wait-stop
    morgan 33346 409327 walk-to
    nozz wait-stop
    nozz 33346 409327 walk-to
    cohen wait-stop
    cohen 36409 408325 walk-to
    morgan wait-stop
    morgan 36409 408325 walk-to
    nozz wait-stop
    nozz 36409 408325 walk-to
    1 object-anim
    cohen wait-stop
    cohen 36368 407619 walk-to
    morgan wait-stop
    morgan 36368 407619 walk-to
    nozz wait-stop
    nozz 36368 407619 walk-to
    cohen 10 goto-stop
    end-script
    ;

\ $80105E70
: entry-80105E70
    matthew 0 1 anim
    -470 -1750 -600 0 0 0 0 camera-set
    cohen camera-at
    matthew cohen camera-face
    4 0 camera-turn
    cohen 3 1 anim
    cohen 0 0 face
    cohen 0 1 anim
    41 cohen say-by-wait
    end-script
    ;

\ $80105EF0
: entry-80105EF0
    54 matthew say-by-wait
    end-script
    ;

: L-80105F00
    25 frames wait
    9 screen-effect
    wait-effect
    9 actor 40717 -800 431420 set-pos
    matthew 39633 -800 432595 set-pos
    matthew 9 actor face-each-other
    -716 -1946 -1112 39633 -800 432595 1466 camera-set
    20 frames wait
    2 screen-effect
    wait-effect
    53 2 say-mode
    9 actor 5519 0 anim
    20 frames wait
    text-close
    give-control
    9 actor wait-anim
    ovl026 $80104340 native
    end-script
    ;

\ $80105FBC
: entry-80105FBC
    -400 -2580 -400 0 0 0 0 camera-set
    matthew camera-at
    matthew 9 0 face
    matthew 2 -20 move-forward
    3 frames wait
    135 0 camera-turn
    7 frames wait
    matthew idle
    9 actor 5524 0 anim
    matthew 5511 5512 1 anim-then
    9 actor anim-hold
    9 actor wait-anim
    ovl026 $80104350 native
    goto L-80105F00
    ;

\ $8010605C
: entry-8010605C
    9 actor 41603 0 415821 set-pos
    9 actor 0 1031 0 set-rot
    matthew 0 1024 0 set-rot
    matthew 12 walk-forward
    15 frames wait
    matthew idle
    matthew 29 30 1 anim-then
    25 frames wait
    matthew 8 walk-forward
    5 frames wait
    matthew idle
    -512 -2738 -716 47351 120 415012 3830 camera-set
    matthew 29 30 1 anim-then
    20 frames wait
    9 actor 54 run
    30 frames wait
    -45 -20 camera-turn
    40 frames wait
    matthew 4 1 anim
    matthew 180 130 turn-by
    matthew wait-turned
    matthew 2 -20 move-forward
    10 frames wait
    9 actor idle
    matthew idle
    ovl026 $80104350 native
    end-script
    ;

\ $80106174
: entry-80106174
    -602 -1734 -1188 34473 48 407743 3664 camera-set
    matthew 5536 0 anim
    matthew wait-anim
    matthew 5538 1 anim
    ovl026 $8010431C native
    end-script
    ;

\ $801061BC
: entry-801061BC
    morgan 79 7 talk
    matthew 80 7 talk
    morgan 81 1 talk
    matthew 82 3 talk
    morgan 83 7 talk
    end-script
    ;

\ $80106210
: entry-80106210
    morgan 84 4 talk
    end-script
    ;

\ $80106224
: entry-80106224
    matthew 30 1 anim
    44 matthew say-by-wait
    end-script
    ;

\ $80106240
: entry-80106240
    -572 -2702 292 32441 -800 426841 -1946 camera-set
    matthew 45 2 talk
    matthew 46 0 talk
    matthew 47 0 talk
    matthew 48 2 talk
    end-script
    ;

\ $80106294
: entry-80106294
    1 1 say-mode-wait
    2 1 say-mode-wait
    events.main:sub-80096E74
    matthew 82 0 anim
    3 1 say-mode-wait
    matthew 54677 0 415736 set-pos
    matthew -646 0 turn-to
    27 -1 54559 407221 -2200 0 warp
    ;

\ $80106304
: entry-80106304
    matthew 0 1 anim
    events.main:sub-80096E74
    0 say-wait-close
    end-script
    ;

\ $80106324
: entry-80106324
    11 screen-effect
    wait-effect
    27 -1 54559 407221 -2200 0 warp
    ;

\ $801063BC
: entry-801063BC
    249 0 call-pack-script
    end-script
    ;

\ $801063CC
: entry-801063CC
    249 1 call-pack-script
    end-script
    ;

\ $801063DC
: entry-801063DC
    249 2 call-pack-script
    end-script
    ;

\ $801063EC
: entry-801063EC
    249 3 call-pack-script
    end-script
    ;

\ $801063FC
: entry-801063FC
    249 4 call-pack-script
    end-script
    ;

\ $8010640C
: entry-8010640C
    262 4 call-pack-script
    end-script
    ;

\ $80106454
: entry-80106454
    259 3 call-pack-script
    end-script
    ;

\ $80106464
: entry-80106464
    259 4 call-pack-script
    end-script
    ;

\ $80106474
: entry-80106474
    259 5 call-pack-script
    end-script
    ;

\ $80106484
: entry-80106484
    259 6 call-pack-script
    end-script
    ;

\ $80106494
: entry-80106494
    259 7 call-pack-script
    end-script
    ;

\ $801064A4
: entry-801064A4
    259 8 call-pack-script
    end-script
    ;

\ $801064B4
: entry-801064B4
    259 9 call-pack-script
    end-script
    ;

\ $801064C4
: entry-801064C4
    ovl026 $80104218 native
    260 0 call-pack-script
    end-script
    ;

\ $801064DC
: entry-801064DC
    252 2 call-pack-script
    ovl026 $80047BFC native
    end-script
    ;

: L-801064F4
    265 0 call-pack-script
    ovl026 $80047C2C native
    end-script
    ;

\ $8010650C
: entry-8010650C
    263 0 call-pack-script
    199 200 cd-pictures
    matthew 49 5 talk
    10 frames wait
    this-one 50 0 talk
    0 cd-picture
    this-one 51 0 talk
    this-one 16 0 anim-at  this-one wait-anim
    201 202 cd-pictures
    264 0 call-pack-script
    goto L-801064F4
    ;

\ $8010658C
: entry-8010658C
    morgan as-this-one
    this-one 55 0 talk
    matthew 56 7 talk
    this-one 57 0 talk
    this-one 58 0 talk
    end-script
    ;

\ $801065C8
: entry-801065C8
    10 actor as-this-one
    this-one 59 0 talk
    matthew 60 5 talk
    this-one 61 0 talk
    matthew 62 2 talk
    this-one 63 0 talk
    end-script
    ;

\ $80106610
: entry-80106610
    10 actor as-this-one
    this-one 64 0 talk
    matthew 65 5 talk
    this-one 66 0 talk
    matthew 67 2 talk
    matthew 68 0 talk
    this-one 69 7 talk
    matthew 70 4 talk
    end-script
    ;

\ $80106670
: entry-80106670
    -358 -1806 -1646 36403 -108 408194 319 camera-set
    100 door-animate
    events.main:sub-80096CF0
    0 set-scene
    end-script
    ;

\ $801066AC
: entry-801066AC
    -200 -2416 -672 36312 -96 407991 2386 camera-set
    100 door-animate
    events.main:sub-80096CF0
    7 set-scene
    end-script
    ;

\ $801066E8
: entry-801066E8
    camera-follow
    7 say
    matthew 81 0 anim
    wait-text
    camera-fixed
    end-script
    ;

\ $80106710
: entry-80106710
    -536 -480 -522 34383 12 407551 4072 camera-set
    8 say
    wait-text
    10 frames wait
    9 matthew say-by
    wait-text
    end-script
    ;

\ $80106758
: entry-80106758
    matthew 34155 0 407827 set-pos
    matthew 0 0 0 set-rot
    -602 -1734 -1188 34473 48 407743 3664 camera-set
    matthew 5535 0 anim
    15 frames wait
    130 sound
    ovl026 $80104258 native
    matthew wait-anim
    matthew 5538 1 anim
    end-script
    ;

: L-8010641C
    ovl026 $80104238 native
    259 load-pack
    0 start-pack-script
    ovl026 $80104238 native
    259 2 call-pack-script
    ovl026 $801040D0 native
    end-script
    ;

\ $801067E0
: entry-801067E0
    matthew 34155 0 407827 set-pos
    matthew 0 0 0 set-rot
    -602 -1734 -1188 34473 48 407743 3664 camera-set
    matthew 5536 0 anim
    matthew wait-anim
    matthew 5538 1 anim
    3 screen-effect
    wait-effect
    ' L-8010641C start-event
    end-script
    ;

\ $80106864
: entry-80106864
    matthew 34555 0 407827 set-pos
    matthew 0 0 0 set-rot
    -476 -1644 -1122 34755 60 407779 3784 camera-set
    matthew 5535 0 anim
    15 frames wait
    130 sound
    ovl026 $80104278 native
    matthew wait-anim
    matthew 5538 1 anim
    end-script
    ;

\ $801068EC
: entry-801068EC
    matthew 34555 0 407827 set-pos
    matthew 0 0 0 set-rot
    -476 -1644 -1122 34755 60 407779 3784 camera-set
    matthew 5536 0 anim
    matthew wait-anim
    matthew 5538 1 anim
    3 screen-effect
    wait-effect
    ' L-8010641C start-event
    end-script
    ;

\ $80106970
: entry-80106970
    matthew 34995 0 407937 set-pos
    matthew 0 -440 0 set-rot
    -488 -1254 -1284 35499 -252 407677 3496 camera-set
    matthew 24 actor ignore-a
    matthew 5551 5529 anim-at  matthew wait-anim
    matthew 5537 5525 1 anim-then
    14 say
    wait-text
    matthew 5530 5538 1 anim-then
    end-script
    ;

\ $80106A0C
: entry-80106A0C
    camera-follow
    4 say
    wait-text
    camera-fixed
    end-script
    ;

\ $80106A24
: entry-80106A24
    -1682 -6716 -2228 15921 -864 427119 -1296 camera-set
    5 frames wait
    15921 -864 422475 18 camera-move
    wait-camera-move
    5 frames wait
    end-script
    ;

\ $80106AD8
: entry-80106AD8
    15 say
    wait-text
    16 say
    wait-text
    end-script
    ;

\ $80106AF4
: entry-80106AF4
    17 say
    wait-text
    end-script
    ;

\ $80106B04
: entry-80106B04
    18 say
    wait-text
    19 say
    wait-text
    end-script
    ;

\ $80106B34
: entry-80106B34
    morgan as-this-one
    9 actor 36312 0 407320 set-pos
    9 actor 0 6491 0 set-rot
    9 actor 5522 1 anim
    matthew 35890 0 407348 set-pos
    matthew 0 1094 0 set-rot
    this-one 35900 0 407882 set-pos
    this-one 0 0 face
    7 set-scene
    -278 -2690 -2048 36040 2 407654 494 camera-set
    matthew 48 50 anim-at  matthew wait-anim
    20 say
    wait-text
    20 frames wait
    21 matthew say-by
    matthew 49 133 0 anim-then
    matthew wait-anim
    matthew 52 53 1 anim-then
    wait-text
    this-one 35900 407692 walk-to
    morgan 22 0 talk
    16 picture-show
    23 32767 actor say-by-wait
    24 32767 actor say-by-wait
    25 32767 actor say-by-wait
    26 32767 actor say-by-wait
    27 32767 actor say-by-wait
    picture-hide
    28 matthew say-by
    matthew 54 0 1 anim-then
    matthew 1 -300 face
    wait-text
    29 morgan say-by
    matthew wait-turned
    wait-text
    matthew 59 60 1 anim-then
    this-one 59 60 1 anim-then
    10 frames wait
    matthew 61 0 1 anim-then
    this-one 61 0 1 anim-then
    45 frames wait
    give-control
    this-one 133 51 anim-at  this-one wait-anim
    20 frames wait
    this-one 55 0 anim-at  this-one wait-anim
    this-one 35423 407685 walk-to
    this-one wait-stop
    this-one 5578 -200 turn-to
    end-script
    ;

\ $80106E2C
: entry-80106E2C
    250 1 call-pack-script
    189 190 cd-pictures
    250 2 call-pack-script
    end-script
    ;

\ $80106E54
: entry-80106E54
    252 0 call-pack-script
    end-script
    ;

\ $80106E64
: entry-80106E64
    252 1 call-pack-script
    end-script
    ;

\ $80106E74
: entry-80106E74
    251 0 call-pack-script
    end-script
    ;
