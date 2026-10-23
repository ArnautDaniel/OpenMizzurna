\ pack-049-2.fs - event pack 049_2 (CD.BIN 247): 5 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-049-2
USING: events ;

\ $8001001C
: script-0
    cohen as-this-one
    this-one door-release
    -372 -3246 -1726 39167 -360 277157 -1028 camera-set
    matthew 15002 4 talk
    matthew 39528 0 277147 set-pos
    matthew 3 0 face
    -606 -1050 -520 39119 -360 277355 -428 camera-set
    10 frames wait
    15050 1 say-mode-wait
    15051 1 say-mode-wait
    15052 1 say-mode-wait
    10 frames wait
    this-one 15003 2 talk
    -372 -3246 -1726 39167 -360 277157 -1028 camera-set
    matthew 15004 4 talk
    10 frames wait
    this-one 85 0 1 anim-then
    10 frames wait
    this-one 15005 7 talk
    10 frames wait
    11 screen-effect
    wait-effect
    10 frames wait
    end-script
    ;

\ $80010160
: script-1
    cohen as-this-one
    this-one 39361 0 277201 set-pos
    this-one 0 1024 0 set-rot
    this-one 0 1 anim
    matthew 39516 0 277498 set-pos
    matthew 0 2800 0 set-rot
    matthew 0 1 anim
    -672 -3090 -94 39446 -360 277548 -4720 camera-set
    2 screen-effect
    wait-effect
    10 frames wait
    matthew 15006 7 talk
    this-one 15007 7 talk
    this-one 15008 5 talk
    matthew 87 0 anim
    matthew wait-anim
    matthew 39679 277170 walk-to
    3 frames wait
    this-one 15009 7 talk
    matthew 14 15 1 anim-then
    this-one 15010 5 talk
    matthew 15011 4 talk
    this-one 39070 276970 walk-to
    this-one wait-stop
    this-one 86 -2 through-door
    this-one place-at-stop
    end-script
    ;

\ $800102AC
: script-2
    dennis as-this-one
    matthew 95687 0 53386 set-pos
    matthew 0 46 0 set-rot
    132 actor 0 -1680 0 set-rot
    133 actor 0 2800 0 set-rot
    134 actor 0 -1260 0 set-rot
    this-one 92199 0 53156 set-pos
    this-one 0 1024 0 set-rot
    -600 -3066 -520 95653 -104 53356 526 camera-set
    133 actor 15012 1 talk
    matthew 14 15 1 anim-then
    132 actor 90 87 0 anim-then
    133 actor 83 84 1 anim-then
    134 actor 15013 5 talk
    10 frames wait
    133 actor 85 0 anim
    133 actor wait-anim
    133 actor 14 15 1 anim-then
    134 actor 86 90 0 anim-then
    132 actor 87 86 0 anim-then
    132 actor wait-anim
    133 actor 16 0 anim
    133 actor wait-anim
    matthew 16 0 1 anim-then
    133 actor 15014 2 talk
    132 actor 15015 4 talk
    matthew 15016 1 talk
    matthew 14 15 1 anim-then
    134 actor 15017 3 talk
    133 actor 85 0 anim
    133 actor wait-anim
    133 actor 17 18 1 anim-then
    matthew 16 0 anim
    133 actor 15018 0 talk
    132 actor 15019 1 talk
    133 actor 19 86 0 anim-then
    -794 -50 camera-angle
    95653 -104 53722 50 camera-move
    10 frames wait
    2 object-anim
    this-one 64 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 94519 53391 walk-to
    133 actor 87 0 anim
    matthew 17 18 anim-at  matthew wait-anim
    matthew -934 -100 turn-to
    matthew 19 0 anim
    this-one wait-stop
    -462 -324 -388 94861 -140 53164 3422 camera-set
    matthew 95507 0 53086 set-pos
    matthew 0 -934 0 set-rot
    132 actor 95369 0 54381 set-pos
    132 actor 0 -1550 0 set-rot
    133 actor 95219 0 53541 set-pos
    133 actor 0 2760 0 set-rot
    134 actor 95979 0 53321 set-pos
    134 actor 0 -1260 0 set-rot
    15053 1 say-mode-wait
    15054 1 say-mode-wait
    15055 1 say-mode-wait
    -660 -2028 -352 95185 -104 53764 1958 camera-set
    this-one 15020 5 talk
    133 actor 15021 3 talk
    132 actor 15022 4 talk
    this-one 15023 1 talk
    133 actor 15024 5 talk
    134 actor 15025 3 talk
    this-one 15026 5 talk
    133 actor 15027 1 talk
    this-one 15028 6 talk
    133 actor 15029 5 talk
    133 actor 86 0 anim
    132 actor 87 0 anim
    134 actor 87 0 anim
    133 actor wait-anim
    matthew 95497 0 52856 set-pos
    matthew 0 -884 0 set-rot
    132 actor depart-alt
    -564 -1668 -796 95077 -104 53152 3206 camera-set
    133 actor 95989 53531 walk-to
    133 actor wait-stop
    133 actor depart-alt
    134 actor depart-alt
    this-one 15030 3 talk
    this-one 14 15 1 anim-then
    this-one 15031 0 talk
    this-one 92589 53201 walk-to
    this-one wait-stop
    end-script
    ;

