# Doors

## Purpose

The doors between an area's rooms and out to the town (src: `door.c`, func_8004535C, func_80018DA8). A door of
an area joins two of its rooms, or a room and the town (room -2); one side only for some (a door one never comes
back through).

## Rows (`scripts/events/doors.fs`, converted once by `tools/doors2forth.py`)

```forth
 15 door  1 in-area  3 room  6 spot  345166 284954 place  2560 facing  1 way  -2 room  24 spot  345166 284946 place  2560 facing  0 way  end-door
```

Per side: its `room`; its `spot` (the door's place in the area's spots, the town's for room -2: D_80081350,
D_800813D4 - the same index names the door's opening animation); where the spot is, x z, and which way it
faces (4096 a turn); which `way` one comes out there (0 or 1). The row words live in `doors.rows`.

## Going through

`door-way ( door room flip -- way | 0 )`: coming out into `room` (flip: through-door-back, coming the other
way: the other side's `way`): the area (the town for room -2), a step from the spot - out of way 1: 125 back
and 300 to the right, facing the spot's way; out of way 0: turned round, 200 back and 360 to the left - and the
heading. The motion of stepping out is $3F (way 1) or $40 (way 0).

## Now

People go through doors on their routes (`through-door` in an event, for a person: `docs/subsystems/townsfolk.md`).

## To come

Matthew through doors (into another area or room: the area loads), the door opening as one passes (its object
animation, when Matthew's room is on one side), knocking (`door.c`: door_knock, door_answer), the door sounds.
