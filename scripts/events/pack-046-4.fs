\ pack-046-4.fs - event pack 046_4 (CD.BIN 234): 3 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-046-4
USING: events ;

\ $80010014
: script-0
    mel as-this-one
    this-one 15000 0 talk
    matthew 15001 5 talk
    this-one 15002 0 talk
    matthew 15003 7 talk
    this-one 15004 0 talk
    matthew 15005 5 talk
    this-one 15006 0 talk
    matthew 15007 6 talk
    this-one 15008 0 talk
    this-one 15009 0 talk
    this-one 15010 0 talk
    this-one 15011 0 talk
    matthew 15012 5 talk
    this-one 15013 0 talk
    end-script
    ;

\ $80010058
: script-1
    mel as-this-one
    matthew 15014 5 talk
    this-one 15015 0 talk
    this-one 15016 0 talk
    matthew 15017 6 talk
    this-one 15018 0 talk
    end-script
    ;

\ $80010078
: script-2
    mel as-this-one
    matthew 15019 0 talk
    this-one 15020 3 talk
    matthew 15021 5 talk
    this-one 15022 1 talk
    matthew 15023 5 talk
    this-one 15024 6 talk
    matthew 15025 0 talk
    this-one 15026 7 talk
    matthew 15027 0 talk
    this-one 15028 5 talk
    end-script
    ;

\ its scripts by number
' script-0 0 234 pack-script!
' script-1 1 234 pack-script!
' script-2 2 234 pack-script!
