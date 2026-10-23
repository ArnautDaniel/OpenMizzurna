\ pack-051-4.fs - event pack 051_4 (CD.BIN 259): 11 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-051-4
USING: events ;

\ $80010034
: script-0
    matthew 34155 0 407827 set-pos
    matthew 0 1024 0 set-rot
    matthew 5500 0 anim
    matthew anim-hold
    60 frames wait
    matthew 35445 2 407658 set-pos
    matthew 0 6059 0 set-rot
    end-script
    ;

\ $800100A8
: script-1
    40 frames wait
    25 actor 5552 anim-once
    8 frames wait
    17 music
    132 sound
    256 10 rumble
    255 10 rumble
    25 actor wait-anim
    25 actor 5610 1 anim
    end-script
    ;

\ $80010108
: script-2
    13 music
    25 actor 36154 2 408181 set-pos
    25 actor 2 set-y
    25 actor 0 0 0 set-rot
    -458 -2042 -482 36167 14 407887 4660 camera-set
    map-sync
    2 screen-effect
    wait-effect
    36695 14 408355 15 camera-move
    9 actor 36382 0 408990 set-pos
    9 actor 0 2048 0 set-rot
    9 actor 2 set-y
    9 actor 5516 1 anim
    ' script-1 start-event
    50 frames wait
    35097 2 407575 110 camera-move
    26 25 camera-turn
    9 actor 5520 80 move-forward
    15 frames wait
    9 actor idle
    9 actor 5520 5508 0 anim-then
    35097 2 407287 11 camera-move
    66 frames wait
    matthew 34238 0 407509 set-pos
    matthew 9 0 face
    matthew 5538 1 anim
    -734 -788 -1148 34778 17 407005 7667 camera-set
    matthew 5529 5525 1 anim-then
    10 frames wait
    -428 -1268 -32 34509 -190 407761 355 camera-set
    112 sound
    10 music
    45 20 camera-turn
    36177 110 407845 80 camera-move
    9 actor wait-anim
    9 actor 0 3028 0 set-rot
    9 actor 5516 1 anim
    end-script
    ;

\ $8001033C
: script-3
    9 actor 36242 0 407570 set-pos
    9 actor 0 3098 0 set-rot
    -794 -2084 -212 36929 122 407545 5164 camera-set
    5 frames wait
    35891 182 407575 75 camera-move
    9 actor 5520 90 move-forward
    15 frames wait
    9 actor idle
    9 actor 105 0 anim
    14 screen-effect
    106 sound
    12 sound
    256 20 rumble
    200 20 rumble
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    15 frames wait
    end-script
    ;

\ $800104E4
: script-4
    9 actor 36242 0 407570 set-pos
    9 actor 0 2688 0 set-rot
    9 actor 5507 0 anim
    -842 -2480 -32 36911 2 408007 4780 camera-set
    5 frames wait
    37499 -274 408379 25 camera-move
    49 frames wait
    30 40 camera-turn
    14 screen-effect
    106 sound
    12 sound
    13 sound
    256 25 rumble
    200 25 rumble
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    10 frames wait
    end-script
    ;

\ $800106F8
: script-5
    9 actor 36242 0 407570 set-pos
    9 actor 0 3098 0 set-rot
    -794 -2084 -212 36929 122 407545 5164 camera-set
    5 frames wait
    9 actor 5524 0 anim
    9 actor anim-hold
    10 frames wait
    35891 182 407575 75 camera-move
    14 screen-effect
    106 sound
    12 sound
    13 sound
    256 15 rumble
    255 15 rumble
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    35891 179 407575 600 camera-move
    1 frames wait
    35891 195 407575 600 camera-move
    1 frames wait
    38 frames wait
    14 screen-effect
    106 sound
    12 sound
    13 sound
    256 15 rumble
    255 15 rumble
    35891 300 407575 900 camera-move
    1 frames wait
    35891 100 407575 900 camera-move
    1 frames wait
    35891 300 407575 900 camera-move
    1 frames wait
    35891 100 407575 900 camera-move
    1 frames wait
    35891 182 407575 900 camera-move
    1 frames wait
    9 actor wait-anim
    5 frames wait
    end-script
    ;

\ $8001093C
: script-6
    9 actor 36242 0 407570 set-pos
    9 actor 0 3098 0 set-rot
    -710 -2012 -524 36527 350 407467 5212 camera-set
    5 frames wait
    9 actor 5523 0 anim
    9 actor wait-anim
    113 sound
    9 actor 5519 0 anim
    37061 -34 407467 28 camera-move
    33 frames wait
    113 sound
    112 sound
    37061 -33 407467 900 camera-move
    1 frames wait
    37061 -35 407467 900 camera-move
    1 frames wait
    37061 -33 407467 900 camera-move
    1 frames wait
    37061 -35 407467 900 camera-move
    1 frames wait
    37061 -33 407467 900 camera-move
    1 frames wait
    37061 -35 407467 900 camera-move
    1 frames wait
    37061 -33 407467 900 camera-move
    1 frames wait
    37061 -35 407467 900 camera-move
    1 frames wait
    37061 -33 407467 900 camera-move
    1 frames wait
    37061 -35 407467 900 camera-move
    10 frames wait
    end-script
    ;

