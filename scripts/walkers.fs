\ walkers.fs - figures walked along the town's footpaths (docs/subsystems/walkers.md; src: walker.c walker_start /
\ walker_remove, carline.c func_8003C08C). The paths are a graph (the common container's 033_4 part 5 entry 0: 32
\ bytes a node - x, z, height; for each of the first 24 nodes as a destination, which of its links leads there; its
\ four neighbours and the lengths to them). A walker goes edge by edge toward its destination at its speed (units a
\ frame of the original's), facing along the edge - half way round at once at a node - in its motion; there it
\ stands and is done. A step at the original's rate (30 a second in the town).
IN: walkers
USING: engine actors messages areas figures townsfolk ;

<PRIVATE
32 constant node-bytes   8 constant height-at   10 constant routes-at   16 constant neighbours-at   24 constant lengths-at
16 constant #walkers   0 constant stands
: graph ( -- addr )  s" cdbin/033_4/5/0.bin" library-file drop ;
: node ( i -- addr )  node-bytes * graph + ;
: node-x ( i -- x )  node sl@ ;
: node-z ( i -- z )  node 4 + sl@ ;
: node-y ( i -- y )  node height-at + sw@ ;
: link-toward ( i dest -- link )   \ (2 bits a destination)
    {: i d :}  i node routes-at + d 4 / + c@  d 4 mod 2* rshift 3 and ;
: neighbour ( i link -- j )  2* neighbours-at + swap node + sw@ ;
: length ( i link -- n )  2* lengths-at + swap node + w@ ;
: heading-to ( i j -- h )   \ from node i toward node j (4096 a turn)
    {: i j :}  j node-x i node-x - s>f  j node-z i node-z - s>f  fatan2 4096e f* 2e pi f* f/ f>s 4095 and ;

begin-structure walker
  field: who   field: from   field: to   field: dest   field: step   field: edge-steps   field: speed   field: heading
  field: gait
end-structure
create walkers-list  walker #walkers * allot
: walker# ( k -- w )  walker * walkers-list + ;
: clear-walkers ( -- )  #walkers 0 do  -1 i walker# who !  loop ;
clear-walkers
: walker-of ( slot -- w | 0 )
    #walkers 0 do  i walker# who @ over = if  drop i walker# unloop exit  then  loop  drop 0 ;
: free-walker ( -- w | 0 )  -1 walker-of ;
fvariable due

: edge ( w -- )   \ from its node toward the destination: the next edge
    {: w :}  w from @ w dest @ link-toward {: k :}
    w from @ k neighbour w to !  0 w step !
    w from @ k length  w speed @ /  1 max w edge-steps !
    w from @ w to @ heading-to w heading ! ;
: the-townsfolk ( -- id )  s" townsfolk" actor-named ;
: place ( w -- )   \ along its edge, facing along it, in its motion
    {: w :}  w step @ w edge-steps @ {: s n :}
    w who @  w from @ node-x  w to @ node-x w from @ node-x - s * n / +
    w from @ node-z  w to @ node-z w from @ node-z - s * n / +  the-townsfolk send person-place
    w who @ w heading @ the-townsfolk send person-turn ;
: walk-done ( w -- )  {: w :}  w who @ stands -1 true the-townsfolk send person-anim  -1 w who ! ;
: at-node ( w -- )   \ the next edge, turning half way at once; or the destination
    {: w :}  w to @ w dest @ = if  w walk-done exit  then
    w heading @ {: old :}  w to @ w from !  w edge
    w heading @ old - 2048 + 4095 and 2048 - 2/ old + 4095 and  w who @ swap the-townsfolk send person-turn ;
: walker-step ( w -- )
    {: w :}  w who @ 0< if  exit  then
    1 w step +!  w place
    w step @ w edge-steps @ >= if  w at-node  then ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: walk ( -- )  steps-due 0 ?do  #walkers 0 do  i walker# walker-step  loop  loop ;

: retarget ( w d -- )   \ walking already: toward d now - back the way it came if that's the way there
    {: w d :}  d w dest !
    w to @ d link-toward {: k :}  w to @ k neighbour w from @ <> if  exit  then
    w to @  w from @ w to !  w from !  w edge-steps @ w step @ - w step !  w heading @ 2048 + 4095 and w heading ! ;
: new-walker ( w slot node dest speed motion -- )
    {: w s n d v m :}
    s w who !  n w from !  d w dest !  v w speed !  m w gait !  w edge
    s n node-x n node-y n node-z the-townsfolk send extra-place
    s w heading @ the-townsfolk send person-turn  s m -1 true the-townsfolk send person-anim ;
: begin-walk ( slot node dest speed motion -- )   \ src: walker_start, walker_set_mode
    {: s n d v m :}
    s walker-of ?dup if  d retarget exit  then
    free-walker ?dup if  s n d v m new-walker  then ;
: end-walk ( slot -- )  walker-of ?dup if  walk-done  then ;   \ src: walker_remove: it stands
PRIVATE>

: walker-active? ( slot -- flag )  walker-of 0<> ;   \ (a fact: walking a path now)

state: walkers-state end-state
behaviour footpaths
  on spawned ( -- )  self subscribe tick  self subscribe area-entered ;
  on tick ( -- )  walk ;
  on walker-start ( slot node dest speed motion -- )  begin-walk ;
  on walker-stop ( slot -- )  end-walk ;
  on area-entered ( area scene -- )  2drop  clear-walkers ;
end-behaviour

: spawn-walkers ( -- id )  footpaths walkers-state s" walkers" spawn ;
