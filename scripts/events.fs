\ events.fs - the event scripts' language (docs/subsystems/story.md). The game's events were bytecode for
\ CEVENT.C's VM; tools/ev2forth.py turned each into Forth (scripts/events/), and this is the vocabulary they
\ are written in. An event runs as a task: its waits are real time (`n frames wait`: the events ran at 15
\ frames a second), a `goto` is a tail jump, and its own actor (`this-one`) and last result are its own.
\ Each op does its work through its subsystem's messages; an op whose subsystem isn't built yet is a stub
\ that takes its arguments and notes it (`unbuilt`).
IN: events
USING: engine actors messages clock window matthew effects camera ;

\ ---- the cast: actor slots (decomp/include/actor.h); the others by number (`160 actor`) ----
0 constant matthew    1 constant morgan    2 constant nozz       3 constant cohen     5 constant samuel
6 constant james      7 constant hudson    12 constant steve     13 constant mel      14 constant father-barton
15 constant winona    20 constant lidds    21 constant wolf      22 constant rolein   89 constant isabella
90 constant barbara   92 constant dennis   93 constant robert
182 constant #cast
: actor ( slot -- actor ) ;   \ (a slot is the actor)

\ ---- time: an event's frame ----
15e fconstant event-rate   \ src: CEvCon_Start sets VSync(4) while an event runs: 15 frames a second
: frames ( n -- steps )  s>f event-rate f/ seconds ;

\ ---- the events running: a thread each (src: 5 threads and the main one) ----
8 constant #threads
create thread-task    #threads cells allot   \ its task (0: free)
create thread-xt      #threads cells allot   \ the word it starts in
create thread-own     #threads cells allot   \ its own actor (this-one)
create thread-result  #threads cells allot   \ the last result an op gave (the VM's result register)
create thread-text    #threads cells allot   \ it waits for its message to close (src: the thread's wait_msg)
thread-task #threads cells erase

