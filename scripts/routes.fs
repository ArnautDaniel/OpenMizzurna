\ routes.fs - the townsfolk's routes (docs/subsystems/townsfolk.md): a person leaving a stop walks its route
\ on their own thread (src: depart, event_thread_start on Actor +0x158); put at a stop, they stop walking it.
IN: routes
USING: engine actors messages events townsfolk ;

state: routes-state end-state
behaviour route-runner
  on spawned ( -- )  self subscribe person-departs  self subscribe person-stopped ;
  on person-departs ( slot route -- )   \ (no route, or one not converted: they travel until the next row's time)
    main-word ?dup if  swap start-route  else  drop  then ;
  on person-stopped ( slot -- )  end-route ;
end-behaviour

: spawn-routes ( -- id )  route-runner routes-state s" routes" spawn ;
