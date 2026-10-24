# Traffic

## Purpose

The town's cars, and the people riding in them (src: `carline.c`, `cobj.c`: actor_attach, car_unload_riders,
actor_apply_placement's partner lines; `CMESHAND.C`). 35 cars - actor slots 26-60 - drive routes through the
road graph; a person with a car (the partners table) has it parked somewhere at each stop of their day, walks to
it on a route, gets in and rides; at the route's end they get out beside their seat.

## Data

- **The road graph** (the library: `cdbin/033_4/5/1.roads.txt`, `2.roads.txt`; engine words `road-node ( i -- x
  y z points first )`, `road-point ( i -- dx dy dz length )`): 170 nodes; the road leaving a node bends along its
  run of points (steps).
- **The routes** (`scripts/events/car-routes.fs`, converted once by `tools/carroutes2forth.py`, 181):
  ```forth
    7 car-route  6 lane  81 79 59 62 64 66 116 60  8 roads  0 parks  end-car-route
   23 car-route  1 lane  35 36 40 ... 32  20 roads  1 loops-to  end-car-route
  ```
  the parking node whose lanes it starts in, the nodes it drives through, and where it parks - or the node it
  drives on from again, going round. Row words in `traffic.rows`.
- The executable's tables: `partners` (a person's slot and their car's), `car-seats` (6 seats: side, forward),
  `lane-steps` (a parking node's lanes: x, z apart).
- A person's schedule (docs/subsystems/townsfolk.md) says where their car waits at each stop - `7 car-at` (the
  start of car route 7), `car-hidden` (off the roads) - and a stop where they ride: `23 360 with-partner` (their car
  drives route 23 at 360).

## Rules (`scripts/traffic.fs`)

- Driving: a car goes straight to the route's next node, then along that node's bends, one after another
  (src: car_next); its speed eases to its top speed by 5 a frame; it turns toward the stretch ahead a quarter of
  the way a frame, at once within 64.
- Parking: a car parking takes the lowest lane no other car at that node has, a lane's step aside each; it
  swings into the lane two bends before the end, slowing (200, then 100 a frame); at the end it stands.
- A route that goes round drives on from its node again, without end.
- Frames are game frames: 15 a second indoors, 30 in the town (`game-frames`).

## Messages

`car-park ( car route )` (at the route's start, standing), `car-hide`, `car-drive ( car route speed )`,
`car-carry ( car route speed )` (riders: at the end `car-arrived ( car )` is broadcast), `car-stop`,
`car-switch ( car route )` (at that route's first node it turns onto it: cmd 0x37), `car-place ( car x y z heading )` (off its route, standing there: Matthew's car as he drives it, driving.md), `car-anim ( car id next )`
(a motion of its own, drawn posed: the bike's). Facts: `car-motion-now`,
`car-now ( car -- x y z heading )`, `car-driving?`, `car-carrying?`, `car-shown?`, `partner-of ( slot -- car | 0 )`,
`seat ( i -- side forward )`, `beside ( x z heading side forward -- x z )`.

## Riding (the townsfolk)

- `attach` (cmd 0x2C, `a route speed`): they ride in their car (not drawn), which carries them along the route;
  `car-arrived`: they get out beside the first seat, facing the car's way, in the town.
- The actor commands (`events.fs`: `act-command`): $41 / $51 walk to their car's first / second seat side, $5A /
  $5B to someone's (a car's) - walking 30 a frame; $42 they turn across the car and climb in (motion $1645, the
  town's); $4B nothing for them.
- `wait-follow` (op $138): until their car has arrived.
- A route may walk them on at once, riding: their walk waits while they ride and goes on from beside the car
  (the original put a rider at 0, 0 and their walk ran on from wherever the car set them down).
- Some trips go on with another script when the car arrives, by the stop they left (`routes.fs`: `goes-on`;
  src: the handlers' CMD_LINK_NOTIFY - Nozz at stops 2 and 6, Cohen at 1, 3, 5, 13 and 6); the scripts are
  converted with the routes.
- At a stop: their car is parked (`car-at`), hidden or stopped; a stop where they ride sets their car on its way
  (unless it is already carrying them).

## A scene's moves

The events' actor commands work on the cars too (src: the same commands on a car actor, actor_update moving
it): placed (`car-put`, its heading kept), turned at once (`car-heading`) or at a speed (`car-turning`), driven
to a point facing it (`car-walk`) or straight on (`car-forward`) until idle (`car-idle`), its own motion
(`car-anim`), and put in another area (`car-area`, from set-floor: drawn where it is - the town's by default).
Facts `car-moving?`, `car-turned?`, `car-scene-on?`, `car-in-area`; the events' wait-stop and wait-turned wait
on them. While a scene moves his car he's carried in it (driving.fs follows it).

## On the slopes

A car on its way in the town is tilted with the ground under it (src: func_8001C264 with its tilt: the engine's
`ground-tilt` from the triangle found; `car-tilt-now`).

## Pushed aside, Mel's bike

- A car on its way pushes Matthew (640) and his car (1024) out of its way in the town: within that room of it
  (both ways, then round) each is put at its edge, on the line between them (src: func_80037D50, D_80093914).
- Mel on his bike: while it's his partner (riding it, or `person-partner`, src func_80029B20) he's on it - its
  place and way, riding (motion $1652), seen (src: func_80037D50's end).

`tests/library/test_traffic.fs`.

## Status

Built. (The cars move only for their people and the scenes - src carline_start's callers; they have no lights
of their own.)
