\ traffic.fs - the town's cars (docs/subsystems/traffic.md; src: carline.c). 35 cars, actor slots 26-60, drive
\ routes (scripts/events/car-routes.fs) through the road graph (engine: road-node, road-point): from node to node
\ along each road's bends, easing their speed and turning toward the way ahead; a car parking takes a free lane
\ at its parking node. Facts: car-now, car-driving?, car-carrying?, partner-of, beside.
IN: traffic
USING: engine actors messages areas figures matthew ui ;

26 constant first-car   35 constant #cars
181 constant #car-routes
: car? ( slot -- flag )  first-car - #cars u< ;

\ ---- the routes (written by traffic/rows.fs) ----
enum: route-end  ends-parked ends-going-round ;
begin-structure car-route-row
  field: route-lane      \ the parking node whose lanes it starts in
  field: route-ends      \ ends-parked or ends-going-round
  field: route-end-at    \ parks: the node it parks at; loops: the node (counted from 0) it drives on from again
  field: #route-nodes
end-structure             \ (then its nodes, a cell each)
create car-routes  #car-routes cells allot   car-routes #car-routes cells erase
: route-of ( n -- row | 0 )  dup #car-routes u< if  cells car-routes + @  else  drop 0  then ;
: route-node ( row i -- node )  cells swap car-route-row + + @ ;

\ who goes with whom: a person and their car (or bike), pairs ended by -1 - src: actor_partner
: who-at ( i -- slot )  2* s" partners" rot exe-s16 ;
: whose-at ( i -- car )  2* 1+ s" partners" rot exe-s16 ;
33 constant #partner-pairs   \ (with the -1 that ends them)
: partner-of ( slot -- car | 0 )
    {: s :}  #partner-pairs 0 do  i who-at  dup -1 = if  drop 0 unloop exit  then
        s = if  i whose-at unloop exit  then  loop  0 ;

\ a point beside something: (side, forward) turned with its heading (src: func_80029994)
: beside ( x z heading side forward -- x' z' )
    {: x z h sd fw :}
    sd s>f fw s>f fatan2  h s>f 2e f* pi f* 4096e f/ f+ {: F: a :}
    sd s>f fdup f*  fw s>f fdup f* f+ fsqrt {: F: r :}
    x  a fsin r f* f>s +   z  a fcos r f* f>s + ;
6 constant #seats
: seat ( i -- side forward )  2* {: k :}  s" car-seats" k exe-s16  s" car-seats" k 1+ exe-s16 ;

