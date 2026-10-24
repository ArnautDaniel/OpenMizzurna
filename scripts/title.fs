\ title.fs - the title (docs/subsystems/title.md; src: opening.c, OPENING.EXE): the logo, the attract movies, the
\ title and its menu - New Game (the opening movie, then the game), Load (the saves' slots), Adjust Screen,
\ Vibration on / off - and, coming back from an ending, its movie and closing card. It runs at OPENING's 60
\ frames a second (two a step). Others read the fact title-up?.
IN: title
USING: engine actors messages keys screen story-flags settings saves events ;

variable title-open
: title-up? ( -- flag )  title-open @ 0<> ;

<PRIVATE
\ ---- the pictures (title/*.png: the converter's, from CD.BIN 034_2) ----
: picture ( addr len -- image )  $clear  s" title/" $+  $+  s" .png" $+  $@ image ;
: at-top-left ( x y -- x' y' )  120 - swap 160 - swap ;   \ (OPENING's screen from its top left)
: draw ( image x y w h -- )   \ at OPENING's place, opaque
    {: im x y w h :}  blend-ps-solid pen-blend  im  x y at-top-left >screen  w scale *  h scale *  draw-image
    blend-usual pen-blend ;
: draw-added ( image x y w h -- )   \ a semi-transparent sprite (its texels marked to blend: added)
    {: im x y w h :}  blend-ps-add pen-blend  im  x y at-top-left >screen  w scale *  h scale *  draw-image
    blend-usual pen-blend ;
: draw-part ( image u v w h x y -- )  {: im u v w h x y :}
    blend-ps-add pen-blend  im u v w h  x y at-top-left >screen  w scale *  h scale *  draw-image-part  blend-usual pen-blend ;
: shade ( v -- )   \ 0 .. 128 (128: as it is)
    255 * 128 / 255 min {: g :}  g 24 lshift  g 16 lshift or  g 8 lshift or  $FF or  pen-color ;

\ ---- the menu's sounds (src: op_sound_init: CD.BIN 058_4 / 058_3, the set 059_0) ----
7 constant title-bank   variable title-set   -1 title-set !
0 constant confirm-sound   1 constant cancel-sound   $78 constant confirm-volume   $66 constant cancel-volume
: sounds-on ( -- )
    title-set @ 0< 0= if  exit  then
    s" cdbin/058_4.bank.txt" s" cdbin/058_3.samples" title-bank bank-load drop
    s" cdbin/059_0.sep" title-bank sep-load title-set ! ;
: sounds-off ( -- )  title-set @ 0< if  exit  then  title-set @ sep-free  title-bank bank-free  -1 title-set ! ;
: play ( n -- )   \ src: op_sound
    title-set @ 0< if  drop exit  then
    dup confirm-sound = if  confirm-volume  else  cancel-volume  then  {: n v :}  title-set @ n 1 v seq-play ;

\ ---- the pad (src: OPENING's: Start / ○ go, ✕ back) ----
variable go-was   variable back-was   variable up-was   variable down-was   variable left-was   variable right-was
variable go   variable back   variable up   variable down   variable left   variable right
: edge ( flag var -- pressed? )  {: f v :}  f v @ 0= and  f v ! ;
: read-pad ( -- )
    pad-start key-down? pad-circle key-down? or go-was edge go !  pad-cross key-down? back-was edge back !
    pad-up key-down? up-was edge up !  pad-down key-down? down-was edge down !
    pad-left key-down? left-was edge left !  pad-right key-down? right-was edge right ! ;
: forget-presses ( -- )  false go !  false back !  false up !  false down !  false left !  false right ! ;

\ ---- the movies (src: D_8012CFF0: PROLOGUE, OPENING, M_DIGEST, M_BADEND, M_FINEND - their last frame, volume,
\ height on the screen) ----
0 constant prologue   1 constant opening   2 constant digest   3 constant bad-end   4 constant final-end
create movie-rows  1232 , 78 , 24 ,   1579 , 98 , 39 ,   939 , 109 , 24 ,   2514 , 78 , 24 ,   4824 , 82 , 24 ,
: movie-row ( n -- addr )  3 cells * movie-rows + ;
: movie-name ( n -- addr len )
    case  prologue of  s" PROLOGUE"  endof  opening of  s" OPENING"  endof  digest of  s" M_DIGEST"  endof
        bad-end of  s" M_BADEND"  endof  s" M_FINEND" rot  endcase ;
320 constant movie-w   192 constant movie-h-usual   162 constant opening-h   variable playing   variable movie-frame   variable movie-y   variable movie-h
: skippable? ( n -- flag )  bad-end < ;

\ ---- the states (src: op_main_loop) ----
enum: title-state  logo-state intro-state title-screen digest-state menu-state opening-state load-state
    adjust-state chosen-state bad-end-state final-end-state bad-card final-card ;
variable step-of-title   variable counter   variable fade   variable name-drop   variable leaving   variable idle-frames-so-far
variable next-attract   variable sound-played   variable cursor   variable opening-frames
variable chosen-mode   \ 0 a new game, n: the save slot n - 1
: start-movie ( n -- )
    {: n :}  n playing !  n movie-name n movie-row cell+ @ movie-open drop  -1 movie-frame !
    n movie-row 2 cells + @ movie-y !  n opening = if  opening-h  else  movie-h-usual  then  movie-h ! ;
: movie-over ( -- )  movie-close  -1 playing ! ;
: after-movie ( -- )   \ src: op_movie_*: where each goes
    playing @ {: n :}  movie-over
    n case
        prologue of  title-screen step-of-title !  endof   digest of  title-screen step-of-title !  endof
        opening of  chosen-state step-of-title !  endof
        bad-end of  bad-card step-of-title !  0 fade !  endof   final-end of  final-card step-of-title !  0 fade !  endof
    endcase ;
: movie-step ( -- )   \ a step of it (its own rate); Start / ○ skips all but the endings
    movie-advance dup movie-frame !  {: f :}
    f -2 =  f playing @ movie-row @ >= or  go @ playing @ skippable? and or if  after-movie  then ;

\ the logo fades in and out (src: op_logo: 90 frames)
45 constant logo-half   10 constant logo-rate   90 constant logo-frames
: logo-frame ( -- )
    counter @ logo-half < if  counter @  else  logo-frames counter @ -  then  logo-rate * 0 max 128 min fade !
    counter @ logo-frames > if  intro-state step-of-title !  0 counter !  prologue start-movie exit  then
    1 counter +! ;
\ the title (src: op_title): it fades in, the name dropping into place; "press start"; idle, an attract movie
200 constant fully-up   50 constant picture-later   110 constant drop-start   40 constant drop-held   4 constant rise
1200 constant idle-frames
: title-frame ( -- )
    counter @ fully-up < if  1 counter +!  then
    leaving @ 0= if  drop-start counter @ - drop-held max name-drop !
    else  name-drop @ rise - 0 max name-drop !  then
    idle-frames-so-far @ idle-frames < if  1 idle-frames-so-far +!
    else  0 idle-frames-so-far !  next-attract @ if  prologue  else  digest  then  start-movie
        next-attract @ if  intro-state  else  digest-state  then  step-of-title !  next-attract @ 0= next-attract !  exit  then
    go @ if  true leaving !  sound-played @ 0= if  confirm-sound play  true sound-played !  then  then
    leaving @  name-drop @ 0= and if  menu-state step-of-title !  false leaving !  false sound-played !  then ;
\ the menu (src: op_menu)
4 constant #lines   3 constant vibration-line
: menu-frame ( -- )
    go @ if
        cursor @ case
            0 of  confirm-sound play  0 chosen-mode !  opening-state step-of-title !  opening start-movie  endof
            1 of  confirm-sound play  load-state step-of-title !  0 counter !  endof
            2 of  confirm-sound play  adjust-state step-of-title !  0 counter !  endof
        endcase
    then
    up @ if  cursor @ 1- #lines + #lines mod cursor !  then
    down @ if  cursor @ 1+ #lines mod cursor !  then
    cursor @ vibration-line =  left @ right @ or and if  vibration? 0= vibration!  then ;
\ the load menu: the save file, one card always there (src: op_load) - "checking", then the used slots, or "no data"
6 constant checking-frames
variable slot-cursor
: any-saved? ( -- flag )  false  #save-slots 0 do  i slot-used? or  loop ;
: next-used ( way -- )   \ the cursor to the next used slot that way
    {: w :}  #save-slots 0 do  slot-cursor @ w + #save-slots + #save-slots mod slot-cursor !  slot-cursor @ slot-used? if  leave  then  loop ;
: first-used ( -- )  0 slot-cursor !  slot-cursor @ slot-used? 0= if  1 next-used  then ;
: load-frame ( -- )
    counter @ checking-frames < if  1 counter +!  counter @ checking-frames = any-saved? and if  first-used  then  exit  then
    back @ if  cancel-sound play  menu-state step-of-title !  exit  then
    any-saved? 0= if  exit  then   \ ("there is no data": back only)
    up @ if  -1 next-used  then  down @ if  1 next-used  then
    go @ if  confirm-sound play  slot-cursor @ 1+ chosen-mode !  chosen-state step-of-title !  then ;
\ adjust the screen (src: op_adjust: kept with the settings; the PC's window shows it whole)
15 constant offset-most   $42 constant panel-drop   variable ox   variable oy
: adjust-frame ( -- )
    right @ if  ox @ 1+ offset-most min ox !  then  left @ if  ox @ 1- offset-most negate max ox !  then
    up @ if  oy @ 1- offset-most negate max oy !  then  down @ if  oy @ 1+ offset-most min oy !  then
    go @ if  confirm-sound play  ox @ oy @ screen-offset!  menu-state step-of-title !  then ;
\ the endings' cards fade in (src: op_end_card); Start / ○ then: the logo again
: card-frame ( -- )
    fade @ 128 < if  1 fade +!  exit  then
    go @ if  0 chosen-mode !  logo-state step-of-title !  0 counter !  sounds-on  then ;

: start-game ( -- )   \ the title done: a new game, or a slot continued
    false title-open !  sounds-off
    chosen-mode @ 0= if  s" story" actor-named send new-game  exit  then
    chosen-mode @ 1- continue-game 0= if  s" story" actor-named send new-game  then ;
: title-frame-all ( -- )   \ one of OPENING's frames
    1 opening-frames +!
    step-of-title @ case
        logo-state of  logo-frame  endof
        title-screen of  title-frame  endof
        menu-state of  menu-frame  endof
        load-state of  load-frame  endof
        adjust-state of  adjust-frame  endof
        bad-card of  card-frame  endof
        final-card of  card-frame  endof
    endcase ;
2 constant frames-a-step   \ (OPENING ran at 60 a second)
: title-step ( -- )
    title-up? 0= if  exit  then
    read-pad
    playing @ 0< 0= if  movie-step exit  then
    step-of-title @ chosen-state = if  start-game exit  then
    frames-a-step 0 do  title-frame-all  forget-presses  loop ;

\ ---- drawing ----
variable logo-image   variable left-image   variable right-image   variable name-image   variable press-image
variable vibration-image   variable panel-image   variable error-image   variable slots-image
variable adjust-left   variable adjust-right   variable bad-image   variable final-image
create menu-images #lines cells allot   menu-images #lines cells erase
: once ( var addr len -- image )  {: v a n :}  v @ ?dup if  exit  then  a n picture dup v ! ;
: draw-picture ( v -- )   \ src: op_title_picture: the title's picture at 30 .. 209
    shade  left-image s" picture-left" once 0 30 256 180 draw  right-image s" picture-right" once 256 30 64 180 draw ;
: draw-name ( v y -- )  {: v y :}  v shade  name-image s" name" once 160 y 143 37 draw-added ;
100 constant name-y
32 constant blink-period   11 constant blink-shown   \ ("press start": shown 21 frames of 32)
: draw-title-screen ( -- )   \ src: op_title: the picture later than the name; "press start" blinking
    counter @ picture-later - 0 max 128 min draw-picture
    counter @ 128 min  name-drop @ name-y + draw-name
    counter @ 128 >  opening-frames @ blink-period 1- and blink-shown >= and if
        $FFFFFFFF pen-color  press-image s" press-start" once 200 180 63 15 draw-added  then ;
: draw-movie ( -- )
    movie-frame @ 0< if  exit  then  $FFFFFFFF pen-color
    0 movie-y @ at-top-left >screen  movie-w scale *  movie-h @ scale *  movie-draw ;
: draw-menu ( -- )
    128 draw-picture  128 name-y draw-name  $FFFFFFFF pen-color
    menu-images cursor @ cells +  cursor @ case  0 of  s" menu-0"  endof  1 of  s" menu-1"  endof
        2 of  s" menu-2"  endof  s" menu-3" rot  endcase  once  192 142 80 48 draw-added
    vibration-image s" vibration" once  0  vibration? 0= 32 and  cursor @ vibration-line = 16 and +  80 16  192 188 draw-part ;
\ the load menu's panel rows (src: op_load: from v $26 of its image)
$26 constant panel-top   160 constant panel-x   150 constant panel-y   143 constant panel-w
$26 constant checking-row   $C6 constant no-data-row   16 constant row-h
: draw-load ( -- )
    128 draw-picture  128 name-y draw-name  $FFFFFFFF pen-color
    counter @ checking-frames < if  panel-image s" card-panel" once  0 checking-row panel-top - panel-w row-h panel-x panel-y draw-part exit  then
    any-saved? 0= if  error-image s" card-panel-error" once  0 no-data-row panel-top - panel-w row-h panel-x panel-y draw-part exit  then
    slots-image s" slots" once  slot-cursor @ 32 *  0 32 48  216 150 draw-part ;
: draw-adjust ( -- )
    128 draw-picture  128 name-y panel-drop + draw-name  $FFFFFFFF pen-color
    adjust-left s" adjust-left" once  0 0 256 240 draw-added
    adjust-right s" adjust-right" once  256 0 64 240 draw-added ;
: draw-title ( -- )
    title-up? 0= if  exit  then
    playing @ 0< 0= if  draw-movie exit  then
    step-of-title @ case
        logo-state of  fade @ shade  logo-image s" logo" once 85 94 150 51 draw  endof
        title-screen of  draw-title-screen  endof
        menu-state of  draw-menu  endof
        load-state of  draw-load  endof
        adjust-state of  draw-adjust  endof
        bad-card of  fade @ shade  bad-image s" end-bad" once 172 180 128 16 draw  endof
        final-card of  fade @ shade  final-image s" end-final" once 94 118 128 16 draw  endof
    endcase ;
' draw-title on-draw
PRIVATE>

\ the endings: the session over, OPENING then plays its movie and card (src: ending_bad / ending_final,
\ opening_exec: the game's world afresh after)
1 constant bad-ending   2 constant final-ending
: ending ( mode -- )
    s" sound" actor-named {: mode snd :}  snd send stop-stream  -1 snd send play-music  mode reboot ;
main-exe $80047BFC :noname  bad-ending ending ; native:
main-exe $80047C2C :noname  final-ending ending ; native:

: title-start ( mode -- )   \ 0 the logo and title; 1 the bad ending's movie, 2 the final one's (src: op_main_loop)
    true title-open !  -1 playing !  0 counter !  0 idle-frames-so-far !  0 cursor !  0 chosen-mode !  false leaving !
    screen-offset oy ! ox !
    case
        1 of  bad-end-state step-of-title !  bad-end start-movie  endof
        2 of  final-end-state step-of-title !  final-end start-movie  endof
        logo-state step-of-title !  sounds-on
    endcase ;

state: title-actor-state end-state
behaviour the-title
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  title-step ;
  on title-boot ( mode -- )  title-start ;
end-behaviour

: spawn-title ( -- id )  the-title title-actor-state s" title" spawn ;
