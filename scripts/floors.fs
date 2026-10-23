\ floors.fs - an area's floors: where the walls are (obstacles: a move into one is undone) and its zones
\ (docs/subsystems/areas.md; src: area.c region_zone_hit, load_area's floor lists; tools/regions.py).
\ A region list: u16 header (bits 0-11 its count, 13 in use, 14 with ids, 15 off), then per region s16 x0 x1
\ z0 z1 (and u16 attributes with ids: the low byte its id, bit 8 inverted - the outside counts); z0 < z1 an
\ upright box, else a box turned 45 degrees. World units from the area's origin.
IN: floors
USING: engine messages areas ;

$0FFF constant count-bits   $2000 constant in-use   $4000 constant with-ids   $8000 constant switched-off
$0100 constant inverted
-1 constant no-region

\ which part of NNN_4 part 2 is a floor's obstacles (its zones the next): src load_area
: floor-part ( floor -- part | -1 )
    area @ case
        0 of  dup 0 = if  drop 2 exit  then  endof   24 of  dup 0 = if  drop 2 exit  then  endof
        30 of  dup 0 = if  drop 2 exit  then  endof
        3 of  dup 2 = if  drop 2 exit  then  endof   23 of  dup 2 = if  drop 2 exit  then  endof
        29 of  dup 2 = if  drop 2 exit  then  endof
        13 of  dup 2 = if  drop 2 exit  then  dup 3 = if  drop 4 exit  then  endof
        17 of  dup 2 = if  drop 2 exit  then  dup 3 = if  drop 4 exit  then  endof
        19 of  dup 2 = if  drop 2 exit  then  dup 3 = if  drop 4 exit  then  dup 0 = if  drop 6 exit  then  endof
        31 of  dup 2 = if  drop 2 exit  then  dup 3 = if  drop 4 exit  then  dup 0 = if  drop 6 exit  then  endof
        32 of  dup 2 = if  drop 2 exit  then  dup 3 = if  drop 4 exit  then  dup 0 = if  drop 6 exit  then  endof
    endcase
    1 = if  0  else  -1  then ;   \ (every area's floor 1: parts 0 and 1)

: region-file ( part -- addr n )   \ "cdbin/NNN_4/2/<part>.bin"
    $clear  s" cdbin/" $+  area @ dup 100 < if  s" 0" $+  then  dup 10 < if  s" 0" $+  then  n>str $+
    s" _4/2/" $+  n>str $+  s" .bin" $+  $@ library-file ;
: in-use? ( list -- flag )  w@ dup switched-off and 0=  swap in-use and 0<> and ;
: list-of ( floor zones? -- addr | 0 )   \ a floor's obstacle (or zone) list, if it has one in use
    {: fl zones? :}  fl floor-part  dup 0< if  drop 0 exit  then  zones? if  1+  then
    region-file  2 < if  drop 0 exit  then
    dup in-use? 0= if  drop 0  then ;
: entries ( list -- first count size )
    dup w@ >r 2 +  r@ count-bits and  r> with-ids and if  10  else  8  then ;

: inside? ( x z entry -- flag )   \ src: zone_inside (the list's origin: the area's)
    {: x z e :}
    area-origin nip {: ox oz :}
    e sw@ ox +  e 2 + sw@ ox +  e 4 + sw@ oz +  e 6 + sw@ oz +  {: x0 x1 z0 z1 :}
    e 4 + sw@ e 6 + sw@ < if
        x x0 >=  x x1 <= and  z z0 >= and  z z1 <= and
    else
        x z0 x0 - + z >=  z x z1 x1 - + >= and  z z1 x1 + x - >= and  z0 x0 + x - z >= and
    then ;
: region-at ( x z list -- id | -1 )   \ the first region holding the point: its id (lists without: 255)
    {: x z l :}  l 0= if  no-region exit  then
    l entries {: e n size :}
    n 0 ?do
        x z e i size * + inside?
        size 10 = if  e i size * + 8 + w@ inverted and if  0=  then  then
        if  size 10 = if  e i size * + 8 + c@  else  255  then  unloop exit  then
    loop  no-region ;
: blocked? ( x z floor -- flag )  false list-of region-at no-region <> ;   \ src: point_blocked (indoors)
: zone-at ( x z floor -- id | -1 )  true list-of region-at ;              \ src: player_zone (indoors)
