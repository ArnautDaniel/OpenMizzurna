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
| movies | STR (MDEC video, XA audio) | MPEG-2 video (decoded at import, encoded with libavcodec) and the sound as FLAC | any player opens them; a mod's movie is any MPEG-1/2 video |
| the executable's tables | addresses in SLPS_017.83 | a file each, by name; readable forms as subsystems want them | no addresses at run time; mods |
| event scripts | bytecode in CD.BIN's event packs and the areas' overlays | converted **once**, at development time, into Forth (`scripts/story/`), not at import | edited by hand from then on |

Order: the executable's tables, CD.BIN's containers unpacked, textures and text first (the first subsystems
need them); then sound, music, models, the town, movies; areas last. Until its turn a kind is copied as it is.

**Libraries.** The conversions may use libraries (as OpenHuey does: stb, dr_flac, cgltf): small vendored ones
where they're enough, a real codec library where one is needed (video). They are needed for a release anyway.

**Conversions are exact**: a converted format gives the engine the same data the original did; each kind has a
check (a ctest) that decodes both and compares.

## Status

**Built**: the picker and the import with its progress (`src/platform/library.c`), the disc image reader
(`src/data/disc.c`: .cue / .bin, ISO 9660, XA streams as raw sectors), CD.BIN's table and unpacking
(`src/data/cdbin.c`), the manifest and the check at start-up (every file listed there at its size), `--import`,
and the first conversions (`src/data/cdconv.c`):

- **CD.BIN's containers as folders** (`src/data/container.c`): an entry or part that is a container becomes a
  folder, its parts numbered, `index.txt` naming each (`part <i> <kind> <file>`) and the zero bytes after the
  last (`tail`). 201 containers.
- **Images as PNG** (`src/data/tim.c`, `src/data/png.c`): every TIM (195) as `N.png` - an indexed PNG (4 / 8 bits)
  keeping each pixel's palette index, shown in the first palette - or RGBA (16 / 24 bits); `N.clut.png` the
  whole palette block (a colour a pixel); `N.tim.txt` the depth and where the game put both in video memory
  (how models and sprites name a texture and palette). Colours: alpha 0 transparent, 128 a texel that blends
  when drawn semi-transparent, 255 solid.
  - A picture with one palette (the photos, the title's pieces) looks right as it is. An **area's texture
    sheet** is many textures in one image, each drawn with its own palette row, so its preview in the first
    palette looks scrambled; the PNGs hold everything, and proper textures (each cut out in its own palette)
    come with the models' conversion, which knows which part of the sheet each polygon uses.
- **Text as UTF-8** (`src/data/text.c`): every text block (87: the areas' and the event packs' in the 2021
  translation's format, four leftover blocks of the original's) as `N.txt` - `#id` then the message's lines;
  `\\ \{ \}` escape, `\#` a `#` starting a line, `{$XXXX}` a code by number; the pad's buttons as △ ○ ✕ □ and
  the dash —. The English text uses nothing else. An event pack keeps its bytes as they are (its script becomes
  Forth at development time) and its text is also written as `N.txt` beside it.
- **Sound banks** (`src/data/vab.c`): a bank's header as `N.bank.txt` - every field named (programs, tones with
  their key ranges, tuning, volume, pan, bend and the envelope as attack / decay / sustain / release), the bytes
  coming back from it - and its body (another entry: an area's `NNN_2` for its `NNN_1`, the music's `058_3` for
  the header in `033_4`; paired by size) as `M.samples/` - `<i>.wav` a sample (mono, 16-bit, 44100 Hz, its loop as
  a `smpl` chunk) and `samples.txt`. 32 bodies. The WAVs are the samples as the game's sound chip decodes them;
  the ADPCM itself isn't kept.
