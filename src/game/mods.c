/* Mods (mods.h, docs/modding.md).
 *
 *   mods/order.txt        on: <name> / off: <name>, a line each, in load order (the title's mods menu writes it);
 *                         a mod it doesn't name is on, after those it does, by name
 *   mods/<folder>/mod.fs  mod: <name>  version: <major> <minor>   (or mods/<name>.zip: mod.fs at its top, or in the
 *                         one folder it holds)
 *                         requires: openmizzurna <major> <minor> (the mod API) / <mod> <major> <minor>
 *                         after: <mod> (loaded after it, if it's on)        \ comments, ( comments )
 *   mods/<folder>/library/...         files over the library's, at the library's paths
 *   mods/<folder>/scripts/<name>.fs   its Forth: the vocabulary <name>, loaded after the game's scripts
 */
#include "mods.h"

#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <strings.h>

#include <SDL3/SDL.h>

#include "../core/files.h"
#include "../core/zip.h"

#define MODS 64                    /* mods at most (found in mods/ and named by --mod) */
#define NEEDS 8                    /* a mod's requires: and after: lines at most, each */
#define MANIFEST_WORDS 8           /* the words of a manifest's line read at most */
#define ORDER_WORDS 4
#define MOD_LOAD_STEPS 50000000L   /* the steps a mod's scripts may take loading: far more than any does ("ran too long") */
#define TURNED_OFF "turned off"    /* the reason order.txt's off: gives */
#define MANIFEST "mod.fs"

typedef struct Need {
    char name[64];
    int major, minor;
} Need;

typedef struct Mod {
    char name[64], dir[1024];   /* (a zip: the .zip's path) */
    Zip *zip;
    char prefix[256];           /* in the zip: where mod.fs is ("" or "folder/") */
    int major, minor;
    Need requires[NEEDS], after[NEEDS];
    int nrequires, nafter;
    int on, listed;     /* listed: its place in order.txt (+1), 0 not there */
    int named;          /* given on the command line (--mod): on for this run, not in order.txt */
    char why[160];      /* why it is off */
} Mod;

static Mod sMods[MODS];
static int sN;
static char sFolder[1024];   /* mods/ (empty: none - the tests) */

/* the mods menu's list (the title's MODS): the folder's mods in the order order.txt will have, each on or off as
 * the player set it - applied when OpenMizzurna next starts */
typedef struct MenuMod {
    char name[64], why[160];
    int on, was_on;
} MenuMod;
static MenuMod sMenu[MODS];
static int sNMenu = -1;   /* (-1: not made yet this run) */

/* the engine's own name in a manifest (`requires: openmizzurna 1 0`) */
static int is_engine(const char *s) { return strcasecmp(s, "openmizzurna") == 0; }

/* names a mod's vocabulary can't have besides the game's scripts' (the language's and the engine's own) */
static int is_reserved(const char *s) {
    static const char *kReserved[] = {"forth", "engine", "actors", "tester", "mods"};
    size_t i;

    for (i = 0; i < sizeof(kReserved) / sizeof(kReserved[0]); i++) {
        if (strcasecmp(s, kReserved[i]) == 0) {
            return 1;
        }
    }
    return is_engine(s);
}

static int good_name(const char *s) {   /* letters, digits, '-' (a vocabulary and a folder name) */
    if (*s == 0) {
        return 0;
    }
    for (; *s != 0; s++) {
        if (!isalnum((unsigned char)*s) && *s != '-') {
            return 0;
        }
    }
    return 1;
}

/* the words of a line, comments (\ to its end, ( ... )) left out */
static int words(char *line, char **w, int max) {
    int n = 0, paren = 0;
    char *save = NULL, *t;

    for (t = strtok_r(line, " \t\r\n", &save); t != NULL && n < max; t = strtok_r(NULL, " \t\r\n", &save)) {
        if (paren) {
            paren = strchr(t, ')') == NULL;
        } else if (strcmp(t, "\\") == 0) {
            break;
        } else if (strcmp(t, "(") == 0) {
            paren = 1;
        } else {
            w[n++] = t;
        }
    }
    return n;
}

