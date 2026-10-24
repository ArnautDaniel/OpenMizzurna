\ the town's frame rules on the game's data (rules/area027.fs): its people about on entering (Lorraine by the
\ diner her hours, the road block's officer before the 27th's end); a gas pump with the car by it (○ fills there); the diner
\ robbery outside on the 27th's late morning, once
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags driving
    rules.area027 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: near-the-diner ( -- )  27 -1 diner-x diner-z 600 + 0 0 warp ;
: into-town ( -- )  ['] near-the-diner start-event  60 steps  events-stop  true broadcast player-control  deliver ;
prologue-done flag-set

testing entering at 15:00: Lorraine out by the diner; the road block's officer before the 27th's end
T{ 25 15 0 at-time  into-town  lorraine-out here?  lorraine-out person-now drop 2drop  rules.area027:road-block-officer person-now drop 2drop -> true $5839F $6BA5E }T
testing at 18:00 she's in again
T{ 25 18 0 at-time  4 steps  lorraine-out here? -> false }T
testing a pump: the car in its box, facing along, nearly still, not full - there; full, not
T{ $638FF $29600 0 s" driving" actor-named send his-car-at  50 s" driving" actor-named send fuel-set  deliver
   at-a-pump?  120 s" driving" actor-named send fuel-set  deliver  at-a-pump? -> true false }T
testing the diner robbery outside: the 27th at 11:45 by the diner, on foot - its scene, once
T{ 27 11 45 at-time
   diner-x 0 diner-z s" matthew" actor-named send figure-place  deliver  4 steps
   robbery-outside-seen flag?  events-running @ 0> -> true true }T

test-summary
