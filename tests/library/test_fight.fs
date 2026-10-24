\ the fight on the game's data (scripts/fight.fs; the moves, attacks and the AI's ways are the executable's
\ tables): begun, they face each other at full health; ○ near enough lands a jab (5); the AI fights back; a
\ knock-out wins it for Matthew (the events' fight op answers the winner); a loss to the AI is game over, and ○
\ fights again from the start; won, the two put 0x100 apart facing each other; style 7's own motions; the names
\ over the bars (none when two play)
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk fight
    fight.private sound screen ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: press ( key -- )  dup true key-hold  2 steps  false key-hold  2 steps ;
106 constant the-robber
: in-the-hideout ( -- )   \ (placed as a scene places them: he isn't the player's meanwhile)
    events-stop  28 0 s" areas" actor-named send enter-area  deliver  false broadcast player-control  deliver
    1 s" matthew" actor-named send figure-floor  575529 0 221563 s" matthew" actor-named send figure-place
    the-robber 575529 250 - 0 221563 set-pos  deliver  2 steps ;
: still-fight ( -- )  matthew 0 the-robber 1 fight ;   \ (the other the second pad's: it stands)
: ai-fight ( -- )  matthew 0 the-robber 3 fight ;
: him ( -- f )  0 fighter# ;   : the-other ( -- f )  1 fighter# ;
: punch-until-won ( -- )  300 0 do  pad-circle press  fighting? 0= if  leave  then  loop ;
: until-again ( -- )  600 0 do  1 steps  lost @ 0= if  leave  then  loop ;
: until-lost ( -- )  3000 0 do  1 steps  lost @ if  leave  then  loop ;
: punch-until-over ( -- )  300 0 do  pad-circle press  events-running @ 0= if  leave  then  loop ;
prologue-done flag-set

testing begun: on, full health, ready (the other the second pad's, untouched: it stands)
T{ in-the-hideout  ' still-fight start-event  2 steps
   fighting?  him health @  the-other health @ -> true 120 120 }T
testing ○ near enough: a jab lands (5)
T{ pad-circle press  20 steps  the-other health @ -> 115 }T
testing its sounds: the jab's voice last (the area's first fight sound on); the music the fight's
T{ last-sound @  jab voice-sound  =  background-row -> true fight-music }T
testing a knock-out: Matthew wins, the fight's over
T{ 1 the-other health !  1 the-other shown !  ready-move the-other move !  matthew-now drop nip  the-other fz ! the-other fx !
   punch-until-won  fighting?  fight-winner @ -> false 0 }T
testing won: the other put 0x100 from him, the two facing each other
T{ apart 255 257 within  him heading @ the-other heading @ - 4095 and -> true 2048 }T
testing the names: none when two play; against the AI each its style's
T{ him name-of  the-other name-of -> no-name no-name }T

testing the AI fights: in time Matthew's hurt
T{ in-the-hideout  ' ai-fight start-event  600 steps  him health @ 120 < -> true }T
T{ him name-of  the-other name-of -> matthew-name 6 }T
testing style 7's own motions: five of the moves', the rest as they are
T{ $74 own-motion-of  $70 own-motion-of  $12 own-motion-of -> $536 $539 $12 }T
testing lost to the AI: game over; ○ fights again from the start at full health
T{ 1 him health !  until-lost  lost @ -> 1 }T
T{ pad-circle press  until-again  lost @  fighting?  him health @ -> 0 true 120 }T

testing the events' op: it waits for the winner and answers it
variable winner-was
: a-fight ( -- )  matthew 0 the-robber 1 fight  0 result= winner-was ! ;
T{ events-stop  in-the-hideout  ' a-fight start-event  10 steps  fighting? -> true }T
T{ 1 the-other health !  punch-until-over  events-running @  winner-was @ -> 0 true }T

test-summary
