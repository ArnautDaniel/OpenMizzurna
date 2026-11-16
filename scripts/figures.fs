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
    $clear  s" models/" $+  dup 5 / three-digits
    s" _" $+  5 mod n>str $+  s" .gltf" $+  $@ ;

: area-first-motion ( area -- id )  >r s" area-motion-first" r> exe-s16 ;
: motion-path ( id -- addr len )   \ "cdbin/033_4/1/<id>.motion.txt", or the area's "cdbin/NNN_4/1/<i>..."
    $clear  s" cdbin/" $+
    dup shared-motions < if
        s" 033" $+
    else
        area @ three-digits
        area @ area-first-motion -
    then
    s" _4/1/" $+  n>str $+  s" .motion.txt" $+  $@ ;
: motion-of ( id -- motion )  motion-path motion ;
: motion-rate ( F: -- frames/s )  game-rate ;   \ (a motion's frames are game frames)

\ a figure's motion: what plays, how far in, what follows (src: the Actor's anim, anim_queued, anim_loop, anim_next,
\ anim_done; func_8002032C plays them)
begin-structure playing
  field: anim        \ the id playing (-1: none: its rest pose)
  field: clip        \ its motion (0: none)
  ffield: at         \ frames in
  field: looping     \ it plays round again - and so does the one queued after it
  field: queued-anim      \ played once this one has played, with the same looping (-1: none)
  field: next-anim   \ after one that doesn't loop, with none queued: played on and on (-1: its last frame held)
  field: done        \ one that doesn't loop, with none queued, reached its end (the events' wait-anim)
  field: hold-move   \ at its end the figure keeps the root's move (src: anim_hold)
end-structure
: switch ( id p -- )   \ to that motion, from its start
    {: id p :}  id p anim !  id 0< if  0  else  id motion-of  then p clip !  0e p at f! ;
: play-queued ( id queued loop next p -- )   \ src: func_8001FF34 (actor_anim_then), actor_anim_set
    {: id q lp nx p :}
    id p anim @ <>  p done @ or  p clip @ 0= or if  id p switch  then   \ (the one playing asked again goes on: src
    q p queued-anim !  lp p looping !  nx p next-anim !  false p hold-move !  \  func_8002032C's anim_prev)
    p clip @ 0= p done ! ;   \ (a motion that isn't there has played already)
: play ( id next loop p -- )  {: id nx lp p :}  id -1 lp nx p play-queued ;   \ none queued
: rest ( p -- )  -1 swap anim ! ;
: frame ( p -- n )  at f@ f>s ;
: hold-last ( p -- )  dup clip @ motion-frames 1- s>f  at f! ;
: advance ( p -- end? )   \ a step on; at the end of one that doesn't loop (nor has one queued): true once
    {: p :}  p clip @ 0= if  false exit  then
    p at f@  motion-rate dt f* f+  p at f!
    p frame p clip @ motion-frames < if  false exit  then
    p queued-anim @ 0< 0= if  p queued-anim @ p switch  -1 p queued-anim !  false exit  then
    p looping @ if  p at f@  p clip @ motion-frames s>f f-  p at f!  false exit  then
    p done @ if  p hold-last  false exit  then
    true p done !   \ (src: then anim = anim_next: it plays on, round and round - or, none, the last frame held)
    p next-anim @ 0< if  p hold-last  else  p next-anim @ p switch  true p looping !  then
    true ;

\ walking to a point (src: actor_nav_step): a walk's motion -1 keeps the one playing, there too (else it stands:
\ motion 0, looping); these walk backwards - the figure faces away from where it goes (+$1800)
0 constant standing-still   $800 constant about-turn
create backward-walks  2 , $80 , $71 , $11BA ,
: backward-walk? ( motion -- flag )  false  4 0 do  over backward-walks i cells + @ = or  loop  nip ;
: walk-facing ( heading motion -- heading' )  backward-walk? if  about-turn + 4095 and  then ;
: walk-motion! ( motion p -- )  {: m p :}  m 0< if  exit  then  m -1 true p play ;   \ (looping)
: walk-over ( motion p -- )  {: m p :}  m 0< if  exit  then  standing-still -1 true p play ;

\ a figure's footfalls (src: func_8001C57C, on the frames of its motion): the sound's to say what's heard
defer footfall ( id frame x z -- )
:noname ( id frame x z -- )  2drop 2drop ; is footfall
: advance-heard ( p x z -- end? )   \ advance, telling footfall of the frame it reaches (where the figure stands)
    {: p x z :}  p frame {: was :}  p advance  p frame {: now :}
    now was <>  p anim @ 0< 0= and if  p anim @ now x z footfall  then ;


\ ---- turning at a speed (src: actor_turn_by / _to_heading / _to_actor and actor_update's turn: the heading moves
\ the speed a game frame - its sign the way - until it passes the target, then stands on it) ----
4096 constant full-circle
: heading-step ( heading target speed -- heading' arrived? )   \ a step of this update's game frames
    {: h t v :}  v 0< if  h t -  else  t h -  then  full-circle 1- and {: left :}   \ (how far, going that way)
    v abs s>f game-frames f* f>s {: step :}
    step left >= if  t true exit  then
    v 0< if  h step -  else  h step +  then  full-circle 1- and  false ;
: way-between ( heading target -- d )   \ the shorter way round from one to the other: -2048 .. 2047 (+ the way
    swap - full-circle 2/ + full-circle 1- and full-circle 2/ - ;   \  headings grow)
: degrees>heading ( degrees -- d )   \ (the scripts' turn-by counts degrees; src: actor_turn_by's C division,
    dup abs full-circle * 360 /  swap 0< if  negate  then ;   \  toward 0)
