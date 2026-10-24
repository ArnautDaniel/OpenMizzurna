\ area023.fs - area 23's rules: Cohen's house (src: ovl023.c; decomp/docs/areas/area23.md; port/src/game/ovl023.c):
\ the locked door upstairs - looked at, then something to force it with found, then forced (the third try) - the
\ room behind it (a scene, once), the envelope (□, once), the front door by the story, who says what.
IN: rules.area023
USING: engine actors messages clock story-flags items townsfolk events look talk areas ;

23 constant this-area
166 constant the-envelope   $13 constant the-envelope-item
$85 constant the-locked-door   $89 constant the-envelope-check   9 constant the-search-zone
3 constant tries-to-force   3 constant the-room
$9183 constant envelope-x   $5F0 negate constant envelope-y   $43231 constant envelope-z   $1EA constant envelope-heading

\ ---- this visit (src: D_8010316C the door looked at, D_80103164 the crowbar found, D_80103168 the door forced,
\ D_80103170 the tries) ----
variable door-looked-at   variable crowbar-found   variable door-forced   variable tries
: entered ( -- )   \ src: func_80101ED0
    0 map-reveal
    found-cohen-envelope flag? if  the-envelope 0 0 0 set-pos
    else  the-envelope this-area 2 set-floor  the-envelope envelope-x envelope-y envelope-z set-pos
        the-envelope 0 envelope-heading 0 set-rot  then
    0 door-looked-at !  0 crowbar-found !  0 door-forced !  0 tries ! ;
: scene-hook ( scene -- )   \ src: func_801022A0: his room, once
    the-room =  cohen-found-scene-seen flag? 0= and if
        cohen-found-scene-seen flag-set  ['] events.ovl023:entry-801024F0 start-event  then ;

\ ---- ○ (0x80101F88 / 0x80102068): the door; the search for something to force it with ----
enum: look-action  nothing-here  2 search  4 the-door ;
: door-open? ( -- flag )  cohen-found-scene-seen flag?  door-forced @ 0<> or ;
: look-check ( zone check -- action )
    {: z c :}
    c the-locked-door = if  door-open?  door-looked-at @ 0= or if  the-door  else  nothing-here  then  exit  then
    z the-search-zone =  crowbar-found @ 0= and  cohen-found-scene-seen flag? 0= and  door-looked-at @ 0<> and if
        search exit  then
    nothing-here ;
: look-do ( action -- )
    case
        search of  ['] events.ovl023:entry-801027C0 start-event  1 crowbar-found !  endof
        the-door of
            door-open? if  ['] events.ovl023:entry-80102D40  else  1 door-looked-at !  ['] events.ovl023:entry-80102824  then
            start-event  endof
        drop run-rows 0
    endcase ;

\ ---- □ (0x80102120 / 0x801021C8): the envelope; forcing the door ----
enum: act-action  nothing-to-do  take-the-envelope  3 force-the-door ;
: act-check ( zone check -- action )
    nip case
        the-locked-door of
            cohen-found-scene-seen flag? 0=  crowbar-found @ 0<> and  door-forced @ 0= and if  force-the-door  else  nothing-to-do  then  endof
        the-envelope-check of  found-cohen-envelope flag? if  nothing-to-do  else  take-the-envelope  then  endof
        nothing-to-do swap
    endcase ;
: act-do ( action -- )
    case
        take-the-envelope of
            found-cohen-envelope flag-set  ['] events.ovl023:entry-8010271C start-event
            the-envelope-item s" items" actor-named send give-item  endof
        force-the-door of
            1 tries +!  tries @ tries-to-force < if  ['] events.ovl023:entry-80102880 start-event
            else  ['] events.ovl023:entry-801028DC start-event  1 door-forced !  then  endof
        drop run-rows 0
    endcase ;

\ ---- talking here (0x8010234C): Morgan at his stop here ----
13 constant morgans-visit   $24 constant morgans-line   7 constant usual-gesture
: talks-here ( slot -- handled? )
    morgan <> if  false exit  then
    morgan morgans-visit standing-at? 0= if  false exit  then
    morgans-line morgan usual-gesture one-liner  true ;
' talks-here this-area area-talk!

\ ---- the C its rows call ----
: the-front-door ( -- )   \ 0x80102300: found open and the room not yet seen, not out; else out to the town
    $01C story@ $180000 and $80000 = if  ['] events.ovl023:entry-80102980  else  ['] events.ovl023:entry-80102C48  then
    start-event ;
this-area ovl $80102300 ' the-front-door native:

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
\ each frame (src: func_801023D0, before drawing): in scene 0 a five-frame texture animation (0); elsewhere it stays
\ where it was
0 constant strip-anim   5 constant strip-frames   0 constant strip-scene
variable strip-frame
: strip-drawn ( -- )
    scene @ strip-scene <> if  exit  then
    strip-anim strip-frame @ texture-frame  strip-frame @ 1+ strip-frames mod strip-frame ! ;
this-area ' strip-drawn area-every-frame!
this-area ' entered ' scene-hook area-hooks!
