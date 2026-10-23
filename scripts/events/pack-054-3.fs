\ pack-054-3.fs - event pack 054_3 (CD.BIN 273): 47 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-054-3
USING: events ;

\ $800100C4
: script-0
    15000 5 say-mode-wait
    end-script
    ;

\ $800100D4
: script-1
    15001 5 say-mode-wait
    end-script
    ;

\ $800100E4
: script-22
    15002 phone-say
    15003 phone-say
    20 frames wait
    15005 phone-say
    end-script
    ;

\ $80010108
: script-24
    15006 phone-say
    15007 phone-say
    15008 phone-say
    end-script
    ;

\ $80010124
: script-5
    15009 phone-say
    15010 phone-say
    15011 say-reset
    15012 phone-say
    15013 say-reset
    15014 phone-say
    15015 say-reset
    end-script
    ;

\ $80010160
: script-6
    15016 phone-say
    15017 phone-say
    15018 say-reset
    end-script
    ;

\ $8001017C
: script-7
    15019 phone-say
    15020 phone-say
    15021 say-reset
    15022 phone-say
    15023 phone-say
    15024 say-reset
    15025 phone-say
    end-script
    ;

\ $800101B8
: script-8
    15026 phone-say
    15027 phone-say
    15028 say-reset
    15029 phone-say
    end-script
    ;

\ $800101DC
: script-11
    15030 phone-say
    15031 phone-say
    15032 say-reset
    15033 phone-say
    15034 say-reset
    end-script
    ;

\ $80010208
: script-12
    15035 phone-say
    15036 phone-say
    15037 phone-say
    end-script
    ;

\ $80010224
: script-13
    15038 phone-say
    15039 phone-say
    15040 say-reset
    15041 phone-say
    15042 phone-say
    15043 say-reset
    15044 phone-say
    15045 say-reset
    15046 phone-say
    15047 say-reset
    15048 phone-say
    end-script
    ;

\ $80010280
: script-14
    15049 phone-say
    15050 phone-say
    15051 say-reset
    15052 phone-say
    end-script
    ;

\ $800102A4
: script-17
    15053 phone-say
    15054 phone-say
    15055 say-reset
    15056 phone-say
    15057 phone-say
    end-script
    ;

\ $800102D0
: script-21
    15058 phone-say
    15059 phone-say
    15060 phone-say
    15061 phone-say
    end-script
    ;

\ $800102F4
: script-23
    15062 phone-say
    15063 phone-say
    15064 phone-say
    15065 phone-say
    end-script
    ;

\ $80010318
: script-25
    15066 phone-say
    15067 phone-say
    15068 phone-say
    15069 phone-say
    end-script
    ;

\ $8001033C
: script-27
    15070 phone-say
    15071 phone-say
    15072 phone-say
    end-script
    ;

\ $80010358
: script-29
    15073 phone-say
    15074 phone-say
    15075 phone-say
    15076 phone-say
    end-script
    ;

\ $8001037C
: script-35
    15077 phone-say
    15078 phone-say
    15079 phone-say
    15080 phone-say
    end-script
    ;

\ $800103A0
: script-36
    15081 phone-say
    15082 phone-say
    15083 phone-say
    end-script
    ;

\ $800103BC
: script-37
    15084 phone-say
    15085 phone-say
    15086 phone-say
    15087 phone-say
    12 map-reveal
    end-script
    ;

\ $800103E8
: script-38
    15088 phone-say
    15089 phone-say
    15090 phone-say
    end-script
    ;

\ $80010404
: script-39
    15091 phone-say
    15092 phone-say
    15093 phone-say
    15094 phone-say
    15095 phone-say
    end-script
    ;

\ $80010430
: script-40
    15096 phone-say
    15097 phone-say
    15098 phone-say
    end-script
    ;

\ $8001044C
: script-41
    15099 phone-say
    15100 phone-say
    15101 phone-say
    15102 phone-say
    15103 say-reset
    15104 phone-say
    end-script
    ;

\ $80010480
: script-42
    15105 phone-say
    15106 phone-say
    15107 phone-say
    15108 phone-say
    15109 phone-say
    8 map-reveal
    end-script
    ;

\ $800104B4
: script-43
    15110 phone-say
    15111 phone-say
    15112 phone-say
    15113 phone-say
    end-script
    ;

\ $800104D8
: script-45
    15114 phone-say
    15115 phone-say
    15116 phone-say
    15117 phone-say
    15118 phone-say
    15119 phone-say
    15120 say-reset
    end-script
    ;

\ its scripts by number
' script-0 0 273 pack-script!
' script-1 1 273 pack-script!
' script-0 2 273 pack-script!
' script-0 3 273 pack-script!
' script-0 4 273 pack-script!
' script-5 5 273 pack-script!
' script-6 6 273 pack-script!
' script-7 7 273 pack-script!
' script-8 8 273 pack-script!
' script-0 9 273 pack-script!
' script-0 10 273 pack-script!
' script-11 11 273 pack-script!
' script-12 12 273 pack-script!
' script-13 13 273 pack-script!
' script-14 14 273 pack-script!
' script-0 15 273 pack-script!
' script-0 16 273 pack-script!
' script-17 17 273 pack-script!
' script-1 18 273 pack-script!
' script-1 19 273 pack-script!
' script-1 20 273 pack-script!
' script-21 21 273 pack-script!
' script-22 22 273 pack-script!
' script-23 23 273 pack-script!
' script-24 24 273 pack-script!
' script-25 25 273 pack-script!
' script-24 26 273 pack-script!
' script-27 27 273 pack-script!
' script-24 28 273 pack-script!
' script-29 29 273 pack-script!
' script-0 30 273 pack-script!
' script-0 31 273 pack-script!
' script-0 32 273 pack-script!
' script-1 33 273 pack-script!
' script-1 34 273 pack-script!
' script-35 35 273 pack-script!
' script-36 36 273 pack-script!
' script-37 37 273 pack-script!
' script-38 38 273 pack-script!
' script-39 39 273 pack-script!
' script-40 40 273 pack-script!
' script-41 41 273 pack-script!
' script-42 42 273 pack-script!
' script-43 43 273 pack-script!
' script-22 44 273 pack-script!
' script-45 45 273 pack-script!
' script-1 46 273 pack-script!
