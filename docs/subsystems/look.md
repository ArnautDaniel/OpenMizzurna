# Looking and acting

## Purpose

What ○ (look, talk) and □ (act: take, operate) do where Matthew stands (src: `cecheck.c`, `cevcon.c`
look_update / act_update / look_execute / act_execute; the area overlays' rules). Each area has four tables
(the executable's D_800962E0 / D_800964B0, rows in the overlays): look zones, look checks, act checks, act
zones. A row names a zone, and for checks a heading range, and its target: an event script, a C function (a
native), or nothing - then the area's own rules decide (from the story's flags, the clock, items).

## Rows (`scripts/events/checks.fs`, converted once from the tables by `tools/checks2forth.py`; written in
`look.rows`' words)

```forth
1 look-checks row  7 zone  2048 facing  80 either-side  script events.ovl001:entry-80104FA0  end-row   \ 180 degrees
1 look-zones row  10 zone  ask-area  -1 hint  end-row
```

- **zone rows** match the floor's zone Matthew is in by its index, and him facing its centre (within 0x300 of
  a turn: src player_face_zone);
- **check rows** match the zone's id and his heading within ± the row's degrees (and the talk state flags,
  bits 13 / 14: in a conversation, or not);
- `n hint` - the row's extra: the text of the hint shown (to come).

## The rules

Pressing ○ with Matthew the player's, and no window up: the figure in front (to come: the townsfolk), else the
area's rules - `look-check` gives an action from the zone (the zone rows' answer) and check (the check rows'
answer), and `look-do` does it (his look motion first: 0x51); an area without rules, or action 0, runs the
check row's target, else the zone row's. □ the same with `act-check` / `act-do`. An area's rules are a
vocabulary `rules.areaNNN` (`scripts/rules/areaNNN.fs`): its four words and the natives its events call,
registered with `area-rules!` / `native:`.

Facts it keeps for the rules: `zone-answer` / `check-answer` (what the tables gave), `zone-match` / `check-match`
(their rows to run: `run-rows`). The areas' hooks (`area-hooks!`: entering, a scene) run on `area-entered` /
`scene-shown`; an area's frame hook (`area-frame!`, src: the overlay's per-frame hook) runs each step while
Matthew is the player's and no event runs, before ○ and □ (it may start one: then they wait; or `take-pad`:
the pad is its own this step - a stool's keys).

## Status

**Built**: the 242 rows (`scripts/events/checks.fs`), the subsystem (`scripts/look.fs`), area 1's rules
(`scripts/rules/area001.fs`: its looks, its acts - the bed, the shower, the books, the key, the lighter, the
letter, Barbara's cigarettes - its hooks: Morgan's morning visit, Dr. James's; the C its events call). Seen: □
by the bed - "I can't sleep now..."; the events' lines that wait now wait at once (src: they slept a frame).
Area 0's (`scripts/rules/area000.fs`, the police station; `tests/library/test_area000.fs`): the map board, the
cabinet, the poster, the cells' confrontations, the computer's knock; arriving (Father Barton, "shove it", the
wolf); each frame - behind the front desk he is put back where he stood (`matthew-was`) and told off, the
stand-in at the desk while Hudson is away, the scene upstairs on the 28th, closing at 22:00; who says what by
their stops; the desk officer's scenes in turn; the C its events call (the cell visit, Isabella held).
Area 21's (`scripts/rules/area021.fs`, Haines Diner; `tests/library/test_area021.fs`): its look and act are the
rows' alone; arriving (meeting Lorraine on the 25th, Mel and Lorraine arguing), today's special, closing (20:00,
from the 28th 19:00), the robbery on the 27th, who says what by their stops, the farm workers, Lorraine's twelve
conversations in turn, then her line. Seen: the meeting on the 25th plays through, Mel walking off after it. Sitting at the counter and ordering:
`docs/subsystems/counter.md`.
Area 5 (`scripts/rules/area005.fs`; `tests/library/test_area005.fs`): one chain of scripts from the warp in to the
warp out; its fights, shoot-out and flare are the minigames' (to come) - the scripts carry on past them.
Area 4 (James's house) has no rules: the visit is one script, reached by knocking at his door
(`docs/subsystems/doors.md`).
Area 3's (`scripts/rules/area003.fs`, the Roland residence; `tests/library/test_area003.fs`): heading home at
20:55, the break-in scene on the 26th, the stairs only with Mrs. Roland along ("Let's go to Emma's room": she
follows him), the vase and Emma's key, the drain and the ring, Emma's desk and the missing diary, the piano and
the floppy; who says what; Steve's and Sarah's talks here.
Area 2's, with 29's and 30's (`scripts/rules/area002.fs`, Gruber Hospital; `tests/library/test_area002.fs`): kept
inside before the prologue ends (the way out puts him back, the off-limits door says so), after it the double
doors swing open and the way out leads to the town (with Morgan, or the attack's aftermath, waiting outside);
closing at 21:55 (upstairs 19:55) by room; the attack on Isabella and its aftermath; Barbara's introduction and
invitation, her door and her cigarettes; the Epoch's analysis; Cohen's room, his lodge key and the envelope;
Isabella's room; the wounded guard; who says what; the hospital talks of Samuel, the nurse, Barbara, James,
Lidds, Cohen and Morgan; the C its events call. To come: the bulletin board's recap, the evening windows.
Area 12's (`scripts/rules/area012.fs`, Club Wolves; `tests/library/test_area012.fs`): who's there by the hour
(the bouncer 21:58 - 23:25, the fan 21:28 - 23:55, Loretta after her call), the nightly timetable (the bouncer on
and off, the fan in and out), Isabella's show at 22:30 (or her cancelling it) and its end at 23:00, James's first
meeting, Mel's confrontation, closing at 01:58; the bar's stools and ordering from Wolf; who says what; the
conversations of Wolf, James, Mel, the bouncer, the fan and Loretta here. To come: the two fights, the prompt.
Area 18's (`scripts/rules/area018.fs`, Ringwald's Gas Station; `tests/library/test_area018.fs`): ○ at the counter
talks to Robert while he's behind it, his and Steve's lines by their stops, 22:00 - 22:59 the attendant sends
Matthew out to the town (not from 23:00: Robert's "open 24 hours").
**To come**: talking to the townsfolk (○ on a figure), the hints (prompts), the town's per-tile rows, the
other areas' rules; in area 0 the computer's case files and minigames 29, 30 and 36.

## The town

The town's look and act checks are a tile's (src: tile_checks: ovl027 D_8010AB44, D_8010AED4): `checks.fs`'s
`col row look-checks town-row ...` rows, matched against the tile Matthew stands on and its zones (floors.fs); it
has no zone tables. Its rows mostly run the town's C: the buildings' doors (docs/subsystems/doors.md).
