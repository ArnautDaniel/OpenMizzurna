# The clock

## Purpose

The game's date and time: Mizzurna Falls runs from 12/25 to 12/31, and its hours drive the story (events at
set times), the townsfolk's schedules, the light, the weather and the HUD's clock. The clock actor holds the
time, runs it while play goes on, and tells the others when a minute passes or the time is set.

The original (`clock.c`: `clock_tick`, `clock_set`, `clock_advance_minute`; `field.c`) added 0.8 of a game
second every frame (`0x30 / 0x3C`) while the clock ran - and its frame rate wasn't fixed: indoors it waited for
4 vertical blanks (15 frames a second), in the town for none (as fast as the town drew: ~30). Here the clock
runs in real time at those two paces (the user's choice: faithful), as named constants:

| where | original | here |
|---|---|---|
| indoors | 0.8 a frame at 15 fps | 12 game seconds a second: a minute every 5 s, an hour every 5 min |
| the town | 0.8 a frame at ~30 fps | 24 game seconds a second: a minute every 2.5 s |

## Time

A **time** is a whole number of game minutes since 12/25 00:00 (the design's "clock minutes"), so times
compare and subtract as numbers. `clock.fs`'s words make and take them apart:

```forth
25 9 0 date>time ( -- t )        \ 12/25 09:00 (the day as the date: 25 .. 31)
t time>date ( -- day hour minute )
t time>day  t time>hour  t time>minute
28 3 0 date>time  t >   \ before 12/28 03:00 (src: before_dec28_0300)
```

`new-game-time` is 12/25 09:00 (src: boot.c). The seconds within a minute are the clock's own (the
original kept them, the HUD never showed them; a save keeps only day, hour and minute).

## API (`scripts/messages.fs`)

To the clock:

- `set-time ( t -- )` - jump to `t` (src: `clock_set`; the story, loading a save). It broadcasts `time-set`.
- `advance-minute ( -- )` - one minute on, as a jump (src: `clock_advance_minute`: the story's waits).
- `hold-clock ( reason -- )` / `release-clock ( reason -- )` - the clock runs only while nothing holds it.
  Reasons (`enum: clock-hold`): `story-stopped` (the scripts' own stop and go: `clock_pause` /
  `clock_resume` from C), `event-running` (an event script has the game: `event_locked`), `no-control` (the player
  can't move Matthew: `player_has_control`), `menu-open` (a screen over the game: `ui_busy`), `text-shown` (an
  event's line on screen: the VM's `clock_pause` while a message is up, `clock_resume` as it closes). The original
  checked these every frame (the run flag shared by the scripts' and the text's stops); here each subsystem says when its own starts and ends.
- `in-town ( flag -- )` - the pace: the town's or indoors' (the areas subsystem, entering an area).
- `what-time ( -- )` - the clock answers the sender with `the-time ( t -- )`.

From the clock (broadcast to subscribers):

- `minute-passed ( t -- )` - a minute ran by (not for a jump). The weather re-rolls on it (src:
  `func_800252A4`), the HUD redraws, the story and the townsfolk check their times.
- `time-set ( t -- )` - the time jumped (the light jumps to it rather than stepping: src: `clock_set`).

## Rules

1. The clock starts held by `story-stopped` (src: boot.c clears the run flag) until the story lets it go.
2. While running, it adds `pace x dt` game seconds a step; each 60 make a minute, broadcast with the new time.
   Several minutes in one step (a long step) are each broadcast, in order.
3. A jump (`set-time`, `advance-minute`) keeps the seconds at 0 and sends only `time-set`.
4. The clock doesn't stop at the end of 12/31: the story ends the game first.

Not the clock's (they listen to it): the weather's re-roll every 90-217 minutes, snow on the morning of 12/25
and a clear last evening (src: `clock_tick`, `func_800252A4`) - `docs/subsystems/weather.md`; the light by the
hour and the town's street lamps (src: `func_800234B4`, `func_800243F0`) - weather / render; the HUD's clock
(src: `func_800157B4`).

## Status

**Built** (`scripts/clock.fs`): the time words, the actor, its messages. Checked by
`tests/clock/test_clock.fs`. Nothing holds or releases it yet but the story's stop (the subsystems that do are
to come).
