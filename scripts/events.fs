\ events.fs - the event scripts' language (docs/subsystems/story.md). The game's events were bytecode for
\ CEVENT.C's VM; tools/ev2forth.py turned each into Forth (scripts/events/), and this is the vocabulary they
\ are written in. An event runs as a task: its waits are real time (`n frames wait`: the events ran at 15
\ frames a second), a `goto` is a tail jump, and its own actor (`this-one`) and last result are its own.
\ Each op does its work through its subsystem's messages; an op whose subsystem isn't built yet is a stub
\ that takes its arguments and notes it (`unbuilt`).
IN: events
USING: engine actors messages clock ;

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
    events-running @ 0= if  no-control release  false control-given !  then ;
: run-event ( -- )   \ a task's body: its word, then it's over
    thread thread-xt slot @ execute  event-ended ;
: start-event ( xt -- )   \ src: event_start (CEvCon_Start): a thread, Matthew no longer the player's
    free-thread {: xt i :}
    i 0< abort" no free event thread"
    xt i thread-xt slot !  matthew i thread-own slot !  0 i thread-result slot !
    ['] run-event forth:spawn  i thread-task slot !   \ (a task: the actors' spawn is another)
    1 events-running +!  no-control hold ;
: events-reset ( -- )  thread-task #threads cells erase  0 events-running !  false control-given ! ;

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
variable natives   \ a list: {link, scope, address, xt}
: native: ( scope address xt -- )  here natives @ , natives !  rot , swap , , ;
: find-native ( scope address -- xt | 0 )
    natives @  begin  dup while
        >r  over r@ cell+ @ =  over r@ 2 cells + @ = and if  2drop r> 3 cells + @ exit  then  r> @
    repeat  nip nip ;

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
variable overlay-words   \ a list: {link, area, address, xt}
: overlay-word! ( xt address area -- )  here overlay-words @ , overlay-words !  , , , ;
: overlay-word ( address -- xt )
    overlay-words @  begin  dup while
        dup cell+ @ event-area @ =  over 2 cells + @ 3 pick = and if  nip 3 cells + @ exit  then  @
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
: give-control ( -- )  true control-given !  no-control release ;   \ src: player_control_on
: take-control ( -- )  false control-given !  no-control hold ;
: time-skip ( minutes -- )   \ src: time_skip: the clock on a minute at a time (the townsfolk follow: to come)
    0 ?do  the-clock send advance-minute  loop ;

\ ---- answers: until their screens are built, the first choice ----
: choice ( text n -- )  2 s" choice" unbuilt  0 result! ;
: prompt ( a b -- )  2 s" prompt" unbuilt  1 result! ;
: fight ( a style b style -- )  4 s" fight" unbuilt  0 result! ;
: memory-card ( mode -- )  1 s" memory-card" unbuilt  0 result! ;

