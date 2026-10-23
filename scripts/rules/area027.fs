\ area027.fs - the town's rules (src: ovl027.c, the town's overlay; decomp/docs/areas/area27.md): the buildings'
\ doors - whether Matthew may go in now (their hours, the story) and the script that takes him in.
IN: rules.area027
USING: engine actors messages clock story-flags townsfolk events events.ovl027 ;

27 constant this-area

\ ---- the time: between two times of the day (day 0: any day; src: event_time_window) ----
: between? ( day h1 m1 h2 m2 -- flag )
    {: d h1 m1 h2 m2 :}  clock-time time>date {: day hour minute :}
    d 0<>  d day <> and if  false exit  then
    hour 60 * minute + {: t :}  t h1 60 * m1 + >=  t h2 60 * m2 + <= and ;
: today ( -- day )  clock-time time>day ;

\ ---- a person where their schedule has them (src: at_place: standing at a stop of that record) ----
: at-record? ( slot record -- flag )
    {: s r :}  s stop-of 0< if  false exit  then  s current-row stop-record @ r = ;
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

\ ---- shut: the "closed" script with its message (src: refuse, D_8010A510's operand) ----
$31 constant closed   $32 constant diner-not-open   $33 constant diner-closing   $34 constant hospital-not-open
$35 constant hospital-closed
variable shut-message
: shut ( -- )   \ (entry-8010A510, its message the variable's)
    events.main:sub-80096E34  matthew 62 0 anim  matthew wait-anim  shut-message @ matthew say-by-wait ;
: refuse ( msg -- false )  shut-message !  ['] shut start-event  false ;

\ ---- may he go through this door now? (src: ovl027_func_80103A70; Winona riding with him: to come) ----
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
        $52 of   \ the diner (src: Mel's first visit there, the robber at the door - to come)
            0 0 0 6 59 between? if  diner-not-open refuse  else
            0 19 55 23 59 between?  today 28 >=  0 18 55 23 59 between? and or if  diner-closing refuse  else  true  then
            then  endof
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
: school-hall       ['] entry-8010B5E0 start-event ;   \ (with Winona riding along: a minigame - to come)

this-area ovl $80106594 ' roland-residence native:   this-area ovl $801065C8 ' sheriffs-office native:
this-area ovl $801065FC ' haines-diner native:       this-area ovl $80106630 ' st-peters native:
this-area ovl $80106664 ' st-peters-side native:     this-area ovl $80106698 ' gas-station native:
this-area ovl $801066CC ' high-school native:        this-area ovl $80106700 ' nozz-store native:
this-area ovl $80106734 ' gruber-hospital native:    this-area ovl $80106768 ' club-wolves native:
this-area ovl $8010679C ' riverside-park native:     this-area ovl $801067D0 ' strasbourg native:
this-area ovl $801067F0 ' lakeside-inn native:       this-area ovl $80106824 ' junkyard native:
this-area ovl $80106858 ' mels-room native:          this-area ovl $8010688C ' rubin-apartments native:
this-area ovl $80106D24 ' wolfs-hideout native:      this-area ovl $80106C50 ' school-hall native:
