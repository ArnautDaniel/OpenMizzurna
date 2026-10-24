# The title

OPENING.EXE's program (src: `opening.c`): the logo, the attract movies, the title and its menu, the load menu,
and the endings' movies with their closing cards. `scripts/title.fs`, the actor `title`; the movies are the
engine's (`movie-open` .. : `src/game/movie.c`); the settings are `scripts/settings.fs`.

## The flow

It runs at OPENING's 60 frames a second, two a step.

1. **The logo** fades in and out over 90 frames.
2. **The prologue's movie** plays. Start or ○ skips it.
3. **The title.** The picture fades in, the name drops into place, and "PRESS START" blinks. After 1200 idle
   frames an attract movie plays: the digest, then the prologue's, in turn. Start or ○ raises the name to the
   menu.
4. **The menu.** Up and down choose; Start or ○ picks:
   - **New Game**: the opening's movie (skippable), then a new game (`new-game` to the story);
   - **Load**: the save file's slots (`saves.fs`). It shows "checking the memory card" a moment, then the
     slots used (up and down among them, ○ continues that one: `continue-game`), or "there is no data". ✕ goes
     back;
   - **Adjust Screen**: the panel. The d-pad moves the offset within ±15, and ○ keeps it with the settings
     (the PC's window shows the picture whole: kept, not applied);
   - **Vibration** on or off: left or right on its line toggles it. It is kept with the settings; the pads'
     rumble reads `vibration?`.
5. **The game.** The title is gone (`title-up?` false).

The menu's sounds are the title's own bank (CD.BIN 058_4 with 058_3's samples) and set (059_0): 0 confirm,
1 cancel.

## The endings and the game's end

- The events' natives $80047BFC (the bad ending) and $80047C2C (the final ending) stop the stream and the
  music, then `reboot` with mode 1 or 2. The scripts' world starts afresh (Forth and the actors), as the
  original's sessions did, and `game.fs` starts the title in that mode.
- In those modes the ending's movie plays (not skippable), then its closing card fades in: "… TRY AGAIN" or
  "© HUMAN 1998". Start or ○ goes back to the logo (on the PlayStation, the reset button).
- The game over's panel (`screen.fs` `draw-game-over`) is the original's: the screen black, the banner,
  "CONTINUE" (CD.BIN 034_1).

## The pictures

All from CD.BIN 034_2, cut by the import into `title/*.png`: the logo, the title's picture (16-bit, two
pages), its name, "press start", the menu as each line is chosen, the vibration line's four, the adjust
frame, the card panel's messages, the slots, the closing cards.

The movies' table (src: OPENING's D_8012CFF0) gives each one's last frame, volume and height on the screen:
the prologue's, the opening's, the digest, and the two endings'.

## Status

**Built.** Headless runs (tests, screenshots) go straight to a new game unless they boot the title themselves.