variable events-running   \ how many
variable control-given    \ an event gave Matthew back to the player (the rest no longer hold him)
: slot ( i addr -- addr' )  swap cells + ;
: thread ( -- i )   \ the running event's (-1: not an event)
    -1  #threads 0 do  i thread-task slot @ me = if  drop i  then  loop ;
: this-one ( -- actor )  thread thread-own slot @ ;
: as-this-one ( actor -- )  thread thread-own slot ! ;
: result! ( n -- )  thread dup 0< if  2drop  else  thread-result slot !  then ;
: result= ( n -- flag )  thread thread-result slot @ = ;

: the-clock ( -- id )  s" clock" actor-named ;
: hold ( reason -- )  the-clock send hold-clock ;
: release ( reason -- )  the-clock send release-clock ;

: free-thread ( -- i )  -1  #threads 0 do  i thread-task slot @ 0= if  drop i leave  then  loop ;
: event-ended ( -- )
    0 thread thread-task slot !  -1 events-running +!
    events-running @ 0= if  no-control release  false control-given !  true broadcast player-control  then ;
: run-event ( -- )   \ a task's body: its word, then it's over
    thread thread-xt slot @ execute  event-ended ;
: start-event ( xt -- )   \ src: event_start (CEvCon_Start): a thread, Matthew no longer the player's
    free-thread {: xt i :}
    i 0< abort" no free event thread"
    xt i thread-xt slot !  matthew i thread-own slot !  0 i thread-result slot !
    ['] run-event forth:spawn  i thread-task slot !   \ (a task: the actors' spawn is another)
    1 events-running +!  no-control hold  false broadcast player-control ;
: events-reset ( -- )  thread-task #threads cells erase  0 events-running !  false control-given ! ;
: events-stop ( -- )   \ every running event ended now (a tool: the console, tests)
    #threads 0 do  i thread-task slot @ ?dup if  forth:kill  then  loop
    events-reset  no-control release  true broadcast player-control ;

\ control flow: `goto word` leaves this word for that one; `end-script` ends this word (the event, at the
\ outermost)
: goto ( "name" -- )  ' postpone literal postpone jump ; immediate
: end-script ( -- )  postpone exit ; immediate

\ ---- the C an event called (call_c): written in Forth one by one, found by where it was ----
0 constant main-exe
: ovl ( area -- scope )  1+ ;
0 ovl constant ovl000   1 ovl constant ovl001   2 ovl constant ovl002   3 ovl constant ovl003
5 ovl constant ovl005   6 ovl constant ovl006   7 ovl constant ovl007   9 ovl constant ovl009
12 ovl constant ovl012  13 ovl constant ovl013  16 ovl constant ovl016  19 ovl constant ovl019
21 ovl constant ovl021  24 ovl constant ovl024  26 ovl constant ovl026  27 ovl constant ovl027
28 ovl constant ovl028
begin-structure native-entry   \ a list of the natives written so far
  field: next-native
  field: native-scope
  field: native-address
  field: native-xt
end-structure
variable natives
: native: ( scope address xt -- )
    here native-entry allot {: s a xt e :}  natives @ e next-native !  s e native-scope !  a e native-address !
    xt e native-xt !  e natives ! ;
: find-native ( scope address -- xt | 0 )
    {: s a :}  natives @  begin  dup while
        dup native-scope @ s =  over native-address @ a = and if  native-xt @ exit  then  next-native @
    repeat ;

variable unbuilt-ops   variable event-trace
: note-unbuilt ( addr len -- )  1 unbuilt-ops +!  event-trace @ if  ." event: not yet " type cr  else  2drop  then ;
: unbuilt ( args.. n addr len -- )  note-unbuilt  0 ?do  drop  loop ;
: act-unbuilt ( actor args.. n -- )  s" actor command" unbuilt  0 result! ;   \ (the VM took any command's result)
: act-command ( actor args.. n cmd -- )  drop 1+ act-unbuilt ;

: native ( scope address -- )  2dup find-native ?dup if  nip nip execute  else  2drop s" native" note-unbuilt  then ;
: native-result ( scope address -- )   \ (its answer is the result)
    2dup find-native ?dup if  nip nip execute result!  else  2drop s" native" note-unbuilt  0 result!  then ;

\ ---- the area overlays' words the main executable's scripts reach by address (in the area being played) ----
variable event-area   \ the area whose scripts are in (the areas subsystem sets it)
begin-structure overlay-entry
  field: next-overlay
  field: overlay-area
  field: overlay-address
  field: overlay-xt
end-structure
variable overlay-words
: overlay-word! ( xt address area -- )
    here overlay-entry allot {: xt a ar e :}  overlay-words @ e next-overlay !  ar e overlay-area !
    a e overlay-address !  xt e overlay-xt !  e overlay-words ! ;
: overlay-word ( address -- xt )
    {: a :}  overlay-words @  begin  dup while
        dup overlay-area @ event-area @ =  over overlay-address @ a = and if  overlay-xt @ exit  then  next-overlay @
    repeat  true abort" no such word in this area's scripts" ;
: goto-overlay ( address -- )  overlay-word jump ;
: call-overlay ( address -- )  overlay-word execute ;

\ ---- event packs: CD.BIN entries of scripts, loaded one at a time; their scripts by number ----
create pack-scripts 600 64 * cells allot   pack-scripts 600 64 * cells erase   \ pack x 64 scripts: the words
variable loaded-pack
: pack-script! ( xt k pack -- )  64 * + cells pack-scripts + ! ;
: pack-script ( k -- xt )  loaded-pack @ 64 * + cells pack-scripts + @  dup 0= abort" no such pack script" ;
: load-pack ( pack -- )  loaded-pack ! ;
: goto-pack-script ( k -- )  pack-script jump ;   \ (src: goto_id: the event goes on there)
: start-pack-script ( k -- )  pack-script start-event ;
: call-pack-script ( pack k -- )  swap load-pack  pack-script execute ;

\ ---- control and time ----
: give-control ( -- )  true control-given !  no-control release  true broadcast player-control ;   \ src: player_control_on
: take-control ( -- )  false control-given !  no-control hold  false broadcast player-control ;
: time-skip ( minutes -- )   \ src: time_skip: the clock on a minute at a time (the townsfolk follow: to come)
    0 ?do  the-clock send advance-minute  loop ;

\ ---- text: the message window (docs/subsystems/window.md) ----
\ a text id: < 10000 the area's, < 15000 the common text's, else the loaded pack's (src: msg_bank)
10000 constant common-ids   15000 constant pack-ids
200 constant first-pack
: text-bank ( id -- bank n )
    dup common-ids < if  event-area @ swap exit  then
    dup pack-ids < if  common-text swap common-ids - exit  then
    loaded-pack @ swap pack-ids - ;
: the-window ( -- id )  s" window" actor-named ;
3 constant phone-palette   \ (the other end of a telephone: src phone_say, 3)
: show ( text speaker palette secs -- )
    {: text who pal secs :}  text text-bank who pal secs the-window send window-say ;
: text-pending ( -- )  thread 0< 0= if  true thread thread-text slot !  then ;
: settle ( -- )   \ an event's waits first let its message close (src: the VM's wait_msg, checked first; the
                  \ ops that wait for their line - "...-wait", say-n, phone-say - settle at once: they slept a frame)
    thread 0< if  exit  then
    thread thread-text slot @ if   \ (a step first: the window takes the message as the actors run)
        begin  yield  window-up? 0= until
        false thread thread-text slot !
    then ;
: wait ( steps -- )  settle  forth:wait ;
: secs ( n -- steps )  s>f seconds ;

: say ( text -- )  nobody 0 0 show ;                                  \ src $108
: say-by ( text a -- )  0 0 show ;                                    \ $109
: say-by-wait ( text a -- )  say-by text-pending settle ;                    \ $125
: say-mode ( text palette -- )  nobody swap 0 show ;                  \ $153
: say-mode-wait ( text palette -- )  say-mode text-pending settle ;          \ $137
: say-n ( text palette a -- )  swap 0 show text-pending settle ;             \ $13C
: say-wait-close ( text -- )  say text-pending settle ;                      \ $158
: say-timed-by ( text a secs -- )  0 swap show ;                     \ $13D: it closes itself
: say-timed-by-wait ( text a secs -- )  dup >r say-timed-by r> secs wait ;   \ $13F
: caption-for-nowait ( text secs -- )  nobody 0 rot show ;           \ $177
: caption-for ( text secs -- )  dup >r caption-for-nowait r> secs wait ;     \ $15B
: wait-text ( -- )  text-pending settle ;                                       \ $10A
: text-close ( -- )  the-window send window-shut  1 wait ;                      \ $14C
: text-hold ( -- )  the-window send window-shut  1 wait ;                       \ $13E
\ talk: a line and the speaker's gesture (src: event_talk; gestures: with the figures)
: gesture ( a n -- )  2 s" gesture" unbuilt ;
: talk ( a text gesture -- )  {: a text g :}  text a 0 0 show  text-pending  a g gesture ;   \ $128 ...
: talk-to ( a text gesture who -- )   \ $16F / $164: the portrait `who`'s (0: the speaker's)
    {: a text g who :}  text  who if  who  else  a  then  0 0 show  text-pending  a g gesture ;

