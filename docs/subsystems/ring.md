# The item ring

The screen of Matthew's items (src: `ring.c`, `cui.c` `ui_update`, `fhbook.c` `item_use`), and showing an item to
someone (src: the handlers' commands $2E / $2F, `cevcon.c` func_80042D0C / func_80042D78, `items.c` `item_react`).
`scripts/ring.fs` (the actor `ring`), `scripts/showing.fs`, `scripts/ui.fs` (the actor `ui`).

## The screens over the field (ui.fs)

While a screen is up the world stands still, as the original's `ui_busy`. The clock is held (`menu-open`).
Matthew, the townsfolk, the cars, the walkers, the camera and the driving don't move, and look/talk leave the
pad alone. The events go on. A screen sends `ui-lock` when it opens and `ui-unlock` when it closes. Anyone reads
the fact `ui-busy?`.

## The ring

- **Opening.** △, when all of these hold:
  - Matthew is the player's, and no screen or message is up;
  - the prologue is over;
  - not in area 5, nor during the forest's hunt;
  - on foot with no other button held, or in the car nearly still.

  The XA stream stops, he stands, and the ring opens on the first tool.
- **The items.** The executable's table (`ring-items`) lists them in the ring's order: the item, its picture
  (`items/<k>.png`, cut from CD.BIN 034_4's cards), its name in 063_3 (the description is the next message), and
  flags. The held ones make two lists:
  - the tools: the phone, the notebook, the map, the lighter, the diary, and the waist bag;
  - the key items (flags 1 or 4).
- **Drawing.** It is drawn as the original's 3D, in 2D: the cards stand on a ring of radius 400, centred at
  z 400. The view is from (0, −500, −1000) toward the origin, at a projection of 400; the one in front is nearest.
  Over the scene:
  - a half-dark backdrop;
  - the name bar (y 38..56; to 89 while a description is up) with the name centred;
  - the title, TOOL or ITEM;
  - the hint ✕ EXIT.

  The pieces are `ui/*.png`, cut from the UI's textures.
- **Turning.** Left and right, held, read once a game frame. A turn takes 4 game frames. The name shows again
  when it ends.
- **○:**
  - on the waist bag: the key items, from their first (or "It's empty.");
  - on a key item: its description in the message window. The photo of the sign, the note from Mel's room and
    Kathy's note are used at once instead;
  - on a tool: chosen.
- **✕:** from the key items, back to the tools; from the tools, closed.
- **□:** in the key items, when the person he faces (`in-front`) would answer to the item, it is shown to them.
  The ring closes.
- **Closing.** `ring-chosen ( item shown-to )` is broadcast: the item, or −1; the person, or −1. Then:
  - an item shown goes to that person (`show-to`);
  - the diary plays its reading (main 0x8009AA14);
  - Kathy's note, Mel's note (its first use once the diary's 23rd is read) and the photo play their scripts;
  - the phone, the notebook and the map hear `ring-chosen` and open (phone.md, notebook.md, map.md).

## Showing an item (showing.fs)

- `answer-to ( slot item -- answer )` and `show-to ( slot answer -- )`. The events' commands $2E / $2F are
  these.
- **A character's own answer.** They answer 0 while busy (between stops, riding, along with Matthew). Otherwise
  the item + 1 when their bit for it is set in `item-reactions`, else 0.
- **Shown.**
  - Their session word (`D_800F68B0`, by character) marks it seen.
  - Their state is kept.
  - Their reaction plays: pack `reaction-packs`, script (answer − 6) × 2, + 1 once seen.
- **Their own ways.** A person's answer and showing can be their own (`item-answer!`, `item-shown!`). Those set
  here:
  - Morgan: the photo once (a meeting in two hours);
  - Samuel: the pills at the hospital (taken; the analysis in two hours), Kathy's note once he knows of it;
  - Mrs. Roland: the floppy disk once;
  - Winona: Emma's key or the locker key, when on her schedule and not along. The locker key is given, a ride
    to the school (minigames 6 to 9, by school hours), and Robert goes there;
  - James: the floppy disk at the hospital once (not in the forest);
  - the desk officer: the floppy disk at the station once;
  - the deputy: the badge (area 27's minigame 27).

  The areas' rules set the rest: area 1's person 180 and area 2's visitor 168 (the tape), area 9's counterman (the
  ring), area 27's photo clerk (the camera).

## Status

**Built:** the ring, the lists, turning, reading, showing (its act prompt over a person who would answer),
using, the phone, the notebook and the map it opens, and the world standing still.
