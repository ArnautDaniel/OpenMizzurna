\ townsfolk.fs - the people of the town and their days (docs/subsystems.md: Townsfolk - to come). For now
\ only the facts other subsystems ask, answering as if nobody were about.
IN: townsfolk
USING: engine ;

: stop-of ( slot -- stop | -1 )  drop -1 ;   \ the schedule stop they stand at (-1: travelling, or not built)
: nozz-behind-counter? ( -- flag )  false ;  \ src: func_80026C50 (Nozz at a stop placed in his shop)
: james-visiting? ( -- flag )  false ;       \ src: func_8002CBE8 (Dr. James by Matthew's bed)
