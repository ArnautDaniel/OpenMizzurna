\ sound.fs - the game's sound (docs/subsystems/sound.md; src: sound.c, ambience.c, session.c sound_init,
\ area.c area_sound_load / area_sep_open). Three banks are open: the common one (033_1 and its samples), the area's
\ (its NNN_1) and the music's (033_4's part 10 and 058_3). An effect is a program of a bank - under 100 the common
\ one's, else the area's (less 100) - played once, at the middle key, its first tone (a few of the areas' play
\ several); a held loop the same, every tone covering the key, until stopped (three of each bank's at once). The
\ sequences: under 100 the common set's, 100 - 199 the area's. The background is a row of the music table - a
\ sequence set of its own on the music bank, once or for ever, at its volume - started when an area begins:
\ its ambience, faded out after 2520 frames and silent until 5400, and silent outdoors at night; a scene's
\ music takes its place. An XA stream (TSG.XA's channel) puts the background aside while it plays.
IN: sound
USING: engine actors messages clock areas figures ;

variable last-sound   -1 last-sound !   \ (a fact: the effect played last - the tests')

<PRIVATE
0 constant common-bank   1 constant area-bank   2 constant music-bank
100 constant area-ids   200 constant music-ids   60 constant middle-key   0 constant first-tone
127 constant full   3 constant #loops
-1 constant nothing
variable common-set   -1 common-set !   variable area-set   -1 area-set !   variable music-set   -1 music-set !
variable background   -1 background !   \ src: D_800A36FC: the music table's row playing (-1 none)
variable background-volume
variable cycle   \ src: D_800A36F8: frames into the background's cycle
variable streaming   \ src: D_800A3700: an XA stream plays
create loops-held  #loops 2 * cells allot   loops-held #loops 2 * cells -1 fill   \ src: D_800A3702 / 6
fvariable due

\ ---- the library's places ----
: three ( n -- )  dup 100 < if  s" 0" $+  then  dup 10 < if  s" 0" $+  then  n>str $+ ;
: entry-path ( group kind addr len -- addr len )   \ cdbin/GGG_K<rest>
    {: g k a n :}  $clear  s" cdbin/" $+  g three  s" _" $+  k n>str $+  a n $+  $@ ;
: copy ( addr len buf -- addr len )  {: a n b :}  a b n move  b n ;
create text-path 64 allot   create samples-path 64 allot   create sep-path 64 allot

\ ---- banks and sets (src: sound_init, area_sound_load, area_sep_open) ----
: open-common ( -- )
    33 1 s" .bank.txt" entry-path text-path copy  33 2 s" .samples" entry-path  common-bank bank-load drop
    s" cdbin/033_4/9.sep" common-bank sep-load common-set !
    s" cdbin/033_4/10.bank.txt" s" cdbin/058_3.samples" music-bank bank-load drop ;
: open-area ( area -- )
    {: a :}  area-set @ 0< 0= if  area-set @ sep-free  -1 area-set !  then  area-bank bank-free
    a 0< if  exit  then
    a 1 s" .bank.txt" entry-path text-path copy  a 2 s" .samples" entry-path samples-path copy
    area-bank bank-load drop
    a 4 s" /9.sep" entry-path sep-path copy  area-bank sep-load area-set ! ;

\ ---- effects and loops (src: sound_play, func_80022354, ambient_start, ambient_stop) ----
: bank-of ( id -- program bank )  dup area-ids < if  common-bank  else  area-ids - area-bank  then ;
: tones-at-once ( id -- n )   \ the areas' effects that sound as several voices
    area @ 5 =  over $71 = and if  drop 3 exit  then
    area @ 26 =  swap $6D = and if  2  else  1  then ;
: play-effect ( id -- )
    dup last-sound !
    dup tones-at-once {: id n :}  id bank-of {: p b :}
    n 0 do  effect-voice b p i middle-key full key-on drop  loop ;
: held ( bank k -- addr )  swap area-bank = if  #loops +  then  cells loops-held + ;
: free-loop ( bank -- k | -1 )  -1  #loops 0 do  over i held @ 0< if  drop i leave  then  loop  nip ;
: loop-on ( id -- )
    bank-of {: p b :}  b free-loop {: k :}  k 0< if  exit  then
    p b k held !  loop-voice b p -1 middle-key full key-on drop ;
: loop-stop ( id -- )
    bank-of {: p b :}  -1  #loops 0 do  b i held @ p = if  drop i  then  loop  {: k :}
    k 0< if  exit  then  -1 b k held !  b p middle-key loop-off ;

\ ---- the car's engine (src: func_80022DF4 / func_80022E84, engine_sound): its tones held, the first's pitch moving ----
3 constant #engine
create engine-voices  #engine cells allot   engine-voices #engine cells -1 fill
: engine-on ( id tones -- )
    {: id n :}  id bank-of {: p b :}
    n #engine min 0 ?do  effect-voice b p i middle-key full key-on  i cells engine-voices + !  loop ;
: engine-off ( -- )   \ (into their release)
    #engine 0 do  i cells engine-voices + @ dup 0< if  drop  else  key-off  then  -1 i cells engine-voices + !  loop ;
: engine-key ( key -- )  engine-voices @ dup 0< if  2drop exit  then  swap 0 voice-pitch ;

\ ---- sequences (src: seq_play, seq_stop) ----
: set-of ( id -- seq set )  dup area-ids < if  common-set @  else  area-ids - area-set @  then ;
: sequence ( id loop? -- )  {: id lp :}  id set-of {: q st :}  st 0< if  exit  then  st q lp 0= 1 and full seq-play ;
: sequence-off ( id -- )  set-of {: q st :}  st 0< if  exit  then  st q seq-stop ;

\ ---- the background (src: music_play, func_800225AC, music_timer_update) ----
12 constant row-bytes   0 constant row-entry   2 constant row-loops   8 constant row-volume
2520 constant fade-at   2700 constant stop-at   2701 constant parked   5400 constant cycle-length
3000 constant fade-ms   \ (src: 180 of the sound chip's frames)
: row-u8 ( row k -- n )  swap row-bytes * + {: k :}  s" music" k exe-u8 ;
: row-entry@ ( row -- entry )  row-bytes * 4 / {: k :}  s" music" k exe-s32 ;
: row-loops@ ( row -- n )  row-bytes * row-loops 2* + 2/ {: k :}  s" music" k exe-s16 ;
: background-off ( -- )   \ src: func_80022D20
    music-set @ 0< 0= if  music-set @ 0 seq-stop  music-set @ sep-free  -1 music-set !  then  -1 background ! ;
: background-on ( row -- )   \ its set on the music bank, its sequence 0
    {: r :}  background-off  r row-entry@ {: e :}
    e 5 / e 5 mod s" .sep" entry-path sep-path copy  music-bank sep-load music-set !
    r row-volume row-u8 background-volume !  r background !
    music-set @ 0< if  exit  then  music-set @ 0 r row-loops@ background-volume @ seq-play ;
: area-ambience ( -- row )   \ the area's (by the hour in areas 21 and 27)
    s" area-sounds" area @ 2* 1+ exe-s16 {: r :}  clock-time time>hour {: h :}
    area @ 21 = if  h 9 < if  2 exit  then  h 15 < if  0 exit  then  h 20 < if  1 exit  then  r exit  then
    area @ the-town = if  h 9 < if  8 exit  then  h 13 < if  12 exit  then  h 18 < if  5 exit  then  9 exit  then
    r ;
: outdoors? ( area -- flag )   \ (the areas whose ambience rests at night)
    {: a :}  a 0 4 within  a 7 = or  a 9 = or  a 13 = or  a 29 = or  a 30 = or ;
: silent-now? ( -- flag )   \ src: func_800231A8: outdoors at night, and the like
    clock-time time>hour {: h :}
    area @ 12 = if  h 20 < exit  then
    area @ the-town = if  h 6 < exit  then
    area @ outdoors? if  h 7 <  h 20 >= or exit  then  false ;
: cycle-frame ( -- )   \ src: music_timer_update
    streaming @ if  exit  then
    area @ 0< if  exit  then
    silent-now? if  parked cycle !  then
    cycle @ {: t :}
    t 0= if  background @ 0< if  area-ambience background-on  then  then
    t 1+ cycle !
    t 1+ fade-at = if  music-set @ 0< 0= if  music-set @ 0 true fade-ms seq-fade  then  then
    cycle @ stop-at = if  background-off  then
    cycle @ cycle-length = if  0 cycle !  then ;
: music! ( row -- )   \ src: music_play: a scene's music in the background's place (-1: none)
    streaming @ if  drop exit  then
    dup 0< if  drop background-off  stop-at cycle !  exit  then
    background-on  0 cycle ! ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: sound-tick ( -- )  steps-due 0 ?do  cycle-frame  loop ;

\ ---- XA (src: xa_play, xa_stop): TSG.XA's channel at its volume ----
create xa-volumes  $4D c, $4C c, $4E c, $52 c, $67 c, $6D c, $7F c,
7 constant #xa
: stream ( n -- )
    dup 0 #xa within 0= if  drop exit  then
    dup xa-volumes + c@  xa-start  true streaming ! ;
: stream-off ( -- )  xa-end  false streaming ! ;

: entered ( area -- )   \ src: area_sound_free, area_sound_load, area_sep_open
    background-off  #loops 2 * 0 do  -1 loops-held i cells + !  loop
    open-area  0 cycle !  0e due f! ;
PRIVATE>

: background-row ( -- row )  background @ ;        \ (a fact: the music table's row playing, -1 none)
\ (the others' way to it)
: sfx ( id -- )  s" sound" actor-named send play-sound ;          \ src: sound_play
: sfx? ( id -- )  dup 0< if  drop  else  sfx  then ;              \ (-1: none)
: music-row ( row -- )  s" sound" actor-named send play-music ;   \ src: music_play (-1: none)

state: sound-state end-state
behaviour sounds
  on spawned ( -- )  self subscribe tick  self subscribe area-entered  open-common ;
  on tick ( -- )  sound-tick  streaming @ if  xa-playing? 0= if  false streaming !  then  then ;
  on area-entered ( area scene -- )  drop entered ;
  on play-sound ( id -- )  play-effect ;
  on loop-sound ( id -- )  loop-on ;
  on stop-loop ( id -- )  loop-stop ;
  on play-sequence ( id loop? -- )  sequence ;
  on stop-sequence ( id -- )  sequence-off ;
  on play-music ( row -- )  music! ;
  on play-stream ( n -- )  background-off  stream ;
  on stop-stream ( -- )  stream-off ;
  on engine-start ( id tones -- )  engine-on ;
  on engine-pitch ( key -- )  engine-key ;
  on engine-stop ( -- )  engine-off ;
end-behaviour

: spawn-sound ( -- id )  sounds sound-state s" sound" spawn ;
