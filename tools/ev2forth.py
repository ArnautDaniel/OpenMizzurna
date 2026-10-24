#!/usr/bin/env python3
"""The event scripts into Forth, once, at development time (docs/subsystems/story.md).

The game's event scripts are bytecode (CEVENT.C's VM: src port/src/game/cevent.c, listing tool
tools/evscript.py): in the event packs (CD.BIN 042_3 .. 058_0), in the area overlays (started from their C) and
in the main executable. Each becomes a Forth file in scripts/events/ - a word per script and per label, an
instruction a line - for the `events` vocabulary (scripts/events.fs) to run as tasks. From then on the Forth is
the source: it is edited by hand (names, structure), and this tool isn't run again over edited files.

  - a script and each place a branch lands is a word; `goto` is a tail jump (the return stack doesn't grow),
    `call` an ordinary call, `return` an exit (the outermost ends the event); a fall into the next label a goto
  - waits are the event's frames: every event ran at 15 frames a second (CEvCon_Start: VSync(4)), so `n frames`
    is n / 15 seconds
  - actors by name where the cast knows them (events.fs: matthew, winona ...), else `n actor`; `this-one` the
    event's own actor (the bytecode's 0xFF)
  - text by id (`60 2 caption-for`); --text adds each line as a comment, for reading (the 2021 translation's
    text stays in the library, not in the repository)
  - the C an event calls (`call_c`) is `$addr native` until it is written in Forth

usage: ev2forth.py <extract/cdbin> <SLPS_017.83> <decomp root> <out dir> [--text <script/en2021>] [--only NAME]
"""
import collections
import os
import re
import struct
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.join(HERE, '..', '..', 'tools'))
import evscript as EV  # noqa: E402

EXE_BASE, OVL_BASE, PACK_BASE = EV.EXE_BASE, EV.OVL_BASE, EV.PACK_BASE
OWN = 0xFF   # the event's own actor

# the cast the events vocabulary names (decomp/include/actor.h; tools/schedule.py's EM_TALK names)
CAST = {0: 'matthew', 1: 'morgan', 2: 'nozz', 3: 'cohen', 5: 'samuel', 6: 'james', 7: 'hudson', 12: 'steve',
        13: 'mel', 14: 'father-barton', 15: 'winona', 20: 'lidds', 21: 'wolf', 22: 'rolein', 89: 'isabella',
        90: 'barbara', 92: 'dennis', 93: 'robert'}

# names given to scripts (scope, address) -> word; the rest are named by where they are
NAMES = {('ovl001', 0x80103FE4): 'new-game-opening',   # src: game_start (boot.c), a new game
         ('ovl001', 0x80103C60): 'wake-by-the-bed',
         ('main', 0x800965B0): 'into-the-car',         # src: func_8004175C, alone
         ('main', 0x80096610): 'into-the-car-with'}    # src: func_8004175C, with a passenger     # src: game_start, a loaded game

