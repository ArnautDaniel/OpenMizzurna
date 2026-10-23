\ the sky on the game's data (docs/subsystems/sky.md)
USING: tester engine actors messages clock areas events daylight sky ;

: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: weather! ( w -- )  true s" daylight" actor-named send set-weather  deliver ;
the-town 0 s" areas" actor-named send enter-area  deliver

testing its pictures, cut at the import: the clouds 256 x 240, the horizon strip 256 x 64, the sun 64 x 64
T{ s" sky/clouds.png" image image-size  s" sky/horizon.png" image image-size  s" sky/sun.png" image image-size
   -> 256 240 256 64 64 64 }T
T{ s" sky/moon-3.png" image 0<>  s" sky/flare-14.png" image 0<> -> true true }T

testing the hour's colours: at noon, clear, the top of the sky is the table's; a jump in time sets it at once
T{ clear weather!  26 12 0 at-time  top-now rgb@ -> 144 160 176 }T
T{ 26 0 0 at-time  top-now rgb@ -> 0 0 16 }T

testing not clear: the sum of the three halved, kept as a byte (as the game did)
T{ overcast weather!  26 12 0 at-time  top-now red @ -> 144 160 + 176 + 2/ 255 and }T

test-summary
