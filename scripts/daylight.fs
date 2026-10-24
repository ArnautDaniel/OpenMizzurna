\ daylight.fs - the day's light and the weather (docs/subsystems/daylight.md; src: clock.c). Outdoors the light
\ is the hour's: three lights, an ambient and the fog's colour from the executable's tables by the hour, the
\ minute's value between this hour's and the next's, the colours easing toward it a step a frame; grey when it
\ isn't clear. Indoors a scene's own light record says how it mixes with the hour's (src: scene_lights_apply).
\ The engine draws the lit models in it (light-way, light-color, ambient-light, fog-color, fog!).
IN: daylight
USING: engine actors messages clock areas ;

\ ---- the weather (src: func_800252A4, clock_tick) ----
enum: weathers  clear overcast snowing ;
variable weather   variable weather-held   variable weather-left
90 constant weather-shortest   128 constant weather-spread   \ (minutes it lasts: 90 .. 217)
: the-weather ( -- w )  weather @ ;
<PRIVATE
: reroll ( -- )   \ each minute: a new weather when its time is up (unless an event holds it)
    weather-held @ if  exit  then
    weather-left @ 0= if  weather-spread random weather-shortest + weather-left !  3 random weather !  then
    -1 weather-left +! ;
: seasonal ( t -- )   \ it snows on the morning of 12/25 and clears on the last evening
    time>date drop {: day hour :}
    day first-day = hour 12 < and if  snowing weather !  then
    day last-day = hour 20 >= and if  clear weather !  then ;
PRIVATE>

\ ---- the hour's values (src: func_800234B4, func_800253C8) ----
24 constant hours
: hour-now ( -- hour next minute )
    clock-time {: t :}  t time>hour  dup 1+ hours mod  t time>minute ;
: between ( this next minute level -- n )   \ the minute's value, scaled to the scene's level (100: as it is)
    {: a b m lv :}  b a - s>f 60e f/ m s>f f* a s>f f+  lv 100 <> if  100e f/ lv s>f f*  then  f>s ;
