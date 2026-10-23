\ area009.fs - area 9's rules: the lake - the boat rental hut's coffee counter and the pier (src: ovl009.c;
\ decomp/docs/areas/area09.md; port/src/game/ovl009.c): the counter's stools and ordering from the counterman
\ (counter.fs), the boat, a scene there once, who says what, the C its events call.
IN: rules.area009
USING: engine actors messages clock story-flags items townsfolk events look talk areas matthew window list-menu
    counter ;

9 constant this-area
97 constant counterman   64 constant the-drink   22 constant item-22
: today ( -- day )  clock-time time>day ;

\ ---- ordering from the counterman (0x80102768) ----
65 constant coffee-label   $4F constant first-drink?   $50 constant another?   $54 constant changed-mind
create drink-lines  $51 , $52 ,   \ src: D_8010434C
enum: drink-steps  asking-him choosing bringing served drinking clearing closing-words ;
variable drink-step   variable drinks
: he-says ( msg -- )  counterman 0 one-liner ;
: drink-menu ( -- )  coffee-label  coffee-label 1+  0 0 0 0 0  s" list-menu" actor-named send list-open ;
: picked ( -- done? )
    menu-up? window-up? or if  false exit  then
    menu-answer @ still-open = if  drink-menu  false exit  then
    menu-answer @ cancelled = if  changed-mind he-says  still-open menu-answer !  closing-words drink-step !  false exit  then
    the-drink the-prop !  menu-answer @ cells drink-lines + @ he-says  still-open menu-answer !
    bringing drink-step !  false ;
: lake-order ( -- done? )
    drink-step @ case
        asking-him of  drinks @ if  another?  else  first-drink?  then he-says  choosing drink-step !  false  endof
        choosing of  picked  endof
        closing-words of  window-up? if  false exit  then  asking-him drink-step !  true  endof
        bringing of  ['] events.ovl009:entry-80104358 start-event  served drink-step !  false  endof
        served of
            stool-circle? if  counterman talk-with  false exit  then
            stool-square? if  drinking drink-step !  then  false  endof
        drinking of  ['] events.ovl009:entry-80104448 start-event  clearing drink-step !  false  endof
        clearing of  ['] events.ovl009:entry-801043E8 start-event  1 drinks +!  asking-him drink-step !  true  endof
    endcase ;

\ the counter's settings (src: ovl009's copy; port/src/game/ovl009.c LAKE_COUNTER)
create lake-counter  bar-counter allot
at-lake lake-counter bar !   false lake-counter along-x? !
$512D9 lake-counter cross-lo !   $513B5 lake-counter cross-hi !   3072 lake-counter facing !
false lake-counter last-front-only? !   false lake-counter flip? !   -1 lake-counter right-step !
0 lake-counter counter-y !   $512DD lake-counter stand-cross !   $5130B lake-counter seat-cross !
: two ( a b addr -- )  tuck cell+ !  ! ;
$844 $843 lake-counter sit-motions two   $84A $847 lake-counter lean-in-motions two
$84B $848 lake-counter lean-motions two   $84C $849 lake-counter lean-out-motions two
$845 $846 lake-counter stand-motions two   $84F lake-counter beckon-motion !
0 lake-counter last-right !   3 lake-counter last-left !   -1 lake-counter no-lean-right !
$538 negate lake-counter camera-yaw !   $538 negate lake-counter camera-yaw-alt !   0 lake-counter camera-alt-seats !
$4B2 negate lake-counter camera-eye-y !   $200 negate lake-counter camera-look-y !   $632 negate lake-counter camera-dist !
$5130B lake-counter camera-cross !
counterman lake-counter barman !   $51117 lake-counter barman-cross !   1024 lake-counter barman-heading !
' lake-order lake-counter order-xt !

\ ---- the area's hooks (0x80102234 entering, 0x80102418 a scene, 0x801023F8 each frame) ----
: entered ( -- )
    12 map-reveal  forget-counter  0 drinks !  asking-him drink-step !
    counterman $51093 0 $6B069 set-pos  counterman 0 $3D4 0 set-rot ;
2 constant the-pier
: scene-hook ( scene -- )   \ the pier, 12/28 - 12/30, at their stop there: a scene once (src: +0x18 & 0x20001080)
    the-pier <> if  exit  then
    $018 story@ $20001080 and $20000080 <> if  exit  then
    winona person-of at-stop @ {: st :}  st $18 =  st $1E = or  st $24 = or 0= if  exit  then
    today 28 31 within 0= if  exit  then
    lake-winona-scene-seen flag-toggle  item-22 s" items" actor-named send give-item
    ['] events.ovl009:entry-80103CA8 start-event ;
: each-frame ( -- )  menu-up? if  exit  then  lake-counter counter-step ;

\ ---- talking here (0x8010203C) ----
create winona-lines  $12 , $48 ,  $18 , $49 ,  30 , $4A ,  36 , $4B ,  -1 ,
create isabella-lines  $20 , $47 ,  -1 ,
create dennis-lines  $38 , $4D ,  $40 , $4E ,  -1 ,
create wolf-lines  7 , $41 ,  14 , $42 ,  21 , $43 ,  28 , $44 ,  35 , $45 ,  40 , $46 ,  -1 ,
7 constant usual-gesture
: line-if ( slot table -- handled? )
    {: s t :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s usual-gesture one-liner  true ;
: talks-here ( slot -- handled? )
    case
        winona of  winona winona-lines line-if  endof
        isabella of  isabella isabella-lines line-if  endof
        dennis of  dennis dennis-lines line-if  endof
        wolf of  wolf wolf-lines line-if  endof
        false swap
    endcase ;
' talks-here this-area area-talk!

\ ---- the C its rows and events call ----
: two-to-their-stops ( -- )  isabella $2B goto-stop  dennis $4A goto-stop ;   \ 0x80102344
8 constant isabella-at-the-lake   $008 24 story-bit constant boat-offered
: the-boat ( -- )   \ 0x80102380: with a companion, or the boatmaster's offer, or the boat
    winona going-along? if  ['] events.ovl009:entry-80103C88
    else  boat-offered flag? 0=  isabella isabella-at-the-lake at-record? and if  ['] events.ovl009:entry-80103C9C
    else  ['] events.ovl009:entry-80103C20  then  then  start-event ;
$51243 constant over-the-counter   $194 negate constant counter-top
: on-the-counter ( -- )   \ 0x80103934: the drink before his stool
    at-lake matthew seat-of nip seat-spot drop nip nip {: z :}  the-prop @ over-the-counter counter-top z set-pos ;
this-area ovl $80102344 ' two-to-their-stops native:   this-area ovl $80102380 ' the-boat native:
this-area ovl $80103934 ' on-the-counter native:
' events.ovl009:entry-80104508 33 minigame-script!   \ (src: area_minigame_start's 33)

this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
