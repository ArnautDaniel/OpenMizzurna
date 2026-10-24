# The actor kernel

C (`src/kernel/`) keeps the actors, their mailboxes and the schedule, so delivery stays fast
and traceable. Forth defines everything else.

**Names.** In C the kernel's actor is an `MActor` ("Mizzurna actor"; OpenHuey's `HActor`). The original game
has its own `Actor` - the record of a character (the decomp's `g_actors`, Matthew in slot 0) - and the engine
will have animated figures; neither is a kernel actor. In the docs and scripts, "actor" means an MActor; a
character's drawn body is a "figure".

Every actor has an **id**: a small number, given by `spawn`, that messages are addressed to.

## Forth words

### Messages

```forth
message heard ( loud area floor source -- )   \ declares a message: a kind and its arity
```

A message has a kind (a small number) and up to 16 cell arguments (MMSG_ARGS). A float travels as a cell
(`f>cell` / `cell>f`). `message` takes the arity from the stack comment, so the comment is
the declaration.

```forth
loud area floor src  listener send heard      \ queue it for `listener`
tick-args  broadcast tick                         \ to everyone subscribed to `tick`
```

`send` and `broadcast` parse the message name that follows them, so a misspelt message is an
error when the script is compiled, not when it runs.

### Behaviours

```forth
behaviour listening
  on heard ( loud area floor src -- )  ... ;
  on tick ( -- )  ... ;
end-behaviour

behaviour stunned  extends listening   \ unhandled messages fall through to `listening`
  on tick ( -- )  ... ;
end-behaviour
```

A handler runs with its message's arguments on the stack, and must consume them. If it
doesn't, or it fails, the kernel names the actor and handler, and puts the stacks (data,
return, float) back as they were. A message the behaviour (and its parents) doesn't handle is
dropped, and the kernel counts it and reports it once per behaviour and kind (`.unhandled`).
The kernel's own messages (`tick`, `frame-end`, `spawned`, `killed`, `held-tick`, `actor-failed`,
`start-over`) are dropped quietly.

**Failures.** A handler that fails (and a task stopped by an error) is also told, as a record, to the actor
named by `errors-to ( id -- )` (`nobody`: none): `actor-failed ( record -- )`, from the actor that failed. The
record holds `failure-actor ( r -- id )` (`nobody` for a task's), `failure-who`, `failure-what` (the message it
was handling), `failure-error`, `failure-chain` (the words running) - each `( r -- addr len )` - and
`failure-frame`; the latest 32 are kept (an older one reads empty). The errors actor's own failures are only
said, never told to it. `start-over ( -- )` is the message an actor that can put itself right handles: the
errors actor (`docs/subsystems/errors.md`) sends it. Its state isn't wiped: what the actor keeps is its own to
reset (most keep it in their vocabulary's variables).

`become ( behaviour -- )` switches the current actor to another behaviour from the next
message on.

### State

```forth
state: listener-state
  cell  field threshold      \ quieter than this, it doesn't hear
  cell  field heard-loud
end-state
```

A field word gives the field's address in the **current** actor's state. The current actor is
the one whose message is being handled (`self`). Using a field with no current actor, or with
an actor whose state is of another type, is an error.

### Actors

```forth
listening listener-state  spawn" Kathy"  ( -- id )
id subscribe tick          \ (or inside a handler: self subscribe tick)
id kill
self  ( -- id )            \ the actor being run
sender  ( -- id )          \ who sent the message being handled (-1: the kernel)
```

### The schedule and tools

- `actors-frame ( -- )`: one frame (tick, deliver, frame-end, deliver). The engine calls it
  each tick, and tests call it directly.
- While the world is held (`pause!`: a menu is up) the engine runs a held frame instead:
  `held-tick` to its subscribers only, then deliver. Messages still reach every actor; only the
  frames stop (the sub screen subscribes to it, so it runs while Matthew and the town stand still).
- `deliver ( -- )`: deliver what is queued now (tests).
- `trace-on` / `trace-off`: print every message as it is delivered (`frame  from -> to  kind
  args`).
- `.actors`: every actor with its behaviour, its state type and its mailbox count.
- `.state ( id -- )`: every field of an actor's state, by name (a cell as a whole number or, when
  it reads as one, a float `1.5e`; a 12-byte vector as its three floats; other sizes as bytes).
- `.recent ( n -- )`: the last n messages delivered (the kernel keeps 256), oldest first, as
  `frame  from -> to  kind args`. Not the frames' own (`tick`, `frame-end`, `held-tick`), and not a
  message the same as its sender's last of that kind: the every-frame broadcasts show their
  changes, once (not a copy per subscriber).
- `enter ( id -- )` / `leave-actor`: the console inside an actor. (Not `leave`: that is the
  loop word.) Its fields, `self`, `become`
  and sends are that actor's, so `matthew enter  stamina @ .` reads Matthew's own stamina. (Later the
  console is an actor itself, and `enter` opens that actor's REPL.)

## C (`mactor.h`)

```c
void bind_mactor(Forth *f);                 /* the words (vocabulary `actors`) */
void mactor_frame(Forth *f);                /* one frame: the engine calls it each tick */
void mactor_held_frame(Forth *f);           /* the world held: `held-tick`, then deliver */
void mactor_send(int from, int to, int kind, const Cell *args, int n);
void mactor_broadcast(int from, int kind, const Cell *args, int n);
void mactor_reset(void);
```

The engine sends some messages itself. The first kinds are the kernel's: `tick`, `frame-end`,
`spawned`, `killed`, `held-tick`, `actor-failed`, `start-over`. The subsystems' own are declared in `scripts/messages.fs`
(`tick`, `frame-end`, `entered-room`, ...).

Tests: `tests/test_actors.fs` (`ctest`).

## Rules

- A handler never waits. Long sequences (a cutscene, a scripted walk) are **tasks** an actor
  starts. The task sends messages and can wait for frames or for a reply message.
- Actors may read facts (C world and asset data) directly, but never another actor's state.
- Replies are messages: to ask an actor something, send it a message and handle its answer.