\ $80010B04
: script-7
    9 actor 36022 0 407530 set-pos
    9 actor 0 3228 0 set-rot
    -1100 -1058 -572 35921 578 407497 5356 camera-set
    5 frames wait
    113 sound
    112 sound
    9 actor 105 0 anim
    35921 579 407497 900 camera-move
    1 frames wait
    35921 577 407497 900 camera-move
    1 frames wait
    35921 579 407497 900 camera-move
    1 frames wait
    35921 577 407497 900 camera-move
    1 frames wait
    35921 579 407497 900 camera-move
    1 frames wait
    35921 577 407497 900 camera-move
    8 frames wait
    9 actor 5513 0 anim
    -1100 -734 -1526 36335 530 407305 4516 camera-set
    13 frames wait
    113 sound
    112 sound
    36335 531 407305 900 camera-move
    1 frames wait
    36335 529 407305 900 camera-move
    1 frames wait
    36335 532 407305 900 camera-move
    1 frames wait
    36335 528 407305 900 camera-move
    1 frames wait
    36335 533 407305 900 camera-move
    1 frames wait
    36335 527 407305 900 camera-move
    1 frames wait
    36335 534 407305 900 camera-move
    1 frames wait
    36335 526 407305 900 camera-move
    1 frames wait
    36335 535 407305 900 camera-move
    20 frames wait
    1 frames wait
    113 sound
    112 sound
    36335 525 407305 900 camera-move
    1 frames wait
    36335 536 407305 900 camera-move
    1 frames wait
    36335 524 407305 900 camera-move
    1 frames wait
    36335 537 407305 900 camera-move
    1 frames wait
    36335 523 407305 900 camera-move
    1 frames wait
    36335 538 407305 900 camera-move
    1 frames wait
    36335 522 407305 900 camera-move
    1 frames wait
    36335 539 407305 900 camera-move
    1 frames wait
    36335 521 407305 900 camera-move
    1 frames wait
    36335 540 407305 900 camera-move
    1 frames wait
    36335 520 407305 900 camera-move
    1 frames wait
    4 frames wait
    end-script
    ;

\ $80010EB8
: script-8
    -1 music
    matthew 35438 0 407399 set-pos
    matthew 0 1339 0 set-rot
    9 actor 35998 0 407156 set-pos
    9 actor 0 3358 0 set-rot
    -248 -1736 -26 34736 -91 407647 1083 camera-set
    matthew 5511 5512 1 anim-then
    9 actor 5524 0 anim
    9 actor anim-hold
    57 frames wait
    14 screen-effect
    12 sound
    106 sound
    107 sound
    9 actor wait-anim
    9 actor 5519 0 anim
    112 sound
    50 frames wait
    5 screen-effect
    wait-effect
    end-script
    ;

\ $80010FBC
: script-9
    -1 music
    9 actor 36008 0 407366 set-pos
    9 actor 0 2988 0 set-rot
    matthew 34358 0 407519 set-pos
    matthew 0 5189 0 set-rot
    matthew 5525 1 anim
    -350 -1292 -362 34286 -91 407509 987 camera-set
    5 frames wait
    34274 -91 407155 8 camera-move
    9 actor 5521 0 anim
    113 sound
    50 frames wait
    107 sound
    114 sound
    34274 -100 407155 900 camera-move
    1 frames wait
    34274 -83 407155 900 camera-move
    1 frames wait
    34274 -100 407155 900 camera-move
    1 frames wait
    34274 -83 407155 900 camera-move
    1 frames wait
    9 actor 5522 1 anim
    20 frames wait
    -530 -2036 -194 34040 2 407657 3319 camera-set
    matthew 5530 5538 anim-at  matthew wait-anim
    matthew 35318 407349 5531 25 walk-to-ex
    20 frames wait
    -638 -830 -1778 35186 2 407665 5501 camera-set
    matthew wait-stop
    matthew 5545 5546 anim-at  matthew wait-anim
    20 frames wait
    9 screen-effect
    wait-effect
    10 frames wait
    0 set-scene
    -674 -6476 -698 35318 2 407313 1686 camera-set
    matthew 4294967295 actor ignore-a
    24 actor 0 0 0 set-pos
    2 screen-effect
    wait-effect
    11 music
    20 frames wait
    3 screen-effect
    wait-effect
    25 actor 0 0 0 set-pos
    end-script
    ;

\ its scripts by number
' script-0 0 259 pack-script!
' script-1 1 259 pack-script!
' script-2 2 259 pack-script!
' script-3 3 259 pack-script!
' script-4 4 259 pack-script!
' script-5 5 259 pack-script!
' script-6 6 259 pack-script!
' script-7 7 259 pack-script!
' script-8 8 259 pack-script!
' script-9 9 259 pack-script!
