# The story

## Purpose

What has happened (the story flags), the events that play it out, and which run when. The original keeps the
flags in a 0x204-byte block saved whole (`story_save`; 118 bits named so far: `tools/flag_names.txt`,
`decomp/include/story.h`), runs its events as bytecode in a small VM (CEVENT.C: `port/src/game/cevent.c`), and
decides in each area overlay's C which event starts (on entering, on a zone, at a time, on examining).

This page covers the first part built: **the events**, converted to Forth. The flags, the chapters
(`func_80047780`) and the areas' decisions follow, with the areas subsystem.

## The events in Forth

The bytecode - 78 event packs (CD.BIN 042_3 .. 058_0), the scripts in 23 area overlays, 35 in the main
executable, ~20,400 instructions - was converted once by `tools/ev2forth.py` into `scripts/events/` (a file per
source: `pack-049-0.fs`, `ovl001.fs`, `main.fs`; `all.fs` loads them all). From then on the Forth is the
source, edited by hand: names for scripts and labels as they're understood, structure where it helps. The
tool is not run over edited files again.

```forth
\ $80103FE4
: new-game-opening
    -1 music
    257 screen-effect
    matthew 348180 0 287578 set-pos
    20 frames wait
    60 2 caption-for
    ...
    62 matthew say-by
    matthew 505 1 anim
    wait-text
```

The language is the `events` vocabulary (`scripts/events.fs`):

- **An event is a task.** `' word start-event` (src: `event_start` / CEvCon_Start) gives it one of 8 threads
  (the original: 5 and the main one); each has its own actor (`this-one`, set by `as-this-one`; the bytecode's
  0xFF) and result (`result=`, `result!`: what the last actor command, choice, prompt or fight gave). While
  any runs, Matthew isn't the player's (the clock's `no-control`) until `give-control` or the last one ends.
- **Time is real time.** Every event ran at 15 frames a second (CEvCon_Start sets `VSync(4)`, in the town
  too), so `n frames` is n / 15 seconds as steps, for `wait`; timed lines (`caption-for`) take seconds.
- **Control flow.** A script and every place a branch lands is a word. `goto word` is a tail jump (the
  `jump` primitive: a loop of gotos never grows the return stack), a `call` an ordinary call, `end-script`
  an exit - at the outermost, the event's end. A loop's target is a `defer` declared first.
- **Across sources.** A pack's scripts by number (`load-pack`, `goto-pack-script`, `call-pack-script`: the
  packs register theirs with `pack-script!`); the main executable's shared scripts reach into the area being
  played by address (`goto-overlay`: each overlay registers the words they reach with `overlay-word!`, by
  `event-area`); overlays call the main executable's directly (`events.main:sub-...`).
- **The cast** by name where known (`matthew`, `winona`, `morgan` ... the decomp's 18), else `160 actor`.
- **Text** by id (the library's text files). The conversion leaves the lines out of the comments (the 2021
  translation's text stays in the library); `ev2forth.py --text` puts them in, for reading.
- **The C an event calls** (`call_c`, 81 places) is `ovl001 $80102678 native`: a word registered with
  `native:` when that function is written in Forth; until then nothing happens.

Each op does its work through its subsystem's messages. An op whose subsystem isn't built yet is a stub: it
takes its arguments, counts itself in `unbuilt-ops` (`true event-trace !` prints each), and passes - a wait
ends at once, a choice answers the first line. Built now: the threads, waits, control flow, `give-control` /
`take-control` and `time-skip` (the clock).

Quirks kept as notes in the converted files: `if_eq_id` (one use) whose id word the original then ran as an
actor command; actor commands past the length table (2 uses: `act-command` with the number); one place in
pack 051_4 that doesn't decode (as `evscript.py` reports).

## The flags

`scripts/story-flags.fs` (vocabulary `story-flags`): the block (`story-block`, 0x204 bytes, zeroed by
`story-clear`), a bit by its number (`$010 2 story-bit`: offset x 8 + bit, as the original's little-endian
words lay out), `flag? flag-set flag-clear flag-toggle`, counters by offset (`story@ story! story+!`). 113
named so far (written from the decomp's `tools/flag_names.txt` by `tools/story_flags.py`; three shortened to
the Forth's 31 characters): `took-lighter flag?`, `prologue-done flag-set`. The flags are facts: anyone reads
them; whoever's part of the story it is sets them (an event's native, an area's rule).

## The story actor (`scripts/story.fs`)

- `new-game` (src: `game_start(0)`): the flags cleared, the events stopped, the clock to 12/25 09:00 and held
  (`story-stopped`, and `event-running`: src `event_lock`), the area 1 overlay's events, the opening started
  (`events.ovl001:new-game-opening`).
- The prologue's end (src: `evfn_80047620`, the main executable's native at $80047620, called by script
  $80096598): the clock let go and set to 12/25 10:00, `prologue-done` set, `prologue-over` broadcast. Its
  placing everyone at their first stop and Matthew and the car at the station waits for the townsfolk and
  Matthew.
- **Open**: the original starts $80096598 when Select is pressed with no event running
  (`event_threads_update`) - the only way the prologue's end is reached in the code found so far. A prologue
  skip, or a leftover? Not wired until it's settled.

## Status

**Built**: the conversion (102 files, 1942 words), the language's core (threads, time, flow, lookups), its
ops as stubs. Checked by `tests/story/test_events.fs` (frames, the goto chain, an event's life, the new-game
opening running to its end in its 19 seconds) and `tests/story/test_all_events.fs` (every file loads; the
lookups by number and address).

Also built: the flags and the story actor's new game and prologue end (`tests/story/test_story.fs`: 8).

**Next**: the window and text ops; the flags in the saves (with the saves subsystem); the areas' rules.
