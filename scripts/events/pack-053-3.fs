\ pack-053-3.fs - event pack 053_3 (CD.BIN 268): 1 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-053-3
USING: events ;

\ $8001000C
: script-0
    james as-this-one
    15000 32767 actor say-by-wait
    james 100458 0 354709 set-pos
    james 0 1 anim
    james 2 1 set-floor
    matthew 101628 0 356139 set-pos
    -562 -2492 -248 101556 132 356013 -1952 camera-set
    matthew 101248 355129 -200 face-point
    matthew 0 1 anim
    10 frames wait
    this-one 101248 355129 walk-to
    this-one wait-stop
    this-one 0 -200 face
    matthew 15001 7 talk
    this-one 15002 4 talk
    matthew 15003 6 talk
    -436 -158 -416 101814 -24 355587 -1472 camera-set
    15052 1 say-mode-wait
    15053 1 say-mode-wait
    15054 1 say-mode-wait
    this-one 15004 6 talk
    -100 -1778 -1964 101250 -84 355767 -1448 camera-set
    this-one 101428 355669 walk-to
    matthew 15005 7 talk
    this-one 15006 0 talk
    matthew 15007 5 talk
    this-one 15008 6 talk
    matthew 15009 1 talk
    this-one 741 742 anim-at  this-one wait-anim
    20 frames wait
    this-one 15010 0 talk
    10 frames wait
    matthew 15011 7 talk
    this-one 743 0 anim-at  this-one wait-anim
    -748 -1274 -674 101622 192 355731 1816 camera-set
    this-one 83 84 anim-at  this-one wait-anim
    20 frames wait
    this-one 85 0 anim-at  this-one wait-anim
    this-one 15012 5 talk
    10 frames wait
    matthew 15013 7 talk
    this-one 15014 2 talk
    10 frames wait
    this-one 85 0 1 anim-then
    matthew 15015 3 talk
    matthew 15016 5 talk
    this-one 15017 4 talk
    matthew 15018 6 talk
    this-one 15019 3 talk
    this-one 15020 7 talk
    matthew 15021 0 talk
    15 frames wait
    matthew 15022 0 talk
    this-one 15023 7 talk
    matthew 86 0 anim
    matthew wait-anim
    matthew 83 84 anim-at  matthew wait-anim
    20 frames wait
    matthew 15024 0 talk
    this-one 15025 3 talk
    this-one 15026 0 talk
    this-one 15027 7 talk
    matthew 85 0 anim-at  matthew wait-anim
    matthew 15028 7 talk
    this-one 15029 7 talk
    matthew 15030 6 talk
    this-one 15031 4 talk
    this-one 15032 3 talk
    matthew 15033 0 talk
    this-one 15034 5 talk
    this-one 15035 6 talk
    matthew 15036 7 talk
    this-one 15037 7 talk
    10 frames wait
    matthew 15038 7 talk
    this-one 15039 2 talk
    this-one 85 0 anim
    matthew 15040 7 talk
    this-one 15041 4 talk
    this-one 15042 3 talk
    matthew 15043 6 talk
    this-one 15044 4 talk
    this-one 15045 2 talk
    10 frames wait
    -712 -5210 -728 101622 252 355587 0 camera-set
    10 frames wait
    15046 3 say-mode-wait
    120 ambient-stop
    121 sound
    this-one 14 15 anim-at  this-one wait-anim
    -748 -1274 -674 101622 192 355731 1816 camera-set
    this-one 15047 0 talk
    this-one 16 0 anim
    this-one 15048 0 talk
    this-one 15049 5 talk
    this-one 15050 7 talk
    matthew 15051 6 talk
    this-one 87 0 anim
    this-one wait-anim
    end-script
    ;

\ its scripts by number
' script-0 0 268 pack-script!
