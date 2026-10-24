\ area000.fs - area 0's rules: the police station - the front desk, the offices, the cells, the map board, the
\ computer's case files (src: ovl000.c, the area's overlay; decomp/docs/areas/area00.md). What ○ and □ do here
\ beyond the tables' rows, the area's hooks, who says what, and the C its events call.
IN: rules.area000
USING: engine actors messages clock story-flags items townsfolk floors events look talk areas matthew ;

0 constant this-area
10 constant desk-stand-in   $3D152 constant at-the-desk   \ (actor 10 stands in at the front desk: its x there)
8 constant desk-officer
2 constant the-map   13 constant item-13   \ (items: not all named yet)

\ ---- what Matthew has done here this visit (src: the overlay's counters, cleared on entering) ----
variable board-looks   variable board-seen   variable cabinet-looks   variable poster-seen
variable upstairs-done   variable closing-done   variable computer-seen
: forget-visit ( -- )
    0 board-looks !  0 board-seen !  0 cabinet-looks !  0 poster-seen !  0 upstairs-done !  0 closing-done ! ;

\ ---- people where their schedules have them (src: at_place, at_xz) ----
: at-place? ( slot x z -- flag )  {: s x z :}  s person-now drop nip {: px pz :}  px x =  pz z = and ;
10 constant mel-in-the-cells          \ (Mel's record 10)
: mel-in-a-cell? ( -- flag )  mel mel-in-the-cells at-record? ;                        \ src: func_8002E1A8
: morgan-at-his-desk? ( -- flag )  morgan $3CA38 $20023 at-place? ;                    \ src: func_8002FF40
: hudson-at-his-desk? ( -- flag )  hudson $3CAE8 $2069B at-place? ;                    \ src: func_8002BCB0
: dennis-at-stop-10? ( -- flag )  dennis 10 standing-at? ;                              \ src: func_8002B1F4

\ ---- ○: the look check (0x801027C4) and its actions (0x801028F0) ----
enum: look-action  nothing-here  1 the-poster-room  the-poster  the-cabinet  5 the-board  7 the-desk  9 out-the-window
  the-cells  the-files ;
: look-check ( zone check -- action )
    case
        $0A of  drop the-desk exit  endof
        $83 of  drop  mel-in-a-cell? if  the-cells  else  nothing-here  then  exit  endof
        $0E of  drop out-the-window exit  endof
        $09 of  drop  computer-seen @ if  the-files  else  nothing-here  then  exit  endof
    endcase
    floor @ 1 <> if  drop nothing-here exit  then
    case
        0 of  the-board  endof   3 of  the-cabinet  endof   5 of  the-poster-room  endof   6 of  the-poster  endof
        nothing-here swap
    endcase ;
: once-then ( var first again -- )  {: v f a :}  v @ if  a  else  f  1 v !  then  start-event ;
23 constant cells-page
: the-cells-scene ( -- )   \ the cells: whom he finds there (src: case 10)
    $018 story@ $20000600 and $200 = if
        cell-confrontation1-seen flag-toggle  ['] events.ovl000:entry-80106048 start-event exit  then
    $018 story@ $60000000 and $20000000 = if   \ (and the notebook learns its page: src g_bio_known bit 23)
        cell-confrontation2-seen flag-toggle  ['] events.ovl000:entry-80106C5C start-event
        cells-page s" notebook" actor-named send learn-bio  exit  then
    ['] events.ovl000:entry-8010550C start-event ;
: look-do ( action -- )
    case
        the-poster-room of  ['] events.ovl000:entry-80107E18 start-event  endof
        the-poster of  poster-seen ['] events.ovl000:entry-80107DF8 ['] events.ovl000:entry-80107E08 once-then  endof
        the-cabinet of
            cabinet-looks @ 0= if  ['] events.ovl000:entry-80107FF0  else  ['] events.ovl000:entry-80108044  then
            start-event  cabinet-looks @ 1+ 2 min cabinet-looks !  endof
        the-board of  board-seen ['] events.ovl000:entry-80107CC0 ['] events.ovl000:entry-80107CF0 once-then  endof
        the-desk of  item-13 has-item? if  ['] events.ovl000:entry-80107BE0  else  ['] events.ovl000:entry-80105488  then
            start-event  endof
        out-the-window of  ['] events.ovl000:entry-80107DDC start-event  endof
        the-cells of  the-cells-scene  endof
        the-files of  ['] events.ovl000:entry-80107BA4 start-event  endof
        drop run-rows 0
    endcase ;

\ ---- □: the act check (0x801038E4) and its actions (0x80102B58) ----
enum: act-action  do-nothing  4 open-the-cabinet  6 study-the-board  12 the-computer ;
: act-check ( zone check -- action )
    9 = if  drop  computer-seen @ 0= if  the-computer  else  do-nothing  then  exit  then
    floor @ 1 <> if  drop do-nothing exit  then
    case
        0 of  board-seen @ if  study-the-board  else  do-nothing  then  endof
        3 of  cabinet-looks @ 2 = if  open-the-cabinet  else  do-nothing  then  endof
        do-nothing swap
    endcase ;
: study-board ( -- )   \ after Hudson's apology, the second look finds a place on the map
    hudson-apology-seen flag? 0= if  ['] events.ovl000:entry-80107D20 start-event exit  then
    board-looks @ case
        0 of  ['] events.ovl000:entry-80107D20 start-event  endof
        1 of  ['] events.ovl000:entry-80107D58 start-event  0 map-reveal  endof
        ['] events.ovl000:entry-80107D90 start-event
    endcase  board-looks @ 1+ 2 min board-looks ! ;
: act-do ( action -- )
    case
        open-the-cabinet of  ['] events.ovl000:entry-80108098 start-event  endof
        study-the-board of  study-board  endof
        the-computer of   \ (someone at the desk: they stop him)
            morgan-at-his-desk? hudson-at-his-desk? or if  ['] events.ovl000:entry-80096DE4  else  ['] events.ovl000:entry-80096DC8  then
            start-event  1 computer-seen !  endof
        drop run-rows 0
    endcase ;

\ ---- the area's hooks (0x80102378 entering, 0x80103650 a scene, 0x80102584 each frame) ----
: arrival ( -- )   \ at most one scene on arriving
    station-father-barton-seen flag? 0=  $02C story@ 0<> and  mel 20 standing-at? and  28 11 0 reached? 0= and if
        station-father-barton-seen flag-toggle  ['] events.ovl000:entry-80105E64 start-event exit  then
    station-shove-it-seen flag? 0=  28 11 5 11 52 between? and if
        station-shove-it-seen flag-toggle  ['] events.ovl000:entry-801062C0 start-event exit  then
    station-wolf-seen flag? 0=  $044 0 story-bit flag? and if
        station-wolf-seen flag-set  ['] events.ovl000:entry-80107288 start-event  then ;
\ entering (src: func_80102378): the prop on the desk until item 2's taken, on its side; the desk officer at his stop
$68 constant desk-prop   2 constant desk-item   $3CAE8 constant prop-x   $168 negate constant prop-y   $211BC constant prop-z
$400 constant on-its-side   8 constant the-desk-officer
: desk-things ( -- )
    desk-item has-item? if  desk-prop 0 0 0 set-pos  else  desk-prop prop-x prop-y prop-z set-pos  then
    desk-prop 0 0 on-its-side set-rot  the-desk-officer place-at-stop ;
: entered ( -- )
    9 map-reveal  forget-visit  desk-things  arrival ;
2 constant the-back-office   3 constant the-computer-room   1 constant the-front
: scene-hook ( scene -- )
    case
        the-back-office of   \ Hudson's apology: he and Morgan leave
            hudson 0 standing-at? if
                morgan s" townsfolk" actor-named send person-depart  hudson s" townsfolk" actor-named send person-depart
                ['] events.ovl000:entry-801056A4 start-event  hudson-apology-seen flag-set
            then  endof
        the-front of  0 computer-seen !  endof
    endcase ;
5 constant upstairs
0 constant counter-zone   \ (the zone's index: behind the front desk)
: closing-time ( -- )   \ 22:00: the station closes, whichever room he is in
    scene @ case
        0 of  ['] events.ovl000:entry-80104E44 start-event  endof   1 of  ['] events.ovl000:entry-80104EA8 start-event  endof
        2 of  ['] events.ovl000:entry-80104F08 start-event  endof   5 of  ['] events.ovl000:entry-80104F9C start-event  endof
        4 of  ['] events.ovl000:entry-8010501C start-event  endof
    endcase ;
: stand-in ( -- )   \ 0x80103810: someone at the front desk while Hudson is away
    hudson 0 standing-at? if  exit  then
    desk-stand-in at-the-desk $348 $20927 set-pos  desk-stand-in 0 $800 0 set-rot  desk-stand-in 0 0 set-floor
    desk-stand-in $48 1 anim ;
: stand-in-there? ( -- flag )  desk-stand-in person-now 2drop drop at-the-desk = ;
$020 1 story-bit constant minigame-30-begun   \ (src: +0x20 bit 1)
$1F8 constant minigame-30-a   $1FC constant minigame-30-b   $200 0 story-bit constant minigame-30-c
: minigame-30? ( -- flag )   \ src: func_80102584: once its three conditions are met
    minigame-30-begun flag? 0=  minigame-30-a story@ 0<> and  minigame-30-b story@ 0<> and  minigame-30-c flag? and ;
: each-frame ( -- )   \ (the computer's screens: the map's - map.fs)
    minigame-30? if  minigame-30-begun flag-set  30 minigame exit  then
    at-zone drop counter-zone = if
        matthew-was matthew-actor send figure-place  ['] events.ovl000:entry-80107DC8 start-event exit  then
    scene @ upstairs =  28 11 45 11 47 between? and  upstairs-done @ 0= and  stand-in-there? and if
        1 upstairs-done !  ['] events.ovl000:entry-801050E4 start-event exit  then
    closing-done @ 0=  0 22 0 22 59 between? and if  1 closing-done !  closing-time exit  then
    hudson 0 standing-at? 0=  stand-in-there? 0= and  25 19 0 19 0 between? and if  stand-in  then ;

\ ---- talking here (0x80102EA0): a line of the area's by where they are ----
create morgan-standing   \ src: D_80105520
  0 , 365 ,  2 , 369 ,  7 , 370 ,  9 , 371 ,  14 , 373 ,  16 , 374 ,  19 , 375 ,  23 , 376 ,  25 , 377 ,  27 , 378 ,
  30 , 379 ,  32 , 380 ,  34 , 381 ,  36 , 382 ,  39 , 383 ,  41 , 384 ,  43 , 385 ,  45 , 386 ,  48 , 387 ,  50 , 388 ,
  52 , 389 ,  54 , 390 ,  58 , 391 ,  60 , 392 ,  63 , 393 ,  68 , 394 ,  70 , 395 ,  73 , 396 ,  78 , 397 ,  80 , 398 ,
  83 , 399 ,  -1 ,
create morgan-walking  0 , $16F ,  1 , $170 ,  13 , $174 ,  -1 ,
create winona-lines  3 , $19A ,  11 , $19B ,  20 , $19C ,  26 , $19D ,  32 , $19E ,  38 , $19F ,  42 , $1A0 ,  -1 ,
create hudson-lines   \ src: D_801055A0, by stop (0: his usual)
  423 , 0 , 426 , 0 , 427 , 0 , 428 , 0 , 429 , 0 , 430 , 0 , 431 , 0 , 432 , 0 , 433 ,
17 constant hudson-stops   $1A8 constant hudsons-usual   $1A9 constant hudson-leaving
create nozz-lines  5 , $190 ,  12 , $191 ,  16 , $192 ,  21 , $193 ,  26 , $194 ,  -1 ,
create cohen-lines  17 , $195 ,  26 , $196 ,  34 , $197 ,  38 , $198 ,  42 , $199 ,  -1 ,
create robert-lines  4 , $1A1 ,  -1 ,
create lidds-lines  $12 , $1A2 ,  -1 ,
create barton-lines  3 , $1A6 ,  -1 ,
create mel-lines  25 , 419 ,  43 , 420 ,  51 , 421 ,  -1 ,                     \ src: D_801055DC
create steve-lines  2 , 437 ,  4 , 438 ,  7 , 439 ,  11 , 440 ,  -1 ,           \ src: D_801055C8
create dennis-lines  21 , 441 ,  27 , 442 ,  33 , 443 ,  51 , 444 ,  59 , 445 ,  -1 ,   \ src: D_801055EC
create person-117-lines  3 , 434 ,  9 , 434 ,  15 , 434 ,  21 , 434 ,  27 , 434 ,  33 , 434 ,  39 , 434 ,  -1 ,
create person-119-lines  7 , 436 ,  15 , 436 ,  23 , 436 ,  31 , 436 ,  39 , 436 ,  47 , 436 ,  55 , 436 ,  -1 ,
create person-84-lines  3 , 435 ,  11 , 435 ,  19 , 435 ,  27 , 435 ,  35 , 435 ,  43 , 435 ,  51 , 435 ,  -1 ,
create person-81-lines  5 , 446 ,  16 , 446 ,  27 , 446 ,  38 , 446 ,  49 , 446 ,  60 , 446 ,  71 , 446 ,  -1 ,
$1C9 constant stand-in-line   7 constant usual-gesture   8 constant desk-gesture
: their-stop ( slot -- stop )  person-of at-stop @ ;   \ (walking: the one they left)
: line-if ( table slot -- handled? )   \ a line by their stop, if the table has one
    {: t s :}  t s their-stop a-line-by-stop  dup 0< if  drop false exit  then  s usual-gesture one-liner  true ;
: line-or-none ( table slot -- true )   \ src: line_table: a line if there is one; nothing said otherwise
    {: t s :}  s walking? if  true exit  then  t s line-if drop  true ;
: standing-line ( table slot -- handled? )  dup walking? if  2drop false exit  then  line-if ;
: hudson-talks ( -- handled? )
    hudson walking? if
        hudson their-stop case  0 of  hudsons-usual  endof  1 of  hudson-leaving  endof  drop false exit  endcase
    else
        hudson their-stop hudson-stops < if  hudson their-stop cells hudson-lines + @  else  0  then
        ?dup 0= if  hudsons-usual  then
    then  hudson usual-gesture one-liner  true ;
: talks-here ( slot -- handled? )
    case
        morgan of  morgan walking? if  morgan-walking morgan line-if  else  morgan-standing morgan line-if  then  endof
        winona of  winona-lines winona standing-line  endof
        hudson of  hudson-talks  endof
        nozz of  nozz-lines nozz standing-line  endof
        cohen of  cohen-lines cohen standing-line  endof
        robert of  robert-lines robert standing-line  endof
        lidds of  lidds-lines lidds standing-line  endof
        father-barton of  barton-lines father-barton standing-line  endof
        mel of  mel-lines mel line-or-none  endof
        steve of  steve-lines steve line-or-none  endof
        dennis of  dennis-lines dennis line-or-none  endof
        $75 of  person-117-lines $75 line-or-none  endof
        $77 of  person-119-lines $77 line-or-none  endof
        $54 of  person-84-lines $54 line-or-none  endof
        $51 of  person-81-lines $51 line-or-none  endof
        desk-stand-in of  stand-in-line desk-stand-in desk-gesture one-liner  true  endof
        false swap
    endcase ;
' talks-here this-area area-talk!

\ the desk officer's talks (0x801032BC): his scenes in order, then a plain line
$2917 constant officers-line
: officer-talks ( -- handled? )
    $080 story@ 0= if  1 $080 story!  ['] events.ovl000:entry-80107584 start-event  true exit  then
    $05C 12 story-bit flag? 0=  clock-time time>day 25 =  25 16 30 reached? and and if
        ['] events.ovl000:entry-80107768 start-event  $05C 12 story-bit flag-set  true exit  then
    $084 story@ 0=  $018 story@ $28000000 and $08000000 = and if
        1 $084 story!  ['] events.ovl000:entry-801076CC start-event  true exit  then
    $08C 0 story-bit flag? 0=  $000 4 story-bit flag? and  clock-time time>day 26 = and if
        $08C 0 story-bit flag-toggle  ['] events.ovl000:entry-8010788C start-event  true exit  then
    $088 story@ 0=  clock-time time>day 27 >= and if
        1 $088 story+!  ['] events.ovl000:entry-80107844 start-event  true exit  then
    officers-line desk-officer desk-gesture one-liner  true ;
' officer-talks desk-officer conversation!

\ ---- the C its events and rows call (ovl000's evfn_*) ----
create mel-cell-stops  41 , 49 ,   create rolein-cell-stops  28 , 30 ,   \ src: D_80102258, D_8010225C (the 29th, 30th)
: cell-visit ( -- )   \ 0x80103498: the cell visit on the 29th or 30th (minigame 36), else a scene
    clock-time time>day {: day :}
    $020 story@ $50 and $10 <>  mel-in-a-cell? 0= or  cell-occupied flag? 0= or  day 29 30 1+ within 0= or if
        ['] events.ovl000:entry-80107B00 start-event exit  then
    $020 story@ $40 xor $020 story!  $044 story@ 1 xor $044 story!
    36 minigame
    mel day 29 - cells mel-cell-stops + @ goto-stop  rolein day 29 - cells rolein-cell-stops + @ goto-stop
    day 29 = if  morgan $41 goto-stop exit  then
    clock-time time>hour dup 16 < if  drop morgan $46 goto-stop exit  then
    21 < if  morgan $49  else  morgan $4B  then  goto-stop ;
: first-or-later ( -- )   \ 0x80103710: minigame 29 once, else a scene
    $01C story@ $C0000000 and $40000000 =  dennis-at-stop-10? 0= and if
        $01C story@ $80000000 or $01C story!  29 minigame exit  then
    $080 story@ 0= if  ['] events.ovl000:entry-80105088  else  ['] events.ovl000:entry-801079C0  then  start-event ;
\ ---- the computer (src: ovl000 FPolice.C): the map's other face, the case files it holds by the story ----
$324862 constant the-files-always   $80080 constant after-the-hunt   $400 constant after-the-cells   8 constant after-the-check
$020 3 story-bit constant the-hunt-told   $00A 0 story-bit constant checked-once   \ (src: +0x20 bit 3, the word at +0x0A)
: listed-files ( -- places )   \ src: func_80103F3C
    the-files-always
    the-hunt-told flag? if  after-the-hunt or  then
    cell-confrontation2-seen flag? if  after-the-cells or  then
    checked-once flag? if  after-the-check or  then ;
$1F8 constant first-file-read   $1FC constant second-file-read   $200 constant last-file-read
$164 0 story-bit constant journey-talk-done   $020 5 story-bit constant the-second-told
$018 27 story-bit constant photo-asked   $018 22 story-bit constant letter-read
: either ( flag xt-then xt-else -- xt )  rot if  drop  else  nip  then ;
: file-of ( place -- xt | 0 )   \ src: func_80103994
    case
        1 of  journey-talk-done flag? ['] events.ovl000:entry-801082AC ['] events.ovl000:entry-80108278 either  endof
        2 of  the-hunt-told flag? ['] events.ovl000:entry-80108308 ['] events.ovl000:entry-801082E0 either  endof
        3 of  1 first-file-read story!  ['] events.ovl000:entry-80108324  endof
        5 of  the-second-told flag? ['] events.ovl000:entry-80108380 ['] events.ovl000:entry-80108358 either  endof
        6 of  ['] events.ovl000:entry-80108560  endof
        7 of  ['] events.ovl000:entry-801083B4  endof
        10 of  1 second-file-read story!  ['] events.ovl000:entry-801083DC  endof
        11 of  ['] events.ovl000:entry-80108410  endof
        14 of  ['] events.ovl000:entry-80108444  endof
        16 of  photo-asked flag? ['] events.ovl000:entry-80108494 ['] events.ovl000:entry-8010846C either  endof
        17 of  ['] events.ovl000:entry-801084A4  endof
        19 of  ['] events.ovl000:entry-801084CC  endof
        20 of  letter-read flag? ['] events.ovl000:entry-80108510 ['] events.ovl000:entry-801084F4 either  endof
        21 of  last-file-read story@ 1 or last-file-read story!  ['] events.ovl000:entry-8010852C  endof
        0 swap
    endcase ;
: open-file ( place -- event | -1 )  file-of ?dup if  launch-event  else  -1  then ;
: the-computer-opens ( -- )   \ 0x80103FEC
    listed-files ['] open-file s" map" actor-named send computer-open ;
: shove-it ( -- )   \ 0x8010388C: once
    station-shove-it-seen flag? if  exit  then  station-shove-it-seen flag-set  ['] events.ovl000:entry-801062C0 start-event ;
: isabella-held ( -- )   \ 0x801037B8
    1 $038 story+!  isabella 40 goto-stop  cell-occupied flag-set ;
this-area ovl $80103498 ' cell-visit native:        this-area ovl $80103710 ' first-or-later native:
this-area ovl $80103FEC ' the-computer-opens native:  this-area ovl $8010388C ' shove-it native:
this-area ovl $80103810 ' stand-in native:          this-area ovl $801037B8 ' isabella-held native:

' look-check ' look-do ' act-check ' act-do  this-area content area-rules!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
' events.ovl000:entry-801041AC 29 minigame-script!   ' events.ovl000:entry-801045B8 30 minigame-script!   \ (src: area_minigame_start)
' events.ovl000:entry-80104708 36 minigame-script!
