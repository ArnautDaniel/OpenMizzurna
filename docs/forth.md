# The Forth

A small Forth in the spirit of jonesforth, written in C (`src/forth/forth.c`) with the rest of
the language in Forth (`scripts/prelude.fs`). It is not ANS Forth; it is a scripting language
for this game. Names are case-insensitive.

## The model

- **Cells** are pointer-sized integers, and addresses are real addresses: `@` and `!` reach C
  structs directly. Memory words come in sizes: `c@ w@ sw@ l@ sl@ @` (8/16/32 bits, signed
  variants) and their stores.
- **Floats** have their own stack (doubles). A number with a `.` or an exponent is a float:
  `1.5`, `2e`, `-0.25e3`. In memory floats are 32-bit like the game's: `sf@ sf!` (and `f@ f!` for
  a Forth `fvariable`, which holds a double).
- **Words** are indirect threaded: each has a code field (a C function) and a body. `see word`
  decompiles a colon definition.
- **Tasks**: every running piece of Forth has its own stacks. `' word spawn` starts one (it gives
  an id); the engine runs each ready task a little every tick. Inside a task, `yield` gives way
  until the next step and `n wait` for n steps - game code writes the time in seconds: `0.5e seconds wait`. `id kill` stops one, `.tasks` lists them.
  `spawn-held` starts one the engine leaves alone: `id resume` gives it its turn (whoever drives
  it decides when; it answers whether the task is still alive). `xt restart` makes the running
  task start over in another word at once, its stacks emptied (the event scripts' "go to").
- **A task that never yields** is stopped after about 20 million steps in one turn ("ran too
  long without yielding"), so one script can't freeze the game.
- **Errors** print a message with the file and line and unwind: at the prompt the stacks are
  emptied; a task that errs is stopped; an `on-tick` hook that errs is removed. `' word catch`
  runs a word and gives -1 instead of unwinding (`error-message` has the text).

## Vocabularies (as in Factor)

Words live in vocabularies. A file starts by saying where its definitions go and what it uses:

```forth
\ scripts/player.fs
IN: player
USING: engine keys vectors views state ;
```

- `USING: a b ;` loads each vocabulary that isn't loaded yet from its file - `a` is
  `scripts/a.fs`, `a.b` is `scripts/a/b.fs` - and makes its words visible here. `USE: a` does one.
- A file sees only its own words, the vocabularies it lists, and the core (`forth`: the
  language itself, from forth.c and prelude.fs). Nothing leaks in from files it didn't ask for.
- Its own definitions win over everything; a used vocabulary's win over the core. If two used
  vocabularies define the same name, the one later in `USING:` wins (list the general ones -
  `engine messages` - first, your own nearest last); `vocab:name` reaches the other.
- `<PRIVATE ... PRIVATE>` puts helpers in `name.private`: visible in the file, not to its users
  (`name.private:word` still reaches one, for debugging).
- Vocabularies loading each other (a cycle of USING:s) is an error.
- The prompt (console, `--eval`, the `forth` tool) sees every loaded vocabulary; `USING:` there
  narrows nothing, it loads. `vocabs` lists them, `vocab-words name` lists one, `words` lists
  what the current source sees.
- The engine's C words are the `engine` vocabulary.

## Writing words

```forth
: square ( n -- n*n ) dup * ;
variable lives   3 lives !
10 constant max-items
0 value score   score 1+ to score
fvariable speed  2.5e speed f!
defer on-hit   ' noop is on-hit   action-of on-hit   \ (what it does now: a hook's chain)
: array ( n "name" -- ) create cells allot does> ( i -- addr ) swap cells + ;
```

A deferred word runs its word in its place (no frame of its own). `xt jump` leaves the running word for xt -
a tail call: a chain of them never grows the return stack (the event scripts' `goto`; not from a word with
locals or a loop open). `flag abort" text"` is an error with that text when flag is true; `erase ( addr n -- )`
zeroes. A vocabulary's own word can shadow a core one of the same name (the actors' `spawn`): `forth:spawn`
reaches the core's.

Control: `if else then`, `begin until`, `begin while repeat`, `begin again`,
`do loop`, `?do loop`, `+loop`, `i j leave unloop`, `case of endof endcase`, `exit`, `recurse`.

Locals (Forth-2012's, with floats): `{: a b F: x | c F: y -- comment :}` inside a definition names
its values - `a` and `b` taken from the stack (`b` from the top), `x` from the float stack (`F:`
makes the names after it floats, until `|`), `c` and `y` starting at 0, the rest a comment. A name
gives its value; `to name` stores into it. They last until the word returns (`exit` too), the
return stack still free for `>r` and loops; a quotation has its own. A definition may have
several `{: :}`: each takes its values where it stands. The first makes the frame, so it can't be
inside an `if` or a loop; later ones can (`... do  i thing {: t :}  ... loop`). A local can't
take the name of an immediate word (`to`, `if`, `loop` ...): it would stop meaning itself. Prefer them
to stack juggling (`2 pick`, `rot`, `>r` chains) and to scratch variables:
`: centred ( addr len x y -- )  {: addr len x y :}  ... ;`.

Records: `begin-structure /point  ffield: p.x  ffield: p.y  end-structure` (`field:` a cell,
`ffield:` a float, `sffield:` / `lfield:` 32 bits, `cfield:` a byte, `n +field` n bytes); `/vec`
(vectors.fs) is the engine's three 32-bit floats (`v.x v.y v.z`).

Enumerations: `enum: mood  calm wary tense angry ;` makes the constants `calm` .. `angry` (0 up; a
number in the list sets the next: `enum: act  $36 hide  $52 lie-down ;`) and `mood>name ( n --
addr len )`, which names a value ("?" for one not listed) - for the debug words and traces. A long
list may run over several lines with comments (`\`, `( )`) between the names.

The dictionary's `find ( addr len -- xt | 0 )` is `find-word` once the prelude is in: its `find` is
the lists' (Factor's).

String lists: `strings: files  intro.sfd  DIR\menu.sfd  8 end.sfd ;` makes `files ( i -- addr len )`,
string i of the list - each a word as written, `\` and all - and "" for one not in it. As in
`enum:`, a number sets the next index (those skipped are ""), and comments may come between. The
rooms' string tables and the music's file names are written so (story/strings.fs).

Combinators (Factor's), to run code on values without shuffling them: `x [: .. ;] dip` (under
x), `x [: .. ;] keep` (x kept after), `2dip`, `2keep`, `x [: p ;] [: q ;] bi` (both on x), `tri`,
`x y [: q ;] bi@` (q on each), `flag [: .. ;] when` / `unless`, `flag a b ?` (a if the flag, else b).

Quotations: `[: ... ;]` leaves anonymous code as an execution token, inside definitions or at
the prompt (Factor's `[ ... ]`): `{ 1 2 3 } [: dup * ;] map`.

Lists: growable arrays of cells. `{ 1 2 3 }` makes one (in a definition, each time it runs),
`list` an empty one; `push ( x l -- )`, `pop`, `nth ( i l -- x )` (negative counts from the end),
`nth!`, `length`, `list-clear`, `list-free`. Higher-order words (prelude.fs): `each`, `map`,
`filter`, `reduce ( l acc xt -- acc' )`, `find ( l xt -- x true | false )`, `any?`, `all?`, `count`,
`contains? ( x l -- flag )`, `range ( n -- l )`, `.list`. Lists made while loading live for good;
free the ones made over and over at run time.

Strings: `s" text"` gives `addr len`; `." text"` prints; `type`, `s=`, `compare`.

Comments: `( ... )` and `\ to the end of the line`.

## Pitfalls

Each of these has cost someone time:

- **Your own words shadow everything**, the core too: a file that defines `move` has lost the core
  `move` (copying bytes) for the rest of its words - and gets no warning. Name words after what
  they mean here. `see word` shows what a name compiles to.
- **Control words compile only inside a definition**: `if`, `do`, `?do`, `case`, `begin` at the top
  of a file or at `--eval` are an error ("only inside a definition"). Wrap them: `: go ... ; go`.
- **`r@` inside a `do` loop is the loop's index**: the loop keeps its counters on the return
  stack, so what you `>r`'d before the loop is under them. Take it into a local instead.
- **The first `{: :}` of a definition** can't be inside an `if` or a loop; later ones can.
- **Floats** are on their own stack (`.s` shows both: the floats after `F:`). Memory floats are 32-bit
  (`sf@ sf!`); `f@ f!` are for an `fvariable`'s double.
- **Two vocabularies with one name**: the later in `USING:` wins; `vocab:name` reaches the other.
- **Messages**: a handler takes its message's arguments, all of them (docs/actors.md).

## Engine words (src/game/bind_engine.c, vocabulary `engine`)

The engine's words so far; the game's (the world, figures, sound, the disc's data) join as their subsystems
are built, each listed here.

| Word | Stack | |
|---|---|---|
| `key:` | `"name" -- scancode` | `key: C`, `key: Left_Shift` (SDL key names, `_` for spaces) |
| `key-down?`, `key-pressed?` | `scancode -- flag` | held now / went down this tick |
| `key-hold` | `scancode flag --` | hold a key from a script (demos, tests) |
| `on-tick`, `off-tick` | `xt --` | run a word every tick |
| `ticks` | `-- n` | simulation steps since start (for tools; game logic uses real time) |
| `dt` | `F: -- seconds` | a step's length (1/30 by default) |
| `seconds` | `F: s -- steps` | real time as steps, for `wait` (`0.5e seconds wait`); at least one for any time |
| `step-blend` | `F: -- 0..1` | how far the display is between the last step and the next (drawing only) |
| `on-draw`, `off-draw` | `xt --` | run a word every frame to draw 2D |
| `pen-color`, `pen-scale` | `rgba --`, `n --` | colour (0xRRGGBBAA) and text size for the drawing words |
| `draw-text`, `draw-rect` | `addr len x y --`, `x y w h --` | in window pixels from the top left |
| `screen-size`, `char-size` | `-- w h` | |
| `hud` | `addr len --` | a line of text at the bottom of the screen (`0 0 hud` clears) |
| `clear-color` | `F: r g b --` | |
| `screenshot` | `addr len --` | save the next frame as a PNG |
| `screenshot-pending?` | `-- flag` | |
| `console!` | `flag --` | open or close the console |
| `hidden?` | `-- flag` | no window (tests, screenshots) |
| `text-of` | `bank id -- addr len` | a message of the game's text (docs/subsystems/window.md); `0 0` if none |
| `font` / `font-palette` | `n --` | the game's font to draw with (0 the dialogue font, 1 the list font) / its palette |
| `font-text` | `addr len x y --` | text in the game's font at the pen's scale and colour |
| `font-width` | `addr len -- px` | its widest line in the font's pixels; `font-line` the line pitch (16), `font-ready?` |
| `exe-table` | `addr len -- addr n` | a table of the executable by name (`s" music" exe-table`); `0 0` if none |

The pad's buttons as keys: `scripts/keys.fs` (`pad-circle`, `pad-cross`, ... the d-pad `pad-up` ...).

## The scripts

`prelude.fs` (the language, `IN: forth`), then `game.fs` (`IN: game`), whose `USING:` pulls in the rest; each
file is a vocabulary of the same name. Each subsystem is its own file, with a folder of the same name when it
is bigger (the map: `docs/subsystems.md`). Shared: `keys.fs` (the pad), `messages.fs` (every message, declared
once).

The console (`` ` ``) evaluates whatever is typed, so any of this can be changed while the game runs: redefine
a word and the next tick uses it (words already compiled into others keep the old one; hooks are looked up by
execution token, so re-register with `on-tick` after a change, or go through a `defer`).

## Tests

`tester` (`scripts/tester.fs`, so any test file anywhere can use it): `T{ code -> results }T` runs
the code and checks the stack against the results; `testing name` labels what follows;
`test-summary` prints the totals and sets the exit status.

- `tests/test_*.fs`: the language and the kernel, run by the `forth` tool (no game).
- `tests/<subsystem>/test_*.fs`: the game's, headless - `openmizzurna --test file.fs` loads the scripts,
  runs the file and exits with its status. `game-tick` runs a tick,
  `deliver` delivers what is queued; `key: X true key-hold` holds a key. Run them with
  `SDL_AUDIO_DRIVER=dummy` (and `--hidden` for other headless runs).
- `ctest` runs both (later also the C checks of the library's conversions, `tests/*_check.c`).
