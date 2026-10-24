# Errors

The actor every failure is told to (`scripts/errors.fs`, the actor `errors`, spawned first). Not the original's:
the engine's own way of keeping a game going past a script's error and saying where it was.

- **Told** by the kernel (`docs/actors.md`, "Failures"): a handler's error, and a task stopped by one (the event
  scripts), as `actor-failed ( record )`. The console still says each as it happens, with the words running.
- **The log**: `errors.log` beside the saves (`errors-log ( addr len -- )`, the engine's). Each run starts with a
  heading (the date, the version); each failure is a line (`frame 1234  window (on window-say): the error`) and
  the words running under it (`  in shut <- window:on window-shut`). The same failure again (who, message and
  error) is counted, not written: `(the error above: n times more)` when another comes, and each 300. Past 1 MiB
  the next run starts a fresh log, the last kept as `errors.old.log`.
- **Starting over**: the actor that failed is sent `start-over`, three times at most in 300 frames (ten seconds);
  failing more it is left as it is, and the log says so once. A task's failure has no one to ask. The start-overs
  in an actor's present window: `errors-start-overs ( id -- n )`.
- **Who starts over** (those whose failure would stop the game: something waits on them):
  - the window: shut, whatever it was doing - the clock let go, `window-closed` told to whoever waits;
  - the effects: none running (the events' `wait-effect` goes on).

  Any other actor may handle `start-over` and reset its own variables; without a handler the message is dropped
  quietly and the actor goes on as it was (the kernel already put its stacks back).

Tests: `tests/errors/test_errors.fs` (the log, the allowance, the window and the effects), `tests/test_actors.fs`
(the kernel's telling).
