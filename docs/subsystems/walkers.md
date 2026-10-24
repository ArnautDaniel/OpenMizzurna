# Walkers

Figures walked along the town's footpaths (src: walker.c `walker_start` / `walker_set_mode` / `walker_remove`,
carline.c `func_8003C08C`). `scripts/walkers.fs`, the actor `walkers`.

## Messages and facts

- `walker-start ( slot node dest speed motion )`: from that node toward the destination (one of the first 24
  nodes), at speed units a frame, in that motion. If the figure is already walking, it turns toward the new
  destination, back along its edge if that's the way.
- `walker-stop ( slot )`: it stands (motion 0).
- Fact: `walker-active? ( slot )`.
- Entering an area clears them.

## Rules

- **The graph** is the common container's `033_4/5/0` (raw in the library): 32 bytes a node.
  - x and z (s32), its height (s16);
  - for each of the 24 destinations, 2 bits giving which of its links leads there;
  - its four neighbours (s16, −1 none) and the lengths to them (u16).
- **Timing.** A step is a frame of the original's (30 a second in the town).
- **Walking.**
  - An edge takes length / speed steps; the figure is placed proportionally along it, facing along it.
  - At a node: the destination means it stands and is done. Otherwise it takes the next edge toward the
    destination, turning half way to the new edge's heading at once.
- **The stalker** (`rules/area027.fs`):
  - Minigame 23 is the warp to the town, after which the stalker is due.
  - Minigame 24 (a code minigame, `minigame-code!`) sends him round nodes 0 → 3 → 9 → 0, running (motion 5) at
    100 a frame, one leg after another, on his own route thread.
  - Talking to him in the town is minigame 25: the walk ends, +0x1C bit 28 toggles, and his scene
    (`entry-8010A178`) plays.
  - Leaving the town ends his loop.

## Status

Built: walking the graph, the stalker's loop and his capture (`tests/library/test_walkers.fs`). The walkers are
the scripts' (command $24, `lift`); the original has no other pedestrians, and its pause for them all
(`func_8003C588`) has no caller.
