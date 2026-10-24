\ driving.fs - Matthew driving his car in the town (docs/subsystems/driving.md; src: ovl027 car_update, cevcon.c
\ func_8004175C / func_8004301C / car_exit / car_can_exit, hud.c fuel_gauge_draw). ○ at the car gets him in (its
\ scene). Driving: ✕ speeds up 0.4 a frame (to 8, or 2 with an empty tank; a unit of fuel each 201 frames of it),
\ □ slows and backs (to -2), neither coasts (0.1 a frame); speed is that × 60 units a frame. Left / right steer 8 a
\ frame up to min(speed / 5, (800 - speed) / 12), easing back 4 when let go; the steering turns the car only while it
\ moves. A wall ahead: the car tries a little more each way (0x20 at a time, to 0x200) and steers 0x32 toward the
\ free side; still hitting it after the move, it's put back and stops. ○ at a near standstill (|speed| <= 9) gets
\ him out at the first free door. The fuel gauge and the speed are shown. A step at the original's rate (30 a
\ second in the town).
IN: driving
USING: engine actors messages keys screen areas figures floors matthew traffic talk look events events.main ;

variable at-the-wheel   \ src: g_driving
120 constant full-tank
variable fuel   full-tank fuel !   \ src: g_fuel (a new game starts full; the gas station fills it)
: driving? ( -- flag )  at-the-wheel @ 0<> ;

