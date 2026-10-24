\ fight.fs - a fistfight (docs/subsystems/fight.md; src: fight.c fight_start / fight_update, the SUGI code; the
\ moves, attacks and the AI's ways: the executable's tables). Two fighters, each the player's (the pad) or the
\ AI's (a style: its row of chances and ranges). A fighter ready (a move of kind 2) takes the pad's move or the
\ AI's; an attack lands on its frame when in reach and the other isn't down or dodging: damage, the other's
\ reaction, a push back. 120 health each; at 0 a knock-out, and when the fallen one is down for good the other
\ wins. A loss to the AI is game over, the fight again from the start for ○ or Start. They face each other, the
\ camera between them; a step at the original's rate (15 a second indoors, 30 in the town).
IN: fight
USING: engine actors messages keys screen areas floors figures matthew townsfolk sound ;

\ ---- facts ----
variable fighting      \ a fight is on (src: fight_active)
variable fight-winner  \ 0 the first, 1 the second (src: fight_winner; 7 none yet)
: fighting? ( -- flag )  fighting @ 0<> ;

<PRIVATE
0 constant player-1   1 constant player-2   7 constant styles-own-motions   8 constant style-never-wins
7 constant no-winner
120 constant full-health
13 constant ready-move   14 constant dodge   15 constant knocked-out   16 constant down-for-good
17 constant light-hit   18 constant kicked   24 constant step-in   25 constant step-back   26 constant dash-in
27 constant dash-back   28 constant taunt   29 constant victory
0 constant jab   1 constant hook   2 constant kick   3 constant dash-back-punch   4 constant lunge   5 constant back-punch
6 constant special-6   9 constant forward-punch   10 constant back-kick   13 constant attacks
2 constant ready   1 constant not-hittable   \ a move's kind
$20 constant step-in-units   $1C constant step-back-units   $58 constant dash-units   $10 constant dash-punch-units
$96 constant lunge-units   4 constant double-tap-frames   $E0 constant too-close   100 constant ko-push
32 constant streak-to-mood   $600 constant far-apart   $300 constant close-again
324 constant jab-reach   480 constant kick-reach   $145 constant closing-in   $177 constant pressing-in
\ the camera: between them (src: fight_update)
$1A2 negate constant fight-look-y   $3CA negate constant fight-eye-y   $584 constant fight-dist-base

