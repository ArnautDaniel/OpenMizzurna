\ clock.fs - the game's date and time (docs/subsystems/clock.md; src: clock.c clock_tick / clock_set,
\ field.c). The clock actor keeps the time and runs it while nothing holds it; others hear minute-passed
\ and time-set.
IN: clock
USING: engine actors messages ;

\ ---- times: whole game minutes since 12/25 00:00 ----
25 constant first-day    31 constant last-day     \ the dates the game runs (12/25 .. 12/31)
60 constant minutes/hour   24 constant hours/day
minutes/hour hours/day * constant minutes/day
60e fconstant seconds/minute
1e-6 fconstant a-hair   \ (a step's game seconds don't sum exactly: 150 x 0.4 is a hair under 60)

: date>time ( day hour minute -- t )  >r  minutes/hour *  swap first-day - minutes/day *  +  r> + ;
: time>day ( t -- day )  minutes/day / first-day + ;
: time>hour ( t -- hour )  minutes/day mod minutes/hour / ;
: time>minute ( t -- minute )  minutes/hour mod ;
: time>date ( t -- day hour minute )  dup time>day  over time>hour  rot time>minute ;

25 9 0 date>time constant new-game-time   \ 12/25 09:00 (src: boot.c clock_set(25, 9, 0, 0))

\ ---- the pace: game seconds a real second ----
\ src: 0.8 of a game second a frame (clock_tick: $30 / $3C), indoors at 15 frames a second (VSync(4)),
\ in the town at ~30 (no frame wait)
12e fconstant indoor-pace    \ a minute every 5 s
24e fconstant town-pace      \ a minute every 2.5 s

\ ---- the actor ----
state: clock-state
  cell field now            \ the time
  1 floats field second     \ game seconds into the minute (0 .. 60)
  cell field holds          \ a bit a clock-hold reason
  cell field town?
end-state

: hold-bit ( reason -- mask )  1 swap lshift ;
: running? ( -- flag )  holds @ 0= ;
: pace ( F: -- p )  town? @ if  town-pace  else  indoor-pace  then ;
: jump ( t -- )  dup now !  0e second f!  broadcast time-set ;
: minute-on ( -- )  1 now +!  now @ broadcast minute-passed ;
: run ( -- )   \ a step's worth of game seconds; each whole minute told
    second f@  pace dt f* f+
    begin  fdup seconds/minute a-hair f- f>= while  seconds/minute f-  minute-on  repeat
    0e fmax second f! ;

behaviour clock
  on spawned ( -- )  self subscribe tick  new-game-time now !  story-stopped hold-bit holds ! ;
  on tick ( -- )  running? if  run  then ;
  on set-time ( t -- )  jump ;
  on advance-minute ( -- )  now @ 1+ jump ;
  on hold-clock ( reason -- )  hold-bit holds @ or holds ! ;
  on release-clock ( reason -- )  hold-bit invert holds @ and holds ! ;
  on in-town ( flag -- )  town? ! ;
  on what-time ( -- )  now @ sender send the-time ;
end-behaviour

: spawn-clock ( -- id )  clock clock-state s" clock" spawn ;
