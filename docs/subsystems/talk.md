# Talking

## Purpose

○ on a person (src: cevcon.c func_800410B0, cobj.c actor_talk_start, the characters' handlers,
char_talk.c, the overlays' talk hooks).

## Who

The first person standing in the area (slot order) with a radius (`actor-radii`), close enough - closer than
both radii and 122 more - and in front of Matthew (within 568 of a turn either side of his heading). Found
before anything else ○ might mean (src: look_execute's case 1).

## What they say (`talk-with`), in layers as the original

1. **Their story conversation** (`conversation! ( xt slot -- )`: xt ( -- handled? )): the talks that move the
   story, by time and flags (src: the handlers' EM_EVTALK, char_talk.c). None written yet.
2. **The area's line for them** (`area-talk! ( xt area -- )`: xt ( slot -- handled? )): an area's lines by
   their stop (src: e.g. ovl001 func_80102D60; tables of (stop, message): `a-line-by-stop`). Area 1: Nozz.
3. **Their own line** (`default-line! ( xt slot -- )`; src: the handlers' EM_TALK: a line by their stop when
   between stops). Until written: "Hmm? What?" (the common text's 512: src 0x2910).

A line is a **one-liner** (src: func_8003FBFC and the executable's templates): the speaker's event - the
camera's talk shot about them (indoors -470 -908 -1370, heading 1856; the town's -474 -2210 -1106, 260),
Matthew standing, the speaker facing him, the line with its gesture. While its window is up the camera holds
and Matthew can't move (src: field_update skips both while ui_busy); control and the follow return as it closes.
With Matthew on a stool the line alone, no shot, no turning (src: template 0x80096230 - the game forces it then);
with the speaker on one, the shot but no turning, no gesture (0x80096298).

## Status

**Built**: who, the layers, one-liners, area 1's lines for Nozz. Seen: Nozz at his counter, "Bears on the
loose...". **To come**: the story conversations (char_talk.c: npc_talk and the event packs), every
character's own lines and the other areas' hooks, gestures, the portrait beside the box.
