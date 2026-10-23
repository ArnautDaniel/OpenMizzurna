\ pack-053-4.fs - event pack 053_4 (CD.BIN 269): 4 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-053-4
USING: events ;

\ $80010018
: script-0
    106 actor 565076 210405 walk-to
    106 actor wait-stop
    106 actor -730 0 turn-to
    106 actor 5701 0 anim
    5 frames wait
    116 sound
    33 frames wait
    117 sound
    106 actor wait-anim
    106 actor 0 0 0 set-pos
    end-script
    ;

\ $80010084
: script-1
    102 actor 7 actor-model
    mel 17 18 anim-at  mel wait-anim
    10 frames wait
    mel 0 1 anim
    mel 102 -200 face
    mel wait-turned
    mel 94 95 anim-at  mel wait-anim
    10 frames wait
    102 actor 91 93 0 anim-then
    30 frames wait
    mel 96 0 anim
    mel 566156 210795 walk-to
    mel wait-stop
    mel 14 15 anim-at  mel wait-anim
    102 actor 566366 210785 walk-to
    102 actor wait-stop
    102 actor -1116 0 turn-to
    10 frames wait
    mel 16 0 anim-at  mel wait-anim
    mel 565076 210405 walk-to
    mel wait-stop
    mel -730 0 turn-to
    mel 5701 0 anim
    5 frames wait
    116 sound
    33 frames wait
    117 sound
    mel wait-anim
    mel 0 0 0 set-pos
    102 actor 565946 209765 walk-to
    end-script
    ;

\ $800101D8
: script-2
    102 actor 7 actor-model
    102 actor 565016 210245 walk-to
    102 actor wait-stop
    102 actor -730 0 turn-to
    102 actor 5701 0 anim
    5 frames wait
    116 sound
    33 frames wait
    117 sound
    102 actor wait-anim
    102 actor 0 0 0 set-pos
    20 frames wait
    177 actor 564336 208895 -1 100 walk-to-ex
    177 actor wait-stop
    177 actor 0 0 0 set-pos
    end-script
    ;

\ $80010280
: script-3
    26 actor 0 0 0 set-pos
    153 actor 1 actor-model
    102 actor 7 actor-model
    177 actor 28 actor-model
    167 actor 27 actor-model
    153 actor as-this-one
    this-one 566766 -17600 211845 set-pos
    this-one 0 -1700 0 set-rot
    this-one 5741 1 anim
    matthew 566286 -17600 212145 set-pos
    matthew 0 5402 0 set-rot
    matthew 15 1 anim
    mel 566366 -17600 211005 set-pos
    mel 0 2310 0 set-rot
    mel 0 1 anim
    106 actor 565366 -17600 210165 set-pos
    106 actor 0 -910 0 set-rot
    106 actor 0 1 anim
    102 actor 566526 -17600 210945 set-pos
    102 actor 0 -1050 0 set-rot
    102 actor 0 1 anim
    167 actor 567706 -17600 211295 set-pos
    167 actor 0 -140 0 set-rot
    177 actor 564696 -17600 210635 set-pos
    177 actor 0 390 0 set-rot
    -528 -3354 -1186 566312 -17552 211481 4744 camera-set
    map-sync
    2 screen-effect
    wait-effect
    ['] script-0 start-event
    30 frames wait
    ['] script-1 start-event
    120 frames wait
    this-one 5742 89 0 anim-then
    this-one wait-anim
    this-one 0 200 face
    this-one 15000 7 talk
    matthew 16 0 anim-at  matthew wait-anim
    matthew 15001 4 talk
    matthew 15002 7 talk
    this-one 15003 6 talk
    matthew 15004 7 talk
    this-one 15005 5 talk
    matthew 89 87 0 anim-then
    matthew wait-anim
    this-one 15006 3 talk
    matthew 15007 7 talk
    153 actor 17 18 anim-at  153 actor wait-anim
    this-one 15008 0 talk
    153 actor 19 0 anim
    this-one 15009 0 talk
    matthew 15010 4 talk
    this-one 15011 2 talk
    102 actor 566606 211395 walk-to
    50 frames wait
    this-one 85 0 anim
    this-one wait-anim
    this-one -1700 -200 turn-to
    102 actor wait-stop
    102 actor 15012 7 7 talk-to
    matthew 14 15 1 anim-then
    matthew 15013 0 talk
    matthew 16 0 anim
    this-one 15014 3 talk
    this-one 15015 5 talk
    102 actor 15016 7 7 talk-to
    this-one 15017 0 talk
    this-one 89 0 anim
    this-one wait-anim
    this-one 15018 7 talk
    ['] script-2 start-event
    30 frames wait
    this-one 567126 211485 walk-to
    this-one wait-stop
    matthew 15019 5 talk
    this-one 14 0 anim
    15 frames wait
    this-one 0 200 face
    this-one 15020 0 talk
    matthew 15021 5 talk
    15 music
    this-one 15022 3 talk
    matthew 15023 2 talk
    20 frames wait
    this-one 15024 7 talk
    matthew 85 87 0 anim-then
    this-one 567206 210985 walk-to
    this-one wait-stop
    this-one -3240 0 turn-to
    882 -3786 -3040 566133 -17552 211356 3038 camera-set
    this-one 5701 0 anim
    5 frames wait
    116 sound
    33 frames wait
    117 sound
    this-one wait-anim
    this-one 0 0 0 set-pos
    20 frames wait
    167 actor 568126 208355 -1 110 walk-to-ex
    167 actor wait-stop
    167 actor 0 0 0 set-pos
    matthew 14 15 anim-at  matthew wait-anim
    matthew 6618 200 turn-to
    matthew 0 1 anim
    20 frames wait
    26 actor 572593 0 205983 set-pos
    26 actor 0 -1054 0 set-rot
    end-script
    ;

\ its scripts by number
' script-0 0 269 pack-script!
' script-1 1 269 pack-script!
' script-2 2 269 pack-script!
' script-3 3 269 pack-script!
