\ townsfolk.fs - the people of the town and their days (docs/subsystems/townsfolk.md; src: cobj.c, the
\ characters' handlers, timetable.c). Each person with a schedule (scripts/events/schedules.fs) is where the
\ clock puts them: at the first stop whose time hasn't come. Those in the area Matthew is in are drawn, in their
\ stop's motion. Facts: stop-of, the stop's place; the townsfolk actor keeps them as the time moves.
IN: townsfolk
USING: engine actors messages clock areas figures ;

182 constant #slots
enum: placed  standing partnered at-diner at-club ;   \ how a stop puts them (src: func_80027944)
$16E constant character-entries                          \ the kinds' models: CD.BIN entries from here

\ ---- the rows ----
begin-structure stop-row
  field: stop-until     \ they stay until (a time: clock.fs)
  field: stop-record    \ which of their placements (the original's index)
  field: stop-area   field: stop-floor   field: stop-x   field: stop-z   field: stop-heading
  field: stop-pose      \ the motion they hold there
  field: stop-placed    \ placed: stand, or with someone, or on a seat
  field: stop-variant   \ another model (0: their own)
  field: stop-route     \ the route they walk on leaving (an event: to come)
end-structure

begin-structure person
  field: first-stop     \ their rows, one after another (0: no schedule)
  field: #stops
  field: at-stop        \ the stop the clock puts them at
  field: figure         \ their model (0: not loaded yet)
  field: heading-now    \ which way they face (their stop's, until turned)
  playing +field moving \ the motion they hold
end-structure
create people  #slots person * allot   people #slots person * erase
: person-of ( slot -- addr )  person * people + ;
: row-of ( person i -- row )  stop-row * swap first-stop @ + ;

variable writing   variable writing-row   \ (the rows being written: townsfolk/rows.fs)

\ ---- where the clock puts them ----
: scheduled? ( slot -- flag )  person-of first-stop @ 0<> ;
: stop-for ( person t -- i )   \ the first stop whose time hasn't come (after the last: the last)
    {: p t :}  p #stops @ 0 ?do  p i row-of stop-until @ t > if  i unloop exit  then  loop  p #stops @ 1- ;
: stop-of ( slot -- stop | -1 )  dup scheduled? if  person-of at-stop @  else  drop -1  then ;
: current-row ( slot -- row )  person-of dup at-stop @ row-of ;
: kind-model ( slot -- addr len )   \ "models/NNN_K.gltf": the kind's model (a slot is its kind)
    >r s" kind-models" r> exe-u8 character-entries +
    $clear  s" models/" $+  dup 5 / dup 100 < if  s" 0" $+  then  dup 10 < if  s" 0" $+  then  n>str $+
    s" _" $+  5 mod n>str $+  s" .gltf" $+  $@ ;

\ the facts the areas' rules ask
<PRIVATE   \ (the cast's names are the events': events.fs)
2 constant nozz   6 constant james
PRIVATE>
0 constant behind-the-counter   \ (Nozz's record 0: his shop's counter)
: nozz-behind-counter? ( -- flag )   \ src: func_80026C50
    nozz scheduled? 0= if  false exit  then  nozz current-row stop-record @ behind-the-counter = ;
: james-visiting? ( -- flag )  false ;   \ src: func_8002CBE8 (James standing by Matthew's bed: the routes)

: settle-all ( t -- )   \ each person to the stop for time t; a new stop's pose starts playing
    {: t :}  #slots 0 do
        i scheduled? if
            i person-of {: p :}  p t stop-for  dup p at-stop @ <> if
                p at-stop !  p dup at-stop @ row-of stop-pose @ -1 true p moving play
                p dup at-stop @ row-of stop-heading @ p heading-now !
            else  drop  then
        then
    loop ;

\ ---- drawing those where Matthew is ----
: here? ( slot -- flag )   \ standing in the area being played
    dup scheduled? 0= if  drop false exit  then
    current-row {: r :}  r stop-placed @ standing =  r stop-area @ area @ = and ;
: model-of ( slot -- model )
    dup person-of figure @ ?dup if  nip exit  then
    dup kind-model model  dup rot person-of figure ! ;
: person-now ( slot -- x y z heading )   \ where they stand (a fact)
    dup current-row {: s r :}  r stop-x @  r stop-area @ r stop-floor @ floor-height  r stop-z @
    s person-of heading-now @ ;
: draw-person ( slot -- )
    dup model-of {: s m :}  s current-row {: r :}  s person-of moving {: mv :}
    m  mv clip @  mv frame  s person-now  draw-figure ;
: draw-townsfolk ( -- )  #slots 0 do  i here? if  i draw-person  then  loop ;
' draw-townsfolk on-draw
: animate ( -- )  #slots 0 do  i here? if  i person-of moving advance drop  then  loop ;

state: townsfolk-state end-state
behaviour townsfolk
  on spawned ( -- )  self subscribe tick  self subscribe time-set  self subscribe minute-passed ;
  on tick ( -- )  animate ;
  on time-set ( t -- )  settle-all ;
  on minute-passed ( t -- )  settle-all ;
  on person-turn ( slot heading -- )  swap person-of heading-now ! ;
end-behaviour

: spawn-townsfolk ( -- id )  townsfolk townsfolk-state s" townsfolk" spawn ;
