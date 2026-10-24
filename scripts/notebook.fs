\ notebook.fs - Matthew's notebook (docs/subsystems/notebook.md; src: fhbook.c): chosen in the ring, it opens
\ over the field, the world standing still (ui.fs): a menu of two pages - the people he knows (a portrait, the
\ name, the job, a few lines about them) and the phone numbers he knows (eight to a page). Up and down choose,
\ ○ opens, ✕ goes back and closes. Who and what he knows are the notebook's facts (bios-known, numbers-known):
\ a talk makes a person known, a call answered or a flyer read a number (learn-bio, learn-number).
IN: notebook
USING: engine actors messages keys screen ui ;

\ ---- facts ----
$2B4092 constant first-bios     \ known at a new game (src: notebook_init)
$80F constant first-numbers     \ the sheriff's department, Winona's, the general store, the hospital, Emma's house
variable bios-known   first-bios bios-known !         \ a bit a person (the rows of `notebook-people`)
variable numbers-known   first-numbers numbers-known ! \ a bit a number (the book's lines from 2)
variable book-open
: notebook-up? ( -- flag )  book-open @ 0<> ;
: bio-known? ( bit -- flag )  1 swap lshift bios-known @ and 0<> ;
: number-known? ( bit -- flag )  1 swap lshift numbers-known @ and 0<> ;

<PRIVATE
\ ---- its text and pictures ----
63 5 * 2 + constant book-text   \ CD.BIN 063_2: 0 "Phone Numbers", 1 "Characters", from 2 the numbers
0 constant phone-numbers-line   1 constant characters-line   2 constant first-number-line
34 constant list-text           \ the list font's labels: "Name:" and "Job:"
0 constant name-label   25 constant job-label
211 constant left-half   212 constant right-half   \ the notebook's picture (CD.BIN 042_1, 042_2)
1 constant list-font   6 constant ink   0 constant list-ink   \ (the dialogue font's brown palette)
20 constant #number-bits   24 constant #people

\ ---- its state ----
enum: page  on-the-menu people-page numbers-page ;
variable page   variable row        \ the menu's row: 0 the characters, 1 the phone numbers
create known #people cells allot   variable #known   \ the page's rows: the known people's, or numbers'
variable at                         \ the person shown, or the numbers' first shown
8 constant rows-a-page
variable up-was   variable down-was   variable pick-was   variable back-was
: edge ( flag var -- pressed? )  {: f v :}  f v @ 0= and  f v ! ;

: gather ( mask bits -- )   \ the known ones' bits, in order
    0 #known !  0 ?do  dup 1 i lshift and if  i known #known @ cells + !  1 #known +!  then  loop  drop ;
: open-page ( page -- )
    dup page !  0 at !
    case
        people-page of  bios-known @ #people gather  endof
        numbers-page of  numbers-known @ #number-bits gather  endof
    endcase ;
: open-book ( -- )
    true book-open !  0 row !  on-the-menu page !  s" ui" actor-named send ui-lock
    pad-up key-down? up-was !  pad-down key-down? down-was !
    pad-circle key-down? pad-square key-down? or pick-was !  pad-cross key-down? back-was ! ;
: close-book ( -- )  false book-open !  s" ui" actor-named send ui-unlock ;
: more-above? ( -- flag )  at @ 0> ;
: more-below? ( -- flag )
    page @ numbers-page = if  at @ rows-a-page + #known @ <  else  at @ 1+ #known @ <  then ;
: step-of ( -- n )  page @ numbers-page = if  rows-a-page  else  1  then ;
: book-step ( -- )   \ src: notebook_update, notebook_bio_page, notebook_phone_page
    pad-up key-down? up-was edge  pad-down key-down? down-was edge
    pad-circle key-down? pad-square key-down? or pick-was edge  pad-cross key-down? back-was edge
    {: u d o x :}
    notebook-up? 0= if  exit  then
    page @ on-the-menu = if
        x if  close-book exit  then
        u if  0 row !  then  d if  1 row !  then
        o if  row @ if  numbers-page  else  people-page  then  open-page  then  exit
    then
    x if  on-the-menu page !  exit  then
    u more-above? and if  step-of negate at +!  exit  then
    d more-below? and if  step-of at +!  then ;

\ ---- drawing (the original's screen) ----
-136 constant book-x   -96 constant book-y   136 constant half-w   192 constant book-h
96 constant hint-x   80 constant hint-y   16 constant button-w   40 constant hint-w   16 constant hint-h
-54 constant menu-x   -40 constant menu-y   54 constant hand-x   -34 constant hand-y   24 constant hand-w
16 constant line-h
-112 constant rows-x   -72 constant rows-y   4 constant rows-before-gap
-128 constant arrow-x   -96 constant above-y   80 constant below-y
-112 constant portrait-x   -88 constant portrait-y   64 constant portrait-w   80 constant portrait-h
-42 constant label-x   -64 constant name-y   -48 constant job-y   40 constant name-after   32 constant job-after
8 constant bio-y
variable left-image   variable right-image   variable hand   variable exit-label   variable cross-button   variable above   variable below
create portraits 256 cells allot   portraits 256 cells erase
: once ( var addr len -- image )   \ a picture of the library's, loaded once
    {: v a n :}  v @ ?dup if  exit  then  a n image  dup v ! ;
: ui-piece ( var addr len -- image )  {: v a n :}  v @ ?dup if  exit  then  $clear  s" ui/" $+  a n $+  s" .png" $+  $@ image  dup v ! ;
: piece ( image x y w h -- )  {: im x y w h :}  im x y >screen  w scale *  h scale *  draw-image ;
: portrait ( kind -- image )
    {: k :}  portraits k cells + @ ?dup if  exit  then
    $clear  s" portraits/" $+  k n>str $+  s" .png" $+  $@ image  dup portraits k cells + ! ;
: ink-text ( addr len x y -- )  >screen  0 font  ink font-palette  font-text ;
: line ( n x y -- )  {: n x y :}  book-text n text-of  x y ink-text ;
: label ( id x y -- )  {: id x y :}  list-font font  list-ink font-palette  list-text id text-of  x y >screen font-text ;
: draw-book ( -- )
    left-image s" cdbin/042_1.png" once  book-x book-y half-w book-h piece
    right-image s" cdbin/042_2.png" once  book-x half-w + book-y half-w book-h piece
    cross-button s" button-cross" ui-piece  hint-x hint-y button-w button-w piece
    exit-label s" exit" ui-piece  hint-x button-w + hint-y hint-w hint-h piece ;
: draw-menu ( -- )
    characters-line menu-x menu-y line  phone-numbers-line menu-x menu-y line-h + line
    hand s" hand" ui-piece  hand-x hand-y row @ line-h * + hand-w line-h piece ;
: arrows ( -- )
    more-above? if  above s" more-above" ui-piece  arrow-x above-y button-w button-w piece  then
    more-below? if  below s" more-below" ui-piece  arrow-x below-y button-w button-w piece  then ;
: row-y ( r -- y )  dup line-h *  swap rows-before-gap < 0= line-h and +  rows-y + ;
: draw-numbers ( -- )
    #known @ at @ - rows-a-page min 0 max 0 ?do
        known at @ i + cells + @ first-number-line +  rows-x i row-y line
    loop  arrows ;
: person-byte ( k -- n )   \ byte k of the person shown's row
    known at @ cells + @ 4 * + {: o :}  s" notebook-people" o exe-u8 ;
: draw-person ( -- )
    #known @ 0= if  exit  then
    1 person-byte ?dup if  portrait  portrait-x portrait-y portrait-w portrait-h piece  then
    name-label label-x name-y label  0 person-byte label-x name-after + name-y label
    job-label label-x job-y label  2 person-byte label-x job-after + job-y label
    3 person-byte rows-x bio-y line  arrows ;
: draw-notebook ( -- )
    notebook-up? 0= if  exit  then
    $FFFFFFFF pen-color  scale pen-scale  draw-book
    page @ case
        on-the-menu of  draw-menu  endof
        people-page of  draw-person  endof
        numbers-page of  draw-numbers  endof
    endcase ;
' draw-notebook on-draw
PRIVATE>

state: notebook-state end-state
behaviour the-notebook
  on spawned ( -- )  self subscribe tick  self subscribe ring-chosen ;
  on tick ( -- )  book-step ;
  on ring-chosen ( item who -- )   \ src: ui_update's choice 1
    0< swap 1 = and  notebook-up? 0= and if  open-book  then ;
  on learn-bio ( bit -- )  1 swap lshift bios-known @ or bios-known ! ;
  on learn-number ( bit -- )  1 swap lshift numbers-known @ or numbers-known ! ;
end-behaviour

: spawn-notebook ( -- id )  the-notebook notebook-state s" notebook" spawn ;
