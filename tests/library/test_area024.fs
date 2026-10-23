\ area 24's rules on the game's data (scripts/rules/area024.fs): the caretaker and the crate placed (its place by
\ his floor), closing time once, shown out before the meeting, the meeting's look by its time, the doors (shut
\ while locked in), the letter (○ then □, once its time's come), the stairs up from the basement, the rope
\ (minigame 17: left and right), who says what
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags items talk crate
    rules.area024 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  10000 0 do  reply  started? 0= if  leave  then  loop ;
: started-then-over ( -- flag )  started?  played-out ;
: put-him ( x z floor -- )
    s" matthew" actor-named send figure-floor  0 swap s" matthew" actor-named send figure-place  deliver ;
: in-the-church ( scene -- )   \ (on the ground floor, by the front doors)
    events-stop  this-area swap s" areas" actor-named send enter-area  deliver
    $90624 $20000 1 put-him  4 steps ;
prologue-done flag-set
kathy-funeral-seen flag-clear  church-repentant-thugs-seen flag-set  club-come-here-often-seen flag-clear

testing on arriving by day: the caretaker; the crate in its ground-floor place
T{ 26 12 0 at-time  0 in-the-church  started?  the-caretaker person-now drop nip -> false caretaker-x caretaker-z }T
T{ the-crate-slot person-now drop nip -> $90DD5 $1FE3B }T
testing in the basement it's placed there
T{ $91126 $1FE81 0 put-him  the-crate forget-crate  4 steps  the-crate-slot person-now drop nip -> $9142D $20205 }T
testing the stairs up: at their foot facing them; not facing away
T{ 0 $400 0 s" matthew" actor-named send figure-turn  deliver  0 -1 look-check -> up-the-stairs }T
T{ 0 0 0 s" matthew" actor-named send figure-turn  deliver  0 -1 look-check -> nothing-here }T

testing closing time in the nave: shown out, to the town
T{ 26 20 0 at-time  0 in-the-church  started-then-over  area @ -> true the-town }T
testing on the 25th before the meeting: shown out of the nave, once
T{ 25 12 57 at-time  0 in-the-church  started?  shown-out @ 0<> -> true true }T
T{ played-out  scene @ -> 1 }T
testing the meeting from up there: before its time a look; at it, the meeting (once); then the short look
T{ 1 in-the-church  town-meeting-seen flag-clear  25 12 30 at-time  the-meeting  started-then-over  town-meeting-seen flag? -> true false }T
T{ 25 13 10 at-time  1 in-the-church  the-meeting  started?  town-meeting-seen flag?  meeting-on @ 0<> -> true true true }T

testing the doors: out to the town; while locked in, shut
T{ events-stop  26 12 0 at-time  0 in-the-church  1 locked-in !  front-door  started-then-over  area @ -> true this-area }T
T{ 0 locked-in !  front-door  started-then-over  area @ -> true the-town }T

testing the letter: ○ looks; □ not before its time; then □ reads it
T{ 26 12 0 at-time  3 in-the-church  church-winona-missing-seen flag-clear  read-kathys-letter flag-clear
   the-letter look-at-it  started?  letter-looked-at @ 0<> -> true true true }T
T{ events-stop  the-letter take-it -> false }T
T{ church-winona-missing-seen flag-set  the-letter take-it  started?  read-kathys-letter flag? -> true true true }T
T{ events-stop -> }T

testing the rope: 80 pulls, left and right in turn, and he's free
: pull ( key -- )  dup true key-hold  1 steps  false key-hold  1 steps ;
: pull-free ( -- )  40 0 do  pad-left pull  pad-right pull  loop ;
T{ 5 in-the-church  17 minigame  4 steps  minigame-running? -> true }T
T{ pad-left pull  pulls @ -> 1 }T
T{ pull-free  300 steps  minigame-running? -> false }T

testing who says what: the caretaker his line; Dennis always his own
T{ events-stop  0 in-the-church  the-caretaker talks-here  dennis talks-here -> true true }T

test-summary
