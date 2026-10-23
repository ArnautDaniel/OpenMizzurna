\ area 26's rules on the game's data (scripts/rules/area026.fs): the deputy at his post or aside, his lines (and
\ his refusal remembered next visit); the morning hunt - slipping past while he's away, through to the hunters,
\ following Cohen (he keeps on while Matthew keeps up), a leg's deadline; the cabin door; the shell box; the
\ waterfall meeting at its time; the last evening's way chosen on arriving
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags items talk
    rules.area026 ;

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
: put-him ( x z -- )  0 swap s" matthew" actor-named send figure-place  deliver ;
: in-the-forest ( -- )   \ (on the town side of the gate)
    events-stop  this-area 0 s" areas" actor-named send enter-area  deliver
    1 s" matthew" actor-named send figure-floor  $DA00 $65A00 put-him  2 steps ;
: said ( -- msg )  line @ ;
prologue-done flag-set  joined-the-hunt flag-clear  cohen-found-scene-seen flag-clear  deputy-refused flag-clear

testing the deputy: at his post before the hunt's day; aside after it (not yet after Cohen's found)
T{ 25 12 0 at-time  in-the-forest  at-his-post?  hunt @ -> true gathering }T
T{ 27 12 0 at-time  in-the-forest  at-his-post?  hunt @ -> false no-hunt }T
testing at his post: his first line, then the other; his refusal remembered the next visit
T{ 25 12 0 at-time  in-the-forest  the-deputy talks-here  said -> true $1E }T
T{ events-stop  the-deputy talks-here  said -> true $1F }T
T{ events-stop  in-the-forest  deputy-refused flag?  the-deputy talks-here  said -> true true $20 }T
T{ events-stop  deputy-refused flag-clear -> }T

testing the hunt's morning: his line; at the gate's side on an even minute he walks away
T{ 26 7 10 at-time  in-the-forest  the-deputy talks-here  said  talked-this-morning @ -> true $21 1 }T
T{ events-stop  $E100 $65000 put-him  2 steps  hunt @  started? -> deputy-away true }T
testing through to the hunters: they set off, he's joined them
T{ events-stop  chased hunt !  hunters-x hunters-z put-him  2 steps  hunt @  joined-the-hunt flag? -> joined true }T
T{ played-out  2 steps  hunt @  route-point @ -> following 0 }T
testing following: near him, Cohen walks to his first point
: cohen-x ( -- x )  cohen person-now 2drop drop ;
: by-cohen ( -- )  cohen person-now drop nip  300 - put-him ;
T{ cohen 33000 0 409000 set-pos  deliver  by-cohen  cohen-x  20 steps  cohen-x = -> false }T
testing too far behind: he stops ("don't go too far"), and waits
T{ 20000 400000 put-him  2 steps  hunt @ -> waiting-scene }T
T{ played-out  by-cohen  2 steps  hunt @ -> following }T
testing a leg's deadline past: the gunshot, the hunt over
T{ 26 8 31 at-time  by-cohen  2 steps  started?  hunt @ -> true no-hunt }T
T{ played-out -> }T

testing the cabin door: Cohen gone from the forest (his stop 10), nothing; else with no hunt, in
T{ 0 cabin-door-outside look-check -> nothing-here }T
T{ cohen 0 goto-stop  deliver  0 cabin-door-outside look-check -> into-the-cabin }T
testing the shell box in the cabin, the bear outside: its shells seen with the gun; pried, then open
T{ in-the-cabin hunt !  0 box-left look-check -> the-shells }T
T{ the-gun-item s" items" actor-named send give-item  deliver  the-shells look-do  started-then-over  shells-seen @ -> true 1 }T
T{ box-pried flag-clear  0 box-left act-check  0 box-right act-check -> pry-left pry-right }T
T{ pry-left act-do  played-out  box-pried flag?  0 box-left act-check -> true open-left }T
T{ events-stop  box-pried flag-clear  no-hunt hunt ! -> }T

testing the waterfall meeting: at its time, near it, once
T{ forest-meeting-seen flag-clear  27 14 0 at-time  in-the-forest
   27 meeting-day story!  14 meeting-hour story!  0 meeting-minute story-c!
   waterfall-x waterfall-z put-him  2 steps  started?  forest-meeting-seen flag?  meeting-day story@ -> true true 0 }T
T{ events-stop -> }T

testing the last evening, the hideout found, a requirement missing: that way chosen on arriving; the exits locked
T{ entered-wolf-hideout flag-set  bad-ending-route flag-clear  0 visited-james-house story!  31 20 30 at-time
   in-the-forest  bad-ending-route flag?  exits-locked @ 0<> -> true true }T
T{ events-stop  entered-wolf-hideout flag-clear  bad-ending-route flag-clear -> }T

test-summary
