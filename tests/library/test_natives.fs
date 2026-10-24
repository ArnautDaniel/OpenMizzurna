\ the C the events call, written in Forth, on the game's data (story.fs, conversations.fs, phone.fs ...): the diary's
\ pages read and asked; the recap's chapter by the story and by the clock; the camera handed in (the photos an hour
\ on); the garage's call back three minutes on
USING: tester engine actors messages clock areas matthew townsfolk events story-flags items story
    conversations conversations.private ;

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

test-summary
