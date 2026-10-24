\ events.fs - the event scripts' language (docs/subsystems/story.md). The game's events were bytecode for
\ CEVENT.C's VM; tools/ev2forth.py turned each into Forth (scripts/events/), and this is the vocabulary they
\ are written in. An event runs as a task: its waits are real time (`n frames wait`: the events ran at 15
\ frames a second), a `goto` is a tail jump, and its own actor (`this-one`) and last result are its own.
\ Each op does its work through its subsystem's messages; an op whose subsystem isn't built yet is a stub
\ that takes its arguments and notes it (`unbuilt`).
IN: events
USING: engine actors messages keys clock areas floors window matthew townsfolk traffic doors effects camera fight story-flags
    pictures ;

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
variable picture-thread   -1 picture-thread !   \ the event that showed the big picture (src: its thread's `picture`)
: pictures-gone ( -- )   \ src: thread_end: the small picture hidden; its big one with the thread
    s" pictures" actor-named {: p :}  p send hide-picture
    thread picture-thread @ = if  -1 picture-thread !  0 0 p send show-big-picture  then ;
: thread-ended ( -- )  pictures-gone  thread #event-threads < if  event-ended  else  0 thread thread-task slot !  then ;
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
\ the events' lock (src: event_lock / event_unlock, a count): while it's on the clock stands - and with it the
\ townsfolk's minutes - a scene keeping the world as it is (hiding, a rescue, the last evening)
variable event-locks
: event-locked? ( -- flag )  event-locks @ 0> ;
: event-lock ( -- )  1 event-locks +!  events-locked s" clock" actor-named send hold-clock ;
: event-unlock ( -- )
    event-locks @ 0> if  -1 event-locks +!  then
    event-locked? 0= if  events-locked s" clock" actor-named send release-clock  then ;
: events-reset ( -- )   \ (src: events_reset: the lock off too)
    #event-threads 0 do  0 i thread-task slot !  loop  0 events-running !  false control-given !
    event-locks @ if  0 event-locks !  events-locked s" clock" actor-named send release-clock  then ;
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
31 ovl constant ovl031  8 ovl constant ovl008   10 ovl constant ovl010  11 ovl constant ovl011
17 ovl constant ovl017  18 ovl constant ovl018  23 ovl constant ovl023  32 ovl constant ovl032
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
: time-skip ( minutes -- )   \ src: time_skip: the clock on a minute at a time (each a time-set: the townsfolk
                                \ settle at the stop for it - townsfolk.fs's settle-all)
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
: portrait-of ( who -- kind | -1 )   \ the speaker's portrait: an actor's kind, or from $FD a kind itself
    dup 0< if  exit  then  dup $FD >= if  exit  then
    dup matthew = if  drop 0 exit  then
    dup figure? if  kind-of exit  then  drop -1 ;
: show ( text speaker palette secs -- )
    {: text who pal secs :}  text text-bank who portrait-of pal secs the-window send window-say ;
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
defer gesture ( a n -- )   \ a talk's gesture (below, with the figures' motions)
: talk ( a text gesture -- )   \ $128 ...: the line, the gesture with it, then the thread waits for the line
    {: a text g :}  text a 0 0 show  a g gesture  text-pending settle ;   \ (src: event_talk sets wait_msg)
: talk-to ( a text gesture who -- )   \ $16F / $164: the portrait `who`'s (0: the speaker's)
    {: a text g who :}  text  who if  who  else  a  then  0 0 show  a g gesture  text-pending settle ;

\ ---- answers ----
: choice ( text kind -- )   \ src $11E: the question; its answer the result
    >r text-bank r> the-window send window-ask  text-pending settle  window-answer @ result! ;
variable prompt-left   -1 prompt-left !   variable prompt-right   -1 prompt-right !   \ (facts: the HUD draws them)
: prompt-shown? ( -- flag )  prompt-left @ 0< 0=  prompt-right @ 0< 0= or ;
1 constant circle-answer   0 constant square-answer
variable prompt-circle-was   variable prompt-square-was
: pressed-now ( key var -- flag )  {: k v :}  k key-down? dup v @ 0= and  swap v ! ;
: prompt-pressed ( -- answer | -1 )
    pad-circle prompt-circle-was pressed-now  pad-square prompt-square-was pressed-now  {: c s :}
    c if  circle-answer exit  then  s if  square-answer exit  then  -1 ;
