\ area026.fs - area 26's rules: Barrows Forest (src: ovl026.c; decomp/docs/areas/area26.md;
\ port/src/game/ovl026.c): the deputy at the entrance (aside or at his post: past him he pushes you back), the
\ morning hunt (slipping past him while he's away, his chase, following Cohen through the forest at his pace,
\ the deadlines, the run back to the cabin), the cabin and its shell box, closing time, the ways in and out, a
\ meeting at the waterfall at its time, the last evening's places and scenes, who says what. The cabin's
\ shoot-out is a minigame to come.
IN: rules.area026
USING: engine actors messages keys clock story-flags items townsfolk events look talk areas matthew shootout ;

26 constant this-area
10 constant the-deputy   9 constant the-bear   24 constant the-gun   59 constant the-marker   58 constant the-other
11 constant the-second-hunter
$F constant the-gun-item   $11 constant the-necklace

$000 0 story-bit constant joined-the-hunt
$000 1 story-bit constant deputy-refused
$000 2 story-bit constant box-open              \ (never set: src BOX_OPEN)
$000 3 story-bit constant box-pried
$000 4 story-bit constant bear-down
$1C0 constant deputy-off-limits-said   $1C4 0 story-bit constant deputy-search-said   $138 constant carcass-talk-done
$09C 2 story-bit constant ritual-known          \ (src: +0x9C bit 2)
$1D4 constant meeting-day   $1D8 constant meeting-hour   $1DC constant meeting-minute   \ (the minute: a byte)

: matthew-near? ( x z d -- flag )   \ within d of the point
    {: x z d :}  matthew-now drop nip {: mx mz :}  mx x - s>f fdup f*  mz z - s>f fdup f* f+ fsqrt  d s>f f< ;
: apart ( slot -- d )   \ from Matthew
    person-now drop nip {: x z :}  matthew-now drop nip {: mx mz :}  mx x - s>f fdup f*  mz z - s>f fdup f* f+ fsqrt f>s ;
: back ( -- )  matthew-was s" matthew" actor-named send figure-place ;   \ src: player_restore_pos
: final-evening? ( -- flag )   \ 12/31 from 20:00
    clock-time  31 20 0 date>time >= ;
: before? ( day h m -- flag )  date>time clock-time > ;

