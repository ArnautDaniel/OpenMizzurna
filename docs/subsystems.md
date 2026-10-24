# The game's subsystems

A map of Mizzurna Falls: what each part owns and where the original's rules are. Each subsystem gets its own
page (`docs/subsystems/<name>.md`) with its messages, rules and status before it is built. "Original" names the
phase-1 port's files (`../port/src/game/`), which name the decomp's functions; the decomp itself is
`../decomp/src/`.

## Engine services (C, not actors)

| Service | Provides (facts and words) | Original |
|---|---|---|
| library | the disc imported once into common formats; every asset read by name | `cd.c` (CD.BIN containers, LZ), `ccdtim.c`, `map_stream.c` (MAP.BIN) |
| text | the game's messages as UTF-8 (the 2021 English script's Huffman text decoded), drawn in the game's font | `ctexstr.c`, `msgbox.c` |
| world | areas and their scenes, floors, walls and trigger zones, doors' positions, the town's tiles and roads | `area.c`, `floor.c`, `cphysics.c`, `map_stream.c`, `carline.c` |
| figures | characters' models, their motions and keyed animation, billboards (trees, lamps) | `model.c`, `actor_anim.c`, `snow.c` (billboards) |
| render | the picture: areas, the town, figures, the sky, snow, the day's light, 2D | `draw.c`, `sky.c`, `snow.c`, `clock.c` (lighting), `ovl027.c` (tile renderer) |
| audio ([page](subsystems/sound.md), built) | sound effects (VAB), sequences (SEP), music and voice streams (XA), reverb | `sound.c`, `ambience.c`, phase 1's `psx/audio.c` |
| movies | the library's movies (MPEG-2 and FLAC) played: `src/game/movie.c` (libavcodec) | phase 1's `psx/movie.c` |
| saves | the save files (the memory card's contents as files) | `cmcard.c` |

## Actors

| Subsystem | Owns | Original |
|---|---|---|
| Clock ([page](subsystems/clock.md), built) | the date and time (12/25 to 12/31), whether time runs, minutes passing | `clock.c`, `field.c` |
| Story ([page](subsystems/story.md), built) | story flags, the event scripts (bytecode converted to Forth once), event packs, chapters | `cevent.c`, `cnrev.c`, `boot.c` |
| Areas ([page](subsystems/areas.md), built) | the area being played, its scene, going from area to area; each frame's hooks, texture animations | `area.c`, `field.c`, `session.c` |
| Matthew ([page](subsystems/matthew.md), built) | walking and running, collision, looking and acting, his state | `player.c`, `player_misc.c`, `cecheck.c` |
| Townsfolk ([page](subsystems/townsfolk.md), built) | each person's daily schedule, where they are, what they do; talking to them | `timetable.c`, `handlers_chars.c`, `char_talk.c`, `walker.c`, `sreg.c` |
| Daylight ([page](subsystems/daylight.md), built) | the hour's light, the scenes' lights, the fog, the weather | `clock.c` |
| Sky ([page](subsystems/sky.md), built) | the sky's colour, stars, clouds, horizon strip, sun and flares, moon | `sky.c` |
| Snow ([page](subsystems/snow.md), built) | the flakes around the camera or past a scene's windows | `snow.c` |
| Town ([page](subsystems/town.md), built) | the streamed outdoors: its tiles around the camera, the street lamps, its moving objects, what stops him | `map_stream.c`, `ovl027.c`, `clock.c` |
| Traffic ([page](subsystems/traffic.md), built) | the town's cars, their routes, riding in them | `carline.c`, `cobj.c` |
| Doors ([page](subsystems/doors.md), built) | the doors between rooms and out to the town; going through, opening, knocking | `door.c` |
| Camera ([page](subsystems/camera.md), built) | following Matthew, the events' shots | `camera.c` |
| Effects ([page](subsystems/effects.md), built) | fades, wipes, the iris, the flash | `effect.c` |
| Look ([page](subsystems/look.md), built) | ○ and □: the tables' rows, the areas' rules | `cecheck.c`, the overlays |
| Items ([page](subsystems/items.md), built) | what Matthew carries | `items.c` |
| Item ring ([page](subsystems/ring.md), built) | the ring of his items; showing one to someone; the screens' standing still (`ui.fs`) | `ring.c`, `cui.c`, `cevcon.c` |
| Talk ([page](subsystems/talk.md), built) | ○ on a person: their story talks, the area's lines, their own | `char_talk.c`, the handlers |
| Car ([page](subsystems/driving.md), built) | driving, fuel, the roads; the boat | `ovl027.c` (`car_update`, `boat_update`), `hud.c` (fuel) |
| Window ([page](subsystems/window.md), built) | the message window, its pages and choices | `msgbox.c`, `ctexstr.c`, `cui.c` |
| List menu ([page](subsystems/list-menu.md), built) | the board of labels to pick from: the diner's, the bar's orders | `cui.c` |
| Crate ([page](subsystems/crate.md), built) | a crate to push or climb, the perch the rows go by | `player_misc.c`, `player.c` |
| Counter ([page](subsystems/counter.md), built) | sitting at a bar's counter: leaning, calling the barman, ordering | `counter.c`, the overlays |
| Phone ([page](subsystems/phone.md), built) | dialling, calls, who calls when | `ftel.c`, `cui.c` |
| Pictures ([page](subsystems/pictures.md), built) | the small picture beside the message box, the big one above it | `ccdtim.c` |
| Notebook ([page](subsystems/notebook.md), built) | the people and numbers Matthew knows | `fhbook.c` |
| Map ([page](subsystems/map.md), built) | the town map: the places known, his arrow, the names | `mapscreen.c` |
| HUD ([page](subsystems/hud.md), built) | the prompts, the clock, the corner map | `hud.c`, `hudprim.c`, `cecheck.c` |
| Saves ([page](subsystems/saves.md), built) | the save file, the save point's dialog, continuing | `cmcard.c`, `boot.c` |
| Pause ([page](subsystems/pause.md), built) | Start: the pause, the prologue's skip | `pad_map.c`, `cui.c` |
| Title ([page](subsystems/title.md), built) | the logo, the movies, the title and its menu, loading, the endings | `opening.c` |
| Boat ([page](subsystems/boat.md), built) | the rowing boat on the town's lake, the boathouses | `ovl027.c` |
| Sound ([page](subsystems/sound.md), built) | effects, held loops, the background music and ambience, XA, footsteps, the areas' own sounds | `sound.c`, `ambience.c` |
| Fight ([page](subsystems/fight.md), built) | a fistfight: the pad or the AI each side, the hits, the game over | `fight.c` |
| Shoot-out ([page](subsystems/shootout.md), built) | button targets on a foe: the rounds, the camera's path, the game over | `qte.c`, `shootout.c`, the overlays |
| Tail ([page](subsystems/tail.md), built) | tailing the lookout to his door, unseen; the door's keypad | `ovl027.c` (`func_801025E0`) |
| Dog chase ([page](subsystems/dog-chase.md), built) | running from the junkyard's dog to the far side | `ovl008.c` (`dog_update`) |
| Walkers ([page](subsystems/walkers.md), built) | figures along the town's footpaths: the stalker's loop | `walker.c`, `carline.c` |
| Flare ([page](subsystems/flare.md), built) | a flare's glow on the ground (areas 5, 24) | `snow.c` (`ground_flare_draw`) |
| Car chase ([page](subsystems/car-chase.md), built) | after the bike in his car: the knocks, the crash, the clock | `ovl027.c` (states 20–26) |
| Rumble ([page](subsystems/rumble.md), built) | the pads' motors | `frame.c` (pad motors) |
| Errors ([page](subsystems/errors.md), built) | every failure logged (`errors.log`); the actor that failed asked to start over | - (the engine's own) |

## Order of work

1. **Engine basics** (done: the window, the renderer's 2D, the Forth system, the kernel, the console).
2. **The library** (done: every known format converted, `library.md`): the disc picker and import, starting with what the first subsystems need (the
   executable's tables, CD.BIN's containers, textures as PNG, text as UTF-8).
3. Then subsystem by subsystem, each with its C services: clock and story first (they drive everything),
   areas and Matthew (walking in his room), the window and text, townsfolk, then the rest.
