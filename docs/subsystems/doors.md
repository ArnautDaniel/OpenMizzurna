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

## The town's doors (`scripts/rules/area027.fs`)

○ at a building's door in the town (the tile's look checks: `checks.fs`, `town-row`) runs the town's C for it
(src: the evfns, `enter`): whether he may go in now (src: ovl027_func_80103A70 - the opening hours by the clock,
some doors by the story's flags or where a person's schedule has them), then the building's script: the camera,
its caption, the door, a fade, and a **warp**. Shut: the "closed" script with the door's message (closed, the diner
not open yet or closing, the hospital's). Nozz's store: shut from midnight to 6:59 and from 17:25 (18:20 from the
29th) to 22:59 - Matthew lives above it.

**Matthew through a door** (`door-animate` op $139, `door-animate-alt` $13A; src: func_80045008): from the room he
is in (the scene; the town's side in the town) he is put at the door's spot on his side, turned through it and
stepping (motion $3F / $40, its move held) - the scripts wait for that; $13A also sets a fixed camera behind him.
The door opens on his side: its spot is the area's animation entry for it (`door-anim`, op $127). (The next
room's objects shown, and the town's doors' own animations: to come.)

**Warp** (op $12A, `events.fs`: `warp ( area scene x z heading script )`; src: func_80019F3C, func_80019D90):
the area changes (scene -1: the town's), Matthew is put at x z facing heading (-1: as he is), the camera behind
him, then the arrival script (the destination area's, by address: converted with the events) or a fade in; the
event is over. Leaving a building is its own script's warp to area 27.

## Now

People go through doors on their routes (`through-door` in an event, for a person: `docs/subsystems/townsfolk.md`).

## Knocking (`scripts/knock.fs`, the actor `knock`; src: SCKNOCK.C door_knock, door_answer, door_resident_leave)

□ at a house's door in the town knocks (`rules/area027.fs`: the act check's answer and the tile's rows' set -
`rows-set`, src D_800A38E4 - make the key: the Astin farm, the Rolands', the Strasbourgs', Mel's, the Lakeside Inn,
James's). `knock ( door -- who | 0 )` follows the original's rules: who's at home (their records), the hours
("Too early", "late in the evening"), the scenes some doors lead to instead (Isabella letting him in, James's
invitation into his house - area 4's visit, its own script - Lorraine's), Winona waiting in the car, otherwise
"no answer" with its reason. `answer ( who side door )`: they step out (the answering scene: he knocks and steps
back, they come through the door the other way and greet him: the line by who it is) and wait in the doorway
facing him (`door-hold`, $53; `person-at-door`); they go back in (their parting line, through the door, to their
stop) after 255 frames, when he's 501 away, or when they're about to leave. One knock at a time.

## To come

the town's doors swinging (its own animation slots), the next room's objects shown as he steps through, the town's doors' remaining rules (Winona
riding along, the diner's robber, Mel's first visit to the diner), the door opening as one passes (its object
animation, when Matthew's room is on one side), the apartment building's doors (area 19's knocks), the door sounds.
