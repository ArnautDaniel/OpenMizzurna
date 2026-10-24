\ area002.fs - area 2's rules, and areas 29's and 30's (they share its overlay): Gruber Hospital (src: ovl002.c;
\ decomp/docs/areas/area02.md; port/src/game/ovl002.c reads it). The ways out (before the prologue ends Matthew
\ stays inside), closing time, the attack on Isabella and its aftermath, Barbara's introduction and invitation,
\ the Epoch's analysis, Cohen's room, Isabella's, the wounded guard; the look and act rows' extras (the doors,
\ Barbara's door and her cigarettes); who says what, and the characters' hospital talks.
IN: rules.area002
USING: engine actors messages clock story-flags items townsfolk floors events look talk areas matthew showing title ;

2 constant this-area   29 constant upstairs   30 constant the-other-wing   \ (29, 30: their geometry, area 2's rules)
10 constant the-guard   107 constant the-nurse   94 constant patient-94   176 constant patient-176
168 constant visitor-168   83 constant person-83   119 constant person-119
21 constant cohens-lodge-key   17 constant bear-necklace
: today ( -- day )  clock-time time>day ;
: hour ( -- h )  clock-time time>hour ;
: here-area ( -- area )  area @ ;   \ (the real one: 2, 29 or 30)
: standing-there? ( slot x z -- flag )  {: s x z :}  s person-now drop nip {: px pz :}  px x =  pz z = and ;
: matthew-xz ( -- x z )  matthew-now {: x y z h :}  x z ;
: give ( item -- )  s" items" actor-named send give-item ;
: take ( item -- )  s" items" actor-named send take-item ;

\ ---- the story's words this area reads (not named beyond what it does) ----
: attacked? ( -- flag )  hospital-attack-scene-seen flag? ;
: attack-due? ( -- flag )  $020 story@ $3000 and $1000 = ;      \ (the hospital's attack: ready, not yet seen)
: guard-wounded-due? ( -- flag )  $020 story@ $6000 and $2000 = ;
: last-scene-done? ( -- flag )  pending-scenes session@ 4 and 0<> ;   \ (the 31st's 23:50 scene: no more of this)
: leaving-with-morgan? ( -- flag )  pending-scenes session@ 1 and 0<> ;
: may-leave? ( -- flag )  prologue-done flag? leaving-with-morgan? or ;

\ ---- people where they are (src: the handlers' checks) ----
1 constant barbara-in-her-room   11 constant lidds-by-barbara   14 constant cohen-in-his-bed   0 constant james-at-work
: barbara-in-her-room? ( -- flag )  barbara barbara-in-her-room at-record? ;            \ src: func_80026444
: barbara-at-the-entrance? ( -- flag )  barbara $19BF6 $57D60 standing-there? ;         \ src: func_80026408
: lidds-by-barbara? ( -- flag )  lidds lidds-by-barbara at-record? ;                     \ src: func_8002D658
: samuel-in-the-lab? ( -- flag )  samuel $18199 $57B6A standing-there? ;               \ src: func_800316E4
: cohen-in-his-bed? ( -- flag )  cohen cohen-in-his-bed at-record? ;                    \ src: func_8002AAE8
: cohen-in-hospital? ( -- flag )  cohen person-of at-stop @ $31 - 9 u< ;               \ src: func_8002AB28 (49 .. 57)
: isabella-at-43? ( -- flag )  isabella person-of at-stop @ $2B = ;                     \ src: func_8002C1CC
: winona-by-the-guard? ( -- flag )  winona $18C1B $556AD standing-there? ;              \ src: func_80032C4C
: james-at-work? ( -- flag )  james james-at-work at-record? ;                           \ src: func_8002CC50
: morgan-at-his-spot? ( -- flag )  morgan $18DF9 $55837 standing-there? ;

\ ---- entering (0x801021A8) ----
variable closing-done   variable upstairs-closing-done   variable knocked-barbara   variable knocked-other
: put ( slot x y z heading area floor motion -- )   \ (src: actor_set_pos, its heading, actor_set_floor, actor_anim)
    {: s x y z h ar fl m :}  s x y z set-pos  s 0 h 0 set-rot  s ar fl set-floor  s m 1 anim ;
: park ( slot -- )  0 0 0 set-pos ;
: the-attack ( -- )   \ ready, and first in: everyone to the attack's places, its scene
    hospital-attack-scene-seen flag-toggle
    isabella s" townsfolk" actor-named send person-away   winona s" townsfolk" actor-named send person-away
    wolf $3E goto-stop  james $46 goto-stop  samuel $3C goto-stop
    ['] events.ovl002:entry-80105348 start-event ;
: guard-upstairs ( -- )   \ area 29: the guard at his post, or wounded
    guard-wounded-due? if  the-guard $18D2F 0 $55E46 $1A4 upstairs 2 $6D put exit  then
    isabella-at-43? if  the-guard $18F3D $4B0 negate $55809 $BBA upstairs 2 0 put  then ;
: entered ( -- )
    5 map-reveal  0 closing-done !  0 upstairs-closing-done !  0 knocked-barbara !  0 knocked-other !
    last-scene-done? 0= if
        here-area this-area = if
            attack-due? if  the-attack  then
            attacked? if  ['] events.ovl002:entry-80104F2C start-event  then
        then
        here-area upstairs = if  guard-upstairs  then
    then
    attacked? if  1 closing-done !  1 upstairs-closing-done !  then ;   \ (after the attack: no closing time)

\ ---- a scene (0x80102B28) ----
create cohens-stops  49 , 53 , 57 ,   \ src: D_8010495C (the 28th, 29th, 30th)
: barbaras-room ( -- )   \ upstairs, scene 5: her introduction, Lidds by her
    0 17 31 19 20 between?  barbara-in-her-room? and 0= if  exit  then
    lidds person-of at-stop @ {: st :}  st 2 =  st 9 = or  st 17 = or 0= if  exit  then
    $010 story@ $21000 and $1000 = if
        ['] events.ovl002:entry-801052A4 start-event  hospital-barbara-intro-seen flag-toggle  then ;
: the-lab ( -- )   \ upstairs, scene 1: Samuel's analysis of the Epoch
    epoch-analysis-scene-seen flag?  $05C story@ 2 and 0= or  samuel-in-the-lab? 0= or if  exit  then
    epoch-analysis-scene-seen flag-toggle  ['] events.ovl002:entry-80105308 start-event
    mel $15 standing-at? if  mel $16 goto-stop  then ;
: cohens-room ( -- )   \ upstairs, scene 3: the lodge key, the envelope; his visitor
    got-cohen-lodge-key flag? 0=  cohen-in-his-bed? and  $170 story@ 0<> and if
        got-cohen-lodge-key flag-toggle  ['] events.ovl002:entry-80104978 start-event  cohens-lodge-key give
    else
        gave-cohen-envelope flag? 0=  found-cohen-envelope flag? and  cohen-in-his-bed? and if
            gave-cohen-envelope flag-toggle  1 $040 story+!  ['] events.ovl002:entry-80104988 start-event  then
    then
    cohen-in-hospital?  $044 story@ 1 and and if  visitor-168 $193D5 $596 negate $55B43 $604 upstairs 2 0 put
    else  visitor-168 park  then ;
: upstairs-scene ( scene -- )
    case
        5 of  barbaras-room  endof
        0 of
            patient-94 $185F7 $4B0 negate $5709D $12 negate upstairs 2 $48 put
            prologue-done flag? if  the-nurse $187FD $4B0 negate $571E9 $BAC upstairs 2 $16 put
            else  the-nurse park  then
            0 knocked-barbara !  0 knocked-other !  endof
        1 of  the-lab  endof
        3 of  cohens-room  endof
    endcase ;
: scene-hook ( scene -- )
    here-area case
        this-area of
            0= if
                attacked? if  patient-94 park  patient-176 park
                else  patient-176 $193A0 0 $57D34 $78A this-area 1 $2E8 put
                    patient-94 $190C6 0 $57EB0 $62 negate this-area 1 $5C put  then
            then  endof
        upstairs of  upstairs-scene  endof
        the-other-wing of
            2 =  isabella-scene-seen flag? 0= and if
                isabella-scene-seen flag-set  ['] events.ovl002:entry-801053A4 start-event  then  endof
        nip
    endcase
    attacked? if  patient-94 park  the-nurse park  then ;

\ ---- each frame (0x8010239C): the ways out, closing time, the scenes on the way ----
$19942 constant exit-x   $56C66 constant exit-z   $7E4 negate constant exit-heading
: out ( arrival -- )  {: a :}  the-town -1 exit-x exit-z exit-heading a warp ;   \ (an event: the warp ends it)
: out-plain ( -- )  0 out ;
: out-with-morgan ( -- )  $8010B700 out ;     \ (the prologue: Morgan outside)
: out-after-attack ( -- )  $8010BC20 out ;    \ (the attack's aftermath)
: zone ( -- index )  at-zone drop ;
: back ( -- )  matthew-was s" matthew" actor-named send figure-place ;   \ src: player_restore_pos
0 constant left-doors   1 constant right-doors   11 constant the-exit   12 constant off-limits
8 constant left-door-anim   10 constant right-door-anim   \ (and the next entry: each pair's other leaf)
: swing ( entry -- )  dup object-anim  1+ object-anim ;
: open-doors ( zone -- )   \ the double doors swing open the first time he reaches them
    left-doors = if  left-door-anim  else  right-door-anim  then {: e :}
    e object-still? if  e swing  ['] events.ovl002:entry-80104F78 start-event  then ;
: leave ( -- )
    leaving-with-morgan? if  pending-scenes session@ 1 invert and pending-scenes session!  ['] out-with-morgan start-event exit  then
    $020 story@ $C00 and $400 = if  $020 story@ $800 xor $020 story!  ['] out-after-attack start-event exit  then
    ['] out-plain start-event ;
: front-way-out ( -- xt )   \ the front hall's: by where he stands
    matthew-xz {: x z :}  z $574E0 > if  ['] events.ovl002:entry-801049CC exit  then
    x $189BF <= if  ['] events.ovl002:entry-80104A6C  else  ['] events.ovl002:entry-80104ACC  then ;
: upstairs-way-out ( -- xt )
    matthew-xz {: x z :}  x $18A87 > if  ['] events.ovl002:entry-80104CB8 exit  then
    z $56D2C > if  ['] events.ovl002:entry-80104C68  else  ['] events.ovl002:entry-80104C90  then ;
: closing ( -- )   \ 21:55 - 22:59 here (19:55 upstairs): shown out, by room
    here-area this-area =  closing-done @ 0= and  0 21 55 22 59 between? and if
        1 closing-done !
        scene @ case
            0 of  front-way-out start-event  endof
            1 of  ['] events.ovl002:entry-80104B1C start-event  endof
            2 of  ['] events.ovl002:entry-80104B88 start-event  endof
            3 of  ['] events.ovl002:entry-80104BD0 start-event  endof
        endcase
    then
    here-area the-other-wing =  closing-done @ 0= and  0 21 55 22 59 between? and if
        1 closing-done !  ['] events.ovl002:entry-80104B64 start-event  then
    here-area upstairs =  upstairs-closing-done @ 0= and  0 19 55 22 59 between? and if
        1 upstairs-closing-done !
        scene @ case
            0 of  upstairs-way-out start-event  endof
            1 of  ['] events.ovl002:entry-80104D34 start-event  endof
            2 of  ['] events.ovl002:entry-80104E24 start-event  endof
            3 of  ['] events.ovl002:entry-80104DE8 start-event  endof
            4 of  ['] events.ovl002:entry-80104DAC start-event  endof
            5 of  ['] events.ovl002:entry-80104D70 start-event  endof
        endcase
    then ;
: on-the-way ( -- )   \ Barbara's invitation outside, before the 29th; the guard's wound on floor 2
    matthew-xz {: x z :}
    x $196F4 >  z $5781D > and  today 29 < and  barbara-at-the-entrance? and
    hospital-barbara-intro-seen flag? and  barbara-invited flag? 0= and if
        barbara-invited flag-set  ['] events.ovl002:entry-801077E8 start-event exit  then
    guard-wounded-due?  floor @ 2 = and if
        x $18D4D - dup *  z $5612C - dup * + s>f fsqrt f>s 400 < if
            hospital-wounded-guard-seen flag-toggle  ['] events.ovl002:entry-80105358 start-event  then
    then ;
: timed ( -- )  closing  events-running @ if  exit  then  on-the-way ;
$18B5F constant hall-x0   $18EFB constant hall-x1   $56D91 constant hall-z0   $570CA constant hall-z1
: in-the-hall? ( -- flag )   \ floor 1, the walk off where Morgan waits (the prologue)
    matthew-xz {: x z :}  floor @ 1 =  x hall-x0 > and  x hall-x1 < and  z hall-z0 > and  z hall-z1 < and ;
: each-frame ( -- )
    attacked? if   \ after the attack the way out is shut; he stays
        zone dup 0< 0= swap dup 2 < swap the-exit = or and if  back exit  then  timed exit  then
    may-leave? if
        zone case
            left-doors of  left-doors open-doors  endof
            right-doors of  right-doors open-doors  endof
            the-exit of  leave  endof
            off-limits of  ['] events.ovl002:entry-80104F90 start-event  back  endof
            timed
        endcase exit
    then
    zone {: z :}   \ before the prologue ends: kept inside
    z off-limits = if  ['] events.ovl002:entry-80104F90 start-event  then
    z the-exit =  z off-limits = or  z 0< 0= z 2 < and or if  back exit  then
    in-the-hall?  hospital-walk-off-seen session@ 0<> and if
        pending-scenes session@ 1 xor pending-scenes session!  ['] events.ovl002:entry-80105284 start-event exit  then
    timed ;

\ ---- ○ and □ (0x80103104 / 0x80103F38, 0x80104060 / 0x80103254) ----
enum: look-action  nothing-here  barbaras-door  2 the-cigarettes  the-board-of-news  the-bulletin  the-other-door
  the-other-act  the-front-door  8 unused-8  cohen-in-bed ;
: look-check ( zone check -- action )
    nip case
        $04 of  prologue-done flag? if  the-front-door  else  nothing-here  then  endof
        $49 of  knocked-barbara @ if  barbaras-door  else  nothing-here  then  endof
        $C1 of  $020 story@ $14000 and $4000 = if  the-board-of-news  else  nothing-here  then  endof
        $0D of  prologue-done flag? if  the-bulletin  else  nothing-here  then  endof
        $43 of  knocked-other @ if  the-other-door  else  nothing-here  then  endof
        $4D of  cohen-in-his-bed? if  cohen-in-bed  else  nothing-here  then  endof
        nothing-here swap
    endcase ;
: look-do ( action -- )
    case
        barbaras-door of
            $1C 14 story-bit flag? 0=  barbara-in-her-room? and  lidds-by-barbara? and  today 28 = and if
                ['] events.ovl002:entry-80104FD0  else  ['] events.ovl002:entry-80106888  then start-event  endof
        the-board-of-news of  ['] events.ovl002:entry-80106900 start-event  endof
        the-bulletin of  main-exe $80047BD8 native  endof   \ (src: evfn_80047BD8: the recap - story.fs)
        the-other-door of  ['] events.ovl002:entry-8010671C start-event  endof
        the-front-door of  ['] events.ovl002:entry-801065B4 start-event  endof
        cohen-in-bed of  cohen talk-with  endof
        drop run-rows 0
    endcase ;
enum: act-action  nothing-to-do  2 knock-barbara  6 knock-other ;
: act-check ( zone check -- action )
    nip case
        $43 of  knocked-other @ 0=  prologue-done flag? and if  knock-other  else  nothing-to-do  then  endof
        $49 of  knocked-barbara @ 0=  $010 story@ 4 and 0<> and if  knock-barbara  else  nothing-to-do  then  endof
        nothing-to-do swap
    endcase ;
: act-do ( action -- )
    case
        knock-barbara of   \ with her cigarettes on her birthday visit: she takes them
            $01C story@ $6000 and $2000 =  28 15 5 20 20 between? and  cigarettes has-item? and if
                cigarettes take  gave-barbara-cigarettes flag-toggle  ['] events.ovl002:entry-801077F8 start-event exit
            then
            ['] events.ovl002:entry-80096DC8 start-event  1 knocked-barbara !  endof
        knock-other of  ['] events.ovl002:entry-80096DC8 start-event  1 knocked-other !  endof
        drop run-rows 0
    endcase ;

\ ---- talking here (0x80103698): their lines by their stops ----
\ by stop, standing and walking (0: none) - src: D_80101FC4 / D_8010203C (Samuel), D_801020B4 / D_801020F4
\ (Barbara), D_80104FFC / D_8010507C (James)
create samuel-standing
  270 , 272 , 0 , 273 , 275 , 277 , 0 , 278 , 280 , 282 , 284 , 286 , 288 , 290 , 292 , 0 , 293 , 295 , 297 , 299 ,
  301 , 303 , 305 , 307 , 0 , 308 , 310 , 312 , 314 , 316 , 318 , 320 , 322 , 0 , 323 , 325 , 327 , 329 , 331 , 333 ,
  335 , 337 , 0 , 338 , 340 , 342 , 344 , 346 , 348 , 350 , 352 , 0 , 353 , 355 , 357 , 359 , 361 , 363 , 365 , 367 ,
create samuel-walking
  271 , 0 , 0 , 274 , 276 , 0 , 0 , 279 , 281 , 283 , 285 , 287 , 289 , 291 , 0 , 0 , 294 , 296 , 298 , 300 ,
  302 , 304 , 306 , 0 , 0 , 309 , 311 , 313 , 315 , 317 , 319 , 321 , 0 , 0 , 324 , 326 , 328 , 330 , 332 , 334 ,
  336 , 0 , 0 , 339 , 341 , 343 , 345 , 347 , 349 , 351 , 0 , 0 , 354 , 356 , 358 , 360 , 362 , 364 , 366 , 0 ,
60 constant samuel-stops
create barbara-standing
  452 , 454 , 456 , 458 , 460 , 462 , 464 , 466 , 468 , 470 , 472 , 474 , 476 , 478 , 480 , 482 , 484 , 486 , 488 , 490 ,
  492 , 494 , 496 , 498 , 500 , 502 , 504 , 506 , 508 , 510 , 0 , 0 ,
create barbara-walking
  453 , 455 , 457 , 459 , 461 , 463 , 465 , 467 , 469 , 471 , 473 , 475 , 477 , 479 , 481 , 483 , 485 , 487 , 489 , 491 ,
  493 , 495 , 497 , 499 , 501 , 503 , 505 , 507 , 509 , 0 , 0 , 0 ,
32 constant barbara-stops   524 constant barbaras-own
create james-standing
  368 , 370 , 372 , 374 , 376 , 378 , 380 , 0 , 0 , 381 , 383 , 385 , 387 , 0 , 388 , 390 , 392 , 394 , 0 , 0 ,
  395 , 397 , 399 , 401 , 0 , 402 , 404 , 406 , 408 , 0 , 0 , 409 , 411 , 413 , 415 , 0 , 416 , 418 , 420 , 422 ,
  0 , 0 , 423 , 425 , 427 , 429 , 0 , 430 , 432 , 434 , 436 , 0 , 0 , 437 , 439 , 441 , 443 , 0 , 444 , 446 ,
  448 , 450 , 0 , 0 ,
create james-walking
  369 , 371 , 373 , 375 , 377 , 379 , 0 , 0 , 0 , 382 , 384 , 386 , 0 , 0 , 389 , 391 , 393 , 0 , 0 , 0 ,
  396 , 398 , 400 , 0 , 0 , 403 , 405 , 407 , 0 , 0 , 0 , 410 , 412 , 414 , 0 , 0 , 417 , 419 , 421 , 0 ,
  0 , 0 , 424 , 426 , 428 , 0 , 0 , 431 , 433 , 435 , 0 , 0 , 0 , 438 , 440 , 442 , 0 , 0 , 445 , 447 ,
  449 , 0 ,
64 constant james-stops
\ (stop, message) pairs ended by -1 - src: D_801050F8 / D_80105138 (Cohen), D_801051B4 (83), D_80105158 / D_80105194 (119)
create cohen-standing
  43 , 241 ,  49 , 242 ,  50 , 244 ,  51 , 246 ,  52 , 247 ,  53 , 248 ,  54 , 250 ,  55 , 252 ,  56 , 253 ,  57 , 254 ,
  58 , 256 ,  59 , 258 ,  60 , 259 ,  61 , 260 ,  62 , 262 ,  -1 ,
create cohen-walking  49 , 243 ,  50 , 245 ,  53 , 249 ,  54 , 251 ,  57 , 255 ,  58 , 257 ,  61 , 261 ,  -1 ,
create person-83-lines  2 , 517 ,  11 , 517 ,  20 , 517 ,  29 , 517 ,  38 , 517 ,  47 , 517 ,  56 , 517 ,  -1 ,
create person-119-standing
  4 , 515 ,  5 , 516 ,  12 , 515 ,  13 , 516 ,  20 , 515 ,  21 , 516 ,  28 , 515 ,  29 , 516 ,  36 , 515 ,  37 , 516 ,
  44 , 515 ,  45 , 516 ,  52 , 515 ,  53 , 516 ,  -1 ,
create person-119-walking  4 , 515 ,  12 , 515 ,  20 , 515 ,  28 , 515 ,  36 , 515 ,  44 , 515 ,  52 , 515 ,  -1 ,
create morgan-lines  $3B , $1FF ,  $45 , $200 ,  $4F , $201 ,  -1 ,
create lidds-lines  2 , $107 ,  9 , $108 ,  17 , $109 ,  23 , $10A ,  29 , $10B ,  35 , $10C ,  42 , $10D ,  -1 ,
7 constant usual-gesture   1 constant morgans-gesture   8 constant brief-gesture   0 constant no-gesture-here
$20D constant morgan-lets-go   $1C3 constant bartons-line   $202 constant winonas-line   $2724 constant just-hmm
3 constant guards-wounded-line   $F0 constant guards-line
: their-stop ( slot -- stop )  person-of at-stop @ ;
: by-stop ( slot standing walking #stops -- handled? )   \ a line from the tables by their stop (0: none)
    {: s st wk n :}  s their-stop {: k :}  k n u< 0= if  false exit  then
    s walking? if  wk  else  st  then  k cells + @  ?dup 0= if  false exit  then  s usual-gesture one-liner  true ;
: from-list ( slot list gesture -- handled? )   \ standing at a stop the list has
    {: s t g :}  s stop-of 0< if  false exit  then
    t s their-stop a-line-by-stop  dup 0< if  drop false exit  then  s g one-liner  true ;
: from-lists ( slot standing walking standing-gesture -- handled? )
    {: s st wk g :}  s walking? if  wk usual-gesture  else  st g  then {: t gg :}
    t s their-stop a-line-by-stop  dup 0< if  drop false exit  then  s gg one-liner  true ;
: morgan-talks ( -- handled? )   \ src: func_8010335C: before the prologue ends, at his spot his walk-off scene once
    prologue-done flag? if  morgan morgan-lines usual-gesture from-list exit  then
    morgan-at-his-spot? if
        hospital-walk-off-seen session@ 0= if
            1 hospital-walk-off-seen session!  ['] events.ovl002:entry-80105274 start-event  then  true exit
    then  morgan-lets-go morgan morgans-gesture one-liner  true ;
: guard-talks ( -- handled? )   \ src: func_80103598
    hospital-wounded-guard-seen flag? if  guards-wounded-line the-guard no-gesture-here one-liner  true exit  then
    guard-wounded-due? if  hospital-wounded-guard-seen flag-set  ['] events.ovl002:entry-80105358 start-event  true exit  then
    $020 story@ $30 and $10 = if   \ (minigame 35: its script)
        $020 story@ $20 xor $020 story!  ['] events.ovl002:entry-8010780C start-event  true exit  then
    $1C4 story@ 2 and 0=  $044 story@ 1 and and if
        $1C4 story@ 2 or $1C4 story!  ['] events.ovl002:entry-80106374 start-event  true exit  then
    guards-line the-guard usual-gesture one-liner  true ;
: winona-talks ( -- handled? )
    hospital-wolf-visit-seen flag? 0=  $044 story@ 1 and and  winona-by-the-guard? and if
        hospital-wolf-visit-seen flag-toggle  ['] events.ovl002:entry-80105338 start-event  true exit  then
    winona $2C standing-at? if  winonas-line winona brief-gesture one-liner  true exit  then
    false ;
: barbara-talks ( -- handled? )
    barbara their-stop {: k :}  k barbara-stops u< if
        barbara walking? if  barbara-walking  else  barbara-standing  then  k cells + @  else  0  then
    ?dup 0= if  barbaras-own  then  barbara usual-gesture one-liner  true ;
: patient-176-talks ( -- handled? )
    prologue-done flag? 0=  leaving-with-morgan? and if  ['] events.ovl002:entry-80104EE0 start-event  true exit  then
    just-hmm patient-176 brief-gesture one-liner  true ;
: talks-here ( slot -- handled? )
    case
        morgan of  morgan-talks  endof
        cohen of  cohen cohen-standing cohen-walking in-their-place from-lists  endof
        samuel of  samuel samuel-standing samuel-walking samuel-stops by-stop  endof
        james of  james james-standing james-walking james-stops by-stop  endof
        the-guard of  guard-talks  endof
        father-barton of  father-barton 1 standing-at? if  bartons-line father-barton usual-gesture one-liner  true
            else  false  then  endof
        winona of  winona-talks  endof
        lidds of  lidds lidds-lines usual-gesture from-list  endof
        person-83 of  person-83 person-83-lines person-83-lines usual-gesture from-lists  endof
        barbara of  barbara-talks  endof
        patient-94 of  just-hmm patient-94 brief-gesture one-liner  true  endof
        person-119 of  person-119 person-119-standing person-119-walking usual-gesture from-lists  endof
        patient-176 of  patient-176-talks  endof
        false swap
    endcase ;
' talks-here this-area area-talk!   ' talks-here upstairs area-talk!   ' talks-here the-other-wing area-talk!

\ ---- their hospital talks (main calls them in the hospital: each once, in order) ----
: in-the-hospital? ( -- flag )  area @ content this-area = ;
: once ( word -- )  1 swap story! ;
: says ( xt -- true )  start-event  true ;
: samuel-talks ( -- handled? )   \ src: func_80103B18
    in-the-hospital? 0= if  false exit  then
    $0B0 story@ 0=  $1BC story@ 1 and and  today 25 = and if  $0B0 once  ['] events.ovl002:entry-80105FD4 says exit  then
    $0B4 story@ 0=  epoch-analysis-scene-seen flag? and if  $0B4 once  ['] events.ovl002:entry-80106000 says exit  then
    $0B8 story@ 0=  $020 story@ $20 and and if  $0B8 once  ['] events.ovl002:entry-8010606C says exit  then
    $0BC story@ 1 and 0=  $044 story@ 1 and and if
        $0BC story@ 1 or $0BC story!  ['] events.ovl002:entry-801060CC says exit  then
    false ;
: nurse-talks ( -- handled? )   \ src: func_80103C78
    in-the-hospital? 0= if  false exit  then
    $1B8 story@ 0=  $0A0 story@ 0<> and if  $1B8 once  ['] events.ovl002:entry-80105ED8 says exit  then
    $1BC story@ 1 and 0=  today 25 = and if  $1BC story@ 1 xor $1BC story!  ['] events.ovl002:entry-80105F98 says exit  then
    false ;
: barbaras-talks ( -- handled? )   \ src: func_80103D40
    in-the-hospital? 0= if  false exit  then
    $090 story@ 0=  $01E story-c@ 1 and and if  $090 once  ['] events.ovl002:entry-80106194 says exit  then
    $094 story@ 1 and 0=  $090 story@ 0<> and if  $094 story@ 1 or $094 story!  ['] events.ovl002:entry-8010623C says exit  then
    false ;
: james-talks ( -- handled? )   \ src: func_80103DF0: after Winona's book; the bear necklace
    in-the-hospital? 0= if  false exit  then
    $168 story@ 0=  found-winonas-book flag? and if  $168 once  ['] events.ovl002:entry-801063C8 says exit  then
    $16C story@ 0=  $168 story@ 0<> and if  $16C once  ['] events.ovl002:entry-80106434 says exit  then
    $020 story@ $C0000 and $40000 =  morgan-necklace-talk-done flag? and  james-at-work? and if
        bear-necklace-scene-seen flag-toggle  bear-necklace give  got-bear-necklace flag-set
        ['] events.ovl002:entry-80105328 says exit  then
    false ;
: lidds-talks ( -- handled? )   \ src: func_801041DC
    in-the-hospital? 0= if  false exit  then
    $07C story@ 1 and  gave-barbara-cigarettes flag? 0= or if  false exit  then
    $07C story@ 1 or $07C story!  ['] events.ovl002:entry-80106174 says ;
: cohen-talks ( -- handled? )   \ src: func_8010424C
    in-the-hospital? 0= if  false exit  then
    $09C story@ 2 and  gave-cohen-envelope flag? 0= or  $028 story@ 0= or  cohen walking? or if  false exit  then
    $09C story@ 2 or $09C story!  ['] events.ovl002:entry-80106314 says ;
: morgans-talks ( -- handled? )   \ src: func_801042DC
    in-the-hospital? 0= if  false exit  then
    $13C story@ 1 and  cohen-found-scene-seen flag? 0= or if  false exit  then
    $13C story@ 1 or $13C story!  ['] events.ovl002:entry-801062A8 says ;
' samuel-talks samuel conversation!   ' nurse-talks the-nurse conversation!   ' barbaras-talks barbara conversation!
' james-talks james conversation!   ' lidds-talks lidds conversation!   ' cohen-talks cohen conversation!
' morgans-talks morgan conversation!

\ ---- the C its events call (ovl002's evfn_*) ----
: a-late-door ( -- )   \ 0x80104188: 19:55 - 23:59 shut, else in
    0 19 55 23 59 between? if  ['] events.ovl002:entry-80104E60  else  ['] events.ovl002:entry-80106A1C  then  start-event ;
: walk-off-or-door ( -- )   \ 0x8010434C
    prologue-done flag? if
        attacked? 0=  isabella person-of at-stop @ $2B >= and if  ['] events.ovl002:entry-80105204
        else  ['] events.ovl002:entry-80106794  then  start-event exit
    then
    morgan-at-his-spot?  hospital-walk-off-seen session@ 0= and if
        1 hospital-walk-off-seen session!  ['] events.ovl002:entry-80105274 start-event exit  then
    ['] events.ovl002:entry-801075FC start-event ;
: not-yet ( -- xt )   \ before the prologue ends: one of two "not now"s
    hospital-walk-off-seen session@ 0= if  ['] events.ovl002:entry-801075EC  else  ['] events.ovl002:entry-801075FC  then ;
: the-lab-door ( -- )   \ 0x80104428
    prologue-done flag? if  ['] events.ovl002:entry-8010671C  else  not-yet  then  start-event ;
: cohens-door ( -- )   \ 0x8010448C
    prologue-done flag? 0= if  not-yet start-event exit  then
    cohen-in-hospital?  $160 story@ 0= and if  ['] events.ovl002:entry-80105210  else  ['] events.ovl002:entry-8010680C  then
    start-event ;
: from-downstairs ( -- )   \ 0x80104524
    $020 story@ $C000 and $4000 = if
        hospital-from-downstairs-seen flag-toggle  ['] events.ovl002:entry-80105368 start-event  then ;
: the-stairs ( -- )   \ 0x80104584
    prologue-done flag? if  ['] events.ovl002:entry-80106E98  else  ['] events.ovl002:entry-801075DC  then  start-event ;
: cohen-to-his-bed ( -- )   \ 0x801045EC: Cohen to his stop for the day (28th - 30th)
    today 28 - 0 max 2 min cells cohens-stops + @  cohen swap goto-stop ;
: the-ending ( -- )  bad-ending ending ;   \ 0x801045CC (src: opening_exec(1), as ending_bad)
this-area ovl $80104188 ' a-late-door native:   this-area ovl $8010434C ' walk-off-or-door native:
this-area ovl $80104428 ' the-lab-door native:   this-area ovl $8010448C ' cohens-door native:
this-area ovl $80104524 ' from-downstairs native:   this-area ovl $80104584 ' the-stairs native:
this-area ovl $801045EC ' cohen-to-his-bed native:   this-area ovl $801045CC ' the-ending native:

' look-check ' look-do ' act-check ' act-do  this-area area-rules!
' events.ovl002:entry-8010780C 35 minigame-script!   \ (src: area_minigame_start's 35)
\ ---- each frame (src: func_80104774 / func_801047E4, before drawing): in scene 0 one of two object sets by where
\ he stands; the windows' palette by the hour (texture animation 0: the day's, from 16:00 the evening's) ----
$19 constant far-set   $1A constant near-set   $19064 constant sets-divide   16 constant evening-hour
0 constant windows-anim
: windows-by-hour ( -- )  windows-anim  clock-time time>hour evening-hour >= 1 and  texture-frame ;
: sets-by-place ( -- )
    scene @ 0<>  matthew-there? 0= or if  exit  then
    matthew-now 2drop drop sets-divide > {: far? :}  far-set far? shown!  near-set far? 0= shown! ;
: hospital-drawn ( -- )  sets-by-place  windows-by-hour ;
this-area ' hospital-drawn area-every-frame!   29 ' windows-by-hour area-every-frame!
this-area ' entered ' scene-hook area-hooks!
this-area ' each-frame area-frame!

\ ---- shown an item (src: actor_handler_168): the tape of "Eternal", once ----
7 constant eternal-tape   $64 constant hears-the-tape
$020 18 story-bit constant tape-heard-168   \ (src: +0x20 bit 18, toggled)
:noname ( slot item -- answer )  nip eternal-tape =  tape-heard-168 flag? 0= and if  hears-the-tape  else  0  then ;
visitor-168 item-answer!
:noname ( slot answer -- )
    nip hears-the-tape = if  tape-heard-168 flag-toggle  ['] events.ovl002:entry-80105318 start-event  then ;
visitor-168 item-shown!
