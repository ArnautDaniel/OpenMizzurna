\ pack-052-0.fs - event pack 052_0 (CD.BIN 260): 1 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-052-0
USING: events ;

\ $8001000C
: script-0
    7 set-scene
    9 actor 36312 0 407320 set-pos
    9 actor 0 6491 0 set-rot
    9 actor 5522 1 anim
    matthew 35412 0 407160 set-pos
    matthew 0 1024 0 set-rot
    matthew 0 1 anim
    morgan 35900 0 407882 set-pos
    morgan 26 1 set-floor
    morgan 2 set-y
    morgan 0 0 face
    morgan 72 1 anim
    11 actor 0 0 0 set-pos
    -170 -2334 -1680 35844 -144 407562 9106 camera-set
    map-sync
    2 screen-effect
    wait-effect
    30 frames wait
    15000 morgan say-by
    morgan 79 72 1 anim-then
    wait-text
    15001 matthew say-by
    matthew 1 -120 face
    matthew 87 82 0 anim-then
    wait-text
    15002 morgan say-by
    wait-text
    morgan 76 77 1 anim-then
    30 frames wait
    15003 morgan say-by
    wait-text
    matthew 14 15 1 anim-then
    60 frames wait
    15004 morgan say-by
    morgan 78 72 1 anim-then
    wait-text
    15005 matthew say-by
    wait-text
    morgan 79 72 1 anim-then
    15006 morgan say-by
    wait-text
    15007 matthew say-by
    wait-text
    morgan 76 77 1 anim-then
    15008 morgan say-by
    wait-text
    15009 matthew say-by
    wait-text
    15010 morgan say-by
    wait-text
    15011 morgan say-by
    wait-text
    morgan 78 71 0 anim-then
    morgan wait-anim
    15012 morgan say-by
    wait-text
    end-script
    ;

\ its scripts by number
' script-0 0 260 pack-script!
