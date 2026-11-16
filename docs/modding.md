# Modding OpenMizzurna

How to make a mod: what it can replace, how its scripts plug into the game, how to test one. Written so that
someone new to the project - or an assistant starting a fresh session - can pick it up from here. The game's data is
`docs/library.md`; the language `docs/forth.md` (and ClankForth's own); the actor kernel `docs/actors.md`; the map
of the game `docs/subsystems.md`. The mod system is OpenHuey's (the Haunting Ground rebuild), adapted.

## In one minute

A mod is a folder (or a `.zip` of one) in `mods/` beside the library (`<home>/mods/`: beside the program, or in
`$OM_HOME`):

```
mods/my-mod/
  mod.fs                 the manifest: its name, version, what it needs
  library/...            files that replace the library's, or add to it (the same paths as the library's)
  scripts/my-mod.fs      its Forth: an actor of its own, a hook taken, words
```

The sample, `examples/mods/clock-line`: an actor that hears the clock and draws the day and hour as a line at the
top of the screen, its caption a file of its own. Try it without installing it:

```sh
openmizzurna --mod examples/mods/clock-line
```

Install one by copying its folder (or zip) into `mods/`. When there are any, the title's menu has **MODS** under
its own four lines: each mod ON or OFF, why one didn't load, its place in the order (below).

## The manifest: mod.fs

OpenMizzurna reads it (it isn't run): a word per line, `\` and `( )` comments.

```forth
mod: my-mod                  \ its name: letters, digits, - ; also its scripts' vocabulary
version: 1 0                 \ major, minor
requires: openmizzurna 1 0   \ the mod API it was written for (below)
requires: other-mod 1 0      \ another mod it needs, at least this version
after: other-mod             \ load after that one, if it's on
```

- **The mod API** (`requires: openmizzurna M m`): the game's messages, broadcasts and hooks a mod uses. This
  OpenMizzurna has **1.1** (`mod-api` at the console: 101; 1.1 added `walk-intent`; `MODS_API_MAJOR` / `MINOR` in `src/game/mods.h`). A mod
  needs the same major and a minor no newer. A hook or message added raises the minor; one renamed or gone, the
  major.
- **Turned off** (the reason on stderr, in `.mods` and in the MODS menu) when: a `requires:` isn't met (the API,
  a mod missing, off or older); its name is one of the game's vocabularies - any vocabulary the game's scripts
  make (`clock`, `matthew`, `title`, `events.main` ...: those loaded, and any file or folder in `scripts/`) or the
  engine's own (`forth`, `engine`, `actors`, `tester`, `mods`, `openmizzurna`); its scripts fail to load - an error
  in them, or code run as they load that never ends (stopped after 50 million steps: "ran too long"). What a
  failed script made before its error stays. Nothing else stops the game.
- **Order**: `mods/order.txt` (`on: name` / `off: name` lines; the MODS menu writes it). A mod it doesn't name is
  on, after those it names, by name. `after:` moves a mod below another. `--mod` ones come last. A later mod's
  files win over an earlier one's; hooks chain in load order (the last loaded asked first).
- **A zip** works the same: `mods/my-mod.zip`, `mod.fs` at its top or in the one folder it holds (as zipping the
  folder makes it). Read in place, nothing unpacked; stored or deflated, no zip64, no encryption; paths' case
  never matters in it.
- **No sandbox**: a mod's scripts can do anything the game's can. OpenMizzurna says so when it loads one.

## Replacing the game's files (the files layer)

Every asset the engine reads goes through `files_read` (`src/core/files.c`): models, motions, images, text, fonts,
records, the executable's tables, sound banks and sequences, the XA music, movies. It looks in each mod's
`library/` first (the last mod in the order first), then in the library. So a mod replaces a file by having one at
the **same path** under its `library/` - `mods/my-mod/library/title/logo.png` for the library's `title/logo.png` -
and adds one by a path the library hasn't (the sample's `clock-line/caption.txt`; read with `library-file`). On
Linux a folder mod's paths are case-sensitive: keep the library's.

What the library holds and in what form is `docs/library.md`; the best start is to open the file in your library,
copy it into the mod at the same path, change it. In short:

