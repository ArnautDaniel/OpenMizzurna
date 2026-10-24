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
USING: engine actors messages keys screen areas figures floors matthew traffic talk look events events.main sound rumble ui townsfolk ;

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
\ its sounds (src: func_80109A10 the engine's two tones; engine_sound's key; car_scrape, the crash)
$66 constant engine-sound   2 constant engine-tones   10 constant key-per-speed   $29 constant idle-key
$69 constant crash-sound   $3D constant loud-enough   2 constant scrape-frames   255 constant crash-strength
$20 constant probe-step   $200 constant probe-limit   $32 constant swerve   300 constant car-half
\ getting in (src: func_8004175C: beside the driver's door, facing across the car)
$1D6 negate constant door-side   $64 negate constant door-forward   $400 constant across
fvariable velocity   variable steer   variable burned   variable speed   variable held   \ (the pad's not)
variable at-a-pump   \ (the town's rules: by a gas pump, ○ fills instead - src func_801069F4)
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
\ the road blocks (src: car_road_block): tiles (column + row x 100) the car may not be in by their zone under it
\ (two corners): building fronts in zone 0, others in zone 1, the police cordon's in zone 0 until the 28th 03:00
create fronts  314 , 612 , 623 , 723 , 823 , 1028 , 1202 , 1407 , 2018 , 2019 , 2023 ,   11 constant #fronts
create zone-1-blocks  512 , 1317 , 1902 , 2008 ,   4 constant #zone-1-blocks
: listed? ( key table n -- flag )  {: k t n :}  n 0 do  t i cells + @ k = if  true unloop exit  then  loop  false ;
: car-zone ( x z -- id )   \ src: player_zone driving: its two corners
    {: x z :}  x car-half - z car-half - 1 zone-at  dup 0< if  drop  x car-half + z car-half + 1 zone-at  then ;
: road-blocked? ( x z -- flag )
    {: x z :}  x z tile-key {: k :}  x z car-zone {: zn :}
    k fronts #fronts listed? if  zn 0= exit  then
    k zone-1-blocks #zone-1-blocks listed? if  zn 1 = exit  then
    k cordoned-tile? if  zn 0=  cordon-holds? and exit  then  false ;
: probe ( h -- blocked? )   \ src: car_probe (the road blocks by where the car is)
    cx @ cz @ rot speed @ along 2dup wall? -rot solid-billboard? or  cx @ cz @ road-blocked? or ;
: hits? ( x z -- flag )   \ src: car_tile_hit: its box's two corners
    {: x z :}  x car-half - z car-half - wall?  x car-half + z car-half + wall? or ;
: scrape ( -- )   \ src: car_scrape: the pad shakes by the speed
    speed @ loud-enough >= if  crash-sound sfx  scrape-frames speed @ 2/ 255 min scrape-frames shake  then ;
: crashed ( -- )   \ src: car_update's hit: full shake and the crash, at speed
    speed @ loud-enough >= if  crash-sound sfx  scrape-frames crash-strength scrape-frames shake  then ;
: swerve-way ( -- h )   \ the heading it moves on: ahead, or the first free one each side (it steers that way)
    ch @ {: h :}  speed @ 0> 0=  h probe 0= or if  h exit  then
    probe-limit probe-step do
        h i + probe 0= if  scrape  swerve ch +!  h i + unloop exit  then
        h i - probe 0= if  scrape  swerve negate ch +!  h i - unloop exit  then
    probe-step +loop  h ;
: ground-under ( x z -- y )  ground-at 0= if  cy @  then ;   \ (the town's ground; off it, as it was)
: move-car ( -- )
    swerve-way {: h :}  cx @ cz @ {: x z :}
    x z h speed @ along {: nx nz :}  x z h speed @ 2/ along {: hx hz :}
    nx nz hits?  hx hz hits? or  nx nz road-blocked? or if  crashed  0e velocity f!  0 speed ! exit  then
    nx cx !  nz cz !  nx nz ground-under cy ! ;
\ stuck: the car standing in someone's place or a wall, moving (or every 32 frames), goes to the nearest free spot
\ 100 at a time out to 1400, along x then z (src: func_80106370, cobj.c's check of the car)
100 constant unstick-step   1500 constant unstick-reach   10 constant moving-enough   32 constant check-every
variable car-frames
: car-stuck? ( x z -- flag )  2dup hits? if  2drop true exit  then  taken? ;
: car-to ( x z -- moved? )  2dup car-stuck? if  2drop false exit  then  cz !  cx !  true ;
: car-around ( d -- moved? )
    {: d :}  cx @ cz @ {: x z :}
    x d + z car-to if  true exit  then  x d - z car-to if  true exit  then
    x z d + car-to if  true exit  then  x z d - car-to ;
: unstick-car ( -- )
    1 car-frames +!  speed @ abs moving-enough <  car-frames @ check-every mod 0<> and if  exit  then
    cx @ cz @ car-stuck? 0= if  exit  then
    unstick-reach unstick-step do  i car-around if  leave  then  unstick-step +loop ;
\ the car's pose as he drives (src: func_80109430): leaning into the turn (its roll less the steer), the wheels on
\ by half the speed a frame, the front turned ten times the steer (the other way backing)
10 constant wheel-turn
: pose-car ( -- )
    his-car  speed @ 2/  steer @ wheel-turn *  speed @ 0< if  negate  then  steer @ negate
    s" traffic" actor-named send car-pose ;
: show-car ( -- )
    his-car cx @ cy @ cz @ ch @ 4095 and s" traffic" actor-named send car-place
    cx @ cy @ cz @ s" matthew" actor-named send figure-place  0 ch @ 4095 and 0 s" matthew" actor-named send figure-turn
    pose-car ;

\ ---- out (src: car_exit, car_can_exit: the seats' points around it) ----
: door ( i -- x z )  seat {: sd fw :}  cx @ cz @ ch @ 4095 and sd fw beside ;
: free-door ( -- i | -1 )  -1  #seats 0 do  i door wall? 0= if  drop i leave  then  loop ;
: get-out ( i -- )
    door {: x z :}  false s" camera" actor-named send camera-driving
    false at-the-wheel !  0e velocity f!  0 speed !  0 steer !  pose-car   \ (the front straight, upright)
    x 0 z s" matthew" actor-named send figure-place  0 ch @ 4095 and 0 s" matthew" actor-named send figure-turn
    true s" matthew" actor-named send figure-shown  true s" matthew" actor-named send figure-on-foot
    0 s" matthew" actor-named send figure-perch  0 -1 true s" matthew" actor-named send figure-anim
    s" sound" actor-named send engine-stop  broadcast left-the-car ;
: wheel-frame ( -- )
    steer-wheel  pedals  speed-now speed !
    speed @ if  steer @ ch +!  then
    speed @ abs still-enough <=  pad-circle key-down? and  at-a-pump @ 0= and if   \ (at a pump ○ fills: the town's)
        free-door dup 0< if  drop  else  get-out exit  then  then
    move-car  unstick-car  show-car
    speed @ abs key-per-speed / idle-key + s" sound" actor-named send engine-pitch ;
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
    perch-in-car s" matthew" actor-named send figure-perch  show-car
    engine-sound engine-tones s" sound" actor-named send engine-start ;
main-exe $8004301C ' at-the-wheel! native:
2 constant in-his-car   \ (the companion's mode: riding with him - src D_800A37B4 = 2)
1 constant passenger-seat
: with-passenger ( -- )   \ src: car_enter_with_passenger (0x8004305C): at the wheel, the one along with him riding
    at-the-wheel!  companion @ 0< if  exit  then   \ (in the car at its second seat: out by it with him)
    companion @ in-his-car s" townsfolk" actor-named send person-ride
    companion @ his-car passenger-seat s" townsfolk" actor-named send person-passenger ;
main-exe $8004305C ' with-passenger native:

\ ---- drawing: the fuel gauge and the speed (src: hud.c fuel_gauge_draw: from the HUD's sheet, ui/clock.png) ----
\ the scale beside a marker easing a unit a frame toward the fuel; the speed's three digits (a sixth of it,
\ jittering from 80 up) and "Km/h" over a dark backdrop
-144 constant gauge-x   30 constant scale-top   15 constant scale-w   64 constant scale-h   80 constant marker-base
16 constant marker-w   8 constant marker-h   -128 constant digits-x   74 constant digits-y   16 constant digit-w
-80 constant unit-x   32 constant unit-w   6 constant speed-shown   80 constant jitter-from   4 constant jitter   \ (src: g_rand & 3)
$F0 constant scale-u   $A0 constant marker-u   $10 constant marker-v   $A8 constant unit-u   $30 constant unit-v
-128 constant backdrop-left   -36 constant backdrop-top-right   -44 constant backdrop-bottom-right   90 constant backdrop-bottom
$00000080 constant backdrop-ink
variable needle   full-tank needle !   variable gauge-sheet
: sheet ( -- image )  gauge-sheet @ ?dup if  exit  then  s" ui/clock.png" image dup gauge-sheet ! ;
: part ( u v w h x y -- )   \ a piece of the sheet at the original's place, opaque
    {: u v w h x y :}  blend-ps-solid pen-blend  sheet u v w h  x y >screen  w scale *  h scale *  draw-image-part
    blend-usual pen-blend ;
: digit ( n x -- )  {: n x :}  n digit-w * 0 digit-w digit-w x digits-y part ;
: draw-gauge ( -- )
    needle @ fuel @ < if  1 needle +!  then  needle @ fuel @ > if  -1 needle +!  then
    backdrop-ink pen-color
    backdrop-left digits-y >screen  backdrop-top-right digits-y >screen
    backdrop-left backdrop-bottom >screen  backdrop-bottom-right backdrop-bottom >screen
    backdrop-ink backdrop-ink backdrop-ink backdrop-ink draw-quad
    $FFFFFFFF pen-color
    scale-u 0 scale-w scale-h gauge-x scale-top part
    marker-u marker-v marker-w marker-h gauge-x marker-base needle @ 4 / - part
    speed @ 0 max speed-shown / dup jitter-from >= if  jitter random +  then {: n :}
    n 100 / digits-x digit  n 10 / 10 mod digits-x digit-w + digit  n 10 mod digits-x digit-w 2* + digit
    unit-u unit-v unit-w digit-w unit-x digits-y part ;
: draw-driving ( -- )  driving? if  draw-gauge  then ;
' draw-driving on-draw
PRIVATE>

: fill-up ( -- )  full-tank fuel ! ;
: nearly-still? ( -- flag )  speed @ abs still-enough <= ;   \ (a fact: slow enough to step out, or for the ring)
: door-free? ( -- flag )  free-door 0< 0= ;   \ (a fact: a way out of the car where it stands - src car_can_exit)
: his-car-now ( -- x z heading )  cx @ cz @ ch @ 4095 and ;   \ (a fact: where he has it)
: put-in-his-car ( -- )  into-his-car ;   \ (the chase's: src func_8004175C)
: step-out ( -- )  driving? if  free-door 0 max get-out  then ;   \ (src: car_exit from the C: at a free door, else the first)   \ src: fuel_set(120): the gas station

state: driving-state end-state
behaviour drivers
  on spawned ( -- )  self subscribe tick  self subscribe area-entered ;
  on tick ( -- )  ui-busy? 0= if  drive-tick  then ;   \ (src: field_update: no player_update while ui_busy)
  on drive-hold ( flag -- )  held !  0e due f! ;
  on car-bumped ( -- )  velocity f@ 16e f/ velocity f! ;
  on car-nudge ( dx dz -- )  cz +!  cx +!  show-car ;
  on his-car-at ( x z heading -- )   \ (parked there: Matthew elsewhere)
    ch !  cz !  cx !  cx @ cz @ ground-under cy !  his-car cx @ cy @ cz @ ch @ 4095 and s" traffic" actor-named send car-place ;
  on fuel-set ( n -- )  fuel ! ;
  on car-at-pump ( flag -- )  at-a-pump ! ;
  on car-stop-now ( -- )  0e velocity f!  0 speed ! ;
  on area-entered ( area scene -- )  2drop  false held !
    driving? if  false at-the-wheel !  false s" camera" actor-named send camera-driving  s" sound" actor-named send engine-stop  then ;
end-behaviour

: spawn-driving ( -- id )  drivers driving-state s" driving" spawn ;
