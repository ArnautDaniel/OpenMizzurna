\ doors.fs - the doors between rooms and out to the town (docs/subsystems/doors.md; src: door.c). A door
\ (scripts/events/doors.fs) joins two rooms of an area, or a room and the town; going through it puts one at
\ its spot on the side one comes out at, a step from the door, facing away from it.
IN: doors
USING: engine messages ;

108 constant #doors
-2 constant town-room   -1 constant no-room

begin-structure door-side
  field: side-room      \ the room on this side (town-room: the town; no-room: none)
  field: side-spot      \ the door's spot in the side's area (its opening animation, its place)
  field: side-x   field: side-z   field: side-heading
  field: side-way       \ which way one comes out on this side (0 or 1)
end-structure
begin-structure door-row
  field: door-area
  door-side +field near-side   door-side +field far-side
end-structure
create doors-table  #doors door-row * allot
: door-of ( n -- row )  door-row * doors-table + ;

\ where going through door n into room `room` leaves one (src: func_8004535C, func_80018DA8, func_8001A1F0)
begin-structure way-out
  field: out-area   field: out-x   field: out-z   field: out-heading
  field: out-way        \ which way they came out (src: 0 the motion $40, 1 $3F)
end-structure
<PRIVATE
create the-way  way-out allot
\ (out of side 1: 125 back from the door and 300 to its right; out of side 0: turned round, 200 back and 360
\ to the left)
125 constant way-1-back   300 constant way-1-aside
200 constant way-0-back   -360 constant way-0-aside
2048 constant half-turn   1024 constant quarter-turn
: step ( x z distance heading -- x' z' )   \ distance along heading (4096 a turn)
    {: x z d h :}  h s>f 2e f* pi f* 4096e f/ {: F: a :}
    x  a fsin d s>f f* f>s +   z  a fcos d s>f f* f>s + ;
: side-for ( row room -- side | 0 )
    {: r room :}  r near-side side-room @ room = if  r near-side exit  then
    r far-side side-room @ room = if  r far-side exit  then  0 ;
: other ( row side -- side' )  over near-side = if  far-side  else  near-side  then ;
PRIVATE>
: door-way ( n room flip -- way | 0 )   \ flip: coming the other way (src: op $15F)
    {: n room flip :}  n door-of {: r :}  r room side-for {: s :}
    s 0= if  0 exit  then
    flip if  r s other  else  s  then  side-way @ {: w :}
    room town-room = if  the-town  else  r door-area @  then  the-way out-area !
    w the-way out-way !
    s side-heading @  w 0= if  half-turn +  then  4095 and {: h :}
    h the-way out-heading !
    s side-x @  s side-z @
    w if  way-1-back negate h step  way-1-aside  else  way-0-back negate h step  way-0-aside  then
    h quarter-turn + step  the-way out-z !  the-way out-x !
    the-way ;
