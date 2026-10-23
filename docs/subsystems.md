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
| audio | sound effects (VAB), sequences (SEP), music and voice streams (XA), reverb | `sound.c`, `ambience.c`, phase 1's `psx/audio.c` |
| movies | the STR movies | phase 1's `psx/movie.c` |
| saves | the save files (the memory card's contents as files) | `cmcard.c` |

## Actors

| Subsystem | Owns | Original |
|---|---|---|
| Clock ([page](subsystems/clock.md), built) | the date and time (12/25 to 12/31), whether time runs, minutes passing | `clock.c`, `field.c` |
| Story ([page](subsystems/story.md), the events built) | story flags, the event scripts (bytecode converted to Forth once), event packs, chapters | `cevent.c`, `cnrev.c`, `boot.c` |
| Areas ([page](subsystems/areas.md), drawn) | the area being played, its scene, going from area to area | `area.c`, `field.c`, `session.c` |
| Matthew ([page](subsystems/matthew.md), placed) | walking and running, collision, looking and acting, his state | `player.c`, `player_misc.c`, `cecheck.c` |
| Townsfolk ([page](subsystems/townsfolk.md), placed by the clock, walking their routes) | each person's daily schedule, where they are, what they do; talking to them | `timetable.c`, `handlers_chars.c`, `char_talk.c`, `walker.c`, `sreg.c` |
| Doors ([page](subsystems/doors.md), people through them) | the doors between rooms and out to the town; going through, opening, knocking | `door.c` |
| Camera ([page](subsystems/camera.md), built) | following Matthew, the events' shots | `camera.c` |
| Effects ([page](subsystems/effects.md), built) | fades, wipes, the iris, the flash | `effect.c` |
| Look ([page](subsystems/look.md), area 1) | ○ and □: the tables' rows, the areas' rules | `cecheck.c`, the overlays |
| Items ([page](subsystems/items.md), the list) | what Matthew carries | `items.c` |
| Talk ([page](subsystems/talk.md), Nozz) | ○ on a person: their story talks, the area's lines, their own | `char_talk.c`, the handlers |
| Doors | locks, knocking, who answers | `door.c` (SCKNOCK.C) |
| Car | driving, fuel, the roads; the boat | `ovl027.c` (`car_update`, `boat_update`), `hud.c` (fuel) |
| Traffic | the other cars on the roads | `carline.c` |
| Camera | how each area and scene is framed, tweens, the shot a scene asks for | `camera.c` |
| Window ([page](subsystems/window.md), built) | the message window, its pages and choices | `msgbox.c`, `ctexstr.c`, `cui.c` |
| Items | the inventory and the item ring | `items.c`, `ring.c`, `cui.c` |
| Phone | dialling, calls, who calls when | `ftel.c` |
| Notebook | the people and numbers Matthew knows | `fhbook.c` |
| Map | the town map, places known | `mapscreen.c` |
| HUD | the clock and fuel on screen, prompts | `hud.c`, `hudprim.c` |
| Weather | snow, the sky, the day's light | `snow.c`, `sky.c`, `clock.c` |
| Music | the background music, ambience, stage music | `sound.c`, `ambience.c` |
| Minigames | fights, the shootouts, the bar counters, chases | `fight.c`, `qte.c`, `shootout.c`, `counter.c`, `ovl0xx.c` |
| Saves | saving and loading the story | `cmcard.c`, `cnrev.c` (story_save) |
| Title | the logo, the title and its menu, the movies, the endings | `opening.c` |
| Pause | the pause | `cui.c` |

## Order of work

1. **Engine basics** (done: the window, the renderer's 2D, the Forth system, the kernel, the console).
2. **The library** (done: every known format converted, `library.md`): the disc picker and import, starting with what the first subsystems need (the
   executable's tables, CD.BIN's containers, textures as PNG, text as UTF-8).
3. Then subsystem by subsystem, each with its C services: clock and story first (they drive everything),
   areas and Matthew (walking in his room), the window and text, townsfolk, then the rest.
