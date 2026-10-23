\ pack-057-1.fs - event pack 057_1 (CD.BIN 286): 33 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-057-1
USING: events ;

\ $8001008C
: script-0
    15000 5 say-mode-wait
    end-script
    ;

\ $8001009C
: script-1
    15001 5 say-mode-wait
    end-script
    ;

\ $800100AC
: script-6
    15002 phone-say
    15003 phone-say
    15004 phone-say
    15005 say-reset
    end-script
    ;

\ $800100D0
: script-24
    15006 phone-say
    15007 phone-say
    15008 phone-say
    end-script
    ;

\ $800100EC
: script-5
    15012 phone-say
    15013 phone-say
    15014 phone-say
    15015 say-reset
    15016 phone-say
    end-script
    ;

\ $80010118
: script-9
    15017 phone-say
    15018 phone-say
    15019 say-reset
    15020 phone-say
    15021 say-reset
    15022 phone-say
    15023 say-reset
    15024 say-reset
    15025 phone-say
    end-script
    ;

\ $80010164
: script-10
    15026 phone-say
    15027 phone-say
    15028 phone-say
    15029 phone-say
    15030 say-reset
    end-script
    ;

\ $80010190
: script-11
    15031 phone-say
    15032 phone-say
    15033 phone-say
    15034 phone-say
    15035 say-reset
    15036 phone-say
    15037 say-reset
    15038 phone-say
    15039 say-reset
    end-script
    ;

\ $800101DC
: script-13
    15040 phone-say
    15041 phone-say
    15042 say-reset
    15043 phone-say
    15044 say-reset
    15045 phone-say
    15046 say-reset
    end-script
    ;

\ $80010218
: script-15
    15047 phone-say
    15048 phone-say
    15049 phone-say
    15050 phone-say
    15051 say-reset
    end-script
    ;

\ $80010244
: script-17
    15052 phone-say
    15053 phone-say
    15054 phone-say
    15055 say-reset
    15056 phone-say
    end-script
    ;

\ $80010270
: script-21
    15057 phone-say
    15058 phone-say
    15059 phone-say
    15060 phone-say
    end-script
    ;

\ $80010294
: script-23
    15061 phone-say
    15062 phone-say
    15063 phone-say
    end-script
    ;

\ $800102B0
: script-25
    15064 phone-say
    15065 phone-say
    15066 phone-say
    15067 phone-say
    15068 phone-say
    end-script
    ;

\ $800102DC
: script-27
    15069 phone-say
    15070 phone-say
    15071 phone-say
    end-script
    ;

\ its scripts by number
' script-0 0 286 pack-script!
' script-1 1 286 pack-script!
' script-0 2 286 pack-script!
' script-0 3 286 pack-script!
' script-0 4 286 pack-script!
' script-5 5 286 pack-script!
' script-6 6 286 pack-script!
' script-5 7 286 pack-script!
' script-6 8 286 pack-script!
' script-9 9 286 pack-script!
' script-10 10 286 pack-script!
' script-11 11 286 pack-script!
' script-0 12 286 pack-script!
' script-13 13 286 pack-script!
' script-0 14 286 pack-script!
' script-15 15 286 pack-script!
' script-6 16 286 pack-script!
' script-17 17 286 pack-script!
' script-1 18 286 pack-script!
' script-1 19 286 pack-script!
' script-1 20 286 pack-script!
' script-21 21 286 pack-script!
' script-6 22 286 pack-script!
' script-23 23 286 pack-script!
' script-24 24 286 pack-script!
' script-25 25 286 pack-script!
' script-6 26 286 pack-script!
' script-27 27 286 pack-script!
' script-24 28 286 pack-script!
' script-17 29 286 pack-script!
' script-1 30 286 pack-script!
' script-1 31 286 pack-script!
' script-1 32 286 pack-script!
