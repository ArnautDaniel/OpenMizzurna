\ keys.fs - the PlayStation pad's buttons as the keys the scripts read (src/platform/platform.c binds them:
\ the keyboard's own keys, a gamepad's buttons, the player's choices). Read them with key-down? / key-pressed?.
IN: keys
USING: engine ;

key: Up constant pad-up         key: Down constant pad-down
key: Left constant pad-left     key: Right constant pad-right
key: C constant pad-circle      \ look, talk, confirm
key: X constant pad-cross       \ run, cancel
key: Z constant pad-square
key: V constant pad-triangle    \ the item ring
key: Q constant pad-l1          key: E constant pad-r1
key: 1 constant pad-l2          key: 3 constant pad-r2
key: Return constant pad-start  \ pause
key: Tab constant pad-select

\ the second pad (the two-player fight's: a second gamepad, or these keys)
key: I constant pad2-up         key: K constant pad2-down
key: J constant pad2-left       key: L constant pad2-right
key: O constant pad2-circle     key: U constant pad2-cross
key: Y constant pad2-square     key: P constant pad2-triangle
key: 7 constant pad2-l1         key: 9 constant pad2-r1
key: H constant pad2-start      key: G constant pad2-select
