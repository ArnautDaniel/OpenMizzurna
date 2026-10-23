\ knock.fs - knocking at a door (docs/subsystems/doors.md; src: SCKNOCK.C door_knock, door_answer,
\ door_resident_leave; port/src/game/door.c): who answers it now, or a scene, or "no answer" with its reason; the
\ resident steps out and greets him, waits in the doorway facing him, and goes back in when he walks off, when
\ their time is up (255 game frames) or when they're about to leave. The actor `knock` keeps the one knock at a
\ time and sends the residents back in.
IN: knock
USING: engine actors messages clock story-flags townsfolk events events.main events.ovl019 events.ovl027 talk areas matthew ;

\ ---- the knock in progress (src: D_800A3948) ----
variable knocking
: knock-idle? ( -- flag )  knocking @ 0= ;
: knock-over ( -- )  0 knocking ! ;   \ (src: 0x80047614, the end of the going-back-in scenes)
main-exe $80047614 ' knock-over native:

\ ---- the scenes the game patches (src: D_80096A34 answering, D_80096A94 going back in, D_80096C0C no answer) ----
variable answering   variable knock-door   variable knock-side   variable greeting   variable parting   variable no-answer
65 constant knocking-motion   2 constant knock-sound   2 constant stepping-back   -20 constant step-back-pace
16 constant step-back-frames   3 constant before-stepping
: answer-scene ( -- )   \ he knocks and steps back; they come out and greet him, waiting there
    answering @ as-this-one  knock-sound door-sound  matthew knocking-motion 0 anim  matthew wait-anim
    before-stepping frames wait  matthew stepping-back step-back-pace move-forward
    step-back-frames frames wait  matthew idle
    this-one knock-door @ knock-side @ through-door-back  this-one greeting @ 1 talk  this-one door-hold ;
4 constant turn-steps   6 constant parting-gesture
: back-in-scene ( -- )   \ they say goodbye and go back in, to their stop
    answering @ as-this-one  matthew 0 1 anim  matthew this-one camera-face  turn-steps 0 camera-turn
    this-one parting @ parting-gesture talk  this-one knock-door @ knock-side @ through-door  this-one place-at-stop
    knock-over ;
: no-answer-scene ( -- )   \ he knocks; no one comes - the reason
    events.main:sub-80096E34  knock-sound door-sound  matthew knocking-motion 0 anim  matthew wait-anim
    no-answer @ matthew say-by-wait ;

\ the lines by who it is (src: door_answer's and door_resident_leave's choices)
17 constant sarah
: greeting-of ( slot -- msg )
    case  mel of $271F endof  nozz of $2721 endof  sarah of $271E endof  $271D swap  endcase ;
: parting-of ( slot -- msg )
    case  mel of $271A endof  nozz of $271C endof  sarah of $2719 endof  $2717 swap  endcase ;

\ ---- who's in (src: their records: at home) ----
0 constant at-home   4 constant nozz-in-his-room   5 constant mel-in-his-room   5 constant james-at-home
3 constant lorraine-at-home
: home? ( slot record -- flag )  at-record? ;