: prompt ( left right -- )   \ op $129: □'s icon and ○'s beside the box, until one is pressed: ○ 1, □ 0 (src: the VM)
    settle  prompt-right !  prompt-left !
    pad-circle key-down? prompt-circle-was !  pad-square key-down? prompt-square-was !
    begin  yield  prompt-pressed dup 0< while  drop  repeat  result!
    -1 prompt-left !  -1 prompt-right ! ;
: fight ( a style b style -- )   \ op $169: until it's won (src: wait_fight; the result its winner)
    s" fight" actor-named send fight-start  settle  yield
    begin  fighting? while  yield  repeat  fight-winner @ result! ;
defer memory-card ( slot -- )   \ op $152 (saves.fs: the save dialog; its result 0 saved, 1 not)

\ ---- actor commands ( actor args -- ): until the townsfolk and Matthew take them ----


: talk-pose ( a x1 -- )  2 act-unbuilt ;   \ cmd $14  (no script uses it)

: anim-step ( a x1 x2 -- )  3 act-unbuilt ;   \ cmd $18  (no script uses it)






defer answer-item ( a item -- answer )   \ (showing.fs)
defer shown-item ( a answer -- )
: item-query ( a item -- )  answer-item result! ;   \ cmd $2E: what they'd answer to the item
: item-show ( a answer -- )  shown-item ;          \ cmd $2F: shown it
: act ( a -- )  1 act-unbuilt ;   \ cmd $36  (no script uses it)




: examine ( a -- )  1 act-unbuilt ;   \ cmd $4A  (no script uses it)

: kill-thread ( a -- )  1 act-unbuilt ;   \ cmd $4E  (no script uses it)



: depart-alt ( a -- )   \ cmd $56: off along their stop's route (src: the labourers' CMD_DEPART_ALT)
    dup scheduled? if  s" townsfolk" actor-named send person-depart  else  1 act-unbuilt  then ;

: go-to-actor ( a x1 -- )  2 act-unbuilt ;   \ cmd $5A  (no script uses it)
: place-alt ( a -- )  1 act-unbuilt ;   \ cmd $5E  (no script uses it)

\ ---- the other ops: until their subsystems are built ----
: sound ( n -- )  s" sound" actor-named send play-sound ;   \ op $114 (src: sound_play)
: sequence-play ( id loop? -- )  0<> s" sound" actor-named send play-sequence ;   \ op $115 (src: seq_play)
: sequence-stop ( id -- )  s" sound" actor-named send stop-sequence ;   \ op $116
: the-pictures ( -- id )  s" pictures" actor-named ;
: picture-show ( n -- )  the-pictures send show-picture ;   \ op $123
: picture-hide ( -- )  the-pictures send hide-picture ;     \ op $124
: big-picture ( left right -- )   \ (0 0: none)
    over 0= if  2drop 0 0  then  over if  thread  else  -1  then  picture-thread !  the-pictures send show-big-picture ;
