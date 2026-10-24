\ area013.fs - area 13's rules: the park (src: ovl013.c; decomp/docs/areas/area13.md; port/src/game/ovl013.c): the
\ old man there by the hour (coming and going), talking to him (his talks are a character pack's), his lessons (a
\ minigame that's a script by the lesson reached), who says what, a scene at night once, the way down with or
\ without a companion, the lights by floor and the time of day.
IN: rules.area013
USING: engine actors messages clock story-flags townsfolk events look talk areas matthew driving ;

13 constant this-area
171 constant the-old-man   86 constant person-86   69 constant person-69   117 constant person-117
119 constant person-119
: hour ( -- h )  clock-time time>hour ;
: at-minute? ( h m -- flag )  swap 60 * +  clock-time minutes/day mod = ;
$00C constant heard-of-him   $010 0 story-bit constant met-him   \ (src: +0x0C set in the town; +0x10 bit 0)
$41900 constant on-his-bench-x

\ ---- he's here (src: D_80103D7C) 14:05 - 17:00, 23:05 - 23:59, 00:00 - 00:59 ----
variable he's-here
: his-hours? ( -- flag )  0 14 5 17 0 between?  0 23 5 23 59 between? or  0 0 0 0 59 between? or ;
: entered ( -- )
    8 map-reveal  his-hours? he's-here !
    he's-here @ if  ['] events.ovl013:entry-801028FC  else  ['] events.ovl013:entry-8010293C  then  start-event ;
: scene-hook ( scene -- )  drop ;

\ ---- the lights (src: func_801027FC): objects 6 .. 17 by a row of 12 - his floor, by day (07:00 - 16:29) or night ----
create light-rows   \ src: D_80103D34 (three by day, three by night)
  1 c, 1 c, 1 c, 1 c, 0 c, 0 c, 0 c, 0 c, 0 c, 0 c, 0 c, 0 c,   1 c, 0 c, 1 c, 1 c, 1 c, 0 c, 0 c, 0 c, 0 c, 0 c, 0 c, 0 c,
  1 c, 0 c, 0 c, 1 c, 1 c, 1 c, 0 c, 0 c, 0 c, 0 c, 0 c, 0 c,   0 c, 0 c, 0 c, 0 c, 0 c, 0 c, 1 c, 1 c, 1 c, 1 c, 0 c, 0 c,
  0 c, 0 c, 0 c, 0 c, 0 c, 0 c, 1 c, 0 c, 1 c, 1 c, 1 c, 0 c,   0 c, 0 c, 0 c, 0 c, 0 c, 0 c, 1 c, 0 c, 0 c, 1 c, 1 c, 1 c,
12 constant lights-a-row   6 constant first-light
variable lights-row   -1 lights-row !
: light-row ( -- row )   \ his floor's, by day; three on by night
    clock-time minutes/day mod  7 60 *  16 60 * 29 + 1+  within if  floor @ 1-  else  floor @ 2 +  then ;
: lights ( -- )
    light-row dup lights-row @ = if  drop exit  then  dup lights-row !  lights-a-row * light-rows + {: r :}
    lights-a-row 0 do  i first-light +  r i + c@ 0<>  s" areas" actor-named send object-shown  loop ;

\ ---- each frame (0x80102670): he comes at 14:04 and 23:00, goes at 17:01 and 01:00 ----
: each-frame ( -- )
    lights
    he's-here @ 0=  14 4 at-minute?  23 0 at-minute? or and if
        ['] events.ovl013:entry-80102958 start-event  1 he's-here ! exit  then
    he's-here @  17 1 at-minute?  1 0 at-minute? or and if
        ['] events.ovl013:entry-80102968 start-event  0 he's-here !  then ;

\ ---- talking here (0x801020E0) ----
create winona-lines  4 , $62 ,  6 , $63 ,  10 , $64 ,  15 , $65 ,  21 , $66 ,  41 , $67 ,  -1 ,
create lidds-lines  3 , $55 ,  10 , $56 ,  12 , $57 ,  24 , $58 ,  30 , $59 ,  36 , $5A ,  -1 ,
create isabella-lines  $D , $5B ,  $13 , $5C ,  $19 , $5D ,  -1 ,
create person-69-lines  5 , 94 ,  13 , 94 ,  21 , 94 ,  29 , 94 ,  37 , 94 ,  45 , 94 ,  53 , 94 ,  -1 ,   \ src: D_80102978
create person-117-lines  1 , 95 ,  7 , 95 ,  13 , 95 ,  19 , 95 ,  25 , 95 ,  31 , 95 ,  37 , 95 ,  -1 ,   \ src: D_80102998
create person-119-lines  6 , 96 ,  14 , 96 ,  22 , 96 ,  30 , 96 ,  38 , 96 ,  46 , 96 ,  54 , 96 ,  -1 ,  \ src: D_801029B8
7 constant usual-gesture
: line-if ( slot table gesture -- handled? )   \ standing at a stop the table has
    {: s t g :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s g one-liner  true ;
: walking-line ( slot table -- handled? )   \ only while walking, by the stop they left (src: the original's own)
    {: s t :}  s walking? 0= if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s usual-gesture one-liner  true ;
244 constant his-talks   2 constant meeting-him   3 constant a-lesson-offered   \ (the character pack: CD.BIN 244)
: meets-him ( -- )  his-talks meeting-him call-pack-script ;
: offers-a-lesson ( -- )  his-talks a-lesson-offered call-pack-script ;
: old-man-talks ( -- true )   \ src: func_80101FF0: at night his own lines; by day the meeting, or a lesson
    hour 20 >=  hour 5 < or if
        heard-of-him story@ 0= if  ['] events.ovl013:entry-80102A6C
        else  the-old-man person-now drop 2drop on-his-bench-x = if  ['] events.ovl013:entry-80102A08
        else  ['] events.ovl013:entry-801029F0  then  then  start-event  true exit
    then
    heard-of-him story@ 0<>  met-him flag? 0= and if  met-him flag-set  ['] meets-him start-event  true exit  then
    ['] offers-a-lesson start-event  true ;
: talks-here ( slot -- handled? )
    case
        person-86 of  $61 person-86 8 one-liner  true  endof
        winona of  winona winona-lines usual-gesture line-if  endof
        lidds of  lidds lidds-lines usual-gesture line-if  endof
        rolein of  rolein 5 standing-at? if  $68 rolein 1 one-liner  true  else  false  then  endof
        isabella of  isabella isabella-lines usual-gesture line-if  endof
        person-69 of  person-69 person-69-lines usual-gesture line-if  endof
        person-117 of  person-117 person-117-lines walking-line  endof
        person-119 of  person-119 person-119-lines usual-gesture line-if  endof
        the-old-man of  old-man-talks  endof
        false swap
    endcase ;
' talks-here this-area area-talk!

\ ---- the C its events call ----
: night-talk ( -- )   \ 0x80102568: on the day her call set, once, at her stop here
    winona-night-talk-seen flag? if  exit  then
    $1C4 story@ 2 rshift $FF and  clock-time time>day <> if  exit  then
    winona person-of at-stop @ {: st :}  st 6 =  st $F = or if
        ['] events.ovl013:entry-80102DCC start-event  winona-night-talk-seen flag-set  then ;
: the-way-down ( -- )   \ 0x80102620: with a companion along, or alone
    following? if  ['] events.ovl013:entry-80102A80  else  ['] events.ovl013:entry-801036A0  then  start-event ;
variable lesson   3 constant last-lesson   20 constant fuel-for-a-lesson   120 constant most-fuel
: lesson-learned ( -- )   \ 0x8010274C: the next lesson (up to the last), and fuel (src: fuel_set: 120 at most)
    lesson @ last-lesson < if  1 lesson +!  then
    fuel @ fuel-for-a-lesson + most-fuel min s" driving" actor-named send fuel-set ;
create lessons  ' events.ovl013:entry-80103938 , ' events.ovl013:entry-80103A64 ,
                ' events.ovl013:entry-80103B54 , ' events.ovl013:entry-80103C74 ,   \ src: D_80103D24
: the-lesson ( -- )  lesson @ cells lessons + @ execute ;   \ minigame 39: the lesson reached (a script)
this-area ovl $80102568 ' night-talk native:   this-area ovl $80102620 ' the-way-down native:
this-area ovl $8010274C ' lesson-learned native:
' the-lesson 39 minigame-script!

this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
