\ the day's light on the game's data (docs/subsystems/daylight.md)
USING: tester engine actors messages clock areas events daylight ;

: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: daylight-id ( -- id )  s" daylight" actor-named ;
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: weather! ( w -- )  true daylight-id send set-weather  deliver ;
: step ( -- )  broadcast tick  deliver ;
the-town 0 s" areas" actor-named send enter-area  deliver

testing the hour's values: at noon, clear, the first light is the table's (229 229 229, shining down and north)
T{ clear weather!  25 12 0 at-time  colors-now 0 light rgb@  ways 0 light rgb@ -> 229 229 229 -3 453 -891 }T

testing the minute's value between this hour's and the next's: 16:30, half way from 225 to 48
T{ 25 16 30 at-time  colors 0 light red @ -> 136 }T

testing the colours ease a step a frame; a jump in time sets them at once
T{ 25 17 0 at-time  colors-now 0 light red @  colors 0 light red @ = -> true }T
T{ 25 17 30 at-time  colors-now 0 light red @ -> 24 }T
T{ clear weather!  25 12 0 at-time  ambient-now rgb@ -> 63 63 75 }T
T{ snowing weather!  step  ambient-now red @  ambient red @ -> 64 67 }T

testing grey when it isn't clear: each colour its mean (and the ambient at least 63 63 75)
T{ snowing weather!  25 12 0 at-time  colors 1 light rgb@  rot over = -rot = and -> true }T

testing the weather: snow on the morning of 12/25 (unless an event holds it)
T{ clear false daylight-id send set-weather  deliver  25 9 0 date>time broadcast minute-passed  deliver  the-weather -> snowing }T

test-summary