# VM ops: Forth word and argument kinds (a actor, m text, n number, f frames, p place, i pack script)
OPS = {
    0x102: ('camera-set', 'nnnnnnn'), 0x103: ('wait-anim', 'a'), 0x104: ('matthew wait-anim', ''),
    0x105: ('say-reset', 'm'), 0x106: ('phone-say', 'm'), 0x107: ('wait', 'f'), 0x108: ('say', 'm'),
    0x109: ('say-by', 'ma'), 0x10A: ('wait-text', ''), 0x10B: ('camera-follow', ''),
    0x10C: ('camera-fixed', ''), 0x10D: ('wait-stop', 'a'), 0x10E: ('give-control', ''),
    0x10F: ('take-control', ''), 0x110: ('set-scene', 'n'), 0x112: ('wait-turned', 'a'),
    0x113: ('camera-at', 'a'), 0x114: ('sound', 'n'), 0x115: ('sequence-play', 'nn'),
    0x116: ('sequence-stop', 'n'), 0x117: ('wait-near', 'annn'), 0x119: ('camera-move', 'nnnn'),
    0x11A: ('scene-reset', 'n'), 0x11B: ('camera-face', 'aa'), 0x11C: ('camera-turn', 'nn'),
    0x11D: ('screen-effect', 'n'), 0x11E: ('choice', 'mn'), 0x121: ('wait-effect', ''),
    0x123: ('picture-show', 'n'), 0x124: ('picture-hide', ''), 0x125: ('say-by-wait', 'ma'),
    0x126: ('door-reveal-room', 'n'), 0x127: ('door-anim', 'n'), 0x128: ('talk', 'amn'),
    0x129: ('prompt', 'nn'), 0x12B: ('cd-picture', 'n'), 0x12F: ('this-one wait-anim', ''),
    0x130: ('this-one wait-stop', ''), 0x131: ('this-one wait-turned', ''), 0x132: ('this-one say-by-wait', 'm*'),
    0x133: ('ignore-a', 'aa'), 0x134: ('ignore-b', 'aa'), 0x135: ('matthew talk', '*mn'),
    0x136: ('this-one talk', '*mn'), 0x137: ('say-mode-wait', 'mn'), 0x138: ('wait-follow', 'a'),
    0x139: ('door-animate', 'w'), 0x13A: ('door-animate-alt', 'w'), 0x13B: ('through-door', 'w'),
    0x13C: ('say-n', 'mna'), 0x13D: ('say-timed-by', 'mas'), 0x13E: ('text-hold', ''),
    0x13F: ('say-timed-by-wait', 'mas'), 0x140: ('this-one through-door', 'w'), 0x141: ('this-one wait-follow', ''),
    0x142: ('camera-angle', 'nn'), 0x144: ('music', 'n'), 0x145: ('minigame', 'n'),
    0x146: ('matthew wait-stop', ''), 0x147: ('matthew wait-turned', ''), 0x148: ('elevator-doors', 'nn'),
    0x149: ('wait-minigame', ''), 0x14A: ('( nop )', ''), 0x14B: ('wait-camera-turn', ''), 0x14C: ('text-close', ''),
    0x14D: ('wait-camera-move', ''), 0x14E: ('camera-height-to', 'nn'), 0x14F: ('wait-camera-height', ''),
    0x150: ('camera-heading', 'a'), 0x152: ('memory-card', 'n'), 0x153: ('say-mode', 'mn'),
    0x154: ('camera-dist-to', 'nn'), 0x155: ('rumble', 'nn'), 0x156: ('object-anim', 'n'),
    0x157: ('time-skip', 'n'), 0x158: ('say-wait-close', 'm'), 0x159: ('actor-model', 'an'),
    0x15A: ('this-one actor-model', '*n'), 0x15B: ('caption-for', 'ms'), 0x15C: ('load-pack', 'n'),
    0x15D: ('sprite-count+', ''), 0x15E: ('sprite-count-close', ''), 0x15F: ('this-one through-door-back', 'w'),
    0x160: ('wait-camera-dist', ''), 0x161: ('effect-level', 'nn'), 0x162: ('face-each-other', 'aa'),
    0x164: ('this-one talk-to', '*mnn'), 0x165: ('map-reveal', 'n'), 0x166: ('ambient-start', 'n'),
    0x167: ('ambient-stop', 'n'), 0x169: ('fight', 'anan'), 0x16B: ('( nop )', ''), 0x16E: ('wait-attached', 'a'),
    0x16F: ('talk-to', 'amnn'), 0x170: ('set-357c', 'n'), 0x172: ('map-sync', ''), 0x173: ('xa-play', 'n'),
    0x174: ('xa-stop', ''), 0x176: ('door-sound', 'n'), 0x177: ('caption-for-nowait', 'ms'),
    0x178: ('wait-attached-to', 'aa'), 0x179: ('cd-pictures', 'nn'), 0x12A: ('warp', 'nhnnhn'),
}
# actor commands (decomp/include/actor.h ActorCmd): Forth word ( actor args -- )
ACTS = {1: 'set-pos', 2: 'set-rot', 3: 'anim', 4: 'anim-then', 5: 'anim-once', 6: 'anim-then-loop',
        7: 'anim-hold', 8: 'walk-to', 9: 'turn-by', 0xA: 'walk-forward', 0xB: 'run', 0xC: 'idle',
        0xD: 'face-point', 0xE: 'face', 0xF: 'walk-to-ex', 0x10: 'move-forward', 0x11: 'turn-to',
        0x12: 'set-floor', 0x13: 'anim-at', 0x14: 'talk-pose', 0x16: 'restore-state', 0x18: 'anim-step',
        0x1E: 'arrive', 0x1F: 'freeze', 0x20: 'unfreeze', 0x21: 'set-x', 0x22: 'set-y', 0x23: 'set-z',
        0x24: 'lift', 0x26: 'ride', 0x27: 'unride', 0x2C: 'attach', 0x2D: 'place-at-stop', 0x2E: 'item-query',
        0x2F: 'item-show', 0x36: 'act', 0x37: 'partner-command', 0x38: 'move-by', 0x3A: 'take-diner-seat',
        0x3B: 'sit-diner-seat', 0x3C: 'ignore-a', 0x3D: 'ignore-b', 0x43: 'set-stop', 0x46: 'sit-club-seat',
        0x47: 'run-to', 0x48: 'leave-diner-seat', 0x49: 'leave-club-seat', 0x4A: 'examine', 0x4D: 'reset',
        0x4E: 'kill-thread', 0x4F: 'save-state', 0x53: 'door-hold', 0x54: 'door-release', 0x55: 'walk-path',
        0x56: 'depart-alt', 0x59: 'ending-schedule', 0x5A: 'go-to-actor', 0x5D: 'goto-stop', 0x5E: 'place-alt'}
