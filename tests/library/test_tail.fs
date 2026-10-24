\ tailing the lookout on the game's data (scripts/tail.fs; his way, his sight's zones and the door's code are the
\ town's tables): by his door he paces; his scene, then his way; seen near him from the third point, his scene and
\ his way back; seen in his sight's zone while he looks back; at his door, its scene, then the keypad - a wrong
\ code tried again, the right one through the door
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk tail
    tail.private rules.area027 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: replying? ( -- flag )
    window-up?  window:stage @ window:finished =  window:stage @ window:page-full = or  window:stage @ window:asking = or  and ;
: reply ( -- )  replying? pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  20000 0 do  reply  started? 0= if  leave  then  loop ;
: put-him ( x z -- )  0 swap s" matthew" actor-named send figure-place  deliver ;
: by-the-lookout ( -- )  lookout person-now drop nip 300 - put-him ;
: until-stage ( s -- )  3000 0 do  dup stage @ = if  leave  then  reply  loop  drop ;
: press ( key -- )  dup true key-hold  4 steps  false key-hold  4 steps ;
: in-town ( -- )   \ (far from his door)
    events-stop  27 12 0 date>time s" clock" actor-named send set-time  deliver
    the-town 0 s" areas" actor-named send enter-area  deliver  560000 210000 put-him  2 steps ;
prologue-done flag-set  tail-armed flag-set  hideout-robbers-caught flag-clear

testing by his door: placed at the first point, pacing to the second
: pacing? ( -- flag )  stage @ on-his-way =  stage @ looking-round = or ;   \ (now and then he looks round)
T{ in-town  lookout-mode @ 0<>  lookout here?  pacing?  tail-point @ -> true true true 1 }T
testing talked to: his scene, then his way from the third point
T{ s" tail" actor-named send tail-start  deliver  2 steps  started?  stage @ -> true intro }T
T{ played-out  2 steps  stage @  tail-point @ -> on-his-way 2 }T
testing near him past the third point: seen - his scene, his way back
T{ 3 tail-point !  by-the-lookout  2 steps  stage @  seen @ -> in-a-scene 1 }T
T{ played-out  4 steps  stage @  tail-point @ -> on-his-way 2 }T
testing looking back, Matthew in his sight's zone: seen
T{ 0 seen !  2 tail-point !  looking-back stage !  40 timer !  579000 219000 put-him  2 steps  stage @  seen @ -> in-a-scene 1 }T
T{ events-stop -> }T
testing at his door, Matthew by it out of sight: its scene, then the keypad
T{ 0 seen !  11 tail-point !  door stage !  577700 223000 put-him  2 steps  stage @ -> in-a-scene }T
T{ played-out  4 steps  stage @  started? -> keypad true }T
testing a wrong code: no good, the keypad again
T{ pad-triangle press  pad-triangle press  pad-triangle press  pad-triangle press  #entered @  right @ -> 4 2 }T
T{ in-a-scene until-stage  keypad until-stage  stage @  misses @  #entered @ -> keypad 1 0 }T
testing the right one: through the door, into the hideout
T{ pad-circle press  pad-circle press  pad-triangle press  pad-triangle press  right @ -> 4 }T
T{ 40 steps  played-out  stage @  area @ -> off 28 }T
testing talked to in the town: his (the tail or his warning); someone else's not his
T{ in-town  the-lookout talks-here  deliver  started?  mel talks-here -> true true false }T
T{ events-stop -> }T

test-summary
