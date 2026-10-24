\ area016.fs - area 16's rules: the high school's library building (src: ovl016.c; decomp/docs/areas/area16.md;
\ port/src/game/ovl016.c): its crate (crate.fs), closing time by room, a teacher by the hour (coming and going), the
\ key rack and the key (with a companion along), a poster, the scenes its rows' C picks (with or without a
\ companion), who says what, the talks here, the building's halves shown by where he is.
IN: rules.area016
USING: engine actors messages clock story-flags items townsfolk events look talk areas matthew crate ;

16 constant this-area
23 constant the-crate-slot   109 constant the-librarian   110 constant the-teacher   26 constant the-key
create the-crate  crate-place allot
the-crate the-crate-slot $5DE7C $F0 negate $22D8F this-area 1 crate!
: has? ( item -- flag )  has-item? ;
: give ( item -- )  s" items" actor-named send give-item ;
: winona-along? ( -- flag )  winona going-along? ;
: at-minute? ( h m -- flag )  swap 60 * +  clock-time minutes/day mod = ;

\ ---- this visit (src: D_8010325C closing done, D_801049DC the key takeable, D_801049E4 poster seen, D_801049E8 the
\ key's scene played, D_801049E0 the teacher here) ----
variable closing-done   variable key-takeable   variable poster-seen   variable key-scene   variable teacher-here

: entered ( -- )
    14 map-reveal  0 closing-done !  the-librarian 0 0 0 set-pos  the-crate forget-crate
    0 14 5 18 0 between? teacher-here !
    teacher-here @ if  ['] events.ovl016:entry-80103B2C  else  ['] events.ovl016:entry-80103B6C  then  start-event
    0 key-takeable !  0 poster-seen !  0 key-scene ! ;
: liz-stopped-early? ( -- flag )   \ at her stop 0 before 14:00, 7 before 15:00 or 5 before 8:00
    lidds person-of at-stop @ {: st :}  clock-time time>hour {: h :}
    st 0 =  h 14 < and  st 7 =  h 15 < and or  st 5 =  h 8 < and or ;
: scene-hook ( scene -- )   \ src: func_80102678
    case
        0 of
            library-christmas-miracle-seen flag? 0=  liz-stopped-early? and if
                ['] events.ovl016:entry-801040B8 start-event  library-christmas-miracle-seen flag-set  then  endof
        1 of
            the-librarian $5F0F3 $F0 negate $22F93 set-pos  the-librarian 0 $C00 0 set-rot
            the-librarian this-area 1 set-floor  the-librarian $A7 1 anim  endof
    endcase ;

\ ---- each frame (0x801030B0) ----
: closing ( -- )   \ 17:59 - 22:59, once: shown out by room and where he is
    closing-done @  0 17 59 22 59 between? 0= or if  exit  then
    matthew-now drop nip nip {: z :}
    scene @ case
        3 of  ['] events.ovl016:entry-801032D8  endof
        2 of  ['] events.ovl016:entry-80103358  endof
        1 of  z $233A8 <= if  ['] events.ovl016:entry-801033C4  else  ['] events.ovl016:entry-80103418  then  endof
        0 of  z $2270B <= if  ['] events.ovl016:entry-801034CC  else  z $23424 <= if  ['] events.ovl016:entry-80103548
              else  ['] events.ovl016:entry-8010346C  then  then  endof
        drop exit
    endcase  start-event  1 closing-done ! ;
variable half-shown   -1 half-shown !
: halves ( -- )   \ src: func_801031F0: in scene 0 the building's far half or near half by where he is
    scene @ 0<> if  exit  then
    matthew-now drop nip nip $23668 > {: far? :}  far? half-shown @ = if  exit  then  far? half-shown !
    2 far? s" areas" actor-named send object-shown  1 far? 0= s" areas" actor-named send object-shown ;
: each-frame ( -- )
    the-crate crate-step  halves  closing
    teacher-here @ 0=  14 4 at-minute? and if  ['] events.ovl016:entry-80103B88 start-event  1 teacher-here ! exit  then
    teacher-here @  18 1 at-minute? and if  ['] events.ovl016:entry-80103B98 start-event  0 teacher-here !  then ;

\ ---- ○ (0x80102C10 / 0x80102C70): the key rack, the poster; □ (0x80102D5C / 0x80102DA8): the key ----
enum: look-action  nothing-here  the-key-rack  the-poster ;
: look-check ( zone check -- action )
    nip case  0 of  the-key-rack  endof  11 of  the-poster  endof  nothing-here swap  endcase ;
: look-do ( action -- )
    case
        the-key-rack of
            key-scene @  the-key has? 0= and  winona-along? and if
                1 key-takeable !  ['] events.ovl016:entry-80103E28  else  ['] events.ovl016:entry-80103E1C  then
            start-event  endof
        the-poster of
            poster-seen @ if  ['] events.ovl016:entry-80104720  else  1 poster-seen !  ['] events.ovl016:entry-80104708  then
            start-event  endof
        drop run-rows 0
    endcase ;
enum: act-action  nothing-to-do  2 take-the-key ;
: act-check ( zone check -- action )  nip 0=  key-takeable @ 0<> and if  take-the-key  else  nothing-to-do  then ;
: act-do ( action -- )
    take-the-key = if  the-key give  ['] events.ovl016:entry-80103E34 start-event  0 key-takeable !  exit  then
    run-rows ;

\ ---- the crate: ○ takes hold, □ climbs ----
: push-it ( slot -- true )  drop the-crate crate-push  true ;
: climb-it ( slot -- true )  drop the-crate crate-climb  true ;
' push-it the-crate-slot examined-by!   ' climb-it the-crate-slot taken-by!

\ ---- talking here (0x801028B0) ----
create cohen-lines  12 , $71 ,  25 , $72 ,  33 , $73 ,  37 , $74 ,  41 , $75 ,  -1 ,
create james-lines  $D , $8F ,  $18 , $90 ,  $23 , $91 ,  $2E , $92 ,  $39 , $93 ,  -1 ,
create winona-lines  19 , $94 ,  23 , $95 ,  27 , $96 ,  29 , $97 ,  33 , $98 ,  35 , $99 ,  39 , $9A ,  -1 ,
create liz-standing   \ src: D_80103F5C (by stop; 0 none)
  118 , 0 , 0 , 0 , 0 , 119 , 0 , 120 , 0 , 0 , 0 , 0 , 0 , 121 , 123 , 125 , 0 , 0 , 0 , 0 , 126 , 128 , 130 , 0 , 0 ,
  0 , 131 , 133 , 135 , 0 , 0 , 0 , 136 , 138 , 140 , 0 , 0 , 0 , 141 , 0 , 142 , 0 ,
42 constant liz-stops
create liz-walking  13 , 122 ,  14 , 124 ,  20 , 127 ,  21 , 129 ,  26 , 132 ,  27 , 134 ,  32 , 137 ,  33 , 139 ,  -1 ,
7 constant usual-gesture   $5DCA5 constant at-the-rack-x
: quietly ( slot table -- true )   \ a line by their stop if there's one; none, nothing (the original's own)
    {: s t :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop true exit  then  s usual-gesture one-liner  true ;
: liz-talks ( -- handled? )   \ src: func_801027CC
    lidds person-of {: p :}
    p travelling @ 0= if
        lidds person-now drop 2drop at-the-rack-x =  winona-along? and if   \ (her scene with the key, once: minigame 11)
            key-scene @ 0= if  1 key-scene !  11 minigame  then  false exit  then
        p at-stop @ liz-stops u< if  p at-stop @ cells liz-standing + @  else  0  then
    else  liz-walking p at-stop @ a-line-by-stop  dup 0< if  drop 0  then  then
    ?dup 0= if  false exit  then  lidds usual-gesture one-liner  true ;
: teacher-talks ( -- true )
    met-ms-holly flag? if  ['] events.ovl016:entry-80103C4C start-event  true exit  then
    $070 story@ 0<> if  met-ms-holly flag-set  ['] events.ovl016:entry-80103BA8 start-event  true exit  then
    $2724 the-teacher usual-gesture one-liner  true ;
: talks-here ( slot -- handled? )
    case
        cohen of  cohen cohen-lines quietly  endof
        james of  james james-lines quietly  endof
        winona of  winona winona-lines quietly  endof
        lidds of  liz-talks  endof
        the-librarian of
            $010 story@ $6000 and $2000 = if  ['] events.ovl016:entry-80103FD8  else  ['] events.ovl016:entry-80103FF0  then
            start-event  true  endof
        the-teacher of  teacher-talks  endof
        false swap
    endcase ;
' talks-here this-area area-talk!
: liz-here ( -- handled? )   \ src: func_80102E58: once
    area @ this-area <>  $078 story@ 0<> or if  false exit  then
    1 $078 story!  ['] events.ovl016:entry-801043D0 start-event  true ;
' liz-here lidds conversation!

\ ---- the C its rows and events call ----
: main-door ( -- )   \ 0x80102DF8: the main building's door - the key, and its hours
    the-key has? 0= if  ['] events.ovl016:entry-80103F00 start-event exit  then
    0 17 40 18 59 between? 0= if  ['] events.ovl016:entry-8010461C start-event  then ;
: a-book ( -- )   \ 0x80102EA8: after a talk on the way, the book; once found, its line
    winona-ride-library-talk-seen flag? 0= if  ['] events.ovl016:entry-80103EF0 start-event exit  then
    found-winonas-book flag? 0= if  ['] events.ovl016:entry-80103E4C start-event  found-winonas-book flag-set exit  then
    ['] events.ovl016:entry-80103EA4 start-event ;
: with-her-or-not ( alone -- )   \ 0x80102F5C / 0x80102FC0: with a companion along, by the key
    winona-along? if  drop  the-key has? if  ['] events.ovl016:entry-80104064  else  ['] events.ovl016:entry-8010408C  then  then
    start-event ;
: door-a ( -- )  ['] events.ovl016:entry-80104464 with-her-or-not ;
: door-b ( -- )  ['] events.ovl016:entry-80104418 with-her-or-not ;
: the-archive ( -- )   \ 0x80103024: once, after a talk of her (+0x74): her page in the notebook (bit 20)
    used-newspaper-archive flag? 0=  $074 story@ 0<> and if
        used-newspaper-archive flag-set  ['] events.ovl016:entry-801035C4 start-event
        20 s" notebook" actor-named send learn-bio exit  then
    ['] events.ovl016:entry-80104038 start-event ;
: flag-30 ( -- )  1 $030 story! ;   \ 0x8010316C
: liz-asked ( -- ) ;   \ 0x8010317C (src: func_8002D620, Liz at her place - its answer unused: nothing)
\ Winona and Robert to the day's stop, the 26th - 28th (src: evfn_80047B7C, D_80098E50 / D_80098E54)
create winonas-day-stop  16 , 22 , 28 ,   create roberts-day-stop  6 , 8 , 12 ,
: day-stop ( table -- stop )  clock-time time>day 26 - 0 max 2 min cells + @ ;
: off-she-goes ( -- )   \ 0x8010319C: the companion leaves him, the two to the day's stops
    winona s" townsfolk" actor-named send person-unride
    winona winonas-day-stop day-stop goto-stop  robert roberts-day-stop day-stop goto-stop ;
this-area ovl $80102DF8 ' main-door native:   this-area ovl $80102EA8 ' a-book native:
this-area ovl $80102F5C ' door-a native:   this-area ovl $80102FC0 ' door-b native:
this-area ovl $80103024 ' the-archive native:   this-area ovl $8010316C ' flag-30 native:
this-area ovl $8010317C ' liz-asked native:   this-area ovl $8010319C ' off-she-goes native:
' events.ovl016:entry-80103704 11 minigame-script!   ' events.ovl016:entry-80103888 12 minigame-script!
' events.ovl016:entry-8010390C 16 minigame-script!

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
: leaving ( -- )  the-crate crate-cancel ;   \ src: func_80102F3C: off the crate
this-area ' leaving area-leave!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
