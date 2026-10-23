\ area012.fs - area 12's rules: Club Wolves (src: ovl012.c, the area's overlay; decomp/docs/areas/area12.md): who is
\ there tonight (the bouncer, the fan, Loretta, Isabella on stage), the night's timetable and Isabella's show,
\ closing time, James's first meeting, Mel's confrontation; the bar's stools (counter.fs) and ordering from Wolf;
\ who says what, the area's conversations (Wolf, James, Mel, the bouncer, the fan, Loretta), the C its events call.
\ Its look and act are the tables' rows alone (its doors). (The fights and the prompt: the minigames, to come.)
IN: rules.area012
USING: engine actors messages clock story-flags townsfolk events look talk areas matthew window list-menu counter ;

12 constant this-area
99 constant bouncer   112 constant fan   111 constant loretta   69 constant person-69
: today ( -- day )  clock-time time>day ;
: minute-of-day ( -- m )  clock-time minutes/day mod ;
: at-minute? ( hour minute -- flag )  swap 60 * +  minute-of-day = ;   \ src: event_time_is (any day)
: standing-at? ( slot stop -- flag )  over stop-of 0< if  2drop false exit  then  swap person-of at-stop @ = ;

\ ---- the story's words this area keeps (src: the story block; not named yet) ----
$024 constant first-show-day   $025 constant fan-fight-day   $026 constant show-cancelled-day   \ (bytes)
$0CC 0 story-bit constant heard-mel-a-jerk      \ (the diner's: Lorraine on Mel)
$184 constant gave-james-item-8   $17C constant james-asked-is-she-ok
$1A8 constant wolf-no-minors   $1AC constant wolf-many-customers   $1B0 constant wolf-lively-tonight
$1B4 0 story-bit constant wolf-on-isabella      $038 constant isabella-held-count
$120 1 story-bit constant mel-stiff-line         $008 23 story-bit constant fans-number-known   \ (never written)
$018 16 story-bit constant loretta-call         \ (call 7 answered: Loretta at Club Wolves)
: on-day? ( offset -- flag )  story-c@ today = ;

\ ---- the night's places (src: func_80103B94) ----
variable bouncer-on   variable show-done   variable on-stage   variable met-james   variable fan-on
variable closing-done   variable church-said   variable show-thread
2 constant at-the-club   \ (Isabella's record: here - src func_8002C1F4)
4 constant james-at-the-club   0 constant wolf-at-the-bar   $10 constant here-tonight   \ (their records)
: isabella-here? ( -- flag )  isabella at-the-club at-record? ;
: put ( slot x z heading -- )   \ on the club's floor, standing (src: actor_set_pos, its heading, actor_set_floor 12 1)
    {: s x z h :}  s x 0 z set-pos  s 0 h 0 set-rot  s this-area 1 set-floor ;
: put-idle ( slot x z heading -- )  {: s x z h :}  s x z h put  s 0 1 anim ;
: park ( slot -- )  0 0 0 set-pos ;
$71B85 constant loretta-x   $4754D constant loretta-z   $48 constant loretta-pose
$71F1E constant door-x   $46909 constant door-z   $7200E constant show-door-x
$718CE constant fan-x   $472C8 constant fan-z
$71932 constant stage-x   $46B08 constant stage-z   $200 constant facing-the-room
: on-stage-tonight ( fan-x fan-z fan-heading -- )   \ Isabella singing, the bouncer at the stage door, the fan before her
    {: fx fz fh :}  isabella stage-x stage-z facing-the-room put
    1 on-stage !  1 show-done !  1 bouncer-on !
    bouncer show-door-x 0 door-z set-pos   fan fx fz fh put-idle ;
: tonight ( -- )
    loretta-call flag?  club-come-here-often-seen flag? 0= and  clock-time time>hour 22 < and if   \ src: func_8002F030
        loretta loretta-x loretta-z 1024 put  loretta loretta-pose 1 anim  then
    0 21 58 23 25 between? if  1 bouncer-on !  bouncer door-x door-z 0 put-idle  else  bouncer park  then
    0 21 28 23 55 between? if  1 fan-on !  fan fan-x fan-z 0 put-idle  else  fan park  then
    fan-fight-day on-day? if  exit  then
    first-show-day on-day? if  $71B08 $46F30 $872 on-stage-tonight exit  then
    today last-day <  isabella-here? and  0 22 30 22 58 between? and  club-isabella-show-seen flag? and if
        ['] events.ovl012:entry-80106954 launch-event show-thread !
        $71A04 $46CD8 $9AA on-stage-tonight
    then ;
: confrontation ( -- )   \ Mel and Winona here after the hospital: Mel's bar confrontation
    $020 story@ $1800 and  $800 =  mel here-tonight at-record? and  winona here-tonight at-record? and if
        mel-bar-confrontation-seen flag-toggle  ['] events.ovl012:entry-80108324 start-event  then ;

\ ---- ordering from Wolf (bar_order): he asks, the menu, his line; no beer for a kid ----
61 constant chips-label   0 constant chips   3 constant beer
$122 constant wanna-order   $123 constant drink-again   $12C constant buzz-off   $126 constant no-alcohol
create first-lines  $127 , $124 , $125 , $126 ,   \ src: D_8010507C, by the pick (chips, coffee, milk, beer)
create again-lines  $129 , $128 , $128 , $126 ,   \ src: D_80105084
create orders-of  4 cells allot                   \ this visit, by the pick (src: D_80108F88)
63 constant the-food   64 constant the-drink
enum: bar-steps  wolf-asks choosing bringing served eating clearing closing-words ;
variable bar-step   variable bar-pick
: forget-orders ( -- )  orders-of 4 cells erase  wolf-asks bar-step ! ;
: wolf-says ( msg -- )  wolf 0 one-liner ;
: had-a-drink? ( -- flag )  orders-of cell+ @  orders-of 2 cells + @ or 0<> ;
: bar-menu ( -- )  chips-label  chips-label 1+  chips-label 2 +  chips-label 3 +  0 0 0
    s" list-menu" actor-named send list-open ;
: bar-picked ( -- done? )
    menu-up? window-up? or if  false exit  then
    menu-answer @ still-open = if  bar-menu  false exit  then
    menu-answer @ cancelled = if  buzz-off wolf-says  still-open menu-answer !  closing-words bar-step !  false exit  then
    menu-answer @ bar-pick !  still-open menu-answer !
    bar-pick @ beer = if  no-alcohol wolf-says  closing-words bar-step !  false exit  then
    bar-pick @ chips = if  the-food  else  the-drink  then the-prop !
    bar-pick @ cells orders-of + @ if  again-lines  else  first-lines  then  bar-pick @ cells + @ wolf-says
    1 bar-pick @ cells orders-of + +!  bringing bar-step !  false ;
: food? ( -- flag )  bar-pick @ chips = ;
: bar-order ( -- done? )   \ a step a frame (the counter's)
    bar-step @ case
        wolf-asks of  had-a-drink? if  drink-again  else  wanna-order  then wolf-says  choosing bar-step !  false  endof
        choosing of  bar-picked  endof
        closing-words of  window-up? if  false exit  then  wolf-asks bar-step !  true  endof
        bringing of  ['] events.ovl012:entry-80105094 start-event  served bar-step !  false  endof
        served of
            stool-circle? if  wolf talk-with  false exit  then
            stool-square? if  eating bar-step !  then  false  endof
        eating of
            food? if  ['] events.ovl012:entry-80105210  else  ['] events.ovl012:entry-80105184  then start-event
            clearing bar-step !  false  endof
        clearing of  ['] events.ovl012:entry-80105124 start-event  wolf-asks bar-step !  true  endof
    endcase ;

\ the bar's settings (src: ovl012's copy of the counter; port/src/game/ovl012.c CLUB_COUNTER)
create club-counter  bar-counter allot
at-club club-counter bar !   true club-counter along-x? !
$46C17 club-counter cross-lo !   $46CF3 club-counter cross-hi !   2048 club-counter facing !
true club-counter last-front-only? !   true club-counter flip? !   -1 club-counter right-step !
0 club-counter counter-y !   $46C1B club-counter stand-cross !   $46C49 club-counter seat-cross !
: two ( a b addr -- )  tuck cell+ !  ! ;
$A93 $AA5 club-counter sit-motions two   $A98 $A95 club-counter lean-in-motions two
$A99 $A96 club-counter lean-motions two   $A9A $A97 club-counter lean-out-motions two
$A94 $AA6 club-counter stand-motions two   $AAF club-counter beckon-motion !
0 club-counter last-right !   5 club-counter last-left !   -1 club-counter no-lean-right !
$940 negate club-counter camera-yaw !   $940 negate club-counter camera-yaw-alt !   0 club-counter camera-alt-seats !
$4B0 negate club-counter camera-eye-y !   $242 negate club-counter camera-look-y !   $6D4 negate club-counter camera-dist !
$46C49 club-counter camera-cross !
wolf club-counter barman !   $46A55 club-counter barman-cross !   0 club-counter barman-heading !
' bar-order club-counter order-xt !

\ ---- the area's hooks (0x80103B94 entering, 0x80104D34 a scene, 0x80103FE8 each frame) ----
: entered ( -- )
    17 map-reveal  -1 show-thread !  forget-counter  forget-orders
    0 met-james !  0 church-said !  0 show-done !  0 closing-done !  0 fan-on !  0 on-stage !  0 bouncer-on !
    tonight  confrontation ;
0 constant the-bar
: scene-hook ( scene -- )   \ James's first meeting, Wolf introduced
    the-bar <> if  exit  then
    club-james-meeting-seen flag? 0=  james james-at-the-club at-record? and  wolf wolf-at-the-bar at-record? and
    today 28 < and if
        ['] events.ovl012:entry-801060E8 start-event  1 met-james !  club-james-meeting-seen flag-toggle  then ;
: staff ( -- )   \ the bouncer on at 22:00 and off at 23:30, the fan in at 21:30 and out at midnight
    bouncer-on @ 0= if  22 0 at-minute? if  ['] events.ovl012:entry-80099120 start-event  1 bouncer-on !  then  then
    bouncer-on @ if  23 30 at-minute? if  ['] events.ovl012:entry-80099154 start-event  0 bouncer-on !  then  then
    fan-on @ 0= if  21 30 at-minute? if  ['] events.ovl012:entry-8009917C start-event  then  then
    fan-on @ if  0 0 at-minute? if  0 fan-on !  ['] events.ovl012:entry-800991DC start-event  then  then
    22 0 at-minute? if  loretta park  then ;
: the-show ( -- )   \ in the bar, on his feet: Isabella's show at 22:30, its end at 23:00
    matthew seated?  scene @ the-bar <> or if  exit  then
    on-stage @  0 23 0 23 15 between? and if
        show-thread @ kill-event  ['] events.ovl012:entry-80106B14 launch-event show-thread !  0 on-stage !  then
    today last-day <  isabella-here? and  show-done @ 0= and  0 22 30 22 35 between? and if
        club-isabella-show-seen flag? if
            ['] events.ovl012:entry-80107BA8 launch-event show-thread !  1 on-stage !
        else   \ she can't sing tonight
            ['] events.ovl012:entry-8010769C start-event  5 time-skip  today show-cancelled-day story-c!
        then  1 show-done !
    then ;
: each-frame ( -- )   \ src: func_80103FE8
    menu-up? if  exit  then
    club-counter counter-step  events-running @ control-given @ 0= and if  exit  then
    fan-fight-day on-day? 0= if  staff the-show  then
    closing-done @ 0=  0 1 58 2 59 between? and if
        1 closing-done !  scene @ 3 < if  ['] events.ovl012:entry-801052D0 start-event  then  then ;

\ ---- talking here (0x801043CC): a line of the area's by where they are ----
create morgan-lines  35 , 301 ,  44 , 302 ,  53 , 303 ,  74 , 304 ,  -1 ,
create james-lines  7 , 305 ,  18 , 306 ,  29 , 307 ,  40 , 308 ,  51 , 309 ,  62 , 310 ,  -1 ,
create winona-lines  45 , 311 ,  -1 ,
create wolf-lines  2 , 315 ,  9 , 316 ,  16 , 317 ,  23 , 318 ,  30 , 319 ,  -1 ,
create lorraine-lines   \ src: D_80105308
  1 , 329 ,  3 , 330 ,  6 , 331 ,  9 , 332 ,  12 , 333 ,  15 , 334 ,  17 , 335 ,  21 , 336 ,  24 , 337 ,  26 , 338 ,
  29 , 339 ,  31 , 340 ,  -1 ,
create person-69-lines  7 , 314 ,  15 , 314 ,  23 , 314 ,  31 , 314 ,  39 , 314 ,  47 , 314 ,  55 , 314 ,  -1 ,
create isabella-lines  17 , 325 ,  23 , 326 ,  29 , 327 ,  36 , 328 ,  -1 ,
create dennis-lines  25 , 320 ,  31 , 321 ,  37 , 322 ,  55 , 323 ,  63 , 324 ,  -1 ,
create mel-lines  17 , 312 ,  48 , 313 ,  -1 ,
7 constant usual-gesture   9 constant their-gesture   312 constant mel-what-for
: line-if ( table slot gesture -- handled? )
    {: t s g :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s g one-liner  true ;
: mel-talks ( -- handled? )   \ src: func_801042E4: at stop 4, once, the dealer's argument
    mel 4 standing-at? if
        club-mel-dealer-argument-seen flag? 0=  heard-mel-a-jerk flag? and  matthew seated? 0= and if
            club-mel-dealer-argument-seen flag-set  ['] events.ovl012:entry-80105718 start-event  true exit  then
        mel-what-for mel their-gesture one-liner  true exit
    then  mel-lines mel their-gesture line-if ;
: talks-here ( slot -- handled? )
    case
        morgan of  morgan-lines morgan usual-gesture line-if  endof
        james of  james-lines james usual-gesture line-if  endof
        mel of  mel-talks  endof
        winona of  winona-lines winona usual-gesture line-if  endof
        wolf of  wolf-lines wolf usual-gesture line-if  endof
        rolein of  lorraine-lines rolein usual-gesture line-if  endof
        person-69 of  person-69-lines person-69 usual-gesture line-if  endof
        isabella of  isabella-lines isabella their-gesture line-if  endof
        dennis of  dennis-lines dennis their-gesture line-if  endof
        false swap
    endcase ;
' talks-here this-area area-talk!

\ ---- their conversations here (called from their own when the area is 12) ----
: in-the-club? ( -- flag )  area @ this-area = ;
: once ( word -- )  1 swap story! ;
: says ( xt -- true )  start-event  true ;
: wolf-talks ( -- handled? )   \ src: func_80104B90
    in-the-club? 0= if  false exit  then
    wolf-no-minors story@ 0= if  wolf-no-minors once  ['] events.ovl012:entry-80108984 says exit  then
    wolf-many-customers story@ 0= if  wolf-many-customers once  ['] events.ovl012:entry-80108998 says exit  then
    wolf-lively-tonight story@ 0=  club-mel-dealer-argument-seen flag? and if
        wolf-lively-tonight once  ['] events.ovl012:entry-801089AC says exit  then
    wolf-on-isabella flag? 0=  isabella-held-count story@ 0<> and if
        wolf-on-isabella flag-set  ['] events.ovl012:entry-801089F4 says exit  then
    false ;
: james-talks ( -- handled? )   \ src: func_8010482C: Isabella's first show, then two talks
    in-the-club? 0= if  false exit  then
    club-isabella-show-seen flag? 0=  james james-at-the-club at-record? and  gave-james-item-8 story@ 0<> and
    show-done @ 0= and  met-james @ 0= and  isabella-here? and  show-cancelled-day on-day? 0= and if
        club-isabella-show-seen flag-toggle  today first-show-day story-c!
        1 fan-on !  1 bouncer-on !  1 on-stage !  1 show-done !
        ['] events.ovl012:entry-8010709C says exit  then
    james-asked-is-she-ok story@ 0=  club-drunk-fan-scene-seen flag? and if
        james-asked-is-she-ok once  ['] events.ovl012:entry-80108B5C says exit  then
    isabella-unsafe-seen flag? 0=  offlimits-door-seen flag? and if
        isabella-unsafe-seen flag-set  ['] events.ovl012:entry-80108BE0 says exit  then
    false ;
: mel-says-stiff ( -- handled? )   \ src: func_80104DF8: once, after his own talks
    in-the-club? 0= if  false exit  then
    mel-stiff-line flag? if  false exit  then  mel-stiff-line flag-set  ['] events.ovl012:entry-80108AE4 says ;
: bouncer-talks ( -- handled? )   \ src: func_80104AF4
    in-the-club? 0= if  false exit  then
    mel-bar-confrontation-seen flag? if  ['] events.ovl012:entry-8010533C says exit  then
    offlimits-door-seen flag? if  ['] events.ovl012:entry-80108C88 says exit  then
    club-drunk-fan-scene-seen flag? if  offlimits-door-seen flag-set  ['] events.ovl012:entry-80108C40 says exit  then
    false ;
: fan-talks ( -- handled? )   \ src: func_801049D8: on stage, the drunk fan's scene; after it, his notebook
    in-the-club? 0= fan-on @ 0= or if  false exit  then
    on-stage @ if
        $01C story@ $A and 2 <> if  false exit  then
        show-thread @ kill-event  -1 show-thread !  0 on-stage !  0 fan-on !
        club-drunk-fan-scene-seen flag-toggle  today fan-fight-day story-c!
        ['] events.ovl012:entry-80107EC0 says exit
    then
    club-drunk-fan-scene-seen flag?  fans-number-known flag? 0= and if  ['] events.ovl012:entry-80108D0C says exit  then
    false ;
create barton-tonight  13 , 17 , 21 , 25 , 29 ,   \ src: D_80105354: Father Barton's night stop, the 27th on
: loretta-talks ( -- handled? )   \ src: func_80104E50
    in-the-club? 0= if  false exit  then
    club-come-here-often-seen flag? 0= if
        club-come-here-often-seen flag-set  ['] events.ovl012:entry-80106414 start-event
        today 27 - 0 max 4 min cells barton-tonight + @  father-barton swap goto-stop  true exit
    then
    church-said @ if  ['] events.ovl012:entry-80108CFC  else  1 church-said !  ['] events.ovl012:entry-80108CDC  then
    says ;
' wolf-talks wolf conversation!   ' james-talks james conversation!   ' mel-says-stiff mel conversation!
' bouncer-talks bouncer conversation!   ' fan-talks fan conversation!   ' loretta-talks loretta conversation!

\ ---- the C its events call (ovl012's) ----
: fan-arrived ( -- )  1 fan-on ! ;   \ 0x80104F0C
$194 negate constant counter-top   $46B81 constant served-z
: on-the-bar ( -- )   \ 0x801037E8: the order before his stool
    at-club matthew seat-of nip seat-spot 2drop drop {: x :}  the-prop @ x counter-top served-z set-pos ;
: on-stool-2 ( -- )  club-counter 2 seat-now ;   \ 0x80103360: Wolf's introduction leaves him seated
this-area ovl $80104F0C ' fan-arrived native:   this-area ovl $801037E8 ' on-the-bar native:
this-area ovl $80103360 ' on-stool-2 native:

' events.ovl012:entry-8010709C 42 minigame-script!   \ (src: area_minigame_start's 42)
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
