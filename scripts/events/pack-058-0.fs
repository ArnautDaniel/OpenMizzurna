\ pack-058-0.fs - event pack 058_0 (CD.BIN 290): 3 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-058-0
USING: events ;

\ $80010014
: script-0
    15000 5 say-mode-wait
    end-script
    ;

\ $80010024
: script-1
    15001 phone-say
    15002 phone-say
    15003 phone-say
    15004 phone-say
    15005 phone-say
    15006 phone-say
    end-script
    ;

\ $80010058
: script-2
    15007 phone-say
    15008 phone-say
    15009 phone-say
    15010 phone-say
    15011 phone-say
    15012 phone-say
    15013 phone-say
    15014 phone-say
    15015 phone-say
    end-script
    ;

\ its scripts by number
' script-0 0 290 pack-script!
' script-1 1 290 pack-script!
' script-2 2 290 pack-script!
