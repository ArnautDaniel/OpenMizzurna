# Pause

Start (src: `pad_map.c` pad_update's Start, `cui.c` ui_update, `field.c`, `cevcon.c` event_threads_update).
`scripts/pause.fs`, the actor `pause`.

- **After the prologue**, Start pauses the game, and pressing it again goes on. It can't pause while another
  screen is up. While paused:
  - the world is held (`ui-lock`);
  - the events' tasks stand still, their waits not counting (`forth:tasks-paused!`, the scheduler's own);
  - "PAUSE" is drawn in the middle of the screen (56 × 16 from the clock's sheet, src: func_800168D0).
- **Before the prologue's end**, with no event running, Start skips the rest of the prologue: the main
  executable's script $80096598 fades out and ends it (evfn_80047620).
- Others read the fact `paused?`.

## Status

**Built.**
