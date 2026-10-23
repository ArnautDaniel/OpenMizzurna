\ main.fs - the main executable's scripts (started from its C, called by others).
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.main
USING: events ;

\ (looped back to: declared first)
defer L-80099064

\ $80096E34
: sub-80096E34
    -500 -1544 -1326 0 0 0 0 camera-set
    matthew camera-at
    matthew camera-heading
    5 0 camera-turn
    end-script
    ;

\ $80087B60
: exe-80087B60
    sub-80096E34
    111 actor 10021 0 talk
    end-script
    ;

: L-800920B0
    this-one 10046 7 talk
    this-one 1 ride
    this-one restore-state
    this-one reset
    main-exe $80032CE4 native
    end-script
    ;

: L-800920D0
    10045 1 choice
    0 result= if  goto L-800920B0  then
    matthew 10047 7 talk
    this-one 10048 6 talk
    40 frames wait
    this-one 10049 1 talk
    this-one restore-state
    end-script
    ;

\ $80092104
: exe-80092104
    winona as-this-one
    this-one save-state
    this-one 10043 7 talk
    this-one 10044 1 talk
    goto L-800920D0
    ;

\ $80092120
: exe-80092120
    winona as-this-one
    this-one save-state
    this-one 10050 1 talk
    goto L-800920D0
    ;

: L-80096138
    -474 -2210 -1106 0 0 0 260 camera-set
    end-script
    ;

\ $80096178
: exe-80096178
    matthew as-this-one
    this-one save-state
    L-80096138
    matthew 0 1 anim
    this-one camera-at
    this-one 0 0 face
    this-one 0 0 talk
    this-one restore-state
    end-script
    ;

\ $800961D4
: exe-800961D4
    matthew as-this-one
    this-one freeze
    L-80096138
    matthew 0 1 anim
    this-one camera-at
    this-one 0 0 face
    this-one 0 0 talk
    this-one unfreeze
    end-script
    ;

\ $80096230
: exe-80096230
    matthew as-this-one
    this-one save-state
    this-one 0 0 talk
    this-one restore-state
    end-script
    ;

\ $8009625C
: exe-8009625C
    matthew as-this-one
    this-one freeze
    L-80096138
    this-one camera-at
    this-one 0 0 talk
    this-one unfreeze
    end-script
    ;

\ $80096298
: exe-80096298
    matthew as-this-one
    L-80096138
    this-one camera-at
    this-one 0 0 talk
    end-script
    ;

\ $800963FC
: exe-800963FC
    118 -1370 -2450 0 0 0 1761 camera-set
    winona camera-at
    winona 10052 1 talk
    winona arrive
    end-script
    ;

\ $8009643C
: exe-8009643C
    118 -1370 -2450 0 0 0 1761 camera-set
    winona camera-at
    winona 10055 2 talk
    matthew 10056 7 talk
    winona 10057 6 talk
    main-exe $80047B7C native
    end-script
    ;

\ $80096598
: exe-80096598
    9 screen-effect
    wait-effect
    main-exe $80047620 native
    end-script
    ;

\ $80096A34
: exe-80096A34
    matthew as-this-one
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    3 frames wait
    matthew 2 -20 move-forward
    16 frames wait
    matthew idle
    this-one 0 0 through-door-back
    this-one 10013 1 talk
    this-one door-hold
    end-script
    ;

\ $80096A94
: exe-80096A94
    matthew as-this-one
    matthew 0 1 anim
    matthew this-one camera-face
    4 0 camera-turn
    this-one 0 6 talk
    this-one 0 0 through-door
    this-one place-at-stop
    main-exe $80047614 native
    end-script
    ;

\ $80096D1C
: sub-80096D1C
    this-one 64 0 anim
    this-one anim-hold
    this-one wait-anim
    end-script
    ;

\ $80096AE8
: exe-80096AE8
    dennis as-this-one
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    3 frames wait
    matthew 2 -20 move-forward
    16 frames wait
    matthew idle
    this-one 92274 -200 53152 set-pos
    this-one -200 set-y
    this-one 5056 0 turn-to
    2 object-anim
    sub-80096D1C
    this-one 10016 5 talk
    this-one door-hold
    end-script
    ;

\ $80096B70
: exe-80096B70
    dennis as-this-one
    matthew 0 1 anim
    matthew this-one camera-face
    4 0 camera-turn
    this-one 10011 1 talk
    this-one 92627 -200 53149 set-pos
    this-one 3058 0 turn-to
    3 object-anim
    sub-80096D1C
    this-one place-at-stop
    main-exe $80047614 native
    end-script
    ;

\ $80096BE4
: exe-80096BE4
    winona 54 1 talk
    end-script
    ;

\ $80096BF8
: exe-80096BF8
    winona 55 1 talk
    end-script
    ;