TERMINAL = {0x12E, 0x171, 0x12A, 0x168}
CONTROL = {0x111, 0x11F, 0x120, 0x12C, 0x12E, 0x143, 0x163, 0x168, 0x171, 0x175, 0x122, 0x151, 0x12D,
           0x100, 0x101, 0x118, 0x16A, 0x16C, 0x16D}


class Insn:
    def __init__(self, a, w, n, args):
        self.a, self.w, self.n, self.args = a, w, n, args
        self.is_op = bool(w & 0x100)
        self.op = w & 0x1FF if self.is_op else None
        self.imm, self.sub = w >> 16, (w >> 9) & 0x7F


def decode(mem, a, op_len, act_len):
    """the instruction at a, or None (lengths as the VM takes them: evscript.disasm)"""
    w = mem.u32(a)
    if w is None:
        return None
    if not w & 0x100:
        cmd = (w >> 9) & 0x7F
        wide = cmd == 0
        if wide:
            cmd = mem.u32(a + 4)
        if cmd is None or cmd == 0 or cmd >= len(act_len) or (act_len[cmd] == 0 and not wide):
            return None
        n = act_len[cmd] + wide
        i = Insn(a, w, n, [mem.u32(a + 4 * k) for k in range(1 + wide, n)])
        i.cmd = cmd
        return i
    op = w & 0x1FF
    if op == 0x100:
        cmd = mem.u32(a + 8)
        n = act_len[cmd] + 2 if cmd is not None and cmd < len(act_len) and act_len[cmd] else 0
    elif op == 0x101:
        cmd = mem.u32(a + 4)
        n = act_len[cmd] + 1 if cmd is not None and cmd < len(act_len) and act_len[cmd] else 0
    elif op == 0x118:
        n = act_len[0x13] + 1
    elif op in (0x12C, 0x175):
        n = 2 if op == 0x12C else 3
    else:
        n = op_len[op] if op < 0x17A else 0
    if n == 0 and op != 0x171:
        return None
    n = max(n, 1)
    return Insn(a, w, n, [mem.u32(a + 4 * k) for k in range(1, n + (op == 0x163))])


def s32(v):
    return v - (1 << 32) if v >= 0x80000000 else v


def s16(v):
    v &= 0xFFFF
    return v - 0x10000 if v >= 0x8000 else v


def actor(v):
    if v == OWN:
        return 'this-one'
    return CAST.get(v, '%d actor' % v)


class Source:
    """one file's scripts: its memory, its entries (address -> word), where its references go"""

    def __init__(self, scope, mem, entries, pack_ids=None, text=None):
        self.scope, self.mem, self.entries, self.pack_ids, self.text = scope, mem, entries, pack_ids, text
        self.insns, self.starts, self.extern = {}, {}, set()

    def owns(self, a):
        if self.scope.startswith('pack'):
            return True
        if self.scope == 'main':
            return a < OVL_BASE
        return a >= OVL_BASE


def walk(src, op_len, act_len, extra=()):
    """decode every reachable instruction; block starts: entries, branch targets, call / thread targets"""
    todo = list(src.entries) + list(extra)
    for a in todo:
        src.starts.setdefault(a, None)
    bad = []
    while todo:
        a = todo.pop()
        while a not in src.insns:
            i = decode(src.mem, a, op_len, act_len)
            if i is None:
                bad.append(a)
                break
            src.insns[a] = i
            if i.is_op:
                tgt = None
                if i.op in (0x11F, 0x120):
                    tgt = i.args[1]
                elif i.op in (0x12C, 0x12E, 0x111):
                    tgt = i.args[0]
                elif i.op in (0x163, 0x168, 0x143) and src.pack_ids is not None:
                    k = i.args[1] if i.op == 0x163 else i.args[0]
                    tgt = src.pack_ids[k] if 0 <= k < len(src.pack_ids) and src.pack_ids[k] else None
                if tgt is not None:
                    if src.owns(tgt):
                        if tgt not in src.starts:
                            src.starts[tgt] = None
                            todo.append(tgt)
                    else:
                        src.extern.add(tgt)
                if i.op in TERMINAL:
                    break
            a += 4 * i.n
    return bad


