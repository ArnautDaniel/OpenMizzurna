/* Mods (docs/modding.md): folders (or zips of one) in <home>/mods/, each with a manifest (mod.fs), files over the
 * library's (library/) and scripts loaded after the game's, in a vocabulary of the mod's own (scripts/<name>.fs).
 * OpenHuey's mod loader, adapted. */
#ifndef MODS_H
#define MODS_H

#include "forth.h"

/* the mod API: the game's messages, broadcasts and hooks a mod may use (a mod's `requires: openmizzurna 1 0`).
 * A new major breaks mods (a message renamed or gone); the minor grows with additions (a hook, a message). */
#define MODS_API_MAJOR 1
#define MODS_API_MINOR 1

/* the mods turned on, decided at start-up: those in `folder` (NULL: none - the tests) as its order.txt says,
 * then the folders or zips `named` (--mod); their files layered over the library (core/files.h) */
void mods_find(const char *folder, const char *const *named, int nnamed);
/* each mod's scripts, after the game's (game.fs); `game_scripts` the game's scripts' folder (a mod may not take
 * the name of one of its vocabularies) */
void mods_load_scripts(Forth *f, const char *game_scripts);
/* the console's `.mods`, `mod-api`, `mod-on?` and the title's mods menu (scripts/mods/menu.fs): in `engine` */
void bind_mods(Forth *f);

#endif
