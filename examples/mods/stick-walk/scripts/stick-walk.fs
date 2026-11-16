\ stick-walk.fs - a third-person camera and walking, in place of the tank controls: the camera keeps its own facing
\ (the game's turns with Matthew) and follows where he goes; the right stick, or L1 / R1, turns it round him. The
\ left stick walks him the way it points on the screen - up away from the camera, left to its left - pushed most of
\ the way, or ✕ held, he runs. Without a stick the d-pad (the arrows) points the same way. Driving, the game's
\ camera. F1's options: Movement, Stick or Tank (the game's own controls and camera), kept beside the saves.
IN: stick-walk
USING: engine keys mods matthew camera driving options ;

hook-before: walk-intent walk-before ( -- heading pace )      \ the game's way (or an earlier mod's)
hook-before: follow-yaw camera-before ( yaw fresh? -- yaw' )

\ ---- the setting: stick (this mod's way) or tank (the game's) ----
create setting  1 cells allot   setting 1 cells erase   \ 0: stick (the default), else tank
: setting-name ( -- addr len )  s" stick-walk" ;
: tank? ( -- flag )  setting @ 0<> ;
: movement-shown ( -- addr len )  tank? if  s" Tank"  else  s" Stick"  then ;
: movement-change ( dir -- )  drop  tank? 0= setting !  setting 1 cells setting-name save-file drop ;
setting 1 cells setting-name load-file drop
s" Movement" ' movement-shown ' movement-change add-option
: ours? ( -- flag )  tank? 0=  driving? 0= and ;

\ ---- the camera: its own facing, turned by the right stick or L1 / R1 ----
4096 constant whole-turn
48e fconstant look-turn     \ a step at full tilt (4096 a turn: round in under three seconds at 30 steps a second)
variable facing
: look-turning ( F: -- d )   \ this step's turn: the right stick, or the shoulders
    pad-look fdrop  look-turn f*
    pad-r1 key-down? if  look-turn f+  then  pad-l1 key-down? if  look-turn f-  then ;
: own-facing ( yaw fresh? -- yaw' )
    ours? 0= if  camera-before  dup facing !  exit  then
    if  facing !  else  drop  then   \ (a reset: behind him again)
    facing @ look-turning f>s +  whole-turn 1- and  dup facing ! ;
' own-facing is follow-yaw

\ ---- walking: where the left stick points, from the camera's facing ----
0.8e fconstant run-tilt   \ pushed this far (of 1, past the dead zone): he runs
: pad-pointing ( F: -- x y )   \ the d-pad as a stick (-1, 0, 1 each way)
    pad-right key-down? 1 and  pad-left key-down? 1 and -  s>f
    pad-down key-down? 1 and  pad-up key-down? 1 and -  s>f ;
: pointing ( F: -- x y )   \ the stick, or the d-pad if the stick is in the middle
    pad-stick  fover f0=  fover f0= and if  fdrop fdrop  pad-pointing  then ;
: tilt ( F: x y -- F: x y t )  fover fdup f*  fover fdup f* f+ fsqrt ;
: stick-run? ( -- flag )  pad-stick tilt {: F: x F: y F: t :}  t run-tilt f>= ;   \ (the d-pad runs only with ✕)
: stick-walk ( -- heading pace )
    tank? if  walk-before exit  then
    pointing tilt {: F: x F: y F: t :}
    t f0= if  0 standing-pace exit  then
    x 1000e f* f>s  y -1000e f* f>s  heading-toward  camera-yaw +  whole-turn 1- and   \ (up the screen: the camera's way)
    stick-run?  pad-cross key-down? or if  running-pace  else  walking-pace  then ;
' stick-walk is walk-intent
