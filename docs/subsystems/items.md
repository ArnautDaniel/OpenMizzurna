# Items

## Purpose

What Matthew carries (src: `items.c`): up to 27 items, each once, in the order taken; a removed one's place
taken by the last. A new game starts with the phone (0), the waist bag (3) and the watch (4).

## API

`give-item ( item -- )`, `take-item ( item -- )`, `items-reset ( -- )`; the fact `has-item? ( item -- flag )`.
Named so far: `cell-phone waist-bag watch isabellas-letter lighter cigarettes` (the rest with the item ring and
its text).

## Status

**Built**: the list (`scripts/items.fs`); the item ring and using items (`docs/subsystems/ring.md`).
