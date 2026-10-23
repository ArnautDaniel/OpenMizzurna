\ area018.fs - area 18's rules: Ringwald's Gas Station (src: ovl018.c, the area's overlay; decomp/docs/areas/area18.md):
\ ○ at the counter talks to Robert when he's behind it, closing time sends Matthew out, Robert's and Steve's
\ lines by their stops. Its doors and the bulletin board are the tables' rows.
IN: rules.area018
USING: engine actors messages clock townsfolk events look talk areas ;

18 constant this-area
0 constant behind-his-counter   \ (Robert's record 0: the counter at (412465, 169071))

\ ---- ○: the look check (0x80101FB8) and its action (0x80102008) ----
enum: look-action  nothing-here  talk-to-robert ;
1 constant the-counter   \ (the zone)
: look-check ( zone check -- action )   \ src: func_80030A68
    drop  the-counter =  robert behind-his-counter at-record? and if  talk-to-robert  else  nothing-here  then ;
: look-do ( action -- )
    talk-to-robert = if  robert talk-with  else  run-rows  then ;
: act-check ( zone check -- action )  2drop 0 ;
: act-do ( action -- )  drop run-rows ;

\ ---- the area's hooks (0x80101F88 entering, 0x8010209C each frame) ----
variable closing-done
: entered ( -- )  18 map-reveal  0 closing-done ! ;
: scene-hook ( scene -- )  drop ;
0 constant the-shop   1 constant the-back
: each-frame ( -- )   \ 22:00 - 22:59 (not later: Robert's "open 24 hours"), once a visit: the attendant sends him out
    closing-done @  0 22 0 22 59 between? 0= or if  exit  then
    1 closing-done !
    scene @ case
        the-shop of  ['] events.ovl018:entry-801021E0 start-event  endof
        the-back of  ['] events.ovl018:entry-801021C4 start-event  endof
    endcase ;

\ ---- talking here (0x80101ED0): Robert's and Steve's lines by their stops ----
create robert-lines   \ src: D_8010221C
  0 , 0 ,  2 , 1 ,  3 , 2 ,  5 , 3 ,  6 , 4 ,  8 , 5 ,  10 , 6 ,  12 , 7 ,  14 , 8 ,  16 , 9 ,  18 , 10 ,  20 , 11 ,
  22 , 12 ,  -1 ,
create steve-lines  5 , 13 ,  12 , 14 ,  -1 ,   \ src: D_80102254
7 constant usual-gesture
: line-if ( table slot -- handled? )   \ a line by their stop, if they stand at one the table has
    {: t s :}  s stop-of 0< if  false exit  then
    t s person-of at-stop @ a-line-by-stop  dup 0< if  drop false exit  then  s usual-gesture one-liner  true ;
: talks-here ( slot -- handled? )
    case
        robert of  robert-lines robert line-if  endof
        steve of  steve-lines steve line-if  endof
        false swap
    endcase ;
' talks-here this-area area-talk!

' look-check ' look-do ' act-check ' act-do  this-area content area-rules!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!
