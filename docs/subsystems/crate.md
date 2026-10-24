# The crate

A crate Matthew pushes or climbs (src: player_misc.c `crate_area_update`, `crate_grab`, `crate_climb`; player.c's
pushing). Areas 10, 11, 16, 19 and 24 have one each; an area keeps its crate's place and state (`crate-place`,
`crate!`) and steps it from its frame hook (`crate-step`). `scripts/crate.fs`.

## Rules

- ○ on the crate (its examining: `crate-push`): he turns to it and reaches (motion $B5), then holds it ($B6) and
  pushes it as he walks - it stays 0xB4 ahead of him; where it would be blocked he stops where he was. His
  motions while pushing: $BA walking, $BD backing, $BB / $BC turning; no running. ○ lets go ($B8, $B9).
- □ on the crate (its taking: `crate-climb`): he climbs up ($BE, its move held) and stands square over it; up
  there his perch is 1 (the look rows marked `$4000` are his), ○ looks, □ climbs down the way he came ($BF).
- `crate-cancel`: let go / off at once (a scene takes over).
- Matthew's perch (`figure-perch`, the fact `matthew-perch`: 0 on his feet, 1 on a crate, 2 on a stool - src:
  talk_state) picks the look and act rows that apply: a row marked `$4000` only on a crate, `$2000` only on a
  stool, the rest only on his feet.

## Status

Built (areas 10's, 11's, 16's, 19's and 24's; 24's place is by the floor he's on, set while it's placing), with
the HUD's prompts while he holds it (○ $C lets go) or stands on it (□ $11 climbs down) - `crate-busy` (its area),
`crate-act-prompt`, `crate-look-prompt`. (The original's crate has no sounds of its own.)
