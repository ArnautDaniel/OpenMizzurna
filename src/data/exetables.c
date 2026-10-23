/* The executable's tables (exetables.h). */
#include "exetables.h"

#include <string.h>

#define EXE_HEADER 0x800
#define EXE_LOAD_ADDRESS 0x80010000u

#define MUSIC_TRACKS 19
#define MUSIC_ENTRY 12   /* {s32 CD.BIN entry, u16 0 / 1, u16 0, u8 volume, 3 bytes 0} */

#define AREA_TABLE (34 * 2)   /* an s16 an area */

static const ExeTable kTables[] = {
    {"area-motion-first", 0x80080670, AREA_TABLE,
     "an area's own character motions: the first motion id they answer to (ids from 0xC1 name an area's; src: func_80020464)"},
    {"area-motion-count", 0x800806B4, AREA_TABLE, "an area's own character motions: how many"},
    {"area-height", 0x80080824, 34 * 8,
     "an area's records of 4 s16: [1] the height of its origin (the metadata has x and z; src: area_meta_origin3)"},
    {"screen-effects", 0x80094AC4, 16 * 8,
     "the screen's effects, 8 bytes each: kind (1 fade, 2 / 3 wipe, 4 iris, 5 tint), start level (s8), step a "
     "frame (s8), frames, blend (0x40 subtract, 0x20 add), colours a and b, polygons (src: screen_effect)"},
    {"effect-fade-levels", 0x80094B44, 33, "a fade's level 0..32 as a grey (src: screen_effect_update)"},
    {"effect-iris-sines", 0x80094B68, 16 * 4, "the iris's 16 segments: s32 sines, 0x10000 = 1"},
    {"effect-tint-levels", 0x80094BA7, 5, "the tint's levels from -1 (its start) to 3"},
    {"kind-models", 0x800959C8, 0xC0,
     "a character kind (an actor slot's, by default): its model's CD.BIN entry, less 0x16E (src: npc_model_stream)"},
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