: cd-picture ( n -- )  0 big-picture ;   \ op $12B (src: cd_picture: n, 0 none)
: music ( row -- )  s" sound" actor-named send play-music ;   \ op $144 (src: music_play)
\ the minigames that are a script alone (src: an area's area_minigame_start setting minigame_script), by number
64 constant #minigames
create minigame-scripts  #minigames cells allot   minigame-scripts #minigames cells erase
: minigame-script! ( xt n -- )  cells minigame-scripts + ! ;
: minigame-xt ( n -- xt | 0 )  dup #minigames u< if  cells minigame-scripts + @  else  drop 0  then ;
variable minigame-thread   -1 minigame-thread !   \ the minigame running: its script's thread, and the script
variable minigame-running
\ the minigames that are an area's code and no script of their own (src: area_minigame_start setting a state):
\ their word, run at once
create minigame-codes  #minigames cells allot   minigame-codes #minigames cells erase
: minigame-code! ( xt n -- )  cells minigame-codes + ! ;
: minigame-code ( n -- xt | 0 )  dup #minigames u< if  cells minigame-codes + @  else  drop 0  then ;
: minigame ( n -- )   \ op $145 (src: minigame_start): a script's, else an area's code (else noted unbuilt)
    dup minigame-xt ?dup if  nip dup minigame-running !  launch-event minigame-thread !  exit  then
    dup minigame-code ?dup if  nip execute exit  then
    1 s" minigame" unbuilt ;
variable minigame-busy   \ a code minigame not over yet (src: minigame_id still set: the car chase)
: minigame-begun ( -- )  true minigame-busy ! ;
: minigame-over ( -- )  false minigame-busy ! ;
: minigame-running? ( -- flag )   \ (its thread still that script's; or a code minigame on)
    minigame-busy @ if  true exit  then
    minigame-thread @ dup event-alive? 0= if  drop false exit  then  thread-xt slot @ minigame-running @ = ;
2 constant the-hospital-content
: elevator-doors ( set open? -- )   \ op $148: the hospital's (by its content area) - four of its objects' motions
    area @ 0< if  2drop exit  then   \ (src: ovl002 func_80104108)
    area @ content the-hospital-content <> if  2drop exit  then
    if  s" elevator-open"  else  s" elevator-close"  then  {: set tab n :}
    4 0 do  tab n  set 4 * 3 i - +  exe-u8  s" areas" actor-named send play-area-anim  loop ;
: wait-minigame ( -- )   \ op $149: until the minigame's over
    settle  begin  minigame-running? while  yield  repeat ;
: rumble ( strength frames -- )   \ op $155: 0x100 the small motor, else the large at that strength
    {: v n :}  s" rumble" actor-named {: r :}  v $100 = if  0 n r send rumble-small  else  0 v n r send rumble-large  then ;
: sprite-count+ ( -- )  0 s" sprite-count+" unbuilt ;   \ op $15D  (no script uses it)
: sprite-count-close ( -- )  0 s" sprite-count-close" unbuilt ;   \ op $15E  (no script uses it)

: map-reveal ( place -- )  1 swap lshift map-known @ or map-known ! ;   \ op $165 (src: map_reveal: the map knows it)
: ambient-start ( id -- )  s" sound" actor-named send loop-sound ;   \ op $166 (src: ambient_start)
: ambient-stop ( id -- )  s" sound" actor-named send stop-loop ;   \ op $167

variable fire-lit   \ src: D_800A357C: area 5's fire burning (its object shown)
10 constant the-fire   \ (area 5's object 10, in its first scene; its flames: rules/area005.fs)
: set-357c ( n -- )  dup fire-lit !  the-fire swap 0<> s" areas" actor-named send object-shown ;   \ op $170
: map-sync ( -- ) ;   \ op $172 (src: waits for the town's tiles: ours are there when drawn)
: xa-play ( n -- )  s" sound" actor-named send play-stream ;   \ op $173 (src: xa_play)
: xa-stop ( -- )  s" sound" actor-named send stop-stream ;   \ op $174
defer door-sound ( kind -- )   \ op $176 (door-sounds.fs)
:noname ( kind -- )  1 s" door-sound" unbuilt ; is door-sound
: wait-attached-to ( a a -- )  2 s" wait-attached-to" unbuilt  settle ;   \ op $178  (no script uses it)
: cd-pictures ( left right -- )  dup 0= if  2drop 0 0  then  big-picture ;   \ op $179: none when right is 0

\ ---- text with a picture (after its stub) ----
: say-reset ( text -- )  picture-hide  matthew 0 0 show  text-pending settle ;              \ $105
: phone-say ( text -- )  0 picture-show  nobody phone-palette 0 show  text-pending settle ;   \ $106

\ ---- figures: Matthew's body, a scheduled person (the townsfolk), or not built yet ----
: figure-of ( slot -- id )  matthew = if  s" matthew" actor-named  else  -1  then ;
: person? ( slot -- flag )  dup matthew <> swap figure? and ;   \ (the townsfolk's: scheduled, or an extra)
: the-townsfolk ( -- id )  s" townsfolk" actor-named ;
: the-traffic ( -- id )  s" traffic" actor-named ;
: set-pos ( a x y z -- )   \ cmd $01 (src: actor_set_pos)
    {: a x y z :}
    a matthew = if  x y z a figure-of send figure-place exit  then
    a scheduled? if  a x z the-townsfolk send person-place exit  then
    a person? if  a x y z the-townsfolk send extra-place exit  then
    a car? if  a x y z the-traffic send car-put exit  then
    a x y z 4 act-unbuilt ;
: set-rot ( a rx ry rz -- )   \ cmd $02
    {: a rx ry rz :}
    a matthew = if  rx ry rz a figure-of send figure-turn exit  then
    a person? if  a ry the-townsfolk send person-turn  a rx rz the-townsfolk send person-tilt exit  then
    a car? if  a ry the-traffic send car-heading exit  then
    a rx ry rz 4 act-unbuilt ;
30 constant walk-pace   80 constant run-pace     \ units a frame (src: actor_command_default's walk_to, path_to)
1 constant walk-motion   5 constant run-motion
: walk-with ( a x z speed motion -- )
    {: a x z v m :}
    a matthew = if  x z v m a figure-of send figure-walk exit  then
    a person? if  a x z v m the-townsfolk send person-walk exit  then
    a car? if  a x z v the-traffic send car-walk exit  then   \ (src: walk_to on a car: driven there)
    a x z 3 act-unbuilt ;
: walk-to ( a x z -- )  walk-pace walk-motion walk-with ;                    \ cmd $08
: run-to ( a x z -- )  run-pace run-motion walk-with ;                       \ cmd $47
: walk-to-ex ( a x z motion speed -- )  $FFFF and swap walk-with ;           \ cmd $0F
: go-forward ( a speed motion -- )   \ straight on along their heading until idle
    {: a v m :}
    a matthew = if  v m a figure-of send figure-forward exit  then
    a person? if  a v m the-townsfolk send person-forward exit  then
    a car? if  a v the-traffic send car-forward exit  then
    a v m 3 act-unbuilt ;
: walk-forward ( a speed -- )  walk-motion go-forward ;                     \ cmd $0A
: move-forward ( a motion speed -- )  swap go-forward ;                     \ cmd $10
: idle ( a -- )   \ cmd $0C
    dup matthew = if  figure-of send figure-idle exit  then
    dup person? if  the-townsfolk send person-idle exit  then
    dup car? if  the-traffic send car-idle exit  then
    1 act-unbuilt ;
: turn-figure ( a heading speed -- )   \ toward the heading at the speed a game frame (0: at once)
    {: a h v :}
    a matthew = if  h v a figure-of send figure-turning exit  then
    a person? if  a h v the-townsfolk send person-turning exit  then
    a car? if  a h v the-traffic send car-turning exit  then
    a h v 3 act-unbuilt ;
: turn-to ( a heading speed -- )  turn-figure ;   \ cmd $11 (src: actor_turn_to_heading)
: set-floor ( a area floor -- )   \ cmd $12 (src: actor_set_floor: Matthew's, and one going along with him too)
    {: a ar f :}
    a matthew = if  f s" matthew" actor-named send figure-floor
        following? if  companion @ f the-townsfolk send person-floor  then  exit  then
    a person? if  a f the-townsfolk send person-floor exit  then
    a car? if  a ar the-traffic send car-area exit  then   \ (a car: the area it's in)
    a f 2 act-unbuilt ;
: arrive ( a -- )   \ cmd $1E
    dup scheduled? if  the-townsfolk send person-arrive  else  1 act-unbuilt  then ;
: goto-stop ( a stop -- )   \ cmd $5D
    over scheduled? if  the-townsfolk send person-goto-stop  else  2 act-unbuilt  then ;
: set-stop ( a stop travelling -- )   \ cmd $43: their stop, not placed (-1: off their schedule, with Matthew)
    rot dup scheduled? if  -rot the-townsfolk send person-set-stop  else  -rot 3 act-unbuilt  then ;
: moving? ( a -- flag )
    dup matthew = if  drop matthew-walking? exit  then  dup car? if  car-moving? exit  then  walking? ;
: wait-stop ( a -- )   \ $10D: until they stop walking (src: the VM's wait_move)
    settle  dup matthew = over person? or  over car? or 0= if  drop exit  then
    begin  yield  dup moving? 0= until  drop ;
: turned? ( a -- flag )
    dup matthew = if  drop matthew-turned? exit  then  dup car? if  car-turned? exit  then  person-turned? ;
: wait-turned ( a -- )   \ op $112: until they face the way they turn to (src: the VM's wait_turn)
    settle  dup matthew = over person? or  over car? or 0= if  drop exit  then
    begin  yield  dup turned? until  drop ;

\ ---- riding (docs/subsystems/traffic.md) ----
: xz-heading ( x y z heading -- x z heading )  {: x y z h :}  x z h ;
: where-is ( a -- x z heading true | false )   \ anyone's place: Matthew, a person, a car
    dup matthew = if  drop matthew-now xz-heading true exit  then
    dup person? if  person-now xz-heading true exit  then
    dup car? if  car-now xz-heading true exit  then
    drop false ;
\ who stands in Matthew's way (src: player_probe_blocked, actor_overlaps_point: actors 26 - 60, the town's cars and
\ props - within his radius and theirs, theirs halved to their sides)
26 constant first-in-the-way   61 constant past-in-the-way
$1A1 constant side-from   $4BF constant side-span   $9A0 constant other-side-from   $4C1 constant other-side-span
: radius-of ( slot -- r )  {: s :}  s" actor-radii" s exe-s16 ;
: to-their-side? ( x z bx bz bh -- flag )
    {: x z bx bz bh :}  x bx - s>f  z bz - s>f  fatan2 4096e f* 2e pi f* f/ f>s  bh - 4095 and {: a :}
    a side-from - side-span u<  a other-side-from - other-side-span u< or ;
: one-in-the-way? ( x z slot -- flag )
    {: x z s :}
    s car? if  area @ the-town <>  s car-shown? 0= or if  false exit  then  then   \ (on the roads, in the town)
    s person? if  s here? 0= if  false exit  then  then
    s where-is 0= if  false exit  then  {: bx bz bh :}
    s radius-of  x z bx bz bh to-their-side? if  2/  then  {: rb :}
    x bx - s>f fdup f*  z bz - s>f fdup f* f+ fsqrt f>s  matthew radius-of rb + < ;
: anyone-in-the-way? ( x z -- flag )
    {: x z :}  past-in-the-way first-in-the-way do  x z i one-in-the-way? if  true unloop exit  then  loop  false ;
' anyone-in-the-way? is in-the-way?
\ and taken at all (src: func_80041928): those, or a person here on his floor within his radius and theirs - in
\ his car its own place left out (the car is what stands there)
26 constant his-own-car   182 constant #actors
variable his-ignore-a   variable his-ignore-b   -1 his-ignore-a !  -1 his-ignore-b !   \ (src: Matthew's ignore_a, _b)
: person-there? ( x z slot -- flag )
    {: x z s :}  s person? 0= if  false exit  then  s here? 0= if  false exit  then
    s his-ignore-a @ =  s his-ignore-b @ = or if  false exit  then
    s person-of on-floor @ floor @ <> if  false exit  then
    s person-now drop nip {: bx bz :}
    x bx - s>f fdup f*  z bz - s>f fdup f* f+ fsqrt f>s  matthew radius-of s radius-of + < ;
: taken-at? ( x z -- flag )
    {: x z :}
    first-in-the-way 1 do  x z i person-there? if  true unloop exit  then  loop
    past-in-the-way first-in-the-way do
        i his-own-car =  matthew-perch perch-in-car = and 0= if
            x z i one-in-the-way? if  true unloop exit  then  then  loop
    #actors past-in-the-way do  x z i person-there? if  true unloop exit  then  loop  false ;
' taken-at? is taken?
: turn-by ( a degrees speed -- )   \ cmd $09: from the way they face (src: actor_turn_by)
    {: a d v :}  a where-is 0= if  exit  then  nip nip {: h :}
    a  h d figures:degrees>heading +  figures:full-circle 1- and  v turn-figure ;
: ride ( a mode -- )   \ cmd $26: along with Matthew (src: actor_ride)
    over person? if  the-townsfolk send person-ride  else  2 act-unbuilt  then ;
: unride ( a -- )   \ cmd $27 (src: actor_unride)
    dup person? if  the-townsfolk send person-unride  else  1 act-unbuilt  then ;
: place-at-stop ( a -- )   \ cmd $2D: at their stop again (src: actor_apply_placement)
    dup scheduled? if  dup person-of at-stop @ the-townsfolk send person-goto-stop  else  1 act-unbuilt  then ;
: door-hold ( a -- )   \ cmd $53: they wait at the door they answered
    dup person? if  the-townsfolk send person-door-hold  else  1 act-unbuilt  then ;
: actor-model ( a kind -- )   \ op $159: the figure shows that kind's model (src: actor_model_variant)
    over person? if  the-townsfolk send person-variant  else  2 s" actor-model" unbuilt  then ;
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
\ walking to a node of the town's paths (src: cmd 0x4C: the paths' graph, 033_4 part 5 - walkers.fs) and riding
\ along in someone's car (cmd 0x50)
$4C constant to-path-node   $50 constant into-their-car   30 constant path-pace   32 constant path-node-bytes
: path-node ( n -- x z )
    path-node-bytes *  s" cdbin/033_4/5/0.bin" library-file drop +  dup sl@  swap 4 + sl@ ;
: to-node ( a n -- )  path-node {: a x z :}  a x z path-pace walk-motion walk-with ;
: in-their-car ( a car -- )  over person? if  the-townsfolk send person-in-car  else  2drop  then ;
: own-command? ( cmd -- flag )  dup to-path-node =  swap into-their-car = or ;
: own-command ( actor arg 1 cmd -- )  swap drop  to-path-node = if  to-node  else  in-their-car  then ;
: act-command ( actor args.. n cmd -- )   \ (src: actor_command_default; the commands it doesn't list it ignores)
    dup seat-command? if  seat-command exit  then
    dup own-command? if  own-command exit  then
    over 2 + pick scheduled? if  riding-command exit  then
    drop 0 ?do  drop  loop  drop ;

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
    a car? if  a id nx the-traffic send car-anim exit  then   \ (a car's own motion: the bike's)
    a id nx lp 4 act-unbuilt ;
: anim ( a id loop -- )  -1 swap anim-then ;   \ cmd $03 (src: actor_anim): play it (looping or not)
: anim-once ( a id -- )  false anim ;               \ cmd $05: once, from its start
: anim-then-loop ( a id next -- )  false anim-then ;   \ cmd $06: once, then the next
\ a talk's gesture (src: event_talk): a motion, then what follows (-1: its last frame held); none while sitting
\ but the seated ones (9 - 12), none while in a resting pose but 8; Barbara's own for the plain ones
create gesture-motions   0 , 0 ,   82 , -1 ,   83 , 84 ,   86 , -1 ,   87 , -1 ,   88 , -1 ,   89 , -1 ,   90 , -1 ,
    79 , 72 ,   2701 , 119 ,   2702 , 2703 ,   2705 , 119 ,   2706 , 119 ,   706 , -1 ,
14 constant #gestures   2 constant raised-hand   8 constant at-rest-gesture   10 constant seated-nod
9 constant first-seated   13 constant past-seated   13 constant barbaras-gesture   90 constant barbara-slot
create resting-motions  68 , 72 , 161 , 167 ,   \ src: D_80080438 (and an area's own below)
: area-resting ( -- id | -1 )   \ src: func_80018F2C's by the area
    area @ case  0 of  $13B  endof  2 of  $2E8  endof  29 of  $2E8  endof  30 of  $2E8  endof  16 of  $DAE  endof  -1 swap  endcase ;
: resting? ( id -- flag )
    {: id :}  id 0< if  false exit  then   \ (none playing)
    4 0 do  resting-motions i cells + @ id = if  true unloop exit  then  loop  id area-resting = ;
: anim-of ( a -- id next )  dup matthew = if  drop matthew-anim-now  else  anim-now  then ;
: sitting? ( a -- flag )  dup matthew = if  drop matthew-perch 2 =  else  seated?  then ;
: plain-gesture? ( g -- flag )   \ (not one of the event_talk switch's own: 0, 2, 8, 10)
    dup 0= over raised-hand = or over at-rest-gesture = or swap seated-nod = or 0= ;
:noname ( a n -- )
    {: a g :}  a matthew <> a person? 0= and if  exit  then
    g 0 #gestures within 0= if  exit  then
    a sitting?  g first-seated past-seated within 0= and if  exit  then
    a anim-of resting? swap resting? or  g at-rest-gesture <> and if  exit  then
    g 0= if  exit  then
    a barbara-slot =  g plain-gesture? and if  barbaras-gesture  else  g  then  2 cells * gesture-motions + {: row :}
    a  row @  row cell+ @  false anim-then ; is gesture
: anim-at ( a id next -- )  false anim-then ;          \ cmd $13: once, then the next (src: actor_anim's next)
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
\ stepping through - a motion that holds its move; the door opening on the side shown)
$3F constant stepping-out-1   $40 constant stepping-out-0   \ (by the side's way)
-470 constant behind-look-y   -812 constant behind-dist   -1802 constant behind-eye-y   -693 constant behind-yaw
: my-room ( -- room )  area @ the-town = if  town-room  else  scene @  then ;
: object-anim ( entry -- )   \ op $156: the area's animation entry; in the town its slot (src: func_8001A190)
    area @ the-town = if  s" town" actor-named send town-door-swing exit  then
    s" areas" actor-named send play-area-anim ;
: door-opens ( door -- )   \ the door opening on the side shown as one walks through (src: func_8004535C's: its
    {: n :}  area @ the-town = if                                         \ room's side indoors, the town's slot)
        n town-room door-spot  dup 0< if  drop  else  s" town" actor-named send town-door-swing  then  exit  then
    n door-of door-area @ area @ <> if  exit  then
    n my-room door-spot  dup 0< if  drop  else  object-anim  then ;
: door-anim ( door -- )   \ op $127: indoors, the side shown's opening - else the far side's (src: cevent's door_anim)
    {: n :}  n door-of {: d :}  d door-area @ area @ <> if  exit  then
    d near-side side-room @ scene @ = if  d near-side side-spot @  else  d far-side side-spot @  then
    dup 0< if  drop  else  object-anim  then ;
16 constant no-reveal-scene   \ (src: not while scene 16 is shown)
: door-reveal-room ( door -- )   \ op $126: the room beyond shown through it (src: func_8001A044: its first object)
    door-of {: d :}  d door-area @ area @ <>  scene @ no-reveal-scene = or if  exit  then
    d near-side side-room @ scene @ = if  d far-side side-room @  else  d near-side side-room @  then
    dup 0< if  drop exit  then  s" areas" actor-named send reveal-room ;
: matthew-through ( door -- way | 0 )   \ (where he went; the door opens on his side)
    {: n :}  n door-of door-area @  area @ <>  area @ the-town <> and if  0 exit  then
    n door-opens
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
: matthew-door-walk ( door room flip -- )   \ src: func_8004535C for Matthew: the same, the door opening on his side
    {: n room flip :}  n room flip door-way {: w :}  w 0= if  exit  then
    n door-opens
    w out-x @ 0 w out-z @  matthew figure-of send figure-place
    0 w out-heading @ 0  matthew figure-of send figure-turn
    w out-way @ if  stepping-out-1  else  stepping-out-0  then  -1 false  matthew figure-of send figure-anim
    matthew figure-of send figure-hold  matthew wait-anim ;
: door-walk ( a door room flip -- )   \ src: func_8004535C: out at the door's far side, the thread waiting
    {: a n room flip :}                \ while they step through
    a matthew = if  n room flip matthew-door-walk exit  then
    a person? 0= if  a n room 3 s" through-door" unbuilt  exit  then
    n door-opens   \ (src: the door opens on the side shown, whoever walks through)
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

\ the area changes under a running scene - Matthew where he is, the scene going on (src: func_80018B44)
: scene-reset ( area -- )   \ op $11A
    {: ar :}  ar 0 s" areas" actor-named send enter-area  yield  ar event-area ! ;
: near? ( a x z dist -- flag )
    {: a x z d :}  a where-is 0= if  true exit  then  drop {: ax az :}
    ax x - s>f fdup f*  az z - s>f fdup f* f+ fsqrt  d s>f f<= ;
: wait-near ( a x z dist -- )   \ op $117: until they're that near the point
    settle  {: a x z d :}  begin  a x z d near? 0= while  yield  repeat ;

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

: heading-to ( x z x' z' -- heading )   \ from the first point toward the second (4096 a turn)
    rot - s>f  swap - s>f fswap fatan2 4096e f* 2e pi f* f/ f>s  4095 and ;
: face ( a target speed -- )   \ cmd $0E: the figure turns to face another (src: actor_turn_to_actor)
    {: a t v :}
    a figure-place@ 0= if  exit  then  drop nip {: ax az :}
    t figure-place@ 0= if  exit  then  drop nip {: tx tz :}
    a  ax az tx tz heading-to  v turn-figure ;
: camera-face ( a b -- )   \ op $11B: the camera's heading from one figure toward the other
    {: a b :}  a figure-place@ 0= if  exit  then  drop nip {: ax az :}
    b figure-place@ 0= if  exit  then  drop nip {: bx bz :}  ax az bx bz heading-to the-camera send camera-face ;
: face-each-other ( a b -- )  {: a b :}  a b 0 face  b a 0 face ;   \ op $162
: face-point ( a x z speed -- )   \ cmd $0D (src: actor_turn_to_point)
    {: a x z v :}  a figure-place@ 0= if  exit  then  drop nip {: ax az :}  a  ax az x z heading-to  v turn-figure ;
: run ( a speed -- )  run-motion go-forward ;   \ cmd $0B: straight on, running

\ ---- where a figure is, piecemeal (cmds $21 - $23, $38) ----
: set-coord ( a which v -- )   \ which: 0 x, 1 height, 2 z
    {: a w v :}  a matthew = if  w v a figure-of send figure-coord exit  then
    a person? if  a w v the-townsfolk send person-coord  then ;
: set-x ( a x -- )  0 swap set-coord ;   \ cmd $21
: set-y ( a y -- )  1 swap set-coord ;   \ cmd $22
: set-z ( a z -- )  2 swap set-coord ;   \ cmd $23
: move-by ( a dx dy dz -- )   \ cmd $38
    {: a dx dy dz :}  a matthew = if  dx dy dz a figure-of send figure-nudge exit  then
    a person? if  a dx dy dz the-townsfolk send person-nudge  then ;

\ ---- kept and given back, held still (cmds $4F, $16, $1F, $20) ----
: save-state ( a -- )  dup person? if  the-townsfolk send person-keep  else  drop  then ;     \ (Matthew's: as he is)
: restore-state ( a -- )  dup person? if  the-townsfolk send person-back  else  drop  then ;
: freeze ( a -- )  dup person? if  the-townsfolk send person-idle  else  drop  then ;   \ (src: frozen: stops where they are)
: unfreeze ( a -- )  drop ;
: door-release ( a -- )  dup person? if  the-townsfolk send person-door-free  else  drop  then ;   \ cmd $54
: reset ( a -- )   \ cmd $4D: off the stool, out of the car, off the paths, at no door, their thread over, standing
    {: a :}  a person? 0= if  exit  then
    a seated? if  a the-townsfolk send seat-leave  then
    a s" walkers" actor-named send walker-stop  a end-route  a the-townsfolk send person-unride
    a the-townsfolk send person-door-free  a the-townsfolk send person-idle ;
\ Matthew walks through everyone for now: whom he'd ignore doesn't matter (src: cmds $3C, $3D: his ignore_a / _b)
$FFFFFFFF constant none-held   \ (the scripts' -1, a 32-bit word)
: held-slot ( b -- b | -1 )  dup none-held and none-held = if  drop -1  then ;
: ignore-a ( a b -- )   \ op $133: a holds b in the right hand (-1: lets go; src: ignore_a - not in his way), a frame
    held-slot  2dup 1 the-townsfolk send figure-holds
    swap matthew = if  his-ignore-a !  else  drop  then  1 frames wait ;
: ignore-b ( a b -- )   \ op $134: in the left hand
    held-slot  2dup 0 the-townsfolk send figure-holds
    swap matthew = if  his-ignore-b !  else  drop  then  1 frames wait ;

\ ---- walking and riding (cmds $24, $37, $55, $59) ----
: lift ( a node dest speed -- )   \ cmd $24: along the town's footpaths (src: actor_lift: a walker)
    walk-motion s" walkers" actor-named send walker-start ;
: partner-command ( a route -- )   \ cmd $37: their car turns onto that route at its first node
    over person? if  swap partner-of ?dup if  swap s" traffic" actor-named send car-switch exit  then  then  2drop ;
: walk-path ( a x y z motion speed -- )   \ cmd $55: to the point, its height too (motion -1: walking)
    {: a x y z m v :}  a y set-y  a x z v  m 0< if  walk-motion  else  m  then  walk-with ;
: ending-schedule ( a route -- )   \ cmd $59: they leave on that route of the executable's (src: EM_ENDINGSCHEDULE)
    {: a r :}  a scheduled? 0= if  exit  then  a the-townsfolk send person-depart
    r $FFFFFFFF and main-word ?dup if  a start-route  then ;
: wait-attached ( a -- )   \ op $16E: until they ride
    settle  dup person? 0= if  drop exit  then  begin  yield  dup riding? until  drop ;
