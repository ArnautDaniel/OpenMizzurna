\ matthew.fs - Matthew (docs/subsystems/matthew.md; src: player.c): his body in the world and his figure
\ (models/073_1.gltf: kind 0's model, CD.BIN 073_1, drawn in the area's textures). Walking, looking and acting
\ come next; for now the events place and turn him.
IN: matthew
USING: engine actors messages keys areas floors figures window ui ;


variable floor   1 floor !   \ his floor (src: a loaded game resumes on area 1's floor 1)

<PRIVATE   \ (his workings: the words below behaviour are his face to the others)
: figure-path ( -- addr len )  s" models/073_1.gltf" ;
variable figure

state: matthew-state end-state
create moving playing allot   -1 moving anim !

: floor-at ( x z -- y )   \ his floor's height there: in the town the ground (src: func_8001C264), else the floor's
    area @ the-town = if  ground-at if  exit  then  0 exit  then  2drop  area @ floor @ floor-height ;
: place ( x y z -- )   \ a height of 0 is "on his floor" (the scripts' set_pos; then the floor holds him)
    {: x y z :}  x  y ?dup 0= if  x z floor-at  then  z  3s>f  area @ floor @ body-place
    false moving hold-move ! ;   \ (put somewhere, a held motion's move no longer carries him)

\ at a held motion's end he stays where it moved him (src: func_8001CA5C: the last frame's root move, turned
\ by his heading, added to his place as it is)
: keep-move ( -- )
    moving clip @ dup motion-frames 1- motion-move  {: side up fwd :}
    self body-yaw 2e f* pi f* 4096e f/  {: F: h :}
    self body-pos  {: F: x F: y F: z :}
    x  fwd s>f h fsin f* f+  side s>f h fcos f* f+
    y  up s>f f-
    z  fwd s>f h fcos f* f+  side s>f h fsin f* f-
    self body-area self body-floor body-place ;

\ ---- walking (src: player_control_foot, player_update) ----
\ The original's numbers are per game frame, and it ran 15 frames a second indoors, ~30 in the town: each step
\ here covers `frames` of them (rate x dt), so he walks and turns as fast as he did there.
15e fconstant indoor-frames   30e fconstant town-frames
: frames ( F: -- n )  area @ the-town = if  town-frames  else  indoor-frames  then  dt f* ;
60e fconstant walk-speed         \ world units a frame (0x3C)
-40e fconstant back-speed        \ stepping back (-0x28)
38e fconstant run-build-max      \ the run builds up 4 a frame to 0x26 ...
4e fconstant run-build-up   2e fconstant run-build-down   1e fconstant run-build-ease
6e fconstant run-build-start     \ ... and from 7 adds 3 a frame of speed for each step past 6
3e fconstant run-speed-per
61e fconstant run-anim-speed     \ from this speed he runs (0x3D)
\ turning builds up: standing 16 a frame to 100, walking 10 to 50, running (speed 100 up) 6 to 30 (4096 a turn)
16e fconstant stand-turn-step  100e fconstant stand-turn-max
10e fconstant walk-turn-step   50e fconstant walk-turn-max
6e fconstant run-turn-step     30e fconstant run-turn-max    100e fconstant running-speed
1024 constant quarter-turn       \ L1 / R1
48 constant slide-step   384 constant slide-limit   50e fconstant slide-ease   \ src: player_update's probing
120e fconstant idle-frames       \ standing this long, he fidgets
\ his motions (the common set)
0 constant stand-motion   1 constant walk-motion   2 constant back-motion   3 constant turn-left-motion
4 constant turn-right-motion   5 constant run-motion   20 constant fidget-motion   22 constant fidget-loop-motion

variable controlled              \ the player's to move
variable on-foot   true on-foot !   \ the pad walks him (not on a stool: src pad field mode 0)
fvariable speed   fvariable run-build   fvariable turn-rate   variable turning   fvariable idle
variable l-was   variable r-was
variable pushing   -1 pushing !   \ the crate he pushes (-1: none - src: g_pushing, g_pushed_actor)
variable perch                     \ 0 on his feet, 1 on a crate, 2 on a stool (src: talk_state: the check rows')
$BA constant push-walk   $BD constant push-back   $BB constant push-right   $BC constant push-left
$B4 constant crate-ahead            \ the crate before him as he pushes (src: player_update: 0xB4)
: pushing? ( -- flag )  pushing @ 0< 0= ;
: motion-for ( normal pushed -- id )  pushing? if  nip  else  drop  then ;
: show ( id -- )   \ a motion of his own: started if it isn't the one playing (src: actor_anim's restart rule)
    dup moving anim @ = if  drop exit  then  -1 true moving play ;
: show-then ( id next -- )  over moving anim @ = if  2drop exit  then  true moving play ;
: turn-by ( F: d -- )  self body-yaw f+ body-turn ;
: turn ( dir -- )   \ -1 left, 1 right: the turn builds up
    dup turning @ <> if  0e turn-rate f!  then  dup turning !
    speed f@ f0= if  stand-turn-step stand-turn-max  else  speed f@ running-speed f< if  walk-turn-step walk-turn-max
    else  run-turn-step run-turn-max  then  then   ( F: step max )
    fswap frames f* turn-rate f@ f+ fmin turn-rate f!
    s>f turn-rate f@ f* frames f* turn-by
    0e idle f! ;
: press ( key var -- flag )  >r key-down? dup r@ @ 0= and swap r> ! ;
: steer ( -- )
    pad-right key-down? if  1 turn  turn-right-motion push-right motion-for show
    else  pad-left key-down? if  -1 turn  turn-left-motion push-left motion-for show
    else  0 turning !  0e turn-rate f!  then  then
    pad-r1 r-was press if  quarter-turn s>f turn-by  then
    pad-l1 l-was press if  quarter-turn negate s>f turn-by  then ;
: pace ( -- )   \ the speed this step
    0e speed f!
    pad-up key-down? if
        walk-speed speed f!
        pad-cross key-down? pushing? 0= and if  run-build f@ run-build-up frames f* f+ run-build-max fmin
        else  run-build f@ run-build-ease frames f* f- 0e fmax  then  run-build f!
        0e idle f!
    else
        run-build f@ run-build-down frames f* f- 0e fmax run-build f!
    then
    run-build f@ run-build-start f> if  run-build f@ run-build-start f- run-speed-per f* speed f@ f+ speed f!  then
    speed f@ f0> if  speed f@ run-anim-speed f>= if  run-motion  else  walk-motion push-walk motion-for  then  show  then
    pad-down key-down? speed f@ f0= and if
        back-speed speed f!  back-motion push-back motion-for show  0e run-build f!  0e idle f!  then ;
: stand ( -- )   \ standing still: the stand, then after a while the fidget
    speed f@ f0= turning @ 0= and  pushing? 0= and if
        idle f@ frames f+ idle f!
        idle f@ idle-frames f>= if  fidget-motion fidget-loop-motion show-then  else  stand-motion show  then
    then ;
: probe ( d -- x z )   \ the point a step of the speed lands, its heading turned by d
    s>f self body-yaw f+ 2e f* pi f* 4096e f/
    self body-pos fswap fdrop   ( F: a x z )
    frot fdup fsin speed f@ f* frames f*  fswap fcos speed f@ f* frames f*   ( F: x z dx dz )
    frot f+ f>s  f+ f>s  swap ;
: open? ( x z -- flag )  floor @ blocked? 0= ;
: free? ( x z -- flag )  2dup open? if  in-the-way? 0=  else  2drop false  then ;   \ src: player_probe_blocked
: cordoned? ( x z -- flag )   \ src: foot_road_block: in the town, the cordon's tiles but their zone 0
    area @ the-town <> if  2drop false exit  then
    2dup tile-key cordoned-tile? 0= if  2drop false exit  then
    floor @ zone-at 0<>  cordon-holds? and ;
: clear? ( x z -- flag )   \ (src: player_update's check after the move: no wall, billboard or cordon there)
    2dup open? 0= if  2drop false exit  then  2dup solid-billboard? if  2drop false exit  then  cordoned? 0= ;
variable was-x   variable was-y   variable was-z   \ where he stood before his last step (src: player_restore_pos)
: step-to ( x z -- )   \ his new place across the floor: indoors his height kept, in the town the ground's there
    {: x z :}  self body-pos 3f>s  was-z !  dup was-y !  swap was-x ! {: y :}
    area @ the-town = if  x z ground-at if  to y  then  then
    x y z  3s>f  self body-area self body-floor body-place ;
: push-along ( -- )   \ the crate before him, 0xB4 on; blocked, he stops where he was (src: player_update)
    pushing? 0= if  exit  then
    self body-yaw 2e f* pi f* 4096e f/ {: F: a :}  self body-pos {: F: x F: y F: z :}
    x a fsin crate-ahead s>f f* f+ f>s  z a fcos crate-ahead s>f f* f+ f>s {: cx cz :}
    cx cz open? 0= if  was-x @ was-y @ was-z @ 3s>f self body-area self body-floor body-place  0e speed f!  exit  then
    pushing @ cx 0 cz s" townsfolk" actor-named send extra-place ;
: walk ( -- )   \ src: player_update: a blocked step tries 0x30 at a time either side, easing his heading
    speed f@ f0= if  exit  then
    0  turning @ 0= speed f@ f0> and if
        0 probe free? 0= if
            slide-limit slide-step do
                i probe free? if  drop i  slide-ease frames f* turn-by  leave  then
                i negate probe free? if  drop i negate  slide-ease fnegate frames f* turn-by  leave  then
            slide-step +loop
        then
    then
    probe 2dup clear? if  step-to  push-along
    else  2drop  0e speed f!  then ;
\ ---- moved by a scene (src: actor_nav_step, actor_update's move_anim and turn - the events' walk-to, walk-forward,
\ turn-to): toward a point at a speed a game frame, or straight on until idle; and turning at a speed ----
variable heading-there   variable there-x   variable there-z   variable there-stride
variable ahead   variable ahead-motion
variable turning-to   variable turn-target   variable turn-speed
: yaw ( -- heading )  self body-yaw f>s full-circle 1- and ;
: along ( heading speed -- )   \ a step of the speed that way
    {: h v :}  h s>f 2e f* pi f* 4096e f/ {: F: a :}  v s>f frames f* {: F: d :}
    self body-pos fswap fdrop  {: F: x F: z :}  x a fsin d f* f+ f>s  z a fcos d f* f+ f>s  step-to ;
: go-there ( -- )
    there-x @ there-z @  self body-pos fswap fdrop f>s f>s swap  {: x z mx mz :}
    x mx -  z mz -  {: dx dz :}
    dx abs s>f there-stride @ s>f frames f* f<  dz abs s>f there-stride @ s>f frames f* f< and if
        x z step-to  0 heading-there !  stand-motion -1 true moving play  exit  then
    dx s>f dz s>f fatan2 4096e f* 2e pi f* f/ f>s {: h :}  h s>f body-turn  h there-stride @ along ;
: scene-moves ( -- )
    heading-there @ if  go-there  then
    ahead @ if  yaw ahead @ along  then
    turning-to @ if  yaw turn-target @ turn-speed @ heading-step  if  0 turning-to !  then  s>f body-turn  then ;

\ ---- crawling (src: ovl019's duct crawl, the area's minigame update): the pad drives him, tank-like, while a
\ scene runs - ahead 0x28 a frame, back 0x1E, turning 0x80; a move that would put a wall within three of his
\ lengths (0x78 each) ahead is undone ----
variable crawling
variable unseen   \ not drawn (src: actors[0].visible 0: the shoot-outs' view)
40e fconstant crawl-ahead   -30e fconstant crawl-back   128e fconstant crawl-turn   120 constant crawl-length
$1006 constant crawl-motion   $1005 constant crawl-still-motion
: clear-ahead? ( -- flag )   \ his place and two lengths on, the way he faces, all open
    self body-yaw 2e f* pi f* 4096e f/ {: F: a :}  self body-pos fswap fdrop {: F: x F: z :}
    3 0 do
        x a fsin i crawl-length * s>f f* f+ f>s  z a fcos i crawl-length * s>f f* f+ f>s  open? 0= if  false unloop exit  then
    loop  true ;
: crawl-step ( F: d -- )   \ ahead (or back) that far a frame, the way he faces
    self body-yaw 2e f* pi f* 4096e f/ {: F: d F: a :}  self body-pos {: F: x F: y F: z :}
    x a fsin d f* frames f* f+  y  z a fcos d f* frames f* f+  self body-area self body-floor body-place ;
variable crawl-shown
: crawl ( -- )
    self body? 0= if  exit  then
    self body-pos  self body-yaw {: F: x F: y F: z F: h :}  crawl-still-motion crawl-shown !
    pad-up key-down? if  crawl-ahead crawl-step  crawl-motion crawl-shown !  then
    pad-down key-down? if  crawl-back crawl-step  crawl-motion crawl-shown !  then
    pad-right key-down? if  crawl-turn frames f* turn-by  crawl-motion crawl-shown !  then
    pad-left key-down? if  crawl-turn fnegate frames f* turn-by  crawl-motion crawl-shown !  then
    clear-ahead? 0= if   \ (blocked: where he was, as he faced)
        x y z self body-area self body-floor body-place  h body-turn  crawl-still-motion crawl-shown !  then
    crawl-shown @ show ;

: control ( -- )
    controlled @ 0= self body? 0= or  window-up? or  on-foot @ 0= or if  exit  then   \ (src: field_update: not while ui_busy)
    steer pace stand walk ;

PRIVATE>

behaviour the-player
  on spawned ( -- )  figure-path model figure !  self subscribe tick  self subscribe player-control ;
  on figure-place ( x y z -- )  0 heading-there !  0 ahead !  place ;
  on tick ( -- )  ui-busy? if  exit  then   \ (src: field_update: no player_update while ui_busy)
    crawling @ if  crawl  then  control  scene-moves
    moving  unseen @  self body? 0= or if  advance  else  self body-pos 3f>s nip  advance-heard  then
    if  moving hold-move @ if  keep-move  then  then ;
  on player-control ( flag -- )  controlled ! ;
  on figure-on-foot ( flag -- )  on-foot ! ;
  on figure-pushes ( crate -- )  pushing ! ;   \ (-1: none)
  on figure-perch ( n -- )  perch ! ;
  on figure-crawl ( flag -- )  crawling ! ;
  on figure-coord ( which v -- )   \ one of his place's
    self body? 0= if  2drop exit  then  {: w v :}  self body-pos {: F: x F: y F: z :}
    w 0 = if  v s>f to x  then  w 1 = if  v s>f to y  then  w 2 = if  v s>f to z  then
    x y z  self body-area self body-floor body-place ;
  on figure-nudge ( dx dy dz -- )
    self body? 0= if  drop 2drop exit  then  {: dx dy dz :}  self body-pos {: F: x F: y F: z :}
    x dx s>f f+  y dy s>f f+  z dz s>f f+  self body-area self body-floor body-place ;
  on figure-shown ( flag -- )  0= unseen ! ;
  on figure-floor ( floor -- )   \ src: actor_set_floor: his floor, and its height under him
    floor !  self body? 0= if  exit  then  self body-pos {: F: x F: y F: z :}
    x  area @ floor @ floor-height s>f  z  area @ floor @ body-place ;
  on figure-anim ( id next loop -- )  moving play ;
  on figure-hold ( -- )  true moving hold-move ! ;
  on figure-walk ( x z speed motion -- )
    {: x z v m :}  x there-x !  z there-z !  v there-stride !  true heading-there !  m -1 true moving play ;
  on figure-forward ( speed motion -- )   \ (-1: the motion as it is)
    {: v m :}  v ahead !  m ahead-motion !  m 0< 0= if  m -1 true moving play  then ;
  on figure-idle ( -- )  0 heading-there !  0 ahead !  stand-motion -1 true moving play ;
  on figure-turning ( target speed -- )
    {: t v :}  v 0= if  t s>f body-turn  0 turning-to !  exit  then
    t full-circle 1- and turn-target !  v turn-speed !  true turning-to ! ;
  on figure-turn ( rx ry rz -- )  drop nip s>f body-turn ;   \ (his heading: the turn about the vertical)
  on area-entered ( area scene -- )   \ (off any stool: the stools are the area's)
    drop  self body? if  body-area!  else  drop  then
    on-foot @ 0= if  true on-foot !  0 s" townsfolk" actor-named send seat-leave  then   \ (0: his slot)
    0 perch !  -1 pushing !  false unseen ! ;   \ (and off any crate; seen)
end-behaviour

: matthew-actor ( -- id )  s" matthew" actor-named ;
: matthew-there? ( -- flag )  matthew-actor dup 0< if  drop false  else  body?  then ;
: matthew-now ( -- x y z yaw )   \ where he stands and his heading, in whole units (a fact: his body)
    matthew-actor {: id :}  id body-pos 3f>s  id body-yaw f>s ;
: matthew-was ( -- x y z )  was-x @ was-y @ was-z @ ;   \ where he stood before his last step (src: player_restore_pos's)
: matthew-walking? ( -- flag )  heading-there @ 0<> ;   \ (facts: the events' wait-stop, wait-turned)
4 constant perch-in-car   \ (driving: src partner = the car)
: matthew-perch ( -- n )  perch @ ;   \ 0 on his feet, 1 on a crate, 2 on a stool, 3 holding a crate, 4 in the car
: matthew-crawling? ( -- flag )  crawling @ 0<> ;
: matthew-pushing? ( -- flag )  pushing @ 0< 0= ;
: matthew-on-foot? ( -- flag )  on-foot @ 0<> ;
: matthew-turned? ( -- flag )  turning-to @ 0= ;
: matthew-anim-now ( -- id next )  moving dup anim @  swap next-anim @ ;   \ (a fact: his motion, what follows)
: anim-done? ( -- flag )  moving done @  moving anim @ 0< or ;   \ (a fact: the events' wait-anim)
: draw-matthew ( -- )
    matthew-there? unseen @ 0= and if
        figure @  moving anim @ 0< if  0 0  else  moving clip @ moving frame  then  matthew-now draw-figure
    then ;
' draw-matthew on-draw

: spawn-matthew ( -- id )  the-player matthew-state s" matthew" spawn  dup subscribe area-entered ;
