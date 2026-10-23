\ pack-051-0.fs - event pack 051_0 (CD.BIN 255): 3 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-051-0
USING: events ;

\ $80010014
: script-0
    5 set-scene
    lidds as-this-one
    barbara 29 2 set-floor
    barbara 98786 0 350590 set-pos
    barbara 0 1444 0 set-rot
    this-one 29 2 set-floor
    this-one 98756 0 349610 set-pos
    this-one 0 2048 0 set-rot
    matthew 29 2 set-floor
    matthew 100921 0 351091 set-pos
    matthew 0 3072 0 set-rot
    -512 -848 -590 99805 -1212 350069 -1316 camera-set
    map-sync
    20 frames wait
    matthew 81 0 anim
    this-one 36 38 0 anim-then
    this-one wait-anim
    this-one 51 1 anim
    10 frames wait
    this-one 39 37 0 anim-then
    this-one wait-anim
    this-one 17 18 1 anim-then
    10 frames wait
    this-one 19 0 1 anim-then
    this-one 633 -100 turn-to
    matthew 99721 350101 walk-to
    this-one wait-turned
    -1298 -2696 -1832 98947 -168 350065 10768 camera-set
    this-one 15000 7 talk
    matthew 15001 1 talk
    matthew 14 15 1 anim-then
    barbara 15002 0 talk
    matthew 15003 0 talk
    matthew 16 0 1 anim-then
    this-one 15004 4 talk
    matthew 15005 3 talk
    this-one 17 18 1 anim-then
    barbara 15006 0 talk
    matthew 14 15 1 anim-then
    this-one 19 0 1 anim-then
    matthew 15007 0 talk
    this-one 15008 7 talk
    barbara 15009 0 talk
    matthew 15010 0 talk
    this-one 15011 7 talk
    barbara 15012 0 talk
    matthew 16 0 1 anim-then
    matthew 15013 5 talk
    this-one 15014 2 talk
    matthew 15015 1 talk
    matthew 14 15 1 anim-then
    barbara 15016 0 talk
    matthew 15017 0 talk
    this-one 85 0 1 anim-then
    this-one 15018 3 talk
    matthew 16 0 1 anim-then
    matthew 15019 0 talk
    matthew 83 84 1 anim-then
    barbara 15020 0 talk
    this-one 17 18 1 anim-then
    this-one 15021 0 talk
    matthew 85 0 1 anim-then
    10 frames wait
    end-script
    ;

\ $80010420
: script-1
    samuel as-this-one
    1 set-scene
    matthew 29 2 set-floor
    matthew 100810 0 358564 set-pos
    matthew 0 3072 0 set-rot
    matthew 0 1 anim
    153 actor 1 actor-model
    153 actor 29 2 set-floor
    153 actor 98749 0 358489 set-pos
    153 actor 0 4052 0 set-rot
    153 actor 15 1 anim
    this-one 29 2 set-floor
    this-one 98789 0 359059 set-pos
    this-one 0 1200 0 set-rot
    this-one 0 1 anim
    -590 -2738 -1238 100336 -1200 358856 -1269 camera-set
    matthew 99430 358564 walk-to
    15023 samuel 2 say-timed-by-wait
    -482 -2798 -1184 98656 -1032 358382 -1509 camera-set
    matthew wait-stop
    matthew 15024 5 talk
    153 actor 16 0 1 anim-then
    153 actor 15025 0 talk
    matthew 15026 7 talk
    this-one 14 15 1 anim-then
    this-one 15027 0 talk
    153 actor 15028 6 talk
    this-one 16 0 anim-at  this-one wait-anim
    this-one 15029 3 talk
    153 actor 15030 5 talk
    this-one 83 84 anim-at  this-one wait-anim
    matthew 81 0 anim
    matthew wait-anim
    this-one 85 0 anim-at  this-one wait-anim
    17 music
    this-one 15031 3 talk
    -866 -1538 -686 99892 -948 359036 -3573 camera-set
    matthew 15032 5 talk
    this-one 153 0 face
    -884 -3410 -422 99712 -924 359324 -3549 camera-set
    153 actor 15033 5 talk
    this-one 15034 6 talk
    153 actor 15035 5 talk
    this-one 17 18 1 anim-then
    this-one 15036 3 talk
    153 actor 0 200 face
    153 actor 15037 5 talk
    matthew 15038 2 talk
    5 frames wait
    9 screen-effect
    wait-effect
    matthew 98880 0 358714 set-pos
    matthew 0 280 0 set-rot
    matthew 0 1 anim
    153 actor 99489 0 359269 set-pos
    153 actor 0 2310 0 set-rot
    153 actor 84 1 anim
    this-one 98679 0 359279 set-pos
    this-one 0 1024 0 set-rot
    this-one 74 1 anim
    -704 -3086 -704 98422 -900 359576 -4677 camera-set
    10 frames wait
    13 music
    8 screen-effect
    wait-effect
    this-one 15039 0 talk
    153 actor 15040 0 talk
    this-one 75 72 anim-at  this-one wait-anim
    153 actor 15042 0 talk
    this-one 15043 8 talk
    153 actor 85 0 1 anim-then
    matthew 15044 5 talk
    153 actor 15045 3 talk
    matthew 15046 1 talk
    153 actor 15047 4 talk
    10 frames wait
    153 actor 99568 358512 walk-to  153 actor wait-stop
    153 actor 100960 358608 walk-to  153 actor wait-stop
    153 actor 22 1 through-door
    153 actor 0 0 0 set-pos
    end-script
    ;

