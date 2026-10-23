#!/usr/bin/env python3
"""The doors into Forth, once, at development time (docs/subsystems/doors.md).

A door (D_80095CF0, 10 bytes a door) joins two rooms of an area, or a room and the town (room -2). Each side
names the door's spot in its area's spots (the town's for room -2): the spot's x z (D_80081350, an area's
pairs of s32) and heading (D_800813D4, an area's s16s), and which way one comes out there. A door with one
side only has -1 for the other's room. Written to scripts/events/doors.fs as readable rows (doors/rows.fs);
edited by hand from then on.

usage: doors2forth.py <SLPS_017.83> <out doors.fs>
"""
import struct
import sys

EXE_BASE = 0x80010000
EXE_HEADER = 0x800
DOORS = 0x80095CF0
DOOR_SIZE = 10
DOOR_COUNT = 108          # (the last the events name is 106; what follows isn't doors)
SPOT_PLACES = 0x80081350  # an area's spots: a pointer to x z pairs
SPOT_HEADINGS = 0x800813D4
AREAS = 34
TOWN = 27
TOWN_ROOM = -2
NO_ROOM = -1


def main():
    exe_path, out = sys.argv[1:3]
    e = open(exe_path, 'rb').read()
    off = lambda a: a - EXE_BASE + EXE_HEADER
    places = struct.unpack_from('<%dI' % AREAS, e, off(SPOT_PLACES))
    headings = struct.unpack_from('<%dI' % AREAS, e, off(SPOT_HEADINGS))

    def side(area, room, spot, way):
        if room == NO_ROOM:
            return ''
        a = TOWN if room == TOWN_ROOM else area
        x, z = struct.unpack_from('<2i', e, off(places[a]) + 8 * spot)
        h = struct.unpack_from('<h', e, off(headings[a]) + 2 * spot)[0] & 0xFFF
        return '  %d room  %d spot  %d %d place  %d facing  %d way' % (room, spot, x, z, h, way)

    lines = ['\\ doors.fs - the doors (docs/subsystems/doors.md): converted once from the executable\'s tables by',
             '\\ tools/doors2forth.py; edited by hand from here on. A door of an area joins two of its rooms, or a room',
             '\\ and the town (room -2); each side: its room, its spot (the area\'s door spots, the town\'s for room -2):',
             '\\ where it is, x z, which way it faces (4096 a turn) - and which way one comes out there.',
             'IN: events.doors', 'USING: doors.rows ;', '']
    for i in range(DOOR_COUNT):
        area, r0, a0, s0, r1, a1, s1, _ = struct.unpack_from('<bbhBbhBB', e, off(DOORS) + DOOR_SIZE * i)
        lines.append('%3d door  %d in-area' % (i, area) + side(area, r0, a0, s0) + side(area, r1, a1, s1)
                     + '  end-door')
    open(out, 'w').write('\n'.join(lines) + '\n')
    print('%d doors' % DOOR_COUNT)


if __name__ == '__main__':
    main()
