\ area003.fs - area 3's rules: the Roland residence, Emma's home (src: ovl003.c; decomp/docs/areas/area03.md;
\ port/src/game/ovl003.c reads it): heading home at night, the break-in scene, the stairs (only with Mrs. Roland
\ along), the vase and Emma's key, the drain and the ring, Emma's desk and the missing diary, the piano and the
\ floppy; who says what; Steve's and Sarah's talks here; the C its events call.
IN: rules.area003
USING: engine actors messages clock story-flags items townsfolk floors events look talk areas matthew ;

3 constant this-area
17 constant sarah   19 constant the-prop-19
5 constant emmas-key   23 constant engraved-ring
: today ( -- day )  clock-time time>day ;
: give ( item -- )  s" items" actor-named send give-item ;
: break-in-known? ( -- flag )   \ (src: +0x14: learned another way; +0x54: the call answered)
    $014 story@ 0<>  roland-break-in-scene-seen flag? or  $054 story@ 0<> or ;
$020 25 story-bit constant locker-key-known   \ (the vase's key known as a locker's: set elsewhere)
: sarah-along? ( -- flag )  sarah going-along? ;                     \ (src: actor 17's ACTOR_F_RIDING)
: has-companion? ( -- flag )  following? ;                           \ (src: D_800A37B4 with D_800A37B0)

\ ---- what he's looked at this visit (src: D_80105B60 .. B6C) ----
variable heading-home-said   variable drain-looked   variable desk-looked   variable vase-looked

\ ---- ○ (0x80102010 / 0x8010213C) ----
enum: look-action  nothing-here  the-stairs  the-vase  3 unused-3  4 unused-4  the-drain  6 unused-6  emmas-desk
  8 unused-8  the-piano ;
0 constant stairs-zone   2 constant piano-row
: look-check ( zone check -- action )
    case
        $00 of  drop the-stairs  endof
        $85 of  drop the-vase  endof
        $88 of  drop  got-engraved-ring flag? if  nothing-here  else  the-drain  then  endof
        $87 of  drop  emma-diary-found-missing flag? 0=  break-in-known? and if  emmas-desk  else  nothing-here  then  endof
        drop  piano-row =  floor @ 1 = and if  the-piano  else  nothing-here  then  0   \ (0: for endcase)
    endcase ;
: look-do ( action -- )
    case
        the-stairs of   \ up with Mrs. Roland; alone, he shouldn't wander the house
            sarah-along? if  ['] events.ovl003:entry-80104E8C  else  ['] events.ovl003:entry-8010583C  then  start-event  endof
        the-vase of
            got-emmas-key flag? 0= if
                vase-looked @ if  ['] events.ovl003:entry-801054D8
                else  1 vase-looked !  ['] events.ovl003:entry-80105484  then
            else  locker-key-known flag? if  ['] events.ovl003:entry-80102EC0  else  ['] events.ovl003:entry-80102EF0  then
            then  start-event  endof
        the-drain of  ['] events.ovl003:entry-8010584C start-event  1 drain-looked !  endof
        emmas-desk of  ['] events.ovl003:entry-801053C4 start-event  1 desk-looked !  endof
        the-piano of  ['] events.ovl003:entry-80105A60 start-event  endof
        drop run-rows 0
    endcase ;

\ ---- □ (0x801022B8 / 0x801023FC) ----
enum: act-action  nothing-to-do  3 take-the-key  play-the-floppy  5 unused-5  take-the-ring  7 unused-7  open-the-desk ;
: act-check ( zone check -- action )
    case
        $85 of  drop  got-emmas-key flag? 0=  vase-looked @ 0<> and if  take-the-key  else  nothing-to-do  then  endof
        $88 of  drop  got-engraved-ring flag? 0=  drain-looked @ 0<> and if  take-the-ring  else  nothing-to-do  then  endof
        $87 of  drop  desk-looked @ if  open-the-desk  else  nothing-to-do  then  endof
        drop  piano-row =  floor @ 1 = and  played-floppy-on-piano flag? 0= and  showed-floppy-to-mrs-roland flag? and
        if  play-the-floppy  else  nothing-to-do  then  0   \ (0: for endcase)
    endcase ;
: act-do ( action -- )
    case
        take-the-key of  ['] events.ovl003:entry-80105710 start-event  endof   \ (the key: evfn_80102A80)
        play-the-floppy of   \ she stops following to sit by it
            has-companion? if  s" townsfolk" actor-named send companion-halt  then
            ['] events.ovl003:entry-80104530 start-event  played-floppy-on-piano flag-set  endof
        take-the-ring of
            got-engraved-ring flag-toggle  ['] events.ovl003:entry-8010585C start-event  engraved-ring give  endof
        open-the-desk of   \ empty: the diary's gone
            0 desk-looked !  emma-diary-found-missing flag-toggle  ['] events.ovl003:entry-8010540C start-event  endof
        drop run-rows 0
    endcase ;

\ ---- the area's hooks (0x80102A38 entering, 0x80102ACC a scene, 0x80101EF4 each frame) ----
: entered ( -- )  6 map-reveal  0 vase-looked !  0 heading-home-said !  0 desk-looked !  0 drain-looked ! ;
3 constant the-washroom
: scene-hook ( scene -- )   \ the prop (actor 19) only in scene 3
    the-washroom = if
        the-prop-19 3 2 set-floor  the-prop-19 $2BB43 $170C negate $6773C set-pos  the-prop-19 0 0 0 set-rot
    else  the-prop-19 0 0 0 set-pos  then ;
: each-frame ( -- )
    heading-home-said @ 0=  0 20 55 23 59 between? and if
        1 heading-home-said !  ['] events.ovl003:entry-80102CB4 start-event exit  then
    roland-break-in-scene-seen flag? 0=  $014 story@ 0= and  26 14 30 15 0 between? and if
        floor @ 1 = if  ['] events.ovl003:entry-80103978  else  ['] events.ovl003:entry-80103A0C  then  start-event
        roland-break-in-scene-seen flag-set  then ;

\ ---- talking here (0x801026B0): their lines by their stops ----
create steve-lines  0 , $B2 ,  3 , $B2 ,  6 , $B3 ,  8 , $B4 ,  13 , $B5 ,  15 , $B6 ,  -1 ,   \ src: D_80102F6C
create morgan-lines  $11 , $B7 ,  $38 , $B8 ,  -1 ,
create robert-lines  $B , $BA ,  $F , $BB ,  $13 , $BC ,  -1 ,
create winona-lines  $D , $BD ,  -1 ,
7 constant usual-gesture   $B2 constant thank-you-for-caring
: line-if ( slot table -- handled? )
    {: s t :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s usual-gesture one-liner  true ;
: steve-talks ( -- handled? )
    steve stop-of 6 =  $014 story@ 0= and  roland-break-in-scene-seen flag? 0= and if
        thank-you-for-caring steve usual-gesture one-liner  true exit  then
    steve steve-lines line-if ;
: sarah-talks ( -- handled? )   \ src: func_80102550: her funeral talk once, then "let's go to Emma's room"
    sarah stop-of case
        0 of  $AF  endof
        1 of  $B0  endof
        2 of
            has-companion? 0=  floor @ 1 = and if
                roland-funeral-talk-seen flag? 0= if
                    ['] events.ovl003:entry-801042F8 start-event  roland-funeral-talk-seen flag-set  true exit  then
                ['] events.ovl003:entry-80102F20 start-event  true exit
            then  $B1  endof
        drop false exit
    endcase  sarah in-their-place one-liner  true ;
: talks-here ( slot -- handled? )
    case
        sarah of  sarah-talks  endof
        steve of  steve-talks  endof
        morgan of  morgan morgan-lines line-if  endof
        robert of  robert robert-lines line-if  endof
        winona of  winona winona-lines line-if  endof
        false swap
    endcase ;
' talks-here this-area area-talk!

\ ---- Steve's and Sarah's talks here (src: their handlers' cmd 0x5C, area 3) ----
: at-home? ( -- flag )  area @ this-area = ;
: steves-talks ( -- handled? )   \ src: func_801028BC
    at-home? 0= if  false exit  then
    $060 story@ 0=  today 25 = and if  1 $060 story+!  ['] events.ovl003:entry-80104834 start-event  true exit  then
    $064 story@ 1 and 0=  hudson-apology-seen flag? and if
        $064 story@ 1 or $064 story!  ['] events.ovl003:entry-80104888 start-event  true exit  then
    false ;
: sarahs-talks ( -- handled? )   \ src: func_8010297C
    at-home? 0= if  false exit  then
    $068 story@ 0=  today 25 = and if  1 $068 story+!  ['] events.ovl003:entry-801048C4 start-event  true exit  then
    roland-barton-suicide-talk-seen flag? 0=  $034 story@ 0<> and if
        roland-barton-suicide-talk-seen flag-set  ['] events.ovl003:entry-80104960 start-event  true exit  then
    false ;
' steves-talks steve conversation!   ' sarahs-talks sarah conversation!

\ ---- the C its events call ----
: emmas-key-taken ( -- )  got-emmas-key flag-toggle  emmas-key give ;   \ 0x80102A80
this-area ovl $80102A80 ' emmas-key-taken native:

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
