\ the library's files as the scripts read them (library-file, exe-table: bind_engine.c): each read once and kept,
\ as many as the game asks for - the town alone has 3072 (its tiles' obstacles and zones), the executable 170 tables
USING: tester engine ;

32 constant columns   24 constant rows
: two-digits ( n -- )  dup 10 < if  s" 0" $+  then  n>str $+ ;
: tile-file ( col row addr len -- addr n )   \ "town/CC-RR/<name>"
    {: c r a n :}  $clear  s" town/" $+  c two-digits  s" -" $+  r two-digits  s" /" $+  a n $+  $@ library-file ;
: every-tile ( -- )
    rows 0 do  columns 0 do  i j s" obstacles.bin" tile-file 2drop  i j s" zones.bin" tile-file 2drop  loop  loop ;

testing every town tile's two files read; the last still there, the first kept (the same bytes again)
T{ 0 0 s" obstacles.bin" tile-file drop  every-tile  0 0 s" obstacles.bin" tile-file drop = -> true }T
T{ 31 23 s" obstacles.bin" tile-file nip -> 2 }T
testing the executable's tables after all that: there, and read the same by both words
T{ s" screen-effects" exe-table nip -> 128 }T
T{ s" screen-effects" exe-table drop  s" exe/screen-effects.bin" library-file drop = -> true }T
testing a file or table the library hasn't: none, asked again none
T{ s" town/99-99/obstacles.bin" library-file  s" no-such-table" exe-table  s" no-such-table" exe-table -> 0 0 0 0 0 0 }T

test-summary
