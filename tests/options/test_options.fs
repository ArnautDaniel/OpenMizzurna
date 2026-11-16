\ the options (scripts/options.fs, docs/subsystems/options.md): F1 opens them over the game, which then reads no
\ keys and stands; the arrows choose and change a row; F1 closes them and the game has its keys back
USING: tester engine actors messages keys ui settings options ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  clear-presses  loop ;
: press ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
key: F1 constant f1
2 steps   \ (the actors spawned: subscribed)

testing closed at first: the game has its keys
T{ options-up?  keys-captured?  pad-up true key-hold  pad-up key-down?  pad-up false key-hold  clear-presses -> false false true }T

testing F1 opens them: the keys captured, the world held
T{ f1 press  options-up?  keys-captured?  ui-busy? -> true true true }T
T{ pad-up true key-hold  pad-up key-down?  pad-up menu-key-down?  pad-up false key-hold  clear-presses -> false true }T

testing the volume row: right is louder, left quieter, within 0 .. volume-steps
T{ volume-steps volume!  pad-right press  volume -> volume-steps }T
T{ pad-left press  pad-left press  volume -> volume-steps 2 - }T

testing down to the vibration: right turns it off, again on
T{ true vibration!  vibration?  pad-down press  pad-right press  vibration? -> true false }T
T{ pad-right press  vibration? -> true }T

testing a mod's row (add-option): shown and changed like the others
variable mod-setting
: mod-shown ( -- addr len )  s" mod" ;
: mod-change ( dir -- )  mod-setting +! ;
T{ s" From a mod" ' mod-shown ' mod-change add-option  pad-up press  pad-up press  mod-setting @ -> 0 }T   \ (from the vibration: round to the last)
T{ pad-left press  mod-setting @ -> -1 }T

testing F1 closes them: the keys back, the world going on
T{ f1 press  options-up?  keys-captured?  ui-busy? -> false false false }T

test-summary
