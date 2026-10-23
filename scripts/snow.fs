\ snow.fs - the snow falling (docs/subsystems/snow.md; src: snow.c, clock.c func_80025224): each scene says how
\ (the area's part 6: around the camera, from its windows' emitters, or none); while it snows the flakes grow to
\ 300, one a frame, and dwindle when it stops; the engine keeps them (snow-step, draw-snow).
IN: snow
USING: engine actors messages areas figures camera daylight ;

300 constant snowing-flakes   \ src: clock_tick's func_80025224(0, 300)
28 constant scene-snow        \ bytes a scene's record (src: 0x1C): s16 kind, count, then x z heading an emitter
<PRIVATE
: snow-record ( -- addr | 0 )   \ the scene's, from cdbin/NNN_4/6.bin
    area @ s" 6.bin" area-path library-file {: a n :}
    scene @ 1+ scene-snow * n > if  0 exit  then  a scene @ scene-snow * + ;
: emitter ( record i -- )   \ its place: from the area's origin
    {: r i :}  r 4 + i 6 * + {: e :}
    i  e sw@ area-origin drop nip +  e 2 + sw@ area-origin nip nip +  e 4 + sw@  snow-emitter ;
: scene-snow! ( -- )   \ src: func_8003AE04
    snow-record ?dup 0= if  -1 snow-scene exit  then  {: r :}
    r sw@ snow-scene
    r 2 + sw@ 0 ?do  r i emitter  loop ;
fvariable frames-due
: step ( -- )
    the-weather snowing = if  snowing-flakes  else  0  then {: target :}
    frames-due f@ game-frames f+ frames-due f!
    begin  frames-due f@ 1e f>=  while
        frames-due f@ 1e f- frames-due f!
        snow-live target < if  snow-live 1+ snow-flakes  else  snow-live target > if  snow-live 1- snow-flakes  then  then
        snow-step
    repeat ;
: draw-snow-here ( -- )  area @ 0< if  exit  then  s" sky/snow.png" image target@ draw-snow ;
' draw-snow-here on-draw
PRIVATE>

state: snow-state end-state
behaviour snowfall
  on spawned ( -- )  self subscribe tick  self subscribe area-entered  self subscribe scene-shown
    self subscribe time-set ;
  on tick ( -- )  area @ 0< 0= if  step  then ;
  on area-entered ( area scene -- )  2drop  scene-snow! ;
  on scene-shown ( scene -- )  drop  scene-snow! ;
  on time-set ( t -- )  drop  the-weather snowing = if  snowing-flakes  else  0  then snow-flakes ;
end-behaviour

: spawn-snow ( -- id )  snowfall snow-state s" snow" spawn ;
