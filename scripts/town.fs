\ town.fs - the town: the streamed outdoors, area 27 (docs/subsystems/town.md; src: map_stream.c,
\ ovl027's tile renderer). The town is 32 x 24 tiles (the library's town/CC-RR/: terrain.gltf, objects.gltf);
\ the tiles around the camera are loaded - a window of 6 x 6, moving with it - and those within two tiles of
\ the camera's drawn, the ground and the buildings and props on it. Its 38 street lamps light up in the evening.
IN: town
USING: engine actors messages clock areas floors camera daylight billboards ;

6 constant window      \ tiles a side kept loaded (src: map_stream_update)
2 constant reach       \ tiles drawn either side of the camera's (src: func_80107F80's 5 x 5)

: camera-tile ( -- col row )  target@ nip tile-of ;
town-columns constant columns   town-rows constant rows

<PRIVATE
create terrains  columns rows * cells allot   terrains columns rows * cells erase
create buildings columns rows * cells allot   buildings columns rows * cells erase
: tile ( col row table -- addr )  -rot columns * + cells + ;
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
\ ---- the tiles' objects that move (src: ovl027 func_801075AC, func_801078E8, func_801076C4) ----
\ 45 animation slots: a door's town side starts its slot - the slot's object of its tile plays the common props'
\ motion once through (the executable's `town-door-motions`); the tiles with slots (`town-animated-tiles`: column,
\ row, first slot, count). Three signs keep moving: two turning, one swinging.
45 constant #town-slots   23 constant #animated-tiles   30e fconstant town-frames-a-second
create slot-motion  #town-slots cells allot   create slot-frame  #town-slots floats allot
: slot-motion@ ( k -- motion | 0 )  cells slot-motion + @ ;
: still-slots ( -- )  #town-slots 0 do  0 i cells slot-motion + !  loop ;
still-slots
: slot-object ( k -- object )  {: k :}  s" town-door-motions" k 2* exe-u8 ;
: slot-motion-id ( k -- id )  {: k :}  s" town-door-motions" k 2* 1+ exe-u8 ;
: swing-slot ( k -- )   \ src: func_8001A190 / func_8002001C
    dup 0 #town-slots within 0= if  drop exit  then
    {: k :}  k slot-motion-id prop-motion  k cells slot-motion + !  0e k floats slot-frame + f! ;
: slot-step ( k -- )   \ (src: once through, then still)
    {: k :}  k slot-motion@ ?dup 0= if  exit  then  {: m :}
    k floats slot-frame + f@ town-frames-a-second dt f* f+  fdup k floats slot-frame + f!
    f>s m motion-frames >= if  0 k cells slot-motion + !  then ;
: slots-step ( -- )  #town-slots 0 do  i slot-step  loop ;
: tile-entry ( i -- col row first count )
    4 * {: e :}  s" town-animated-tiles" e exe-u8  s" town-animated-tiles" e 1+ exe-u8
    s" town-animated-tiles" e 2 + exe-u8  s" town-animated-tiles" e 3 + exe-u8 ;
: pose-town-slot ( k -- )
    {: k :}  k slot-motion@ ?dup 0= if  exit  then  {: m :}
    k floats slot-frame + f@ f>s m motion-frames 1- min {: f :}
    k slot-object  m f motion-move  m f motion-turn  pose-object ;
: pose-entry ( col row i -- )
    tile-entry {: c r ec er first n :}  ec c =  er r = and if  first n + first ?do  i pose-town-slot  loop  then ;
: pose-slots ( col row -- )  {: c r :}  #animated-tiles 0 do  c r i pose-entry  loop ;
\ the signs (object 2 of their tiles): (16, 12) and (20, 7) turning $1E a frame; (17, 13) swinging - its turns
\ about x and z each $14 a frame between -$50 and $50, its bob 4 up and 2 down between 0 and $14, hung at $EB0
2 constant sign-object   $1E constant sign-turn-step   $14 constant swing-step   $50 constant swing-reach
4 constant bob-up   2 constant bob-down   $14 constant bob-reach   $EB0 constant swing-height   1440 constant swing-rest
begin-structure swinger
  field: sw-now   field: sw-way
