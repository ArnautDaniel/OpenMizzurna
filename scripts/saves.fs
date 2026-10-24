\ saves.fs - saving and continuing (docs/subsystems/saves.md; src: cmcard.c save_build / save_apply /
\ memcard_dialog_update, boot.c game_start): the events' op memory-card (Matthew's room's save point) saves the
\ game into one of three slots of the save file, as the original's memory card file laid it out (0x1980 bytes:
\ the story block, every actor's stop and kind, the time, his car, the fuel, the items, the map, the numbers and
\ the people known, a byte sum). The PC has one card, always there: the dialog is "checking", "save to slot 1?",
\ "saving". Continuing loads a slot back and wakes Matthew by his bed.
IN: saves
USING: engine actors messages clock areas story-flags events events.main items notebook driving townsfolk ;

\ ---- the file: the original's layout (src: save_build) ----
$1980 constant save-size
create save-image save-size allot
: save-name ( -- addr len )  s" mizzurna.sav" ;
3 constant #save-slots
$200 constant format-at   3 constant save-format
$201 constant used-at
$204 constant story-at   $204 constant story-stride
$810 constant people-at   $5B0 constant people-stride   8 constant person-bytes   4 constant kind-at
$1920 constant day-at   $1923 constant hour-at   $1926 constant minute-at
$192A constant car-x-at   $1930 constant car-z-at   $1936 constant car-heading-at   4 constant car-shift
$193C constant fuel-at
$1940 constant items-at   $194C constant map-at   $1958 constant numbers-at   $1964 constant bios-at
$1970 constant sum-at
: image-at ( offset -- addr )  save-image + ;
: byte-at ( offset slot -- addr )  + image-at ;
: half-at ( offset slot -- addr )  2* + image-at ;
: word-at ( offset slot -- addr )  4 * + image-at ;
: byte-sum ( -- n )  0  sum-at 0 do  i image-at c@ +  loop ;
: read-save ( -- flag )   \ the file whole and of this layout
    save-image save-size save-name load-file save-size =  format-at image-at c@ save-format = and ;
: fresh ( -- )  save-image save-size erase  save-format format-at image-at c! ;
: slot-used? ( slot -- flag )  read-save if  used-at swap byte-at c@ 1 =  else  drop false  then ;

\ ---- saving a slot (src: save_build's fill) ----
: person-at ( slot actor -- addr )  person-bytes * swap people-stride * + people-at + image-at ;
: keep-people ( slot -- )
    {: s :}  #slots 0 do  i last-stop  s i person-at l!  i kind-of  s i person-at kind-at + c!  loop ;
: keep-car ( slot -- )
    {: s :}  his-car-now {: x z h :}
    x car-shift rshift  car-x-at s half-at w!  z car-shift rshift  car-z-at s half-at w!  h car-heading-at s half-at w! ;
: fill ( slot -- )
    {: s :}  1 used-at s byte-at c!
    story-block  story-at s story-stride * + image-at  story-bytes move
    s keep-people  s keep-car
    clock-time time>date {: d h m :}  d day-at s byte-at c!  h hour-at s byte-at c!  m minute-at s byte-at c!
    items-mask items-at s word-at l!  map-known @ map-at s word-at l!  fuel @ fuel-at s byte-at c!
    bios-known @ bios-at s word-at l!  numbers-known @ numbers-at s word-at l!
    byte-sum sum-at image-at l! ;
: write-slot ( slot -- ok? )
    read-save 0= if  fresh  then  fill  save-image save-size save-name save-file ;

\ ---- the memory card's dialog, one card always there (src: memcard_dialog_update; the save overlay's scripts) ----
variable card-answer
: answered ( -- )  thread thread-result slot @ card-answer ! ;   \ src: func_8004E5B8: the thread's answer
main-exe $8004E5B8 ' answered native:
main-exe $8004E594 ' answered native:   main-exe $8004E5F4 ' answered native:   \ (the other cards' and the format's: one card, never asked)
1 constant cancelled
: dialog ( slot -- )   \ (in the save point's event): its result 0 saved, 1 not
    ['] events.ovl001:entry-80103624 execute         \ "checking the memory card"
    ['] events.ovl001:entry-80103680 execute         \ a card in slot 1: save there?
    card-answer @ cancelled = if  drop cancelled result! exit  then
    ['] events.ovl001:entry-801037F0 execute         \ saving to slot 1
    write-slot if  0 result!  else  ['] events.ovl001:entry-80103880 execute  cancelled result!  then ;
' dialog is memory-card

\ ---- continuing (src: game_start(n), save_apply) ----
: restore-people ( slot -- )
    {: s :}  #slots 1 do  i  s i person-at sl@  s i person-at kind-at + c@  s" townsfolk" actor-named send person-restore  loop ;
: restore ( slot -- )
    {: s :}
    story-clear  story-at s story-stride * + image-at  story-block story-bytes move
    day-at s byte-at c@  hour-at s byte-at c@  minute-at s byte-at c@  date>time s" clock" actor-named send set-time
    items-at s word-at sl@ s" items" actor-named send items-set
    map-at s word-at sl@ map-known !
    bios-at s word-at sl@  numbers-at s word-at sl@  s" notebook" actor-named send know
    car-x-at s half-at sw@ car-shift lshift  car-z-at s half-at sw@ car-shift lshift  car-heading-at s half-at w@
    s" driving" actor-named send his-car-at
    fuel-at s byte-at c@ s" driving" actor-named send fuel-set
    s restore-people ;
1 constant matthews-house   1 constant his-floor
$55014 constant by-the-bed-x   $4638C constant by-the-bed-z
: continue-game ( slot -- ok? )   \ src: game_start(slot + 1): loads resume in Matthew's room, by the bed
    dup slot-used? 0= if  drop false exit  then
    events-reset  restore
    story-stopped s" clock" actor-named send release-clock
    matthews-house event-area !
    matthews-house 0 s" areas" actor-named send enter-area
    matthew by-the-bed-x 0 by-the-bed-z set-pos  matthew 0 0 0 set-rot  matthew matthews-house his-floor set-floor
    ['] events.ovl001:wake-by-the-bed start-event  true ;
