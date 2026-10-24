# OpenMizzurna

Mizzurna Falls (PlayStation, Human Entertainment, 1998) rebuilt as a modern program: a C engine on SDL3 and
OpenGL 4.6, with the game written in Forth as actors passing messages. The architecture is OpenHuey's (the
Haunting Ground rebuild), whose Forth system and actor kernel it reuses.

- A decompilation of the original and a runnable C port of it (phase 1) are the **specification**: when we
  need to know what the game does, we read it there. They aren't part of this repository (they hold the
  original's code and data); the `src:` notes through the code name the original's functions and addresses.
- This repository is the new program. The game should feel the same; the code shouldn't look decompiled, and
  nothing of the PlayStation's hardware survives in it.

None of the game's data is here: you need your own copy of the English disc (a .cue / .bin image). The first
run imports it once into a library of common formats (`docs/library.md`).

Start with `docs/design.md`, then `docs/actors.md` (the kernel), `docs/forth.md` (the language) and
`docs/subsystems.md` (the map of the game). The game's data: `docs/library.md`.

## Building and running

Needs CMake, a C compiler, SDL3 and OpenGL 4.6; libavcodec (FFmpeg) for the movies, found by pkg-config.

```sh
cmake -S . -B build -G Ninja && ninja -C build
build/openmizzurna                                   # the game (` opens the Forth console)
build/openmizzurna --hidden --frames 3 --eval "..."  # headless; `s" shot.png" screenshot` for a picture
cd build && SDL_AUDIO_DRIVER=dummy ctest             # the tests
build/forth                                          # the Forth on its own (a prompt)
```

Keys: the PlayStation pad as the keyboard (`scripts/keys.fs`): arrows / W A S D the d-pad, C circle, X cross,
Z square, V triangle, Q / E L1 / R1, 1 / 3 L2 / R2, Return start, Tab select; a gamepad works as the pad.

## Layout

| path | what |
|---|---|
| `src/forth/` | the Forth system (from OpenHuey) and the `forth` tool |
| `src/kernel/` | the actor kernel: MActors, mailboxes, delivery (from OpenHuey's HActor) |
| `src/platform/` | SDL3 window, GL 4.6 loader, input and controls |
| `src/render/` | the renderer |
| `src/game/` | the engine loop's state, the engine's Forth words, the console |
| `scripts/` | the game: `prelude.fs` (the language), `game.fs` (where it starts), a file per subsystem |
| `tests/` | `test_*.fs` the language and kernel (the `forth` tool); `<subsystem>/test_*.fs` the game's, headless |
| `docs/` | the design, the dialect, the subsystems' pages |

## License

GPL-3.0 (`LICENSE`), as OpenHuey, whose Forth system and actor kernel it reuses. The libraries in
`src/third_party/` keep their own licenses: stb MIT or public domain, dr_flac public domain or MIT-0, cgltf
MIT. Mizzurna Falls itself belongs to its rights holders; nothing of it is distributed here.
