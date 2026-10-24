\ floors.fs - an area's floors: where the walls are (obstacles: a move into one is undone) and its zones
\ (docs/subsystems/areas.md; src: area.c region_zone_hit, load_area's floor lists; tools/regions.py).
\ A region list: u16 header (bits 0-11 its count, 13 in use, 14 with ids, 15 off), then per region s16 x0 x1
\ z0 z1 (and u16 attributes with ids: the low byte its id, bit 8 inverted - the outside counts); z0 < z1 an
\ upright box, else a box turned 45 degrees. World units from the area's origin - in the town, each tile's lists
\ (town/CC-RR/obstacles.bin, zones.bin) from the tile's centre, the tile the point is in (src: tile_lists).
IN: floors
USING: engine messages clock areas ;

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
: town-file ( col row zones? -- addr n )   \ "town/CC-RR/obstacles.bin" or "zones.bin"
    {: c r zones? :}  $clear  s" town/" $+  c 10 < if  s" 0" $+  then  c n>str $+  s" -" $+
    r 10 < if  s" 0" $+  then  r n>str $+  zones? if  s" /zones.bin"  else  s" /obstacles.bin"  then  $+  $@ library-file ;
\ the list last found, and the point its regions are measured from
variable list-x   variable list-z
: in-use? ( list -- flag )  w@ dup switched-off and 0=  swap in-use and 0<> and ;
: usable ( addr n -- list | 0 )  2 < if  drop 0 exit  then  dup in-use? 0= if  drop 0  then ;
: list-of ( floor zones? -- addr | 0 )   \ a floor's obstacle (or zone) list, if it has one in use
    {: fl zones? :}  area-origin nip list-z ! list-x !
    fl floor-part  dup 0< if  drop 0 exit  then  zones? if  1+  then
    region-file usable ;
-2 constant off-the-map   \ (a point off the town's tiles: blocked)
: tile-list ( x z zones? -- addr | 0 | off-the-map )   \ the town: the list of the tile the point is in
    {: x z zones? :}  x z tile-of {: c r :}  c r on-map? 0= if  off-the-map exit  then
    c r tile-centre list-z ! list-x !
    c r zones? town-file usable ;
: list-at ( x z floor zones? -- list | 0 | off-the-map )
    {: x z fl zones? :}  area @ the-town = if  x z zones? tile-list  else  fl zones? list-of  then ;
: entries ( list -- first count size )
    dup w@ >r 2 +  r@ count-bits and  r> with-ids and if  10  else  8  then ;

: inside? ( x z entry -- flag )   \ src: zone_inside (from the list's origin)
    {: x z e :}
    list-x @ list-z @ {: ox oz :}
    e sw@ ox +  e 2 + sw@ ox +  e 4 + sw@ oz +  e 6 + sw@ oz +  {: x0 x1 z0 z1 :}
    e 4 + sw@ e 6 + sw@ < if
        x x0 >=  x x1 <= and  z z0 >= and  z z1 <= and
    else
        x z0 x0 - + z >=  z x z1 x1 - + >= and  z z1 x1 + x - >= and  z0 x0 + x - z >= and
    then ;
: region-hit ( x z list -- index id | -1 -1 )   \ the first region holding the point: its index and id
    {: x z l :}  l 0= if  no-region no-region exit  then   \ (lists without ids: 255)
    l entries {: e n size :}
    n 0 ?do
        x z e i size * + inside?
        size 10 = if  e i size * + 8 + w@ inverted and if  0=  then  then
        if  i  size 10 = if  e i size * + 8 + c@  else  255  then  unloop exit  then
    loop  no-region no-region ;
: region-at ( x z list -- id | -1 )  region-hit nip ;
: blocked? ( x z floor -- flag )   \ src: point_blocked (on foot)
    {: x z fl :}  x z fl false list-at {: l :}
    l off-the-map = if  true exit  then  x z l region-at no-region <> ;
defer solid-billboard? ( x z -- flag )   \ a solid billboard there (src: billboard_blocked: billboards.fs, town.fs)
:noname ( x z -- flag )  2drop false ; is solid-billboard?
defer in-the-way? ( x z -- flag )   \ someone or a car standing there, in his way (src: player_probe_blocked: events.fs)
:noname ( x z -- flag )  2drop false ; is in-the-way?
\ the police cordon (src: foot_road_block, car_road_block): two road tiles (column + row x 100) closed but in their
\ zone 0 until the 28th 03:00
100 constant tile-row-key   922 constant cordon-a   1022 constant cordon-b
: tile-key ( x z -- key )  tile-of tile-row-key * + ;
: cordoned-tile? ( key -- flag )  dup cordon-a =  swap cordon-b = or ;
: cordon-holds? ( -- flag )  28 3 0 reached? 0= ;   \ src: before_dec28_0300
: zones-of ( x z floor -- list | 0 )  true list-at  dup off-the-map = if  drop 0  then ;
: zone-at ( x z floor -- id | -1 )  {: x z fl :}  x z  x z fl zones-of  region-at ;   \ src: player_zone
: zone-hit ( x z floor -- index id | -1 -1 )  {: x z fl :}  x z  x z fl zones-of  region-hit ;
: zone-centre ( index x z floor -- x z )   \ src: region_zone_centre (zone index of the list at x z), from its origin
    zones-of  dup 0= if  2drop 0 0 exit  then
    entries nip  rot * +  {: e :}
    e sw@ e 2 + sw@ + 2/  list-x @ +   e 4 + sw@ e 6 + sw@ + 2/  list-z @ + ;

\ ---- headings (4096 a turn) ----
4096 constant full-turn
: heading-within? ( heading angle range -- flag )   \ src: func_800290C0 (the range either side)
    {: h a r :}  a r - full-turn 1- and  a r + full-turn 1- and  {: lo hi :}  h full-turn 1- and {: hh :}
    lo hi < if  hh lo >=  hh hi <= and  else  hh hi <=  hh lo >= or  then ;
