\ events.fs - the event scripts' language (docs/subsystems/story.md). The game's events were bytecode for
\ CEVENT.C's VM; tools/ev2forth.py turned each into Forth (scripts/events/), and this is the vocabulary they
\ are written in. An event runs as a task: its waits are real time (`n frames wait`: the events ran at 15
\ frames a second), a `goto` is a tail jump, and its own actor (`this-one`) and last result are its own.
\ Each op does its work through its subsystem's messages; an op whose subsystem isn't built yet is a stub
\ that takes its arguments and notes it (`unbuilt`).
IN: events
USING: engine actors messages clock areas window matthew townsfolk traffic doors effects camera ;

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

\ ---- the events running: a thread each (src: 5 threads and the main one), and the townsfolk's routes (src:
\ each actor's own thread: Actor +0x158) ----
8 constant #event-threads   72 constant #threads   \ (threads 8 .. 71: routes)
create thread-task    #threads cells allot   \ its task (0: free)
create thread-xt      #threads cells allot   \ the word it starts in
create thread-own     #threads cells allot   \ its own actor (this-one)
create thread-result  #threads cells allot   \ the last result an op gave (the VM's result register)
create thread-text    #threads cells allot   \ it waits for its message to close (src: the thread's wait_msg)
: threads-erase ( -- )
    thread-task #threads cells erase  thread-xt #threads cells erase  thread-own #threads cells erase
    thread-result #threads cells erase  thread-text #threads cells erase ;
threads-erase

