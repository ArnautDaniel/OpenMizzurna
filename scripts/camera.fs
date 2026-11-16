\ camera.fs - the camera (docs/subsystems/camera.md; src: camera.c camera_follow / camera_reset /
\ camera_apply_view, the events' camera ops). Following, it stands where Matthew stood and faces as he faced
\ a moment ago (two of the original's frames: a chase camera behind his back), at the scene's heights and
\ distance; an event puts it where it wants (fixed). Its state is its own (variables: the only camera), drawn
\ each frame through the engine's `camera`.
IN: camera
USING: engine actors messages areas matthew window ui mods ;

500 constant indoor-projection   400 constant town-projection   \ src: GsSetProjection(g_on_overworld ? 400 : 500)
\ the town's profile (src: camera_reset / camera_follow on the overworld)
-700 constant town-eye-y   -600 constant town-look-y   -2850 constant town-dist   -900 constant driving-eye-y
\ the follow's lag: two of the original's frames (15 a second indoors, 30 in the town) - in steps (30 a second)
2e fconstant lag-frames   \ (src: camera_follow's two-frame ring, D_800A4380)

variable following   variable reset-due   variable driving   \ (following the car)
create target 3 cells allot   variable yaw
variable dist   variable eye-y   variable look-y   variable projection
: target! ( x y z -- )  target 2 cells + !  target cell+ !  target ! ;
: target@ ( -- x y z )  target @  target cell+ @  target 2 cells + @ ;

\ Matthew's last steps: where he was and how he faced (a ring)
16 constant #history
create history #history 4 * cells allot   variable newest
: entry ( k -- addr )  newest @ swap - #history + #history mod 4 * cells history + ;   \ k steps ago
: remember ( x y z yaw -- )
    newest @ 1+ #history mod newest !  0 entry  tuck 3 cells + !  tuck 2 cells + !  tuck cell+ !  ! ;
: lag ( -- n )  lag-frames game-rate f/ seconds ;   \ steps behind him: two game frames, at the game's rate

\ the scene's profile: its heights and distance (the town's own), which the following camera takes every step
\ (src: camera_follow: g_camera_profile each frame - an event's shot or move lasts only while it isn't following)
variable profile-eye-y   variable profile-look-y   variable profile-dist
: profile ( -- )
    area @ the-town = if  driving @ if  driving-eye-y  else  town-eye-y  then  town-look-y town-dist  town-projection
    else  scene @ scene-camera  indoor-projection  then
    projection !  profile-dist !  profile-look-y !  profile-eye-y ! ;
: as-profiled ( -- )  profile-eye-y @ eye-y !  profile-look-y @ look-y !  profile-dist @ dist ! ;
variable turned-round   \ he faces backwards (the boat: src camera_follow's boat_mode)
4096 constant full-turn   2048 constant half-turn
: followed ( -- x y z yaw )  matthew-now  turned-round @ if  half-turn + full-turn 1- and  then ;
: chase-yaw ( yaw fresh? -- yaw' )  drop ;   \ the game's: as he faced
' chase-yaw hook: follow-yaw ( yaw fresh? -- yaw' )   \ which way the following camera faces, given the way he faced
                                                     \ (fresh: behind him anew - a reset); a mod may keep its own
: camera-reset ( -- )   \ src: camera_reset: behind Matthew now, the history filled with where he is
    profile as-profiled  matthew-there? 0= if  exit  then
    #history 0 do  followed remember  loop
    followed true follow-yaw yaw !  target! ;
: follow ( -- )   \ src: camera_follow
    matthew-there? 0= if  exit  then
    followed remember
    lag entry {: e :}  e @  e cell+ @  e 2 cells + @  target!  e 3 cells + @ false follow-yaw yaw !  as-profiled ;

\ behind him, low, turning as he turns (src: ovl019's duct crawl: its own heights and distance)
variable low-behind
: keep-behind ( -- )  matthew-there? 0= if  exit  then  matthew-now yaw !  target! ;

: draw-camera ( -- )  target@ yaw @ dist @ eye-y @ look-y @ projection @ camera ;
' draw-camera on-draw

\ ---- the events' moves (src: camera_tween_update, nav_step), a step each of the original's frames - 15 a
\ second while an event runs ----
fvariable due   \ (a move steps a game frame at a time: src: camera_tween_update, from event_threads_update)
variable moving   create goal 3 cells allot   variable move-speed
variable turning   variable turn-goal   variable turn-step
variable lifting   variable height-goal   variable height-step
variable pulling   variable dist-goal   variable dist-step
: camera-yaw ( -- yaw )  yaw @ ;
: camera-moving? ( -- flag )  moving @ ;              \ (facts: the events' waits)
: camera-turning? ( -- flag )  turning @ ;
: camera-height-moving? ( -- flag )  lifting @ ;
: camera-dist-moving? ( -- flag )  pulling @ ;
: goal@ ( -- x y z )  goal @  goal cell+ @  goal 2 cells + @ ;
: move-step ( -- )   \ toward the goal by the speed (across; up and down by the pitch to it)
    goal@ target@ {: gx gy gz x y z :}  gx x - gy y - gz z - {: dx dy dz :}  move-speed @ {: v :}
    dx abs v <  dz abs v < and  dy abs v < and if  goal@ target!  false moving !  exit  then
    dx s>f dz s>f fatan2 {: F: dir :}
    x  dir fsin v s>f f* f>s +   z  dir fcos v s>f f* f>s +   {: nx nz :}
    dy abs v >= if  y  dy s>f dx s>f fatan2 fsin v s>f f* f>s +  else  y  then   \ (src: the pitch against x alone)
    nx swap nz target! ;
: passed? ( now before -- flag )   \ the goal lies between where the heading was a step back and where it is
    {: now before :}  turn-goal @ {: g :}
    turn-step @ 0< if  g now >=  g before < and  else  g before >=  g now < and  then ;
\ src: camera_tween_update: the heading steps the step's way (a negative step turns it down), not the shorter
\ one; past the goal, on it (a goal it starts on: once round)
: turn-toward ( -- )
    turn-step @ 0= if  turn-goal @ yaw !  false turning !  exit  then
    yaw @ full-turn 1- and {: now :}  now turn-step @ - {: before :}
    turn-step @ 0<  turn-goal @ half-turn < and  before full-turn >= and if  full-turn turn-goal +!  then
    now before passed? if  turn-goal @ yaw !  false turning !  else  now turn-step @ + yaw !  then ;
: tween ( v goal step -- v' done? )   \ toward goal by step (past it: on it)
    {: v g st :}  v g < if  v st + dup g >=  else  v st - dup g <=  then  if  drop g true  else  false  then ;
: tween-frame ( -- )
    moving @ if  move-step  then
    turning @ if  turn-toward  then
    lifting @ if  eye-y @ height-goal @ height-step @ tween if  false lifting !  then  eye-y !  then
    pulling @ if  dist @ dist-goal @ dist-step @ tween if  false pulling !  then  dist !  then ;
: tweens ( -- )
    moving @ turning @ or lifting @ or pulling @ or 0= if  0e due f!  exit  then
    due f@  game-frames f+  begin  fdup 1e f>= while  tween-frame  1e f-  repeat  due f! ;

state: camera-state end-state
behaviour follow-camera
  on spawned ( -- )  self subscribe tick  self subscribe area-entered  self subscribe scene-shown
    self subscribe player-control
    indoor-projection projection !  true following ! ;
  on tick ( -- )   \ (src: field_update: no following while a window is up - ui_busy)
    low-behind @ if  keep-behind  then  tweens   \ (src: the moves, then the following over them, as it draws)
    following @ window-up? 0= and  ui-busy? 0= and if  reset-due @ if  camera-reset  false reset-due !  then  follow  then ;
  on camera-move-to ( x y z speed -- )  move-speed !  goal 2 cells + !  goal cell+ !  goal !  true moving ! ;
  on camera-turn-to ( yaw step -- )  turn-step !  full-turn 1- and turn-goal !  true turning ! ;
  on camera-height-to ( eye-y step -- )  height-step !  height-goal !  true lifting ! ;
  on camera-dist-to ( dist step -- )  dist-step !  dist-goal !  true pulling ! ;
  on camera-face ( yaw -- )  yaw ! ;
  on camera-pivot ( x y z -- )  target! ;
  on area-entered ( area scene -- )  2drop  camera-reset ;
  on scene-shown ( scene -- )  drop  camera-reset ;   \ src: set_scene_state's camera_reset
  on camera-place ( look-y dist eye-y x y z yaw -- )   \ (src: camera_set writes the camera; following is control's)
    false low-behind !  yaw !  target!  eye-y !  dist !  look-y ! ;
  on player-control ( flag -- )   \ src: player_control_on / off: camera_follow_set (behind him again - once
    dup if  false low-behind !  then  dup following @ 0= and if  true reset-due !  then  following ! ;   \ no window is up)
  on camera-follow ( -- )  following @ 0= if  camera-reset  then  true following ! ;   \ src: camera_follow_set
  on camera-fixed ( -- )  false following ! ;
  on camera-behind ( look-y dist eye-y -- )  eye-y !  dist !  look-y !  false following !  true low-behind ! ;
  on camera-reset ( -- )  camera-reset ;
  on camera-driving ( flag -- )  driving !  profile ;
  on camera-turned-round ( flag -- )  turned-round ! ;
end-behaviour

: spawn-camera ( -- id )  follow-camera camera-state s" camera" spawn ;
