/* The game's text: blocks of messages, each a list of the game's character codes, and their form in the library -
 * UTF-8 text files.
 *
 * Two block formats are on the disc: MDD, the 2021 English translation's (Huffman-coded), and MDT, the
 * original's (a byte per character from a table of 128, or two bytes for any other code; a few leftover blocks).
 * The codes the English text uses: the printable ASCII characters, the pad's button symbols, a dash and the line
 * break; anything else is kept as its number.
 *
 * The text file (`N.txt`):
 *     \ a comment line (the file's first lines)
 *     #0
 *     Bed:
 *     #1
 *     I can't sleep now. Maybe I
 *     should take a shower before
 *     Winona comes over.
 * `#id` starts a message; its lines follow (a line break inside a message is the game's). In the text, `\\`,
 * `\{` and `\}` are those characters, `\#` a `#` at a line's start, `{$A0FF}` a code by number, and △ ○ ✕ □ —
 * the pad's buttons and the dash. */
#ifndef TEXT_H
#define TEXT_H

#include <stddef.h>
#include <stdint.h>

#define TEXT_LINE_BREAK 0xFF6E

typedef struct TextMessage {
    uint16_t *codes;
    int n;
} TextMessage;

typedef struct TextBlock {
    TextMessage *messages;
    int n;
} TextBlock;

/* where a text block (MDD or MDT) starts in d (it may follow other data), or -1 */
long text_find_block(const uint8_t *d, size_t n);
/* a block's messages; 0 if it isn't one or is damaged */
int text_decode(TextBlock *b, const uint8_t *d, size_t n);
void text_free(TextBlock *b);

/* the text file (malloc'd, `comment` its first line), and back; 0 / NULL on a malformed file */
char *text_to_utf8(const TextBlock *b, const char *comment, size_t *n);
int text_from_utf8(TextBlock *b, const char *s, size_t n, char *why, size_t nwhy);

#endif