\ $80096C0C
: exe-80096C0C
    sub-80096E34
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    0 matthew say-by-wait
    end-script
    ;

: L-80096DC8
    2 door-sound
    matthew 65 0 anim
    matthew wait-anim
    end-script
    ;

\ $80096C40
: exe-80096C40
    2 door-sound
    L-80096DC8
    63 say-wait-close
    -1776 -8064 -436 392730 228 419791 -1385 camera-set
    25 2 caption-for-nowait
    11 screen-effect
    wait-effect
    4 0 365809 387970 1672 -2146426904 warp
    ;

\ $80096CAC
: exe-80096CAC
    1 5 say-mode-wait
    end-script
    ;

: L-80096CBC
    55 frames wait
    11 screen-effect
    10 frames wait
    main-exe $80047C5C native
    10 screen-effect
    wait-effect
    matthew wait-anim
    end-script
    ;

\ $80096CF0
: sub-80096CF0
    ' L-80096CBC start-event
    65 frames wait
    end-script
    ;

\ $80096D34
: sub-80096D34
    this-one 70 0 anim
    this-one wait-anim
    this-one 72 1 anim
    end-script
    ;

\ $80096D74
: sub-80096D74
    this-one 67 0 anim
    this-one wait-anim
    end-script
    ;

\ $80096E74
: sub-80096E74
    -564 -1338 -700 0 0 0 0 camera-set
    matthew camera-at
    matthew camera-heading
    190 0 camera-turn
    end-script
    ;

\ $80096EB4
: sub-80096EB4
    -600 -2526 -1258 0 0 0 0 camera-set
    matthew camera-at
    matthew camera-heading
    90 0 camera-turn
    end-script
    ;

\ $80096FB0
: sub-80096FB0
    this-one 4508 0 anim
    this-one anim-hold
    this-one wait-anim
    this-one 0 1 anim
    this-one leave-diner-seat
    end-script
    ;

\ $80098EBC
: exe-80098EBC
    10000 0 choice
    main-exe $80047C90 native
    end-script
    ;

: L-80098ED4
    10504 say-wait-close
    picture-hide
    end-script
    ;

\ $80098EE4
: exe-80098EE4
    0 picture-show
    10502 say-wait-close
    10503 0 choice
    0 result= if  goto L-80098ED4  then
    10505 say-wait-close
    picture-hide
    main-exe $8004791C native
    26 actor 0 0 0 set-pos
    end-script
    ;

: L-80098F34
    10511 say-wait-close
    picture-hide
    end-script
    ;

: L-80098F44
    26 actor 349685 0 283687 set-pos
    26 actor -1072 0 turn-to
    goto L-80098F34
    ;

: L-80098F68
    26 actor 249781 0 140655 set-pos
    26 actor 1007 0 turn-to
    goto L-80098F34
    ;

: L-80098F8C
    26 actor 408056 0 163688 set-pos
    26 actor -82 0 turn-to
    goto L-80098F34
    ;

: L-80098FB0
    26 actor 112168 0 353607 set-pos
    26 actor -9244 0 turn-to
    goto L-80098F34
    ;

: L-80098FD4
    26 actor 584585 0 117565 set-pos
    26 actor -7 0 turn-to
    goto L-80098F34
    ;

: L-80098FF8
    26 actor 55993 0 404621 set-pos
    26 actor -8056 0 turn-to
    goto L-80098F34
    ;

: L-8009901C
    26 actor 101759 0 57326 set-pos
    26 actor -10220 0 turn-to
    goto L-80098F34
    ;

: L-80099040
    26 actor 464130 0 417929 set-pos
    26 actor -7198 0 turn-to
    goto L-80098F34
    ;

:noname
    10507 2 choice
    0 result= if  goto L-80098F44  then
    1 result= if  goto L-80098F68  then
    10508 2 choice
    0 result= if  goto L-80098F8C  then
    1 result= if  goto L-80098FB0  then
    10509 2 choice
    0 result= if  goto L-80098FD4  then
    1 result= if  goto L-80098FF8  then
    10510 2 choice
    0 result= if  goto L-8009901C  then
    1 result= if  goto L-80099040  then
    goto L-80099064
    ; is L-80099064

\ $800990FC
: exe-800990FC
    matthew wait-anim
    0 picture-show
    10502 say-wait-close
    10506 say-wait-close
    goto L-80099064
    ;

