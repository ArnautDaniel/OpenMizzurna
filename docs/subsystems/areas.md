# Areas

`object-shown ( object flag )` shows or hides one of the area's objects until the next scene (src: object_show /
object_hide: the areas' hooks - the cemetery's day and night lights).

## Purpose

The area being played (src: `area.c` load_area, `model.c` func_8001D698, `draw.c` area_draw): its two model
groups - the shell and the objects, `NNN_4` part 3 (`models/NNN_4.3.0.gltf`, `.3.1.gltf`) - drawn at the
area's origin in the area's texture set; which objects each scene shows; each scene's camera profile. The
town (area 27) is drawn by its own subsystem (town.md).

## Data (`NNN_4` part 0, the metadata: read where it lies, `library-file`)

Its first words are s16 offsets of its tables:

| table | holds | words |
|---|---|---|
| 0 header | s32 origin x, z; u8 shell objects, model objects | `header-table` |
| 1 shown | a byte a scene and object (shell objects first): shown or not | `shown?` |
| 2 cameras | per scene s16 eye height, look height, distance | `scene-camera` |
| 5 lights | per scene 0x22 bytes: its mode, level, ambient, three lights | daylight.fs |
| 6 reverb | per scene {type, depth} (the depth unused: every scene's 0x28) | `scene-reverb`, sound.fs |

A scene's record is read only where the file has it (`record-of`, `scene-record`, `meta-c@`): a scene past the
area's own (an event of another area sharing its scripts, as 29 does 2's) shows nothing and reads zeros - no
camera profile, light or reverb. A file shorter than the offsets (the town's: 4 bytes) is no table at all.

The origin's height and each floor's: the executable's `area-height` table (4 s16 an area: floor 0..3; the
origin stands on floor 1; src: func_80020738).

Object visibility as the original: object i shows in the shell when i is below the shell's count, and in
the objects' group by the same index (src: object_show).

## The objects' animations

An area's animated props - a door swinging, a fan - play the common props' motions (`cdbin/033_4/0/<n>.motion.txt`:
a frame's place, side height forward along the object's rest heading, and its turn added to its rest turn; src:
model.c func_8001DE24, func_8001E0F8, func_8001DFB4). The area's list (its metadata's table 3: six bytes an entry,
the object and the motion) names them: `play-area-anim ( entry )` (src: func_8001A150; the events' `object-anim`,
the doors' `door-anim`) plays one once - a frame each game frame - and its object stays on the last frame; one held
plays again. Engine: `pose-object` (for the next `draw-model-shown`), `motion-turn`. A frame's place is in the
original's object units, half the model's (src: func_8001E0F8 adds it to the place four times the SPL's, then
model_pose_update halves the offset from the parent): the engine scales it so (`OBJECT_PLACE_TO_GLTF`), whatever
the TMD's scale - a drawer pulled out four times too far before.

## The billboards

Some areas (7, 8, 9, 10, 13, 26: the outdoor ones) have billboards - trees, posts - each scene its set (the
library's `cdbin/NNN_4/5.billboards.txt`: `set <scene> count <n> at <halfword>`, then the entries), from the area's
origin; their sprites `sprites/NNN.txt`, `sprites/NNN/<i>.png` (cut at the import from the area's sheet). Drawn as
the town's (billboards.fs: a stand of them, turned to the camera's heading, in the first light's colour; src:
func_8003B2DC).

## Floors (`scripts/floors.fs`)

Each floor has an obstacle list and a zone list (`NNN_4` part 2: parts 0 / 1 floor 1; other floors by area,
src load_area). A region list: u16 header (count, in use, with ids, off), then boxes s16 x0 x1 z0 z1 (with
ids: u16 the id and "inverted"); z0 < z1 an upright box, else one turned 45 degrees (src: zone_inside).
`blocked? ( x z floor -- flag )`, `zone-at ( x z floor -- id | -1 )`, `zone-hit ( x z floor -- index id )`,
`zone-centre ( index x z floor -- x z )`. In the town the lists are the tile's the point is in
(`town/CC-RR/obstacles.bin`, `zones.bin`), measured from the tile's centre; a point off the map is blocked. The town's
tiles (`tile-of`, `tile-centre`, `on-map?`) and the ground under a point (`ground-at`, the town's terrain: filled in by
town.fs) are areas' words.

## API (`scripts/messages.fs`)

- `enter-area ( area scene -- )` - load and show it; broadcasts `area-entered ( area scene -- )`.
- `show-scene ( scene -- )` - another scene's objects; broadcasts `scene-shown` (the camera resets to its
  profile: src set_scene_state). The events' `set-scene`.
- Facts: `area`, `scene`, `area-origin`, `floor-height`, `scene-camera`.

## The game's rate

The original's numbers are per game frame, and how many it ran a second was its VSync pacing (src: D_800F8240):
4 (15 a second) indoors and while an event runs - `CEvCon_Start` sets it, in the town too - and 1 in the town
(~30 a second: the town took two of the screen's frames to draw), set again by `load_area` and when the last event
holding Matthew ends there. `game-rate ( F: -- frames/s )` is that (`event-paced`, set by the events'
`event-pacing` message and cleared on entering the town); `game-frames` the game frames a step covers;
`frames-due ( addr -- n )` the whole ones due, the rest kept. Every subsystem steps by these: figures' motions and
walking, the cars, the hooks, the town's objects.

## Every frame's hooks (src: func_800185A4, before drawing)

An area's rules give a word run once a game frame of the original's (`game-rate`, below) whatever Matthew is
doing: `area-every-frame! ( area xt -- )`. What they do:
- show or hide objects (`shown! ( object flag -- )`): the hospital's two sets by where he stands (2), the church's
  later set from the 25th 14:00 on entering (24), the falls' object from the 26th 10:00 (26), area 5's fire while
  it burns;
- turn objects (`turn-object ( object rx ry rz -- )`, kept and posed each draw until the area changes; src:
  func_80020144): the fans and lights turning $28 a frame (3, 12, 21), area 3's object on its side from the 26th
  15:00;
- step a texture animation (`texture-frame ( anim frame -- )`): the original copied blocks of video memory each
  frame; the import cuts each frame's textures (docs/library.md) and a material shows its frame's while it's
  set. Area 5's fire (16 frames), area 10's (scene 1) and area 23's (scene 0) strips (5, kept elsewhere), area
  26's waterfall (16) and the cabin's water (5), the town's water (6, a frame every other), the hospital's
  windows by the hour (the day's palette, from 16:00 the evening's: 2 and 29). Area 26's window is patched lit
  or dark by its events' C (two one-frame animations). Entering an area sets none.

Tested in `tests/library/test_area_drawn.fs`.

## The ground outdoors

In the forest (but the cabin's scene), the cemetery and the yard (its scene 0) the shell is the ground (src:
func_80020580, func_8001C3D4): Matthew and those there stand at its height (`ground-follows?`, `shell-ground` -
the shell's part 0; on the mountain the part by the tile, src mountain_part; the engine's `ground-part`). In the
town the tiles' terrain is (town.md). Elsewhere a floor's height holds. (`tests/library/test_blocking.fs`.)

## Status

**Built**: areas indoors drawn with their scenes' objects, the objects' animations, the frames' hooks and
texture animations. The lights (daylight.md), billboards and snow, reverb, the floors are their own pages.
