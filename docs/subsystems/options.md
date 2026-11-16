# Options

The engine's own options (scripts/options.fs), not one of the original's menus: **F1** anywhere - the title, a
screen, the pause, a scene - opens them over the game.

- While they're up the world stands as under the pause (`ui-lock`, the tasks paused) and the game reads no keys:
  the menu captures them (`capture-keys!`; the engine's `key-down?` / `key-pressed?` give nothing to the game) and
  reads them itself (`menu-key-down?`, `menu-key-pressed?`). Closing gives the keys back, and the tasks unless
  the pause holds them.
- ↑ ↓ choose a row, ← → change it, F1, Esc or ✕ close.
- Rows:
  - **Volume** 0 .. 10: everything heard (the engine's `sound-volume`, 0 .. 127, after the mix: music, voices,
    effects).
  - **Vibration**: the pads' rumble (the same setting the title's menu changes).
  - **Fullscreen**: the window (F11 and Alt+Return still change it, without keeping it).
  - Any a mod adds: `add-option ( label len value-xt change-xt -- )`, value ( -- addr len ) what it shows,
    change ( dir -- ) for ← (-1) and → (1). The label is copied; 16 rows at most.
- Kept in the settings file beside the saves (settings.fs: a record whose fields' 0 is their default, so an
  older, shorter file reads the same) and applied as the game starts.

Facts: `options-up?`. Tests: `tests/options/test_options.fs`.
