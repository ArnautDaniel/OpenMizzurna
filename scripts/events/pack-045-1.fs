\ pack-045-1.fs - event pack 045_1 (CD.BIN 226): 38 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-045-1
USING: events ;

\ $800100A0
: script-6
    rolein as-this-one
    this-one 15000 0 talk
    matthew 15001 0 talk
    this-one 15002 0 talk
    matthew 15003 0 talk
    this-one 15004 0 talk
    end-script
    ;

\ $800100C0
: script-7
    rolein 15005 0 talk
    end-script
    ;

\ $800100D4
: script-4
    rolein as-this-one
    matthew 15006 0 talk
    this-one 15007 0 talk
    this-one 15008 0 talk
    matthew 15009 0 talk
    this-one 15010 0 talk
    this-one 15011 0 talk
    end-script
    ;

\ $800100F8
: script-5
    rolein 15012 0 talk
    end-script
    ;

\ $8001010C
: script-0
    rolein as-this-one
    this-one 15013 0 talk
    matthew 15014 0 talk
    this-one 15015 0 talk
    matthew 15016 0 talk
    matthew 15017 0 talk
    this-one 15018 0 talk
    end-script
    ;

\ $80010130
: script-1
    rolein 15019 0 talk
    end-script
    ;

\ $80010144
: script-22
    rolein as-this-one
    this-one 15020 0 talk
    this-one 15021 0 talk
    matthew 15022 0 talk
    this-one 15023 0 talk
    this-one 15024 0 talk
    end-script
    ;

\ $80010164
: script-23
    rolein 15025 0 talk
    end-script
    ;

\ $80010178
: script-10
    rolein as-this-one
    this-one 15026 0 talk
    this-one 15027 0 talk
    end-script
    ;

\ $8001018C
: script-11
    rolein 15028 0 talk
    end-script
    ;

\ $800101A0
: script-16
    rolein 15029 0 talk
    matthew 15030 0 talk
    rolein 15031 0 talk
    rolein 15032 0 talk
    matthew 15033 0 talk
    rolein 15034 0 talk
    end-script
    ;

\ $800101EC
: script-17
    rolein 15035 0 talk
    end-script
    ;

\ $80010200
: script-36
    rolein as-this-one
    this-one 15036 0 talk
    matthew 15037 0 talk
    this-one 15038 0 talk
    matthew 15039 0 talk
    this-one 15040 0 talk
    this-one 15041 0 talk
    matthew 15042 0 talk
    this-one 15043 0 talk
    this-one 15044 0 talk
    end-script
    ;

\ $80010260
: script-37
    rolein 15045 0 talk
    end-script
    ;

\ $80010274
: script-34
    rolein as-this-one
    this-one 15046 0 talk
    this-one 15047 0 talk
    this-one 15048 0 talk
    matthew 15049 0 talk
    this-one 15050 0 talk
    this-one 15051 0 talk
    matthew 15052 0 talk
    end-script
    ;

\ $8001029C
: script-35
    rolein 15053 0 talk
    end-script
    ;

\ $800102B0
: script-26
    rolein 15054 0 talk
    15055 5 say-mode-wait
    end-script
    ;

\ $800102D0
: script-27
    rolein 15056 0 talk
    end-script
    ;

\ $800102E4
: script-14
    rolein as-this-one
    this-one 15057 0 talk
    matthew 15058 0 talk
    this-one 15059 0 talk
    matthew 15060 0 talk
    this-one 15061 0 talk
    this-one 15062 0 talk
    matthew 15063 0 talk
    this-one 15064 0 talk
    end-script
    ;

\ $80010310
: script-15
    rolein 15065 0 talk
    rolein 15066 0 talk
    end-script
    ;

\ $80010334
: script-28
    matthew 15067 7 talk
    rolein 15068 0 talk
    rolein 15069 0 talk
    end-script
    ;

\ $8001035C
: script-29
    rolein 15070 0 talk
    end-script
    ;

\ $80010370
: script-30
    rolein 15071 0 talk
    matthew 15072 0 talk
    rolein 15073 0 talk
    matthew 15074 0 talk
    rolein 15075 0 talk
    end-script
    ;

\ $800103AC
: script-31
    rolein 15076 0 talk
    end-script
    ;

\ $800103C0
: script-32
    rolein 15077 0 talk
    matthew 15078 0 talk
    rolein 15079 0 talk
    rolein 15080 0 talk
    end-script
    ;

\ $800103F8
: script-33
    rolein 15081 0 talk
    end-script
    ;

\ $8001040C
: script-24
    rolein 15082 0 talk
    rolein 15083 0 talk
    rolein 15084 0 talk
    matthew 15085 0 talk
    rolein 15086 0 talk
    end-script
    ;

\ $80010454
: script-25
    rolein 15087 0 talk
    end-script
    ;

\ its scripts by number
' script-0 0 226 pack-script!
' script-1 1 226 pack-script!
' script-0 2 226 pack-script!
' script-1 3 226 pack-script!
' script-4 4 226 pack-script!
' script-5 5 226 pack-script!
' script-6 6 226 pack-script!
' script-7 7 226 pack-script!
' script-10 10 226 pack-script!
' script-11 11 226 pack-script!
' script-14 14 226 pack-script!
' script-15 15 226 pack-script!
' script-16 16 226 pack-script!
' script-17 17 226 pack-script!
' script-22 22 226 pack-script!
' script-23 23 226 pack-script!
' script-24 24 226 pack-script!
' script-25 25 226 pack-script!
' script-26 26 226 pack-script!
' script-27 27 226 pack-script!
' script-28 28 226 pack-script!
' script-29 29 226 pack-script!
' script-30 30 226 pack-script!
' script-31 31 226 pack-script!
' script-32 32 226 pack-script!
' script-33 33 226 pack-script!
' script-34 34 226 pack-script!
' script-35 35 226 pack-script!
' script-36 36 226 pack-script!
' script-37 37 226 pack-script!