def block_name(src, a):
    if (src.scope, a) in NAMES:
        return NAMES[(src.scope, a)]
    if a in src.entries and src.entries[a]:
        return src.entries[a]
    return 'L-%08X' % a


def ref(src, a, main_src):
    """the word for a reference to address a from src (None: main's into the area's overlay, found as it runs)"""
    if src.owns(a):
        return block_name(src, a)
    if src.scope == 'main':
        return None
    return 'events.main:' + block_name(main_src, a)


def flow(src, a, main_src, how):
    """goto / call / start a: a word, or main's into the overlay loaded (by address)"""
    r = ref(src, a, main_src)
    if r is None:
        return {'goto': '$%08X goto-overlay', 'call': '$%08X call-overlay',
                'start': '$%08X overlay-word start-event'}[how] % a
    return {'goto': 'goto %s', 'call': '%s', 'start': "['] %s start-event"}[how] % r


def blocks(src):
    """block start -> the instructions in it, in order"""
    starts = sorted(src.starts)
    out = collections.OrderedDict()
    for k, s in enumerate(starts):
        if s not in src.insns:
            continue
        body, a = [], s
        while a in src.insns:
            i = src.insns[a]
            body.append(i)
            a += 4 * i.n
            if i.is_op and i.op in TERMINAL:
                a = None
                break
            if a in src.starts:
                break
        out[s] = (body, a)   # a: the block falls into it (None: it ends)
    return out


def emit_insn(src, i, main_src, names_used, text_comment):
    """the Forth for one instruction (a list of lines)"""
    w = i.w
    if not i.is_op:   # an actor command word: actor in the low byte
        return [act_line(actor(w & 0xFF), i.cmd, i.args)]
    op, args = i.op, i.args
    if op == 0x100:
        return [act_line(actor(args[0]), args[1], args[2:])]
    if op == 0x101:
        return [act_line('matthew', args[0], args[1:])]
    if op == 0x118:
        who = actor(args[0])
        return [act_line(who, 0x13, args[1:]) + '  %s wait-anim' % who]
    if op == 0x16A:
        who = actor(i.imm & 0xFF)
        line = '%s %d %d walk-to' % (who, (args[0] >> 16) * 16, (args[0] & 0xFFFF) * 16)
        return [line + ('  %s wait-stop' % who if i.imm & 0x100 else '')]
    if op in (0x16C, 0x16D):
        line = '%s %d %d talk' % ('matthew' if op == 0x16C else 'this-one', i.imm, i.sub)
        return [line + text_comment(i.imm)]
    if op == 0x12D:
        if (src.scope, i.a) in PROP_SCRIPTS:   # (its actor set by the C that starts it: a prop, a companion)
            return ['the-prop @ as-this-one']
        return ['%s as-this-one' % actor(args[0])]
    if op == 0x107:
        return ['%d frames wait' % s32(args[0])]
    if op == 0x11F:
        return ['%d result= if  %s  then' % (s32(args[0]), flow(src, args[1], main_src, 'goto'))]
    if op == 0x120:
        return ['%d result= 0= if  %s  then' % (s32(args[0]), flow(src, args[1], main_src, 'goto'))]
    if op == 0x12E:
        return [flow(src, args[0], main_src, 'goto')]
    if op == 0x12C:
        return [flow(src, args[0], main_src, 'call')]
    if op == 0x111:
        return [flow(src, args[0], main_src, 'start')]
    if op == 0x171:
        return ['end-script']
    if op in (0x122, 0x151):
        scope = 'main-exe' if src.scope == 'main' or args[0] < OVL_BASE else src.scope   # (main's C, from anywhere)
        return ['%s $%08X native%s' % (scope, args[0], '-result' if op == 0x151 else '')]
    if op == 0x168:
        return [pack_ref(src, args[0], 'goto-pack-script')]
    if op == 0x143:
        return [pack_ref(src, args[0], 'start-pack-script')]
    if op == 0x163:   # QUIRK: its table length is 2, so falling through runs the id word as an actor command
        return ['%d result= if  %s  then  \\ QUIRK: the id word then runs as an actor command (one use)'
                % (s32(args[0]), pack_ref(src, args[1], 'goto-pack-script'))]
    if op == 0x175:
        return ['%d %d call-pack-script' % (s32(args[0]), s32(args[1]))]
    if op == 0x13B:   # through-door: its actor is the third word (the others' own actor first)
        return ['%s %d %d through-door' % (actor(args[2]), s32(args[0]), s32(args[1]))]
    if op == 0x133 or op == 0x134:
        return ['%s %s %s' % (actor(args[0]), actor(args[1]), OPS[op][0])]
    word, kinds = OPS.get(op, ('op-%03X' % op, None))
    parts, note = [], ''
    kinds = kinds or ''
    pre = ''
    if word.startswith('this-one ') or word.startswith('matthew '):   # the actor is the op's own
        pre, word = word.split(' ', 1)
        if '*' not in kinds:
            kinds = '*' + kinds
    k = 0
    for kind in kinds:
        if kind == '*':
            parts.append(pre)
            continue
        if kind == 'w':   # a door op: its words as they are (decoded by the door subsystem)
            parts += [str(s32(v)) for v in args]
            k = len(args)
            break
        v = args[k] if k < len(args) else 0
        k += 1
        if kind == 'a':
            parts.append(actor(v))
        elif kind == 'm':
            parts.append(str(s32(v)))
            note = text_comment(s32(v))
        elif kind == 'h':
            parts.append(str(s16(v)))
        else:
            parts.append(str(s32(v)))
    return [' '.join(parts + [word]) + note]


