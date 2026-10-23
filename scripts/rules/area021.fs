\ area021.fs - area 21's rules: Haines Diner (src: ovl021.c, the area's overlay; decomp/docs/areas/area21.md). Its
\ look and act are the tables' rows alone (its three doors); here: the area's hooks (arriving, each frame: closing,
\ the robbery), today's special, who says what, Lorraine's conversations, and the C its events call.
\ (Sitting at the counter and ordering: to come.)
IN: rules.area021
USING: engine actors messages clock story-flags townsfolk events look talk areas matthew ;

21 constant this-area
40 constant passing-car   $1D constant passing-car-route   \ src: carline_start(40, 0x1D): a car passing outside
2 constant at-the-diner   \ (Mel's record 2: in the diner)
132 constant farm-worker   133 constant farm-worker-2   134 constant farm-worker-3

: today ( -- day )  clock-time time>day ;
: hour ( -- hour )  clock-time time>hour ;
: mel-here? ( -- flag )  mel at-the-diner at-record? ;   \ src: func_8002E244
: standing-at? ( slot stop -- flag )  over stop-of 0< if  2drop false exit  then  swap person-of at-stop @ = ;

\ ---- the story's words Lorraine's talks count in (src: the story block +0xC0 .. +0xEC; not named yet) ----
$0C0 constant asked-about-emma   $0C4 constant protest-told   $0C8 constant workers-told   $0CC 0 story-bit constant mel-a-jerk
$0D0 constant seen-mel?   $0D4 constant cant-reach-mel   $0D8 constant mel-looked-pale   $0DC 0 story-bit constant break-in-heard
$0E0 constant startled   $0E8 constant that-dumb   $0EC 1 story-bit constant asked-about-mels-drugs
$010 15 story-bit constant lorraine-told-on-mel   \ (src: +0x10 bit 15: read, set elsewhere)
$014 constant break-in-words                     \ (src: +0x14)
$05C constant diner-open-day   4 constant open-day-shift   $FF0 constant open-day-bits   \ (the door to the diner: ovl027)

\ ---- today's special (src: the area's init; the order menu's last entry) ----
create specials-line  238 , 239 , 240 , 241 , 242 , 243 , 237 ,   \ src: D_80104AE4 ("Today's special is ...!")
variable special-line
: today's-special ( -- )  today first-day - cells specials-line + @ special-line ! ;

\ ---- the area's hooks (0x8010362C entering, 0x801036B0 each frame) ----
variable closing-done   variable worker-told
: lorraine-meets-him ( -- )   \ 12/25: the car outside, Mel waiting
    passing-car s" traffic" actor-named send car-stop  passing-car passing-car-route s" traffic" actor-named send car-park
    mel s" townsfolk" actor-named send person-unlink  mel 0 1 anim   \ (and the bios: Lorraine's - to come)
    diner-lorraine-intro-seen flag-set  ['] events.ovl021:entry-80105024 start-event ;
: arrival ( -- )   \ src: func_80103B2C (both checked, in order)
    diner-lorraine-intro-seen flag? 0=  25 11 50 14 30 between? and if  lorraine-meets-him  then
    diner-mel-lorraine-argued flag? 0=  mel 13 standing-at? and if
        diner-mel-lorraine-argued flag-toggle  ['] events.ovl021:entry-80105828 start-event  then ;
: entered ( -- )
    11 map-reveal  0 worker-told !  0 closing-done !  today's-special  arrival ;
: scene-hook ( scene -- )  drop ;

1 constant the-back   \ (scene 1: behind door 84)
28 constant earlier-closing-day   20 constant closing-hour   19 constant earlier-closing-hour
$50179 constant near-morgans-x   $42125 constant near-morgans-z   22 constant morgan-at-his-table
: closing? ( -- flag )   \ from 20:00, from the 28th 19:00
    today earlier-closing-day < if  hour closing-hour >=  else  hour earlier-closing-hour >=  then ;
: by-morgan? ( -- flag )   \ Matthew past the table Morgan sits at
    matthew-now drop nip {: x z :}  x near-morgans-x >  z near-morgans-z > and ;
: robbery ( xt -- )  diner-robbery-seen flag-toggle  start-event ;   \ (his seat left, Morgan's talk ended: to come)
: each-frame ( -- )   \ src: func_80103C64
    closing-done @ 0=  closing? and if  1 closing-done !  ['] events.ovl021:entry-80104D4C start-event exit  then
    diner-robbery-seen flag? if  exit  then
    scene @ the-back <>  27 11 30 11 59 between? and if  ['] events.ovl021:entry-80106DF0 robbery exit  then
    morgan morgan-at-his-table standing-at?  by-morgan? and if  ['] events.ovl021:entry-80106308 robbery  then ;

\ ---- talking here (0x80103F80): a line of the area's by where they are ----
create mel-lines   \ src: D_80104F20
  2 , 250 ,  8 , 251 ,  13 , 252 ,  16 , 253 ,  24 , 254 ,  37 , 255 ,  42 , 256 ,  47 , 257 ,  50 , 258 ,  55 , 259 ,  -1 ,
create morgan-lines  26 , 260 ,  29 , 261 ,  38 , 262 ,  47 , 263 ,  66 , 264 ,  -1 ,
create winona-lines  2 , 249 ,  -1 ,
create cohen-lines  24 , 265 ,  -1 ,
create steve-lines  10 , 297 ,  17 , 298 ,  21 , 299 ,  25 , 300 ,  -1 ,
create hudson-lines  20 , 301 ,  -1 ,
create person-68-lines  2 , 266 ,  -1 ,
create person-69-lines   \ src: D_80104F4C (a tourist)
  1 , 267 ,  6 , 268 ,  9 , 267 ,  14 , 268 ,  17 , 267 ,  22 , 268 ,  25 , 267 ,  30 , 268 ,  33 , 267 ,  38 , 268 ,
  42 , 267 ,  46 , 268 ,  50 , 267 ,  54 , 268 ,  -1 ,
create person-70-lines  3 , 302 ,  10 , 302 ,  17 , 302 ,  24 , 302 ,  31 , 302 ,  38 , 302 ,  45 , 302 ,  -1 ,   \ src: D_80104FA8
create person-72-lines  1 , 303 ,  5 , 303 ,  9 , 303 ,  13 , 303 ,  17 , 303 ,  21 , 303 ,  25 , 303 ,  -1 ,    \ src: D_80104F88
create person-81-lines   \ src: D_80104FC8
  3 , 304 ,  14 , 304 ,  25 , 304 ,  36 , 304 ,  47 , 304 ,  58 , 304 ,  69 , 304 ,
  8 , 304 ,  19 , 304 ,  30 , 304 ,  41 , 304 ,  52 , 304 ,  63 , 304 ,  74 , 304 ,  -1 ,
create person-117-lines  4 , 269 ,  10 , 269 ,  16 , 269 ,  22 , 269 ,  28 , 269 ,  34 , 269 ,  40 , 269 ,  -1 ,   \ src: D_80105004
7 constant usual-gesture   0 constant no-gesture-here
: line-if ( table slot gesture -- handled? )   \ a line by their stop, if they stand at one the table has
    {: t s g :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s g one-liner  true ;
\ the farm workers' lines, by the first one's stop (src: D_80101FB4: 0 none)
create workers-lines
  0 , 0 , 270 , 0 , 271 , 0 , 0 , 272 , 0 , 273 , 0 , 0 , 274 , 0 , 275 , 0 , 0 , 276 , 0 , 277 , 0 , 0 , 278 , 0 ,
  279 , 0 , 0 , 280 , 0 , 281 , 0 , 0 , 282 , 0 , 283 , 0 ,
36 constant #workers-lines
: workers-talk ( slot -- handled? )   \ src: func_80103EA8: with the protest done, on an odd minute, the dial deluge
    {: s :}  farm-worker stop-of {: n :}  n 0< if  false exit  then
    president-protest-seen flag?  clock-time time>minute 1 and and if
        worker-told @ if  ['] events.ovl021:entry-80107404  else  ['] events.ovl021:entry-801073B8  then  start-event
        1 worker-told !  true exit  then
    n #workers-lines < if  n cells workers-lines + @  else  0  then  ?dup 0= if  false exit  then
    s usual-gesture one-liner  true ;
: talks-here ( slot -- handled? )
    case
        mel of  mel-lines mel usual-gesture line-if  endof
        morgan of  morgan-lines morgan usual-gesture line-if  endof
        winona of  winona-lines winona usual-gesture line-if  endof
        cohen of  cohen-lines cohen no-gesture-here line-if  endof
        steve of  steve-lines steve usual-gesture line-if  endof
        hudson of  hudson-lines hudson usual-gesture line-if  endof
        $44 of  person-68-lines $44 usual-gesture line-if  endof
        $45 of  person-69-lines $45 usual-gesture line-if  endof
        $46 of  person-70-lines $46 usual-gesture line-if  endof
        $48 of  person-72-lines $48 usual-gesture line-if  endof
        $51 of  person-81-lines $51 usual-gesture line-if  endof
        $75 of  person-117-lines $75 usual-gesture line-if  endof
        farm-worker of  farm-worker workers-talk  endof
        farm-worker-2 of  farm-worker-2 workers-talk  endof
        farm-worker-3 of  farm-worker-3 workers-talk  endof
        false swap
    endcase ;
' talks-here this-area area-talk!

\ ---- Lorraine's conversations here (0x801043A8): the first that applies, each once; else her line ----
$5071B constant workers-spot-x   $4188B constant workers-spot-z
: workers-here? ( -- flag )  farm-worker person-now 2drop {: x z :}  x workers-spot-x =  z workers-spot-z = and ;  \ src: func_8002D0D0
: dennis-at-74? ( -- flag )  dennis $4A standing-at? ;   \ src: func_8002B1AC
: once ( word -- )  1 swap story+! ;
: talks ( xt -- true )  start-event  true ;
284 constant lorraines-line   \ "Feel like a bite to eat? What can I get you?"
: lorraine-talks ( -- handled? )
    area @ this-area <> if  false exit  then
    lorraine-robbery-confessed flag? 0=  diner-robbery-seen flag? and if
        lorraine-robbery-confessed flag-toggle  ['] events.ovl021:entry-801071D8 talks exit  then
    asked-about-emma story@ 0=  diner-lorraine-intro-seen flag? and  today 29 < and if
        asked-about-emma once  ['] events.ovl021:entry-80106E8C talks exit  then
    protest-told story@ 0=  workers-here? and if
        protest-told once  ['] events.ovl021:entry-80106EBC talks exit  then
    workers-told story@ 0=  president-protest-seen flag? and  dennis-at-74? 0= and if
        workers-told once  ['] events.ovl021:entry-80106F38 talks exit  then
    mel-a-jerk flag? 0=  today 25 = and  hour 16 >= and  mel-here? 0= and if
        mel-a-jerk flag-toggle  ['] events.ovl021:entry-80106F98 talks exit  then
    seen-mel? story@ 0=  today 26 = and  hour 15 < and  mel-here? 0= and if
        seen-mel? once  ['] events.ovl021:entry-80106FE0 talks exit  then
    cant-reach-mel story@ 0=  today 26 = and  hour 17 < and  mel-here? 0= and if
        cant-reach-mel once  ['] events.ovl021:entry-80107028 talks exit  then
    mel-looked-pale story@ 0=  break-in-words story@ 0<> and  roland-break-in-scene-seen flag? and
    today 26 = and  mel-here? 0= and if
        mel-looked-pale once  ['] events.ovl021:entry-80107058 talks exit  then
    break-in-heard flag? 0=  emma-diary-found-missing flag? and if
        break-in-heard flag-set  ['] events.ovl021:entry-80107094 talks exit  then
    startled story@ 0=  today 27 = and if  startled once  ['] events.ovl021:entry-801070E8 talks exit  then
    that-dumb story@ 0=  today 27 = and  mel-here? 0= and if
        that-dumb once  ['] events.ovl021:entry-80107184 talks exit  then
    asked-about-mels-drugs flag? 0=  epoch-analysis-scene-seen flag? and  lorraine-told-on-mel flag? 0= and if
        asked-about-mels-drugs flag-set   \ (and the diner's door opens 15:00 - 19:59 today: ovl027)
        diner-open-day story@ open-day-bits invert and  today 255 and open-day-shift lshift or  diner-open-day story!
        ['] events.ovl021:entry-801072F4 talks exit  then
    lorraines-line rolein usual-gesture one-liner  true ;
' lorraine-talks rolein conversation!

\ ---- the C its events call (ovl021's evfn_*) ----
: mel-goes ( -- )   \ 0x8010497C: after meeting Lorraine, Mel walks his way (src: func_8002E13C; Matthew seated - to come)
    $800870F0 main-word ?dup if  mel start-route  then ;
: to-the-seat ( -- )  0 s" Lorraine to a counter seat" unbuilt ;     \ 0x8010383C (the counter: to come)
: on-the-counter ( -- )  0 s" the order on the counter" unbuilt ;   \ 0x8010389C
this-area ovl $8010497C ' mel-goes native:
this-area ovl $8010383C ' to-the-seat native:   this-area ovl $8010389C ' on-the-counter native:

this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