\ $800108F4
: script-2
    winona as-this-one
    0 set-scene
    wolf 189 actor-model
    matthew 101665 0 349890 set-pos
    matthew 0 2818 0 set-rot
    matthew 0 1 anim
    matthew 29 2 set-floor
    this-one 101365 0 349660 set-pos
    this-one 0 1024 0 set-rot
    this-one 167 1 anim
    this-one 29 2 set-floor
    wolf 101925 0 351980 set-pos
    wolf 0 1 anim
    wolf 29 2 set-floor
    -356 -1940 -146 101591 -1188 349744 3746 camera-set
    this-one 15048 0 talk
    matthew 15049 7 talk
    this-one 168 170 0 anim-then
    this-one wait-anim
    this-one 171 173 0 anim-then
    this-one wait-anim
    this-one 167 1 anim
    matthew 15050 0 talk
    -1 music
    wolf 15051 0 talk
    4178 24 camera-angle
    wolf 101925 350420 walk-to
    10 frames wait
    matthew 4498 200 turn-to
    wolf wait-stop
    this-one 171 172 1 anim-then
    matthew 15052 0 talk
    wolf 17 18 anim-at  wolf wait-anim
    30 frames wait
    wolf 19 0 anim-at  wolf wait-anim
    wolf 14 15 anim-at  wolf wait-anim
    wolf 15053 0 talk
    this-one 15054 0 talk
    wolf 15055 0 talk
    matthew 15056 5 talk
    wolf 16 0 anim-at  wolf wait-anim
    wolf 15057 0 talk
    wolf 15058 3 talk
    wolf 17 18 anim-at  wolf wait-anim
    10 frames wait
    wolf 0 -200 turn-to
    3 frames wait
    wolf 0 1 anim
    wolf wait-turned
    wolf 100995 355700 walk-to
    15 frames wait
    matthew 17 18 1 anim-then
    20 frames wait
    this-one 15059 0 talk
    matthew 15060 0 talk
    matthew 15061 0 talk
    this-one 173 167 anim-at  this-one wait-anim
    this-one 15062 0 talk
    matthew -1208 -200 turn-to
    matthew 0 1 anim
    matthew 15063 0 talk
    15 music
    this-one 15064 0 talk
    matthew 15065 6 talk
    10 frames wait
    matthew 15066 2 talk
    this-one 15067 0 talk
    this-one 171 172 anim-at  this-one wait-anim
    this-one 15068 0 talk
    matthew 85 0 anim-at  matthew wait-anim
    matthew 15069 0 talk
    this-one 173 167 anim-at  this-one wait-anim
    10 frames wait
    wolf 0 0 0 set-pos
    end-script
    ;

\ its scripts by number
' script-0 0 255 pack-script!
' script-1 1 255 pack-script!
' script-2 2 255 pack-script!