| What | Path | Format |
|---|---|---|
| a picture (the title's, the HUD's, the sky's, the sprites, fonts' glyphs) | `title/*.png`, `ui/`, `sky/`, `sprites/NNN/<i>.png`, `cdbin/033_4/5/3.font.png` ... | PNG. An indexed one (4 / 8 bits) where the library's is (a font, an area's sheet: its palette `.clut.png`), else RGBA; alpha 0 clear, 128 blends when drawn semi-transparent, 255 solid |
| a model's texture | `textures/common/<name>.png`, an area's own `textures/area-NNN/<name>.png`, a streamed character's `models/<entry>.texture/` | PNG, RGBA, the size the library's is |
| a model | `models/<CD.BIN path>.gltf` + `.bin` (an area's props, the characters); the town's tiles `town/CC-RR/terrain.gltf`, `objects.gltf` | glTF 2.0 as the import writes it: a node per part (its exact rest turn in `extras`), materials naming their texture in `extras` (`area` / `own`) |
| a motion | `cdbin/033_4/1/<i>.txt` (the characters' shared ones), `cdbin/NNN_4/1/` (an area's), `cdbin/033_4/0/` (props') | text, a line a frame: each part's turn, the root's move |
| text | `cdbin/NNN_4/4.txt` (an area's), `cdbin/033_4/4.txt` (the common), `cdbin/033_4/6.txt` (the list menus'), `cdbin/NNN_T.txt` (an event pack's) | UTF-8: `#id` then the message's lines; the pad's buttons as △ ○ ✕ □, codes as `{page}` ... (`src/data/text.h`). A translation is these files |
| a sound | `cdbin/<body>.samples/<i>.wav` (a bank's sample; the bank `cdbin/<header>.bank.txt`) | WAV, mono, 16-bit, its loop a `smpl` chunk; its pitch is the bank's, not the file's rate |
| a tune | `cdbin/<entry>.sep/<i>.mid` | Standard MIDI, as the import writes it (the loops' controllers kept) |
| music and voices | `xa/TSG/<channel>.flac` | FLAC, stereo, 37.8 kHz |
| a movie | `movies/NAME.m2v` + `NAME.flac` + `NAME.txt` | MPEG-2 video (any MPEG-1 / 2 elementary stream), its sound as FLAC, the text its size, rate, frames, delay and sound |
| one of the executable's tables | `exe/<name>.bin` | the table's bytes (`src/data/exetables.c` names them) |
| an area's billboards, the roads | `cdbin/NNN_4/5.billboards.txt`, `cdbin/033_4/5/1.roads.txt`, `2.roads.txt` | text, a record a line |

What can't be replaced in a readable form yet: the areas' own data (floors, walls, cameras, lights - in
`cdbin/NNN_4/` as the disc has them, until their subsystems name their fields) and the event scripts, which are
Forth (`scripts/events/`), not data.

## Scripts: an actor of your own

`scripts/<name>.fs` is loaded after the game's scripts (`game.fs`), as the vocabulary `<name>`. Its folder is also a
root for `USING:`, so a bigger mod splits into `scripts/<name>/part.fs` (`USING: <name>.part ;`). The game's actors
already exist; a mod spawns its own as its file loads, and the first messages arrive at the next step. Going back
to the title (the game over, an ending) loads every script again, mods too.

```forth
\ scripts/my-mod.fs
IN: my-mod
USING: engine actors messages clock ;      \ the game's vocabularies it uses

message hours-seen? ( -- )                 \ a mod declares its own messages the same way
message hours-seen ( n -- )

state: my-state
    cell field hours                       \ its own fields
end-state

behaviour my-behaviour
  on spawned ( -- )  self subscribe minute-passed ;
  on minute-passed ( t -- )  time>minute 0= if  1 hours +!  then ;
  on hours-seen? ( -- )  hours @  sender send hours-seen ;
end-behaviour

my-behaviour my-state s" my-mod" spawn constant me
```

The rules are the game's own (`docs/design.md`, `docs/actors.md`, `CLAUDE.md`):

- **Everything is actors passing messages.** A mod's actor hears the game's broadcasts (`subscribe`) and sends the
  game's messages (`send`); it never writes another actor's state. To ask something, send a message and handle the
  answer (the sample answers `minutes-heard?`).
- **Facts may be read**: the engine's C words (bodies, models, the library), and the words a subsystem offers for
  reading (`clock-time`, `title-up?` ...).
- **Messages are declared once**, the stack comment the declaration: `scripts/messages.fs` holds the game's.
- **A handler takes exactly its message's arguments** and leaves nothing; **every word's stack comment is checked**
  against its code as it loads. The game loads with no warnings (`--eval "error-count ."` prints 0); a mod should
  too, and with no `hides` warnings: name your words after what they mean in your mod.
- **Readable Forth**: named numbers (constants, `enum:`), locals over stack juggling, a structure
  (`begin-structure`) where data has fields; your workings inside `<PRIVATE ... PRIVATE>`.
- **A sequence over time** is a task, or a handler that waits (`n wait`; ClankForth's `docs/actors.md`).
- **Real time**: the game steps 30 times a second (`dt`); say durations in seconds (`0.5e seconds wait`).

### Drawing

Drawing is a frame's 2D, outside the actors: `' my-draw on-draw` runs a word every frame. What it shows is kept
by the actor in its vocabulary's variables (written by the actor alone), as `flare.fs` does and the sample does.
The words (`docs/forth.md`): `pen-color ( rgba -- )`, `pen-scale`, `draw-text`, `draw-rect`, `image` /
`draw-image`, and the game's font: `0 font  0 font-palette  scale pen-scale` then `font-text ( addr len x y -- )`
(ASCII, and △ ○ ✕ □ as UTF-8). `screen.fs` places things on the original's 320 x 240 screen, its origin at the
centre: `>screen ( x y -- px py )`, `scale`.

### The game's vocabularies a mod usually uses

| Vocabulary | Has |
|---|---|
| `engine` | the C words: keys, drawing, the library's files (`library-file`, `image`, `model`, `motion`), sound, `.mods`, `mod-on?` |
| `actors` | the kernel: `message`, `behaviour` / `on` / `end-behaviour`, `state:` / `field` / `end-state`, `spawn`, `send`, `broadcast`, `subscribe`, `self`, `sender`, `actor-named` |
| `messages` | every message of the game's, and names they use (`common-text`, `the-town`, `clock-hold` ...) |
| `mods` | the hooks: `hook-before:` (and `hook:` for the game) |
| `keys` | the pad on the keyboard: `pad-circle`, `pad-cross`, `pad-up` ...; `went-down?` |
| `screen` | the original's screen: `>screen`, `scale`, the blends |
| `clock` | the story's time: `clock-time`, `time>date`, `date>time` |
| `title` | `title-up?`: the title is showing (no game) |
| `options` | the options menu (F1): `add-option` - a row of a mod's own (`docs/subsystems/options.md`) |

`vocab-words <name>` at the console lists a vocabulary; `.actors` the actors.

### Broadcasts worth hearing (subscribe to them)

| Message | Stack | When |
|---|---|---|
| `tick` | `--` | every step (30 a second) |
| `time-set` / `minute-passed` | `t --` | the story's clock jumped / a minute ran by (`t`: minutes since 12/25 00:00) |
| `prologue-over` | `--` | the prologue ended (Matthew at the station, 12/25 10:00) |
| `area-entered` | `area scene --` | an area is in play (27 the town) |
| `scene-shown` | `scene --` | the area's scene changed |
| `player-control` | `flag --` | Matthew is the player's to move, or not |
| `left-the-car` | `--` | Matthew got out of his car |
| `window-closed` / `list-closed` | `answer --` | the message window / a list menu closed, with the answer (-1 none) |
| `ring-chosen` | `item shown-to --` | the item ring closed on an item (-1 none) |
| `person-departs` / `person-stopped` | `slot route --` / `slot --` | one of the townsfolk left their stop / stopped walking a route |
| `car-arrived` | `car --` | a car with riders reached its route's end |

### Messages worth sending

| Message | To | Stack | Does |
|---|---|---|---|
| `set-time`, `advance-minute`, `hold-clock` / `release-clock` | `s" clock" actor-named` | `t --`, `--`, `reason --` | the clock |
| `window-say` | `s" window" actor-named` | `bank id portrait palette secs --` | a message of the game's text |
| `give-item` / `take-item` | `s" items" actor-named` | `item --` | into / out of Matthew's things |
| `play-sound`, `play-music` | `s" sound" actor-named` | `id --`, `row --` | an effect, the background music |
| `screen-effect` | `s" effects" actor-named` | `n --` | one of the screen's effects |
| `set-weather` | `s" daylight" actor-named` | `weather hold --` | clear, overcast or snowing |

All of them: `scripts/messages.fs`, each with what it means and whom it goes to. Before sending one, read its
handler (`grep -rn "on <name>" scripts/`) to see what it really does.

## Hooks: when the game must ask

An actor of the mod's own changes what happens by reacting. Where a mod must change what the game *decides* -
how Matthew walks, what an item does - the game offers a **hook**: a deferred word holding the game's own way, its
stack effect said (`scripts/mods.fs`).

**The game's side.** In the subsystem's file, the game's way as a word, then the hook made with it; the code calls
the hook:

```forth
: walk-by-pad ( -- )  ... ;                   \ the game's own way
' walk-by-pad hook: matthew-walk ( -- )       \ hook: ( xt "name" -- ), its stack comment right after the name
```

**A mod's side.** It keeps what the hook holds now (the game's way, or an earlier mod's), sets its own word, and
for anything it doesn't handle passes on to the kept one with the same arguments:

```forth
USING: mods matthew ;
hook-before: matthew-walk walk-before ( -- )  \ hook-before: ( "hook" "name" -- ): walk-before runs what was there
: analog-walk ( -- )  stick-tilted? if  ...  else  walk-before  then ;
' analog-walk is matthew-walk                 \ checked: analog-walk must have the hook's effect
```

So mods taking the same hook chain in load order: the last loaded is asked first, each handling or passing on, the
game's way at the end. Taking a hook doesn't disturb the others before it.

| Word | Stack | |
|---|---|---|
| `hook:` | `xt "name" --` | (the game's) a hook named so holding xt, the game's way; its stack comment follows the name |
| `hook-before:` | `"hook" "name" --` | (a mod's) `name`: a deferred word holding what the hook holds now; its stack comment (the hook's) follows. An error if `hook` isn't a hook |
| `hook?` | `xt -- flag` | is it a hook |
| `hook-taken?` | `hook -- flag` | has a mod set its own (`' matthew-walk hook-taken?`) |
| `hook-game's` | `hook -- xt` | the game's own way, what every chain ends in |

**The hooks there are** (each also on its subsystem's page; adding one raises the mod API's minor):

| Hook | In | Since | What |
|---|---|---|---|
| `walk-intent ( -- heading pace )` | matthew.fs | 1.1 | where Matthew should head (0 .. 4095, the game's angles) and how fast - `standing-pace`, `walking-pace`, `running-pace` - in place of the pad's tank controls; `by-the-pad` (-1) leaves them as they are. The game's answer is always `by-the-pad`. He turns toward the heading quickly and walks with the game's speeds, motions and walls. The sample: `examples/mods/stick-walk` |

A new hook, when a mod needs one: the game's way as a word, `hook:` in the subsystem's file with the call site
going through it, a line in the table above and on the subsystem's page, `MODS_API_MINOR` raised
(`src/game/mods.h`), and a test of it with a mod taking it.

## Testing a mod

OpenMizzurna runs Forth tests headless (`--test`): a file using `tester`, `T{ ... -> ... }T` checks, ending in
`test-summary`.

```forth
\ my-mod.fs
USING: tester engine actors messages my-mod ;

: settle ( -- )  4 0 do  deliver  loop ;
settle
testing the mod is on and its actor there
T{ s" my-mod" mod-on?  s" my-mod" actor-named 0< -> true false }T
test-summary
```

```sh
SDL_AUDIO_DRIVER=dummy openmizzurna --mod path/to/my-mod --test my-mod.fs
```

- A `--test` run sees only the `--mod` ones (never `mods/`), or the folder `--mods DIR` names.
- With no library (`OM_HOME` a folder without one) the game's actors are there but no game starts; with the
  library, a headless run starts a new game at once (no title).
- `deliver` delivers the messages queued; `1 tick-tasks actors-frame` runs a step; `key: X true key-hold` holds a
  key. To read an actor's own fields in a test: `actor enter ... leave-actor` (tests only).
- To see it: `openmizzurna --mod my-mod --hidden --frames 2 --eval ": w 300 0 ?do run-tick loop ; w  s\" /tmp/shot.png\" screenshot"`.
- The repository's own: `tests/mods/` (`mods_clock_line`, `mods_clock_line_zipped`, `mods_layers` - the order,
  the files layer and every way a mod is turned off -, `mods_menu` / `mods_menu_after`, `mods_test_hooks`).

## Pitfalls

- **Your own words win over the game's** in your file, the core's too: a mod defining `move` breaks `move` for the
  rest of its file (the loader warns: "hides").
- **Two used vocabularies with the same word**: the later in `USING:` wins in a file; `vocab:word` reaches the other.
- **Control words only inside a definition** (`if`, `do`, `case`): not at a file's top level.
- **Locals**: the first `{: :}` of a definition can't be inside an `if` or a loop.
- **Floats are on their own stack**: a float in a message travels as a cell.
- **Never write another actor's state**: send it a message.

## The code

`src/game/mods.c` (finding, ordering, loading, the console's words, the menu's list), `src/core/files.c` (the files
layer), `src/core/zip.c`, `scripts/mods.fs` (the hooks), `scripts/mods/menu.fs` (the MODS menu, from
`scripts/title.fs`), `src/main.c` (`--mod`, `--mods`).
