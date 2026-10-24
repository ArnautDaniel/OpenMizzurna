\ area027.fs - the town's rules (src: ovl027.c, the town's overlay; decomp/docs/areas/area27.md): the buildings'
\ doors - whether Matthew may go in now (their hours, the story) and the script that takes him in.
IN: rules.area027
USING: engine actors messages clock story-flags townsfolk areas look talk matthew knock events events.ovl027 tail
    walkers driving showing ;

27 constant this-area

: today ( -- day )  clock-time time>day ;

\ ---- a person where their schedule has them (src: at_place: standing at a stop of that record) ----
5 constant mel-at-his-room       \ (Mel's record 5)
0 constant cohen-at-home         \ (Cohen's record 0)

\ ---- the story's flags these doors read (not named yet beyond what the doors tell) ----
$008 3 story-bit constant mel-room-a     \ src: STORY_WORD(0x08) & 0x18, both: Mel's room shut
$008 4 story-bit constant mel-room-b
$108 constant mel-room-counter           \ src: STORY_WORD(0x108) != 0: Mel's room may open
$020 12 story-bit constant hospital-always-open
$018 25 story-bit constant lakeside-inn-open
$01C 18 story-bit constant strasbourg-visit      \ src: STORY_WORD(0x1C) & 0xC0000 == 0x40000 ...
$01C 19 story-bit constant strasbourg-visited    \ ... and then this set
$01C 21 story-bit constant strasbourg-open
$01C 29 story-bit constant junkyard-a   $01C 30 story-bit constant junkyard-b   \ src: & 0x60000000 == 0x20000000
$01C 27 story-bit constant junkyard-c   $008 16 story-bit constant junkyard-d
$020 16 story-bit constant hideout-a    $020 17 story-bit constant hideout-b    \ src: & 0x30000 == 0x10000
$018 2 story-bit constant mel-at-diner-seen
10 constant junkyard-guard   $1A123 constant guard-at-gate   \ (actor 10 at x 0x1A123: the junkyard's gate kept)
$16D07 constant guard-at-the-farm   \ (at Astin Farm)
variable hall-tried   \ src: D_8010CA2E: the school's main hall tried with Winona this visit

\ ---- shut: the "closed" script with its message (src: refuse, D_8010A510's operand) ----
$31 constant closed   $32 constant diner-not-open   $33 constant diner-closing   $34 constant hospital-not-open
$35 constant hospital-closed
variable shut-message
: shut ( -- )   \ (entry-8010A510, its message the variable's)
    events.main:sub-80096E34  matthew 62 0 anim  matthew wait-anim  shut-message @ matthew say-by-wait ;
: refuse ( msg -- false )  shut-message !  ['] shut start-event  false ;

\ ---- may he go through this door now? (src: ovl027_func_80103A70; Winona riding with him: to come) ----
variable lookout-mode   \ src: D_8010CA2C: 1 the lookout by his door (0: the robber)
$018 2 story-bit constant mel-overheard   16 constant mel-on-his-way   \ (src: +0x18 bit 2; Mel's stop 16)
$010 15 story-bit constant robber-met   $0EC 1 story-bit constant robber-armed   \ (src: +0x10 bit 15, +0xEC bit 1)
$05C constant robbery-day-word   4 constant robbery-day-shift   \ (src: the day in +0x5C bits 4 - 11)
: robbery-day ( -- day )  robbery-day-word story@ robbery-day-shift rshift $FF and ;
: diner-door ( -- open? )   \ src: ovl027_func_80103A70's 0x52
    mel-overheard flag? 0=  mel last-stop mel-on-his-way = and if
        mel-overheard flag-set  ['] entry-8010B89C start-event  false exit  then
    0 0 0 6 59 between? if  diner-not-open refuse exit  then
    0 19 55 23 59 between?  today 28 >=  0 18 55 23 59 between? and or if  diner-closing refuse exit  then
    robber-met flag? 0=  robber-armed flag? and  robbery-day 15 0 19 59 between? and if
        18 minigame  robber-met flag-set  0 lookout-mode !  false exit  then
    true ;
: open? ( door -- flag )
    case
        $00 of   \ the sheriff's department
            0 0 0 6 59 between?  0 21 45 23 59 between? or if  closed refuse  else  true  then  endof
        $0F of   \ Nozz's store
            0 0 0 6 59 between? if  closed refuse  else
            today 29 < if  0 17 25 22 59  else  0 18 20 22 59  then  between? if  closed refuse  else  true  then
            then  endof
        $43 of   \ the gas station (src: also when func_80042EA4 - to come)
            0 0 0 6 59 between?  0 22 0 23 59 between? or if  closed refuse  else  true  then  endof
        $24 of   \ Mel's room
            mel mel-at-his-room at-record? 0=  mel-room-a flag? mel-room-b flag? and 0= and
            mel-room-counter story@ 0<> and if  true  else  closed refuse  then  endof
        $1B of  closed refuse  endof   \ the Roland residence
        $52 of  diner-door  endof
        $10 of   \ the hospital
            hospital-always-open flag? if  true  else
            0 0 0 7 59 between? if  hospital-not-open refuse  else
            0 21 45 23 59 between? if  hospital-closed refuse  else  true  then  then  then  endof
        $30 of  0 0 45 19 59 between? if  closed refuse  else  true  then  endof   \ Club Wolves
        $38 of  0 7 0 17 59 between? if  true  else  closed refuse  then  endof    \ the high school
        $39 of  0 7 0 17 59 between? if  true  else  closed refuse  then  endof
        $2D of  lakeside-inn-open flag? if  true  else  closed refuse  then  endof   \ the Lakeside Inn
        $56 of   \ the Strasbourg residence
            strasbourg-visit flag?  strasbourg-visited flag? 0= and  cohen cohen-at-home at-record? and if
                strasbourg-visited flag-set  ['] entry-8010A5EC start-event  false
            else  strasbourg-open flag? if  ['] entry-8010A624 start-event  false  else  closed refuse  then  then  endof
        $5A of   \ St. Peter's
            0 7 0 19 59 between?  25 12 55 13 40 between? 0= and if  true  else  closed refuse  then  endof
        $5D of  25 12 0 13 59 between? if  true  else  closed refuse  then  endof
        $26 of   \ the junkyard
            junkyard-a flag?  junkyard-b flag? 0= and if  true  else
            junkyard-c flag?  junkyard-d flag? 0= and  junkyard-guard person-now drop 2drop guard-at-gate <> and
            if  true  else  closed refuse  then  then  endof
        $20 of  hideout-a flag?  hideout-b flag? 0= and if  true  else  closed refuse  then  endof   \ Wolf's hideout
        true swap
    endcase ;

\ ---- the doors: through the gate, then the building's script (src: enter, the evfns) ----
: go-in ( door xt -- )  swap open? if  start-event  else  drop  then ;
: roland-residence  $1B ['] entry-8010BE90 go-in ;
: sheriffs-office   $00 ['] entry-8010BF0C go-in ;
: haines-diner      $52 ['] entry-8010C080 go-in ;
: st-peters         $5A ['] entry-8010C7B4 go-in ;
: st-peters-side    $5D ['] entry-8010C810 go-in ;
: gas-station       $43 ['] entry-8010C5F0 go-in ;
: high-school       $38 ['] entry-8010C574 go-in ;
: nozz-store        $0F ['] entry-8010C208 go-in ;
: gruber-hospital   $10 ['] entry-8010BD88 go-in ;
: club-wolves       $30 ['] entry-8010C718 go-in ;
: riverside-park    $6F ['] entry-8010BF88 go-in ;
: strasbourg        $56 open? drop ;   \ (the gate does it)
: lakeside-inn      $2D ['] entry-8010AAAC go-in ;
: junkyard          $26 ['] entry-8010BE04 go-in ;
: mels-room         $24 ['] entry-8010BD0C go-in ;
: rubin-apartments  $47 ['] entry-8010C004 go-in ;
: wolfs-hideout     $20 open? if  hideout-a flag-set  ['] entry-8010AA78 start-event  then ;   \ src: |= 0x20000
: school-hall ( -- )   \ src: evfn_80106C50: with Winona along, her scene there (minigame 10), the hall tried
    winona going-along? if  10 minigame  1 hall-tried ! exit  then  ['] entry-8010B5E0 start-event ;
' entry-80109F04 10 minigame-script!   \ (src: area_minigame_start's 10)

this-area ovl $80106594 ' roland-residence native:   this-area ovl $801065C8 ' sheriffs-office native:
this-area ovl $801065FC ' haines-diner native:       this-area ovl $80106630 ' st-peters native:
this-area ovl $80106664 ' st-peters-side native:     this-area ovl $80106698 ' gas-station native:
this-area ovl $801066CC ' high-school native:        this-area ovl $80106700 ' nozz-store native:
this-area ovl $80106734 ' gruber-hospital native:    this-area ovl $80106768 ' club-wolves native:
this-area ovl $8010679C ' riverside-park native:     this-area ovl $801067D0 ' strasbourg native:
this-area ovl $801067F0 ' lakeside-inn native:       this-area ovl $80106824 ' junkyard native:
this-area ovl $80106858 ' mels-room native:          this-area ovl $8010688C ' rubin-apartments native:
this-area ovl $80106D24 ' wolfs-hideout native:      this-area ovl $80106C50 ' school-hall native:
this-area ovl $80106D04 ' fill-up native:   \ (the gas station's full service)

\ ---- □ at a house's door knocks (src: ovl027 func_80106A04 / func_80106B3C: the act check's answer and the
\ tile's rows' set make the key) ----
enum: knocks  no-knock  at-astin-farm  at-the-rolands  at-the-strasbourgs  at-mels  at-the-lakeside-inn  at-james ;
-2 constant from-the-street
: knock-key ( check -- key )   \ the answer, and the set of the tile he stands on (src: D_800A38E4 << 8)
    matthew-now drop nip tile-of rows-set 8 lshift + ;
: act-check ( zone check -- action )
    nip knock-idle? 0= if  drop no-knock exit  then
    knock-key case
        $C01 of  at-astin-farm  endof
        $300 of  at-the-rolands  endof
        $A40 of  at-the-strasbourgs  endof
        $C00 of  at-mels  endof
        $1003 of  at-the-lakeside-inn  endof
        $1500 of  at-james  endof
        no-knock swap
    endcase ;
: answers ( who door -- )  {: who n :}  who if  who from-the-street n answer  then ;
: act-do ( action -- )
    case
        at-astin-farm of  $70 knock if  answer-dennis  then  endof
        at-the-rolands of  $1B knock $1B answers  endof
        at-the-strasbourgs of  $56 knock if  cohen $56 answers  then  endof
        at-mels of  $24 knock if  mel $24 answers  then  endof
        at-the-lakeside-inn of  $2D knock drop  endof
        at-james of  $1E knock if  james $1E answers  then  endof
        drop run-rows 0
    endcase ;
: look-check ( zone check -- action )  2drop 0 ;   \ (the town's look: its rows alone)
: look-do ( action -- )  drop run-rows ;
' look-check ' look-do ' act-check ' act-do  this-area area-rules!

\ ---- the lookout (106) by his door (src: ovl027_func_801040B0's minigame 1; lookout_act) - tail.fs ----
$000 6 story-bit constant tail-armed   \ (src: +0x00 bit 6: the town's tail armed)
106 constant the-lookout   $14 constant mel-still-around   \ (src: Mel's stop under 0x14)
: entered ( -- )
    tail-armed flag?  hideout-robbers-caught flag? 0= and dup lookout-mode !
    if  s" tail" actor-named send tail-pace  then ;
: talks-here ( slot -- handled? )
    dup the-lookout =  the-robber-out? and if  drop  s" tail" actor-named send robber-spoken-to  true exit  then
    the-lookout <>  lookout-mode @ 0= or if  false exit  then
    mel stop-of mel-still-around <  tail-started? 0= and if  s" tail" actor-named send tail-start
    else  ['] entry-8010B470 start-event  then  true ;

\ ---- the stalker (112): minigame 23's warp to the town, then round his loop on the footpaths (minigame 24: nodes 0
\ -> 3 -> 9 -> 0, running) until he's talked to - caught (minigame 25) ----
112 constant the-stalker   100 constant stalker-speed   5 constant he-runs
$01C 28 story-bit constant stalker-caught   \ (src: +0x1C bit 28, toggled)
variable stalker-due   variable stalking
: stalker-arrives ( -- )  1 stalker-due !  events.main:exe-800969EC ;   \ minigame 23 (src: D_800A34B8)
' stalker-arrives 23 minigame-script!
: leg ( from dest -- )   \ src: states 30 - 32: the next leg once he's done the last
    stalking @ 0= if  2drop exit  then
    the-stalker -rot stalker-speed he-runs s" walkers" actor-named send walker-start
    yield  begin  the-stalker walker-active?  stalking @ and while  yield  repeat ;
: rounds ( -- )  begin  stalking @ while  0 3 leg  3 9 leg  9 0 leg  repeat ;
: stalk ( -- )  true stalking !  ['] rounds the-stalker start-route ;   \ minigame 24
' stalk 24 minigame-code!
: robber-at-the-door ( -- )  s" tail" actor-named send robber-start ;   \ minigame 18
' robber-at-the-door 18 minigame-code!
: caught ( -- )   \ minigame 25 (src: actor_handler_112 in the town)
    false stalking !  the-stalker end-route  the-stalker s" walkers" actor-named send walker-stop
    stalker-caught flag-toggle  ['] entry-8010A178 start-event ;
' caught 25 minigame-code!

: entered-town ( -- )
    entered  false stalking !  the-stalker end-route
    stalker-due @ if  0 stalker-due !  24 minigame  then ;
: the-guard ( -- handled? )   \ src: func_80106E40: at the farm his scene (minigame 34); at the gate his talk
    junkyard-guard person-now 2drop drop {: x :}
    x guard-at-the-farm = if  $020 story@ $10 or $020 story!  34 minigame  true exit  then
    x guard-at-gate = if  junkyard-guard 0 npc-talk-as-matthew  then  true ;
' entry-8010A358 34 minigame-script!   \ (src: area_minigame_start's 34)
: talks-to ( slot -- handled? )
    dup junkyard-guard = if  drop the-guard exit  then
    dup the-stalker = if  drop 25 minigame  true exit  then  talks-here ;
' talks-to this-area area-talk!
this-area ' entered-town 0 area-hooks!

\ ---- the deputy shown the sheriff's badge: his scene (src: area_minigame_start's 27; showing.fs) ----
' entry-8010A240 27 minigame-script!

\ ---- the photo shop's clerk shown the pocket camera, once he has met him (src: actor_handler_158) ----
158 constant photo-clerk   10 constant pocket-camera   $64 constant takes-the-film
$1BC 1 story-bit constant met-the-clerk   \ (src: +0x1BC bit 1: set at his first talk, func_80105FF8)
:noname ( slot item -- answer )
    over busy? if  2drop 0 exit  then
    nip pocket-camera =  met-the-clerk flag? and if  takes-the-film  else  0  then ;  photo-clerk item-answer!
:noname ( slot answer -- )  nip takes-the-film = if  ['] entry-8010BC6C start-event  then ;  photo-clerk item-shown!
