\ pack-043-3.fs - event pack 043_3 (CD.BIN 218): 38 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-043-3
USING: events ;

\ $800100A0
: script-6
    matthew 15000 5 talk
    hudson 15001 0 talk
    matthew 15002 7 talk
    hudson 15003 0 talk
    end-script
    ;

\ $800100CC
: script-7
    hudson 15004 0 talk
    end-script
    ;

\ $800100E0
: script-4
    hudson as-this-one
    this-one 15005 0 talk
    matthew 15006 5 talk
    this-one 15007 0 talk
    this-one 15008 0 talk
    end-script
    ;

\ $800100FC
: script-5
    hudson 15009 0 talk
    end-script
    ;

\ $80010110
: script-0
    matthew 15010 5 talk
    hudson 15011 0 talk
    matthew 15012 7 talk
    15013 5 say-mode-wait
    end-script
    ;

\ $80010138
: script-1
    hudson 15014 0 talk
    15015 5 say-mode-wait
    end-script
    ;

\ $80010158
: script-22
    matthew 15016 7 talk
    hudson 15017 0 talk
    15018 5 say-mode-wait
    matthew 15019 3 talk
    hudson 15020 0 talk
    end-script
    ;

\ $80010190
: script-23
    hudson 15021 0 talk
    end-script
    ;

\ $800101A4
: script-10
    hudson as-this-one
    this-one 15022 0 talk
    matthew 15023 4 talk
    this-one 15024 0 talk
    this-one 15025 0 talk
    end-script
    ;

\ $800101C0
: script-11
    hudson as-this-one
    this-one 15026 0 talk
    end-script
    ;

\ $800101D0
: script-16
    hudson 15027 0 talk
    matthew 15028 7 talk
    hudson 15029 0 talk
    matthew 15030 4 talk
    end-script
    ;

\ $800101FC
: script-17
    hudson 15031 0 talk
    end-script
    ;

\ $80010210
: script-36
    matthew 15032 7 talk
    hudson 15033 0 talk
    matthew 15034 5 talk
    hudson 15035 0 talk
    end-script
    ;

\ $8001023C
: script-37
    hudson 15036 0 talk
    end-script
    ;

\ $800102A8
: script-26
    matthew 15042 7 talk
    hudson 15043 0 talk
    matthew 15044 6 talk
    end-script
    ;

\ $800102C4
: script-27
    hudson 15045 0 talk
    end-script
    ;

\ $800102D8
: script-14
    matthew 15046 5 talk
    hudson 15047 0 talk
    15048 5 say-mode-wait
    end-script
    ;

\ $800102FC
: script-15
    hudson 15049 0 talk
    end-script
    ;

\ $80010310
: script-28
    matthew 15050 7 talk
    hudson 15051 0 talk
    hudson 15052 0 talk
    end-script
    ;

\ $80010338
: script-29
    hudson 15053 0 talk
    end-script
    ;

\ $8001034C
: script-30
    hudson 15054 0 talk
    hudson 15055 0 talk
    matthew 15056 7 talk
    end-script
    ;

\ $80010374
: script-31
    hudson 15057 0 talk
    end-script
    ;

\ $80010388
: script-32
    matthew 15058 7 talk
    hudson 15059 0 talk
    hudson 15060 0 talk
    end-script
    ;

\ $800103B0
: script-33
    hudson 15061 0 talk
    end-script
    ;

\ $800103C4
: script-24
    hudson 15062 0 talk
    matthew 15063 7 talk
    hudson 15064 0 talk
    end-script
    ;

\ $800103F4
: script-25
    hudson 15065 0 talk
    end-script
    ;

\ its scripts by number
' script-0 0 218 pack-script!
' script-1 1 218 pack-script!
' script-0 2 218 pack-script!
' script-1 3 218 pack-script!
' script-4 4 218 pack-script!
' script-5 5 218 pack-script!
' script-6 6 218 pack-script!
' script-7 7 218 pack-script!
' script-10 10 218 pack-script!
' script-11 11 218 pack-script!
' script-14 14 218 pack-script!
' script-15 15 218 pack-script!
' script-16 16 218 pack-script!
' script-17 17 218 pack-script!
' script-22 22 218 pack-script!
' script-23 23 218 pack-script!
' script-24 24 218 pack-script!
' script-25 25 218 pack-script!
' script-26 26 218 pack-script!
' script-27 27 218 pack-script!
' script-28 28 218 pack-script!
' script-29 29 218 pack-script!
' script-30 30 218 pack-script!
' script-31 31 218 pack-script!
' script-32 32 218 pack-script!
' script-33 33 218 pack-script!
' script-36 36 218 pack-script!
' script-37 37 218 pack-script!
