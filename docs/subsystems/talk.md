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
   story, by time and flags (src: the handlers' EM_EVTALK, char_talk.c). `scripts/conversations.fs`, loaded
   before the areas' rules so its pickers come first: Barton, James, Liz, Mel, Morgan (and his statement with the
   photo in hand), Robert, Samuel, Winona (and the locker key), Nozz, Cohen, Dennis - each tries its talks in the
   original's order, once each (a story word marks it), and plays the first from the character's pack
   (`npc-talk ( slot n )`: the executable's `character-packs` by the character index - src npc_talk,
   event_start_talk: they turn to him and stand).
2. **The area's line for them** (`area-talk! ( xt area -- )`: xt ( slot -- handled? )): an area's lines by
   their stop (src: e.g. ovl001 func_80102D60; tables of (stop, message): `a-line-by-stop`). Area 1: Nozz.
3. **Their own line** (`default-line! ( xt slot -- )`; src: the handlers' EM_TALK): walking between stops, the
   line for the stop they left (the executable's `lines-<name>`; Samuel's a table of stop and line); else their
   own fallback (Morgan's, Steve's ... "Hmm? What?", 0x2910; Isabella's and Lorraine's 0x2911; Barton's 0x2912;
   Dennis's 0x2913; Mel's 0x2914; Winona's 0x2916, and at her first stop on the 25th her first meetings). Anyone
   else: "Hmm? What?".

A person's story talks are a list (`conversation!` adds one after those they have; the first with something to
say wins): their own, then an area's (Club Wolves' for Wolf, James, Mel ...: each checks the area itself).

A line is a **one-liner** (src: func_8003FBFC and the executable's templates): the speaker's event - the
camera's talk shot about them (indoors -470 -908 -1370, heading 1856; the town's -474 -2210 -1106, 260),
Matthew standing, the speaker facing him, the line with its gesture. While its window is up the camera holds
and Matthew can't move (src: field_update skips both while ui_busy); control and the follow return as it closes.
With Matthew on a stool the line alone, no shot, no turning (src: template 0x80096230 - the game forces it then);
with the speaker on one, the shot but no turning, no gesture (0x80096298).

## Status

**Built**: who, the layers, one-liners, the characters' story conversations and own lines, the areas' hooks
(their rules) - `tests/library/test_conversations.fs`. **To come**: showing them items (the item ring: their
reactions, src CMD_ITEM_QUERY / CMD_ITEM_SHOW), the townspeople's lines by their kind (src func_800300E4), the
waitresses' and the protesters' lines, gestures, the portrait beside the box.