<PRIVATE
26 constant his-car
\ the speed (src: g_car_velocity: 60 units a frame for 1)
0.4e fconstant accelerating   8e fconstant top   2e fconstant top-on-empty   -2e fconstant top-backing
0.1e fconstant coasting   -0.4e fconstant backing-still   60e fconstant units-per
201 constant frames-a-unit   9 constant still-enough
8 constant steer-step   4 constant steer-ease   800 constant steer-turnover   5 constant steer-slow   12 constant steer-fast
$20 constant probe-step   $200 constant probe-limit   $32 constant swerve   300 constant car-half
\ getting in (src: func_8004175C: beside the driver's door, facing across the car)
$1D6 negate constant door-side   $64 negate constant door-forward   $400 constant across
fvariable velocity   variable steer   variable burned   variable speed   variable held   \ (the pad's not)
variable cx   variable cz   variable cy   variable ch
fvariable due

: speed-now ( -- n )  velocity f@ units-per f* f>s ;
: steer-limit ( -- n )  speed @ steer-slow /  steer-turnover speed @ - steer-fast /  min ;
: steer-wheel ( -- )   \ src: car_update's steering
    pad-right key-down? if  steer-step steer +!  steer @ steer-limit min steer ! exit  then
    pad-left key-down? if  steer-step negate steer +!  steer @ steer-limit negate max steer ! exit  then
    steer @ steer-ease >= if  steer-ease negate steer +! exit  then
    steer @ steer-ease negate < if  steer-ease steer +! exit  then
    0 steer ! ;
: burn ( -- )  1 burned +!  burned @ frames-a-unit >= if  0 burned !  fuel @ 1- 0 max fuel !  then ;
: coast ( -- )   \ src: velocity_coast
    velocity f@ coasting f>= if  velocity f@ coasting f- velocity f! exit  then
    velocity f@ backing-still f< if  velocity f@ accelerating f+ velocity f! exit  then
    0e velocity f! ;
: pedals ( -- )
    pad-square key-down? if  velocity f@ accelerating f- top-backing fmax velocity f! exit  then
    pad-cross key-down? if
        velocity f@ accelerating f+  fuel @ if  top  else  top-on-empty  then  fmin velocity f!  burn exit  then
    coast ;

\ ---- the car in the town ----
: along ( x z h d -- x' z' )
    {: x z h d :}  h s>f 2e f* pi f* 4096e f/ {: F: a :}
    x s>f a fsin d s>f f* f+ f>s  z s>f a fcos d s>f f* f+ f>s ;
: wall? ( x z -- flag )  1 blocked? ;   \ (the town's floor)
: probe ( h -- blocked? )  cx @ cz @ rot speed @ along 2dup wall? -rot solid-billboard? or ;   \ src: car_probe
: hits? ( x z -- flag )   \ src: car_tile_hit: its box's two corners
    {: x z :}  x car-half - z car-half - wall?  x car-half + z car-half + wall? or ;
: swerve-way ( -- h )   \ the heading it moves on: ahead, or the first free one each side (it steers that way)
    ch @ {: h :}  speed @ 0> 0=  h probe 0= or if  h exit  then
    probe-limit probe-step do
        h i + probe 0= if  swerve ch +!  h i + unloop exit  then
        h i - probe 0= if  swerve negate ch +!  h i - unloop exit  then
    probe-step +loop  h ;
: ground-under ( x z -- y )  ground-at 0= if  cy @  then ;   \ (the town's ground; off it, as it was)
: move-car ( -- )
    swerve-way {: h :}  cx @ cz @ {: x z :}
    x z h speed @ along {: nx nz :}  x z h speed @ 2/ along {: hx hz :}
    nx nz hits?  hx hz hits? or if  0e velocity f!  0 speed ! exit  then   \ (the sound and the rumble: to come)
    nx cx !  nz cz !  nx nz ground-under cy ! ;
: show-car ( -- )
    his-car cx @ cy @ cz @ ch @ 4095 and s" traffic" actor-named send car-place
    cx @ cy @ cz @ s" matthew" actor-named send figure-place  0 ch @ 4095 and 0 s" matthew" actor-named send figure-turn ;

\ ---- out (src: car_exit, car_can_exit: the seats' points around it) ----
: door ( i -- x z )  seat {: sd fw :}  cx @ cz @ ch @ 4095 and sd fw beside ;
: free-door ( -- i | -1 )  -1  #seats 0 do  i door wall? 0= if  drop i leave  then  loop ;
: get-out ( i -- )
    door {: x z :}  false s" camera" actor-named send camera-driving
    false at-the-wheel !  0e velocity f!  0 speed !  0 steer !
    x 0 z s" matthew" actor-named send figure-place  0 ch @ 4095 and 0 s" matthew" actor-named send figure-turn
    true s" matthew" actor-named send figure-shown  true s" matthew" actor-named send figure-on-foot
    0 s" matthew" actor-named send figure-perch  0 -1 true s" matthew" actor-named send figure-anim
    broadcast left-the-car ;
: wheel-frame ( -- )
    steer-wheel  pedals  speed-now speed !
    speed @ if  steer @ ch +!  then
    speed @ abs still-enough <=  pad-circle key-down? and if  free-door dup 0< if  drop  else  get-out exit  then  then
    move-car  show-car ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: drive-tick ( -- )
    driving? 0= if  exit  then  steps-due  held @ if  drop show-car exit  then
    0 ?do  driving? if  wheel-frame  then  loop ;

\ ---- in (src: func_8004175C, func_8004301C) ----
: car-reach? ( -- flag )   \ the car before him, close enough (src: look result 5)
    area @ the-town <> if  false exit  then  his-car car-shown? 0= if  false exit  then
    his-car car-now drop {: x y z :}  matthew-now {: mx my mz h :}
    mx mz x z distance  matthew radius  his-car radius +  reach-margin +  <
    h  x mx - s>f z mz - s>f fatan2 4096e f* 2e pi f* f/ f>s  talk-angle heading-within?  and ;
: into-his-car ( -- )   \ beside the door, his scene (src: func_8004175C)
    his-car car-now {: x y z h :}
    x z h door-side door-forward beside {: dx dz :}  dx 0 dz s" matthew" actor-named send figure-place
    0 h across + 4095 and 0 s" matthew" actor-named send figure-turn
    ['] into-the-car start-event ;
: get-in ( -- handled? )  car-reach? 0= if  false exit  then  into-his-car  true ;   \ ○ (src: look_execute case 5)
' get-in circle-first!
: at-the-wheel! ( -- )   \ src: func_8004301C (from his scene): driving
    his-car car-now ch !  cz !  cy !  cx !  0e velocity f!  0 steer !  0 burned !  0e due f!
    true at-the-wheel !  true s" camera" actor-named send camera-driving
    false s" matthew" actor-named send figure-shown  false s" matthew" actor-named send figure-on-foot
    perch-in-car s" matthew" actor-named send figure-perch  show-car ;
main-exe $8004301C ' at-the-wheel! native:

\ ---- drawing: the fuel gauge and the speed (src: fuel_gauge_draw; their pictures to come) ----
-144 constant gauge-x   30 constant gauge-top   64 constant gauge-h   16 constant gauge-w   80 constant needle-base
-128 constant speed-x   74 constant speed-y   6 constant speed-shown   \ (the speed shown: a sixth of it)
$302030E0 constant gauge-back   $F0C060FF constant needle-ink   $FFFFFFFF constant speed-ink
variable needle   full-tank needle !
: draw-gauge ( -- )
    needle @ fuel @ < if  1 needle +!  then  needle @ fuel @ > if  -1 needle +!  then
    scale {: k :}
    gauge-back pen-color  gauge-x gauge-top >screen  gauge-w k *  gauge-h k *  draw-rect
    needle-ink pen-color  gauge-x needle-base needle @ 4 / - >screen  gauge-w k *  8 k *  draw-rect
    speed-ink pen-color  0 font  k pen-scale  speed @ 0 max speed-shown / n>str  speed-x speed-y >screen draw-text ;
: draw-driving ( -- )  driving? if  draw-gauge  then ;
' draw-driving on-draw
PRIVATE>

: fill-up ( -- )  full-tank fuel ! ;
: his-car-now ( -- x z heading )  cx @ cz @ ch @ 4095 and ;   \ (a fact: where he has it)
: put-in-his-car ( -- )  into-his-car ;   \ (the chase's: src func_8004175C)
: step-out ( -- )  driving? if  free-door 0 max get-out  then ;   \ (src: car_exit from the C: at a free door, else the first)   \ src: fuel_set(120): the gas station

state: driving-state end-state
behaviour drivers
  on spawned ( -- )  self subscribe tick  self subscribe area-entered ;
  on tick ( -- )  drive-tick ;
  on drive-hold ( flag -- )  held !  0e due f! ;
  on car-bumped ( -- )  velocity f@ 16e f/ velocity f! ;
  on car-nudge ( dx dz -- )  cz +!  cx +!  show-car ;
  on area-entered ( area scene -- )  2drop  false held !  driving? if  false at-the-wheel !  false s" camera" actor-named send camera-driving  then ;
end-behaviour

: spawn-driving ( -- id )  drivers driving-state s" driving" spawn ;
