\ pack-047-0.fs - event pack 047_0 (CD.BIN 235): 6 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-047-0
USING: events ;

\ $80010020
: script-0
    father-barton as-this-one
    matthew 15000 5 talk
    this-one 15001 0 talk
    matthew 15002 7 talk
    this-one 15003 0 talk
    this-one 15004 0 talk
    this-one 15005 0 talk
    this-one 15006 0 talk
    this-one 15007 0 talk
    this-one 15008 0 talk
    this-one 15009 0 talk
    this-one 15010 0 talk
    end-script
    ;

\ $80010058
: script-1
    matthew 15011 5 talk
    father-barton 15012 0 talk
    father-barton 15013 0 talk
    4 map-reveal
    end-script
    ;

\ $80010088
: script-2
    father-barton as-this-one
    matthew 15014 5 talk
    this-one 15015 0 talk
    matthew 15016 7 talk
    this-one 15017 0 talk
    matthew 15018 4 talk
    this-one 15019 0 talk
    matthew 15020 7 talk
    this-one 15021 0 talk
    this-one 15022 0 talk
    this-one 15023 0 talk
    end-script
    ;

\ $800100BC
: script-3
    father-barton as-this-one
    matthew 15024 5 talk
    this-one 15025 0 talk
    matthew 15026 7 talk
    this-one 15027 0 talk
    matthew 15028 5 talk
    this-one 15029 0 talk
    matthew 15030 1 talk
    this-one 15031 0 talk
    matthew 15032 7 talk
    this-one 15033 0 talk
    this-one 15034 0 talk
    matthew 15035 4 talk
    end-script
    ;

\ $800100F8
: script-4
    father-barton as-this-one
    this-one 15036 0 talk
    matthew 15037 4 talk
    this-one 15038 0 talk
    matthew 15039 3 talk
    this-one 15040 0 talk
    end-script
    ;

\ $80010118
: script-5
    father-barton as-this-one
    matthew 15041 5 talk
    this-one 15042 0 talk
    matthew 15043 6 talk
    this-one 15044 0 talk
    matthew 15045 7 talk
    this-one 15046 0 talk
    matthew 15047 6 talk
    matthew 15048 5 talk
    this-one 15049 0 talk
    this-one 15050 0 talk
    matthew 15051 7 talk
    end-script
    ;

\ its scripts by number
' script-0 0 235 pack-script!
' script-1 1 235 pack-script!
' script-2 2 235 pack-script!
' script-3 3 235 pack-script!
' script-4 4 235 pack-script!
' script-5 5 235 pack-script!
