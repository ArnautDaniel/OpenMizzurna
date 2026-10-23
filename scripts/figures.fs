\ figures.fs - the people's figures in motion (docs/subsystems/matthew.md; src: actor_anim.c): which motion
\ an animation id names, and a motion played in real time. A figure's motions advanced one frame a game
\ frame - 15 a second indoors, 30 in the town - and so they play here at those rates.
IN: figures
USING: engine actors messages areas ;

193 constant shared-motions   \ ids below are the common set (CD.BIN 033_4 part 1); from here an area's own
                              \ (src: func_80020464, 0xC1)
$16E constant character-entries   \ the kinds' models: CD.BIN entries from here
: kind-model ( slot -- addr len )   \ "models/NNN_K.gltf": the kind's model (a slot is its kind)
    {: s :}  s" kind-models" s exe-u8 character-entries +
    $clear  s" models/" $+  dup 5 / dup 100 < if  s" 0" $+  then  dup 10 < if  s" 0" $+  then  n>str $+
    s" _" $+  5 mod n>str $+  s" .gltf" $+  $@ ;
15e fconstant indoor-motion-rate   \ frames a second
30e fconstant town-motion-rate

: area-first-motion ( area -- id )  >r s" area-motion-first" r> exe-s16 ;
: motion-path ( id -- addr len )   \ "cdbin/033_4/1/<id>.motion.txt", or the area's "cdbin/NNN_4/1/<i>..."
    $clear  s" cdbin/" $+
    dup shared-motions < if
        s" 033" $+
    else
        area @ dup 100 < if  s" 0" $+  then  dup 10 < if  s" 0" $+  then  n>str $+
        area @ area-first-motion -
    then
    s" _4/1/" $+  n>str $+  s" .motion.txt" $+  $@ ;
: motion-of ( id -- motion )  motion-path motion ;
: motion-rate ( F: -- frames/s )  area @ the-town = if  town-motion-rate  else  indoor-motion-rate  then ;

\ a figure's motion: what plays, how far in, what follows
begin-structure playing
  field: anim        \ the id playing (-1: none: its rest pose)
  field: clip        \ its motion (0: none)
  ffield: at         \ frames in
  field: looping
  field: next-anim   \ after it (-1: hold its last frame)
  field: done        \ a motion that doesn't loop reached its end (the events' wait-anim)
  field: hold-move   \ at its end the figure keeps the root's move (src: anim_hold)
end-structure
: play ( id next loop p -- )
    {: id nx lp p :}  id p anim !  id 0< if  0  else  id motion-of  then p clip !
    0e p at f!  lp p looping !  nx p next-anim !  false p hold-move !
    p clip @ 0= p done ! ;   \ (a motion that isn't there has played already)
: rest ( p -- )  -1 swap anim ! ;
: frame ( p -- n )  at f@ f>s ;
: advance ( p -- end? )   \ a step on; at the end of one that doesn't loop: true once
    {: p :}  p clip @ 0= if  false exit  then
    p at f@  motion-rate dt f* f+  p at f!
    p frame p clip @ motion-frames < if  false exit  then
    p looping @ if  p at f@  p clip @ motion-frames s>f f-  p at f!  false exit  then
    p done @ if  p clip @ motion-frames 1- s>f p at f!  false exit  then
    true p done !  p clip @ motion-frames 1- s>f p at f!
    p next-anim @ 0< 0= if  p next-anim @ -1 true p play  then
    true ;

\ ---- the original's game frames: the figures moved a step each (15 a second indoors, ~30 in the town) ----
15e fconstant indoor-game-frames   30e fconstant town-game-frames
: game-frames ( F: -- n )   \ how many of them this step covers
    area @ the-town = if  town-game-frames  else  indoor-game-frames  then  dt f* ;
