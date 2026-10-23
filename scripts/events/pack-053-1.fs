\ pack-053-1.fs - event pack 053_1 (CD.BIN 266): 2 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-053-1
USING: events ;

\ $80010010
: script-0
    winona as-this-one
    108 actor 103381 0 358978 set-pos
    108 actor 0 420 0 set-rot
    108 actor 745 1 anim
    matthew 104011 0 358378 set-pos
    matthew 0 3530 0 set-rot
    matthew 0 1 anim
    winona unride
    this-one 104551 0 358578 set-pos
    this-one 0 -724 0 set-rot
    this-one 0 1 anim
    -452 -3248 -164 103135 0 358488 2986 camera-set
    map-sync
    10 frames wait
    this-one 130 131 1 anim-then
    124 sound
    14 music
    10 frames wait
    matthew 15001 0 talk
    matthew 103841 358988 walk-to
    matthew wait-stop
    matthew -598 0 turn-to
    -124 -1772 -992 103999 0 358884 1090 camera-set
    matthew 48 50 anim-at  matthew wait-anim
    30 frames wait
    matthew 15002 0 talk
    this-one 15003 0 talk
    -304 -2738 -236 103219 -144 357552 898 camera-set
    103219 -144 358488 12 camera-move
    matthew 15004 0 talk
    this-one 15005 0 talk
    matthew 15006 0 talk
    wait-camera-move
    matthew 49 0 anim-at  matthew wait-anim
    103219 -144 358056 36 camera-move
    wait-camera-move
    10 frames wait
    884 -2918 -2252 103219 -144 359310 -854 camera-set
    matthew 97 98 anim-at  matthew wait-anim
    matthew 100 101 anim-at  matthew wait-anim
    3 1 sequence-play
    80 frames wait
    matthew 15007 0 talk
    3 sequence-stop
    matthew 103 0 anim-at  matthew wait-anim
    this-one 132 0 1 anim-then
    matthew 15 200 face
    matthew 137 0 anim-at  matthew wait-anim
    matthew 15008 0 talk
    this-one 104031 358848 walk-to
    matthew 59 60 anim-at  matthew wait-anim
    this-one wait-stop
    this-one 15009 4 talk
    this-one 59 61 0 anim-then
    15 frames wait
    matthew 61 0 anim-at  matthew wait-anim
    matthew 15010 6 talk
    this-one 15011 5 talk
    matthew 15012 4 talk
    10 frames wait
    this-one 104331 358488 2 20 walk-to-ex
    this-one wait-stop
    this-one 104451 357828 5 60 walk-to-ex
    this-one wait-stop
    this-one 0 0 0 set-pos
    26 actor 0 0 0 set-pos
    10 frames wait
    end-script
    ;

\ $800103D4
: script-1
    10 actor as-this-one
    10 actor 101679 0 351814 set-pos
    10 actor 0 420 0 set-rot
    10 actor 109 1 anim
    10 actor 29 2 set-floor
    matthew 101459 0 353434 set-pos
    matthew 0 1710 0 set-rot
    matthew 0 1 anim
    matthew 29 2 set-floor
    -254 -1880 -104 101739 -1200 352414 1990 camera-set
    10 frames wait
    18 music
    15013 32767 actor 1 say-timed-by
    matthew 101669 352074 5 60 walk-to-ex
    matthew wait-stop
    matthew 1710 0 turn-to
    matthew 48 50 anim-at  matthew wait-anim
    matthew 15014 0 talk
    13 sound
    10 frames wait
    16 sound
    12 frames wait
    16 sound
    10 frames wait
    16 sound
    5 frames wait
    this-one 15016 0 talk
    matthew 15017 0 talk
    10 frames wait
    this-one 15018 0 talk
    this-one 15019 0 talk
    124 sound
    5 frames wait
    matthew 49 0 anim-at  matthew wait-anim
    124 sound
    matthew 81 0 anim
    matthew 15020 0 talk
    10 frames wait
    matthew 0 0 turn-to
    end-script
    ;

\ its scripts by number
' script-0 0 266 pack-script!
' script-1 1 266 pack-script!
