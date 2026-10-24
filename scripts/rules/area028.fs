\ area028.fs - area 28's rules: the hideout cabin (src: ovl028.c; decomp/docs/areas/area28.md;
\ port/src/game/ovl028.c): the closed door □ (a first knock, then the later ones), the C its scene calls. Its
\ look rows are the table's; the scene's fight is the events' op (fight.fs).
IN: rules.area028
USING: engine actors messages story-flags events look talk areas ;

28 constant this-area

variable knocked   \ src: D_80102D2C: the door knocked at this visit
: entered ( -- )  0 knocked ! ;
: the-closed-door ( -- )   \ 0x80102080
    knocked @ if  ['] events.ovl028:entry-80102C78  else  1 knocked !  ['] events.ovl028:entry-80102C0C  then  start-event ;
: caught ( -- )  hideout-robbers-caught flag-set ;   \ 0x80102064
this-area ovl $80102080 ' the-closed-door native:   this-area ovl $80102064 ' caught native:

this-area ' entered 0 area-hooks!
