/* The game's text blocks and their text files (text.h). */
#include "text.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* character codes */
#define CODE_ASCII_FIRST 0xA000   /* 0xA000 + (c - ' ') for the printable ASCII characters */
#define ASCII_FIRST 0x20
#define ASCII_LAST 0x7E
static const struct { uint16_t code; const char *utf8; } kSymbols[] = {
    {0xA05F, "\xE2\x96\xB3"},   /* △ triangle */
    {0xA060, "\xE2\x97\x8B"},   /* ○ circle */
    {0xA061, "\xE2\x9C\x95"},   /* ✕ cross */
    {0xA062, "\xE2\x96\xA1"},   /* □ square */
    {0xA063, "\xE2\x80\x94"},   /* — dash */
};
#define NSYMBOLS (int)(sizeof(kSymbols) / sizeof(kSymbols[0]))

/* MDD: the words after the magic are offsets from the block's start + 4 */
#define MDD_COUNT 1
#define MDD_INDEX 2       /* {u16 byte offset, u16 length} a message */
#define MDD_START_BIT 3   /* a byte a message: bits of its first byte already used */
#define MDD_BITS 4
#define MDD_ZERO 5        /* the tree's 0-branches (u16 a node) */
#define MDD_ONE 7         /* its 1-branches */
#define MDD_LEAVES 9      /* the codes (u16); a branch >= 0x8000 is leaf (branch - 0x8000) */
#define MDD_LEAF_FLAG 0x8000
#define MDD_WORDS 11

/* MDT */
#define MDT_TABLE 8       /* 128 big-endian codes for the one-byte characters */
#define MDT_INDEX 0x108   /* {u16 offset, u16 length} a message; the text after the index */
#define MDT_WIDE 0x80     /* a byte from 0x80 starts a two-byte code */

static uint32_t le32(const uint8_t *p) { return p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24; }
static uint16_t le16(const uint8_t *p) { return (uint16_t)(p[0] | p[1] << 8); }

long text_find_block(const uint8_t *d, size_t n) {
    size_t i;

    for (i = 0; i + 8 <= n; i += 4) {
        if (memcmp(d + i, "MDD", 4) == 0 || memcmp(d + i, "MDT", 4) == 0) {
            return (long)i;
        }
    }
    return -1;
}

static int decode_mdd(TextBlock *b, const uint8_t *d, size_t n) {
    uint32_t w[MDD_WORDS];
    const uint8_t *base = d + 4;
    int i, m;

    if (n < MDD_WORDS * 4) {
        return 0;
    }
    for (i = 0; i < MDD_WORDS; i++) {
        w[i] = le32(d + i * 4);
    }
    for (i = MDD_INDEX; i < MDD_WORDS; i += i < MDD_ZERO ? 1 : 2) {
        if (w[i] + 4 > n) {
            return 0;
        }
    }
    b->n = (int)w[MDD_COUNT];
    b->messages = calloc((size_t)b->n + 1, sizeof(TextMessage));
    for (m = 0; m < b->n; m++) {
        const uint8_t *entry = base + w[MDD_INDEX] + m * 4, *p;
        int len = le16(entry + 2), used = base[w[MDD_START_BIT] + m], node = 0;
        uint8_t cur;
        TextMessage *msg = &b->messages[m];

        p = base + w[MDD_BITS] + le16(entry);
        if (p >= d + n) {
            return 0;
        }
        cur = (uint8_t)(*p++ << used);
        msg->codes = malloc(sizeof(uint16_t) * (size_t)(len + 1));
        while (msg->n < len) {
            int bit;

            if (used >= 8) {
                if (p >= d + n) {
                    return 0;
                }
                cur = *p++;
                used = 0;
                continue;
            }
            bit = cur & 0x80;
            cur = (uint8_t)(cur << 1);
            used++;
            node = le16(base + (bit ? w[MDD_ONE] : w[MDD_ZERO]) + node * 2);
            if (node >= MDD_LEAF_FLAG) {
                msg->codes[msg->n++] = le16(base + w[MDD_LEAVES] + (node - MDD_LEAF_FLAG) * 2);
                node = 0;
            }
        }
    }
    return 1;
}

