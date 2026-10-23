\ pack-052-1.fs - event pack 052_1 (CD.BIN 261): 10 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-052-1
USING: events ;

\ $80010030
: script-0
    145 actor as-this-one
    this-one -1 550 move-forward
    15 frames wait
    this-one 3584 -200 turn-to
    this-one wait-turned
    this-one 0 200 turn-to
    this-one wait-turned
    6 frames wait
    this-one 1100 600 turn-to
    this-one -1 450 move-forward
    this-one wait-turned
    106 sound
    106 sound
    3 frames wait
    this-one -1 550 move-forward
    this-one 550 -300 turn-to
    this-one wait-turned
    this-one 1024 200 turn-to
    this-one wait-turned
    15 frames wait
    end-script
    ;

\ $800100E0
: script-1
    18 frames wait
    104 ambient-start
    131 actor -1 550 move-forward
    15 frames wait
    104 ambient-stop
    131 actor 343445 283582 -1 450 walk-to-ex
    106 sound
    106 sound
    105 sound
    131 actor 200 -400 turn-by
    131 actor 343445 283582 -1 350 walk-to-ex
    131 actor -1 250 move-forward
    5 frames wait
    131 actor -1 150 move-forward
    5 frames wait
    131 actor idle
    end-script
    ;

\ $80010188
: script-2
    -1 music
    138 actor 0 0 0 set-pos
    139 actor 0 0 0 set-pos
    matthew 329845 0 269364 set-pos
    145 actor 334375 -4082 265484 set-pos
    145 actor 0 770 0 set-rot
    131 actor 331935 -4082 277953 set-pos
    131 actor 0 1024 0 set-rot
    morgan 332165 -4082 268934 set-pos
    morgan 0 1120 0 set-rot
    morgan 0 1 anim
    10 frames wait
    -798 -2346 -280 329725 -3866 269346 -896 camera-set
    map-sync
    331345 -3866 269346 60 camera-move
    2 screen-effect
    wait-effect
    morgan 81 0 anim
    matthew 331935 269364 5 60 walk-to-ex
    matthew wait-stop
    matthew 81 0 anim
    -594 -2670 -1024 334071 -4094 269400 1792 camera-set
    339237 -4094 276594 220 camera-move
    568 -40 camera-angle
    ' script-0 start-event
    ' script-1 start-event
    30 frames wait
    129 actor 332715 -4082 274014 set-pos
    129 actor 0 800 0 set-rot
    129 actor 131 1 anim
    morgan 333915 -4082 272564 set-pos
    morgan 0 1 anim
    matthew 333195 -4082 272544 set-pos
    matthew 0 394 0 set-rot
    matthew 0 1 anim
    20 frames wait
    -474 -3210 -484 333832 -4082 272980 720 camera-set
    333832 -4082 275050 20 camera-move
    morgan 340131 279947 5 60 walk-to-ex
    matthew 14 16 0 anim-then
    matthew wait-anim
    matthew 340765 277874 5 60 walk-to-ex
    10 frames wait
    matthew idle
    matthew 17 18 1 anim-then
    35 frames wait
    9 screen-effect
    wait-effect
    wait-camera-move
    morgan wait-stop
    145 actor idle
    145 actor 0 0 0 set-pos
    131 actor 0 0 0 set-pos
    matthew 328996 0 268097 set-pos
    end-script
    ;

\ $80010470
: script-3
    129 actor as-this-one
    this-one 15014 0 talk
    this-one 15015 0 talk
    this-one 15016 0 talk
    matthew 15017 0 talk
    this-one 15018 0 talk
    this-one 15019 0 talk
    matthew 15020 0 talk
    this-one 15021 0 talk
    end-script
    ;

\ $8001049C
: script-4
    129 actor 15008 5 talk
    end-script
    ;

\ $800104B0
: script-5
    129 actor 15009 6 talk
    end-script
    ;

\ $800104C4
: script-6
    129 actor as-this-one
    matthew 15000 5 talk
    this-one 15001 0 talk
    matthew 15002 7 talk
    this-one 15003 0 talk
    this-one 15004 0 talk
    matthew 15005 7 talk
    this-one 15006 0 talk
    8 13 prompt
    1 result= if  goto script-4  then  \ QUIRK: the id word then runs as an actor command (one use)
    4 actor 0 360 act-command
    goto script-5
    ;

\ $80010508
: script-7
    129 actor 15010 7 talk
    matthew 15011 5 talk
    129 actor 15012 3 talk
    129 actor 15013 6 talk
    end-script
    ;

\ $80010540
: script-8
    129 actor as-this-one
    this-one 15022 5 talk
    matthew 15023 0 talk
    this-one 15024 6 talk
    matthew 15025 7 talk
    this-one 15026 5 talk
    matthew 15027 6 talk
    this-one 15028 7 talk
    15029 5 say-mode-wait
    end-script
    ;

\ $80010574
: script-9
    morgan as-this-one
    138 actor 328777 0 269371 set-pos
    139 actor 328377 0 269371 set-pos
    morgan 328377 0 269371 set-pos
    21 object-anim
    10 frames wait
    15030 matthew say-by
    matthew 2988 0 turn-to
    -600 -2850 -700 330234 -4082 269394 2988 camera-set
    10 frames wait
    138 actor 333331 269364 run-to
    139 actor 333331 269364 run-to
    text-hold
    15031 matthew say-by
    matthew 129 1 anim
    11 frames wait
    -402 -1626 -214 330232 -4082 269656 -1798 camera-set
    matthew 113 114 1 anim-then
    matthew 330647 -4082 269443 -1 50 walk-path
    morgan 330587 269743 run-to
    8 frames wait
    -402 -2202 -214 330232 -4082 269656 -3934 camera-set
    this-one wait-stop
    2 frames wait
    morgan -2310 280 turn-to
    this-one wait-turned
    morgan 150 152 1 anim-then
    text-hold
    15032 morgan say-by
    26 frames wait
    morgan 153 0 1 anim-then
    text-hold
    15033 matthew say-by
    8 frames wait
    text-hold
    15034 morgan say-by
    12 frames wait
    morgan -3150 0 turn-to
    morgan 81 0 anim
    this-one wait-anim
    text-close
    morgan 333347 272083 run-to
    4 frames wait
    matthew 115 0 anim
    9 screen-effect
    wait-effect
    90 time-skip
    end-script
    ;

\ its scripts by number
' script-0 0 261 pack-script!
' script-1 1 261 pack-script!
' script-2 2 261 pack-script!
' script-3 3 261 pack-script!
' script-4 4 261 pack-script!
' script-5 5 261 pack-script!
' script-6 6 261 pack-script!
' script-7 7 261 pack-script!
' script-8 8 261 pack-script!
' script-9 9 261 pack-script!
