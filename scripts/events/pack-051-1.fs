\ pack-051-1.fs - event pack 051_1 (CD.BIN 256): 3 script ids.
\ Converted once from the bytecode by tools/ev2forth.py (docs/subsystems/story.md); edited by hand
\ from here on.
IN: events.pack-051-1
USING: events ;

\ $80010014
: script-0
    -1 music
    cohen as-this-one
    3 set-scene
    matthew 103417 0 351199 set-pos
    matthew 0 1024 0 set-rot
    matthew 0 1 anim
    matthew 29 2 set-floor
    this-one 103720 0 351067 set-pos
    this-one 0 4 0 set-rot
    this-one 735 1 anim
    this-one 29 2 set-floor
    -356 -2594 -824 103016 -1080 350604 2456 camera-set
    10 frames wait
    matthew 15000 7 talk
    this-one 15001 0 talk
    matthew 15002 4 talk
    10 frames wait
    matthew 1951 200 turn-to
    matthew wait-turned
    matthew 44 anim-once
    matthew wait-anim
    15 frames wait
    122 sound
    10 frames wait
    matthew 45 0 anim-at  matthew wait-anim
    matthew 3 -200 face
    matthew wait-turned
    1 xa-play
    50 frames wait
    this-one 15003 0 talk
    matthew 15004 6 talk
    this-one 738 739 1 anim-then
    this-one 15005 0 talk
    this-one 15006 0 talk
    matthew 14 15 1 anim-then
    matthew 15007 0 talk
    this-one 15008 0 talk
    15 frames wait
    matthew 16 0 1 anim-then
    matthew 15009 0 talk
    15 frames wait
    this-one 740 735 1 anim-then
    this-one 15010 0 talk
    matthew 15011 7 talk
    580 -812 -2354 103622 -1068 350898 2312 camera-set
    15 frames wait
    this-one 15012 0 talk
    matthew 15013 4 talk
    this-one 738 739 1 anim-then
    this-one 15014 0 talk
    matthew 15015 7 talk
    this-one 15016 0 talk
    this-one 740 735 1 anim-then
    15 music
    this-one 15017 0 talk
    matthew 15018 5 talk
    this-one 15019 0 talk
    matthew 15020 1 talk
    this-one 15021 0 talk
    matthew 15022 0 talk
    this-one 15023 0 talk
    matthew 15024 7 talk
    this-one 15025 0 talk
    this-one 15026 0 talk
    matthew 15027 5 talk
    this-one 15028 0 talk
    this-one 15029 0 talk
    matthew 15030 0 talk
    -650 -998 -368 103550 -996 351102 2816 camera-set
    10 frames wait
    this-one 15031 0 talk
    this-one 15032 0 talk
    this-one 15033 0 talk
    matthew 15034 7 talk
    this-one 15035 0 talk
    10 frames wait
    matthew 15036 4 talk
    this-one 738 739 1 anim-then
    this-one 15037 0 talk
    matthew 15038 3 talk
    this-one 15039 0 talk
    this-one 15040 0 talk
    matthew 15041 7 talk
    this-one 15042 0 talk
    this-one 740 735 1 anim-then
    this-one 15043 0 talk
    this-one 15044 0 talk
    this-one 15045 0 talk
    matthew 15046 3 talk
    this-one 15047 0 talk
    matthew 15048 0 talk
    10 frames wait
    xa-stop
    end-script
    ;

\ $800104C8
: script-1
    james as-this-one
    3 set-scene
    matthew 103631 0 352451 set-pos
    matthew 0 2048 0 set-rot
    matthew 0 1 anim
    this-one 103704 0 352091 set-pos
    this-one 0 0 0 set-rot
    this-one 72 1 anim
    -608 -1430 -1112 103373 108 352353 9780 camera-set
    10 frames wait
    matthew 15049 7 talk
    this-one 15050 8 talk
    matthew 15051 5 talk
    this-one 15052 8 talk
    10 frames wait
    matthew 15053 7 talk
    this-one 73 74 anim-at  this-one wait-anim
    this-one 15054 0 talk
    matthew 15055 0 talk
    matthew 103491 352101 walk-to
    matthew wait-stop
    matthew 103491 351961 walk-to
    matthew wait-stop
    matthew 1024 -200 turn-to
    this-one 75 72 anim-at  this-one wait-anim
    this-one 15056 0 talk
    this-one 722 723 anim-at  this-one wait-anim
    10 frames wait
    matthew 719 anim-once
    18 music
    matthew 15057 0 talk
    10 frames wait
    matthew 15058 0 talk
    this-one 15059 0 talk
    15060 5 say-mode-wait
    matthew 15061 0 talk
    matthew 720 0 anim
    matthew wait-anim
    this-one 724 72 1 anim-then
    matthew 721 51 anim-at  matthew wait-anim
    this-one 76 77 anim-at  this-one wait-anim
    matthew 15062 0 talk
    this-one 15063 0 talk
    matthew 15064 0 talk
    this-one 15065 0 talk
    matthew 141 0 anim-at  matthew wait-anim
    this-one 15066 0 talk
    this-one 78 72 anim-at  this-one wait-anim
    this-one 15067 0 talk
    matthew 15068 7 talk
    this-one 15069 8 talk
    matthew 15070 4 talk
    this-one 76 77 anim-at  this-one wait-anim
    this-one 15071 0 talk
    matthew 15072 4 talk
    matthew 134 137 0 anim-then
    matthew wait-anim
    16 picture-show
    15073 3 say-mode-wait
    picture-hide
    this-one 78 72 anim-at  this-one wait-anim
    end-script
    ;

\ $800107E0
: script-2
    111 actor as-this-one
    this-one 15074 7 talk
    matthew 15075 7 talk
    this-one 15076 7 talk
    this-one 15078 0 talk
    matthew 15079 7 talk
    this-one 15080 0 talk
    15 frames wait
    this-one 15081 3 talk
    matthew 15082 6 talk
    matthew 15083 5 talk
    15 frames wait
    this-one 15084 0 talk
    matthew 15085 7 talk
    this-one 15086 6 talk
    this-one 15087 3 talk
    matthew 15088 7 talk
    15 frames wait
    this-one 15089 0 talk
    this-one 15090 4 talk
    matthew 15091 7 talk
    end-script
    ;

\ its scripts by number
' script-0 0 256 pack-script!
' script-1 1 256 pack-script!
' script-2 2 256 pack-script!