\ $80099210
: exe-80099210
    2 1 sequence-play
    30 frames wait
    matthew 81 0 anim
    matthew wait-anim
    matthew 104 0 anim
    matthew wait-anim
    2 sequence-stop
    matthew 102 1 anim
    10023 matthew say-by-wait
    0 picture-show
    10024 32767 actor say-by-wait
    picture-hide
    10025 matthew say-by-wait
    0 picture-show
    10026 32767 actor say-by-wait
    picture-hide
    10027 matthew say-by-wait
    0 picture-show
    10028 32767 actor say-by-wait
    picture-hide
    10029 matthew say-by-wait
    10030 matthew say-by-wait
    0 picture-show
    10031 32767 actor say-by-wait
    picture-hide
    10032 matthew say-by-wait
    matthew 103 0 anim-at  matthew wait-anim
    10 frames wait
    9 screen-effect
    wait-effect
    2 -1 -1 -1 0 -2146411136 warp
    ;

\ $80099350
: exe-80099350
    252 4 call-pack-script
    end-script
    ;

\ $80099360
: exe-80099360
    250 3 call-pack-script
    209 210 cd-pictures
    250 4 call-pack-script
    0 cd-picture
    250 5 call-pack-script
    end-script
    ;

\ $8009949C
: sub-8009949C
    139 actor 4535 4519 1 anim-then
    8 frames wait
    morgan 0 1 anim
    morgan 0 0 face
    matthew 111 0 1 anim-then
    109 sound
    12 sound
    256 10 rumble
    255 10 rumble
    20 frames wait
    $80106048 goto-overlay
    ;

\ $80099518
: sub-80099518
    92 2 choice
    0 result= if  $80104A38 goto-overlay  then
    1 result= if  $80104A54 goto-overlay  then
    2 result= if  $80104820 goto-overlay  then
    end-script
    ;

\ $8009954C
: sub-8009954C
    rolein as-this-one
    give-control
    this-one 85 0 anim-at  this-one wait-anim
    this-one 328752 267840 walk-to  this-one wait-stop
    this-one wait-stop
    this-one 327616 267840 walk-to  this-one wait-stop
    this-one wait-stop
    this-one 85 0 through-door
    this-one 327568 269280 walk-to  this-one wait-stop
    this-one wait-stop
    end-script
    ;

\ $800995C0
: sub-800995C0
    give-control
    700 frames wait
    xa-stop
    end-script
    ;

\ $800996E0
: exe-800996E0
    main-exe $8004C640 native
    matthew wait-anim
    picture-hide
    end-script
    ;

\ $800996F4
: exe-800996F4
    10459 5 say-mode-wait
    end-script
    ;

\ $80099704
: exe-80099704
    10460 phone-say
    10461 say-reset
    end-script
    ;

\ $80099718
: exe-80099718
    10462 5 say-mode-wait
    10463 5 say-mode-wait
    end-script
    ;

: L-8009A888
    20 frames wait
    10087 5 say-mode-wait
    10088 5 say-mode-wait
    main-exe $8004F2C8 native
    end-script
    ;

: L-8009A8B4
    10080 say
    wait-text
    10081 say
    wait-text
    10082 say
    wait-text
    10083 say
    wait-text
    10084 say
    wait-text
    10085 say
    wait-text
    10086 say
    wait-text
    main-exe $8004F31C native-result
    0 result= if  goto L-8009A888  then
    end-script
    ;

: L-8009A920
    20 frames wait
    10094 5 say-mode-wait
    10095 5 say-mode-wait
    10096 5 say-mode-wait
    main-exe $8004F2E4 native
    end-script
    ;

: L-8009A958
    10089 say
    wait-text
    10090 say
    wait-text
    10091 say
    wait-text
    10092 say
    wait-text
    10093 say
    wait-text
    main-exe $8004F334 native-result
    0 result= if  goto L-8009A920  then
    end-script
    ;

: L-8009A9AC
    20 frames wait
    10101 5 say-mode-wait
    main-exe $8004F300 native
    end-script
    ;

: L-8009A9CC
    10097 say
    wait-text
    10098 say
    wait-text
    10099 say
    wait-text
    10100 say
    wait-text
    main-exe $8004F34C native-result
    0 result= if  goto L-8009A9AC  then
    end-script
    ;

\ $8009AA14
: exe-8009AA14
    15 picture-show
    10078 32767 actor say-by-wait
    10079 2 choice
    picture-hide
    0 result= if  goto L-8009A8B4  then
    1 result= if  goto L-8009A958  then
    2 result= if  goto L-8009A9CC  then
    end-script
    ;

\ $8009AA60
: exe-8009AA60
    203 204 cd-pictures
    10499 say
    wait-text
    0 cd-picture
    end-script
    ;

\ $8009AA84
: exe-8009AA84
    203 204 cd-pictures
    10500 say
    wait-text
    0 cd-picture
    end-script
    ;

\ $8009AAA8
: exe-8009AAA8
    197 198 cd-pictures
    10501 say
    wait-text
    0 cd-picture
    end-script
    ;

\ $8009AACC
: exe-8009AACC
    209 210 cd-pictures
    10497 say
    wait-text
    10498 say
    wait-text
    0 cd-picture
    end-script
    ;
