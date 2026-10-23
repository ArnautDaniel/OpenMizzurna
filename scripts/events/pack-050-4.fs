\ pack-050-4.fs - event pack 050_4 (CD.BIN 254): 1 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-050-4
USING: events ;

\ $8001000C
: script-0
    morgan as-this-one
    matthew 103879 0 359121 set-pos
    matthew 0 -350 0 set-rot
    matthew 0 1 anim
    matthew 2 1 set-floor
    this-one 103489 0 359121 set-pos
    this-one 0 0 0 set-rot
    this-one 0 1 anim
    this-one 2 1 set-floor
    barbara 104979 0 359371 set-pos
    barbara 0 -1260 0 set-rot
    barbara 126 1 anim
    barbara 2 1 set-floor
    -1004 -770 -1340 103839 192 358365 8016 camera-set
    8 screen-effect
    wait-effect
    this-one 15000 7 talk
    176 actor 15001 8 talk
    matthew 14 15 anim-at  matthew wait-anim
    8544 20 camera-angle
    10 frames wait
    barbara 104615 359241 -1 15 walk-to-ex
    wait-camera-turn
    matthew 0 1 anim
    matthew 90 200 face
    barbara 15002 0 talk
    this-one 103889 358781 walk-to
    this-one wait-stop
    this-one 90 -200 face
    this-one 15003 7 talk
    20 frames wait
    -644 -872 -506 104769 252 359343 544 camera-set
    15020 1 say-mode-wait
    15021 1 say-mode-wait
    barbara 15004 7 talk
    -632 -1064 -1250 103803 -144 358713 424 camera-set
    barbara 713 715 0 anim-then
    barbara wait-anim
    barbara 126 1 anim
    barbara 15005 0 talk
    matthew 15006 7 talk
    barbara 15007 7 talk
    barbara 703 704 anim-at  barbara wait-anim
    barbara 15008 0 talk
    20 frames wait
    barbara 705 126 anim-at  barbara wait-anim
    barbara 713 714 1 anim-then
    barbara 15009 0 talk
    barbara 715 126 1 anim-then
    this-one 15010 7 talk
    barbara 707 708 anim-at  barbara wait-anim
    10 frames wait
    barbara 15011 0 talk
    -464 -1568 -722 104775 -60 358749 -680 camera-set
    this-one 17 18 1 anim-then
    176 actor 15012 8 talk
    this-one 0 1 anim
    this-one 0 -200 face
    this-one 15013 0 talk
    176 actor 15014 8 talk
    barbara 709 126 anim-at  barbara wait-anim
    barbara 15015 7 talk
    this-one 14 15 1 anim-then
    30 frames wait
    this-one 16 0 anim
    this-one wait-anim
    -284 -1592 -74 102723 -60 358665 688 camera-set
    this-one 103349 358551 walk-to
    matthew -1452 200 turn-to
    matthew wait-turned
    matthew 103359 358881 walk-to
    matthew wait-stop
    matthew 15016 0 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 15017 0 talk
    matthew 15018 0 talk
    this-one 16 0 anim-at  this-one wait-anim
    this-one 15019 0 talk
    10 frames wait
    end-script
    ;

\ its scripts by number
' script-0 0 254 pack-script!
