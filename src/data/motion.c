/* The game's motions (motion.h). */
#include "motion.h"

#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* a character's packed motion, in u16 words: [0] the header's offset (frames' table at 4), [3] the root moves'
 * offset; the header: parts, frames; frame f's stream at word [4 + f]; in a stream, per part and axis either the
 * value (s16) or a hold: (s16)w >> 12 == 7 keeps the value (w & 0xFF) frames, this one included */
#define HOLD_TAG 7
#define MAX_PARTS 64
#define MAX_FRAMES 4096
/* a prop's: [0] the header's offset {stride, frames}, [3] the places' offset (3 a frame), [4] the turns' (stride x 3
 * a frame) */

static uint16_t w16(const uint8_t *d, size_t i) { return (uint16_t)(d[i * 2] | d[i * 2 + 1] << 8); }

void motion_free(Motion *m) {
    free(m->move);
    free(m->turns);
    memset(m, 0, sizeof(*m));
}

static int alloc(Motion *m) {
    if (m->parts <= 0 || m->parts > MAX_PARTS || m->frames <= 0 || m->frames > MAX_FRAMES) {
        return 0;
    }
    m->move = calloc((size_t)m->frames, sizeof(*m->move));
    m->turns = calloc((size_t)m->frames * m->parts * 3, sizeof(int16_t));
    return 1;
}

int motion_decode(Motion *m, const uint8_t *d, size_t n, int prop) {
    size_t words = n / 2, head;
    int f, p, k;

    memset(m, 0, sizeof(*m));
    m->prop = prop;
    if (words < 5) {
        return 0;
    }
    head = w16(d, 0);
    if (head + 2 > words) {
        return 0;
    }
    if (prop) {
        size_t places = w16(d, 3), turns = w16(d, 4);

        m->parts = w16(d, head);
        m->frames = w16(d, head + 1);
        if (!alloc(m) || places + (size_t)m->frames * 3 > words || turns + (size_t)m->frames * m->parts * 3 > words) {
            motion_free(m);
            return 0;
        }
        for (f = 0; f < m->frames; f++) {
            for (k = 0; k < 3; k++) {
                m->move[f][k] = (int16_t)w16(d, places + (size_t)f * 3 + k);
            }
            for (p = 0; p < m->parts * 3; p++) {
                m->turns[(size_t)f * m->parts * 3 + p] = (int16_t)w16(d, turns + ((size_t)f * m->parts * 3) + p);
            }
        }
        return 1;
    } else {
        size_t root = w16(d, 3);
        int hold[MAX_PARTS][3], value[MAX_PARTS][3];

        m->parts = w16(d, head);
        m->frames = w16(d, head + 1);
        if (head != 4 + (size_t)m->frames || !alloc(m) || root + (size_t)m->frames * 3 > words) {
            motion_free(m);
            return 0;
        }
        memset(hold, 0, sizeof(hold));
        memset(value, 0, sizeof(value));
        for (f = 0; f < m->frames; f++) {
            size_t at = w16(d, 4 + (size_t)f);

            for (k = 0; k < 3; k++) {
                m->move[f][k] = (int16_t)w16(d, root + (size_t)f * 3 + k);
            }
            for (p = 0; p < m->parts; p++) {
                for (k = 0; k < 3; k++) {
                    if (f == 0) {
                        hold[p][k] = 0;
                    }
                    if (hold[p][k] > 0) {
                        hold[p][k]--;
                    } else {
                        int16_t v;

                        if (at >= words) {
                            motion_free(m);
                            return 0;
                        }
                        v = (int16_t)w16(d, at++);
                        if (v >> 12 == HOLD_TAG) {
                            hold[p][k] = (v & 0xFF) - 1;
                        } else {
                            value[p][k] = v;
                        }
                    }
                    m->turns[((size_t)f * m->parts + p) * 3 + k] = (int16_t)value[p][k];
                }
            }
        }
        return 1;
    }
}

typedef struct Out {
    char *s;
    size_t n, cap;
} Out;

static void outf(Out *o, const char *fmt, ...) __attribute__((format(printf, 2, 3)));
static void outf(Out *o, const char *fmt, ...) {
    va_list ap;
    int k;

    for (;;) {
        va_start(ap, fmt);
        k = vsnprintf(o->s + o->n, o->cap - o->n, fmt, ap);
        va_end(ap);
        if (k >= 0 && o->n + (size_t)k < o->cap) {
            o->n += (size_t)k;
            return;
        }
        o->cap = o->cap * 2 + (size_t)k + 4096;
        o->s = realloc(o->s, o->cap);
    }
}

