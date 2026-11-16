\ real-drive.fs - the car driven as a car. A step at the wheel is a little simulation, in the game's units (a
\ speed in units a frame - the speedometer shows a sixth of it, km/h - turning in radians a frame):
\  - the engine: a torque curve through five gears (auto, or L1 / R1 by hand), its revs from the speed, a pause in
\    the drive as it shifts, engine braking off the gas, revs held up push-was away (the clutch slipping);
\  - the gas (R2, or ✕) and the brakes (L2, or □; still, they back it up), as hard as the tyres allow;
\  - the tyres: each axle's sideways grip rises with its slip angle to a peak, then falls away (a magic formula);
\    the load on each moves with the braking and the pull - the brakes give the front bite, the gas the back's
\    grip taken by its pull (too much in a low gear: the tail steps out);
\  - the car turning as those grips swing it (a yaw with weight to it); at a crawl simply where the wheels point;
\  - the wheel (the left stick, or the d-pad eased), its lock less as the speed builds;
\  - walls slid along, losing speed; head on it stops and rocks back.
\ A tachometer and the gear by the speedometer; the engine's note by the revs; the camera swinging after where the
\ car goes, its slide showing. F1's options: Driving (Real or Game), Gears (Auto or Manual), kept beside the saves.
IN: real-drive
USING: engine keys mods screen figures floors camera driving driving.private options ;

