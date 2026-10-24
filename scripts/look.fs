\ look.fs - looking and acting (docs/subsystems/look.md; src: cecheck.c, cevcon.c look_update / act_update /
\ look_execute / act_execute): what ○ and □ would do where Matthew stands, and doing it. The tables' rows
\ (scripts/events/checks.fs) and each area's rules (scripts/rules/) say what.
IN: look
USING: engine actors messages keys areas floors figures matthew townsfolk events talk ui ;

\ ---- the rows ----
enum: table  look-zones look-checks act-checks act-zones ;
enum: target-kind  ask-the-area run-script run-native ;
$2000 constant in-conversation   $4000 constant greeting   \ a check row's talk states (src: func_8003EF88)
33 constant #content-areas

begin-structure check-row
  field: next-row
  field: row-zone       \ a zone row: the zone's index on its floor; a check row: its id
  field: row-facing     \ a check row: the heading it wants (4096 a turn)
  field: row-within     \ and how far either side (degrees)
  field: row-states     \ a check row: the talk states it's for
  field: row-kind       \ target-kind
  field: row-target     \ the event's word, or the native's address
  field: row-scope      \ the native's scope
  field: row-prompt     \ the hint's text (the table's extra)
end-structure

create first-rows  #content-areas 4 * cells allot   first-rows #content-areas 4 * cells erase
variable building     \ the row being written (look/rows.fs)
: table-list ( area table -- addr )  swap 4 * + cells first-rows + ;
\ the town's: a tile's look and act checks (src: tile_checks; it has no zone tables)
create tile-rows  town-columns town-rows * 2* cells allot   tile-rows town-columns town-rows * 2* cells erase
create tile-sets  town-columns town-rows * allot   tile-sets town-columns town-rows * erase   \ (src: D_8010AB44)
: tile-set! ( col row set -- )  -rot town-columns * + tile-sets + c! ;
: rows-set ( col row -- set )   \ a tile's rows' set (0: none) - the town's act rules key on it (src: D_800A38E4)
    2dup 0 town-rows within  swap 0 town-columns within and 0= if  2drop 0 exit  then
    town-columns * + tile-sets + c@ ;
: town-list ( col row table -- addr )
    {: c r t :}  r town-columns * c + 2*  t act-checks = if  1+  then  cells tile-rows + ;


\ ---- where he stands ----
768 constant face-zone-range   \ src: player_face_zone (0x300)
: at-zone ( -- index id )  matthew-now drop nip floor @ zone-hit ;
: turned ( F: angle -- turn )  4096e f* 2e pi f* f/ ;
: faces? ( x z -- flag )   \ his heading within range of the way to (x, z)
    matthew-now {: tx tz mx my mz h :}  tx mx - s>f tz mz - s>f fatan2 turned f>s  h swap face-zone-range
    heading-within? ;
: faces-zone? ( index -- flag )  matthew-now drop nip floor @ zone-centre faces? ;

\ ---- what a table answers: a row to run, or (asking the area) the zone ----
variable zone-answer   variable zone-match   variable check-answer   variable check-match
: rows ( table -- first )   \ the table's rows here: the area's, or in the town the tile's he stands on
    {: t :}  area @ the-town <> if  area @ content t table-list @ exit  then
    t look-checks =  t act-checks = or 0= if  0 exit  then
    matthew-now drop nip tile-of  t town-list @ ;
: zone-table ( table -- )   \ src: func_8003F1A8
    -1 zone-answer !  0 zone-match !
    at-zone drop {: t idx :}  idx 0< if  exit  then
    t rows  begin  dup while
        dup row-zone @ idx =  idx faces-zone? and if
            dup row-kind @ ask-the-area = if  drop idx zone-answer !  else  zone-match !  then  exit
        then  next-row @
    repeat  drop
    idx faces-zone? if  idx zone-answer !  then ;   \ (src: func_8003FFB0: a zone faced, no row)
: faced? ( row heading -- flag )   \ the heading within the row's range
    {: r h :}  h  r row-facing @  r row-within @ 4096 * 360 /  heading-within? ;
: perch-fits? ( row -- flag )   \ a row for on a crate ($4000) or on a stool ($2000), or for neither
    row-states @ {: st :}  matthew-perch {: p :}
    st $4000 and 0<>  p 1 = =  st $2000 and 0<>  p 2 = = and ;
: check-table ( table -- )   \ src: func_8003EF88 (the rows for his perch: talk_state 1 or 2, else neither)
    -1 check-answer !  0 check-match !
    at-zone nip {: t id :}  id 0< if  exit  then
    matthew-now nip nip nip {: h :}
    t rows  begin  dup while
        dup row-zone @ id =  over perch-fits? and  over h faced? and if
            dup row-kind @ ask-the-area = if  drop id check-answer !  else  check-match !  then  exit
        then  next-row @
    repeat  drop ;
