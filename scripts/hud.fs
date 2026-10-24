\ hud.fs - what the field shows over the picture while Matthew is the player's (docs/subsystems/hud.md; src: hud.c,
\ cui.c func_800485AC, field.c field_update): the two prompts top right - □'s frame and ○'s, each with the icon
\ of what that button would do here (look.fs's facts; the ring's while it's up), animated, dimmed with nothing
\ to do; the clock top left - the date, the weekday, the time - while it runs; and in the town the corner map,
\ seen with his heading, his mark on it. The notebook and the map hide it. It only draws.
IN: hud
USING: engine actors messages screen clock areas figures matthew driving look ui ring notebook map events boat ;

<PRIVATE
: ui-picture ( var addr len -- image )   \ ui/<name>.png, loaded once
    {: v a n :}  v @ ?dup if  exit  then  $clear  s" ui/" $+  a n $+  s" .png" $+  $@ image  dup v ! ;
: part ( image u v w h x y -- )   \ texels (u, v, w, h) at the original's (x, y), their size, opaque
    {: im u v w h x y :}  blend-ps-solid pen-blend  im u v w h  x y >screen  w scale *  h scale *  draw-image-part
    blend-usual pen-blend ;
2 constant on-a-stool   \ (his perch)
: hud-on? ( -- flag )  has-control?  notebook-up? 0= and  map-up? 0= and ;   \ (src: D_800A3528: they hide it)

\ ---- the prompts (src: prompt_draw, big: the field's place) ----
variable frames-image   variable icons-image
20 constant act-x   60 constant look-x   -92 constant prompts-y   48 constant frame-w   24 constant frame-h
-56 constant asked-act-x   4 constant asked-look-x   -16 constant asked-y   \ (src: prompt_draw's beside the box)
24 constant icon-in   4 constant icon-down   16 constant icon-size   16 constant second-set   48 constant second-set-v
3 constant icon-frames   8 constant frames-an-icon-frame
variable icon-frame   variable icon-count   fvariable due
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: animate ( -- )   \ src: D_800A3428: the next icon frame every 8 game frames
    steps-due 0 ?do
        1 icon-count +!  icon-count @ frames-an-icon-frame >= if
            0 icon-count !  icon-frame @ 1+ icon-frames mod icon-frame !  then
    loop ;
: prompt-frame-at ( id x u y -- )   \ a prompt's frame (dimmed, half see-through, with nothing to do) and its icon
    {: id x u y :}
    id 0< if  blend-ps-half pen-blend  frames-image s" prompt-frames" ui-picture u 0 frame-w frame-h
        x y >screen  frame-w scale *  frame-h scale *  draw-image-part  blend-usual pen-blend exit  then
    frames-image s" prompt-frames" ui-picture u 0 frame-w frame-h x y part
    icons-image s" prompt-icons" ui-picture  id icon-size * 255 and
    id second-set < 0= second-set-v and  icon-frame @ icon-size * +  icon-size icon-size
    x icon-in + y icon-down + part ;
: prompt-frame ( id x u -- )  prompts-y prompt-frame-at ;
: the-prompts ( -- act look )
    ring-up? if  ring-act-prompt @ ring-look-prompt @ exit  then
    rowing? if  -1 boat-prompt @ exit  then   \ (src: the boathouses' ○)
    act-prompt @ look-prompt @ ;
: draw-prompts ( -- )  the-prompts {: a l :}  a act-x 0 prompt-frame  l look-x frame-w prompt-frame ;

\ ---- the clock (src: func_800157B4: from the sheet's v $80) ----
variable clock-image
-144 constant month-x   -92 constant date-y   -112 constant day-x   -80 constant weekday-x
-144 constant hour-x   -76 constant time-y   -112 constant colon-x   -104 constant minute-x   -72 constant am-x
0 constant big-digits   16 constant small-digits   32 constant weekdays   48 constant words
16 constant digit-w   24 constant weekday-w   8 constant bracket-w   32 constant month-w
$A0 constant open-bracket   $A8 constant close-bracket   $A8 constant colon-u   $78 constant am-u   $90 constant pm-u
: clock-part ( u v w h x y -- )  {: u v w h x y :}  clock-image s" clock" ui-picture u v w h x y part ;
: digit ( n row x y -- )  {: n row x y :}  n digit-w * row digit-w digit-w x y clock-part ;
: two-digits ( n row x y -- )  {: n row x y :}  n 10 / row x y digit  n 10 mod row x digit-w + y digit ;
: draw-clock ( -- )
    clock-time time>date {: day hour minute :}  hour 12 mod {: h12 :}
    0 words month-w digit-w month-x date-y clock-part   \ "Dec."
    day big-digits day-x date-y two-digits
    open-bracket big-digits bracket-w digit-w weekday-x date-y clock-part
    day first-day - weekday-w * weekdays weekday-w digit-w weekday-x bracket-w + date-y clock-part
    close-bracket big-digits bracket-w digit-w weekday-x bracket-w + weekday-w + date-y clock-part
    h12 10 < if  h12 small-digits hour-x digit-w + time-y digit  else  h12 small-digits hour-x time-y two-digits  then
    colon-u weekdays bracket-w digit-w colon-x time-y clock-part
    minute small-digits minute-x time-y two-digits
    hour 12 < if  am-u  else  pm-u  then  words weekday-w digit-w am-x time-y clock-part ;

\ ---- the corner map, in the town (src: func_800152D8) ----
variable map-image
80 constant corner-x   30 constant corner-y   64 constant corner-size   \ (its window: src r_clip 240, 150)
112 constant corner-mid-x   64 constant corner-mid-y   \ (src: (84, 48) at a depth of 300, seen at 400)
40 constant window-half   4e 3e f/ fconstant seen-at
2500 constant units-across   2000 constant units-down   8 constant map-left   248 constant map-bottom
40 constant near-edge   215 constant far-right   199 constant far-down   3 constant mark-size   $800000FF constant mark-ink
: clamp ( n lo hi -- n' )  rot min max ;
fvariable sin-a   fvariable cos-a
: seen ( x y -- sx sy )   \ a point of the window, turned and seen, in window pixels
    {: x y :}  x s>f cos-a f@ f*  y s>f sin-a f@ f* f-  seen-at f* f>s corner-mid-x +
    x s>f sin-a f@ f*  y s>f cos-a f@ f* f+  seen-at f* f>s corner-mid-y +  >screen ;
: where-he-is ( -- x z heading )   \ (rowing: the boat's way, he faces back - src: the map turned round)
    driving? if  his-car-now exit  then  matthew-now {: x y z h :}  x z  h rowing? if  2048 +  then ;
: draw-corner ( -- )
    where-he-is {: x z h :}
    x units-across / map-left +  map-bottom z units-down / -  {: px py :}
    px near-edge far-right clamp  py near-edge far-down clamp  {: cx cy :}
    h 4095 and negate s>f 4096e f/ 2e f* pi f*  fdup fsin sin-a f!  fcos cos-a f!
    corner-x corner-y >screen  corner-size scale *  dup  clip-2d
    blend-ps-half pen-blend  $FFFFFFFF pen-color
    map-image s" map" ui-picture  cx window-half - cy window-half - window-half 2* dup
    window-half negate dup seen  window-half window-half negate seen  window-half negate window-half seen
    window-half window-half seen  draw-image-quad-part
    blend-usual pen-blend
    px cx - py cy - seen {: mx my :}  mark-size scale * {: k :}
    mx my k -  mx k - my k +  mx k + my k +  mx k + my k +  mark-ink mark-ink mark-ink mark-ink draw-quad
    0 0 0 0 clip-2d ;

: draw-asked ( -- )   \ src: the VM's prompt op: beside the box
    $FFFFFFFF pen-color  prompt-left @ asked-act-x 0 asked-y prompt-frame-at  prompt-right @ asked-look-x frame-w asked-y prompt-frame-at ;
: draw-hud ( -- )
    prompt-shown? if  draw-asked  then
    hud-on? 0= if  exit  then
    $FFFFFFFF pen-color  draw-prompts
    ui-busy? if  exit  then   \ (src: field_update: the clock and the map with the field running)
    clock-running? if  draw-clock  then
    area @ the-town =  matthew-there? and  matthew-perch on-a-stool <> and if  draw-corner  then ;
' draw-hud on-draw
PRIVATE>

state: hud-state end-state
behaviour the-hud
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  animate ;
end-behaviour

: spawn-hud ( -- id )  the-hud hud-state s" hud" spawn ;
