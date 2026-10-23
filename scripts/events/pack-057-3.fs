\ pack-057-3.fs - event pack 057_3 (CD.BIN 288): 81 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-057-3
USING: events ;

\ $8001014C
: script-0
    15000 5 say-mode-wait
    end-script
    ;

\ $8001015C
: script-1
    15001 5 say-mode-wait
    end-script
    ;

\ $8001016C
: script-13
    15002 phone-say
    15003 say-reset
    15004 phone-say
    15005 say-reset
    end-script
    ;

\ $80010190
: script-8
    15006 phone-say
    15007 phone-say
    end-script
    ;

\ $800101A4
: script-24
    15008 phone-say
    15009 phone-say
    15010 phone-say
    end-script
    ;

\ $800101C0
: script-6
    15011 phone-say
    15012 say-reset
    15013 phone-say
    15014 say-reset
    15015 phone-say
    15016 say-reset
    15017 phone-say
    15018 phone-say
    15019 phone-say
    15020 say-reset
    15021 phone-say
    15022 phone-say
    15023 say-reset
    end-script
    ;

\ $8001022C
: script-7
    15024 phone-say
    15025 phone-say
    15026 say-reset
    15027 phone-say
    15028 phone-say
    15029 phone-say
    15030 say-reset
    15031 phone-say
    end-script
    ;

\ $80010270
: script-9
    15032 phone-say
    15033 say-reset
    15034 phone-say
    15035 say-reset
    15036 phone-say
    15037 phone-say
    end-script
    ;

\ $800102A4
: script-11
    15038 phone-say
    15039 say-reset
    15040 phone-say
    15041 say-reset
    15042 phone-say
    15043 say-reset
    end-script
    ;

\ $800102D8
: script-19
    15044 phone-say
    15045 phone-say
    15046 phone-say
    15047 phone-say
    4 map-reveal
    end-script
    ;

\ $80010304
: script-23
    15048 phone-say
    15049 phone-say
    15050 phone-say
    end-script
    ;

\ $80010320
: script-25
    15051 phone-say
    15052 phone-say
    end-script
    ;

\ $80010334
: script-26
    15053 phone-say
    15054 phone-say
    end-script
    ;

\ $80010348
: script-33
    15055 phone-say
    15056 phone-say
    15057 phone-say
    end-script
    ;

\ $80010364
: script-34
    15058 phone-say
    15059 phone-say
    15060 phone-say
    end-script
    ;

\ $80010380
: script-35
    15061 phone-say
    15062 phone-say
    15063 phone-say
    15064 phone-say
    15065 phone-say
    end-script
    ;

\ $800103AC
: script-37
    15066 phone-say
    15067 phone-say
    15068 phone-say
    15069 phone-say
    15070 phone-say
    end-script
    ;

\ $800103D8
: script-38
    15071 phone-say
    15072 phone-say
    end-script
    ;

\ $800103EC
: script-45
    15073 phone-say
    15074 phone-say
    15075 phone-say
    15076 phone-say
    end-script
    ;

\ $80010410
: script-47
    15077 phone-say
    15078 phone-say
    15079 phone-say
    15080 phone-say
    end-script
    ;

\ $80010434
: script-49
    15081 phone-say
    15082 phone-say
    15083 phone-say
    15084 phone-say
    end-script
    ;

\ $80010458
: script-59
    15085 phone-say
    15086 phone-say
    15087 phone-say
    end-script
    ;

\ $80010474
: script-61
    15088 phone-say
    15089 phone-say
    15090 phone-say
    15091 phone-say
    end-script
    ;

\ $80010498
: script-62
    15092 phone-say
    15093 phone-say
    15094 phone-say
    end-script
    ;

\ $800104B4
: script-69
    15095 phone-say
    15096 phone-say
    15097 phone-say
    end-script
    ;

\ $800104D0
: script-71
    15098 phone-say
    15099 phone-say
    15100 phone-say
    end-script
    ;

\ $800104EC
: script-73
    15101 phone-say
    15102 phone-say
    15103 phone-say
    15104 phone-say
    15105 say-reset
    15106 phone-say
    end-script
    ;

\ $80010520
: script-75
    15107 phone-say
    15108 phone-say
    15109 phone-say
    end-script
    ;

\ its scripts by number
' script-0 0 288 pack-script!
' script-1 1 288 pack-script!
' script-0 2 288 pack-script!
' script-0 3 288 pack-script!
' script-0 4 288 pack-script!
' script-0 5 288 pack-script!
' script-6 6 288 pack-script!
' script-7 7 288 pack-script!
' script-8 8 288 pack-script!
' script-9 9 288 pack-script!
' script-8 10 288 pack-script!
' script-11 11 288 pack-script!
' script-0 12 288 pack-script!
' script-13 13 288 pack-script!
' script-13 14 288 pack-script!
' script-1 15 288 pack-script!
' script-1 16 288 pack-script!
' script-13 17 288 pack-script!
' script-13 18 288 pack-script!
' script-19 19 288 pack-script!
' script-8 20 288 pack-script!
' script-13 21 288 pack-script!
' script-13 22 288 pack-script!
' script-23 23 288 pack-script!
' script-24 24 288 pack-script!
' script-25 25 288 pack-script!
' script-26 26 288 pack-script!
' script-13 27 288 pack-script!
' script-13 28 288 pack-script!
' script-1 29 288 pack-script!
' script-1 30 288 pack-script!
' script-13 31 288 pack-script!
' script-13 32 288 pack-script!
' script-33 33 288 pack-script!
' script-34 34 288 pack-script!
' script-35 35 288 pack-script!
' script-24 36 288 pack-script!
' script-37 37 288 pack-script!
' script-38 38 288 pack-script!
' script-13 39 288 pack-script!
' script-13 40 288 pack-script!
' script-1 41 288 pack-script!
' script-1 42 288 pack-script!
' script-13 43 288 pack-script!
' script-13 44 288 pack-script!
' script-45 45 288 pack-script!
' script-8 46 288 pack-script!
' script-47 47 288 pack-script!
' script-24 48 288 pack-script!
' script-49 49 288 pack-script!
' script-38 50 288 pack-script!
' script-13 51 288 pack-script!
' script-13 52 288 pack-script!
' script-1 53 288 pack-script!
' script-1 54 288 pack-script!
' script-13 55 288 pack-script!
' script-13 56 288 pack-script!
' script-33 57 288 pack-script!
' script-8 58 288 pack-script!
' script-59 59 288 pack-script!
' script-24 60 288 pack-script!
' script-61 61 288 pack-script!
' script-62 62 288 pack-script!
' script-13 63 288 pack-script!
' script-13 64 288 pack-script!
' script-1 65 288 pack-script!
' script-1 66 288 pack-script!
' script-13 67 288 pack-script!
' script-13 68 288 pack-script!
' script-69 69 288 pack-script!
' script-8 70 288 pack-script!
' script-71 71 288 pack-script!
' script-0 72 288 pack-script!
' script-73 73 288 pack-script!
' script-8 74 288 pack-script!
' script-75 75 288 pack-script!
' script-0 76 288 pack-script!
' script-13 77 288 pack-script!
' script-13 78 288 pack-script!
' script-1 79 288 pack-script!
' script-1 80 288 pack-script!
