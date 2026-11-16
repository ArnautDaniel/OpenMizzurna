/* The game's files: paths relative to the library (platform/library.h: "cdbin/033_4/4.txt", "title/logo.png"),
 * looked for first in the layers over it - the mods' library/ folders (docs/modding.md), the last added first -
 * then in the library. Every asset the engine reads comes through here. */
#ifndef FILES_H
#define FILES_H

#include <stddef.h>
#include <stdint.h>

/* the whole file (malloc'd, NUL-terminated past its end), or NULL; its size in *size */
uint8_t *files_read(const char *path, size_t *size);
int files_exist(const char *path);

/* a folder laid out as the library is (a mod's library/) over it */
void files_layer_add(const char *dir);
/* the same inside a zip: its `prefix`library/ (prefix "" or "folder/": a zipped folder) */
struct Zip;
void files_layer_add_zip(const struct Zip *z, const char *prefix);
/* all of the layers taken away */
void files_layers_clear(void);

#endif