\ the tables (the executable's)
: move-motion ( m -- id )  >r s" fight-moves" r> 2* exe-s16 ;
: move-kind ( m -- k )  >r s" fight-moves" r> 4 * 2 + exe-u8 ;
: move-next ( m -- m' )  >r s" fight-moves" r> 4 * 3 + exe-u8 ;
: hit-frame ( m -- f )  >r s" fight-attacks" r> 4 * exe-s16 ;
: reach ( m -- d )  >r s" fight-attacks" r> 4 * 1+ exe-s16 ;
: damage ( m -- n )  >r s" fight-attacks" r> 8 * 4 + exe-u8 ;
: reaction ( m -- m' )  >r s" fight-attacks" r> 8 * 5 + exe-u8 ;
: push ( m -- n )  >r s" fight-attacks" r> 4 * 3 + exe-s16 $FFFF and ;
: ai ( style i -- u8 )  swap 2 - 20 * + >r s" fight-ai" r> exe-u8 ;
: ai16 ( style i -- s16 )  swap 2 - 20 * + 2/ >r s" fight-ai" r> exe-s16 ;
: step-of ( addr len f n -- d )  over swap 0 swap within if  exe-s8  else  drop 2drop 0  then ;   \ (past its end: 0)
12 constant hook-steps   16 constant down-steps   20 constant ko-steps   16 constant special-steps

\ ---- the two ----
begin-structure fighter
  field: slot   field: style   field: move   field: move-frame   field: health   field: shown
  field: combo   field: pushed   field: tap   field: mood
  field: fx   field: fz   field: heading   field: start-x   field: start-z   field: last-motion
  field: was-x   field: was-z
end-structure
create fighters  fighter 2 * allot
: fighter# ( i -- f )  fighter * fighters + ;
: other ( i -- o )  1 xor ;
variable ko   variable streak-victim   variable streak
variable lost   \ 0 on, 1 the game over's up, 2 fading out to try again (src: fight_active 2, 3)
fvariable due

: human? ( f -- flag )  style @ player-2 <= ;
: the-player? ( f -- flag )  style @ player-1 = ;
: him? ( f -- flag )  slot @ 0= ;   \ (Matthew's slot)
: place-of ( slot -- x z )  dup 0= if  drop matthew-now drop nip  else  person-now drop nip  then ;
: floor-of ( f -- n )  dup him? if  drop floor @  else  slot @ person-of on-floor @  then ;
: motion-done? ( f -- flag )  dup him? if  drop anim-done?  else  slot @ person-anim-done?  then ;
: step ( f d -- )   \ along its heading
    {: f d :}  f heading @ s>f 2e f* pi f* 4096e f/ {: F: a :}
    f fx @ s>f a fsin d s>f f* f+ f>s f fx !  f fz @ s>f a fcos d s>f f* f+ f>s f fz ! ;
: apart ( -- d )  0 fighter# fx @ 1 fighter# fx @ - s>f fdup f*  0 fighter# fz @ 1 fighter# fz @ - s>f fdup f* f+ fsqrt f>s ;
: angle ( -- a )   \ from the second toward the first (4096 a turn)
    0 fighter# fx @ 1 fighter# fx @ - s>f  0 fighter# fz @ 1 fighter# fz @ - s>f  fatan2 4096e f* 2e pi f* f/ f>s 4095 and ;

\ ---- the pad ----
create keys-were 2 cells allot   create pads-held 2 cells allot   create pads-pressed 2 cells allot
: first-pad ( -- held )   \ bits: punch kick dodge right left taunt
    pad-circle key-down? 1 and  pad-cross key-down? 2 and or  pad-down key-down? 4 and or
    pad-right key-down? 8 and or  pad-left key-down? 16 and or  pad-select key-down? 32 and or ;
: second-pad ( -- held )   \ (the second player's: style 1)
    pad2-circle key-down? 1 and  pad2-cross key-down? 2 and or  pad2-down key-down? 4 and or
    pad2-right key-down? 8 and or  pad2-left key-down? 16 and or  pad2-select key-down? 32 and or ;
: keys-now ( port -- held )  if  second-pad  else  first-pad  then ;
1 constant punch-key   2 constant kick-key   4 constant dodge-key   8 constant right-key   16 constant left-key
32 constant taunt-key
variable held   variable pressed   \ (the pad of the fighter acting now)
: read-port ( port -- )
    {: n :}  n keys-now {: k :}  k n cells keys-were + @ invert and n cells pads-pressed + !
    k n cells pads-held + !  k n cells keys-were + ! ;
: read-pad ( -- )  0 read-port  1 read-port  0 cells pads-held + @ held !  0 cells pads-pressed + @ pressed ! ;
: use-pad ( port -- )  cells {: k :}  k pads-held + @ held !  k pads-pressed + @ pressed ! ;
: forward-key ( i -- k )  if  left-key  else  right-key  then ;   \ (the first on the left: forward is right)
: back-key ( i -- k )  if  right-key  else  left-key  then ;

: start-move ( f m -- )  over move !  0 swap move-frame ! ;
: human-input ( f i -- )   \ src: human_input
    {: f i :}  i forward-key {: fwd :}  i back-key {: bk :}
    f tap @ 0> if  -1 f tap +!  else  f tap @ 0< if  1 f tap +!  then  then
    ko @ if  f victory start-move exit  then
    pressed @ punch-key and if
        f combo @ 2 >= if  hook  else  held @ fwd and if  forward-punch  else  held @ bk and if  back-punch  else  jab  then  then  then
        f swap start-move exit  then
    pressed @ kick-key and if  f  held @ bk and if  back-kick  else  kick  then  start-move exit  then
    pressed @ dodge-key and if  f dodge start-move exit  then
    held @ fwd and if
        f tap @ 0>  pressed @ fwd and and if  f dash-in start-move  0 f tap ! exit  then
        f step-in start-move  f step-in-units step  pressed @ fwd and if  double-tap-frames f tap !  then  exit  then
    held @ bk and if
        f tap @ 0<  pressed @ bk and and if  f dash-back start-move  0 f tap ! exit  then
        f step-back start-move  f step-back-units negate step  pressed @ bk and if  double-tap-frames negate f tap !  then  exit  then
    f  held @ taunt-key and if  taunt  else  ready-move  then  start-move ;

\ ---- the AI (src: ai_input) ----
: chance ( n -- flag )  256 random > ;
: threat ( o -- n )   \ the other's attack: 0 none, 1 past, 2 about to land in reach, 3 coming
    {: o :}  o move @ attacks < 0= if  0 exit  then
    o move @ hit-frame {: hf :}  o move-frame @ hf < 0= if  1 exit  then
    o move-frame @ hf 1- =  apart o move @ reach 100 + < and if  2  else  3  then ;
: mood-now ( f i -- )
    {: f i :}
    streak-victim @ i =  streak @ streak-to-mood >= and if  2 f mood ! exit  then
    apart far-apart > if  3 f mood ! exit  then
    streak-victim @ i other =  streak @ streak-to-mood >= and if  4 f mood !  then ;
: keep-distance ( f m -- )   \ moods 0 and 1: back off when near, come in when far
    {: f m :}  f style @ {: s :}
    apart s 8 m 2* + ai16 < if  f step-back start-move  f step-back-units negate step
    else  s 12 m 2* + ai16 apart < if  f step-in start-move  f step-in-units step  else  f ready-move start-move  then  then
    64 random 0= if  f mood @ 0= 1 and f mood !  then ;
: easy-mood ( f th -- )
    {: f th :}  f mood @ {: m :}  f style @ {: s :}
    th 2 >= if  th 2 =  s m ai chance and if  f dodge start-move  else  0 f move-frame !  then  exit  then
    apart jab-reach <  s 2 m + ai chance and if  f jab start-move exit  then
    apart kick-reach <  s 4 m + ai chance and if  f kick start-move exit  then
    apart s 18 ai16 <  s 6 m + ai chance and if  f s 16 ai start-move exit  then
    f m keep-distance ;
: defending ( f i th -- )
    {: f i th :}  32 random 0=  th 1 = or {: lucky :}
    streak-victim @ i other = if  f ready-move start-move  0 f mood ! exit  then
    th 2 >= if  th 2 = if  f dodge start-move  else  0 f move-frame !  then  exit  then
    lucky apart jab-reach < and if  f jab start-move exit  then
    lucky apart kick-reach < and if  f kick start-move exit  then
    apart closing-in >= if  f step-in start-move  f step-in-units step exit  then
    f ready-move start-move ;
: pressing ( f i -- )
    {: f i :}
    32 random 0=  apart jab-reach < and if  f jab start-move
    else  32 random 0=  apart kick-reach < and if  f kick start-move
    else  apart pressing-in >= if  f step-in start-move  f step-in-units step
    else  streak-victim @ i = if  0 f mood !  then  f ready-move start-move  then  then  then
    0 streak ! ;
: ai-input ( f i -- )
    {: f i :}
    ko @ if  f victory start-move exit  then
    i other fighter# threat {: th :}  f i mood-now
    f mood @ case
        2 of  f i th defending  endof
        3 of  apart close-again < if  0 f mood !  else  f dash-in start-move  then  endof
        4 of  f i pressing  endof
        drop  f th easy-mood  0
    endcase ;

\ ---- the sounds (src: fight_snd_base by area, then by the move: fight_snd_hit, fight_snd_voice) ----
4 constant missed-sound   7 constant fall-sound   8 constant fall-frame
: sound-base ( -- id )  s" fight-sound-base" area @ exe-u8 ;
: hit-sound ( m -- id )  {: m :}  s" fight-hit-sounds" m exe-u8 sound-base + ;
: voice-sound ( m -- id )  {: m :}  s" fight-voices" m exe-u8 sound-base + ;

\ ---- an attack's move-frame (src: attack_resolve) ----
: felt ( o strength small large -- )   \ a human hurt: their pad shakes (src: fight.c rumble)
    {: o v sf lf :}  o human? 0= if  exit  then
    s" rumble" actor-named {: r :}  o style @ sf r send rumble-small  o style @ v lf r send rumble-large ;
14 constant hurt-flash   8 constant shake-a-damage   2 constant hurt-small   5 constant hurt-large
255 constant ko-shake   7 constant ko-small   7 constant ko-large
: lands ( f o i -- )
    {: f o i :}  f move @ {: m :}
    o human? if  hurt-flash s" effects" actor-named send screen-effect  o m damage shake-a-damage * hurt-small hurt-large felt  then
    1 f combo +!  0 o combo !  o m reaction start-move  m push o pushed !
    m damage {: d :}  d negate o health +!
    streak-victim @ i other = if  d streak +!  else  i other streak-victim !  d streak !  then
    o health @ 0 <= if  0 o health !  o knocked-out start-move  1 ko !  o ko-shake ko-small ko-large felt  then ;
: attack ( f o i -- )
    {: f o i :}  f move @ {: m :}
    f move-frame @ m hit-frame <> if  exit  then
    apart m reach <  o move @ move-kind not-hittable <> and 0= if  sound-base missed-sound + sfx exit  then
    m hit-sound sfx  m voice-sound sfx  f o i lands ;

\ ---- a move's own motion (src: move_effects) ----
: out-of-the-dash ( f -- )   \ forward and punch: a lunge; back and punch: a punch
    {: f :}  f fighters - fighter / {: i :}
    held @ i forward-key punch-key or = if  f lunge start-move  f lunge-units step  then
    held @ i back-key punch-key or = if  f dash-back-punch start-move  then ;
: falls ( frame -- )   \ hitting the ground (src: move_effects: frame 8; outdoors the next sound)
    fall-frame = if  sound-base fall-sound +  area @ the-town = 1 and +  sfx  then ;
: own-motion ( f o -- )
    {: f o :}  f move-frame @ {: fr :}
    f move @ case
        jab of  o move @ light-hit =  fr 4 7 within and  pressed @ kick-key and 0<> and  f the-player? and if  f kick start-move  then  endof
        kick of  o move @ kicked =  fr 8 11 within and  pressed @ punch-key and 0<> and  f combo @ 2 = and  f the-player? and if
            f hook start-move  then  endof
        hook of  0 f combo !  f s" fight-hook-steps" fr hook-steps step-of step  endof
        dash-back-punch of  f dash-punch-units step  endof
        lunge of  f dash-punch-units step  endof
        special-6 of  f s" fight-special-steps" fr special-steps step-of step  endof
        knocked-out of  fr 6 < if  ko-push f pushed !  then  f s" fight-ko-steps" fr ko-steps step-of step  endof
        down-for-good of  fight-winner @ no-winner = if  f fighters - fighter / other fight-winner !  then  endof
        19 of  f s" fight-down-steps" fr down-steps step-of step  fr falls  endof
        20 of  f s" fight-down-steps" fr down-steps step-of step  fr falls  endof
        dash-in of  f dash-units step  f the-player? if  f out-of-the-dash  then  endof
        dash-back of  f dash-units negate step  endof
    endcase ;

\ ---- a move-frame of the fight ----
: blocked-now? ( f o -- flag )
    {: f o :}  f fx @ f fz @ f floor-of blocked?  if  true exit  then
    f fx @ o fx @ - s>f fdup f*  f fz @ o fz @ - s>f fdup f* f+ fsqrt too-close s>f f< ;
: show-fighter ( f -- )   \ where it is, its heading, its move's motion (started when it changes)
    {: f :}  f move @ move-motion {: id :}
    f slot @ 0= if
        f fx @ 0 f fz @ s" matthew" actor-named send figure-place  0 f heading @ 0 s" matthew" actor-named send figure-turn
    else
        f slot @ f fx @ f fz @ s" townsfolk" actor-named send person-place
        f slot @ f heading @ s" townsfolk" actor-named send person-turn
    then
    id f last-motion @ = if  exit  then  id f last-motion !
    f slot @ 0= if  id -1 false s" matthew" actor-named send figure-anim
    else  f slot @ id -1 false s" townsfolk" actor-named send person-anim  then ;
: fighter-frame ( i -- )
    {: i :}  i fighter# {: f :}  i other fighter# {: o :}
    f fx @ f was-x !  f fz @ f was-z !
    f human? if  f style @ use-pad  then   \ (its own pad: the first, or the second's)
    f move-frame @ 0<>  f motion-done? and if  f  f move @ move-next  start-move  then
    f pushed @ ?dup if  f swap negate step  f pushed @ 2/ f pushed !  then
    f move @ move-kind ready = if  f human? if  f i human-input  else  f i ai-input  then  then
    f move @ attacks < if  f o i attack  then
    f o own-motion
    1 f move-frame +!
    f o blocked-now? if  f was-x @ f fx !  f was-z @ f fz !  then
    f health @ f shown @ < if  -1 f shown +!  then ;
: aim ( -- )   \ facing each other; the camera between them, across
    angle {: a :}  a 2048 + 4095 and 0 fighter# heading !  a 1 fighter# heading !
    fight-look-y  apart 2* fight-dist-base +  fight-eye-y
    0 fighter# fx @ 1 fighter# fx @ + 2/  0  0 fighter# fz @ 1 fighter# fz @ + 2/  a 1024 -
    s" camera" actor-named send camera-place ;
3 constant fade-out   10 constant fight-opens   \ (src: screen_effect 3 on retrying, 0xA as it begins)
$A constant fight-music   $D constant own-music   \ (rows of the music table)
: fresh ( f -- )   \ full health, ready, where it began
    {: f :}  full-health f health !  full-health f shown !  0 f combo !  0 f tap !  0 f pushed !  0 f mood !
    ready-move f move !  0 f move-frame !  -1 f last-motion !  f start-x @ f fx !  f start-z @ f fz ! ;
: begin-fight ( -- )   \ (again from the start after a loss)
    0 fighter# fresh  1 fighter# fresh
    0 ko !  0 streak !  no-winner fight-winner !  0 lost !  0e due f!
    0 keys-now keys-were !  1 keys-now keys-were cell+ !
    fighting?  0 fighter# style @ styles-own-motions <> or if   \ src: fight_begin's music (again on a retry)
        0 fighter# style @ styles-own-motions = if  own-music  else  fight-music  then  music-row  then
    true fighting !
    s" camera" actor-named send camera-fixed  fight-opens s" effects" actor-named send screen-effect ;
: it's-over ( -- )
    0 fighter# style @ style-never-wins = 0= if
        fight-winner @ fighter# human? 0= if   \ lost to the AI: game over
            1 lost !  -1 music-row  exit  then  then
    false fighting ! ;   \ (the camera the scene's again: it's an event's)
: fight-frame ( -- )
    read-pad  aim  0 fighter-frame  1 fighter-frame  0 fighter# show-fighter  1 fighter# show-fighter
    fight-winner @ no-winner <> if  it's-over  then ;
: game-over ( -- )   \ ○ or Start: the fight again
    lost @ 1 = if  read-pad  pressed @ punch-key and  pad-start key-down? or if
        fade-out s" effects" actor-named send screen-effect  2 lost !  then  exit  then
    effects:effect-running? 0= if  begin-fight  then ;
: steps-due ( -- n )  due f@ motion-rate dt f* f+  fdup f>s  dup s>f f-  due f! ;
: fight-tick ( -- )
    fighting? 0= if  exit  then
    lost @ if  game-over exit  then
    steps-due 0 ?do  fighting? 0= lost @ or if  leave  then  fight-frame  loop ;

\ ---- drawing: the health bars, the game over ----
140 negate constant bar-1-x   18 constant bar-2-x   80 negate constant bars-y
: draw-fight ( -- )
    fighting? 0= if  exit  then
    lost @ if  draw-game-over exit  then
    bar-1-x bars-y 0 fighter# shown @ full-health health-bar  bar-2-x bars-y 1 fighter# shown @ full-health health-bar ;
' draw-fight on-draw

: start ( a style b style -- )
    {: a sa b sb :}
    a 0 fighter# slot !  sa 0 fighter# style !  b 1 fighter# slot !  sb 1 fighter# style !
    2 0 do  i fighter# dup slot @ place-of  rot tuck start-z !  start-x !  loop
    begin-fight ;
PRIVATE>

: winner-human? ( -- flag )  fight-winner @ fighter# human? ;   \ (a fact: src fight_winner_is_human)

state: fight-state end-state
behaviour fights
  on spawned ( -- )  self subscribe tick ;
  on tick ( -- )  fight-tick ;
  on fight-start ( a style b style -- )  start ;
end-behaviour

: spawn-fight ( -- id )  fights fight-state s" fight" spawn ;