static int decode_mdt(TextBlock *b, const uint8_t *d, size_t n) {
    uint32_t count;
    size_t text;
    int m;

    if (n < MDT_INDEX) {
        return 0;
    }
    count = le32(d + 4);
    text = MDT_INDEX + (size_t)count * 4;
    if (text > n) {
        return 0;
    }
    b->n = (int)count;
    b->messages = calloc((size_t)b->n + 1, sizeof(TextMessage));
    for (m = 0; m < b->n; m++) {
        const uint8_t *entry = d + MDT_INDEX + m * 4;
        size_t at = text + le16(entry);
        int len = le16(entry + 2);
        TextMessage *msg = &b->messages[m];

        msg->codes = malloc(sizeof(uint16_t) * (size_t)(len + 1));
        while (msg->n < len) {
            if (at >= n) {
                return 0;
            }
            if (d[at] & MDT_WIDE) {
                if (at + 1 >= n) {
                    return 0;
                }
                msg->codes[msg->n++] = (uint16_t)(d[at] << 8 | d[at + 1]);
                at += 2;
            } else {
                const uint8_t *t = d + MDT_TABLE + d[at] * 2;
                msg->codes[msg->n++] = (uint16_t)(t[0] << 8 | t[1]);
                at++;
            }
        }
    }
    return 1;
}

int text_decode(TextBlock *b, const uint8_t *d, size_t n) {
    int ok;

    memset(b, 0, sizeof(*b));
    if (n < 8) {
        return 0;
    }
    ok = memcmp(d, "MDD", 4) == 0 ? decode_mdd(b, d, n) : memcmp(d, "MDT", 4) == 0 ? decode_mdt(b, d, n) : 0;
    if (!ok) {
        text_free(b);
    }
    return ok;
}

void text_free(TextBlock *b) {
    int i;

    for (i = 0; b->messages != NULL && i < b->n; i++) {
        free(b->messages[i].codes);
    }
    free(b->messages);
    memset(b, 0, sizeof(*b));
}

/* ---- the text file ---- */
typedef struct Out {
    char *s;
    size_t n, cap;
} Out;

static void put(Out *o, const char *s, size_t n) {
    if (o->n + n + 1 > o->cap) {
        o->cap = (o->n + n + 1) * 2;
        o->s = realloc(o->s, o->cap);
    }
    memcpy(o->s + o->n, s, n);
    o->n += n;
    o->s[o->n] = 0;
}

static void puts_(Out *o, const char *s) { put(o, s, strlen(s)); }

char *text_to_utf8(const TextBlock *b, const char *comment, size_t *n) {
    Out o = {0};
    char buf[32];
    int m, i, k;

    puts_(&o, "\\ ");
    puts_(&o, comment);
    puts_(&o, "\n");
    for (m = 0; m < b->n; m++) {
        const TextMessage *msg = &b->messages[m];
        int line_start = 1;

        snprintf(buf, sizeof(buf), "#%d\n", m);
        puts_(&o, buf);
        for (i = 0; i < msg->n; i++) {
            uint16_t c = msg->codes[i];

            if (c == TEXT_LINE_BREAK) {
                puts_(&o, "\n");
                line_start = 1;
                continue;
            }
            if (c >= CODE_ASCII_FIRST && c <= CODE_ASCII_FIRST + (ASCII_LAST - ASCII_FIRST)) {
                char ch = (char)(c - CODE_ASCII_FIRST + ASCII_FIRST);

                if (ch == '\\' || ch == '{' || ch == '}' || (ch == '#' && line_start)) {
                    put(&o, "\\", 1);
                }
                put(&o, &ch, 1);
            } else {
                for (k = 0; k < NSYMBOLS && kSymbols[k].code != c; k++) {
                }
                if (k < NSYMBOLS) {
                    puts_(&o, kSymbols[k].utf8);
                } else {
                    snprintf(buf, sizeof(buf), "{$%04X}", c);
                    puts_(&o, buf);
                }
            }
            line_start = 0;
        }
        puts_(&o, "\n");
    }
    *n = o.n;
    return o.s;
}

