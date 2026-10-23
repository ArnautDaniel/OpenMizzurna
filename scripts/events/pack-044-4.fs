\ pack-044-4.fs - event pack 044_4 (CD.BIN 224): 38 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-044-4
USING: events ;

\ $800100A0
: script-6
    morgan as-this-one
    this-one 15000 0 talk
    matthew 15001 5 talk
    this-one 15002 0 talk
    matthew 15003 4 talk
    this-one 15004 0 talk
    this-one 15005 0 talk
    end-script
    ;

\ $800100C4
: script-7
    morgan 15006 0 talk
    end-script
    ;

\ $800100D8
: script-4
    morgan 15007 0 talk
    matthew 15008 5 talk
    morgan 15009 0 talk
    end-script
    ;

\ $80010100
: script-5
    morgan 15010 0 talk
    end-script
    ;

\ $80010114
: script-0
    morgan as-this-one
    this-one 15011 0 talk
    this-one 15012 0 talk
    15013 5 say-mode-wait
    matthew 15014 7 talk
    this-one 15015 0 talk
    end-script
    ;

\ $8001013C
: script-1
    15016 5 say-mode-wait
    end-script
    ;

\ $8001014C
: script-22
    morgan as-this-one
    this-one 15017 0 talk
    this-one 15018 0 talk
    this-one 15019 0 talk
    end-script
    ;

\ $80010164
: script-23
    morgan 15020 0 talk
    end-script
    ;

\ $80010178
: script-10
    morgan as-this-one
    this-one 15021 0 talk
    matthew 15022 5 talk
    this-one 15023 0 talk
    matthew 15024 7 talk
    this-one 15025 0 talk
    matthew 15026 4 talk
    this-one 15027 0 talk
    this-one 15028 0 talk
    matthew 15029 7 talk
    end-script
    ;

\ $800101A8
: script-11
    morgan 15030 0 talk
    end-script
    ;

\ $800101BC
: script-16
    morgan as-this-one
    this-one 15031 0 talk
    matthew 15032 4 talk
    this-one 15034 0 talk
    matthew 15035 7 talk
    end-script
    ;

\ $800101D8
: script-17
    morgan 15036 0 talk
    end-script
    ;

\ $800101EC
: script-36
    matthew 15037 7 talk
    matthew 15038 5 talk
    morgan 15039 0 talk
    matthew 15040 2 talk
    end-script
    ;

\ $8001020C
: script-37
    morgan 15041 0 talk
    end-script
    ;

\ $80010220
: script-34
    morgan 15042 0 talk
    morgan 15043 0 talk
    morgan 15044 0 talk
    morgan 15045 0 talk
    end-script
    ;

\ $80010264
: script-35
    morgan 15046 0 talk
    end-script
    ;

\ $80010278
: script-26
    morgan as-this-one
    this-one 15047 0 talk
    matthew 15048 7 talk
    this-one 15049 0 talk
    this-one 15050 0 talk
    this-one 15051 0 talk
    this-one 15052 0 talk
    end-script
    ;

\ $8001029C
: script-27
    morgan 15053 0 talk
    end-script
    ;

\ $800102B0
: script-14
    morgan as-this-one
    this-one 15054 7 talk
    matthew 15055 6 talk
    this-one 15056 7 talk
    matthew 15057 7 talk
    this-one 15058 7 talk
    matthew 15059 7 talk
    this-one 15060 1 talk
    matthew 15061 7 talk
    this-one 15062 7 talk
    15063 5 say-mode-wait
    matthew 15064 7 talk
    this-one 15065 7 talk
    15066 5 say-mode-wait
    this-one 15067 7 talk
    matthew 15068 1 talk
    this-one 15069 2 talk
    matthew 15070 7 talk
    this-one 15071 6 talk
    matthew 15072 7 talk
    this-one 15073 7 talk
    matthew 15074 4 talk
    end-script
    ;

\ $80010320
: script-15
    end-script
    ;

\ $80010324
: script-28
    matthew 15075 5 talk
    morgan 15076 0 talk
    matthew 15077 7 talk
    morgan 15078 0 talk
    end-script
    ;

\ $80010350
: script-29
    morgan 15079 0 talk
    end-script
    ;

\ $80010364
: script-30
    morgan 15080 0 talk
    matthew 15081 3 talk
    morgan 15082 0 talk
    end-script
    ;

\ $8001038C
: script-31
    morgan 15083 0 talk
    end-script
    ;

\ $800103A0
: script-32
    morgan 15084 0 talk
    morgan 15085 0 talk
    matthew 15086 7 talk
    end-script
    ;

\ $800103C8
: script-33
    morgan 15087 0 talk
    end-script
    ;

\ $800103DC
: script-24
    morgan 15088 0 talk
    matthew 15089 7 talk
    morgan 15090 0 talk
    end-script
    ;

\ $8001040C
: script-25
    morgan 15091 0 talk
    end-script
    ;

\ its scripts by number
' script-0 0 224 pack-script!
' script-1 1 224 pack-script!
' script-0 2 224 pack-script!
' script-1 3 224 pack-script!
' script-4 4 224 pack-script!
' script-5 5 224 pack-script!
' script-6 6 224 pack-script!
' script-7 7 224 pack-script!
' script-10 10 224 pack-script!
' script-11 11 224 pack-script!
' script-14 14 224 pack-script!
' script-15 15 224 pack-script!
' script-16 16 224 pack-script!
' script-17 17 224 pack-script!
' script-22 22 224 pack-script!
' script-23 23 224 pack-script!
' script-24 24 224 pack-script!
' script-25 25 224 pack-script!
' script-26 26 224 pack-script!
' script-27 27 224 pack-script!
' script-28 28 224 pack-script!
' script-29 29 224 pack-script!
' script-30 30 224 pack-script!
' script-31 31 224 pack-script!
' script-32 32 224 pack-script!
' script-33 33 224 pack-script!
' script-34 34 224 pack-script!
' script-35 35 224 pack-script!
' script-36 36 224 pack-script!
' script-37 37 224 pack-script!