: hour-u8 ( addr len channel level -- n )   \ a byte table: channel 0..2 (red green blue, x y z) of 24 each
    {: a n c lv :}  hour-now {: h h' m :}
    a n c hours * h + exe-u8  a n c hours * h' + exe-u8  m lv between ;
: hour-s16 ( addr len channel level -- n )
    {: a n c lv :}  hour-now {: h h' m :}
    a n c hours * h + exe-s16  a n c hours * h' + exe-s16  m lv between ;
<PRIVATE
: light-colors ( i -- addr len )  case 0 of s" hour-light-1" endof 1 of s" hour-light-2" endof s" hour-light-3" rot endcase ;
: light-ways ( i -- addr len )  case 0 of s" hour-light-1-way" endof 1 of s" hour-light-2-way" endof s" hour-light-3-way" rot endcase ;
PRIVATE>

\ the light: what it should be, and what it is (colours ease toward it); three of each
begin-structure rgb  field: red  field: green  field: blue  end-structure
: rgb@ ( addr -- r g b )  dup red @  over green @  rot blue @ ;
: rgb! ( r g b addr -- )  tuck blue !  tuck green !  red ! ;
create ways  3 rgb * allot          \ the lights' ways (x y z in the rgb fields)
create colors  3 rgb * allot   create colors-now  3 rgb * allot
create ambient  rgb allot   create ambient-now  rgb allot
create fog  rgb allot   create fog-now  rgb allot
: light ( table i -- addr )  rgb * + ;

<PRIVATE
63 constant least-red   63 constant least-green   75 constant least-blue   \ the hour's ambient at least
: hour-rgb ( addr len level dest -- )   \ a colour table's minute value
    {: a n lv d :}  a n 0 lv hour-u8  a n 1 lv hour-u8  a n 2 lv hour-u8  d rgb! ;
: grey ( addr -- )  {: a :}  a rgb@ + + 3 /  dup dup  a rgb! ;   \ (src: overcast and snow: the colours' mean)
: the-hours ( level -- )   \ the hour's targets
    {: lv :}
    3 0 do
        i light-ways 0 lv hour-s16  i light-ways 1 lv hour-s16  i light-ways 2 lv hour-s16  ways i light rgb!
        i light-colors lv  colors i light  hour-rgb
    loop
    s" hour-ambient" lv ambient hour-rgb
    ambient rgb@  rot least-red max  rot least-green max  rot least-blue max  ambient rgb!
    s" hour-fog" lv fog hour-rgb
    the-weather clear <> if  3 0 do  colors i light grey  loop  ambient grey  fog grey  then ;
: ease ( now target -- )   \ a step of 1 toward it, each colour (src: step_u8)
    {: n t :}  3 0 do
        n i cells + @  t i cells + @  2dup < if  2drop 1 n i cells + +!  else  > if  -1 n i cells + +!  then  then
    loop ;
: ease-all ( -- )  3 0 do  colors-now i light  colors i light ease  loop  ambient-now ambient ease  fog-now fog ease ;
: jump-all ( -- )   \ at once (src: clock_set's set forms)
    colors colors-now 3 rgb * move  ambient ambient-now rgb move  fog fog-now rgb move ;

16 constant outdoor-ambient   10 constant indoor-ambient   \ the ambient's scale (src: GsSetAmbient's: x16, x10)
: outdoors ( -- )   \ src: func_80023D70: the hour's lights, eased, and the fog
    3 0 do  i  ways i light rgb@  light-way   i  colors-now i light rgb@  light-color  loop
    ambient-now rgb@  rot outdoor-ambient *  rot outdoor-ambient *  rot outdoor-ambient *  ambient-light
    fog-now rgb@ fog-color  true fog! ;

\ a scene's light record (the area's light table: src scene_lights, 34 bytes a scene)
34 constant scene-light-record   10 constant scene-light   6 constant light-rgb
29 constant scene-ambient   32 constant scene-level   33 constant scene-mode
2 constant hour-light-mode      \ (mode 2: the hour's light and the fog, as outdoors)
: light-record ( -- addr )  light-table table  scene @ scene-light-record * + ;
: sum ( addr -- n )  dup c@  over 1+ c@ +  swap 2 + c@ + ;
: scene-way ( i -- x y z )  scene-light * light-record + {: a :}  a sw@  a 2 + sw@  a 4 + sw@ ;
: scene-rgb ( i -- addr )  scene-light * light-rgb + light-record + ;
: way-of ( i -- x y z )   \ the brighter light's way: the scene's or the hour's
    {: i :}  i scene-rgb sum  colors i light rgb@ + + <  if  ways i light rgb@  else  i scene-way  then ;
: brighter ( mine hours mode -- n )  {: s h md :}  s h <  md 0= or if  h  else  s  then ;
: mixed ( mode -- )   \ src: scene_lights_apply: a light's way the brighter one's; each colour the brighter
    {: md :}                \ (mode 0: the hour's colours always)
    3 0 do
        i  i way-of  light-way
        i  i scene-rgb c@  colors i light red @  md brighter
           i scene-rgb 1+ c@  colors i light green @  md brighter
           i scene-rgb 2 + c@  colors i light blue @  md brighter  light-color
    loop
    light-record scene-ambient + {: a :}
    a c@ ambient red @ md brighter indoor-ambient *
    a 1+ c@ ambient green @ md brighter indoor-ambient *
    a 2 + c@ ambient blue @ md brighter indoor-ambient *  ambient-light
    false fog! ;
: indoors ( -- )
    light-record scene-mode + c@ {: md :}
    md hour-light-mode = if  outdoors  else  md mixed  then ;
: level ( -- pct )  area @ the-town = if  100  else  light-record scene-level + c@  then ;
\ a shot's flash (src: shootout_flash_lights, muzzle_flash_lights): for a frame three bright lights, the scene's
\ after
create flash-ways  $298 , $283 , -383 ,   0 , $157 , $3AC ,   -813 , $157 , -469 ,
create flash-colors  $FF , $FF , $C8 ,   $FF , $FF , $96 ,   $FF , $FF , $64 ,
variable flash-due
: triple ( table i -- x y z )  3 * cells + {: a :}  a @  a cell+ @  a 2 cells + @ ;
: flash ( -- )
    3 0 do  i  flash-ways i triple  light-way   i  flash-colors i triple  light-color  loop
    false flash-due ! ;
: lighting ( -- )   \ each frame (src: func_80024634)
    area @ 0< if  exit  then
    level the-hours  ease-all
    flash-due @ if  flash exit  then
    area @ the-town = if  outdoors  else  indoors  then ;
PRIVATE>

state: daylight-state end-state
behaviour the-daylight
  on spawned ( -- )  self subscribe tick  self subscribe minute-passed  self subscribe time-set
    self subscribe area-entered ;
  on tick ( -- )  lighting ;
  on minute-passed ( t -- )  reroll  seasonal ;
  on set-weather ( w hold -- )  weather-held !  weather ! ;
  on muzzle-flash ( -- )  true flash-due ! ;
  on time-set ( t -- )  seasonal  area @ 0< 0= if  level the-hours  jump-all  then ;
  on area-entered ( area scene -- )  2drop  level the-hours  jump-all ;
end-behaviour

: spawn-daylight ( -- id )  the-daylight daylight-state s" daylight" spawn ;
