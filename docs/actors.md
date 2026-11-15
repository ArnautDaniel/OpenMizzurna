# The actor kernel

The kernel is ClankForth's (the submodule `external/clankforth`): its page,
[external/clankforth/docs/actors.md](../external/clankforth/docs/actors.md), has messages, behaviours, state,
spawning and killing, handlers that wait (`wait`, `await`), monitors and links, failures, the schedule and the tools
(`.actors`, `.state`, `.recent`, `trace-on`, `enter`). This page has what is OpenMizzurna's: the names, the bodies,
the engine's side, the rules the game keeps.

**Names.** In C the kernel's actor is an `MActor` ("Mizzurna actor"; OpenHuey's `HActor`). The original game
has its own `Actor` - the record of a character (the decomp's `g_actors`, Matthew in slot 0) - and the engine
has animated figures (game/actor.h); neither is a kernel actor. In the docs and scripts, "actor" means an MActor; a
character's drawn body is a "figure".

**Ids.** An actor's id names it for good: its slot (0 .. `actors-max` - 1) and, over it, how many have had the slot
before, so an id kept after its actor is gone names nobody (`alive?` is false, `send` to it a dead letter). A table
with a cell for each actor is indexed by `actor-slot ( id -- n )`, never by the id, and keeps whose each entry is
(the slot's next actor is another: `scripts/errors.fs`, the start-over allowance).

**Failures.** A handler's error is told to the errors actor (`docs/subsystems/errors.md`), spawned first and named
with `errors-to`. With none named an actor that fails dies of it, so the game names one before any actor runs,
and again after `actors-reset` (which forgets it); a test that makes a handler fail names one first.

## Bodies

Where a character physically is (docs/design.md, "Bodies"): a fact anyone may read, set only by the actor itself.
Each actor has one (`MBody`, src/kernel/mactor.h), its words in the vocabulary `actors` beside the kernel's.

```forth
area floor  x y z  body-place    \ ( area floor -- ) ( F: x y z -- ) in that area, on that floor, there
yaw body-turn   radius height body-size   body-off   \ (out of the world: in no area)
area body-area!   floor body-floor!
id body?  id body-area  id body-floor  id body-pos ( F: -- x y z )  id body-yaw  id body-dims ( F: -- radius height )
id body-at ( -- v )              \ its position as a vector (read it, don't write it)
x y z body-head!   id body-head ( -- v )        \ where its head is (else the top of its body)
```

## C (`mactor.h`)

```c
void bind_mactor(Forth *f);                 /* the kernel's words and the bodies' (vocabulary `actors`) */
void mactor_frame(Forth *f);                /* one frame: the engine calls it each tick */
void mactor_held_frame(Forth *f);           /* the world held: `held-tick`, then deliver */
void mactor_send(int from, int to, int kind, const Cell *args, int n);
void mactor_broadcast(int from, int kind, const Cell *args, int n);
void mactor_reset(void);
MBody *mactor_body(int id);                 /* NULL for an id whose actor is gone */
```

These are the kernel's calls (`forth_actors_frame`, `forth_actor_send` ...) under the names the engine's C uses,
with the game's one Forth at hand. While the world is held (`pause!`: a menu is up) the engine runs a held frame:
`held-tick` to its subscribers only, then deliver. Messages still reach every actor; only the frames stop (the sub
screen subscribes to it, so it runs while Matthew and the town stand still).

The engine sends some messages itself. The first kinds are the kernel's: `tick`, `frame-end`, `spawned`, `killed`,
`held-tick`, `actor-failed`, `start-over`, `died`. The subsystems' own are declared in `scripts/messages.fs`.

Tests: `tests/test_actors.fs` (the kernel as the game uses it; ClankForth's own are its `tests/test_actors.fs`,
both run by `ctest`).

## Rules

- A handler never waits. Long sequences (a cutscene, a scripted walk) are **tasks** an actor
  starts. The task sends messages and can wait for frames or for a reply message. (The kernel allows a handler
  that waits; the game hasn't needed one yet.)
- Actors may read facts (C world and asset data) directly, but never another actor's state.
- Replies are messages: to ask an actor something, send it a message and handle its answer.
