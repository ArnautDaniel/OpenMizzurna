\ area 19's rules on the game's data (scripts/rules/area019.fs; events.fs scene-reset, wait-near, wait-minigame;
\ matthew.fs crawling; camera.fs low behind): the photo placed, the mailboxes, her door open, the photo's two
\ scenes and the scene after, knocking, the duct cover's lines, the duct refused, the crawl (the pad drives him
\ through area 32; near the end it stops), the chase out to the town
USING: tester engine actors messages clock areas floors keys matthew townsfolk events window story-flags items talk crate
    knock rules.area019 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: reply ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  10000 0 do  reply  started? 0= if  leave  then  loop ;
: in-the-building ( -- )   \ (on the hallway floor)
    events-stop  this-area 1 s" areas" actor-named send enter-area  deliver
    311000 0 304800 s" matthew" actor-named send figure-place  2 s" matthew" actor-named send figure-floor  deliver  2 steps ;
prologue-done flag-set
27 12 0 at-time

testing on arriving: the photo on the shelf, the crate in its place
T{ in-the-building  4 steps  the-photo person-now drop nip  the-crate-slot here? -> photo-x photo-z true }T
testing the mailboxes: their first line, then the other
T{ 0 the-mailboxes look-check dup read-the-mailboxes =  swap look-do  started?  mailbox-read @ 0<> -> true true true }T
T{ played-out  0 the-mailboxes look-check look-do  started?  played-out -> true }T
testing her door: nothing (nothing known); open, in
T{ her-room-open flag-clear  0 her-door look-check -> nothing-here }T
T{ her-room-open flag-set  0 her-door look-check -> into-her-room }T
T{ into-her-room look-do  started?  played-out  her-room-open flag-clear -> true }T

testing the photo: not before her room's seen; then its first scene, its second (the talk after it, once)
T{ $01C story@ $1E0 invert and $01C story!  the-photo take-it -> false }T
T{ isabellas-room-seen flag-set  the-photo take-it  started?  photo-first-seen flag? -> true true true }T
T{ events-stop  0 after-the-talk look-check -> nothing-here }T
T{ the-photo take-it  photo-second-seen flag?  photo-talk-done @ 0<> -> true true true }T
T{ events-stop  0 after-the-talk look-check -> the-mistake }T
T{ the-mistake look-do  started?  isabella-mistake-scene-seen flag? -> true true }T
T{ events-stop -> }T

testing □ at his door and hers: a knock each
T{ 0 his-door act-check  0 her-door act-check -> knock-his knock-hers }T
testing the duct cover: three lines, the last again
T{ 0 cover-lines !  the-cover events-stop  the-cover events-stop  cover-lines @  the-cover events-stop  cover-lines @ -> 2 2 }T
testing the duct: without the hint, not now
T{ nozz-air-duct-hint-heard flag-clear  the-duct  started?  crawled-air-duct flag? -> true false }T
T{ played-out -> }T

testing the crawl (minigame 41): into the duct, the pad drives him; near the end it stops; out to the far room
: him-z ( -- z )  matthew-now drop nip nip ;
T{ 41 minigame  60 steps  area @  matthew-crawling? -> 32 true }T
: him-x ( -- x )  matthew-now drop 2drop ;
: press-for ( key n -- )  over true key-hold  steps  false key-hold ;
T{ him-x  pad-up 30 press-for  him-x = -> true }T   \ (a wall ahead of where he starts: no way on)
T{ pad-right 16 press-for  matthew-now nip nip nip -> 3072 }T
T{ him-x  pad-up 30 press-for  him-x > -> true }T
T{ 308726 0 305651 s" matthew" actor-named send figure-place  deliver  10 steps  matthew-crawling? -> false }T
T{ played-out  minigame-running? -> false }T

testing the chase (minigame 22): its scene, then out to the town
T{ in-the-building  22 minigame  played-out  area @ -> the-town }T

testing a scene's set-floor on Matthew: his floor, its height under him
T{ in-the-building  matthew this-area 1 set-floor  deliver  floor @  matthew-now 2drop nip  this-area 1 floor-height = -> 1 true }T
T{ matthew this-area 2 set-floor  deliver  floor @  matthew-now 2drop nip  this-area 2 floor-height = -> 2 true }T

testing Dennis's line at a stop of his; Isabella's silence
T{ in-the-building  dennis talks-here  isabella talks-here -> true true }T

test-summary
