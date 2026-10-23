\ area 3's rules on the game's data (scripts/rules/area003.fs): the Roland residence - Mrs. Roland goes along with
\ him and follows; the stairs only with her; the vase and Emma's key; the desk after the break-in; heading home
USING: tester engine actors messages clock areas floors keys matthew townsfolk items events window story-flags talk
    rules.area003 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: started? ( -- flag )  events-running @ 0<> ;
: answer ( -- )
    window-up?  stage @ finished =  stage @ page-full = or  stage @ asking = or  and   \ (a question: its first answer)
    pad-circle swap key-hold  1 steps  pad-circle false key-hold ;
: played-out ( -- )  2000 0 do  answer  started? 0= if  leave  then  loop ;
: at-home ( -- )  events-stop  this-area 0 s" areas" actor-named send enter-area  deliver
    178000 0 423400 s" matthew" actor-named send figure-place  deliver  2 steps ;
prologue-done flag-set   roland-funeral-talk-seen flag-set   roland-break-in-scene-seen flag-set

testing on the 26th at noon Mrs. Roland is home, at her stop 2
T{ 26 12 0 at-time  at-home  events-stop  sarah stop-of  sarah here? -> 2 true }T
testing alone, the stairs: "I shouldn't just walk around the house."
T{ the-stairs look-do  started?  played-out -> true }T
testing talking to her downstairs: "Let's go to Emma's room" - she goes along with him, following
T{ sarah talk-with  played-out  sarah going-along?  following? -> true true }T
: how-far ( -- d )   \ how far she is from him
    matthew-now drop {: mx my mz :}  sarah person-now drop {: sx sy sz :}
    mx sx - dup *  mz sz - dup * + s>f fsqrt f>s ;
T{ 178600 0 424200 s" matthew" actor-named send figure-place  deliver  2 steps  how-far $1FF - abs 2 < -> true }T
testing with her along, the stairs take them up
T{ the-stairs look-do  started?  played-out -> true }T

testing the vase: a look, a second look, then □ takes Emma's key
T{ events-stop  got-emmas-key flag-clear  0 $85 look-check -> the-vase }T
T{ the-vase look-do  played-out  0 $85 act-check -> take-the-key }T
T{ take-the-key act-do  played-out  got-emmas-key flag?  emmas-key has-item? -> true true }T

testing Emma's desk after the break-in: ○ then □ - the diary's gone
T{ emma-diary-found-missing flag-clear  0 $87 look-check -> emmas-desk }T
T{ emmas-desk look-do  played-out  0 $87 act-check  open-the-desk act-do  played-out  emma-diary-found-missing flag?
   -> open-the-desk true }T

testing 20:55 - 23:59: "I should head home." - once a visit
T{ 26 21 0 at-time  at-home  started?  heading-home-said @ -> true 1 }T

test-summary
