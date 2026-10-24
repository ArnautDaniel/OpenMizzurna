\ areas.fs - the area being played (docs/subsystems/areas.md; src: area.c load_area, model.c, draw.c
\ area_draw): its two model groups (the shell and the objects, NNN_4 part 3) at the area's origin, which of
\ their objects each scene shows, the scene's camera profile. The town (area 27) is the town subsystem's.
IN: areas
USING: engine actors messages ;

\ the area whose tables, overlay and text an area uses (src: func_8003F6A8: 29 and 30 are 2's, 31 and 32 19's)
: content ( area -- area' )
    case  29 of  2  endof  30 of  2  endof  31 of  19  endof  32 of  19  endof  dup  endcase ;

\ ---- facts: the area being played ----
variable area   -1 area !
variable scene
create origin 3 cells allot   \ x y z (world units)
: area-origin ( -- x y z )  origin @  origin cell+ @  origin 2 cells + @ ;

\ ---- an area's data: the metadata (NNN_4 part 0), read where it lies ----
\ its first words: s16 offsets of its tables (src: area_meta_set ...)
0 constant header-table   1 constant shown-table   2 constant camera-table   3 constant anim-table
5 constant light-table
6 constant reverb-table
create path 32 allot
: area-path ( area addr len -- addr len )   \ "cdbin/NNN_4/<rest>"
    {: a s n :}  s" cdbin/" path swap move
    a 100 / [char] 0 + path 6 + c!  a 10 / 10 mod [char] 0 + path 7 + c!  a 10 mod [char] 0 + path 8 + c!
    s" _4/" path 9 + swap move  s path 12 + n move  path 12 n + ;
: meta ( -- addr )  area @ s" 0.bin" area-path library-file drop ;
: table ( n -- addr )  meta swap 2* + sw@ meta + ;
: shell-objects ( -- n )  header-table table 8 + c@ ;
: model-objects ( -- n )  header-table table 9 + c@ ;
: objects ( -- n )  shell-objects model-objects + ;
: shown? ( scene object -- flag )  swap objects * + shown-table table + c@ 0<> ;
: scene-camera ( scene -- eye-y look-y dist )   \ src: scene_camera_profile
    6 * camera-table table +  dup sw@  over 2 + sw@  rot 4 + sw@ ;
4 constant floors/area
: floor-height ( area floor -- y )   \ src: func_80020738 (the origin stands on floor 1; 0 with no table)
    swap floors/area * +  >r s" area-height" r> exe-s16 ;
: origin-height ( area -- y )  1 floor-height ;

\ ---- the town's tiles (docs/subsystems/town.md): 32 x 24 of 19980 world units ----
32 constant town-columns   24 constant town-rows
19980 constant tile-size   9990 constant half-tile
: tile-centre ( col row -- x z )   \ (src: func_8001D374: the rows one further than the sum says)
    swap tile-size * half-tile +  swap 1+ tile-size * half-tile + ;
: tile-of ( x z -- col row )  tile-size /  swap tile-size /  swap 1- ;   \ (src: an actor's tile_col, tile_row)
: on-map? ( col row -- flag )  town-rows u<  swap town-columns u<  and ;
\ the ground under a point in the town (town.fs fills it in: the tiles' terrain; none: false)
defer ground-at ( x z -- y true | false )
: no-ground ( x z -- false )  2drop false ;
' no-ground is ground-at

\ ---- the models, and which of their parts show ----
variable shell   variable models
create shell-shown 256 allot   create models-shown 256 allot
: model-path ( area part -- addr len )   \ "models/NNN_4.3.<part>.gltf"
    >r  s" models/" path swap move
    dup 100 / [char] 0 + path 7 + c!  dup 10 / 10 mod [char] 0 + path 8 + c!  10 mod [char] 0 + path 9 + c!
    s" _4.3." path 10 + swap move  r> [char] 0 + path 15 + c!  s" .gltf" path 16 + swap move  path 21 ;
: show-objects ( -- )   \ src: load_area's object_show / object_hide (the shell's by the first indices too)
    shell-shown 256 erase  models-shown 256 erase
    objects 0 ?do
        scene @ i shown? if  i shell-objects < if  1 shell-shown i + c!  then  1 models-shown i + c!  then
    loop ;
\ ---- the objects' animations (src: model.c func_8001DE24, func_80020198; the area's list: its table 3, six bytes
\ an entry {u16 object, u16 motion, u16 -}): a door swinging, a fan turning. A motion played once leaves its object
\ in its last frame (a door stays open); one held plays again from its start ----
6 constant anim-entry   32 constant #moving-objects
15e fconstant object-frames   \ its frames a second (the areas ran at 15)
begin-structure moving-object
  field: mo-object   field: mo-motion   field: mo-playing   field: mo-again   ffield: mo-frame   field: mo-next
end-structure
create moving-objects  #moving-objects moving-object * allot
: objects-still ( -- )  moving-objects #moving-objects moving-object * erase
    #moving-objects 0 do  -1 moving-objects i moving-object * + mo-object !  loop ;
objects-still
: area-anim ( entry -- object motion )   \ the area's list's entry
    anim-entry * anim-table table + {: e :}  e w@  e 2 + w@ ;
: prop-motion ( id -- motion )   \ the common props' motions (033_4 part 0)
    $clear  s" cdbin/033_4/0/" $+  n>str $+  s" .motion.txt" $+  $@ motion ;
: slot-of ( object -- addr )   \ its slot (a free one if it has none)
    {: o :}  #moving-objects 0 do  moving-objects i moving-object * +  dup mo-object @ o = if  unloop exit  then  drop  loop
    #moving-objects 0 do  moving-objects i moving-object * +  dup mo-object @ 0< if  unloop exit  then  drop  loop
    moving-objects ;   \ (all taken: the first)
: object-still? ( entry -- flag )   \ the list's entry's object never animated yet (src: func_8001A1C8)
    area-anim drop {: o :}
    #moving-objects 0 do  moving-objects i moving-object * + mo-object @ o = if  false unloop exit  then  loop  true ;
: play-object ( object motion-id again? -- )   \ src: func_80020198
    {: o id loops :}  o slot-of {: s :}  o s mo-object !  id prop-motion s mo-motion !
    loops s mo-again !  0e s mo-frame f!  true s mo-playing !  -1 s mo-next ! ;
: play-object-then ( object motion-id next -- )   \ src: func_800201EC: once, then the next held looping
    {: o id nx :}  o id false play-object  nx o slot-of mo-next ! ;
: step-object ( slot -- )
    {: s :}  s mo-playing @  s mo-motion @ 0<> and 0= if  exit  then
    s mo-frame f@ object-frames dt f* f+  s mo-frame f!
    s mo-frame f@ f>s  s mo-motion @ motion-frames < if  exit  then
    s mo-again @ if  0e s mo-frame f!  exit  then
    s mo-next @ 0< 0= if  s mo-next @ prop-motion s mo-motion !  -1 s mo-next !  true s mo-again !  0e s mo-frame f!  exit  then
    false s mo-playing !  s mo-motion @ motion-frames 1- s>f s mo-frame f! ;
: objects-step ( -- )   \ a step of the game's frames (src: a frame each)
    #moving-objects 0 do  moving-objects i moving-object * + step-object  loop ;
: pose-slot ( slot -- )   \ its object's pose for the coming draw
    {: s :}  s mo-object @ 0<  s mo-motion @ 0= or if  exit  then
    s mo-motion @ s mo-frame f@ f>s {: m f :}
    s mo-object @  m f motion-move  m f motion-turn  pose-object ;
: pose-objects ( -- )  #moving-objects 0 do  moving-objects i moving-object * + pose-slot  loop ;

: draw-area ( -- )
    area @ 0< area @ the-town = or if  exit  then
    shell @ area-origin 0 shell-shown 256 draw-model-shown
    pose-objects  models @ area-origin 0 models-shown 256 draw-model-shown ;
' draw-area on-draw

: load-town ( scene -- )   \ the town: its textures; its tiles the town's own (town.fs)
    scene !  the-town area !  the-town texture-area  0 origin !  0 origin cell+ !  0 origin 2 cells + ! ;
: load ( area scene -- )
    over the-town = if  nip load-town exit  then
    scene !  dup area !
    dup texture-area
    dup 0 model-path model shell !  dup 1 model-path model models !
    header-table table  dup sl@ origin !  4 + sl@ origin 2 cells + !
    origin-height origin cell+ !
    show-objects  objects-still ;

state: areas-state end-state
behaviour areas
  on spawned ( -- )  self subscribe tick ;
  on enter-area ( area scene -- )  2dup load  broadcast area-entered ;
  on show-scene ( scene -- )  dup scene !  show-objects  broadcast scene-shown ;
  on play-area-anim ( entry -- )   \ src: func_8001A150: the list's entry, played once
    area @ 0< area @ the-town = or if  drop exit  then  area-anim false play-object ;
  on tick ( -- )  objects-step ;
  on object-motion ( object id next -- )   \ one of the area's objects: a motion, then the next looping (-1: held)
    rot {: id nx o :}  nx 0< if  o id false play-object  else  o id nx play-object-then  then ;
  on reveal-room ( scene -- )   \ src: func_8001A044: the first of that scene's objects shown too (seen through a door)
    {: sc :}  objects 0 ?do  sc i shown? if  1 models-shown i + c!  i shell-objects < if  1 shell-shown i + c!  then  leave  then  loop ;
  on object-shown ( object flag -- )   \ src: object_show / object_hide (the area's hooks: day and night lights)
    {: o f :}  o 256 u< 0= if  exit  then  f 0<> 1 and {: b :}
    b models-shown o + c!  o shell-objects < if  b shell-shown o + c!  then ;
end-behaviour

: spawn-areas ( -- id )  areas areas-state s" areas" spawn ;