\ ---- answers ----
: choice ( text kind -- )   \ src $11E: the question; its answer the result
    >r text-bank r> the-window send window-ask  text-pending settle  window-answer @ result! ;
: prompt ( a b -- )  2 s" prompt" unbuilt  1 result! ;
: fight ( a style b style -- )  4 s" fight" unbuilt  0 result! ;
: memory-card ( mode -- )  1 s" memory-card" unbuilt  0 result! ;

\ ---- actor commands ( actor args -- ): until the townsfolk and Matthew take them ----
: anim-once ( a x1 -- )  2 act-unbuilt ;   \ cmd $05
: anim-then-loop ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $06
: walk-to ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $08
: turn-by ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $09
: walk-forward ( a x1 -- )  2 act-unbuilt ;   \ cmd $0A
: run ( a x1 -- )  2 act-unbuilt ;   \ cmd $0B
: idle ( a -- )  1 act-unbuilt ;   \ cmd $0C
: face-point ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $0D
: face ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $0E
: walk-to-ex ( a x1 x2 x3 x4 -- )  5 act-unbuilt ;   \ cmd $0F
: move-forward ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $10
: turn-to ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $11
: set-floor ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $12
: anim-at ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $13
: talk-pose ( a x1 -- )  2 act-unbuilt ;   \ cmd $14
: restore-state ( a -- )  1 act-unbuilt ;   \ cmd $16
: anim-step ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $18
: arrive ( a -- )  1 act-unbuilt ;   \ cmd $1E
: freeze ( a -- )  1 act-unbuilt ;   \ cmd $1F
: unfreeze ( a -- )  1 act-unbuilt ;   \ cmd $20
: set-x ( a x1 -- )  2 act-unbuilt ;   \ cmd $21
: set-y ( a x1 -- )  2 act-unbuilt ;   \ cmd $22
: set-z ( a x1 -- )  2 act-unbuilt ;   \ cmd $23
: lift ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $24
: ride ( a x1 -- )  2 act-unbuilt ;   \ cmd $26
: unride ( a -- )  1 act-unbuilt ;   \ cmd $27
: attach ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $2C
: place-at-stop ( a -- )  1 act-unbuilt ;   \ cmd $2D
: item-query ( a x1 -- )  2 act-unbuilt ;   \ cmd $2E
: item-show ( a x1 -- )  2 act-unbuilt ;   \ cmd $2F
: act ( a -- )  1 act-unbuilt ;   \ cmd $36
: partner-command ( a x1 -- )  2 act-unbuilt ;   \ cmd $37
: move-by ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $38
: take-diner-seat ( a x1 -- )  2 act-unbuilt ;   \ cmd $3A
: sit-diner-seat ( a -- )  1 act-unbuilt ;   \ cmd $3B
: ignore-a ( a x1 -- )  2 act-unbuilt ;   \ cmd $3C
: ignore-b ( a x1 -- )  2 act-unbuilt ;   \ cmd $3D
: set-stop ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $43
: sit-club-seat ( a -- )  1 act-unbuilt ;   \ cmd $46
: run-to ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $47
: leave-diner-seat ( a -- )  1 act-unbuilt ;   \ cmd $48
: leave-club-seat ( a -- )  1 act-unbuilt ;   \ cmd $49
: examine ( a -- )  1 act-unbuilt ;   \ cmd $4A
: reset ( a -- )  1 act-unbuilt ;   \ cmd $4D
: kill-thread ( a -- )  1 act-unbuilt ;   \ cmd $4E
: save-state ( a -- )  1 act-unbuilt ;   \ cmd $4F
: door-hold ( a -- )  1 act-unbuilt ;   \ cmd $53
: door-release ( a -- )  1 act-unbuilt ;   \ cmd $54
: walk-path ( a x1 x2 x3 x4 x5 -- )  6 act-unbuilt ;   \ cmd $55
: depart-alt ( a -- )  1 act-unbuilt ;   \ cmd $56
: ending-schedule ( a x1 -- )  2 act-unbuilt ;   \ cmd $59
: go-to-actor ( a x1 -- )  2 act-unbuilt ;   \ cmd $5A
: goto-stop ( a x1 -- )  2 act-unbuilt ;   \ cmd $5D
: place-alt ( a -- )  1 act-unbuilt ;   \ cmd $5E

