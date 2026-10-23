/* Records for the scripts (engine words; docs/forth.md): the lines of a library text that start with a word, each
 * line's numbers a row (its other words skipped) - "billboard 3 at 120 -40 from -4072 to -5457 ..." the row
 * 3 120 -40 -4072 -5457 ... Read once, kept. */
#include "engine.h"

#include "../core/files.h"

#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define PRIM(name) static void name(Forth *f, Word *w)
#define POP() forth_pop(f)
#define PUSH(x) forth_push(f, (Cell)(x))
#define MAX_TABLES 4096
#define MAX_FIELDS 16

typedef struct Table {
    char key[192];       /* path and word */
    Cell (*rows)[MAX_FIELDS];
    int nrows;
} Table;

static Table sTables[MAX_TABLES];
static int sNTables;

static int is_number(const char *s) {
    if (*s == '-' || *s == '+') {
        s++;
    }
    if (!isdigit((unsigned char)*s)) {
        return 0;
    }
    while (isdigit((unsigned char)*s)) {
        s++;
    }
    return *s == 0;
}

static void read_table(Table *t, const char *path, const char *word) {
    size_t n;
    char *text = (char *)files_read(path, &n), *line, *end;
    int cap = 0;

    if (text == NULL) {
        return;
    }
    for (line = text; line != NULL && *line; line = end != NULL ? end + 1 : NULL) {
        char *tok, *save;
        int k = 0;

        end = strchr(line, '\n');
        if (end != NULL) {
            *end = 0;
        }
        tok = strtok_r(line, " \t\r", &save);
        if (tok == NULL || strcmp(tok, word) != 0) {
            continue;
        }
        if (t->nrows == cap) {
            cap = cap ? cap * 2 : 64;
            t->rows = realloc(t->rows, sizeof(*t->rows) * (size_t)cap);
        }
        memset(t->rows[t->nrows], 0, sizeof(t->rows[0]));
        while ((tok = strtok_r(NULL, " \t\r", &save)) != NULL && k < MAX_FIELDS) {
            if (is_number(tok)) {
                t->rows[t->nrows][k++] = (Cell)strtol(tok, NULL, 10);
            }
        }
        t->nrows++;
    }
    free(text);
}

PRIM(p_records) {   /* ( path len word len -- table ) the file's lines that start with the word; 0: no such file */
    Cell wl = POP();
    const char *ws = (const char *)POP();
    Cell pl = POP();
    const char *ps = (const char *)POP();
    char key[192], path[160], word[32];
    int i;

    snprintf(path, sizeof(path), "%.*s", (int)pl, ps);
    snprintf(word, sizeof(word), "%.*s", (int)wl, ws);
    snprintf(key, sizeof(key), "%s|%s", path, word);
    for (i = 0; i < sNTables; i++) {
        if (strcmp(sTables[i].key, key) == 0) {
            PUSH(sTables[i].rows != NULL ? i + 1 : 0);
            return;
        }
    }
    if (sNTables == MAX_TABLES) {
        forth_error(f, "records: too many tables");
    }
    snprintf(sTables[sNTables].key, sizeof(sTables[sNTables].key), "%s", key);
    read_table(&sTables[sNTables], path, word);
    sNTables++;
    PUSH(sTables[sNTables - 1].rows != NULL ? sNTables : 0);
}
PRIM(p_nrecords) {   /* ( table -- n ) */
    Cell t = POP();
    PUSH(t >= 1 && t <= sNTables ? sTables[t - 1].nrows : 0);
}
PRIM(p_record) {   /* ( table i -- addr ) row i: its numbers, a cell each (0 past them) */
    static Cell none[MAX_FIELDS];
    Cell i = POP(), t = POP();
    PUSH(t >= 1 && t <= sNTables && i >= 0 && i < sTables[t - 1].nrows ? sTables[t - 1].rows[i] : none);
}

void bind_records(Forth *f) {
    forth_prim(f, "records", p_records);
    forth_prim(f, "#records", p_nrecords);
    forth_prim(f, "record", p_record);
}
