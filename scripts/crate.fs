\ crate.fs - a crate Matthew pushes or climbs (docs/subsystems/crate.md; src: player_misc.c crate_area_update,
\ crate_grab, crate_climb; player.c's pushing; areas 10, 11, 16, 19 and 24 have one each). ○ on it takes hold:
\ he pushes it ahead of him as he walks (0xB4 on, stopping where it's blocked), ○ lets go. □ on it climbs up: on
\ top, the look rows for there apply (his perch: 1), □ climbs down the way he came. An area keeps its crate's place
\ and state (a structure) and steps it from its frame hook.
IN: crate
USING: engine actors messages keys areas matthew townsfolk events talk look ;

begin-structure crate-place
  field: crate-slot   field: crate-home-x   field: crate-home-y   field: crate-home-z   field: crate-area   field: crate-floor
  field: crate-state   field: crate-stage   field: crate-heading
end-structure
enum: crate-states  placing standing-by held on-top ;
: crate! ( c slot x y z area floor -- )   \ an area's crate, placed on its first step
    {: c s x y z ar fl :}  c crate-place erase
    s c crate-slot !  x c crate-home-x !  y c crate-home-y !  z c crate-home-z !  ar c crate-area !  fl c crate-floor ! ;
: forget-crate ( c -- )  placing swap crate-state ! ;   \ (entering the area: placed again)
: on-the-crate? ( c -- flag )  crate-state @ on-top = ;
: holding? ( c -- flag )  crate-state @ held = ;

<PRIVATE
$B5 constant reaching   $B6 constant holding   $B8 constant letting-go   $B9 constant let-go
$BE constant climbing-up   $BF constant climbing-down   0 constant on-feet-pose
0 constant on-his-feet   1 constant on-a-crate   3 constant at-a-crate
: him ( -- id )  s" matthew" actor-named ;
: pose ( id -- )  -1 false him send figure-anim ;
: hold-pose ( id -- )  pose  him send figure-hold ;
: perch! ( n -- )  him send figure-perch ;
: facing-it ( c -- )   \ he turns to the crate at once
    crate-slot @ person-now drop nip {: x z :}  matthew-now drop nip {: mx mz :}
    x mx - s>f z mz - s>f fatan2 4096e f* 2e pi f* f/ f>s 4095 and {: h :}  0 h 0 him send figure-turn ;
variable crate-circle-was   variable crate-square-was
: circle? ( -- flag )  pad-circle key-down? dup crate-circle-was @ 0= and  swap crate-circle-was ! ;
: square? ( -- flag )  pad-square key-down? dup crate-square-was @ 0= and  swap crate-square-was ! ;
: done-with ( c -- )  standing-by over crate-state !  0 swap crate-stage !  on-his-feet perch!  true him send figure-on-foot ;

: place-it ( c -- )   \ in its place, square on
    {: c :}  c crate-slot @ c crate-home-x @ c crate-home-y @ c crate-home-z @ set-pos
    c crate-slot @ c crate-area @ c crate-floor @ set-floor  c crate-slot @ 0 0 0 set-rot  standing-by c crate-state ! ;
: holding-it ( c -- )   \ src: crate_grab_update, the pushing state: ○ lets go
    {: c :}  take-pad
    c crate-stage @ case
        0 of  anim-done? if   \ (hold of it: his feet his again, pushing)
            holding pose  c crate-slot @ him send figure-pushes  true him send figure-on-foot  1 c crate-stage !  then  endof
        1 of  circle? if
            false him send figure-on-foot  letting-go pose  -1 him send figure-pushes  2 c crate-stage !  then  endof
        2 of  anim-done? if  let-go pose  3 c crate-stage !  then  endof
        3 of  anim-done? if  c done-with  then  endof
    endcase ;
: up-on-it ( c -- )   \ src: crate_climb_update and the on-top state: ○ looks from up there, □ climbs down
    {: c :}  take-pad
    c crate-stage @ case
        0 of  anim-done? if   \ on top: square over it
            c crate-slot @ person-now drop {: x y z :}  matthew-now drop {: mx my mz :}
            x my z him send figure-place  matthew-now nip nip nip c crate-heading !  1 c crate-stage !  then  endof
        1 of
            circle? if  press-circle exit  then
            square? if  0 c crate-heading @ 0 him send figure-turn  climbing-down hold-pose  on-his-feet perch!
                2 c crate-stage !  then  endof
        2 of  anim-done? if  c done-with  then  endof
    endcase ;
PRIVATE>

: crate-step ( c -- )   \ the area's frame hook steps it
    dup crate-state @ case
        placing of  place-it  endof
        held of  holding-it  endof
        on-top of  up-on-it  endof
        nip
    endcase ;
: crate-push ( c -- )   \ ○ on it: he takes hold (src: crate_area_push)
    {: c :}  c crate-state @ standing-by <> if  exit  then
    c facing-it  false him send figure-on-foot  reaching pose  at-a-crate perch!  held c crate-state !  0 c crate-stage !
    pad-circle key-down? crate-circle-was ! ;
: crate-climb ( c -- )   \ □ on it: he climbs up (src: crate_area_climb)
    {: c :}  c crate-state @ standing-by <> if  exit  then
    c facing-it  false him send figure-on-foot  climbing-up hold-pose  on-a-crate perch!
    on-top c crate-state !  0 c crate-stage !  pad-square key-down? crate-square-was !  pad-circle key-down? crate-circle-was ! ;
: crate-cancel ( c -- )   \ let go, or off it, at once (src: crate_area_cancel: a scene takes over)
    {: c :}  c crate-state @ held < if  exit  then
    -1 him send figure-pushes  on-feet-pose pose  c done-with ;