static void need(Need *list, int *n, char **w, int nw) {
    if (*n < NEEDS && nw >= 2) {
        snprintf(list[*n].name, sizeof(list[0].name), "%s", w[1]);
        list[*n].major = nw > 2 ? atoi(w[2]) : 0;
        list[*n].minor = nw > 3 ? atoi(w[3]) : 0;
        (*n)++;
    }
}

/* a file of the mod's (`rel` from where its mod.fs is), whole and NUL-terminated (free it), or NULL */
static char *mod_read(const Mod *m, const char *rel, size_t *size) {
    char path[1400];
    void *data;

    if (m->zip != NULL) {
        snprintf(path, sizeof(path), "%s%s", m->prefix, rel);
        return (char *)zip_read(m->zip, zip_find(m->zip, path), size);
    }
    snprintf(path, sizeof(path), "%s/%s", m->dir, rel);
    data = SDL_LoadFile(path, size);   /* (NUL-terminated past its end) */
    if (data != NULL) {
        char *copy = malloc(*size + 1);

        if (copy != NULL) {
            memcpy(copy, data, *size + 1);
        }
        SDL_free(data);
        return copy;
    }
    return NULL;
}

static int is_zip(const char *path) {
    size_t len = strlen(path);

    return len > 4 && strcasecmp(path + len - 4, ".zip") == 0;
}

/* where a zip's mod.fs is: at its top, else in a folder (the shortest such path) */
static int zip_prefix(const Zip *z, char *prefix, size_t n) {
    int i, best = -1;

    for (i = 0; i < zip_count(z); i++) {
        const char *name = zip_name(z, i), *slash = strrchr(name, '/');

        if (strcasecmp(slash != NULL ? slash + 1 : name, MANIFEST) == 0 &&
            (best < 0 || strlen(name) < strlen(zip_name(z, best)))) {
            best = i;
        }
    }
    if (best < 0) {
        return 0;
    }
    snprintf(prefix, n, "%.*s", (int)(strlen(zip_name(z, best)) - strlen(MANIFEST)), zip_name(z, best));
    return 1;
}

static void read_manifest(Mod *m, char *text) {
    char *w[MANIFEST_WORDS], *line, *save = NULL;

    for (line = strtok_r(text, "\n", &save); line != NULL; line = strtok_r(NULL, "\n", &save)) {
        int n = words(line, w, MANIFEST_WORDS);

        if (n == 0) {
            continue;
        }
        if (n >= 2 && strcmp(w[0], "mod:") == 0) {
            snprintf(m->name, sizeof(m->name), "%s", w[1]);
        } else if (n >= 2 && strcmp(w[0], "version:") == 0) {
            m->major = atoi(w[1]);
            m->minor = n > 2 ? atoi(w[2]) : 0;
        } else if (strcmp(w[0], "requires:") == 0) {
            need(m->requires, &m->nrequires, w, n);
        } else if (strcmp(w[0], "after:") == 0) {
            need(m->after, &m->nafter, w, n);
        } else {
            fprintf(stderr, "mods: %s's mod.fs: \"%s\" isn't a word mod.fs knows\n", m->dir, w[0]);
        }
    }
}

static void add_mod(const char *dir) {   /* (a folder - no "/" at its end - or a .zip) */
    const char *base = strrchr(dir, '/');
    char *text;
    Mod *m;
    size_t size;
    int i;

    if (sN == MODS) {
        fprintf(stderr, "mods: more than %d - %s left out\n", MODS, dir);
        return;
    }
    m = &sMods[sN];
    memset(m, 0, sizeof(*m));
    snprintf(m->dir, sizeof(m->dir), "%s", dir);
    snprintf(m->name, sizeof(m->name), "%s", base != NULL ? base + 1 : dir);
    if (is_zip(dir)) {
        m->name[strlen(m->name) - strlen(".zip")] = 0;
        if ((m->zip = zip_open(dir)) == NULL) {
            return;
        }
        if (!zip_prefix(m->zip, m->prefix, sizeof(m->prefix))) {
            fprintf(stderr, "mods: %s has no mod.fs - not a mod\n", dir);
            zip_close(m->zip);
            return;
        }
    }
    if ((text = mod_read(m, MANIFEST, &size)) == NULL) {
        zip_close(m->zip);
        return;   /* (not a mod: no manifest) */
    }
    m->on = 1;
    read_manifest(m, text);
    free(text);
    if (!good_name(m->name) || is_reserved(m->name)) {
        fprintf(stderr, "mods: %s: its name \"%s\" can't be a mod's (letters, digits and -, not the engine's)\n", dir,
                m->name);
        zip_close(m->zip);
        return;
    }
    for (i = 0; i < sN; i++) {
        if (strcasecmp(sMods[i].name, m->name) == 0) {
            fprintf(stderr, "mods: %s and %s are both \"%s\" - the first is used\n", sMods[i].dir, dir, m->name);
            zip_close(m->zip);
            return;
        }
    }
    sN++;
}

