\ area 21's rules on the game's data (scripts/rules/area021.fs): Haines Diner - meeting Lorraine on the 25th,
\ today's special, closing time, the robbery on the 27th, Lorraine's conversations in turn
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk list-menu counter
    rules.area021 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
329445 constant inside-x   269064 constant inside-z   \ (where the getaway's script brings him back in)
: in-diner ( -- )   \ in its dining room, standing inside the door
    events-stop  this-area 0 s" areas" actor-named send enter-area  deliver
    inside-x 0 inside-z s" matthew" actor-named send figure-place  deliver  2 steps ;

testing 12/25 at noon, the first time: he meets Lorraine; today's special is the grits
T{ 25 12 0 at-time  diner-lorraine-intro-seen flag-clear  in-diner
   started?  diner-lorraine-intro-seen flag?  special-line @ -> true true 238 }T

testing at 20:00 before the 28th (from 19:00 after): Lorraine closes up, once a visit
T{ 26 20 30 at-time  diner-mel-lorraine-argued flag-set  in-diner  started?  closing-done @ -> true 1 }T
T{ events-stop  5 steps  started? -> false }T
T{ 28 19 10 at-time  in-diner  started?  special-line @ -> true 241 }T

testing 12/27 11:30 - 11:59, in the dining room: the robbery, once
T{ 27 11 40 at-time  diner-robbery-seen flag-clear  in-diner  started?  diner-robbery-seen flag? -> true true }T
T{ in-diner  started? -> false }T

testing Lorraine: the robbery's confession first, then her talks in turn, each once; then her line
T{ events-stop  lorraine-robbery-confessed flag-clear  lorraine-talks  lorraine-robbery-confessed flag? -> true true }T
T{ events-stop  0 asked-about-emma story!  lorraine-talks  asked-about-emma story@ -> true 1 }T
T{ events-stop  0 startled story!  lorraine-talks  startled story@ -> true 1 }T

testing the stools: on the 25th at 15:30 Winona sits at the counter (a stop "at the diner"), on a stool of its own
T{ 25 15 30 at-time  winona seated?  winona seat-of drop  winona person-of person-area @ -> true at-diner this-area }T
: on-her-stool? ( -- flag )
    winona seat-of {: b n :}  b n sitter winona =  b n seat-spot drop {: x y z :}  winona person-now drop
    z = swap y = and swap x = and and ;
T{ on-her-stool? -> true }T
testing a jump to the 26th puts her elsewhere: the stool is free again
T{ winona seat-of  26 0 0 at-time  sitter  winona seated? -> -1 false }T

testing a scene walks Matthew and turns him: there at its pace, then round at its speed (src: walk_to, turn_to)
328354 constant to-x   270184 constant to-z
: walk-there ( -- )  matthew to-x to-z walk-to  matthew wait-stop  matthew 1024 120 turn-to  matthew wait-turned
    end-script ;
: scene-over ( -- )  300 0 do  started? 0= if  leave  then  1 steps  loop ;
T{ 25 10 0 at-time  in-diner  events-stop  ' walk-there start-event  scene-over  started? -> false }T
: where-he-is ( -- x z heading )  matthew-now {: x y z h :}  x z h ;
T{ where-he-is -> to-x to-z 1024 }T

testing the counter: □ before a free stool, facing it, sits him down (the pad's now the stool's)
328450 constant before-the-stools   4 constant stool-4   269930 constant stool-4-z
: answer ( -- )   \ ○ on a line that waits for it
    window-up? stage @ finished = stage @ page-full = or and pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: answering ( n -- )  0 ?do  answer  loop ;
: press ( key -- )  dup true key-hold  2 steps  false key-hold  2 steps ;
: held ( key n -- )  over true key-hold  steps  false key-hold ;
: at-the-stools ( -- )
    before-the-stools 0 stool-4-z s" matthew" actor-named send figure-place  0 3072 0 s" matthew" actor-named send figure-turn
    deliver  2 steps ;
T{ 25 13 0 at-time  diner-lorraine-intro-seen flag-set  in-diner  events-stop  at-the-stools
   pad-square press  60 steps  at-the-counter?  matthew seat-of  matthew-on-foot? -> true at-diner stool-4 false }T
testing sitting down sounds the diner's own (src: its sit_sound)
T{ sound:last-sound @ -> $70 }T
testing □ waves Lorraine over: she comes to his stool
T{ pad-square 10 held  200 steps  barman-here -> stool-4 }T
testing □ again orders: her welcome, then the menu; ○ picks the pancakes, she brings them; □ eats
: until-the-menu ( -- )  100 0 do  answer  menu-up? if  leave  then  loop ;
T{ pad-square 3 held  until-the-menu  menu-up? -> true }T
T{ pad-down press  pad-down press  pad-circle press  400 answering  menu-up? -> false }T
T{ pad-square 3 held  600 answering  events-running @  orders @ -> 0 1 }T
testing leaning toward the next stool, □ gets him up: on his feet, the stool free
testing leaning, held: □'s prompt to get up (none for ○ with the next stool free)
T{ pad-left 20 held  pad-left true key-hold  20 steps  counter-act-prompt @  counter-look-prompt @ -> $B -1 }T
T{ pad-square press  pad-left false key-hold  100 steps
   at-the-counter?  at-diner stool-4 seat-taken?  matthew-on-foot? -> false false true }T

test-summary
