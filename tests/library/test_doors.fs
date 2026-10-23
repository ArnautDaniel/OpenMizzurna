\ the town's doors on the game's data (docs/subsystems/doors.md): ○ at a building's door takes Matthew in - when
\ it's open - by the building's script and a warp
USING: tester engine actors messages clock keys areas floors matthew window events story-flags talk ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: matthew-at ( x z heading -- )
    0 swap 0 s" matthew" actor-named send figure-turn  0 swap s" matthew" actor-named send figure-place  deliver ;
: press-circle ( -- )  pad-circle true key-hold  2 steps  pad-circle false key-hold ;
: in-town ( -- )  events-stop  the-town 0 s" areas" actor-named send enter-area  deliver  2 steps ;
prologue-done flag-set   \ (past the prologue: no morning visit when he comes in)

testing Nozz's store (tile 17,13; its door the tile's zone 0, faced at 512): open at noon - in, to its scene 3
T{ in-town  26 12 0 at-time  344846 284920 512 matthew-at  1 steps  344846 284920 1 zone-hit -> 0 0 }T
T{ press-circle  400 steps  area @  scene @ -> 1 3 }T
T{ matthew-now drop nip -> 345196 285243 }T

testing out again: the store's door (area 1's zone 15, faced at 2560) - back to the town, outside it
15 constant way-out   \ (the zone's id: its look check runs the store's way out)
: door-zone ( -- x z )   \ the centre of the zone with that id
    {: | x z :}  40 0 do  i 345196 285243 1 zone-centre to z to x  x z 1 zone-hit nip way-out = if  x z unloop exit  then
    loop  0 0 ;
: events-over ( -- )  50 0 do  events-running @ 0= if  unloop exit  then  20 steps  loop ;   \ (at most 1000 steps)
T{ events-over  door-zone 2560 matthew-at  1 steps  press-circle  400 steps  events-over
   area @  matthew-now drop nip -> the-town 344881 284957 }T

testing shut in the evening (12/26 20:00: 17:25 - 22:59 before the 29th): he stays out, and the window says so
T{ in-town  26 20 0 at-time  344846 284920 512 matthew-at  1 steps  press-circle  200 steps  area @  window-up?
   -> the-town true }T

test-summary