- **Sequences** (`src/data/seqmidi.c`): a SEP as `N.sep/` - `<i>.mid` a Standard MIDI file per sequence (its
  header's tempo and time signature as its first events; the loops' NRPN controllers as they are) and
  `index.txt` (the version, each sequence's id, and where the original carries running status across a tempo
  change, which a MIDI file can't). The SEP comes back byte for byte.
- **XA music and voices** (`src/data/xa.c`, OpenHuey's FLAC encoder): TSG.XA as `xa/TSG/<channel>.flac`, one per
  channel (7, stereo, 37.8 kHz), and `index.txt` - the same samples the XA decodes to.
- **The executable's tables** (`src/data/exetables.c`): each table a subsystem names, as `exe/<name>.bin`; the
  engine reads them by name (`exe-table`). The first: `music`. The areas' tables (an area's code file, CD.BIN's
  `<area>_0`, loaded at 0x80101ED0) go the same way: the shoot-outs' `shootout-5`, `shootout-26`;
  the tail's `tail-waypoints`, `tail-zones`, `keypad-code`.
- **Fonts** (`src/data/mfo.c`): the game's two (MFO: the dialogue font 033_4/5/3, the list font 5/4) as
  `N.font.png` (the glyphs, an indexed PNG), `N.font.clut.png` (their palettes) and `N.font.txt` (cell, each
  glyph's width and height); the bytes come back from them.
- Everything else as it is, named by its kind: `.spl` (models), `.exe` (OPENING.EXE), `.bin`.

```
library/
  manifest.txt     openmizzurna-library 1 / disc <crc> <program> / convert <kind> <format> <version> ... / file <size> <path> ...
  exe/SLPS_017.83  the executable (until the tables the game reads from it are files of their own)
  cdbin/           CD.BIN's 565 entries: folders, PNG, text, the rest as it is (29 MB)
  exe/<name>.bin   the executable's and the areas' tables by name
  xa/TSG/          TSG.XA's channels as FLAC
  town/            MAP.BIN's 768 tiles: index.txt, a folder per tile (CC-RR/: terrain, objects as glTF; the rest as it is)
  disc/            FILELINK.BIN as it is
  movies/          the five movies: NAME.m2v, NAME.flac, NAME.txt (the STR as it is when built without libavcodec)
```

Checks (ctest, with the developers' disc - `OM_DEV_DISC`): `disc_check` (every file of the image the same as
phase 1's extraction), `cdbin_check` (every CD.BIN entry unpacked the same), `library_import` then
`library_check` (every entry against its library form: the containers and images making the same bytes again,
the text the same characters, the banks' headers and the sequences the same bytes, the samples and the XA tracks
the same sound sample for sample, every model's glTF holding its parts and polygons exactly, every texture the
same texels as video memory, the rest the same bytes). The import takes ~30 s (optimized build).

### Models and their textures

A model (an SPL: a part hierarchy, then a TMD) becomes glTF 2.0: one node per part (its rest position, and its
exact rest angles in `extras`), the polygons as triangles with positions, normals, vertex colours and UVs, a
material per texture. Textures are cut at import from what the game's video memory held when the model was drawn
- that is the only place the PlayStation's texture pages and palettes are known; the engine sees PNG textures:

- **the areas' sets**: for each area, its context - the common images (033_3, in the game's upload order), the
  area's sheet (NNN_3) and the character sheet it loads (063_4 / 064_x, the executable's table) - cut into a
  256 x 256 texture per page and palette that any model drawn there uses (the area's props, and the characters
  whose textures stay loaded: the same character can look different from area to area, as in the original).
  Each texture's most common version is in `textures/common/`; an area whose version differs has its own in
  `textures/area-NNN/` - the engine looks in the area's folder, then in common (1889 common, ~1800 overrides);
- **a streamed character's own** (`models/<entry>.texture/`): its image as the game placed it in a character
  slot, a texture per palette row its polygons use.

A material names its texture in `extras` (`area`: the set of the area being played, by name; `own`: the
model's); its glTF image is a default (the model's own area, or area 1) so viewers show it. 194 models (64 areas'
props, 69 streamed characters, 61 more): `models/<CD.BIN path>.gltf` + `.bin`; a container's `index.txt` names
the model a part became. `tools/gltf_view.py` renders one to a PNG (a look without the engine).

**Motions** (`src/data/motion.c`): every animation as text, a line a frame - a character's (the 193 shared in
`033_4/1/`, each area's own in `NNN_4/1/`: each part's turn and the root's move), a prop's (the 30 in `033_4/0/`:
its place and turn). The game packs a character's frames ("hold this value n frames"); the text has every
frame's values. 1193 motions. Which motion ids are an area's own: the executable's tables `area-motion-first` and
`area-motion-count`. `tools/gltf_view.py` poses a model by a motion's frame.

**Billboards** (`src/data/billboards.c`): an area's part 5 (`NNN_4/5.billboards.txt`: its 16 scenes' sets and the
entries - place, bottom, top, half width, sprite), checked byte-exact; every area's sprites (part 7) as
`sprites/NNN.txt` and `sprites/NNN/<i>.png`, cut at the import from the area's sheet (the town's: `town/sprites`).

