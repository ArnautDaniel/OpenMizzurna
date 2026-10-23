/* The executable's tables (exetables.h). */
#include "exetables.h"

#include <string.h>

#define EXE_HEADER 0x800
#define EXE_LOAD_ADDRESS 0x80010000u

#define MUSIC_TRACKS 19
#define MUSIC_ENTRY 12   /* {s32 CD.BIN entry, u16 0 / 1, u16 0, u8 volume, 3 bytes 0} */

static const ExeTable kTables[] = {
    {"music", 0x800814DC, MUSIC_TRACKS * MUSIC_ENTRY,
     "the background music: per track its sequence's CD.BIN entry, then 0 or 1, and its volume (src: music_play)"},
};
#define NTABLES (int)(sizeof(kTables) / sizeof(kTables[0]))

int exetable_count(void) { return NTABLES; }
const ExeTable *exetable_at(int i) { return i >= 0 && i < NTABLES ? &kTables[i] : NULL; }

const ExeTable *exetable_named(const char *name) {
    int i;

    for (i = 0; i < NTABLES; i++) {
        if (strcmp(kTables[i].name, name) == 0) {
            return &kTables[i];
        }
    }
    return NULL;
}

const uint8_t *exetable_bytes(const ExeTable *t, const uint8_t *exe, size_t exe_size) {
    size_t at = EXE_HEADER + (t->address - EXE_LOAD_ADDRESS);

    return at + t->size <= exe_size ? exe + at : NULL;
}