def pack_ref(src, k, word):
    if src.pack_ids is not None and 0 <= k < len(src.pack_ids) and src.pack_ids[k]:
        return 'goto %s' % block_name(src, src.pack_ids[k]) if word == 'goto-pack-script' else \
            "['] %s start-event" % block_name(src, src.pack_ids[k])
    return '%d %s' % (k, word)


def act_line(who, cmd, args):
    name = ACTS.get(cmd, None)
    vals = ' '.join(str(s32(v)) for v in args)
    if name is None:
        return '%s %s %d %d act-command' % (who, vals, len(args), cmd) if vals else '%s 0 %d act-command' % (who, cmd)
    return ('%s %s %s' % (who, vals, name)) if vals else '%s %s' % (who, name)


def emit(src, main_src, header, text_comment):
    bl = blocks(src)
    # order: each block after those it refers to (a cycle's are deferred)
    refs = {}
    for s, (body, fall) in bl.items():
        r = []
        for i in body:
            if i.is_op:
                if i.op in (0x11F, 0x120):
                    r.append(i.args[1])
                elif i.op in (0x12C, 0x12E, 0x111):
                    r.append(i.args[0])
                elif i.op in (0x163, 0x168, 0x143) and src.pack_ids is not None:
                    k = i.args[1] if i.op == 0x163 else i.args[0]
                    if 0 <= k < len(src.pack_ids) and src.pack_ids[k]:
                        r.append(src.pack_ids[k])
        if fall is not None:
            r.append(fall)
        refs[s] = [x for x in r if x in bl]
    order, state, deferred = [], {}, set()

    def visit(s):
        state[s] = 1
        for t in refs[s]:
            if state.get(t) == 1:
                deferred.add(t)
            elif t not in state:
                visit(t)
        state[s] = 2
        order.append(s)

    for s in sorted(bl, key=lambda x: (x not in src.entries, x)):
        if s not in state:
            visit(s)
    names_used = set()
    out = list(header)
    if deferred:
        out.append('')
        out.append('\\ (looped back to: declared first)')
        out += ['defer %s' % block_name(src, s) for s in sorted(deferred)]
    for s in order:
        body, fall = bl[s]
        name = block_name(src, s)
        out.append('')
        if s in src.entries or (src.scope, s) in NAMES:
            out.append('\\ $%08X' % s)
        out.append((':noname' if s in deferred else ': %s' % name))
        for i in body:
            for ln in emit_insn(src, i, main_src, names_used, text_comment):
                out.append('    ' + ln)
        if fall is not None:
            out.append('    goto %s' % block_name(src, fall))
        out.append('    ; is %s' % name if s in deferred else '    ;')
    return out


