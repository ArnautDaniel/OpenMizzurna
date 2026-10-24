\ area 1's frame rules on the game's data (rules/area001.fs, src func_80102384): Nozz closing up in its scene 3 at
\ his hour (17:25 - 22:59; from the 29th 18:20), once; before the prologue's end, by the stairs, his shout once
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags look
    rules.area001 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
$55014 constant by-his-bed-x   $4638C constant by-his-bed-z
: to-his-room ( -- )  1 3 by-his-bed-x by-his-bed-z 0 0 warp ;
: in-his-room ( -- )  ['] to-his-room start-event  60 steps  events-stop  true broadcast player-control  deliver ;

testing the closing hours: 17:25 - 22:59 before the 29th, 18:20 - 22:59 from it
T{ 26 17 30 at-time  closing-time?  26 17 20 at-time  closing-time?  29 18 0 at-time  closing-time?  29 18 30 at-time  closing-time?
   -> true false false true }T
testing in scene 3 at his hour: Nozz closes up, once
T{ prologue-done flag-set  26 12 0 at-time  in-his-room  26 18 0 at-time  closed-up @  2 steps  closed-up @  events-running @ 0>
   -> 0 1 true }T
T{ events-stop  true broadcast player-control  deliver  4 steps  events-running @ -> 0 }T

test-summary
