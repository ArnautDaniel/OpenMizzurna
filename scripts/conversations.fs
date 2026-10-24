\ conversations.fs - the characters' story conversations and their own lines (docs/subsystems/talk.md; src:
\ char_talk.c's pickers, handlers_chars.c's EM_TALK and EM_EVTALK). A character's picker tries their
\ conversations in order - each once, when its story condition holds (a story word marks it had) - and plays the
\ first from their pack (npc-talk); the areas' own talks come after (their rules). Their own line, when nothing
\ else: walking between stops, the line for the stop they left (the executable's `lines-<name>`); else a line of
\ their own. Loaded before the areas' rules, so the pickers come first in each character's list.
IN: conversations
USING: engine actors messages clock areas story-flags townsfolk events events.main events.ovl027 talk items knock ;

<PRIVATE
\ ---- the story's words as the pickers ask them ----
: unset? ( off -- flag )  story@ 0= ;
: set? ( off -- flag )  story@ 0<> ;
: mark ( off -- )  1 swap story! ;
: count-up ( off -- )  1 swap story+! ;
: bit? ( off bit -- flag )  story-bit flag? ;
: today ( -- day )  clock-time time>day ;
: masked ( off mask -- n )  swap story@ and ;
$28000000 constant robbery-bits   $08000000 constant robbery-told   \ src: +0x18 & 0x28000000 == 0x08000000
: robbery-told? ( -- flag )  $018 robbery-bits masked robbery-told = ;
: talk-it ( slot n -- true )  npc-talk true ;
: item-held? ( item -- flag )  has-item? ;
: give ( item -- )  s" items" actor-named send give-item ;
: start-as ( xt slot -- )   \ a scene of theirs (src: event_start_actor / _talk: they turn to him, stand)
    {: xt s :}  s matthew 0 face  s idle  xt start-event ;

\ ---- Father Barton (src: func_8002B2F0) ----
: barton-talks ( -- handled? )
    $0F0 unset? if  $0F0 mark  father-barton 0 talk-it exit  then
    $0F4 unset?  26 8 0 reached? 0= and if  $0F4 count-up  father-barton 1 talk-it exit  then
    $0F8 unset?  diner-lorraine-intro-seen flag? and if  $0F8 mark  father-barton 2 talk-it exit  then
    $0FC unset?  $150 0 bit? and if  $0FC mark  father-barton 3 talk-it exit  then
    $100 unset?  epoch-analysis-scene-seen flag? and if  $100 mark  father-barton 4 talk-it exit  then
    $104 0 bit? 0=  $094 0 bit? and if  $104 0 story-bit flag-set  father-barton 5 talk-it exit  then
    false ;

\ ---- James (src: func_8002C3E0) ----
26 constant the-forest   $69 constant james-in-the-forest   2 constant the-hospital
: james-talks ( -- handled? )
    area @ the-forest = if  james-in-the-forest james 0 one-liner  true exit  then
    $188 unset?  town-meeting-seen flag?  $0A0 set? or and if  $188 mark  james 0 talk-it exit  then
    $18C unset?  epoch-analysis-scene-seen flag? and if  $18C mark  james 1 talk-it exit  then
    $190 unset?  forest-meeting-seen flag? and if  $190 mark  james 2 talk-it exit  then
    $194 0 bit? 0=  $190 set? and if  $194 0 story-bit flag-set  james 3 talk-it exit  then
    $170 unset?  $01C $11000 masked $11000 = and if  $170 mark  james 4 talk-it exit  then
    $174 unset?  cohen-found-scene-seen flag? and if  $174 mark  james 5 talk-it exit  then
    $19C unset?  gave-cohen-envelope flag? and  morgan-necklace-talk-done flag? 0= and if  $19C mark  james 6 talk-it exit  then
    $178 0 bit? 0=  $09C 1 bit? and  area @ content the-hospital = and if  $178 0 story-bit flag-toggle  james 7 talk-it exit  then
    $1A4 0 bit? 0=  $020 4 bit? and if  $1A4 0 story-bit flag-set  james 8 talk-it exit  then
    false ;

\ ---- Liz (src: actor_handler_lidds's EM_EVTALK) ----
: liz-talks ( -- handled? )
    $070 unset?  $0A4 set? and if  $070 mark  lidds 0 talk-it exit  then
    $074 unset?  gave-barbara-cigarettes flag? and if  $074 mark  lidds 1 talk-it exit  then
    false ;

\ ---- Mel (src: func_8002DB7C) ----
3 constant mel-place-3   3 constant the-junkyard-on-the-map
: mel-talks ( -- handled? )
    $120 0 bit? 0=  seat-searched flag? 0= and  isabella-room-locked-in flag? and if
        $120 0 story-bit flag-set  mel 2 npc-talk  the-junkyard-on-the-map map-reveal  true exit  then
    $11C unset?  $008 20 bit? and if  $11C mark  mel 0 talk-it exit  then
    $120 2 bit? 0=  mel mel-place-3 at-record? and  kathy-funeral-seen flag? and if
        $120 2 story-bit flag-toggle  mel 1 talk-it exit  then
    false ;

\ ---- Morgan (src: morgan_talk_pick) ----
$3CA38 constant morgans-desk-x   $20023 constant morgans-desk-z
$3CA10 constant statement-x   $168 negate constant statement-y   $1FECF constant statement-z
14 constant the-photo   13 constant the-statement
: evening-talks ( -- handled? )   \ 12/25 from 16:30
    25 16 30 23 59 between? 0= if  false exit  then
    $05C 12 bit? 0= if  $05C 12 story-bit flag-toggle  morgan 12 talk-it exit  then
    $05C $6000 masked $2000 = if  $05C 14 story-bit flag-toggle  morgan 11 talk-it exit  then
    false ;
: statement ( -- handled? )   \ the photo in hand: he takes Matthew's statement
    $010 9 bit?  the-photo item-held? 0= or  morgan travelling? or if  false exit  then
    morgan person-now drop nip  morgans-desk-z = swap morgans-desk-x = and if
        morgan statement-x statement-y statement-z set-pos  morgan idle  then
    $010 9 story-bit flag-toggle  the-statement give  $02C mark
    ['] exe-80099350 morgan start-as  true ;
: morgan-talks ( -- handled? )
    evening-talks if  true exit  then
    $140 unset?  diner-lorraine-intro-seen flag? and if  $140 count-up  morgan 0 talk-it exit  then
    $144 unset?  town-meeting-seen flag? and if  $144 count-up  morgan 1 talk-it exit  then
    $148 unset?  today 27 = and if  $148 count-up  morgan 2 talk-it exit  then
    $14C unset?  $010 5 bit? and if  $14C count-up  morgan 3 talk-it exit  then
    $150 0 bit? 0=  $028 set? and  26 15 0 reached? and if  $150 0 story-bit flag-toggle  morgan 4 talk-it exit  then
    $154 unset?  $038 set? and if  $154 count-up  morgan 5 talk-it exit  then
    $15C unset?  robbery-told? and if  $15C count-up  morgan 7 talk-it exit  then
    $160 unset?  $1C0 set? and if  $160 count-up  morgan 8 talk-it exit  then
    $164 0 bit? 0=  gave-cohen-envelope flag? and if  $164 0 story-bit flag-toggle  morgan 9 talk-it exit  then
    $164 1 bit? 0=  $044 0 bit? and  $020 18 bit? and if  $164 1 story-bit flag-toggle  morgan 10 talk-it exit  then
    statement if  true exit  then
    $158 unset?  $008 12 bit? and if  $158 count-up  morgan 6 talk-it exit  then
    false ;

\ ---- Robert (src: actor_handler_robert's EM_EVTALK) ----
: robert-talks ( -- handled? )
    $114 unset? if  $114 count-up  robert 0 talk-it exit  then
    $118 0 bit? 0=  $128 set? and if  $118 0 story-bit flag-toggle  robert 1 talk-it exit  then
    false ;

\ ---- Samuel (src: func_8003121C) ----
: samuel-talks ( -- handled? )
    epoch-analysis-scene-seen flag? 0=  $05C 1 bit? and if  samuel 4 talk-it exit  then
    $0A0 unset? if  $0A0 mark  samuel 0 talk-it exit  then
    town-meeting-seen flag?  $0A4 unset? and if  $0A4 mark  samuel 1 talk-it exit  then
    $0A8 unset? if  $000 4 bit? 0= if  false exit  then  $0A8 mark  samuel 2 talk-it exit  then
    $0AC 0 bit? 0= if  $0AC 0 story-bit flag-set  samuel 3 talk-it exit  then
    false ;

\ ---- Winona (src: func_80032424) ----
$2755 constant winona-busy   1 constant plain-gesture   12 constant her-break-in-stop
10 constant morgan-place-10   5 constant hudson-place-5   0 constant steve-place-0   12 constant the-locker-key
: break-in? ( -- flag )   \ src: her stop 12, the three where they'd be
    winona last-stop her-break-in-stop =  $014 unset? and  roland-break-in-scene-seen flag? 0= and
    morgan morgan-place-10 at-record? and  hudson hudson-place-5 at-record? and  steve steve-place-0 at-record? and ;
: winona-talks ( -- handled? )
    winona off-schedule? if  winona-busy winona plain-gesture one-liner  true exit  then
    break-in? if  $014 count-up  ['] entry-8010B864 start-event  true exit  then
    $124 unset? if  $124 mark  winona 0 talk-it exit  then
    $128 unset? if  $128 mark  winona 1 talk-it exit  then
    winona travelling? 0=  $008 1 bit? and if
        ['] exe-80099360 winona start-as  the-locker-key give  $008 1 story-bit flag-clear  true exit  then
    $12C unset?  $010 $50 masked $40 = and if  $12C mark  winona 2 talk-it exit  then
    $130 unset?  robbery-told? and if  $130 mark  winona 3 talk-it exit  then
    $134 0 bit? 0=  $01C 17 bit? and if  $134 0 story-bit flag-set  winona 4 talk-it exit  then
    false ;

\ ---- Nozz (src: func_800266DC): the 25th's evening ----
: nozz-talks ( -- handled? )
    25 16 30 23 59 between? 0= if  false exit  then
    $05C 13 bit? 0= if  nozz 0 npc-talk  $05C 13 story-bit flag-set  true exit  then
    $05C 14 bit? 0= if  nozz 1 talk-it exit  then   \ (its script marks it)
    $05C 15 bit? 0= if  nozz 2 npc-talk  $05C 15 story-bit flag-set  true exit  then
    false ;

\ ---- Cohen (src: func_8002A3FC) ----
8 constant cohens-hunt-stop   $2B constant cohen-on-the-hunt   0 constant cohens-home
$31 constant first-away-stop   9 constant away-stops
: cohen-talks ( -- handled? )
    cohen last-stop cohens-hunt-stop = if  cohen-on-the-hunt cohen 0 one-liner  true exit  then
    $010 5 bit? 0=  today 25 = and  cohen cohens-home at-record? and  area @ the-town = and if
        knock-reset  $010 5 story-bit flag-set  ['] entry-8010B770 start-event  true exit  then
    cohen last-stop first-away-stop - away-stops u< if  false exit  then
    $098 unset? if  $098 mark  cohen 0 talk-it exit  then
    $09C 0 bit? 0=  $194 0 bit? and if  $09C 0 story-bit flag-set  cohen 1 talk-it exit  then
    $09C 2 bit? 0=  $01C 17 bit? and  $034 set? and if  $09C 2 story-bit flag-set  cohen 2 talk-it exit  then
    false ;

\ ---- Dennis (src: func_8002AC00): at the farm house ----
0 constant dennis-at-home
: dennis-talks ( -- handled? )
    $104 2 bit? 0=  robbery-told? and if  $104 2 story-bit flag-set  dennis 0 talk-it exit  then
    dennis dennis-at-home at-record? 0= if  false exit  then
    $108 unset? if  $108 count-up  dennis 1 talk-it exit  then
    $10C unset?  $0C4 set? and if  $10C count-up  dennis 2 talk-it exit  then
    $110 0 bit? 0=  $014 set? and if  $110 0 story-bit flag-set  dennis 3 talk-it exit  then
    false ;

\ ---- their own lines (src: the handlers' EM_TALK) ----
7 constant usual-gesture
$2910 constant hmm   $2911 constant hmm-2   $2912 constant hmm-3   $2913 constant hmm-4   $2914 constant hmm-mel
$2916 constant hmm-winona
: stop-line ( slot table len -- msg | 0 )   \ walking: the line for the stop they left
    {: s t n :}  s travelling? 0= if  0 exit  then  t n s last-stop exe-s16 ;
: own-line ( slot table len fallback speaker gesture -- )   \ the stop's line, or the fallback
    {: s t n fb sp g :}  s t n stop-line ?dup if  s usual-gesture one-liner exit  then  fb sp g one-liner ;
: morgans-line ( slot -- )  s" lines-morgan" hmm morgan usual-gesture own-line ;
21 constant hudsons-stops
: hudsons-line ( slot -- )
    dup last-stop hudsons-stops < if  s" lines-hudson" hmm hudson plain-gesture own-line  else  drop hmm hudson plain-gesture one-liner  then ;
: dennis-line ( slot -- )  s" lines-dennis" hmm-4 dennis plain-gesture own-line ;
: steves-line ( slot -- )  s" lines-steve" hmm steve plain-gesture own-line ;
: isabellas-line ( slot -- )  s" lines-isabella" hmm-2 isabella plain-gesture own-line ;
: nozz-line ( slot -- )  s" lines-nozz" hmm nozz plain-gesture own-line ;
: bartons-line ( slot -- )  s" lines-barton" hmm-3 father-barton usual-gesture own-line ;
4 constant roberts-gesture
: walking-line ( slot table len -- msg )   \ the stop's line, even none
    {: s t n :}  t n s last-stop exe-s16 ;
: roberts-line ( slot -- )   \ (walking: the stop's line, even none)
    dup travelling? if  s" lines-robert" walking-line  else  drop hmm  then  robert roberts-gesture one-liner ;
: liz-line ( slot -- )  s" lines-liz" hmm-2 lidds usual-gesture own-line ;
: cohens-line ( slot -- )  s" lines-cohen" hmm cohen plain-gesture own-line ;
5 constant mels-gesture
: mels-line ( slot -- )  s" lines-mel" hmm-mel mel mels-gesture own-line ;
: james-line ( slot -- )  s" lines-james" hmm james plain-gesture own-line ;
: wolfs-line ( slot -- )  s" lines-wolf" hmm wolf usual-gesture own-line ;
: lorraines-line ( slot -- )
    dup travelling? if  s" lines-lorraine" walking-line rolein usual-gesture one-liner exit  then
    drop hmm-2 rolein plain-gesture one-liner ;
\ Samuel: a table of stops and lines (the fallback credited to James, as the original)
: samuels-line ( slot -- )
    {: s :}  s travelling? if  -1  64 0 do  s" lines-samuel" i 2* exe-s16 dup -1 = if  drop leave  then
        s last-stop = if  drop s" lines-samuel" i 2* 1+ exe-s16 leave  then  loop
    else  -1  then
    dup -1 <> if  samuel usual-gesture one-liner exit  then  drop  hmm-4 james plain-gesture one-liner ;
\ Winona: at her first stop on the 25th her first meetings; else her lines
$2745 constant winona-first   variable winona-met-once
: winonas-line ( slot -- )
    {: s :}  s last-stop 0= if
        25 10 1 13 59 between? if
            winona-met-once @ if  ['] exe-80092120  else  true winona-met-once !  ['] exe-80092104  then  start-event exit  then
        winona-first winona plain-gesture one-liner exit  then
    s s" lines-winona" hmm-winona winona plain-gesture own-line ;
\ ---- the townspeople (src: func_800300E4, the shared handler's EM_TALK): walking, a line by their kind (their
\ schedule's) and the stop they left; else, or none for it, the passer-by's ----
$2724 constant passer-by   $41 constant first-people   $71 constant later-people   24 constant later-kinds-from
$50 constant kind-15-speaker   0 constant still-gesture   4 constant small-gesture
$285A constant kind-21-even   $285B constant kind-21-odd   $28D9 constant kind-16-line   $28DA constant kind-16-after
11 constant kind-16-round
: kind-of-person ( slot -- kind )   \ (src: ovl033's slots by kind: $41.. kinds 0 - 23, $71.. the rest)
    dup later-people >= if  later-people - later-kinds-from +  else  first-people -  then ;
: townsperson? ( slot -- flag )
    dup first-people dup later-kinds-from + within  swap later-people dup 12 + within or ;
: pair-line ( addr len stop -- msg | -1 )   \ a table of (stop, message) pairs ended by -1 (src: func_800429C8)
    {: t n st :}  -1  64 0 do
        t n i 2* exe-s16 {: k :}  k -1 = if  leave  then
        k st = if  drop  t n i 2* 1+ exe-s16  leave  then  loop ;
: by-pairs ( addr len stop -- msg )  pair-line dup -1 = if  drop passer-by  then ;
: by-stop ( addr len stop -- msg )  exe-s16 ?dup 0= if  passer-by  then ;
: paired-table ( kind -- addr len | 0 0 )
    case  3 of  s" people-lines-3"  endof   28 of  s" people-lines-28"  endof   19 of  s" people-lines-19"  endof
        6 of  s" people-lines-6"  endof   18 of  s" people-lines-18"  endof   5 of  s" people-lines-5"  endof
        7 of  s" people-lines-7"  endof   0 0 rot  endcase ;
: direct-table ( kind -- addr len | 0 0 )
    case  4 of  s" people-lines-4"  endof   30 of  s" people-lines-30"  endof   0 0 rot  endcase ;
: kind-line ( slot kind stop -- msg speaker gesture )
    {: s k st :}
    k 21 = if  st 1 and if  kind-21-odd s small-gesture  else  kind-21-even s plain-gesture  then  exit  then
    k 15 = if  s" people-lines-15" st pair-line dup -1 = if  drop passer-by s usual-gesture
        else  kind-15-speaker still-gesture  then  exit  then
    k 16 = if  st kind-16-round mod 1 = if  kind-16-after s plain-gesture  else  kind-16-line s usual-gesture  then  exit  then
    k paired-table ?dup if  st by-pairs s usual-gesture exit  then  drop
    k direct-table ?dup if  st by-stop s usual-gesture exit  then  drop
    passer-by s usual-gesture ;
: workers-own-line ( slot -- )   \ src: func_8002D10C (the farm workers, 132 - 134)
    {: s :}  s travelling? if  s" lines-workers" s last-stop exe-s16 if
        s" lines-workers" s last-stop 5 mod exe-s16 s usual-gesture one-liner exit  then  then
    passer-by s usual-gesture one-liner ;
: townsperson-line ( slot -- )
    {: s :}  s travelling? 0= if  passer-by s usual-gesture one-liner exit  then
    s  s kind-of-person  s last-stop  kind-line one-liner ;
PRIVATE>

\ ---- who has which ----
' barton-talks father-barton conversation!   ' james-talks james conversation!   ' liz-talks lidds conversation!
' mel-talks mel conversation!   ' morgan-talks morgan conversation!   ' robert-talks robert conversation!
' samuel-talks samuel conversation!   ' winona-talks winona conversation!   ' nozz-talks nozz conversation!
' cohen-talks cohen conversation!   ' dennis-talks dennis conversation!
' morgans-line morgan default-line!   ' hudsons-line hudson default-line!   ' dennis-line dennis default-line!
' steves-line steve default-line!   ' isabellas-line isabella default-line!   ' nozz-line nozz default-line!
' bartons-line father-barton default-line!   ' roberts-line robert default-line!   ' liz-line lidds default-line!
' cohens-line cohen default-line!   ' mels-line mel default-line!   ' james-line james default-line!
' wolfs-line wolf default-line!   ' lorraines-line rolein default-line!   ' samuels-line samuel default-line!
' winonas-line winona default-line!
: people-lines ( -- )  #slots 0 do  i townsperson? if  ['] townsperson-line i default-line!  then  loop ;
people-lines
' workers-own-line 132 default-line!   ' workers-own-line 133 default-line!   ' workers-own-line 134 default-line!

\ ---- the C their scenes call (src: char_talk.c) ----
: james-off ( -- )  ['] events.main:route-800858AC james start-route ;   \ src: evfn_8002CC24: his own walk
: liz-off ( -- )  ['] events.main:route-80086EBC lidds start-route ;     \ src: evfn_8002D5F4
10 constant pocket-camera   $1C8 constant film-day   $1CC constant film-hour   $1D0 constant film-minute
: camera-handed-in ( -- )   \ src: func_8002E988: the photos due an hour on
    pocket-camera s" items" actor-named send take-item
    clock-time minutes/hour + time>date {: d h m :}  d film-day story!  h film-hour story!  m film-minute story-c! ;
$018 27 story-bit constant winona-photo-asked   \ (src: evfn_80030F84 reads +0x18 bit 27)
: photo-asked ( -- n )  winona-photo-asked flag? 1 and ;
18 constant winonas-place
: winona-off ( -- )   \ src: evfn_80032CE4: her place on the map; she sets off if standing
    winonas-place map-reveal  winona travelling? 0= if  winona s" townsfolk" actor-named send person-depart  then ;
main-exe $8002CC24 ' james-off native:   main-exe $8002D5F4 ' liz-off native:
main-exe $8002E988 ' camera-handed-in native:   main-exe $80030F84 ' photo-asked native:
main-exe $80032CE4 ' winona-off native:
