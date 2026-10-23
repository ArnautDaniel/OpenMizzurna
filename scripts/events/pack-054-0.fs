\ pack-054-0.fs - event pack 054_0 (CD.BIN 270): 62 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-054-0
USING: events ;

\ $80010100
: script-0
    15000 5 say-mode-wait
    end-script
    ;

\ $80010110
: script-42
    15001 phone-say
    15002 phone-say
    15003 phone-say
    end-script
    ;

\ $8001012C
: script-28
    15004 phone-say
    15005 phone-say
    15006 phone-say
    end-script
    ;

\ $80010148
: script-30
    15007 phone-say
    15008 phone-say
    15009 say-reset
    15010 phone-say
    15011 phone-say
    end-script
    ;

\ $80010174
: script-5
    15012 phone-say
    15013 phone-say
    15014 say-reset
    15015 phone-say
    15016 phone-say
    15017 say-reset
    end-script
    ;

\ $800101A8
: script-6
    15018 phone-say
    15019 phone-say
    15020 phone-say
    15021 phone-say
    15022 say-reset
    end-script
    ;

\ $800101D4
: script-7
    15023 phone-say
    15024 phone-say
    15025 phone-say
    15026 phone-say
    15027 say-reset
    end-script
    ;

\ $80010200
: script-8
    15028 phone-say
    15029 phone-say
    15030 say-reset
    15031 phone-say
    end-script
    ;

\ $80010224
: script-9
    15032 phone-say
    15033 phone-say
    15034 phone-say
    15035 phone-say
    15036 say-reset
    15037 phone-say
    20 map-reveal
    end-script
    ;

\ $80010260
: script-10
    15038 phone-say
    15039 phone-say
    15040 phone-say
    end-script
    ;

\ $8001027C
: script-11
    15041 phone-say
    15042 phone-say
    15043 phone-say
    15044 phone-say
    20 map-reveal
    end-script
    ;

\ $800102A8
: script-12
    15045 phone-say
    15046 phone-say
    15047 say-reset
    end-script
    ;

\ $800102C4
: script-13
    15048 phone-say
    15049 phone-say
    15050 phone-say
    15051 say-reset
    15052 phone-say
    15053 say-reset
    end-script
    ;

\ $800102F8
: script-15
    15054 phone-say
    15055 phone-say
    15056 phone-say
    15057 phone-say
    15058 say-reset
    15059 phone-say
    0 map-reveal
    end-script
    ;

\ $80010334
: script-16
    15060 phone-say
    15061 phone-say
    15062 say-reset
    end-script
    ;

\ $80010350
: script-17
    15063 phone-say
    15064 phone-say
    15065 phone-say
    15066 phone-say
    end-script
    ;

\ $80010374
: script-18
    15067 phone-say
    15068 phone-say
    15069 say-reset
    15070 phone-say
    end-script
    ;

\ $80010398
: script-19
    15071 phone-say
    15072 phone-say
    15073 phone-say
    15074 phone-say
    end-script
    ;

\ $800103BC
: script-21
    15075 phone-say
    15076 phone-say
    15077 say-reset
    15078 phone-say
    15079 say-reset
    15080 phone-say
    15081 say-reset
    15082 phone-say
    end-script
    ;

\ $80010400
: script-23
    15083 phone-say
    15084 phone-say
    15085 say-reset
    15086 phone-say
    15087 say-reset
    15088 phone-say
    end-script
    ;

\ $80010434
: script-24
    15001 phone-say
    15002 phone-say
    15003 phone-say
    end-script
    ;

\ $80010450
: script-25
    15089 phone-say
    15090 phone-say
    15091 say-reset
    15092 phone-say
    15093 say-reset
    end-script
    ;

\ $8001047C
: script-27
    15094 phone-say
    15095 phone-say
    15096 phone-say
    15097 phone-say
    end-script
    ;

\ $800104A0
: script-29
    15098 phone-say
    15099 phone-say
    15100 phone-say
    15101 say-reset
    15102 phone-say
    end-script
    ;

