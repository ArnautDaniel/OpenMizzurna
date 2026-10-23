\ talk.fs - talking to the townsfolk (docs/subsystems/talk.md; src: cevcon.c func_800410B0, cobj.c
\ actor_talk_start, the characters' handlers, char_talk.c, the overlays' talk hooks). ○ on a person: their
\ story conversation if they have one now, else the area's line for them, else their own default line.
IN: talk
USING: engine actors messages clock areas floors figures matthew townsfolk events ;

\ ---- who ○ would talk to (src: func_800410B0) ----
122 constant reach-margin   568 constant talk-angle   \ (0x7A over both radii; 0x238 of a turn either side)
: radius ( slot -- r )  >r s" actor-radii" r> exe-s16 ;
: distance ( x z x' z' -- d )  rot - dup *  -rot - dup *  + s>f fsqrt f>s ;
: before-him? ( slot -- flag )   \ close enough, and in front of him
    {: s :}  s person-now drop {: px py pz :}  matthew-now {: mx my mz h :}
    mx mz px pz distance  matthew radius  s radius +  reach-margin +  <
    h  px mx - s>f pz mz - s>f fatan2 4096e f* 2e pi f* f/ f>s  talk-angle heading-within?  and ;
: in-front ( -- slot | -1 )   \ the first person standing here before him
    -1  #slots 1 do  i here? i radius 0<> and if  i before-him? if  drop i leave  then  then  loop ;

\ ---- a one-liner (src: func_8003FBFC and its templates in the executable) ----
\ the shot: a camera set about the speaker, indoors or out (src: D_800963E0, D_8009615C)
-470 constant indoor-look-y  -908 constant indoor-dist  -1370 constant indoor-eye-y  1856 constant indoor-yaw
-474 constant outdoor-look-y  -2210 constant outdoor-dist  -1106 constant outdoor-eye-y  260 constant outdoor-yaw
variable line   variable speaker   variable gesture
: talk-shot ( -- )
    area @ the-town = if  outdoor-look-y outdoor-dist outdoor-eye-y 0 0 0 outdoor-yaw
    else  indoor-look-y indoor-dist indoor-eye-y 0 0 0 indoor-yaw  then  camera-set ;
: one-liner-event ( -- )   \ (an event: the template's ops)
    speaker @ as-this-one  talk-shot  matthew 0 1 anim  this-one camera-at
    this-one matthew 0 face  this-one line @ gesture @ talk ;
: one-liner ( msg slot gesture -- )  gesture !  speaker !  line !  ['] one-liner-event start-event ;
65535 constant no-gesture
10512 constant hmm-what    \ "Hmm? What?" (src: 0x2910: the common text's 512)

\ ---- the layers of a conversation ----
create conversations  #slots cells allot   conversations #slots cells erase   \ a person's story talks
create default-lines  #slots cells allot   default-lines #slots cells erase   \ their own lines
create area-talks  33 cells allot   area-talks 33 cells erase                 \ an area's lines for them
: conversation! ( xt slot -- )  cells conversations + ! ;   \ xt ( -- handled? )
: default-line! ( xt slot -- )  cells default-lines + ! ;   \ xt ( slot -- )
: area-talk! ( xt area -- )  cells area-talks + ! ;          \ xt ( slot -- handled? )
: a-line-by-stop ( table key -- msg | -1 )   \ a table of (key, message) pairs ended by -1 (src: func_800429C8)
    {: t k :}  t  begin  dup @ -1 <> while  dup @ k = if  cell+ @ exit  then  2 cells +  repeat  drop -1 ;
: talk-to ( slot -- )   \ src: actor_talk_start
    {: s :}
    s cells conversations + @ ?dup if  execute if  exit  then  then
    area @ cells area-talks + @ ?dup if  s swap execute if  exit  then  then
    s cells default-lines + @ ?dup if  s swap execute exit  then
    hmm-what s 7 one-liner ;
