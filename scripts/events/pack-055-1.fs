\ pack-055-1.fs - event pack 055_1 (CD.BIN 276): 9 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-055-1
USING: events ;

\ $8001002C
: script-0
    matthew wait-anim
    15000 phone-say
    15001 say-reset
    15002 phone-say
    15003 say-reset
    15004 phone-say
    15005 say-reset
    15006 phone-say
    8 map-reveal
    end-script
    ;

\ $80010074
: script-1
    matthew wait-anim
    15007 phone-say
    15008 say-reset
    15009 phone-say
    15010 say-reset
    15011 phone-say
    15012 say-reset
    end-script
    ;

\ $800100AC
: script-2
    matthew wait-anim
    15013 phone-say
    15014 say-reset
    15015 phone-say
    15016 say-reset
    15017 phone-say
    15018 say-reset
    4 map-reveal
    end-script
    ;

\ $800100EC
: script-3
    matthew wait-anim
    15019 say-reset
    15020 phone-say
    15021 say-reset
    15022 phone-say
    15023 say-reset
    12 map-reveal
    end-script
    ;

\ $80010124
: script-4
    matthew wait-anim
    15024 phone-say
    15025 say-reset
    15026 phone-say
    15027 say-reset
    15028 phone-say
    end-script
    ;

\ $80010154
: script-5
    matthew wait-anim
    15037 say-reset
    15038 phone-say
    15039 say-reset
    15040 phone-say
    15041 say-reset
    15042 phone-say
    15043 say-reset
    end-script
    ;

\ $80010194
: script-6
    matthew wait-anim
    15044 say-reset
    15045 phone-say
    15046 say-reset
    15047 phone-say
    15048 say-reset
    15049 phone-say
    15050 say-reset
    17 map-reveal
    end-script
    ;

\ $800101DC
: script-7
    matthew wait-anim
    15051 say-reset
    15052 phone-say
    15053 say-reset
    15054 phone-say
    end-script
    ;

\ $80010204
: script-8
    matthew wait-anim
    15055 say-reset
    15056 phone-say
    15057 say-reset
    15058 phone-say
    15059 say-reset
    15060 phone-say
    15061 say-reset
    15062 say-reset
    end-script
    ;

\ its scripts by number
' script-0 0 276 pack-script!
' script-1 1 276 pack-script!
' script-2 2 276 pack-script!
' script-3 3 276 pack-script!
' script-4 4 276 pack-script!
' script-5 5 276 pack-script!
' script-6 6 276 pack-script!
' script-7 7 276 pack-script!
' script-8 8 276 pack-script!
