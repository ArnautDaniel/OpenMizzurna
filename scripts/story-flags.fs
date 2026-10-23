\ story-flags.fs - the story block (docs/subsystems/story.md): the original's 0x204 bytes of flags and
\ counters (src: D_800F6928, story_save), and the ones named so far. Written by tools/story_flags.py from
\ the decomp's tools/flag_names.txt; more names are added here by hand as they are understood.
IN: story-flags

$204 constant story-bytes
create story-block  story-bytes allot
: story-clear ( -- )  story-block story-bytes erase ;  story-clear

\ a bit: its number in the block (`$010 2 story-bit`); anyone may read one, its owner sets it
: story-bit ( offset bit -- n )  swap 8 * + ;
: bit-at ( n -- addr mask )  dup 3 rshift story-block +  1 rot 7 and lshift ;
: flag? ( n -- flag )  bit-at swap c@ and 0<> ;
: flag-set ( n -- )  bit-at over c@ or swap c! ;
: flag-clear ( n -- )  bit-at invert over c@ and swap c! ;
: flag-toggle ( n -- )  dup flag? if  flag-clear  else  flag-set  then ;
\ a counter: a 32-bit word at its offset
: story@ ( offset -- n )  story-block + sl@ ;
: story! ( n offset -- )  story-block + l! ;
: story+! ( n offset -- )  tuck story@ + swap story! ;

