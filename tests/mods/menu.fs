\ The title's MODS and the mods menu (scripts/title.fs, scripts/mods/menu.fs) on the game's data: run with `--mods`
\ naming a folder of copies (CMakeLists.txt: mods_menu - the sample clock-line, the test mods layer-a and future-mod).
\ MODS under the menu's four lines; a mod turned off and moved up; ✕ back writes mods/order.txt; menu-after.fs then
\ starts OpenMizzurna again and finds that order in force.
USING: tester engine actors messages keys events title title.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
: row-name ( i -- addr len )  mods-menu-mod drop ;
: row-on? ( i -- flag )  mods-menu-mod nip nip ;

testing the folder's mods by name, each asked on; future-mod's reason kept
T{ mods-menu-count  0 row-on?  1 row-on?  2 row-on? -> 3 true true true }T
T{ 0 row-name s" clock-line" compare  2 row-name s" layer-a" compare  1 mods-menu-why nip 0> -> 0 0 true }T

testing the title's menu: MODS a fifth line, up from New Game
T{ 0 title-start  60 steps  pad-start tap  120 steps  pad-start tap  80 steps  step-of-title @ -> menu-state }T
T{ mods?  menu-lines  pad-up tap  cursor @ -> true 6 mods-line }T
T{ pad-circle tap  step-of-title @ -> mods-state }T

testing down to layer-a, off, moved to the top: changed
T{ pad-down tap  pad-down tap  pad-circle tap  pad-left tap  pad-left tap  mods-menu-changed? -> true }T
T{ 0 row-name s" layer-a" compare  0 row-on? -> 0 false }T
T{ s" layer-a" mod-on? -> true }T   \ (this run's as it started: the change applies next time)

testing ✕: order.txt written, back to the menu; down again wraps to New Game
T{ pad-cross tap  step-of-title @  mods-menu-changed? -> menu-state true }T
T{ pad-down tap  cursor @ -> 0 }T
T{ false title-open !  movie-close -> }T

test-summary
