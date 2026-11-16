\ the last of the events' ops on the game's data (scripts/events.fs): the prompt (□'s and ○'s icons beside the
\ box until one is pressed: ○ 1, □ 0); the events' lock (the clock held); the hospital's elevator doors (four of its objects' motions, in its
\ content areas only); a talk's gestures (a motion and what follows; none sitting but the seated ones; Barbara
\ her own); a labourer off along their stop's route; Matthew through a door
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags camera ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
: tap ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
prologue-done flag-set
25 12 0 date>time s" clock" actor-named send set-time  deliver

testing the prompt: shown, the event waiting; ○ answers 1 and it's gone
variable answer
: asks ( -- )  13 8 prompt  thread thread-result slot @ answer ! ;
T{ -1 answer !  ' asks start-event  2 steps  prompt-left @  prompt-right @  prompt-shown? -> 13 8 true }T
T{ pad-circle tap  2 steps  answer @  prompt-shown? -> 1 false }T
testing □ answers 0
T{ -1 answer !  ' asks start-event  2 steps  pad-square tap  2 steps  answer @ -> 0 }T

testing a gesture: Matthew's raised hand - its motion, then the next looping
T{ 1 into  events-stop  deliver  matthew raised-hand gesture  deliver  matthew-anim-now -> 83 84 }T
testing a plain one: once, its last frame held; none at all sitting (but the seated ones)
T{ matthew 3 gesture  deliver  matthew-anim-now -> 86 -1 }T
T{ 2 s" matthew" actor-named send figure-perch  deliver  matthew 3 gesture  deliver  matthew-anim-now
   0 s" matthew" actor-named send figure-perch  deliver -> 86 -1 }T

testing Matthew through a door (area 1's 14 into its room 3): out at its far side, stepping, the scene waiting
variable walked
: through ( -- )  matthew 14 3 through-door  true walked ! ;
T{ 1 into  events-stop  false walked !  ' through start-event  2 steps  walked @  matthew-anim-now drop dup $3F = swap $40 = or
   -> false true }T
T{ 300 steps  walked @ -> true }T
T{ events-stop  deliver -> }T

testing the events' lock: the clock stands while it's on, counted; reset with the events
: lock-held? ( -- flag )  1 events-locked lshift clock-holds @ and 0<> ;   \ (its own reason, among the others)
T{ events-stop  deliver  event-lock  deliver  event-locked?  lock-held? -> true true }T
T{ event-lock  event-unlock  deliver  event-locked?  event-unlock  deliver  event-locked?  lock-held? -> true false false }T
T{ event-lock  deliver  events-stop  deliver  event-locked?  lock-held? -> false false }T
testing a prop in his hand (the events' ignore-a: the right): held, where he stands; -1 lets go
24 constant the-gun
: hold-it ( -- )  matthew the-gun ignore-a ;   : drop-it ( -- )  matthew -1 ignore-a ;
T{ 1 into  events-stop  ' hold-it start-event  4 steps  the-gun townsfolk.private:held?
   the-gun person-now drop nip  matthew-now drop nip  rot = -rot = and -> true true }T
T{ ' drop-it start-event  4 steps  the-gun townsfolk.private:held? -> false }T
testing the elevator doors: nothing outside the hospital
T{ 1 1 elevator-doors  unbuilt-ops @ -> unbuilt-ops @ }T

testing a walk that gets him nowhere (at no speed: as if blocked) is given up on after 75 frames all told (src:
\ the VM's wait_move)
$55014 constant by-his-bed-x   $4638C constant by-his-bed-z   2000 constant a-way-on   0 constant no-speed
: to-his-room ( -- )  1 0 by-his-bed-x by-his-bed-z 0 0 warp ;
variable gave-up
: nowhere ( -- )  matthew by-his-bed-x a-way-on + by-his-bed-z walk-motion no-speed walk-to-ex  matthew wait-stop
    true gave-up ! ;
T{ ' to-his-room start-event  60 steps  events-stop  false gave-up !  ' nowhere start-event  100 steps
   gave-up @  matthew moving? -> false true }T
T{ 60 steps  gave-up @  matthew-now drop nip drop -> true by-his-bed-x }T
T{ events-stop -> }T
testing waiting on his motion while he walks to a point doesn't wait (src: wait_anim: ACTOR_F_NAV)
variable anim-waited
: walking-wait ( -- )  matthew by-his-bed-x a-way-on + by-his-bed-z walk-motion no-speed walk-to-ex  matthew wait-anim
    true anim-waited ! ;
T{ false anim-waited !  ' walking-wait start-event  4 steps  anim-waited @ -> true }T
T{ events-stop  s" matthew" actor-named send figure-idle  deliver -> }T

testing camera-set's heights, distance and heading as the VM's 16 bits: 64382 is -1154 (ovl001's Morgan scene)
: placed-camera ( -- )  camera-fixed  -730 63118 -2306 345609 -4080 285716 63084 camera-set  100 frames wait ;
T{ ' placed-camera start-event  2 steps  camera:dist @  camera:eye-y @  camera-yaw 4095 and -> -2418 -2306 -2452 4095 and }T
T{ events-stop  deliver -> }T

testing a scene's set-pos then a turn toward a point reads the new place (the VM did each op at once)
variable faced
: place-and-face ( -- )  matthew 346901 0 287566 set-pos  matthew 345848 287272 0 face-point  matthew where-is drop nip nip faced ! ;
T{ 1 into  ' place-and-face start-event  2 steps  faced @ -> 2895 }T
testing a scene placing someone their schedule has elsewhere: brought into the area played (Morgan in the shop)
: morgan-in ( -- )  morgan 345547 0 285294 set-pos ;
T{ morgan here?  ' morgan-in start-event  2 steps  morgan here? -> false true }T
T{ events-stop  deliver -> }T

test-summary
