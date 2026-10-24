\ ring.fs - the item ring (docs/subsystems/ring.md; src: ring.c, cui.c ui_update, fhbook.c item_use): △ with
\ Matthew the player's opens it over the field, and the world stands still (ui.fs). His items stand on a ring,
\ left and right turn it, the one in front named below. Two lists: the tools (the phone, the notebook, the map,
\ the lighter, the diary) and, behind the waist bag, the key items - ○ reads one (a few are used at once), □ shows
\ it to the person he faces when they would answer to it (showing.fs). ✕ goes back, or closes. Chosen: the
\ phone, the notebook, the map (theirs: ring-chosen), the diary, or the item used.
\ Its state is its own; others read the fact ring-up?.
IN: ring
USING: engine actors messages keys screen clock areas figures matthew window items talk showing ui driving
    shootout story-flags events events.main ;

variable ring-open
: ring-up? ( -- flag )  ring-open @ 0<> ;
variable ring-act-prompt   variable ring-look-prompt   \ (facts: the HUD's prompts while it's up; -1 none)

<PRIVATE
\ ---- the items: the executable's table, in the ring's order (src: D_80095B48) ----
23 constant #rows
1 constant key-item   2 constant opens-key-items   4 constant also-key-item   \ a row's flags
: row-item ( row -- item )  s" ring-items" rot 6 * exe-u8 ;
: row-picture ( row -- k )  s" ring-items" rot 6 * 1+ exe-u8 ;
: row-name ( row -- msg )  s" ring-items" rot 3 * 1+ exe-s16 ;
: row-flags ( row -- flags )  s" ring-items" rot 3 * 2 + exe-s16 ;
63 5 * 3 + constant item-text   \ the items' text (CD.BIN 063_3: a name, its description the next)
0 constant nothing-there        \ "It's empty."
\ used at once from the key items, not read (src: ring_menu_input)
16 constant mels-note   \ (the photo and Kathy's note: showing.fs's)
\ the tools that are screens of their own, and the diary
1 constant notebook   2 constant town-map   14 constant emmas-diary
-1 constant none

\ ---- its state ----
create tools #rows cells allot   variable #tools   \ the rows held, each list
create keys #rows cells allot    variable #keys
variable key-list?    \ showing the key items
variable sel          \ the one in front
variable turn         \ game frames of a turn left (0: still)
variable turn-way     \ 1 turning right (the next), -1 left
variable reading      \ an item's description up
variable name-msg     \ the name shown (-1: none, while turning)
variable answer       \ what the person faced would answer to the item in front (0: nothing)
variable faced        \ who
variable controlled
fvariable due
variable circle-was   variable cross-was   variable square-was   variable triangle-was
4 constant turn-frames   \ src: ring_menu_rotate: a quarter of the step a frame

: list ( -- addr n )  key-list? @ if  keys #keys @  else  tools #tools @  then ;
: #shown ( -- n )  list nip ;
: row-now ( -- row )  list drop sel @ cells + @ ;
: item-now ( -- item )  row-now row-item ;
: empty? ( -- flag )  #shown 0= ;
: name-now ( -- )  empty? if  nothing-there  else  row-now row-name  then  name-msg ! ;

: gather ( -- )   \ src: ring_menu_open: the held items into their lists
    0 #tools !  0 #keys !
    #rows 0 do
        i row-item has-item? if
            i row-flags key-item also-key-item or and if
                i keys #keys @ cells + !  1 #keys +!
            else  i tools #tools @ cells + !  1 #tools +!  then
        then
    loop ;

\ ---- the buttons ----
: edge ( key var -- pressed? )  {: k v :}  k key-down? dup v @ 0= and  swap v ! ;
variable circle   variable cross   variable square   variable triangle
: buttons ( -- )   \ this step's presses (edges kept every step, whoever has the pad)
    pad-circle circle-was edge circle !  pad-cross cross-was edge cross !
    pad-square square-was edge square !  pad-triangle triangle-was edge triangle ! ;

: the-items ( -- id )  s" items" actor-named ;
: unlock ( -- )  s" ui" actor-named send ui-unlock ;
: open-ring ( -- )   \ src: ring_menu_update state 0
    gather  #tools @ #keys @ + 0= if  exit  then
    s" sound" actor-named send stop-stream
    matthew-actor send figure-idle
    s" ui" actor-named send ui-lock
    true ring-open !  false key-list? !  0 sel !  0 turn !  false reading !   \ (src: always the first: SAVED stays 0)
    0 answer !  -1 faced !  name-now  0e due f! ;
: show-list ( key? -- )   \ src: ring.c show_list (each list from its first)
    key-list? !  0 sel !  0 turn !  0 answer !  name-now ;
