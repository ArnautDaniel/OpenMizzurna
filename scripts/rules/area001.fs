\ area001.fs - area 1's rules: Captain Nozz's General Goods and Matthew's room (src: ovl001.c, the area
\ overlay; decomp/docs/areas/area01.md). What ○ and □ do here beyond the tables' rows, the area's and the
\ scene's hooks, and the C its events call.
IN: rules.area001
USING: engine actors messages clock story-flags items townsfolk floors events look talk ;

1 constant this-area
$008 9 story-bit constant letter-waiting   \ (not named yet: lets the letter be found - src: D_800F6930 & 0x200)

\ ---- the events (ovl001's scripts) ----
\ firsts: the first look at something says one thing, the later ones another (src: once_then)
variable shelves-seen   variable goods-seen   variable tools-seen   variable today-seen
variable cigarettes-asked   variable lighter-seen   variable letter-found   variable flyer-seen
: once-then ( var first again -- )  {: v f a :}  v @ if  a  else  f  1 v !  then  start-event ;
: forget-firsts ( -- )   \ src: the area's entering (0x80103080)
    0 shelves-seen !  0 goods-seen !  0 tools-seen !  0 today-seen !  0 letter-found !  0 flyer-seen ! ;

\ ---- ○: the look check (0x80102564) and its actions (0x8010270C) ----
enum: look-action  nothing-here  2 photo  5 shelves goods nozz-at-counter radio fishing-tools  11 bulletin-board cigarettes-note
  14 the-lighter  16 the-letter  18 garage-flyer ;
: look-check ( zone check -- action )
    case
        13 of  drop bulletin-board exit  endof
        16 of  drop  took-lighter flag? if  nothing-here  else  the-lighter  then  exit  endof
        17 of  drop  got-barbaras-cigarettes story@ 0=  prologue-done flag? and if  cigarettes-note  else  nothing-here  then
            exit  endof
        18 of  drop radio exit  endof
        19 of  drop fishing-tools exit  endof
    endcase
    case
        3 of  shelves  endof
        8 of  prologue-done flag?  read-garage-flyer flag? 0= and if  garage-flyer
            else  got-isabella-letter flag? if  nothing-here  else  the-letter  then  then  endof
        21 of  goods  endof
        24 of  photo  endof
        25 of  nozz-behind-counter? if  nozz-at-counter  else  nothing-here  then  endof
        nothing-here swap
    endcase ;
: look-do ( action -- )
    case
        photo of  ['] events.ovl001:entry-801051A8 start-event  endof
        shelves of  shelves-seen ['] events.ovl001:entry-801055F4 ['] events.ovl001:entry-80105604 once-then  endof
        goods of  goods-seen ['] events.ovl001:entry-80105614 ['] events.ovl001:entry-80105624 once-then  endof
        nozz-at-counter of  nozz talk-to  endof   \ (src: actor_talk_start)
        radio of  ['] events.ovl001:entry-801053A0 start-event  endof
        fishing-tools of  tools-seen ['] events.ovl001:entry-80105644 ['] events.ovl001:entry-80105654 once-then  endof
        bulletin-board of  ['] events.ovl001:entry-80105634 start-event  endof
        cigarettes-note of  ['] events.ovl001:entry-801055E4 start-event  1 cigarettes-asked !  endof
        the-lighter of  ['] events.ovl001:entry-80105684 start-event  1 lighter-seen !  endof
        the-letter of
            letter-waiting flag?  isabellas-letter has-item? 0= and if
                ['] events.ovl001:entry-80105470 start-event  1 letter-found !
            else  ['] events.ovl001:entry-80105458 start-event  then  endof
        garage-flyer of  ['] events.ovl001:entry-80103C70 start-event  1 flyer-seen !  endof
        drop run-rows 0   \ (anything else: the rows' own)
    endcase ;

\ ---- □: the act check (0x8010292C) and its actions (0x80102B00) ----
enum: act-action  do-nothing the-bed  3 the-shower the-books  10 take-the-key  13 buy-cigarettes  15 take-lighter
  17 take-letter  19 read-flyer ;
: act-check ( zone check -- action )
    case
        23 of  drop  1 has-item? if  do-nothing  else  take-the-key  then  exit  endof
        17 of  drop   \ buying cigarettes for Barbara before 12/28 20:20
            got-barbaras-cigarettes story@ 0=  cigarettes-asked @ 0<> and  nozz-behind-counter? and
            barbara-invited flag?  $008 2 story-bit flag? or and  clock-time 28 20 20 date>time < and
            if  buy-cigarettes  else  do-nothing  then  exit  endof
        16 of  drop  took-lighter flag? 0=  lighter-seen @ 0<> and if  take-lighter  else  do-nothing  then  exit  endof
    endcase
    case
        1 of  the-shower  endof
        8 of  isabellas-letter has-item? 0=  letter-found @ 0<> and if  take-letter  else  do-nothing  then
            flyer-seen @ if  drop read-flyer  then  endof
        10 of  the-bed  endof
        24 of  the-books  endof
        do-nothing swap
    endcase ;
: act-do ( action -- )
    case
        the-bed of  prologue-done flag? if  ['] events.ovl001:entry-80103C18  else  ['] events.ovl001:entry-80105198  then
            start-event  endof
        the-books of
            clock-time time>day 25 =  today-seen @ 0= and if  ['] events.ovl001:entry-801051B8 start-event  1 today-seen !
            else  ['] events.ovl001:entry-80105248 start-event  then  endof
        the-shower of  ['] events.ovl001:entry-801052E8 start-event  endof
        take-the-key of  prologue-done flag? if  ['] events.ovl001:entry-801059AC  else  ['] events.ovl001:entry-80105808  then
            start-event  endof
        buy-cigarettes of  ['] events.ovl001:entry-80104E10 start-event  1 got-barbaras-cigarettes story+!
            cigarettes s" items" actor-named send give-item  endof
        take-lighter of
            prologue-done flag?  nozz-behind-counter? 0= and if
                ['] events.ovl001:entry-80105700 start-event  took-lighter flag-toggle  lighter s" items" actor-named send give-item
            else  ['] events.ovl001:entry-80105694 start-event  then  endof
        take-letter of  ['] events.ovl001:entry-80105488 start-event  isabellas-letter s" items" actor-named send give-item
            got-isabella-letter flag-set  endof
        read-flyer of  0 flyer-seen !  read-garage-flyer flag-toggle  ['] events.ovl001:entry-80103C80 start-event
            ( the phone learns the garage's number: the phone )  endof
        drop run-rows 0
    endcase ;

\ ---- the area's hooks (0x80103080 entering, 0x8010314C a scene) ----
3 constant matthews-room   \ (the area's scene 3)
: entered ( -- )   \ src: map_reveal(13) and the area's first-time counters; its two props placed (the figures)
    13 map-reveal  forget-firsts  0 cigarettes-asked !  0 lighter-seen ! ;
: scene-hook ( scene -- )   \ Morgan's morning visit before the prologue ends; after it, once, Dr. James's
    matthews-room <> if  exit  then
    prologue-done flag? 0= if  ['] events.ovl001:entry-80104514 start-event exit  then
    dr-james-visit-seen flag? 0=  james-visiting? and  $044 0 story-bit flag? and if
        dr-james-visit-seen flag-toggle  ['] events.ovl001:entry-801049F8 start-event
    then ;

\ ---- talking here (0x80102D60): some say a line of the area's by where they are ----
create nozz-standing   \ Nozz at a stop: its line (src: D_80103DFC)
  0 , 168 ,  2 , 169 ,  4 , 170 ,  9 , 171 ,  10 , 173 ,  11 , 175 ,  15 , 176 ,  18 , 177 ,  19 , 179 ,  20 , 181 ,
  23 , 182 ,  24 , 184 ,  25 , 186 ,  28 , 187 ,  29 , 189 ,  30 , 191 ,  32 , 192 ,  33 , 194 ,  34 , 196 ,
  36 , 197 ,  37 , 199 ,  38 , 201 ,  -1 ,
create nozz-leaving    \ and leaving it (src: D_80103E58)
  9 , 172 ,  10 , 174 ,  18 , 178 ,  19 , 180 ,  23 , 183 ,  24 , 185 ,  28 , 188 ,  29 , 190 ,  32 , 193 ,  33 , 195 ,
  36 , 198 ,  37 , 200 ,  -1 ,
: nozz-talks ( -- handled? )   \ (until the routes, Nozz is never between stops: his standing lines)
    nozz-standing nozz stop-of a-line-by-stop  dup 0< if  drop false exit  then
    nozz 7 one-liner  true ;
: talks-here ( slot -- handled? )
    case
        nozz of  nozz-talks  endof
        ( Morgan, James, Winona, Lidds, the townspeople 80 and 81, Isabella: their lines by their stops - with them )
        false swap
    endcase ;
' talks-here this-area area-talk!

\ ---- the C its events call (ovl001's evfn_*) ----
: nozz-greets ( -- )   \ 0x80103204: by the time of day
    clock-time time>hour dup 17 >= swap 7 < or if  ['] events.ovl001:entry-80103D38  else  ['] events.ovl001:entry-80105090  then
    start-event ;
: room-or-shop ( -- )  prologue-done flag? if  ['] events.ovl001:entry-801050CC  else  ['] events.ovl001:entry-80105A50  then
    start-event ;   \ 0x80103328
: prologue-done? ( -- n )  prologue-done flag? 1 and ;   \ 0x801032B0 (its answer the result)
: sleep-hours ( hours -- )   \ 0x801033D0: the clock on an hour at a time, no further than 12/31 23:49
    60 *  clock-time +  31 23 49 date>time min  clock-time - time-skip ;
: object-6-on ( -- )  1 s" object animation" unbuilt ;   \ 0x80103450 (the area's animated objects: to come)
1 constant item-1   \ (not named yet)
1 ovl $80103204 ' nozz-greets native:
1 ovl $80103328 ' room-or-shop native:
1 ovl $801032B0 ' prologue-done? native:
1 ovl $801032C8 ' object-6-on native:   1 ovl $801032E8 ' object-6-on native:   1 ovl $80103308 ' object-6-on native:
: give-item-1 ( -- )  item-1 s" items" actor-named send give-item ;
1 ovl $80103370 ' give-item-1 native:
: sleep-an-hour ( -- )  1 sleep-hours ;   : sleep-five-hours ( -- )  5 sleep-hours ;
1 ovl $80103390 ' sleep-an-hour native:   1 ovl $801033B0 ' sleep-five-hours native:

' look-check ' look-do ' act-check ' act-do  this-area content area-rules!
this-area ' entered ' scene-hook area-hooks!
