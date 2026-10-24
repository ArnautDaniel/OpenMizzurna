\ flare.fs - a flare's glow on the ground around someone (docs/subsystems/flare.md; src: snow.c ground_flare_draw,
\ minigames 38 and 40 in areas 24 and 5). A square of the area's glow picture lying at their feet, added onto the
\ scene, growing as the sine of its time: 0x10 a frame of the original's to a quarter turn - 256 either side at
\ its full size - and so it stays until Matthew leaves the area.
IN: flare
USING: engine actors messages areas figures townsfolk ;

<PRIVATE
8 constant above-the-floor   \ (the original drew by its draw order, no depth: lifted clear of the floor here)
$10 constant growth   $400 constant full-grown   256 constant full-half   128 constant as-it-is   2 constant adds
variable lit   variable who   variable grown
fvariable due
: picture ( -- image )   \ (the area's own)
    $clear  s" flares/" $+  area @ dup 100 < if  s" 0" $+  then  dup 10 < if  s" 0" $+  then  n>str $+  s" .png" $+
    $@ image ;
: half ( -- n )   \ src: rsin(t) >> 4
    grown @ s>f 2e f* pi f* 4096e f/ fsin full-half s>f f* f>s ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: grow ( -- )  lit @ 0= if  exit  then  steps-due growth * grown +!  grown @ full-grown min grown ! ;
: draw-flare ( -- )
    lit @ 0= if  exit  then
    picture  who @ person-now drop {: x y z :}  x  y above-the-floor -  z  half  as-it-is adds  draw-ground-quad ;
' draw-flare on-draw
PRIVATE>

state: flare-state end-state
behaviour flares
  on spawned ( -- )  self subscribe tick  self subscribe area-entered ;
  on tick ( -- )  grow ;
  on flare-on ( slot -- )  who !  0 grown !  0e due f!  true lit ! ;
  on area-entered ( area scene -- )  2drop  false lit ! ;
end-behaviour

: spawn-flare ( -- id )  flares flare-state s" flare" spawn ;
