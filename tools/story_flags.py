#!/usr/bin/env python3
"""scripts/story-flags.fs from the decomp's flag names (../tools/flag_names.txt), at development time.

A bit flag becomes a constant: its bit in the story block (offset * 8 + bit: the block's 32-bit words are
little-endian, so bit b of the word at offset o is bit b mod 8 of byte o + b / 8); a value its offset. The
evidence goes along as a comment, its quoted lines left out (the text stays in the library).
usage: story_flags.py <flag_names.txt> <out story-flags.fs>
"""
import re
import sys

# names past the Forth's 31 characters, shortened
SHORT = {'lorraine_robbery_confession_seen': 'lorraine-robbery-confessed',
         'diner_mel_lorraine_argument_seen': 'diner-mel-lorraine-argued',
         'forest_mortal_journey_thought_seen': 'forest-mortal-journey-thought'}
src, out = sys.argv[1:3]
lines = ['\\ story-flags.fs - the story block (docs/subsystems/story.md): the original\'s 0x204 bytes of flags and',
         '\\ counters (src: D_800F6928, story_save), and the ones named so far. Written by tools/story_flags.py from',
         '\\ the decomp\'s tools/flag_names.txt; more names are added here by hand as they are understood.',
         'IN: story-flags', '',
         '$204 constant story-bytes', 'create story-block  story-bytes allot',
         ': story-clear ( -- )  story-block story-bytes erase ;  story-clear', '',
         '\\ a bit: its number in the block (`$010 2 story-bit`); anyone may read one, its owner sets it',
         ': story-bit ( offset bit -- n )  swap 8 * + ;',
         ': bit-at ( n -- addr mask )  dup 3 rshift story-block +  1 rot 7 and lshift ;',
         ': flag? ( n -- flag )  bit-at swap c@ and 0<> ;',
         ': flag-set ( n -- )  bit-at over c@ or swap c! ;',
         ': flag-clear ( n -- )  bit-at invert over c@ and swap c! ;',
         ': flag-toggle ( n -- )  dup flag? if  flag-clear  else  flag-set  then ;',
         '\\ a counter: a 32-bit word at its offset',
         ': story@ ( offset -- n )  story-block + sl@ ;',
         ': story! ( n offset -- )  story-block + l! ;',
         ': story+! ( n offset -- )  tuck story@ + swap story! ;', '',
         '\\ ---- the names (\'?\': a likely reading, not settled) ----']
n = 0
for ln in open(src, encoding='utf-8'):
    m = re.match(r'\+0x([0-9A-F]+) (?:bit(\d+)|value)\s+(\S+)\s*(?:--\s*(.*))?', ln)
    if not m:
        continue
    off, bit, name = int(m.group(1), 16), m.group(2), m.group(3)
    unsure = name.endswith('?')
    word = SHORT.get(name.rstrip('?'), name.rstrip('?')).replace('_', '-')
    assert len(word) <= 31, word   # (the Forth's names: 31 characters at most)
    ev = re.sub(r'"[^"]*"', '"..."', (m.group(4) or '').strip())[:110]
    if bit is not None:
        lines.append('$%03X %2d story-bit constant %-34s \\ %s%s' % (off, int(bit), word, '? ' if unsure else '', ev))
    else:
        lines.append('$%03X constant %-45s \\ (a counter) %s%s' % (off, word, '? ' if unsure else '', ev))
    n += 1
open(out, 'w', encoding='utf-8').write('\n'.join(lines) + '\n')
print('%d names -> %s' % (n, out))
