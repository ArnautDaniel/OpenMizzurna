# The HUD

What the field shows over the picture while Matthew is the player's (src: `hud.c`, `cui.c` func_800485AC,
`field.c` field_update). `scripts/hud.fs` (it only draws); the prompts' facts are look.fs's and ring.fs's.

- **The prompts.** Two frames at the top right: □'s at (20, −92) and ○'s at (60, −92), each 48 × 24
  (`ui/prompt-frames.png`).
  - Each shows the icon of what its button would do here (`ui/prompt-icons.png`: 16 × 16, three frames down,
    changing every 8 game frames; from id 16 the second set).
  - A frame with nothing to do is drawn half see-through.
  - They show whenever he has control (`has-control?`), the ring too; the notebook and the map hide them.
- **What each button would do** (look.fs's facts `look-prompt` and `act-prompt`; src: cecheck.c prompt_id):
  1. the thing before him: its answer's icon (`answer-prompts`, src D_80096128): talked to 7, examined 3,
     taken 6;
  2. else the zone row's hint, then the check row's (the tables' extra; -1 none);
  3. else the area's own check (its rules' look-check / act-check) through the area's prompt table
     (`prompts-NNN`, from its overlay: src D_80095C7C).

  While the ring is up it sets its own: ○ "read" (3) whenever there is an item, □ "show" ($13) when the
  person faced would answer.
- **The clock** (src: func_800157B4) at the top left, while it runs (`clock-running?`) and the field isn't
  held. It reads "Dec.", the date, [the weekday] (the 25th's is Mon), and below the hour (12-hour, no leading
  zero), a colon, the minute, and AM or PM, from `ui/clock.png`.
- **The corner map** (src: func_800152D8), in the town:
  - an 80 × 80 window of the map picture around him (or his car), kept 40 texels from its edges;
  - turned with his heading and seen at 400 / 300, half see-through;
  - clipped to the 64 × 64 square at (80, 30) (`clip-2d`);
  - his mark, a small red triangle, where he is off the window's centre.

## Status

**Built:** the prompts and their facts (the ring's and the boat's too), the events' prompt beside the message
box, the clock, the corner map. (The boat has no gauge; the game-over panel is screen.fs's `draw-game-over`.)
