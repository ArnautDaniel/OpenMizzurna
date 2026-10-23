\ pack-046-3.fs - event pack 046_3 (CD.BIN 233): 13 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-046-3
USING: events ;

\ $8001003C
: script-0
    morgan as-this-one
    matthew 15000 6 talk
    this-one 15001 0 talk
    matthew 15002 7 talk
    this-one 15003 0 talk
    matthew 15004 5 talk
    this-one 15005 0 talk
    matthew 15006 7 talk
    this-one 15007 0 talk
    matthew 15008 5 talk
    this-one 15009 0 talk
    this-one 15010 0 talk
    end-script
    ;

\ $80010074
: script-1
    morgan as-this-one
    matthew 15011 7 talk
    this-one 15012 0 talk
    matthew 15013 5 talk
    this-one 15014 0 talk
    matthew 15015 7 talk
    this-one 15016 0 talk
    this-one 15017 0 talk
    this-one 15018 0 talk
    matthew 15019 3 talk
    this-one 15020 0 talk
    matthew 15021 4 talk
    end-script
    ;

\ $800100AC
: script-2
    morgan as-this-one
    matthew 15022 5 talk
    this-one 15023 0 talk
    matthew 15024 7 talk
    this-one 15025 0 talk
    matthew 15026 6 talk
    this-one 15027 0 talk
    this-one 15028 0 talk
    end-script
    ;

\ $800100D4
: script-3
    morgan as-this-one
    this-one 15029 0 talk
    matthew 15030 4 talk
    this-one 15031 0 talk
    matthew 15032 7 talk
    this-one 15033 0 talk
    matthew 15034 3 talk
    this-one 15035 0 talk
    end-script
    ;

\ $800100FC
: script-4
    morgan as-this-one
    matthew 15036 7 talk
    this-one 15037 0 talk
    matthew 15038 5 talk
    this-one 15039 0 talk
    matthew 15040 6 talk
    this-one 15041 0 talk
    matthew 15042 4 talk
    end-script
    ;

\ $80010124
: script-5
    morgan as-this-one
    this-one 15043 0 talk
    matthew 15044 3 talk
    this-one 15045 0 talk
    this-one 15046 0 talk
    matthew 15047 7 talk
    this-one 15048 0 talk
    matthew 15049 4 talk
    end-script
    ;

\ $8001014C
: script-6
    morgan as-this-one
    matthew 15050 5 talk
    this-one 15051 0 talk
    matthew 15052 2 talk
    this-one 15053 0 talk
    matthew 15054 7 talk
    this-one 15055 0 talk
    matthew 15056 5 talk
    this-one 15057 0 talk
    matthew 15058 6 talk
    15059 5 say-mode-wait
    end-script
    ;

\ $80010188
: script-7
    morgan as-this-one
    matthew 15060 7 talk
    this-one 15061 0 talk
    matthew 15062 5 talk
    this-one 15063 0 talk
    matthew 15064 6 talk
    this-one 15065 0 talk
    15066 5 say-mode-wait
    end-script
    ;

\ $800101B8
: script-8
    morgan as-this-one
    matthew 15067 7 talk
    this-one 15068 0 talk
    matthew 15069 1 talk
    this-one 15070 0 talk
    matthew 15071 4 talk
    end-script
    ;

\ $800101D8
: script-9
    morgan as-this-one
    matthew 15072 5 talk
    this-one 15073 0 talk
    matthew 15074 7 talk
    this-one 15075 0 talk
    matthew 15076 4 talk
    this-one 15077 0 talk
    this-one 15078 0 talk
    end-script
    ;

\ $80010200
: script-10
    morgan as-this-one
    matthew 15079 5 talk
    this-one 15080 0 talk
    matthew 15081 7 talk
    this-one 15082 0 talk
    end-script
    ;

\ $8001021C
: script-11
    matthew 15083 7 talk
    morgan 15084 3 talk
    matthew 15085 1 talk
    morgan 15086 3 talk
    matthew 15087 5 talk
    morgan 15088 3 talk
    morgan 15089 5 talk
    morgan 15090 6 talk
    end-script
    ;

\ $800102A0
: script-12
    matthew 15091 5 talk
    morgan 15092 7 talk
    matthew 15093 1 talk
    morgan 86 0 anim
    morgan wait-anim
    morgan 15094 7 talk
    matthew 15095 0 talk
    morgan 15096 2 talk
    morgan 85 0 anim
    morgan wait-anim
    morgan 15097 5 talk
    end-script
    ;

\ its scripts by number
' script-0 0 233 pack-script!
' script-1 1 233 pack-script!
' script-2 2 233 pack-script!
' script-3 3 233 pack-script!
' script-4 4 233 pack-script!
' script-5 5 233 pack-script!
' script-6 6 233 pack-script!
' script-7 7 233 pack-script!
' script-8 8 233 pack-script!
' script-9 9 233 pack-script!
' script-10 10 233 pack-script!
' script-11 11 233 pack-script!
' script-12 12 233 pack-script!
