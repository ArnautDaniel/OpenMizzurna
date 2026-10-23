\ pack-050-1.fs - event pack 050_1 (CD.BIN 251): 1 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-050-1
USING: events ;

\ $8001000C
: script-0
    91 actor 32784 0 427096 set-pos
    91 actor 0 -1980 0 set-rot
    91 actor 109 1 anim
    91 actor -760 set-y
    cohen 26 1 set-floor
    cohen 40174 -770 412792 set-pos
    24 actor 40174 -770 412792 set-pos
    cohen 0 -3345 0 set-rot
    cohen 5538 1 anim
    cohen as-this-one
    camera-follow
    matthew 42227 415365 5 70 walk-to-ex
    matthew wait-stop
    camera-fixed
    -572 -1406 -680 40316 2 412975 -1659 camera-set
    cohen 24 actor ignore-a
    this-one 0 -300 face
    this-one wait-turned
    this-one 15000 0 talk
    -572 -1730 -680 40257 2 413021 -975 camera-set
    matthew 40371 413287 5 70 walk-to-ex
    matthew wait-stop
    matthew 15001 7 talk
    this-one 15002 1 talk
    matthew 15003 7 talk
    this-one 15004 7 talk
    matthew 15005 7 talk
    this-one 15006 5 talk
    matthew 15007 7 talk
    cohen 1 ride
    end-script
    ;

\ its scripts by number
' script-0 0 251 pack-script!