\ ---- a car on the roads ----
<PRIVATE
begin-structure car
  field: driving        \ on its way (src: CarLine.active)
  field: carrying       \ someone rides: car-arrived at the end (src: ACTOR_F_ATTACHED)
  field: shown          \ on the roads (car-hide: not)
  field: route-row      \ its route
  field: next-node      \ the route's node it goes to next (#route-nodes: the end)
  field: cx   field: cy   field: cz          \ where the stretch it is on starts
  field: dx   field: dy   field: dz          \ the stretch
  field: stretch        \ its length
  ffield: along         \ how far along it
  ffield: speed         \ units a frame
  field: top-speed      \ the speed it eases toward (src: target_speed)
  field: bends          \ the road's bends still to come
  field: bend           \ the next one
  field: aim            \ the heading of the stretch (unwrapped toward the car's)
  field: lane-node      \ the parking node whose lane it has (-1: none)
  field: lane-number
  field: lane-turn      \ at this many bends left the car swings into its lane (-1: done)
  field: lane-dx   field: lane-dz
  field: switch-route   \ another route it turns onto at that route's first node (0: none; src: loop_node, cmd 0x37)
  field: x   field: y   field: z   field: heading   \ where it is
  playing +field car-motion   \ a motion of its own (the bike's: shaken, falling - src: actor_anim on it)
  field: wheels-spun   field: wheels-steer   field: car-lean   \ as he drives it (src: func_8001A2F4)
end-structure
create cars  #cars car * allot
: car-of ( slot -- c )  first-car - car * cars + ;
PRIVATE>

: car-now ( slot -- x y z heading )  car-of {: c :}  c x @  c y @  c z @  c heading @ 4095 and ;
: car-driving? ( slot -- flag )  car-of driving @ 0<> ;
: car-carrying? ( slot -- flag )  car-of carrying @ 0<> ;
: car-shown? ( slot -- flag )  car-of shown @ 0<> ;
: car-motion-now ( slot -- id )  car-of car-motion anim @ ;
: car-wheels-now ( slot -- spun steer lean )  car-of {: c :}  c wheels-spun @  c wheels-steer @  c car-lean @ ;   \ (a fact: its own motion playing; 0 / -1 none)

<PRIVATE
5 constant speed-ease          \ a frame, toward its top speed
200 constant pulling-in-speed   100 constant parked-speed   \ swinging into a parking lane
64 constant turn-snap          \ nearer than this the car faces the way at once; else a quarter of it a frame
4096 constant full-turn   2048 constant half-turn

: lanes ( node -- x z )  2* {: k :}  s" lane-steps" k exe-s16  s" lane-steps" k 1+ exe-s16 ;
: lane-bit ( c node o -- mask )   \ the lane car o has at that node, as a bit (0: another node's, or c itself)
    {: c n o :}  o lane-node @ n =  o c <> and if  1 o lane-number @ lshift  else  0  then ;
: used-lanes ( c node -- mask )  {: c n :}  0  #cars 0 do  c n  cars i car * +  lane-bit or  loop ;
: free-lane ( c node -- )   \ the lowest lane no other car parking there has (src: car_lane)
    {: c n :}  c lane-node @ n = if  exit  then
    c n used-lanes {: used :}  0 begin  used over rshift 1 and  while  1+  repeat
    c lane-number !  n c lane-node ! ;
: lane-offset ( c node sign -- )   \ its lane's place from the node's
    {: c n sg :}  c n free-lane  n lanes {: lx lz :}
    lx c lane-number @ * sg * c lane-dx !   lz c lane-number @ * sg * c lane-dz ! ;

: route-end? ( c -- flag )  dup next-node @ swap route-row @ #route-nodes @ >= ;
: parks-next? ( c -- flag )   \ the next is the end, and it parks
    dup route-end?  swap route-row @ route-ends @ ends-parked = and ;
: aim-at ( c dx dy dz length -- )  {: c ddx ddy ddz l :}  ddx c dx !  ddy c dy !  ddz c dz !  l c stretch ! ;
: next-bend ( c -- )   \ along the road's next bend
    {: c :}  c bend @ road-point {: px py pz l :}  c px py pz l aim-at
    -1 c bends +!  1 c bend +!
    c lane-turn @ c bends @ = if   \ (here it swings into its lane)
        -1 c lane-turn !  c lane-dx @ negate c dx +!  c lane-dz @ negate c dz +!
        c parks-next? if  pulling-in-speed s>f c speed f!  parked-speed c top-speed !  else  -1 c lane-node !  then
    then ;
: switch? ( c -- )   \ at the switch route's first node: on along it (src: car_next's loop_node)
    {: c :}  c switch-route @ ?dup 0= if  exit  then  {: sw :}
    c route-row @ c next-node @ 1- 0 max route-node  sw 0 route-node = if
        sw c route-row !  1 c next-node !  0 c switch-route !  then ;
: next-node! ( c -- more? )   \ toward the route's next node; false: the route is over (src: car_next)
    dup switch?
    {: c :}  c route-row @ {: r :}
    c route-end? if
        r route-ends @ ends-parked = if  false c driving !  0e c along f!  false exit  then
        r route-end-at @ c next-node !   \ (round again)
    then
    r c next-node @ route-node road-node {: nx ny nz pts first :}
    nx c cx @ -  ny c cy @ -  nz c cz @ -  {: ddx ddy ddz :}
    c ddx ddy ddz  ddx s>f fdup f*  ddy s>f fdup f* f+  ddz s>f fdup f* f+ fsqrt f>s  aim-at
    first c bend !  pts 1- c bends !  1 c next-node +!
    c parks-next? if  c r route-end-at @ -1 lane-offset  1 c lane-turn !  then
    true ;
: next-stretch ( c -- more? )  dup bends @ 0> if  next-bend true  else  next-node!  then ;
: unwrapped ( aim heading -- aim' )   \ the aim within half a turn of the heading
    {: a h :}  a h half-turn - < if  a full-turn +  else  a h half-turn + > if  a full-turn -  else  a  then  then ;
: ease ( F: v target step -- v' )   \ toward the target by step
    {: F: v F: t F: s :}  v t f< if  v s f+ t fmin  else  v s f- t fmax  then ;
: turn ( c frames -- )   \ the car turns toward its aim: a quarter of the way a frame, the last bit at once
    {: c F: n :}  c heading @ c aim @ - {: d :}
    d abs turn-snap < if  c aim @  else  c heading @  d s>f  1e 0.75e n f** f-  f* f>s -  then
    c heading ! ;   \ (kept unwrapped, as its aim is: whole turns off only at a node)
: place ( c -- )   \ along its stretch
    {: c :}  c along f@ c stretch @ s>f f/ {: F: k :}
    c cx @  c dx @ s>f k f* f>s +  c x !
    c cy @  c dy @ s>f k f* f>s +  c y !
    c cz @  c dz @ s>f k f* f>s +  c z ! ;
\ a car on its way pushes Matthew and his car out of its way, in the town (src: func_80037D50, D_80093914: each
\ within its room of the car - both ways, then round - is put that far from it, on the line between them)
640 constant matthew-room   26 constant his-car   1024 constant his-car-room
: within-room? ( x z ox oz r -- flag )
    {: x z ox oz r :}  x ox - abs r <  z oz - abs r < and  0= if  false exit  then
    x ox - s>f fdup f*  z oz - s>f fdup f* f+  r s>f fdup f* f< ;
: room-from ( x z ox oz r -- ox' oz' )   \ r from the car (x z), on its line to (ox oz)
    {: x z ox oz r :}  x ox - s>f  z oz - s>f  fatan2 {: F: a :}
    x s>f a fsin r s>f f* f- f>s  z s>f a fcos r s>f f* f- f>s ;
: push-matthew ( c -- )
    {: c :}  matthew-there? 0= if  exit  then  matthew-now drop {: mx my mz :}
    c x @ c z @ mx mz matthew-room within-room? 0= if  exit  then
    c x @ c z @ mx mz matthew-room room-from {: nx nz :}  nx my nz s" matthew" actor-named send figure-place ;
: push-his-car ( c -- )
    {: c :}  his-car car-of {: h :}  h c =  h shown @ 0= or if  exit  then
    c x @ c z @ h x @ h z @ his-car-room within-room? 0= if  exit  then
    c x @ c z @ h x @ h z @ his-car-room room-from  h z !  h x ! ;
: push-aside ( c -- )  area @ the-town <> if  drop exit  then  dup push-matthew  push-his-car ;
: drive ( c slot -- )   \ a step on (src: func_80037D50)
    {: c s :}  game-frames {: F: n :}
    c along f@  c speed f@ n f* f+  c along f!
    c speed f@  c top-speed @ s>f  speed-ease s>f n f* ease  c speed f!
    begin  c driving @  c stretch @ 0> and  c along f@ c stretch @ s>f f>= and  while
        c along f@ c stretch @ s>f f- c along f!
        c dx @ c cx +!  c dy @ c cy +!  c dz @ c cz +!
        c next-stretch if
            c heading @ full-turn 1- and c heading !
            c dx @ s>f c dz @ s>f fatan2 full-turn s>f f* 2e pi f* f/ f>s  c heading @ unwrapped  c aim !
        then
    repeat
    c place  c n turn  c push-aside
    c driving @ 0=  c carrying @ and if  false c carrying !  s broadcast car-arrived  then ;
: start ( c route speed -- )   \ at the route's start in its lane, facing its first bend (src: carline_start)
    {: c n v :}  n route-of {: r :}  r 0= if  exit  then
    r c route-row !  true c shown !
    c r route-lane @ 1 lane-offset
    r 0 route-node road-node {: nx ny nz pts first :}
    first road-point {: px py pz l :}
    nx c lane-dx @ +  dup c cx !  c x !   ny dup c cy !  c y !   nz c lane-dz @ +  dup c cz !  c z !
    px s>f pz s>f fatan2 full-turn s>f f* 2e pi f* f/ f>s full-turn 1- and  dup c heading !  c aim !
    v 0= if  exit  then
    1 c next-node !  first 1+ c bend !  pts 2 - c bends !  c px py pz l aim-at
    0e c along f!  v c top-speed !  0e c speed f!  true c driving !  c bends @ 2 - c lane-turn ! ;
: traffic-reset ( -- )
    cars #cars car * erase
    #cars 0 do  cars i car * +  -1 over lane-node !  drop  loop ;
: step-car ( c slot -- )  over driving @ if  drive  else  2drop  then ;
: car-motions-step ( -- )  #cars 0 do  cars i car * + car-motion advance drop  loop ;
: traffic-step ( -- )  #cars 0 do  cars i car * +  i first-car +  step-car  loop  car-motions-step ;
PRIVATE>

\ ---- drawing them, in the town ----
<PRIVATE
create car-models  #cars cells allot   car-models #cars cells erase
: car-model ( slot -- model )   \ its kind's model, loaded when first drawn
    dup first-car - cells car-models + {: s at :}
    at @ 0= if  s kind-model model at !  then  at @ ;
\ the wheels (src: func_8001A2F4: cars 26 and 27 their objects 3 - 6, the others 1 - 4; the first two the front's)
26 constant first-big   27 constant second-big   3 constant big-wheels   1 constant small-wheels
create every-part  64 allot   every-part 64 1 fill
: first-wheel ( slot -- object )  dup first-big =  swap second-big = or if  big-wheels  else  small-wheels  then ;
: pose-wheels ( slot c -- )
    {: s c :}  s first-wheel {: w :}  c wheels-spun @ 4095 and {: spin :}
    w 0 0 0 spin c wheels-steer @ 0 pose-object   w 1+ 0 0 0 spin c wheels-steer @ 0 pose-object
    w 2 + 0 0 0 spin 0 0 pose-object   w 3 + 0 0 0 spin 0 0 pose-object
    0 0 0 0 0 0 c car-lean @ pose-object ;
: posed? ( c -- flag )  dup wheels-spun @ 0<>  over wheels-steer @ 0<> or  swap car-lean @ 0<> or ;
: draw-car ( slot -- )   \ (posed while a motion of its own is on; his, its wheels as he drives it)
    dup car-of {: s c :}  c shown @ 0= if  exit  then
    s car-model ?dup 0= if  exit  then  {: m :}
    c car-motion clip @ ?dup if  m swap c car-motion frame  c x @ c y @ c z @  s car-now nip nip nip  draw-figure exit  then
    c posed? if  s c pose-wheels  m  c x @  c y @  c z @  s car-now nip nip nip  every-part 64 draw-model-shown exit  then
    m  c x @  c y @  c z @  s car-now nip nip nip  draw-model ;
: draw-cars ( -- )  area @ the-town = if  #cars 0 do  i first-car + draw-car  loop  then ;
' draw-cars on-draw
PRIVATE>

state: traffic-state end-state
behaviour traffic-runner
  on spawned ( -- )  traffic-reset  self subscribe tick ;
  on tick ( -- )  ui-busy? 0= if  traffic-step  then ;   \ (src: carline.c: none while ui_busy)
  on car-pose ( car spin steer lean -- )
    {: n d st ln :}  n car-of {: c :}  d c wheels-spun +!  st c wheels-steer !  ln c car-lean ! ;
  on car-anim ( car id next -- )  rot car-of car-motion {: id nx p :}  id nx false p play ;
  on car-park ( car route -- )  swap car-of {: n c :}  false c driving !  false c carrying !  c n 0 start ;
  on car-hide ( car -- )  car-of {: c :}  false c driving !  false c carrying !  false c shown !  0 c x !  0 c z ! ;
  on car-drive ( car route speed -- )  rot car-of -rot start ;
  on car-carry ( car route speed -- )  rot car-of {: n v c :}  c n v start  c driving @ c carrying ! ;
  on car-stop ( car -- )  car-of false swap driving ! ;
  on car-speed ( car speed -- )  swap car-of top-speed ! ;
  on car-switch ( car route -- )  route-of swap car-of switch-route ! ;
  on car-place ( car x y z heading -- )   \ (Matthew driving it: off its route, where he has it)
    {: n px py pz ph :}  n car-of {: c :}  false c driving !  false c carrying !  true c shown !  -1 c lane-node !
    px c x !  py c y !  pz c z !  ph c heading ! ;
end-behaviour

: spawn-traffic ( -- id )  traffic-runner traffic-state s" traffic" spawn ;