: chosen ( item who -- )   \ closed on an item (-1 none), shown to someone (-1: for use)
    false ring-open !  unlock  broadcast ring-chosen ;

\ ---- choosing (src: cui.c ui_update, fhbook.c item_use) ----
: use-mels-note ( -- )   \ (the diary's 23rd read: the first time, its own)
    read-diary-dec23 flag?  motel-clue-realized flag? 0= and if
        motel-clue-realized flag-set  ['] events.main:exe-8009AA84 start-event
    else  ['] events.main:exe-8009AA60 start-event  then ;
: use ( item -- )
    case
        emmas-diary of  ['] events.main:exe-8009AA14 start-event  endof
        kathys-note of  ['] events.main:exe-8009AAA8 start-event  endof
        mels-note of  use-mels-note  endof
        no-entry-photo of  ['] events.main:exe-8009AACC start-event  endof
    endcase ;
: dispatch ( item who -- )   \ (the phone, the notebook, the map: their screens hear ring-chosen)
    {: it who :}  who 0< 0= if  who answer @ show-to exit  then
    it none = if  exit  then  it use ;
: close-on ( item who -- )  2dup chosen dispatch ;

\ ---- a step ----
: faced-answer ( -- )   \ src: ring_menu_input: in the key items, what the person faced would answer
    0 answer !  -1 faced !
    key-list? @ 0=  empty? or if  exit  then
    in-front dup faced !  dup 0< if  drop exit  then  item-now answer-to answer ! ;
$13 constant show-prompt   3 constant read-prompt   \ src: func_80048500 / func_8004850C: □ shows, ○ reads
: ring-prompts ( -- )   \ (src: ○'s whenever there's an item; □'s for a person who'd answer)
    answer @ if  show-prompt ring-act-prompt !  then
    empty? 0= if  read-prompt ring-look-prompt !  then ;
: start-turn ( way -- )
    dup turn-way !  sel @ + #shown + #shown mod sel !  turn-frames turn !  -1 name-msg ! ;
: turn-step ( -- )   \ src: ring_menu_rotate, a game frame
    turn @ if  -1 turn +!  turn @ 0= if  name-now  then  exit  then
    #shown 1 > 0= if  exit  then
    pad-left key-down? if  -1 start-turn exit  then
    pad-right key-down? if  1 start-turn  then ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: read-it ( -- )   \ its description, in the message window
    item-text row-now row-name 1+ nobody 0 0 s" window" actor-named send window-say  true reading !  -1 name-msg ! ;
: circle-key-item ( -- )
    item-now dup no-entry-photo = over mels-note = or over kathys-note = or if  none close-on exit  then
    drop read-it ;
: circle-tool ( -- )
    row-now row-flags opens-key-items and if
        #keys @ if  true show-list  else  nothing-there name-msg !  then  exit  then
    item-now none close-on ;
: inputs ( -- )   \ src: ring_menu_input
    -1 ring-act-prompt !  -1 ring-look-prompt !
    turn @ if  exit  then
    reading @ if  window-up? 0= if  false reading !  name-now  then  exit  then
    faced-answer  ring-prompts
    square @ answer @ 0<> and if  item-now faced @ close-on exit  then
    circle @ if
        empty? if  exit  then
        key-list? @ if  circle-key-item  else  circle-tool  then  exit
    then
    cross @ if  key-list? @ if  false show-list  else  none none close-on  then  then ;
: may-open? ( -- flag )   \ src: pad_update's menu_allowed, field_actions; cui.c ui_update
    controlled @ 0=  field-pad? 0= or  window-up? or  matthew-there? 0= or if  false exit  then
    prologue-done flag? 0= if  false exit  then
    area @ 5 = if  false exit  then   \ (the shoot-out's town square)
    area @ 26 =  shooting? and if  false exit  then
    driving? if  nearly-still? exit  then
    pad-up key-down? pad-down key-down? or  pad-left key-down? or  pad-right key-down? or
    pad-circle key-down? or  pad-cross key-down? or  pad-square key-down? or  0= ;
: ring-step ( -- )
    buttons
    ring-up? 0= if  triangle @ may-open? and if  open-ring  then  exit  then
    steps-due 0 ?do  turn-step  loop
    inputs ;

\ ---- drawing (the original's screen; the ring's view from (0, -500, -1000) toward the ring's centre, at 400) ----
400e fconstant ring-radius   400e fconstant ring-z   96e fconstant card-half   400e fconstant projection
500e fconstant eye-up   1000e fconstant eye-back
: project ( F: x y z -- F: sx sy )   \ seen from above and behind, looking down at the origin
    {: F: x F: y F: z :}  y eye-up f+ {: F: h :}  z eye-back f+ {: F: d :}
    h eye-up f*  d eye-back f* f+ {: F: depth :}   \ (the depth times the eye's distance)
    eye-up eye-up f* eye-back eye-back f* f+ fsqrt {: F: reach :}
    projection reach f* x f* depth f/
    projection  h eye-back f*  d eye-up f* f-  f* depth f/ ;
: on-screen ( F: sx sy -- x y )  fswap f>s f>s >screen ;
4096 constant full-turn
: angle-of ( k -- F: radians )  s>f full-turn s>f f/ 2e f* pi f* ;
: ring-angle ( -- a )   \ src: ring_menu_rotate: the one in front at a quarter turn, a turn's quarter steps
    full-turn #shown 1 max / {: step :}
    step sel @ * full-turn 4 / +  step 4 / turn @ * turn-way @ * - ;
: card-place ( k -- F: x z )   \ the k-th card, the ring turned
    full-turn #shown / * angle-of ring-angle angle-of {: F: a F: t :}
    a fcos ring-radius f* a fsin ring-radius f* {: F: x F: z :}
    t fcos x f*  t fsin z f* f+   t fsin fnegate x f*  t fcos z f* f+  ring-z f+ ;
create pictures #rows cells allot   pictures #rows cells erase
: picture ( row -- image )   \ items/<k>.png, loaded once
    row-picture {: k :}  pictures k cells + @ ?dup if  exit  then
    $clear  s" items/" $+  k n>str $+  s" .png" $+  $@ image  dup pictures k cells + ! ;
: card ( k -- )
    dup list drop swap cells + @ picture {: k im :}  k card-place {: F: x F: z :}
    im  x card-half f- card-half fnegate z project on-screen  x card-half f+ card-half fnegate z project on-screen
    x card-half f- card-half z project on-screen  x card-half f+ card-half z project on-screen  draw-image-quad ;
variable drawn   \ the cards drawn so far (a bit each)
: card-z ( k -- F: z )  card-place fswap fdrop ;
: drawn? ( k -- flag )  1 swap lshift drawn @ and 0<> ;
: farthest ( -- k )   \ of those not yet drawn
    -1 -1e30 {: F: far :}  #shown 0 ?do
        i drawn? 0= if  i card-z  fdup far f> if  to far  drop i  else  fdrop  then  then
    loop ;
: cards ( -- )  $FFFFFFFF pen-color  0 drawn !  #shown 0 ?do  farthest  1 over lshift drawn @ or drawn !  card  loop ;

$00000080 constant backdrop-color   $C8AA6480 constant bar-color   \ src: func_8003D6E0: half see-through
38 constant bar-top   56 constant bar-bottom   89 constant bar-tall-bottom
-24 constant title-x   -60 constant title-y   48 constant title-w   16 constant title-h
96 constant hint-x   80 constant hint-y   16 constant button-w   40 constant hint-w
-152 constant name-left   304 constant name-w   40 constant name-y
variable title-tools   variable title-items   variable exit-label   variable cross-button
: ui-image ( var addr len -- image )   \ ui/<name>.png, loaded once
    {: v a n :}  v @ ?dup if  exit  then  $clear  s" ui/" $+  a n $+  s" .png" $+  $@ image  dup v ! ;
: stripe ( color top bottom -- )  {: c t b :}  c pen-color  screen-left t >screen  screen-w scale *  b t - scale *  draw-rect ;
: draw-name ( -- )
    name-msg @ 0< if  exit  then
    0 font  0 font-palette  $FFFFFFFF pen-color  scale pen-scale
    item-text name-msg @ text-of  2dup font-width {: w :}  name-left name-w w - 2/ + name-y >screen font-text ;
: draw-ring ( -- )
    ring-up? 0= if  exit  then
    backdrop-color screen-top screen-bottom stripe
    cards
    bar-color bar-top  reading @ if  bar-tall-bottom  else  bar-bottom  then  stripe
    $FFFFFFFF pen-color
    key-list? @ if  title-items s" ring-items"  else  title-tools s" ring-tools"  then  ui-image
    title-x title-y title-w title-h sprite
    reading @ 0= if
        cross-button s" button-cross" ui-image hint-x hint-y button-w button-w sprite
        exit-label s" exit" ui-image hint-x button-w + hint-y hint-w title-h sprite
    then
    draw-name ;
' draw-ring on-draw
PRIVATE>

state: ring-state end-state
behaviour the-ring
  on spawned ( -- )  self subscribe tick  self subscribe player-control ;
  on tick ( -- )  ring-step ;
  on player-control ( flag -- )  controlled ! ;
end-behaviour

: spawn-ring ( -- id )  the-ring ring-state s" ring" spawn ;