static Mod *named(const char *name) {
    int i;

    for (i = 0; i < sN; i++) {
        if (strcasecmp(sMods[i].name, name) == 0) {
            return &sMods[i];
        }
    }
    return NULL;
}

static SDL_EnumerationResult folder_one(void *user, const char *dir, const char *name) {
    char path[1100];
    SDL_PathInfo info;

    (void)user;
    snprintf(path, sizeof(path), "%s%s", dir, name);
    if (name[0] != '.' && SDL_GetPathInfo(path, &info) &&
        (info.type == SDL_PATHTYPE_DIRECTORY || (info.type == SDL_PATHTYPE_FILE && is_zip(name)))) {
        add_mod(path);
    }
    return SDL_ENUM_CONTINUE;
}

static int by_name(const void *a, const void *b) { return strcasecmp(((const Mod *)a)->name, ((const Mod *)b)->name); }
static int by_listed(const void *a, const void *b) {   /* (those order.txt names first, in its order) */
    const Mod *x = a, *y = b;

    if ((x->listed != 0) != (y->listed != 0)) {
        return x->listed != 0 ? -1 : 1;
    }
    return x->listed != y->listed ? x->listed - y->listed : strcasecmp(x->name, y->name);
}

static void read_order(const char *folder) {
    char path[1100], line[512], *w[ORDER_WORDS];
    FILE *f;
    int k = 0;

    snprintf(path, sizeof(path), "%sorder.txt", folder);
    if ((f = fopen(path, "r")) == NULL) {
        return;
    }
    while (fgets(line, sizeof(line), f) != NULL) {
        int n = words(line, w, ORDER_WORDS);
        Mod *m = n >= 2 ? named(w[1]) : NULL;

        if (m != NULL && strcmp(w[0], "on:") == 0) {
            m->listed = ++k;
        } else if (m != NULL && strcmp(w[0], "off:") == 0) {
            m->listed = ++k;
            m->on = 0;
            snprintf(m->why, sizeof(m->why), TURNED_OFF);
        }
    }
    fclose(f);
}

/* off: those whose needs aren't met (a mod off can turn off those needing it - until none) */
static void check_needs(void) {
    int changed = 1, i, k;

    for (i = 0; i < sN; i++) {   /* (the mod API first: a mod off for it is off for those needing it) */
        Mod *m = &sMods[i];

        for (k = 0; k < m->nrequires && m->on; k++) {
            const Need *r = &m->requires[k];

            if (is_engine(r->name) && (r->major != MODS_API_MAJOR || r->minor > MODS_API_MINOR)) {
                snprintf(m->why, sizeof(m->why), "it needs OpenMizzurna's mod API %d.%d (this OpenMizzurna has %d.%d)",
                         r->major, r->minor, MODS_API_MAJOR, MODS_API_MINOR);
                m->on = 0;
            }
        }
    }
    while (changed) {
        changed = 0;
        for (i = 0; i < sN; i++) {
            Mod *m = &sMods[i];

            for (k = 0; k < m->nrequires && m->on; k++) {
                const Need need = m->requires[k], *r = &need;   /* (a copy: its name goes into the same Mod's why) */
                Mod *o;

                if (is_engine(r->name)) {
                    continue;
                }
                if ((o = named(r->name)) == NULL || !o->on) {
                    snprintf(m->why, sizeof(m->why), "it needs the mod %s, %s", r->name, o == NULL ? "not there" : "off");
                    m->on = 0;
                } else if (o->major < r->major || (o->major == r->major && o->minor < r->minor)) {
                    snprintf(m->why, sizeof(m->why), "it needs %s %d.%d or newer (there is %d.%d)", r->name, r->major,
                             r->minor, o->major, o->minor);
                    m->on = 0;
                }
                changed |= !m->on;
            }
        }
    }
}

