\ area 21's rules on the game's data (scripts/rules/area021.fs): Haines Diner - meeting Lorraine on the 25th,
\ today's special, closing time, the robbery on the 27th, Lorraine's conversations in turn
USING: tester engine actors messages clock areas matthew events story-flags talk rules.area021 ;

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

test-summary
