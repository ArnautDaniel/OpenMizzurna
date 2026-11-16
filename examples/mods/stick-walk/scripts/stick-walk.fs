\ stick-walk.fs - Matthew walks where the left stick points, the way it points on the screen: up away from the
\ camera, left to its left. The stick pushed past most of the way, or ✕ held, he runs. Without a stick the d-pad (the
\ arrows) points the same way. The camera's facing is taken as the stick leaves the middle and kept until it comes
\ back: the camera that follows him turns as he does, and pushing left would otherwise send him round in circles.
\ F1's options: Movement, Stick or Tank (the game's own controls), kept beside the saves.
IN: stick-walk
USING: engine keys mods matthew camera options ;

hook-before: walk-intent walk-before ( -- heading pace )   \ the game's way (or an earlier mod's)

\ ---- the setting: stick (this mod's way) or tank (the game's) ----
create setting  1 cells allot   setting 1 cells erase   \ 0: stick (the default), else tank
: setting-name ( -- addr len )  s" stick-walk" ;
: tank? ( -- flag )  setting @ 0<> ;
: movement-shown ( -- addr len )  tank? if  s" Tank"  else  s" Stick"  then ;
: movement-change ( dir -- )  drop  tank? 0= setting !  setting 1 cells setting-name save-file drop ;
setting 1 cells setting-name load-file drop
s" Movement" ' movement-shown ' movement-change add-option

\ ---- where the stick points ----
0.8e fconstant run-tilt   \ pushed this far (of 1, past the dead zone): he runs
: pad-pointing ( F: -- x y )   \ the d-pad as a stick (-1, 0, 1 each way)
    pad-right key-down? 1 and  pad-left key-down? 1 and -  s>f
    pad-down key-down? 1 and  pad-up key-down? 1 and -  s>f ;
: pointing ( F: -- x y )   \ the stick, or the d-pad if the stick is in the middle
    pad-stick  fover f0=  fover f0= and if  fdrop fdrop  pad-pointing  then ;
: tilt ( F: x y -- F: x y t )  fover fdup f*  fover fdup f* f+ fsqrt ;
: stick-run? ( -- flag )  pad-stick tilt {: F: x F: y F: t :}  t run-tilt f>= ;   \ (the d-pad runs only with ✕)

\ the camera's facing as the stick left the middle
variable held-yaw   variable holding
: camera-held ( -- yaw )  holding @ 0= if  camera-yaw held-yaw !  true holding !  then  held-yaw @ ;

: stick-walk ( -- heading pace )
    tank? if  walk-before exit  then
    pointing tilt {: F: x F: y F: t :}
    t f0= if  false holding !  0 standing-pace exit  then
    x 1000e f* f>s  y -1000e f* f>s  heading-toward  camera-held +  4095 and   \ (up the screen: the camera's way)
    stick-run?  pad-cross key-down? or if  running-pace  else  walking-pace  then ;
' stick-walk is walk-intent
