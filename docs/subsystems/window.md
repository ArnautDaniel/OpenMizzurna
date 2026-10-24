# The message window

## Purpose

The box low on the screen where the game's lines are typed (src: `msgbox.c`, `ctexstr.c`; the events' text
ops: `cevent.c`, `event_talk`). A message is typed a character at a time; when the box is full and more lines
follow, an arrow waits for ○ and the next line scrolls in; a finished message closes with ○. Questions put
their answers under a cursor; captions (the chapter titles) close themselves. While a message waiting for ○
is up, the clock stands still.

## The original's numbers (`scripts/window.fs`, named)

| | original | here |
|---|---|---|
| the box | text at (-95, 40) on the 320 x 240 screen (its centre the origin), 240 x 48: three lines of 16 | the same, scaled to the window by a whole factor |
| the band behind it | dark, y 37 .. 89, the screen's width (`func_80015DCC`) | black at 56% |
| typing | 4 glyphs a frame at 15 frames a second | 60 characters a second |
| scrolling | 2 pixels a frame | 30 pixels a second, a line at a time, after ○ (a reading: the original's page-wait) |
| the arrow | at (129, 87), animated | the same: seven 8 x 8 frames, a frame each of the game's (`ui/page-arrow.png`) |
| a question | text at x -79, the cursor at -95 on the answer's row (`msg_choice`) | the same; the cursor its 16 x 16 sprite (`ui/cursor.png`) |
| the font | the dialogue font (033_4/5/3), a glyph its own width (the 2021 patch), lines 16 apart | the same, from the library (`font-text`) |
| colour | "raise": which of the font's palettes (3: a voice on the telephone) | `font-palette` |

## API (`scripts/messages.fs`)

- `window-say ( bank id speaker palette secs -- )` - show message `id` of `bank` (`text-of`: 0..32 an
  area's, 33 the common text, 200+ an event pack's); `secs` 0: it waits for ○ and holds the clock
  (`text-shown`); else it closes itself after that long.
- `window-ask ( bank id kind -- )` - a question (src `msg_choice`: kind 1 / 2 that many answers more and no
  question in the box; else a question line and two answers below it).
- `window-shut ( -- )` - close now.
- `window-closed ( answer -- )` - broadcast as it closes (the answer, -1 none).
- Facts: `window-up?`, `window-answer`.

The events' text ops (`events.fs`: `say`, `say-by`, `talk`, `caption-for`, `phone-say`, `choice`, `wait-text`
...) send these; an event that showed a line waits for it to close before its next wait (`settle`: src the
thread's `wait_msg`, checked before its other waits). Text ids: < 10000 the area's, < 15000 the common text,
else the loaded pack's.

The window is drawn over everything: as a message opens it moves to the end of the drawing (over a screen opened
since, such as the item ring's).

## Status

**Built**: the window, its drawing, the events' text ops. Checked by `tests/window/test_window.fs` (11) and the
story's tests (a player answering each line); seen on screen (the opening's "Prologue" caption and its first
lines). The speaker's portrait is drawn beside the box at (−156, 16), 64 × 80. `window-say`'s portrait is a kind: the
events give the speaker's (`portrait-of`: an actor's kind, or from $FD a kind itself). It shows when the area's
sheet has that kind (`portrait-tables`, src: func_80014D18; the pictures are `portraits/<kind>.png`, cut by the
import from the sheets). The arrow's and the cursor's sprites; the talk gestures (events.fs). The list font's
windows are the list menus' (list-menu.md).