\ $800104CC
: script-31
    15103 phone-say
    15104 phone-say
    15105 phone-say
    15106 say-reset
    end-script
    ;

\ $800104F0
: script-33
    15107 phone-say
    15108 phone-say
    15109 phone-say
    15110 phone-say
    end-script
    ;

\ $80010514
: script-34
    15111 phone-say
    15112 phone-say
    15113 phone-say
    15114 phone-say
    end-script
    ;

\ $80010538
: script-35
    15115 phone-say
    15116 phone-say
    15117 say-reset
    15118 phone-say
    15119 phone-say
    15120 phone-say
    end-script
    ;

\ $8001056C
: script-36
    15121 phone-say
    15122 phone-say
    15123 phone-say
    end-script
    ;

\ $80010588
: script-37
    15124 phone-say
    15125 phone-say
    15126 say-reset
    15127 phone-say
    15128 say-reset
    15129 phone-say
    end-script
    ;

\ $800105BC
: script-39
    15130 phone-say
    15131 phone-say
    15132 say-reset
    15133 phone-say
    15134 say-reset
    15135 phone-say
    end-script
    ;

\ $800105F0
: script-41
    15136 phone-say
    15137 phone-say
    15138 phone-say
    end-script
    ;

\ $8001060C
: script-59
    15139 phone-say
    15140 phone-say
    15141 phone-say
    end-script
    ;

\ $80010628
: script-60
    15142 phone-say
    15143 phone-say
    15144 phone-say
    end-script
    ;

\ $80010644
: script-61
    15145 phone-say
    15146 phone-say
    end-script
    ;

\ its scripts by number
' script-0 0 270 pack-script!
' script-0 1 270 pack-script!
' script-0 2 270 pack-script!
' script-0 3 270 pack-script!
' script-0 4 270 pack-script!
' script-5 5 270 pack-script!
' script-6 6 270 pack-script!
' script-7 7 270 pack-script!
' script-8 8 270 pack-script!
' script-9 9 270 pack-script!
' script-10 10 270 pack-script!
' script-11 11 270 pack-script!
' script-12 12 270 pack-script!
' script-13 13 270 pack-script!
' script-12 14 270 pack-script!
' script-15 15 270 pack-script!
' script-16 16 270 pack-script!
' script-17 17 270 pack-script!
' script-18 18 270 pack-script!
' script-19 19 270 pack-script!
' script-0 20 270 pack-script!
' script-21 21 270 pack-script!
' script-0 22 270 pack-script!
' script-23 23 270 pack-script!
' script-24 24 270 pack-script!
' script-25 25 270 pack-script!
' script-0 26 270 pack-script!
' script-27 27 270 pack-script!
' script-28 28 270 pack-script!
' script-29 29 270 pack-script!
' script-30 30 270 pack-script!
' script-31 31 270 pack-script!
' script-28 32 270 pack-script!
' script-33 33 270 pack-script!
' script-34 34 270 pack-script!
' script-35 35 270 pack-script!
' script-36 36 270 pack-script!
' script-37 37 270 pack-script!
' script-28 38 270 pack-script!
' script-39 39 270 pack-script!
' script-30 40 270 pack-script!
' script-41 41 270 pack-script!
' script-42 42 270 pack-script!
' script-0 43 270 pack-script!
' script-0 44 270 pack-script!
' script-0 45 270 pack-script!
' script-0 46 270 pack-script!
' script-0 47 270 pack-script!
' script-0 48 270 pack-script!
' script-0 49 270 pack-script!
' script-0 50 270 pack-script!
' script-0 51 270 pack-script!
' script-0 52 270 pack-script!
' script-0 53 270 pack-script!
' script-0 54 270 pack-script!
' script-0 55 270 pack-script!
' script-0 56 270 pack-script!
' script-0 57 270 pack-script!
' script-0 58 270 pack-script!
' script-59 59 270 pack-script!
' script-60 60 270 pack-script!
' script-61 61 270 pack-script!