static void add_code(TextMessage *msg, int *cap, uint16_t c) {
    if (msg->n == *cap) {
        *cap = *cap * 2 + 16;
        msg->codes = realloc(msg->codes, sizeof(uint16_t) * (size_t)*cap);
    }
    msg->codes[msg->n++] = c;
}

int text_from_utf8(TextBlock *b, const char *s, size_t n, char *why, size_t nwhy) {
    size_t i = 0;
    int cap_msgs = 0, line = 1;

    memset(b, 0, sizeof(*b));
    /* the comment lines before the first message */
    while (i < n && s[i] == '\\' && i + 1 < n && s[i + 1] == ' ') {
        while (i < n && s[i] != '\n') {
            i++;
        }
        i++;
        line++;
    }
    while (i < n) {
        TextMessage *msg;
        int id, cap = 0, line_start = 1;
        char *end;

        if (s[i] != '#') {
            snprintf(why, nwhy, "line %d: a message starts with #<number>", line);
            text_free(b);
            return 0;
        }
        id = (int)strtol(s + i + 1, &end, 10);
        if (end == s + i + 1 || id != b->n || *end != '\n') {
            snprintf(why, nwhy, "line %d: expected #%d", line, b->n);
            text_free(b);
            return 0;
        }
        i = (size_t)(end - s) + 1;
        line++;
        if (b->n == cap_msgs) {
            cap_msgs = cap_msgs * 2 + 16;
            b->messages = realloc(b->messages, sizeof(TextMessage) * (size_t)cap_msgs);
        }
        msg = &b->messages[b->n++];
        memset(msg, 0, sizeof(*msg));
        for (;;) {   /* the message ends at the line break before the next #, or the file's last one */
            unsigned char c;
            int k;

            if (i >= n) {
                break;
            }
            c = (unsigned char)s[i];
            if (c == '\n') {
                if (i + 1 >= n || s[i + 1] == '#') {
                    i++;
                    line++;
                    break;
                }
                add_code(msg, &cap, TEXT_LINE_BREAK);
                i++;
                line++;
                line_start = 1;
                continue;
            }
            if (c == '\\' && i + 1 < n && (s[i + 1] == '\\' || s[i + 1] == '{' || s[i + 1] == '}' || (s[i + 1] == '#' && line_start))) {
                add_code(msg, &cap, (uint16_t)(CODE_ASCII_FIRST + (s[i + 1] - ASCII_FIRST)));
                i += 2;
            } else if (c == '{' && i + 2 < n && s[i + 1] == '$') {
                unsigned code;

                if (sscanf(s + i + 2, "%4x}", &code) != 1 || i + 7 > n || s[i + 6] != '}') {
                    snprintf(why, nwhy, "line %d: a code is {$XXXX}", line);
                    text_free(b);
                    return 0;
                }
                add_code(msg, &cap, (uint16_t)code);
                i += 7;
            } else if (c >= ASCII_FIRST && c <= ASCII_LAST && c != '{' && c != '}' && c != '\\') {
                add_code(msg, &cap, (uint16_t)(CODE_ASCII_FIRST + (c - ASCII_FIRST)));
                i++;
            } else {
                for (k = 0; k < NSYMBOLS; k++) {
                    size_t len = strlen(kSymbols[k].utf8);
                    if (i + len <= n && memcmp(s + i, kSymbols[k].utf8, len) == 0) {
                        add_code(msg, &cap, kSymbols[k].code);
                        i += len;
                        break;
                    }
                }
                if (k == NSYMBOLS) {
                    snprintf(why, nwhy, "line %d: the game's font has no character for byte 0x%02X here", line, c);
                    text_free(b);
                    return 0;
                }
            }
            line_start = 0;
        }
    }
    return 1;
}
