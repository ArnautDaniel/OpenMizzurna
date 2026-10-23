# Townsfolk

## Purpose

The people of Mizzurna Falls and their days (src: `cobj.c`, the characters' handlers in `handlers_chars.c`,
`timetable.c`; decoded by the decomp's `tools/schedule.py`). Each scheduled person - the named cast with their
own handlers (Morgan, Nozz, Cohen, ... 18) and the 26 townspeople with schedules of the shared handler (person
numbers: slots 65-88 are 0-23, 113-124 are 24-35: src ovl033) - lives by rows: until when they stay at a stop,
where the stop is, the route they walk on leaving it.

## Rows (`scripts/events/schedules.fs`, converted once by `tools/schedules2forth.py`)

```forth
2 schedule
   0 stop  25 11 40 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  $80081ED4 route  end-stop
   ...
end-schedule
```

`until` a day, hour and minute; `record` which of their placements (the rules ask: Nozz's 0 is his counter);
`in-area` area and floor, `place` x z (world), `facing` (4096 a turn), `pose` the motion held; or placed
`with-partner` (riding with someone), at the `diner-seat` / `club-seat` (src: func_80027944 types 1-3);
`variant` another model; `route` the event they walk on leaving; `car-at` / `car-hidden` where their car waits
meanwhile (`docs/subsystems/traffic.md`); `with-partner` takes their car's route and speed.
The row words live apart, in `townsfolk.rows` (`scripts/townsfolk/rows.fs`): `until`, `stop`, `place` would
hide the language's own elsewhere.

## Now

The clock places them: a person stands at the first stop whose time hasn't come (`stop-for`); a jump in time
(`time-set`) puts each at the stop for it. Those standing in the area being played are drawn (their kind's
model - `kind-models`, a slot is its kind - in their own textures) in the stop's motion. Facts: `stop-of` (-1
while walking a route), `person-now`, `here?`, `walking?`, `person-anim-done?`, `at-record?` (standing at a stop of
that record: the handlers' "at the diner", "in the cells" checks), `nozz-behind-counter?` (src: func_80026C50).
`person-unlink` takes a person out of their car where they are (src: partner = 0: the diner's scene on the 25th).

The minute's rules, kept only while Matthew is the player's (src: npc_clock_tick; in a scene time stands for
them):
- standing: at the exact minute of their stop's `until` they **depart** (src: sched_depart): they are
  travelling, and `person-departs ( slot route )` is broadcast;
- travelling: at the next row's minute, still on the way, they are put at that stop at once (src: sched_skip).

## Routes (`scripts/routes.fs`)

A stop's route is event bytecode the person walks themself (`main.fs`: `route-XXXXXXXX`, 429 converted with the
events - the schedules' and those the executable's helpers start: Mel after the diner, James, Lidds - registered by address: `main-word`). The routes actor answers `person-departs` by starting the route on
a thread of its own (threads 8-71: unlike an event's 0-7 it doesn't take the player's control), its own actor
`this-one`; `person-stopped` (put at a stop) ends it. A route ends in `arrive` (cmd 0x1E): the next stop, the
person placed there (src: actor_arrive).

The events' figure ops work for a person (`events.fs`: `person?`, sent to the townsfolk as `person-*`
messages): `set-pos`, `set-rot`, `set-floor`, `walk-to` (30 a frame, motion 1), `run-to` (80, motion 5),
`idle`, `walk-forward` / `move-forward` (straight on along their heading until `idle`), `turn-to`, `anim`,
`anim-then`, `anim-hold`, `wait-anim`, `wait-stop`, `arrive`, `goto-stop`, riding (`attach`, `wait-follow`, the
car commands: `docs/subsystems/traffic.md`), and the
doors: `through-door` / `through-door-back` put them out of a door (`docs/subsystems/doors.md`) in the
stepping-out motion ($3F / $40) and the thread waits for it.
Everyone's motions play, seen or not: a route waits on them wherever its person is.
Turning (src: actor_turn_by / _to_heading / _to_actor, actor_update's turn): `turn-to`, `turn-by` (degrees),
`face` turn them a speed a game frame toward the heading, its sign the way (`person-turning`); `wait-turned`
waits on `person-turned?`.

## Extras

Anyone without a schedule - a bouncer, a fan, the gas station's attendant, the food on the counter (every slot
but Matthew's and the cars' has its kind's model) - is an extra: where the scenes put them (`extra-place ( slot x
y z )`, an event's `set-pos`), in the area being played; put at (0, 0) they are parked off the scene (src:
actor_set_pos: an actor has no area). They walk, turn, play their motions and go through doors like the rest;
the schedule's commands (`arrive`, `goto-stop`, riding) are the scheduled people's alone. `figure?`, `extra?`.
A scene stages the people it sends through a door of the area being played in that area, its town side too
(src: they stand at the door's spot, seen from the room); a route's go where the door leads.

## The stools (src: SREG.C seat_*)

The diner's counter has 9 stools, Club Wolves' bar 6 (`diner-seats`, `club-seats`: their places; x 0x502D0,
z 0x4182A + it facing the counter, or x 0x720C1 + it, z 0x46C49 facing the bar). The townsfolk keep who sits on
which (`sitter`, `seat-taken?`, `seated?`, `seat-of`; Matthew's too). A stop "at the diner" / "at Club Wolves"
puts one on the first free in the game's order (`diner-seat-order`, `club-seat-order`) unless they sit already;
the routes take one (`take-diner-seat`, `take-club-seat`: $3A, $45), walk to its front (`act-command` $39, $44:
`seat-approach`), sit (`sit-diner-seat`, $3B, $46: seated motion $77) and leave it ($48, $49) - messages
`seat-take`, `seat-sit`, `seat-leave`. A jump in time frees them all.

## To come

Talking to them: built in part (`docs/subsystems/talk.md`). Riding is built (`docs/subsystems/traffic.md`). The streamed models' variants, a door opening as they pass (the doors).
