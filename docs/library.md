# The game library

The player's disc is imported once into a **library** of game data in common formats, and the engine reads only
the library. (OpenHuey's design, `docs/library-and-mods.md` there, adapted to Mizzurna's disc.)

## The first run

With no library, OpenMizzurna shows a **disc picker** (an SDL file dialog): the player chooses their disc image.
The import then runs on a thread of its own with a progress window (the file at hand, how much is done;
closing it cancels). `openmizzurna --import <disc.cue>` does the same from a command line.

- **Accepted**: a raw image - `.cue` with its `.bin`, or a lone `.bin` (2352-byte sectors, MODE2). The music
  (TSG.XA) and the movies (.STR) are CD-XA form 2 sectors, which a 2048-byte `.iso` doesn't keep: such an image
  is refused, saying why.
- **Checked**: the executable's hash says which disc it is. The English version (the 2021 fan translation)
  is the one supported; anything else is refused by name ("the Japanese original: not supported yet",
  "not Mizzurna Falls").
- **Atomic**: written to `library.new/` and renamed when done, so a failed or cancelled import leaves no half
  library.

## Where things go

```
<game folder>/
  openmizzurna         the program
  scripts/             the game's scripts (shipped)
  library/             made by the import
    manifest.txt       what it was made from and how (format versions, every file with its size)
    ...                the converted data (below)
  mods/                the player's mods
  user/                saves, settings
```

The library is portable (beside the program); where that folder can't be written, the user's data folder
for programs is used, and `OM_HOME` names another (tests, tools).

## The disc and what it becomes

The disc: `SLPS_017.83` (the game's executable), `CD.BIN` (19 MB: every area's data in containers, most of
them LZ-packed), `MAP.BIN` (3.6 MB: the town's tiles), `TSG.XA` (110 MB: the music and voices, 7 interleaved
tracks), five `.STR` movies (260 MB), `FILELINK.BIN`.

| Kind | On the disc | In the library | Why |
|---|---|---|---|
| textures | TIM images (4 / 8 / 16-bit, with palettes) in CD.BIN's containers | PNG (RGBA; a 4 / 8-bit image's other palettes as more PNGs, a sidecar naming them) | edit with anything; texture packs |
| text | the 2021 script: Huffman-coded message blocks, the game's glyph codes | UTF-8 text files, one per message table, ids kept (`#id` then the lines; buttons and control codes as `{circle}`, `{page}` ...) | read, fix, translate |
| models | TMD (vertices, normals, coloured and textured polygons) | glTF 2.0 with its textures | new figures, costumes, props |
| motions | the game's keyed animation ("GSWHPANM" sets) | the engine's own format, written plainly (glTF animation can't hold the keys exactly) | |
| areas | each area's container: model groups, floors, walls and zones, cameras, objects, lights | a folder per area: the meshes as glTF, the rest as text where its fields are known, else as it is | the biggest modding payoff; last |
| the town | MAP.BIN's tiles: models, textures, billboard props | glTF + PNG per tile | |
| sound banks | VAB (programs, tones, envelopes; ADPCM samples) | a WAV per sample (its loop in a `smpl` chunk) and the bank as text (programs, tones, envelopes) | replace a sound |
| sequences | SEP / SEQ (the music and effect sequences) | Standard MIDI files, the loop points as markers | replace a tune |
| music and voices | TSG.XA (XA ADPCM, 7 tracks) | FLAC, one per track | replace a track |
| movies | STR (MDEC video, XA audio) | a modern video file (decoded once at import and encoded with a video library, e.g. libavcodec); the sound as FLAC | any player opens them; a mod's movie is any video |
| the executable's tables | addresses in SLPS_017.83 | a file each, by name; readable forms as subsystems want them | no addresses at run time; mods |
| event scripts | bytecode in CD.BIN's event packs and the areas' overlays | converted **once**, at development time, into Forth (`scripts/story/`), not at import | edited by hand from then on |

Order: the executable's tables, CD.BIN's containers unpacked, textures and text first (the first subsystems
need them); then sound, music, models, the town, movies; areas last. Until its turn a kind is copied as it is.

**Libraries.** The conversions may use libraries (as OpenHuey does: stb, dr_flac, cgltf): small vendored ones
where they're enough, a real codec library where one is needed (video). They are needed for a release anyway.

**Conversions are exact**: a converted format gives the engine the same data the original did; each kind has a
check (a ctest) that decodes both and compares.

## Status

**Built** (step 1, the plain library): the picker and the import with its progress (`src/platform/library.c`), the
disc image reader (`src/data/disc.c`: .cue / .bin, ISO 9660, XA streams as raw sectors), CD.BIN's table and
unpacking (`src/data/cdbin.c`), the manifest and the check at start-up (every file listed there at its size),
`--import`. The library today:

```
library/
  manifest.txt     openmizzurna-library 1 / disc <crc> <program> / convert ... / file <size> <path> ...
  exe/SLPS_017.83  the executable (until the tables the game reads from it are files of their own)
  cdbin/NNN_K.bin  CD.BIN's 565 entries, unpacked (group NNN, kind K)
  disc/            MAP.BIN, TSG.XA, FILELINK.BIN as they are
  movies/          the five .STR as they are
```

Checks (ctest, with the developers' disc - `OM_DEV_DISC`): `disc_check` (every file of the image the same as
phase 1's extraction) and `cdbin_check` (every CD.BIN entry unpacked the same).

**Next**: the containers inside CD.BIN's entries, their TIM images to PNG; the text to UTF-8; the executable's
tables by name.
