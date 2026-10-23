\ pack-047-3.fs - event pack 047_3 (CD.BIN 238): 4 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-047-3
USING: events ;

\ $80010018
: script-0
    dennis as-this-one
    matthew 15000 5 talk
    this-one 15001 0 talk
    matthew 15002 7 talk
    this-one 15003 0 talk
    matthew 15004 3 talk
    this-one 15005 0 talk
    end-script
    ;

\ $8001003C
: script-1
    dennis as-this-one
    this-one 15006 0 talk
    matthew 15007 7 talk
    this-one 15008 0 talk
    this-one 15009 0 talk
    end-script
    ;

\ $80010058
: script-2
    dennis as-this-one
    this-one 15010 0 talk
    matthew 15011 7 talk
    this-one 15012 0 talk
    this-one 15013 0 talk
    end-script
    ;

\ $80010074
: script-3
    dennis as-this-one
    this-one 15014 0 talk
    matthew 15015 7 talk
    this-one 15016 0 talk
    matthew 15017 5 talk
    this-one 15018 0 talk
    this-one 15019 0 talk
    end-script
    ;

\ its scripts by number
' script-0 0 238 pack-script!
' script-1 1 238 pack-script!
' script-2 2 238 pack-script!
' script-3 3 238 pack-script!
