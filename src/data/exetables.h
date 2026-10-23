/* The tables the game reads out of its executable (SLPS_017.83), by name. The import writes each to
 * library/exe/<name>.bin - the table's bytes as the executable has them - and the engine asks for them by name
 * (exe-table), never by address. A subsystem adds its tables here as it is built, each with what it holds; a
 * readable form (text a mod edits) replaces a table's bytes when the subsystem wants one. */
#ifndef EXETABLES_H
#define EXETABLES_H

#include <stddef.h>
#include <stdint.h>

typedef struct ExeTable {
    const char *name;
    uint32_t address;   /* in the running program (the executable loads at 0x80010000) */
    uint32_t size;      /* bytes */
    const char *what;
} ExeTable;

int exetable_count(void);
const ExeTable *exetable_at(int i);
const ExeTable *exetable_named(const char *name);
/* the table's bytes in an executable (a PS-X EXE); NULL if the executable is too short */
const uint8_t *exetable_bytes(const ExeTable *t, const uint8_t *exe, size_t exe_size);

#endif
