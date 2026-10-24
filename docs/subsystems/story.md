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

A line (`talk`, `talk-to`: src event_talk) holds its thread until it closes - the gesture plays with it, the ops
after it wait (src: wait_msg, checked before every op); the other says wait only with their "-wait" forms or
`wait-text`. The area's text and the overlay words an event reaches by address are those of the area being
played (its content area: `script-area`).

`launch-event ( xt -- i )` starts one and gives its thread (a show the area later cuts short: `kill-event`;
src: event_thread_kill). While an event that gave the player control back runs, the areas' frame hooks and ○ / □
run (src: player_has_control).

The converter also takes the scripts an area's minigames queue (src: `minigame_script = &D_X` anywhere in the
overlay, and the scripts it runs on threads of its own) and the arrival scripts the overlays' C warps with (src: func_80019F3C's last
argument), in the destination area's overlay.

The minigames that are a script alone (src: area_minigame_start setting minigame_script) are registered by
number (`minigame-script!`), and the `minigame` op starts that script as an event. Those that are an area's
code alone are registered with `minigame-code!` (the stalker's 24 and 25, the car chase's 21), and the op runs
that word at once; one that lasts says so (`minigame-begun` / `minigame-over`) and `wait-minigame` waits for it.
Any others are to come.
`wait-minigame` (op $149) waits for that script's end (`minigame-running?`). An area can register a word of its
own as its script (area 19's chase: its scene, then the main executable's minigame 23; area 8's dog chase). `scene-reset` (op $11A, src:
func_80018B44) changes the area under a running scene, Matthew where he is; `wait-near` (op $117) waits until
someone is within a distance of a point.

The minigame scripts include those an area picks from a table (src: `minigame_script = *(... + &D_T)`: the old
man's lessons): the converter reads the table's words that point into the overlay.
Those an area runs on a private thread each frame (src: `event_thread_start(&D_T, &D_X)` there: area 17's
companion leading the way) are taken too; the area starts them as a route (`start-route`: Matthew stays the
player's). The main executable's (src: minigame_main_start: Winona's ride to the school, 6 - 9, and 23) are
registered in `scripts/game.fs`.

A few scripts' own actor is given by the C that starts them (src: `M2C_FIELD(&D_X, s32 *, 4) = ...->id`: the
counters' orders - the food, the drink): the converter finds them and writes `the-prop @ as-this-one`; the rules
set `the-prop` before starting them.

## Status

**Built**: the conversion (102 files, 1942 words), the language's core (threads, time, flow, lookups), its
ops as stubs. Checked by `tests/story/test_events.fs` (frames, the goto chain, an event's life, the new-game
opening running to its end in its 19 seconds) and `tests/story/test_all_events.fs` (every file loads; the
lookups by number and address).

Also built: the flags and the story actor's new game and prologue end (`tests/story/test_story.fs`: 8).

The text ops go to the message window (`docs/subsystems/window.md`).

**Next**: the flags in the saves (with the saves subsystem); the areas' rules.

## The actor commands and the small ops (scripts/events.fs)

Built on the townsfolk's, Matthew's, the walkers' and traffic's messages:
- **Place.** `set-x` / `set-y` / `set-z` (one coordinate, sent as such so a script's following ops see it) and
  `move-by`.
- **Heading.** `face-point`, `face-each-other`, `camera-face` (the camera's heading from one figure to another).
- **Moving.** `run` (straight on, running); `lift` (along the town's footpaths); `walk-path` (to a point, with its
  height).
- **State.** `save-state` / `restore-state` (heading and motion kept); `freeze` (they stop where they are);
  `reset` (off the stool, out of the car, off the paths, at no door, their route over, standing);
  `door-release`.
- **Cars and endings.** `partner-command` (their car turns onto a route at its first node); `ending-schedule`
  (they leave on that route of the executable's).
- **Waits.** `wait-attached` (until they ride).
- **The map and the fire.** `map-reveal` (the map knows a place: story-flags.fs's `map-known`, saved with the
  story); `set-357c` (area 5's fire object).
- **Nothing to do here:** `map-sync` (the original waited for the town's tiles); `ignore-a` / `ignore-b`
  (Matthew walks through people for now).
- **Pictures and items.** `picture-show` / `picture-hide` and `cd-picture(s)` are the pictures'
  (`docs/subsystems/pictures.md`). An event's end hides the small picture, and the big one it showed.
  `item-query` / `item-show` are showing.fs's answers (`docs/subsystems/ring.md`).
- **Saving.** `memory-card` is the save dialog (`docs/subsystems/saves.md`).
- **The prompt** (op $129): □'s icon and ○'s beside the box (the HUD draws `prompt-left` / `prompt-right`);
  the event waits for one: ○ answers 1, □ 0.
- **The hospital's elevator doors** (op $148): in its content areas only, four of its animation list's
  entries (`elevator-open` / `elevator-close`, from ovl002).
- **A talk's gesture** (src: event_talk): a motion, then what follows (`gesture-motions`, src D_80012ED4).
  There is none while sitting, except the seated ones (9–12), and none in a resting pose (src: func_80018F2C),
  except 8. Barbara has her own for the plain ones.
- **`depart-alt`** (cmd $56): a labourer off along their stop's route.
- **The C the events call** (`native`): every one they reach is written now, each with its subsystem:
  the story's (the recap, the day's stops, the diary's pages), the characters' (`conversations.fs`), the
  phone's, the boat's, the car's, the saves', the fight's, the areas'.
- **Never reached** (no event uses them, so they stay notes): `talk-pose`, `anim-step`, `examine`, `act`,
  `kill-thread`, `go-to-actor`, `place-alt`, `door-reveal-room`, the sprite counters,
  `wait-attached-to`.
