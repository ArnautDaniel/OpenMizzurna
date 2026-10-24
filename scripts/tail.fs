\ tail.fs - tailing the lookout, and his door's keypad (docs/subsystems/tail.md; src: ovl027 func_801025E0's
\ states -2 to 10). In the town by his door he paces between two points, now and then looking round. Started,
\ he walks his way (the town's table: a point's x, z, what he does there, the next point, the next once he's seen
\ Matthew, his sight's zone): he looks back or aside at some points - Matthew in that zone then, he's seen - and
\ from the third point on Matthew coming within 800 is seen too. Seen, his scene, and his way leads him back;
\ before the third point it's forgotten and he paces again. At his door with Matthew hidden by it, the door's
\ scene, then the keypad: four face buttons, the code the town's own; wrong, again (the third time with a hint);
\ right, through the door. A step at the original's rate (30 a second in the town).
IN: tail
USING: engine actors messages keys screen areas figures matthew townsfolk story-flags events events.ovl027 ;

\ ---- facts ----
variable tail-point   \ src: D_8010CA28: the point he walks to (the keypad's: frames since the 4th press)
: tail-started? ( -- flag )  tail-point @ 2 >= ;   \ src: ovl027_tail_started

<PRIVATE
106 constant lookout   800 constant earshot   64 constant look-round-odds   2 constant first-of-the-way
\ what he does at a point (the table's)
1 constant goes-on   4 constant looks-back   5 constant looks-aside   7 constant at-his-door
enum: tail-stage  off  pacing  intro  next-point  to-point  on-his-way  looking-back  looking-aside  in-a-scene  door
    looking-round  keypad  robber-at-door  robber-walking  robber-stopped  robber-fleeing  robber-caught ;
variable stage   variable seen   variable timer   variable after-scene   variable scene-thread
fvariable due

\ ---- the town's tables ----
12 constant point-bytes   8 constant does-at   9 constant next-at   10 constant next-seen-at   11 constant zone-at
: point-xz ( i -- x z )  3 * {: k :}  s" tail-waypoints" k exe-s32  s" tail-waypoints" k 1+ exe-s32 ;
: point-u8 ( i off -- n )  swap point-bytes * + {: k :}  s" tail-waypoints" k exe-u8 ;
: corner ( zone k -- n )  swap 4 * + {: k :}  s" tail-zones" k exe-s32 ;
: between ( n a b -- flag )  2dup max -rot min {: n hi lo :}  lo n <  n hi < and ;   \ (strictly, either way round)
: in-zone? ( zone -- flag )   \ Matthew in it (src: func_800296F8)
    {: z :}  matthew-now drop nip {: x mz :}  x  z 0 corner  z 2 corner between  mz  z 1 corner  z 3 corner between and ;
: point-zone ( -- zone )  tail-point @ zone-at point-u8 ;
: in-sight? ( -- flag )  point-zone in-zone? ;

\ ---- him ----
: near-him? ( -- flag )   \ Matthew within earshot
    lookout person-now drop nip {: x z :}  matthew-now drop nip {: mx mz :}
    mx x - s>f fdup f*  mz z - s>f fdup f* f+ fsqrt  earshot s>f f< ;
: heading ( -- h )  lookout person-now nip nip nip ;
: turn-him ( d -- )  heading + 4095 and  lookout swap the-townsfolk send person-turn ;
: play-scene ( xt -- )  launch-event scene-thread ! ;
: scene-over? ( -- flag )  scene-thread @ event-alive? 0= ;
: spotted ( xt then -- )   \ his "who's there" scene, then on (src: spotted)
    after-scene !  play-scene  1 seen !  in-a-scene stage ! ;
: arrived ( code -- stage )
    case  looks-back of  looking-back  endof  looks-aside of  looking-aside  endof
        at-his-door of  door  endof  next-point swap  endcase ;

\ ---- the keypad (src: states 9 and 10) ----
4 constant digits   16 constant settle-frames   3 constant hint-after
create entered digits cells allot   variable #entered   variable right   variable misses
create was-down digits cells allot
: digit-key ( d -- key )  case  0 of  pad-triangle  endof  1 of  pad-circle  endof  2 of  pad-cross  endof
    drop pad-square 0  endcase ;
$10 constant first-button-bit
: code-has? ( d -- flag )   \ the code's button at this place
    first-button-bit swap lshift  s" keypad-code" #entered @ exe-u8 and 0<> ;
: press-digit ( d -- )   \ (src: the lowest pressed: △ ○ ✕ □)
    dup #entered @ cells entered + !  dup code-has? if  1 right +!  then  drop  1 #entered +! ;
: watch-digit ( d -- pressed? )
    {: d :}  d digit-key key-down? {: down :}  down  d cells was-down + @ 0= and  down d cells was-down + ! ;
variable taken   \ (a step takes one press: the first)
: read-keypad ( -- )
    false taken !  4 0 do  i watch-digit  taken @ 0= and if  i press-digit  true taken !  then  loop ;
: forget-keys ( -- )  4 0 do  i digit-key key-down? i cells was-down + !  loop ;
: keypad-hold ( -- )   \ an event while it's up: Matthew no longer the player's (src: control off, field mode 8)
    begin  stage @ keypad =  while  yield  repeat ;
: keypad-again ( -- )  0 #entered !  0 right !  0 tail-point !  forget-keys  keypad stage !  ['] keypad-hold launch-event drop ;
: keypad-step ( -- )
    #entered @ digits < if  read-keypad exit  then
    1 tail-point +!  tail-point @ settle-frames < if  exit  then
    right @ digits = if  ['] entry-80109EB4 play-scene  off stage !  exit  then   \ "it opened": through the door
    1 misses +!  misses @ hint-after >= if  ['] entry-80109EF8  else  ['] entry-80109EF0  then
    ['] keypad-again spotted ;

\ ---- the robber (src: states 11 - 17): from the diner's door round his loop of the town's points, walking; once
\ he's been spoken to, fleeing round it - running with Matthew within 0x800 ----
8 constant #patrol   $800 constant too-close   30 constant robber-walks   $82 constant robber-runs
1 constant walking-motion   5 constant running-motion
variable patrol-point   variable gait-now
: patrol-xz ( i -- x z )  2* {: k :}  s" robber-patrol" k exe-s32  s" robber-patrol" k 1+ exe-s32 ;
: close-to-him? ( -- flag )
    lookout person-now drop nip {: x z :}  matthew-now drop nip {: mx mz :}
    mx x - s>f fdup f*  mz z - s>f fdup f* f+ fsqrt too-close s>f f< ;
: go-patrol ( speed motion -- )   \ toward the point (again only when the point or the gait changes)
    {: v m :}  m gait-now @ = if  exit  then  m gait-now !
    lookout patrol-point @ patrol-xz v m the-townsfolk send person-walk ;
: patrol-step ( flee? -- )   \ src: patrol
    {: flee :}  lookout walking? 0=  gait-now @ 0< 0= and if
        patrol-point @ 1+ #patrol mod patrol-point !  -1 gait-now !  then
    flee close-to-him? and if  robber-runs running-motion  else  robber-walks walking-motion  then  go-patrol ;
: robber-frame ( -- )
    stage @ case
        robber-at-door of  scene-over? if  0 patrol-point !  -1 gait-now !  robber-walking stage !  then  endof
        robber-walking of  false patrol-step  endof
        robber-stopped of  scene-over? if  -1 gait-now !  robber-fleeing stage !  then  endof
        robber-fleeing of  true patrol-step  endof
        robber-caught of  scene-over? if  off stage !  then  endof
    endcase ;
: robber-out? ( -- flag )  stage @ robber-at-door robber-caught 1+ within ;
variable robber-talks   \ src: D_8010CA31
: at-the-door ( -- )  ['] entry-80109F5C play-scene  robber-at-door stage !  0 robber-talks !  0e due f! ;   \ minigame 18
: spoken-to ( -- )   \ minigames 19 (first), 20 (caught: src +0x10 bit 16)
    robber-talks @ 0= if  1 robber-talks !  lookout idle  ['] entry-8010A008 play-scene  robber-stopped stage ! exit  then
    $010 story@ $10000 or $010 story!  ['] entry-8010A064 play-scene  robber-caught stage ! ;

\ ---- a step (src: func_801025E0) ----
: walk-on ( -- )  lookout tail-point @ point-xz walk-to  on-his-way stage ! ;
: next-one ( -- )   \ src: state 1
    tail-point @  seen @ if  next-seen-at  else  next-at  then  point-u8 tail-point !
    tail-point @ first-of-the-way < if  0 seen !  then  walk-on ;
: on-his-way-step ( -- )   \ src: state 3
    lookout walking? 0= if  seen @ if  next-point  else  tail-point @ does-at point-u8 arrived  then  stage !  0 timer ! exit  then
    tail-point @ first-of-the-way <  look-round-odds random 0= and if
        ['] entry-80109E28 lookout start-route  looking-round stage ! exit  then
    tail-point @ first-of-the-way 1+ >=  seen @ 0= and  near-him? and if
        ['] entry-80109CF8 ['] next-one spotted  then ;
16 constant looks-from   32 constant watches-from   62 constant turns-back-from   78 constant looked
: look-back-step ( -- )   \ src: state 4: he turns half round, watches, turns back
    timer @ {: t :}  1 timer +!
    t looks-from watches-from within if  128 turn-him exit  then
    t watches-from turns-back-from within if
        in-sight?  seen @ 0= and if  turns-back-from timer !  ['] entry-80109CF8 ['] next-one spotted  then  exit  then
    t turns-back-from looked within if  -128 turn-him exit  then
    t looked >= if  next-point stage !  then ;
8 constant aside-from   16 constant aside-watch   46 constant aside-back   54 constant aside-turn   62 constant aside-done
: aside-again ( -- )  looking-aside stage ! ;
$10 constant lean-step   variable leaning
: lean-him ( d -- )  leaning +!  lookout 0 leaning @ the-townsfolk send person-tilt ;
: look-aside-step ( -- )   \ src: state 5: he turns aside, leans ($10 a frame), watches, leans back, turns back
    timer @ {: t :}  1 timer +!
    t aside-from < if  -128 turn-him exit  then
    t aside-from aside-watch within if  lean-step negate lean-him exit  then
    t aside-watch aside-back within if
        in-sight?  seen @ 0= and if  aside-back timer !  ['] entry-80109D38 ['] aside-again spotted  then  exit  then
    t aside-back aside-turn within if  lean-step lean-him exit  then
    t aside-turn aside-done within if  seen @ if  next-point stage ! exit  then  128 turn-him exit  then
    t aside-done >= if  next-point stage !  then ;
: door-step ( -- )   \ src: state 7: at his door
    point-zone in-zone? if  0 misses !  ['] entry-80109D60 ['] keypad-again spotted  0 seen ! exit  then
    near-him? if  ['] entry-80109CF8 ['] next-one spotted  then ;
: tail-frame ( -- )
    robber-out? if  robber-frame exit  then
    stage @ case
        intro of  scene-over? if  first-of-the-way tail-point !  walk-on  then  endof
        next-point of  next-one  endof
        on-his-way of  on-his-way-step  endof
        looking-back of  look-back-step  endof
        looking-aside of  look-aside-step  endof
        in-a-scene of  scene-over? if  after-scene @ execute  then  endof
        door of  door-step  endof
        looking-round of  lookout route-thread 0< if  walk-on  then  endof
        keypad of  keypad-step  endof
    endcase ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: tail-tick ( -- )  stage @ off = if  exit  then  steps-due 0 ?do  stage @ off = if  leave  then  tail-frame  loop ;

\ ---- drawing: the digits entered (src: 16 x 16 icons at x -32 + 16 i, y 60) ----
-32 constant digits-x   60 constant digits-y   16 constant digit-pitch   16 constant digit-size
: digit-icon ( d -- image )  case  0 of  triangle-icon  endof  1 of  circle-icon  endof  2 of  cross-icon  endof
    drop square-icon 0  endcase ;
: draw-keypad ( -- )
    stage @ keypad = 0= if  exit  then  $FFFFFFFF pen-color
    #entered @ 0 ?do  i cells entered + @ digit-icon
        digits-x i digit-pitch * + digits-y digit-size digit-size sprite  loop ;
' draw-keypad on-draw

: pace ( -- )   \ src: state -2: by his door, between the first two points
    lookout 0 point-xz {: x z :}  x 0 z the-townsfolk send extra-place
    1 tail-point !  0 seen !  0e due f!  walk-on ;
: begin-tail ( -- )   \ src: state -1: his in-a-scene, then the way
    lookout route-thread 0< 0= if  lookout end-route  then
    ['] entry-80109E54 play-scene  0 seen !  0 misses !  0e due f!  intro stage ! ;
PRIVATE>
: the-robber-out? ( -- flag )  robber-out? ;   \ (a fact: the town's talk asks)

state: tail-state end-state
behaviour tails
  on spawned ( -- )  self subscribe tick  self subscribe area-entered ;
  on tick ( -- )  tail-tick ;
  on tail-pace ( -- )  pace ;
  on tail-start ( -- )  begin-tail ;
  on robber-start ( -- )  at-the-door ;
  on robber-spoken-to ( -- )  spoken-to ;
  on area-entered ( area in-a-scene -- )  drop the-town <> if  off stage !  0 tail-point !  then ;
end-behaviour

: spawn-tail ( -- id )  tails tail-state s" tail" spawn ;
