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

Things before him: ○ on an actor that has an examining (`examined-by!`, src: the handler's CMD_EXAMINE when it
answers cmd 0x31 with 6) runs it, else talking; □ on one with a taking (`taken-by!`, CMD_ACT, cmd 0x32's 4) runs
it, else the place's act (talk.fs: `look-at-it`, `take-it`).

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
Area 26's (`scripts/rules/area026.fs`, Barrows Forest; `tests/library/test_area026.fs`): the deputy at the
entrance (at his post he pushes Matthew back; aside by the date and the story), his lines (his refusal
remembered the next visit); the morning hunt - his walk away on an even minute after a word with him, his
"Hey!" in the clearing and his chase, through to the hunters, following Cohen along his route at his pace (too
far, running, lingering: his words), his signs (○), each leg's deadline, the run back to the cabin with the bear
after him; the cabin door, the shell box (○ then □); closing time, the ways in and out (zones), the waterfall
meeting at its time; the last evening's way chosen on arriving and its places. (To come: the shoot-out minigame,
the movies the last scenes end in, the ambience, the water's animated texture; the events' lock is the area's
own for now.)
Area 28's (`scripts/rules/area028.fs`, the hideout cabin; `tests/library/test_area028.fs`): the closed door (□: a
first knock, then the later ones), the call its scene makes. (To come: the fight its scene runs.)
Area 24's (`scripts/rules/area024.fs`, the church; `tests/library/test_area024.fs`): the crate (its place by the
floor he's on), the caretaker, the scenes on arriving (the first that fits), a room's scene once, being shown
out before the 25th's meeting and at closing time, the meeting seen from above by its time, the doors by the
story (shut while he's locked in), a letter (○ looks, □ reads once its time's come), the stairs up from the
basement, who says what, a talk hook (a key), the rope (minigame 17: each new press of left or right a pull,
80 to be free). (To come: minigame 38's flare, the bell, the nave's two sets of things by a flag not yet
understood.)
Area 23's (`scripts/rules/area023.fs`, Cohen's house; `tests/library/test_area023.fs`): the locked door upstairs
(looked at, something found to force it, three tries), the room behind it (a scene, once), the envelope (□,
once), the front door by the story, who says what. (To come: its sounds, a texture's animation in scene 0.)
Area 19's (`scripts/rules/area019.fs`, the apartment building - 31 and 32 its ground floor and its air duct;
`tests/library/test_area019.fs`): the crate, the photo on the shelf (□, two scenes, then a scene ○ starts), the
doors (○: the keyhole when it's time, or in when open; □: knocking at two), the mailboxes, the duct cover's
lines, the duct itself (the crawl, minigame 41: the pad drives him), the way out of the far room (the chase,
minigame 22, out to the town), two rooms shown by the zones he walks into, a room's two sets of things by the
story, who says what, two talk hooks. (To come: the stalker in the town after the chase, minigame 24; the
events' lock.)
Area 17's (`scripts/rules/area017.fs`, the school's main building; `tests/library/test_area017.fs`): with a
companion along (off her schedule) she leads the way up on her own thread - Matthew stays the player's - and waits
at the locker; ○ there opens it (items taken and given), the desk after it, the way out not without her, closing
time by floor and where he is (once).
Area 16's (`scripts/rules/area016.fs`, the school library; `tests/library/test_area016.fs`): the crate, closing
time by room (once), a teacher by the hour, the key rack and the key (with a companion along), a poster, the doors'
scenes by the key and the hours, a book and the newspaper archive (once each), who says what, the building's halves
shown by where he is, three minigames that are scripts. (To come: the bios the archive waits on, two EXE functions
not understood yet.)
Area 13's (`scripts/rules/area013.fs`, the park; `tests/library/test_area013.fs`): the old man there by the hour,
talking to him (a character pack's scripts), his lessons (the one reached: a minigame that's a script), who says
what, a night scene once, the way down with or without a companion, the lights by floor and time of day. (The
lesson's fuel: with the car, to come.)
Area 11's (`scripts/rules/area011.fs`, the inn's room; `tests/library/test_area011.fs`): a scene the first time
in, the crate, a thing to take once found.
Area 10's (`scripts/rules/area010.fs`, the north-shore cabin; `tests/library/test_area010.fs`): the crate
(`docs/subsystems/crate.md`), the window from on top of it, a scene at the landing (two minigames that are
scripts).
Area 9's (`scripts/rules/area009.fs`, the lake; `tests/library/test_area009.fs`): the hut's coffee counter
(four stools, ordering from the counterman), the boat, a scene at the pier once, who says what.
Area 8's (`scripts/rules/area008.fs`, the junkyard's bus; `tests/library/test_area008.fs`): the seat looked at
twice then searched, getting off the bus, a figure there at its point in the story. Its two real minigames: to
come.
Area 7's (`scripts/rules/area007.fs`, the cemetery; `tests/library/test_area007.fs`): its two timed scenes (on
arriving or as they come), a visitor at a grave, walking out of the grounds, who says what, Father Barton's talk
here, the day's and the night's lights (`object-shown`, to the areas), the C its events call.
Area 6's (`scripts/rules/area006.fs`, Astin Farm; `tests/library/test_area006.fs`): the two things in the room to
take, the warning when its owner heads home, the closet and the way out, the closet's doors.
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
