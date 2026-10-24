\ townsfolk.fs - the people of the town and their days (docs/subsystems/townsfolk.md; src: cobj.c, the
\ characters' handlers, timetable.c). Each person with a schedule (scripts/events/schedules.fs) is where the
\ clock puts them: at the first stop whose time hasn't come. Those in the area Matthew is in are drawn, in their
\ stop's motion. Facts: stop-of, the stop's place; the townsfolk actor keeps them as the time moves.
IN: townsfolk
USING: engine actors messages clock areas floors figures doors traffic matthew ui ;

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
  field: stop-route     \ the route they walk on leaving (routes.fs walks it on person-departs)
  field: stop-car       \ where their car waits meanwhile: a car route's start (0: where it is; -1: off the roads)
end-structure

begin-structure person
  field: first-stop     \ their rows, one after another (0: no schedule)
  field: #stops
  field: at-stop        \ the stop the clock puts them at
  field: figure         \ their model (0: not loaded yet)
  field: travelling     \ between stops, walking their route
  field: person-area   field: on-floor   field: px   field: py   field: pz   field: heading-now   \ where they are
  field: ride-seat      \ the car's seat they get out by (src: a->ride_seat)
  field: tilt-x   field: leaning-z   \ their tilt (src: rot_x, rot_z: a prop on its side, the lookout leaning)
  field: kept-heading   field: kept-anim   \ (src: actor_save_state: kept to come back to)
  field: walking        \ walking to a point (src: ACTOR_F_NAV)
  field: goal-x   field: goal-z   field: stride   field: stride-motion
  field: riding         \ the car they ride in (0: none)
  field: forward        \ going straight ahead, units a frame (0: not; src: move_speed)
  field: forward-motion \ in this motion (-1: whatever plays; src: move_anim)
  field: seat-bar   field: seat-no   \ the stool they have (bar: at-diner or at-club; 0: none - src: talk_state 2, seat)
  field: shown-kind   field: figure-kind   \ the kind whose model they show (0: their own - src: +0x29F, a variant)
  field: turning   field: turn-target   field: turn-speed
  field: at-door   field: door-wait   field: answered-door   field: answered-side   \ at a door they answered (src: ACTOR_F_DOOR,
                                                                         \  door_wait, door, door_side)   \ turning toward a heading, a speed a frame (src: turn_target)
  playing +field moving \ the motion they hold
end-structure
create people  #slots person * allot   people #slots person * erase
: person-of ( slot -- addr )  person * people + ;
: row-of ( person i -- row )  stop-row * swap first-stop @ + ;

variable writing   variable writing-row   \ (the rows being written: townsfolk/rows.fs)

\ ---- where they are ----
: scheduled? ( slot -- flag )  person-of first-stop @ 0<> ;
\ the extras: anyone without a schedule (a bouncer, a fan, the food on the counter) - where the scenes put them,
\ in the area being played (src: actor_set_pos: an actor has no area; put at (0, 0), parked off the scene)
: extra? ( slot -- flag )  dup 0= over first-car - #cars u< or  swap scheduled? or 0= ;
: no-extras ( -- )  #slots 0 do  i extra? if  -1 i person-of person-area !  then  loop ;
no-extras
: figure? ( slot -- flag )  dup scheduled? swap extra? or ;   \ (the scenes' figures but Matthew's and the cars)
: stop-for ( person t -- i )   \ the first stop whose time hasn't come (after the last: the last)
    {: p t :}  p #stops @ 0 ?do  p i row-of stop-until @ t > if  i unloop exit  then  loop  p #stops @ 1- ;
: stop-of ( slot -- stop | -1 )   \ the stop they stand at (-1: walking, or no schedule)
    dup scheduled? 0= if  drop -1 exit  then  person-of dup travelling @ if  drop -1  else  at-stop @  then ;
: current-row ( slot -- row )  person-of dup at-stop @ row-of ;
: off-schedule? ( slot -- flag )   \ stop -1: their schedule set aside (src: Winona's, while she's with Matthew)
    dup scheduled? if  person-of at-stop @ 0<  else  drop false  then ;

\ the facts the areas' rules ask
<PRIVATE   \ (the cast's names are the events': events.fs)
2 constant nozz
PRIVATE>
0 constant behind-the-counter   \ (Nozz's record 0: his shop's counter)
: nozz-behind-counter? ( -- flag )   \ src: func_80026C50
    nozz stop-of 0< if  false exit  then  nozz current-row stop-record @ behind-the-counter = ;
: standing-at? ( slot stop -- flag )   \ at that stop of theirs, not walking (src: the handlers' stop checks)
    over stop-of 0< if  2drop false exit  then  swap person-of at-stop @ = ;
: at-record? ( slot record -- flag )   \ standing at a stop of that record (src: at_place; the handlers' checks)
    {: s r :}  s stop-of 0< if  false exit  then  s current-row stop-record @ r = ;

: kind-of ( slot -- kind )  dup person-of shown-kind @ ?dup if  nip  then ;   \ the kind whose model they show
: person-now ( slot -- x y z heading )  person-of {: p :}  p px @  p py @  p pz @  p heading-now @ ;
: stands-at? ( slot x z -- flag )  {: s x z :}  s person-now drop nip  z =  swap x = and ;   \ (src: the at_xz checks)
6 constant the-doctor   $545CB constant bedside-x   $45A6E constant bedside-z
: james-visiting? ( -- flag )  the-doctor bedside-x bedside-z stands-at? ;   \ src: func_8002CBE8 (by Matthew's bed)
\ Mel on his bike: while it's his partner he's on it (src: func_80037D50's end: placed there, its way, riding)
13 constant bike-rider   40 constant mels-bike   $1652 constant riding-mels-bike
: on-mels-bike? ( slot -- flag )  dup bike-rider =  swap person-of riding @ mels-bike = and ;
: here? ( slot -- flag )   \ in the area being played, not in a car (but Mel on his bike)
    dup figure? 0= if  drop false exit  then
    dup on-mels-bike? {: s bike :}  s person-of {: p :}
    p person-area @ area @ =  p person-area @ 0< 0= and  p riding @ 0= bike or and ;
: walking? ( slot -- flag )  person-of walking @ 0<> ;
: travelling? ( slot -- flag )  person-of travelling @ 0<> ;   \ between stops (src: a->travelling)
: last-stop ( slot -- stop )  person-of at-stop @ ;          \ the stop they're at or left (src: a->stop)
: riding? ( slot -- flag )  person-of riding @ 0<> ;
: person-turned? ( slot -- flag )  person-of turning @ 0= ;
: seated? ( slot -- flag )  person-of seat-bar @ 0<> ;
: seat-of ( slot -- bar seat )  person-of dup seat-bar @ swap seat-no @ ;

\ ---- the stools: the diner's counter, Club Wolves' bar, the lake hut's (src: SREG.C seat_*) - who sits on which ----
begin-structure stools
  field: stools-count   field: stools-sitters   field: stools-area
  field: stools-x   field: stools-y   field: stools-z   field: stools-facing
  field: stools-along-x?                       \ the stools run along x (else z), each its table's offset on
  2 cells +field stools-table                  \ their offsets (an executable table's name)
  2 cells +field stools-order                  \ the order the townsfolk take them in (none: the first free)
end-structure
create bars  5 stools * allot   bars 5 stools * erase   \ (by placed: at-diner, at-club, at-lake)
: bar-of ( bar -- addr )  stools * bars + ;
: two! ( addr len dest -- )  tuck cell+ !  ! ;
: stools! ( bar count area x y z facing along-x? table len order len -- )
    {: b n ar x y z h ax t tl o ol :}  b bar-of {: r :}
    n r stools-count !  here n cells allot  r stools-sitters !  ar r stools-area !
    x r stools-x !  y r stools-y !  z r stools-z !  h r stools-facing !  ax r stools-along-x? !
    t tl r stools-table two!  o ol r stools-order two! ;
4 constant at-lake   21 constant the-diner   12 constant the-club   9 constant the-lake
at-diner 9 the-diner  $502D0 $FF0 negate $4182A 3072 false  s" diner-seats" s" diner-seat-order" stools!
at-club 6 the-club  $720C1 0 $46C49 2048 true  s" club-seats" s" club-seat-order" stools!
at-lake 4 the-lake  $5130B 0 $6AE7D 3072 false  s" lake-seats" 0 0 stools!
: free-all ( bar -- )  bar-of {: r :}  r stools-sitters @ r stools-count @ cells -1 fill ;
: no-sitters ( -- )   \ (-1: free)
    at-lake 1+ at-diner do  i free-all  loop ;
no-sitters
: sitters ( bar -- addr )  bar-of stools-sitters @ ;
: #stools ( bar -- n )  bar-of stools-count @ ;
: sitter ( bar seat -- slot | -1 )  cells swap sitters + @ ;
: seat-taken? ( bar seat -- flag )  sitter 0< 0= ;
1 constant stools-floor   $77 constant seated-motion
: seat-spot ( bar seat -- x y z heading )   \ src: seat_diner_place, seat_club_place, seat_lake_place
    {: b n :}  b bar-of {: r :}  r stools-table 2@ swap n exe-s16 {: d :}
    r stools-along-x? @ if  r stools-x @ d +  r stools-y @  r stools-z @
    else  r stools-x @  r stools-y @  r stools-z @ d +  then  r stools-facing @ ;
$7202F constant club-approach-x   $46C77 constant club-approach-z   $502D0 constant diner-approach-x
: seat-approach ( bar seat -- x z )   \ where one walks to before sitting (src: actor commands 0x39, 0x44)
    {: b n :}  b at-club = if  n 400 * club-approach-x +  club-approach-z
    else  diner-approach-x  s" diner-seat-approach" n exe-s32  then ;
: seat-area ( bar -- area )  bar-of stools-area @ ;
: free-seat ( bar -- seat | -1 )   \ the first free in the order they're taken (src: seat_*_pick_free)
    {: b :}  b bar-of stools-order 2@ swap {: a n :}
    b #stools 0 do
        n if  a n i exe-u8  else  i  then  b over seat-taken? 0= if  unloop exit  then  drop
    loop  -1 ;
\ ---- a companion: someone going along with Matthew (src: actor_ride, D_800A37B0 / D_800A37B4) ----
variable companion   -1 companion !   \ who (-1: no one)
variable companion-mode                \ 0 stopped where they are, 1 following him on foot, 2 in his car
1 constant along-on-foot   2 constant along-in-the-car   26 constant his-car-slot
: minutes-to-go ( slot -- n )   \ till they leave their stop (src: the handlers' cmd 0x52; walking: 0)
    dup stop-of 0< if  drop 0 exit  then  dup current-row stop-until @  swap drop  clock-time - ;
: at-a-door? ( slot -- flag )  person-of at-door @ 0<> ;
: door-of-theirs ( slot -- door side )  person-of dup answered-door @ swap answered-side @ ;
: door-time-up? ( slot -- flag )  person-of door-wait @ 0<= ;
: going-along? ( slot -- flag )  companion @ = ;          \ (src: ACTOR_F_RIDING)
: following? ( -- flag )  companion @ 0< 0=  companion-mode @ 0<> and ;   \ (src: D_800A37B4 with D_800A37B0)
: person-anim-done? ( slot -- flag )  person-of moving {: m :}  m done @  m anim @ 0< or ;

<PRIVATE   \ (their workings: the messages are their face)
\ ---- what moves them ----
: show ( slot id next loop -- )  {: s id nx lp :}  id nx lp  s person-of moving play ;
: to-traffic ( -- id )  s" traffic" actor-named ;

\ riding in their car (docs/subsystems/traffic.md)
$1645 constant getting-in      \ the motion of climbing in (the town's own)
1024 constant get-in-turn      \ turned so from the car's way (src: func_80029AEC, 0x1400 of a turn)
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
    x z h p ride-seat @ seat beside  p pz !  p px !  y p py !  h p heading-now !
    0 p riding !  0 p ride-seat !  the-town p person-area ! ;
: unload ( car -- )  #slots 0 do  i scheduled? if  i person-of riding @ over = if  i over get-out  then  then  loop  drop ;
: ride-on ( slot row -- )   \ a stop with them riding: their car on its way (unless it is already)
    {: s r :}  s partner-of {: c :}  c 0= if  exit  then  s person-of {: p :}
    c car-carrying? if  c p riding !  the-town p person-area !  else  s r stop-x @ r stop-z @ attach  then ;
: park-car ( slot row -- )   \ their car where the stop leaves it (src: actor_apply_placement's partner lines)
    {: s r :}  s partner-of {: c :}  c 0= if  exit  then
    r stop-car @ {: line :}
    line 0= if  c to-traffic send car-stop  exit  then
    line 0< if  c to-traffic send car-hide  else  c line to-traffic send car-park  then ;
: take-seat ( slot bar seat -- )   \ src: seat_*_take (-1: the first free)
    {: s b n :}  n 0< if  b free-seat to n  then  n 0< if  exit  then
    s person-of {: p :}  b p seat-bar !  n p seat-no !  s  b sitters n cells + ! ;
: leave-seat ( slot -- )   \ src: seat_*_leave
    person-of {: p :}  p seat-bar @ 0= if  exit  then
    -1  p seat-bar @ sitters p seat-no @ cells + !  0 p seat-bar ! ;
: sit ( slot -- )   \ on their stool (src: seat_*_place: there, facing the counter, seated)
    {: s :}  s person-of {: p :}  p seat-bar @ 0= if  exit  then
    p seat-bar @ p seat-no @ seat-spot  p heading-now !  p pz !  p py !  p px !
    p seat-bar @ seat-area p person-area !  stools-floor p on-floor !
    s seated-motion -1 true show ;
: place-at-stop ( slot -- )   \ src: actor_apply_placement (cmd 0x2D) - off their schedule, nowhere to put them
    dup off-schedule? if  drop exit  then  {: s :}  s person-of {: p :}  s current-row {: r :}
    0 p walking !  0 p travelling !  0 p riding !  r stop-variant @ p shown-kind !
    r stop-placed @ partnered = if  s leave-seat  s r ride-on  exit  then
    s r park-car
    r stop-placed @ at-diner =  r stop-placed @ at-club = or if   \ (already on one: they stay)
        s seated? 0= if  s r stop-placed @ -1 take-seat  then  s sit  exit  then
    s leave-seat
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
: on-ground ( person -- )   \ where the ground's followed (the town, an outdoor area's shell: src func_8001C264,
    {: p :}  p person-area @ area @ <>  ground-follows? 0= or if  exit  then   \ func_8001C3D4), their height its
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
: through-door ( slot door room flip stage -- )   \ src: func_8004535C (the door opening: events.fs's door-walk)
    {: s n room flip stage :}  n room flip door-way {: w :}  w 0= if  exit  then
    s person-of {: p :}  0 p walking !
    stage 0< if  w out-area @  else  stage  then  p person-area !  w out-x @ p px !  w out-z @ p pz !  w out-heading @ p heading-now !
    p person-area @ p on-floor @ floor-height p py !
    s  w out-way @ if  through-door-1  else  through-door-0  then  -1 false show
    true p moving hold-move ! ;

\ the minute's rules (src: npc_clock_tick: cmd 0x19 standing, 0x1A walking - while Matthew is the player's)
variable controlled
: minute ( t -- )
    {: t :}  #slots 0 do
        i scheduled?  i going-along? 0= and  i off-schedule? 0= and if   \ (one going along with Matthew keeps with him)
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
    {: t :}  no-sitters  #slots 0 do  0 i person-of seat-bar !  loop  #slots 0 do
        i scheduled?  i off-schedule? 0= and if  i  i person-of t stop-for  goto-stop  then
    loop ;

\ ---- drawing those where Matthew is ----
: model-of ( slot -- model )   \ (loaded again when the kind it shows changes)
    {: s :}  s person-of {: p :}  s kind-of {: k :}
    p figure @  p figure-kind @ k = and if  p figure @ exit  then
    k kind-model model  dup p figure !  k p figure-kind !  ;
: draw-person ( slot -- )
    dup model-of {: s m :}  s person-of moving {: mv :}
    s person-of tilt-x @  s person-of leaning-z @ figure-tilt  m  mv clip @  mv frame  s person-now  draw-figure ;
: draw-townsfolk ( -- )  #slots 0 do  i here? if  i draw-person  then  loop ;
' draw-townsfolk on-draw
: step-forward ( slot -- )   \ src: actor_update's move_anim: straight on along their heading
    {: s :}  s person-of {: p :}
    p forward-motion @ 0< 0=  p moving anim @ p forward-motion @ <> and if  s p forward-motion @ -1 true show  then
    p heading-now @ s>f 2e f* pi f* 4096e f/ {: F: a :}  p forward @ s>f game-frames f* {: F: v :}
    p px @ s>f  a fsin v f* f+ f>s p px !   p pz @ s>f  a fcos v f* f+ f>s p pz ! ;
: turn-a-step ( slot -- )
    person-of {: p :}  p heading-now @ p turn-target @ p turn-speed @ heading-step  if  0 p turning !  then
    p heading-now ! ;
: shows ( slot id -- )   \ that motion, looping, unless it plays already
    {: s id :}  s person-of moving anim @ id = if  exit  then  s id -1 true show ;
$200 constant too-far   $1FF constant kept-at   $100 constant too-near   $101 constant stepped-to
$40000 constant too-far-squared   $FFFF constant near-squared
0 constant standing-by   1 constant catching-up   2 constant stepping-back
: behind-him ( heading distance -- x z )   \ that far behind Matthew, along the heading toward him
    {: h d :}  matthew-now drop {: mx my mz :}  h s>f 2e f* pi f* 4096e f/ {: F: a :}
    mx s>f a fsin d s>f f* f- f>s  mz s>f a fcos d s>f f* f- f>s ;
: follow ( -- )   \ src: func_80037944: facing him, kept 0x100 .. 0x200 behind: pulled up, or stepping back
    companion @ {: s :}  s person-of {: p :}  matthew-now drop {: mx my mz :}
    area @ p person-area !  floor @ p on-floor !
    mx p px @ -  mz p pz @ -  {: dx dz :}  dx dz heading-to {: h :}  h p heading-now !
    dx dx * dz dz * + {: d2 :}
    dx too-far >  dz too-far > or  d2 too-far-squared > or if
        h kept-at behind-him  p pz !  p px !  s catching-up shows
    else  dx too-near <  dz too-near < and  d2 near-squared <= and if
        p px @ p pz @ {: ox oz :}  h stepped-to behind-him  p pz !  p px !
        p px @ p pz @ floor @ blocked? if  ox p px !  oz p pz !  s standing-by shows  else  s stepping-back shows  then
    else  s standing-by shows  then  then
    area @ floor @ floor-height p py ! ;
: door-waits ( -- )   \ those at a door count down, facing him (src: npc_clock_tick: a frame each)
    matthew-there? 0= if  exit  then  matthew-now drop {: mx my mz :}
    #slots 0 do  i person-of {: p :}
        p at-door @ if
            -1 p door-wait +!  mx p px @ -  mz p pz @ -  heading-to p heading-now !
        then
    loop ;
: ride-mels-bike ( -- )
    bike-rider on-mels-bike? 0=  mels-bike car-shown? 0= or if  exit  then
    bike-rider person-of {: p :}  mels-bike car-now {: x y z h :}
    x p px !  y p py !  z p pz !  h p heading-now !  the-town p person-area !
    bike-rider person-anim-done? if  riding-mels-bike -1 true  p moving play  then ;
: animate ( -- )   \ walking and motions for everyone (the routes wait on them, seen or not)
    door-waits  ride-mels-bike
    companion-mode @ along-on-foot =  companion @ 0< 0= and  matthew-there? and if  follow  then
    #slots 0 do
        i scheduled?  i extra? i person-of person-area @ 0< 0= and  or if
            i walking? i riding? 0= and if  i walk-step  then   \ (riding: their walk waits for the car)
            i person-of forward @ if  i step-forward  then
            i person-of turning @ if  i turn-a-step  then
            i here? if  i person-of moving  i person-now drop nip  advance-heard  else  i person-of moving advance  then  drop
        then
    loop ;

PRIVATE>

: anim-now ( slot -- id next )  person-of moving dup anim @  swap next-anim @ ;   \ (a fact: the motion playing, what follows)
: person-pose ( slot -- model motion frame x y z heading )   \ as drawn now (a fact: where a part of them is)
    dup model-of {: s m :}  s person-of moving {: mv :}  m  mv clip @  mv frame  s person-now ;

state: townsfolk-state end-state
behaviour townsfolk
  on spawned ( -- )  self subscribe tick  self subscribe time-set  self subscribe minute-passed
    self subscribe car-arrived  self subscribe left-the-car
    self subscribe player-control ;
  on tick ( -- )  ui-busy? 0= if  animate  then ;   \ (src: cobj.c: no moving while ui_busy)
  on time-set ( t -- )  settle-all ;
  on minute-passed ( t -- )  controlled @ if  minute  else  drop  then ;
  on player-control ( flag -- )  controlled ! ;
  on person-turn ( slot heading -- )  swap person-of  0 over turning !  heading-now ! ;
  on person-tilt ( slot rx rz -- )  rot person-of {: x z p :}  x p tilt-x !  z p leaning-z ! ;
  on person-turning ( slot target speed -- )
    rot person-of {: t v p :}  v 0= if  t p heading-now !  0 p turning !  exit  then
    t full-circle 1- and p turn-target !  v p turn-speed !  true p turning ! ;
  on person-place ( slot x z -- )  rot person-of {: x z p :}  x p px !  z p pz ! ;
  on extra-place ( slot x y z -- )   \ (0, 0): parked; a height of 0: on their floor (1 unless a scene says)
    {: s x y z :}  s person-of {: p :}  0 p walking !  0 p forward !
    x 0=  z 0= and if  -1 p person-area !  exit  then
    area @ p person-area !  p on-floor @ 0= if  1 p on-floor !  then
    x p px !  z p pz !  y ?dup 0= if  area @ p on-floor @ floor-height  then  p py ! ;
  on person-floor ( slot floor -- )
    swap person-of {: f p :}  f p on-floor !  p person-area @ f floor-height p py ! ;
  on person-walk ( slot x z speed motion -- )
    {: s x z v m :}  s person-of {: p :}  x p goal-x !  z p goal-z !  v p stride !  m p stride-motion !
    true p walking !  s m -1 true show ;
  on seat-take ( slot bar seat -- )  take-seat ;
  on seat-sit ( slot -- )  sit ;
  on seat-leave ( slot -- )  leave-seat ;
  on person-ride ( slot mode -- )  companion-mode !  companion ! ;   \ src: actor_ride (cmd 0x26)
  on person-unride ( slot -- )  going-along? if  -1 companion !  0 companion-mode !  then ;   \ src: actor_unride
  on companion-halt ( -- )  0 companion-mode ! ;   \ they stay where they are (src: D_800A37B4 = 0)
  on person-at-door ( slot door side -- )   \ they answer it (src: door_answer: door, side; door_wait 0xFF)
    rot person-of {: n sd p :}  n p answered-door !  sd p answered-side ! ;
  on person-door-hold ( slot -- )  person-of {: p :}  true p at-door !  255 p door-wait ! ;   \ cmd $53
  on person-door-free ( slot -- )  person-of 0 swap at-door ! ;   \ back in (src: ACTOR_F_DOOR cleared)
  on person-coord ( slot which v -- )
    rot person-of {: w v p :}  w 0 = if  v p px !  then  w 1 = if  v p py !  then  w 2 = if  v p pz !  then ;
  on person-nudge ( slot dx dy dz -- )  {: s dx dy dz :}  s person-of {: p :}  dx p px +!  dy p py +!  dz p pz +! ;
  on person-keep ( slot -- )  person-of {: p :}  p heading-now @ p kept-heading !  p moving anim @ p kept-anim ! ;
  on person-back ( slot -- )
    dup person-of {: s p :}  p kept-heading @ p heading-now !  0 p turning !
    p kept-anim @ 0< 0= if  s p kept-anim @ -1 true show  then ;
  on person-variant ( slot kind -- )  swap person-of shown-kind ! ;   \ src: actor_model_variant
  on person-away ( slot -- )  person-of {: p :}  true p travelling !  0 p walking !  -1 p person-area ! ;
  on person-unlink ( slot -- )  person-of 0 swap riding ! ;
  on person-partner ( slot car -- )  swap person-of riding ! ;   \ src: func_80029B20: their car their partner again
  on person-idle ( slot -- )  dup person-of {: p :}  0 p walking !  0 p forward !  0 -1 true show ;
  on person-forward ( slot speed motion -- )  rot person-of {: v m p :}  v p forward !  m p forward-motion ! ;
  on person-anim ( slot id next loop -- )  show ;
  on person-hold ( slot -- )  person-of moving true swap hold-move ! ;
  on person-arrive ( slot -- )  arrive ;
  on person-goto-stop ( slot stop -- )  goto-stop ;
  on person-restore ( slot stop kind -- )   \ src: actor_load: at the stop, placed; the kind it showed
    {: s st k :}  s scheduled? 0= if  exit  then   \ (the others: as their areas place them)
    s person-of {: p :}  st p at-stop !  0 p travelling !  s place-at-stop
    k s = if  0  else  k  then  p shown-kind ! ;   \ (a slot is its own kind)
  on person-set-stop ( slot stop travelling -- )  rot person-of {: st tr p :}  st p at-stop !  tr 0<> p travelling ! ;
  on person-depart ( slot -- )  depart ;
  on person-through-door ( slot door room flip stage -- )  through-door ;
  on person-attach ( slot route speed -- )  attach ;
  on person-get-in ( slot -- )  get-in ;
  on person-in-car ( slot car -- )   \ src: cmd 0x50: parked, that car their partner - out when it unloads
    swap person-of {: c p :}  0 p walking !  0 p forward !  c p riding !  the-town p person-area ! ;
  on car-arrived ( car -- )  unload ;
  on left-the-car ( -- )   \ src: car_exit: his car's riders out by their seats, one along with him on foot again
    his-car-slot unload  companion-mode @ along-in-the-car = if  along-on-foot companion-mode !  then ;
  on person-passenger ( slot car seat -- )   \ src: car_enter_with_passenger: in that car, out by that seat
    rot person-of {: c st p :}  0 p walking !  0 p forward !  c p riding !  st p ride-seat !  the-town p person-area ! ;
end-behaviour

: spawn-townsfolk ( -- id )  townsfolk townsfolk-state s" townsfolk" spawn ;