\ ---- the other ops: until their subsystems are built ----
: wait-stop ( a -- )  1 s" wait-stop" unbuilt  settle ;   \ op $10D
: wait-turned ( a -- )  1 s" wait-turned" unbuilt  settle ;   \ op $112
: sound ( n -- )  1 s" sound" unbuilt ;   \ op $114
: sequence-play ( n n -- )  2 s" sequence-play" unbuilt ;   \ op $115
: sequence-stop ( n -- )  1 s" sequence-stop" unbuilt ;   \ op $116
: wait-near ( a n n n -- )  4 s" wait-near" unbuilt  settle ;   \ op $117
: scene-reset ( n -- )  1 s" scene-reset" unbuilt ;   \ op $11A
: picture-show ( n -- )  1 s" picture-show" unbuilt ;   \ op $123
: picture-hide ( -- )  0 s" picture-hide" unbuilt ;   \ op $124
: door-reveal-room ( n -- )  1 s" door-reveal-room" unbuilt ;   \ op $126
: door-anim ( n -- )  1 s" door-anim" unbuilt ;   \ op $127
: warp ( n n n n n n -- )  6 s" warp" unbuilt ;   \ op $12A
: cd-picture ( n -- )  1 s" cd-picture" unbuilt ;   \ op $12B
: wait-follow ( a -- )  1 s" wait-follow" unbuilt  settle ;   \ op $138
: door-animate ( w1 -- )  1 s" door-animate" unbuilt ;   \ op $139
: door-animate-alt ( w1 -- )  1 s" door-animate-alt" unbuilt ;   \ op $13A
: through-door ( a w1 w2 -- )  3 s" through-door" unbuilt ;   \ op $13B
: music ( n -- )  1 s" music" unbuilt ;   \ op $144
: minigame ( n -- )  1 s" minigame" unbuilt ;   \ op $145
: elevator-doors ( n n -- )  2 s" elevator-doors" unbuilt ;   \ op $148
: wait-minigame ( -- )  0 s" wait-minigame" unbuilt ;   \ op $149
: rumble ( n n -- )  2 s" rumble" unbuilt ;   \ op $155
: object-anim ( n -- )  1 s" object-anim" unbuilt ;   \ op $156
: actor-model ( a n -- )  2 s" actor-model" unbuilt ;   \ op $159
: sprite-count+ ( -- )  0 s" sprite-count+" unbuilt ;   \ op $15D
: sprite-count-close ( -- )  0 s" sprite-count-close" unbuilt ;   \ op $15E
: through-door-back ( a w1 w2 -- )  3 s" through-door-back" unbuilt ;   \ op $15F
: face-each-other ( a a -- )  2 s" face-each-other" unbuilt ;   \ op $162
: map-reveal ( n -- )  1 s" map-reveal" unbuilt ;   \ op $165
: ambient-start ( n -- )  1 s" ambient-start" unbuilt ;   \ op $166
: ambient-stop ( n -- )  1 s" ambient-stop" unbuilt ;   \ op $167
: wait-attached ( a -- )  1 s" wait-attached" unbuilt  settle ;   \ op $16E
: set-357c ( n -- )  1 s" set-357c" unbuilt ;   \ op $170
: map-sync ( -- )  0 s" map-sync" unbuilt ;   \ op $172
: xa-play ( n -- )  1 s" xa-play" unbuilt ;   \ op $173
: xa-stop ( -- )  0 s" xa-stop" unbuilt ;   \ op $174
: door-sound ( n -- )  1 s" door-sound" unbuilt ;   \ op $176
: wait-attached-to ( a a -- )  2 s" wait-attached-to" unbuilt  settle ;   \ op $178
: cd-pictures ( n n -- )  2 s" cd-pictures" unbuilt ;   \ op $179

