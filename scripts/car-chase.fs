\ car-chase.fs - the car chase in the town (docs/subsystems/car-chase.md; src: ovl027 func_801025E0's states 20 to
\ 26, carchase_timer_start, carchase_end, chase_lost; area 27's minigame 21). Matthew is put in his car; once he's
\ in, the bike sets off along its road and the camera sweeps from it to the car. Then the chase: the camera far
\ behind the car; coming within 1700 of the bike knocks it (his car slows to a sixteenth) - the bike shaken, it
\ speeds up a while - and the third knock brings it down: the crash, the rider falling, a fade, it's over (won). The
\ chase has 2700 frames: when they're up, or Matthew gets out of his car, they've got away. A step at the
\ original's rate (30 a second in the town).
IN: car-chase
USING: engine actors messages keys screen clock areas figures matthew townsfolk traffic driving events events.ovl027 sound rumble ;

variable chase-left   \ src: carchase_timer: frames before they get away (0: no chase)
: chase-on? ( -- flag )  chase-left @ 0<> ;

<PRIVATE
26 constant his-car   40 constant the-bike   $75 constant bike-road   430 constant bike-speed   530 constant bike-shaken
$70504 constant start-x   $649DA constant start-z   0 constant start-heading
2700 constant chase-frames
\ the sweep (src: state 22): 80 frames, from the bike toward the car, swinging out its first 32
80 constant sweep-frames   32 constant swinging   8 constant swing-step   $2000 constant swung   $1000 constant swing-mid
\ the chase's camera (src: state 23): above, far behind, its pivot ahead of the car
$600 negate constant chase-eye-y   $2000 negate constant chase-dist   -600 constant chase-look-y   4096 constant pivot-ahead
1700 constant bump-reach   3 constant knocks-to-win   150 constant shaken-frames
\ the crash (src: state 24)
30 constant crash-frames   $85 constant bike-slide   $100 constant rider-slide-turn   128 constant rider-slide
128 constant car-slide   $126 negate constant crash-look-y   $110 negate constant crash-eye-y   $700 negate constant crash-dist
$40 constant crash-turn
3 constant knock-small   255 constant knock-strength   1 constant knock-large   \ (src: rumble(0xFF, 3, 1))
$6E constant knock-sound   $69 constant crash-sound
$1660 constant rider-falls   $1666 constant rider-shaken   $1655 constant rider-riding
$1663 constant bike-wobbles   $1664 constant bike-falls   $1665 constant bike-down   \ (the bike's own: src actor_anim)
11 constant fade-out
$21 constant rider-after   $16 constant rider-got-away
$3C02C constant car-home-x   $74 negate constant car-home-y   $21933 constant car-home-z   $30 constant car-home-heading

enum: chase-stage  off  getting-in  sweeping  chasing  crashing  falling  fading ;
variable stage   variable timer   variable knocks
fvariable due

: car-held ( flag -- )  s" driving" actor-named send drive-hold ;
: angle ( dx dz -- a )  s>f s>f fswap fatan2 4096e f* 2e pi f* f/ f>s 4095 and ;
: along ( x z h d -- x' z' )
    {: x z h d :}  h s>f 2e f* pi f* 4096e f/ {: F: a :}
    x s>f a fsin d s>f f* f+ f>s  z s>f a fcos d s>f f* f+ f>s ;
: bike ( -- x z heading )  the-bike car-now nip ;   \ (its height: the road's)
: on-the-bike ( -- )   \ the rider on the bike, as it goes (src: its place - its height too - and its way)
    the-bike car-now {: x y z h :}  mel x y z the-townsfolk send extra-place  mel h the-townsfolk send person-turn ;
: apart ( -- d )   \ the bike from his car
    bike drop  his-car-now drop {: bx bz cx cz :}
    bx cx - s>f fdup f*  bz cz - s>f fdup f* f+ fsqrt f>s ;

\ ---- the cameras ----
: sweep-camera ( -- )   \ src: state 22: from the bike, toward the car, swinging out
    timer @ swinging < if  timer @ swing-step lshift  else  swung  then {: sw :}
    bike drop  his-car-now drop {: bx bz cx cz :}
    bx cx -  bz cz - sw swing-mid - - {: dx dz :}
    the-camera {: c :}  c send camera-fixed
    chase-look-y  dx s>f fdup f*  dz s>f fdup f* f+ fsqrt f>s negate  chase-eye-y
    bx the-bike car-now drop nip nip bz  dx dz angle  c send camera-place ;
: chase-camera ( -- )   \ src: state 23
    his-car-now {: x z h :}  the-camera {: c :}  c send camera-fixed
    chase-look-y chase-dist chase-eye-y  x z h pivot-ahead along {: px pz :}  px 0 pz  h  c send camera-place ;
: crash-camera ( -- )   \ src: state 24: on the rider, from the car's side
    mel person-now drop {: x y z :}  his-car-now drop {: cx cz :}
    the-camera {: c :}  c send camera-fixed
    crash-look-y crash-dist crash-eye-y  x y z  cx x - cz z - angle crash-turn - 4095 and  c send camera-place ;

\ ---- the stages ----
: off-it-goes ( -- )   \ src: state 21: he's in - the bike sets off, the sweep
    the-bike bike-road bike-speed s" traffic" actor-named send car-drive
    mel -1 0 the-townsfolk send person-set-stop   \ (the rider off their day while it lasts)
    mel rider-riding -1 true the-townsfolk send person-anim
    sweep-frames timer !  true car-held  sweeping stage ! ;
: knocked ( -- )   \ src: state 23's bump
    s" driving" actor-named send car-bumped  -1 knocks +!  knock-small knock-strength knock-large shake
    knocks @ if  knock-sound sfx
        shaken-frames timer !  the-bike bike-shaken s" traffic" actor-named send car-speed
        the-bike bike-wobbles -1 s" traffic" actor-named send car-anim
        mel rider-shaken rider-riding false the-townsfolk send person-anim  exit  then
    mel rider-falls -1 false the-townsfolk send person-anim  true car-held  crash-frames timer !
    the-bike bike-falls bike-down s" traffic" actor-named send car-anim
    the-bike s" traffic" actor-named send car-stop  crashing stage !  crash-sound sfx  -1 music-row ;
: chase-step ( -- )
    on-the-bike  chase-camera
    timer @ if  -1 timer +!  timer @ 0= if  the-bike bike-speed s" traffic" actor-named send car-speed  then  exit  then
    apart bump-reach < if  knocked  then ;
: crash-step ( -- )   \ the bike and the rider sliding on, the car too
    bike {: x z h :}  x z h bike-slide along {: nx nz :}
    the-bike nx  the-bike car-now drop nip nip  nz h s" traffic" actor-named send car-place
    mel person-now {: rx ry rz rh :}  mel  rx rz rh rider-slide-turn + rider-slide along  the-townsfolk send person-place
    his-car-now {: cx cz ch :}  cx cz ch car-slide along cz - swap cx - swap  s" driving" actor-named send car-nudge
    crash-camera  -1 timer +!  timer @ 0= if  falling stage !  then ;
: chase-over ( -- )  false car-held  the-camera send camera-follow  off stage !  minigame-over ;
: chase-frame ( -- )
    stage @ case
        getting-in of  driving? if  off-it-goes  then  endof
        sweeping of  on-the-bike  sweep-camera  -1 timer +!  timer @ 0= if  knocks-to-win knocks !  false car-held  chasing stage !  then  endof
        chasing of  chase-step  endof
        crashing of  crash-step  endof
        falling of  crash-camera  mel person-anim-done? if  fade-out s" effects" actor-named send screen-effect  fading stage !  then  endof
        fading of  crash-camera  effects:effect-running? 0= if  chase-over  then  endof
    endcase ;
: got-away ( -- )   \ src: chase_lost: their line, the chase over, the rider back to their day
    ['] entry-8010B660 start-event  0 chase-left !  off stage !  minigame-over  false car-held
    the-bike s" traffic" actor-named send car-stop  mel rider-got-away the-townsfolk send person-goto-stop ;
: clock-step ( -- )   \ src: area 27's frame: the chase's time running out
    chase-on? 0= if  exit  then  -1 chase-left +!  chase-left @ 0= if  got-away  then ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: chase-tick ( -- )  steps-due 0 ?do  clock-step  stage @ off <> if  chase-frame  then  loop ;

: begin-chase ( -- )   \ minigame 21 (src: state 20): his car where it starts, him put in it; the bike on its road
    his-car start-x 0 start-z start-heading s" traffic" actor-named send car-place
    start-x 0 start-z s" matthew" actor-named send figure-place
    the-bike bike-road 0 s" traffic" actor-named send car-drive   \ (at its road's start, standing)
    put-in-his-car  0e due f!  0 timer !  getting-in stage !  minigame-begun ;
' begin-chase 21 minigame-code!
: abort ( -- )  off stage !  minigame-over  false car-held ;   \ minigame 3 (src: area_minigame_start's 3)
' abort 3 minigame-code!
: start-clock ( -- )  chase-frames chase-left ! ;   \ ovl027 0x80106950 (src: carchase_timer_start)
27 ovl $80106950 ' start-clock native:
: day ( -- k )  clock-time time>day 28 - 0 max 2 min ;
: after-stop ( who k -- )  3 * day + {: k :}  s" chase-after-stops" k exe-u8 the-townsfolk send person-goto-stop ;
: won ( -- )   \ ovl027 0x801060BC (src: carchase_end): unless they got away - out of the car, on to the station
    chase-on? 0= if  exit  then  0 chase-left !
    step-out  ['] entry-8010B678 start-event
    mel rider-after the-townsfolk send person-goto-stop
    dennis 0 after-stop  wolf 1 after-stop  rolein 2 after-stop
    his-car car-home-x car-home-y car-home-z car-home-heading s" traffic" actor-named send car-place ;
27 ovl $801060BC ' won native:
PRIVATE>

state: car-chase-state end-state
behaviour car-chases
  on spawned ( -- )  self subscribe tick  self subscribe left-the-car  self subscribe area-entered ;
  on tick ( -- )  chase-tick ;
  on left-the-car ( -- )  chase-on? if  got-away  then ;   \ src: func_80106980
  on area-entered ( area scene -- )  drop the-town <> if  stage @ off <> if  abort  then  then ;
end-behaviour

: spawn-car-chase ( -- id )  car-chases car-chase-state s" car-chase" spawn ;