hook-before: car-handling handling-before ( -- )
hook-before: car-moving moving-before ( -- )
hook-before: engine-note note-before ( speed -- key )
hook-before: follow-yaw camera-before ( yaw fresh? -- yaw' )

\ ---- the settings: real or the game's driving; auto or manual gears ----
begin-structure settings-record  field: game-driving  field: manual-gears  end-structure
create settings  settings-record allot   settings settings-record erase
: settings-name ( -- addr len )  s" real-drive" ;
: settings-write ( -- )  settings settings-record settings-name save-file drop ;
settings settings-record settings-name load-file drop
: game's? ( -- flag )  settings game-driving @ 0<> ;
: manual? ( -- flag )  settings manual-gears @ 0<> ;
: driving-shown ( -- addr len )  game's? if  s" Game"  else  s" Real"  then ;
: driving-change ( dir -- )  drop  game's? 0= settings game-driving !  settings-write ;
: gears-shown ( -- addr len )  manual? if  s" Manual"  else  s" Auto"  then ;
: gears-change ( dir -- )  drop  manual? 0= settings manual-gears !  settings-write ;
s" Driving" ' driving-shown ' driving-change add-option
s" Gears" ' gears-shown ' gears-change add-option
: ours? ( -- flag )  game's? 0=  driving? and ;

\ ---- the units ----
4096e fconstant whole-turn-f
: >radians ( F: heading -- r )  2e f* pi f* whole-turn-f f/ ;
: >heading ( F: r -- heading )  whole-turn-f f* 2e pi f* f/ ;
7.06e fconstant g              \ a g, in units a frame a frame (km/h = speed / 6)
: clamp ( F: x lo hi -- x' )  frot fmin fmax ;   \ (lo <= x <= hi)
: lerp ( F: a b t -- a' )  {: F: a F: b F: t :}  b a f- t f* a f+ ;

\ ---- the player's hands ----
: pressed ( key -- F: 0|1 )  key-down? if  1e  else  0e  then ;
: gas ( F: -- t )  pad-triggers fswap fdrop  pad-cross pressed fmax ;     \ 0 .. 1
: brake ( F: -- t )  pad-triggers fdrop  pad-square pressed fmax ;
0.1e fconstant dpad-ease   fvariable dpad-wheel
: wheel ( F: -- w )   \ -1 .. 1, right +: the stick as it is; the d-pad turned toward full lock a little a frame
    pad-stick fdrop  fdup f0= 0= if  0e dpad-wheel f!  exit  then  fdrop
    pad-right pressed pad-left pressed f-  dpad-wheel f@ f-  dpad-ease fnegate dpad-ease clamp
    dpad-wheel f@ f+  dpad-wheel f!  dpad-wheel f@ ;

\ ---- the car's state (the game's: velocity - ahead, 60 units a frame for 1 - ch, cx cz; and these) ----
fvariable heading   fvariable at-x   fvariable at-z    \ (the game's are whole numbers)
fvariable sideways      \ units a frame, to the right
fvariable yaw-rate      \ radians a frame, right +
fvariable lock          \ the front wheels, radians, right +
fvariable push-was       \ the last step's push ahead (units a frame a frame: braking -)
fvariable throttle   fvariable revs
fvariable scrub         \ this step's speed lost to the tyres' sideways work (the front's turned against it)
variable ch-set   variable cx-set   variable cz-set
: in-step ( -- )   \ the game (a scene, a bump, the unsticking) moved the car since: from where it is now
    ch @ ch-set @ <> if  ch @ s>f heading f!  0e yaw-rate f!  then
    cx @ cx-set @ <>  cz @ cz-set @ <> or if  cx @ s>f at-x f!  cz @ s>f at-z f!  0e sideways f!  then ;
: kept ( -- )  ch @ ch-set !  cx @ cx-set !  cz @ cz-set ! ;
: ahead ( F: -- u )  velocity f@ units-per f* ;
: ahead! ( F: u -- )  units-per f/ velocity f! ;

\ ---- the engine and the gears ----
5 constant #gears   0 constant reverse
create gear-tops  0e f,  240e f,  390e f,  540e f,  670e f,  800e f,   \ units a frame at the red line (1st: 40 km/h)
create gear-pulls  0e f,  4.2e f,  2.6e f,  1.85e f,  1.5e f,  1.26e f,  \ units a frame a frame at the torque's peak
2.4e fconstant reverse-pull   -150e fconstant reverse-top
7000e fconstant red-line   900e fconstant idle-revs   6600e fconstant shift-up   2600e fconstant shift-down
3500e fconstant launch-revs   5 constant shift-frames
variable gear   1 gear !   variable shifting
: gear-top ( n -- F: u )  floats gear-tops + f@ ;
: gear-pull ( n -- F: a )  floats gear-pulls + f@ ;
\ the torque by the revs, of its peak (a curve through its points)
create torque-curve  1000e f, 0.6e f,  2500e f, 0.82e f,  4500e f, 1e f,  6000e f, 0.95e f,  7000e f, 0.8e f,
5 constant #torque-points
: point ( i -- F: revs torque )  2* floats torque-curve + dup f@ float+ f@ ;
: torque ( F: revs -- t )
    {: F: r :}  0 point fdrop r f>= if  0 point fnip exit  then
    #torque-points 1 do
        i point fdrop r f>= if  i 1- point  i point {: F: r0 F: t0 F: r1 F: t1 :}
            t0 t1  r r0 f- r1 r0 f- f/  lerp unloop exit  then
    loop  #torque-points 1- point fnip ;
: revs-now ( F: u -- )   \ from the speed in the gear; push-was away, the clutch holds them up by the gas
    fabs gear @ reverse = if  reverse-top fabs f/  else  gear @ gear-top f/  then  red-line f*
    idle-revs throttle f@ launch-revs f* f+  gear @ 1 > if  fdrop idle-revs  then  fmax
    idle-revs red-line clamp revs f! ;
: shift ( n -- )  1 max #gears min  dup gear @ <> if  gear !  shift-frames shifting !  else  drop  then ;
: auto-shift ( -- )
    gear @ reverse = if  exit  then
    revs f@ shift-up f>  throttle f@ f0> and  if  gear @ 1+ shift exit  then
    revs f@ shift-down f<  if  gear @ 1- shift  then ;
variable l1-was   variable r1-was
: hand-shift ( -- )
    pad-r1 r1-was key-went-down? if  gear @ reverse = if  1 gear !  else  gear @ 1+ shift  then  then
    pad-l1 l1-was key-went-down? if  gear @ 1- shift  then ;
: engine-push ( F: -- a )   \ the drive's push ahead, by the gas (none while it shifts; past the red line, cut)
    shifting @ if  -1 shifting +!  0e exit  then
    revs f@ red-line f>= if  0e exit  then
    gear @ reverse = if  reverse-pull fnegate  else  gear @ gear-pull  then
    revs f@ torque f*  throttle f@ f* ;

\ ---- the drag and the brakes ----
0.05e fconstant rolling   1.9e-6 fconstant air   0.25e fconstant engine-braking   \ (of a g, at the red line)
1.4e fconstant brakes           \ of a g, at most (as the tyres)
5e fconstant still               \ units a frame: slower, stopped
: drag ( F: u -- a )   \ against the way it moves: the road, the air, the engine off the gas
    {: F: u :}  u fabs {: F: s :}  s s f* air f* rolling f+ {: F: d :}
    throttle f@ f0= if  revs f@ red-line f/ engine-braking f* g f*  d f+  to d  then
    s f0= if  0e exit  then  u f0< if  d  else  d fnegate  then ;
: backing? ( -- flag )  gear @ reverse = ;
: pedals-now ( F: u -- a )   \ the push ahead this step: the drive, the brakes, the drag
    {: F: u :}
    \ still: the brake into reverse, the gas out of it
    u fabs still f< if  brake f0> backing? 0= and if  reverse gear !  then
                        gas f0> backing? and if  1 gear !  then  then
    \ in reverse the brake pedal drives it and the gas stops it; ahead, the other way round
    backing? if  brake gas  else  gas brake  then {: F: drive F: stopping :}
    drive throttle f!
    u revs-now  manual? if  hand-shift  else  auto-shift  then
    engine-push
    stopping brakes g f* f* {: F: br :}   \ (the brakes: against the way it rolls)
    u f0> if  br f-  else  u f0< if  br f+  then  then
    u drag f+ ;

\ ---- the tyres ----
\ (by the speedometer's km/h the game's car is under a metre long: this one is a car's size, 2.2 m between the
\ axles, or its turning would be a toy's)
1400e fconstant wheelbase                              \ units (648 a metre, by the speedometer)
0.52e fconstant front-weight                           \ the front's share of the weight: the middle nearer the front
wheelbase 1e front-weight f- f* fconstant to-front     \ units from the middle to each axle
wheelbase front-weight f* fconstant to-rear
to-front to-rear f* fconstant yaw-weight               \ (the car's swing: its mass spread about the middle)
1.6e fconstant tyre-grip                               \ of a g, at the peak (a road car's is about 1: the town
                                                       \  was laid out for the game's own grip, half again as much)
0.24e fconstant load-shift                             \ the weight shifted by the push (its part of it)
10e fconstant stiffness   1.6e fconstant curve-shape   \ the magic formula's B and C (its peak near 0.1 radians)
1.1e fconstant rear-bite                               \ the rear grips this much more: the front gives first - a
                                                       \  little understeer, steady as a road car
: grip ( F: slip most stiffness -- force )   \ sideways, against the slip, to a peak and then falling away
    {: F: a F: d F: b :}  a b f* 1e fatan2 curve-shape f* fsin d f* fnegate ;
: loads ( F: -- front rear )   \ each axle's grip at most (units a frame a frame): the weight shifts by the push
    push-was f@ load-shift f* {: F: shifted :}
    g front-weight f* shifted f-  0.1e g f* fmax  tyre-grip f*
    g 1e front-weight f- f* shifted f+  0.1e g f* fmax  tyre-grip f* ;
: rear-left ( F: most push -- most' )   \ the rear's sideways grip left beside its pull (a circle of grip)
    fdup f*  fover fdup f*  fswap f-  0e fmax fsqrt  fswap 0.15e f* fmax ;

\ ---- turning ----
0.55e fconstant full-lock      \ radians, at a crawl
0.12e fconstant best-slip      \ radians: the front tyres' slip a little short of their peak
0.25e fconstant lock-ease      \ a frame, the wheels toward where the wheel's turned
0.3e fconstant wheels-shown    \ radians the front wheels show at full turn of the wheel, at least
40e fconstant grips-from   120e fconstant grips-fully   \ the tyres' simulation from these speeds (slower: rolling)
4 constant substeps
: lock-allowed ( F: u -- a )   \ the wheel turned all the way: the turn the grip holds at this speed, and the
    fdup f*  fdup f0= if  fdrop full-lock exit  then   \  front's best slip on it (full lock is the most the tyres can give)
    wheelbase tyre-grip f* g f* fswap f/ 1e fatan2  best-slip f+  full-lock fmin ;
: lock-now ( F: u -- )
    lock-allowed  wheel f*
    lock f@ f-  lock-ease f*  lock f@ f+  lock f! ;
: rolling-turn ( F: u -- r )  lock f@ ftan f*  wheelbase f/ ;   \ where the wheels point
: tyres-step ( F: u push dt -- )   \ the sideways speed and the yaw by the tyres' grip
    {: F: u F: push F: dt :}
    loads push rear-left {: F: front F: rear :}
    sideways f@ yaw-rate f@ to-front f* f+  u fabs fatan2  lock f@ f-  front stiffness grip {: F: fy :}
    sideways f@ yaw-rate f@ to-rear f* f-  u fabs fatan2  rear rear-bite f* stiffness grip {: F: ry :}
    fy lock f@ fsin f* fnegate  sideways f@ yaw-rate f@ f* f+  dt f*  scrub f@ f+  scrub f!
    fy lock f@ fcos f* ry f+  u yaw-rate f@ f* f-  dt f*  sideways f@ f+  sideways f!
    fy to-front f*  ry to-rear f* f-  yaw-weight f/  dt f*  yaw-rate f@ f+  yaw-rate f! ;
: turning-step ( F: u push dt -- )
    {: F: u F: push F: dt :}
    u fabs grips-from f- grips-fully grips-from f- f/  0e 1e clamp {: F: w :}
    u f0> w f0> and if  u push dt tyres-step  then
    u rolling-turn  yaw-rate f@  w lerp  yaw-rate f!   \ (slow, as it rolls; backing too)
    sideways f@ w f*  sideways f! ;

\ ---- the pose: the front wheels as the wheel's turned; the body rolling in a turn and pitching under the pedals ----
35e fconstant roll-per-g      \ the game's angle units (4096 a turn), a g sideways
40e fconstant pitch-per-g     \ a g ahead or braking
0.2e fconstant body-ease      \ a frame, toward them (its springs)
fvariable rolled   fvariable pitched
: shown-wheels ( -- )   \ (the game's steer is a tenth of the pose's turn)
    lock f@  wheel wheels-shown f*  fover fabs fover fabs f< if  fswap  then  fdrop
    >heading 10e f/ f>s steer ! ;
: body-now ( -- )
    \ rolling outward (turning right, to its left), pitching up under the gas
    ahead yaw-rate f@ f* g f/ roll-per-g f*  rolled f@ f-  body-ease f*  rolled f@ f+  rolled f!
    push-was f@ g f/ pitch-per-g f*  pitched f@ f-  body-ease f*  pitched f@ f+  pitched f!
    rolled f@ f>s body-lean !  pitched f@ f>s body-pitch ! ;

\ ---- a step at the wheel ----
: real-handling ( -- )
    ours? 0= if  handling-before exit  then
    in-step  ahead {: F: u :}
    u pedals-now {: F: push :}
    u lock-now
    push u f* f0<  u push f+ u f* f0< and  if  0e  else  u push f+  then {: F: u' :}   \ (brakes stop it, not reverse it)
    push push-was f!  u' ahead!
    0e scrub f!  substeps 0 do  u' push  1e substeps s>f f/  turning-step  loop
    u' f0> if  ahead scrub f@ f+ 0e fmax ahead!  then
    yaw-rate f@ >heading heading f@ f+  heading f!  heading f@ f>s 4095 and ch !
    shown-wheels  body-now
    throttle f@ f0> if  burn  then ;
' real-handling is car-handling

\ ---- moving: ahead and sideways; walls slid along ----
-0.15e fconstant rocked        \ head on: back by this part of the speed
: blocked-at? ( x z -- flag )   \ the car's box there in a wall, a billboard, a road block
    {: x z :}  x z hits?  x z wall? or  x z solid-billboard? or  x z road-blocked? or ;
: place ( F: x z -- )  at-z f!  at-x f!  at-x f@ f>s cx !  at-z f@ f>s cz ! ;
: world ( F: -- wx wz )   \ its velocity on the ground (ahead and sideways turned by the heading)
    heading f@ >radians {: F: a :}  a fsin a fcos {: F: sn F: cs :}  ahead sideways f@ {: F: u F: v :}
    u sn f*  v cs f* f+   u cs f*  v sn f* f- ;
0.97e fconstant along-wall     \ of the speed along a wall kept, a frame against it
: world! ( F: wx wz -- )   \ back to ahead and sideways
    heading f@ >radians {: F: a :}  a fsin a fcos {: F: sn F: cs :}  {: F: wx F: wz :}
    wx sn f*  wz cs f* f+ ahead!   wx cs f*  wz sn f* f- sideways f! ;
: slid-along ( F: wx wz -- )   \ against a wall: what went into it gone, the rest kept but for the rub
    along-wall f* fswap along-wall f* fswap world!  scrape  yaw-rate f@ 0.7e f* yaw-rate f! ;
: move-by ( F: dx dz -- moved? )   \ a part of the step: on, or along a wall, or stopped by it
    {: F: dx F: dz :}  at-x f@ dx f+  at-z f@ dz f+ {: F: nx F: nz :}
    nx f>s nz f>s blocked-at? 0= if  nx nz place  true exit  then
    nx f>s cz @ blocked-at? 0= if  world fdrop 0e slid-along  nx at-z f@ place  false exit  then
    cx @ nz f>s blocked-at? 0= if  world fnip 0e fswap slid-along  at-x f@ nz place  false exit  then
    crashed  ahead rocked f* ahead!  0e sideways f!  0e yaw-rate f!  false ;
: real-moving ( -- )
    ours? 0= if  moving-before exit  then
    heading f@ >radians {: F: a :}  a fsin a fcos {: F: sn F: cs :}   \ ahead (sin, cos); to the right (cos, -sin)
    ahead sideways f@ {: F: u F: v :}
    u sn f*  v cs f* f+  substeps s>f f/   u cs f*  v sn f* f-  substeps s>f f/ {: F: dx F: dz :}
    substeps 0 do  dx dz move-by 0= if  leave  then  loop
    cx @ cz @ ground-under cy !  tilt-car  kept ;
' real-moving is car-moving

\ ---- the engine's note: by the revs ----
41 constant idle-note   40 constant note-span
: real-note ( speed -- key )
    ours? 0= if  note-before exit  then
    drop  revs f@ idle-revs f- red-line idle-revs f- f/ note-span s>f f* f>s idle-note + ;
' real-note is engine-note

\ ---- the tachometer and the gear (by the game's speedometer) ----
-128 constant tach-x   92 constant tach-y   88 constant tach-w   5 constant tach-h   -36 constant gear-x   74 constant gear-y
$00000090 constant tach-back   $E8E8E8FF constant tach-ink   $FF5040FF constant tach-red
create gear-names  char R c,  char 1 c,  char 2 c,  char 3 c,  char 4 c,  char 5 c,
: draw-tach ( -- )
    ours? 0= if  exit  then
    blend-usual pen-blend  tach-x tach-y >screen {: x y :}  tach-w scale *  tach-h scale * {: w h :}
    tach-back pen-color  x y w h draw-rect
    revs f@ shift-up f>= if  tach-red  else  tach-ink  then pen-color
    x y  revs f@ red-line f/ w s>f f* f>s  h draw-rect
    tach-ink pen-color  scale 2* pen-scale  gear-names gear @ + 1  gear-x gear-y >screen draw-text ;
' draw-tach on-draw

\ ---- the camera: after where the car goes (its slide showing) ----
0.14e fconstant camera-ease   0.6e fconstant toward-going   fvariable camera-way
: going ( F: -- way )   \ between the way it faces and the way it moves; slow or backing, the way it faces
    ahead fdup 60e f< if  fdrop heading f@ exit  then
    sideways f@ fswap fatan2 >heading toward-going f*  heading f@ f+ ;
: real-camera ( yaw fresh? -- yaw' )
    ours? 0= if  camera-before  dup s>f camera-way f!  exit  then
    if  s>f camera-way f!  else  drop  then
    camera-way f@ f>s 4095 and  going f>s 4095 and  way-between s>f camera-ease f*  camera-way f@ f+  camera-way f!
    camera-way f@ f>s 4095 and ;
' real-camera is follow-yaw
