\ matthew.fs - Matthew (docs/subsystems/matthew.md; src: player.c): his body in the world and his figure
\ (models/073_1.gltf: kind 0's model, CD.BIN 073_1, drawn in the area's textures). Walking, looking and acting
\ come next; for now the events place and turn him.
IN: matthew
USING: engine actors messages areas figures ;


: figure-path ( -- addr len )  s" models/073_1.gltf" ;
variable figure

state: matthew-state end-state
create moving playing allot   -1 moving anim !

variable floor   1 floor !   \ (src: a loaded game resumes on area 1's floor 1)
: place ( x y z -- )   \ a height of 0 is "on his floor" (the scripts' set_pos; then the floor holds him)
    >r  dup 0= if  drop area @ floor @ floor-height  then  r>
    >r >r s>f r> s>f r> s>f  area @ floor @ body-place ;

\ at a held motion's end he stays where it moved him (src: func_8001CA5C: the last frame's root move, turned
\ by his heading, added to his place as it is)
: keep-move ( -- )
    moving clip @ dup motion-frames 1- motion-move  {: side up fwd :}
    self body-yaw 2e f* pi f* 4096e f/  {: F: h :}
    self body-pos  {: F: x F: y F: z :}
    x  fwd s>f h fsin f* f+  side s>f h fcos f* f+
    y  up s>f f-
    z  fwd s>f h fcos f* f+  side s>f h fsin f* f-
    self body-area self body-floor body-place ;

behaviour matthew
  on spawned ( -- )  figure-path model figure !  self subscribe tick ;
  on figure-place ( x y z -- )  place ;
  on tick ( -- )  moving advance if  moving hold-move @ if  keep-move  then  then ;
  on figure-anim ( id next loop -- )  moving play ;
  on figure-hold ( -- )  true moving hold-move ! ;
  on figure-turn ( rx ry rz -- )  drop nip s>f body-turn ;   \ (his heading: the turn about the vertical)
  on area-entered ( area scene -- )  drop  self body? if  body-area!  else  drop  then ;
end-behaviour

: matthew-actor ( -- id )  s" matthew" actor-named ;
: matthew-there? ( -- flag )  matthew-actor dup 0< if  drop false  else  body?  then ;
: matthew-now ( -- x y z yaw )   \ where he stands and his heading, in whole units (a fact: his body)
    matthew-actor {: id :}  id body-pos  f>s f>s f>s swap rot  id body-yaw f>s ;
: anim-done? ( -- flag )  moving done @  moving anim @ 0< or ;   \ (a fact: the events' wait-anim)
: draw-matthew ( -- )
    matthew-there? if
        figure @  moving anim @ 0< if  0 0  else  moving clip @ moving frame  then  matthew-now draw-figure
    then ;
' draw-matthew on-draw

: spawn-matthew ( -- id )  matthew matthew-state s" matthew" spawn  dup subscribe area-entered ;
