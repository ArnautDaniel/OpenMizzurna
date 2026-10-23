\ pack-053-0.fs - event pack 053_0 (CD.BIN 265): 1 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-053-0
USING: events ;

\ $8001000C
: script-0
    -1 music
    james as-this-one
    matthew 18399 0 418234 set-pos
    matthew 0 1 anim
    91 actor 17499 0 418194 set-pos
    91 actor -1200 set-y
    91 actor 0 1380 0 set-rot
    91 actor 5588 1 anim
    this-one 16009 0 417254 set-pos
    this-one 0 7590 0 set-rot
    this-one 5577 1 anim
    -896 -2972 -260 20451 -1356 418528 738 camera-set
    2 screen-effect
    wait-effect
    5 frames wait
    matthew 18399 417604 5 60 walk-to-ex
    matthew wait-stop
    matthew 81 0 anim
    matthew wait-anim
    matthew 14 15 anim-at  matthew wait-anim
    -152 -1304 -716 17296 -1044 418359 -659 camera-set
    10 frames wait
    -290 -3176 -464 17584 -1104 418539 -347 camera-set
    matthew 17800 418113 5 60 walk-to-ex
    matthew wait-stop
    matthew -731 0 turn-to
    matthew 150 151 1 anim-then
    10 frames wait
    -278 -1520 -398 17512 -1116 418611 -179 camera-set
    matthew 15024 0 talk
    91 actor 5589 5590 anim-at  91 actor wait-anim
    15 frames wait
    91 actor 15025 0 talk
    matthew 15026 0 talk
    10 frames wait
    91 actor 5591 5588 anim-at  91 actor wait-anim
    10 frames wait
    91 actor 15028 0 talk
    91 actor 15029 0 talk
    10 frames wait
    matthew 15030 0 talk
    10 frames wait
    matthew 15031 0 talk
    -458 -2132 -164 17404 -1116 417963 -1067 camera-set
    matthew 153 0 anim-at  matthew wait-anim
    matthew 6 -200 face
    -188 -908 -1112 15754 -1200 417519 2873 camera-set
    15586 -1200 417519 1 camera-move
    this-one 5578 5579 0 anim-then
    this-one wait-anim
    15032 32767 actor 2 say-timed-by
    16 sound
    this-one 5580 5581 0 anim-then
    this-one wait-anim
    matthew 17140 0 417773 set-pos
    matthew 0 2920 0 set-rot
    this-one 5580 5581 1 anim-then
    10 frames wait
    15033 32767 actor 1 say-timed-by-wait
    16 sound
    text-close
    15034 32767 actor 2 say-timed-by
    16 sound
    wait-camera-move
    this-one 15719 0 417444 set-pos
    this-one 109 1 anim
    -260 -2636 -752 15520 -1200 417747 2993 camera-set
    15520 -1200 417387 2 camera-move
    15037 matthew 2 say-timed-by-wait
    text-close
    this-one 110 0 anim
    this-one wait-anim
    this-one 417344 set-z
    this-one 29 30 anim-at  this-one wait-anim
    16 sound
    15 frames wait
    16 sound
    15 frames wait
    16 sound
    wait-camera-move
    -380 -1088 -950 15412 -1200 417557 3256 camera-set
    16 sound
    10 frames wait
    16 sound
    10 frames wait
    16 sound
    15035 32767 actor 1 say-timed-by-wait
    text-hold
    15036 32767 actor 2 say-timed-by-wait
    text-close
    this-one 31 0 anim-at  this-one wait-anim
    this-one 5594 0 anim
    10 frames wait
    12 sound
    15033 32767 actor 2 say-timed-by
    this-one wait-anim
    this-one 23 24 anim-at  this-one wait-anim
    15 frames wait
    -512 -1160 -482 16888 -1200 417629 3232 camera-set
    15038 32767 actor 2 say-timed-by-wait
    text-hold
    15039 matthew 2 say-timed-by-wait
    text-close
    this-one 25 0 anim-at  this-one wait-anim
    -476 -326 -530 15988 -1380 417281 3424 camera-set
    this-one 14 15 anim-at  this-one wait-anim
    this-one 0 1 anim
    this-one 0 200 face
    this-one wait-turned
    -476 -380 -404 15976 -1272 417382 2968 camera-set
    14 music
    this-one 82 0 anim
    this-one 15040 0 255 talk-to
    40 frames wait
    13 music
    this-one 17 18 anim-at  this-one wait-anim
    this-one 15041 0 255 talk-to
    matthew 15042 5 talk
    -590 -2126 -440 16672 -1200 417875 664 camera-set
    this-one 15043 3 255 talk-to
    matthew 15044 5 talk
    -476 -380 -404 15976 -1272 417382 2968 camera-set
    this-one 15045 1 255 talk-to
    this-one 15046 5 255 talk-to
    -590 -2126 -440 16672 -1200 417875 664 camera-set
    matthew 15047 3 talk
    this-one 5594 0 anim
    15 frames wait
    15048 32767 actor 2 say-timed-by
    12 sound
    13 sound
    14 sound
    this-one wait-anim
    -566 -1226 -530 15418 -1200 417616 -560 camera-set
    this-one 15049 3 255 talk-to
    this-one 15050 4 255 talk-to
    this-one 15051 5 255 talk-to
    this-one 15052 1 255 talk-to
    matthew 5525 1 anim
    15 frames wait
    this-one 15053 0 255 talk-to
    18 music
    -584 -2300 -296 15346 -1200 417330 3016 camera-set
    15054 32767 actor 2 say-timed-by
    -1544 90 camera-dist-to
    this-one 5554 0 anim
    matthew 5529 5528 0 anim-then
    wait-camera-dist
    -410 -1418 -692 17410 -1212 418295 4504 camera-set
    138 sound
    14 screen-effect
    3 frames wait
    109 sound
    14 screen-effect
    matthew wait-anim
    matthew 5525 1 anim
    14 screen-effect
    this-one 5604 0 anim
    this-one anim-hold
    -542 -1016 -698 16390 -1164 417513 2896 camera-set
    10 frames wait
    -614 -764 -986 16858 -1164 417725 5224 camera-set
    14 screen-effect
    matthew 112 0 anim
    10 frames wait
    -596 -842 -464 16384 -864 417496 2920 camera-set
    this-one wait-anim
    this-one 5605 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 5606 1 anim
    -1892 -446 -2786 15262 24 417150 2776 camera-set
    this-one 5607 0 anim
    this-one anim-hold
    5 frames wait
    448 -2234 -3290 15724 -768 417622 4768 camera-set
    12 sound
    13 sound
    14 sound
    15 sound
    16 sound
    this-one wait-anim
    matthew 17184 -1200 417135 set-pos
    matthew 5596 0 anim
    -1076 -1004 -1070 17184 -648 417135 5200 camera-set
    5 frames wait
    400 8 -3224 16810 -204 417077 3112 camera-set
    45 frames wait
    5 screen-effect
    wait-effect
    4 screen-effect
    wait-effect
    10 frames wait
    3 screen-effect
    matthew wait-anim
    wait-effect
    end-script
    ;

\ its scripts by number
' script-0 0 265 pack-script!
