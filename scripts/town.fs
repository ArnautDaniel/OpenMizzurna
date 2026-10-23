\ town.fs - the town: the streamed outdoors, area 27 (docs/subsystems/town.md; src: map_stream.c,
\ ovl027's tile renderer). The town is 32 x 24 tiles (the library's town/CC-RR/: terrain.gltf, objects.gltf);
\ the tiles around the camera are loaded - a window of 6 x 6, moving with it - and those within two tiles of
\ the camera's drawn, the ground and the buildings and props on it. Its 38 street lamps light up in the evening.
IN: town
USING: engine actors messages clock areas camera daylight ;

32 constant columns   24 constant rows
19980 constant tile-size
9990 constant half-tile
6 constant window      \ tiles a side kept loaded (src: map_stream_update)
2 constant reach       \ tiles drawn either side of the camera's (src: func_80107F80's 5 x 5)

: tile-centre ( col row -- x z )   \ (src: func_8001D374: the rows one further than the camera's sum says)
    swap tile-size * half-tile +  swap 1+ tile-size * half-tile + ;
: camera-tile ( -- col row )  target@ nip  {: x z :}  x tile-size /  z tile-size / 1- ;

<PRIVATE
create terrains  columns rows * cells allot   terrains columns rows * cells erase
create buildings columns rows * cells allot   buildings columns rows * cells erase
: tile ( col row table -- addr )  -rot columns * + cells + ;
: on-map? ( col row -- flag )  rows u<  swap columns u<  and ;
: two-digits ( n -- )  dup 10 < if  s" 0" $+  then  n>str $+ ;
: tile-path ( col row addr len -- addr len )   \ "town/CC-RR/<file>"
    {: c r s n :}  $clear  s" town/" $+  c two-digits  s" -" $+  r two-digits  s" /" $+  s n $+  $@ ;
: load-tile ( col row -- )
    {: c r :}  c r terrains tile @ if  exit  then
    c r s" terrain.gltf" tile-path model  c r terrains tile !
    c r s" objects.gltf" tile-path model  c r buildings tile ! ;
: drop-tile ( col row -- )
    {: c r :}  c r terrains tile @ ?dup if  forget-model  0 c r terrains tile !  then
    c r buildings tile @ ?dup if  forget-model  0 c r buildings tile !  then ;
: window-corner ( -- col row )   \ the window's first tile: the camera's nearer half-tile picks the side
    target@ nip {: x z :}
    x tile-size / 3 -  x tile-size mod half-tile > if  1+  then
    z tile-size / 4 -  z tile-size mod half-tile > if  1+  then ;
: in-window? ( col row c0 r0 -- flag )  {: c r c0 r0 :}  c c0 - window u<  r r0 - window u<  and ;
: stream ( -- )   \ the window's tiles in, the rest out (src: map_stream_update - at once, not a tile a frame)
    window-corner {: c0 r0 :}
    rows 0 do  columns 0 do
        i j c0 r0 in-window?  i j on-map? and if  i j load-tile  else  i j drop-tile  then
    loop  loop ;
: draw-tile ( col row -- )
    {: c r :}  c r tile-centre {: x z :}
    c r terrains tile @ ?dup if  x 0 z 0 draw-model  then
    c r buildings tile @ ?dup if  x 0 z 0 draw-model  then ;
\ the billboards: the trees, lamps, bins standing in a tile, upright pictures turned to the camera's heading
\ (src: func_8003B41C, billboard_draw; the library's town/CC-RR/billboards.txt, town/sprites.txt)
1 constant at-x   2 constant at-z   3 constant bottom   4 constant top   5 constant half-width   6 constant sprite
3 constant full-bright   4 constant blend-mode    \ (a sprite's fields: 0 index, 1 2 size, 3 full-bright, 4 blend)
40 constant dimmest   6 constant light-share   128 constant as-it-is
: nth ( row k -- n )  cells + @ ;
: billboards ( col row -- table )  s" billboards.txt" tile-path  s" billboard" records ;
: tile-origin ( col row -- x z )  s" billboards.txt" tile-path  s" origin" records  0 record {: o :}  o 0 nth  o 1 nth ;
: sprites ( -- table )  s" town/sprites.txt" s" sprite" records ;
: sprite-image ( i -- image )  $clear  s" town/sprites/" $+  n>str $+  s" .png" $+  $@ image ;
: billboard-grey ( -- grey )   \ src: D_800A3848: the first light's colour, a sixth of its sum, at least 40
    colors-now 0 light rgb@ + +  light-share /  dimmest max ;
: draw-billboard-row ( row ox oz grey -- )
    {: b ox oz grey :}  sprites b sprite nth record {: sp :}
    b sprite nth sprite-image
    ox b at-x nth +  oz b at-z nth +  b bottom nth  b top nth  b half-width nth
    camera-yaw  sp full-bright nth if  as-it-is  else  grey  then  sp blend-mode nth  draw-billboard ;
: draw-billboards ( col row -- )
    2dup billboards {: c r t :}  t 0= if  exit  then
    c r tile-origin billboard-grey {: ox oz grey :}
    t #records 0 ?do  t i record ox oz grey draw-billboard-row  loop ;
: draw-tile-all ( col row -- )  2dup draw-tile  draw-billboards ;
: draw-town ( -- )
    area @ the-town <> if  exit  then
    camera-tile {: cc cr :}
    reach 1+ reach negate do  reach 1+ reach negate do
        cc i +  cr j +  2dup on-map? if  draw-tile-all  else  2drop  then
    loop  loop ;
' draw-town on-draw
: leave-town ( -- )   \ every tile let go
    rows 0 do  columns 0 do  i j drop-tile  loop  loop ;
PRIVATE>

: tiles-loaded ( -- n )  0  columns rows * 0 do  terrains i cells + @ 0<> -  loop ;

\ ---- the street lamps (src: func_800243F0): on in the evening, off late at night, a few at a time ----
16 60 * 1+ constant lamps-start     \ 16:01: they come on, one here, one there
17 60 * 1+ constant lamps-on        \ 17:01: all of them
23 60 * 1+ constant lamps-out       \ 23:01: they go off, one here, one there (and stay off till evening)
8 constant lamp-chance              \ (a lamp switches 7 frames in 8, while they come on or go off)
create lamps-lit  #lamps allot   lamps-lit #lamps erase
: lamp-lit? ( i -- flag )  lamps-lit + c@ 0<> ;
<PRIVATE
: switch-lamp ( i lit -- )  2dup 0<> 1 and swap lamps-lit + c!  lamp ;
: lamps-wanted ( -- lit sure? )   \ what they should be now, and whether all at once
    clock-time minutes/day mod {: m :}
    m lamps-out >= if  false false exit  then
    m lamps-on >= if  true true exit  then
    m lamps-start >= if  true false exit  then
    false true ;
: lamps-step ( -- )
    lamps-wanted {: lit sure :}
    #lamps 0 do
        i lamp-lit? lit <>  sure  lamp-chance random 0<> or  and if  i lit switch-lamp  then
    loop ;
PRIVATE>

state: town-state end-state
behaviour the-town-runner
  on spawned ( -- )  self subscribe area-entered  self subscribe tick ;
  on tick ( -- )  area @ the-town = if  stream  lamps-step  then ;
  on area-entered ( area scene -- )  drop the-town <> if  leave-town  then ;
end-behaviour

: spawn-town ( -- id )  the-town-runner town-state s" town" spawn ;