end-structure
create swing-x  swinger allot   create swing-z  swinger allot   create bob  swinger allot
variable sign-turn
: swing ( s step reach low -- )   \ to the reach, then back to the low
    {: s st hi lo :}  s sw-way @ 0= if  st s sw-now +!  s sw-now @ hi >= if  1 s sw-way !  then
    else  st negate s sw-now +!  s sw-now @ lo < if  0 s sw-way !  then  then ;
: signs-step ( -- )
    sign-turn-step sign-turn @ + 4095 and sign-turn !
    swing-x swing-step swing-reach 1+ swing-reach negate swing  swing-z swing-step swing-reach 1+ swing-reach negate swing
    bob sw-way @ if  bob bob-down bob-reach 1+ 0 swing  else  bob bob-up bob-reach 1+ 0 swing  then ;
: pose-signs ( col row -- )
    {: c r :}
    c 16 =  r 12 = and  c 20 =  r 7 = and or if  sign-object 0 0 0 0 sign-turn @ 0 pose-object  then
    c 17 =  r 13 = and if
        sign-object 0  swing-height swing-rest - bob sw-now @ -  0  swing-x sw-now @  0  swing-z sw-now @  pose-object  then ;
create all-shown  256 allot   all-shown 256 1 fill
: draw-tile ( col row -- )
    {: c r :}  c r tile-centre {: x z :}
    c r terrains tile @ ?dup if  x 0 z 0 draw-model  then
    c r buildings tile @ ?dup if  c r pose-slots  c r pose-signs  x 0 z 0 all-shown 256 draw-model-shown  then ;
\ the ground: the highest of the loaded tile's terrain polygons under the point (src: func_8001C264)
: town-ground ( x z -- y true | false )
    2dup tile-of 2dup on-map? 0= if  2drop 2drop false exit  then
    {: x z c r :}  c r terrains tile @ ?dup 0= if  false exit  then
    x z  c r tile-centre  ground-height ;
: ground-anywhere ( x z -- y true | false )   \ the town's; an outdoor area's shell's
    area @ the-town = if  town-ground exit  then
    ground-follows? if  shell-ground exit  then  2drop false ;
' ground-anywhere is ground-at

\ the billboards standing in a tile (the library's town/CC-RR/billboards.txt, town/sprites.txt; billboards.fs)
: billboards ( col row -- table )  s" billboards.txt" tile-path  s" billboard" records ;
: tile-origin ( col row -- x z )  s" billboards.txt" tile-path  s" origin" records  0 record {: o :}  o 0 nth  o 1 nth ;
: sprites ( -- table )  s" town/sprites.txt" s" sprite" records ;
: draw-billboards ( col row -- )
    2dup billboards {: c r t :}  t 0= if  exit  then
    t the-stand stand-rows !  0 the-stand stand-first !  t #records the-stand stand-count !
    c r tile-origin  the-stand stand-z !  the-stand stand-x !
    sprites the-stand stand-sprites !  s" town/sprites"  the-stand stand-folder-length !  the-stand stand-folder !
    draw-stand ;
: draw-tile-all ( col row -- )  2dup draw-tile  draw-billboards ;
: town-billboard? ( x z -- flag )   \ a solid billboard of the tile there (src: billboard_blocked on the overworld)
    2dup tile-of 2dup on-map? 0= if  2drop 2drop false exit  then
    2dup billboards {: x z c r t :}  t 0= if  false exit  then
    t the-stand stand-rows !  0 the-stand stand-first !  t #records the-stand stand-count !
    c r tile-origin  the-stand stand-z !  the-stand stand-x !  x z stand-blocks? ;
: any-billboard? ( x z -- flag )  area @ the-town = if  town-billboard?  else  area-billboard?  then ;
' any-billboard? is solid-billboard?
: draw-town ( -- )
    area @ the-town <> if  exit  then
    camera-tile {: cc cr :}
    reach 1+ reach negate do  reach 1+ reach negate do
        cc i +  cr j +  2dup on-map? if  draw-tile-all  else  2drop  then
    loop  loop ;
' draw-town on-draw
: leave-town ( -- )   \ every tile let go
    rows 0 do  columns 0 do  i j drop-tile  loop  loop  still-slots ;
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
  on tick ( -- )  area @ the-town = if  stream  lamps-step  slots-step  signs-step  then ;
  on town-door-swing ( slot -- )  swing-slot ;
  on area-entered ( area scene -- )  drop the-town <> if  leave-town  then ;
end-behaviour

: spawn-town ( -- id )  the-town-runner town-state s" town" spawn ;
