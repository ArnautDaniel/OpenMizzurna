\ pack-048-2.fs - event pack 048_2 (CD.BIN 242): 5 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-048-2
USING: events ;

\ $8001001C
: script-0
    samuel as-this-one
    matthew 15000 5 talk
    this-one 15001 0 talk
    matthew 15002 7 talk
    this-one 15003 0 talk
    matthew 15004 5 talk
    this-one 15005 0 talk
    matthew 15006 4 talk
    this-one 15007 0 talk
    this-one 15008 0 talk
    this-one 15009 0 talk
    this-one 15010 0 talk
    matthew 15011 7 talk
    this-one 15012 0 talk
    this-one 15013 0 talk
    matthew 15014 5 talk
    this-one 15015 0 talk
    matthew 15016 7 talk
    this-one 15017 0 talk
    this-one 15018 0 talk
    matthew 15019 4 talk
    end-script
    ;

\ $80010078
: script-1
    samuel as-this-one
    matthew 15020 5 talk
    this-one 15021 0 talk
    matthew 15022 4 talk
    this-one 15023 0 talk
    matthew 15024 3 talk
    this-one 15025 0 talk
    matthew 15026 6 talk
    this-one 15027 0 talk
    this-one 15028 0 talk
    matthew 15029 7 talk
    this-one 15030 0 talk
    this-one 15031 0 talk
    matthew 15032 1 talk
    this-one 15033 0 talk
    matthew 15034 7 talk
    this-one 15035 0 talk
    matthew 15036 6 talk
    this-one 15037 0 talk
    this-one 15038 0 talk
    matthew 15039 3 talk
    this-one 15040 0 talk
    this-one 15041 0 talk
    15042 5 say-mode-wait
    end-script
    ;

\ $800100E8
: script-2
    samuel as-this-one
    matthew 15043 7 talk
    this-one 15044 0 talk
    matthew 15045 5 talk
    this-one 15046 0 talk
    matthew 15047 7 talk
    this-one 15048 0 talk
    matthew 15049 6 talk
    this-one 15050 0 talk
    end-script
    ;

\ $80010114
: script-3
    samuel as-this-one
    matthew 15051 5 talk
    this-one 15052 0 talk
    matthew 15053 4 talk
    this-one 15054 0 talk
    matthew 15055 7 talk
    this-one 15056 0 talk
    matthew 15057 6 talk
    end-script
    ;

\ $8001013C
: script-4
    samuel as-this-one
    this-one 15058 1 talk
    this-one 15059 5 talk
    this-one 15060 7 talk
    end-script
    ;

\ its scripts by number
' script-0 0 242 pack-script!
' script-1 1 242 pack-script!
' script-2 2 242 pack-script!
' script-3 3 242 pack-script!
' script-4 4 242 pack-script!
