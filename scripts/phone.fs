\ phone.fs - the cell phone (docs/subsystems/phone.md; src: ftel.c, cui.c phone_incoming_update): chosen in the
\ ring it rings out - a number is four face buttons (○ 0, ✕ 1, □ 2, △ 3; the first the lowest) - and whoever has
\ that number answers by where they are or by the time, from their pack; a number answered by someone becomes
\ known (the notebook's). The story's calls come in at their times, ringing a while; chosen in the ring then, it
\ is answered. On foot Matthew takes it out, holds it up and puts it away; in the car or on a stool he only dials.
\ The phone's state is its own; others read the facts phone-up?, ringing?, in-call?. The numbers known are the
\ notebook's.
IN: phone
USING: engine actors messages keys screen clock areas figures matthew townsfolk events events.main story-flags
    window ui driving ;

\ ---- facts ----
variable dialling                  \ the dial screen is up
variable calling                   \ who is calling: 0 no one, a story call-number 1 .. 10 ringing, on-the-line
variable in-call                   \ talking on it
255 constant on-the-line
: phone-up? ( -- flag )  dialling @ 0<> ;
: ringing? ( -- flag )  calling @ dup 0<>  swap on-the-line <> and ;
: in-call? ( -- flag )  in-call @ 0<> ;

<PRIVATE
\ ---- the numbers (their lines in the notebook: 063_2) ----
40 constant sheriff   195 constant winonas-house   90 constant general-store   76 constant hospital
121 constant gruber-office   66 constant high-school   240 constant diner   9 constant club-wolves
124 constant cohens-lodge   215 constant james-house   152 constant gruber-house   68 constant emmas-house
149 constant mary-lous-house   205 constant nozz-apartment   199 constant isabellas-apartment
154 constant morgans-house   255 constant auto-garage   242 constant dennis-house   238 constant liz-house
144 constant party-committee   188 constant unlisted-188   82 constant unlisted-82

\ ---- answering: a pack's script; the C's own scripts (src: D_800996F4 ..) ----
: masked ( off mask -- n )  swap story@ and ;
: answered-by ( pack k -- )  swap load-pack start-pack-script ;
: not-here ( -- )  ['] events.main:exe-800996F4 start-event ;      \ (the person is right here)
: wrong-number ( -- )  ['] events.main:exe-80099704 start-event ;
: no-answer ( -- )  ['] events.main:exe-80099718 start-event ;
: calling-from? ( area -- flag )   \ calling from where they are: "right here"
    area @ = dup if  not-here  then ;
: script-xt ( k -- xt | 0 )  loaded-pack @ 64 * + cells pack-scripts + @ ;
5 constant #unanswered   \ a pack's first scripts: no one answering
: someone? ( k -- flag )   \ src: func_8004C680: script k is none of those
    script-xt {: xt :}  #unanswered 0 do  i script-xt xt = if  false unloop exit  then  loop  true ;
: known! ( mask -- )   \ (the notebook's: a bit a number)
    32 0 do  dup 1 i lshift and if  i s" notebook" actor-named send learn-number  then  loop  drop ;

\ the last script each number played (src: D_800F68B0 + $64 ..: the session's): a call-number again plays the other
$64 constant last-calls   6 constant morgans-last   14 constant morgans-nights   22 constant winonas-last
: last@ ( shift -- n )  last-calls session@ swap rshift $FF and ;
: last! ( n shift -- )   {: n sh :}  last-calls session@  $FF sh lshift invert and  n $FF and sh lshift or  last-calls session! ;

\ ---- by the time: a table of (day hour minute script again), the first still to come ----
variable table-addr   variable table-len
5 constant row-bytes
: table! ( addr len -- )  table-len !  table-addr ! ;
: byte ( i -- n )  table-addr @ table-len @ rot exe-u8 ;
: row-day ( r -- n )  row-bytes * byte ;
: row-time ( r -- day hour minute )  row-bytes * {: o :}  o byte  o 1+ byte  o 2 + byte ;
: row-script ( r -- n )  row-bytes * 3 + byte ;
: row-again ( r -- n )  row-bytes * 4 + byte ;
: due-row ( -- r | -1 )   \ the first whose time is still to come
    0  begin  dup row-day while  dup row-time reached? 0= if  exit  then  1+  repeat  drop -1 ;
: play-row ( r last known -- )   \ src: ftel.c call_table
    {: r l k :}  r row-script  dup l session-c@ = if  drop r row-again  then  start-pack-script
    r row-script l session-c!
    k 0<>  r row-script someone? and if  k known!  then ;
: by-the-time ( pack addr len last known -- )   \ (none still to come: no answer at all, as the original)
    {: p a n l k :}  p load-pack  a n table!  due-row dup 0< if  drop exit  then  l k play-row ;

\ ---- where a scheduled person is: their stop * 2, + 1 walking from it ----
: where-now ( slot -- n )  dup last-stop 2*  swap travelling? 1 and + ;
: by-where ( pos last-shift -- )   \ a table by where they are (script, again: the first two)
    {: pos sh :}  pos row-bytes * {: o :}  o byte  dup sh last@ = if  drop o 1+ byte  then  start-pack-script
    o byte sh last! ;

\ ---- Morgan (src: func_8004A600): by where he is; three night calls; the 27th about noon ----
0 constant the-station
270 constant morgan-early   271 constant morgan-midday   272 constant morgan-late   \ (packs: by where he is)
$29 constant midday-from   $6F constant late-from
$3B constant first-night-call   3 constant night-calls   128 constant noon-of-the-27th
$C00 constant robbery-reported   \ (src: +0x1C bits 10, 11)
: morgans-pack ( pos -- pack )
    dup midday-from < if  drop morgan-early exit  then  late-from < if  morgan-midday  else  morgan-late  then ;
: night-call? ( -- flag )   \ 1:00 - 5:59, the first three
    0 1 0 5 59 between? 0= if  false exit  then
    morgans-nights last@ {: c :}  c night-calls < 0= if  false exit  then
    first-night-call c + start-pack-script  c 1+ morgans-nights last!  true ;
: morgans-pos ( -- pos )
    morgan where-now
    27 11 30 12 29 between?  $01C robbery-reported masked 0<> and if  drop noon-of-the-27th  then
    27 12 30 12 59 between? if  drop noon-of-the-27th  then ;
: call-morgan ( -- )
    the-station calling-from? if  exit  then
    morgan where-now morgans-pack load-pack
    night-call? if  exit  then
    s" phone-morgan" table!  morgans-pos morgans-last by-where ;

\ ---- Winona (src: func_8004A810): by where she is, a few times their own ----
18 constant winonas-room   273 constant winona-early   274 constant winona-late   $2D constant late-pos
: winonas-pos ( -- pos )
    winona where-now {: p :}
    p $0E =  27 6 0 23 59 between? and if  $5B to p  then
    p $2C = if
        27 22 0 23 59 between? if  $5C to p  then
        28 0 0 23 59 between? if  $20 to p  then
    then
    p $38 =  p $44 = or  p $50 = or if
        0 18 30 19 59 between? if  $5D to p  then
        0 20 0 23 59 between? if  $5E to p  then
        0 0 0 6 0 between? if  $5F to p  then
    then  p ;
: call-winona ( -- )
    winonas-room calling-from? if  exit  then
    winona where-now late-pos < if  winona-early  else  winona-late  then  load-pack
    s" phone-winona" table!  winonas-pos winonas-last by-where ;

\ ---- the others: by the time, a few their own way ----
1 constant the-store   2 constant the-hospital   3 constant emmas   16 constant the-school   
23 constant cohens   15 constant mary-lous   19 constant nozzs   14 constant lizs   6 constant dennis'   4 constant james'
: call-general-store ( -- )  the-store calling-from? if  exit  then  275 s" calls-general-store" $68 0 by-the-time ;
: call-hospital ( -- )   \ (the 25th's morning: their first answer)
    the-hospital calling-from? if  exit  then
    25 10 0 11 59 between?  $69 session-c@ 0= and if  279 load-pack  $37 start-pack-script  1 $69 session-c! exit  then
    279 s" calls-hospital" $69 0 by-the-time ;
: emmas-pack ( -- pack )  25 0 0 date>time clock-time <=  clock-time 27 5 59 date>time <= and if  286  else  287  then ;
: call-emmas-house ( -- )   \ (the 25th's late morning, Winona there: the next answer)
    emmas calling-from? if  exit  then
    emmas-pack load-pack  s" calls-emmas-house" table!  due-row dup 0< if  drop exit  then
    25 10 0 11 29 between?  winona where-now 1 = and if  1+  then  $6A 0 play-row ;
$20 constant school-known
: call-school ( -- )
    the-school calling-from? if  exit  then
    25 10 0 14 49 between?  $6B session-c@ 0= and if
        288 load-pack  6 start-pack-script  1 $6B session-c!  school-known known! exit  then
    288 s" calls-school" $6B school-known by-the-time ;
: call-diner ( -- )  the-diner calling-from? if  exit  then  278 s" calls-diner" $6C $40 by-the-time ;
0 constant cohens-home   $4F constant cohen-out
: call-cohens-lodge ( -- )
    cohens calling-from? if  exit  then
    cohen cohens-home at-record? 0= if  282 cohen-out answered-by exit  then
    282 s" calls-cohens-lodge" $6D $100 by-the-time ;
: call-mary-lou ( -- )  mary-lous calling-from? if  exit  then  285 s" calls-mary-lou" $6E $1000 by-the-time ;
: call-nozz ( -- )  nozzs calling-from? if  exit  then  284 s" calls-nozz-apartment" $6F $2000 by-the-time ;
: call-morgans-house ( -- )  283 s" calls-morgans-house" $70 $8000 by-the-time ;
: call-liz ( -- )  lizs calling-from? if  exit  then  281 s" calls-liz" $71 $40000 by-the-time ;
$4A constant dennis-out-back   4 constant dennis-busy
: call-dennis ( -- )
    dennis' calling-from? if  exit  then
    dennis stop-of dennis-out-back = if  277 dennis-busy answered-by exit  then
    277 s" calls-dennis" $72 $20000 by-the-time ;
: call-james ( -- )  james' calling-from? if  exit  then  280 s" calls-james" $73 $200 by-the-time ;
: call-unlisted ( -- )   \ src: func_8004B934: by day the first two calls, a third, then another
    290 load-pack
    0 10 0 17 59 between? 0= if  0 start-pack-script exit  then
    $74 session-c@ {: c :}  c 2 > if  2 start-pack-script exit  then
    c 2 = if  1  else  0  then  start-pack-script  c 1+ $74 session-c! ;
$3800 constant gruber-bits   $1800 constant gruber-expects   \ (src: +0x18 bits 11 - 13)
$018 13 story-bit constant gruber-called
: call-gruber-office ( -- )
    area @ content the-hospital <>  $018 gruber-bits masked gruber-expects = and if
        gruber-called flag-set  289 1 answered-by exit  then
    no-answer ;
$01C 24 story-bit constant committee-called   $018 6 story-bit constant committee-heard-of   $80000 constant committee-known
: call-party-committee ( -- )
    committee-called flag? 0=  committee-heard-of flag? and if
        committee-called flag-set  289 0 answered-by  committee-known known! exit  then
    no-answer ;
: call-garage ( -- )   \ (src: D_800F6B14 0 and a way out of his car)
    door-free? if  ['] events.main:exe-80098EE4 start-event  else  no-answer  then ;
: call-number ( number -- )   \ src: func_8004B9E4
    case
        sheriff of  call-morgan  endof          high-school of  call-school  endof
        emmas-house of  call-emmas-house  endof hospital of  call-hospital  endof
        general-store of  call-general-store  endof   cohens-lodge of  call-cohens-lodge  endof
        mary-lous-house of  call-mary-lou  endof  morgans-house of  call-morgans-house  endof
        unlisted-188 of  call-unlisted  endof    winonas-house of  call-winona  endof
        nozz-apartment of  call-nozz  endof      james-house of  call-james  endof
        liz-house of  call-liz  endof            diner of  call-diner  endof
        dennis-house of  call-dennis  endof      gruber-office of  call-gruber-office  endof
        party-committee of  call-party-committee  endof   auto-garage of  call-garage  endof
        club-wolves of  no-answer  endof         unlisted-82 of  no-answer  endof
        gruber-house of  no-answer  endof        isabellas-apartment of  no-answer  endof
        wrong-number
    endcase ;

\ ---- dialling (src: dial_update, func_80049D08, func_8004A10C) ----
enum: dial-step  put-away taking-out keying holding-up ringing-out putting-away ;
variable step
variable plain           \ in the car or on a stool: no taking it out
variable hung-up
create digits 4 cells allot   variable #digits
variable frames-left
fvariable due
$61 constant take-out   $62 constant held   $63 constant put-back   $64 constant hold-up   $65 constant at-his-ear
62 constant ringing-frames   \ src: dial_blink: the caption a while
3 constant ring-out          \ the common set's sequences: ringing out, ringing in
2 constant ring-in
2 constant on-a-stool
: the-sound ( -- id )  s" sound" actor-named ;
: ui-actor ( -- id )  s" ui" actor-named ;
: his-motion ( id loop? -- )  -1 swap matthew-actor send figure-anim ;
: dialled ( -- number )   \ src: the first symbol the lowest
    0  4 0 do  4 * digits 3 i - cells + @ +  loop ;
: open-dial ( -- )
    true dialling !  false hung-up !  0 #digits !
    ui-actor send take-pad  phone-up s" clock" actor-named send hold-clock
    driving?  matthew-perch on-a-stool = or plain !
    plain @ 0= if  false matthew-actor send figure-on-foot  then
    0 s" pictures" actor-named send show-picture
    plain @ if  keying  else  take-out false his-motion  taking-out  then  step ! ;
: close-dial ( -- )
    false dialling !  put-away step !
    ui-actor send give-pad  phone-up s" clock" actor-named send release-clock
    plain @ 0= if  true matthew-actor send figure-on-foot  then
    s" pictures" actor-named send hide-picture
    hung-up @ 0= if  dialled call-number  then ;
: ringing-out! ( -- )  ringing-frames frames-left !  ring-out true the-sound send play-sequence  ringing-out step ! ;
: keyed ( -- )   \ the fourth: it rings out
    plain @ if  ringing-out!  else  hold-up false his-motion  holding-up step !  then ;
: hang-up ( -- )
    true hung-up !  plain @ if  close-dial  else  put-back false his-motion  putting-away step !  then ;
\ the dial-keys (src: the pad's mode 7: a shoulder clears, or hangs up; then ○ □ △ ✕, each with its tone)
variable shoulder-was   variable circle-was   variable cross-was   variable square-was   variable triangle-was
: edge ( flag var -- pressed? )  {: f v :}  f v @ 0= and  f v ! ;
: shoulders ( -- flag )  pad-l1 key-down? pad-l2 key-down? or  pad-r1 key-down? or  pad-r2 key-down? or ;
: key-in ( symbol tone -- )
    the-sound send play-sound  digits #digits @ cells + !  1 #digits +!
    #digits @ 4 = if  keyed  then ;
: dial-keys ( -- )
    shoulders shoulder-was edge  pad-circle key-down? circle-was edge  pad-square key-down? square-was edge
    pad-triangle key-down? triangle-was edge  pad-cross key-down? cross-was edge  {: sh ci sq tr cro :}
    step @ keying <> if  exit  then
    sh if  #digits @ 0= if  hang-up  else  0 #digits !  then  exit  then
    ci if  0 1 key-in exit  then
    sq if  2 4 key-in exit  then
    tr if  3 3 key-in exit  then
    cro if  1 2 key-in  then ;
: dial-frame ( -- )   \ a game frame
    step @ case
        taking-out of  anim-done? if  held true his-motion  keying step !  then  endof
        holding-up of  anim-done? if  ringing-out!  at-his-ear true his-motion  then  endof
        ringing-out of  -1 frames-left +!  frames-left @ 0> 0= if  ring-out the-sound send stop-sequence  close-dial  then  endof
        putting-away of  anim-done? if  0 true his-motion  close-dial  then  endof
    endcase ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;

\ ---- the story's calls (src: phone_incoming_update): who, when ----
$1C4 constant call-days      \ the days of calls 1, 4, 7 (bits 2.., 10.., 18..)
2 constant call-1-day   10 constant call-4-day   18 constant call-7-day
: day-of-call ( shift -- day )  call-days story@ swap rshift $FF and ;
: day-of-call! ( day shift -- )   {: d sh :}  call-days story@  $FF sh lshift invert and  d $FF and sh lshift or  call-days story! ;
: today ( -- day )  clock-time time>day ;
: at? ( day hour minute -- flag )   \ that very minute (day 0: any day; src: event_time_is)
    {: d h m :}  clock-time time>date {: day hour minute :}
    d 0<>  d day <> and if  false exit  then  hour h =  minute m = and ;
13 constant the-club-room   2 constant lake-scene   \ (the lake: the townsfolk's)
6 constant winona-home   15 constant winona-back-home
variable controlled
: call-1? ( -- flag )
    call-1-day day-of-call if  false exit  then
    area @ the-club-room = if  false exit  then
    25 22 5 at? if  true exit  then
    26 22 5 at?  winona stop-of dup winona-home = swap winona-back-home = or and ;
: call-2? ( -- flag )
    $014 story@ 0=  $018 1 story-bit flag? 0= and  $054 story@ 0= and  26 15 32 at? and
    area @ content emmas <> and ;
: call-3? ( -- flag )  $058 story@ 0=  26 5 45 at? and ;
: call-4? ( -- flag )
    controlled @ 0= if  false exit  then
    call-4-day day-of-call today =  if  false exit  then
    $018 $20001080 masked $20000080 <> if  false exit  then
    winona stop-of {: s :}  s $18 =  s $1E = or  s $24 = or ;
: call-5? ( -- flag )  $05C 0 story-bit flag? 0=  28 11 2 at? and ;
$18199 constant lab-x   $57B6A constant lab-z
: call-6? ( -- flag )
    controlled @ 0= if  false exit  then
    samuel lab-x lab-z stands-at?  area @ content the-hospital <> and
    $1E0 story@ $1E4 story@ $1E8 story-c@ reached? and  28 12 0 reached? and ;
: call-7? ( -- flag )  call-7-day day-of-call today =  $018 $14000 masked $4000 = and  0 19 46 at? and ;
0 constant nozz-at-the-store
: call-8? ( -- flag )
    $05C 2 story-bit flag? 0=  31 7 33 at? and  $020 8 story-bit flag? 0= and  james-visiting? and
    $044 story@ 1 and 0<> and  nozz travelling? 0= and  nozz nozz-at-the-store at-record? and ;
$1EC constant call-9-day   $1F0 constant call-9-hour   $1F4 constant call-9-minute
: call-9-due? ( -- flag )  controlled @  call-9-day story@ call-9-hour story@ call-9-minute story-c@ reached? and ;
8 constant isabella-at-the-lake
: call-10? ( -- flag )
    $05C 3 story-bit flag? 0=  area @ the-lake = and  scene @ lake-scene = and
    isabella isabella-at-the-lake at-record? and  $01C story@ 0< and  $00B story-c@ 1 and 0= and ;
: call-9? ( -- flag )   \ (his car nowhere: the call-number; already got through: no more)
    call-9-due? 0= if  false exit  then
    his-car-now drop 0= swap 0= and 0= if  false exit  then
    $020 13 story-bit flag? if  0 call-9-day story!  false exit  then  true ;
: whose-call ( -- n | 0 )
    call-1? if  1 exit  then  call-2? if  2 exit  then  call-3? if  3 exit  then  call-4? if  4 exit  then
    call-5? if  5 exit  then  call-6? if  6 exit  then  call-7? if  7 exit  then  call-8? if  8 exit  then
    call-9-due? if  call-9? if  9  else  0  then  exit  then
    call-10? if  10  else  0  then ;
254 constant rings-for   \ game frames (src: D_800A39A8)
variable ring-frames
: comes-again ( -- )   \ src: func_8004791C: call-number 9 missed comes again 3 minutes on, him out of the car
    driving? if  step-out  then
    clock-time 3 + time>date {: d h m :}  d call-9-day story!  h call-9-hour story!  m call-9-minute story-c! ;
: ring-frame ( -- )   \ a game frame of it ringing
    -1 ring-frames +!  ring-frames @ 0> if  exit  then
    calling @ 9 = if  comes-again  then
    0 calling !  ring-in the-sound send stop-sequence ;
: listen ( -- )   \ a game frame: a call-number coming in, or one ringing on
    ringing? if  ring-frame exit  then
    calling @ in-call? or if  exit  then
    whose-call ?dup if  calling !  rings-for ring-frames !  ring-in false the-sound send play-sequence  then ;

\ ---- answering (src: phone_answer, phone_call_start, phone_call_end) ----
276 constant story-calls   \ their pack
$68 constant to-his-ear   $66 constant listening   $67 constant hanging-up
variable call-thread   -1 call-thread !
$6F constant loretta   $71B85 constant club-bar-x   $4754D constant club-bar-z   $400 constant facing-the-bar
$48 constant at-the-bar   \ (the club: the townsfolk's)
: loretta-at-the-bar ( -- )   \ src: func_8002F030 (with call-number 7)
    loretta club-bar-x 0 club-bar-z set-pos  loretta 0 facing-the-bar 0 set-rot
    loretta the-club 1 set-floor  loretta at-the-bar true anim ;
: line-open ( n -- )   \ the call-number's script (-1: the C's own)
    on-the-line calling !  true in-call !
    to-his-ear listening false matthew-actor send figure-anim
    dup 0< if  drop ['] events.main:exe-800990FC  else  pack-script  then  launch-event call-thread ! ;
: answer ( -- )
    calling @ {: c :}
    c 9 <> if  story-calls load-pack  then
    ring-in the-sound send stop-sequence
    c case
        1 of  today call-1-day day-of-call!  0  endof
        2 of  1 $054 story+!  1  endof
        3 of  1 $058 story+!  2  endof
        4 of  today call-4-day day-of-call!  3  endof
        5 of  $05C 0 story-bit flag-toggle  4  endof
        6 of  0 $1E0 story!  $05C 1 story-bit flag-toggle  5  endof
        7 of  0 call-7-day day-of-call!  $018 16 story-bit flag-toggle  6 line-open  loretta-at-the-bar  exit  endof
        8 of  $05C 2 story-bit flag-toggle  7  endof
        9 of  -1  endof
        10 of  $05C 3 story-bit flag-toggle  8  endof
        drop exit
    endcase  line-open ;
: call-over ( -- )   \ src: func_8004C640
    hanging-up false his-motion  false in-call !  0 calling ! ;
main-exe $8004C640 ' call-over native:
main-exe $8004791C ' comes-again native:   \ (the garage's: it calls back in three minutes)
: watch-call ( -- )   \ the call-number's script over: he puts it away (src: phone_incoming_update's D_800996E0)
    in-call? 0= if  exit  then  call-thread @ event-alive? if  exit  then
    -1 call-thread !  ['] events.main:exe-800996E0 start-event ;

\ ---- drawing the dial ----
-80 constant caption-x   40 constant caption-y   495 constant enter-a-number   496 constant its-ringing
-48 constant digits-x   56 constant digits-y   16 constant digit-size
33 constant common-text
create symbol-images 4 cells allot   symbol-images 4 cells erase   variable blank-image
: ui-picture ( addr len -- image )  $clear  s" ui/" $+  $+  s" .png" $+  $@ image ;
: symbol-image ( symbol -- image )
    {: s :}  symbol-images s cells + @ ?dup if  exit  then
    s case  0 of  s" button-circle"  endof  1 of  s" button-cross"  endof  2 of  s" button-square"  endof
        s" button-triangle" rot  endcase  ui-picture  dup symbol-images s cells + ! ;
: blank ( -- image )  blank-image @ ?dup if  exit  then  s" digit-blank" ui-picture dup blank-image ! ;
: draw-dial ( -- )
    phone-up? 0= if  exit  then
    band
    0 font  0 font-palette  $FFFFFFFF pen-color  scale pen-scale
    common-text  step @ keying = if  enter-a-number  else  its-ringing  then  text-of  caption-x caption-y >screen font-text
    4 0 do
        i #digits @ < if  digits i cells + @ symbol-image  else  blank  then
        digits-x i digit-size * + digits-y digit-size digit-size sprite
    loop ;
' draw-dial on-draw
PRIVATE>

: dial-or-answer ( -- )   \ src: ui_update's choice 0: the ringing one answered (not in the car), else dialling
    ringing? if  driving? 0= if  answer  then  exit  then
    phone-up? in-call? or 0= if  open-dial  then ;

state: phone-state end-state
behaviour the-phone
  on spawned ( -- )  self subscribe tick  self subscribe ring-chosen  self subscribe player-control ;
  on tick ( -- )
    dial-keys  watch-call
    steps-due 0 ?do  phone-up? if  dial-frame  then  listen  loop ;
  on ring-chosen ( item who -- )  0< swap 0= and if  dial-or-answer  then ;
  on player-control ( flag -- )  controlled ! ;
end-behaviour

: spawn-phone ( -- id )  the-phone phone-state s" phone" spawn ;
