\ area 12's rules on the game's data (scripts/rules/area012.fs): Club Wolves - who's there by the hour, the night
\ of Isabella's first show, Wolf's talks, James's first meeting, sitting at the bar, closing time
USING: tester engine actors messages clock areas keys matthew townsfolk events window story-flags talk counter
    rules.area012 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
466718 constant inside-x   289033 constant inside-z   \ (by the backstage door)
: in-club ( -- )
    events-stop  this-area 0 s" areas" actor-named send enter-area  deliver
    inside-x 0 inside-z s" matthew" actor-named send figure-place  deliver  2 steps ;
prologue-done flag-set   club-james-meeting-seen flag-set   \ (James's meeting aside, until its own test)

testing at 20:00 neither the bouncer nor the fan; at 22:00 both (21:58 - 23:25, 21:28 - 23:55)
T{ 26 20 0 at-time  in-club  events-stop  bouncer here?  fan here? -> false false }T
T{ 26 22 0 at-time  in-club  events-stop  bouncer here?  fan here? -> true true }T

testing the night of her first show: Isabella on stage as he comes in
T{ events-stop  26 first-show-day story-c!  26 22 40 at-time  in-club  events-stop
   on-stage @  isabella person-now drop 2drop -> 1 $71932 }T
T{ 0 first-show-day story-c! -> }T

testing Wolf's talks, each once: no alcohol for minors first, then the customers
T{ events-stop  0 wolf-no-minors story!  0 wolf-many-customers story!  26 20 0 at-time  in-club  events-stop
   wolf talk-with  started?  wolf-no-minors story@ -> true 1 }T
T{ events-stop  wolf talk-with  started?  wolf-many-customers story@ -> true 1 }T

testing James's first meeting: in the bar before the 28th, James and Wolf at their places
T{ events-stop  25 20 30 at-time  in-club  events-stop  james 4 at-record?  wolf 0 at-record? -> true true }T
T{ club-james-meeting-seen flag-clear  0 scene-hook  started?  club-james-meeting-seen flag?  met-james @
   -> true true 1 }T

testing the bar's stools: □ before a free one, facing the bar, sits him down
: before-stool ( seat -- )   \ across the counter from it, facing it
    at-club swap seat-spot drop 2drop {: x :}  x 0 $46CA0 s" matthew" actor-named send figure-place
    0 2048 0 s" matthew" actor-named send figure-turn  deliver  2 steps ;
: press ( key -- )  dup true key-hold  2 steps  false key-hold  2 steps ;
T{ events-stop  club-james-meeting-seen flag-set  26 20 0 at-time  in-club  events-stop  at-club 1 seat-taken? -> false }T
T{ 1 before-stool  pad-square press  60 steps  at-the-counter?  matthew seat-of -> true at-club 1 }T

testing ordering from Wolf: □ waves him over; beer is refused, milk served and drunk
USING: list-menu ;
: answer ( -- )
    window-up? stage @ finished = stage @ page-full = or and pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: answering ( n -- )  0 ?do  answer  loop ;
: held ( key n -- )  over true key-hold  steps  false key-hold ;
: until-the-menu ( -- )  200 0 do  answer  menu-up? if  leave  then  loop ;
: row ( n -- )  0 ?do  pad-down press  loop  pad-circle press ;
T{ events-stop  25 21 0 at-time  in-club  events-stop  1 before-stool  pad-square press  60 steps
   pad-square 10 held  300 steps  barman-here -> 1 }T
T{ pad-square 3 held  until-the-menu  menu-up?  3 row  300 answering  menu-up? -> true false }T
T{ 3 cells orders-of + @ -> 0 }T
T{ pad-square 3 held  until-the-menu  2 row  400 answering  pad-square 3 held  600 answering
   2 cells orders-of + @  events-running @ -> 1 0 }T

testing closing time, 01:58 - 02:59: once a visit
T{ 27 2 10 at-time  in-club  started?  closing-done @ -> true 1 }T

test-summary