/* each mod after those it names in after: (moved down until it is - a circle is left as it is) */
static void order_after(void) {
    int pass, moved = 1, i, k, j;

    for (pass = 0; moved && pass < sN * sN + 1; pass++) {
        moved = 0;
        for (i = 0; i < sN && !moved; i++) {
            for (k = 0; k < sMods[i].nafter && !moved; k++) {
                for (j = i + 1; j < sN; j++) {
                    if (strcasecmp(sMods[j].name, sMods[i].after[k].name) == 0) {
                        Mod m = sMods[i];

                        memmove(&sMods[i], &sMods[i + 1], sizeof(Mod) * (size_t)(j - i));
                        sMods[j] = m;
                        moved = 1;
                        break;
                    }
                }
            }
        }
    }
    if (moved) {
        fprintf(stderr, "mods: their after: lines go round in a circle; the order is left as it was\n");
    }
}

static void layer_files(void) {   /* (the mods on: their library/ over the library, in load order) */
    char dir[1100];
    int i;

    files_layers_clear();
    for (i = 0; i < sN; i++) {
        snprintf(dir, sizeof(dir), "%s/library", sMods[i].dir);
        if (sMods[i].on && sMods[i].zip != NULL) {
            files_layer_add_zip(sMods[i].zip, sMods[i].prefix);
        } else if (sMods[i].on && SDL_GetPathInfo(dir, NULL)) {
            files_layer_add(dir);
        }
    }
}

static void turn_off(Mod *m, const char *why) {
    snprintf(m->why, sizeof(m->why), "%s", why);
    fprintf(stderr, "mods: %s is off: %s\n", m->name, m->why);
    m->on = 0;
    layer_files();
}

void mods_find(const char *folder, const char *const *names, int nnamed) {
    char dir[1100];
    int i, before;

    for (i = 0; i < sN; i++) {
        zip_close(sMods[i].zip);
    }
    files_layers_clear();
    sN = 0;
    if (folder != NULL) {
        SDL_EnumerateDirectory(folder, folder_one, NULL);
        qsort(sMods, (size_t)sN, sizeof(Mod), by_name);
        read_order(folder);
        qsort(sMods, (size_t)sN, sizeof(Mod), by_listed);
    }
    snprintf(sFolder, sizeof(sFolder), "%s", folder != NULL ? folder : "");
    sNMenu = -1;
    for (i = 0; i < nnamed; i++) {   /* (--mod: on, after the folder's) */
        size_t len;

        snprintf(dir, sizeof(dir), "%s", names[i]);
        len = strlen(dir);
        while (len > 1 && dir[len - 1] == '/') {
            dir[--len] = 0;
        }
        before = sN;
        add_mod(dir);
        if (sN > before) {
            sMods[sN - 1].named = 1;
        } else {
            fprintf(stderr, "mods: %s isn't a mod (no mod.fs in it)\n", names[i]);
        }
    }
    check_needs();
    order_after();
    for (i = 0; i < sN; i++) {
        if (!sMods[i].on) {
            fprintf(stderr, "mods: %s is off: %s\n", sMods[i].name, sMods[i].why);
        }
    }
    layer_files();
}

/* Forth's reader (forth_set_reader): a zipped mod's scripts, "<the .zip>!/<path in it>" */
static char *zip_reader(void *ctx, const char *path, size_t *len) {
    int i;

    (void)ctx;
    for (i = 0; i < sN; i++) {
        size_t n = strlen(sMods[i].dir);

        if (sMods[i].zip != NULL && strncmp(path, sMods[i].dir, n) == 0 && strncmp(path + n, "!/", 2) == 0) {
            return (char *)zip_read(sMods[i].zip, zip_find(sMods[i].zip, path + n + 2), len);
        }
    }
    return NULL;
}

/* the name is one of the game's vocabularies: made by its scripts so far, or a file or folder of them (one not
 * loaded yet) */