def route_addresses(exe, src_dir, root, exe_img):
    """every route a schedule names (the named cast's handlers' tables and the townspeople's)"""
    import re
    import schedule as SC
    e = open(exe, 'rb').read()
    off = lambda a: a - EXE_BASE + 0x800
    ovl = open(os.path.join(src_dir, '033_0.bin'), 'rb').read()
    hand = struct.unpack_from('<182I', ovl, 0x80103BB8 - OVL_BASE)
    syms = {}
    for ln in open(os.path.join(root, 'config', 'symbols.main.txt')):
        m = re.match(r'(\w+) = (0x[0-9A-Fa-f]+);', ln)
        if m:
            syms[int(m.group(2), 16)] = m.group(1)
    info = SC.handlers(open(os.path.join(root, 'src', 'main', 'main.c')).read())
    tables = []
    for slot in range(182):
        h = info.get(syms.get(hand[slot], ''))
        if h and h['departures']:
            tables.append((h['departures'], 99))
    u32 = lambda a: struct.unpack_from('<I', e, off(a))[0]
    for pn in range(36):
        if e[off(0x8008FA64) + pn]:
            tables.append((u32(0x8008F9D4 + 4 * pn), e[off(0x8008FA64) + pn]))
    out = set()
    for dep, n in tables:
        if not dep:
            continue
        for i in range(n):
            d, _, _, _, scr = struct.unpack_from('<4BI', e, off(dep) + 8 * i)
            if d == 0:
                break
            if EXE_BASE <= scr < EXE_BASE + len(exe_img):
                out.add(scr)
    return out | set(ARRIVAL_SCRIPTS)


# what a trip goes on with when the person's car arrives (src: the handlers' CMD_LINK_NOTIFY - Nozz at stops 2
# and 6, Cohen at 1 3 5 13 and 6): converted with the routes
# scripts whose own actor the overlay's C sets before starting them (src: M2C_FIELD(&D_X, s32 *, 4) = ...->id: the
# counters' orders - the food, the drink): their `as-this-one` takes events.fs's the-prop
PROP_SCRIPTS = set()


def minigame_scripts(c, ovl=None):
    """the scripts an overlay's area_minigame_start queues (src: minigame_script = &D_X, or by a local; or a table
    of them indexed, minigame_script = *(... + &D_T): its words that point into the overlay)"""
    m = re.search(r'\nvoid area_minigame_start\(void\) \{\n(.*?)\n\}\n', c, re.S)
    if not m:
        return set()
    out = set(int(x, 16) for x in re.findall(r'\w+ = &D_([0-9A-F]{8})', m.group(1)))
    # (or one queued at a step of its update, wherever the overlay does it: area 19's chase, the town's tail)
    out |= set(int(x, 16) for x in re.findall(r'minigame_script = &D_([0-9A-F]{8})', c))
    # (or one it runs on a private thread of its own: event_thread_start(&D_T, &D_X))
    out |= set(int(x, 16) for x in re.findall(r'event_thread_start\(&D_[0-9A-F]{8}, &D_([0-9A-F]{8})\)', c))
    if ovl is not None:
        for t in re.findall(r'minigame_script = \*\([^;]*&D_([0-9A-F]{8})\)', m.group(1)):
            a = int(t, 16) - OVL_BASE
            for k in range(16):
                w = struct.unpack_from('<I', ovl, a + 4 * k)[0] if a + 4 * k + 4 <= len(ovl) else 0
                if not OVL_BASE <= w < OVL_BASE + len(ovl):
                    break
                out.add(w)
    return out


def find_prop_scripts(scope, c):
    for x in re.findall(r'M2C_FIELD\(&D_([0-9A-F]{8}), s32 \*, 4\) = \(s32\) [^;]*->id;', c):
        PROP_SCRIPTS.add((scope, int(x, 16)))


ARRIVAL_SCRIPTS = [0x80081F94, 0x80081F74, 0x8008299C, 0x800829B4]


