# Tail

Tailing the lookout to his door, and the door's keypad (src: ovl027 `func_801025E0`, states −2 to 10;
`lookout_act`; `decomp/docs/research/minigames/README.md` §3). `scripts/tail.fs`, the actor `tail`.

## Messages and facts

- `tail-pace`: the lookout (slot 106) paces by his door (src: minigame 1).
- `tail-start`: his scene, then the tail (src: minigame 2).
- Fact: `tail-started?` (he's past his second point).
- The town's rules (`rules/area027.fs`):
  - Entering the town with the tail armed (+0x00 bit 6) and the hideout not yet done (`hideout-robbers-caught`,
    +0x18 bit 5): he paces.
  - Talking to him: while Mel's stop is under 0x14 and the tail hasn't started, the tail; otherwise his warning
    (`entry-8010B470`).
- Leaving the town ends it.

## Rules

- **The town's tables** (the importer's area tables):
  - `tail-waypoints`: 12 points, each its x and z, what he does there (on, look back, look aside, his door),
    the next point, the next once he's seen, and his sight's zone.
  - `tail-zones`: 5 boxes, two corners each.
  - `keypad-code`: a button's bit for each of the four places (○ ○ △ △).
- **Timing.** A step is a frame of the original's: 30 a second in the town.
- **Pacing.** He paces between the first two points. On a step there, 1 in 64, he looks round (`entry-80109E28`,
  on his own thread); then he walks on.
- **Walking his way.** He walks to each point in turn. From the third point on, Matthew within 800 means he's
  seen.
- **At a point.**
  - Look back: he turns +0x80 a frame for frames 16–31, watches for frames 32–61, then turns back. Matthew in the
    point's zone while he watches means he's seen.
  - Look aside: he turns −0x80 a frame for frames 0–7, watches for frames 16–45, turns back for frames 54–61.
    The lean is to come.
- **Seen.**
  - Seen while walking or looking back: his "who's there" scene (`entry-80109CF8`).
  - Seen while looking aside: `entry-80109D38`.
  - After either, the points' seen links lead him back. Before the third point, being seen is forgotten and he
    paces again.
- **At his door.** Matthew in zone 4 (by the door, out of sight): the door's scene (`entry-80109D60`), then the
  keypad. Matthew near but outside the zone: seen.
- **The keypad.**
  - While it's up, an event holds the game: Matthew isn't the player's.
  - Each step takes the first newly pressed face button (△ ○ ✕ □); it's right if the code has that button at
    that place.
  - 16 frames after the fourth press: all four right, "it opened" (`entry-80109EB4`: through door 104 into area
    28). Otherwise "no good" (`entry-80109EF0`; from the third miss with the hint, `entry-80109EF8`) and the
    keypad again.
- **Drawn:** the buttons entered, 16 × 16 at (−32 + 16·i, 60), in the buttons' colours (`screen.fs`).

## The robber (minigames 18 to 20)

- **At the diner's door** (src: the diner's door check): on the robbery's day (+0x5C bits 4–11), 15:00–19:59, with
  +0xEC bit 1 set and +0x10 bit 15 clear, there's no entry. Instead his scene plays (`entry-80109F5C`, minigame
  18), +0x10 bit 15 is set, and the lookout mode is off.
- **His loop.** Afterwards he walks round the town's eight points (`robber-patrol`) at 30, motion 1.
- **Spoken to** (src: lookout_act's robber mode):
  - the first time, his scene (`entry-8010A008`, minigame 19); then he flees round the loop, running (0x82,
    motion 5) whenever Matthew is within 0x800;
  - the next time, caught (`entry-8010A064`, minigame 20; +0x10 bit 16).
- The diner's door also has the original's first check: Mel's stop 16 and +0x18 bit 2 clear give the overheard
  scene (`entry-8010B89C`).

## Status

Built: pacing, the tail, being seen, the door, the keypad (`tests/library/test_tail.fs`).

To come:
- the look aside's lean;
- the keypad's button icons;
- (the robber's minigames are built: see below).
