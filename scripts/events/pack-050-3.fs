\ pack-050-3.fs - event pack 050_3 (CD.BIN 253): 3 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-050-3
USING: events ;

\ $80010014
: script-0
    108 actor 588211 133617 walk-to
    108 actor wait-stop
    108 actor 0 0 0 set-pos
    20 frames wait
    58 actor -1 80 move-forward
    30 frames wait
    58 actor 75 50 turn-by
    30 frames wait
    58 actor idle
    58 actor 0 0 0 set-pos
    55 frames wait
    146 actor 585310 132520 -1 240 walk-to-ex
    146 actor wait-stop
    146 actor 585310 130960 -1 140 walk-to-ex
    146 actor wait-stop
    20 frames wait
    177 actor 586030 0 131090 set-pos
    177 actor 27 1 set-floor
    177 actor 586030 132830 walk-to
    end-script
    ;

\ $8001011C
: script-1
    5 frames wait
    10 actor 587351 131567 walk-to
    10 actor wait-stop
    10 actor 590951 132227 walk-to
    10 actor wait-stop
    10 actor 90 -2 through-door
    10 actor 0 0 0 set-pos
    end-script
    ;

\ $8001017C
: script-2
    26 actor 0 0 0 set-pos
    153 actor 1 actor-model
    102 actor 15 actor-model
    146 actor 28 actor-model
    167 actor 27 actor-model
    177 actor 7 actor-model
    153 actor as-this-one
    matthew 583990 0 126590 set-pos
    matthew 0 -3660 0 set-rot
    matthew 0 1 anim
    58 actor 588200 0 133130 set-pos
    58 actor 0 -770 0 set-rot
    108 actor 587081 0 134627 set-pos
    108 actor 27 1 set-floor
    108 actor 0 2870 0 set-rot
    108 actor 0 1 anim
    10 actor 586211 0 134327 set-pos
    10 actor 27 1 set-floor
    10 actor 0 770 0 set-rot
    10 actor 0 1 anim
    146 actor 585310 0 137860 set-pos
    146 actor 0 -2030 0 set-rot
    167 actor 585400 0 127190 set-pos
    167 actor 0 -420 0 set-rot
    this-one 584180 0 127170 set-pos
    this-one 27 1 set-floor
    this-one 0 -1820 0 set-rot
    this-one 0 1 anim
    102 actor 584370 0 126880 set-pos
    102 actor 27 1 set-floor
    102 actor 0 -5040 0 set-rot
    102 actor 0 1 anim
    -600 -2850 -700 583929 -25600 133428 -3356 camera-set
    20 frames wait
    8 screen-effect
    583929 -25600 127164 14 camera-move
    10 actor 89 0 anim
    10 actor wait-anim
    108 actor 87 0 anim
    108 actor wait-anim
    ' script-0 start-event
    ' script-1 start-event
    280 frames wait
    this-one 15000 0 talk
    this-one 15001 3 talk
    matthew 15002 4 talk
    matthew 14 15 anim-at  matthew wait-anim
    matthew 15003 0 talk
    10 frames wait
    matthew 15004 0 talk
    matthew 16 0 anim-at  matthew wait-anim
    matthew 15005 6 talk
    102 actor 5743 5744 anim-at  102 actor wait-anim
    10 frames wait
    102 actor 15006 0 talk
    102 actor 5745 0 anim-at  102 actor wait-anim
    102 actor 17 18 anim-at  102 actor wait-anim
    102 actor 15007 0 talk
    this-one 83 84 anim-at  this-one wait-anim
    this-one 15008 0 talk
    102 actor 19 0 1 anim-then
    this-one 15009 0 talk
    this-one 85 0 anim-at  this-one wait-anim
    this-one 15010 6 talk
    this-one 15011 3 talk
    102 actor 15012 7 talk
    102 actor 87 0 anim
    this-one 15013 3 talk
    matthew 83 84 anim-at  matthew wait-anim
    10 frames wait
    matthew 15014 0 talk
    10 frames wait
    this-one 15015 3 talk
    this-one 15016 7 talk
    102 actor 130 131 1 anim-then
    102 actor 15017 0 talk
    177 actor wait-stop
    177 actor 583730 0 128400 set-pos
    -504 -1590 -1264 584325 -25588 127344 -1244 camera-set
    177 actor 583890 127200 walk-to
    matthew 85 0 1 anim-then
    177 actor wait-stop
    177 actor 153 -200 face
    177 actor wait-turned
    this-one 14 15 anim-at  this-one wait-anim
    177 actor 90 0 anim
    177 actor wait-anim
    102 actor 132 0 1 anim-then
    this-one 16 0 anim-at  this-one wait-anim
    this-one 87 0 anim-at  this-one wait-anim
    177 actor 583890 128400 walk-to
    this-one 17 18 anim-at  this-one wait-anim
    this-one 0 1 anim
    this-one 102 -200 face
    this-one wait-turned
    177 actor wait-stop
    this-one 15018 0 talk
    102 actor 15019 4 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 15020 0 talk
    matthew 87 0 anim-at  matthew wait-anim
    3 screen-effect
    wait-effect
    this-one 0 0 0 set-pos
    102 actor 0 0 0 set-pos
    146 actor 0 0 0 set-pos
    177 actor 0 0 0 set-pos
    -366 -3066 -562 584721 -25552 127344 196 camera-set
    30 frames wait
    116 sound
    10 frames wait
    116 sound
    10 frames wait
    117 sound
    20 frames wait
    117 sound
    2 screen-effect
    wait-effect
    20 frames wait
    167 actor -1 110 move-forward
    20 frames wait
    167 actor 0 60 turn-to
    167 actor wait-turned
    167 actor idle
    167 actor 586670 164012 -1 320 walk-to-ex
    30 frames wait
    15021 5 say-mode-wait
    15022 5 say-mode-wait
    15023 5 say-mode-wait
    167 actor wait-stop
    167 actor 0 0 0 set-pos
    26 actor 584880 0 119743 set-pos
    26 actor 0 0 0 set-rot
    end-script
    ;

\ its scripts by number
' script-0 0 253 pack-script!
' script-1 1 253 pack-script!
' script-2 2 253 pack-script!
