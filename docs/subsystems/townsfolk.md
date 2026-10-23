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

## Now

The clock places them: a person stands at the first stop whose time hasn't come (`stop-for`), re-read on every
`time-set` and `minute-passed`. Those standing in the area being played are drawn (their kind's model -
`kind-models`, a slot is its kind - in their own textures) in the stop's motion. Facts: `stop-of`,
`nozz-behind-counter?` (src: func_80026C50).

A reading, until the routes walk: leaving a stop, a person is at the next one at once (the original walked the
route and, if it hadn't finished by the next row's time, put them there: cmd 0x1A).

## To come

Talking to them (○ on a figure: their handlers' conversations by time and story), the routes (event bytecode
walked by the person themself), seats and partners (the diner, Club Wolves, riding along), the events placing
and moving them (`set-pos`, `walk-to` ... for every slot), the streamed models' variants.
