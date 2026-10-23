"""gltf_view.py <file.gltf> <out.png> [yaw] [pitch] [motion.txt frame]: a quick software render of one of the
library's models, optionally posed by a frame of a character motion (developers: a look without the engine;
needs numpy). Ours only: non-indexed triangles, node TRS."""
import json, struct, sys, os, math, zlib
import numpy as np
path, out = sys.argv[1], sys.argv[2]
yaw = math.radians(float(sys.argv[3]) if len(sys.argv) > 3 else 30); pitch = math.radians(float(sys.argv[4]) if len(sys.argv) > 4 else 25)
# optional: a character motion (.motion.txt) and a frame - each part's turn as that frame's (it replaces the rest turn)
pose = None
if len(sys.argv) > 6:
    want = int(sys.argv[6])
    for line in open(sys.argv[5]):
        w = line.split()
        if w and w[0] == 'frame' and int(w[1]) == want:
            t = list(map(int, w[w.index('turns') + 1:])); pose = [t[i:i+3] for i in range(0, len(t), 3)]
def game_quat(rx, ry, rz):
    a = [v * 2 * math.pi / 4096 for v in (rx, ry, rz)]
    cx, sx, cy, sy, cz, sz = math.cos(a[0]), math.sin(a[0]), math.cos(a[1]), math.sin(a[1]), math.cos(a[2]), math.sin(a[2])
    r = np.array([[cy*cz + sy*sx*sz, -cy*sz + sy*sx*cz, sy*cx], [cx*sz, cx*cz, -sx], [-sy*cz + cy*sx*sz, sy*sz + cy*sx*cz, cy*cx]])
    f = np.array([1, -1, -1]); return r * f[:, None] * f[None, :]
g = json.load(open(path)); base = os.path.dirname(path)
bin_ = open(os.path.join(base, g['buffers'][0]['uri']), 'rb').read()
def acc(i):
    a = g['accessors'][i]; v = g['bufferViews'][a['bufferView']]
    n = {'VEC2': 2, 'VEC3': 3, 'VEC4': 4}[a['type']]
    return np.frombuffer(bin_, '<f4', a['count'] * n, v.get('byteOffset', 0)).reshape(-1, n)
def png_rgba(p):
    d = open(p, 'rb').read(); pos = 8; w = h = 0; idat = b''; ct = 0; plte = None; trns = None
    while pos < len(d):
        ln = struct.unpack('>I', d[pos:pos+4])[0]; t = d[pos+4:pos+8]; c = d[pos+8:pos+8+ln]
        if t == b'IHDR': w, h, bd, ct = struct.unpack('>IIBB', c[:10])
        elif t == b'IDAT': idat += c
        elif t == b'PLTE': plte = c
        elif t == b'tRNS': trns = c
        pos += 12 + ln
    raw = zlib.decompress(idat); bpp = 4 if ct == 6 else 3 if ct == 2 else 1
    stride = w * bpp; out = np.zeros((h, stride), np.uint8); prev = np.zeros(stride, np.int32)
    for y in range(h):
        f = raw[y*(stride+1)]; line = np.frombuffer(raw, np.uint8, stride, y*(stride+1)+1).astype(np.int32)
        cur = np.zeros(stride, np.int32)
        for x in range(stride):
            a = cur[x-bpp] if x >= bpp else 0; b = prev[x]; c = prev[x-bpp] if x >= bpp else 0
            pr = 0 if f == 0 else a if f == 1 else b if f == 2 else (a+b)//2 if f == 3 else (a if abs(b-c) <= abs(a-c) and abs(b-c) <= abs(a+b-2*c) else b if abs(a-c) <= abs(a+b-2*c) else c)
            cur[x] = (line[x] + pr) & 255
        out[y] = cur; prev = cur
    return out.reshape(h, w, bpp) if bpp == 4 else None
def quat_m(q):
    x, y, z, w = q
    return np.array([[1-2*(y*y+z*z), 2*(x*y-z*w), 2*(x*z+y*w)], [2*(x*y+z*w), 1-2*(x*x+z*z), 2*(y*z-x*w)], [2*(x*z-y*w), 2*(y*z+x*w), 1-2*(x*x+y*y)]])
