\ area 7's rules on the game's data (scripts/rules/area007.fs): the cemetery - its timed scenes on arriving (each
\ once, playing to their ends), walking out of the grounds, the day's and the night's lights
USING: tester engine actors messages clock areas floors keys matthew townsfolk events window story-flags talk
    rules.area007 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  10000 0 do  reply  started? 0= if  leave  then  loop ;
: in-the-cemetery ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps ;
: inside ( -- )   \ a zone of the grounds (not the edge: zone 0 takes him out)
    1 0 0 1 zone-centre {: x z :}  x 0 z s" matthew" actor-named send figure-place  deliver ;
prologue-done flag-set

testing on arriving at the 26th's morning scene's time: it plays, once, to its end
T{ 26 9 45 at-time  kathy-funeral-seen flag-clear  in-the-cemetery  started?  kathy-funeral-seen flag? -> true true }T
T{ played-out  started? -> false }T
T{ in-the-cemetery  started? -> false }T
testing the evening's scene on arriving, once, to its end
T{ events-stop  26 21 0 at-time  cemetery-mel-meeting-overheard flag-clear  in-the-cemetery
   started?  played-out  started?  cemetery-mel-meeting-overheard flag? -> true false true }T

testing by day the day's lights (object 1), by night the night's (object 2)
T{ events-stop  26 12 0 at-time  in-the-cemetery  inside  2 steps  models-shown 1+ c@  models-shown 2 + c@ -> 1 0 }T
T{ 26 22 0 at-time  2 steps  models-shown 1+ c@  models-shown 2 + c@ -> 0 1 }T

testing walking out at the edge (zone 0): out to the town
: at-the-edge ( -- )  0 0 0 1 zone-centre {: x z :}  x 0 z s" matthew" actor-named send figure-place  deliver ;
T{ events-stop  26 12 0 at-time  in-the-cemetery  inside  2 steps  at-the-edge  2 steps  started?  played-out  area @
   -> true the-town }T

test-summary
