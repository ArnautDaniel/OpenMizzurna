\ items.fs - what Matthew carries (docs/subsystems/items.md; src: items.c): a list of up to 27 items, each
\ once, in the order taken (a removed one's place taken by the last). The list is a fact (has-item?); the
\ items actor changes it. The item ring (the screen) comes later.
IN: items
USING: engine actors messages ;

27 constant max-items
\ the items named so far (their numbers; the text's names with the screen)
0 constant cell-phone   3 constant waist-bag   4 constant watch
7 constant isabellas-letter   9 constant lighter   20 constant cigarettes

create held max-items allot   variable #held
: has-item? ( item -- flag )  #held @ 0 ?do  held i + c@ over = if  drop true unloop exit  then  loop  drop false ;
: add ( item -- )  dup has-item? #held @ max-items >= or if  drop exit  then  held #held @ + c!  1 #held +! ;
: remove ( item -- )   \ (the last takes its place, as the original)
    #held @ 0 ?do
        held i + c@ over = if  drop  -1 #held +!  held #held @ + c@ held i + c!  unloop exit  then
    loop  drop ;
: reset ( -- )  0 #held !  cell-phone add  waist-bag add  watch add ;
: items-mask ( -- mask )  0  #held @ 0 ?do  1 held i + c@ lshift or  loop ;   \ (a fact: src items_mask)
reset

state: items-state end-state
behaviour carrying
  on give-item ( item -- )  add ;
  on take-item ( item -- )  remove ;
  on items-reset ( -- )  reset ;
  on items-set ( mask -- )   \ src: items_from_mask: the three first, then the rest by number
    reset  32 0 do  dup 1 i lshift and if  i add  then  loop  drop ;
end-behaviour

: spawn-items ( -- id )  carrying items-state s" items" spawn ;
