\ pack-048-4.fs - event pack 048_4 (CD.BIN 244): 11 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-048-4
USING: events ;

\ $80010034
: script-0
    10 actor as-this-one
    this-one 15000 0 talk
    matthew 15001 7 talk
    this-one 15002 0 talk
    matthew 15003 5 talk
    this-one 15004 0 talk
    matthew 15005 7 talk
    this-one 15006 0 talk
    end-script
    ;

\ $8001005C
: script-1
    129 actor as-this-one
    this-one 15007 5 talk
    matthew 15008 6 talk
    this-one 15009 2 talk
    matthew 15010 1 talk
    this-one 15011 7 talk
    this-one 15012 6 talk
    matthew 15013 1 talk
    this-one 15014 7 talk
    matthew 15015 7 talk
    this-one 15016 5 talk
    this-one 15017 7 talk
    end-script
    ;

\ $80010094
: script-2
    171 actor as-this-one
    matthew 15018 7 talk
    this-one 15019 8 talk
    matthew 15020 7 talk
    this-one 15021 8 talk
    matthew 15022 7 talk
    this-one 15023 8 talk
    matthew 15024 7 talk
    this-one 15025 8 talk
    matthew 15026 7 talk
    this-one 15027 8 talk
    matthew 15028 7 talk
    this-one 15029 8 talk
    this-one 15030 8 talk
    matthew 15031 7 talk
    this-one 15032 8 talk
    matthew 15033 7 talk
    this-one 15034 8 talk
    matthew 15035 7 talk
    this-one 15036 8 talk
    matthew 15037 7 talk
    this-one 15038 8 talk
    15039 5 say-mode-wait
    end-script
    ;

\ $80010100
: script-3
    171 actor as-this-one
    this-one 15040 8 talk
    matthew 15041 7 talk
    end-script
    ;

\ $80010114
: script-4
    129 actor 15042 1 talk
    end-script
    ;

\ $80010128
: script-5
    152 actor as-this-one
    -564 -1842 -538 441212 -4120 223109 545 camera-set
    matthew 15043 7 talk
    this-one 15044 3 talk
    this-one 15045 7 talk
    matthew 15046 5 talk
    this-one 15047 6 talk
    this-one 15048 7 talk
    this-one 15049 3 talk
    end-script
    ;

\ $800101A8
: script-6
    152 actor as-this-one
    -564 -1842 -538 441212 -4120 223109 545 camera-set
    152 actor 98 0 face
    this-one 15050 5 talk
    15051 32767 actor say-by-wait
    152 actor 0 0 face
    this-one 15052 4 talk
    end-script
    ;

\ $80010218
: script-7
    152 actor as-this-one
    -564 -1842 -538 441212 -4120 223109 545 camera-set
    this-one 15053 7 talk
    matthew 15054 4 talk
    this-one 15055 3 talk
    this-one 15056 7 talk
    end-script
    ;

\ $80010274
: script-8
    152 actor as-this-one
    -564 -1842 -538 441212 -4120 223109 545 camera-set
    this-one 15057 7 talk
    152 actor 98 0 face
    this-one 15058 5 talk
    15059 32767 actor say-by-wait
    this-one 15060 1 talk
    15061 32767 actor say-by-wait
    152 actor 0 0 face
    this-one 15062 4 talk
    end-script
    ;

\ $80010308
: script-9
    152 actor as-this-one
    -564 -1842 -538 441212 -4120 223109 545 camera-set
    this-one 15063 7 talk
    this-one 15064 4 talk
    10 frames wait
    this-one 15065 5 talk
    matthew 15066 7 talk
    this-one 15067 7 talk
    matthew 15068 4 talk
    this-one 15069 5 talk
    this-one 15070 6 talk
    end-script
    ;

\ $8001039C
: script-10
    152 actor as-this-one
    -564 -1842 -538 441212 -4120 223109 545 camera-set
    this-one 15071 7 talk
    152 actor 98 0 face
    this-one 15072 6 talk
    15073 32767 actor say-by-wait
    this-one 15074 5 talk
    15075 32767 actor say-by-wait
    this-one 15076 1 talk
    15077 32767 actor say-by-wait
    10 frames wait
    152 actor 0 0 face
    this-one 15078 4 talk
    end-script
    ;

\ its scripts by number
' script-0 0 244 pack-script!
' script-1 1 244 pack-script!
' script-2 2 244 pack-script!
' script-3 3 244 pack-script!
' script-4 4 244 pack-script!
' script-5 5 244 pack-script!
' script-6 6 244 pack-script!
' script-7 7 244 pack-script!
' script-8 8 244 pack-script!
' script-9 9 244 pack-script!
' script-10 10 244 pack-script!