static int game_vocabulary(Forth *f, const char *game_scripts, const char *name) {
    char path[1300];

    snprintf(path, sizeof(path), "%s/%s.fs", game_scripts, name);
    if (forth_find_vocab(f, name) != NULL || SDL_GetPathInfo(path, NULL)) {
        return 1;
    }
    snprintf(path, sizeof(path), "%s/%s", game_scripts, name);
    return SDL_GetPathInfo(path, NULL);
}

void mods_load_scripts(Forth *f, const char *game_scripts) {
    char path[1300], line[200], *text;
    size_t size;
    int i, warned = 0, failed;

    forth_set_reader(f, zip_reader, NULL);
    for (i = 0; i < sN; i++) {
        Mod *m = &sMods[i];

        if (!m->on) {
            continue;
        }
        snprintf(path, sizeof(path), "scripts/%s.fs", m->name);
        if ((text = mod_read(m, path, &size)) == NULL) {
            fprintf(stderr, "mods: %s %d.%d on (files only)\n", m->name, m->major, m->minor);
            continue;
        }
        free(text);
        if (game_vocabulary(f, game_scripts, m->name)) {
            turn_off(m, "its name is one of the game's vocabularies");
            continue;
        }
        if (!warned) {
            fprintf(stderr, "mods: a mod's scripts can do anything the game's can - load only mods you trust\n");
            warned = 1;
        }
        if (m->zip != NULL) {
            snprintf(path, sizeof(path), "%s!/%sscripts", m->dir, m->prefix);
        } else {
            snprintf(path, sizeof(path), "%s/scripts", m->dir);
        }
        forth_add_root(f, path);
        snprintf(line, sizeof(line), "USING: %s ;", m->name);   /* (as any vocabulary: from its file) */
        forth_set_eval_budget(f, MOD_LOAD_STEPS);
        failed = forth_eval(f, line, strlen(line), m->name) != 0;
        forth_set_eval_budget(f, -1);
        if (failed) {
            turn_off(m, "its scripts failed to load (what it made before the error stays)");
            continue;
        }
        fprintf(stderr, "mods: %s %d.%d on\n", m->name, m->major, m->minor);
    }
}

/* ---- the console's words ---- */

static void p_mods(Forth *f, Word *w) {   /* ( -- ) the mods, in load order: on or off and why */
    int i;

    (void)w;
    if (sN == 0) {
        forth_printf(f, "no mods\n");
    }
    for (i = 0; i < sN; i++) {
        forth_printf(f, "%-24s %d.%-4d %s%s\n", sMods[i].name, sMods[i].major, sMods[i].minor,
                     sMods[i].on ? "on" : "off: ", sMods[i].on ? "" : sMods[i].why);
    }
}

static void p_mod_on(Forth *f, Word *w) {   /* ( addr len -- flag ) is the mod of that name on? */
    Cell n = forth_pop(f), a = forth_pop(f);
    char name[64];
    Mod *m;

    (void)w;
    snprintf(name, sizeof(name), "%.*s", (int)n, (const char *)a);
    m = named(name);
    forth_push(f, m != NULL && m->on ? -1 : 0);
}

/* ---- the mods menu ---- */

static void menu_init(void) {
    int i;

    if (sNMenu >= 0) {
        return;
    }
    sNMenu = 0;
    for (i = 0; i < sN; i++) {
        MenuMod *mm;
        int turned_off = strcmp(sMods[i].why, TURNED_OFF) == 0;

        if (sMods[i].named) {
            continue;
        }
        mm = &sMenu[sNMenu++];
        snprintf(mm->name, sizeof(mm->name), "%s", sMods[i].name);
        snprintf(mm->why, sizeof(mm->why), "%s", sMods[i].on || turned_off ? "" : sMods[i].why);
        mm->on = mm->was_on = sMods[i].on || !turned_off;   /* (off for a reason of its own: still asked for) */
    }
}

static int menu_index(Forth *f) {
    Cell i = forth_pop(f);

    menu_init();
    if (i < 0 || i >= sNMenu) {
        forth_error(f, "mods menu: no mod %ld (there are %d)", (long)i, sNMenu);
    }
    return (int)i;
}

