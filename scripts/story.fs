\ story.fs - the story actor (docs/subsystems/story.md): starts a game and plays the events the story calls
\ for. The flags are facts (story-flags.fs): read by anyone, set by whoever's part of the story it is.
IN: story
USING: engine actors messages clock story-flags events events.all townsfolk fight ;

1 constant matthews-house   \ the area of the opening (src: game_start's ovl001 scripts)

\ ---- the prologue's end (src: evfn_80047620, called by the main executable's script $80096598) ----
25 10 0 date>time constant prologue-end-time   \ 12/25 10:00: Matthew at the station, the car beside him
: end-prologue ( -- )
    \ (everyone to their first stop, Matthew and the car placed, the diner's and the club's seats freed: the
    \ townsfolk, Matthew and the areas, as they are built)
    story-stopped release  event-running release
    prologue-end-time the-clock send set-time
    prologue-done flag-set
    broadcast prologue-over ;
main-exe $80047620 ' end-prologue native:

\ ---- the C the story's scenes call (src: cnrev.c, fhbook.c) ----
26 constant first-of-the-days   \ (the day stops' tables from 12/26)
: day-stop ( name len -- stop )  clock-time time>day first-of-the-days - exe-u8 ;
: to-the-days-stops ( -- )   \ src: evfn_80047B7C: Winona and Robert where the day has them
    winona s" winona-day-stops" day-stop  s" townsfolk" actor-named send person-goto-stop
    robert s" robert-day-stops" day-stop  s" townsfolk" actor-named send person-goto-stop ;
variable recap-thread   -1 recap-thread !
: recap-asked ( -- )  ['] events.main:exe-80098EBC launch-event recap-thread ! ;   \ src: evfn_80047BD8
232 constant recap-pack   \ (CD.BIN 046_2: the story so far)
$018 22 story-bit constant letter-read   $008 16 story-bit constant car-searched   $020 3 story-bit constant late-on
$010 1 story-bit constant recap-past-the-27th
: by-the-story ( -- n )   \ src: func_80047780: the chapter by where the story is
    25 14 0 reached? 0= if  0 exit  then
    26 9 0 reached? 0= if  2 exit  then
    26 16 0 reached? 0= if  4 exit  then
    27 13 30 reached? 0=  recap-past-the-27th flag? 0= and if  6 exit  then
    28 14 0 reached? 0=  recap-past-the-27th flag? 0= and  mel last-stop 20 < and if  7 exit  then
    letter-read flag? 0= if
        car-searched flag? 0= if  8 exit  then  recap-past-the-27th flag-set
    else  recap-past-the-27th flag-set  car-searched flag? 0= if  10 exit  then  then
    late-on flag? if  clock-time time>hour 1 and if  14  else  15  then  exit  then  13 ;
: by-the-time ( -- n )   \ (the other answer: the chapter by the clock)
    25 15 0 reached? 0= if  1 exit  then  25 23 0 reached? 0= if  3 exit  then
    26 8 0 reached? 0= if  5 exit  then  27 23 0 reached? 0= if  9 exit  then
    29 23 0 reached? 0= if  16 exit  then  12 ;
: recap ( -- )   \ src: func_80047C90: the recap's question answered 0, by the story; else by the clock
    recap-thread @ thread-result slot @ 0= if  by-the-story  else  by-the-time  then
    recap-pack swap call-pack-script ;
2 constant few-free-threads   4 constant script-threads
: follow-unless-busy ( -- )   \ src: evfn_80047C5C: the camera follows again with few event threads free
    script-threads events-running @ - few-free-threads < if  s" camera" actor-named send camera-follow  then ;
\ the diary's pages read (src: fhbook.c evfn_8004F2C8 ..: +0x08 bits 10 - 12)
: read-21 ( -- )  read-diary-dec21 flag-set ;   : read-22 ( -- )  read-diary-dec22 flag-set ;   : read-23 ( -- )  read-diary-dec23 flag-set ;
: read-21? ( -- n )  read-diary-dec21 flag? 1 and ;   : read-22? ( -- n )  read-diary-dec22 flag? 1 and ;
: read-23? ( -- n )  read-diary-dec23 flag? 1 and ;
main-exe $80037928 :noname  winner-human? 1 and ; native:   \ (the fight's: src fight_winner_is_human)
main-exe $80047B7C ' to-the-days-stops native:   main-exe $80047BD8 ' recap-asked native:
main-exe $80047C90 ' recap native:   main-exe $80047C5C ' follow-unless-busy native:
main-exe $8004F2C8 ' read-21 native:   main-exe $8004F2E4 ' read-22 native:   main-exe $8004F300 ' read-23 native:
main-exe $8004F31C ' read-21? native:   main-exe $8004F334 ' read-22? native:   main-exe $8004F34C ' read-23? native:

state: story-state end-state

behaviour story
  on new-game ( -- )   \ src: game_start(0)
    story-clear  events-reset
    new-game-time the-clock send set-time   \ 12/25 09:00, held until the prologue ends
    story-stopped hold
    event-running hold                      \ (src: event_lock)
    matthews-house event-area !
    matthews-house 0 s" areas" actor-named send enter-area   \ src: player_state_init, session_start_in(1, 0)
    ['] events.ovl001:new-game-opening start-event ;
end-behaviour

: spawn-story ( -- id )  story story-state s" story" spawn ;