char *motion_text(const Motion *m, const char *comment, size_t *len) {
    Out o = {NULL, 0, 0};
    int f, p;

    outf(&o, "\\ %s\n", comment);
    if (m->prop) {
        outf(&o, "om-prop-motion 1\nframes %d\nstride %d\n", m->frames, m->parts);
        outf(&o, "\\ frame <i> place <side> <height> <forward> turns <rx ry rz> ... (4096 a full turn)\n");
    } else {
        outf(&o, "om-motion 1\nparts %d\nframes %d\n", m->parts, m->frames);
        outf(&o, "\\ frame <i> move <dx> <dy up> <dz> turns <rx ry rz> a part ... (4096 a full turn)\n");
    }
    for (f = 0; f < m->frames; f++) {
        outf(&o, "frame %d %s %d %d %d turns", f, m->prop ? "place" : "move", m->move[f][0], m->move[f][1], m->move[f][2]);
        for (p = 0; p < m->parts; p++) {
            const int16_t *t = m->turns + ((size_t)f * m->parts + p) * 3;
            outf(&o, "  %d %d %d", t[0], t[1], t[2]);
        }
        outf(&o, "\n");
    }
    *len = o.n;
    return o.s;
}

int motion_from_text(Motion *m, const char *s, size_t n, char *why, size_t nwhy) {
    char *copy = malloc(n + 1), *line, *next;
    int got_format = 0, f = 0;

    memcpy(copy, s, n);
    copy[n] = 0;
    memset(m, 0, sizeof(*m));
    for (line = copy; line != NULL; line = next) {
        next = strchr(line, '\n');
        if (next != NULL) {
            *next++ = 0;
        }
        if (line[0] == '\\' || line[0] == 0) {
            continue;
        }
        if (strcmp(line, "om-motion 1") == 0 || strcmp(line, "om-prop-motion 1") == 0) {
            got_format = 1;
            m->prop = line[3] == 'p';
        } else if (sscanf(line, "parts %d", &m->parts) == 1 || sscanf(line, "stride %d", &m->parts) == 1 ||
                   sscanf(line, "frames %d", &m->frames) == 1) {
        } else if (strncmp(line, "frame ", 6) == 0) {
            char *at = line + 6, *end;
            int i, p, k;

            if (m->move == NULL && !alloc(m)) {
                snprintf(why, nwhy, "a motion's parts and frames come before its frames");
                free(copy);
                return 0;
            }
            i = (int)strtol(at, &end, 10);
            if (i != f || f >= m->frames) {
                snprintf(why, nwhy, "frame %d where frame %d was due", i, f);
                free(copy);
                motion_free(m);
                return 0;
            }
            at = end;
            at += strspn(at, " ");
            at += strcspn(at, " ");   /* (place / move) */
            for (k = 0; k < 3; k++) {
                m->move[f][k] = (int16_t)strtol(at, &end, 10);
                at = end;
            }
            at += strspn(at, " ");
            at += strcspn(at, " ");   /* (turns) */
            for (p = 0; p < m->parts * 3; p++) {
                m->turns[(size_t)f * m->parts * 3 + p] = (int16_t)strtol(at, &end, 10);
                if (end == at) {
                    snprintf(why, nwhy, "frame %d has too few turns", f);
                    free(copy);
                    motion_free(m);
                    return 0;
                }
                at = end;
            }
            f++;
        } else {
            snprintf(why, nwhy, "a motion has no line \"%.40s\"", line);
            free(copy);
            motion_free(m);
            return 0;
        }
    }
    free(copy);
    if (!got_format || f != m->frames) {
        snprintf(why, nwhy, "not a motion, or frames missing");
        motion_free(m);
        return 0;
    }
    return 1;
}

int motion_same(const Motion *a, const Motion *b) {
    return a->prop == b->prop && a->parts == b->parts && a->frames == b->frames &&
           memcmp(a->move, b->move, sizeof(*a->move) * (size_t)a->frames) == 0 &&
           memcmp(a->turns, b->turns, sizeof(int16_t) * (size_t)a->frames * a->parts * 3) == 0;
}
