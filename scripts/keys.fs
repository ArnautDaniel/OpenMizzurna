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
