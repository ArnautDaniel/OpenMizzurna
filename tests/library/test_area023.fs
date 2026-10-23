\ area 23's rules on the game's data (scripts/rules/area023.fs): the envelope placed until taken; the locked door
\ looked at, the search, three tries to force it, in; the room's scene once; the front door by the story
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags items talk
    rules.area023 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  10000 0 do  reply  started? 0= if  leave  then  loop ;
: in-the-house ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps ;
: started-then-over ( -- flag )  started?  played-out ;
prologue-done flag-set
27 12 0 at-time

testing on arriving: the envelope where it lies (not once it's found)
T{ found-cohen-envelope flag-clear  cohen-found-scene-seen flag-clear  in-the-house
   the-envelope person-now drop nip -> envelope-x envelope-z }T
testing the door: no search before it's looked at; looked at (locked); then nothing; the search finds a crowbar
T{ the-search-zone -1 look-check -> nothing-here }T
T{ 0 the-locked-door look-check dup the-door =  swap look-do  started-then-over  door-looked-at @ 0<> -> true true true }T
T{ 0 the-locked-door look-check  0 the-locked-door act-check -> nothing-here nothing-to-do }T
T{ the-search-zone -1 look-check dup search =  swap look-do  started-then-over  crowbar-found @ 0<> -> true true true }T
testing forcing it: two tries fail, the third opens it; then ○ goes in
T{ 0 the-locked-door act-check -> force-the-door }T
T{ force-the-door act-do played-out  force-the-door act-do played-out  door-forced @ -> 0 }T
T{ force-the-door act-do started-then-over  door-forced @ 0<>  0 the-locked-door act-check -> true true nothing-to-do }T
T{ 0 the-locked-door look-check -> the-door }T
testing his room: its scene once
T{ the-room scene-hook  started?  cohen-found-scene-seen flag? -> true true }T
T{ events-stop  the-room scene-hook  started? -> false }T

testing the envelope: □ takes it, once
T{ in-the-house  0 the-envelope-check act-check -> take-the-envelope }T
T{ take-the-envelope act-do  deliver  started?  the-envelope-item has-item?  found-cohen-envelope flag? -> true true true }T
T{ events-stop  0 the-envelope-check act-check -> nothing-to-do }T
T{ in-the-house  the-envelope here? -> false }T

testing the front door: found open and his room unseen, not out; else out to the town
T{ $01C story@ $180000 invert and $80000 or $01C story!  the-front-door  started-then-over  area @ -> true this-area }T
T{ cohen-found-scene-seen flag-set  the-front-door  started-then-over  area @ -> true the-town }T

test-summary