: run-row ( row -- )
    ?dup 0= if  exit  then
    dup row-kind @ case
        run-script of  row-target @ start-event  endof
        run-native of  dup row-scope @ swap row-target @ native  endof
        nip
    endcase ;
: run-rows ( -- )  check-match @ ?dup if  run-row  else  zone-match @ run-row  then ;   \ src: func_800401B0

\ ---- the areas' rules ----
begin-structure rules
  field: look-check-xt   field: look-do-xt   field: act-check-xt   field: act-do-xt
  field: on-enter     field: on-scene     \ the area's hooks: entered ( -- ), a scene ( scene -- )
  field: on-frame     \ and each step while Matthew is the player's ( -- ) (src: the overlay's per-frame hook)
end-structure
create area-rules  #content-areas rules * allot   area-rules #content-areas rules * erase
: rules-of ( area -- addr )
    dup 0 #content-areas within 0= abort" rules-of: no such area"  rules * area-rules + ;
: area-rules! ( look-check-xt look-do-xt act-check-xt act-do-xt area -- )
    rules-of {: lc ld ac ad r :}  lc r look-check-xt !  ld r look-do-xt !  ac r act-check-xt !  ad r act-do-xt ! ;
: area-hooks! ( area enter scene -- )  rot rules-of {: e sc r :}  e r on-enter !  sc r on-scene ! ;
: hook ( xt|0 -- )  ?dup if  execute  then ;
: area-frame! ( area xt -- )  swap rules-of on-frame ! ;

\ ---- pressing ○ and □ ----
81 constant look-motion   \ src: func_8003FA8C (0x51)
variable circle-first   \ a word ○ tries before the rest ( -- handled? ): the car's door (driving.fs)
: circle-first! ( xt -- )  circle-first ! ;
: press-circle ( -- )   \ src: look_execute: a thing before him (case 6: examined), a person (case 1: talking),
    circle-first @ ?dup if  execute if  exit  then  then   \ src: case 5, the car: getting in
    in-front dup 0< 0= if  dup look-at-it if  drop exit  then  talk-with exit  then  drop   \ else the place (case 0)
    look-zones zone-table  look-checks check-table
    area @ content rules-of {: r :}
    r look-check-xt @ if
        look-motion -1 false matthew-actor send figure-anim
        zone-answer @ check-answer @ r look-check-xt @ execute  r look-do-xt @ execute
    else  run-rows  then ;
: press-square ( -- )   \ src: act_execute: a thing before him (case 4: taken), else the place (case 0)
    in-front dup 0< 0= if  take-it if  exit  then  else  drop  then
    act-zones zone-table  act-checks check-table
    area @ content rules-of {: r :}
    r act-check-xt @ if  zone-answer @ check-answer @ r act-check-xt @ execute  r act-do-xt @ execute
    else  run-rows  then ;

variable controlled   variable circle-was   variable square-was
: pressed ( key var -- flag )  >r key-down? dup r@ @ 0= and swap r> ! ;   \ (a held button: once)
variable pad-taken   \ a frame hook took the pad this step (src: the pad's field mode: a stool's keys)
: take-pad ( -- )  true pad-taken ! ;
: watch ( -- )
    pad-circle circle-was pressed  pad-square square-was pressed  {: o s :}
    controlled @ 0=  events-running @ control-given @ 0= and or  area @ 0< or  matthew-there? 0= or if  exit  then
    field-pad? 0= if  exit  then   \ (a screen up: the pad is its own)
    matthew-perch perch-in-car = if  exit  then   \ (driving: the car has the pad)
    events-running @ {: before :}   \ (an event that gave him back: he acts as the player - src: player_has_control)
    false pad-taken !  area @ content rules-of on-frame @ hook
    events-running @ before >  pad-taken @ or if  exit  then   \ (the hook may have started one, or have the pad)
    o if  press-circle exit  then
    s if  press-square  then ;

state: look-state end-state
behaviour looking
  on spawned ( -- )  self subscribe tick  self subscribe player-control
    self subscribe area-entered  self subscribe scene-shown ;
  on area-entered ( area scene -- )   \ src: the overlay's entering, then its scene hook
    swap content rules-of {: sc r :}  r on-enter @ hook  r on-scene @ ?dup if  sc swap execute  then ;
  on scene-shown ( scene -- )  area @ content rules-of on-scene @ ?dup if  execute  else  drop  then ;
  on tick ( -- )  watch ;
  on player-control ( flag -- )  controlled ! ;
end-behaviour

: spawn-look ( -- id )  looking look-state s" look" spawn ;
