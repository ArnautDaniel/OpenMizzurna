\ the C the events call, written in Forth, on the game's data (story.fs, conversations.fs, phone.fs ...): the diary's
\ pages read and asked; the recap's chapter by the story and by the clock; the camera handed in (the photos an hour
\ on); the garage's call back three minutes on; Winona and Robert to the day's stops; the actor commands $50, $4C
USING: tester engine actors messages clock areas matthew townsfolk events story-flags items story
    conversations conversations.private rules.area016 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: ask ( address -- n )  main-exe swap find-native execute ;

testing the diary's 21st: read, then asked
T{ read-diary-dec21 flag-clear  $8004F31C ask  $8004F2C8 ask  $8004F31C ask -> 0 1 }T
testing the recap's chapter: by the story on the 25th's morning, the first; by the clock that afternoon, the third
T{ 25 10 0 at-time  by-the-story  25 16 0 at-time  by-the-time -> 0 3 }T
testing the camera handed in: taken, the photos due an hour on
T{ 26 14 30 at-time  pocket-camera s" items" actor-named send give-item  deliver  camera-handed-in  deliver
   pocket-camera has-item?  film-day story@  film-hour story@  film-minute story-c@ -> false 26 15 30 }T
testing the garage calls back three minutes on (call 9's time)
T{ 26 14 30 at-time  $8004791C ask  $1EC story@  $1F0 story@  $1F4 story-c@ -> 26 14 33 }T
testing Winona and Robert to the day's stops (the 27th: hers 22, his 8; src: evfn_80047B7C)
T{ 27 12 0 at-time  off-she-goes  deliver  winona last-stop  robert last-stop -> 22 8 }T
testing the actor commands: $50 into a car (riding it, out of sight); $4C walking to a node of the town's paths
T{ winona 30 1 into-their-car act-command  deliver  winona riding?  winona here? -> true false }T
T{ winona s" townsfolk" actor-named send person-unlink  deliver  27 0 s" areas" actor-named send enter-area  deliver
   winona 3 1 to-path-node act-command  deliver  winona walking? -> true }T
T{ events-stop -> }T

test-summary
