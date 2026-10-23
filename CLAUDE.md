# OpenMizzurna (src-next): notes for an AI assistant

OpenMizzurna is Mizzurna Falls rebuilt: a C engine (`src/`, SDL3 + OpenGL 4.6) with the game as Forth actors
passing messages (`scripts/`). The architecture follows OpenHuey (github.com/ArnautDaniel/OpenHuey,
`openhuey-src/`), whose Forth system and actor kernel were copied here. The decomp (`../decomp/`) and the
phase-1 port (`../port/`, runnable C) are the reference for what the game does; the original executable can be
read too. Read `README.md`, then `docs/design.md`, `docs/actors.md`, `docs/forth.md`, `docs/subsystems.md`.

How this project works:
- **Intent, not structure**: reproduce what the game does (timings, rules, schedules), not how the PS1 code was
  shaped. No PS1 hardware concepts (VRAM, tpages, OTs, fixed point, SPU, sectors) in the engine.
- **Readable**: name every magic number, in C and in Forth (constants, `enum:`, name tables). Readable Forth,
  not transliterated C: named words and structures, locals over stack juggling; clumsy maths becomes a C word.
  A word that juggles the return stack (`>r >r ... r> r> r>`) or reaches deep (`2 pick`, `3 pick`) is a sign: the
  data wants a structure (`begin-structure`), or the work wants a C word. A vocabulary's workings go
  `<PRIVATE ... PRIVATE>`; the scripts load with no `hides` or `defined again` warnings.
- **Actors**: each subsystem is an actor (or a family) with a small vocabulary of messages as its API; an actor
  never writes another's state. Messages are declared in `scripts/messages.fs`.
- **Docs move with the code**: a subsystem's page (`docs/subsystems/<name>.md`) records its messages, rules and
  status; written before the subsystem is built.
- **Tests with every change**: `T{ ... -> ... }T` in `tests/<subsystem>/` (on the game's data: `tests/library/`); screenshots for anything drawn.
- The data comes from the player's disc, imported once into `library/` in common formats (`docs/library.md`).

Building and testing:
```sh
cmake -S . -B build -G Ninja && ninja -C build
cd build && SDL_AUDIO_DRIVER=dummy ctest
./openmizzurna --hidden --frames 3 --eval "..."     # headless
```