\ ---- actor commands ( actor args -- ): until the townsfolk and Matthew take them ----
: set-pos ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $01
: set-rot ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $02
: anim ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $03
: anim-then ( a x1 x2 x3 -- )  4 act-unbuilt ;   \ cmd $04
: anim-once ( a x1 -- )  2 act-unbuilt ;   \ cmd $05
: anim-then-loop ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $06
: anim-hold ( a -- )  1 act-unbuilt ;   \ cmd $07
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
: camera-set ( n n n n n n n -- )  7 s" camera-set" unbuilt ;   \ op $102
: wait-anim ( a -- )  1 s" wait-anim" unbuilt ;   \ op $103
: say-reset ( text -- )  1 s" say-reset" unbuilt ;   \ op $105
: phone-say ( text -- )  1 s" phone-say" unbuilt ;   \ op $106
: say ( text -- )  1 s" say" unbuilt ;   \ op $108
: say-by ( text a -- )  2 s" say-by" unbuilt ;   \ op $109
: wait-text ( -- )  0 s" wait-text" unbuilt ;   \ op $10A
: camera-follow ( -- )  0 s" camera-follow" unbuilt ;   \ op $10B
: camera-fixed ( -- )  0 s" camera-fixed" unbuilt ;   \ op $10C
: wait-stop ( a -- )  1 s" wait-stop" unbuilt ;   \ op $10D
: set-scene ( n -- )  1 s" set-scene" unbuilt ;   \ op $110
: wait-turned ( a -- )  1 s" wait-turned" unbuilt ;   \ op $112
: camera-at ( a -- )  1 s" camera-at" unbuilt ;   \ op $113
: sound ( n -- )  1 s" sound" unbuilt ;   \ op $114
: sequence-play ( n n -- )  2 s" sequence-play" unbuilt ;   \ op $115
: sequence-stop ( n -- )  1 s" sequence-stop" unbuilt ;   \ op $116
: wait-near ( a n n n -- )  4 s" wait-near" unbuilt ;   \ op $117
: camera-move ( n n n n -- )  4 s" camera-move" unbuilt ;   \ op $119
: scene-reset ( n -- )  1 s" scene-reset" unbuilt ;   \ op $11A
: camera-face ( a a -- )  2 s" camera-face" unbuilt ;   \ op $11B
: camera-turn ( n n -- )  2 s" camera-turn" unbuilt ;   \ op $11C
: screen-effect ( n -- )  1 s" screen-effect" unbuilt ;   \ op $11D
: wait-effect ( -- )  0 s" wait-effect" unbuilt ;   \ op $121
: picture-show ( n -- )  1 s" picture-show" unbuilt ;   \ op $123
: picture-hide ( -- )  0 s" picture-hide" unbuilt ;   \ op $124
: say-by-wait ( text a -- )  2 s" say-by-wait" unbuilt ;   \ op $125
: door-reveal-room ( n -- )  1 s" door-reveal-room" unbuilt ;   \ op $126
: door-anim ( n -- )  1 s" door-anim" unbuilt ;   \ op $127
: talk ( a text n -- )  3 s" talk" unbuilt ;   \ op $128
: warp ( n n n n n n -- )  6 s" warp" unbuilt ;   \ op $12A
: cd-picture ( n -- )  1 s" cd-picture" unbuilt ;   \ op $12B
: say-mode-wait ( text n -- )  2 s" say-mode-wait" unbuilt ;   \ op $137
: wait-follow ( a -- )  1 s" wait-follow" unbuilt ;   \ op $138
: door-animate ( w1 -- )  1 s" door-animate" unbuilt ;   \ op $139
: door-animate-alt ( w1 -- )  1 s" door-animate-alt" unbuilt ;   \ op $13A
: through-door ( a w1 w2 -- )  3 s" through-door" unbuilt ;   \ op $13B
: say-n ( text n a -- )  3 s" say-n" unbuilt ;   \ op $13C
: say-timed-by ( text a secs -- )  3 s" say-timed-by" unbuilt ;   \ op $13D
: text-hold ( -- )  0 s" text-hold" unbuilt ;   \ op $13E
: say-timed-by-wait ( text a secs -- )  3 s" say-timed-by-wait" unbuilt ;   \ op $13F
: camera-angle ( n n -- )  2 s" camera-angle" unbuilt ;   \ op $142
: music ( n -- )  1 s" music" unbuilt ;   \ op $144
: minigame ( n -- )  1 s" minigame" unbuilt ;   \ op $145
: elevator-doors ( n n -- )  2 s" elevator-doors" unbuilt ;   \ op $148
: wait-minigame ( -- )  0 s" wait-minigame" unbuilt ;   \ op $149
: wait-camera-turn ( -- )  0 s" wait-camera-turn" unbuilt ;   \ op $14B
: text-close ( -- )  0 s" text-close" unbuilt ;   \ op $14C
: wait-camera-move ( -- )  0 s" wait-camera-move" unbuilt ;   \ op $14D
: camera-height-to ( n n -- )  2 s" camera-height-to" unbuilt ;   \ op $14E
: wait-camera-height ( -- )  0 s" wait-camera-height" unbuilt ;   \ op $14F
: camera-heading ( a -- )  1 s" camera-heading" unbuilt ;   \ op $150
: say-mode ( text n -- )  2 s" say-mode" unbuilt ;   \ op $153
: camera-dist-to ( n n -- )  2 s" camera-dist-to" unbuilt ;   \ op $154
: rumble ( n n -- )  2 s" rumble" unbuilt ;   \ op $155
: object-anim ( n -- )  1 s" object-anim" unbuilt ;   \ op $156
: say-wait-close ( text -- )  1 s" say-wait-close" unbuilt ;   \ op $158
: actor-model ( a n -- )  2 s" actor-model" unbuilt ;   \ op $159
: caption-for ( text secs -- )  2 s" caption-for" unbuilt ;   \ op $15B
: sprite-count+ ( -- )  0 s" sprite-count+" unbuilt ;   \ op $15D
: sprite-count-close ( -- )  0 s" sprite-count-close" unbuilt ;   \ op $15E
: through-door-back ( a w1 w2 -- )  3 s" through-door-back" unbuilt ;   \ op $15F
: wait-camera-dist ( -- )  0 s" wait-camera-dist" unbuilt ;   \ op $160
: effect-level ( n n -- )  2 s" effect-level" unbuilt ;   \ op $161
: face-each-other ( a a -- )  2 s" face-each-other" unbuilt ;   \ op $162
: talk-to ( a text n n -- )  4 s" talk-to" unbuilt ;   \ op $164
: map-reveal ( n -- )  1 s" map-reveal" unbuilt ;   \ op $165
: ambient-start ( n -- )  1 s" ambient-start" unbuilt ;   \ op $166
: ambient-stop ( n -- )  1 s" ambient-stop" unbuilt ;   \ op $167
: wait-attached ( a -- )  1 s" wait-attached" unbuilt ;   \ op $16E
: set-357c ( n -- )  1 s" set-357c" unbuilt ;   \ op $170
: map-sync ( -- )  0 s" map-sync" unbuilt ;   \ op $172
: xa-play ( n -- )  1 s" xa-play" unbuilt ;   \ op $173
: xa-stop ( -- )  0 s" xa-stop" unbuilt ;   \ op $174
: door-sound ( n -- )  1 s" door-sound" unbuilt ;   \ op $176
: caption-for-nowait ( text secs -- )  2 s" caption-for-nowait" unbuilt ;   \ op $177
: wait-attached-to ( a a -- )  2 s" wait-attached-to" unbuilt ;   \ op $178
: cd-pictures ( n n -- )  2 s" cd-pictures" unbuilt ;   \ op $179