\ ---- the names ('?': a likely reading, not settled) ----
$004 constant got-barbaras-cigarettes                       \ (a counter) ovl001 act action 13: "..." -> +=1, item 0x14
$008  0 story-bit constant took-lighter                       \ ovl001 act action 15: "..." -> toggled, item 9
$008  6 story-bit constant read-garage-flyer                  \ ovl001 act action 19: "..." -> toggled, D_800A3504 |= 0x10000
$008  8 story-bit constant got-isabella-letter                \ ovl001 act action 17: "..." -> set, item 7
$010  2 story-bit constant prologue-done                      \ set by evfn_80047620 (EXE script 80096598: fade, everyone to stop 0); cleared at new game;
$020  8 story-bit constant dr-james-visit-seen                \ ovl001 scene-3 hook: "..."
$010  3 story-bit constant hudson-apology-seen                \ ovl000 scene 2, Hudson (actor 7) at stop 0: "..."
$018  9 story-bit constant station-father-barton-seen         \ ovl000 init: actor 13 at stop 20, from 12/28 11:00, +0x2C != 0: "..."
$018 24 story-bit constant station-shove-it-seen              \ ovl000 init 12/28 11:05-11:52 (and evfn_8010388C): "..."
$018 10 story-bit constant cell-confrontation1-seen           \ ovl000 look 10 when (+0x18 & 0x20000600) == 0x200: "..."
$018 30 story-bit constant cell-confrontation2-seen           \ ovl000 look 10 when (+0x18 & 0x60000000) == 0x20000000: "..."; D_800A3500 |= 0x800000
$018 29 story-bit constant cell-occupied                      \ ? set by ovl000 func_801037B8 (with +0x38 += 1, actor 89 to stop 40); gates the second confrontation
$020  9 story-bit constant station-wolf-seen                  \ ovl000 init when +0x44 bit0: "..."
$020  1 story-bit constant station-minigame30-started         \ ovl000 tick: minigame 0x1E once +0x1F8, +0x1FC != 0 and +0x200 bit0
$01C 14 story-bit constant gave-barbara-cigarettes            \ ovl002 act action 2 (Barbara's door, 12/28 15:05-20:20, item 0x14 removed): pack 051_3 script1 "..."
$01C 13 story-bit constant barbara-invited                    \ ovl002 tick: pack 051_3 script0 "..." sets it; required (with bit14 clear) for the birthday hand-over
$020 20 story-bit constant bad-ending-route                   \ set by ovl026 init entering the forest 12/31 20:00-23:30 (with +0x20 bit17) unless +0x48, +0x4C and +0x50 bit0
$020 22 story-bit constant bad-ending-started                 \ ovl026: "..." -> evfn_80047BFC (mode 1)
$020 24 story-bit constant final-ending-started               \ ovl026: "..." -> pack 053_0 -> evfn_80047C2C (mode 2)
$048 constant visited-james-house                           \ (a counter) knock code func_80046F30 case 0x1E (needs +0x178 bit0, not +0x1C bit23): "..." -> warp to area 4 (Loloma resid
$04C constant church-james-scene-seen                       \ (a counter) ovl024 scene 5 when (+0x1C & 0x02020000) == 0x20000 and +0x34: "..."; final-ending requirement
$050  0 story-bit constant got-bear-necklace                  \ ovl002 func_80103DF0 when (+0x20 & 0xC0000) == 0x40000 and +0x164 bit1: item 0x11 + pack 051_1 "..."; final-en
$008 15 story-bit constant found-cohen-envelope               \ ovl023:func_801021C8: "..."
$008 16 story-bit constant found-epoch-in-car                 \ ovl008:func_801020B4: "..." | "There's too much of it to take with me... I need to report
$010  4 story-bit constant town-meeting-seen                  \ ovl024:evfn_80103328: "..." | "..."
$010  7 story-bit constant president-protest-seen             \ ovl027:func_801051DC: pack 049_2 script2: "..."
$010 12 story-bit constant library-christmas-miracle-seen     \ ovl016:func_80102678: "..."
$010 14 story-bit constant found-winonas-book                 \ ovl016:evfn_80102EA8: "..." | "..."
$010 22 story-bit constant roland-funeral-talk-seen           \ ovl003:func_80102550: "..."
$018  6 story-bit constant met-ms-holly                       \ ovl016:func_801028B0: "..." | "Ms. Holly? Are you Kathy's homeroom teacher?
$018 11 story-bit constant street-old-man-scene-seen          \ ovl027:func_801044E0: pack 049_3 script2: "..."
$018 12 story-bit constant lake-winona-scene-seen             \ ovl009:func_80102418: "..."
$018 17 story-bit constant club-come-here-often-seen          \ ovl012:func_80104E50: "..."
$018 23 story-bit constant motel-need-a-room-seen             \ ovl027:func_801068C0: pack 049_3 script6: "..."
$018 26 story-bit constant kathys-room-seen                   \ ovl011:func_80101ED0: "..."
$01C  4 story-bit constant uncle-night-knock-seen             \ ovl019:func_801025D0, knocking on door 0x4B when the Captain (slot 2) is home there and +0x180 bit0: 801039DC 
$01C  5 story-bit constant isabellas-room-seen                \ ovl019:func_801022AC: "..."
$01C 10 story-bit constant diner-robbery-seen                 \ ovl021:func_80103C64 12/27 11:30-11:59 (or Morgan at stop 0x16 and Matthew inside): 80106DF0 "..." / 80106308 
$01C 12 story-bit constant forest-meeting-seen                \ ovl026:func_80102490: pack 050_0 script1: "..."
$01C 16 story-bit constant used-newspaper-archive             \ ovl016:evfn_80103024: "..."
$01C 20 story-bit constant cohen-found-scene-seen             \ ovl023:func_801022A0: "..."
$01C 25 story-bit constant church-james-scene-bit             \ ovl024:func_80102B6C: "..."
$020 12 story-bit constant mel-bar-confrontation-seen         \ ovl012:func_80103B94 (init) when +0x20 bits 11-12 = 01, Mel (13) and Winona (15) at the club: 80108324 "..." /
$020 15 story-bit constant hospital-from-downstairs-seen      \ ovl002:func_80104524: "..."
$020 16 story-bit constant isabella-scene-seen                \ ovl002:func_80102B28: "..."
$020 19 story-bit constant bear-necklace-scene-seen           \ ovl002:func_80103DF0: pack 051_1 script1: "..."
$020 21 story-bit constant bad-route-matthew-scene-seen       \ ovl026:func_80102490: pack 050_2 script1: "..."
$020 23 story-bit constant final-route-matthew-scene-seen     \ ovl026:func_80102490: pack 050_2 script1: "..." | pack 050_1 script0: "..."
$05C 12 story-bit constant forest-sweep-question-seen         \ ovl000:func_801032BC: "..."
$008 10 story-bit constant read-diary-dec21                   \ evfn_8004F2C8 after the first read of Emma's diary, Dec. 21st (main 8009A888): "..."
$008 11 story-bit constant read-diary-dec22                   \ evfn_8004F2E4 after the Dec. 22nd entry (main 8009A920): "..."; required for Isabella to open her door (knock 
$008 12 story-bit constant read-diary-dec23                   \ evfn_8004F300 after the Dec. 23rd entry (main 8009A9AC): "..."
$008 13 story-bit constant motel-clue-realized                \ func_8004F364 examining item 16 (Mel's scrap) after read_diary_dec23: 8009AA84 '"..."... This is it!'
$008 21 story-bit constant isabella-no-response-seen          \ door_knock 0x4A when +0x1B4 bit0 and not +0x1C bit27: 80096CAC "..."
$01C  8 story-bit constant isabella-mistake-scene-seen        \ ovl019:func_80102744: 80104DC8 "..." / "..."
$01C  9 story-bit constant isabella-let-in                    \ door_knock 0x4A with +0x1C bit8, read_diary_dec22, Isabella (89) home, Wolf (21) away: ovl019 80105540 "..." /
$03C constant isabella-let-in-count                         \ (a counter) door_knock 0x4A: += 1 with isabella_let_in
$01C 23 story-bit constant james-door-scene-seen              \ door_knock 0x1E, set with visited_james_house: 80096C40 "..." (later knocks just call James to the door)
$018 28 story-bit constant rolein-door-scene-seen             \ door_knock 0x2F (Rolein, slot 22, home, +0xEC bit1), once per day (day kept in byte +0x27): pack 049_3 script7
$180  0 story-bit constant isabella-unsafe-seen               \ ovl012:func_8010482C when +0x1BC bit2: 80108BE0 "..."; enables uncle_night_knock_seen
$1BC  2 story-bit constant offlimits-door-seen                \ ovl012:func_80104AF4 when +0x1C bit3: 80108C40 "..."
$000  6 story-bit constant lorraine-robbery-confessed         \ ovl021:func_801043A8, talk to Lorraine (22) once +0x1C bit10: 801071D8 "..." ... "..." / "..."
$008  3 story-bit constant got-emmas-diary                    \ actor_handler_140 (area 6, Mel's house) take: ovl006 801022CC "..." / "...", item 0xE
$008  4 story-bit constant got-mels-note                      \ actor_handler_162 (area 6) take: item 0x10 "..."; calls ovl006 func_80102124 (Mel-coming-back counter)
$008  5 story-bit constant got-emmas-key                      \ ovl003:evfn_80102A80 (call_c from the vase script 801056A0 "..."), item 5
$008  7 story-bit constant emma-diary-found-missing           \ ovl003 act 8 (Emma's desk, after look "..."): 8010540C "..." / "..." / "..."
$008 14 story-bit constant got-epoch-pills                    \ actor_handler_141 (area 11) take: ovl011 8010263C "...", item 0x12 (Pills Labeled "...")
$008 18 story-bit constant showed-no-entry-photo-to-morgan    \ func_8002FB1C (Morgan's item handler, item 0xC Photo of "..." Sign): stores now+2h at +0x1D4/+0x1D8/+0x1DC = t
$008 20 story-bit constant gave-epoch-pills-to-gruber         \ func_80031110 case 0x64 (Samuel, item 0x12 at the hospital): item removed, analysis time now+2h at +0x1E0/+0x1
$008 22 story-bit constant showed-kathys-note-to-gruber       \ ? func_80031110 case 0x65 (Samuel, item 0x16 Kathy's Note, needs +0x18 bit13)
$010  6 story-bit constant diner-lorraine-intro-seen          \ ovl021:func_80103B2C (init) 12/25 11:50-14:30: 80105024 "..." + Lorraine Haines / Mel Astin bios
$010  8 story-bit constant club-mel-dealer-argument-seen      \ ovl012:func_801042E4, Mel (13) at stop 4 with +0xCC bit0: 80105718 "..." / actor 101: "..." / "..."
$010 10 story-bit constant roland-morning-visit-seen          \ ovl027:func_80105084, knock at the Roland door with Steve home (stop 0) 12/25 10:01-11:29: 8010A654 "..." -> w
$010 11 story-bit constant got-engraved-ring                  \ ovl003 act 6 (drain, after look "..."): 8010585C "..." / "..."I"...", item 0x17
$010 13 story-bit constant winona-ride-library-talk-seen      \ ovl027:func_801044E0, dropping Winona at tile 0x2D0 (her stop 0): pack 049_3 script0 / 8010B908 "..." (philoso
$010 17 story-bit constant hospital-barbara-intro-seen        \ ovl002:func_80102B28 area 29 scene 5, 17:31-19:20, Lidds (20) at stop 2/9/17, needs +0x10 bit12: pack 051_0 sc
$010 18 story-bit constant club-james-meeting-seen            \ ovl012:func_80104D34 (scene 0, before 12/28): 801060E8 James (6) "..." / "..." / "..."
$010 20 story-bit constant winona-night-talk-seen             \ ovl013:evfn_80102568 on the day stored by incoming call 1 (+0x1C4 bits 2-9), Winona at stop 6/15: 80102DCC "..
$010 21 story-bit constant winona-night-ride-home-seen        \ ovl027:func_801044E0, dropping Winona at tile 0x2D0 with her at stop 6 or 15 (night): pack 049_3 script5 "..."
$018  1 story-bit constant roland-break-in-scene-seen         \ ovl003:func_80101EF4 tick 12/26 14:30-15:00 if +0x14 == 0: 80103978 "..." -> "..." / "..." / "..."; blocks inc
$018  3 story-bit constant cemetery-mel-meeting-overheard     \ ovl007 init 12/26 20:54-21:24 / tick when Mel at stop 15: 801028AC "..." / "..." / "..."
$018  4 story-bit constant diner-mel-lorraine-argued          \ ovl021:func_80103B2C (init) with Mel at stop 13: 80105828 "..." / "..." / "..."
$018  7 story-bit constant kathy-funeral-seen                 \ ovl007 init 12/26 09:30-10:18: 801037C4 Winona "..." / "..."; tick at 12/26 09:30: 801027D8 Barton "..." / "..
$018  8 story-bit constant church-barton-prayer-seen          \ ovl024:func_80102930 (init) when +0x18 bit7 set, bit8 clear, Barton home, before club_come_here_often_seen: 80
$018 15 story-bit constant church-repentant-thugs-seen        \ ovl024:func_80102930 when +0x104 bit1: 80104878 "..." / "..."
$018 18 story-bit constant church-winona-missing-seen         \ ovl024:func_80102930 when func_80042EA4 (club_come_here_often_seen set, bit18 clear, 20:00-04:59): 80104C6C ".
$018 22 story-bit constant read-kathys-letter                 \ actor_handler_149 (ovl024's opened letter, func_801034B0 "..."): 80104DA0 "..." / "..."December 24th, Kathy Fl
$018 25 story-bit constant got-motel-key                      \ ovl024:func_801033E8, Barton talk when motel_need_a_room_seen: 80105914 "..." ... "...", item 0x18
$018 27 story-bit constant epoch-analysis-scene-seen          \ ovl002:func_80102B28 area 29 scene 1, needs +0x5C bit1 (Gruber's call): pack 051_0 script1 "..." / "..." / "..
$01C  0 story-bit constant played-floppy-on-piano             \ ovl003 act 4 (electric piano, floor 1) needs +0x6C bit0: 80104530 "..." / "..." / "..."Eternal"..."
$01C  1 story-bit constant club-isabella-show-seen            \ ovl012:func_8010482C (needs +0x184, not +0x26 day): 8010709C James "..." ... "..."; day in byte +0x24
$01C  3 story-bit constant club-drunk-fan-scene-seen          \ ovl012:func_801049D8 when (+0x1C & 0xA) == 2: 80107EC0 "..." / "..." / "..."; day in byte +0x25
$01C 11 story-bit constant diner-robbery-outside-seen         \ ovl027:func_801044E0 12/27 11:30-12:30 on foot near the diner, (+0x1C & 0xC00) == 0: pack 052_1 script9 "..." 
$01C 15 story-bit constant cemetery-ms-holden-talk-seen       \ ovl007:func_80102264 talk to actor 156 (placed when gave_barbara_cigarettes and not bit15): 80103A88 "..." / "
$01C 18 story-bit constant forest-mortal-journey-thought      \ ovl026 look zone 5 (needs +0x9C bit2): 80106240 "..."
$01C 19 story-bit constant cohen-house-found-open             \ ovl027:func_80103A70 door 0x56 when bit18 set, bit19 clear: 8010A5EC "..." -> warp area 23 (Cohen's house) -> 
$01C 21 story-bit constant got-cohen-lodge-key                \ ovl002:func_80102B28 area 29 scene 3, needs +0x170: pack 051_2 script1 "..." / "..." ... "...", item 0x15
$01C 22 story-bit constant gave-cohen-envelope                \ ovl002:func_80102B28 area 29 scene 3 with found_cohen_envelope: pack 051_2 script2 "..." / "..."Project Mizzur
$01C 26 story-bit constant crawled-air-duct                   \ ? ovl019:evfn_801027A8 (look zone 81, the duct) with +0x120 bit3 and the Lighter (item 9): minigame 41, else "..
$020  7 story-bit constant roland-barton-suicide-talk-seen    \ ovl003:func_8010297C, Mrs. Roland (17) once +0x34 != 0: 80104960 "..."
$020 10 story-bit constant hospital-wolf-visit-seen           \ ovl002:func_80103698 talk to Winona when +0x44 bit0: pack 051_0 script2 "..." / "..." / Wolf "..."
$020 13 story-bit constant hospital-attack-scene-seen         \ ovl002:func_801021A8 (init, area 2) when +0x20 bits 12-13 = 01: pack 053_1 script0 "..." / "..." / "..."
$020 14 story-bit constant hospital-wounded-guard-seen        \ ovl002 tick (floor 2 near 0x18D4D,0x5612C) or talk to actor 10, when bits 13-14 = 10: pack 053_1 script1 "..."
$020 17 story-bit constant entered-wolf-hideout               \ ? ovl027:evfn_80106D24 (tile 5,1 door 32) only while +0x20 bits 16-17 = 10 (after isabella_scene_seen; tick: "..
$06C  0 story-bit constant showed-floppy-to-mrs-roland        \ actor_handler_17 item 8 (Floppy Disk) -> code 0x64; enables played_floppy_on_piano
$070 constant holden-kathy-talk-done                        \ (a counter) actor_handler_lidds cmd 0x5C (needs +0xA4 != 0) -> pack 048_0 script0 "..."
$074 constant holden-cougar-case-talk-done                  \ (a counter) actor_handler_lidds cmd 0x5C (needs gave_barbara_cigarettes) -> pack 048_0 script1 "..." / "..."
$078 constant holden-main-building-talk-done                \ (a counter) ovl016:func_80102E58 -> 801043D0 "..." / "..."
$120  3 story-bit constant nozz-air-duct-hint-heard           \ ovl019:func_80102BBC (needs +0x08 bit21 isabella_no_response_seen) -> 80105A58 "..."; gates the duct (with the
$01C 27 story-bit constant isabella-room-locked-in            \ ? ovl019:evfn_80102934 leaving Isabella's room (not +0x44 bit0) -> 80103804 "..."
$008 19 story-bit constant isabella-cabin-photo-seen          \ ovl019:evfn_80102A04 -> 8010385C "..." / "..." / "..."
$164  0 story-bit constant morgan-mortal-journey-talk-done    \ func_8002F554 (Morgan's talk picker), needs +0x1C bit22: npc_talk(1, 9) pack 046_3 script9 "..."
$164  1 story-bit constant morgan-necklace-talk-done          \ func_8002F554, needs +0x44 bit0 and +0x20 bit18: npc_talk(1, 10) "..."; required by ovl002 for the necklace sc
