\ pack-053-2.fs - event pack 053_2 (CD.BIN 267): 1 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-053-2
USING: events ;

\ $8001000C
: script-0
    morgan as-this-one
    0 set-scene
    matthew 29 2 set-floor
    this-one 101881 0 350263 set-pos
    this-one 0 1 anim
    this-one 29 2 set-floor
    matthew 101598 350996 walk-to
    matthew wait-stop
    this-one 0 0 face
    matthew 1 0 face
    -656 -2924 -254 101833 -1056 349993 2014 camera-set
    this-one 15000 7 talk
    this-one 15001 5 talk
    matthew 15002 4 talk
    this-one 102185 350150 walk-to
    this-one wait-stop
    this-one 23 0 through-door
    matthew 81 0 anim
    matthew wait-anim
    -446 -2600 -1100 101887 -1116 350767 1966 camera-set
    matthew 101372 349905 walk-to
    matthew wait-stop
    matthew 1024 0 turn-to
    matthew 70 72 anim-at  matthew wait-anim
    20 frames wait
    this-one 15003 0 talk
    -572 -2744 -398 102859 -1080 350203 1030 camera-set
    10 frames wait
    james 101711 0 352473 set-pos
    james 29 2 set-floor
    father-barton 101851 0 351963 set-pos
    father-barton 29 2 set-floor
    samuel 15004 0 talk
    this-one 15005 0 talk
    samuel 15006 0 talk
    samuel 15007 0 talk
    10 frames wait
    samuel 15008 0 talk
    this-one 15009 0 talk
    samuel 15010 0 talk
    10 frames wait
    this-one 15011 0 talk
    samuel 15012 0 talk
    107 actor 15013 0 talk
    samuel 15014 0 talk
    10 frames wait
    father-barton as-this-one
    this-one 101851 350503 walk-to
    1 frames wait
    james 101711 350853 walk-to
    matthew 76 77 anim-at  matthew wait-anim
    10 frames wait
    -494 -3068 -1244 102193 -996 350635 310 camera-set
    matthew 71 0 anim
    matthew wait-anim
    matthew 0 1 anim
    matthew 604 -200 turn-to
    this-one wait-stop
    this-one 0 200 face
    matthew 15015 7 talk
    -968 -2270 -494 102805 -1008 351391 526 camera-set
    15047 1 say-mode-wait
    15048 1 say-mode-wait
    15049 1 say-mode-wait
    this-one 15016 7 talk
    -494 -3068 -1244 102193 -996 350635 310 camera-set
    10 frames wait
    matthew 87 0 anim
    matthew wait-anim
    matthew 15017 3 talk
    this-one 17 18 anim-at  this-one wait-anim
    10 frames wait
    this-one 15018 0 talk
    this-one 19 0 anim
    this-one wait-anim
    this-one 102185 350150 walk-to
    this-one wait-stop
    this-one 1024 -200 turn-to
    this-one wait-turned
    5 frames wait
    this-one 716 717 anim-at  this-one wait-anim
    10 frames wait
    this-one 718 0 anim
    this-one wait-anim
    2 door-sound
    this-one 65 0 anim
    this-one wait-anim
    10 frames wait
    morgan 15020 0 talk
    this-one 15021 0 talk
    10 frames wait
    samuel 15022 0 talk
    this-one 23 0 through-door
    james 101852 350535 walk-to
    james wait-stop
    james 14 15 anim-at  james wait-anim
    james 15019 0 talk
    james 16 0 anim
    james wait-anim
    james 102185 350150 walk-to
    james wait-stop
    james 1024 -200 turn-to
    james wait-turned
    james 23 0 through-door
    -62 -1778 -2852 102037 -888 349717 1750 camera-set
    matthew 101892 350055 walk-to
    matthew wait-stop
    matthew 716 717 anim-at  matthew wait-anim
    this-one 15023 0 talk
    samuel 15024 0 talk
    10 frames wait
    this-one 15025 0 talk
    10 frames wait
    matthew 718 0 1 anim-then
    102 0 sequence-play
    20 frames wait
    15026 107 actor 1 say-timed-by-wait
    text-close
    125 sound
    15028 samuel 1 say-timed-by-wait
    text-close
    25 frames wait
    124 sound
    matthew 102185 350150 walk-to
    15030 107 actor 2 say-timed-by-wait
    text-hold
    15031 samuel 2 say-timed-by-wait
    text-hold
    15032 107 actor 1 say-timed-by-wait
    text-hold
    125 sound
    15034 107 actor 1 say-timed-by-wait
    text-close
    35 frames wait
    15035 32767 actor 3 say-timed-by-wait
    text-hold
    35 frames wait
    15036 32767 actor 1 say-timed-by-wait
    text-hold
    15037 32767 actor 1 say-timed-by-wait
    text-hold
    15038 32767 actor 1 say-timed-by-wait
    text-hold
    20 frames wait
    15039 107 actor 1 say-timed-by-wait
    text-hold
    10 frames wait
    15040 samuel 2 say-timed-by-wait
    text-close
    30 frames wait
    matthew 81 0 anim
    matthew wait-anim
    matthew 0 1024 0 set-rot
    matthew 62 0 anim
    matthew wait-anim
    morgan as-this-one
    matthew 101652 350115 2 20 walk-to-ex
    this-one 23 4 through-door
    matthew 15041 5 talk
    10 frames wait
    this-one 86 0 anim
    this-one wait-anim
    this-one 15042 0 talk
    matthew 15043 0 talk
    this-one 15044 3 talk
    this-one 716 717 anim-at  this-one wait-anim
    this-one 15045 0 talk
    10 frames wait
    this-one 718 0 anim-at  this-one wait-anim
    this-one 101472 352935 walk-to
    10 frames wait
    matthew 17 18 anim-at  matthew wait-anim
    this-one wait-stop
    this-one 0 0 0 set-pos
    matthew 19 0 anim-at  matthew wait-anim
    15046 5 say-mode-wait
    10 frames wait
    end-script
    ;

\ its scripts by number
' script-0 0 267 pack-script!
