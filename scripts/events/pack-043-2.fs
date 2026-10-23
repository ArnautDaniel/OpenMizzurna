\ pack-043-2.fs - event pack 043_2 (CD.BIN 217): 38 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-043-2
USING: events ;

\ $800100A0
: script-6
    father-barton as-this-one
    this-one 15000 0 talk
    matthew 15001 7 talk
    this-one 15002 0 talk
    this-one 15003 0 talk
    end-script
    ;

\ $800100BC
: script-7
    father-barton 15004 0 talk
    end-script
    ;

\ $800100D0
: script-4
    father-barton as-this-one
    this-one 15005 0 talk
    this-one 15006 0 talk
    matthew 15007 7 talk
    this-one 15008 0 talk
    this-one 15009 0 talk
    end-script
    ;

\ $800100F0
: script-5
    father-barton 15010 0 talk
    end-script
    ;

\ $80010104
: script-0
    father-barton as-this-one
    this-one 15011 0 talk
    matthew 15012 7 talk
    this-one 15013 0 talk
    this-one 15014 0 talk
    matthew 15015 4 talk
    end-script
    ;

\ $80010124
: script-1
    father-barton 15016 0 talk
    end-script
    ;

\ $80010138
: script-22
    father-barton as-this-one
    this-one 15017 7 talk
    30 frames wait
    this-one 15018 0 talk
    matthew 15019 7 talk
    this-one 15020 4 talk
    matthew 15021 3 talk
    this-one 14 15 anim-at  this-one wait-anim
    this-one 15022 0 talk
    this-one 15023 0 talk
    this-one 16 0 anim-at  this-one wait-anim
    this-one 15024 0 talk
    this-one 15025 7 talk
    end-script
    ;

\ $80010190
: script-23
    father-barton 15026 0 talk
    end-script
    ;

\ $800101A4
: script-10
    father-barton as-this-one
    this-one 15027 0 talk
    matthew 15028 5 talk
    matthew 15029 7 talk
    this-one 15030 0 talk
    matthew 15031 7 talk
    this-one 15032 0 talk
    end-script
    ;

\ $800101C8
: script-11
    father-barton 15033 0 talk
    end-script
    ;

\ $800101DC
: script-16
    father-barton as-this-one
    this-one 15034 0 talk
    matthew 15035 6 talk
    this-one 15036 0 talk
    this-one 15037 0 talk
    end-script
    ;

\ $800101F8
: script-17
    father-barton 15038 0 talk
    end-script
    ;

\ $8001020C
: script-36
    father-barton 15039 0 talk
    matthew 15040 5 talk
    father-barton 15041 0 talk
    end-script
    ;

\ $80010234
: script-37
    father-barton 15042 0 talk
    end-script
    ;

\ $80010248
: script-34
    father-barton as-this-one
    this-one 15043 7 talk
    matthew 15044 4 talk
    this-one 15045 0 talk
    matthew 15046 7 talk
    this-one 15047 0 talk
    this-one 15048 7 talk
    this-one 15049 7 talk
    matthew 15050 7 talk
    this-one 15051 0 talk
    this-one 15052 4 talk
    this-one 15053 0 talk
    end-script
    ;

\ $800102D8
: script-35
    father-barton 15054 0 talk
    end-script
    ;

\ $800102EC
: script-26
    father-barton as-this-one
    this-one 15055 7 talk
    matthew 15056 7 talk
    this-one 15057 0 talk
    matthew 15058 7 talk
    this-one 15059 5 talk
    matthew 15060 7 talk
    this-one 15061 3 talk
    matthew 15062 7 talk
    this-one 15063 5 talk
    30 frames wait
    this-one 15064 0 talk
    end-script
    ;

\ $80010378
: script-27
    father-barton 15065 0 talk
    end-script
    ;

\ $8001038C
: script-14
    father-barton 15066 0 talk
    matthew 15067 7 talk
    father-barton 15068 0 talk
    end-script
    ;

\ $800103B4
: script-15
    father-barton 15069 0 talk
    end-script
    ;

\ $800103C8
: script-28
    father-barton 15070 0 talk
    matthew 15071 5 talk
    father-barton 15072 0 talk
    end-script
    ;

\ $800103F0
: script-29
    father-barton 15073 0 talk
    end-script
    ;

\ $80010404
: script-30
    father-barton 15074 0 talk
    matthew 15075 7 talk
    father-barton 15076 0 talk
    end-script
    ;

\ $8001042C
: script-31
    father-barton 15077 0 talk
    end-script
    ;

\ $80010440
: script-32
    matthew 15078 7 talk
    father-barton 15079 0 talk
    father-barton 15080 0 talk
    end-script
    ;

\ $80010468
: script-33
    father-barton 15081 0 talk
    end-script
    ;

\ $8001047C
: script-24
    father-barton 15082 0 talk
    matthew 15083 7 talk
    father-barton 15084 0 talk
    end-script
    ;

\ $800104AC
: script-25
    father-barton 15085 0 talk
    end-script
    ;

\ its scripts by number
' script-0 0 217 pack-script!
' script-1 1 217 pack-script!
' script-0 2 217 pack-script!
' script-1 3 217 pack-script!
' script-4 4 217 pack-script!
' script-5 5 217 pack-script!
' script-6 6 217 pack-script!
' script-7 7 217 pack-script!
' script-10 10 217 pack-script!
' script-11 11 217 pack-script!
' script-14 14 217 pack-script!
' script-15 15 217 pack-script!
' script-16 16 217 pack-script!
' script-17 17 217 pack-script!
' script-22 22 217 pack-script!
' script-23 23 217 pack-script!
' script-24 24 217 pack-script!
' script-25 25 217 pack-script!
' script-26 26 217 pack-script!
' script-27 27 217 pack-script!
' script-28 28 217 pack-script!
' script-29 29 217 pack-script!
' script-30 30 217 pack-script!
' script-31 31 217 pack-script!
' script-32 32 217 pack-script!
' script-33 33 217 pack-script!
' script-34 34 217 pack-script!
' script-35 35 217 pack-script!
' script-36 36 217 pack-script!
' script-37 37 217 pack-script!
