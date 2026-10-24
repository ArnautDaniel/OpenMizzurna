# Shoot-out

Shooting at button targets on a foe's body (src: `qte.c` in the executable, `shootout_update` in the overlays of
area 5 and area 26; `decomp/docs/research/minigames/README.md` §8). `scripts/shootout.fs`, the actor `shootout`.

## Messages and facts

- `shootout-start ( plan )`: a shoot-out by an area's plan. The plan is a `shootout-plan` structure followed by
  its rounds (`shootout-round`; `plan-bytes`, `round!`). It holds:
  - the rounds' table;
  - where Matthew and the first foe stand;
  - the stances and the shot motions;
  - the foes' full health (two pools);
  - what to do when it's won.
  
  Each round holds whose health it is, a hit's damage, a miss's damage, and its three scenes (missed, won, lost).
- Fact: `shooting?`.
- Area 5: its script calls `shootout-begin` (native 0x8010222C), then waits while `shootout-on` (0x8010224C)
  answers 1.
- Area 26: `shootout-begin` (0x801040D0) starts it in the cabin. When it's won: the hunt is over, the bear is
  down, and the hunters go to the cabin (src: func_801043AC).

## Rules

- **The tables.** Each area's table comes from the area's code file (the importer's area tables:
  `shootout-5`, four rounds; `shootout-26`, three). A round is:
  - 17 rows of 13 bytes: the target's slot, the frames its targets are up − 1, the frames until the next row,
    the shots, an end mark, then four of (body part, button: 1 △, 2 □, 3 ✕, 4 ○);
  - 17 camera steps: the slot to look at, the heights, the distance, the turn, the frames, how it moves to the
    next (cut, tween, turning up or down).
- **Timing.** A step is a frame of the original's (15 a second indoors).
- **The rows.**
  - A row loads when the last one's frames are spent. The list loops.
  - While the row's window is open, its targets are drawn at their parts' places on the screen (the engine's
    `part-on-screen`).
  - A press of △ □ ✕ ○ (one a step, in that priority) spends a shot. The matching button clears its target, and
    a cleared target bursts for 4 frames.
- **When the window closes.**
  - Every target cleared is a hit: the foe loses the round's damage and plays its hit motion (if it has one).
    Ten hits win the round.
  - Any target left is a miss: the round's scene plays (the foe shoots back). After it, Matthew loses the
    round's damage and the round goes on from the next row.
- **The camera** follows the round's steps around the foe, and the foe faces it. Matthew isn't drawn while
  aiming.
- **Winning.** A round won: its scene, then the next round's first row. The last round won: Matthew is shown,
  the round's scene plays, then it's over (and the plan's won word runs).
- **Losing.** Out of health (100): the round's lost scene, then game over. ○ or Start starts again from the
  first round with full health.
- **Drawn:**
  - the targets (for now, squares in the button's colour);
  - the shots left as bullets at (−140, 40);
  - Matthew's bar at (−140, 80), out of 100, his name above;
  - the foe's bar at (18, −80), out of its pool's full health, its name above (the plan's: area 5 the dog and
    the gunman, area 26 the bear).

  The bars and the game over are `screen.fs`'s `health-bar` and `draw-game-over`, shared with the fight.

## Status

Built (`tests/library/test_shootout.fs`):
- the rows, the hits and misses, the rounds;
- the camera path;
- losing, game over and retrying;
- both areas' plans.

To come:
- the original's button and burst icons;
- the pad's rumble (the shots' and hits' sounds, the retry's music: sound.md);
- the muzzle flash's lights;
- area 26's own gun control (L2 to raise the gun, walking with it);
- the gun prop hidden while aiming.