\ ---- the doors' rules (src: door_knock) ----
$2711 constant no-one-home   $2712 constant too-late   $2713 constant isabellas-silence   $2714 constant cohen-out
$2715 constant too-early   $2716 constant late-evening
$01C 21 story-bit constant cohen-lodge-key-got   $01C 27 story-bit constant isabella-moved
$1B4 0 story-bit constant heard-of-isabella   $01C 8 story-bit constant isabella-knock-due
$0EC 1 story-bit constant lorraine-told   $018 28 story-bit constant lorraine-door-visited   $027 constant lorraine-day
: winona-along? ( -- flag )  winona going-along? ;
: say-no-answer ( msg -- 0 )  no-answer !  ['] no-answer-scene start-event  0 ;
: starts ( xt -- 0 )  start-event  0 ;   \ (a scene instead of an answer)
: hours? ( h1 m1 h2 m2 -- flag )  {: a b c d :}  0 a b c d between? ;
: roland-door ( -- who | 0 )
    0 0 6 59 hours? if  too-early say-no-answer exit  then
    20 45 23 59 hours? if  late-evening say-no-answer exit  then
    steve at-home home? if  steve exit  then
    clock-time time>day 27 >= if  sarah exit  then
    clock-time time>day 26 <  clock-time time>hour 11 < or if  no-one-home say-no-answer exit  then  sarah ;
: strasbourg-door ( -- who | 0 )
    0 0 6 59 hours? if  too-early say-no-answer exit  then
    22 45 23 59 hours? if  late-evening say-no-answer exit  then
    cohen-lodge-key-got flag? if  0 exit  then
    $01C story@ $C0000 and $40000 =  cohen at-home home? and if  cohen-out say-no-answer exit  then
    cohen at-home home? if  cohen exit  then  cohen-out say-no-answer ;
: isabellas-door ( -- who | 0 )
    isabella-let-in flag? 0=  read-diary-dec22 flag? and  isabella-knock-due flag? and
    isabella at-home home? and  wolf 2 home? 0= and if
        isabella-let-in flag-set  1 isabella-let-in-count story+!  ['] events.ovl019:entry-80105540 starts exit  then
    heard-of-isabella flag?  isabella-moved flag? 0= and if
        $008 story@ $200000 or $008 story!  ['] events.main:exe-80096CAC starts exit  then
    isabellas-silence say-no-answer ;
: lorraines-door ( -- who | 0 )
    rolein lorraine-at-home home? 0=  lorraine-told flag? 0= or  lorraine-day story-c@ clock-time time>day = or if
        no-one-home say-no-answer exit  then
    lorraine-door-visited flag-set  clock-time time>day lorraine-day story-c!  ['] events.ovl027:entry-8010BBFC starts ;
: james-door ( -- who | 0 )
    james james-at-home home? 0= if  no-one-home say-no-answer exit  then
    james-door-scene-seen flag? 0=  $178 story@ 1 and and if   \ his invitation: in to his house
        james-door-scene-seen flag-set  1 visited-james-house story!  ['] events.main:exe-80096C40 starts exit  then
    james ;
: knock ( door -- who | 0 )   \ src: door_knock: who will answer, or 0 (a scene or "no answer" started instead)
    knock-idle? 0= if  drop 0 exit  then
    winona-along? if   \ Winona waits in the car
        drop  winona stop-of $2D = if  ['] events.main:exe-80096BF8  else  ['] events.main:exe-80096BE4  then  starts exit  then
    case
        $1B of  roland-door  endof
        $56 of  strasbourg-door  endof
        $4A of  isabellas-door  endof
        $4B of  nozz nozz-in-his-room home? if  nozz  else  no-one-home say-no-answer  then  endof
        $24 of  mel mel-in-his-room home? if  mel  else  no-one-home say-no-answer  then  endof
        $70 of  dennis at-home home? if  dennis  else  no-one-home say-no-answer  then  endof
        $2F of  lorraines-door  endof
        $1E of  james-door  endof
        too-late say-no-answer swap
    endcase ;

\ ---- answering: they step out to the door (src: door_answer, door_answer_dennis) ----
2 constant answers-with-time   \ (they answer only with this many minutes before they leave, or more)
: answer ( who side door -- )
    {: who sd n :}  knocking @ if  exit  then  who minutes-to-go answers-with-time < if  exit  then
    who answering !  n knock-door !  sd knock-side !  who greeting-of greeting !
    who n sd s" townsfolk" actor-named send person-at-door
    ['] answer-scene start-event  1 knocking ! ;
: answer-dennis ( -- )   \ Dennis at the farm's door: his own scene (src: D_80096AE8)
    knocking @ if  exit  then  dennis minutes-to-go answers-with-time < if  exit  then
    dennis answering !  -1 knock-door !  dennis -1 0 s" townsfolk" actor-named send person-at-door
    ['] events.main:exe-80096AE8 start-event  1 knocking ! ;

variable his-to-move   \ (the player's: src player_has_control)
\ ---- back in (src: npc_clock_tick: the door's wait over, he's 501 away, or they're leaving soon) ----
501 constant walked-off   3 constant leaving-soon
: too-far? ( slot -- flag )
    person-now drop {: x y z :}  matthew-now drop {: mx my mz :}
    mx x - dup *  mz z - dup * + s>f fsqrt f>s walked-off >= ;
: back-in ( slot -- )   \ src: door_resident_leave
    {: s :}  s s" townsfolk" actor-named send person-door-free  s answering !
    s door-of-theirs knock-side ! knock-door !  s parting-of parting !
    knock-door @ 0< if  ['] events.main:exe-80096B70  else  ['] back-in-scene  then  start-event ;
: residents ( -- )
    his-to-move @ 0=  events-running @ or  matthew-there? 0= or if  exit  then
    #slots 0 do
        i at-a-door? if
            i door-time-up?  i minutes-to-go leaving-soon < or  i too-far? or if  i back-in  leave  then
        then
    loop ;
: knock-reset ( -- )   \ src: door_knock_reset: everyone at a door released
    knocking @ 0= if  exit  then  0 knocking !
    #slots 0 do  i at-a-door? if  i s" townsfolk" actor-named send person-door-free  then  loop ;

state: knock-state end-state
behaviour knocks
  on spawned ( -- )  self subscribe tick  self subscribe player-control ;
  on tick ( -- )  residents ;
  on player-control ( flag -- )  his-to-move ! ;
end-behaviour
: spawn-knock ( -- id )  knocks knock-state s" knock" spawn ;
