\ the title on the game's data (scripts/title.fs): the logo fades, the prologue's movie plays (Start skips it),
\ the title rises to its menu; Load shows the saves; vibration toggles (kept with the settings); New Game plays
\ the opening's movie and starts the game; an ending's movie can't be skipped, its card then fades in
USING: tester engine actors messages keys clock areas events story-flags settings saves title title.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;

testing the logo, then the prologue's movie playing; Start skips it to the title
T{ 0 title-start  title-up?  step-of-title @ -> true logo-state }T
T{ 60 steps  playing @ -> prologue }T
T{ pad-start tap  playing @  step-of-title @ -> -1 title-screen }T
testing Start on the title: it rises, the menu
T{ 120 steps  pad-start tap  80 steps  step-of-title @  cursor @ -> menu-state 0 }T
testing vibration: on the fourth line left or right toggles it (the settings keep it)
T{ vibration?  pad-up tap  cursor @  pad-right tap  vibration?  pad-left tap  vibration? -> true 3 false true }T
testing Load: "checking", then the saves' slots (or "no data"); ✕ back to the menu
T{ pad-down tap  pad-down tap  cursor @  pad-circle tap  step-of-title @ -> 1 load-state }T
T{ 10 steps  pad-cross tap  step-of-title @ -> menu-state }T
testing New Game: the opening's movie; skipped, the game starts (the title gone, an event running)
T{ pad-up tap  pad-circle tap  playing @ -> opening }T
T{ pad-start tap  2 steps  title-up?  events-running @ 0> -> false true }T
T{ events-stop  deliver -> }T
testing an ending: its movie can't be skipped; over, its card fades in
T{ bad-ending title-start  playing @  bad-end skippable?  pad-start tap  playing @ -> bad-end false bad-end }T
T{ after-movie  step-of-title @  fade @  40 steps  fade @ 0> -> bad-card 0 true }T
T{ false title-open !  movie-close -> }T

test-summary
