# Looking and acting

## Purpose

What ○ (look, talk) and □ (act: take, operate) do where Matthew stands (src: `cecheck.c`, `cevcon.c`
look_update / act_update / look_execute / act_execute; the area overlays' rules). Each area has four tables
(the executable's D_800962E0 / D_800964B0, rows in the overlays): look zones, look checks, act checks, act
zones. A row names a zone, and for checks a heading range, and its target: an event script, a C function (a
native), or nothing - then the area's own rules decide (from the story's flags, the clock, items).

## Rows (`scripts/events/checks.fs`, converted once from the tables by `tools/checks2forth.py`)

```forth
1 look-checks row  7 zone  2048 facing  80 either-side  script events.ovl001:entry-80104FA0  end-row   \ 180 degrees
1 look-zones row  10 zone  ask-area  -1 prompt  end-row
```

- **zone rows** match the floor's zone Matthew is in by its index, and him facing its centre (within 0x300 of
  a turn: src player_face_zone);
- **check rows** match the zone's id and his heading within ± the row's degrees (and the talk state flags,
  bits 13 / 14: in a conversation, or not);
- `n prompt` - the row's extra: the text of the hint shown (to come).

## The rules

Pressing ○ with Matthew the player's, and no window up: the figure in front (to come: the townsfolk), else the
area's rules - `look-check` gives an action from the zone (the zone rows' answer) and check (the check rows'
answer), and `look-do` does it (his look motion first: 0x51); an area without rules, or action 0, runs the
check row's target, else the zone row's. □ the same with `act-check` / `act-do`. An area's rules are a
vocabulary `rules.areaNNN` (`scripts/rules/areaNNN.fs`): its four words and the natives its events call,
registered with `area-rules!` / `native:`.

Facts it keeps for the rules: `zone-answer` / `check-answer` (what the tables gave), `zone-match` / `check-match`
(their rows to run: `run-rows`). The areas' hooks (`area-hooks!`: entering, a scene) run on `area-entered` /
`scene-shown`.

## Status

**Built**: the 242 rows (`scripts/events/checks.fs`), the subsystem (`scripts/look.fs`), area 1's rules
(`scripts/rules/area001.fs`: its looks, its acts - the bed, the shower, the books, the key, the lighter, the
letter, Barbara's cigarettes - its hooks: Morgan's morning visit, Dr. James's; the C its events call). Seen: □
by the bed - "I can't sleep now..."; the events' lines that wait now wait at once (src: they slept a frame).
**To come**: talking to the townsfolk (○ on a figure), the hints (prompts), the town's per-tile rows, the
other areas' rules.