def main():
    a = sys.argv[1:]
    text_dir = a[a.index('--text') + 1] if '--text' in a else None
    only = a[a.index('--only') + 1] if '--only' in a else None
    src_dir, exe, root, out = a[:4]
    op_len, act_len, exe_img = EV.tables(exe)
    texts = EV.Text(text_dir) if text_dir else None
    os.makedirs(out, exist_ok=True)
    sources = []

    def comment_for(scope_text):
        def c(m):
            if texts is None:
                return ''
            s = scope_text(m)
            return '   \\ "%s"' % s.replace('\\n', ' ') if s else ''
        return c

    # the packs
    for f in sorted(os.listdir(src_dir)):
        g, t = int(f[:3]), int(f[4])
        if not (42 * 5 + 3 <= g * 5 + t <= 58 * 5):
            continue
        b = open(os.path.join(src_dir, f), 'rb').read()
        base, textp = struct.unpack_from('<II', b)
        first = min(x for x in struct.unpack_from('<%dI' % ((textp - base) // 4), b) if x > base + 8)
        ids = struct.unpack_from('<%dI' % ((first - base) // 4 - 2), b, 8)
        entries = {}
        for k, ad in enumerate(ids):
            if ad and ad not in entries:
                entries[ad] = 'script-%d' % k
        name = 'pack-%s' % f[:-4].replace('_', '-')
        pt = EV.pack_text(texts, f[:-4]) if texts else (lambda m: None)
        sources.append((Source(name, EV.Mem([(PACK_BASE, b)]), entries, ids), comment_for(pt),
                        'event pack %s (CD.BIN %d): %d script ids' % (f[:-4], g * 5 + t, len(ids))))
    # the overlays' scripts (started from their C, or from the main C by overlay address)
    import checks
    check_targets = checks.script_entries(checks.area_tables(exe, src_dir))
    main_c = open(os.path.join(root, 'src', 'main', 'main.c')).read()
    homes = EV.ovl_homes(src_dir, root, sorted(x for x in EV.started_scripts(main_c) if x >= OVL_BASE), op_len,
                         act_len, exe_img)
    for d in sorted(os.listdir(os.path.join(root, 'src'))):
        if not (d.startswith('ovl') and d[3:].isdigit() and len(d) == 6):
            continue
        area = d[3:]
        c = open(os.path.join(root, 'src', d, d + '.c')).read()
        find_prop_scripts(d, c)
        ovl_path = os.path.join(src_dir, '%s_0.bin' % area)
        ovl_img = open(ovl_path, 'rb').read() if os.path.exists(ovl_path) else None
        addrs = minigame_scripts(c, ovl_img) | set(EV.started_scripts(c)) | {x for ar, x in check_targets if ar == int(area) and x >= OVL_BASE} | \
            {x for x, h in homes.items() if h == area}
        if not addrs:
            continue
        ovl = open(os.path.join(src_dir, '%s_0.bin' % area), 'rb').read()
        tx = (lambda m, area=area: texts.get('%s_4' % area, m) if m < 10000 else
              texts.get('033_4', m - 10000) if m < 15000 else None) if texts else (lambda m: None)
        sources.append((Source(d, EV.Mem([(OVL_BASE, ovl), (EXE_BASE, exe_img)]),
                               {x: 'entry-%08X' % x for x in sorted(addrs)}), comment_for(tx),
                        'area %s overlay (CD.BIN %s_0): its scripts' % (area, area)))
    # walk all but main first: main also holds what the others call
    extern = set()
    for s, _, _ in sources:
        bad = walk(s, op_len, act_len)
        if bad:
            print('%s: undecodable at %s' % (s.scope, ' '.join('%08X' % x for x in bad)))
        extern |= s.extern
    # (and the minigames that are a script alone: src: minigame_main_start's)
    m = re.search(r'\nvoid minigame_main_start\(void\) \{\n(.*?)\n\}\n', main_c, re.S)
    main_minigames = set(int(x, 16) for x in re.findall(r'&D_([0-9A-F]{8})', m.group(1))) if m else set()
    # (and the car's: getting in, alone or with a passenger - src: func_8004175C starts them on a thread it picks
    # itself, not through event_start)
    car_scripts = {0x800965B0, 0x80096610}
    addrs = sorted(set(x for x in EV.started_scripts(main_c) if x < EXE_BASE + len(exe_img)) | main_minigames | car_scripts |
                   {x for _, x in check_targets if EXE_BASE <= x < EXE_BASE + len(exe_img)})
    tx = (lambda m: texts.get('033_4', m - 10000) if 10000 <= m < 15000 else None) if texts else (lambda m: None)
    main_src = Source('main', EV.Mem([(EXE_BASE, exe_img)]), {x: 'exe-%08X' % x for x in addrs})
    routes = route_addresses(exe, src_dir, root, exe_img)   # the townsfolk's routes (schedules.fs): by address
    for x in routes:
        main_src.entries.setdefault(x, 'route-%08X' % x)
    # and the routes the executable's helpers start on a person's thread (the overlays call them: Mel after
    # the diner, James, Lidds)
    for x in re.findall(r'event_thread_start\(\(EventThread \*\) g_actors\[\w+\]\.script, &D_([0-9A-F]{8})\)', main_c):
        main_src.entries.setdefault(int(x, 16), 'route-%s' % x)
    walk(main_src, op_len, act_len, extra=sorted(x for x in extern if x < OVL_BASE))
    for x in extern:
        if x < OVL_BASE and x not in main_src.entries:
            main_src.entries[x] = 'sub-%08X' % x
    # the scripts a warp starts on arriving (op 0x12A's last word): in the destination area's overlay
    arrivals = {}
    for s, _, _ in sources + [(main_src, None, None)]:
        for i in s.insns.values():
            if i.is_op and i.op == 0x12A and i.args[5]:
                arrivals.setdefault(i.args[0], set()).add(i.args[5] & 0xFFFFFFFF)
    # and those the overlays' C warps with (src: func_80019F3C(area, scene, x, z, heading, &script))
    for d in sorted(os.listdir(os.path.join(root, 'src'))):
        cp = os.path.join(root, 'src', d, d + '.c')
        if d.startswith('ovl') and os.path.exists(cp):
            for ar, x in re.findall(r'func_80019F3C\((0x[0-9A-Fa-f]+|\d+), [^;]*?&D_([0-9A-F]{8})\)', open(cp).read()):
                arrivals.setdefault(int(ar, 0), set()).add(int(x, 16))
    by_scope = {s.scope: s for s, _, _ in sources}
    for area, targets in sorted(arrivals.items()):
        scope = 'ovl%03d' % area
        if scope not in by_scope:
            p = os.path.join(src_dir, '%03d_0.bin' % area)
            if not os.path.exists(p):
                continue
            tx = (lambda m, area=area: texts.get('%03d_4' % area, m) if m < 10000 else
                  texts.get('033_4', m - 10000) if m < 15000 else None) if texts else (lambda m: None)
            src = Source(scope, EV.Mem([(OVL_BASE, open(p, 'rb').read()), (EXE_BASE, exe_img)]), {})
            sources.append((src, comment_for(tx), 'area %03d overlay (CD.BIN %03d_0): its scripts' % (area, area)))
            by_scope[scope] = src
        src = by_scope[scope]
        for x in targets:
            src.entries.setdefault(x, 'arrive-%08X' % x)
        bad = walk(src, op_len, act_len)
        if bad:
            print('%s: undecodable at %s' % (scope, ' '.join('%08X' % x for x in bad)))
        new = {x for x in src.extern if x < OVL_BASE and x not in main_src.entries}
        if new:
            walk(main_src, op_len, act_len, extra=sorted(new))
            for x in new:
                main_src.entries.setdefault(x, 'sub-%08X' % x)
    into_overlay = set(main_src.extern) | {x for x in main_src.entries if x >= OVL_BASE}
    sources.append((main_src, comment_for(tx), 'the main executable\'s scripts (started from its C, called by others)'))
    n_words = 0
    for s, tc, what in sources:
        if only and s.scope != only:
            continue
        uses = 'events' + (' events.main' if any(x < OVL_BASE for x in s.extern) and s.scope != 'main' else '')
        header = ['\\ %s.fs - %s.' % (s.scope, what),
                  '\\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand',
                  '\\ from here on.',
                  'IN: events.%s' % s.scope, 'USING: %s ;' % uses]
        lines = emit(s, main_src, header, tc)
        if s.scope.startswith('ovl'):   # its words main's scripts go to by address (in this area)
            reg = sorted(x for x in into_overlay | arrivals.get(int(s.scope[3:]), set()) if x in s.starts and x in s.insns)
            if reg:
                lines += ['', '\\ the words the main executable\'s scripts go to here, by address'] + [
                    "' %s $%08X %d overlay-word!" % (block_name(s, x), x, int(s.scope[3:])) for x in reg]
        if s.scope == 'main':   # the routes, found by address (townsfolk.fs)
            lines += ['', '\\ the townsfolk\'s routes, by address'] + [
                "' %s $%08X main-word!" % (block_name(s, x), x) for x in sorted(routes) if x in s.insns]
        if s.pack_ids is not None:   # the pack's scripts by number (load-pack, goto-pack-script ...)
            flat = int(s.scope[5:8]) * 5 + int(s.scope[9])
            lines += ['', '\\ its scripts by number'] + [
                "' %s %d %d pack-script!" % (block_name(s, x), k, flat) for k, x in enumerate(s.pack_ids)
                if x and x in s.insns]
        n_words += sum(1 for ln in lines if ln.startswith(': ') or ln == ':noname')
        open(os.path.join(out, s.scope + '.fs'), 'w', encoding='utf-8').write('\n'.join(lines) + '\n')
    if not only:   # one vocabulary loading them all (the story uses it)
        names = ['events.%s' % s.scope for s, _, _ in sources]
        body = ['\\ all.fs - every converted event file (tools/ev2forth.py).', 'IN: events.all', 'USING:']
        body += ['    ' + ' '.join(names[k:k + 6]) for k in range(0, len(names), 6)] + ['    ;']
        open(os.path.join(out, 'all.fs'), 'w', encoding='utf-8').write('\n'.join(body) + '\n')
    print('%d files, %d words -> %s' % (len(sources) if not only else 1, n_words, out))


if __name__ == '__main__':
    main()
