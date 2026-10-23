# Areas

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

## API (`scripts/messages.fs`)

- `enter-area ( area scene -- )` - load and show it; broadcasts `area-entered ( area scene -- )`.
- `show-scene ( scene -- )` - another scene's objects; broadcasts `scene-shown` (the camera resets to its
  profile: src set_scene_state). The events' `set-scene`.
- Facts: `area`, `scene`, `area-origin`, `floor-height`, `scene-camera`.

## Status

**Built**: areas indoors drawn with their scenes' objects (seen: Matthew's room). **To come**: the lights,
object animations, billboards and snow, reverb, the floors' walls and zones (with Matthew's walking), the town.
