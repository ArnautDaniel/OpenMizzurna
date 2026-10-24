\ errors.fs - the errors actor (docs/subsystems/errors.md): the kernel tells it of every handler's error and every
\ task's (actor-failed, its record by number). It writes each to the errors' log (errors.log, beside the saves),
\ a repeat counted rather than written again, and asks the actor that failed to start over (start-over): one that
\ can put itself right handles it. A few times in a while; failing more it is left as it is, and the log says so.
IN: errors
USING: engine actors messages ;

3 constant start-overs-allowed   300 constant start-over-window   \ so many in so many frames (ten seconds)
300 constant repeats-written     \ a repeated error's count written each so many (as the console's)

<PRIVATE
\ ---- the log's lines: made a piece at a time (a chain is longer than the $ buffer) ----
1400 constant line-bytes
create line line-bytes allot   variable line-n
: line-clear ( -- )  0 line-n ! ;
: line+ ( addr len -- )  line-bytes line-n @ - min {: a n :}  a line line-n @ + n move  n line-n +! ;
: line-out ( -- )  line line-n @ errors-log ;

\ ---- repeats: the same who, message and error as the last ----
600 constant key-bytes   \ (who, message and error together)
create last-key key-bytes allot   variable last-key-n   variable repeats
create this-key key-bytes allot   variable this-key-n
: this-key+ ( addr len -- )  key-bytes this-key-n @ - min {: a n :}  a this-key this-key-n @ + n move  n this-key-n +! ;
: this-key! ( r -- )
    {: r :}  0 this-key-n !  r failure-who this-key+  s" |" this-key+  r failure-what this-key+  s" |" this-key+
    r failure-error this-key+ ;
: repeat? ( -- flag )  this-key this-key-n @ last-key last-key-n @ compare 0= ;
: remember-key ( -- )  this-key last-key this-key-n @ move  this-key-n @ last-key-n ! ;
: write-repeats ( -- )
    repeats @ 0= if  exit  then
    line-clear  s" (the error above: " line+  repeats @ n>str line+  s"  times more)" line+  line-out  0 repeats ! ;

\ ---- a failure's lines ----
: write-failure ( r -- )
    {: r :}  line-clear  s" frame " line+  r failure-frame n>str line+  s"   " line+  r failure-who line+
    r failure-what nip if  s"  (on " line+  r failure-what line+  s" )" line+  then
    s" : " line+  r failure-error line+  line-out
    r failure-chain nip if  line-clear  s"   in " line+  r failure-chain line+  line-out  then ;

\ ---- starting over: each actor's allowance ----
create window-start  actors-max cells allot   window-start actors-max cells erase
create start-overs  actors-max cells allot   start-overs actors-max cells erase
: allowed? ( id frame -- flag )   \ another start-over within its allowance (a new window when the last is past)
    {: id fr :}  id cells {: k :}
    fr window-start k + @ - start-over-window >= if  fr window-start k + !  0 start-overs k + !  then
    start-overs k + @ start-overs-allowed < dup if  1 start-overs k + +!  then ;
: write-left ( r -- )   \ (said once, when the allowance runs out)
    {: r :}  line-clear  r failure-who line+  s"  failed again: left as it is (" line+
    start-overs-allowed n>str line+  s"  start-overs in " line+  start-over-window n>str line+  s"  frames)" line+  line-out ;
: ask-start-over ( r -- )   \ the actor that failed asked to put itself right (a task's: nobody to ask)
    {: r :}  r failure-actor {: id :}
    id 0< id actors-max < 0= or if  exit  then  id alive? 0= if  exit  then
    id r failure-frame allowed? if  id send start-over  exit  then
    start-overs id cells + @ start-overs-allowed = if  r write-left  1 start-overs id cells + +!  then ;
PRIVATE>

: errors-start-overs ( id -- n )  cells start-overs + @ ;   \ (a fact: start-overs it was asked for in its window)

state: errors-state end-state
behaviour errors
  on actor-failed ( r -- )
    {: r :}  r this-key!
    repeat? if  1 repeats +!  repeats @ repeats-written = if  write-repeats  then
    else  write-repeats  remember-key  r write-failure  then
    r ask-start-over ;
end-behaviour

\ (told of failures from its spawning on: those before its first message too)
: spawn-errors ( -- id )  errors errors-state s" errors" spawn  dup errors-to ;
