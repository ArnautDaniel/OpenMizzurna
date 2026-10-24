# The counter

Sitting at a bar's counter: the diner's (area 21), Club Wolves' (area 12) and the lake hut's (area 9) - the overlays' copies of
the same code (src: ovl021 `func_801022B8` .. `func_801037A8`; the phase-1 port reads them as one,
`port/src/game/counter.c` with a `CounterDef` each). `scripts/counter.fs`: a structure of a bar's settings
(`bar-counter`) and the steps; an area's frame hook calls `counter-step ( settings -- )`.

## Rules

- **Standing**: at a free stool (`seat-taken?`), within its reach along the counter (0xBE either side), standing
  across where one may sit, facing the counter (or turned toward it from the side he came), □ sits down.
- **Sitting down**: the stool is his (`seat-take`), the pad is the stool's (`figure-on-foot`: Matthew's own
  walking stops; `take-pad`: ○ and □ no longer look or act), a fixed camera beside the stool, the sitting motion
  from whichever side he came (its move held), then seated (motion $77).
- **Seated** (the pad's field mode 1: → / ← held, ○ pressed, □ held):
  - → (not on the last stool) / ← lean toward the next stool; leaning, ○ talks to who sits there, □ gets up,
    letting go sits back.
  - With the barman at his stool, ○ talks to them, □ orders (the bar's own order, a step a frame: `order-xt`).
  - Otherwise □ waves them over: they walk to his stool (40 a frame) and turn to him.
- **Getting up**: the motion for the side he leaned, the stool freed, the pad and the camera his again.
- `seat-now ( settings seat -- )`: on a stool at once (a scene leaves him there); `counter-off`: off it at once
  (the robbery).

The diner's order (`rules/area021.fs`): Lorraine asks ("Welcome! What can I get you?", later "How about
another?"), the menu (`docs/subsystems/list-menu.md`: the six and today's special), her line for it, she brings
it (the food or the drink: the order's prop), ○ talks to her, □ eats or drinks, she clears it away. No money
or time passes (the overlay's own).

Club Wolves' (`rules/area012.fs`): the stools along x, the approach mirrored (`flip?`), leaning right looks at
the stool below (`right-step` -1); Wolf asks ("What? You wanna order?", once he's had a drink "You here to drink
again?"), the menu (chips, coffee, milk, beer), each item's first line and its "again" line, no beer for a kid,
cancelling ends it ("If you ain't drinkin', buzz off."). A warp takes him off his stool (the area he sat in).

The lake hut's (`rules/area009.fs`): four stools along z, the counterman, a list of two drinks.

## Status

The diner's, Club Wolves' and the lake hut's built, with: sitting down's sound (each bar's: $70, $6D, $6E); the
bar's own as he sits (`on-sit`: the club's food and drink, the hut's drink to the counter); leaning, held, the
HUD's prompts - □ ($B) gets up, ○ (7) talks to the next stool's when someone's there (`counter-act-prompt`,
`counter-look-prompt`).
