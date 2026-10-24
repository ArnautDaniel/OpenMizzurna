\ window.fs - the message window (docs/subsystems/window.md; src: msgbox.c, ctexstr.c): a message typed into
\ the box low on the screen, a page at a time, closed with ○; questions with a cursor; timed captions. While
\ a message waiting for ○ is up the clock stands still (src: the VM's clock_pause / clock_resume).
\ Its state is the window's own (variables: it is the only window), read by its drawing; others read only the
\ facts window-up? and window-answer.
IN: window
USING: engine actors messages keys clock screen effects ;

\ ---- the original's screen (320 x 240, the origin at its centre) and the box ----
-95 constant box-x      40 constant box-y        \ src: msg_init, func_80035BAC(w, -0x5F, 0x28, 0xF0, 0x30)
240 constant box-w      3 constant box-lines     \ (48 pixels: three 16-pixel lines)
-79 constant choice-x                            \ src: msg_choice (the text right of the cursor)
-95 constant cursor-x
37 constant band-top    89 constant band-bottom  \ the dark band behind it (src: func_80015DCC)
129 constant arrow-x    87 constant arrow-y      \ the next-page arrow (src: func_80033278)
$00000090 constant band-color

\ ---- its pace ----
\ src: 4 glyphs a frame (func_800349C4: +4 a frame, one a unit) and lines scrolling 2 pixels a frame, at the
\ 15 frames a second the game ran while a window was up
60e fconstant type-rate     \ characters a second
30e fconstant scroll-rate   \ pixels a second

\ ---- facts ----
variable window-up      \ a message is up
variable window-answer  \ the last question's answer (-1: none)
: window-up? ( -- flag )  window-up @ 0<> ;

\ ---- the window's own ----
enum: stage  closed typing page-full scrolling finished asking ;
variable stage   closed stage !
create text 2048 allot   variable text-len
variable lines            \ the message's lines
variable top              \ the first line in the box
fvariable typed           \ characters shown (of the text)
fvariable scrolled        \ pixels the lines have moved up (0 .. 16)
variable speaker   variable palette   variable left
fvariable time-left       \ a timed message: seconds before it closes (0: it waits for ○)
variable choices   variable first-choice   variable cursor
variable confirm-was   variable up-was   variable down-was

\ the text: lines and characters
variable wanted
: line-start ( n -- offset )   \ where line n begins (after its n newlines)
    dup wanted !  0= if  0 exit  then
    0  text-len @ 0 ?do
        text i + c@ 10 = if  1+  dup wanted @ = if  drop i 1+ unloop exit  then  then
    loop  drop text-len @ ;
: count-lines ( -- n )  1  text-len @ 0 ?do  text i + c@ 10 = if  1+  then  loop ;
: shown ( -- n )  typed f@ f>s text-len @ min ;

\ the pad, a press at a time (its own edges: a held button is one press)
: press-edge ( key var -- flag )  >r key-down? dup r@ @ 0= and swap r> ! ;
: confirm? ( -- flag )  pad-circle confirm-was press-edge ;

: clock-id ( -- id )  s" clock" actor-named ;
: open ( bank id speaker palette -- )
    palette ! speaker !  text-of  2047 min dup text-len !  text swap move
    count-lines lines !  0 top !  0e typed f!  0e scrolled f!  -1 window-answer !
    box-x left !  true window-up !  typing stage !  pad-circle key-down? confirm-was ! ;
: shut ( -- )
    stage @ closed <> if
        closed stage !  false window-up !  text-shown clock-id send release-clock
        window-answer @ broadcast window-closed
    then ;

\ ---- a window-step of it ----
: page-end ( -- offset )   \ where the box is full (the start of the line after it), or the text's end
    top @ box-lines + dup lines @ < if  line-start 1-  else  drop text-len @  then ;
: type-on ( -- )
    typed f@  type-rate dt f* f+  page-end s>f fmin  typed f!
    shown page-end >= if
        top @ box-lines + lines @ < if  page-full  else  choices @ if  asking  else  finished  then  then  stage !
    then ;
: scroll-on ( -- )
    scrolled f@  scroll-rate dt f* f+  fdup font-line s>f f>= if
        fdrop 0e  1 top +!  typing stage !
    then  scrolled f! ;
: count-down ( -- )   \ a timed message closes itself
    time-left f@ f0> if  time-left f@ dt f- fdup time-left f!  f0> 0= if  shut  then  then ;
: choose ( -- )
    pad-up up-was press-edge if  cursor @ 1- 0 max cursor !  then
    pad-down down-was press-edge if  cursor @ 1+ choices @ 1- min cursor !  then
    confirm? if  cursor @ window-answer !  shut  then ;
: window-step ( -- )
    stage @ case
        typing of  type-on  confirm? drop  endof
        page-full of  confirm? if  scrolling stage !  then  endof
        scrolling of  scroll-on  endof
        finished of  time-left f@ f0> 0= if  confirm? if  shut  then  then  endof
        asking of  choose  endof
    endcase
    count-down ;

\ ---- drawing: the original's screen, scaled to the window ----
: band ( -- )  band-color pen-color  -160 band-top >screen  screen-w scale *  band-bottom band-top - scale *  draw-rect ;
: line-y ( k -- y )   \ the box's k-th line, scrolled
    font-line * box-y +  scrolled f@ f>s - ;
: draw-lines ( -- )
    0 font  palette @ font-palette  $FFFFFFFF pen-color  scale pen-scale
    box-lines 1+ 0 do
        top @ i + dup lines @ < if
            dup line-start  swap 1+ dup lines @ < if  line-start 1-  else  drop text-len @  then   ( from to )
            shown min over - 0 max   ( from n )
            dup 0> i line-y box-y >= and if
                >r text + r>  left @ i line-y >screen font-text
            else  2drop  then
        else  drop  then
    loop ;
: draw-arrow ( -- )   \ (blinking: the original's animated arrow sprite, a mark here)
    stage @ page-full = ticks 8 / 2 mod 0= and if
        $FFFFFFFF pen-color  arrow-x arrow-y >screen  scale 4 * dup draw-rect
    then ;
: draw-cursor ( -- )
    stage @ asking = if
        $FFFFFFFF pen-color  0 font  scale pen-scale
        s" >" cursor-x  cursor @ first-choice @ + font-line * box-y +  >screen font-text
    then ;
: draw-window ( -- )
    window-up? if  band draw-lines draw-arrow draw-cursor  then ;
' draw-window on-draw
: on-top ( -- )  ['] draw-window dup off-draw on-draw ;   \ (drawn last: over any screen opened since)
' draw-effect-over on-draw   \ (the screen's effects over the window; its +256 ones under it: effects.fs)

\ ---- the actor ----
state: window-state end-state
behaviour message-window
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  window-step ;
  on window-say ( bank id speaker palette secs -- )
    >r open  on-top  r> s>f time-left f!  0 choices !
    time-left f@ f0> 0= if  text-shown clock-id send hold-clock  then ;
  on window-ask ( bank id kind -- )   \ src: msg_choice: kind 1 / 2 that many answers more, below no question;
                                      \ else a question and two answers below it
    >r nobody 0 open  on-top  0e time-left f!  choice-x left !  0 cursor !
    r@ 1 = r@ 2 = or if  r@ 1+ choices !  0 first-choice !  else  2 choices !  1 first-choice !  then  r> drop
    text-shown clock-id send hold-clock ;
  on window-shut ( -- )  shut ;
end-behaviour

: spawn-window ( -- id )  message-window window-state s" window" spawn ;
