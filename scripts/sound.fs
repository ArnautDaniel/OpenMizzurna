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
USING: engine actors messages clock areas figures camera matthew ;

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
fvariable sound-due

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
: play-effect-at ( id volume -- )
    over last-sound !
    over tones-at-once {: id v n :}  id bank-of {: p b :}
    n 0 do  effect-voice b p i middle-key v key-on drop  loop ;
: play-effect ( id -- )  full play-effect-at ;
: held ( bank k -- addr )  swap area-bank = if  #loops +  then  cells loops-held + ;
: free-loop ( bank -- k | -1 )  -1  #loops 0 do  over i held @ 0< if  drop i leave  then  loop  nip ;
: loop-on-at ( id volume -- )
    {: id v :}  id bank-of {: p b :}  b free-loop {: k :}  k 0< if  exit  then
    p b k held !  loop-voice b p -1 middle-key v key-on drop ;
: loop-on ( id -- )  full loop-on-at ;
: loop-stop ( id -- )
    bank-of {: p b :}  -1  #loops 0 do  b i held @ p = if  drop i  then  loop  {: k :}
    k 0< if  exit  then  -1 b k held !  b p middle-key loop-off ;

\ ---- footsteps (src: func_8001C57C and the areas' ground sounds): on set frames of the walking and running motions,
\ the ground's sound, quieter with the camera further (a twentieth of the distance off; out of hearing at 127) ----
$64 constant plain-ground
create grounds   \ area, scene (-1 any), its sound - by the areas' ground words (src: func_80104148 ...)
  0 , 2 , $72 ,   0 , 4 , $73 ,   0 , 5 , $73 ,   1 , 0 , $78 ,   1 , 1 , $65 ,   1 , 3 , $66 ,
  3 , 1 , $65 ,   3 , 0 , $6E ,   3 , 3 , $6E ,   4 , 0 , $65 ,   5 , 1 , $78 ,   5 , 0 , $65 ,
  7 , -1 , $65 ,  8 , 1 , $65 ,  10 , 1 , $65 ,  11 , 0 , $66 ,  12 , 0 , $65 ,  21 , 1 , $65 ,
  24 , 3 , $65 ,  28 , 0 , $65 ,  19 , 2 , $67 ,  19 , 4 , $67 ,  19 , 6 , $67 ,  19 , 3 , $68 ,
  19 , 5 , $68 ,  19 , 7 , $68 ,  26 , 7 , $66 ,  26 , 8 , $66 ,
here grounds - 3 cells / constant #grounds
create decks   \ area 26's wooden decks: x from, x to, z above, z to (src: func_80105074)
  $B4C3 , $B4C3 $18E5 + , $6571C , $65937 ,   $6582 , $6582 $C03 + , $63CB8 , $63F97 ,
  $3DAE , $3DAE $5F9 + , $69186 , $69C8C ,   $5439 , $5439 $ACA + , $68842 , $691E1 ,
4 constant #decks   $65 constant deck-sound
: deck? ( x z d -- flag )
    {: x z d :}  x d @ d cell+ @ within  z d 2 cells + @ >  and  z d 3 cells + @ <= and ;
: on-deck? ( x z -- flag )
    {: x z :}  #decks 0 do  x z decks i 4 * cells + deck? if  true unloop exit  then  loop  false ;
: ground-row ( k -- addr )  3 * cells grounds + ;
: here-ground? ( row -- flag )   \ this area, and this scene (or any)
    {: r :}  r @ area @ =  r cell+ @ 0<  r cell+ @ scene @ = or and ;
: ground ( x z -- id )
    area @ 26 = if  2dup on-deck? if  2drop deck-sound exit  then  then  2drop
    #grounds 0 do  i ground-row here-ground? if  i ground-row 2 cells + @ unloop exit  then  loop  plain-ground ;
create treads   \ a motion, then the frames of it that sound (ended by -1) - src: func_8001C57C's cases
  $1 , 3 , 9 , -1 ,   $5 , 3 , 9 , -1 ,   $BA , 3 , 9 , -1 ,   $159B , 3 , 9 , -1 ,   $159E , 3 , 9 , -1 ,
  $2 , 3 , 11 , -1 ,  $BD , 3 , 11 , -1 ,  $159C , 3 , 11 , -1 ,
  $3F , 20 , 31 , 42 , -1 ,   $40 , 40 , 45 , 50 , -1 ,
  $7 , 5 , 10 , -1 ,  $B , 5 , 10 , -1 ,   $8 , 4 , 9 , -1 ,   $C , 4 , 9 , -1 ,   $6 , 5 , -1 ,  $A , 5 , -1 ,
  $9 , 4 , -1 ,  $D , 4 , -1 ,
here constant treads-end
: treads-of ( id -- addr | 0 )   \ its frames
    treads begin  dup treads-end <  while
        2dup @ = if  nip cell+ exit  then
        cell+ begin  dup @ -1 <> while  cell+  repeat  cell+
    repeat  2drop 0 ;
: sounds-on? ( id frame -- flag )
    swap treads-of dup 0= if  nip exit  then
    begin  dup @ -1 <> while  2dup @ = if  2drop true exit  then  cell+  repeat  2drop false ;
20 constant hearing-step   127 constant out-of-hearing
: footfall-heard ( id frame x z -- )
    {: id f x z :}  id f sounds-on? 0= if  exit  then
    target@ nip {: cx cz :}  x cx - s>f fdup f*  z cz - s>f fdup f* f+ fsqrt f>s hearing-step / {: att :}
    att out-of-hearing >= if  exit  then
    x z ground  full att -  s" sound" actor-named send play-sound-at ;
' footfall-heard is footfall

\ ---- the car's engine (src: func_80022DF4 / func_80022E84, engine_sound): its tones held, the first's pitch moving ----
3 constant #engine   variable engine-going
create engine-voices  #engine cells allot   engine-voices #engine cells -1 fill
: engine-on ( id tones -- )   \ (he drives: the town's ambience minds it)
    true engine-going !
    {: id n :}  id bank-of {: p b :}
    n #engine min 0 ?do  effect-voice b p i middle-key full key-on  i cells engine-voices + !  loop ;
: engine-off ( -- )   \ (into their release)
    false engine-going !
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
: steps-sound-due ( -- n )  sound-due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  sound-due f! ;

\ ---- the areas' own ambience, a game frame at a time (src: func_80018748 and the areas' ambience words) ----
64 constant now-and-then   128 constant every-so-often   512 constant rarely   65536 constant very-rarely
: chance ( n -- flag )  random 0= ;
variable on-scene   \ (the scene a held loop was started for: -1 none)
: loop-in-scene ( id scene -- )   \ held while the area shows that scene
    {: id sc :}  scene @ sc = if  on-scene @ sc <> if  id loop-on  sc on-scene !  then
    else  on-scene @ sc = if  id loop-stop  -1 on-scene !  then  then ;
\ the bells: three tolls a frame count apart, in the first 11 minutes of 7, 12 and 17 o'clock
variable toll   11 constant bell-minutes   255 constant toll-count-top
: bell-hour? ( -- flag )
    clock-time time>hour {: h :}  clock-time 60 mod bell-minutes <  h 7 = h 12 = or h 17 = or and ;
: bells ( id apart -- )
    {: id ap :}  bell-hour? 0= if  0 toll ! exit  then
    toll @ 0=  toll @ ap = or  toll @ ap 2* = or if  id play-effect  then
    toll @ toll-count-top < if  1 toll +!  then ;
\ a river: a held voice louder as he comes near (src: func_80023254 / func_80023340 / func_800232EC)
variable river   -1 river !
: river-on ( id -- )
    river @ 0< 0= if  drop exit  then  bank-of {: p b :}  effect-voice b p first-tone middle-key 0 key-on river ! ;
: river-off ( -- )  river @ 0< if  exit  then  river @ key-off  -1 river ! ;
: river-volume ( v -- )  river @ 0< if  drop exit  then  river @ swap voice-volume ;
\ area 26 (src: func_801052C0): outdoors birds, a call, the wind and the river louder toward its x; the cabin's hum
$7D constant forest-birds   $7F constant forest-call   $80 constant forest-wind   60 constant wind-down
$81 constant forest-river   $3A98 constant river-x   300 constant river-step   60 constant river-loudest
$85 constant cabin-hum   7 constant the-cabin-scene
variable wind-on
: forest ( -- )
    scene @ 0= if
        now-and-then chance if  forest-birds play-effect  then
        every-so-often chance if  forest-call play-effect  then
        wind-on @ 0= if  forest-wind full wind-down - loop-on-at  true wind-on !  then
        forest-river river-on
        river-loudest  matthew-now 2drop drop river-x - river-step / river-loudest min -  river-volume
    else  wind-on @ if  forest-wind loop-stop  false wind-on !  then  river-off  then
    cabin-hum the-cabin-scene loop-in-scene ;
\ the town (src: func_80018748): now and then a sound; the bells; by the hour, birds, a call, the night's; the
\ river by the bridge (on foot)
$86 constant town-sound   $88 constant town-bell   120 constant town-toll-apart   64 constant town-quiet
$7D constant morning-birds   $7E constant afternoon-call   $85 constant night-call   60 constant birds-frames
$89 constant town-river   $6EC15 constant bridge-x0   $72B4 constant bridge-width   $44BEB constant bridge-z0
$4C175 constant bridge-z1   $725A6 constant bridge-x   $47718 constant bridge-z   10 constant river-loud
variable birds
: by-the-river? ( x z -- flag )  {: x z :}  x bridge-x0 bridge-x0 bridge-width + within  z bridge-z0 >  and  z bridge-z1 <= and ;
: town-hours ( -- )
    clock-time time>hour {: h :}
    h 7 12 within  rarely chance and  birds @ 0= and if  morning-birds full town-quiet - loop-on-at  1 birds !  then
    h 13 18 within  rarely chance and if  afternoon-call full town-quiet - play-effect-at  then
    h 19 24 within  rarely chance and if  night-call full town-quiet - play-effect-at  then ;
: town ( -- )
    now-and-then chance if  town-sound play-effect  then
    town-bell town-toll-apart bells
    engine-going @ 0= if  town-hours  then
    birds @ if  birds @ birds-frames < if  1 birds +!  else  morning-birds loop-stop  0 birds !  then  then
    matthew-now drop nip {: x z :}
    x z by-the-river?  engine-going @ 0= and if
        town-river river-on
        x bridge-x - s>f fdup f*  z bridge-z - s>f fdup f* f+ fsqrt f>s river-step / 0 max river-loud min
        river-loud swap - river-volume exit  then
    river-off ;
: area-sounds ( -- )   \ src: func_80018748
    area @ case
        5 of  $76 1 loop-in-scene  endof
        7 of  now-and-then chance if  $68 play-effect  then  very-rarely chance if  $69 play-effect  then  endof
        8 of  now-and-then chance if  $74 play-effect  then  endof
        10 of  scene @ 0= if  now-and-then chance if  $6E play-effect  then  then
            scene @ 0<> if  on-scene @ 0< if  $6F loop-on  scene @ on-scene !  then
            else  on-scene @ 0< 0= if  $6F loop-stop  -1 on-scene !  then  then  endof
        13 of  now-and-then chance if  $6F play-effect  then  endof
        23 of  scene @ 2 = now-and-then chance and if  $6E play-effect  then  $70 0 loop-in-scene  endof
        24 of  $73 60 bells  endof
        26 of  forest  endof
        the-town of  town  endof
    endcase
    area @ the-town <>  area @ 26 <> and if  river-off  then ;
: sound-tick ( -- )  steps-sound-due 0 ?do  cycle-frame  area @ 0< 0= if  area-sounds  then  loop ;

\ ---- XA (src: xa_play, xa_stop): TSG.XA's channel at its volume ----
create xa-volumes  $4D c, $4C c, $4E c, $52 c, $67 c, $6D c, $7F c,
7 constant #xa
: stream ( n -- )
    dup 0 #xa within 0= if  drop exit  then
    dup xa-volumes + c@  xa-start  true streaming ! ;
: stream-off ( -- )  xa-end  false streaming ! ;

: entered ( area -- )   \ src: area_sound_free, area_sound_load, area_sep_open
    background-off  #loops 2 * 0 do  -1 loops-held i cells + !  loop
    -1 on-scene !  0 toll !  0 birds !  false wind-on !  -1 river !
    open-area  0 cycle !  0e sound-due f! ;
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
  on play-sound-at ( id volume -- )  play-effect-at ;
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