static void p_menu_count(Forth *f, Word *w) {   /* ( -- n ) the folder's mods (the menu's rows) */
    (void)w;
    menu_init();
    forth_push(f, sNMenu);
}

static void p_menu_mod(Forth *f, Word *w) {   /* ( i -- addr len on? ) a row: its name, asked on */
    int i = menu_index(f);

    (void)w;
    forth_push(f, (Cell)sMenu[i].name);
    forth_push(f, (Cell)strlen(sMenu[i].name));
    forth_push(f, sMenu[i].on ? -1 : 0);
}

static void p_menu_why(Forth *f, Word *w) {   /* ( i -- addr len ) why it didn't load this run ("": it did, or turned off) */
    int i = menu_index(f);

    (void)w;
    forth_push(f, (Cell)sMenu[i].why);
    forth_push(f, (Cell)strlen(sMenu[i].why));
}

static void p_menu_switch(Forth *f, Word *w) {   /* ( i -- ) on <-> off */
    int i = menu_index(f);

    (void)w;
    sMenu[i].on = !sMenu[i].on;
}

static void p_menu_move(Forth *f, Word *w) {   /* ( i delta -- i' ) moved up (-1) or down (1) the load order */
    Cell delta = forth_pop(f);
    int i = menu_index(f), j = i + (delta < 0 ? -1 : 1);

    (void)w;
    if (j >= 0 && j < sNMenu) {
        MenuMod t = sMenu[i];

        sMenu[i] = sMenu[j];
        sMenu[j] = t;
        i = j;
    }
    forth_push(f, i);
}

static void p_menu_changed(Forth *f, Word *w) {   /* ( -- flag ) the menu differs from this run's */
    int i, k = 0, changed = 0;

    (void)w;
    menu_init();
    for (i = 0; i < sN && !changed; i++) {
        if (sMods[i].named) {
            continue;
        }
        changed = strcmp(sMods[i].name, sMenu[k].name) != 0 || sMenu[k].on != sMenu[k].was_on;
        k++;
    }
    forth_push(f, changed ? -1 : 0);
}

static void p_menu_save(Forth *f, Word *w) {   /* ( -- flag ) mods/order.txt written from the menu */
    char path[1100];
    FILE *o;
    int i, ok;

    (void)w;
    menu_init();
    snprintf(path, sizeof(path), "%sorder.txt", sFolder);
    if (sFolder[0] == 0 || (o = fopen(path, "w")) == NULL) {
        forth_push(f, 0);
        return;
    }
    fprintf(o, "\\ the mods' load order, each on or off (written by OpenMizzurna's mods menu; docs/modding.md)\n");
    for (i = 0; i < sNMenu; i++) {
        fprintf(o, "%s %s\n", sMenu[i].on ? "on:" : "off:", sMenu[i].name);
    }
    ok = fclose(o) == 0;
    forth_push(f, ok ? -1 : 0);
}

void bind_mods(Forth *f) {
    Vocab *was = forth_current(f);

    forth_set_current(f, forth_vocab(f, "engine"));
    forth_set_effect(f, forth_prim(f, "mods-menu-count", p_menu_count), "( -- n )");
    forth_set_effect(f, forth_prim(f, "mods-menu-mod", p_menu_mod), "( i -- addr len on? )");
    forth_set_effect(f, forth_prim(f, "mods-menu-why", p_menu_why), "( i -- addr len )");
    forth_set_effect(f, forth_prim(f, "mods-menu-switch", p_menu_switch), "( i -- )");
    forth_set_effect(f, forth_prim(f, "mods-menu-move", p_menu_move), "( i delta -- i' )");
    forth_set_effect(f, forth_prim(f, "mods-menu-changed?", p_menu_changed), "( -- flag )");
    forth_set_effect(f, forth_prim(f, "mods-menu-save", p_menu_save), "( -- flag )");
    forth_set_effect(f, forth_prim(f, ".mods", p_mods), "( -- )");
    forth_set_effect(f, forth_prim(f, "mod-on?", p_mod_on), "( addr len -- flag )");
    forth_constant(f, "mod-api", MODS_API_MAJOR * 100 + MODS_API_MINOR);
    forth_set_current(f, was);
}
