\ shootout.fs - a shoot-out (docs/subsystems/shootout.md; src: qte.c, ovl005 / ovl026 shootout_update): button
\ targets on a foe's body. A round's rows come one after another (the area's table): each puts up to four
\ targets on parts of the foe - a face button each - for a few frames, with so many shots. A press spends a shot;
\ the right button clears its target. When the targets' time is up: all cleared, a hit (the foe's health down);
\ any left, a miss - the foe shoots back (the round's scene), Matthew's health down, and on. Ten hits win the
\ round (its scene); the last round's won, the shoot-out's over. Out of health: the round's scene, game over,
\ and from the start again for ○ or Start. The camera follows the round's own path round the foe; a step at the
\ original's rate (15 a second indoors). An area describes its shoot-out in a plan.
IN: shootout
USING: engine actors messages keys screen areas figures matthew townsfolk events ;

\ ---- facts ----
variable shootout-going   \ a shoot-out is on (src: D_800A3578 / D_800A3570)
: shooting? ( -- flag )  shootout-going @ 0<> ;

\ ---- a plan: what an area gives (its rounds follow it) ----
begin-structure shootout-plan
  field: plan-table   field: plan-table-len   \ the rounds' table (the area's: data/exetables.c)
  field: plan-rounds
  field: plan-him-x   field: plan-him-z   field: plan-stance       \ where he stands, his stance (-1 none)
  field: plan-shot-motion   field: plan-aimed-motion                \ his shot and after it (-1 none)
  field: plan-foe-x   field: plan-foe-z   field: plan-foe-heading     \ where the first foe stands
  field: plan-foe-stance   field: plan-foe-hit                        \ the foes' stance, hit (-1 none)
  field: plan-full-0   field: plan-full-1                             \ the foes' full health (by pool)
  field: plan-won                                                     \ when it's won (an xt; 0 none)
end-structure
begin-structure shootout-round
  field: round-pool   field: round-per-hit   field: round-miss-damage   \ whose health, a hit's, a miss's
  field: round-missed   field: round-won   field: round-lost            \ the scenes (xts)
end-structure
: round# ( plan i -- round )  shootout-round * + shootout-plan + ;
: plan-bytes ( rounds -- n )  shootout-round * shootout-plan + ;
: round! ( plan i pool per-hit miss-damage missed won lost -- )
    {: p i pool hit miss m w l :}  p i round# {: r :}
    pool r round-pool !  hit r round-per-hit !  miss r round-miss-damage !  m r round-missed !  w r round-won !
    l r round-lost ! ;

<PRIVATE
100 constant full-health   10 constant hits-to-win   4 constant #targets
\ the table: a round's rows (13 bytes: slot, frames up - 1, frames, shots, 0xFF at the end, 4 x part button),
\ then its camera's steps (36 bytes)
836 constant round-bytes   224 constant path-offset   13 constant row-bytes   36 constant step-bytes
4 constant row-key   5 constant row-targets   $FF constant the-end   $FF constant no-button
12 constant eye-at   16 constant look-at   20 constant dist-at   24 constant turn-at   28 constant frames-at
32 constant moves-at   0 constant cut   1 constant tween   2 constant turning-up   3 constant turning-down
1 constant triangle   2 constant square   3 constant cross   4 constant circle
4 constant burst-frames

variable plan   variable round   variable stage
enum: shootout-stage  aiming  won-scene  missed-scene  final-scene  lost-scene  game-over ;
variable health   variable health-shown   create pools 2 cells allot   create pools-shown 2 cells allot
variable hits   variable scene-thread
fvariable due

: rounds-table ( -- addr len )  plan @ plan-table @  plan @ plan-table-len @ ;
: this-round ( -- r )  plan @ round @ round# ;
: round-base ( -- n )  round @ round-bytes * ;
: row-u8 ( row k -- n )  swap row-bytes * +  round-base + {: at :}  rounds-table at exe-u8 ;
: step-base ( k -- n )  step-bytes * path-offset + round-base + ;
: step-s16 ( k off -- n )  swap step-base + 2/ {: i :}  rounds-table i exe-s16 ;
: step-slot ( k -- slot )  step-base 4 / {: i :}  rounds-table i exe-s32 ;
: step-place ( k -- x z )  step-base 4 / {: i :}  rounds-table i 1+ exe-s32  rounds-table i 2 + exe-s32 ;
: step-moves ( k -- kind )  step-base moves-at + {: i :}  rounds-table i exe-u8 ;

