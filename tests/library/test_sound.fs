\ the sound on the game's data (scripts/sound.fs, audio/): the common bank open from the start; an area's bank
\ and sequences on entering it; an effect once (some of the areas' as several voices), dying away; a held loop
\ until stopped; a sequence; the area's ambience as its background, faded and stopped in its cycle; a scene's
\ music in its place; silence outdoors at night; an XA stream putting the background aside
USING: tester engine actors messages clock areas keys events story-flags matthew sound sound.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: the-sound ( -- id )  s" sound" actor-named ;
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: into ( area -- )  events-stop  all-voices-off  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: a-second ( -- )  audio-rate audio-run ;
: hush ( -- )  -1 the-sound send play-music  deliver  all-voices-off ;   \ (the background's notes aside)
prologue-done flag-set

testing an effect of the common bank: a voice, gone a while after
T{ 26 12 0 at-time  6 into  hush  4 the-sound send play-sound  deliver  voices-on -> 1 }T
T{ a-second a-second  voices-on -> 0 }T
testing the area's own: area 5's gunshot as three voices
T{ 5 into  hush  $71 the-sound send play-sound  deliver  voices-on -> 3 }T
testing a held loop (area 5's program 18, its sample looping): still sounding a while on; stopped, it dies away
T{ hush  118 the-sound send loop-sound  deliver  a-second  voices-on 0> -> true }T
T{ 118 the-sound send stop-loop  deliver  a-second a-second  voices-on -> 0 }T
testing a sequence of the common set
T{ 0 false the-sound send play-sequence  deliver  common-set @ 0 seq-playing? -> true }T
T{ 0 the-sound send stop-sequence  deliver  common-set @ 0 seq-playing? -> false }T

testing the area's ambience: its row of the music table, playing
T{ 5 into  2 steps  background-row  music-set @ 0 seq-playing? -> s" area-sounds" 5 2* 1+ exe-s16 true }T
testing the cycle: faded and stopped by its time
T{ stop-at 1- cycle !  4 steps  background-row -> -1 }T
testing a scene's music in its place; none
T{ 10 the-sound send play-music  deliver  background-row  music-set @ 0 seq-playing? -> 10 true }T
T{ -1 the-sound send play-music  deliver  background-row -> -1 }T
testing outdoors at night: silent
T{ 26 23 0 at-time  0 into  10 steps  background-row -> -1 }T
testing an XA stream: playing, the background aside
T{ 26 12 0 at-time  6 into  4 steps  0 the-sound send play-stream  deliver  xa-playing?  background-row  streaming @ 0<> -> true -1 true }T
T{ the-sound send stop-stream  deliver  xa-playing? -> false }T

testing footsteps: Matthew walking in area 5's first scene - its ground's sound on his steps' frames
T{ 5 into  1 s" matthew" actor-named send figure-floor  $1C056 0 $DA77 s" matthew" actor-named send figure-place  deliver
   -1 last-sound !  pad-up true key-hold  40 steps  pad-up false key-hold  last-sound @ -> $65 }T
testing which frames: a walk's 3 and 9, not 4; an unlisted motion none
T{ 1 3 sounds-on?  1 9 sounds-on?  1 4 sounds-on?  0 9 sounds-on? -> true true false false }T
testing the ground: area 26's decks, its cabin, the rest
T{ 26 into  $B4C3 1+ $6571C 1+ ground  0 0 ground -> $65 $64 }T

testing a door's sounds: the third kind, the area's (the church's 107), after its wait; kind 5: 108
T{ 26 14 0 at-time  24 into  -1 last-sound !  2 door-sound  60 steps  last-sound @ -> 107 }T
T{ -1 last-sound !  5 door-sound  60 steps  last-sound @ -> 108 }T

testing the areas' own: the church's bells at noon - tolling (heard), past its third toll in time
: tolled? ( -- flag )  false  300 0 do  -1 last-sound !  1 steps  last-sound @ $73 = or  loop ;
T{ 26 12 0 at-time  24 into  tolled?  toll @ 120 > -> true true }T
testing the forest outdoors: the wind held, the river's voice
T{ 26 14 0 at-time  26 into  4 steps  wind-on @ 0<>  river @ 0< 0= -> true true }T
T{ 26 7 s" areas" actor-named send enter-area  deliver  4 steps  wind-on @  river @ -> 0 -1 }T

test-summary
