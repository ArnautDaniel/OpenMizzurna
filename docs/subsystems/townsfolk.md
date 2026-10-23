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
`variant` another model; `route` the event they walk on leaving (an address until the routes are converted).
The row words live apart, in `townsfolk.rows` (`scripts/townsfolk/rows.fs`): `until`, `stop`, `place` would
hide the language's own elsewhere.

## Now

The clock places them: a person stands at the first stop whose time hasn't come (`stop-for`); a jump in time
(`time-set`) puts each at the stop for it. Those standing in the area being played are drawn (their kind's
model - `kind-models`, a slot is its kind - in their own textures) in the stop's motion. Facts: `stop-of` (-1
while walking a route), `person-now`, `here?`, `walking?`, `person-anim-done?`, `nozz-behind-counter?` (src:
func_80026C50).

The minute's rules, kept only while Matthew is the player's (src: npc_clock_tick; in a scene time stands for
them):
- standing: at the exact minute of their stop's `until` they **depart** (src: sched_depart): they are
  travelling, and `person-departs ( slot route )` is broadcast;
- travelling: at the next row's minute, still on the way, they are put at that stop at once (src: sched_skip).

## Routes (`scripts/routes.fs`)

A stop's route is event bytecode the person walks themself (`main.fs`: `route-XXXXXXXX`, 422 converted with the
events, registered by address: `main-word`). The routes actor answers `person-departs` by starting the route on
a thread of its own (threads 8-71: unlike an event's 0-7 it doesn't take the player's control), its own actor
`this-one`; `person-stopped` (put at a stop) ends it. A route ends in `arrive` (cmd 0x1E): the next stop, the
person placed there (src: actor_arrive).

The events' figure ops work for a person (`events.fs`: `person?`, sent to the townsfolk as `person-*`
messages): `set-pos`, `set-rot`, `set-floor`, `walk-to` (30 a frame, motion 1), `run-to` (80, motion 5),
`idle`, `turn-to`, `anim`, `anim-then`, `anim-hold`, `wait-anim`, `wait-stop`, `arrive`, `goto-stop`, and the
doors: `through-door` / `through-door-back` put them out of a door (`docs/subsystems/doors.md`) in the
stepping-out motion ($3F / $40) and the thread waits for it.

## To come

Talking to them: built in part (`docs/subsystems/talk.md`). Riding (`attach`, `wait-follow`, the actor
commands 0x41 / 0x4B / 0x42: walking to a partner's car and getting in - until then a route that rides waits
there, and the next row's minute puts them at their stop: Nozz on 12/25 stands in the street 12:13 to 13:42), seats and partners (the diner, Club Wolves, riding along), turning in place (`turn-by`,
`wait-turned`), `walk-forward`, the streamed models' variants, a door opening as they pass (the doors).
