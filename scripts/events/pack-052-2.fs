\ pack-052-2.fs - event pack 052_2 (CD.BIN 262): 5 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-052-2
USING: events ;

\ $8001001C
: script-0
    cohen 5502 0 anim
    cohen anim-hold
    cohen wait-anim
    cohen 5503 1 anim
    10 frames wait
    cohen 5504 5505 1 anim-then
    end-script
    ;

\ $8001006C
: script-1
    5 frames wait
    matthew 5500 0 anim
    matthew wait-anim
    matthew 39767 -800 431250 set-pos
    matthew 5501 1 anim
    end-script
    ;

\ $800100B0
: script-2
    25 frames wait
    112 sound
    35 frames wait
    109 sound
    12 sound
    13 sound
    106 sound
    end-script
    ;

\ $800100EC
: script-3
    53 frames wait
    112 sound
    end-script
    ;

\ $80010100
: script-4
    cohen 40747 -800 431300 set-pos
    cohen 0 -1644 0 set-rot
    cohen 5546 1 anim
    matthew 40497 -800 431620 set-pos
    matthew 0 -2104 0 set-rot
    matthew 0 1 anim
    matthew 40497 431390 walk-to
    9 actor 41717 -800 431420 set-pos
    9 actor 0 -1924 0 set-rot
    9 actor 5515 1 anim
    -2024 -78 -2714 40659 -800 431416 -1864 camera-set
    25 frames wait
    15000 matthew say-by
    wait-text
    15001 cohen say-by
    wait-text
    matthew 48 50 1 anim-then
    15002 matthew say-by
    wait-text
    20 frames wait
    matthew 5509 anim-once
    cohen 5510 anim-once
    -704 -1452 -1422 41001 -908 429938 3824 camera-set
    57 frames wait
    104 0 sequence-play
    11 actor 40977 -800 430630 set-pos
    11 actor 0 426 0 set-rot
    11 actor 5506 anim-once
    10 frames wait
    256 10 rumble
    matthew wait-anim
    -404 528 -1206 40707 -1004 430316 2048 camera-set
    15004 cohen say-by
    wait-text
    15005 matthew say-by
    wait-text
    102 0 sequence-play
    -752 1878 -366 40683 -860 430262 4064 camera-set
    ' script-1 start-event
    ' script-0 start-event
    15 frames wait
    9 actor 40717 -800 431420 set-pos
    -284 -60 -258 40614 -704 430574 4136 camera-set
    14 music
    9 actor 5507 anim-once
    256 10 rumble
    255 10 rumble
    ' script-2 start-event
    40 frames wait
    -218 2052 -876 40233 -1244 430394 6440 camera-set
    40 frames wait
    15006 matthew say-by-wait
    40 frames wait
    -782 -840 -1146 41575 -188 431870 576 camera-set
    10 frames wait
    15007 cohen say-by-wait
    16 sound
    -272 -1368 -174 40091 -884 430898 -536 camera-set
    15008 cohen say-by-wait
    cohen 114 1 anim
    -218 2052 -876 40233 -1244 430394 6440 camera-set
    9 actor 5508 0 anim
    ' script-3 start-event
    9 actor wait-anim
    9 actor 0 3206 0 set-rot
    9 actor 5516 1 anim
    -698 -3938 -650 39155 -272 431316 2888 camera-set
    10 frames wait
    15009 cohen say-by
    wait-text
    give-control
    9 actor 5519 0 anim
    9 actor wait-anim
    10 music
    end-script
    ;

\ its scripts by number
' script-0 0 262 pack-script!
' script-1 1 262 pack-script!
' script-2 2 262 pack-script!
' script-3 3 262 pack-script!
' script-4 4 262 pack-script!
