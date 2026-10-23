\ areas.fs - the area being played (docs/subsystems/areas.md; src: area.c load_area, model.c, draw.c
\ area_draw): its two model groups (the shell and the objects, NNN_4 part 3) at the area's origin, which of
\ their objects each scene shows, the scene's camera profile. The town (area 27) is the town subsystem's.
IN: areas
USING: engine actors messages ;

\ ---- facts: the area being played ----
variable area   -1 area !
variable scene
create origin 3 cells allot   \ x y z (world units)
: area-origin ( -- x y z )  origin @  origin cell+ @  origin 2 cells + @ ;

\ ---- an area's data: the metadata (NNN_4 part 0), read where it lies ----
\ its first words: s16 offsets of its tables (src: area_meta_set ...)
0 constant header-table   1 constant shown-table   2 constant camera-table   5 constant light-table
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
: draw-area ( -- )
    area @ 0< area @ the-town = or if  exit  then
    shell @ area-origin 0 shell-shown 256 draw-model-shown
    models @ area-origin 0 models-shown 256 draw-model-shown ;
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
    show-objects ;

state: areas-state end-state
behaviour areas
  on enter-area ( area scene -- )  2dup load  broadcast area-entered ;
  on show-scene ( scene -- )  dup scene !  show-objects  broadcast scene-shown ;
end-behaviour

: spawn-areas ( -- id )  areas areas-state s" areas" spawn ;
