\ pack-047-1.fs - event pack 047_1 (CD.BIN 236): 3 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-047-1
USING: events ;

\ $80010014
: script-0
    nozz 15000 5 talk
    nozz 15001 3 talk
    matthew 15002 7 talk
    nozz 15003 6 talk
    nozz 15004 5 talk
    matthew 15005 7 talk
    nozz 15006 5 talk
    matthew 15007 1 talk
    nozz 15008 3 talk
    end-script
    ;

\ $800100A8
: script-1
    matthew 15009 5 talk
    nozz 15010 3 talk
    end-script
    ;

\ $800100CC
: script-2
    nozz 15011 5 talk
    matthew 15012 3 talk
    matthew 90 0 anim
    matthew wait-anim
    nozz 15013 5 talk
    nozz 15014 1 talk
    nozz 83 84 0 anim-then
    nozz wait-anim
    20 frames wait
    nozz 85 0 anim
    nozz wait-anim
    nozz 15015 5 talk
    nozz 15016 4 talk
    matthew 15017 7 talk
    nozz 15018 6 talk
    nozz 15019 5 talk
    end-script
    ;

\ its scripts by number
' script-0 0 236 pack-script!
' script-1 1 236 pack-script!
' script-2 2 236 pack-script!
