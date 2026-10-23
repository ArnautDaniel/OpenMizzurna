\ pack-047-2.fs - event pack 047_2 (CD.BIN 237): 3 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-047-2
USING: events ;

\ $80010014
: script-0
    cohen as-this-one
    matthew 15000 5 talk
    this-one 15001 0 talk
    this-one 15002 0 talk
    matthew 15003 7 talk
    this-one 15004 0 talk
    this-one 15005 0 talk
    end-script
    ;

\ $80010038
: script-1
    cohen as-this-one
    matthew 15006 5 talk
    this-one 15007 0 talk
    matthew 15008 7 talk
    this-one 15009 0 talk
    matthew 15010 5 talk
    this-one 15011 0 talk
    matthew 15012 6 talk
    this-one 15013 0 talk
    matthew 15014 3 talk
    this-one 15015 0 talk
    matthew 15016 4 talk
    end-script
    ;

\ $80010070
: script-2
    matthew 15017 7 talk
    matthew 15018 5 talk
    cohen 15019 0 talk
    matthew 15020 4 talk
    cohen 15021 6 talk
    matthew 15022 0 talk
    cohen 15023 7 talk
    15 frames wait
    cohen 15024 0 talk
    matthew 15025 7 talk
    end-script
    ;

\ its scripts by number
' script-0 0 237 pack-script!
' script-1 1 237 pack-script!
' script-2 2 237 pack-script!
