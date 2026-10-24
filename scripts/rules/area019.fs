\ area019.fs - area 19's rules: the apartment building (src: ovl019.c; decomp/docs/areas/area19.md;
\ port/src/game/ovl019.c); areas 31 (its ground floor) and 32 (the air duct) are its too. Its crate (crate.fs),
\ the photo on the shelf (□), the doors to knock at (□), the keyhole and the doors (○), the mailboxes, the air
\ duct (the crate under it: the crawl, minigame 41), the way out of the far room (the chase, minigame 22), two
\ rooms entered by where he walks, who says what, the talks here.
IN: rules.area019
USING: engine actors messages clock story-flags items townsfolk events look talk areas matthew crate knock ;

19 constant this-area
16 constant the-crate-slot   164 constant the-photo   9 constant the-lighter   7 constant the-tape
create the-crate  crate-place allot
the-crate the-crate-slot $4B935 $4B0 negate $4ADFC this-area 2 crate!

$044 0 story-bit constant her-room-open        \ (src: +0x44 bit 0: her door open, no one in)
$01C 6 story-bit constant photo-first-seen     \ (set by the photo, actor 164's handler)
$01C 7 story-bit constant photo-second-seen
$008 9 story-bit constant letter-waiting       \ (area 1's mailbox then)
2 constant wolf-away-record   8 constant shut-in-record   \ (their schedule records: src func_80033138, func_8002C250)
2 constant answers-at-time    \ (minutes before she leaves, at the least)

\ ---- this visit (src: D_8010662C the mailboxes read, D_80106634 in by the keyhole, D_80106630 the photo's talk
\ done, D_80106638 the duct cover's lines) ----
variable mailbox-read   variable by-the-keyhole   variable photo-talk-done   variable cover-lines

$4AC00 constant photo-x   $5AA negate constant photo-y   $4B264 constant photo-z
: entered ( -- )   \ src: func_801023B8
    10 map-reveal
    the-photo this-area 2 set-floor  the-photo photo-x photo-y photo-z set-pos  the-photo $400 $3F8 negate 0 set-rot
    0 mailbox-read !  0 by-the-keyhole !  0 photo-talk-done !  0 cover-lines !  the-crate forget-crate ;

\ ---- her room's two sets of things, by the story (src: func_8010310C) ----
4 constant her-room   $19 constant her-room-then   $1A constant her-room-now
: her-room-things ( -- )
    scene @ her-room <> if  exit  then  cell-occupied flag? {: now? :}
    her-room-then now? 0= s" areas" actor-named send object-shown  her-room-now now? s" areas" actor-named send object-shown ;
: scene-hook ( scene -- )  drop her-room-things ;

\ ---- each frame (0x80102A94): the crate; walking into the zones of two rooms shows them ----
$4D constant empty-room-zone   $4E constant far-room-zone   6 constant empty-room   7 constant far-room
: room-by-zone ( -- )
    area @ this-area <> if  exit  then
    at-zone nip case  empty-room-zone of  empty-room  endof  far-room-zone of  far-room  endof  drop exit  endcase
    dup scene @ = if  drop exit  then  s" areas" actor-named send show-scene ;
: each-frame ( -- )  area @ this-area = if  the-crate crate-step  then  room-by-zone ;

\ ---- ○ (0x80102450 / 0x801022AC): her door (the keyhole, or open), a talk's after-scene, the mailboxes ----
$46 constant her-door   $4F constant after-the-talk   7 constant the-mailboxes   $40 constant his-door
enum: look-action  nothing-here  the-keyhole  the-mistake  3 unused-3  4 unused-4  read-the-mailboxes  into-her-room ;
: keyhole-now? ( -- flag )   \ his knock heard, the room unseen; she's home, not leaving soon; the other away
    $01C story@ $30 and $10 =  isabella at-home at-record? and  isabella minutes-to-go answers-at-time >= and
    wolf wolf-away-record at-record? 0= and ;
: look-check ( zone check -- action )
    nip case
        her-door of  keyhole-now? if  the-keyhole  else  her-room-open flag? if  into-her-room  else  nothing-here  then  then  endof
        after-the-talk of  photo-talk-done @ if  the-mistake  else  nothing-here  then  endof
        the-mailboxes of  read-the-mailboxes  endof
        nothing-here swap
    endcase ;
: the-mistake-scene ( -- )   \ 0x80102744
    photo-talk-done @ 0= if  exit  then
    isabella-mistake-scene-seen flag-toggle  ['] events.ovl019:entry-80104DC8 start-event  event-unlock ;
: look-do ( action -- )
    case
        the-keyhole of
            isabellas-room-seen flag-toggle  ['] events.ovl019:entry-80103D1C start-event  event-lock  1 by-the-keyhole !  endof
        the-mistake of  the-mistake-scene  endof
        read-the-mailboxes of
            mailbox-read @ if  ['] events.ovl019:entry-80106460  else  1 mailbox-read !  ['] events.ovl019:entry-80106428  then
            start-event  endof
        into-her-room of  ['] events.ovl019:entry-801060D4 start-event  endof
        drop run-rows 0
    endcase ;

\ ---- □ (0x8010255C / 0x801025D0): knocking at his door and hers ----
enum: act-action  nothing-to-do  3 knock-his  knock-hers ;
: act-check ( zone check -- action )
    nip knock-idle? 0= if  drop nothing-to-do exit  then
    case  his-door of  knock-his  endof  her-door of  knock-hers  endof  nothing-to-do swap  endcase ;
: his-knock ( -- )   \ once, late, when he's in: his own scene; else he answers
    $4B knock ?dup 0= if  exit  then {: who :}
    uncle-night-knock-seen flag? 0=  isabella-unsafe-seen flag? and  nozz nozz-in-his-room at-record? and if
        uncle-night-knock-seen flag-toggle  ['] events.ovl019:entry-801039DC start-event exit  then
    who 1 $4B answer ;
: act-do ( action -- )
    case
        knock-his of  his-knock  endof
        knock-hers of  $4A knock ?dup if  4 $4A answer  then  endof
        drop run-rows 0
    endcase ;

\ ---- the crate: ○ takes hold, □ climbs; the photo: □ (src: actor_handler_164) ----
: push-it ( slot -- true )  drop the-crate crate-push  true ;
: climb-it ( slot -- true )  drop the-crate crate-climb  true ;
' push-it the-crate-slot examined-by!   ' climb-it the-crate-slot taken-by!
: photo-now? ( -- flag )   \ (src: EM_CHECKVAR 4)
    her-room-open flag? 0=  $01C story@ dup $60 and $20 =  swap $C0 and $40 = or and ;
: take-the-photo ( slot -- handled? )
    drop photo-now? 0= if  false exit  then
    photo-first-seen flag? 0= if  photo-first-seen flag-set  ['] events.ovl019:entry-801042AC start-event  true exit  then
    photo-second-seen flag? 0= if
        photo-second-seen flag-set  ['] events.ovl019:entry-80104B54 start-event  1 photo-talk-done !  then
    true ;
' take-the-photo the-photo taken-by!

\ ---- talking here (0x80102B0C) and the talk hooks (0x801026E8, 0x80102BBC) ----
create dennis-lines  12 , 222 ,  19 , 223 ,  24 , 224 ,  30 , 225 ,  36 , 226 ,  40 , 227 ,  42 , 228 ,  52 , 229 ,
  60 , 230 ,  68 , 231 ,  70 , 232 ,  -1 ,
$2725 constant her-silence   7 constant usual-gesture
: talks-here ( slot -- handled? )
    case
        isabella of  isabella person-of travelling @ if  false exit  then
            her-silence isabella in-their-place one-liner  true  endof
        dennis of  dennis person-of travelling @ if  true exit  then
            dennis-lines dennis person-of at-stop @ a-line-by-stop  dup 0< if  drop  else  dennis usual-gesture one-liner  then
            true  endof
        false swap
    endcase ;
' talks-here this-area area-talk!
: her-plea ( -- handled? )   \ after the photo's talk, this visit
    area @ content this-area <> if  false exit  then
    $01C story@ $180 and $80 =  photo-talk-done @ and if  ['] events.ovl019:entry-80104D88 start-event  true exit  then
    false ;
' her-plea isabella conversation!
: his-hint ( -- handled? )   \ once, after her door went unanswered
    area @ content this-area <>  nozz-air-duct-hint-heard flag? or  isabella-no-response-seen flag? 0= or if  false exit  then
    nozz-air-duct-hint-heard flag-set  ['] events.ovl019:entry-80105A58 start-event  true ;
' his-hint nozz conversation!

\ ---- the air duct: the crawl (minigame 41: the pad drives him; src: area_minigame_update's steps 5 and 6) ----
$138 negate constant crawl-look-y   $38C negate constant crawl-dist   $208 negate constant crawl-eye-y
: duct-step ( -- )   \ 0x80102234: the crawl on (the camera low behind him); again, off (the camera as it is)
    matthew-crawling? if  false s" matthew" actor-named send figure-crawl exit  then
    true s" matthew" actor-named send figure-crawl
    crawl-look-y crawl-dist crawl-eye-y s" camera" actor-named send camera-behind ;
: the-duct ( -- )   \ 0x801027A8: with his hint and her shut in, the other away, and a light: in
    crawled-air-duct flag?  nozz-air-duct-hint-heard flag? 0= or
    isabella shut-in-record at-record? 0= or  wolf wolf-away-record at-record? or if
        ['] events.ovl019:entry-801037A4 start-event exit  then
    the-lighter has-item? 0= if  ['] events.ovl019:entry-8010369C start-event exit  then
    crawled-air-duct flag-toggle  the-crate crate-cancel  ['] events.ovl019:entry-801036FC start-event  event-lock ;
create cover-scenes  ' events.ovl019:entry-801037D0 , ' events.ovl019:entry-801037E4 , ' events.ovl019:entry-801037F8 ,
2 constant last-cover-line
: the-cover ( -- )   \ 0x80102888: three lines, the last again
    cover-lines @ cells cover-scenes + @ start-event
    cover-lines @ last-cover-line < if  1 cover-lines +!  then ;
: out-of-her-room ( -- )   \ 0x80102934: open; or in by the duct: out, and the chase; or locked
    her-room-open flag? if  ['] events.ovl019:entry-80106110 start-event exit  then
    by-the-keyhole @ 0= if  isabella-room-locked-in flag-toggle  event-unlock  ['] events.ovl019:entry-80103804 start-event exit  then
    ['] events.ovl019:entry-80103834 start-event ;
$4AEF4 constant left-x   $4B0 negate constant left-y   $4AA04 constant left-z   181 constant what-was-left
: the-far-room ( -- )   \ 0x80102A04: once, after the crawl
    isabella-cabin-photo-seen flag?  crawled-air-duct flag? 0= or if  exit  then
    isabella-cabin-photo-seen flag-set  what-was-left left-x left-y left-z set-pos
    ['] events.ovl019:entry-8010385C start-event ;
: the-tape-given ( -- )  the-tape s" items" actor-named send give-item ;   \ 0x801029C8
: a-letter-waits ( -- )  letter-waiting flag-set ;                        \ 0x801029E8
: the-photo-talked ( -- )  1 photo-talk-done ! ;                           \ 0x80102BAC
this-area ovl $80102234 ' duct-step native:   this-area ovl $801027A8 ' the-duct native:
this-area ovl $80102888 ' the-cover native:   this-area ovl $80102934 ' out-of-her-room native:
this-area ovl $80102A04 ' the-far-room native:   this-area ovl $801029C8 ' the-tape-given native:
this-area ovl $801029E8 ' a-letter-waits native:   this-area ovl $80102BAC ' the-photo-talked native:

\ ---- the chase (minigame 22): its scene, then the main executable's (23: out to the town) ----
: the-chase ( -- )  events.ovl019:entry-80103260  23 minigame ;
' the-chase 22 minigame-script!   ' events.ovl019:entry-801034A0 41 minigame-script!

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
