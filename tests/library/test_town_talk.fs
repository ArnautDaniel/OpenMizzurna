\ the townspeople's lines on the game's data (conversations.fs, rules/area027.fs; the tables are the executable's
\ and the town's code's): a passer-by walking says the line for their kind and the stop they left (some kinds
\ their own way), standing the passer-by's; in the town each its own by its stop (pairs, by-stop tables, by
\ threes, odd or even); the farm workers by the first one's stop; the road block's officer a script a day
USING: tester engine actors messages clock areas keys events window story-flags townsfolk talk
    conversations conversations.private rules.area027 ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
: fresh ( -- )  events-stop  story-clear  prologue-done flag-set ;
: put-at-stop ( slot stop walking? -- )  s" townsfolk" actor-named send person-set-stop  deliver ;
: said ( -- msg speaker gesture )  line @  speaker @  line-gesture @ ;
prologue-done flag-set

testing a passer-by of kind 3 walking from stop 1: their kind's line; standing, the passer-by's
T{ fresh  $44 1 true put-at-stop  $44 townsperson-line  said -> 10311 $44 usual-gesture }T
T{ fresh  $44 1 false put-at-stop  $44 townsperson-line  said -> passer-by $44 usual-gesture }T
testing kind 21 by odd and even stops; kind 15's said by slot $50; kind 16 after a round of 11
T{ fresh  $56 3 true put-at-stop  $56 townsperson-line  said -> kind-21-odd $56 small-gesture }T
T{ fresh  $50 0 true put-at-stop  $50 townsperson-line  said -> 10449 kind-15-speaker still-gesture }T
T{ fresh  $51 12 true put-at-stop  $51 townsperson-line  said -> kind-16-after $51 1 }T
testing kind 4's by-stop table: stop 0's line, stop 2 none (the passer-by's)
T{ fresh  $45 0 true put-at-stop  $45 townsperson-line  line @ -> 10313 }T
T{ fresh  $45 2 true put-at-stop  $45 townsperson-line  line @ -> passer-by }T

testing the town: person 65 standing at its table's first stop says its line
: first-pair ( -- stop msg )  s" town-65-standing" 0 exe-s16  s" town-65-standing" 1 exe-s16 ;
T{ fresh  $41 first-pair drop false put-at-stop  $41 town-line  line @ -> true first-pair nip }T
testing walking by its by-stop table (person 74 from stop 1)
T{ fresh  $4A 1 true put-at-stop  $4A town-line  line @ -> true s" town-74-walking" 1 exe-s16 }T
testing first, odd, even (person 75) and by threes (person 67: stop 4 the second, with its gesture)
T{ fresh  $4B 0 true put-at-stop  $4B town-line  line @ -> true $143 }T
T{ fresh  $4B 3 true put-at-stop  $4B town-line  line @ -> true $144 }T
T{ fresh  $43 4 true put-at-stop  $43 town-line  said -> true $13C $43 shrug }T
testing standing, nothing for them in the town: their own (false)
T{ fresh  $4A 1 false put-at-stop  $4A town-line -> false }T

testing the road block's officer: on the 26th his day's script, then its repeat; on the 28th nothing
T{ fresh  26 12 0 at-time  road-block-talk  talking-n @  road-block-talk  talking-n @ -> true 7 true 8 }T
T{ fresh  28 12 0 at-time  events-stop  road-block-talk  events-running @ -> true 0 }T

testing the farm workers walking: the line for the first one's stop
T{ fresh  first-worker 5 true put-at-stop  first-worker workers-line  line @ -> true s" town-workers-walking" 5 exe-s16 }T
testing their own line (anywhere): walking from a stop with a line, the one of the first five by it mod 5
: expected-own ( stop -- msg )
    {: st :}  s" lines-workers" st exe-s16 if  s" lines-workers" st 5 mod exe-s16  else  passer-by  then ;
T{ fresh  first-worker 7 true put-at-stop  first-worker workers-own-line  line @ -> 7 expected-own }T
T{ events-stop -> }T

test-summary
