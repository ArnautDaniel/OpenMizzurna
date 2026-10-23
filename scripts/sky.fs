\ sky.fs - the sky behind the scene (docs/subsystems/sky.md; src: sky.c): its colour by the hour (solid above, a
\ gradient down to the horizon), the stars at night, the clouds drifting, the horizon strip (the mountains), the
\ sun by day with its lens flares, the moon by night - in the original's screen pixels (320 x 240, the origin at
\ the centre), scrolled sideways with the camera's heading (a full turn is 2048 pixels) and set on the horizon.
IN: sky
USING: engine actors messages clock areas camera screen figures daylight ;

\ ---- the hour's colours and shades, eased a step every other frame (src: func_800248C0, func_80024E48) ----
create top  rgb allot   create low  rgb allot   create top-now  rgb allot   create low-now  rgb allot
variable clouds-shade   variable horizon-shade   variable clouds-now   variable horizon-now
variable frames-seen   fvariable drift   \ (frames; the clouds' drift, pixels)

<PRIVATE
: hour-rgb ( addr len dest -- )  {: a n d :}  a n 0 100 hour-u8  a n 1 100 hour-u8  a n 2 100 hour-u8  d rgb! ;
: greyish ( addr -- )   \ src: not clear: the sum of the three halved, kept as a byte (sic)
    {: a :}  a rgb@ + + 2/ 255 and  dup dup  a rgb! ;
: the-hours ( -- )
    s" hour-sky-top" top hour-rgb   s" hour-sky-low" low hour-rgb
    s" hour-clouds" 0 100 hour-u8  s" hour-horizon" 0 100 hour-u8  {: c h :}
    the-weather clear <> if  top greyish  low greyish  then
    the-weather overcast = if  c 2/  else  the-weather snowing = if  c 4 /  else  c  then  then  clouds-shade !
    h horizon-shade ! ;
: toward ( now target -- now' )  2dup < if  drop 1+  else  2dup > if  drop 1-  else  drop  then  then ;
: ease-rgb ( now target -- )  {: n t :}  3 0 do  n i cells + @  t i cells + @ toward  n i cells + !  loop ;
: ease ( -- )
    top-now top ease-rgb  low-now low ease-rgb
    clouds-now @ clouds-shade @ toward clouds-now !   horizon-now @ horizon-shade @ toward horizon-now ! ;
: jump ( -- )   \ (a jump in time: at once)
    top top-now rgb move  low low-now rgb move  clouds-shade @ clouds-now !  horizon-shade @ horizon-now ! ;

\ ---- the original's screen ----
2048 constant pan   1024 constant half-pan
: heading-x ( -- x )  camera-yaw full-turn 1- and 2/ negate ;   \ a turn to the left scrolls the sky right
: wrapped ( x -- x' )  half-pan + pan 1- and half-pan - ;      \ (the PlayStation's 11-bit corners wrapped)
: horizon ( -- y )   \ where the scene's horizon is: up from the centre as the camera looks down
    look-y @ eye-y @ -  projection @ *  dist @ abs 1 max /  negate ;
: edge ( -- x )  screen-size drop 2/ scale / 1+ ;   \ the window's half width, in the original's pixels
: rgba ( r g b -- c )  rot 24 lshift  rot 16 lshift or  swap 8 lshift or  $FF or ;
: shade ( grey -- c )  2* 255 min  dup dup rgba ;   \ (128: the picture as it is)
: corner ( x y -- x' y' )  >screen ;
: band ( y0 y1 c0 c1 -- )   \ a band the window's width, colour c0 at y0 to c1 at y1
    {: y0 y1 c0 c1 :}  edge {: e :}
    e negate y0 corner  e y0 corner  e negate y1 corner  e y1 corner  c0 c0 c1 c1 draw-quad ;
: picture ( image x0 y0 x1 y1 -- )   \ an image from corner to corner (upside down: y1 above y0)
    {: im x0 y0 x1 y1 :}  x0 y0 corner {: wx wy :}  x1 y1 corner {: wx' wy' :}
    im wx wy wx' wx - wy' wy - draw-image ;
: piece-at ( image x w y0 y1 -- )  {: im x w a b :}  im x a x w + b picture ;   \ w wide, from y0 to y1

\ ---- its pieces (src: sky.c) ----
113 constant gradient    \ the colour's gradient: over the 113 pixels above the horizon (src: 0x78 .. 7)
120 constant above   7 constant gap   487 constant sky-top
20 constant fill-red-low   70 constant fill-red-high   30 constant fill-low   80 constant fill-high
: fill ( -- c )   \ the screen behind it all: the top colour kept dim
    top-now rgb@  rot fill-red-low max fill-red-high min  rot fill-low max fill-high min
    rot fill-low max fill-high min  rgba ;
: colours ( h -- )   \ src: func_8001ECBC
    {: h :}  screen-size nip 2/ scale / 1+ {: v :}  v negate v fill fill band
    h sky-top -  h above -  top-now rgb@ rgba dup band
    h above -  h gap -  top-now rgb@ rgba  low-now rgb@ rgba  band ;

20 constant #stars   320 constant star-repeat   200 constant star-blue
$46 constant star-dim   $64 constant star-bright
8 60 * 1+ constant stars-set   16 60 * constant stars-rise   \ from 16:00 to 8:00 (src: m - 0x1E1 < 0x1DF)
: night-sky? ( -- flag )  clock-time minutes/day mod {: m :}  m stars-set <  m stars-rise >= or ;
: star ( n h -- )
    {: n h :}  s" stars-x" n exe-s32 heading-x + full-turn 1- and  dup star-repeat > if  star-repeat mod  then {: x :}
    s" stars-y" n exe-s32 h + above - {: y :}
    edge star-repeat / 1+  edge negate star-repeat / 1-  do   \ (repeated across a wider window)
        x i star-repeat * + screen-right -  y  corner  scale scale draw-rect
    loop ;
: stars ( h -- )   \ src: func_8001F000: twinkling, on clear nights
    the-weather clear <>  night-sky? 0= or if  drop exit  then
    {: h :}  frames-seen @ 1 and if  star-dim  else  star-bright  then  {: c :}
    c c star-blue rgba pen-color  1 pen-blend
    #stars 0 do  i h star  loop ;

8 constant pieces   256 constant piece   246 constant cloud-band
: clouds ( h -- )   \ src: func_8001F26C: 8 above the horizon, 8 mirrored above them, drifting
    {: h :}  s" sky/clouds.png" image {: im :}  im 0= if  exit  then
    clouds-now @ shade pen-color  3 pen-blend
    heading-x drift f@ f>s + full-turn 1- and {: o :}
    pieces 0 do
        im  o screen-right - i piece * + wrapped  piece  h cloud-band -  h gap -  piece-at
        im  o screen-right - full-turn - i pieces + piece * + wrapped  piece  h cloud-band -  h sky-top -  piece-at
    loop ;
70 constant strip
: mountains ( h -- )   \ src: func_8001F4C0: the horizon strip
    {: h :}  s" sky/horizon.png" image {: im :}  im 0= if  exit  then
    horizon-now @ shade pen-color  4 pen-blend
    heading-x {: o :}
    pieces 0 do  im  o screen-right - i piece * + wrapped  piece  h strip -  h gap -  piece-at  loop ;

\ the sun and the moon: their heights by the hour (the hour's entry read as a byte, as the game did)
: height ( addr len hour -- y )
    {: a n hr :}  hr 1+ 24 mod {: nx :}
    a n nx exe-s16  a n hr exe-s16 - s>f 60e f/  clock-time time>minute s>f f*  a n hr exe-s16 255 and s>f f+ f>s ;
8 60 * constant sun-up   16 60 * constant sun-down
$2A7 constant east   $6A7 constant west
14 constant #flares
: sun-x ( -- x )  heading-x  clock-time time>hour 12 >= if  west  else  east  then + ;
: sun ( -- )   \ src: func_8001F670: by day in clear weather, and its lens flares
    the-weather clear <> if  exit  then
    clock-time minutes/day mod {: m :}  m sun-up <  m sun-down > or if  exit  then
    s" sun-heights" clock-time time>hour height {: sy :}  sun-x {: sx :}
    s" sky/sun.png" image ?dup if
        $80 shade pen-color  3 pen-blend
        sx $C0 - wrapped  $40  sy $98 -  sy $58 -  piece-at
    then ;
: flare-at ( i sx sy -- x y )   \ src: along the line from the sun through the screen's centre
    {: i sx sy :}  s" flare-distances" i exe-s16 s>f {: F: d :}
    $A0 s>f  $5A sy - s>f fatan2 {: F: a1 :}   $A0 sx - s>f  $5A s>f fatan2 {: F: a2 :}
    sx  d a2 fsin f* f>s -   sy  d a1 fcos f* f>s - ;
: flare-name ( i -- addr len )  $clear s" sky/flare-" $+  1+ n>str $+  s" .png" $+  $@ ;
: flare ( i sx sy -- )   \ a flare, its half size about its place
    {: i sx sy :}  i flare-name image {: im :}  im 0= if  exit  then
    s" flare-sizes" i exe-u8 {: r :}  i sx sy flare-at {: fx fy :}
    im  fx r - $A0 -  fy r - $78 -  fx r + $A0 -  fy r + $78 -  picture ;
: flares ( -- )   \ over the scene (front)
    the-weather clear <> if  exit  then
    clock-time minutes/day mod {: m :}  m sun-up <  m sun-down > or if  exit  then
    s" sun-heights" clock-time time>hour height {: sy :}  sun-x wrapped {: sx :}
    $80 shade pen-color  3 pen-blend
    #flares 0 do  i sx sy flare  loop ;
28 constant waxing   31 constant full-moon   \ its phases: to the 27th, to the 30th, then the 31st
2 60 * 1+ constant moon-set   17 60 * constant moon-rise
22 constant moon-west
: moon ( -- )   \ src: func_8001F9B0: by night in clear weather, rising an hour later each day
    the-weather clear <> if  exit  then
    clock-time {: t :}  t time>hour first-day + t time>day - 24 + 24 mod {: hr :}
    hr 60 * t time>minute + {: m :}  m moon-set >=  m moon-rise < and if  exit  then
    s" moon-heights" hr height {: my :}
    heading-x  t time>hour moon-west >= if  west  else  east  then + {: mx :}
    t time>day waxing < if  s" sky/moon-1.png"  else  t time>day full-moon < if  s" sky/moon-2.png"  else  s" sky/moon-3.png"  then  then
    image ?dup if
        $80 shade pen-color  3 pen-blend
        mx $AC - wrapped  24  my $84 -  my $6C -  piece-at
    then ;

: draw-sky ( -- )
    area @ 0< if  exit  then
    true behind  horizon {: h :}
    h colours  h stars  sun  moon  h clouds  h mountains
    false behind  flares
    $FFFFFFFF pen-color  0 pen-blend ;
' draw-sky on-draw

: sky-step ( -- )
    1 frames-seen +!  game-frames 8e f/ drift f@ f+ drift f!
    frames-seen @ 1 and if  the-hours ease  then ;
PRIVATE>

state: sky-state end-state
behaviour the-sky
  on spawned ( -- )  self subscribe tick  self subscribe time-set  self subscribe area-entered ;
  on tick ( -- )  sky-step ;
  on time-set ( t -- )  drop  the-hours jump ;
  on area-entered ( area scene -- )  2drop  the-hours jump ;
end-behaviour

: spawn-sky ( -- id )  the-sky sky-state s" sky" spawn ;
