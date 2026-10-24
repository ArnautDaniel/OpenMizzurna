\ the characters' conversations on the game's data (scripts/conversations.fs; their packs and their lines by stop
\ are the executable's tables): a picker plays the first conversation whose time has come, once, from the
\ character's pack; then the next; with nothing to say, false (the areas' after them); their own line: the one for
\ the stop they left while walking, else their own
USING: tester engine actors messages clock areas keys events window story-flags townsfolk talk
    conversations conversations.private ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: started? ( -- flag )  events-running @ 0<> ;
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: fresh ( -- )  events-stop  story-clear  prologue-done flag-set ;
prologue-done flag-set

testing the packs: Morgan's, Barton's, Dennis's (through his character index)
T{ morgan pack-of  father-barton pack-of  dennis pack-of -> 233 235 238 }T
testing Barton: his first conversation once, then (before the 26th 8:00) his second, then nothing yet
T{ fresh  25 12 0 at-time  barton-talks  talking-pack @ talking-n @  $0F0 story@ -> true 235 0 1 }T
T{ events-stop  barton-talks  talking-n @ -> true 1 }T
T{ events-stop  barton-talks -> false }T
testing Morgan on the 25th's evening: the evening's first, then its second once its script's mark is there
T{ fresh  25 17 0 at-time  morgan-talks  talking-n @  $05C 12 story-bit flag? -> true 12 true }T
T{ events-stop  $05C 13 story-bit flag-set  morgan-talks  talking-n @ -> true 11 }T
testing Samuel: the analysis's talk first while it waits, without marking it
T{ fresh  $05C 1 story-bit flag-set  samuel-talks  talking-n @  samuel-talks  talking-n @ -> true 4 true 4 }T
testing Liz with nothing to say yet: false (the areas' next)
T{ fresh  liz-talks -> false }T
testing their own lines: standing, their own (Morgan's "Hmm? What?")
T{ fresh  morgan morgans-line  line @ -> hmm }T
testing walking: the line for the stop they left (Samuel's table: stop 1)
: samuel-walking ( -- )  samuel 1 true s" townsfolk" actor-named send person-set-stop  deliver ;
T{ samuel-walking  samuel samuels-line  line @ -> 10286 }T
T{ events-stop -> }T

test-summary
