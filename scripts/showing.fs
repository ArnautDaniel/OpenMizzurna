\ showing.fs - showing an item to someone (docs/subsystems/ring.md; src: the characters' handlers' commands $2E /
\ $2F, cevcon.c func_80042D0C / func_80042D78, items.c item_react, char_talk.c). In the ring's key items, the
\ person Matthew faces is asked what they'd answer to the item (0: nothing, and □ does nothing); □ shows it and
\ the answer goes back to them. A character's own answer: the items they react to (a bit an item, `item-reactions`)
\ answer the item + 1, and shown they play their reaction from their pack - its first, or once seen its second.
\ A few answer an item their own way (Morgan, Samuel, Winona, ...: here; the areas' people: their rules).
IN: showing
USING: engine actors messages clock areas townsfolk talk events story-flags items events.main events.ovl000
    events.ovl002 ;

\ the items named here (the ring's text, 063_3)
5 constant emmas-key   6 constant locker-key   8 constant floppy-disk   12 constant no-entry-photo
13 constant sheriffs-badge   18 constant epoch-pills   22 constant kathys-note
\ the people answering in their own way
8 constant desk-officer   10 constant deputy   17 constant mrs-roland

\ ---- the layers: a person's own answer, else a character's ----
create answerers  #slots cells allot   answerers #slots cells erase   \ xt ( slot item -- answer )
create showers  #slots cells allot   showers #slots cells erase       \ xt ( slot answer -- )
: item-answer! ( xt slot -- )  cells answerers + ! ;
: item-shown! ( xt slot -- )  cells showers + ! ;

\ ---- a character's own (src: func_80042D0C, func_80042D78) ----
6 constant first-reacting-item   \ (the reactions' scripts: two an item from here - unseen, seen)
: busy? ( slot -- flag )  dup travelling?  over riding? or  swap going-along? or ;   \ (src: item_busy)
: reacts? ( slot item -- flag )  swap character-index s" item-reactions" rot exe-s32  swap rshift 1 and 0<> ;
: character-answer ( slot item -- answer )   \ the item + 1, or 0
    over character-index 0< if  2drop 0 exit  then
    over busy? if  2drop 0 exit  then
    2dup reacts? if  nip 1+  else  2drop 0  then ;
: seen-word ( slot -- offset )  character-index cells ;   \ (in the session block: src D_800F68B0)
: react ( slot answer -- )   \ src: func_80042D78: marked seen, their state kept, their reaction played
    {: s b :}  s seen-word session@ b rshift 1 and {: seen :}
    seen 0= if  s seen-word session@  1 b lshift or  s seen-word session!  then
    s save-state
    s  s" reaction-packs" s character-index exe-u8  b first-reacting-item - 2* seen +  pack-talk ;

: answer-to ( slot item -- answer )   \ what they'd answer to the item (0: nothing)
    over cells answerers + @ ?dup if  execute exit  then  character-answer ;
: show-to ( slot answer -- )
    over cells showers + @ ?dup if  execute exit  then  react ;

\ ---- the events' commands $2E / $2F ----
:noname ( a item -- answer )  answer-to ; is answer-item
:noname ( a answer -- )  show-to ; is shown-item

\ ---- in two hours' time, kept in the story block as the day, the hour and the minute (src: func_8002FB1C) ----
2 constant two-hours
: in-two-hours ( day-at hour-at minute-at -- )
    {: d h m :}  clock-time minutes/hour two-hours * + time>date {: day hour minute :}
    day d story!  hour h story!  minute m story-c! ;

\ ---- Morgan (src: actor_handler_morgan, func_8002FB1C): the "No Entry" photo once - the forest's meeting in
\ two hours, and his reaction as to item 12 ----
$65 constant asks-about-photo   $D constant photo-reaction
$1D4 constant forest-meeting-day   $1D8 constant forest-meeting-hour   $1DC constant forest-meeting-minute
:noname ( slot item -- answer )
    dup no-entry-photo = if  over busy? if  2drop 0 exit  then
        2drop  showed-no-entry-photo-to-morgan flag? if  0  else  asks-about-photo  then  exit  then
    character-answer ;  morgan item-answer!
:noname ( slot answer -- )
    dup asks-about-photo = if
        drop photo-reaction  showed-no-entry-photo-to-morgan flag-toggle
        forest-meeting-day forest-meeting-hour forest-meeting-minute in-two-hours
    then  react ;  morgan item-shown!

\ ---- Samuel (src: actor_handler_sammuel, func_80031110): the pills at the hospital (the analysis due in two
\ hours), Kathy's note once he knows of her ----
2 constant the-hospital
$64 constant takes-the-pills   $65 constant reads-the-note   $17 constant note-reaction
$018 13 story-bit constant knows-of-the-note   \ (src: +0x18 bit 13)
$1E0 constant analysis-day   $1E4 constant analysis-hour   $1E8 constant analysis-minute
: samuels-answer ( item -- answer )
    dup epoch-pills = if  drop
        area @ content the-hospital =  gave-epoch-pills-to-gruber flag? 0= and if  takes-the-pills  else  0  then  exit  then
    dup kathys-note = if  drop
        showed-kathys-note-to-gruber flag? 0=  knows-of-the-note flag? and if  reads-the-note  else  0  then  exit  then
    samuel swap character-answer ;
:noname ( slot item -- answer )  over busy? if  2drop 0  else  nip samuels-answer  then ;  samuel item-answer!
:noname ( slot answer -- )
    dup takes-the-pills = if  2drop
        epoch-pills s" items" actor-named send take-item  ['] events.ovl002:entry-801048EC start-event
        analysis-day analysis-hour analysis-minute in-two-hours  gave-epoch-pills-to-gruber flag-toggle  exit  then
    dup reads-the-note = if  drop note-reaction react  showed-kathys-note-to-gruber flag-toggle  exit  then
    react ;  samuel item-shown!

\ ---- Mrs. Roland (src: actor_handler_17): the floppy disk once ----
$64 constant takes-the-floppy   9 constant floppy-reaction
:noname ( slot item -- answer )
    over busy? if  2drop 0 exit  then
    dup floppy-disk =  showed-floppy-to-mrs-roland flag? 0= and if  2drop takes-the-floppy exit  then
    character-answer ;  mrs-roland item-answer!
:noname ( slot answer -- )
    dup takes-the-floppy = if  drop showed-floppy-to-mrs-roland flag-set  floppy-reaction  then  react ;
mrs-roland item-shown!

\ ---- Winona (src: actor_handler_winona, func_80032668): Emma's key, or the locker key - to the school, her
\ locker opened (minigames 6 - 9: the ride there, in school hours or not), Robert to the school ----
$64 constant to-the-locker
$020 25 story-bit constant locker-key-given   \ (src: +0x20 bit 25)
6 constant ride-to-school   7 constant ride-not-now   8 constant ride-again   9 constant ride-again-not-now
26 constant first-locker-day
: school-hours? ( -- flag )   \ src: the 26th from 15:00, the 27th and 28th from 7:00, to 17:30
    26 15 0 17 30 between?  27 7 0 17 30 between? or  28 7 0 17 30 between? or ;
:noname ( slot item -- answer )
    over busy? if  2drop 0 exit  then
    dup emmas-key = over locker-key = or  winona off-schedule? 0= and  following? 0= and if  2drop to-the-locker exit  then
    character-answer ;  winona item-answer!
: robert-to-the-school ( -- )
    clock-time time>day first-locker-day - {: k :}
    robert  s" robert-locker-stops" k exe-s32  the-townsfolk send person-goto-stop ;
: to-her-locker ( -- )
    winona off-schedule? if  exit  then
    locker-key-given flag? if
        school-hours? if  ride-again minigame  else  ride-again-not-now minigame exit  then
    else
        locker-key-given flag-set  locker-key s" items" actor-named send give-item  emmas-key s" items" actor-named send take-item
        school-hours? if  ride-to-school minigame  else  ride-not-now minigame exit  then
    then  robert-to-the-school ;
:noname ( slot answer -- )  dup to-the-locker = if  2drop to-her-locker  else  react  then ;  winona item-shown!

\ ---- James (src: actor_handler_james): not in the forest; the floppy disk at the hospital, once ----
26 constant the-forest
$64 constant plays-the-floppy
$184 constant floppy-played   \ (a counter)
:noname ( slot item -- answer )
    over busy? area @ the-forest = or if  2drop 0 exit  then
    dup floppy-disk =  floppy-played story@ 0= and  played-floppy-on-piano flag? and  area @ content the-hospital = and if
        2drop plays-the-floppy exit  then
    character-answer ;  james item-answer!
:noname ( slot answer -- )
    dup plays-the-floppy = if  2drop
        floppy-disk s" items" actor-named send take-item  ['] events.ovl002:entry-80106494 start-event
        1 floppy-played story+! exit  then
    react ;  james item-shown!

\ ---- the desk officer (src: actor_handler_8): the floppy disk at the station, once ----
0 constant the-station
$64 constant takes-the-disk
$018 31 story-bit constant disk-handed-in   \ (src: +0x18 bit 31)
:noname ( slot item -- answer )
    over busy? if  2drop 0 exit  then
    dup floppy-disk =  area @ content the-station = and  disk-handed-in flag? 0= and  played-floppy-on-piano flag? 0= and if
        2drop takes-the-disk exit  then
    character-answer ;  desk-officer item-answer!
:noname ( slot answer -- )
    dup takes-the-disk = if  drop disk-handed-in flag-set  restore-state  ['] events.ovl000:entry-80106E30 start-event  exit  then
    react ;  desk-officer item-shown!

\ ---- the deputy (src: actor_handler_10): the sheriff's badge - his scene (area 27's minigame 27) ----
1 constant knows-the-badge   27 constant badge-scene
$01C 29 story-bit constant badge-shown   \ (src: +0x1C bit 29, toggled)
:noname ( slot item -- answer )
    over busy? if  2drop 0 exit  then  nip sheriffs-badge = if  knows-the-badge  else  0  then ;  deputy item-answer!
:noname ( slot answer -- )  nip knows-the-badge = if  badge-scene minigame  badge-shown flag-toggle  then ;
deputy item-shown!
