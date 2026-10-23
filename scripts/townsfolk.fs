\ townsfolk.fs - the people of the town and their days (docs/subsystems/townsfolk.md; src: cobj.c, the
\ characters' handlers, timetable.c). Each person with a schedule (scripts/events/schedules.fs) is where the
\ clock puts them: at the first stop whose time hasn't come. Those in the area Matthew is in are drawn, in their
\ stop's motion. Facts: stop-of, the stop's place; the townsfolk actor keeps them as the time moves.
IN: townsfolk
USING: engine actors messages clock areas figures doors traffic ;

182 constant #slots
enum: placed  standing partnered at-diner at-club ;   \ how a stop puts them (src: func_80027944)

\ ---- the rows ----
begin-structure stop-row
  field: stop-until     \ they stay until (a time: clock.fs)
  field: stop-record    \ which of their placements (the original's index)
  field: stop-area   field: stop-floor   field: stop-x   field: stop-z   field: stop-heading
  field: stop-pose      \ the motion they hold there
  field: stop-placed    \ placed: stand, or with someone, or on a seat
  field: stop-variant   \ another model (0: their own)
  field: stop-route     \ the route they walk on leaving (an event: to come)
  field: stop-car       \ where their car waits meanwhile: a car route's start (0: where it is; -1: off the roads)
end-structure

begin-structure person
  field: first-stop     \ their rows, one after another (0: no schedule)
  field: #stops
  field: at-stop        \ the stop the clock puts them at
  field: figure         \ their model (0: not loaded yet)
  field: travelling     \ between stops, walking their route
  field: person-area   field: on-floor   field: px   field: py   field: pz   field: heading-now   \ where they are
  field: walking        \ walking to a point (src: ACTOR_F_NAV)
  field: goal-x   field: goal-z   field: stride   field: stride-motion
  field: riding         \ the car they ride in (0: none)
  field: forward        \ going straight ahead, units a frame (0: not; src: move_speed)
  field: forward-motion \ in this motion (-1: whatever plays; src: move_anim)
  playing +field moving \ the motion they hold
end-structure
create people  #slots person * allot   people #slots person * erase
: person-of ( slot -- addr )  person * people + ;
: row-of ( person i -- row )  stop-row * swap first-stop @ + ;

variable writing   variable writing-row   \ (the rows being written: townsfolk/rows.fs)

\ ---- where they are ----
: scheduled? ( slot -- flag )  person-of first-stop @ 0<> ;
: stop-for ( person t -- i )   \ the first stop whose time hasn't come (after the last: the last)
    {: p t :}  p #stops @ 0 ?do  p i row-of stop-until @ t > if  i unloop exit  then  loop  p #stops @ 1- ;
: stop-of ( slot -- stop | -1 )   \ the stop they stand at (-1: walking, or no schedule)
    dup scheduled? 0= if  drop -1 exit  then  person-of dup travelling @ if  drop -1  else  at-stop @  then ;
: current-row ( slot -- row )  person-of dup at-stop @ row-of ;

\ the facts the areas' rules ask
<PRIVATE   \ (the cast's names are the events': events.fs)
2 constant nozz
PRIVATE>
0 constant behind-the-counter   \ (Nozz's record 0: his shop's counter)
: nozz-behind-counter? ( -- flag )   \ src: func_80026C50
    nozz stop-of 0< if  false exit  then  nozz current-row stop-record @ behind-the-counter = ;
: james-visiting? ( -- flag )  false ;   \ src: func_8002CBE8 (James by Matthew's bed: his position, to come)

: person-now ( slot -- x y z heading )  person-of {: p :}  p px @  p py @  p pz @  p heading-now @ ;
: here? ( slot -- flag )   \ in the area being played, not in a car (seated ones: to come)
    dup scheduled? 0= if  drop false exit  then
    person-of {: p :}  p person-area @ area @ =  p person-area @ 0< 0= and  p riding @ 0= and ;
: walking? ( slot -- flag )  person-of walking @ 0<> ;
: riding? ( slot -- flag )  person-of riding @ 0<> ;
: person-anim-done? ( slot -- flag )  person-of moving {: m :}  m done @  m anim @ 0< or ;

<PRIVATE   \ (their workings: the messages are their face)
\ ---- what moves them ----
: show ( slot id next loop -- )  {: s id nx lp :}  id nx lp  s person-of moving play ;
: to-traffic ( -- id )  s" traffic" actor-named ;

\ riding in their car (docs/subsystems/traffic.md)
$1645 constant getting-in      \ the motion of climbing in (the town's own)
1024 constant get-in-turn      \ turned so from the car's way (src: func_80029AEC, 0x1400 of a turn)
0 constant riders-seat         \ (src: actor_attach - every rider the first seat)
: attach ( slot route speed -- )   \ src: actor_attach: they ride, the car drives the route
    {: s n v :}  s partner-of {: c :}  c 0= if  exit  then
    s person-of {: p :}  0 p walking !  c p riding !  the-town p person-area !
    c n v to-traffic send car-carry ;
: get-in ( slot -- )   \ src: cmd 0x42: facing across the car, climbing in
    {: s :}  s partner-of {: c :}  c 0= if  exit  then
    c car-now nip nip nip  get-in-turn +  4095 and  s person-of heading-now !
    s getting-in -1 false show ;
: get-out ( slot car -- )   \ src: car_unload_riders: beside their seat, facing the car's way
    {: s c :}  s person-of {: p :}  c car-now {: x y z h :}
    x z h riders-seat seat beside  p pz !  p px !  y p py !  h p heading-now !
    0 p riding !  the-town p person-area ! ;
: unload ( car -- )  #slots 0 do  i scheduled? if  i person-of riding @ over = if  i over get-out  then  then  loop  drop ;
: ride-on ( slot row -- )   \ a stop with them riding: their car on its way (unless it is already)
    {: s r :}  s partner-of {: c :}  c 0= if  exit  then  s person-of {: p :}
    c car-carrying? if  c p riding !  the-town p person-area !  else  s r stop-x @ r stop-z @ attach  then ;
: park-car ( slot row -- )   \ their car where the stop leaves it (src: actor_apply_placement's partner lines)
    {: s r :}  s partner-of {: c :}  c 0= if  exit  then
    r stop-car @ {: line :}
    line 0= if  c to-traffic send car-stop  exit  then
    line 0< if  c to-traffic send car-hide  else  c line to-traffic send car-park  then ;
: place-at-stop ( slot -- )   \ src: actor_apply_placement (cmd 0x2D)
    {: s :}  s person-of {: p :}  s current-row {: r :}
    0 p walking !  0 p travelling !  0 p riding !
    r stop-placed @ partnered = if  s r ride-on  exit  then
    s r park-car
    r stop-placed @ standing <> if  -1 p person-area !  exit  then   \ (seats: to come)
    r stop-area @ p person-area !  r stop-floor @ p on-floor !
    r stop-x @ p px !  r stop-z @ p pz !  r stop-area @ r stop-floor @ floor-height p py !
    r stop-heading @ p heading-now !
    s  r stop-pose @ -1 true show ;
: goto-stop ( slot stop -- )   \ src: actor_goto_stop
    over person-of at-stop !  dup place-at-stop  broadcast person-stopped ;
: arrive ( slot -- )   \ src: actor_arrive
    dup person-of {: s p :}  p at-stop @ 1+ p #stops @ 1- min p at-stop !  s place-at-stop ;
: depart ( slot -- )   \ src: depart: travelling, the stop's route on their own thread
    dup person-of {: s p :}  true p travelling !  s  s current-row stop-route @  broadcast person-departs ;
: heading-to ( dx dz -- heading )  s>f s>f fswap fatan2 4096e f* 2e pi f* f/ f>s 4095 and ;
: on-ground ( person -- )   \ in the town, their height the ground's under them (src: func_8001C264)
    {: p :}  p person-area @ the-town <> if  exit  then
    p px @ p pz @ ground-at if  p py !  then ;
: walk-step ( slot -- )   \ src: actor_nav_step, nav_step: a step of their speed toward the goal
    person-of {: p :}  p goal-x @ p px @ -  p goal-z @ p pz @ -  {: dx dz :}
    p stride @ s>f game-frames f*  {: F: v :}
    dx abs s>f v f<  dz abs s>f v f< and if   \ (there: they stand)
        p goal-x @ p px !  p goal-z @ p pz !  0 p walking !  0 -1 true p moving play  exit
    then
    dx dz heading-to {: h :}  h p heading-now !
    h s>f 2e f* pi f* 4096e f/ {: F: a :}
    p px @ s>f  a fsin v f* f+ f>s p px !   p pz @ s>f  a fcos v f* f+ f>s p pz !
    p on-ground ;

$3F constant through-door-1   $40 constant through-door-0   \ the motions of stepping out of a door, by way
: through-door ( slot door room flip -- )   \ src: func_8004535C (the door's opening: the doors, to come)
    {: s n room flip :}  n room flip door-way {: w :}  w 0= if  exit  then
    s person-of {: p :}  0 p walking !
    w out-area @ p person-area !  w out-x @ p px !  w out-z @ p pz !  w out-heading @ p heading-now !
    p person-area @ p on-floor @ floor-height p py !
    s  w out-way @ if  through-door-1  else  through-door-0  then  -1 false show
    true p moving hold-move ! ;

\ the minute's rules (src: npc_clock_tick: cmd 0x19 standing, 0x1A walking - while Matthew is the player's)
variable controlled
: minute ( t -- )
    {: t :}  #slots 0 do
        i scheduled? if
            i person-of {: p :}
            p travelling @ if
                p at-stop @ 1+ p #stops @ < if
                    p dup at-stop @ 1+ row-of stop-until @ t = if  i p at-stop @ 1+ goto-stop  then   \ (src: sched_skip)
                then
            else
                p dup at-stop @ row-of stop-until @ t = if  i depart  then   \ (src: sched_depart: at its minute)
            then
        then
    loop ;
: settle-all ( t -- )   \ a jump in time: each to the stop for it (src: time_skip's finishing and catching up)
    {: t :}  #slots 0 do
        i scheduled? if  i  i person-of t stop-for  goto-stop  then
    loop ;

\ ---- drawing those where Matthew is ----
: model-of ( slot -- model )
    dup person-of figure @ ?dup if  nip exit  then
    dup kind-model model  dup rot person-of figure ! ;
: draw-person ( slot -- )
    dup model-of {: s m :}  s person-of moving {: mv :}
    m  mv clip @  mv frame  s person-now  draw-figure ;
: draw-townsfolk ( -- )  #slots 0 do  i here? if  i draw-person  then  loop ;
' draw-townsfolk on-draw
: step-forward ( slot -- )   \ src: actor_update's move_anim: straight on along their heading
    {: s :}  s person-of {: p :}
    p forward-motion @ 0< 0=  p moving anim @ p forward-motion @ <> and if  s p forward-motion @ -1 true show  then
    p heading-now @ s>f 2e f* pi f* 4096e f/ {: F: a :}  p forward @ s>f game-frames f* {: F: v :}
    p px @ s>f  a fsin v f* f+ f>s p px !   p pz @ s>f  a fcos v f* f+ f>s p pz ! ;
: animate ( -- )   \ walking and motions for everyone (the routes wait on them, seen or not)
    #slots 0 do
        i scheduled? if
            i walking? i riding? 0= and if  i walk-step  then   \ (riding: their walk waits for the car)
            i person-of forward @ if  i step-forward  then
            i person-of moving advance drop
        then
    loop ;

PRIVATE>

state: townsfolk-state end-state
behaviour townsfolk
  on spawned ( -- )  self subscribe tick  self subscribe time-set  self subscribe minute-passed
    self subscribe car-arrived
    self subscribe player-control ;
  on tick ( -- )  animate ;
  on time-set ( t -- )  settle-all ;
  on minute-passed ( t -- )  controlled @ if  minute  else  drop  then ;
  on player-control ( flag -- )  controlled ! ;
  on person-turn ( slot heading -- )  swap person-of heading-now ! ;
  on person-place ( slot x z -- )  rot person-of {: x z p :}  x p px !  z p pz ! ;
  on person-floor ( slot floor -- )
    swap person-of {: f p :}  f p on-floor !  p person-area @ f floor-height p py ! ;
  on person-walk ( slot x z speed motion -- )
    {: s x z v m :}  s person-of {: p :}  x p goal-x !  z p goal-z !  v p stride !  m p stride-motion !
    true p walking !  s m -1 true show ;
  on person-idle ( slot -- )  dup person-of {: p :}  0 p walking !  0 p forward !  0 -1 true show ;
  on person-forward ( slot speed motion -- )  rot person-of {: v m p :}  v p forward !  m p forward-motion ! ;
  on person-anim ( slot id next loop -- )  show ;
  on person-hold ( slot -- )  person-of moving true swap hold-move ! ;
  on person-arrive ( slot -- )  arrive ;
  on person-goto-stop ( slot stop -- )  goto-stop ;
  on person-depart ( slot -- )  depart ;
  on person-through-door ( slot door room flip -- )  through-door ;
  on person-attach ( slot route speed -- )  attach ;
  on person-get-in ( slot -- )  get-in ;
  on car-arrived ( car -- )  unload ;
end-behaviour

: spawn-townsfolk ( -- id )  townsfolk townsfolk-state s" townsfolk" spawn ;
