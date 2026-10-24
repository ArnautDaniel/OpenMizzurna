\ area007.fs - area 7's rules: the cemetery (src: ovl007.c; decomp/docs/areas/area07.md; port/src/game/ovl007.c):
\ the area's two timed scenes (on arriving, or as they happen), a visitor at a grave, walking out of the grounds,
\ who says what, Father Barton's talk here, the day and night lighting, the C its events call.
IN: rules.area007
USING: engine actors messages clock story-flags townsfolk events look talk areas ;

7 constant this-area
156 constant grave-visitor   110 constant person-110   83 constant person-83   84 constant person-84
$1D3BD constant at-the-grave-x   $4ADC9 constant at-the-grave-z
: today ( -- day )  clock-time time>day ;
: park ( slot -- )  0 0 0 set-pos ;
: stop-is? ( slot stop -- flag )  over stop-of 0< if  2drop false exit  then  swap person-of at-stop @ = ;
: bio-21 ( -- )  21 s" notebook" actor-named send learn-bio ;   \ (src: g_bio_known bit 21: Kathy's page)

variable lit-for-day   2 lit-for-day !   \ (2: not yet - neither day nor night)
\ ---- entering (0x80101F24) ----
: grave-visit-due? ( -- flag )  $01C story@ $C000 and $4000 =  lidds person-of at-stop @ $17 = and ;
: entered ( -- )
    4 map-reveal
    kathy-funeral-seen flag? 0=  26 9 30 10 18 between? and if
        kathy-funeral-seen flag-set  bio-21  ['] events.ovl007:entry-801037C4 start-event
    else  cemetery-mel-meeting-overheard flag? 0=  26 20 54 21 24 between? and if
        cemetery-mel-meeting-overheard flag-set  ['] events.ovl007:entry-801028AC start-event  then  then
    grave-visit-due? if
        grave-visitor at-the-grave-x 0 at-the-grave-z set-pos  grave-visitor 0 $42E negate 0 set-rot
        grave-visitor this-area 1 set-floor  grave-visitor $6A9 1 anim
    else  grave-visitor park  then
    person-110 park  2 lit-for-day ! ;
: scene-hook ( scene -- )  drop ;

\ ---- each frame (0x801020FC) ----
0 constant outside-the-grounds   \ (the zone's index)
: visitor-there? ( -- flag )  grave-visitor person-now drop 2drop at-the-grave-x = ;
: day-lights? ( -- flag )   \ src: func_80102700: 07:00 - 16:29 the day's lights (object 1), else the night's (2)
    clock-time minutes/day mod  7 60 *  16 60 * 29 + 1+  within ;
\ (lit-for-day: the lights set for day or night)
: lighting ( -- )   \ (when it changes)
    day-lights? dup lit-for-day @ = if  drop exit  then  dup lit-for-day !  {: day? :}
    1 day? s" areas" actor-named send object-shown  2 day? 0= s" areas" actor-named send object-shown ;
: each-frame ( -- )
    lighting
    visitor-there?  clock-time time>hour 20 = and if  grave-visitor park  then
    at-zone drop outside-the-grounds = if  ['] events.ovl007:entry-80102844 start-event exit  then
    kathy-funeral-seen flag? 0=  clock-time 26 9 30 date>time = and if
        kathy-funeral-seen flag-set  bio-21  ['] events.ovl007:entry-801027D8 start-event exit  then
    cemetery-mel-meeting-overheard flag? 0=  mel $F stop-is? and if
        cemetery-mel-meeting-overheard flag-set  ['] events.ovl007:entry-801028AC start-event  then ;

\ ---- talking here (0x80102264) ----
create person-84-lines  2 , 91 ,  10 , 91 ,  18 , 91 ,  26 , 91 ,  34 , 91 ,  42 , 91 ,  50 , 91 ,  -1 ,   \ src: D_8010286C
create person-83-lines  3 , 92 ,  12 , 92 ,  21 , 92 ,  30 , 92 ,  39 , 92 ,  48 , 92 ,  57 , 92 ,  -1 ,   \ src: D_8010288C
create barton-lines  6 , $51 ,  10 , $52 ,  14 , $53 ,  18 , $54 ,  22 , $55 ,  26 , $56 ,  -1 ,
7 constant usual-gesture   2 constant barton-gesture   3 constant mel-gesture
: line-if ( slot table gesture -- handled? )
    {: s t g :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s g one-liner  true ;
: one-at ( slot stop msg gesture -- handled? )
    {: s st m g :}  s st stop-is? 0= if  false exit  then  m s g one-liner  true ;
: grave-visitor-talks ( -- handled? )   \ once, then she's back at work (src: Lidds to the hospital)
    cemetery-ms-holden-talk-seen flag? if  true exit  then
    ['] events.ovl007:entry-80103A88 start-event  cemetery-ms-holden-talk-seen flag-set
    lidds $18153 $3C0 negate $5565B set-pos  lidds 29 2 set-floor  lidds 0 $409 0 set-rot  true ;
: talks-here ( slot -- handled? )
    case
        james of  james $42 $59 usual-gesture one-at  endof
        mel of  mel $F $5A mel-gesture one-at  endof
        father-barton of  father-barton barton-lines barton-gesture line-if  endof
        winona of  winona 9 $58 usual-gesture one-at  endof
        lidds of  lidds 6 $57 usual-gesture one-at  endof
        person-83 of  person-83 person-83-lines usual-gesture line-if  endof
        person-84 of  person-84 person-84-lines usual-gesture line-if  endof
        grave-visitor of  grave-visitor-talks  endof
        false swap
    endcase ;
' talks-here this-area area-talk!

\ ---- Father Barton's talk here (src: func_80102644, main's handler in area 7) ----
: bartons-talk ( -- handled? )
    area @ this-area <> if  false exit  then
    $104 story@ 2 and  $038 story@ 0= or  street-old-man-scene-seen flag? 0= or if  false exit  then
    $104 story@ 2 or $104 story!  ['] events.ovl007:entry-801038FC start-event  true ;
' bartons-talk father-barton conversation!

\ ---- the C its events call ----
40 constant mels-bike-slot
: mel-with-his-car ( -- )   \ 0x801026C8 (src: func_80029B20: his bike his partner again - in the town his motion
    mel mels-bike-slot s" townsfolk" actor-named send person-partner   \ by it, else none)
    the-town area @ = if  mel $1655 true anim  else  mel idle  then ;
: mel-off-his-car ( -- )  mel s" townsfolk" actor-named send person-unlink ;   \ 0x801026EC (src: partner = 0)
this-area ovl $801026C8 ' mel-with-his-car native:   this-area ovl $801026EC ' mel-off-his-car native:

this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
