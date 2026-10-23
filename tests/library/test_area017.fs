\ area 17's rules on the game's data (scripts/rules/area017.fs): with Winona along and off her schedule she leads
\ the way up on her own thread (he stays the player's) and waits at the locker; ○ there opens it (the items);
\ the desk after, with the camera; the way out not without her; closing time, once; set-stop (events.fs)
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags items talk
    rules.area017 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  10000 0 do  reply  started? 0= if  leave  then  loop ;
: in-the-school ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver  2 steps ;
: walked-up ( -- )  3000 0 do  10 steps  leading? 0= if  leave  then  loop ;
prologue-done flag-set
: has? ( item -- flag )  has-item? ;

testing set-stop -1: off her schedule (the minute's rules leave her be)
T{ 27 10 0 at-time  winona off-schedule? -> false }T
T{ winona 1 ride  winona -1 1 set-stop  deliver  winona off-schedule?  winona stop-of -> true -1 }T

testing on arriving with her along: she leads the way, he's still the player's; she ends at the locker
T{ in-the-school  leading?  started? -> true false }T
T{ walked-up  leading?  she-waits-there? -> false true }T
testing the way out: not without her
T{ the-way-out  started?  played-out -> true }T

testing ○ at the locker: it opens (the key spent, the floppy and the camera found), once
T{ the-locker-key give  deliver  0 the-locker-check look-check -> open-the-locker }T
T{ open-the-locker look-do  deliver  started?  the-locker-key has?  the-floppy has?  the-pocket-camera has? -> true false true true }T
T{ played-out  0 the-locker-check look-check -> nothing-here }T
testing the desk: with the camera, she's with him: the main building's key handed back
T{ the-main-key give  deliver  the-desk-zone -1 look-check -> sit-at-the-desk }T
T{ sit-at-the-desk look-do  deliver  started?  the-main-key has? -> true false }T

testing without her: the way out's scene, closing time shows him out to the town
T{ events-stop  winona 0 goto-stop  deliver  in-the-school  leading?  winona off-schedule? -> false false }T
T{ 27 18 0 at-time  2 steps  started?  closing-done @ 0<> -> true true }T
T{ played-out  area @ -> the-town }T

test-summary
