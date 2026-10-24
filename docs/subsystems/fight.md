# Fight

A fistfight between two figures (src: fight.c `fight_start` / `fight_update`, the SUGI code in the executable;
`decomp/docs/research/minigames/README.md` §2). `scripts/fight.fs`, the actor `fight`.

## Messages and facts

- `fight-start ( a style b style )`: a and b (Matthew's slot 0, or a person's) fight; a style is 0 (the pad), 1
  (the second pad: rumble.md's port 1) or 2 - 8 (the AI, by its row of the executable's `fight-ai`).
- Facts: `fighting?`, `fight-winner` (0 the first, 1 the second).
- The events' op `fight ( a style b style )` (op $169) starts one and waits for its end; its result is the
  winner.

## Rules

- The executable's tables (imported into the library: `fight-moves`, `fight-attacks`, `fight-ai`, the steps,
  the sounds): 30 moves {motion, kind, the move after}; kind 2 is ready (it takes input), 1 hit or down (not to
  be hit), 0 acting. 13 attacks {the frame it lands on, reach, damage, the victim's move, push}.
- A step of the fight is a frame of the original's: 15 a second indoors, 30 in the town (as the figures'
  motions). They face each other; the camera stands between them, across, further back the further apart
  (dist 2·d + 0x584, eye −0x3CA, look −0x1A2).
- The pad (a ready fighter, style 0): ○ punches (the hook after two hits, forward + ○, back + ○), ✕ kicks
  (back + ✕), Down dodges, forward / back step (0x20 / 0x1C), a double tap dashes (0x58 a frame), Select taunts.
  Follow-ups: a jab into a kick, a kick into the hook, a dash into a lunge or a punch.
- The AI (a ready fighter, style 2 - 8): two moods with their chances of dodging an attack about to land,
  jabbing (in 324), kicking (in 480), its special (in its reach), else keeping its distance; a beating (32
  damage in a row) makes it defend, far apart it dashes in, winning it presses on.
- An attack on its frame, in reach, the other not kind 1: damage, the other's reaction move and push back; 120
  health; at 0 a knock-out, and when the fallen one is down for good the other wins. A move's motion ending
  gives the move after it. Neither may come within 0xE0 of the other nor into a wall or a solid billboard.
- Style 7 has its own motions for five of the moves' ($74, $8E, $8F, $6F, $70 → $536, $53A, $53B, $538, $539).
- Won by the pad: over, the second put 0x100 from the first on the line between them, the two facing each
  other (the camera the scene's again). Won by style 8: over. Won by the AI: game over ("GAME OVER"), ○ or Start fades out and the fight begins
  again from where they stood, full health (style 8 never wins).
- Drawn: two health bars (screen.fs's `health-bar`), the original's places (−140, −80) and (18, −80), 120 pixels
  for 120, each with its fighter's name above (the executable's `fight-icons` by style, of
  `ui/fight-names.png`; none when two play).

## Status

Built: the moves, the pad, the AI, the hits, the knock-out, the game over and retry, the camera, the bars and
their names, style 7's motions, the two put apart after a win, the walls and billboards
(`tests/library/test_fight.fs`). Its sounds - by the area and the move: a hit's and its voice, a miss, a fall;
its music, off on a loss - are sound.md's; its rumble rumble.md's.
