\ rumble.fs - the pads' motors (docs/subsystems/rumble.md; src: frame.c pad_read_port's motors, cevent's op $155).
\ A pad has two: the small, on or off, and the large at a strength (0 - 255); each runs for so many of the
\ original's pad frames (60 a second; -1: until told otherwise) and then stops.
IN: rumble
USING: engine actors messages ;

<PRIVATE
2 constant #ports   60e fconstant pad-frames
begin-structure motors
  ffield: small-left   ffield: large-left   field: large-strength
end-structure
create ports  motors #ports * allot   ports motors #ports * erase
: port# ( n -- p )  motors * ports + ;
: running? ( F: left -- flag )  f0= 0= ;
: wear ( p -- )   \ a tick off each running motor (-1: on for ever)
    {: p :}  p small-left f@ f0> if  p small-left f@ dt pad-frames f* f- 0e fmax p small-left f!  then
    p large-left f@ f0> if  p large-left f@ dt pad-frames f* f- 0e fmax p large-left f!  then ;
: drive ( n -- )
    dup port# {: n p :}  p wear
    n  p small-left f@ running?  p large-left f@ running? if  p large-strength @  else  0  then  pad-rumble ;
: rumble-tick ( -- )  #ports 0 do  i drive  loop ;
PRIVATE>

state: rumble-state end-state
behaviour motors-runner
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  rumble-tick ;
  on rumble-small ( port frames -- )  swap port# {: n p :}  n s>f p small-left f! ;   \ (frames: -1 for ever)
  on rumble-large ( port strength frames -- )
    rot port# {: v n p :}  v p large-strength !  n s>f p large-left f! ;
  on rumble-stop ( -- )  ports motors #ports * erase ;
end-behaviour

: spawn-rumble ( -- id )  motors-runner rumble-state s" rumble" spawn ;
\ (the others' way to it)
: shake ( small-frames strength large-frames -- )   \ the first pad: the small for so long, the large so hard for so long
    {: sf v lf :}  s" rumble" actor-named {: r :}  0 sf r send rumble-small  0 v lf r send rumble-large ;
