\ the events' actor commands on the game's data (scripts/events.fs): a figure's place piecemeal, its heading toward a
\ point or another, kept and given back; running; a person reset; their car turning onto a route; the map told
\ of a place; an ending's route
USING: tester engine actors messages clock areas keys matthew townsfolk traffic events story-flags walkers ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: in-area ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
prologue-done flag-set
157 constant someone   \ (an extra: the junkyard's dog)

testing piecemeal: x, height, z, and moved by
T{ 8 in-area  someone 1000 0 2000 set-pos  deliver  someone 1500 set-x  someone -40 set-y  someone 2500 set-z  deliver
   someone person-now drop -> 1500 -40 2500 }T
T{ someone 10 0 -10 move-by  deliver  someone person-now drop -> 1510 -40 2490 }T
testing toward a point: east (a quarter turn); toward another: back toward the first
T{ someone 9000 2490 0 face-point  deliver  someone person-now nip nip nip -> 1024 }T
T{ matthew 1510 0 5000 set-pos  deliver  someone matthew face-each-other  deliver  someone person-now nip nip nip -> 0 }T
testing kept and given back: the heading as it was
T{ someone save-state  deliver  someone 2048 0 turn-to  deliver  someone restore-state  deliver
   someone person-now nip nip nip -> 0 }T
testing running: straight on
T{ someone person-now drop nip nip  someone 80 run  deliver  10 steps  someone person-now drop nip nip = -> false }T
testing reset: standing, off the footpaths
T{ someone 0 3 100 5 s" walkers" actor-named send walker-start  deliver  someone reset  deliver
   someone walker-active?  someone walking? -> false false }T
testing the map told of a place
T{ 0 map-known !  3 map-reveal  7 map-reveal  map-known @ -> $88 }T
testing a car turning onto a route at its first node (Hudson's: 55)
T{ hudson 55 partner-command  deliver -> }T
testing an ending's route: they leave on it
T{ 31 20 0 at-time  mel -2146928900 ending-schedule  deliver  mel route-thread 0< 0= -> true }T
T{ mel end-route -> }T

test-summary
