\ routes.fs - the townsfolk's routes (docs/subsystems/townsfolk.md): a person leaving a stop walks its route
\ on their own thread (src: depart, event_thread_start on Actor +0x158); put at a stop, they stop walking it.
\ When a person's car arrives, some trips go on with another script (src: the handlers' CMD_LINK_NOTIFY).
IN: routes
USING: engine actors messages events townsfolk traffic ;

\ ---- what a trip goes on with when the car arrives: by whose car it is and the stop they left ----
begin-structure arrival
  field: arrival-slot   field: arrival-stop   field: arrival-route   \ (the route: its address, main-word)
end-structure
8 constant #arrivals
create arrivals  #arrivals arrival * allot   variable arrivals-written
: goes-on ( slot stop address -- )   \ (written below, once)
    arrivals-written @ arrival * arrivals + {: s n r a :}  s a arrival-slot !  n a arrival-stop !  r a arrival-route !
    1 arrivals-written +! ;
nozz 2 $80081F94 goes-on   nozz 6 $80081F74 goes-on
cohen 1 $8008299C goes-on   cohen 3 $8008299C goes-on   cohen 5 $8008299C goes-on   cohen 13 $8008299C goes-on
cohen 6 $800829B4 goes-on

<PRIVATE
: goes-on-with ( slot stop -- address | 0 )
    {: s n :}  arrivals-written @ 0 ?do  arrivals i arrival * +  dup arrival-slot @ s =  over arrival-stop @ n = and
        if  arrival-route @ unloop exit  then  drop  loop  0 ;
: car-owner ( car -- slot | -1 )   \ whose car it is (src: the car's link, ovl033's actor setup)
    {: c :}  #slots 0 do  i scheduled? if  i partner-of c = if  i unloop exit  then  then  loop  -1 ;
: goes-on? ( car -- )   \ the trip goes on, if it does at this stop
    car-owner dup 0< if  drop exit  then
    dup dup person-of at-stop @ goes-on-with main-word ?dup if  swap start-route  else  drop  then ;
PRIVATE>

state: routes-state end-state
behaviour route-runner
  on spawned ( -- )  self subscribe person-departs  self subscribe person-stopped  self subscribe car-arrived ;
  on person-departs ( slot route -- )   \ (no route, or one not converted: they travel until the next row's time)
    main-word ?dup if  swap start-route  else  drop  then ;
  on person-stopped ( slot -- )  end-route ;
  on car-arrived ( car -- )  goes-on? ;
end-behaviour

: spawn-routes ( -- id )  route-runner routes-state s" routes" spawn ;