variable events-running   \ how many events (routes aside)
variable control-given    \ an event gave Matthew back to the player (the rest no longer hold him)
: slot ( i addr -- addr' )  swap cells + ;
: thread ( -- i )   \ the running thread's (-1: not an event's or a route's)
    -1  #threads 0 do  i thread-task slot @ me = if  drop i leave  then  loop ;
: this-one ( -- actor )  thread thread-own slot @ ;
: as-this-one ( actor -- )  thread thread-own slot ! ;
variable the-prop   \ the actor the C gives a script it starts (src: the script's actor word set: the food, the drink)
: result! ( n -- )  thread dup 0< if  2drop  else  thread-result slot !  then ;
: result= ( n -- flag )  thread thread-result slot @ = ;

: the-clock ( -- id )  s" clock" actor-named ;
: hold ( reason -- )  the-clock send hold-clock ;
: release ( reason -- )  the-clock send release-clock ;

: free-thread ( first last -- i )  -1 -rot swap ?do  i thread-task slot @ 0= if  drop i leave  then  loop ;
: event-over ( i -- )   \ its thread is free; the last over, Matthew is the player's again
    0 swap thread-task slot !  -1 events-running +!
    events-running @ 0= if  no-control release  false control-given !  true broadcast player-control  then ;
: event-ended ( -- )  thread event-over ;
: thread-ended ( -- )  thread #event-threads < if  event-ended  else  0 thread thread-task slot !  then ;
: run-event ( -- )   \ a task's body: its word, then it's over
    thread thread-xt slot @ execute  thread-ended ;
: begin-thread ( xt actor i -- )
    {: xt a i :}  xt i thread-xt slot !  a i thread-own slot !  0 i thread-result slot !  0 i thread-text slot !
    ['] run-event forth:spawn  i thread-task slot ! ;   \ (a task: the actors' spawn is another)
: launch-event ( xt -- i )   \ src: event_start (CEvCon_Start): a thread, Matthew no longer the player's
    0 #event-threads free-thread {: xt i :}
    i 0< abort" no free event thread"
    xt matthew i begin-thread
    1 events-running +!  no-control hold  false broadcast player-control  i ;
: start-event ( xt -- )  launch-event drop ;
: event-alive? ( i -- flag )  dup 0< if  drop false exit  then  thread-task slot @ 0<> ;
: kill-event ( i -- )   \ src: event_thread_kill (a show cut short)
    dup event-alive? 0= if  drop exit  then  dup thread-task slot @ forth:kill  event-over ;
: route-thread ( actor -- i | -1 )   \ the route an actor walks now
    {: a :}  -1  #threads #event-threads do
        i thread-task slot @ 0<>  i thread-own slot @ a = and if  drop i leave  then
    loop ;
: end-route ( actor -- )
    route-thread dup 0< if  drop exit  then
    dup thread-task slot @ forth:kill  0 swap thread-task slot ! ;
: start-route ( xt actor -- )   \ src: event_thread_start on the actor's own thread: Matthew stays the player's
    dup end-route  #event-threads #threads free-thread {: xt a i :}
    i 0< abort" no free route thread"  xt a i begin-thread ;
: events-reset ( -- )  #event-threads 0 do  0 i thread-task slot !  loop  0 events-running !  false control-given ! ;
: events-stop ( -- )   \ every running event ended now (a tool: the console, tests); the routes go on
    #event-threads 0 do  i thread-task slot @ ?dup if  forth:kill  then  loop
    events-reset  no-control release  true broadcast player-control ;

\ the main executable's words found by address (the townsfolk's routes)
begin-structure main-entry  field: next-main  field: main-address  field: main-xt  end-structure
variable main-words
: main-word! ( xt address -- )
    here main-entry allot {: xt a e :}  main-words @ e next-main !  a e main-address !  xt e main-xt !  e main-words ! ;
: main-word ( address -- xt | 0 )
    {: a :}  main-words @  begin  dup while  dup main-address @ a = if  main-xt @ exit  then  next-main @  repeat ;

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
28 ovl constant ovl028  4 ovl constant ovl004   29 ovl constant ovl029  30 ovl constant ovl030
31 ovl constant ovl031
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
: act-unbuilt-command ( actor args.. n cmd -- )  drop 1+ act-unbuilt ;

: native ( scope address -- )  2dup find-native ?dup if  nip nip execute  else  2drop s" native" note-unbuilt  then ;
: native-result ( scope address -- )   \ (its answer is the result)
    2dup find-native ?dup if  nip nip execute result!  else  2drop s" native" note-unbuilt  0 result!  then ;

\ ---- the area overlays' words the main executable's scripts reach by address (in the area being played) ----
variable event-area   \ the area whose scripts are in before any is played (a new game's)
: script-area ( -- area )   \ the area whose overlay is in: the one played (its content area), else event-area
    area @ dup 0< if  drop event-area @  else  content  then ;
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
        dup overlay-area @ script-area =  over overlay-address @ a = and if  overlay-xt @ exit  then  next-overlay @
    repeat  true abort" no such word in this area's scripts" ;
: arrival-script ( area address -- xt | 0 )   \ a script of that area's by address (none: 0)
    {: ar a :}  overlay-words @  begin  dup while
        dup overlay-area @ ar =  over overlay-address @ a = and if  overlay-xt @ exit  then  next-overlay @
    repeat ;
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
    dup common-ids < if  script-area swap exit  then
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
: talk ( a text gesture -- )   \ $128 ...: the line, the gesture with it, then the thread waits for the line
    {: a text g :}  text a 0 0 show  a g gesture  text-pending settle ;   \ (src: event_talk sets wait_msg)
: talk-to ( a text gesture who -- )   \ $16F / $164: the portrait `who`'s (0: the speaker's)
    {: a text g who :}  text  who if  who  else  a  then  0 0 show  a g gesture  text-pending settle ;

\ ---- answers ----
: choice ( text kind -- )   \ src $11E: the question; its answer the result
    >r text-bank r> the-window send window-ask  text-pending settle  window-answer @ result! ;
: prompt ( a b -- )  2 s" prompt" unbuilt  1 result! ;
: fight ( a style b style -- )  4 s" fight" unbuilt  0 result! ;
: memory-card ( mode -- )  1 s" memory-card" unbuilt  0 result! ;

\ ---- actor commands ( actor args -- ): until the townsfolk and Matthew take them ----
: run ( a x1 -- )  2 act-unbuilt ;   \ cmd $0B
: face-point ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $0D
: anim-at ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $13
: talk-pose ( a x1 -- )  2 act-unbuilt ;   \ cmd $14
: restore-state ( a -- )  1 act-unbuilt ;   \ cmd $16
: anim-step ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $18
: freeze ( a -- )  1 act-unbuilt ;   \ cmd $1F
: unfreeze ( a -- )  1 act-unbuilt ;   \ cmd $20
: set-x ( a x1 -- )  2 act-unbuilt ;   \ cmd $21
: set-y ( a x1 -- )  2 act-unbuilt ;   \ cmd $22
: set-z ( a x1 -- )  2 act-unbuilt ;   \ cmd $23
: lift ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $24
: ride ( a x1 -- )  2 act-unbuilt ;   \ cmd $26
: unride ( a -- )  1 act-unbuilt ;   \ cmd $27
: place-at-stop ( a -- )  1 act-unbuilt ;   \ cmd $2D
: item-query ( a x1 -- )  2 act-unbuilt ;   \ cmd $2E
: item-show ( a x1 -- )  2 act-unbuilt ;   \ cmd $2F
: act ( a -- )  1 act-unbuilt ;   \ cmd $36
: partner-command ( a x1 -- )  2 act-unbuilt ;   \ cmd $37
: move-by ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $38
: ignore-a ( a x1 -- )  2 act-unbuilt ;   \ cmd $3C
: ignore-b ( a x1 -- )  2 act-unbuilt ;   \ cmd $3D
: set-stop ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $43
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
: place-alt ( a -- )  1 act-unbuilt ;   \ cmd $5E

\ ---- the other ops: until their subsystems are built ----
: sound ( n -- )  1 s" sound" unbuilt ;   \ op $114
: sequence-play ( n n -- )  2 s" sequence-play" unbuilt ;   \ op $115
: sequence-stop ( n -- )  1 s" sequence-stop" unbuilt ;   \ op $116
: wait-near ( a n n n -- )  4 s" wait-near" unbuilt  settle ;   \ op $117
: scene-reset ( n -- )  1 s" scene-reset" unbuilt ;   \ op $11A
: picture-show ( n -- )  1 s" picture-show" unbuilt ;   \ op $123
: picture-hide ( -- )  0 s" picture-hide" unbuilt ;   \ op $124
: cd-picture ( n -- )  1 s" cd-picture" unbuilt ;   \ op $12B
: music ( n -- )  1 s" music" unbuilt ;   \ op $144
: minigame ( n -- )  1 s" minigame" unbuilt ;   \ op $145
: elevator-doors ( n n -- )  2 s" elevator-doors" unbuilt ;   \ op $148
: wait-minigame ( -- )  0 s" wait-minigame" unbuilt ;   \ op $149
: rumble ( n n -- )  2 s" rumble" unbuilt ;   \ op $155
: actor-model ( a n -- )  2 s" actor-model" unbuilt ;   \ op $159
: sprite-count+ ( -- )  0 s" sprite-count+" unbuilt ;   \ op $15D
: sprite-count-close ( -- )  0 s" sprite-count-close" unbuilt ;   \ op $15E
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

\ ---- figures: Matthew's body, a scheduled person (the townsfolk), or not built yet ----
: figure-of ( slot -- id )  matthew = if  s" matthew" actor-named  else  -1  then ;
: person? ( slot -- flag )  dup matthew <> swap figure? and ;   \ (the townsfolk's: scheduled, or an extra)
: the-townsfolk ( -- id )  s" townsfolk" actor-named ;
: set-pos ( a x y z -- )   \ cmd $01 (src: actor_set_pos)
    {: a x y z :}
    a matthew = if  x y z a figure-of send figure-place exit  then
    a scheduled? if  a x z the-townsfolk send person-place exit  then
    a person? if  a x y z the-townsfolk send extra-place exit  then
    a x y z 4 act-unbuilt ;
: set-rot ( a rx ry rz -- )   \ cmd $02
    {: a rx ry rz :}
    a matthew = if  rx ry rz a figure-of send figure-turn exit  then
    a person? if  a ry the-townsfolk send person-turn exit  then
    a rx ry rz 4 act-unbuilt ;
30 constant walk-pace   80 constant run-pace     \ units a frame (src: actor_command_default's walk_to, path_to)
1 constant walk-motion   5 constant run-motion
: walk-with ( a x z speed motion -- )
    {: a x z v m :}
    a matthew = if  x z v m a figure-of send figure-walk exit  then
    a person? if  a x z v m the-townsfolk send person-walk  else  a x z 3 act-unbuilt  then ;
: walk-to ( a x z -- )  walk-pace walk-motion walk-with ;                    \ cmd $08
: run-to ( a x z -- )  run-pace run-motion walk-with ;                       \ cmd $47
: walk-to-ex ( a x z motion speed -- )  $FFFF and swap walk-with ;           \ cmd $0F
: go-forward ( a speed motion -- )   \ straight on along their heading until idle
    {: a v m :}
    a matthew = if  v m a figure-of send figure-forward exit  then
    a person? if  a v m the-townsfolk send person-forward  else  a v m 3 act-unbuilt  then ;
: walk-forward ( a speed -- )  walk-motion go-forward ;                     \ cmd $0A
: move-forward ( a motion speed -- )  swap go-forward ;                     \ cmd $10
: idle ( a -- )   \ cmd $0C
    dup matthew = if  figure-of send figure-idle exit  then
    dup person? if  the-townsfolk send person-idle  else  1 act-unbuilt  then ;
: turn-figure ( a heading speed -- )   \ toward the heading at the speed a game frame (0: at once)
    {: a h v :}
    a matthew = if  h v a figure-of send figure-turning exit  then
    a person? if  a h v the-townsfolk send person-turning  else  a h v 3 act-unbuilt  then ;
: turn-to ( a heading speed -- )  turn-figure ;   \ cmd $11 (src: actor_turn_to_heading)
: set-floor ( a area floor -- )   \ cmd $12
    nip  over person? if  the-townsfolk send person-floor  else  2 act-unbuilt  then ;
: arrive ( a -- )   \ cmd $1E
    dup scheduled? if  the-townsfolk send person-arrive  else  1 act-unbuilt  then ;
: goto-stop ( a stop -- )   \ cmd $5D
    over scheduled? if  the-townsfolk send person-goto-stop  else  2 act-unbuilt  then ;
: moving? ( a -- flag )  dup matthew = if  drop matthew-walking?  else  walking?  then ;
: wait-stop ( a -- )   \ $10D: until they stop walking (src: the VM's wait_move)
    settle  dup matthew = over person? or 0= if  drop exit  then
    begin  yield  dup moving? 0= until  drop ;
: turned? ( a -- flag )  dup matthew = if  drop matthew-turned?  else  person-turned?  then ;
: wait-turned ( a -- )   \ op $112: until they face the way they turn to (src: the VM's wait_turn)
    settle  dup matthew = over person? or 0= if  drop exit  then
    begin  yield  dup turned? until  drop ;

\ ---- riding (docs/subsystems/traffic.md) ----
: xz-heading ( x y z heading -- x z heading )  {: x y z h :}  x z h ;
: where-is ( a -- x z heading true | false )   \ anyone's place: Matthew, a person, a car
    dup matthew = if  drop matthew-now xz-heading true exit  then
    dup person? if  person-now xz-heading true exit  then
    dup car? if  car-now xz-heading true exit  then
    drop false ;
: turn-by ( a degrees speed -- )   \ cmd $09: from the way they face (src: actor_turn_by)
    {: a d v :}  a where-is 0= if  exit  then  nip nip {: h :}
    a  h d figures:degrees>heading +  figures:full-circle 1- and  v turn-figure ;
: walk-beside ( a b seat -- )   \ to a seat's side of b (src: actor_side_point, then path_to)
    {: a b k :}  b where-is 0= if  exit  then  k seat beside {: x z :}
    a x z walk-pace walk-motion walk-with ;
: attach ( a route speed -- )   \ cmd $2C: into their car, which drives the route (src: actor_attach)
    {: a n v :}  a scheduled? if  a n v the-townsfolk send person-attach  else  a n v 3 act-unbuilt  then ;
: wait-follow ( a -- )   \ op $138: until their car has arrived (src: the VM's wait_follow)
    settle  dup person? 0= if  drop exit  then
    begin  yield  dup riding? 0= until  drop ;
$41 constant to-car-door    $51 constant to-car-far-door   \ the actor commands for riding (src: CMESHAND.C)
$5A constant to-door-of     $5B constant to-far-door-of
$42 constant into-car       $4B constant at-car-door       \ ($4B: nothing for the townsfolk)
: riding-command ( a args.. n cmd -- )
    {: c :}  c to-car-door = c to-car-far-door = or if
        drop  dup partner-of  c to-car-door = if  0  else  1  then  walk-beside  exit
    then
    c to-door-of = c to-far-door-of = or if
        drop  c to-door-of = if  0  else  1  then  walk-beside  exit
    then
    c into-car = if  drop the-townsfolk send person-get-in  exit  then
    c at-car-door = if  drop drop  exit  then
    c act-unbuilt-command ;
\ ---- the stools (src: actor commands 0x39 - 0x3B, 0x44 - 0x46, 0x48, 0x49: SREG.C) ----
: seats-of? ( a -- flag )  dup matthew = swap person? or ;
: take-seat ( a bar seat -- )  {: a b n :}  a seats-of? if  a b n the-townsfolk send seat-take  settle  then ;
: take-diner-seat ( a seat -- )  at-diner swap take-seat ;   \ cmd $3A (-1: the first free)
: take-club-seat ( a seat -- )  at-club swap take-seat ;     \ cmd $45
: matthew-sits ( -- )   \ Matthew on the stool he took (his own body)
    matthew seat-of {: b n :}  b 0= if  exit  then
    b n seat-spot {: x y z h :}  x y z matthew figure-of send figure-place  0 h 0 matthew figure-of send figure-turn
    seated-motion -1 true matthew figure-of send figure-anim ;
: sit-seat ( a -- )   \ on the stool they took (a person's: the townsfolk place them)
    dup matthew = if  drop matthew-sits exit  then
    dup person? if  the-townsfolk send seat-sit  else  1 act-unbuilt  then ;
: sit-diner-seat ( a -- )  sit-seat ;   \ cmd $3B
: sit-club-seat ( a -- )  sit-seat ;    \ cmd $46
: leave-seat ( a -- )  dup seats-of? if  the-townsfolk send seat-leave  settle  else  1 act-unbuilt  then ;
: leave-diner-seat ( a -- )  leave-seat ;   \ cmd $48
: leave-club-seat ( a -- )  leave-seat ;    \ cmd $49
30 constant to-stool-pace
: go-seat ( a -- )   \ to their stool's front (src: cmds 0x39, 0x44: path_to at 30)
    {: a :}  a seat-of {: b n :}  b 0= if  exit  then
    a  b n seat-approach  to-stool-pace walk-motion walk-with ;
$39 constant to-diner-stool   $44 constant to-club-stool   $45 constant takes-club-stool
: seat-command? ( cmd -- flag )  dup to-diner-stool =  over to-club-stool = or  swap takes-club-stool = or ;
: seat-command ( actor args.. n cmd -- )
    takes-club-stool = if  drop take-club-seat exit  then
    drop go-seat ;
: act-command ( actor args.. n cmd -- )   \ a person's riding commands; the rest still to come
    dup seat-command? if  seat-command exit  then
    over 2 + pick scheduled? if  riding-command  else  act-unbuilt-command  then ;

\ ---- the camera and the scene ----
: the-camera ( -- id )  s" camera" actor-named ;
: camera-set ( look-y dist eye-y x y z yaw -- )  the-camera send camera-place ;   \ $102
: camera-follow ( -- )  the-camera send camera-follow ;                           \ $10B
: camera-fixed ( -- )  the-camera send camera-fixed ;                             \ $10C
: set-scene ( scene -- )  s" areas" actor-named send show-scene ;                  \ $110 (src: set_scene_state)

\ ---- figures in motion ----
: anim-then ( a id next loop -- )   \ cmd $04: play it, then the next
    {: a id nx lp :}
    a matthew = if  id nx lp a figure-of send figure-anim exit  then
    a person? if  a id nx lp the-townsfolk send person-anim exit  then
    a id nx lp 4 act-unbuilt ;
: anim ( a id loop -- )  -1 swap anim-then ;   \ cmd $03 (src: actor_anim): play it (looping or not)
: anim-once ( a id -- )  false anim ;               \ cmd $05: once, from its start
: anim-then-loop ( a id next -- )  false anim-then ;   \ cmd $06: once, then the next
: anim-hold ( a -- )   \ cmd $07: at the motion's end the figure stays where it moved
    dup matthew = if  figure-of send figure-hold exit  then
    dup person? if  the-townsfolk send person-hold exit  then
    1 act-unbuilt ;
: anim-finished? ( a -- flag )  dup matthew = if  drop anim-done?  else  person-anim-done?  then ;
: wait-anim ( a -- )   \ $103: until its motion has played (src: the VM's wait_anim)
    settle  dup matthew = over person? or 0= if  drop exit  then
    begin  yield  dup anim-finished? until  drop ;

\ ---- doors (docs/subsystems/doors.md) ----
\ Matthew steps through a door from the room he is in (src: func_80045008: put at its spot on his side, turned,
\ stepping through - a motion that holds its move; the door's opening and the next room's objects: to come)
$3F constant stepping-out-1   $40 constant stepping-out-0   \ (by the side's way)
-470 constant behind-look-y   -812 constant behind-dist   -1802 constant behind-eye-y   -693 constant behind-yaw
: my-room ( -- room )  area @ the-town = if  town-room  else  scene @  then ;
: object-anim ( entry -- )  s" areas" actor-named send play-area-anim ;   \ op $156 (the town's own: to come)
: door-anim ( door -- )   \ op $127: the door opening on Matthew's side
    {: n :}  n door-of door-area @ area @ <> if  exit  then
    n my-room door-spot  dup 0< if  drop  else  object-anim  then ;
: door-reveal-room ( door -- )  1 s" door-reveal-room" unbuilt ;   \ op $126 (the room beyond's objects: to come)
: matthew-through ( door -- way | 0 )   \ (where he went; the door opens on his side)
    {: n :}  n door-of door-area @  area @ <>  area @ the-town <> and if  0 exit  then
    n door-anim
    n my-room false door-way {: w :}  w 0= if  0 exit  then
    w out-x @ 0 w out-z @  matthew figure-of send figure-place
    0 w out-heading @ 0  matthew figure-of send figure-turn
    w out-way @ if  stepping-out-1  else  stepping-out-0  then  -1 false  matthew figure-of send figure-anim
    matthew figure-of send figure-hold  w ;
: door-animate ( door -- )  matthew-through drop ;   \ op $139
: door-animate-alt ( door -- )   \ op $13A: and a fixed camera behind him
    matthew-through ?dup 0= if  exit  then  {: w :}
    behind-look-y behind-dist behind-eye-y  w out-x @  matthew-now drop nip nip  w out-z @  behind-yaw
    the-camera send camera-place ;
: in-a-scene? ( -- flag )  thread dup 0< 0= swap #event-threads < and ;   \ (an event's thread, not a route's)
: stage-for ( door -- area | -1 )   \ a scene stages its people in the room being played: a door of this area,
    door-of door-area @ area @ =  in-a-scene? and if  area @  else  -1  then ;   \ its town side too, is here
: door-walk ( a door room flip -- )   \ src: func_8004535C: out at the door's far side, the thread waiting
    {: a n room flip :}                \ while they step through
    a person? 0= if  a n room 3 s" through-door" unbuilt  exit  then   \ (Matthew's: the doors, to come)
    a n room flip n stage-for the-townsfolk send person-through-door  a wait-anim ;
: through-door ( a door room -- )  false door-walk ;        \ op $13B, $140
: through-door-back ( a door room -- )  true door-walk ;    \ op $15F: coming the other way

\ ---- the screen's effects (docs/subsystems/effects.md) ----
: the-effects ( -- id )  s" effects" actor-named ;
: screen-effect ( n -- )  the-effects send screen-effect ;                \ $11D
: effect-level ( from to -- )  the-effects send effect-level ;           \ $161
: wait-effect ( -- )   \ $121: until it has run its frames, and two more (src: the VM's wait_effect)
    settle  begin  yield  effect-running? 0= until  2 frames wait ;

\ ---- into another area (src: func_80019F3C, func_80019D90, func_80018B44) ----
-1 constant as-he-is     \ (a place or heading of -1: Matthew's own)
2 constant fade-in       \ the screen effect a warp without a script ends in
: end-thread ( -- )  thread-ended  me forth:kill ;   \ (the event over: its task stops here)
: warp ( area scene x z heading script -- )   \ op $12A: the area changes, Matthew placed; then the script, or a fade in
    {: ar sc x z h scr :}
    ar  sc 0 max  s" areas" actor-named send enter-area  yield  ar event-area !
    x as-he-is <> if  x 0 z s" matthew" actor-named send figure-place  then
    h as-he-is <> if  0 h 0 s" matthew" actor-named send figure-turn  then
    yield  s" camera" actor-named send camera-reset
    scr if  ar scr $FFFFFFFF and arrival-script ?dup if  start-event  then  else  fade-in screen-effect  then
    end-thread ;

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
: figure-place@ ( a -- x y z yaw true | false )   \ where a figure is: Matthew, or a person standing here
    dup matthew = if  drop matthew-now true exit  then
    dup here? if  person-now true  else  drop false  then ;
: camera-at ( a -- )   \ $113: the pivot where the figure stands
    figure-place@ if  drop the-camera send camera-pivot  then ;
: camera-heading ( a -- )   \ $150: the camera faces as the figure does
    figure-place@ if  >r 2drop drop r> the-camera send camera-face  then ;
: camera-face ( a b -- )   \ $11B: heading from one figure toward the other
    2 s" camera-face" unbuilt ;

: heading-to ( x z x' z' -- heading )   \ from the first point toward the second (4096 a turn)
    rot - s>f  swap - s>f fswap fatan2 4096e f* 2e pi f* f/ f>s  4095 and ;
: face ( a target speed -- )   \ cmd $0E: the figure turns to face another (src: actor_turn_to_actor)
    {: a t v :}
    a figure-place@ 0= if  exit  then  drop nip {: ax az :}
    t figure-place@ 0= if  exit  then  drop nip {: tx tz :}
    a  ax az tx tz heading-to  v turn-figure ;
