# Saves

Saving and continuing (src: `cmcard.c` save_build / save_apply / memcard_dialog_update, `boot.c` game_start).
`scripts/saves.fs`; the engine's `save-file` / `load-file` (the player's own files, `<home>/saves/`).

## The file

`saves/mizzurna.sav` keeps the original memory card file's layout, 0x1980 bytes with three slots:

| at | what |
|---|---|
| $200 | 3 (the layout) |
| $201 + slot | 1: the slot is used |
| $204 + slot × $204 | the story block (story-flags.fs) |
| $810 + slot × $5B0 | each of the 182 actors: s32 its stop, u8 its kind (8 bytes each) |
| $1920 / $1923 / $1926 + slot | the day, the hour, the minute |
| $192A / $1930 / $1936 + slot × 2 | his car: x / 16, z / 16, its heading |
| $193C + slot | the fuel |
| $1940 / $194C / $1958 / $1964 + slot × 4 | the items, the map, the numbers, the people known (masks) |
| $1970 | the byte sum of all before it |

The card's own header (the title, the icon) is left out.

## Saving

The events' op `memory-card ( slot )` ($152; the save point in Matthew's room) runs the dialog in the event's
thread. The PC has one card, always there, so the dialog is:
1. "Checking the memory card" (the save overlay's scripts);
2. "A card is in slot 1: save there?" (`answered`, src: func_8004E5B8, takes the answer);
3. "Saving to slot 1".

The slot is then written. The result is 0 when saved, 1 when not.

## Continuing

`continue-game ( slot -- ok? )` (src: game_start(n), save_apply). It is false for a slot not saved. Otherwise:
- the events are reset;
- the story block, the time, the items (`items-set`: the three first, then by number), the map, the people
  and numbers (`know`), his car (`his-car-at`) and the fuel (`fuel-set`) come back;
- each scheduled person is put back at their stop, showing their kind (`person-restore`);
- the clock goes (no prologue hold);
- Matthew wakes by his bed in his house (area 1, floor 1; ovl001's `wake-by-the-bed`).

## Status

**Built:** the file, saving from the save point, continuing.

**To come:** the title's load menu, which will call `continue-game`.
