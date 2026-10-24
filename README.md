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

Needs CMake, a C compiler, SDL3 and OpenGL 4.6; libavcodec (FFmpeg) for the movies, found by pkg-config. The
Forth system is [ClankForth](https://github.com/ArnautDaniel/ClankForth), a submodule: clone with
`--recurse-submodules` (or, in a clone, `git submodule update --init`).

```sh
cmake -S . -B build -G Ninja && ninja -C build
build/openmizzurna                                   # the game (` opens the Forth console)
build/openmizzurna --hidden --frames 3 --eval "..."  # headless; `s" shot.png" screenshot` for a picture
cd build && SDL_AUDIO_DRIVER=dummy ctest             # the tests
build/forth                                          # the Forth with the actor kernel (a prompt)
```

Keys: the PlayStation pad as the keyboard (`scripts/keys.fs`): arrows / W A S D the d-pad, C circle, X cross,
Z square, V triangle, Q / E L1 / R1, 1 / 3 L2 / R2, Return start, Tab select; a gamepad works as the pad.

## Releases

Every change to `master` rebuilds the Linux release: `.github/workflows/release-linux.yml` runs
`tools/release-linux.sh` in Ubuntu 22.04 and publishes the tarball as the rolling `latest` release (its version
`<project version>-<date>-<commit>`, the project version from `CMakeLists.txt`); the first build of each project
version is kept as that version's own release too (`v0.0.2`). It is one
binary (SDL3 and an MPEG-2-only FFmpeg linked in) beside its `scripts/`; unpack it, run `./openmizzurna`, and give
it your disc. Each start it brings itself up to date with the newest build and runs that (`src/platform/update.c`:
your library and saves stay; `OM_NO_UPDATE=1` turns it off). The same script makes it on your own machine:
`tools/release-linux.sh [version]`.

## Layout

| path | what |
|---|---|
| `external/clankforth/` | the Forth system: ClankForth, a submodule shared with OpenHuey |
| `src/kernel/` | the actor kernel: MActors, mailboxes, delivery (from OpenHuey's HActor) |
| `src/platform/` | SDL3 window, GL 4.6 loader, input and controls |
| `src/render/` | the renderer |
| `src/game/` | the engine loop's state, the engine's Forth words, the console |
| `scripts/` | the game: `prelude.fs` (its few words for every script), `game.fs` (where it starts), a file per subsystem |
| `tests/` | `test_*.fs` the kernel (the `forth` tool, `tools/forth-tool.c`); `<subsystem>/test_*.fs` the game's, headless |
| `docs/` | the design, the dialect, the subsystems' pages |

## License

GPL-3.0 (`LICENSE`), as OpenHuey, whose Forth system and actor kernel it reuses. The libraries in
`src/third_party/` keep their own licenses: stb MIT or public domain, dr_flac public domain or MIT-0, cgltf
MIT. Mizzurna Falls itself belongs to its rights holders; nothing of it is distributed here.
