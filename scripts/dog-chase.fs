\ dog-chase.fs - running from the junkyard's dog (docs/subsystems/dog-chase.md; src: ovl008 dog_reset /
\ dog_update). Matthew runs where the d-pad points (the area's table of headings), 60 a frame, turning to face it
\ 0x200 a frame; walls stop him. The dog waits, turns toward him (0x100 a frame) and, lined up, charges (1 in 4,
\ or when he's near) - turning 0x30 a frame, faster each frame to 150; off his line by 0x300 it skids to a stop
\ and turns again. Its bite: 30 of his 120 health, he staggers, it backs off and turns away. Out of health: he
\ falls, game over, and ○ or Start runs it from the start. The camera looks down on him. A step at the
\ original's rate (15 a second indoors); the scene's script decides when he's got away.
IN: dog-chase
USING: engine actors messages keys screen areas figures floors matthew townsfolk sound ;

variable chasing
: chasing? ( -- flag )  chasing @ 0<> ;

<PRIVATE
157 constant the-dog   120 constant full-health   30 constant bite-damage   60 constant his-stride
$19F58 constant his-start-x   $336FA constant his-start-z   $1A312 constant dog-start-x   $339DC constant dog-start-z
$A00 constant dog-start-heading   10 constant first-wait
$76D constant dog-ready   $76E constant dog-charging   $773 constant dog-backing   $772 constant staggering
$6C constant fallen   5 constant running   0 constant stands-still
$100 constant dog-turn   $30 constant charge-turn   $300 constant off-line   $200 constant his-turn
50 constant speed-step   150 constant top-speed   25 constant slowest   $200 constant near   $80 constant bite-reach
17 constant hurt-frames   8 constant after-bite-wait   $400 constant turns-away   $100 constant apart-after-bite
\ the camera (src: dog_update: over him)
$200 negate constant chase-look-y   $DE2 negate constant chase-dist   $A00 negate constant chase-eye-y
\ the screen's effects (src: screen_effect)
10 constant fade-in   14 constant bite-flash   9 constant falling   2 constant game-over-in   3 constant fade-out
-140 constant bar-x   -80 constant bar-y
$72 constant bite-sound   $A constant chase-music   \ (the music table's row)

enum: dog-stage  waiting  charging  skidding  backing-off ;
enum: his-stage  free  staggered  hurt  down  game-over  retrying ;
variable dog   variable him   variable health   variable shown   variable wait   variable dog-speed
variable hurt-left   variable facing   variable facing-goal
variable his-x   variable his-z   variable dog-x   variable dog-z   variable dog-heading
variable his-motion   variable dog-motion   variable shown-his   variable shown-dog
fvariable due

\ ---- turning, distances (src: heading_toward, actor_step) ----
: toward ( h target step -- h' )   \ by at most step
    {: h t s :}  h t = if  t exit  then  t h - 4095 and {: d :}
    d s <  4096 s - d < or if  t exit  then
    d 2048 >= if  h s -  else  h s +  then  4095 and ;
: angle-to-him ( -- a )   \ from the dog
    his-x @ dog-x @ - s>f  his-z @ dog-z @ - s>f  fatan2 4096e f* 2e pi f* f/ f>s 4095 and ;
: apart ( -- d )  his-x @ dog-x @ - s>f fdup f*  his-z @ dog-z @ - s>f fdup f* f+ fsqrt f>s ;
: along ( x z h d -- x' z' )   \ d along heading h
    {: x z h d :}  h s>f 2e f* pi f* 4096e f/ {: F: a :}
    x s>f a fsin d s>f f* f+ f>s  z s>f a fcos d s>f f* f+ f>s ;
: effect ( n -- )  s" effects" actor-named send screen-effect ;

\ ---- the dog ----
: bitten ( -- )   \ (the pad's rumble: to come)
    bite-sound sfx  bite-flash effect  bite-damage negate health +!
    health @ 0 <= if  down him !  falling effect  else  staggered him !  then
    backing-off dog !  hurt-frames wait !  0 dog-speed !
    angle-to-him {: a :}  a dog-heading !   \ src: actor_place_apart: it's put that far from him, facing him
    his-x @ his-z @ a apart-after-bite negate along  dog-z !  dog-x ! ;
: lined-up? ( a step -- flag )  {: a s :}  dog-heading @ a s toward a = ;
: wait-and-turn ( a -- )   \ src: state 0
    {: a :}  50 dog-speed !  dog-ready dog-motion !
    wait @ if  -1 wait +! exit  then
    dog-heading @ a dog-turn toward dog-heading !
    a dog-turn lined-up?  4 random 0=  apart near < or and if  charging dog !  then ;
: charge ( a -- )   \ src: state 1
    {: a :}  dog-charging dog-motion !
    dog-heading @ a charge-turn toward dog-heading !
    dog-speed @ top-speed < if  speed-step dog-speed +!  then
    a off-line lined-up? 0= if  skidding dog !  then
    him @ free =  apart bite-reach < and if  bitten  then ;
: skid ( -- )   \ src: state 2
    dog-ready dog-motion !
    dog-speed @ slowest > if  speed-step negate dog-speed +!  dog-speed @ 0= if  slowest dog-speed !  then  exit  then
    him @ free = if  waiting dog !  then ;
: back-off ( -- )   \ src: state 4
    dog-backing dog-motion !  0 dog-speed !  -1 wait +!
    wait @ if  exit  then
    dog-ready dog-motion !  waiting dog !  after-bite-wait wait !  dog-heading @ turns-away + 4095 and dog-heading ! ;
: dog-frame ( -- )
    angle-to-him {: a :}
    dog @ case
        waiting of  a wait-and-turn  endof
        charging of  a charge  endof
        skidding of  skid  endof
        backing-off of  back-off  endof
    endcase
    dog-x @ dog-z @ dog-heading @ dog-speed @ along  dog-z !  dog-x ! ;

\ ---- him ----
: pad-bits ( -- n )   \ Up 1, Right 2, Down 4, Left 8 (the table's index)
    pad-up key-down? 1 and  pad-right key-down? 2 and or  pad-down key-down? 4 and or  pad-left key-down? 8 and or ;
: run-toward ( bits -- )   \ that way at once; he turns to face it
    {: b :}  s" dog-directions" b exe-s16 4095 and facing-goal !
    his-x @ his-z @ facing-goal @ his-stride along  his-z !  his-x ! ;
: run ( -- )
    pad-bits ?dup if  run-toward running  else  stands-still  then  his-motion !
    facing @ facing-goal @ his-turn toward facing ! ;
variable confirm-was
: try-again? ( -- flag )   \ ○ or Start, newly
    pad-circle key-down?  pad-start key-down? or {: d :}  d confirm-was @ 0= and  d confirm-was ! ;
: reset ( -- )   \ src: dog_reset: the chase's music again
    full-health health !  full-health shown !  waiting dog !  free him !  0 facing !  0 facing-goal !
    first-wait wait !  0 dog-speed !  his-start-x his-x !  his-start-z his-z !  dog-start-x dog-x !  dog-start-z dog-z !
    the-dog dog-start-x 0 dog-start-z s" townsfolk" actor-named send extra-place   \ (in the yard)
    dog-start-heading dog-heading !  -1 shown-his !  -1 shown-dog !  dog-ready dog-motion !  stands-still his-motion !
    fade-in effect  chase-music music-row ;
: fall ( -- )   \ src: state 3: once he's down, game over
    fallen his-motion !  effects:effect-running? if  exit  then
    game-over him !  0 health !  game-over-in effect  -1 music-row ;
: his-frame ( -- )
    his-x @ his-z @ {: x z :}
    him @ case
        free of  run  endof
        staggered of  hurt-frames hurt-left !  hurt him !  staggering his-motion !  endof
        hurt of  -1 hurt-left +!  hurt-left @ 0= if  free him !  then  endof
        down of  fall  endof
        game-over of  try-again? if  retrying him !  fade-out effect  then  endof
        retrying of  effects:effect-running? 0= if  reset  then  endof
    endcase
    his-x @ his-z @ floor @ blocked? if  x his-x !  z his-z !  then ;

\ ---- showing them: where they are, their motions (started when they change), the camera ----
: show-him ( -- )
    his-x @ 0 his-z @ s" matthew" actor-named send figure-place  0 facing @ 0 s" matthew" actor-named send figure-turn
    his-motion @ shown-his @ = if  exit  then  his-motion @ dup shown-his !  -1 true s" matthew" actor-named send figure-anim ;
: show-dog ( -- )
    the-dog dog-x @ dog-z @ s" townsfolk" actor-named send person-place
    the-dog dog-heading @ s" townsfolk" actor-named send person-turn
    dog-motion @ shown-dog @ = if  exit  then
    dog-motion @ dup shown-dog !  the-dog swap -1 true s" townsfolk" actor-named send person-anim ;
: aim ( -- )
    s" camera" actor-named {: c :}  c send camera-fixed
    chase-look-y chase-dist chase-eye-y  his-x @ 0 his-z @  0  c send camera-place ;
: chase-frame ( -- )
    him @ game-over < if  dog-frame  then
    his-frame  show-him  show-dog  aim
    health @ shown @ < if  -1 shown +!  then ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: chase-tick ( -- )  chasing? 0= if  exit  then  steps-due 0 ?do  chase-frame  loop ;

\ ---- drawing: his health, the game over ----
: draw-chase ( -- )
    chasing? 0= if  exit  then
    him @ game-over < 0= if  draw-game-over exit  then
    bar-x bar-y shown @ full-health health-bar ;
' draw-chase on-draw
PRIVATE>

state: dog-chase-state end-state
behaviour dog-chases
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  chase-tick ;
  on dog-chase-start ( -- )  true chasing !  0e due f!  reset ;
  on dog-chase-stop ( -- )  false chasing !  s" camera" actor-named send camera-follow ;
end-behaviour

: spawn-dog-chase ( -- id )  dog-chases dog-chase-state s" dog-chase" spawn ;
