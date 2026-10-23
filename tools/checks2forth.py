#!/usr/bin/env python3
"""The areas' look and act tables into Forth, once, at development time (docs/subsystems/look.md).

Each area (its content area: 29/30 use 2's, 31/32 19's) has four tables - look zones and look checks
(D_800962E0), act checks and act zones (D_800964B0) - of rows naming a zone (and for checks a heading
range) and a target: an event script, a C function (a native) or nothing (the area's rules decide).
Written to scripts/events/checks.fs as readable rows (look.fs's words); edited by hand from then on.

usage: checks2forth.py <SLPS_017.83> <extract/cdbin> <out checks.fs>
"""
import os
import struct
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.join(HERE, '..', '..', 'tools'))
import checks as CK  # noqa: E402

EXE_BASE, OVL_BASE = CK.EXE_BASE, CK.OVL_BASE
C_FUNCTION = 0x1000
STATES = 0x6000


def tables(exe_path, src):
    e = open(exe_path, 'rb').read()
    off = lambda a: a - EXE_BASE + 0x800
    out = {}
    for a in CK.CONTENT:
        p = os.path.join(src, '%03d_0.bin' % a)
        img = CK.Img(e, open(p, 'rb').read() if os.path.exists(p) else b'')
        lz, lc = struct.unpack_from('<2I', e, off(0x800962E0) + 8 * a)
        ac, az = struct.unpack_from('<2I', e, off(0x800964B0) + 8 * a)
        out[a] = [('look-zones', CK.rows(img, lz, False)), ('look-checks', CK.rows(img, lc, True)),
                  ('act-checks', CK.rows(img, ac, True)), ('act-zones', CK.rows(img, az, False))]
    return out


def target(area, r):
    t = r['target']
    if t == 0:
        return 'ask-area'
    if r['flags'] & C_FUNCTION:
        scope = 'main-exe' if t < OVL_BASE else '%d ovl' % area
        return '%s $%08X native-at' % (scope, t)
    if t < OVL_BASE:
        return 'script events.main:exe-%08X' % t
    return 'script events.ovl%03d:entry-%08X' % (area, t)


def main():
    exe, src, out = sys.argv[1:4]
    lines = ['\\ checks.fs - every area\'s look and act tables (docs/subsystems/look.md): converted once from the',
             '\\ executable\'s and the overlays\' tables by tools/checks2forth.py; edited by hand from here on.',
             '\\ A row: its area and table, its zone (zone tables: the index on the floor; check tables: the id),',
             '\\ for checks the heading (4096 a turn) and degrees either side, its target, its hint\'s text.',
             'IN: events.checks', 'USING: events events.all look look.rows ;   \\ (look.rows last: its prompt, not the events\' op)', '']
    n = 0
    for a, ts in sorted(tables(exe, src).items()):
        if not any(rows for _, rows in ts):
            continue
        lines.append('\\ ---- area %d ----' % a)
        for name, rows in ts:
            for r in rows:
                parts = ['%d %s row' % (a, name), '%d zone' % r['zone']]
                if r['facing'] is not None:
                    parts.append('%d facing  %d either-side' % (r['facing'], r['tol']))
                    if r['flags'] & STATES:
                        parts.append('$%04X states' % (r['flags'] & STATES))
                parts.append(target(a, r))
                if r['extra']:
                    parts.append('%d hint' % r['extra'])
                comment = '   \\ %d degrees' % (r['facing'] * 360 // 4096) if r['facing'] is not None else ''
                lines.append('  '.join(parts) + '  end-row' + comment)
                n += 1
        lines.append('')
    open(out, 'w', encoding='utf-8').write('\n'.join(lines))
    print('%d rows -> %s' % (n, out))


if __name__ == '__main__':
    main()
