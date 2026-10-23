/* The executable's tables (exetables.h). */
#include "exetables.h"

#include <string.h>

#define EXE_HEADER 0x800
#define EXE_LOAD_ADDRESS 0x80010000u

#define MUSIC_TRACKS 19
#define MUSIC_ENTRY 12   /* {s32 CD.BIN entry, u16 0 / 1, u16 0, u8 volume, 3 bytes 0} */

#define AREA_TABLE (34 * 2)   /* an s16 an area */
#define HOURS 24
#define HOUR_RGB (HOURS * 3)       /* u8 red, green, blue tables */
#define HOUR_XYZ (HOURS * 3 * 2)   /* s16 x, y, z tables */

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
    {"actor-radii", 0x8008248C, 182 * 2, "an actor slot's radius (s16, world units; 0: not solid - src: actor_radius)"},
    {"kind-models", 0x800959C8, 0xC0,
     "a character kind (an actor slot's, by default): its model's CD.BIN entry, less 0x16E (src: npc_model_stream)"},
    {"partners", 0x800825F8, 33 * 4,
     "who goes with whom: pairs of s16 (a person's slot, their car's or bike's), ended by -1 (src: actor_partner)"},
    {"car-seats", 0x80082474, 6 * 4,
     "a car's seats: s16 side, forward from the car (world units; turned with it) - where a rider gets out"},
    {"diner-seats", 0x800968DC, 9 * 2,
     "the diner's counter stools: s16 each, along z from the first (src: seat_diner_place: x 0x502D0, z 0x4182A + it)"},
    {"diner-seat-order", 0x80096F74, 9, "the order the townsfolk take its stools in (src: seat_diner_pick_free)"},
    {"diner-seat-approach", 0x80082450, 9 * 4,
     "where one walks to before sitting on each stool: s32 z, x 0x502D0 (src: actor command 0x39)"},
    {"club-seats", 0x800968F0, 6 * 2,
     "Club Wolves' stools: s16 each, along x from the first (src: seat_club_place: x 0x720C1 + it, z 0x46C49)"},
    {"club-seat-order", 0x800A34BC, 6, "the order they take its stools in (src: seat_club_pick_free)"},
    {"lane-steps", 0x800935C0, 32 * 4,
     "a parking node's lanes: s16 x, z between one car's place and the next's (src: car_lane)"},
    /* the day's light (src: clock.c): tables by the hour, 24 entries; the game takes the minute's value between
     * this hour's and the next */
    {"hour-fog", 0x800815E8, HOUR_RGB, "the fog's colour by the hour: 24 red, 24 green, 24 blue (u8)"},
    {"hour-ambient", 0x80081630, HOUR_RGB, "the ambient light by the hour: 24 red, 24 green, 24 blue"},
    {"hour-light-1", 0x80081678, HOUR_RGB, "the first of the three lights' colour by the hour: red, green, blue"},
    {"hour-light-1-way", 0x800816C0, HOUR_XYZ, "its direction by the hour: 24 x, 24 y, 24 z (s16; y downward)"},
    {"hour-light-2", 0x80081750, HOUR_RGB, "the second light's colour"},
    {"hour-light-2-way", 0x80081798, HOUR_XYZ, "its direction"},
    {"hour-light-3", 0x80081828, HOUR_RGB, "the third light's colour"},
    {"hour-light-3-way", 0x80081870, HOUR_XYZ, "its direction"},
    {"hour-sky-top", 0x80081900, HOUR_RGB, "the sky's colour above by the hour (src: func_800248C0)"},
    {"hour-sky-low", 0x80081948, HOUR_RGB, "and at the horizon"},
    {"hour-clouds", 0x80081990, HOURS, "the clouds' shade by the hour (u8; src: func_80024E48)"},
    {"hour-horizon", 0x800819A8, HOURS, "the horizon strip's shade by the hour (src: func_80024FAC)"},
    {"sun-heights", 0x80080798, HOURS * 2, "the sun's height on the screen by the hour (s16; src: func_8001F670)"},
    {"moon-heights", 0x800807F4, HOURS * 2, "the moon's, by its own hour (src: func_8001F9B0)"},
    {"flare-distances", 0x800807C8, 14 * 2, "the sun's 14 lens flares: how far each is along the line through the centre"},
    {"flare-sizes", 0x800807E4, 14, "and its half size (u8)"},
    {"stars-x", 0x800806F8, 20 * 4, "the 20 stars: where across (s32; a 320-pixel repeat; src: func_8001F000)"},
    {"stars-y", 0x80080748, 20 * 4, "and how high above the horizon's top"},
    {"snow-wobble", 0x8009583C, 32 * 2, "a snowflake's sway: 32 steps of s8 x, z (src: the flakes' loop)"},
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
