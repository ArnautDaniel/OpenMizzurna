\ pack-048-3.fs - event pack 048_3 (CD.BIN 243): 5 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-048-3
USING: events ;

\ $8001001C
: script-0
    winona as-this-one
    this-one 15000 0 talk
    matthew 15001 5 talk
    this-one 15002 0 talk
    matthew 15003 7 talk
    this-one 15004 0 talk
    matthew 15005 4 talk
    this-one 15006 0 talk
    matthew 15007 6 talk
    this-one 15008 0 talk
    end-script
    ;

\ $8001004C
: script-1
    winona 15009 0 talk
    matthew 15010 7 talk
    winona 15011 0 talk
    matthew 15012 6 talk
    end-script
    ;

\ $80010078
: script-2
    winona as-this-one
    this-one 15024 0 talk
    matthew 15025 4 talk
    this-one 15026 0 talk
    matthew 15027 7 talk
    matthew 15028 5 talk
    this-one 15029 0 talk
    matthew 15030 4 talk
    this-one 15031 0 talk
    matthew 15032 6 talk
    this-one 15033 0 talk
    end-script
    ;

\ $800100AC
: script-3
    winona as-this-one
    matthew 15034 5 talk
    this-one 15035 0 talk
    matthew 15036 7 talk
    this-one 15037 0 talk
    matthew 15038 3 talk
    this-one 15039 0 talk
    matthew 15040 5 talk
    this-one 15041 0 talk
    this-one 15042 0 talk
    matthew 15043 7 talk
    this-one 15044 0 talk
    matthew 15045 2 talk
    this-one 15046 0 talk
    end-script
    ;

\ $800100EC
: script-4
    winona as-this-one
    matthew 15047 5 talk
    this-one 15048 0 talk
    matthew 15049 7 talk
    this-one 15050 0 talk
    matthew 15051 6 talk
    this-one 15052 0 talk
    15053 5 say-mode-wait
    end-script
    ;

\ its scripts by number
' script-0 0 243 pack-script!
' script-1 1 243 pack-script!
' script-2 2 243 pack-script!
' script-3 3 243 pack-script!
' script-4 4 243 pack-script!
