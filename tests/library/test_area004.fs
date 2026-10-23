\ area 4 on the game's data (no rules of its own: src ovl004 has none in use): James's invitation at his door
\ (knock.fs) - in to his house, the visit's scene, out to the town; once, then his door as usual
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk knock ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  20000 0 do  reply  started? 0= if  leave  then  loop ;
: area-seen ( -- )   \ (the area the scene's in, while it plays)
    20000 0 do  reply  area @ 4 = if  leave  then  started? 0= if  leave  then  loop ;
prologue-done flag-set

testing invited, James at home: his door takes Matthew in; the visit plays out, back to the town
T{ james-door-scene-seen flag-clear  $178 story@ 1 or $178 story!  0 visited-james-house story!
   events-stop  the-town 0 s" areas" actor-named send enter-area  deliver
   390585 0 417928 s" matthew" actor-named send figure-place  deliver
   30 23 0 at-time  james james-at-home at-record? -> true }T
T{ $1E knock  started?  area-seen  area @  james-door-scene-seen flag?  visited-james-house story@ -> 0 true 4 true 1 }T
T{ played-out  area @ -> the-town }T

test-summary
