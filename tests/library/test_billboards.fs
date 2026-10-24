\ the areas' billboards on the game's data (docs/subsystems/areas.md)
USING: tester engine actors messages clock areas events billboards billboards.private town town.private ;

: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled

testing area 26's scene 0: 883 trees from its first entry; its sprites a pine first, 96 x 200
T{ 26 0 s" areas" actor-named send enter-area  deliver  area-text s" set" records 0 record
   dup set-count nth  swap set-at nth 2* sets-head - entry-bytes / -> 883 0 }T
T{ area-sprites #records  area-sprites 0 record 1 nth  area-sprites 0 record 2 nth -> 10 96 200 }T
T{ area-folder s" sprites/026" compare -> 0 }T

testing area 13's scenes 0, 1, 2 have their own sets
: counts ( -- a b c )  area-text s" set" records {: t :}  t 0 record set-count nth  t 1 record set-count nth  t 2 record set-count nth ;
T{ 13 0 s" areas" actor-named send enter-area  deliver  counts -> 51 63 63 }T

testing the town's lamp posts: drawn lit from 16:00 to 09:00 (sprite 6), else unlit (5); other sprites as they are
: at-time ( day hour minute -- )  date>time s" clock" actor-named send set-time  deliver ;
T{ the-town 0 s" areas" actor-named send enter-area  deliver
   25 12 0 at-time  lamp-post sprite-shown  25 16 0 at-time  lamp-post sprite-shown  26 8 59 at-time  lamp-post sprite-shown
   0 sprite-shown -> lamp-unlit lamp-lit lamp-lit 0 }T
testing elsewhere sprite 1 is itself
T{ 26 0 s" areas" actor-named send enter-area  deliver  lamp-post sprite-shown -> lamp-post }T

test-summary
