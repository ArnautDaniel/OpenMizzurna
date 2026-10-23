/* The game's files, from the library (platform/library.h): paths relative to it ("cdbin/033_4.bin",
 * "disc/MAP.BIN"). Mods will layer over it here. */
#ifndef FILES_H
#define FILES_H

#include <stddef.h>
#include <stdint.h>

/* the whole file (malloc'd, NUL-terminated past its end), or NULL; its size in *size */
uint8_t *files_read(const char *path, size_t *size);
int files_exist(const char *path);

#endif
