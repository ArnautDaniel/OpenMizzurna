\ the camera on the game's data (scripts/camera.fs): following Matthew it keeps the scene's heights and distance
\ (the area's camera table) - a shot an event left, or one it set while he was the player's, doesn't stay
USING: tester engine actors messages areas matthew events camera ;

: steps ( n -- )  0 ?do  1 tick-tasks  actors-frame  loop ;
: settled ( -- )  10 0 do  deliver  loop  events-stop  deliver ;
settled
: profiled? ( -- flag )   \ the camera at the scene's own heights and distance
    scene @ scene-camera {: e l d :}  eye-y @ e =  look-y @ l = and  dist @ d = and ;

21 constant the-diner   329445 constant inside-x   269064 constant inside-z   \ (test_area021's: inside its door)
: in-diner ( -- )
    the-diner 0 s" areas" actor-named send enter-area  deliver
    inside-x 0 inside-z s" matthew" actor-named send figure-place  deliver  2 steps  events-stop  deliver ;

testing entering an area: behind Matthew, at the scene's profile
T{ in-diner  matthew-there?  following @  profiled? -> true true true }T
T{ scene @ scene-camera nip nip 0<> -> true }T

testing a shot set while he is the player's: the following takes the profile back (src: camera_follow)
T{ -116 -6374 -1802 0 0 0 0 the-camera send camera-place  deliver  2 steps  profiled? -> true }T

testing a shot then camera-follow while it was still following: the profile, not the shot's far distance
T{ the-camera send camera-follow  deliver  -116 -6374 -1802 0 0 0 0 the-camera send camera-place  deliver
   the-camera send camera-follow  deliver  2 steps  profiled? -> true }T

test-summary
