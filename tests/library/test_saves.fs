\ saving and continuing on the game's data (scripts/saves.fs): a slot written as the original's file laid it
\ out and read back - the story, the time, the items, the map, the people and numbers known, the fuel, where
\ everyone stood; the save point's dialog (one card, always there) answered yes saves; continuing wakes him by
\ his bed in his room
USING: tester engine actors messages clock areas keys matthew townsfolk events story-flags items notebook driving
    saves ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: into ( area -- )  events-stop  0 s" areas" actor-named send enter-area  deliver  2 steps ;
13 constant badge   5 constant cemetery   20 constant fuel-left   2 constant saved-slot
prologue-done flag-set
26 14 30 at-time  1 into

testing a slot written: the file of the original's size, the slot marked used, the byte sum right
T{ badge s" items" actor-named send give-item  1 cemetery lshift map-known !
   fuel-left s" driving" actor-named send fuel-set  9 s" notebook" actor-named send learn-number  deliver
   saved-slot write-slot  saved-slot slot-used?  byte-sum sum-at image-at sl@ = -> true true true }T
testing all of it changed, then the slot continued: as it was saved, him in his room
T{ 28 9 0 at-time  s" items" actor-named send items-reset  0 map-known !  prologue-done flag-clear
   0 s" driving" actor-named send fuel-set  first-numbers first-bios swap s" notebook" actor-named send know  deliver
   saved-slot continue-game  deliver  2 steps
   clock-time time>date  badge has-item?  map-known @ 1 cemetery lshift and 0<>  fuel @  9 number-known?  prologue-done flag?
   area @ -> true 26 14 30 true true fuel-left true true 1 }T   \ (the map: his house's own revealed too, entering it)
T{ events-stop  deliver -> }T
testing a slot not saved can't be continued
T{ 7 slot-used?  7 continue-game -> false false }T

testing the save point's dialog: "checking", "save to slot 1?" - yes (○) - "saving": saved, the result 0
variable said
: save-scene ( -- )  0 memory-card  thread thread-result slot @ said ! ;
: answering ( -- )   \ ○ now and then until it's over
    600 0 do  events-running @ 0= if  leave  then  pad-circle true key-hold  2 steps  pad-circle false key-hold  6 steps  loop ;
T{ -1 said !  ' save-scene start-event  answering  said @  0 slot-used? -> 0 true }T

test-summary
