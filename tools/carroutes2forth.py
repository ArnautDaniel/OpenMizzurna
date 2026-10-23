#!/usr/bin/env python3
"""The cars' routes into Forth, once, at development time (docs/subsystems/traffic.md).

A car route (D_80093640: 181 pointers to byte strings; src carline.c) is the parking node whose lanes it starts
in, then the road nodes it drives through, then either 0xFF and the node it parks at, or 0xFE n: back n bytes,
driving round again. Written to scripts/events/car-routes.fs as readable rows (traffic/rows.fs); edited by hand
from then on.

usage: carroutes2forth.py <SLPS_017.83> <out car-routes.fs>
"""
import struct
import sys

EXE_BASE = 0x80010000
EXE_HEADER = 0x800
ROUTES = 0x80093640
ROUTE_COUNT = 181
PARK = 0xFF
LOOP = 0xFE


def main():
    exe_path, out = sys.argv[1:3]
    e = open(exe_path, 'rb').read()
    off = lambda a: a - EXE_BASE + EXE_HEADER
    pointers = struct.unpack_from('<%dI' % ROUTE_COUNT, e, off(ROUTES))
    lines = ['\\ car-routes.fs - the cars\' routes (docs/subsystems/traffic.md): converted once from the executable\'s',
             '\\ table by tools/carroutes2forth.py; edited by hand from here on. A route: the parking node whose lanes',
             '\\ it starts in, the road nodes it drives through (how many), then where it parks - or, going round, the',
             '\\ node (counted from 0) it drives on from again.',
             'IN: events.car-routes', 'USING: traffic.rows ;', '']
    for i, p in enumerate(pointers):
        o = off(p)
        lane, nodes = e[o], []
        o += 1
        while e[o] not in (PARK, LOOP):
            nodes.append(e[o])
            o += 1
        if e[o] == PARK:
            end = '%d parks' % e[o + 1]
        else:
            back = len(nodes) - e[o + 1]   # (back n bytes from the 0xFE: a node, counted after the lane byte)
            end = '%d loops-to' % back
        lines.append('%3d car-route  %d lane  %s  %d roads  %s  end-car-route' % (
            i, lane, ' '.join(map(str, nodes)), len(nodes), end))
    open(out, 'w').write('\n'.join(lines) + '\n')
    print('%d car routes' % ROUTE_COUNT)


if __name__ == '__main__':
    main()
