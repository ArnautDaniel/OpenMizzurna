\ camera.fs - the camera (docs/subsystems/camera.md; src: camera.c camera_follow / camera_reset /
\ camera_apply_view, the events' camera ops). Following, it stands where Matthew stood and faces as he faced
\ a moment ago (two of the original's frames: a chase camera behind his back), at the scene's heights and
\ distance; an event puts it where it wants (fixed). Its state is its own (variables: the only camera), drawn
\ each frame through the engine's `camera`.
IN: camera
USING: engine actors messages areas matthew ;

500 constant indoor-projection   400 constant town-projection   \ src: GsSetProjection(g_on_overworld ? 400 : 500)
\ the town's profile (src: camera_reset / camera_follow on the overworld)
-700 constant town-eye-y   -600 constant town-look-y   -2850 constant town-dist
\ the follow's lag: two of the original's frames (15 a second indoors, 30 in the town) - in steps (30 a second)
4 constant indoor-lag   2 constant town-lag

variable following
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
: lag ( -- n )  area @ the-town = if  town-lag  else  indoor-lag  then ;

: profile ( -- )   \ the scene's heights and distance (the town's own)
    area @ the-town = if  town-eye-y town-look-y town-dist  town-projection
    else  scene @ scene-camera  indoor-projection  then
    projection !  dist !  look-y !  eye-y ! ;
: reset ( -- )   \ src: camera_reset: behind Matthew now, the history filled with where he is
    profile  matthew-there? 0= if  exit  then
    #history 0 do  matthew-now remember  loop
    matthew-now yaw !  target! ;
: follow ( -- )
    matthew-there? 0= if  exit  then
    matthew-now remember
    lag entry  dup @ over cell+ @ rot 2 cells + dup @ swap cell+ @   ( x y z yaw )
    yaw !  target! ;

: draw-camera ( -- )  target@ yaw @ dist @ eye-y @ look-y @ projection @ camera ;
' draw-camera on-draw

state: camera-state end-state
behaviour the-camera
  on spawned ( -- )  self subscribe tick  self subscribe area-entered  self subscribe scene-shown
    indoor-projection projection !  true following ! ;
  on tick ( -- )  following @ if  follow  then ;
  on area-entered ( area scene -- )  2drop  reset ;
  on scene-shown ( scene -- )  drop  reset ;   \ src: set_scene_state's camera_reset
  on camera-place ( look-y dist eye-y x y z yaw -- )
    yaw !  target!  eye-y !  dist !  look-y !  false following ! ;
  on camera-follow ( -- )  following @ 0= if  reset  then  true following ! ;   \ src: camera_follow_set
  on camera-fixed ( -- )  false following ! ;
end-behaviour

: spawn-camera ( -- id )  the-camera camera-state s" camera" spawn ;
