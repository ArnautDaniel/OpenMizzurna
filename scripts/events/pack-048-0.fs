\ pack-048-0.fs - event pack 048_0 (CD.BIN 240): 2 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-048-0
USING: events ;

\ $80010010
: script-0
    lidds as-this-one
    matthew 15000 5 talk
    this-one 15001 0 talk
    this-one 15002 0 talk
    matthew 15003 4 talk
    this-one 15004 0 talk
    matthew 15005 7 talk
    this-one 15006 0 talk
    matthew 15007 5 talk
    this-one 15008 0 talk
    matthew 15009 7 talk
    this-one 15010 0 talk
    matthew 15011 4 talk
    end-script
    ;

\ $8001004C
: script-1
    lidds as-this-one
    matthew 15012 7 talk
    this-one 15013 0 talk
    matthew 15014 5 talk
    this-one 15015 0 talk
    this-one 15016 0 talk
    matthew 15017 1 talk
    this-one 15018 0 talk
    matthew 15019 3 talk
    this-one 15020 0 talk
    matthew 15021 7 talk
    this-one 15022 0 talk
    matthew 15023 4 talk
    end-script
    ;

\ its scripts by number
' script-0 0 240 pack-script!
' script-1 1 240 pack-script!