**Roads** (`src/data/roads.c`): the town's road graph the cars drive (`033_4/5/1.roads.txt`: 170 nodes, each
where it is and the run of bends leaving it; `033_4/5/2.roads.txt`: 746 bends, each a step and its length).
Known by where they are, like the motions. The cars' routes through it are the executable's
(`scripts/events/car-routes.fs`: docs/subsystems/traffic.md).

### The town

MAP.BIN (`src/data/town.c`) is the town's 32 x 24 tiles, which the game streams in around the camera; the
executable's grid says where each is. `town/index.txt` lists them (`tile <col> <row> <folder> centre <x> <z>`, in
world units: twice the models'), and each tile's folder `town/CC-RR/` holds:

- `terrain.gltf` - its ground (a TMD with no parts), `objects.gltf` - its objects (an SPL model), both cut from
  the town's (area 27's) texture set like the area props (`../../textures/`);
- `obstacles.bin`, `zones.bin` (its region lists, as they are until the movement subsystem reads them), and
  `billboards.txt`: its billboards (`origin x z`, then `billboard i at x z from bottom to top half-width w sprite s`;
  the town's sprites: `town/sprites.txt`, `town/sprites/<i>.png`, cut from the town's sheet).

The street lamps: the town's 38 lamps light windows and lamp heads through palettes the game fills as they go on
and off (src: func_800243F0: the last row of video memory, lamp i's palette at 16 i). Their textures are cut as
they are by day, unlit, and again lit (`textures/lamps/`); a material that is a lamp's light says which
(`"lamp": i` in its extras).

The sky's pictures (`sky/`: clouds, the horizon strip, sun, flares, moon) are cut from the common images the same way (docs/subsystems/sky.md).

The check: every tile five parts and nothing more, the region parts the same bytes and the billboards the same again from their text, the models'
glTF exact as for any model. (MAP.BIN's sector padding, and a tile's padding between parts, hold leftovers of
the disc's making that the game never reads; they aren't kept.) 768 tiles, 1536 models.

**Deferred to their subsystems**: the areas' own data (`NNN_4` part 0: the area's description, scenes' object
visibility and cameras, object animations, lights, reverb; part 2: floors' obstacles and zones; parts 5-7:
billboards and snow; parts 8, 10: not known yet) stays as it is in `cdbin/` until the subsystem that reads it is
built and its fields are named - then it becomes text. The town's region and billboard parts the same.

### Movies

A movie (`src/data/str.c`: the five STR files, MDEC pictures and XA sound interleaved) becomes
`movies/NAME.m2v` - MPEG-2 video at 15 frames a second, encoded at import by libavcodec's own encoder at a fine
fixed quantiser - `NAME.flac` (the sound, exact) and `NAME.txt` (size, rate, frames, the delay before frame 0
shows, the sound's file). The pictures are the one conversion that can't be exact (no common format holds the
MDEC's); the check decodes both and holds every picture to 38 dB PSNR of the STR's own (the worst is 43 dB).
173 MB for the STRs' 227. Any player opens them; the engine plays them with libavcodec (built in when found by
pkg-config; without it the import keeps the STRs as they are).

**Next**: the executable's tables as each subsystem names them; then the subsystems.