\ ---- this visit (src: the overlay's locals D_80107860 ..) ----
variable exits-locked      \ src: event_lock (the last evening: no way out)
variable deeper            \ src: D_80107880: past the path in (region 15)
variable closing-done      \ src: D_80107868
variable evening-placed    \ src: D_80107890
variable talks-at-post     \ src: D_8010788D: his lines at the post this visit
variable talked-this-morning   variable gathering-turn   \ src: D_80107862, D_80107861
variable search-talks      \ src: D_80107896: Morgan's at the search
variable shells-seen       \ src: D_80107878: "Shells!" this visit
variable shot-out          \ src: D_80107897: the shoot-out begun
variable first-sign-seen   variable second-sign-seen   \ src: D_80107864, D_80107860
variable carcass-looks     \ src: D_80107884
variable refused-last-visit   \ (src: func_80104110, on leaving: his refusal remembered)

\ ---- the deputy at the entrance (0x801020F8) ----
$D373 constant post-x   $6588A constant post-z   $AE2 negate constant post-heading
$D48F constant aside-x   $65B80 constant aside-z   $602 constant aside-heading
$D25B constant blocked-x   $658D0 constant blocked-z   $17C constant blocked-near
: steps-aside? ( -- flag )
    31 23 30 before?  26 10 0 before? 0= and 0= if  false exit  then
    final-evening? if  1 evening-placed !  entered-wolf-hideout flag? exit  then
    cohen-found-scene-seen flag? 0= ;
: place-deputy ( -- )
    bad-ending-route flag? if  the-deputy 0 0 0 set-pos exit  then
    steps-aside? if  the-deputy aside-x 0 aside-z set-pos  the-deputy 0 aside-heading 0 set-rot
    else  the-deputy post-x 0 post-z set-pos  the-deputy 0 post-heading 0 set-rot  then
    the-deputy this-area 1 set-floor  the-deputy 0 1 anim ;
: at-his-post? ( -- flag )  the-deputy person-now 2drop drop post-x = ;

\ ---- the hunt's state (src: D_800A3750) ----
-1 constant no-hunt
0 constant gathering   1 constant deputy-away   2 constant deputy-waits   3 constant deputy-back
10 constant caught   11 constant chased   12 constant sent-off
20 constant joined   21 constant following   30 constant at-the-first-stop   31 constant first-look   32 constant after-first
33 constant following-on   40 constant at-the-second-stop   41 constant second-look   42 constant after-second
50 constant the-walks-end   51 constant mauled   52 constant the-run   53 constant swiped   54 constant in-the-cabin
55 constant way-blocked   56 constant shooting   57 constant after-the-shootout   58 constant the-late-run
100 constant waiting-scene   101 constant waiting-motion
variable hunt   no-hunt hunt !
variable hunt-thread   -1 hunt-thread !   \ the hunt's scene running
variable deputy-walk   -1 deputy-walk !   \ his walk away and back (src: D_80107888)
variable resume-at     \ src: D_80107863: the state a waiting scene returns to
variable aside-scene   -1 aside-scene !   \ src: D_80107870
variable countdown     \ src: D_80107865 (his wait away)
variable route-point   \ src: D_8010786C (-1: none)
variable been-close    variable lost-once   \ src: D_80107865 (on the walk), D_80107866
variable leg           -1 leg !   \ src: D_80107895: the walk's leg, for its deadline (-1 none)
variable hurried       \ src: D_8010788C: "no time for sightseeing" said this leg
variable the-sign      \ src: D_80107894: 1 the tracks seen, 2 the trunk
variable walk-end-wait \ src: D_8010786D
variable bear-warned   \ src: D_8010788F: the way out blocked, once
: hunt-scene ( xt -- )  launch-event hunt-thread ! ;
: hunt-done? ( -- flag )  hunt-thread @ event-alive? 0= ;
: aside ( xt -- )  launch-event aside-scene ! ;

\ (a step command: its motion and speed, a game frame - src: actor_command 0x18)
begin-structure gait  field: gait-motion  field: gait-speed  end-structure
create deputy-stride  gait allot   5 deputy-stride gait-motion !
create sneaking-stride  gait allot   5531 sneaking-stride gait-motion !  27 sneaking-stride gait-speed !
create walking-stride  gait allot   5 walking-stride gait-motion !  55 walking-stride gait-speed !
create bear-stride  gait allot   5518 bear-stride gait-motion !
90 constant deputy-first-speed   55 constant bear-first-speed
: stride-toward ( slot x z stride -- )   \ turned to the point, a step on
    {: s x z st :}  s person-now drop nip {: sx sz :}
    s  sx sz x z heading-to  0 turn-figure
    s st gait-speed @ st gait-motion @ the-townsfolk send person-forward ;
: stride-at-him ( slot stride -- )  {: s st :}  matthew-now drop nip  {: x z :}  s x z st stride-toward ;
: halt ( slot -- )  the-townsfolk send person-idle ;

\ ---- entering (0x80102244) ----
$DBD5 constant marker-x   $659DB constant marker-z   $C08 constant marker-heading
: routes ( -- )   \ the last evening, once the hideout's found: the exits locked, the way it goes
    entered-wolf-hideout flag?  final-evening? and 0= if  exit  then
    1 exits-locked !
    bad-ending-route flag?  31 23 30 before? 0= or if  exit  then
    visited-james-house story@ 0=  church-james-scene-seen story@ 0= or  got-bear-necklace flag? 0= or if
        bad-ending-route flag-toggle  ['] events.ovl026:entry-80106E54 start-event  the-deputy 0 0 0 set-pos  then
    the-marker 0 0 0 set-pos ;
: entered ( -- )
    1 map-reveal
    refused-last-visit @ if  deputy-refused flag-toggle  0 refused-last-visit !  then
    joined-the-hunt flag?  26 10 0 before? 0= or if  no-hunt  else  gathering  then hunt !
    deputy-first-speed deputy-stride gait-speed !  bear-first-speed bear-stride gait-speed !
    0 evening-placed !  0 closing-done !  place-deputy  0 deeper !  0 exits-locked !
    the-marker marker-x 0 marker-z set-pos  the-marker 0 marker-heading 0 set-rot  the-bear 0 0 0 set-pos
    0 talked-this-morning !  0 gathering-turn !  0 talks-at-post !  0 bear-warned !  0 search-talks !
    0 second-sign-seen !  0 first-sign-seen !  -1 leg !  0 shells-seen !  0 shot-out !  -1 route-point !
    the-other 0 0 0 set-pos  routes ;
7 constant the-cabin   $102 negate constant gun-y   $17CA constant gun-heading
: scene-hook ( scene -- )   \ src: func_80104090: in the cabin, the gun on the floor
    the-cabin <> if  exit  then
    the-gun the-gun person-now drop nip gun-y swap set-pos  the-gun 0 gun-heading 0 set-rot ;

\ ---- the hunt (src: hunt_update, hunt_deadline) ----
create deadlines  30 , 40 , 50 , 60 , 70 ,   \ src: D_8010634C: each leg's, minutes after 8:00
: deadline ( -- )
    leg @ 0< if  exit  then
    leg @ cells deadlines + @  clock-time time>hour 8 - 60 *  clock-time 60 mod + < if
        ['] events.ovl026:entry-80105D84 hunt-scene  -1 route-point !  no-hunt hunt !  -1 leg !
        the-second-hunter 0 0 0 set-pos  then ;
\ the gathering: his wait away (7:00 - 7:59, the even minutes), at the gate's side
$D51A constant gate-x0   $E88D constant gate-x1   $6451D constant gate-z0   $65A48 constant gate-z1
$E013 constant gate-far-x   $6511A constant gate-near-z
: by-the-gate? ( -- flag )
    matthew-now drop nip {: x z :}
    x gate-x0 gate-x1 within  z gate-z0 >= and  z gate-z1 <= and  x gate-far-x >  z gate-near-z <= or and ;
\ the clearing they gather in: off limits (his "Hey!")
$D0AD constant clearing-x0   $D664 constant clearing-x1   $65783 constant clearing-z0   $65CBD constant clearing-z1
: in-the-clearing? ( -- flag )
    matthew-now drop nip {: x z :}  x clearing-x0 clearing-x1 within  z clearing-z0 >= and  z clearing-z1 <= and ;
: spotted? ( -- flag )   \ src: func_80104298
    in-the-clearing? 0= if  false exit  then
    hunt-thread @ kill-event  ['] events.ovl026:entry-80105B0C hunt-scene  true ;
$D49E constant town-side-x   $B381 constant hunters-x   $6581B constant hunters-z   $1E0 constant hunters-near
$F0 constant caught-near   61 constant deputy-slowest   10 constant deputy-slows
60 constant away-frames
: gather ( -- )
    clock-time time>day 26 =  clock-time time>hour 7 = and  by-the-gate? and  talked-this-morning @ 1 = and
    clock-time 2 mod 0= and if
        deputy-away hunt !  ['] events.ovl026:entry-801059B8 launch-event dup hunt-thread !  deputy-walk !
        0 talked-this-morning !  gathering-turn @ 1 xor gathering-turn !  then ;
: chase ( -- )
    hunters-x hunters-z hunters-near matthew-near? if   \ through to the hunters
        \ (src: the three hunters depart too - the scene puts them at their stops at once)
        the-deputy halt  ['] events.ovl026:entry-801063BC hunt-scene
        joined hunt !  joined-the-hunt flag-toggle  exit  then
    the-deputy deputy-stride stride-at-him
    the-deputy apart caught-near < if
        the-deputy halt  ['] events.ovl026:entry-80105BBC hunt-scene  sent-off hunt !
        deputy-stride gait-speed @ deputy-slowest >= if  deputy-slows negate deputy-stride gait-speed +!  then  then ;

\ following Cohen (0x801037B4, 0x8010398C): his route, a point at a time (x 0: a motion of his, its z)
create cohen-route
  34219 , 409387 ,  31867 , 410704 ,  0 , 81 ,  0 , 4 ,  32377 , 411484 ,  31668 , 412226 ,  32000 , 413273 ,
  34085 , 413235 ,  35012 , 412591 ,  35569 , 413399 ,  35849 , 414505 ,  0 , 81 ,  35095 , 415060 ,
  32568 , 415037 ,  32042 , 416007 ,  32315 , 415525 ,  30404 , 418124 ,  30586 , 419360 ,  31460 , 420864 ,
  32049 , 421476 ,  32247 , 421770 ,  32453 , 422261 ,  32516 , 422673 ,  32477 , 423144 ,  32327 , 423722 ,
  32110 , 424071 ,  31743 , 424374 ,  30953 , 424897 ,  0 , 81 ,  0 , 48 ,  0 , 49 ,  0 , 4 ,  31093 , 426081 ,
  30244 , 428928 ,  29547 , 431825 ,  0 , 81 ,  34244 , 433040 ,  0 , 81 ,  38105 , 431663 ,  0 , 81 ,
  39381 , 431700 ,  0 , 81 ,  40804 , 431160 ,
16 constant first-leg-end   33 constant second-leg-end   43 constant route-end
$30 constant crouch-motion   $32 constant crouched-motion   $56 constant kneeling-motion
100 constant point-near   3000 constant too-far   2000 constant close-enough   2600 constant keeps-pace
500 constant reached-him   255 constant end-wait-frames
: point ( n -- x z )  2* cells cohen-route + dup @ swap cell+ @ ;
variable cohen-striding
: cohen-step ( -- )   \ toward his point at the leg's pace
    route-point @ point {: x z :}
    cohen x z  route-point @ second-leg-end < if  sneaking-stride  else  walking-stride  then  stride-toward
    true cohen-striding ! ;
: cohen-stop ( -- )  cohen-striding @ if  cohen halt  false cohen-striding !  then ;
: waiting ( state xt -- )  aside  resume-at !  waiting-scene hunt ! ;
: keep-up ( time? -- )   \ src: func_8010398C
    {: time? :}  cohen apart {: d :}  false {: lost :}
    d too-far >= if  true to lost
    else
        d close-enough < if  1 been-close !  then
        been-close @ if
            d keeps-pace < if
                hunt @ waiting-motion <>  route-point @ 0< 0= and if  cohen-step  then  0 lost-once !
            else  cohen-stop  lost-once @ 0= if  true to lost  1 lost-once !  then  then
        else  cohen-stop  then
    then
    lost if  cohen-stop  hunt @ ['] events.ovl026:entry-80105C70 waiting  back  0 been-close !  exit  then
    route-point @ second-leg-end <  pad-cross key-down? and if
        cohen-stop  hunt @ ['] events.ovl026:entry-80105E70 waiting  exit  then
    time? 0=  hurried @ or if  exit  then
    leg @ 0 =  clock-time 60 mod 21 >= and  leg @ 2 =  clock-time 60 mod 41 >= and or
    leg @ 4 =  clock-time time>hour 9 = and or if
        cohen-stop  hunt @ ['] events.ovl026:entry-80105D04 waiting  1 hurried !  then ;
: walk-on ( state -- )   \ src: func_801037B4
    resume-at !
    route-point @ 0< if  true keep-up exit  then
    route-point @ point {: x z :}
    x 0= if   \ a motion of his
        cohen-stop  cohen z  z crouch-motion = if  crouched-motion  else  -1  then  false anim-then
        hunt @ resume-at !  waiting-motion hunt !  1 route-point +!  exit  then
    cohen person-now drop nip  z - s>f fdup f*  x - s>f fdup f* f+ fsqrt  point-near s>f f< 0= if  true keep-up exit  then
    1 route-point +!
    route-point @ case
        first-leg-end of  cohen-stop  at-the-first-stop hunt !  ['] events.ovl026:entry-801063CC hunt-scene  0 hurried !  1 leg !  endof
        second-leg-end of  cohen-stop  at-the-second-stop hunt !  ['] events.ovl026:entry-801063EC hunt-scene  0 hurried !  3 leg !  endof
        route-end of  cohen-stop  the-walks-end hunt !  end-wait-frames walk-end-wait !
            cohen crouch-motion crouched-motion false anim-then  -1 leg !  endof
    endcase ;
$7F98 constant after-x   $320 negate constant after-y   $68377 constant after-z   $219 negate constant after-heading
$B85D constant way-out-x   $65850 constant way-out-z   300 constant way-out-near   400 constant swipe-near
$8842 constant cabin-gun-x   $63A23 constant cabin-gun-z
: the-bear-chases ( -- )  the-bear bear-stride stride-at-him ;
: run-for-it ( -- )   \ the bear after him, to the cabin
    bear-warned @ 0=  way-out-x way-out-z way-out-near matthew-near? and if
        ['] events.ovl026:entry-8010605C hunt-scene  way-blocked hunt !  1 bear-warned !  then
    the-bear-chases
    the-bear apart swipe-near < if  the-bear halt  ['] events.ovl026:entry-80105FBC hunt-scene  swiped hunt !  then
    clock-time time>hour 9 =  clock-time 60 mod 21 >= and if
        ['] events.ovl026:entry-80106224 hunt-scene  the-late-run hunt !  then ;
: hunt-step ( -- )
    hunt @ case
        gathering of  gather  endof
        deputy-away of  spotted? if  caught hunt !  else  hunt-done? if  away-frames countdown !  deputy-waits hunt !  then  then  endof
        deputy-waits of  spotted? if  caught hunt !  else  -1 countdown +!  countdown @ 0= if
            deputy-back hunt !  ['] events.ovl026:entry-80105A7C launch-event dup hunt-thread !  deputy-walk !  then  then  endof
        deputy-back of  spotted? if  caught hunt !  else  hunt-done? if  gathering hunt !  then  then  endof
        caught of  hunt-done? if
            matthew-now 2drop drop town-side-x > if  place-deputy  gathering hunt !  else  chased hunt !  then  then  endof
        chased of  chase  endof
        sent-off of  hunt-done? if  0 deeper !  gathering hunt !  then  endof
        joined of  hunt-done? if  0 route-point !  1 been-close !  0 lost-once !  0 leg !  0 hurried !  following hunt !  then  endof
        following of  following walk-on  endof
        at-the-first-stop of  hunt-done? if  -1 route-point !  0 the-sign !  cohen kneeling-motion 1 anim  first-look hunt !  then  endof
        first-look of  first-look walk-on  the-sign @ if
            ['] events.ovl026:entry-801063DC hunt-scene  0 hurried !  2 leg !  after-first hunt !  then  endof
        after-first of  hunt-done? if  first-leg-end route-point !  following-on hunt !  then  endof
        following-on of  following-on walk-on  endof
        at-the-second-stop of  hunt-done? if  cohen kneeling-motion 1 anim  0 the-sign !  -1 route-point !  second-look hunt !  then  endof
        second-look of  second-look walk-on  the-sign @ 2 = if
            ['] events.ovl026:entry-801063FC hunt-scene  0 hurried !  4 leg !  after-second hunt !  then  endof
        after-second of  hunt-done? if
            matthew after-x after-y after-z set-pos  matthew 0 after-heading 0 set-rot
            following hunt !  second-leg-end route-point !  then  endof
        the-walks-end of
            -1 walk-end-wait +!  cohen apart reached-him <  walk-end-wait @ 0= or if
                ['] events.ovl026:entry-8010640C hunt-scene  mauled hunt !  then  endof
        mauled of  hunt-done? if  the-run hunt !  1 exits-locked !  then  endof
        the-run of  run-for-it  endof
        way-blocked of  hunt-done? if  the-run hunt !  then  endof
        the-late-run of  the-bear-chases
            the-bear apart swipe-near < if  the-bear halt  ['] events.ovl026:entry-80105FBC hunt-scene  swiped hunt !  then  endof
        waiting-scene of  aside-scene @ event-alive? 0= if  resume-at @ hunt !  then  endof
        waiting-motion of  cohen person-anim-done? if  resume-at @ hunt !  else  false keep-up  then  endof
    endcase ;

\ ---- each frame (0x80102490) ----
$B49C constant entrance-x   $65817 constant entrance-z   $190 constant entrance-near
$8010 constant tree-x   $68458 constant tree-z   $258 constant tree-near
$3E25 constant waterfall-x   $677CC constant waterfall-z   $708 constant waterfall-near
4 constant the-way-out   15 constant the-path-in   16 constant the-path-back
: at-the-tree ( -- )   \ (the flags as they were before either)
    $020 story@ {: w :}
    w $600000 and $200000 = if  bad-ending-started flag-toggle  ['] events.ovl026:entry-801064DC start-event  then
    w $1800000 and $800000 = if  final-ending-started flag-toggle  ['] events.ovl026:entry-8010650C start-event  then ;
: the-last-evening ( -- )
    entrance-x entrance-z entrance-near matthew-near? if
        $020 story@ $300000 and $100000 = if
            bad-route-matthew-scene-seen flag-toggle  ['] events.ovl026:entry-80106E64 start-event  then
        $020 story@ $900000 and 0= if
            final-route-matthew-scene-seen flag-toggle  ['] events.ovl026:entry-80106E74 start-event  then
    then
    tree-x tree-z tree-near matthew-near? if  at-the-tree  then ;
: no-way-out ( -- )  back  ['] events.ovl026:entry-80106304 start-event ;
: the-ways ( -- )   \ the zones he walks into: out, the path in, back
    at-zone nip case
        the-way-out of  exits-locked @ if  no-way-out  else  ['] events.ovl026:entry-80106324 start-event  then  endof
        the-path-in of  deeper @ 0= if  1 deeper !  final-evening? if  1 exits-locked !  then  then  endof
        the-path-back of  deeper @ if  final-evening? if  no-way-out  else  0 deeper !  then  then  endof
    endcase ;
: meeting-time? ( -- flag )   \ from the time it was set for, the hour after
    meeting-day story@  meeting-hour story@  meeting-minute story-c@  meeting-hour story@ 1+  meeting-minute story-c@ between? ;
: the-meeting ( -- )
    forest-meeting-seen flag?  meeting-day story@ 0= or if  exit  then
    waterfall-x waterfall-z waterfall-near matthew-near? 0= if  exit  then
    meeting-time? 0= if  exit  then
    0 meeting-day story!  forest-meeting-seen flag-toggle  ['] events.ovl026:entry-80106E2C start-event ;
: each-frame ( -- )
    at-his-post?  blocked-x blocked-z blocked-near matthew-near? and if  back  then
    exits-locked @ 0=  closing-done @ 0= and  deeper @ and  31 20 0 20 29 between? and if
        1 closing-done !  ['] events.ovl026:entry-80106294 start-event  0 deeper !  then
    evening-placed @ 0=  final-evening? and if  1 evening-placed !  place-deputy  then
    final-evening? if  the-last-evening  then
    events-running @ if  exit  then
    the-ways  events-running @ if  exit  then
    the-meeting  deadline  hunt-step ;

\ ---- ○ (0x801031C0 / 0x80103524) ----
1 constant the-tracks-zone   5 constant the-trunk-zone
3 constant cabin-door-inside   2 constant cabin-door-outside   9 constant box-left   10 constant box-right
10 constant cohen-gone-stop   8 constant cohen-crouches-stop
enum: look-action  nothing-here  the-tracks  the-trunk  into-the-cabin  stay-close  the-shells
  10 out-of-the-cabin  12 a-thought ;
: cohen-at? ( stop -- flag )  cohen stop-of = ;
: look-check ( zone check -- action )
    {: z c :}
    at-zone nip case
        the-tracks-zone of
            first-sign-seen @ 0=  hunt @ first-look = and  cohen-crouches-stop cohen-at? and if  the-tracks  else  nothing-here  then  exit  endof
        the-trunk-zone of
            second-sign-seen @ 0=  hunt @ second-look = and  cohen-crouches-stop cohen-at? and if  the-trunk exit  then
            forest-mortal-journey-thought flag? 0=  ritual-known flag? and if  a-thought  else  nothing-here  then  exit  endof
    endcase
    c case
        cabin-door-inside of  out-of-the-cabin  endof
        cabin-door-outside of
            cohen-gone-stop cohen-at? if  nothing-here
            else  hunt @ dup no-hunt =  over the-run = or  swap after-the-shootout = or if  into-the-cabin  else  stay-close  then  then  endof
        box-left of  shot-out @ 0=  hunt @ in-the-cabin = and  box-pried flag? 0= and if  the-shells  else  nothing-here  then  endof
        box-right of  shot-out @ 0=  hunt @ in-the-cabin = and  box-pried flag? 0= and if  the-shells  else  nothing-here  then  endof
        nothing-here swap
    endcase ;
$1800 constant gun-tilt
: look-do ( action -- )
    case
        the-tracks of  1 the-sign !  1 first-sign-seen +!  endof
        the-trunk of  2 the-sign !  1 second-sign-seen +!  endof
        a-thought of  forest-mortal-journey-thought flag-toggle  ['] events.ovl026:entry-80106240 start-event  endof
        into-the-cabin of
            ['] events.ovl026:entry-801066AC start-event
            hunt @ the-run = if  in-the-cabin hunt !
                the-gun cabin-gun-x gun-y cabin-gun-z set-pos  the-gun gun-tilt 0 0 set-rot  then  endof
        stay-close of  hunt @ ['] events.ovl026:entry-80105C70 waiting  endof
        the-shells of
            the-gun-item has-item? if  ['] events.ovl026:entry-80106710 start-event  1 shells-seen !
            else  ['] events.ovl026:entry-801066E8 start-event  then  endof
        out-of-the-cabin of
            hunt @ in-the-cabin = if  ['] events.ovl026:entry-80105EF0  else  ['] events.ovl026:entry-80106670  then  start-event  endof
        drop run-rows 0
    endcase ;

\ ---- □ (0x801033B8 / 0x801036E0): the shell box's halves ----
enum: act-action  nothing-to-do  6 pry-right  open-right  pry-left  open-left ;
: act-check ( zone check -- action )
    nip {: c :}
    c box-left <>  c box-right <> and if  nothing-to-do exit  then
    box-open flag?  the-gun-item has-item? 0= or  shot-out @ or if  nothing-to-do exit  then   \ (after: the reload - to come)
    hunt @ in-the-cabin <>  shells-seen @ 0= or if  nothing-to-do exit  then
    c box-left = if  box-pried flag? if  open-left  else  pry-left  then
    else  box-pried flag? if  open-right  else  pry-right  then  then ;
: act-do ( action -- )
    case
        pry-right of  ['] events.ovl026:entry-80106864 start-event  box-pried flag-toggle  endof
        pry-left of  ['] events.ovl026:entry-80106758 start-event  box-pried flag-toggle  endof
        open-right of  ['] events.ovl026:entry-801068EC start-event  endof
        open-left of  ['] events.ovl026:entry-801067E0 start-event  endof
        drop run-rows 0
    endcase ;

\ ---- talking here (0x80103F9C) ----
7 constant usual-gesture   1 constant plain-gesture
: deputy-talks ( -- true )   \ src: 0x80103C30
    clock-time time>day 26 =  clock-time time>hour 7 = and if   \ the hunt's morning: his two lines
        talked-this-morning @ 0= if
            gathering-turn @ if  $23  else  $21  then  the-deputy usual-gesture one-liner  1 talked-this-morning !
        else  $22 the-deputy usual-gesture one-liner  then  true exit  then
    deputy-off-limits-said story@ 0=  cohen-found-scene-seen flag? and if
        1 deputy-off-limits-said story!  ['] events.ovl026:entry-801065C8 start-event  true exit  then
    deputy-search-said flag? 0=  25 15 0 17 0 between? and if
        deputy-search-said flag-toggle  ['] events.ovl026:entry-80106610 start-event  true exit  then
    at-his-post? 0= if  $27 the-deputy plain-gesture one-liner  true exit  then
    deputy-refused flag? if  $20 the-deputy usual-gesture one-liner  true exit  then
    talks-at-post @ if  $1F  else  $1E  then the-deputy usual-gesture one-liner  1 talks-at-post !
    1 refused-last-visit !  true ;
: morgan-talks ( -- handled? )   \ src: 0x80103E5C
    morgan person-of {: p :}
    p travelling @ if  p at-stop @ 12 <> if  false exit  then  $55 morgan plain-gesture one-liner  true exit  then
    p at-stop @ case
        4 of  search-talks @ if  ['] events.ovl026:entry-80106210  else  1 search-talks !  ['] events.ovl026:entry-801061BC  then
            start-event  true  endof
        12 of  $56 morgan usual-gesture one-liner  true  endof
        $39 of  $57 morgan usual-gesture one-liner  true  endof
        $40 of  $58 morgan plain-gesture one-liner  true  endof
        $43 of  $59 morgan plain-gesture one-liner  true  endof
        $4D of  $5A morgan plain-gesture one-liner  true  endof
        false swap
    endcase ;
create cohen-lines  5 , 92 ,  11 , 93 ,  13 , 94 ,  15 , 95 ,  19 , 96 ,  21 , 97 ,  23 , 98 ,  28 , 99 ,  30 , 100 ,
  32 , 101 ,  36 , 102 ,  40 , 103 ,  45 , 104 ,  -1 ,
: talks-here ( slot -- handled? )
    case
        the-deputy of  deputy-talks  endof
        morgan of  morgan-talks  endof
        cohen of  cohen stop-of 0< if  false exit  then
            cohen-lines cohen person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then
            cohen usual-gesture one-liner  true  endof
        nozz of  nozz 14 standing-at? 0= if  false exit  then  $5B nozz usual-gesture one-liner  true  endof
        false swap
    endcase ;
' talks-here this-area area-talk!
: after-the-bear ( -- handled? )   \ src: func_801041AC: once
    area @ this-area <>  carcass-talk-done story@ 0<> or  bear-down flag? 0= or if  false exit  then
    1 carcass-talk-done story!  ['] events.ovl026:entry-8010658C start-event  true ;
' after-the-bear morgan conversation!

\ ---- the C its events call ----
: back-to-the-chase ( -- )  the-run hunt ! ;                    \ 0x80104340
: the-bear-slows ( -- )   \ 0x80104350
    bear-stride gait-speed @ 30 >= if  -8 bear-stride gait-speed +!  then ;
: cabin-door ( flag -- )  dup 0 swap s" areas" actor-named send object-shown  1 swap s" areas" actor-named send object-shown ;
: door-shown ( -- )  true cabin-door ;    \ 0x80104218 (src: the cabin door's two objects)
: door-hidden ( -- )  false cabin-door ;  \ 0x80104238
: window-patch ( -- ) ;   \ 0x80104258 / 0x80104278: the window's texture (to come)
\ the shoot-out at the cabin (src: shootout_update): three rounds against the bear
$86A2 constant his-x   $6392C constant his-z   $8D89 constant bear-x   $63770 constant bear-z   $C40 constant bear-heading
$158D constant bear-stance   $1593 constant bear-hit   $15A2 constant gun-down   $1598 constant gun-fired
$1599 constant gun-up   90 constant bear-health   3 constant a-hit   18 constant first-swipe   24 constant second-swipe
30 constant third-swipe   0 constant bear-pool   9 constant after-the-bear-stop
create the-shootout  3 plan-bytes allot   the-shootout 3 plan-bytes erase
: bear-shot ( -- )   \ src: func_801043AC: the hunters to the cabin
    0 exits-locked !  hunt-thread @ kill-event  after-the-shootout hunt !
    ['] events.ovl026:entry-801064C4 start-event
    cohen after-the-bear-stop goto-stop  nozz after-the-bear-stop goto-stop  bear-down flag-toggle ;
: plan! ( -- )
    the-shootout {: p :}  s" shootout-26" p plan-table-len ! p plan-table !  3 p plan-rounds !
    his-x p plan-him-x !  his-z p plan-him-z !  gun-down p plan-stance !  gun-fired p plan-shot-motion !
    gun-up p plan-aimed-motion !  bear-x p plan-foe-x !  bear-z p plan-foe-z !  bear-heading p plan-foe-heading !
    bear-stance p plan-foe-stance !  bear-hit p plan-foe-hit !  bear-health p plan-full-0 !  0 p plan-full-1 !
    ['] bear-shot p plan-won !
    p 0 bear-pool a-hit first-swipe ['] events.ovl026:entry-80106454 ['] events.ovl026:entry-80106484 ['] events.ovl026:entry-801064A4 round!
    p 1 bear-pool a-hit second-swipe ['] events.ovl026:entry-80106464 ['] events.ovl026:entry-80106494 ['] events.ovl026:entry-801064A4 round!
    p 2 bear-pool a-hit third-swipe ['] events.ovl026:entry-80106474 ['] events.ovl026:entry-801064B4 ['] events.ovl026:entry-801064A4 round! ;
plan!
: shootout-begin ( -- )   \ 0x801040D0
    shooting hunt !  1 shot-out !  the-shootout s" shootout" actor-named send shootout-start ;
: reloaded ( -- ) ;       \ 0x8010431C (the shoot-out's)
this-area ovl $801020F8 ' place-deputy native:   this-area ovl $80104340 ' back-to-the-chase native:
this-area ovl $80104350 ' the-bear-slows native:   this-area ovl $80104218 ' door-shown native:
this-area ovl $80104238 ' door-hidden native:   this-area ovl $80104258 ' window-patch native:
this-area ovl $80104278 ' window-patch native:   this-area ovl $801040D0 ' shootout-begin native:
this-area ovl $8010431C ' reloaded native:

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
