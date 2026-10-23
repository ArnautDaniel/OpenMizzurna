\ matthew.fs - Matthew (docs/subsystems/matthew.md; src: player.c): his body in the world and his figure
\ (models/073_1.gltf: kind 0's model, CD.BIN 073_1, drawn in the area's textures). Walking, looking and acting
\ come next; for now the events place and turn him.
IN: matthew
USING: engine actors messages keys areas floors figures window ;


variable floor   1 floor !   \ his floor (src: a loaded game resumes on area 1's floor 1)

<PRIVATE   \ (his workings: the words below behaviour are his face to the others)
: figure-path ( -- addr len )  s" models/073_1.gltf" ;
variable figure

state: matthew-state end-state
create moving playing allot   -1 moving anim !

: floor-at ( x z -- y )   \ his floor's height there: in the town the ground (src: func_8001C264), else the floor's
    area @ the-town = if  ground-at if  exit  then  0 exit  then  2drop  area @ floor @ floor-height ;
: place ( x y z -- )   \ a height of 0 is "on his floor" (the scripts' set_pos; then the floor holds him)
    {: x y z :}  x  y ?dup 0= if  x z floor-at  then  z  3s>f  area @ floor @ body-place ;

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
fvariable speed   fvariable run-build   fvariable turn-rate   variable turning   fvariable idle
variable l-was   variable r-was
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
    pad-right key-down? if  1 turn  turn-right-motion show  else  pad-left key-down? if  -1 turn  turn-left-motion show
    else  0 turning !  0e turn-rate f!  then  then
    pad-r1 r-was press if  quarter-turn s>f turn-by  then
    pad-l1 l-was press if  quarter-turn negate s>f turn-by  then ;
: pace ( -- )   \ the speed this step
    0e speed f!
    pad-up key-down? if
        walk-speed speed f!
        pad-cross key-down? if  run-build f@ run-build-up frames f* f+ run-build-max fmin
        else  run-build f@ run-build-ease frames f* f- 0e fmax  then  run-build f!
        0e idle f!
    else
        run-build f@ run-build-down frames f* f- 0e fmax run-build f!
    then
    run-build f@ run-build-start f> if  run-build f@ run-build-start f- run-speed-per f* speed f@ f+ speed f!  then
    speed f@ f0> if  speed f@ run-anim-speed f>= if  run-motion  else  walk-motion  then  show  then
    pad-down key-down? speed f@ f0= and if  back-speed speed f!  back-motion show  0e run-build f!  0e idle f!  then ;
: stand ( -- )   \ standing still: the stand, then after a while the fidget
    speed f@ f0= turning @ 0= and if
        idle f@ frames f+ idle f!
        idle f@ idle-frames f>= if  fidget-motion fidget-loop-motion show-then  else  stand-motion show  then
    then ;
: probe ( d -- x z )   \ the point a step of the speed lands, its heading turned by d
    s>f self body-yaw f+ 2e f* pi f* 4096e f/
    self body-pos fswap fdrop   ( F: a x z )
    frot fdup fsin speed f@ f* frames f*  fswap fcos speed f@ f* frames f*   ( F: x z dx dz )
    frot f+ f>s  f+ f>s  swap ;
: open? ( x z -- flag )  floor @ blocked? 0= ;
: step-to ( x z -- )   \ his new place across the floor: indoors his height kept, in the town the ground's there
    {: x z :}  self body-pos 3f>s drop nip {: y :}
    area @ the-town = if  x z ground-at if  to y  then  then
    x y z  3s>f  self body-area self body-floor body-place ;
: walk ( -- )   \ src: player_update: a blocked step tries 0x30 at a time either side, easing his heading
    speed f@ f0= if  exit  then
    0  turning @ 0= speed f@ f0> and if
        0 probe open? 0= if
            slide-limit slide-step do
                i probe open? if  drop i  slide-ease frames f* turn-by  leave  then
                i negate probe open? if  drop i negate  slide-ease fnegate frames f* turn-by  leave  then
            slide-step +loop
        then
    then
    probe 2dup open? if  step-to
    else  2drop  0e speed f!  then ;
: control ( -- )
    controlled @ 0= self body? 0= or  window-up? or if  exit  then   \ (src: field_update: not while ui_busy)
    steer pace stand walk ;

PRIVATE>

behaviour the-player
  on spawned ( -- )  figure-path model figure !  self subscribe tick  self subscribe player-control ;
  on figure-place ( x y z -- )  place ;
  on tick ( -- )  control  moving advance if  moving hold-move @ if  keep-move  then  then ;
  on player-control ( flag -- )  controlled ! ;
  on figure-anim ( id next loop -- )  moving play ;
  on figure-hold ( -- )  true moving hold-move ! ;
  on figure-turn ( rx ry rz -- )  drop nip s>f body-turn ;   \ (his heading: the turn about the vertical)
  on area-entered ( area scene -- )  drop  self body? if  body-area!  else  drop  then ;
end-behaviour

: matthew-actor ( -- id )  s" matthew" actor-named ;
: matthew-there? ( -- flag )  matthew-actor dup 0< if  drop false  else  body?  then ;
: matthew-now ( -- x y z yaw )   \ where he stands and his heading, in whole units (a fact: his body)
    matthew-actor {: id :}  id body-pos 3f>s  id body-yaw f>s ;
: anim-done? ( -- flag )  moving done @  moving anim @ 0< or ;   \ (a fact: the events' wait-anim)
: draw-matthew ( -- )
    matthew-there? if
        figure @  moving anim @ 0< if  0 0  else  moving clip @ moving frame  then  matthew-now draw-figure
    then ;
' draw-matthew on-draw

: spawn-matthew ( -- id )  the-player matthew-state s" matthew" spawn  dup subscribe area-entered ;