\ ---- text with a picture (after its stub) ----
: say-reset ( text -- )  picture-hide  matthew 0 0 show  text-pending settle ;              \ $105
: phone-say ( text -- )  0 picture-show  nobody phone-palette 0 show  text-pending settle ;   \ $106

\ ---- figures: the actor that is a slot's body (Matthew's so far; the townsfolk's as they are built) ----
: figure-of ( slot -- id )  matthew = if  s" matthew" actor-named  else  -1  then ;
: set-pos ( a x y z -- )   \ cmd $01 (src: actor_set_pos)
    {: a x y z :}  a figure-of dup 0< if  drop  a x y z 4 act-unbuilt  else  >r x y z r> send figure-place  then ;
: set-rot ( a rx ry rz -- )   \ cmd $02
    {: a rx ry rz :}  a figure-of dup 0< if  drop  a rx ry rz 4 act-unbuilt  else  >r rx ry rz r> send figure-turn  then ;

\ ---- the camera and the scene ----
: the-camera ( -- id )  s" camera" actor-named ;
: camera-set ( look-y dist eye-y x y z yaw -- )  the-camera send camera-place ;   \ $102
: camera-follow ( -- )  the-camera send camera-follow ;                           \ $10B
: camera-fixed ( -- )  the-camera send camera-fixed ;                             \ $10C
: set-scene ( scene -- )  s" areas" actor-named send show-scene ;                  \ $110 (src: set_scene_state)