\ ---- the rows (src: qte_row_update, qte_check) ----
begin-structure target
  field: part   field: button   field: up   field: burst
end-structure
create targets  target #targets * allot
: target# ( i -- t )  target * targets + ;
variable row   variable row-left   variable window   variable shots   variable foe
: targets-shown? ( -- flag )  window @ 2 >= ;
: load-target ( i -- )
    {: i :}  i target# {: t :}  row @ row-targets i 2* + row-u8 t part !  row @ row-targets i 2* + 1+ row-u8 t button !
    t button @ no-button <> t up !  t up @ if  0  else  burst-frames  then  t burst ! ;
: load-row ( -- )
    row @ row-key row-u8 the-end = if  0 row !  exit  then
    row @ 0 row-u8 foe !  row @ 1 row-u8 1+ window !  row @ 2 row-u8 row-left !  row @ 3 row-u8 shots !
    #targets 0 do  i load-target  loop  1 row +! ;
: bursting ( t -- )  {: t :}  t up @ 0=  t burst @ burst-frames < and if  1 t burst +!  then ;   \ (a hit's burst)
: row-frame ( -- )
    row-left @ 0= if  load-row exit  then
    -1 row-left +!
    targets-shown? if  #targets 0 do  i target# bursting  loop  then
    window @ if  -1 window +!  then ;
: targets-left ( -- n )  0  #targets 0 do  i target# up @ if  1+  then  loop ;
: this-button? ( b t -- flag )  {: b t :}  t up @  t button @ b = and ;
: hit-target? ( b -- flag )   \ a target up with that button: down
    #targets 0 do  dup i target# this-button? if  drop  false i target# up !  true unloop exit  then  loop  drop false ;
: rows-reset ( -- )  0 row !  0 row-left !  0 window !  0 shots !  #targets 0 do  false i target# up !  loop ;   \ src: qte_reset

\ ---- the camera's path (src: camera_path_update) ----
begin-structure view
  field: view-eye   field: view-look   field: view-dist   field: view-turn
end-structure
create cam view allot   create cam-step view allot
variable path-step   variable path-left   variable path-moves   variable cam-x   variable cam-y   variable cam-z
: per-frame ( from to n -- d )  {: a b n :}  b a - n / ;
: turn-per-frame ( from to n -- d )  {: a b n :}  b a - abs n / ;
: centre ( k -- )   \ on its slot (or its place)
    {: k :}  k step-slot {: s :}
    s 0< if  k step-place cam-z !  cam-x !  0 cam-y !  exit  then
    s person-now drop cam-z !  cam-y !  cam-x ! ;
: set-steps ( k n -- )   \ toward step k + 1 over n frames
    {: k n :}  path-moves @ {: m :}  cam-step view erase
    m tween turning-down 1+ within 0= if  exit  then
    cam view-eye @   k 1+ eye-at step-s16   n per-frame cam-step view-eye !
    cam view-look @  k 1+ look-at step-s16  n per-frame cam-step view-look !
    cam view-dist @  k 1+ dist-at step-s16  n per-frame cam-step view-dist !
    m tween = if  exit  then
    cam view-turn @  k 1+ turn-at step-s16  n turn-per-frame  m turning-down = if  negate  then  cam-step view-turn ! ;
: next-step ( -- )
    path-step @ {: k :}  k step-moves path-moves !
    path-moves @ the-end = if  0 path-step !  0 path-left !  exit  then
    k centre  k eye-at step-s16 cam view-eye !  k look-at step-s16 cam view-look !  k dist-at step-s16 cam view-dist !
    k turn-at step-s16 cam view-turn !  k frames-at step-s16 path-left !
    k path-left @ 1 max set-steps  1 path-step +! ;
: path-frame ( -- )
    path-left @ 0= if  next-step exit  then
    cam-step view-eye @ cam view-eye +!  cam-step view-look @ cam view-look +!  cam-step view-dist @ cam view-dist +!
    cam-step view-turn @ cam view-turn +!  -1 path-left +! ;
: aim-camera ( -- )
    s" camera" actor-named {: c :}  c send camera-fixed
    cam view-look @  cam view-dist @  cam view-eye @  cam-x @ cam-y @ cam-z @  cam view-turn @  c send camera-place ;
: path-reset ( -- )  0 path-step !  0 path-left ! ;

\ ---- the pad: the face buttons, a press each (src: field mode 6) ----
create was-down circle 1+ cells allot   variable pressed   \ (the buttons pressed since the last step: a bit each)
: button-key ( b -- key )  case  triangle of  pad-triangle  endof  square of  pad-square  endof
    cross of  pad-cross  endof  drop pad-circle 0  endcase ;
: watch-button ( b -- )
    {: b :}  b button-key key-down? {: d :}
    d  b cells was-down + @ 0= and if  1 b lshift pressed @ or pressed !  then  d b cells was-down + ! ;
: watch-pad ( -- )  circle 1+ triangle do  i watch-button  loop ;
: shot ( -- b )   \ the first pressed, by priority (0 none)
    circle 1+ triangle do  1 i lshift pressed @ and if  i unloop exit  then  loop  0 ;
: forget-presses ( -- )  0 pressed !  circle 1+ triangle do  i button-key key-down? i cells was-down + !  loop ;

\ ---- the people ----
: him ( -- id )  s" matthew" actor-named ;
: foe-motion ( id -- )   \ the round's foe in it, looping (-1: none)
    dup 0< if  drop exit  then  foe @ swap -1 true the-townsfolk send person-anim ;
: foe-faces ( -- )  foe @  cam view-turn @ 2048 + 4095 and  the-townsfolk send person-turn ;
: him-shown ( flag -- )  him send figure-shown ;
: play-scene ( xt -- )  launch-event scene-thread ! ;
: scene-over? ( -- flag )  scene-thread @ event-alive? 0= ;

\ ---- a step ----
: fresh-health ( -- )
    full-health health !  full-health health-shown !
    plan @ plan-full-0 @ dup pools !  pools-shown !  plan @ plan-full-1 @ dup pools cell+ !  pools-shown cell+ ! ;
: set-up ( -- )   \ src: shootout_update's first frame
    plan @ {: p :}
    false him-shown  false him send figure-on-foot
    p plan-him-x @ 0 p plan-him-z @ him send figure-place
    p plan-stance @ dup 0< if  drop  else  -1 true him send figure-anim  then
    0 round !  0 0 row-u8 foe !
    foe @ p plan-foe-x @ p plan-foe-z @ the-townsfolk send person-place
    foe @ p plan-foe-heading @ the-townsfolk send person-turn
    p plan-foe-stance @ foe-motion
    rows-reset  path-reset  0 hits !  fresh-health  aiming stage !  forget-presses ;
: pool-of ( -- addr )  this-round round-pool @ cells pools + ;
: hit ( -- )
    1 hits +!  this-round round-per-hit @ negate pool-of +!
    plan @ plan-foe-hit @ dup 0< if  drop  else  foe @ swap plan @ plan-foe-stance @ false the-townsfolk send person-anim  then
    hits @ hits-to-win = if  rows-reset  path-reset  0 hits !  won-scene stage !
        round @ 1+ plan @ plan-rounds @ = if  final-scene stage !  true him-shown  then
        this-round round-won @ play-scene  then ;
: missed ( -- )  missed-scene stage !  this-round round-missed @ play-scene ;
: fire ( -- )   \ a shot spent (his shot's motion; the sound, the flash and the pad's rumble: to come)
    -1 shots +!  plan @ plan-shot-motion @ dup 0< if  drop exit  then
    plan @ plan-aimed-motion @ false him send figure-anim ;
: aim-frame ( -- )
    row-frame  path-frame  aim-camera  foe-faces
    shots @ if  shot  else  0  then {: b :}
    targets-shown? if  b if  b hit-target? drop  then
    else  window @ 1 = if  targets-left if  missed  else  hit  then  then  then
    shots @  pressed @ 0<> and if  fire  then  0 pressed ! ;
: next-round ( -- )   \ its scene over
    1 round +!  0 row-left !  0 path-left !  aiming stage !
    0 0 row-u8 foe !  plan @ plan-foe-stance @ foe-motion ;
: back-to-it ( -- )   \ the miss's scene over: its damage, on with the round
    this-round round-miss-damage @ negate health +!  0 row-left !  0 path-left !  aiming stage !
    plan @ plan-foe-stance @ foe-motion ;
: won-it ( -- )
    false shootout-going !  true him-shown  true him send figure-on-foot
    plan @ plan-won @ ?dup if  execute  then ;
: lost ( -- )  lost-scene stage !  true him-shown  this-round round-lost @ play-scene ;
4 constant game-over-effect
: try-again? ( -- flag )  shot circle =  pad-start key-down? or ;
: shootout-frame ( -- )
    stage @ case
        aiming of  aim-frame  endof
        won-scene of  scene-over? if  next-round  then  endof
        missed-scene of  scene-over? if  back-to-it  then  endof
        final-scene of  scene-over? if  won-it  then  endof
        lost-scene of  scene-over? if  game-over stage !  game-over-effect s" effects" actor-named send screen-effect  then  endof
        game-over of  try-again? if  set-up  then  0 pressed !  endof   \ (src: music 0xA again: to come)
    endcase
    stage @ aiming =  health @ 0 <= and if  1 health !  lost  then
    health @ health-shown @ < if  -1 health-shown +!  then
    2 0 do  i cells pools + @  i cells pools-shown + @ < if  -1 i cells pools-shown + +!  then  loop ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: shootout-tick ( -- )
    shooting? 0= if  exit  then
    watch-pad  steps-due 0 ?do  shooting? 0= if  leave  then  shootout-frame  loop ;

\ ---- drawing: the targets, the bullets, the bars (src: qte_draw_targets, shootout_ammo_draw, hp_bar_draw) ----
$FFF0A0FF constant burst-ink   $202020FF constant mark-ink   $C8A040FF constant bullet-ink
8 constant target-half   16 constant bullet-pitch   -140 constant bullets-x   40 constant bullets-y
6 constant bullet-w   24 constant bullet-h
-140 constant his-bar-x   80 constant his-bar-y   18 constant foe-bar-x   -80 constant foe-bar-y
: button-ink ( b -- rgba )  case  triangle of  triangle-ink  endof  square of  square-ink  endof
    cross of  cross-ink  endof  circle-ink swap  endcase ;
: square-at ( px py half ink -- )   \ (window pixels; half in the original's)
    pen-color  scale * {: px py h :}  px h - py h - h 2* h 2* draw-rect ;
: target-at ( t px py -- )   \ its button; or, hit, its burst
    {: t px py :}
    t up @ if  px py target-half t button @ button-ink square-at  px py target-half 2/ mark-ink square-at  exit  then
    px py target-half t burst @ 2* + burst-ink square-at ;
: draw-target ( t -- )
    {: t :}  t up @  t burst @ burst-frames < or 0= if  exit  then
    t  foe @ person-pose  t part @  part-on-screen if  target-at  else  drop  then ;
: draw-targets ( -- )  targets-shown? if  #targets 0 do  i target# draw-target  loop  then ;
: draw-bullets ( -- )
    bullet-ink pen-color  shots @ 0 ?do
        bullets-x i bullet-pitch * + bullets-y >screen  bullet-w scale *  bullet-h scale *  draw-rect  loop ;
: foe-full ( pool -- n )  if  plan @ plan-full-1 @  else  plan @ plan-full-0 @  then ;
: draw-shootout ( -- )
    shooting? 0= if  exit  then
    stage @ game-over = if  draw-game-over exit  then
    stage @ aiming = if  draw-targets  draw-bullets  then
    stage @ lost-scene = stage @ final-scene = or if  exit  then
    his-bar-x his-bar-y health-shown @ full-health health-bar
    this-round round-pool @ {: k :}  foe-bar-x foe-bar-y  k cells pools-shown + @  k foe-full  health-bar ;
' draw-shootout on-draw

: start ( plan -- )  plan !  true shootout-going !  0e due f!  set-up ;
PRIVATE>

state: shootout-state end-state
behaviour shoot-outs
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  shootout-tick ;
  on shootout-start ( plan -- )  start ;
end-behaviour

: spawn-shootout ( -- id )  shoot-outs shootout-state s" shootout" spawn ;
