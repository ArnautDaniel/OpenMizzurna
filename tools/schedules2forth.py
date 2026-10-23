#!/usr/bin/env python3
"""The townsfolk's schedules into Forth, once, at development time (docs/subsystems/townsfolk.md).

A scheduled character (their own handler: Morgan, Nozz, Winona ...) or a townsperson (the shared handler: a
person number, slots 65-88 and 113-124) has rows: until when they stay at a stop, the route they walk when
they leave it, and where the stop is (a placement: area, floor, x z, heading, motion, how they're placed).
The decoding is the decomp's tools/schedule.py. Written to scripts/events/schedules.fs, edited by hand from
then on; the routes (event bytecode) are converted with the events (to come).

usage: schedules2forth.py <SLPS_017.83> <extract/cdbin> <decomp root> <out schedules.fs>
"""
import os
import re
import struct
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.join(HERE, '..', '..', 'tools'))
import schedule as SC  # noqa: E402
import ev2forth as EF  # noqa: E402

OVL_BASE = SC.OVL_BASE
PEOPLE = [(0x41 + n, n) for n in range(0x18)] + [(0x71 + n - 0x18, n) for n in range(0x18, 0x24)]   # src: ovl033
TYPES = {0: 'stand', 1: 'with-partner', 2: 'diner-seat', 3: 'club-seat'}


def stop_line(i, d, hh, mm, scr, k, rec):
    x, z, hd, area, floor, anim, typ, model = rec
    parts = ['%2d stop' % i, '%d %d %d until' % (d, hh, mm), '%d record' % k]
    if typ == 0:
        parts.append('%d %d in-area  %d %d place  %d facing  %d pose' % (area, floor, x, z, hd & 0xFFF, anim))
    else:
        parts.append(TYPES.get(typ, '%d placed-as' % typ))
    if model:
        parts.append('%d variant' % model)
    if scr:
        parts.append('$%08X route' % scr)
    return '  ' + '  '.join(parts) + '  end-stop'


def main():
    exe_path, src, root, out = sys.argv[1:5]
    e = open(exe_path, 'rb').read()
    off = lambda a: a - 0x80010000 + 0x800
    ovl = open(os.path.join(src, '033_0.bin'), 'rb').read()
    hand = struct.unpack_from('<182I', ovl, 0x80103BB8 - OVL_BASE)
    syms = {}
    for ln in open(os.path.join(root, 'config', 'symbols.main.txt')):
        m = re.match(r'(\w+) = (0x[0-9A-Fa-f]+);', ln)
        if m:
            syms[int(m.group(2), 16)] = m.group(1)
    info = SC.handlers(open(os.path.join(root, 'src', 'main', 'main.c')).read())
    lines = ['\\ schedules.fs - the townsfolk\'s days (docs/subsystems/townsfolk.md): converted once from the',
             '\\ executable\'s tables by tools/schedules2forth.py; edited by hand from here on. A person\'s rows: until when',
             '\\ (day hour minute) they stay at a stop, where it is - area floor, x z, heading (4096 a turn), the motion they',
             '\\ hold there - or how they are placed (with a partner, at the diner\'s counter, at Club Wolves), the route',
             '\\ they walk on leaving it.',
             'IN: events.schedules', 'USING: townsfolk.rows ;', '']
    count = 0
    for slot in range(182):
        fn = syms.get(hand[slot])
        h = info.get(fn) if fn else None
        if not h or not h['departures'] or not h['stop_index']:
            continue
        lines.append('\\ ---- slot %d: %s ----' % (slot, EF.CAST.get(slot, (h['name'] or fn).lower())))
        lines.append('%d schedule' % slot)
        for i in range(99):
            d, hh, mm, _, scr = struct.unpack_from('<4BI', e, off(h['departures']) + 8 * i)
            if d == 0:
                break
            k = e[off(h['stop_index']) + i]
            lines.append(stop_line(i, d, hh, mm, scr, k, struct.unpack_from('<iiHBBhBB', e, off(h['records']) + 16 * k)))
        lines.append('end-schedule')
        lines.append('')
        count += 1
    u32 = lambda a: struct.unpack_from('<I', e, off(a))[0]
    for slot, pn in PEOPLE:
        n = e[off(0x8008FA64) + pn]
        dep, idx, rec = u32(0x8008F9D4 + 4 * pn), u32(0x80089B38 + 4 * pn), u32(0x80089BC8 + 4 * pn)
        if not (n and dep and idx and rec) or (n == 99 and e[off(dep)] == 0):
            continue
        lines.append('\\ ---- slot %d: townsperson %d ----' % (slot, pn))
        lines.append('%d schedule' % slot)
        for i in range(n):
            d, hh, mm, _, scr = struct.unpack_from('<4BI', e, off(dep) + 8 * i)
            if d == 0:
                break
            k = e[off(idx) + i]
            lines.append(stop_line(i, d, hh, mm, scr, k, struct.unpack_from('<iiHBBhBB', e, off(rec) + 16 * k)))
        lines.append('end-schedule')
        lines.append('')
        count += 1
    open(out, 'w', encoding='utf-8').write('\n'.join(lines))
    print('%d schedules -> %s' % (count, out))


if __name__ == '__main__':
    main()
