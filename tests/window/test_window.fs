\ the message window (docs/subsystems/window.md): typing, pages, closing, questions, timed captions
USING: tester engine actors messages keys clock window ;

\ a message of our own, in place of the library's (a test has none): text-of's place
create sample 200 allot   variable sample-len
: sample! ( addr len -- )  dup sample-len !  sample swap move ;
s" one" sample!

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: press ( -- )  pad-circle true key-hold  1 steps  pad-circle false key-hold  1 steps ;
s" window" actor-named constant the-window
variable closed-with
behaviour window-listener
  on spawned ( -- )  self subscribe window-closed ;
  on window-closed ( answer -- )  closed-with ! ;
end-behaviour
state: no-state end-state
window-listener no-state s" window-test" spawn drop  deliver

testing the window's lines (a message set by hand: the library isn't there in a test)
: bars ( addr len -- addr len )   \ '|' a new line
    2dup 0 ?do  dup i + c@ [char] | = if  10 over i + c!  then  loop  drop ;
: put ( addr len -- )  2047 min dup text-len !  text swap move  count-lines lines ! ;
T{ s" one" put  lines @  0 line-start -> 1 0 }T
T{ s" ab|cd|ef" bars put  lines @  1 line-start  2 line-start  3 line-start -> 3 3 6 8 }T

testing a page: typed at 60 characters a second, then the box full; ○ scrolls a line in
: show ( addr len -- )  put  0 top !  0e typed f!  0e scrolled f!  true window-up !  typing stage !
    0e time-left f!  0 choices !  -1 window-answer !  box-x left ! ;
T{ s" 1234567890|1234567890|1234567890|1234" bars show  1e seconds steps  stage @ -> page-full }T
T{ press  stage @ -> scrolling }T
T{ 1e seconds steps  top @  stage @ -> 1 finished }T
T{ -2 closed-with !  press  window-up?  closed-with @ -> false -1 }T

testing a question: the cursor moves, ○ answers
T{ s" Take it?|Yes|Not now" bars show  2 choices !  1 first-choice !  0 cursor !  1e seconds steps  stage @ -> asking }T
T{ pad-down true key-hold  1 steps  pad-down false key-hold  1 steps  cursor @ -> 1 }T
T{ press  window-answer @  closed-with @ -> 1 1 }T

testing a timed caption closes itself
T{ s" Prologue" show  2e time-left f!  1e seconds steps  window-up? -> true }T
T{ 1e seconds steps  window-up? -> false }T

test-summary
