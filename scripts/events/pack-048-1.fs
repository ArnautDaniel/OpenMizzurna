\ pack-048-1.fs - event pack 048_1 (CD.BIN 241): 2 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-048-1
USING: events ;

\ $80010010
: script-0
    robert as-this-one
    this-one 15000 0 talk
    matthew 15001 7 talk
    this-one 15002 0 talk
    this-one 15003 0 talk
    matthew 15004 4 talk
    end-script
    ;

\ $80010030
: script-1
    matthew 15005 7 talk
    robert 15006 0 talk
    matthew 15007 3 talk
    robert 15008 0 talk
    end-script
    ;

\ its scripts by number
' script-0 0 241 pack-script!
' script-1 1 241 pack-script!
