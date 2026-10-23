\ area 5 on the game's data (scripts/rules/area005.fs): Wolf's warehouse - the finale's chain of scripts plays
\ from the warp in to the warp out to the town (its fights and shoot-out passed over until the minigames exist)
USING: tester engine actors messages clock areas keys matthew events window story-flags rules.area005 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  20000 0 do  reply  started? 0= if  leave  then  loop ;

testing the warp in (src: ovl027 0x8010AA78): the arrival's script, the chain to its end - out to the town
: in-to-the-warehouse ( -- )  this-area 0 112815 51313 -3192 $80104614 warp ;
T{ 30 23 0 date>time s" clock" actor-named send set-time  deliver
   ' in-to-the-warehouse start-event  played-out  started?  area @ -> false the-town }T

test-summary
