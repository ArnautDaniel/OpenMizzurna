\ the cars (docs/subsystems/traffic.md), on the game's own data: the library the tests imported
USING: tester engine actors messages traffic townsfolk events events.car-routes ;
39 constant nozz-car

testing who goes with whom (the partners table): Nozz drives car 39, Matthew car 26; slot 4 has none
T{ nozz partner-of  0 partner-of  4 partner-of -> nozz-car 26 0 }T

testing a car's seats, and a point beside something: along its heading, turned with it
T{ 0 seat  5 seat -> -670 100 0 1000 }T
T{ 0 0 0 0 100 beside  0 0 1024 0 100 beside -> 0 100 100 0 }T

testing the routes: 7 starts in node 6's lanes and drives 8 nodes from 81 to park at node 0; 23 goes round
T{ 7 route-of route-lane @  7 route-of #route-nodes @ -> 6 8 }T
T{ 7 route-of 0 route-node  7 route-of route-end-at @ -> 81 0 }T
T{ 23 route-of route-ends @  23 route-of route-end-at @ -> ends-going-round 1 }T

testing the road graph from the library
T{ roads-ready?  0 road-node -> true 373695 -4000 281926 2 0 }T

testing a car parked at a route's start: at its first node (the lane's first car: no offset), not driving
: the-traffic ( -- id )  s" traffic" actor-named ;
: x-z ( x y z -- x z )  nip ;
T{ nozz-car 7 the-traffic send car-park  deliver  nozz-car car-driving? -> false }T
T{ nozz-car car-now drop x-z -> 81 road-node 2drop x-z }T

testing driving: on its way, and at its route's end it parks and its riders get out
: steps-until-parked ( -- n )   \ (the tick, as the engine sends it)
    0 begin  1+  broadcast tick  deliver  nozz-car car-driving? 0=  over 20000 > or  until ;
T{ nozz-car 11 3000 the-traffic send car-drive  deliver  nozz-car car-driving? -> true }T
T{ steps-until-parked 20000 < -> true }T
T{ nozz 11 3000 s" townsfolk" actor-named send person-attach  deliver  nozz riding?  nozz here? -> true false }T
T{ nozz-car car-carrying? -> true }T
T{ steps-until-parked 20000 <  deliver  nozz riding?  nozz-car car-carrying? -> true false false }T

testing the rider got out beside the first seat, facing the car's way
: seat-0-of-car ( -- x z heading )
    nozz-car car-now {: x y z h :}  x z h 0 seat beside  h ;
: nozz-place ( -- x z heading )  nozz person-now {: x y z h :}  x z h ;
T{ nozz-place -> seat-0-of-car }T

testing riding pauses their walk: they walk on from beside the car once it has arrived
T{ nozz 11 3000 the-townsfolk send person-attach  nozz 1000 2000 30 1 the-townsfolk send person-walk  deliver
   nozz-place  broadcast tick  deliver  nozz-place  -> nozz-place nozz-place }T
T{ steps-until-parked drop  deliver  nozz riding?  nozz walking? -> false true }T

testing a trip that goes on when the car arrives: Nozz at his stop 2 (src: his CMD_LINK_NOTIFY)
T{ nozz 2 the-townsfolk send person-goto-stop  deliver  nozz route-thread -> -1 }T
T{ nozz 11 3000 the-townsfolk send person-attach  deliver  steps-until-parked drop  deliver  deliver
   nozz route-thread 0< -> false }T

test-summary