\ ---- figures in motion (Matthew's so far) ----
: anim ( a id loop -- )   \ cmd $03 (src: actor_anim): play it (looping or not)
    {: a id lp :}  a figure-of dup 0< if  drop a id lp 3 act-unbuilt  else  >r id -1 lp r> send figure-anim  then ;
: anim-then ( a id next loop -- )   \ cmd $04: play it, then the next
    {: a id nx lp :}  a figure-of dup 0< if  drop a id nx lp 4 act-unbuilt  else  >r id nx lp r> send figure-anim  then ;
: anim-hold ( a -- )   \ cmd $07: at the motion's end the figure stays where it moved
    dup figure-of dup 0< if  drop 1 act-unbuilt  else  nip send figure-hold  then ;
: wait-anim ( a -- )   \ $103: until its motion has played (src: the VM's wait_anim)
    settle  figure-of 0< if  exit  then
    begin  yield  anim-done? until ;

\ ---- the screen's effects (docs/subsystems/effects.md) ----
: the-effects ( -- id )  s" effects" actor-named ;
: screen-effect ( n -- )  the-effects send screen-effect ;                \ $11D
: effect-level ( from to -- )  the-effects send effect-level ;           \ $161
: wait-effect ( -- )   \ $121: until it has run its frames, and two more (src: the VM's wait_effect)
    settle  begin  yield  effect-running? 0= until  2 frames wait ;

\ ---- the camera's moves (src: camera_move_to, camera_turn_to ...: docs/subsystems/camera.md) ----
: camera-move ( x y z speed -- )  the-camera send camera-move-to ;                   \ $119
: camera-angle ( yaw step -- )  the-camera send camera-turn-to ;                      \ $142
: camera-turn ( degrees step -- )   \ $11C: turned by so many degrees (step 0: at once)
    swap 4096 * 360 / camera-yaw +  swap camera-angle ;
: camera-height-to ( eye-y step -- )  the-camera send camera-height-to ;              \ $14E
: camera-dist-to ( dist step -- )  the-camera send camera-dist-to ;                   \ $154
: until-still ( xt -- )  settle  begin  yield  dup execute 0= until  drop  1 frames wait ;
: wait-camera-move ( -- )  ['] camera-moving? until-still ;                          \ $14D
: wait-camera-turn ( -- )  ['] camera-turning? until-still ;                         \ $14B
: wait-camera-height ( -- )  ['] camera-height-moving? until-still ;                 \ $14F
: wait-camera-dist ( -- )  ['] camera-dist-moving? until-still ;                     \ $160
: figure-place@ ( a -- x y z yaw true | false )   \ where a figure is (Matthew's so far)
    figure-of 0< if  false  else  matthew-now true  then ;
: camera-at ( a -- )   \ $113: the pivot where the figure stands
    figure-place@ if  drop the-camera send camera-pivot  then ;
: camera-heading ( a -- )   \ $150: the camera faces as the figure does
    figure-place@ if  >r 2drop drop r> the-camera send camera-face  then ;
: camera-face ( a b -- )   \ $11B: heading from one figure toward the other
    2 s" camera-face" unbuilt ;
