# The list menu

A board of up to seven labels on the right of the screen, a cursor, a pick: the diner's order (area 21), Club
Wolves' (area 12), area 9's (src: `cui.c` `func_80047D70` opens it, `func_80047F30` steps it, `func_80048128`
answers; the form `D_800995D4`). `scripts/list-menu.fs`, the actor `list-menu`.

## Messages and facts

- `list-open ( id0 .. id6 -- )`: its labels, the first on top (0: a row left empty). The labels are the list
  bank's (text bank 34: CD.BIN 033_4 part 6, stored in the list font's own codes and drawn with font 1).
- `list-closed ( answer -- )`, broadcast when it closes: the row picked (0 ..), or -1 cancelled.
- Facts: `menu-up?`, `menu-answer` (-2 while open or unasked, -1 cancelled, else the row).

## Rules

- ↑ / ↓ move the cursor, wrapping; ○ or □ picks, ✕ cancels (src: the pad's list mode).
- The board (from the form): 144 × 168 at (0, −84) on the original's 320 × 240 screen (its centre the origin),
  a quad its four corners shaded (170 170 110, 100 125 15, 135 210 110, 18 105 52), half see-through; "MENU"
  (label 46) at (56, 4); the rows from (16, 24), 16 apart; a seventh row sits under "DAILY SPECIAL" (label 47),
  shown only when all seven are there.
- The diner's labels: 48 - 53 its six (burger, omelet, pancakes, chicken, coffee, milk), 54 - 60 the specials
  (grits, rice, fish, beans, lasagna, sandwich, steak: 12/25 - 12/31); Club Wolves' 61 - 64 (chips, coffee,
  milk, beer).

## Status

Built; the cursor is the UI's 16 x 16 sprite (`ui/cursor.png`), as the window's choices'.
