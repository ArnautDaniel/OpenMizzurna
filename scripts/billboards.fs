\ billboards.fs - the billboards: trees, lamp posts, bins - upright pictures turned to face the camera's heading
\ (docs/subsystems/town.md, areas.md; src: billboard_draw, func_8003B41C, func_8003B2DC). A stand of them: rows of a
\ library text (each `billboard i at x z from bottom to top half-width w sprite s`), from an origin, their sprites a
\ list (`sprite i size w h full-bright f blend b`) and a folder of pictures; drawn in the first light's colour.
IN: billboards
USING: engine messages areas floors camera daylight ;

1 constant at-x   2 constant at-z   3 constant bottom   4 constant top   5 constant half-width   6 constant sprite
3 constant full-bright   4 constant blend-mode    \ (a sprite's fields: 0 index, 1 2 size, 3 full-bright, 4 blend)
40 constant dimmest   6 constant light-share   128 constant as-it-is
: nth ( row k -- n )  cells + @ ;
: billboard-grey ( -- grey )   \ src: D_800A3848: the first light's colour, a sixth of its sum, at least 40
    colors-now 0 light rgb@ + +  light-share /  dimmest max ;

begin-structure stand
  field: stand-rows   field: stand-first   field: stand-count   \ the rows (records), the first, how many
  field: stand-x   field: stand-z                               \ their origin
  field: stand-sprites   field: stand-folder   field: stand-folder-length   \ the sprites' list and pictures
end-structure
create the-stand  stand allot

<PRIVATE
: sprite-image ( i -- image )
    $clear  the-stand stand-folder @ the-stand stand-folder-length @ $+  s" /" $+  n>str $+  s" .png" $+  $@ image ;
: draw-row ( row grey -- )
    {: b grey :}  the-stand stand-sprites @ b sprite nth record {: sp :}
    b sprite nth sprite-image
    the-stand stand-x @ b at-x nth +  the-stand stand-z @ b at-z nth +  b bottom nth  b top nth  b half-width nth
    camera-yaw  sp full-bright nth if  as-it-is  else  grey  then  sp blend-mode nth  draw-billboard ;
PRIVATE>
\ the solid ones: a sprite under the area's number of solid kinds, within 128 of the point (src: billboard_hit)
128 constant solid-reach
: solid-kinds ( -- n )  s" billboard-solid-kinds" area @ exe-s16 ;
: row-blocks? ( x z row kinds -- flag )
    {: x z b k :}  b sprite nth k < 0= if  false exit  then
    x the-stand stand-x @ - b at-x nth -  dup *   z the-stand stand-z @ - b at-z nth -  dup *  +
    solid-reach dup * <= ;
: stand-blocks? ( x z -- flag )   \ one of the stand's solid billboards there
    the-stand stand-rows @ 0= if  2drop false exit  then  solid-kinds {: x z k :}
    the-stand stand-first @  dup the-stand stand-count @ +  swap ?do
        x z  the-stand stand-rows @ i record  k row-blocks? if  true unloop exit  then
    loop  false ;
: draw-stand ( -- )   \ the stand's billboards, this frame
    the-stand stand-rows @ 0= if  exit  then  billboard-grey {: grey :}
    the-stand stand-first @  dup the-stand stand-count @ +  swap ?do
        the-stand stand-rows @ i record grey draw-row
    loop ;

\ ---- an area's: the scene's set of them (cdbin/NNN_4/5.billboards.txt; sprites/NNN.txt, sprites/NNN/) ----
64 constant sets-head   12 constant entry-bytes   \ (a set's start: halfwords from the part's start)
1 constant set-count   2 constant set-at
<PRIVATE
: area-text ( -- addr len )  area @ s" 5.billboards.txt" area-path ;
: three-digits ( n -- )  dup 100 < if  s" 0" $+  then  dup 10 < if  s" 0" $+  then  n>str $+ ;
: area-sprites ( -- table )  $clear  s" sprites/" $+  area @ three-digits  s" .txt" $+  $@  s" sprite" records ;
create folder-name 16 allot
: area-folder ( -- addr len )   \ "sprites/NNN" (kept: the stand holds it)
    $clear  s" sprites/" $+  area @ three-digits  $@  {: a n :}  a folder-name n move  folder-name n ;
: draw-area-billboards ( -- )
    area @ 0<  area @ the-town = or if  exit  then
    area-text s" set" records {: sets :}  sets 0= if  exit  then
    sets scene @ record {: set :}  set set-count nth 0= if  exit  then
    area-text s" billboard" records the-stand stand-rows !
    set set-at nth 2* sets-head - entry-bytes / the-stand stand-first !  set set-count nth the-stand stand-count !
    area-origin nip the-stand stand-z ! the-stand stand-x !
    area-sprites the-stand stand-sprites !  area-folder the-stand stand-folder-length ! the-stand stand-folder !
    draw-stand ;
: area-stand ( -- set? )   \ the scene's set as the stand (its rows; false: none)
    area-text s" set" records {: sets :}  sets 0= if  false exit  then
    sets scene @ record {: set :}  set set-count nth 0= if  false exit  then
    area-text s" billboard" records the-stand stand-rows !
    set set-at nth 2* sets-head - entry-bytes / the-stand stand-first !  set set-count nth the-stand stand-count !
    area-origin nip the-stand stand-z ! the-stand stand-x !  true ;
' draw-area-billboards on-draw
PRIVATE>
: area-billboard? ( x z -- flag )  area-stand if  stand-blocks?  else  2drop false  then ;   \ (indoors)