\ $80010798
: script-3
    26 actor 0 0 0 set-pos
    winona 175708 0 418022 set-pos
    winona 0 1 anim
    matthew 176023 0 417738 set-pos
    matthew 0 1 anim
    matthew 15 0 face
    winona 0 0 face
    hudson 175713 0 423088 set-pos
    hudson 0 2048 0 set-rot
    morgan 175576 0 423577 set-pos
    morgan 0 2048 0 set-rot
    127 actor 173813 0 416698 set-pos
    127 actor 0 560 0 set-rot
    137 actor 173013 0 416678 set-pos
    137 actor 0 490 0 set-rot
    -726 -3318 -412 174110 -4080 416732 4451 camera-set
    map-sync
    10 frames wait
    winona 15032 1 talk
    matthew 15033 5 talk
    winona 15034 3 talk
    -708 -2328 -1246 175735 -4082 422742 -42 camera-set
    hudson 175243 423088 walk-to
    hudson wait-stop
    hudson 0 1024 0 set-rot
    morgan 27 0 through-door
    -696 -3030 -1516 175801 -4082 417936 -210 camera-set
    matthew 1 0 face
    20 frames wait
    winona 1 -300 face
    175657 -4082 421284 35 camera-move
    15035 matthew say-by
    morgan wait-anim
    matthew 175813 420708 walk-to
    morgan 175715 421358 walk-to
    10 frames wait
    winona 175368 420842 walk-to
    wait-text
    matthew wait-stop
    winona 1 0 face
    matthew 15036 3 talk
    morgan 15037 1 talk
    morgan 15038 2 talk
    morgan 85 0 anim
    morgan wait-anim
    morgan 15039 3 talk
    matthew 15040 5 talk
    morgan 15041 7 talk
    morgan 15042 4 talk
    20 frames wait
    end-script
    ;

\ $80010A64
: script-4
    matthew 329823 -4080 269337 set-pos
    matthew 0 3072 0 set-rot
    -828 -3142 -1466 329967 -4080 269265 -996 camera-set
    10 frames wait
    15043 32767 actor say-by-wait
    15044 32767 actor say-by-wait
    15045 32767 actor say-by-wait
    15046 32767 actor say-by-wait
    15047 32767 actor say-by-wait
    15048 32767 actor say-by-wait
    15049 32767 actor say-by-wait
    end-script
    ;

\ its scripts by number
' script-0 0 247 pack-script!
' script-1 1 247 pack-script!
' script-2 2 247 pack-script!
' script-3 3 247 pack-script!
' script-4 4 247 pack-script!
