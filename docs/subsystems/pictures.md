# Pictures

The pictures shown with the text (src: `ccdtim.c`). `scripts/pictures.fs`, the actor `pictures`.

- **The small picture**, beside the message box at (−136, 40), 48 × 48: an item got, the phone on a call.
  The 28 are cut from the UI's textures into `pictures/<n>.png` (src: the rectangles D_80095BD4 and the
  palettes D_80095C44).
- **The big picture**, above the box at (−90, −80), 180 × 120: a photograph in a scene. It is two CD.BIN
  pictures side by side, each 90 × 120 (`cdbin/NNN_K.png`).
- **Messages:** `show-picture ( n )`, `hide-picture`, `show-big-picture ( left right )` (0 0: none). Facts:
  `picture-up?`, `big-picture-up?`.
- **The events' ops:**
  - `picture-show` ($123) and `picture-hide` ($124);
  - `cd-picture` ($12B; only 0, none, is used);
  - `cd-pictures` ($179: the two halves; none when the second is 0).

  The VM drew the big picture only while the thread that showed it ran. When a thread ends, the small picture
  is hidden, and the big one too if that thread showed it.

## Status

**Built:** both pictures and the ops. The phone shows picture 0 while it dials.
