# Areas

`object-shown ( object flag )` shows or hides one of the area's objects until the next scene (src: object_show /
object_hide: the areas' hooks - the cemetery's day and night lights).

## Purpose

The area being played (src: `area.c` load_area, `model.c` func_8001D698, `draw.c` area_draw): its two model
groups - the shell and the objects, `NNN_4` part 3 (`models/NNN_4.3.0.gltf`, `.3.1.gltf`) - drawn at the
area's origin in the area's texture set; which objects each scene shows; each scene's camera profile. The
town (area 27) is drawn by its own subsystem (to come).

## Data (`NNN_4` part 0, the metadata: read where it lies, `library-file`)

Its first words are s16 offsets of its tables:

| table | holds | words |
|---|---|---|
| 0 header | s32 origin x, z; u8 shell objects, model objects | `header-table` |
| 1 shown | a byte a scene and object (shell objects first): shown or not | `shown?` |
| 2 cameras | per scene s16 eye height, look height, distance | `scene-camera` |
| 5 lights | per scene 0x22 bytes (to come: the light) | |
| 6 reverb | per scene {type, depth} (to come: the sound) | |

The origin's height and each floor's: the executable's `area-height` table (4 s16 an area: floor 0..3; the
origin stands on floor 1; src: func_80020738).

Object visibility as the original: object i shows in the shell when i is below the shell's count, and in
the objects' group by the same index (src: object_show).

## The objects' animations

An area's animated props - a door swinging, a fan - play the common props' motions (`cdbin/033_4/0/<n>.motion.txt`:
a frame's place, side height forward along the object's rest heading, and its turn added to its rest turn; src:
model.c func_8001DE24, func_8001E0F8, func_8001DFB4). The area's list (its metadata's table 3: six bytes an entry,
the object and the motion) names them: `play-area-anim ( entry )` (src: func_8001A150; the events' `object-anim`,
the doors' `door-anim`) plays one once - at 15 frames a second - and its object stays on the last frame; one held
plays again. Engine: `pose-object` (for the next `draw-model-shown`), `motion-turn`.

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

## Status

**Built**: areas indoors drawn with their scenes' objects (seen: Matthew's room). **To come**: the lights,
object animations, billboards and snow, reverb, the floors' walls and zones (with Matthew's walking), the town.
