\ pack-057-4.fs - event pack 057_4 (CD.BIN 289): 2 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-057-4
USING: events ;

\ $80010010
: script-0
    15000 phone-say
    15001 say-reset
    15002 phone-say
    15003 say-reset
    15004 phone-say
    15005 say-reset
    15006 phone-say
    15007 say-reset
    15008 phone-say
    15009 say-reset
    15010 phone-say
    15011 5 say-mode-wait
    15012 phone-say
    15013 say-reset
    15014 phone-say
    end-script
    ;

\ $80010090
: script-1
    15015 phone-say
    15016 say-reset
    15017 phone-say
    15018 5 say-mode-wait
    15019 phone-say
    15020 say-reset
    15021 phone-say
    15022 say-reset
    15023 5 say-mode-wait
    15024 5 say-mode-wait
    end-script
    ;

\ its scripts by number
' script-0 0 289 pack-script!
' script-1 1 289 pack-script!
