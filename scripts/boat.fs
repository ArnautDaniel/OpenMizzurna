\ boat.fs - the rowing boat (docs/subsystems/boat.md; src: ovl027.c boat_update and its boathouses, player_misc.c
\ boat_mode_begin / _end, pad_map.c's field mode 7): from the lake bar's landing he rows the boat (actor 103)
\ across the town's lake, sitting in it facing backwards: ✕ pulls ahead, □ backs water, L1 and R1 turn it, each
\ with the oars' motions (his and the boat's); the camera looks the way the boat goes. By either boathouse ○
\ lands there. The field leaves the pad to it. Others read rowing? and boat-prompt.
IN: boat
USING: engine actors messages keys areas figures floors matthew townsfolk events events.main events.ovl027 ui
    look ;

variable rowing
: rowing? ( -- flag )  rowing @ 0<> ;
variable boat-prompt   -1 boat-prompt !   \ (a fact: ○'s icon by a boathouse, for the HUD)

<PRIVATE
103 constant the-boat
\ the oars (src: boat_update: velocity and steering as the car's, its own limits)
0.4e fconstant pulling   0.2e fconstant backing   8e fconstant top   -6e fconstant top-back   20e fconstant units-per
0.1e fconstant coasting   -0.4e fconstant backing-still
8 constant steer-step   200 constant steer-turnover   10 constant steer-slow   2048 constant facing-back
fvariable velocity   variable steer   variable speed   variable bx   variable by   variable bz   variable bh
fvariable due
\ the motions (the town's own: his and the boat's, ahead and backing)
$1648 constant sitting   $1649 constant pull   $165A constant back-water
$164A constant left-oar   $165B constant left-oar-back   $164B constant right-oar   $165C constant right-oar-back
$164C constant boat-pull   $165D constant boat-back
$164D constant boat-left   $165E constant boat-left-back   $164E constant boat-right   $165F constant boat-right-back
variable his-motion   variable boat-motion
: show-motions ( his boat -- )   \ (each told only when it changes: a motion told again starts over)
    {: m b :}
    m his-motion @ <> if  m his-motion !  m -1 m sitting = s" matthew" actor-named send figure-anim  then
    b 0< if  exit  then   \ (none: the boat's goes on)
    b boat-motion @ <> if  b boat-motion !  the-boat b -1 false s" townsfolk" actor-named send person-anim  then ;
: backing? ( -- flag )  speed @ 0< ;
: oars ( -- his boat )   \ src: boat_update: the steering, then the pulling, each its motions
    sitting -1
    pad-l1 key-down? if
        steer-step steer +!  steer-turnover speed @ - steer-slow /  steer @ min steer !
        2drop  backing? if  left-oar-back boat-left-back  else  left-oar boat-left  then
    else  pad-r1 key-down? if
        steer-step negate steer +!  speed @ steer-turnover - steer-slow /  steer @ max steer !
        2drop  backing? if  right-oar-back boat-right-back  else  right-oar boat-right  then
    else
        steer @ 2 >= if  -1 steer +!  else  steer @ -1 < if  1 steer +!  else  0 steer !  then  then
    then  then
    pad-square key-down? if
        velocity f@ backing f- top-back fmax velocity f!  2drop back-water boat-back exit  then
    pad-cross key-down? if
        velocity f@ pulling f+ top fmin velocity f!  2drop pull boat-pull exit  then
    velocity f@ coasting f< 0= if  velocity f@ coasting f- velocity f!  exit  then   \ src: velocity_coast
    velocity f@ backing-still f< if  velocity f@ backing-still fnegate f+ velocity f!  exit  then
    0e velocity f! ;
: ahead ( x z h d -- x' z' )   \ d along heading h
    {: x z h d :}  h s>f 4096e f/ 2e f* pi f* {: F: a :}  x d s>f a fsin f* f>s +  z d s>f a fcos f* f>s + ;
1 constant the-towns-floor
: blocked-there? ( x z -- flag )  2dup the-towns-floor blocked?  -rot solid-billboard? or ;
: place-all ( -- )
    the-boat bx @ bz @ s" townsfolk" actor-named send person-place
    the-boat bh @ 4095 and s" townsfolk" actor-named send person-turn
    bx @ by @ bz @ s" matthew" actor-named send figure-place
    0 bh @ facing-back + 4095 and 0 s" matthew" actor-named send figure-turn ;
: row-frame ( -- )   \ a game frame (src: boat_update)
    oars show-motions
    velocity f@ units-per f* f>s speed !
    steer @ bh +!
    bx @ bz @ bh @ speed @ ahead {: nx nz :}
    nx nz blocked-there? if  0e velocity f!  0 speed !  exit  then   \ (src: back where it was, stopped)
    nx bx !  nz bz !  place-all ;
\ the boathouses (src: ovl027 tick, field mode 7: within 500 of each, ○'s prompt; ○ lands)
$71BC3 constant east-x   $70C0A constant east-z   $524DF constant west-x   $6C294 constant west-z
500 constant landing-reach   $10 constant landing-prompt
: by-the-landing? ( x z -- flag )  {: x z :}  bx @ x - s>f fdup f*  bz @ z - s>f fdup f* f+ fsqrt landing-reach s>f f< ;
variable landing-circle-was
: landing ( -- )
    -1 boat-prompt !
    pad-circle key-down? dup landing-circle-was @ 0= and  swap landing-circle-was !  {: o :}
    east-x east-z by-the-landing? if  landing-prompt boat-prompt !  o if  ['] events.ovl027:entry-8010A998 start-event  then  exit  then
    west-x west-z by-the-landing? if  landing-prompt boat-prompt !  o if  ['] events.ovl027:entry-8010A974 start-event  then  then ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: row-tick ( -- )
    rowing? 0= if  exit  then  steps-due {: n :}
    has-control? 0= if  exit  then   \ (a scene playing: the boat still)
    n 0 ?do  row-frame  loop  landing ;

\ ---- in and out (src: boat_mode_begin / _end) ----
: boat-begin ( -- )   \ (the boat brought into the town where it was put: its place the scene's)
    the-boat person-now {: x y z h :}  x bx !  y by !  z bz !  h bh !
    the-boat x y z s" townsfolk" actor-named send extra-place
    0e velocity f!  0 steer !  0 speed !  0e due f!  -1 his-motion !  -1 boat-motion !
    true rowing !  s" ui" actor-named send take-pad
    s" matthew" actor-named send figure-idle  false s" matthew" actor-named send figure-on-foot
    true s" camera" actor-named send camera-turned-round  place-all ;
: boat-end ( -- )
    rowing? 0= if  exit  then
    false rowing !  -1 boat-prompt !  s" ui" actor-named send give-pad
    true s" matthew" actor-named send figure-on-foot  false s" camera" actor-named send camera-turned-round ;
main-exe $8001A290 ' boat-begin native:
main-exe $8001A2C8 ' boat-end native:
PRIVATE>

state: boat-state end-state
behaviour the-boat-ride
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  ui-busy? 0= if  row-tick  then ;
end-behaviour

: spawn-boat ( -- id )  the-boat-ride boat-state s" boat" spawn ;
