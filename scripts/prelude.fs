\ prelude.fs - the game's own words for every script, after ClankForth's prelude (forth_prelude: the language)
IN: forth

: three-digits ( n -- )  dup 100 < if  s" 0" $+  then  dup 10 < if  s" 0" $+  then  n>str $+ ;   \ ("005": the library's names)
\ `wait` ( frames -- ) and `yield` give way to the rest of the frame; `seconds` turns seconds
\ into frames (30 a second: the game's rate).
: seconds  30 * ;