tris = []
texcache = {}
def walk(ni, M):
    nd = g['nodes'][ni]; L = np.eye(4)
    L[:3, :3] = quat_m(nd.get('rotation', [0, 0, 0, 1])); L[:3, 3] = nd.get('translation', [0, 0, 0])
    if pose is not None and ni < len(pose):
        L[:3, :3] = game_quat(*pose[ni])
    W = M @ L
    if 'mesh' in nd:
        for pr in g['meshes'][nd['mesh']]['primitives']:
            P = acc(pr['attributes']['POSITION']); P = (W[:3, :3] @ P.T).T + W[:3, 3]
            C = acc(pr['attributes']['COLOR_0'])
            UV = acc(pr['attributes']['TEXCOORD_0']) if 'TEXCOORD_0' in pr['attributes'] else None
            mat = g['materials'][pr['material']]; tex = None
            if UV is not None:
                uri = g['images'][g['textures'][mat['pbrMetallicRoughness']['baseColorTexture']['index']]['source']]['uri']
                f = os.path.normpath(os.path.join(base, uri))
                if f not in texcache: texcache[f] = png_rgba(f) if os.path.exists(f) else None
                tex = texcache[f]
            for i in range(0, len(P), 3): tris.append((P[i:i+3], C[i:i+3], None if UV is None else UV[i:i+3], tex))
    for c in nd.get('children', []): walk(c, W)
for r in g['scenes'][0]['nodes']: walk(r, np.eye(4))
allp = np.concatenate([t[0] for t in tris]); centre = (allp.max(0) + allp.min(0)) / 2; size = (allp.max(0) - allp.min(0)).max()
cy, sy, cp, sp = math.cos(yaw), math.sin(yaw), math.cos(pitch), math.sin(pitch)
R = np.array([[cy, 0, -sy], [0, 1, 0], [sy, 0, cy]]); R = np.array([[1, 0, 0], [0, cp, -sp], [0, sp, cp]]) @ R
W_, H_ = 640, 480; img = np.full((H_, W_, 3), 40, np.uint8); zb = np.full((H_, W_), -1e9)
s = min(W_, H_) * 0.9 / size
for P, C, UV, tex in tris:
    q = (R @ (P - centre).T).T; xs = q[:, 0] * s + W_/2; ys = H_/2 - q[:, 1] * s; zs = q[:, 2]
    x0, x1 = int(max(0, min(xs))), int(min(W_-1, max(xs))); y0, y1 = int(max(0, min(ys))), int(min(H_-1, max(ys)))
    if x1 < x0 or y1 < y0: continue
    d = (xs[1]-xs[0])*(ys[2]-ys[0]) - (xs[2]-xs[0])*(ys[1]-ys[0])
    if abs(d) < 1e-9: continue
    X, Y = np.meshgrid(np.arange(x0, x1+1)+0.5, np.arange(y0, y1+1)+0.5)
    w1 = ((X-xs[0])*(ys[2]-ys[0]) - (xs[2]-xs[0])*(Y-ys[0])) / d; w2 = ((xs[1]-xs[0])*(Y-ys[0]) - (X-xs[0])*(ys[1]-ys[0])) / d; w0 = 1-w1-w2
    m = (w0 >= 0) & (w1 >= 0) & (w2 >= 0)
    if not m.any(): continue
    z = w0*zs[0] + w1*zs[1] + w2*zs[2]
    col = (w0[..., None]*C[0, :3] + w1[..., None]*C[1, :3] + w2[..., None]*C[2, :3])
    if UV is not None and tex is not None:
        u = (w0*UV[0, 0] + w1*UV[1, 0] + w2*UV[2, 0]) * 256; v = (w0*UV[0, 1] + w1*UV[1, 1] + w2*UV[2, 1]) * 256
        t = tex[np.clip(v.astype(int), 0, 255), np.clip(u.astype(int), 0, 255)]
        m = m & (t[..., 3] > 0); rgb = t[..., :3] * col
    else:
        rgb = col * 255
    sub = zb[y0:y1+1, x0:x1+1]; m = m & (z > sub)
    sub[m] = z[m]; img[y0:y1+1, x0:x1+1][m] = np.clip(rgb[m], 0, 255).astype(np.uint8)
raw = b''.join(b'\0' + img[y].tobytes() for y in range(H_))
def chunk(t, b): return struct.pack('>I', len(b)) + t + b + struct.pack('>I', zlib.crc32(t + b) & 0xffffffff)
open(out, 'wb').write(b'\x89PNG\r\n\x1a\n' + chunk(b'IHDR', struct.pack('>IIBBBBB', W_, H_, 8, 2, 0, 0, 0)) + chunk(b'IDAT', zlib.compress(raw)) + chunk(b'IEND', b''))
print(len(tris), 'triangles')
