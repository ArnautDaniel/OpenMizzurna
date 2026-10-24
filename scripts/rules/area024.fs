\ area024.fs - area 24's rules: the church (src: ovl024.c; decomp/docs/areas/area24.md; port/src/game/ovl024.c):
\ its crate (crate.fs; its place by the floor he's on), the caretaker, the scenes on arriving (the first that
\ fits), the scenes by room, a meeting's time and closing time, the doors by the story, a letter (○ then □), the
\ stairs up from the basement, who says what, the talks here, the rope (minigame 17: the pad's left and right).
IN: rules.area024
USING: engine actors messages keys clock story-flags items townsfolk events look talk areas matthew crate rumble ;

24 constant this-area
16 constant the-crate-slot   95 constant the-caretaker   149 constant the-letter   148 constant the-rope
$18 constant the-motel-key
create the-crate  crate-place allot
\ (src: func_801020C8: its place on the ground floor (floor 1) or in the basement (floor 0), by his floor)
: crate-home ( -- )
    floor @ 1 = if  the-crate the-crate-slot $90DD5 $64F0 negate $1FE3B this-area 1 crate!
    else  the-crate the-crate-slot $9142D $6040 negate $20205 this-area 0 crate!  then ;
crate-home

$034 constant barton-gone            \ (src: +0x34: the letter's sequence over)
$01C 17 story-bit constant heard-the-old-story   \ (src: +0x1C bit 17: the caretaker's, after the donation)
$01C 24 story-bit constant basement-door-told    \ (src: +0x1C bit 24: a phone call's)
$018 19 story-bit constant letter-gone           \ (src: +0x18 bit 19: never set)
$104 1 story-bit constant barton-talked-in-the-cemetery   \ (src: +0x104 bit 1)
$094 0 story-bit constant donation-asked         \ (src: +0x94 bit 0: a hospital talk's)
30 constant barton-gone-stop
: barton-home? ( -- flag )  father-barton 0 at-record? ;   \ (src: func_8002B8AC: at the church)

\ ---- this visit (src: D_800A3940 the meeting on, D_801066E8 shown out of it, D_801066EC closing done,
\ D_801066F0 the letter looked at, D_801066F4 locked in) ----
variable meeting-on   variable shown-out   variable closing-done   variable letter-looked-at   variable locked-in

: night? ( -- flag )  0 5 0 19 59 between? 0= ;
: missing-now? ( -- flag )   \ src: func_80042EA4
    barton-home?  $018 story@ $60000 and $20000 = and  night? and ;
$90624 constant caretaker-x   $64F0 negate constant caretaker-y   $206BA constant caretaker-z   $7A8 negate constant caretaker-heading
: on-arriving ( -- )   \ the first that fits (the prayer only on an even minute: else none)
    kathy-funeral-seen flag?  church-barton-prayer-seen flag? 0= and  barton-home? and  club-come-here-often-seen flag? 0= and if
        clock-time 2 mod 0= if  church-barton-prayer-seen flag-set  ['] events.ovl024:entry-8010465C start-event  then  exit  then
    church-repentant-thugs-seen flag? 0=  barton-talked-in-the-cemetery flag? and  barton-home? and
    club-come-here-often-seen flag? 0= and if
        church-repentant-thugs-seen flag-set  ['] events.ovl024:entry-80104878 start-event  exit  then
    missing-now? if
        church-winona-missing-seen flag-toggle  ['] events.ovl024:entry-80104C6C start-event  event-lock
        the-caretaker 0 0 0 set-pos  father-barton 0 0 0 set-pos  1 locked-in !  then ;
\ scene 0's two object sets: from the 25th 14:00 the later one (src: load_area's, kept while here; func_80103820)
$14 constant before-set   $15 constant later-set
variable later-church
: church-drawn ( -- )
    scene @ 0<> if  exit  then  before-set later-church @ 0= shown!  later-set later-church @ shown! ;
: entered ( -- )   \ src: func_80102930
    25 14 0 reached? later-church !
    20 map-reveal  0 meeting-on !  0 shown-out !  0 locked-in !
    the-caretaker caretaker-x caretaker-y caretaker-z set-pos  the-caretaker 0 caretaker-heading 0 set-rot
    the-crate forget-crate  on-arriving  0 letter-looked-at !  0 closing-done ! ;

$91706 constant letter-x   $663A negate constant letter-y   $1F5B7 constant letter-z   $15E negate constant letter-heading
5 constant the-basement-room
: scene-hook ( scene -- )   \ src: func_80102B6C: the letter; the basement room's scene, once
    letter-gone flag? if  the-letter 0 0 0 set-pos
    else  the-letter letter-x letter-y letter-z set-pos  the-letter 0 letter-heading 0 set-rot  then
    the-basement-room <> if  exit  then
    $01C story@ $02020000 and $20000 =  barton-gone story@ 0<> and if
        church-james-scene-bit flag-toggle  1 church-james-scene-seen story!
        ['] events.ovl024:entry-80105BA0 start-event exit  then
    the-crate forget-crate ;

\ ---- each frame (0x80102C68): the crate; out of the meeting at its time; closing time ----
: crate-step-here ( -- )  the-crate crate-state @ placing = if  crate-home  then  the-crate crate-step ;
: meeting-soon? ( -- flag )  25 12 57 12 59 between? ;
: each-frame ( -- )
    crate-step-here
    shown-out @ 0=  meeting-soon? and  scene @ 0= and  matthew-perch 1 <> and if
        1 shown-out !  ['] events.ovl024:entry-80104408 start-event exit  then
    closing-done @  0 20 0 20 59 between? 0= or if  exit  then
    scene @ case
        0 of  1 closing-done !  ['] events.ovl024:entry-80103B9C start-event  endof
        1 of  1 closing-done !  ['] events.ovl024:entry-8010393C start-event  endof
    endcase ;

\ ---- ○ (0x80103250 / 0x801032E4): at the foot of the basement stairs, facing them: up ----
enum: look-action  nothing-here  up-the-stairs ;
$91126 constant stairs-x   $1FE81 constant stairs-z   200 constant stairs-near
$281 constant stairs-heading-from   $5C4 constant stairs-heading-to
: at-the-stairs? ( -- flag )
    matthew-now {: x y z h :}  x stairs-x - s>f fdup f*  z stairs-z - s>f fdup f* f+ fsqrt stairs-near s>f f<
    h 4095 and stairs-heading-from stairs-heading-to within and ;
: look-check ( zone check -- action )
    nip 0< 0=  floor @ 0<> or if  nothing-here exit  then
    at-the-stairs? if  up-the-stairs  else  nothing-here  then ;
: look-do ( action -- )
    up-the-stairs = if  ['] events.ovl024:entry-80106384 start-event exit  then  run-rows ;
: act-check ( zone check -- action )  2drop 0 ;
: act-do ( action -- )  drop run-rows ;

\ ---- the crate: ○ takes hold, □ climbs; the letter: ○ looks (once its time's come), then □ reads it ----
: push-it ( slot -- true )  drop the-crate crate-push  true ;
: climb-it ( slot -- true )  drop the-crate crate-climb  true ;
' push-it the-crate-slot examined-by!   ' climb-it the-crate-slot taken-by!
: look-at-the-letter ( slot -- true )   \ src: 0x801034B0
    drop ['] events.ovl024:entry-8010652C start-event  1 letter-looked-at !  true ;
: letter-readable? ( -- flag )  church-winona-missing-seen flag?  letter-looked-at @ 0<> and ;   \ src: 0x80103480
: read-the-letter ( slot -- handled? )   \ src: actor_handler_149's act
    drop letter-readable? 0= if  false exit  then
    true father-barton person-of travelling !  read-kathys-letter flag-toggle
    ['] events.ovl024:entry-80104DA0 start-event  true ;
' look-at-the-letter the-letter examined-by!   ' read-the-letter the-letter taken-by!

\ ---- talking here (0x80102DA4) ----
7 constant usual-gesture
: line-at ( slot stop msg gesture -- handled? )   \ at that stop, the line
    {: s st m g :}  s st standing-at? 0= if  false exit  then  m s g one-liner  true ;
: line-from ( slot table -- handled? )
    {: s t :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s usual-gesture one-liner  true ;
create barton-lines  0 , 199 ,  2 , 200 ,  5 , 201 ,  7 , 202 ,  9 , 203 ,  11 , 204 ,  13 , 205 ,  15 , 206 ,
  17 , 207 ,  19 , 208 ,  21 , 209 ,  23 , 210 ,  25 , 211 ,  27 , 212 ,  -1 ,
create lidds-lines  1 , $C4 ,  8 , $C5 ,  16 , $C6 ,  -1 ,
create steve-lines  1 , $D9 ,  18 , $DA ,  22 , $DB ,  26 , $DC ,  -1 ,
create praying-lines  1 , 222 ,  9 , 222 ,  17 , 222 ,  25 , 222 ,  33 , 222 ,  41 , 222 ,  49 , 222 ,  -1 ,
create new-year-lines  5 , 225 ,  13 , 225 ,  21 , 225 ,  29 , 225 ,  37 , 225 ,  45 , 225 ,  53 , 225 ,  -1 ,
create for-her-lines  4 , 226 ,  13 , 226 ,  22 , 226 ,  31 , 226 ,  40 , 226 ,  49 , 226 ,  58 , 226 ,  -1 ,
create next-year-lines  1 , 227 ,  8 , 227 ,  15 , 227 ,  23 , 227 ,  30 , 227 ,  37 , 227 ,  44 , 227 ,  -1 ,
create for-the-lost-lines  1 , 228 ,  12 , 228 ,  23 , 228 ,  34 , 228 ,  45 , 228 ,  56 , 228 ,  67 , 228 ,
  9 , 228 ,  20 , 228 ,  31 , 228 ,  42 , 228 ,  53 , 228 ,  64 , 228 ,  75 , 228 ,  -1 ,
119 constant one-praying   71 constant one-for-new-year   83 constant one-for-her   70 constant one-for-next-year
81 constant one-for-the-lost
5 constant prayer-line   4 constant caretaker-gesture
: caretaker-talks ( -- true )   \ the donation, once asked about it; else his line
    heard-the-old-story flag? 0=  donation-asked flag? and if  ['] events.ovl024:entry-80103A94 start-event  true exit  then
    prayer-line the-caretaker caretaker-gesture one-liner  true ;
: talks-here ( slot -- handled? )
    case
        morgan of  morgan 3 $C1 in-their-place line-at  endof
        cohen of  cohen 1 $C3 in-their-place line-at  endof
        nozz of  nozz 1 $C2 usual-gesture line-at  endof
        james of  james $43 $DD in-their-place line-at  endof
        robert of  robert 1 $D6 usual-gesture line-at  endof
        lidds of  lidds lidds-lines line-from  endof
        steve of  steve steve-lines line-from  endof
        dennis of  dennis 1 $D7 usual-gesture line-at drop  true  endof
        samuel of  samuel 2 $D8 usual-gesture line-at drop  true  endof
        father-barton of  father-barton barton-lines line-from  endof
        one-praying of  one-praying praying-lines line-from  endof
        one-for-new-year of  one-for-new-year new-year-lines line-from  endof
        one-for-her of  one-for-her for-her-lines line-from  endof
        one-for-next-year of  one-for-next-year next-year-lines line-from  endof
        one-for-the-lost of  one-for-the-lost for-the-lost-lines line-from  endof
        the-caretaker of  caretaker-talks  endof
        false swap
    endcase ;
' talks-here this-area area-talk!
: the-key-for-the-room ( -- handled? )   \ src: func_801033E8: after he needed a room, the key, once
    area @ this-area <> if  false exit  then
    $018 story@ $02800000 and $800000 <>  barton-home? 0= or if  false exit  then
    got-motel-key flag-toggle  the-motel-key s" items" actor-named send give-item
    ['] events.ovl024:entry-80105914 start-event  true ;
' the-key-for-the-room father-barton conversation!

\ ---- the C its rows and events call ----
: the-meeting ( -- )   \ 0x80103328: the 25th, 13:00 - 13:30: the meeting from up here (once); else a look
    25 13 0 13 30 between? 0= if  ['] events.ovl024:entry-80104370 start-event exit  then
    town-meeting-seen flag? if  ['] events.ovl024:entry-801042FC start-event exit  then
    ['] events.ovl024:entry-80103CCC start-event  1 meeting-on !  town-meeting-seen flag-set ;
: shut-or ( xt -- )  locked-in @ if  drop ['] events.ovl024:entry-80104580  then  start-event ;
: front-door ( -- )  ['] events.ovl024:entry-80105E8C shut-or ;   \ 0x801034DC
: side-door ( -- )  ['] events.ovl024:entry-80105FD4 shut-or ;    \ 0x8010351C
: to-the-quarters ( -- )   \ 0x8010355C: open only while he's locked in
    locked-in @ 0=  letter-gone flag? or if  ['] events.ovl024:entry-80104580  else  ['] events.ovl024:entry-80106024  then
    start-event ;
: basement-room ( -- )   \ 0x801035B8
    basement-door-told flag?  barton-gone story@ 0<> and if  ['] events.ovl024:entry-8010618C  else  ['] events.ovl024:entry-80104580  then
    start-event ;
: out-of-the-office ( -- )   \ 0x80103614: after the letter, its sequence's end
    read-kathys-letter flag?  barton-gone story@ 0= and if  ['] events.ovl024:entry-801045A8  else  ['] events.ovl024:entry-801060D8  then
    start-event ;
: to-the-basement ( -- )   \ 0x80103674
    barton-gone story@ if  ['] events.ovl024:entry-801061C8  else  ['] events.ovl024:entry-80104580  then  start-event ;
: the-old-story-heard ( -- )  heard-the-old-story flag-toggle ;   \ 0x801036D4
: barton-gone! ( -- )   \ 0x80103708 (the world goes on)
    father-barton barton-gone-stop goto-stop  event-unlock  1 barton-gone story! ;
: off-the-crate ( -- )  the-crate crate-cancel ;   \ 0x80103740
: into-the-nave ( -- )   \ 0x80103760: shut late, and around the meeting
    clock-time time>hour 22 >=  25 12 54 13 59 between? or if  ['] events.ovl024:entry-80105F5C  else  ['] events.ovl024:entry-80105F18  then
    start-event ;
: crate-again ( -- )  the-crate forget-crate ;   \ 0x801033DC
this-area ovl $80103328 ' the-meeting native:   this-area ovl $801034DC ' front-door native:
this-area ovl $8010351C ' side-door native:   this-area ovl $8010355C ' to-the-quarters native:
this-area ovl $801035B8 ' basement-room native:   this-area ovl $80103614 ' out-of-the-office native:
this-area ovl $80103674 ' to-the-basement native:   this-area ovl $801036D4 ' the-old-story-heard native:
this-area ovl $80103708 ' barton-gone! native:   this-area ovl $80103740 ' off-the-crate native:
this-area ovl $80103760 ' into-the-nave native:   this-area ovl $801033DC ' crate-again native:

\ ---- the rope (minigame 17, src: area_minigame_update): each new press of left or right a pull; at 10, 25 and
\ 50 his strain shows; at 80 he's free (its scene) ----
80 constant pulls-to-free   1 constant a-tug   255 constant free-strength   10 constant free-shake   \ (the pad: src D_800F8248)
$1401 constant straining   $1403 constant strain-1   $1404 constant strain-1-held
$1405 constant strain-2   $1406 constant strain-2-held   $1407 constant strain-3
variable pulls   variable left-was   variable right-was
: pulled? ( -- flag )
    pad-left key-down? dup left-was @ 0= and  swap left-was !
    pad-right key-down? dup right-was @ 0= and  swap right-was !  or ;
: strain ( -- )
    pulls @ case
        10 of  matthew strain-1 strain-1-held 0 anim-then  endof
        25 of  matthew strain-2 strain-2-held 0 anim-then  endof
        50 of  matthew strain-3 strain-3 0 anim-then  endof
    endcase ;
: the-rope-minigame ( -- )
    0 pulls !  pad-left key-down? left-was !  pad-right key-down? right-was !
    matthew straining 1 anim
    begin  yield  pulled? if  1 pulls +!  strain  a-tug 0 0 shake  then  pulls @ pulls-to-free >= until
    0 free-strength free-shake shake  events.ovl024:entry-80103A08 ;   \ (free: the large motor 10 frames)
' the-rope-minigame 17 minigame-script!

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
this-area ' church-drawn area-every-frame!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!

: barton's-flare ( -- )  father-barton s" flare" actor-named send flare-on ;   \ minigame 38 (flare.fs)
' barton's-flare 38 minigame-code!
