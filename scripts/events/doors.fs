\ doors.fs - the doors (docs/subsystems/doors.md): converted once from the executable's tables by
\ tools/doors2forth.py; edited by hand from here on. A door of an area joins two of its rooms, or a room
\ and the town (room -2); each side: its room, its spot (the area's door spots, the town's for room -2):
\ where it is, x z, which way it faces (4096 a turn) - and which way one comes out there.
IN: events.doors
USING: doors.rows ;

  0 door  0 in-area  0 room  5 spot  250150 135690 place  2048 facing  0 way  -2 room  14 spot  250150 135870 place  2048 facing  1 way  end-door
  1 door  0 in-area  0 room  0 spot  249350 135690 place  0 facing  1 way  -2 room  15 spot  249350 135870 place  0 facing  0 way  end-door
  2 door  0 in-area  0 room  13 spot  248918 134470 place  3072 facing  1 way  end-door
  3 door  0 in-area  0 room  1 spot  249950 133842 place  2048 facing  1 way  1 room  9 spot  249950 133902 place  2048 facing  0 way  end-door
  4 door  0 in-area  2 room  6 spot  247150 133098 place  0 facing  1 way  1 room  10 spot  247150 133042 place  0 facing  0 way  end-door
  5 door  0 in-area  4 room  3 spot  246850 133102 place  2048 facing  0 way  1 room  12 spot  246850 133042 place  2048 facing  1 way  end-door
  6 door  0 in-area  3 room  2 spot  250950 133102 place  0 facing  1 way  1 room  11 spot  250950 133042 place  0 facing  0 way  end-door
  7 door  0 in-area  5 room  4 spot  250562 132470 place  3072 facing  0 way  end-door
  8 door  0 in-area  5 room  7 spot  250970 132262 place  0 facing  0 way  end-door
  9 door  0 in-area  5 room  8 spot  252170 132262 place  0 facing  0 way  end-door
 10 door  0 in-area  0 room  13 spot  248918 134470 place  3072 facing  0 way  end-door
 11 door  0 in-area  5 room  4 spot  250562 132470 place  3072 facing  1 way  end-door
 12 door  1 in-area  0 room  0 spot  349410 287090 place  2048 facing  1 way  1 room  3 spot  349410 287130 place  2048 facing  0 way  end-door
 13 door  1 in-area  0 room  1 spot  349670 287554 place  3072 facing  0 way  2 room  2 spot  349630 287554 place  3072 facing  1 way  end-door
 14 door  1 in-area  1 room  4 spot  347030 287710 place  1024 facing  0 way  3 room  5 spot  347070 287710 place  1024 facing  1 way  end-door
 15 door  1 in-area  3 room  6 spot  345166 284954 place  2560 facing  1 way  -2 room  24 spot  345166 284946 place  2560 facing  0 way  end-door
 16 door  2 in-area  0 room  12 spot  101290 349230 place  0 facing  0 way  -2 room  10 spot  101290 349250 place  0 facing  1 way  end-door
 17 door  2 in-area  0 room  13 spot  102090 349230 place  2048 facing  1 way  -2 room  8 spot  102090 349250 place  2048 facing  0 way  end-door
 18 door  2 in-area  0 room  15 spot  100270 358454 place  3072 facing  1 way  1 room  17 spot  100270 358454 place  3072 facing  0 way  end-door
 19 door  2 in-area  0 room  14 spot  102310 352450 place  1024 facing  1 way  3 room  16 spot  102310 352450 place  1024 facing  0 way  end-door
 20 door  2 in-area  0 room  18 spot  98690 355670 place  0 facing  1 way  2 room  19 spot  98690 355670 place  0 facing  0 way  end-door
 21 door  29 in-area  0 room  10 spot  98690 355670 place  0 facing  1 way  2 room  11 spot  98690 355670 place  0 facing  0 way  end-door
 22 door  29 in-area  0 room  8 spot  101070 358450 place  3072 facing  1 way  1 room  9 spot  101070 358450 place  3072 facing  0 way  end-door
 23 door  29 in-area  0 room  13 spot  102310 350450 place  1024 facing  1 way  4 room  16 spot  102310 350450 place  1024 facing  0 way  end-door
 24 door  29 in-area  0 room  12 spot  102310 352050 place  1024 facing  1 way  3 room  15 spot  102310 352050 place  1024 facing  0 way  end-door
 25 door  29 in-area  0 room  14 spot  101070 350850 place  3072 facing  1 way  5 room  17 spot  101070 350850 place  3072 facing  0 way  end-door
 26 door  2 in-area  1 room  9 spot  105090 358034 place  2048 facing  1 way  2 room  11 spot  105090 356842 place  2048 facing  0 way  end-door
 27 door  3 in-area  0 room  2 spot  175874 423434 place  2048 facing  1 way  -2 room  12 spot  175874 423426 place  2048 facing  0 way  end-door
 28 door  3 in-area  2 room  0 spot  179074 425042 place  2048 facing  1 way  1 room  1 spot  179074 425062 place  2048 facing  0 way  end-door
 29 door  3 in-area  3 room  3 spot  178274 423854 place  0 facing  1 way  1 room  4 spot  178274 423834 place  0 facing  0 way  end-door
 30 door  4 in-area  -2 room  29 spot  389402 416014 place  2048 facing  1 way  0 room  29 spot  109650 207170 place  0 facing  0 way  end-door
 31 door  5 in-area  -2 room  5 spot  108290 57554 place  2048 facing  0 way  end-door
 32 door  5 in-area  -2 room  4 spot  112698 51118 place  3072 facing  0 way  0 room  0 spot  112630 51510 place  1024 facing  1 way  end-door
 33 door  5 in-area  0 room  1 spot  115490 54330 place  0 facing  1 way  1 room  3 spot  115490 54330 place  0 facing  0 way  end-door
 34 door  5 in-area  0 room  2 spot  116290 54330 place  2048 facing  0 way  1 room  4 spot  116290 54330 place  2048 facing  1 way  end-door
 35 door  5 in-area  1 room  5 spot  112890 60734 place  2048 facing  1 way  2 room  6 spot  112890 60734 place  2048 facing  0 way  end-door
 36 door  6 in-area  0 room  0 spot  89710 47930 place  2048 facing  1 way  -2 room  1 spot  89710 47950 place  2048 facing  0 way  end-door
 37 door  6 in-area  0 room  2 spot  89842 49990 place  0 facing  1 way  -2 room  2 spot  92410 52750 place  1024 facing  1 way  end-door
 38 door  8 in-area  -2 room  6 spot  106818 198754 place  2560 facing  1 way  end-door
 39 door  8 in-area  -2 room  7 spot  106254 199318 place  512 facing  0 way  end-door
 40 door  9 in-area  0 room  0 spot  333364 437284 place  1536 facing  1 way  -2 room  44 spot  333350 437306 place  1536 facing  0 way  end-door
 41 door  9 in-area  2 room  6 spot  333800 439716 place  2560 facing  1 way  end-door
 42 door  9 in-area  0 room  3 spot  333788 439704 place  512 facing  1 way  end-door
 43 door  9 in-area  0 room  1 spot  332212 437280 place  512 facing  0 way  1 room  2 spot  332212 437280 place  512 facing  1 way  end-door
 44 door  10 in-area  0 room  0 spot  470930 466750 place  2048 facing  0 way  1 room  1 spot  470930 466750 place  2048 facing  1 way  end-door
 45 door  11 in-area  -2 room  35 spot  467126 411990 place  2048 facing  1 way  0 room  0 spot  467130 412010 place  2048 facing  0 way  end-door
 46 door  11 in-area  0 room  1 spot  466950 409790 place  3072 facing  0 way  1 room  2 spot  466950 409790 place  3072 facing  1 way  end-door
 47 door  11 in-area  -2 room  34 spot  468726 411990 place  2048 facing  1 way  end-door
 48 door  12 in-area  0 room  0 spot  468530 292530 place  2048 facing  0 way  -2 room  32 spot  468526 292514 place  2048 facing  1 way  end-door
 49 door  12 in-area  0 room  2 spot  469750 291310 place  1024 facing  1 way  1 room  3 spot  469750 291310 place  1024 facing  0 way  end-door
 50 door  12 in-area  0 room  1 spot  466530 288890 place  0 facing  1 way  end-door
 51 door  12 in-area  1 room  4 spot  470570 291742 place  2048 facing  0 way  2 room  5 spot  470570 291742 place  2048 facing  1 way  end-door
 52 door  12 in-area  0 room  1 spot  466530 288890 place  0 facing  0 way  end-door
 53 door  12 in-area  -2 room  33 spot  464530 288502 place  1024 facing  1 way  end-door
 54 door  14 in-area  -2 room  19 spot  337070 65126 place  3072 facing  1 way  end-door
 55 door  15 in-area  -2 room  20 spot  328270 70130 place  1024 facing  1 way  end-door
 56 door  16 in-area  0 room  0 spot  381186 140210 place  3072 facing  1 way  -2 room  28 spot  381202 140210 place  3072 facing  0 way  end-door
 57 door  16 in-area  0 room  1 spot  381190 141090 place  3072 facing  1 way  -2 room  26 spot  381202 141090 place  3072 facing  0 way  end-door
 58 door  16 in-area  0 room  2 spot  383230 141450 place  1024 facing  1 way  1 room  7 spot  383190 141450 place  1024 facing  0 way  end-door
 59 door  16 in-area  0 room  8 spot  383218 149050 place  1024 facing  1 way  2 room  9 spot  383218 149050 place  1024 facing  0 way  end-door
 60 door  16 in-area  0 room  12 spot  381190 151450 place  1024 facing  0 way  3 room  10 spot  381230 151450 place  1024 facing  1 way  end-door
 61 door  16 in-area  3 room  14 spot  379590 150650 place  3072 facing  1 way  end-door
 62 door  17 in-area  0 room  2 spot  379646 151446 place  1024 facing  1 way  end-door
 63 door  17 in-area  2 room  0 spot  363630 146630 place  2048 facing  1 way  3 room  1 spot  363630 146630 place  2048 facing  0 way  end-door
 64 door  17 in-area  2 room  4 spot  364430 146646 place  0 facing  1 way  end-door
 65 door  17 in-area  2 room  5 spot  364430 146646 place  0 facing  1 way  end-door
 66 door  17 in-area  0 room  3 spot  379646 150646 place  3072 facing  0 way  end-door
 67 door  18 in-area  0 room  0 spot  411970 170030 place  1024 facing  0 way  -2 room  30 spot  411990 170030 place  1024 facing  1 way  end-door
 68 door  18 in-area  0 room  2 spot  413930 168610 place  2048 facing  1 way  1 room  1 spot  413930 168650 place  2048 facing  0 way  end-door
 69 door  18 in-area  0 room  3 spot  413290 167810 place  2048 facing  1 way  end-door
 70 door  18 in-area  0 room  3 spot  413290 167810 place  2048 facing  0 way  end-door
 71 door  31 in-area  0 room  1 spot  306110 304490 place  3072 facing  1 way  -2 room  18 spot  306090 304482 place  3072 facing  0 way  end-door
 72 door  31 in-area  0 room  2 spot  310930 304470 place  0 facing  0 way  1 room  3 spot  310930 304470 place  0 facing  1 way  end-door
 73 door  19 in-area  1 room  0 spot  310930 304470 place  0 facing  0 way  0 room  12 spot  310930 304470 place  0 facing  1 way  end-door
 74 door  19 in-area  1 room  1 spot  306930 305310 place  0 facing  1 way  4 room  4 spot  306930 305310 place  0 facing  0 way  end-door
 75 door  19 in-area  1 room  2 spot  314930 304470 place  2048 facing  1 way  2 room  5 spot  314930 304470 place  2048 facing  0 way  end-door
 76 door  19 in-area  1 room  3 spot  310130 305310 place  0 facing  1 way  6 room  6 spot  310130 305310 place  0 facing  0 way  end-door
 77 door  31 in-area  1 room  5 spot  311330 301782 place  2048 facing  1 way  -2 room  17 spot  311330 301770 place  2048 facing  0 way  end-door
 78 door  19 in-area  4 room  9 spot  308150 306130 place  1024 facing  1 way  5 room  10 spot  308150 306130 place  1024 facing  0 way  end-door
 79 door  19 in-area  2 room  8 spot  313710 303650 place  3072 facing  1 way  3 room  7 spot  313710 303650 place  3072 facing  0 way  end-door
 80 door  19 in-area  5 room  15 spot  308730 305754 place  0 facing  0 way  end-door
 81 door  31 in-area  0 room  0 spot  306110 305290 place  1024 facing  0 way  -2 room  16 spot  306090 305282 place  1024 facing  1 way  end-door
 82 door  21 in-area  0 room  0 spot  329690 269130 place  3072 facing  0 way  -2 room  21 spot  329690 269126 place  3072 facing  1 way  end-door
 83 door  21 in-area  0 room  1 spot  327390 268138 place  0 facing  0 way  end-door
 84 door  21 in-area  0 room  2 spot  327250 267730 place  1024 facing  0 way  1 room  3 spot  327290 267730 place  1024 facing  1 way  end-door
 85 door  21 in-area  0 room  1 spot  327390 268138 place  0 facing  1 way  end-door
 86 door  23 in-area  0 room  1 spot  38870 277330 place  1024 facing  1 way  -2 room  0 spot  38930 277330 place  1024 facing  0 way  end-door
 87 door  23 in-area  1 room  4 spot  38870 276530 place  1024 facing  1 way  2 room  5 spot  38870 276530 place  1024 facing  0 way  end-door
 88 door  23 in-area  1 room  3 spot  37250 275914 place  0 facing  0 way  3 room  0 spot  37250 275914 place  0 facing  1 way  end-door
 89 door  23 in-area  2 room  2 spot  38878 275330 place  3072 facing  1 way  3 room  7 spot  38878 275330 place  3072 facing  0 way  end-door
 90 door  24 in-area  0 room  0 spot  590990 131870 place  3072 facing  1 way  -2 room  41 spot  591010 131870 place  3072 facing  0 way  end-door
 91 door  24 in-area  0 room  6 spot  590990 132670 place  1024 facing  0 way  -2 room  43 spot  591010 132670 place  1024 facing  1 way  end-door
 92 door  24 in-area  0 room  1 spot  594010 130890 place  2048 facing  1 way  1 room  2 spot  594010 130890 place  2048 facing  0 way  end-door
 93 door  24 in-area  1 room  3 spot  591990 130670 place  1024 facing  0 way  -2 room  42 spot  592014 130670 place  1024 facing  1 way  end-door
 94 door  24 in-area  1 room  4 spot  593610 130050 place  2048 facing  1 way  2 room  7 spot  593610 130050 place  2048 facing  0 way  end-door
 95 door  24 in-area  1 room  5 spot  596806 130890 place  2048 facing  0 way  6 room  10 spot  596806 130890 place  2048 facing  1 way  end-door
 96 door  24 in-area  2 room  8 spot  593810 128430 place  1024 facing  1 way  3 room  9 spot  593810 128430 place  1024 facing  0 way  end-door
 97 door  24 in-area  4 room  11 spot  593810 130870 place  0 facing  1 way  5 room  12 spot  593810 130870 place  0 facing  0 way  end-door
 98 door  24 in-area  6 room  13 spot  596610 132630 place  0 facing  1 way  5 room  14 spot  596610 132630 place  0 facing  0 way  end-door
 99 door  25 in-area  -2 room  31 spot  468722 178070 place  1024 facing  1 way  end-door
100 door  26 in-area  0 room  1 spot  36170 408170 place  0 facing  0 way  7 room  0 spot  36170 408210 place  0 facing  1 way  end-door
101 door  27 in-area  -2 room  13 spot  222370 407466 place  2048 facing  1 way  end-door
102 door  27 in-area  -2 room  22 spot  353250 254954 place  2048 facing  1 way  end-door
103 door  27 in-area  -2 room  23 spot  352210 254954 place  0 facing  0 way  end-door
104 door  28 in-area  1 room  0 spot  575830 223450 place  0 facing  0 way  -2 room  40 spot  576230 223450 place  2048 facing  1 way  end-door
105 door  28 in-area  1 room  2 spot  576450 222630 place  3072 facing  1 way  end-door
106 door  28 in-area  0 room  3 spot  575630 222198 place  2048 facing  0 way  1 room  1 spot  575630 222198 place  2048 facing  1 way  end-door
107 door  28 in-area  -2 room  39 spot  574030 220570 place  0 facing  1 way  end-door
