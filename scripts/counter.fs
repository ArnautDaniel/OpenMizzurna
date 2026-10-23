\ counter.fs - sitting at a bar's counter (docs/subsystems/counter.md; src: the diner's and Club Wolves' overlays'
\ copies of it - port/src/game/counter.c reads them as one): standing at a free stool facing the counter, □ sits
\ down; seated, → / ← lean toward the next stool (○ talks to who sits there, □ gets up, letting go sits back), □
\ waves the barman over and, once there, orders (the bar's own: the order xt), ○ talks to them. An area's frame
\ hook steps it (counter-step) with the bar's settings; the stools are the townsfolk's (seat-take, seat-leave).
IN: counter
USING: engine actors messages keys areas figures matthew townsfolk events talk look ;

\ ---- a bar's settings ----
begin-structure bar-counter
  field: bar               \ its stools (at-diner, at-club)
  field: along-x?          \ the stools run along x (else z)
  field: cross-lo   field: cross-hi   \ standing across the counter where one may sit: lo < it <= hi
  field: facing            \ the heading seated
  field: last-front-only?  \ the last stool can be sat on from the low side only
  field: flip?             \ the approach side's test mirrored (src: CounterDef.flip)
  field: right-step        \ leaning right looks at the stool this many on (left: the other way)
  field: counter-y         \ the height seated
  field: stand-cross   field: seat-cross   \ across: standing to sit, and seated
  2 cells +field sit-motions        \ sitting down, coming from the low side, the high
  2 cells +field lean-in-motions    \ leaning toward the next stool: right, left
  2 cells +field lean-motions
  2 cells +field lean-out-motions
  2 cells +field stand-motions      \ getting up after leaning right, left
  field: beckon-motion
  field: last-right   field: last-left   \ the stools with no neighbour leaning that way
  field: no-lean-right     \ a stool that cannot lean right (-1 none)
  field: camera-yaw   field: camera-yaw-alt   field: camera-alt-seats   \ (a bit a stool)
  field: camera-eye-y   field: camera-look-y   field: camera-dist   field: camera-cross
  field: barman   field: barman-cross   field: barman-heading
  field: order-xt          \ ( -- done? ) ordering, a step a frame
end-structure

\ ---- where Matthew is at it (one counter at a time: his) ----
enum: at-counter  off-stool on-a-stool ;
enum: counter-mode  sitting-down 1 settling  2 on-stool  20 leaning  40 getting-up ;
0 constant no-lean   1 constant lean-right   2 constant lean-left
variable the-counter   variable at-state   variable mode   variable step   variable seat   variable lean
variable doing   variable talking-with   variable barman-at   variable walking-over
variable sat-in   \ (the area he sat in: elsewhere since, a warp took him off his stool)
-1 constant nobody-there
: forget-counter ( -- )  off-stool at-state !  0 mode !  0 step !  -1 seat !  no-lean lean !
    nobody-there talking-with !  -1 barman-at !  0 walking-over ! ;
forget-counter
: at-the-counter? ( -- flag )  at-state @ on-a-stool = ;
: barman-here ( -- seat | -1 )  barman-at @ ;
: talking-to ( -- slot | -1 )  talking-with @ ;

\ the pad on a stool (src: pad_map field mode 1: → / ← held, ○ pressed, □ held)
variable stool-circle-was
: stool-circle? ( -- flag )  pad-circle key-down? dup stool-circle-was @ 0= and  swap stool-circle-was ! ;
: stool-square? ( -- flag )  pad-square key-down? ;

<PRIVATE
: c ( -- def )  the-counter @ ;
$77 constant seated-pose   $92 constant sit-step   $BE constant stool-reach
$400 constant quarter   $C00 constant three-quarters   4095 constant heading-mask
40 constant barman-pace   1 constant barman-walk-motion
: along ( x z -- a )  c along-x? @ if  drop  else  nip  then ;
: across ( x z -- a )  c along-x? @ if  nip  else  drop  then ;
: stool-along ( seat -- a )  c bar @ swap seat-spot drop {: x y z :}  x z along ;
: point ( along across -- x z )  c along-x? @ 0= if  swap  then ;
: matthew-xz ( -- x z )  matthew-now {: x y z h :}  x z ;
: side ( -- 0|1 )  lean @ lean-right = if  0  else  1  then ;
: nth ( addr i -- n )  cells + @ ;

\ ---- Matthew, the camera, the barman: by their messages ----
: matthew-id ( -- id )  s" matthew" actor-named ;
: put ( along y across -- )   \ Matthew there, facing the counter
    {: a y x :}  a x point {: px pz :}  px y pz matthew-id send figure-place
    0 c facing @ 0 matthew-id send figure-turn ;
: pose ( id loop -- )  -1 swap matthew-id send figure-anim ;
: sit-still ( -- )  seated-pose true pose ;
: on-foot ( flag -- )  matthew-id send figure-on-foot ;
: counter-camera ( -- )   \ a fixed camera beside the stool (src: func_80102404)
    the-camera send camera-fixed
    c camera-alt-seats @ seat @ rshift 1 and if  c camera-yaw-alt @  else  c camera-yaw @  then {: yaw :}
    seat @ stool-along c camera-cross @ point {: x z :}
    c camera-look-y @ c camera-dist @ c camera-eye-y @  x c counter-y @ z  yaw  the-camera send camera-place ;
: the-folk ( -- id )  s" townsfolk" actor-named ;

\ ---- the steps ----
: find-stool ( -- seat | -1 )   \ the free stool he stands at, facing the counter (src: counter_find_seat)
    matthew-xz {: x z :}  x z across {: ac :}  x z along {: al :}
    matthew-now nip nip nip c facing @ - heading-mask and {: h :}
    ac c cross-hi @ >  ac c cross-lo @ <= or if  -1 exit  then
    c bar @ #stools 0 do
        c bar @ i seat-taken? 0= if
            i stool-along {: st :}
            c last-front-only? @  i c bar @ #stools 1- = and  al st >= and 0=
            al st stool-reach + <  al st stool-reach - >= and  and if
                h 0= if  i unloop exit  then
                c flip? @ if  al st <  else  st al <  then if  h three-quarters >= if  i  else  -1  then  unloop exit  then
                h quarter <= if  i  else  -1  then  unloop exit
            then
        then
    loop  -1 ;
: play-step ( -- done? )   \ his transition motion (src: counter_anim: the pad off meanwhile)
    anim-done? 0= if  false exit  then
    matthew-xz {: x z :}  x c counter-y @ z matthew-id send figure-place  true ;
: sit-down ( -- )   \ src: counter_sit
    matthew c bar @ seat @ the-folk send seat-take
    seat @ stool-along {: st :}  matthew-xz along {: al :}
    c flip? @ if  al st <=  else  al st <  then if  st sit-step -  0  else  st sit-step +  1  then {: at k :}
    c sit-motions k nth doing !
    false on-foot  counter-camera  2 matthew-id send figure-perch
    at c counter-y @ c stand-cross @ put
    doing @ false pose  matthew-id send figure-hold ;
: barman-walks ( -- arrived? )   \ src: counter_barman_walk
    walking-over @ 0= if
        c barman @  seat @ stool-along c barman-cross @ point  barman-pace barman-walk-motion  the-folk send person-walk
        true walking-over !  false exit
    then
    c barman @ walking? if  false exit  then
    0 walking-over !  seat @ barman-at !  c barman @ c barman-heading @ the-folk send person-turn  true ;
: neighbour ( -- slot | -1 )   \ who sits on the next stool the way he leans
    lean @ lean-right = if  seat @ c last-right @ = if  -1 exit  then  seat @ c right-step @ +
    else  seat @ c last-left @ = if  -1 exit  then  seat @ c right-step @ -  then {: n :}
    n 0<  n c bar @ #stools >= or if  -1 exit  then
    c bar @ n sitter  dup matthew = if  drop -1  then ;

: on-the-stool ( -- )   \ src: counter_seated
    step @ case
        0 of
            barman-at @ seat @ = if  c barman @  else  nobody-there  then talking-with !
            pad-right key-down? seat @ c no-lean-right @ <> and if
                c lean-in-motions 0 nth doing !  lean-right lean !  leaning mode !  0 step !  doing @ false pose  exit  then
            pad-left key-down? if
                c lean-in-motions 1 nth doing !  lean-left lean !  leaning mode !  0 step !  doing @ false pose  exit  then
            talking-with @ 0< 0= if
                stool-circle? if  talking-with @ talk-with exit  then
                stool-square? if  4 step !  then
            else
                stool-square? if  c beckon-motion @ doing !  doing @ false pose  1 step !  then
            then  endof
        1 of  play-step if  sit-still  2 step !  then  endof   \ the wave, then the barman comes over
        2 of  barman-walks if  0 step !  then  endof
        4 of  c order-xt @ execute if  0 step !  then  endof
    endcase ;
: leaning-over ( -- )   \ src: counter_lean
    step @ case
        0 of  play-step if  nobody-there talking-with !  c lean-motions side nth true pose  1 step !  then  endof
        1 of
            neighbour talking-with !
            lean @ lean-right = if  pad-right  else  pad-left  then  key-down? if
                talking-with @ 0< 0= stool-circle? and if  talking-with @ talk-with exit  then
                stool-square? if  getting-up mode !  0 step !  nobody-there talking-with !  then
                exit
            then
            c lean-out-motions side nth doing !  doing @ false pose  2 step !  endof
        2 of  play-step if  sit-still  0 step !  on-stool mode !  no-lean lean !  then  endof
    endcase ;
: up-and-off ( -- )   \ src: counter_stand
    step @ 0= if  c stand-motions side nth doing !  doing @ false pose  matthew-id send figure-hold  1 step !  exit  then
    play-step 0= if  exit  then
    matthew the-folk send seat-leave  true on-foot  the-camera send camera-follow  0 matthew-id send figure-perch
    forget-counter ;
: at-it ( -- )   \ src: counter_at
    mode @ case
        sitting-down of  sit-down  settling mode !  endof
        settling of  play-step if  sit-still  on-stool mode !  then  endof
        on-stool of  on-the-stool  endof
        leaning of  leaning-over  endof
        getting-up of  up-and-off  endof
    endcase ;
variable stool-square-was
: square-pressed? ( -- flag )  pad-square key-down? dup stool-square-was @ 0= and  swap stool-square-was ! ;
PRIVATE>

\ ---- an area's frame hook steps it (src: counter_update), with the bar's settings ----
: counter-step ( def -- )
    the-counter !
    at-state @ on-a-stool =  sat-in @ area @ <> and if  forget-counter  then   \ (elsewhere since: a warp)
    at-state @ off-stool = if
        find-stool seat !  square-pressed?  seat @ 0< 0= and if
            take-pad  on-a-stool at-state !  area @ sat-in !  sitting-down mode !  0 step !  pad-circle key-down? stool-circle-was !
        then  exit
    then
    take-pad  at-it ;
: seat-now ( def seat -- )   \ on that stool at once (src: counter_seat_now: a scene leaves him there)
    {: d n :}  d the-counter !  n seat !
    matthew c bar @ n the-folk send seat-take
    false on-foot  2 matthew-id send figure-perch  n stool-along c counter-y @ c seat-cross @ put  sit-still  counter-camera
    on-a-stool at-state !  area @ sat-in !  on-stool mode !  0 step !  no-lean lean ! ;
: counter-off ( -- )   \ off his stool at once (src: func_8010371C: the robbery)
    at-state @ on-a-stool = if  matthew the-folk send seat-leave  true on-foot  0 matthew-id send figure-perch  then
    forget-counter ;
