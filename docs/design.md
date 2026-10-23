# Design

OpenMizzurna is Mizzurna Falls rebuilt as a modern program: a C engine (`src/`) on SDL3 and OpenGL 4.6, and
the game itself as Forth actors passing messages (`scripts/`). It follows OpenHuey's architecture (the
Haunting Ground rebuild) and reuses its Forth system and actor kernel.

## Goals

1. **Behave like the original.** Timings, distances, schedules, odds, what a character does when: these are
   the game. The decompilation (`../decomp/`, with the phase-1 port `../port/` as its runnable form) is the
   specification. Each subsystem is checked against it.
2. **Read like a new program.** No raw offsets, PS1 addresses, ordering-table slots or control flow kept only
   because the PlayStation or the compiler shaped it so. Names come from what things mean in the game; magic
   numbers get names, in C and in Forth. A comment may cite the original function a rule came from
   (`\ src: npc_talk`, `/* src: car_update */`), and that is the only trace of the decompilation.
3. **A modern program, not an emulator.** Nothing of the PS1's hardware survives: no VRAM, texture pages,
   CLUTs, ordering tables, fixed-point GTE maths, SPU RAM, CD sectors or 2 MB memory map. The intent of each
   piece is rebuilt with today's means (meshes and textures on the GPU, float maths, decoded audio, files).
4. **Moddable.** All game logic is Forth, loaded at run time; the game's data is a library of common formats
   (PNG, WAV, glTF, text) made from the player's disc. A mod changes either.
5. **Fail loudly and safely.** A mistake in a script prints an error naming the word and the actor; it never
   corrupts memory or crashes the engine.

## The split

| C | Forth |
|---|---|
| platform, input, timing, window | what input means |
| **assets**: importing the disc, loading every format (models, textures, the town's tiles, sounds, music, movies, text, the executable's tables) | asking for them by name or number |
| rendering: models, the town, the sky, weather, lighting, 2D | which figure plays which motion, where; what the screen shows |
| geometry and world queries (floors, walls, collision regions, the road network, door positions) | the rules that use them |
| maths that would be clumsy in Forth (matrices, curves, interpolation, pathing) | the decisions |
| **the actor kernel**: actors, mailboxes, delivery, scheduling, tracing | the actors: their state, behaviours and messages |
| the Forth VM and its tasks | game logic, scripts, tuning |

Assets and world data belong to C. Forth reads them through words and never lays out or parses a file
format. The game state that scripts own (story flags, the clock, inventory, people's whereabouts) lives in
actors.

## Actors

Everything with behaviour is an actor: Matthew, each of the townsfolk, the clock, the story, the doors, the
phone, the car, the camera, the music. An actor has:

- **state**: named fields, private to it;
- **a behaviour**: a table of message handlers (`on heard ... ;`); it can `become` another;
- **a mailbox**: messages others `send` it.

Actors don't touch each other's state. They talk in messages, and the messages a subsystem accepts and sends
**are its API** (listed on its page in `docs/subsystems/`).

- **Facts** may be read directly: C-owned world and asset data (an area's floors, the road network, a
  figure's current motion frame), and bodies.
- **Changes** are messages. To open a door, start a phone call or move the clock on, send a message to
  whoever owns it.

**Bodies.** Where a character physically is - its area (a building, or the town outside), the floor of that
area it stands on, its position, heading, radius and height - is a fact kept in C as its *body*. Only the
character moves its body, but anyone may look (who is near, who can be talked to, who is in the house).

Kernel details: `docs/actors.md`. The Forth dialect: `docs/forth.md`.

## A frame

```
a step (fixed rate): input -> tasks -> [ tick ] -> deliver -> [ frame-end ] -> deliver -> figures advance
a frame (display rate): draw, blending the last two steps
```

**Time is real time.** The game's logic never counts frames: speeds are units a second, durations seconds,
the story's clock minutes. The simulation steps at a fixed rate (30 steps a second by default; `dt` is the
step's length, an engine setting the scripts read), and drawing runs as fast as the display allows - 60, 120,
144 a second - interpolating figures, the camera and anything moving between the last two steps. Changing the
step rate changes no script.

- **Fixed steps, not variable ones**: every step the same length keeps the game deterministic (the same inputs
  give the same game; tests and traces repeat exactly).
- **The original's per-frame values are converted.** Its field loop waited for 2 vertical blanks outdoors (30
  frames a second) and 4 indoors (15), and its speeds, timers, animation steps and script waits were tuned to
  whichever applied. Each is rewritten in what it means - units a second, seconds - named, with the
  original's value and frame rate in a comment (`\ src: 12 a frame at 15 fps (180 a second)`). The words that
  turn real time into steps (`seconds>steps`, `per-second*dt`) live in one place.
- **The event scripts' waits** are converted the same way when the bytecode becomes Forth: `n frames` at the
  area's frame rate becomes seconds (`0.5 seconds wait`).
- **Behaviours tied to the frame itself** (a decision every frame, a random roll every frame) are restated so
  they happen as often in real time, and the subsystem's page says so.
- **Drawing never changes the game**: the renderer reads the last two steps' state and blends; only steps
  change state.

- `tick` goes to every actor subscribed to it, once a step.
- **deliver**: the queued messages are handled in rounds; what a round sends waits for the next. It stops
  when the queue is empty, or after a limit (the rest carries over and the kernel reports it).
- `frame-end` is for actors that collect during the frame and act on the whole of it.

Order is deterministic: rounds are first-in first-out, broadcasts go out in actor-creation order.

## Checking against the original

For each subsystem:

1. Read the original's functions (decomp, and the phase-1 port's cleaned C) and write the rules down on its
   page in `docs/subsystems/`, in game terms.
2. Build it as actors, with the C services it needs.
3. Test it: headless Forth tests with fixed inputs and expected outputs taken from the original; later,
   traces from the phase-1 port (positions, states, messages per frame for a scripted scene) compared.

## Conventions

- One subsystem per vocabulary (`IN: clock`), in its own file (`scripts/clock.fs`).
- Messages are declared once, in `scripts/messages.fs`, with their stack comments. A message name is a verb
  or an event: `talk-to`, `entered-area`, `hour-struck`.
- Fields are declared with the actor's state (`state: ... end-state`) and named for what they mean.
  Anything that lasts beyond one message is a field of the actor that owns it.
- **Names, not numbers.** Areas, people, items, story flags, messages, sounds: each gets a name in a table
  (`scripts/area-names.fs`, `people.fs`, `story-names.fs` ...); a raw id appears only in the table that names
  it. A name marked "?" is only partly understood.
- Units: positions in the game's units; angles in radians inside, degrees at the edges; time in game frames
  where the original counted frames, and in the clock's minutes where it counted those.
