\ schedules.fs - the townsfolk's days (docs/subsystems/townsfolk.md): converted once from the
\ executable's tables by tools/schedules2forth.py; edited by hand from here on. A person's rows: until when
\ (day hour minute) they stay at a stop, where it is - area floor, x z, heading (4096 a turn), the motion they
\ hold there - or how they are placed (with a partner, at the diner's counter, at Club Wolves), the route
\ they walk on leaving it.
IN: events.schedules
USING: townsfolk.rows ;

\ ---- slot 1: morgan ----
1 schedule
   0 stop  25 10 52 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80087EC8 route  end-stop
   1 stop  25 11 30 until  1 record  0 1 in-area  251837 131902 place  1780 facing  0 pose  3 car-at  $80087EF0 route  end-stop
   2 stop  25 11 51 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80087F74 route  end-stop
   3 stop  25 13 40 until  2 record  24 1 in-area  594409 132528 place  3058 facing  72 pose  2 car-at  $80087FA0 route  end-stop
   4 stop  25 15 25 until  3 record  26 1 in-area  54742 416364 place  2884 facing  0 pose  12 car-at  $80087FF0 route  end-stop
   5 stop  25 16 41 until  4 record  26 1 in-area  36250 407645 place  0 facing  0 pose  12 car-at  $80088010 route  end-stop
   6 stop  25 17 15 until  5 record  23 1 in-area  45871 277889 place  0 facing  0 pose  14 car-at  $80088048 route  end-stop
   7 stop  25 19 40 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088080 route  end-stop
   8 stop  25 20 40 until  6 record  27 1 in-area  349281 286027 place  0 facing  0 pose  19 car-at  $800880D0 route  end-stop
   9 stop  25 22 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  21 car-at  $80088118 route  end-stop
  10 stop  26 5 15 until  7 record  27 1 in-area  470012 177908 place  0 facing  0 pose  25 car-at  $80088148 route  end-stop
  11 stop  26 8 5 until  8 record  26 1 in-area  44461 415308 place  4053 facing  0 pose  26 car-at  $80088180 route  end-stop
  12 stop  26 10 2 until  4 record  26 1 in-area  36250 407645 place  0 facing  0 pose  26 car-at  $80088198 route  end-stop
  13 stop  26 11 52 until  1 record  0 1 in-area  251837 131902 place  1780 facing  0 pose  49 car-at  $80087EF0 route  end-stop
  14 stop  26 12 40 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  49 car-at  $800881E0 route  end-stop
  15 stop  26 13 30 until  9 record  27 1 in-area  1 1 place  1 facing  0 pose  50 car-at  $800882E8 route  end-stop
  16 stop  26 14 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  21 car-at  $80088400 route  end-stop
  17 stop  26 16 0 until  10 record  27 1 in-area  177133 423615 place  1242 facing  0 pose  44 car-at  $80088474 route  end-stop
  18 stop  26 17 30 until  11 record  23 360 with-partner  $8008848C route  end-stop
  19 stop  26 22 0 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  21 car-at  $80088118 route  end-stop
  20 stop  27 7 0 until  7 record  27 1 in-area  470012 177908 place  0 facing  0 pose  56 car-at  $800884B0 route  end-stop
  21 stop  27 10 40 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  21 car-at  $800884E0 route  end-stop
  22 stop  27 12 30 until  12 record  21 1 in-area  328400 271130 place  3072 facing  119 pose  109 car-at  $800883C8 route  end-stop
  23 stop  27 13 50 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  21 car-at  $80088518 route  end-stop
  24 stop  27 16 0 until  13 record  27 1 in-area  92633 47526 place  4080 facing  0 pose  48 car-at  $80088318 route  end-stop
  25 stop  27 18 40 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088348 route  end-stop
  26 stop  27 19 40 until  14 record  diner-seat  64 car-at  $80088388 route  end-stop
  27 stop  27 21 40 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088118 route  end-stop
  28 stop  28 6 0 until  7 record  27 1 in-area  470012 177908 place  0 facing  0 pose  22 car-at  $80088578 route  end-stop
  29 stop  28 8 0 until  14 record  diner-seat  64 car-at  $80088388 route  end-stop
  30 stop  28 9 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $800885D0 route  end-stop
  31 stop  28 11 30 until  15 record  27 1 in-area  576718 214646 place  2160 facing  81 pose  28 car-at  $80088620 route  end-stop
  32 stop  28 14 20 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088670 route  end-stop
  33 stop  28 17 20 until  16 record  27 1 in-area  93839 46672 place  3402 facing  81 pose  48 car-at  $800886FC route  end-stop
  34 stop  28 19 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $8008872C route  end-stop
  35 stop  28 21 30 until  17 record  club-seat  164 car-at  $80088774 route  end-stop
  36 stop  28 23 10 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088118 route  end-stop
  37 stop  29 6 0 until  7 record  27 1 in-area  470012 177908 place  0 facing  0 pose  56 car-at  $80088578 route  end-stop
  38 stop  29 8 0 until  14 record  diner-seat  64 car-at  $80088388 route  end-stop
  39 stop  29 9 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $800885D0 route  end-stop
  40 stop  29 11 30 until  15 record  27 1 in-area  576718 214646 place  2160 facing  81 pose  28 car-at  $80088620 route  end-stop
  41 stop  29 14 20 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088670 route  end-stop
  42 stop  29 17 20 until  16 record  27 1 in-area  93839 46672 place  3402 facing  81 pose  48 car-at  $800886FC route  end-stop
  43 stop  29 19 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $8008872C route  end-stop
  44 stop  29 21 30 until  17 record  club-seat  164 car-at  $80088774 route  end-stop
  45 stop  29 23 10 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088118 route  end-stop
  46 stop  30 6 0 until  7 record  27 1 in-area  470012 177908 place  0 facing  0 pose  56 car-at  $80088578 route  end-stop
  47 stop  30 8 0 until  14 record  diner-seat  64 car-at  $80088388 route  end-stop
  48 stop  30 9 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $800885D0 route  end-stop
  49 stop  30 11 30 until  15 record  27 1 in-area  576718 214646 place  2160 facing  81 pose  28 car-at  $80088620 route  end-stop
  50 stop  30 14 20 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088670 route  end-stop
  51 stop  30 17 20 until  16 record  27 1 in-area  93839 46672 place  3402 facing  81 pose  48 car-at  $800886FC route  end-stop
  52 stop  30 19 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $8008872C route  end-stop
  53 stop  30 21 30 until  17 record  club-seat  164 car-at  $80088774 route  end-stop
  54 stop  30 23 10 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  3 car-at  $80088118 route  end-stop
  55 stop  31 5 20 until  7 record  27 1 in-area  470012 177908 place  0 facing  0 pose  56 car-at  $80088B04 route  end-stop
  56 stop  31 8 0 until  10 record  27 1 in-area  177133 423615 place  1242 facing  0 pose  5 car-at  $800887AC route  end-stop
  57 stop  31 9 30 until  18 record  26 1 in-area  44431 415766 place  0 facing  0 pose  12 car-at  $800888EC route  end-stop
  58 stop  31 11 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  108 car-at  $8008880C route  end-stop
  59 stop  31 14 0 until  19 record  29 2 in-area  100559 359425 place  2982 facing  0 pose  4 car-at  $80088834 route  end-stop
  60 stop  31 16 10 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  108 car-at  $80088670 route  end-stop
  61 stop  31 17 20 until  16 record  27 1 in-area  93839 46672 place  3402 facing  81 pose  161 car-at  $8008887C route  end-stop
  62 stop  31 19 0 until  20 record  27 1 in-area  464072 289060 place  2030 facing  22 pose  63 car-at  $800888B4 route  end-stop
  63 stop  31 22 30 until  0 record  0 1 in-area  248376 131107 place  0 facing  315 pose  108 car-at  $80088924 route  end-stop
end-schedule

\ ---- slot 2: nozz ----
2 schedule
   0 stop  25 11 40 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  7 car-at  $80081ED4 route  end-stop
   1 stop  25 13 42 until  1 record  24 1 in-area  593712 132681 place  1152 facing  72 pose  9 car-at  $80081F0C route  end-stop
   2 stop  25 14 45 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  170 car-at  $80081F38 route  end-stop
   3 stop  25 16 25 until  2 record  26 1 in-area  36250 407645 place  0 facing  0 pose  13 car-at  $80081FB4 route  end-stop
   4 stop  25 18 20 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $80081FF4 route  end-stop
   5 stop  25 20 30 until  3 record  0 1 in-area  249912 134752 place  792 facing  0 pose  16 car-at  $80082028 route  end-stop
   6 stop  26 5 30 until  4 record  19 2 in-area  314484 302479 place  990 facing  68 pose  16 car-at  $8008205C route  end-stop
   7 stop  26 8 5 until  5 record  26 1 in-area  44431 415766 place  2005 facing  0 pose  13 car-at  $80082094 route  end-stop
   8 stop  26 10 0 until  6 record  26 1 in-area  36573 407872 place  0 facing  0 pose  13 car-at  $800820AC route  end-stop
   9 stop  26 11 35 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $800820EC route  end-stop
  10 stop  26 12 25 until  7 record  1 1 in-area  349278 285691 place  0 facing  0 pose  16 car-at  $80082128 route  end-stop
  11 stop  26 13 35 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $80082160 route  end-stop
  12 stop  26 14 50 until  3 record  0 1 in-area  249912 134752 place  792 facing  0 pose  54 car-at  $80082198 route  end-stop
  13 stop  26 16 20 until  8 record  23 1 in-area  37552 277067 place  0 facing  0 pose  11 car-at  $800821C8 route  end-stop
  14 stop  26 17 20 until  5 record  26 1 in-area  44431 415766 place  2005 facing  0 pose  13 car-at  $800820AC route  end-stop
  15 stop  26 18 50 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $80081FF4 route  end-stop
  16 stop  26 21 10 until  3 record  0 1 in-area  249912 134752 place  792 facing  0 pose  16 car-at  $80082028 route  end-stop
  17 stop  27 6 15 until  4 record  19 2 in-area  314484 302479 place  990 facing  68 pose  16 car-at  $800821F8 route  end-stop
  18 stop  27 11 30 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $800820EC route  end-stop
  19 stop  27 12 25 until  7 record  1 1 in-area  349278 285691 place  0 facing  0 pose  16 car-at  $80082128 route  end-stop
  20 stop  27 17 45 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $80081FF4 route  end-stop
  21 stop  27 20 0 until  3 record  0 1 in-area  249912 134752 place  792 facing  0 pose  16 car-at  $80082028 route  end-stop
  22 stop  28 6 15 until  4 record  19 2 in-area  314484 302479 place  990 facing  68 pose  16 car-at  $800821F8 route  end-stop
  23 stop  28 11 20 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $800820EC route  end-stop
  24 stop  28 12 20 until  7 record  1 1 in-area  349278 285691 place  0 facing  0 pose  16 car-at  $80082128 route  end-stop
  25 stop  28 17 30 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $80081FF4 route  end-stop
  26 stop  28 20 10 until  3 record  0 1 in-area  249912 134752 place  792 facing  0 pose  16 car-at  $80082028 route  end-stop
  27 stop  29 6 15 until  4 record  19 2 in-area  314484 302479 place  990 facing  68 pose  16 car-at  $800821F8 route  end-stop
  28 stop  29 11 20 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $800820EC route  end-stop
  29 stop  29 12 20 until  7 record  1 1 in-area  349278 285691 place  0 facing  0 pose  16 car-at  $80082128 route  end-stop
  30 stop  29 18 30 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $8008222C route  end-stop
  31 stop  30 6 15 until  4 record  19 2 in-area  314484 302479 place  990 facing  68 pose  16 car-at  $800821F8 route  end-stop
  32 stop  30 11 20 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $800820EC route  end-stop
  33 stop  30 12 20 until  7 record  1 1 in-area  349278 285691 place  0 facing  0 pose  16 car-at  $80082128 route  end-stop
  34 stop  30 18 30 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $8008222C route  end-stop
  35 stop  31 6 15 until  4 record  19 2 in-area  314484 302479 place  990 facing  68 pose  16 car-at  $800821F8 route  end-stop
  36 stop  31 11 20 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $800820EC route  end-stop
  37 stop  31 12 20 until  7 record  1 1 in-area  349278 285691 place  0 facing  0 pose  16 car-at  $80082128 route  end-stop
  38 stop  31 18 30 until  0 record  1 1 in-area  346344 285214 place  3176 facing  0 pose  16 car-at  $8008222C route  end-stop
end-schedule

\ ---- slot 3: cohen ----
3 schedule
   0 stop  25 11 50 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  1 car-at  $80082854 route  end-stop
   1 stop  25 13 50 until  1 record  24 1 in-area  593712 132917 place  1216 facing  72 pose  2 car-at  $8008290C route  end-stop
   2 stop  25 15 10 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $800828AC route  end-stop
   3 stop  25 16 40 until  2 record  26 1 in-area  36250 407645 place  1216 facing  0 pose  12 car-at  $80082964 route  end-stop
   4 stop  25 19 40 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $800828AC route  end-stop
   5 stop  25 21 40 until  2 record  26 1 in-area  36250 407645 place  1216 facing  0 pose  12 car-at  $80082964 route  end-stop
   6 stop  26 6 40 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $800828E4 route  end-stop
   7 stop  26 8 5 until  3 record  26 1 in-area  33427 413212 place  0 facing  0 pose  12 car-at  $800829BC route  end-stop
   8 stop  26 10 0 until  4 record  26 1 in-area  36573 407872 place  0 facing  0 pose  12 car-at  $800829D4 route  end-stop
   9 stop  26 11 40 until  5 record  2 1 in-area  1 1 place  0 facing  0 pose  45 car-at  $800829DC route  end-stop
  10 stop  26 13 10 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  58 car-at  $800828AC route  end-stop
  11 stop  26 14 50 until  2 record  26 1 in-area  36250 407645 place  1216 facing  0 pose  12 car-at  $80082A04 route  end-stop
  12 stop  26 17 0 until  6 record  16 1 in-area  388153 146937 place  2078 facing  0 pose  59 car-at  $80082A3C route  end-stop
  13 stop  26 19 45 until  4 record  26 1 in-area  36573 407872 place  0 facing  0 pose  12 car-at  $80082964 route  end-stop
  14 stop  27 6 30 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  58 car-at  $800828AC route  end-stop
  15 stop  27 7 5 until  2 record  26 1 in-area  36250 407645 place  1216 facing  0 pose  12 car-at  $80082CB8 route  end-stop
  16 stop  27 10 0 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $80082CF8 route  end-stop
  17 stop  27 11 30 until  7 record  0 1 in-area  250443 135045 place  951 facing  0 pose  54 car-at  $80082D30 route  end-stop
  18 stop  27 13 0 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  58 car-at  $800828AC route  end-stop
  19 stop  27 13 35 until  2 record  26 1 in-area  36250 407645 place  1216 facing  0 pose  12 car-at  $80082CB8 route  end-stop
  20 stop  27 17 30 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  58 car-at  $800828AC route  end-stop
  21 stop  27 18 5 until  2 record  26 1 in-area  36250 407645 place  1216 facing  0 pose  12 car-at  $80082CB8 route  end-stop
  22 stop  28 5 40 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  58 car-at  $80082D60 route  end-stop
  23 stop  28 7 20 until  8 record  26 1 in-area  32027 426272 place  172 facing  0 pose  12 car-at  $80082DF0 route  end-stop
  24 stop  28 9 30 until  9 record  diner-seat  109 car-at  $80082E50 route  end-stop
  25 stop  28 10 45 until  6 record  16 1 in-area  388153 146937 place  2078 facing  0 pose  158 car-at  $80082EC0 route  end-stop
  26 stop  28 13 30 until  10 record  0 1 in-area  248354 131757 place  2014 facing  22 pose  108 car-at  $80082F10 route  end-stop
  27 stop  28 16 10 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $80082F40 route  end-stop
  28 stop  28 18 10 until  11 record  26 1 in-area  23231 411558 place  3560 facing  81 pose  12 car-at  $80082F88 route  end-stop
  29 stop  28 20 20 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $80082D60 route  end-stop
  30 stop  28 22 10 until  8 record  26 1 in-area  32027 426272 place  172 facing  0 pose  12 car-at  $80082E28 route  end-stop
  31 stop  29 6 30 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  58 car-at  $80082D60 route  end-stop
  32 stop  29 8 50 until  8 record  26 1 in-area  32027 426272 place  172 facing  0 pose  12 car-at  $80082FD0 route  end-stop
  33 stop  29 11 50 until  6 record  16 1 in-area  388153 146937 place  2078 facing  0 pose  158 car-at  $80082EC0 route  end-stop
  34 stop  29 16 0 until  10 record  0 1 in-area  248354 131757 place  2014 facing  22 pose  108 car-at  $80082F10 route  end-stop
  35 stop  30 6 30 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $80082D60 route  end-stop
  36 stop  30 8 50 until  8 record  26 1 in-area  32027 426272 place  172 facing  0 pose  12 car-at  $80082FD0 route  end-stop
  37 stop  30 11 50 until  6 record  16 1 in-area  388153 146937 place  2078 facing  0 pose  158 car-at  $80082EC0 route  end-stop
  38 stop  30 16 0 until  10 record  0 1 in-area  248354 131757 place  2014 facing  22 pose  108 car-at  $80082F10 route  end-stop
  39 stop  31 6 30 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $80082D60 route  end-stop
  40 stop  31 8 50 until  8 record  26 1 in-area  32027 426272 place  172 facing  0 pose  12 car-at  $80082FD0 route  end-stop
  41 stop  31 11 50 until  6 record  16 1 in-area  388153 146937 place  2078 facing  0 pose  158 car-at  $80082EC0 route  end-stop
  42 stop  31 16 30 until  10 record  0 1 in-area  248354 131757 place  2014 facing  22 pose  108 car-at  $80083000 route  end-stop
  43 stop  31 19 30 until  12 record  2 1 in-area  103832 359085 place  3854 facing  0 pose  4 car-at  $80083040 route  end-stop
  44 stop  31 22 30 until  0 record  23 1 in-area  37639 278098 place  2801 facing  68 pose  11 car-at  $80083078 route  end-stop
end-schedule

\ ---- slot 5: samuel ----
5 schedule
   0 stop  25 10 52 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090674 route  end-stop
   1 stop  25 11 20 until  1 record  29 2 in-area  100173 356954 place  3049 facing  0 pose  4 car-at  186 variant  $800906DC route  end-stop
   2 stop  25 14 0 until  2 record  24 1 in-area  593183 132896 place  1041 facing  72 pose  36 car-at  186 variant  $80090764 route  end-stop
   3 stop  25 16 22 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800907A8 route  end-stop
   4 stop  25 18 0 until  4 record  2 1 in-area  102218 359401 place  631 facing  89 pose  4 car-at  $800907C8 route  end-stop
   5 stop  25 19 20 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090808 route  end-stop
   6 stop  26 7 20 until  5 record  27 1 in-area  222205 406064 place  0 facing  0 pose  115 car-at  186 variant  $8009087C route  end-stop
   7 stop  26 8 45 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $800908D8 route  end-stop
   8 stop  26 11 45 until  6 record  2 1 in-area  104199 360139 place  2771 facing  22 pose  4 car-at  $80090908 route  end-stop
   9 stop  26 12 50 until  7 record  29 2 in-area  100331 357638 place  2118 facing  22 pose  4 car-at  $80090950 route  end-stop
  10 stop  26 14 15 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090978 route  end-stop
  11 stop  26 16 20 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800907A8 route  end-stop
  12 stop  26 18 20 until  4 record  2 1 in-area  102218 359401 place  631 facing  89 pose  4 car-at  $800909A0 route  end-stop
  13 stop  26 19 45 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800909C0 route  end-stop
  14 stop  26 20 50 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090808 route  end-stop
  15 stop  27 7 20 until  5 record  27 1 in-area  222205 406064 place  0 facing  0 pose  115 car-at  186 variant  $8009087C route  end-stop
  16 stop  27 8 45 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $800908D8 route  end-stop
  17 stop  27 11 45 until  6 record  2 1 in-area  104199 360139 place  2771 facing  22 pose  4 car-at  $80090908 route  end-stop
  18 stop  27 12 50 until  7 record  29 2 in-area  100331 357638 place  2118 facing  22 pose  4 car-at  $80090950 route  end-stop
  19 stop  27 14 15 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090978 route  end-stop
  20 stop  27 16 20 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800907A8 route  end-stop
  21 stop  27 18 20 until  4 record  2 1 in-area  102218 359401 place  631 facing  89 pose  4 car-at  $800909A0 route  end-stop
  22 stop  27 19 45 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800909C0 route  end-stop
  23 stop  27 20 50 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090808 route  end-stop
  24 stop  28 7 20 until  5 record  27 1 in-area  222205 406064 place  0 facing  0 pose  115 car-at  186 variant  $8009087C route  end-stop
  25 stop  28 8 45 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $800908D8 route  end-stop
  26 stop  28 11 45 until  6 record  2 1 in-area  104199 360139 place  2771 facing  22 pose  4 car-at  $80090908 route  end-stop
  27 stop  28 12 50 until  7 record  29 2 in-area  100331 357638 place  2118 facing  22 pose  4 car-at  $80090950 route  end-stop
  28 stop  28 14 15 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090978 route  end-stop
  29 stop  28 16 20 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800907A8 route  end-stop
  30 stop  28 18 20 until  4 record  2 1 in-area  102218 359401 place  631 facing  89 pose  4 car-at  $800909A0 route  end-stop
  31 stop  28 19 45 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800909C0 route  end-stop
  32 stop  28 20 50 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090808 route  end-stop
  33 stop  29 7 20 until  5 record  27 1 in-area  222205 406064 place  0 facing  0 pose  115 car-at  186 variant  $8009087C route  end-stop
  34 stop  29 8 45 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $800908D8 route  end-stop
  35 stop  29 11 45 until  6 record  2 1 in-area  104199 360139 place  2771 facing  22 pose  4 car-at  $80090908 route  end-stop
  36 stop  29 12 50 until  7 record  29 2 in-area  100331 357638 place  2118 facing  22 pose  4 car-at  $80090950 route  end-stop
  37 stop  29 14 15 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090978 route  end-stop
  38 stop  29 16 20 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800907A8 route  end-stop
  39 stop  29 18 20 until  4 record  2 1 in-area  102218 359401 place  631 facing  89 pose  4 car-at  $800909A0 route  end-stop
  40 stop  29 19 45 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800909C0 route  end-stop
  41 stop  29 20 50 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090808 route  end-stop
  42 stop  30 7 20 until  5 record  27 1 in-area  222205 406064 place  0 facing  0 pose  115 car-at  186 variant  $8009087C route  end-stop
  43 stop  30 8 45 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $800908D8 route  end-stop
  44 stop  30 11 45 until  6 record  2 1 in-area  104199 360139 place  2771 facing  22 pose  4 car-at  $80090908 route  end-stop
  45 stop  30 12 50 until  7 record  29 2 in-area  100331 357638 place  2118 facing  22 pose  4 car-at  $80090950 route  end-stop
  46 stop  30 14 15 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090978 route  end-stop
  47 stop  30 16 20 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800907A8 route  end-stop
  48 stop  30 18 20 until  4 record  2 1 in-area  102218 359401 place  631 facing  89 pose  4 car-at  $800909A0 route  end-stop
  49 stop  30 19 45 until  3 record  2 1 in-area  98623 358527 place  1457 facing  72 pose  4 car-at  $800909C0 route  end-stop
  50 stop  30 20 50 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090808 route  end-stop
  51 stop  31 7 20 until  5 record  27 1 in-area  222205 406064 place  0 facing  0 pose  115 car-at  186 variant  $8009087C route  end-stop
  52 stop  31 8 45 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $800908D8 route  end-stop
  53 stop  31 10 10 until  6 record  2 1 in-area  104199 360139 place  2771 facing  22 pose  4 car-at  $800909F0 route  end-stop
  54 stop  31 12 15 until  8 record  29 2 in-area  101538 353119 place  2 facing  0 pose  4 car-at  $80090A18 route  end-stop
  55 stop  31 13 50 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090A38 route  end-stop
  56 stop  31 15 20 until  9 record  29 2 in-area  100068 355776 place  4024 facing  22 pose  4 car-at  $80090A60 route  end-stop
  57 stop  31 17 15 until  10 record  29 2 in-area  100422 350760 place  2868 facing  0 pose  4 car-at  $80090A90 route  end-stop
  58 stop  31 19 45 until  11 record  2 1 in-area  105089 359697 place  514 facing  22 pose  4 car-at  $80090AC0 route  end-stop
  59 stop  31 20 50 until  0 record  29 2 in-area  98713 359274 place  969 facing  72 pose  4 car-at  $80090808 route  end-stop
end-schedule

\ ---- slot 6: james ----
6 schedule
   0 stop  25 11 55 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
   1 stop  25 12 55 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  4 car-at  $800853D0 route  end-stop
   2 stop  25 14 25 until  2 record  29 2 in-area  99440 350139 place  3092 facing  84 pose  4 car-at  $80085408 route  end-stop
   3 stop  25 15 25 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
   4 stop  25 16 25 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  4 car-at  $800854A0 route  end-stop
   5 stop  25 18 30 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $800854D0 route  end-stop
   6 stop  25 19 0 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800854F0 route  end-stop
   7 stop  25 22 50 until  4 record  12 1 in-area  467038 291003 place  1070 facing  72 pose  94 car-at  187 variant  $80085570 route  end-stop
   8 stop  26 6 30 until  5 record  27 1 in-area  389609 414992 place  0 facing  0 pose  132 car-at  187 variant  $800855C4 route  end-stop
   9 stop  26 7 45 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800855FC route  end-stop
  10 stop  26 8 50 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  11 stop  26 9 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  133 car-at  $800854A0 route  end-stop
  12 stop  26 12 0 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  133 car-at  $80085630 route  end-stop
  13 stop  26 13 30 until  6 record  16 1 in-area  388721 147763 place  4081 facing  57 pose  130 car-at  187 variant  $800856AC route  end-stop
  14 stop  26 15 20 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  15 stop  26 16 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  4 car-at  $800854A0 route  end-stop
  16 stop  26 18 30 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $800854D0 route  end-stop
  17 stop  26 19 0 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800854F0 route  end-stop
  18 stop  26 22 50 until  4 record  12 1 in-area  467038 291003 place  1070 facing  72 pose  94 car-at  187 variant  $80085570 route  end-stop
  19 stop  27 6 30 until  5 record  27 1 in-area  389609 414992 place  0 facing  0 pose  132 car-at  187 variant  $800855C4 route  end-stop
  20 stop  27 7 45 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800855FC route  end-stop
  21 stop  27 8 50 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  22 stop  27 9 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  133 car-at  $800854A0 route  end-stop
  23 stop  27 12 0 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  133 car-at  $80085630 route  end-stop
  24 stop  27 13 30 until  6 record  16 1 in-area  388721 147763 place  4081 facing  57 pose  130 car-at  187 variant  $800856AC route  end-stop
  25 stop  27 15 20 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  26 stop  27 16 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  4 car-at  $800854A0 route  end-stop
  27 stop  27 18 30 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $800854D0 route  end-stop
  28 stop  27 19 0 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800854F0 route  end-stop
  29 stop  27 22 50 until  4 record  12 1 in-area  467038 291003 place  1070 facing  72 pose  94 car-at  187 variant  $80085570 route  end-stop
  30 stop  28 6 30 until  5 record  27 1 in-area  389609 414992 place  0 facing  0 pose  132 car-at  187 variant  $800855C4 route  end-stop
  31 stop  28 7 45 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800855FC route  end-stop
  32 stop  28 8 50 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  33 stop  28 9 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  133 car-at  $800854A0 route  end-stop
  34 stop  28 12 0 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  133 car-at  $80085630 route  end-stop
  35 stop  28 13 30 until  6 record  16 1 in-area  388721 147763 place  4081 facing  57 pose  130 car-at  187 variant  $800856AC route  end-stop
  36 stop  28 15 20 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  37 stop  28 16 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  4 car-at  $800854A0 route  end-stop
  38 stop  28 18 30 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $800854D0 route  end-stop
  39 stop  28 19 0 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800854F0 route  end-stop
  40 stop  28 22 50 until  4 record  12 1 in-area  467038 291003 place  1070 facing  72 pose  94 car-at  187 variant  $80085570 route  end-stop
  41 stop  29 6 30 until  5 record  27 1 in-area  389609 414992 place  0 facing  0 pose  132 car-at  187 variant  $800855C4 route  end-stop
  42 stop  29 7 45 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800855FC route  end-stop
  43 stop  29 8 50 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  44 stop  29 9 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  133 car-at  $800854A0 route  end-stop
  45 stop  29 12 0 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  133 car-at  $80085630 route  end-stop
  46 stop  29 13 30 until  6 record  16 1 in-area  388721 147763 place  4081 facing  57 pose  130 car-at  187 variant  $800856AC route  end-stop
  47 stop  29 15 20 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  48 stop  29 16 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  4 car-at  $800854A0 route  end-stop
  49 stop  29 18 30 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $800854D0 route  end-stop
  50 stop  29 19 0 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800854F0 route  end-stop
  51 stop  29 22 50 until  4 record  12 1 in-area  467038 291003 place  1070 facing  72 pose  94 car-at  187 variant  $80085570 route  end-stop
  52 stop  30 6 30 until  5 record  27 1 in-area  389609 414992 place  0 facing  0 pose  132 car-at  187 variant  $800855C4 route  end-stop
  53 stop  30 7 45 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800855FC route  end-stop
  54 stop  30 8 50 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  55 stop  30 9 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  133 car-at  $800854A0 route  end-stop
  56 stop  30 12 0 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  133 car-at  $80085630 route  end-stop
  57 stop  30 13 30 until  6 record  16 1 in-area  388721 147763 place  4081 facing  57 pose  130 car-at  187 variant  $800856AC route  end-stop
  58 stop  30 15 20 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $80085388 route  end-stop
  59 stop  30 16 20 until  1 record  2 1 in-area  105061 360121 place  2083 facing  68 pose  4 car-at  $800854A0 route  end-stop
  60 stop  30 18 30 until  0 record  2 1 in-area  103766 352088 place  400 facing  72 pose  4 car-at  $800854D0 route  end-stop
  61 stop  30 19 0 until  3 record  29 2 in-area  99171 357585 place  31 facing  92 pose  4 car-at  187 variant  $800854F0 route  end-stop
  62 stop  30 22 50 until  4 record  12 1 in-area  467038 291003 place  1070 facing  72 pose  94 car-at  187 variant  $80085570 route  end-stop
  63 stop  31 6 50 until  5 record  27 1 in-area  389609 414992 place  0 facing  0 pose  132 car-at  187 variant  $80085714 route  end-stop
  64 stop  31 10 0 until  7 record  1 1 in-area  345547 285294 place  840 facing  0 pose  170 car-at  187 variant  $8008574C route  end-stop
  65 stop  31 14 0 until  8 record  26 1 in-area  44631 415966 place  0 facing  0 pose  12 car-at  187 variant  $8008578C route  end-stop
  66 stop  31 15 40 until  9 record  7 1 in-area  122882 312591 place  1062 facing  717 pose  84 car-at  187 variant  $800857D4 route  end-stop
  67 stop  31 18 0 until  10 record  24 1 in-area  593046 132263 place  1036 facing  0 pose  41 car-at  187 variant  $80085814 route  end-stop
  68 stop  31 20 30 until  11 record  5 1 in-area  1 1 place  1 facing  0 pose  161 car-at  187 variant  $8008585C route  end-stop
end-schedule

\ ---- slot 7: hudson ----
7 schedule
   0 stop  25 10 46 until  0 record  0 0 in-area  250194 133415 place  2048 facing  72 pose  3 car-at  $8008472C route  end-stop
   1 stop  25 11 31 until  1 record  0 1 in-area  251837 131902 place  1780 facing  0 pose  3 car-at  $800847F0 route  end-stop
   2 stop  25 13 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $80084818 route  end-stop
   3 stop  25 14 40 until  3 record  23 360 with-partner  $80084860 route  end-stop
   4 stop  25 17 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $80084818 route  end-stop
   5 stop  25 18 0 until  3 record  23 360 with-partner  $80084860 route  end-stop
   6 stop  25 19 30 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $80084818 route  end-stop
   7 stop  25 20 0 until  3 record  23 360 with-partner  $80084860 route  end-stop
   8 stop  25 22 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $800848CC route  end-stop
   9 stop  26 6 40 until  4 record  27 1 in-area  329037 69996 place  0 facing  0 pose  127 car-at  $800848FC route  end-stop
  10 stop  26 9 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $80084818 route  end-stop
  11 stop  26 11 0 until  3 record  23 360 with-partner  $80084860 route  end-stop
  12 stop  26 14 30 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $8008493C route  end-stop
  13 stop  26 16 0 until  5 record  27 1 in-area  175243 423088 place  1024 facing  0 pose  5 car-at  $80084964 route  end-stop
  14 stop  26 21 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $800848CC route  end-stop
  15 stop  27 6 40 until  4 record  27 1 in-area  329037 69996 place  0 facing  0 pose  127 car-at  $800848FC route  end-stop
  16 stop  27 9 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $8008483C route  end-stop
  17 stop  27 11 35 until  6 record  24 360 with-partner  $8008488C route  end-stop
  18 stop  27 12 30 until  7 record  21 1 in-area  329029 267632 place  3144 facing  72 pose  109 car-at  $800849F4 route  end-stop
  19 stop  27 14 15 until  8 record  27 1 in-area  596338 58699 place  3071 facing  0 pose  149 car-at  $80084A14 route  end-stop
  20 stop  27 16 0 until  9 record  diner-seat  109 car-at  $80084A54 route  end-stop
  21 stop  27 20 30 until  3 record  23 360 with-partner  $80084AA0 route  end-stop
  22 stop  28 6 40 until  4 record  27 1 in-area  329037 69996 place  0 facing  0 pose  127 car-at  $800848FC route  end-stop
  23 stop  28 9 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $8008483C route  end-stop
  24 stop  28 11 0 until  6 record  24 360 with-partner  $80084860 route  end-stop
  25 stop  28 12 30 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $80084AD4 route  end-stop
  26 stop  28 14 0 until  8 record  27 1 in-area  596338 58699 place  3071 facing  0 pose  149 car-at  $80084A14 route  end-stop
  27 stop  28 15 45 until  9 record  diner-seat  109 car-at  $80084A54 route  end-stop
  28 stop  28 20 30 until  3 record  23 360 with-partner  $80084AA0 route  end-stop
  29 stop  29 6 40 until  4 record  27 1 in-area  329037 69996 place  0 facing  0 pose  127 car-at  $800848FC route  end-stop
  30 stop  29 9 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $8008483C route  end-stop
  31 stop  29 11 0 until  6 record  24 360 with-partner  $80084860 route  end-stop
  32 stop  29 12 30 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $80084AD4 route  end-stop
  33 stop  29 14 0 until  8 record  27 1 in-area  596338 58699 place  3071 facing  0 pose  149 car-at  $80084A14 route  end-stop
  34 stop  29 15 45 until  9 record  diner-seat  109 car-at  $80084A54 route  end-stop
  35 stop  29 20 30 until  3 record  23 360 with-partner  $80084AA0 route  end-stop
  36 stop  30 6 40 until  4 record  27 1 in-area  329037 69996 place  0 facing  0 pose  127 car-at  $800848FC route  end-stop
  37 stop  30 9 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $8008483C route  end-stop
  38 stop  30 11 0 until  6 record  24 360 with-partner  $80084860 route  end-stop
  39 stop  30 12 30 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $80084AD4 route  end-stop
  40 stop  30 14 0 until  8 record  27 1 in-area  596338 58699 place  3071 facing  0 pose  149 car-at  $80084A14 route  end-stop
  41 stop  30 15 45 until  9 record  diner-seat  109 car-at  $80084A54 route  end-stop
  42 stop  30 20 30 until  3 record  23 360 with-partner  $80084AA0 route  end-stop
  43 stop  31 6 40 until  4 record  27 1 in-area  329037 69996 place  0 facing  0 pose  127 car-at  $800848FC route  end-stop
  44 stop  31 9 0 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $8008483C route  end-stop
  45 stop  31 11 0 until  6 record  24 360 with-partner  $80084860 route  end-stop
  46 stop  31 12 30 until  2 record  0 1 in-area  248552 132763 place  1024 facing  72 pose  3 car-at  $80084AD4 route  end-stop
  47 stop  31 14 0 until  8 record  27 1 in-area  596338 58699 place  3071 facing  0 pose  149 car-at  $80084A14 route  end-stop
  48 stop  31 15 45 until  9 record  diner-seat  109 car-at  $80084A54 route  end-stop
  49 stop  31 17 30 until  3 record  23 360 with-partner  $80084B04 route  end-stop
  50 stop  31 21 20 until  10 record  26 1 in-area  44000 415200 place  3072 facing  0 pose  26 car-at  $80084B58 route  end-stop
end-schedule

\ ---- slot 12: steve ----
12 schedule
   0 stop  25 11 47 until  0 record  3 1 in-area  178118 424040 place  1084 facing  0 pose  5 car-at  $80090E0C route  end-stop
   1 stop  25 16 0 until  1 record  24 1 in-area  593209 131335 place  976 facing  72 pose  41 car-at  $80090E68 route  end-stop
   2 stop  25 18 30 until  2 record  0 1 in-area  250382 134947 place  1325 facing  0 pose  40 car-at  $80090EA0 route  end-stop
   3 stop  26 9 15 until  0 record  3 1 in-area  178118 424040 place  1084 facing  0 pose  44 car-at  $80090EE0 route  end-stop
   4 stop  26 12 0 until  2 record  0 1 in-area  250382 134947 place  1325 facing  0 pose  42 car-at  $800910CC route  end-stop
   5 stop  26 13 55 until  3 record  18 1 in-area  412722 170916 place  3558 facing  22 pose  43 car-at  $80091104 route  end-stop
   6 stop  26 16 0 until  0 record  3 1 in-area  178118 424040 place  1084 facing  0 pose  44 car-at  $80090EE0 route  end-stop
   7 stop  26 17 30 until  2 record  0 1 in-area  250382 134947 place  1325 facing  0 pose  40 car-at  $80090EA0 route  end-stop
   8 stop  27 6 40 until  0 record  3 1 in-area  178118 424040 place  1084 facing  0 pose  44 car-at  $80090F20 route  end-stop
   9 stop  27 9 0 until  4 record  27 1 in-area  350028 260017 place  3036 facing  22 pose  73 car-at  $80090FB4 route  end-stop
  10 stop  27 11 0 until  5 record  diner-seat  109 car-at  $80091084 route  end-stop
  11 stop  27 13 20 until  2 record  0 1 in-area  250382 134947 place  1325 facing  0 pose  40 car-at  $800910CC route  end-stop
  12 stop  27 15 0 until  3 record  18 1 in-area  412722 170916 place  3558 facing  22 pose  43 car-at  $80091104 route  end-stop
  13 stop  27 19 50 until  0 record  3 1 in-area  178118 424040 place  1084 facing  0 pose  44 car-at  $8009113C route  end-stop
  14 stop  27 22 20 until  6 record  27 1 in-area  466975 293218 place  306 facing  22 pose  63 car-at  $80091174 route  end-stop
  15 stop  28 7 10 until  0 record  3 1 in-area  178118 424040 place  1084 facing  0 pose  44 car-at  $80090F20 route  end-stop
  16 stop  28 9 30 until  4 record  27 1 in-area  350028 260017 place  3036 facing  22 pose  73 car-at  $80090FB4 route  end-stop
  17 stop  28 11 0 until  5 record  diner-seat  109 car-at  $800911AC route  end-stop
  18 stop  28 14 30 until  1 record  24 1 in-area  593209 131335 place  976 facing  72 pose  41 car-at  $80091210 route  end-stop
  19 stop  29 7 10 until  0 record  3 1 in-area  178118 424040 place  1084 facing  0 pose  44 car-at  $80090F20 route  end-stop
  20 stop  29 9 30 until  4 record  27 1 in-area  350028 260017 place  3036 facing  22 pose  73 car-at  $80090FB4 route  end-stop
  21 stop  29 11 0 until  5 record  diner-seat  109 car-at  $800911AC route  end-stop
  22 stop  29 14 30 until  1 record  24 1 in-area  593209 131335 place  976 facing  72 pose  41 car-at  $80091210 route  end-stop
  23 stop  30 7 10 until  0 record  3 1 in-area  178118 424040 place  1084 facing  0 pose  44 car-at  $80090F20 route  end-stop
  24 stop  30 9 30 until  4 record  27 1 in-area  350028 260017 place  3036 facing  22 pose  73 car-at  $80090FB4 route  end-stop
  25 stop  30 11 0 until  5 record  diner-seat  109 car-at  $800911AC route  end-stop
  26 stop  30 14 30 until  1 record  24 1 in-area  593209 131335 place  976 facing  72 pose  41 car-at  $80091210 route  end-stop
end-schedule

\ ---- slot 13: mel ----
13 schedule
   0 stop  25 11 25 until  0 record  0 1 in-area  251837 131902 place  1780 facing  0 pose  24 car-at  $800870B4 route  end-stop
   1 stop  25 15 0 until  1 record  24 400 with-partner  $80087160 route  end-stop
   2 stop  25 16 10 until  2 record  diner-seat  20 car-at  $80087194 route  end-stop
   3 stop  25 20 0 until  3 record  27 1 in-area  573002 221446 place  2539 facing  22 pose  28 car-at  $800871D4 route  end-stop
   4 stop  26 1 0 until  4 record  club-seat  63 car-at  $80087268 route  end-stop
   5 stop  26 3 0 until  1 record  24 400 with-partner  $80087280 route  end-stop
   6 stop  26 6 25 until  5 record  6 1 in-area  90434 48417 place  22 facing  68 pose  48 car-at  $800872B4 route  end-stop
   7 stop  26 7 40 until  6 record  27 1 in-area  177009 426322 place  2058 facing  22 pose  5 car-at  $8008734C route  end-stop
   8 stop  26 8 20 until  2 record  diner-seat  67 car-at  $800873E4 route  end-stop
   9 stop  26 11 0 until  3 record  27 1 in-area  573002 221446 place  2539 facing  22 pose  112 car-at  $80087424 route  end-stop
  10 stop  26 12 0 until  7 record  38 400 with-partner  $80087458 route  end-stop
  11 stop  26 14 0 until  9 record  27 1 in-area  1 1 place  1 facing  0 pose  5 car-at  $800874F4 route  end-stop
  12 stop  26 17 5 until  5 record  6 1 in-area  90434 48417 place  22 facing  68 pose  48 car-at  $8008768C route  end-stop
  13 stop  26 17 50 until  17 record  21 1 in-area  328869 271656 place  2756 facing  179 pose  67 car-at  $800874D4 route  end-stop
  14 stop  26 20 0 until  3 record  27 1 in-area  573002 221446 place  2539 facing  22 pose  112 car-at  $80087544 route  end-stop
  15 stop  26 21 30 until  8 record  7 1 in-area  134740 309521 place  2353 facing  22 pose  84 car-at  $800875A4 route  end-stop
  16 stop  26 23 0 until  2 record  diner-seat  67 car-at  $800875BC route  end-stop
  17 stop  27 0 30 until  4 record  club-seat  94 car-at  $80087614 route  end-stop
  18 stop  27 6 35 until  5 record  6 1 in-area  90434 48417 place  22 facing  68 pose  48 car-at  $800876C4 route  end-stop
  19 stop  28 1 0 until  9 record  27 1 in-area  1 1 place  1 facing  0 pose  112 car-at  $800876F4 route  end-stop
  20 stop  28 11 55 until  10 record  0 0 in-area  251992 131383 place  172 facing  22 pose  108 car-at  $80087720 route  end-stop
  21 stop  28 13 30 until  1 record  24 400 with-partner  $80087744 route  end-stop
  22 stop  31 6 10 until  9 record  27 1 in-area  1 1 place  1 facing  0 pose  car-hidden  $80087790 route  end-stop
  23 stop  31 8 20 until  5 record  6 1 in-area  90434 48417 place  22 facing  68 pose  48 car-at  $8008764C route  end-stop
  24 stop  31 10 40 until  2 record  diner-seat  67 car-at  $80087798 route  end-stop
  25 stop  31 11 20 until  11 record  0 1 in-area  249072 135117 place  1078 facing  22 pose  108 car-at  $800877E0 route  end-stop
  26 stop  31 13 20 until  12 record  27 1 in-area  304720 308345 place  1456 facing  22 pose  141 car-at  $80087810 route  end-stop
  27 stop  31 14 10 until  13 record  27 1 in-area  575318 224490 place  2042 facing  22 pose  27 car-at  $80087858 route  end-stop
  28 stop  31 16 10 until  14 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $8008764C route  end-stop
  29 stop  31 19 30 until  2 record  diner-seat  67 car-at  $800878D8 route  end-stop
end-schedule

\ ---- slot 14: father-barton ----
14 schedule
   0 stop  25 13 40 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800841A4 route  end-stop
   1 stop  25 16 0 until  1 record  29 2 in-area  99983 359510 place  2525 facing  74 pose  4 car-at  $80084228 route  end-stop
   2 stop  25 18 0 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $80084288 route  end-stop
   3 stop  25 19 40 until  2 record  0 1 in-area  250142 134846 place  811 facing  0 pose  108 car-at  $800842D0 route  end-stop
   4 stop  25 21 45 until  3 record  27 1 in-area  463138 291111 place  189 facing  22 pose  94 car-at  $80084300 route  end-stop
   5 stop  26 7 10 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $80084340 route  end-stop
   6 stop  26 10 24 until  4 record  7 1 in-area  131143 317737 place  770 facing  0 pose  85 car-at  $80084378 route  end-stop
   7 stop  26 19 45 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843B0 route  end-stop
   8 stop  26 22 30 until  3 record  27 1 in-area  463138 291111 place  189 facing  22 pose  94 car-at  $80084300 route  end-stop
   9 stop  27 8 10 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843E0 route  end-stop
  10 stop  27 10 40 until  5 record  7 1 in-area  131841 317609 place  48 facing  1705 pose  85 car-at  $80084378 route  end-stop
  11 stop  27 19 45 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843B0 route  end-stop
  12 stop  27 22 30 until  3 record  27 1 in-area  463138 291111 place  189 facing  22 pose  94 car-at  $80084300 route  end-stop
  13 stop  28 8 10 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843E0 route  end-stop
  14 stop  28 10 10 until  5 record  7 1 in-area  131841 317609 place  48 facing  1705 pose  85 car-at  $80084378 route  end-stop
  15 stop  28 19 45 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843B0 route  end-stop
  16 stop  28 22 30 until  3 record  27 1 in-area  463138 291111 place  189 facing  22 pose  94 car-at  $80084300 route  end-stop
  17 stop  29 8 10 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843E0 route  end-stop
  18 stop  29 10 10 until  5 record  7 1 in-area  131841 317609 place  48 facing  1705 pose  85 car-at  $80084378 route  end-stop
  19 stop  29 19 45 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843B0 route  end-stop
  20 stop  29 22 30 until  3 record  27 1 in-area  463138 291111 place  189 facing  22 pose  94 car-at  $80084300 route  end-stop
  21 stop  30 8 10 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843E0 route  end-stop
  22 stop  30 10 10 until  5 record  7 1 in-area  131841 317609 place  48 facing  1705 pose  85 car-at  $80084378 route  end-stop
  23 stop  30 19 45 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843B0 route  end-stop
  24 stop  30 22 30 until  3 record  27 1 in-area  463138 291111 place  189 facing  22 pose  94 car-at  $80084300 route  end-stop
  25 stop  31 8 10 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843E0 route  end-stop
  26 stop  31 10 10 until  5 record  7 1 in-area  131841 317609 place  48 facing  1705 pose  85 car-at  $80084378 route  end-stop
  27 stop  31 19 45 until  0 record  24 1 in-area  594053 133411 place  2286 facing  0 pose  9 car-at  $800843B0 route  end-stop
  28 stop  31 21 50 until  3 record  27 1 in-area  463138 291111 place  189 facing  22 pose  94 car-at  $80084300 route  end-stop
end-schedule

\ ---- slot 15: winona ----
15 schedule
   0 stop  25 11 30 until  0 record  3 1 in-area  178654 423348 place  0 facing  0 pose  $800914FC route  end-stop
   1 stop  25 15 15 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $80091558 route  end-stop
   2 stop  25 16 30 until  2 record  diner-seat  $800915A8 route  end-stop
   3 stop  25 18 15 until  3 record  0 1 in-area  250453 135238 place  1024 facing  0 pose  $800915F4 route  end-stop
   4 stop  25 19 10 until  4 record  13 3 in-area  269236 93182 place  3228 facing  0 pose  $80091628 route  end-stop
   5 stop  25 21 25 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $8009166C route  end-stop
   6 stop  25 23 0 until  5 record  13 3 in-area  268490 92047 place  3062 facing  0 pose  $800916A8 route  end-stop
   7 stop  26 5 50 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $800916E8 route  end-stop
   8 stop  26 7 20 until  6 record  1 1 in-area  348888 285697 place  1148 facing  0 pose  $80091728 route  end-stop
   9 stop  26 10 30 until  7 record  7 1 in-area  131373 316687 place  190 facing  0 pose  $80091764 route  end-stop
  10 stop  26 12 35 until  4 record  13 3 in-area  269236 93182 place  3228 facing  0 pose  $800917A4 route  end-stop
  11 stop  26 13 20 until  3 record  0 1 in-area  250453 135238 place  1024 facing  0 pose  $800917EC route  end-stop
  12 stop  26 16 0 until  8 record  27 1 in-area  175708 418022 place  3858 facing  0 pose  $80091820 route  end-stop
  13 stop  26 16 15 until  9 record  3 1 in-area  176162 424310 place  1451 facing  0 pose  $80091850 route  end-stop
  14 stop  26 20 55 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $80091890 route  end-stop
  15 stop  26 23 0 until  4 record  13 3 in-area  269236 93182 place  3228 facing  0 pose  $800916A8 route  end-stop
  16 stop  27 4 50 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $800918D0 route  end-stop
  17 stop  27 7 55 until  10 record  27 1 in-area  378934 431473 place  168 facing  84 pose  $80091920 route  end-stop
  18 stop  27 10 0 until  11 record  9 1 in-area  334619 441412 place  2362 facing  84 pose  $80091B54 route  end-stop
  19 stop  27 11 40 until  12 record  16 1 in-area  383746 144514 place  3136 facing  22 pose  $80091BC4 route  end-stop
  20 stop  27 14 20 until  3 record  0 1 in-area  250453 135238 place  1024 facing  0 pose  $80091C3C route  end-stop
  21 stop  27 16 0 until  4 record  13 3 in-area  269236 93182 place  3228 facing  0 pose  $80091C70 route  end-stop
  22 stop  28 4 30 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $80091D4C route  end-stop
  23 stop  28 7 50 until  13 record  16 1 in-area  384303 144833 place  166 facing  22 pose  $80091DA4 route  end-stop
  24 stop  28 9 57 until  11 record  9 1 in-area  334619 441412 place  2362 facing  84 pose  $80091DF4 route  end-stop
  25 stop  28 11 0 until  14 record  1 1 in-area  345894 285870 place  1077 facing  22 pose  $80091E54 route  end-stop
  26 stop  28 13 0 until  3 record  0 1 in-area  250453 135238 place  1024 facing  0 pose  $80091E90 route  end-stop
  27 stop  28 15 0 until  13 record  16 1 in-area  384303 144833 place  166 facing  22 pose  $80091EDC route  end-stop
  28 stop  29 4 30 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $80091D4C route  end-stop
  29 stop  29 7 50 until  13 record  16 1 in-area  384303 144833 place  166 facing  22 pose  $80091DA4 route  end-stop
  30 stop  29 9 57 until  11 record  9 1 in-area  334619 441412 place  2362 facing  84 pose  $80091DF4 route  end-stop
  31 stop  29 11 0 until  14 record  1 1 in-area  345894 285870 place  1077 facing  22 pose  $80091E54 route  end-stop
  32 stop  29 13 0 until  3 record  0 1 in-area  250453 135238 place  1024 facing  0 pose  $80091E90 route  end-stop
  33 stop  29 15 0 until  13 record  16 1 in-area  384303 144833 place  166 facing  22 pose  $80091EDC route  end-stop
  34 stop  30 4 30 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $80091D4C route  end-stop
  35 stop  30 7 50 until  13 record  16 1 in-area  384303 144833 place  166 facing  22 pose  $80091DA4 route  end-stop
  36 stop  30 9 57 until  11 record  9 1 in-area  334619 441412 place  2362 facing  84 pose  $80091DF4 route  end-stop
  37 stop  30 11 0 until  14 record  1 1 in-area  345894 285870 place  1077 facing  22 pose  $80091E54 route  end-stop
  38 stop  30 13 0 until  3 record  0 1 in-area  250453 135238 place  1024 facing  0 pose  $80091E90 route  end-stop
  39 stop  30 15 0 until  13 record  16 1 in-area  384303 144833 place  166 facing  22 pose  $80091EDC route  end-stop
  40 stop  31 5 0 until  1 record  18 1 in-area  413646 170139 place  3084 facing  167 pose  $80091F2C route  end-stop
  41 stop  31 8 15 until  4 record  13 3 in-area  269236 93182 place  3228 facing  0 pose  $80091F6C route  end-stop
  42 stop  31 10 20 until  3 record  0 1 in-area  250453 135238 place  1024 facing  0 pose  $80091FB4 route  end-stop
  43 stop  31 15 10 until  15 record  27 1 in-area  463489 413369 place  1020 facing  0 pose  $80092010 route  end-stop
  44 stop  31 18 30 until  17 record  29 2 in-area  101403 349869 place  1042 facing  167 pose  $80092060 route  end-stop
end-schedule

\ ---- slot 20: lidds ----
20 schedule
   0 stop  25 14 50 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086704 route  end-stop
   1 stop  25 16 30 until  1 record  24 1 in-area  592752 131820 place  1009 facing  72 pose  36 car-at  $8008676C route  end-stop
   2 stop  25 19 30 until  2 record  29 2 in-area  98643 349787 place  1033 facing  72 pose  61 car-at  $800867BC route  end-stop
   3 stop  25 21 0 until  3 record  13 3 in-area  269280 93017 place  3147 facing  0 pose  62 car-at  $80086850 route  end-stop
   4 stop  26 6 20 until  4 record  27 1 in-area  335777 65277 place  0 facing  0 pose  98 car-at  $80086888 route  end-stop
   5 stop  26 8 25 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $800868C0 route  end-stop
   6 stop  26 10 20 until  5 record  7 1 in-area  132553 317277 place  3606 facing  0 pose  126 car-at  $80086908 route  end-stop
   7 stop  26 15 33 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086940 route  end-stop
   8 stop  26 17 0 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086A54 route  end-stop
   9 stop  26 19 30 until  2 record  29 2 in-area  98643 349787 place  1033 facing  72 pose  61 car-at  $800867BC route  end-stop
  10 stop  26 21 0 until  3 record  13 3 in-area  269280 93017 place  3147 facing  0 pose  62 car-at  $80086850 route  end-stop
  11 stop  27 6 20 until  4 record  27 1 in-area  335777 65277 place  0 facing  0 pose  98 car-at  $80086AAC route  end-stop
  12 stop  27 8 0 until  3 record  13 3 in-area  269280 93017 place  3147 facing  0 pose  62 car-at  $80086B04 route  end-stop
  13 stop  27 9 50 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086948 route  end-stop
  14 stop  27 12 20 until  6 record  16 1 in-area  389606 141436 place  2102 facing  3502 pose  31 car-at  $800869F4 route  end-stop
  15 stop  27 14 50 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086704 route  end-stop
  16 stop  27 16 30 until  1 record  24 1 in-area  592752 131820 place  1009 facing  72 pose  36 car-at  $8008676C route  end-stop
  17 stop  27 20 0 until  2 record  29 2 in-area  98643 349787 place  1033 facing  72 pose  61 car-at  $80086B3C route  end-stop
  18 stop  27 21 40 until  7 record  0 1 in-area  250155 135369 place  1111 facing  0 pose  62 car-at  $80086B9C route  end-stop
  19 stop  28 6 20 until  4 record  27 1 in-area  335777 65277 place  0 facing  0 pose  98 car-at  $80086888 route  end-stop
  20 stop  28 8 15 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086948 route  end-stop
  21 stop  28 8 45 until  6 record  16 1 in-area  389606 141436 place  2102 facing  3502 pose  31 car-at  $800869F4 route  end-stop
  22 stop  28 13 30 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086A54 route  end-stop
  23 stop  28 19 30 until  11 record  29 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $800867BC route  end-stop
  24 stop  28 21 0 until  3 record  13 3 in-area  269280 93017 place  3147 facing  0 pose  62 car-at  $80086850 route  end-stop
  25 stop  29 6 20 until  4 record  27 1 in-area  335777 65277 place  0 facing  0 pose  98 car-at  $80086888 route  end-stop
  26 stop  29 8 15 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086948 route  end-stop
  27 stop  29 8 45 until  6 record  16 1 in-area  389606 141436 place  2102 facing  3502 pose  31 car-at  $800869F4 route  end-stop
  28 stop  29 13 30 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086A54 route  end-stop
  29 stop  29 19 30 until  2 record  29 2 in-area  98643 349787 place  1033 facing  72 pose  61 car-at  $800867BC route  end-stop
  30 stop  29 21 0 until  3 record  13 3 in-area  269280 93017 place  3147 facing  0 pose  62 car-at  $80086850 route  end-stop
  31 stop  30 6 20 until  4 record  27 1 in-area  335777 65277 place  0 facing  0 pose  98 car-at  $80086888 route  end-stop
  32 stop  30 8 15 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086948 route  end-stop
  33 stop  30 8 45 until  6 record  16 1 in-area  389606 141436 place  2102 facing  3502 pose  31 car-at  $800869F4 route  end-stop
  34 stop  30 13 30 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086A54 route  end-stop
  35 stop  30 19 30 until  2 record  29 2 in-area  98643 349787 place  1033 facing  72 pose  61 car-at  $800867BC route  end-stop
  36 stop  30 21 0 until  3 record  13 3 in-area  269280 93017 place  3147 facing  0 pose  62 car-at  $80086850 route  end-stop
  37 stop  31 6 30 until  4 record  27 1 in-area  335777 65277 place  0 facing  0 pose  98 car-at  $80086888 route  end-stop
  38 stop  31 7 50 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086BCC route  end-stop
  39 stop  31 9 45 until  8 record  27 1 in-area  94813 54997 place  2506 facing  0 pose  161 car-at  $80086C24 route  end-stop
  40 stop  31 13 45 until  0 record  16 1 in-area  384165 150357 place  2048 facing  3502 pose  31 car-at  $80086C6C route  end-stop
  41 stop  31 16 30 until  9 record  1 1 in-area  345625 286244 place  1595 facing  0 pose  19 car-at  $80086CC4 route  end-stop
  42 stop  31 20 0 until  10 record  2 1 in-area  101979 355447 place  171 facing  0 pose  4 car-at  $80086B3C route  end-stop
end-schedule

\ ---- slot 21: wolf ----
21 schedule
   0 stop  25 10 45 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $80092484 route  end-stop
   1 stop  25 18 0 until  1 record  12 1 in-area  1 1 place  3983 facing  0 pose  87 car-at  189 variant  $800924BC route  end-stop
   2 stop  26 1 30 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $800924EC route  end-stop
   3 stop  26 3 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009252C route  end-stop
   4 stop  26 8 40 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $80092484 route  end-stop
   5 stop  26 11 10 until  1 record  12 1 in-area  1 1 place  3983 facing  0 pose  87 car-at  189 variant  $8009255C route  end-stop
   6 stop  26 14 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009258C route  end-stop
   7 stop  26 16 0 until  3 record  9 1 in-area  335531 440563 place  778 facing  22 pose  144 car-at  189 variant  $800925BC route  end-stop
   8 stop  26 19 20 until  4 record  27 1 in-area  469853 292621 place  4018 facing  22 pose  63 car-at  189 variant  $800925F4 route  end-stop
   9 stop  27 1 30 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $800924EC route  end-stop
  10 stop  27 3 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009252C route  end-stop
  11 stop  27 8 40 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $80092484 route  end-stop
  12 stop  27 11 10 until  1 record  12 1 in-area  1 1 place  3983 facing  0 pose  87 car-at  189 variant  $8009255C route  end-stop
  13 stop  27 14 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009258C route  end-stop
  14 stop  27 16 0 until  3 record  9 1 in-area  335531 440563 place  778 facing  22 pose  144 car-at  189 variant  $800925BC route  end-stop
  15 stop  27 19 20 until  4 record  27 1 in-area  469853 292621 place  4018 facing  22 pose  63 car-at  189 variant  $800925F4 route  end-stop
  16 stop  28 1 30 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $800924EC route  end-stop
  17 stop  28 3 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009252C route  end-stop
  18 stop  28 8 45 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $80092484 route  end-stop
  19 stop  28 11 10 until  1 record  12 1 in-area  1 1 place  3983 facing  0 pose  87 car-at  189 variant  $8009255C route  end-stop
  20 stop  28 14 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009258C route  end-stop
  21 stop  28 16 0 until  3 record  9 1 in-area  335531 440563 place  778 facing  22 pose  144 car-at  189 variant  $800925BC route  end-stop
  22 stop  28 19 20 until  4 record  27 1 in-area  469853 292621 place  4018 facing  22 pose  63 car-at  189 variant  $800925F4 route  end-stop
  23 stop  29 1 30 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $800924EC route  end-stop
  24 stop  29 3 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009252C route  end-stop
  25 stop  29 8 45 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $80092484 route  end-stop
  26 stop  29 11 10 until  1 record  12 1 in-area  1 1 place  3983 facing  0 pose  87 car-at  189 variant  $8009255C route  end-stop
  27 stop  29 14 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009258C route  end-stop
  28 stop  29 16 0 until  3 record  9 1 in-area  335531 440563 place  778 facing  22 pose  144 car-at  189 variant  $800925BC route  end-stop
  29 stop  29 19 20 until  4 record  27 1 in-area  469853 292621 place  4018 facing  22 pose  63 car-at  189 variant  $800925F4 route  end-stop
  30 stop  30 1 30 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $800924EC route  end-stop
  31 stop  30 3 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009252C route  end-stop
  32 stop  30 8 45 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $80092484 route  end-stop
  33 stop  30 11 10 until  1 record  12 1 in-area  1 1 place  3983 facing  0 pose  87 car-at  189 variant  $8009255C route  end-stop
  34 stop  30 14 30 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $8009258C route  end-stop
  35 stop  30 16 0 until  3 record  9 1 in-area  335531 440563 place  778 facing  22 pose  144 car-at  189 variant  $800925BC route  end-stop
  36 stop  30 19 20 until  4 record  27 1 in-area  469853 292621 place  4018 facing  22 pose  63 car-at  189 variant  $800925F4 route  end-stop
  37 stop  31 7 30 until  0 record  12 1 in-area  469042 289170 place  3983 facing  0 pose  63 car-at  $800924EC route  end-stop
  38 stop  31 9 20 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $80092694 route  end-stop
  39 stop  31 11 30 until  6 record  27 1 in-area  113817 52532 place  0 facing  0 pose  105 car-at  189 variant  $800926E8 route  end-stop
  40 stop  31 13 30 until  3 record  9 1 in-area  335531 440563 place  778 facing  22 pose  169 car-at  189 variant  $80092718 route  end-stop
  41 stop  31 14 30 until  7 record  12 1 in-area  465657 288166 place  0 facing  0 pose  63 car-at  $800924EC route  end-stop
  42 stop  31 16 0 until  2 record  19 2 in-area  307995 306371 place  3364 facing  22 pose  141 car-at  189 variant  $80092750 route  end-stop
  43 stop  31 19 45 until  7 record  12 1 in-area  465657 288166 place  0 facing  0 pose  63 car-at  $80092634 route  end-stop
end-schedule

\ ---- slot 22: rolein ----
22 schedule
   0 stop  25 21 40 until  0 record  21 1 in-area  327913 269469 place  1024 facing  0 pose  $80090160 route  end-stop
   1 stop  25 23 10 until  1 record  club-seat  188 variant  $80090188 route  end-stop
   2 stop  26 21 40 until  0 record  21 1 in-area  327913 269469 place  1024 facing  0 pose  $80090160 route  end-stop
   3 stop  26 23 10 until  1 record  club-seat  188 variant  $80090188 route  end-stop
   4 stop  27 20 0 until  0 record  21 1 in-area  327913 269469 place  1024 facing  0 pose  $800901B0 route  end-stop
   5 stop  27 21 40 until  2 record  13 3 in-area  271720 91241 place  3068 facing  0 pose  188 variant  $80090210 route  end-stop
   6 stop  28 0 20 until  1 record  club-seat  188 variant  $80090188 route  end-stop
   7 stop  28 19 0 until  0 record  21 1 in-area  327913 269469 place  1024 facing  0 pose  $80090268 route  end-stop
   8 stop  28 21 40 until  3 record  27 1 in-area  8 8 place  8 facing  0 pose  188 variant  $800902F0 route  end-stop
   9 stop  29 0 20 until  1 record  club-seat  188 variant  $80090188 route  end-stop
  10 stop  29 19 0 until  0 record  21 1 in-area  327913 269469 place  1024 facing  0 pose  $80090268 route  end-stop
  11 stop  29 21 40 until  3 record  27 1 in-area  8 8 place  8 facing  0 pose  188 variant  $800902F0 route  end-stop
  12 stop  30 0 20 until  1 record  club-seat  188 variant  $80090188 route  end-stop
  13 stop  30 19 0 until  0 record  21 1 in-area  327913 269469 place  1024 facing  0 pose  $80090268 route  end-stop
  14 stop  30 21 40 until  3 record  27 1 in-area  8 8 place  8 facing  0 pose  188 variant  $800902F0 route  end-stop
  15 stop  31 0 20 until  1 record  club-seat  188 variant  $80090188 route  end-stop
  16 stop  31 19 10 until  0 record  21 1 in-area  327913 269469 place  1024 facing  0 pose  $80090160 route  end-stop
  17 stop  31 21 40 until  1 record  club-seat  188 variant  $80090188 route  end-stop
end-schedule

\ ---- slot 89: isabella ----
89 schedule
   0 stop  25 13 40 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E28 route  end-stop
   1 stop  25 16 10 until  1 record  8 1 in-area  106928 194677 place  0 facing  0 pose  142 car-at  184 variant  $80084E58 route  end-stop
   2 stop  25 19 0 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E80 route  end-stop
   3 stop  26 0 30 until  2 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80084EB0 route  end-stop
   4 stop  26 13 40 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E28 route  end-stop
   5 stop  26 16 10 until  1 record  8 1 in-area  106928 194677 place  0 facing  0 pose  142 car-at  184 variant  $80084E58 route  end-stop
   6 stop  26 19 0 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E80 route  end-stop
   7 stop  27 0 30 until  2 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80084EB0 route  end-stop
   8 stop  27 13 40 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E28 route  end-stop
   9 stop  27 16 10 until  1 record  8 1 in-area  106928 194677 place  0 facing  0 pose  142 car-at  184 variant  $80084E58 route  end-stop
  10 stop  27 19 0 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E80 route  end-stop
  11 stop  28 0 30 until  2 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80084EB0 route  end-stop
  12 stop  28 5 15 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084EEC route  end-stop
  13 stop  28 6 45 until  3 record  13 3 in-area  271720 91241 place  3068 facing  0 pose  162 car-at  184 variant  $80084F1C route  end-stop
  14 stop  28 13 40 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E28 route  end-stop
  15 stop  28 16 10 until  1 record  8 1 in-area  106928 194677 place  0 facing  0 pose  142 car-at  184 variant  $80084E58 route  end-stop
  16 stop  28 19 0 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E80 route  end-stop
  17 stop  29 0 30 until  2 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80084EB0 route  end-stop
  18 stop  29 5 15 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084EEC route  end-stop
  19 stop  29 6 45 until  3 record  13 3 in-area  271720 91241 place  3068 facing  0 pose  162 car-at  184 variant  $80084F1C route  end-stop
  20 stop  29 13 40 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E28 route  end-stop
  21 stop  29 16 10 until  1 record  8 1 in-area  106928 194677 place  0 facing  0 pose  142 car-at  184 variant  $80084E58 route  end-stop
  22 stop  29 19 0 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E80 route  end-stop
  23 stop  30 0 30 until  2 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80084EB0 route  end-stop
  24 stop  30 5 15 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084EEC route  end-stop
  25 stop  30 6 45 until  3 record  13 3 in-area  271720 91241 place  3068 facing  0 pose  162 car-at  184 variant  $80084F1C route  end-stop
  26 stop  30 13 40 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E28 route  end-stop
  27 stop  30 16 10 until  1 record  8 1 in-area  106928 194677 place  0 facing  0 pose  142 car-at  184 variant  $80084E58 route  end-stop
  28 stop  30 19 0 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084E80 route  end-stop
  29 stop  31 0 30 until  2 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80084EB0 route  end-stop
  30 stop  31 6 10 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  145 car-at  184 variant  $80084F54 route  end-stop
  31 stop  31 8 30 until  4 record  27 1 in-area  395919 432181 place  370 facing  0 pose  169 car-at  184 variant  $80084FAC route  end-stop
  32 stop  31 10 0 until  5 record  9 1 in-area  335245 440975 place  524 facing  0 pose  169 car-at  184 variant  $80084FFC route  end-stop
  33 stop  31 11 45 until  6 record  1 1 in-area  345102 285706 place  1047 facing  0 pose  109 car-at  184 variant  $8008504C route  end-stop
  34 stop  31 18 30 until  0 record  19 2 in-area  307439 306593 place  4062 facing  68 pose  141 car-at  184 variant  $80084E80 route  end-stop
  35 stop  31 19 50 until  2 record  12 1 in-area  466308 288205 place  0 facing  0 pose  140 car-at  $8008508C route  end-stop
end-schedule

\ ---- slot 90: barbara ----
90 schedule
   0 stop  25 10 48 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
   1 stop  25 14 10 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
   2 stop  25 17 30 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
   3 stop  26 7 10 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
   4 stop  26 11 40 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
   5 stop  26 14 10 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
   6 stop  26 17 30 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
   7 stop  27 9 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
   8 stop  27 11 40 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
   9 stop  27 14 10 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  10 stop  27 17 30 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  11 stop  28 9 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  12 stop  28 11 10 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  13 stop  28 12 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  14 stop  28 14 40 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  15 stop  29 9 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  16 stop  29 11 10 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  17 stop  29 12 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  18 stop  29 14 40 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  19 stop  30 9 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  20 stop  30 11 10 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  21 stop  30 12 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  22 stop  30 14 40 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  23 stop  31 9 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  24 stop  31 11 10 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  25 stop  31 12 40 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081B94 route  end-stop
  26 stop  31 14 40 until  0 record  2 1 in-area  105462 359776 place  2057 facing  126 pose  $80081B54 route  end-stop
  27 stop  31 17 20 until  1 record  29 2 in-area  98786 350590 place  1444 facing  126 pose  $80081BD4 route  end-stop
  28 stop  31 19 30 until  2 record  2 1 in-area  102033 356275 place  1589 facing  126 pose  $80081BFC route  end-stop
end-schedule

\ ---- slot 92: dennis ----
92 schedule
   0 stop  25 11 35 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083794 route  end-stop
   1 stop  25 14 0 until  1 record  24 1 in-area  592739 131379 place  998 facing  72 pose  124 car-at  $800837EC route  end-stop
   2 stop  25 16 0 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083834 route  end-stop
   3 stop  25 18 30 until  2 record  27 1 in-area  565344 214334 place  1029 facing  0 pose  28 car-at  $8008392C route  end-stop
   4 stop  25 22 40 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $8008396C route  end-stop
   5 stop  26 0 10 until  3 record  27 1 in-area  113817 52532 place  0 facing  0 pose  48 car-at  $8008398C route  end-stop
   6 stop  26 6 50 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $8008396C route  end-stop
   7 stop  26 7 45 until  3 record  27 1 in-area  113817 52532 place  0 facing  0 pose  48 car-at  $8008398C route  end-stop
   8 stop  26 8 20 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083834 route  end-stop
   9 stop  26 11 0 until  2 record  27 1 in-area  565344 214334 place  1029 facing  0 pose  28 car-at  $8008392C route  end-stop
  10 stop  26 20 20 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $8008396C route  end-stop
  11 stop  26 21 15 until  3 record  27 1 in-area  113817 52532 place  0 facing  0 pose  48 car-at  $800839C8 route  end-stop
  12 stop  26 23 30 until  4 record  19 2 in-area  310191 307011 place  314 facing  151 pose  139 car-at  $800839F8 route  end-stop
  13 stop  27 7 20 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $8008396C route  end-stop
  14 stop  27 8 15 until  3 record  27 1 in-area  113817 52532 place  0 facing  0 pose  48 car-at  $80083A28 route  end-stop
  15 stop  27 10 45 until  5 record  9 1 in-area  334412 441486 place  684 facing  22 pose  144 car-at  $80083A70 route  end-stop
  16 stop  27 13 20 until  6 record  8 1 in-area  1 1 place  1 facing  0 pose  87 car-at  $80083AB8 route  end-stop
  17 stop  27 20 20 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $8008396C route  end-stop
  18 stop  27 21 15 until  3 record  27 1 in-area  113817 52532 place  0 facing  0 pose  48 car-at  $800839C8 route  end-stop
  19 stop  27 23 45 until  4 record  19 2 in-area  310191 307011 place  314 facing  151 pose  139 car-at  $800839F8 route  end-stop
  20 stop  28 10 0 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083864 route  end-stop
  21 stop  28 11 56 until  7 record  0 0 in-area  249398 133667 place  1352 facing  22 pose  108 car-at  $800838E4 route  end-stop
  22 stop  28 13 30 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083AE0 route  end-stop
  23 stop  28 17 30 until  8 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80083B10 route  end-stop
  24 stop  28 21 30 until  4 record  19 2 in-area  310191 307011 place  314 facing  151 pose  139 car-at  $80083B40 route  end-stop
  25 stop  28 22 30 until  9 record  club-seat  63 car-at  $80083B70 route  end-stop
  26 stop  29 10 0 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083864 route  end-stop
  27 stop  29 11 51 until  7 record  0 0 in-area  249398 133667 place  1352 facing  22 pose  108 car-at  $800838E4 route  end-stop
  28 stop  29 13 30 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083AE0 route  end-stop
  29 stop  29 17 30 until  8 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80083B10 route  end-stop
  30 stop  29 21 30 until  4 record  19 2 in-area  310191 307011 place  314 facing  151 pose  139 car-at  $80083B40 route  end-stop
  31 stop  29 22 30 until  9 record  club-seat  63 car-at  $80083B70 route  end-stop
  32 stop  30 10 0 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083864 route  end-stop
  33 stop  30 11 51 until  7 record  0 0 in-area  249398 133667 place  1352 facing  22 pose  108 car-at  $800838E4 route  end-stop
  34 stop  30 13 30 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083AE0 route  end-stop
  35 stop  30 17 30 until  8 record  12 1 in-area  466308 288205 place  0 facing  0 pose  63 car-at  $80083B10 route  end-stop
  36 stop  30 21 30 until  4 record  19 2 in-area  310191 307011 place  314 facing  151 pose  139 car-at  $80083B40 route  end-stop
  37 stop  30 22 30 until  9 record  club-seat  63 car-at  $80083B70 route  end-stop
  38 stop  31 7 20 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $8008396C route  end-stop
  39 stop  31 8 15 until  3 record  27 1 in-area  113817 52532 place  0 facing  0 pose  48 car-at  $800839C8 route  end-stop
  40 stop  31 12 30 until  4 record  19 2 in-area  310191 307011 place  314 facing  151 pose  139 car-at  $800839F8 route  end-stop
  41 stop  31 16 15 until  0 record  6 1 in-area  90631 53264 place  0 facing  0 pose  48 car-at  $80083D60 route  end-stop
  42 stop  31 19 15 until  4 record  19 2 in-area  310191 307011 place  314 facing  151 pose  141 car-at  $800839F8 route  end-stop
end-schedule

\ ---- slot 93: robert ----
93 schedule
   0 stop  25 12 5 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  6 car-at  $8008FB54 route  end-stop
   1 stop  25 13 40 until  1 record  24 1 in-area  593718 131467 place  53 facing  0 pose  15 car-at  $8008FBA4 route  end-stop
   2 stop  25 19 3 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  6 car-at  $8008FBDC route  end-stop
   3 stop  25 21 25 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  6 car-at  $8008FC84 route  end-stop
   4 stop  25 22 40 until  2 record  0 1 in-area  249150 134008 place  2047 facing  22 pose  42 car-at  $8008FCD8 route  end-stop
   5 stop  26 20 55 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FC30 route  end-stop
   6 stop  27 7 40 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FD18 route  end-stop
   7 stop  27 10 3 until  3 record  27 1 in-area  337424 436815 place  404 facing  22 pose  153 car-at  $8008FD50 route  end-stop
   8 stop  28 7 40 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FDAC route  end-stop
   9 stop  28 10 0 until  3 record  27 1 in-area  337424 436815 place  404 facing  22 pose  153 car-at  $8008FE00 route  end-stop
  10 stop  28 14 25 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FE5C route  end-stop
  11 stop  28 17 45 until  4 record  3 1 in-area  177757 423327 place  1722 facing  72 pose  155 car-at  $8008FED4 route  end-stop
  12 stop  29 7 40 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FDAC route  end-stop
  13 stop  29 10 0 until  3 record  27 1 in-area  337424 436815 place  404 facing  22 pose  153 car-at  $8008FE00 route  end-stop
  14 stop  29 14 25 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FE5C route  end-stop
  15 stop  29 17 45 until  4 record  3 1 in-area  177757 423327 place  1722 facing  72 pose  155 car-at  $8008FED4 route  end-stop
  16 stop  30 7 40 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FDAC route  end-stop
  17 stop  30 10 0 until  3 record  27 1 in-area  337424 436815 place  404 facing  22 pose  153 car-at  $8008FE00 route  end-stop
  18 stop  30 14 25 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FE5C route  end-stop
  19 stop  30 17 45 until  4 record  3 1 in-area  177757 423327 place  1722 facing  72 pose  155 car-at  $8008FED4 route  end-stop
  20 stop  31 14 10 until  0 record  18 1 in-area  412465 169071 place  58 facing  0 pose  95 car-at  $8008FF0C route  end-stop
  21 stop  31 18 30 until  5 record  27 1 in-area  112502 353775 place  2236 facing  22 pose  4 car-at  $8008FF60 route  end-stop
end-schedule

\ ---- slot 65: townsperson 0 ----
65 schedule
   0 stop  25 10 10 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
   1 stop  25 12 0 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B40C route  end-stop
   2 stop  25 16 20 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
   3 stop  25 18 20 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
   4 stop  26 7 30 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
   5 stop  26 9 35 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
   6 stop  26 10 10 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
   7 stop  26 12 0 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B40C route  end-stop
   8 stop  26 16 20 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
   9 stop  26 18 20 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  10 stop  27 7 30 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  11 stop  27 9 35 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  12 stop  27 10 10 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  13 stop  27 12 0 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B40C route  end-stop
  14 stop  27 16 20 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  15 stop  27 18 20 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  16 stop  28 7 30 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  17 stop  28 9 35 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  18 stop  28 10 10 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  19 stop  28 12 0 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B40C route  end-stop
  20 stop  28 16 20 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  21 stop  28 18 20 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  22 stop  29 7 30 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  23 stop  29 9 35 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  24 stop  29 10 10 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  25 stop  29 12 0 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B40C route  end-stop
  26 stop  29 16 20 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  27 stop  29 18 20 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  28 stop  30 7 30 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  29 stop  30 9 35 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  30 stop  30 10 10 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  31 stop  30 12 0 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B40C route  end-stop
  32 stop  30 16 20 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  33 stop  30 18 20 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  34 stop  31 7 30 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  35 stop  31 9 35 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
  36 stop  31 10 10 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  37 stop  31 12 0 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B40C route  end-stop
  38 stop  31 16 20 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008B3B4 route  end-stop
  39 stop  31 18 20 until  1 record  27 1 in-area  379835 69617 place  1052 facing  0 pose  $8008B44C route  end-stop
end-schedule

\ ---- slot 66: townsperson 1 ----
66 schedule
   0 stop  25 10 2 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B464 route  end-stop
   1 stop  25 14 40 until  1 record  27 1 in-area  361637 289673 place  3983 facing  0 pose  $8008B4CC route  end-stop
   2 stop  25 18 30 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B514 route  end-stop
   3 stop  26 2 15 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B5B4 route  end-stop
   4 stop  26 10 2 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B464 route  end-stop
   5 stop  26 14 40 until  1 record  27 1 in-area  361637 289673 place  3983 facing  0 pose  $8008B4CC route  end-stop
   6 stop  26 18 30 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B514 route  end-stop
   7 stop  27 2 15 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B5B4 route  end-stop
   8 stop  27 10 2 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B464 route  end-stop
   9 stop  27 14 40 until  1 record  27 1 in-area  361637 289673 place  3983 facing  0 pose  $8008B4CC route  end-stop
  10 stop  27 18 30 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B514 route  end-stop
  11 stop  28 2 15 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B5B4 route  end-stop
  12 stop  28 10 2 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B464 route  end-stop
  13 stop  28 14 40 until  1 record  27 1 in-area  361637 289673 place  3983 facing  0 pose  $8008B4CC route  end-stop
  14 stop  28 18 30 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B514 route  end-stop
  15 stop  29 2 15 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B5B4 route  end-stop
  16 stop  29 10 2 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B464 route  end-stop
  17 stop  29 14 40 until  1 record  27 1 in-area  361637 289673 place  3983 facing  0 pose  $8008B4CC route  end-stop
  18 stop  29 18 30 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B514 route  end-stop
  19 stop  30 2 15 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B5B4 route  end-stop
  20 stop  30 10 2 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B464 route  end-stop
  21 stop  30 14 40 until  1 record  27 1 in-area  361637 289673 place  3983 facing  0 pose  $8008B4CC route  end-stop
  22 stop  30 18 30 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B514 route  end-stop
  23 stop  31 2 15 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B5B4 route  end-stop
  24 stop  31 10 2 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B464 route  end-stop
  25 stop  31 14 40 until  1 record  27 1 in-area  361637 289673 place  3983 facing  0 pose  $8008B4CC route  end-stop
  26 stop  31 18 30 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008B514 route  end-stop
end-schedule

\ ---- slot 67: townsperson 2 ----
67 schedule
   0 stop  25 10 14 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   1 stop  25 13 19 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   2 stop  25 16 24 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   3 stop  26 10 14 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   4 stop  26 13 19 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   5 stop  26 16 24 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   6 stop  27 10 14 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   7 stop  27 13 19 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   8 stop  27 16 24 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
   9 stop  28 10 14 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  10 stop  28 13 19 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  11 stop  28 16 24 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  12 stop  29 10 14 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  13 stop  29 13 19 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  14 stop  29 16 24 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  15 stop  30 10 14 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  16 stop  30 13 19 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  17 stop  30 16 24 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  18 stop  31 10 14 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  19 stop  31 13 19 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
  20 stop  31 16 24 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008B604 route  end-stop
end-schedule

\ ---- slot 68: townsperson 3 ----
68 schedule
   0 stop  25 10 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008B61C route  end-stop
   1 stop  25 13 10 until  1 record  37 360 with-partner  $8008B658 route  end-stop
   2 stop  25 14 30 until  2 record  diner-seat  67 car-at  $8008B68C route  end-stop
   3 stop  25 17 0 until  3 record  90 360 with-partner  $8008B6E8 route  end-stop
   4 stop  25 19 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008B71C route  end-stop
   5 stop  25 20 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008B74C route  end-stop
   6 stop  26 10 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008B61C route  end-stop
   7 stop  26 13 10 until  1 record  37 360 with-partner  $8008B658 route  end-stop
   8 stop  26 14 30 until  2 record  diner-seat  67 car-at  $8008B68C route  end-stop
   9 stop  26 17 0 until  3 record  90 360 with-partner  $8008B6E8 route  end-stop
  10 stop  26 19 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008B71C route  end-stop
  11 stop  26 20 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008B74C route  end-stop
  12 stop  27 10 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008B61C route  end-stop
  13 stop  27 13 10 until  1 record  37 360 with-partner  $8008B658 route  end-stop
  14 stop  27 14 30 until  2 record  diner-seat  67 car-at  $8008B68C route  end-stop
  15 stop  27 17 0 until  3 record  90 360 with-partner  $8008B6E8 route  end-stop
  16 stop  27 19 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008B71C route  end-stop
  17 stop  27 20 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008B74C route  end-stop
  18 stop  28 10 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008B61C route  end-stop
  19 stop  28 13 10 until  1 record  37 360 with-partner  $8008B658 route  end-stop
  20 stop  28 14 30 until  2 record  diner-seat  67 car-at  $8008B68C route  end-stop
  21 stop  28 17 0 until  3 record  90 360 with-partner  $8008B6E8 route  end-stop
  22 stop  28 19 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008B71C route  end-stop
  23 stop  28 20 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008B74C route  end-stop
  24 stop  29 10 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008B61C route  end-stop
  25 stop  29 13 10 until  1 record  37 360 with-partner  $8008B658 route  end-stop
  26 stop  29 14 30 until  2 record  diner-seat  67 car-at  $8008B68C route  end-stop
  27 stop  29 17 0 until  3 record  90 360 with-partner  $8008B6E8 route  end-stop
  28 stop  29 19 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008B71C route  end-stop
  29 stop  29 20 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008B74C route  end-stop
  30 stop  30 10 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008B61C route  end-stop
  31 stop  30 13 10 until  1 record  37 360 with-partner  $8008B658 route  end-stop
  32 stop  30 14 30 until  2 record  diner-seat  67 car-at  $8008B68C route  end-stop
  33 stop  30 17 0 until  3 record  90 360 with-partner  $8008B6E8 route  end-stop
  34 stop  30 19 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008B71C route  end-stop
  35 stop  30 20 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008B74C route  end-stop
  36 stop  31 10 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008B61C route  end-stop
  37 stop  31 13 10 until  1 record  37 360 with-partner  $8008B658 route  end-stop
  38 stop  31 14 30 until  2 record  diner-seat  67 car-at  $8008B68C route  end-stop
  39 stop  31 17 0 until  3 record  90 360 with-partner  $8008B6E8 route  end-stop
  40 stop  31 19 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008B71C route  end-stop
  41 stop  31 20 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008B74C route  end-stop
end-schedule

\ ---- slot 69: townsperson 4 ----
69 schedule
   0 stop  25 10 10 until  0 record  27 1 in-area  468531 411566 place  0 facing  0 pose  78 car-at  $8008B79C route  end-stop
   1 stop  25 10 50 until  1 record  diner-seat  109 car-at  $8008B808 route  end-stop
   2 stop  25 11 20 until  2 record  18 1 in-area  412881 169603 place  512 facing  22 pose  32 car-at  $8008B85C route  end-stop
   3 stop  25 13 10 until  3 record  90 360 with-partner  $8008B888 route  end-stop
   4 stop  25 15 10 until  4 record  27 1 in-area  346072 312393 place  0 facing  0 pose  73 car-at  $8008B8DC route  end-stop
   5 stop  25 19 0 until  5 record  13 3 in-area  270840 88769 place  4039 facing  72 pose  97 car-at  $8008B990 route  end-stop
   6 stop  25 20 2 until  1 record  diner-seat  34 car-at  $8008B9F0 route  end-stop
   7 stop  25 22 0 until  6 record  club-seat  94 car-at  $8008BA48 route  end-stop
   8 stop  26 8 10 until  0 record  27 1 in-area  468531 411566 place  0 facing  0 pose  78 car-at  $8008B79C route  end-stop
   9 stop  26 9 10 until  1 record  diner-seat  109 car-at  $8008B808 route  end-stop
  10 stop  26 9 40 until  2 record  18 1 in-area  412881 169603 place  512 facing  22 pose  32 car-at  $8008B85C route  end-stop
  11 stop  26 13 10 until  3 record  90 360 with-partner  $8008B888 route  end-stop
  12 stop  26 15 10 until  4 record  27 1 in-area  346072 312393 place  0 facing  0 pose  73 car-at  $8008B8DC route  end-stop
  13 stop  26 19 0 until  5 record  13 3 in-area  270840 88769 place  4039 facing  72 pose  97 car-at  $8008B990 route  end-stop
  14 stop  26 20 2 until  1 record  diner-seat  34 car-at  $8008B9F0 route  end-stop
  15 stop  26 22 0 until  6 record  club-seat  94 car-at  $8008BA48 route  end-stop
  16 stop  27 8 10 until  0 record  27 1 in-area  468531 411566 place  0 facing  0 pose  78 car-at  $8008B79C route  end-stop
  17 stop  27 9 10 until  1 record  diner-seat  109 car-at  $8008B808 route  end-stop
  18 stop  27 9 40 until  2 record  18 1 in-area  412881 169603 place  512 facing  22 pose  32 car-at  $8008B85C route  end-stop
  19 stop  27 13 10 until  3 record  90 360 with-partner  $8008B888 route  end-stop
  20 stop  27 15 10 until  4 record  27 1 in-area  346072 312393 place  0 facing  0 pose  73 car-at  $8008B8DC route  end-stop
  21 stop  27 19 0 until  5 record  13 3 in-area  270840 88769 place  4039 facing  72 pose  97 car-at  $8008B990 route  end-stop
  22 stop  27 20 2 until  1 record  diner-seat  34 car-at  $8008B9F0 route  end-stop
  23 stop  27 22 0 until  6 record  club-seat  94 car-at  $8008BA48 route  end-stop
  24 stop  28 8 10 until  0 record  27 1 in-area  468531 411566 place  0 facing  0 pose  78 car-at  $8008B79C route  end-stop
  25 stop  28 9 10 until  1 record  diner-seat  109 car-at  $8008B808 route  end-stop
  26 stop  28 9 40 until  2 record  18 1 in-area  412881 169603 place  512 facing  22 pose  32 car-at  $8008B85C route  end-stop
  27 stop  28 13 10 until  3 record  90 360 with-partner  $8008B888 route  end-stop
  28 stop  28 15 10 until  4 record  27 1 in-area  346072 312393 place  0 facing  0 pose  73 car-at  $8008B8DC route  end-stop
  29 stop  28 19 0 until  5 record  13 3 in-area  270840 88769 place  4039 facing  72 pose  97 car-at  $8008B990 route  end-stop
  30 stop  28 20 2 until  1 record  diner-seat  34 car-at  $8008B9F0 route  end-stop
  31 stop  28 22 0 until  6 record  club-seat  94 car-at  $8008BA48 route  end-stop
  32 stop  29 8 10 until  0 record  27 1 in-area  468531 411566 place  0 facing  0 pose  78 car-at  $8008B79C route  end-stop
  33 stop  29 9 10 until  1 record  diner-seat  109 car-at  $8008B808 route  end-stop
  34 stop  29 9 40 until  2 record  18 1 in-area  412881 169603 place  512 facing  22 pose  32 car-at  $8008B85C route  end-stop
  35 stop  29 13 10 until  3 record  90 360 with-partner  $8008B888 route  end-stop
  36 stop  29 15 10 until  4 record  27 1 in-area  346072 312393 place  0 facing  0 pose  73 car-at  $8008B8DC route  end-stop
  37 stop  29 19 0 until  5 record  13 3 in-area  270840 88769 place  4039 facing  72 pose  97 car-at  $8008B990 route  end-stop
  38 stop  29 20 2 until  1 record  diner-seat  34 car-at  $8008B9F0 route  end-stop
  39 stop  29 22 0 until  6 record  club-seat  94 car-at  $8008BA48 route  end-stop
  40 stop  30 8 10 until  0 record  27 1 in-area  468531 411566 place  0 facing  0 pose  78 car-at  $8008B79C route  end-stop
  41 stop  30 9 10 until  1 record  diner-seat  109 car-at  $8008B808 route  end-stop
  42 stop  30 9 40 until  2 record  18 1 in-area  412881 169603 place  512 facing  22 pose  32 car-at  $8008B85C route  end-stop
  43 stop  30 13 10 until  3 record  90 360 with-partner  $8008B888 route  end-stop
  44 stop  30 15 10 until  4 record  27 1 in-area  346072 312393 place  0 facing  0 pose  73 car-at  $8008B8DC route  end-stop
  45 stop  30 19 0 until  5 record  13 3 in-area  270840 88769 place  4039 facing  72 pose  97 car-at  $8008B990 route  end-stop
  46 stop  30 20 2 until  1 record  diner-seat  34 car-at  $8008B9F0 route  end-stop
  47 stop  30 22 0 until  6 record  club-seat  94 car-at  $8008BA48 route  end-stop
  48 stop  31 8 10 until  0 record  27 1 in-area  468531 411566 place  0 facing  0 pose  78 car-at  $8008B79C route  end-stop
  49 stop  31 9 10 until  1 record  diner-seat  109 car-at  $8008B808 route  end-stop
  50 stop  31 9 40 until  2 record  18 1 in-area  412881 169603 place  512 facing  22 pose  32 car-at  $8008B85C route  end-stop
  51 stop  31 13 10 until  3 record  90 360 with-partner  $8008B888 route  end-stop
  52 stop  31 15 10 until  4 record  27 1 in-area  346072 312393 place  0 facing  0 pose  73 car-at  $8008B8DC route  end-stop
  53 stop  31 19 0 until  5 record  13 3 in-area  270840 88769 place  4039 facing  72 pose  97 car-at  $8008B990 route  end-stop
  54 stop  31 20 2 until  1 record  diner-seat  34 car-at  $8008B808 route  end-stop
  55 stop  31 22 0 until  2 record  18 1 in-area  412881 169603 place  512 facing  22 pose  32 car-at  $8008B85C route  end-stop
end-schedule

\ ---- slot 70: townsperson 5 ----
70 schedule
   0 stop  25 10 40 until  0 record  27 1 in-area  215150 148718 place  0 facing  0 pose  119 car-at  $8008BA9C route  end-stop
   1 stop  25 12 15 until  1 record  24 1 in-area  592761 131490 place  988 facing  68 pose  9 car-at  $8008BB08 route  end-stop
   2 stop  25 14 30 until  2 record  27 1 in-area  461032 430450 place  4094 facing  22 pose  78 car-at  $8008BB78 route  end-stop
   3 stop  25 16 0 until  3 record  diner-seat  67 car-at  $8008BBC0 route  end-stop
   4 stop  25 18 0 until  4 record  27 1 in-area  357268 263855 place  46 facing  72 pose  120 car-at  $8008BC10 route  end-stop
   5 stop  25 19 0 until  5 record  37 360 with-partner  $8008BC3C route  end-stop
   6 stop  25 21 0 until  6 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BC60 route  end-stop
   7 stop  26 10 40 until  0 record  27 1 in-area  215150 148718 place  0 facing  0 pose  119 car-at  $8008BA9C route  end-stop
   8 stop  26 12 15 until  1 record  24 1 in-area  592761 131490 place  988 facing  68 pose  9 car-at  $8008BB08 route  end-stop
   9 stop  26 14 30 until  2 record  27 1 in-area  461032 430450 place  4094 facing  22 pose  78 car-at  $8008BB78 route  end-stop
  10 stop  26 16 0 until  3 record  diner-seat  67 car-at  $8008BBC0 route  end-stop
  11 stop  26 18 0 until  4 record  27 1 in-area  357268 263855 place  46 facing  72 pose  120 car-at  $8008BC10 route  end-stop
  12 stop  26 19 0 until  5 record  37 360 with-partner  $8008BC3C route  end-stop
  13 stop  26 21 0 until  6 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BC60 route  end-stop
  14 stop  27 10 40 until  0 record  27 1 in-area  215150 148718 place  0 facing  0 pose  119 car-at  $8008BA9C route  end-stop
  15 stop  27 12 15 until  1 record  24 1 in-area  592761 131490 place  988 facing  68 pose  9 car-at  $8008BB08 route  end-stop
  16 stop  27 14 30 until  2 record  27 1 in-area  461032 430450 place  4094 facing  22 pose  78 car-at  $8008BB78 route  end-stop
  17 stop  27 16 0 until  3 record  diner-seat  67 car-at  $8008BBC0 route  end-stop
  18 stop  27 18 0 until  4 record  27 1 in-area  357268 263855 place  46 facing  72 pose  120 car-at  $8008BC10 route  end-stop
  19 stop  27 19 0 until  5 record  37 360 with-partner  $8008BC3C route  end-stop
  20 stop  27 21 0 until  6 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BC60 route  end-stop
  21 stop  28 10 40 until  0 record  27 1 in-area  215150 148718 place  0 facing  0 pose  119 car-at  $8008BA9C route  end-stop
  22 stop  28 12 15 until  1 record  24 1 in-area  592761 131490 place  988 facing  68 pose  9 car-at  $8008BB08 route  end-stop
  23 stop  28 14 30 until  2 record  27 1 in-area  461032 430450 place  4094 facing  22 pose  78 car-at  $8008BB78 route  end-stop
  24 stop  28 16 0 until  3 record  diner-seat  67 car-at  $8008BBC0 route  end-stop
  25 stop  28 18 0 until  4 record  27 1 in-area  357268 263855 place  46 facing  72 pose  120 car-at  $8008BC10 route  end-stop
  26 stop  28 19 0 until  5 record  37 360 with-partner  $8008BC3C route  end-stop
  27 stop  28 21 0 until  6 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BC60 route  end-stop
  28 stop  29 10 40 until  0 record  27 1 in-area  215150 148718 place  0 facing  0 pose  119 car-at  $8008BA9C route  end-stop
  29 stop  29 12 15 until  1 record  24 1 in-area  592761 131490 place  988 facing  68 pose  9 car-at  $8008BB08 route  end-stop
  30 stop  29 14 30 until  2 record  27 1 in-area  461032 430450 place  4094 facing  22 pose  78 car-at  $8008BB78 route  end-stop
  31 stop  29 16 0 until  3 record  diner-seat  67 car-at  $8008BBC0 route  end-stop
  32 stop  29 18 0 until  4 record  27 1 in-area  357268 263855 place  46 facing  72 pose  120 car-at  $8008BC10 route  end-stop
  33 stop  29 19 0 until  5 record  37 360 with-partner  $8008BC3C route  end-stop
  34 stop  29 21 0 until  6 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BC60 route  end-stop
  35 stop  30 10 40 until  0 record  27 1 in-area  215150 148718 place  0 facing  0 pose  119 car-at  $8008BA9C route  end-stop
  36 stop  30 12 15 until  1 record  24 1 in-area  592761 131490 place  988 facing  68 pose  9 car-at  $8008BB08 route  end-stop
  37 stop  30 14 30 until  2 record  27 1 in-area  461032 430450 place  4094 facing  22 pose  78 car-at  $8008BB78 route  end-stop
  38 stop  30 16 0 until  3 record  diner-seat  67 car-at  $8008BBC0 route  end-stop
  39 stop  30 18 0 until  4 record  27 1 in-area  357268 263855 place  46 facing  72 pose  120 car-at  $8008BC10 route  end-stop
  40 stop  30 19 0 until  5 record  37 360 with-partner  $8008BC3C route  end-stop
  41 stop  30 21 0 until  6 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BC60 route  end-stop
  42 stop  31 10 40 until  0 record  27 1 in-area  215150 148718 place  0 facing  0 pose  119 car-at  $8008BA9C route  end-stop
  43 stop  31 12 15 until  1 record  24 1 in-area  592761 131490 place  988 facing  68 pose  9 car-at  $8008BB08 route  end-stop
  44 stop  31 14 30 until  2 record  27 1 in-area  461032 430450 place  4094 facing  22 pose  78 car-at  $8008BB78 route  end-stop
  45 stop  31 16 0 until  3 record  diner-seat  67 car-at  $8008BBC0 route  end-stop
  46 stop  31 18 0 until  4 record  27 1 in-area  357268 263855 place  46 facing  72 pose  120 car-at  $8008BC10 route  end-stop
  47 stop  31 19 0 until  5 record  37 360 with-partner  $8008BC3C route  end-stop
  48 stop  31 21 0 until  6 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BC60 route  end-stop
end-schedule

\ ---- slot 71: townsperson 6 ----
71 schedule
   0 stop  25 10 3 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BCA0 route  end-stop
   1 stop  25 12 30 until  1 record  37 360 with-partner  $8008BCDC route  end-stop
   2 stop  25 14 0 until  2 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BD00 route  end-stop
   3 stop  25 15 0 until  1 record  37 360 with-partner  $8008BD34 route  end-stop
   4 stop  25 16 40 until  3 record  1 1 in-area  345470 288162 place  4083 facing  22 pose  67 car-at  $8008BDD8 route  end-stop
   5 stop  25 19 10 until  4 record  24 1 in-area  593200 133019 place  965 facing  68 pose  33 car-at  $8008BE60 route  end-stop
   6 stop  25 22 0 until  1 record  37 360 with-partner  $8008BEA4 route  end-stop
   7 stop  26 2 0 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BEF0 route  end-stop
   8 stop  26 10 3 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BCA0 route  end-stop
   9 stop  26 12 30 until  1 record  37 360 with-partner  $8008BCDC route  end-stop
  10 stop  26 14 0 until  2 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BD00 route  end-stop
  11 stop  26 15 0 until  1 record  37 360 with-partner  $8008BD34 route  end-stop
  12 stop  26 16 40 until  3 record  1 1 in-area  345470 288162 place  4083 facing  22 pose  67 car-at  $8008BDD8 route  end-stop
  13 stop  26 19 10 until  4 record  24 1 in-area  593200 133019 place  965 facing  68 pose  33 car-at  $8008BE60 route  end-stop
  14 stop  26 22 0 until  1 record  37 360 with-partner  $8008BEA4 route  end-stop
  15 stop  27 2 0 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BEF0 route  end-stop
  16 stop  27 10 3 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BCA0 route  end-stop
  17 stop  27 12 30 until  1 record  37 360 with-partner  $8008BCDC route  end-stop
  18 stop  27 14 0 until  2 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BD00 route  end-stop
  19 stop  27 15 0 until  1 record  37 360 with-partner  $8008BD34 route  end-stop
  20 stop  27 16 40 until  3 record  1 1 in-area  345470 288162 place  4083 facing  22 pose  67 car-at  $8008BDD8 route  end-stop
  21 stop  27 19 10 until  4 record  24 1 in-area  593200 133019 place  965 facing  68 pose  33 car-at  $8008BE60 route  end-stop
  22 stop  27 22 0 until  1 record  37 360 with-partner  $8008BEA4 route  end-stop
  23 stop  28 2 0 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BEF0 route  end-stop
  24 stop  28 10 3 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BCA0 route  end-stop
  25 stop  28 12 30 until  1 record  37 360 with-partner  $8008BCDC route  end-stop
  26 stop  28 14 0 until  2 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BD00 route  end-stop
  27 stop  28 15 0 until  1 record  37 360 with-partner  $8008BD34 route  end-stop
  28 stop  28 16 40 until  3 record  1 1 in-area  345470 288162 place  4083 facing  22 pose  67 car-at  $8008BDD8 route  end-stop
  29 stop  28 19 10 until  4 record  24 1 in-area  593200 133019 place  965 facing  68 pose  33 car-at  $8008BE60 route  end-stop
  30 stop  28 22 0 until  1 record  37 360 with-partner  $8008BEA4 route  end-stop
  31 stop  29 2 0 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BEF0 route  end-stop
  32 stop  29 10 3 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BCA0 route  end-stop
  33 stop  29 12 30 until  1 record  37 360 with-partner  $8008BCDC route  end-stop
  34 stop  29 14 0 until  2 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BD00 route  end-stop
  35 stop  29 15 0 until  1 record  37 360 with-partner  $8008BD34 route  end-stop
  36 stop  29 16 40 until  3 record  1 1 in-area  345470 288162 place  4083 facing  22 pose  67 car-at  $8008BDD8 route  end-stop
  37 stop  29 19 10 until  4 record  24 1 in-area  593200 133019 place  965 facing  68 pose  33 car-at  $8008BE60 route  end-stop
  38 stop  29 22 0 until  1 record  37 360 with-partner  $8008BEA4 route  end-stop
  39 stop  30 2 0 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BEF0 route  end-stop
  40 stop  30 10 3 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BCA0 route  end-stop
  41 stop  30 12 30 until  1 record  37 360 with-partner  $8008BCDC route  end-stop
  42 stop  30 14 0 until  2 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BD00 route  end-stop
  43 stop  30 15 0 until  1 record  37 360 with-partner  $8008BD34 route  end-stop
  44 stop  30 16 40 until  3 record  1 1 in-area  345470 288162 place  4083 facing  22 pose  67 car-at  $8008BDD8 route  end-stop
  45 stop  30 19 10 until  4 record  24 1 in-area  593200 133019 place  965 facing  68 pose  33 car-at  $8008BE60 route  end-stop
  46 stop  30 22 0 until  1 record  37 360 with-partner  $8008BEA4 route  end-stop
  47 stop  31 2 0 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BEF0 route  end-stop
  48 stop  31 10 3 until  0 record  27 1 in-area  190912 404549 place  0 facing  0 pose  46 car-at  $8008BCA0 route  end-stop
  49 stop  31 12 30 until  1 record  37 360 with-partner  $8008BCDC route  end-stop
  50 stop  31 14 0 until  2 record  20 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008BD00 route  end-stop
  51 stop  31 15 0 until  1 record  37 360 with-partner  $8008BD34 route  end-stop
  52 stop  31 16 40 until  3 record  1 1 in-area  345470 288162 place  4083 facing  22 pose  67 car-at  $8008BDD8 route  end-stop
  53 stop  31 19 10 until  4 record  24 1 in-area  593200 133019 place  965 facing  68 pose  33 car-at  $8008BE60 route  end-stop
  54 stop  31 22 0 until  1 record  37 360 with-partner  $8008BEA4 route  end-stop
end-schedule

\ ---- slot 72: townsperson 7 ----
72 schedule
   0 stop  25 10 5 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  71 car-at  $8008BF50 route  end-stop
   1 stop  25 10 45 until  1 record  diner-seat  67 car-at  $8008BFA8 route  end-stop
   2 stop  25 11 10 until  2 record  27 1 in-area  471092 288167 place  3006 facing  0 pose  63 car-at  $8008BFF0 route  end-stop
   3 stop  25 12 0 until  3 record  29 360 with-partner  $8008C02C route  end-stop
   4 stop  26 10 5 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  71 car-at  $8008BF50 route  end-stop
   5 stop  26 10 45 until  1 record  diner-seat  67 car-at  $8008BFA8 route  end-stop
   6 stop  26 11 10 until  2 record  27 1 in-area  471092 288167 place  3006 facing  0 pose  63 car-at  $8008BFF0 route  end-stop
   7 stop  26 12 0 until  3 record  29 360 with-partner  $8008C02C route  end-stop
   8 stop  27 10 5 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  71 car-at  $8008BF50 route  end-stop
   9 stop  27 10 45 until  1 record  diner-seat  67 car-at  $8008BFA8 route  end-stop
  10 stop  27 11 10 until  2 record  27 1 in-area  471092 288167 place  3006 facing  0 pose  63 car-at  $8008BFF0 route  end-stop
  11 stop  27 12 0 until  3 record  29 360 with-partner  $8008C02C route  end-stop
  12 stop  28 10 5 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  71 car-at  $8008BF50 route  end-stop
  13 stop  28 10 45 until  1 record  diner-seat  67 car-at  $8008BFA8 route  end-stop
  14 stop  28 11 10 until  2 record  27 1 in-area  471092 288167 place  3006 facing  0 pose  63 car-at  $8008BFF0 route  end-stop
  15 stop  28 12 0 until  3 record  29 360 with-partner  $8008C02C route  end-stop
  16 stop  29 10 5 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  71 car-at  $8008BF50 route  end-stop
  17 stop  29 10 45 until  1 record  diner-seat  67 car-at  $8008BFA8 route  end-stop
  18 stop  29 11 10 until  2 record  27 1 in-area  471092 288167 place  3006 facing  0 pose  63 car-at  $8008BFF0 route  end-stop
  19 stop  29 12 0 until  3 record  29 360 with-partner  $8008C02C route  end-stop
  20 stop  30 10 5 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  71 car-at  $8008BF50 route  end-stop
  21 stop  30 10 45 until  1 record  diner-seat  67 car-at  $8008BFA8 route  end-stop
  22 stop  30 11 10 until  2 record  27 1 in-area  471092 288167 place  3006 facing  0 pose  63 car-at  $8008BFF0 route  end-stop
  23 stop  30 12 0 until  3 record  29 360 with-partner  $8008C02C route  end-stop
  24 stop  31 10 5 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  71 car-at  $8008BF50 route  end-stop
  25 stop  31 10 45 until  1 record  diner-seat  67 car-at  $8008BFA8 route  end-stop
  26 stop  31 11 10 until  2 record  27 1 in-area  471092 288167 place  3006 facing  0 pose  63 car-at  $8008BFF0 route  end-stop
  27 stop  31 12 0 until  3 record  29 360 with-partner  $8008C02C route  end-stop
end-schedule

\ ---- slot 74: townsperson 9 ----
74 schedule
   0 stop  25 10 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C22C route  end-stop
   1 stop  26 6 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C274 route  end-stop
   2 stop  26 10 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C22C route  end-stop
   3 stop  27 6 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C274 route  end-stop
   4 stop  27 10 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C22C route  end-stop
   5 stop  28 6 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C274 route  end-stop
   6 stop  28 10 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C22C route  end-stop
   7 stop  29 6 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C274 route  end-stop
   8 stop  29 10 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C22C route  end-stop
   9 stop  30 6 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C274 route  end-stop
  10 stop  30 10 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C22C route  end-stop
  11 stop  31 6 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C274 route  end-stop
  12 stop  31 10 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C22C route  end-stop
end-schedule

\ ---- slot 75: townsperson 10 ----
75 schedule
   0 stop  25 10 10 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2AC route  end-stop
   1 stop  26 5 0 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2F4 route  end-stop
   2 stop  26 10 10 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2AC route  end-stop
   3 stop  27 5 0 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2F4 route  end-stop
   4 stop  27 10 10 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2AC route  end-stop
   5 stop  28 5 0 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2F4 route  end-stop
   6 stop  28 10 10 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2AC route  end-stop
   7 stop  29 5 0 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2F4 route  end-stop
   8 stop  29 10 10 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2AC route  end-stop
   9 stop  30 5 0 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2F4 route  end-stop
  10 stop  30 10 10 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2AC route  end-stop
  11 stop  31 5 0 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2F4 route  end-stop
  12 stop  31 10 10 until  0 record  27 1 in-area  322734 287201 place  18 facing  0 pose  $8008C2AC route  end-stop
end-schedule

\ ---- slot 77: townsperson 12 ----
77 schedule
   0 stop  25 10 5 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C32C route  end-stop
   1 stop  25 14 0 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C374 route  end-stop
   2 stop  26 9 5 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C32C route  end-stop
   3 stop  26 14 0 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C374 route  end-stop
   4 stop  27 9 5 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C32C route  end-stop
   5 stop  27 14 0 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C374 route  end-stop
   6 stop  28 9 5 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C32C route  end-stop
   7 stop  28 14 0 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C374 route  end-stop
   8 stop  29 9 5 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C32C route  end-stop
   9 stop  29 14 0 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C374 route  end-stop
  10 stop  30 9 5 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C32C route  end-stop
  11 stop  30 14 0 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C374 route  end-stop
  12 stop  31 9 5 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C32C route  end-stop
  13 stop  31 14 0 until  0 record  27 1 in-area  329037 69996 place  0 facing  0 pose  $8008C374 route  end-stop
end-schedule

\ ---- slot 78: townsperson 13 ----
78 schedule
   0 stop  25 10 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C3BC route  end-stop
   1 stop  25 15 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C414 route  end-stop
   2 stop  26 10 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C3BC route  end-stop
   3 stop  26 15 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C414 route  end-stop
   4 stop  27 10 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C3BC route  end-stop
   5 stop  27 15 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C414 route  end-stop
   6 stop  28 10 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C3BC route  end-stop
   7 stop  28 15 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C414 route  end-stop
   8 stop  29 10 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C3BC route  end-stop
   9 stop  29 15 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C414 route  end-stop
  10 stop  30 10 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C3BC route  end-stop
  11 stop  30 15 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C414 route  end-stop
  12 stop  31 10 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C3BC route  end-stop
  13 stop  31 15 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C414 route  end-stop
end-schedule

\ ---- slot 79: townsperson 14 ----
79 schedule
   0 stop  25 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   1 stop  25 13 15 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   2 stop  25 16 20 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   3 stop  26 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   4 stop  26 13 15 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   5 stop  26 16 20 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   6 stop  27 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   7 stop  27 13 15 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   8 stop  27 16 20 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
   9 stop  28 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  10 stop  28 13 15 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  11 stop  28 16 20 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  12 stop  29 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  13 stop  29 13 15 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  14 stop  29 16 20 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  15 stop  30 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  16 stop  30 13 15 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  17 stop  30 16 20 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  18 stop  31 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  19 stop  31 13 15 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
  20 stop  31 16 20 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008C45C route  end-stop
end-schedule

\ ---- slot 80: townsperson 15 ----
80 schedule
   0 stop  25 10 2 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008C474 route  end-stop
   1 stop  25 13 5 until  1 record  1 1 in-area  345853 288196 place  0 facing  22 pose  119 car-at  $8008C54C route  end-stop
   2 stop  26 7 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008C7C8 route  end-stop
   3 stop  26 20 0 until  2 record  90 460 with-partner  $8008C85C route  end-stop
   4 stop  27 10 2 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008C474 route  end-stop
   5 stop  27 13 5 until  1 record  1 1 in-area  345853 288196 place  0 facing  22 pose  119 car-at  $8008C54C route  end-stop
   6 stop  28 7 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008C7C8 route  end-stop
   7 stop  28 20 0 until  2 record  90 460 with-partner  $8008C85C route  end-stop
   8 stop  29 10 2 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008C474 route  end-stop
   9 stop  29 13 5 until  1 record  1 1 in-area  345853 288196 place  0 facing  22 pose  119 car-at  $8008C54C route  end-stop
  10 stop  30 7 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008C7C8 route  end-stop
  11 stop  30 20 0 until  2 record  90 460 with-partner  $8008C85C route  end-stop
  12 stop  31 10 2 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008C474 route  end-stop
  13 stop  31 13 5 until  1 record  1 1 in-area  345853 288196 place  0 facing  22 pose  119 car-at  $8008C54C route  end-stop
end-schedule

\ ---- slot 81: townsperson 16 ----
81 schedule
   0 stop  25 10 20 until  0 record  27 1 in-area  174927 406586 place  25 facing  0 pose  46 car-at  $8008C8B0 route  end-stop
   1 stop  25 11 45 until  1 record  24 1 in-area  592269 133263 place  1038 facing  68 pose  15 car-at  $8008C91C route  end-stop
   2 stop  25 12 45 until  2 record  18 1 in-area  413668 171310 place  1042 facing  57 pose  32 car-at  $8008C970 route  end-stop
   3 stop  25 14 0 until  3 record  diner-seat  34 car-at  $8008C9C0 route  end-stop
   4 stop  25 15 0 until  4 record  29 360 with-partner  $8008C9F4 route  end-stop
   5 stop  25 15 35 until  5 record  0 1 in-area  249269 134013 place  2090 facing  0 pose  18 car-at  $8008CA3C route  end-stop
   6 stop  25 16 25 until  6 record  1 1 in-area  345079 287719 place  4001 facing  22 pose  76 car-at  $8008CA9C route  end-stop
   7 stop  25 17 10 until  7 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  32 car-at  $8008CAD4 route  end-stop
   8 stop  25 17 50 until  3 record  diner-seat  67 car-at  $8008CB14 route  end-stop
   9 stop  25 18 45 until  8 record  24 1 in-area  593200 133019 place  965 facing  68 pose  36 car-at  $8008CB78 route  end-stop
  10 stop  25 21 0 until  9 record  2 1 in-area  105188 360085 place  1978 facing  72 pose  80 car-at  $8008CBDC route  end-stop
end-schedule

\ ---- slot 82: townsperson 17 ----
82 schedule
   0 stop  25 10 8 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  47 car-at  $8008CC1C route  end-stop
end-schedule

\ ---- slot 83: townsperson 18 ----
83 schedule
   0 stop  25 10 2 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  81 car-at  $8008CC58 route  end-stop
   1 stop  25 11 0 until  1 record  38 360 with-partner  $8008CCA4 route  end-stop
   2 stop  25 13 0 until  2 record  2 1 in-area  105488 360085 place  1978 facing  72 pose  80 car-at  $8008CD04 route  end-stop
   3 stop  25 13 45 until  3 record  7 1 in-area  133657 307711 place  0 facing  0 pose  84 car-at  $8008CD5C route  end-stop
   4 stop  25 15 30 until  4 record  24 1 in-area  593200 133019 place  965 facing  68 pose  33 car-at  $8008CDB0 route  end-stop
   5 stop  25 19 0 until  5 record  23 360 with-partner  $8008CDF4 route  end-stop
   6 stop  25 19 40 until  6 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  32 car-at  $8008CE28 route  end-stop
   7 stop  25 20 20 until  1 record  38 360 with-partner  $8008CE5C route  end-stop
   8 stop  25 21 0 until  7 record  37 360 with-partner  $8008CE84 route  end-stop
end-schedule

\ ---- slot 84: townsperson 19 ----
84 schedule
   0 stop  25 11 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
   1 stop  25 12 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CFB8 route  end-stop
   2 stop  25 12 35 until  1 record  7 1 in-area  125308 312885 place  3072 facing  151 pose  84 car-at  $8008D024 route  end-stop
   3 stop  25 13 30 until  2 record  0 1 in-area  249613 134153 place  2636 facing  0 pose  108 car-at  $8008D074 route  end-stop
   4 stop  25 15 0 until  3 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D0A4 route  end-stop
   5 stop  25 18 20 until  4 record  37 360 with-partner  $8008D0E8 route  end-stop
   6 stop  25 21 15 until  5 record  27 1 in-area  357268 263885 place  46 facing  68 pose  82 car-at  $8008D114 route  end-stop
   7 stop  26 8 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
   8 stop  26 11 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
   9 stop  26 12 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CFB8 route  end-stop
  10 stop  26 12 35 until  1 record  7 1 in-area  125308 312885 place  3072 facing  151 pose  84 car-at  $8008D024 route  end-stop
  11 stop  26 13 30 until  2 record  0 1 in-area  249613 134153 place  2636 facing  0 pose  108 car-at  $8008D074 route  end-stop
  12 stop  26 15 0 until  3 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D0A4 route  end-stop
  13 stop  26 18 20 until  4 record  37 360 with-partner  $8008D0E8 route  end-stop
  14 stop  26 21 15 until  5 record  27 1 in-area  357268 263885 place  46 facing  68 pose  82 car-at  $8008D114 route  end-stop
  15 stop  27 8 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  16 stop  27 11 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  17 stop  27 12 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CFB8 route  end-stop
  18 stop  27 12 35 until  1 record  7 1 in-area  125308 312885 place  3072 facing  151 pose  84 car-at  $8008D024 route  end-stop
  19 stop  27 13 30 until  2 record  0 1 in-area  249613 134153 place  2636 facing  0 pose  108 car-at  $8008D074 route  end-stop
  20 stop  27 15 0 until  3 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D0A4 route  end-stop
  21 stop  27 18 20 until  4 record  37 360 with-partner  $8008D0E8 route  end-stop
  22 stop  27 21 15 until  5 record  27 1 in-area  357268 263885 place  46 facing  68 pose  82 car-at  $8008D114 route  end-stop
  23 stop  28 8 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  24 stop  28 11 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  25 stop  28 12 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CFB8 route  end-stop
  26 stop  28 12 35 until  1 record  7 1 in-area  125308 312885 place  3072 facing  151 pose  84 car-at  $8008D024 route  end-stop
  27 stop  28 13 30 until  2 record  0 1 in-area  249613 134153 place  2636 facing  0 pose  108 car-at  $8008D074 route  end-stop
  28 stop  28 15 0 until  3 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D0A4 route  end-stop
  29 stop  28 18 20 until  4 record  37 360 with-partner  $8008D0E8 route  end-stop
  30 stop  28 21 15 until  5 record  27 1 in-area  357268 263885 place  46 facing  68 pose  82 car-at  $8008D114 route  end-stop
  31 stop  29 8 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  32 stop  29 11 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  33 stop  29 12 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CFB8 route  end-stop
  34 stop  29 12 35 until  1 record  7 1 in-area  125308 312885 place  3072 facing  151 pose  84 car-at  $8008D024 route  end-stop
  35 stop  29 13 30 until  2 record  0 1 in-area  249613 134153 place  2636 facing  0 pose  108 car-at  $8008D074 route  end-stop
  36 stop  29 15 0 until  3 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D0A4 route  end-stop
  37 stop  29 18 20 until  4 record  37 360 with-partner  $8008D0E8 route  end-stop
  38 stop  29 21 15 until  5 record  27 1 in-area  357268 263885 place  46 facing  68 pose  82 car-at  $8008D114 route  end-stop
  39 stop  30 8 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  40 stop  30 11 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  41 stop  30 12 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CFB8 route  end-stop
  42 stop  30 12 35 until  1 record  7 1 in-area  125308 312885 place  3072 facing  151 pose  84 car-at  $8008D024 route  end-stop
  43 stop  30 13 30 until  2 record  0 1 in-area  249613 134153 place  2636 facing  0 pose  108 car-at  $8008D074 route  end-stop
  44 stop  30 15 0 until  3 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D0A4 route  end-stop
  45 stop  30 18 20 until  4 record  37 360 with-partner  $8008D0E8 route  end-stop
  46 stop  30 21 15 until  5 record  27 1 in-area  357268 263885 place  46 facing  68 pose  82 car-at  $8008D114 route  end-stop
  47 stop  31 8 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  48 stop  31 11 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CEC0 route  end-stop
  49 stop  31 12 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  61 car-at  $8008CFB8 route  end-stop
  50 stop  31 12 35 until  1 record  7 1 in-area  125308 312885 place  3072 facing  151 pose  84 car-at  $8008D024 route  end-stop
  51 stop  31 13 30 until  2 record  0 1 in-area  249613 134153 place  2636 facing  0 pose  108 car-at  $8008D074 route  end-stop
  52 stop  21 15 0 until  3 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D0A4 route  end-stop
  53 stop  31 18 20 until  4 record  37 360 with-partner  $8008D0E8 route  end-stop
  54 stop  31 21 15 until  5 record  27 1 in-area  357268 263885 place  46 facing  68 pose  82 car-at  $8008D114 route  end-stop
end-schedule

\ ---- slot 86: townsperson 21 ----
86 schedule
   0 stop  25 10 15 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D14C route  end-stop
   1 stop  25 15 0 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D1B0 route  end-stop
   2 stop  26 10 15 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D14C route  end-stop
   3 stop  26 15 0 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D1B0 route  end-stop
   4 stop  27 10 15 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D14C route  end-stop
   5 stop  27 15 0 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D1B0 route  end-stop
   6 stop  28 10 15 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D14C route  end-stop
   7 stop  28 15 0 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D1B0 route  end-stop
   8 stop  29 10 15 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D14C route  end-stop
   9 stop  29 15 0 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D1B0 route  end-stop
  10 stop  30 10 15 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D14C route  end-stop
  11 stop  30 15 0 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D1B0 route  end-stop
  12 stop  31 10 15 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D14C route  end-stop
  13 stop  31 15 0 until  0 record  27 1 in-area  376371 69697 place  1052 facing  0 pose  $8008D1B0 route  end-stop
end-schedule

\ ---- slot 87: townsperson 22 ----
87 schedule
   0 stop  25 10 3 until  0 record  27 1 in-area  344155 245296 place  0 facing  0 pose  $8008D220 route  end-stop
   1 stop  25 15 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D258 route  end-stop
   2 stop  26 5 20 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2A0 route  end-stop
   3 stop  26 8 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2D8 route  end-stop
   4 stop  26 10 3 until  0 record  27 1 in-area  344155 245296 place  0 facing  0 pose  $8008D220 route  end-stop
   5 stop  26 15 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D258 route  end-stop
   6 stop  27 5 20 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2A0 route  end-stop
   7 stop  27 8 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2D8 route  end-stop
   8 stop  27 10 3 until  0 record  27 1 in-area  344155 245296 place  0 facing  0 pose  $8008D220 route  end-stop
   9 stop  27 15 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D258 route  end-stop
  10 stop  28 5 20 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2A0 route  end-stop
  11 stop  28 8 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2D8 route  end-stop
  12 stop  28 10 3 until  0 record  27 1 in-area  344155 245296 place  0 facing  0 pose  $8008D220 route  end-stop
  13 stop  28 15 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D258 route  end-stop
  14 stop  29 5 20 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2A0 route  end-stop
  15 stop  29 8 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2D8 route  end-stop
  16 stop  29 10 3 until  0 record  27 1 in-area  344155 245296 place  0 facing  0 pose  $8008D220 route  end-stop
  17 stop  29 15 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D258 route  end-stop
  18 stop  30 5 20 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2A0 route  end-stop
  19 stop  30 8 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2D8 route  end-stop
  20 stop  30 10 3 until  0 record  27 1 in-area  344155 245296 place  0 facing  0 pose  $8008D220 route  end-stop
  21 stop  30 15 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D258 route  end-stop
  22 stop  31 5 20 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2A0 route  end-stop
  23 stop  31 8 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D2D8 route  end-stop
  24 stop  31 10 3 until  0 record  27 1 in-area  344155 245296 place  0 facing  0 pose  $8008D220 route  end-stop
  25 stop  31 15 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D258 route  end-stop
end-schedule

\ ---- slot 113: townsperson 24 ----
113 schedule
   0 stop  25 11 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008D308 route  end-stop
   1 stop  25 14 10 until  1 record  37 360 with-partner  $8008D344 route  end-stop
   2 stop  25 15 30 until  2 record  diner-seat  67 car-at  $8008D378 route  end-stop
   3 stop  25 18 0 until  3 record  90 360 with-partner  $8008D3D4 route  end-stop
   4 stop  25 20 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D408 route  end-stop
   5 stop  25 21 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008D438 route  end-stop
   6 stop  26 8 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008D308 route  end-stop
   7 stop  26 11 10 until  1 record  37 360 with-partner  $8008D344 route  end-stop
   8 stop  26 12 30 until  2 record  diner-seat  67 car-at  $8008D378 route  end-stop
   9 stop  26 15 0 until  3 record  90 360 with-partner  $8008D3D4 route  end-stop
  10 stop  26 17 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D408 route  end-stop
  11 stop  26 18 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008D438 route  end-stop
  12 stop  27 8 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008D308 route  end-stop
  13 stop  27 11 10 until  1 record  37 360 with-partner  $8008D344 route  end-stop
  14 stop  27 12 30 until  2 record  diner-seat  67 car-at  $8008D378 route  end-stop
  15 stop  27 15 0 until  3 record  90 360 with-partner  $8008D3D4 route  end-stop
  16 stop  27 17 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D408 route  end-stop
  17 stop  27 18 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008D438 route  end-stop
  18 stop  28 8 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008D308 route  end-stop
  19 stop  28 11 10 until  1 record  37 360 with-partner  $8008D344 route  end-stop
  20 stop  28 12 30 until  2 record  diner-seat  67 car-at  $8008D378 route  end-stop
  21 stop  28 15 0 until  3 record  90 360 with-partner  $8008D3D4 route  end-stop
  22 stop  28 17 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D408 route  end-stop
  23 stop  28 18 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008D438 route  end-stop
  24 stop  29 8 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008D308 route  end-stop
  25 stop  29 11 10 until  1 record  37 360 with-partner  $8008D344 route  end-stop
  26 stop  29 12 30 until  2 record  diner-seat  67 car-at  $8008D378 route  end-stop
  27 stop  29 15 0 until  3 record  90 360 with-partner  $8008D3D4 route  end-stop
  28 stop  29 17 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D408 route  end-stop
  29 stop  29 18 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008D438 route  end-stop
  30 stop  30 8 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008D308 route  end-stop
  31 stop  30 11 10 until  1 record  37 360 with-partner  $8008D344 route  end-stop
  32 stop  30 12 30 until  2 record  diner-seat  67 car-at  $8008D378 route  end-stop
  33 stop  30 15 0 until  3 record  90 360 with-partner  $8008D3D4 route  end-stop
  34 stop  30 17 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D408 route  end-stop
  35 stop  30 18 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008D438 route  end-stop
  36 stop  31 8 2 until  0 record  27 1 in-area  356732 63919 place  1010 facing  0 pose  69 car-at  $8008D308 route  end-stop
  37 stop  31 11 10 until  1 record  37 360 with-partner  $8008D344 route  end-stop
  38 stop  31 12 30 until  2 record  diner-seat  67 car-at  $8008D378 route  end-stop
  39 stop  31 15 0 until  3 record  90 360 with-partner  $8008D3D4 route  end-stop
  40 stop  31 17 40 until  4 record  27 1 in-area  1 1 place  1 facing  0 pose  66 car-at  $8008D408 route  end-stop
  41 stop  31 18 40 until  5 record  27 1 in-area  584127 118075 place  855 facing  0 pose  33 car-at  $8008D438 route  end-stop
end-schedule

\ ---- slot 114: townsperson 25 ----
114 schedule
   0 stop  25 12 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D488 route  end-stop
   1 stop  25 17 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D4E0 route  end-stop
   2 stop  26 8 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D488 route  end-stop
   3 stop  26 13 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D4E0 route  end-stop
   4 stop  27 12 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D488 route  end-stop
   5 stop  27 17 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D4E0 route  end-stop
   6 stop  28 8 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D488 route  end-stop
   7 stop  28 13 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D4E0 route  end-stop
   8 stop  29 12 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D488 route  end-stop
   9 stop  29 17 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D4E0 route  end-stop
  10 stop  30 8 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D488 route  end-stop
  11 stop  30 13 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D4E0 route  end-stop
  12 stop  31 12 5 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D488 route  end-stop
  13 stop  31 17 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D4E0 route  end-stop
end-schedule

\ ---- slot 116: townsperson 27 ----
116 schedule
   0 stop  25 10 8 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008D528 route  end-stop
   1 stop  25 14 0 until  1 record  38 360 with-partner  $8008D598 route  end-stop
   2 stop  25 19 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  81 car-at  $8008D5F0 route  end-stop
   3 stop  26 10 8 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008D528 route  end-stop
   4 stop  26 14 0 until  1 record  38 360 with-partner  $8008D598 route  end-stop
   5 stop  26 19 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  81 car-at  $8008D5F0 route  end-stop
   6 stop  27 10 8 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008D528 route  end-stop
   7 stop  27 14 0 until  1 record  38 360 with-partner  $8008D598 route  end-stop
   8 stop  27 19 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  81 car-at  $8008D5F0 route  end-stop
   9 stop  28 10 8 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008D528 route  end-stop
  10 stop  28 14 0 until  1 record  38 360 with-partner  $8008D598 route  end-stop
  11 stop  28 19 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  81 car-at  $8008D5F0 route  end-stop
  12 stop  29 10 8 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008D528 route  end-stop
  13 stop  29 14 0 until  1 record  38 360 with-partner  $8008D598 route  end-stop
  14 stop  29 19 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  81 car-at  $8008D5F0 route  end-stop
  15 stop  30 10 8 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008D528 route  end-stop
  16 stop  30 14 0 until  1 record  38 360 with-partner  $8008D598 route  end-stop
  17 stop  30 19 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  81 car-at  $8008D5F0 route  end-stop
  18 stop  31 10 8 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  119 car-at  $8008D528 route  end-stop
  19 stop  31 14 0 until  1 record  38 360 with-partner  $8008D598 route  end-stop
  20 stop  31 19 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  81 car-at  $8008D5F0 route  end-stop
end-schedule

\ ---- slot 117: townsperson 28 ----
117 schedule
   0 stop  25 10 21 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D650 route  end-stop
   1 stop  25 12 45 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D6B0 route  end-stop
   2 stop  25 14 0 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D6E0 route  end-stop
   3 stop  25 15 10 until  2 record  0 1 in-area  247881 135199 place  1034 facing  72 pose  $8008D720 route  end-stop
   4 stop  25 18 30 until  3 record  diner-seat  $8008D77C route  end-stop
   5 stop  26 6 50 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D7DC route  end-stop
   6 stop  26 10 21 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D650 route  end-stop
   7 stop  26 12 45 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D6B0 route  end-stop
   8 stop  26 14 0 until  2 record  0 1 in-area  247881 135199 place  1034 facing  72 pose  $8008D6E0 route  end-stop
   9 stop  26 15 10 until  3 record  diner-seat  $8008D720 route  end-stop
  10 stop  26 18 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D77C route  end-stop
  11 stop  27 6 50 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D7DC route  end-stop
  12 stop  27 10 21 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D650 route  end-stop
  13 stop  27 12 45 until  2 record  0 1 in-area  247881 135199 place  1034 facing  72 pose  $8008D6B0 route  end-stop
  14 stop  27 14 0 until  3 record  diner-seat  $8008D6E0 route  end-stop
  15 stop  27 15 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D720 route  end-stop
  16 stop  27 18 30 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D77C route  end-stop
  17 stop  28 6 50 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D7DC route  end-stop
  18 stop  28 10 21 until  2 record  0 1 in-area  247881 135199 place  1034 facing  72 pose  $8008D650 route  end-stop
  19 stop  28 12 45 until  3 record  diner-seat  $8008D6B0 route  end-stop
  20 stop  28 14 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D6E0 route  end-stop
  21 stop  28 15 10 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D720 route  end-stop
  22 stop  28 18 30 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D77C route  end-stop
  23 stop  29 6 50 until  2 record  0 1 in-area  247881 135199 place  1034 facing  72 pose  $8008D7DC route  end-stop
  24 stop  29 10 21 until  3 record  diner-seat  $8008D650 route  end-stop
  25 stop  29 12 45 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D6B0 route  end-stop
  26 stop  29 14 0 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D6E0 route  end-stop
  27 stop  29 15 10 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D720 route  end-stop
  28 stop  29 18 30 until  2 record  0 1 in-area  247881 135199 place  1034 facing  72 pose  $8008D77C route  end-stop
  29 stop  30 6 50 until  3 record  diner-seat  $8008D7DC route  end-stop
  30 stop  30 10 21 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D650 route  end-stop
  31 stop  30 12 45 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D6B0 route  end-stop
  32 stop  30 14 0 until  1 record  27 1 in-area  279154 97986 place  939 facing  0 pose  $8008D6E0 route  end-stop
  33 stop  30 15 10 until  2 record  0 1 in-area  247881 135199 place  1034 facing  72 pose  $8008D720 route  end-stop
  34 stop  30 18 30 until  3 record  diner-seat  $8008D77C route  end-stop
  35 stop  31 6 50 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D7DC route  end-stop
  36 stop  31 10 21 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D650 route  end-stop
  37 stop  31 12 45 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D6B0 route  end-stop
  38 stop  31 14 0 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D6E0 route  end-stop
  39 stop  31 15 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D720 route  end-stop
  40 stop  31 18 30 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008D77C route  end-stop
end-schedule

\ ---- slot 119: townsperson 30 ----
119 schedule
   0 stop  25 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  167 car-at  $8008D814 route  end-stop
   1 stop  25 11 0 until  1 record  24 1 in-area  592220 133316 place  1046 facing  68 pose  15 car-at  $8008D880 route  end-stop
   2 stop  25 12 13 until  2 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  6 car-at  $8008D8C0 route  end-stop
   3 stop  25 14 5 until  3 record  27 1 in-area  357268 263885 place  46 facing  68 pose  79 car-at  $8008D8F0 route  end-stop
   4 stop  25 15 20 until  4 record  2 1 in-area  103642 359143 place  3890 facing  89 pose  4 car-at  $8008D938 route  end-stop
   5 stop  25 15 45 until  5 record  29 2 in-area  101194 356100 place  3072 facing  89 pose  4 car-at  $8008D960 route  end-stop
   6 stop  25 17 0 until  6 record  13 3 in-area  269236 93182 place  3072 facing  22 pose  99 car-at  $8008D9A8 route  end-stop
   7 stop  25 17 45 until  7 record  0 1 in-area  249387 134433 place  2224 facing  22 pose  3 car-at  $8008D9E8 route  end-stop
   8 stop  26 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  167 car-at  $8008D814 route  end-stop
   9 stop  26 11 0 until  1 record  24 1 in-area  592220 133316 place  1046 facing  68 pose  15 car-at  $8008D880 route  end-stop
  10 stop  26 12 13 until  2 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  6 car-at  $8008D8C0 route  end-stop
  11 stop  26 14 5 until  3 record  27 1 in-area  357268 263885 place  46 facing  68 pose  79 car-at  $8008D8F0 route  end-stop
  12 stop  26 15 20 until  4 record  2 1 in-area  103642 359143 place  3890 facing  89 pose  4 car-at  $8008D938 route  end-stop
  13 stop  26 15 45 until  5 record  29 2 in-area  101194 356100 place  3072 facing  89 pose  4 car-at  $8008D960 route  end-stop
  14 stop  26 17 0 until  6 record  13 3 in-area  269236 93182 place  3072 facing  22 pose  99 car-at  $8008D9A8 route  end-stop
  15 stop  26 17 45 until  7 record  0 1 in-area  249387 134433 place  2224 facing  22 pose  3 car-at  $8008D9E8 route  end-stop
  16 stop  27 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  167 car-at  $8008D814 route  end-stop
  17 stop  27 11 0 until  1 record  24 1 in-area  592220 133316 place  1046 facing  68 pose  15 car-at  $8008D880 route  end-stop
  18 stop  27 12 13 until  2 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  6 car-at  $8008D8C0 route  end-stop
  19 stop  27 14 5 until  3 record  27 1 in-area  357268 263885 place  46 facing  68 pose  79 car-at  $8008D8F0 route  end-stop
  20 stop  27 15 20 until  4 record  2 1 in-area  103642 359143 place  3890 facing  89 pose  4 car-at  $8008D938 route  end-stop
  21 stop  27 15 45 until  5 record  29 2 in-area  101194 356100 place  3072 facing  89 pose  4 car-at  $8008D960 route  end-stop
  22 stop  27 17 0 until  6 record  13 3 in-area  269236 93182 place  3072 facing  22 pose  99 car-at  $8008D9A8 route  end-stop
  23 stop  27 17 45 until  7 record  0 1 in-area  249387 134433 place  2224 facing  22 pose  3 car-at  $8008D9E8 route  end-stop
  24 stop  28 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  167 car-at  $8008D814 route  end-stop
  25 stop  28 11 0 until  1 record  24 1 in-area  592220 133316 place  1046 facing  68 pose  15 car-at  $8008D880 route  end-stop
  26 stop  28 12 13 until  2 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  6 car-at  $8008D8C0 route  end-stop
  27 stop  28 14 5 until  3 record  27 1 in-area  357268 263885 place  46 facing  68 pose  79 car-at  $8008D8F0 route  end-stop
  28 stop  28 15 20 until  4 record  2 1 in-area  103642 359143 place  3890 facing  89 pose  4 car-at  $8008D938 route  end-stop
  29 stop  28 15 45 until  5 record  29 2 in-area  101194 356100 place  3072 facing  89 pose  4 car-at  $8008D960 route  end-stop
  30 stop  28 17 0 until  6 record  13 3 in-area  269236 93182 place  3072 facing  22 pose  99 car-at  $8008D9A8 route  end-stop
  31 stop  28 17 45 until  7 record  0 1 in-area  249387 134433 place  2224 facing  22 pose  3 car-at  $8008D9E8 route  end-stop
  32 stop  29 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  167 car-at  $8008D814 route  end-stop
  33 stop  29 11 0 until  1 record  24 1 in-area  592220 133316 place  1046 facing  68 pose  15 car-at  $8008D880 route  end-stop
  34 stop  29 12 13 until  2 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  6 car-at  $8008D8C0 route  end-stop
  35 stop  29 14 5 until  3 record  27 1 in-area  357268 263885 place  46 facing  68 pose  79 car-at  $8008D8F0 route  end-stop
  36 stop  29 15 20 until  4 record  2 1 in-area  103642 359143 place  3890 facing  89 pose  4 car-at  $8008D938 route  end-stop
  37 stop  29 15 45 until  5 record  29 2 in-area  101194 356100 place  3072 facing  89 pose  4 car-at  $8008D960 route  end-stop
  38 stop  29 17 0 until  6 record  13 3 in-area  269236 93182 place  3072 facing  22 pose  99 car-at  $8008D9A8 route  end-stop
  39 stop  29 17 45 until  7 record  0 1 in-area  249387 134433 place  2224 facing  22 pose  3 car-at  $8008D9E8 route  end-stop
  40 stop  30 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  167 car-at  $8008D814 route  end-stop
  41 stop  30 11 0 until  1 record  24 1 in-area  592220 133316 place  1046 facing  68 pose  15 car-at  $8008D880 route  end-stop
  42 stop  30 12 13 until  2 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  6 car-at  $8008D8C0 route  end-stop
  43 stop  30 14 5 until  3 record  27 1 in-area  357268 263885 place  46 facing  68 pose  79 car-at  $8008D8F0 route  end-stop
  44 stop  30 15 20 until  4 record  2 1 in-area  103642 359143 place  3890 facing  89 pose  4 car-at  $8008D938 route  end-stop
  45 stop  30 15 45 until  5 record  29 2 in-area  101194 356100 place  3072 facing  89 pose  4 car-at  $8008D960 route  end-stop
  46 stop  30 17 0 until  6 record  13 3 in-area  269236 93182 place  3072 facing  22 pose  99 car-at  $8008D9A8 route  end-stop
  47 stop  30 17 45 until  7 record  0 1 in-area  249387 134433 place  2224 facing  22 pose  3 car-at  $8008D9E8 route  end-stop
  48 stop  31 10 10 until  0 record  27 1 in-area  1 1 place  1 facing  0 pose  167 car-at  $8008D814 route  end-stop
  49 stop  31 11 0 until  1 record  24 1 in-area  592220 133316 place  1046 facing  68 pose  15 car-at  $8008D880 route  end-stop
  50 stop  31 12 13 until  2 record  27 1 in-area  409010 169192 place  3167 facing  22 pose  6 car-at  $8008D8C0 route  end-stop
  51 stop  31 14 5 until  3 record  27 1 in-area  357268 263885 place  46 facing  68 pose  79 car-at  $8008D8F0 route  end-stop
  52 stop  31 15 20 until  4 record  2 1 in-area  103642 359143 place  3890 facing  89 pose  4 car-at  $8008D938 route  end-stop
  53 stop  31 15 45 until  5 record  29 2 in-area  101194 356100 place  3072 facing  89 pose  4 car-at  $8008D960 route  end-stop
  54 stop  31 17 0 until  6 record  13 3 in-area  269236 93182 place  3072 facing  22 pose  99 car-at  $8008D9A8 route  end-stop
  55 stop  31 17 45 until  7 record  0 1 in-area  249387 134433 place  2224 facing  22 pose  3 car-at  $8008D9E8 route  end-stop
end-schedule

\ ---- slot 120: townsperson 31 ----
120 schedule
   0 stop  25 10 3 until  0 record  27 1 in-area  345197 125352 place  0 facing  0 pose  $8008DA20 route  end-stop
   1 stop  26 5 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DA70 route  end-stop
   2 stop  26 10 3 until  0 record  27 1 in-area  345197 125352 place  0 facing  0 pose  $8008DA20 route  end-stop
   3 stop  27 5 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DA70 route  end-stop
   4 stop  27 10 3 until  0 record  27 1 in-area  345197 125352 place  0 facing  0 pose  $8008DA20 route  end-stop
   5 stop  28 5 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DA70 route  end-stop
   6 stop  28 10 3 until  0 record  27 1 in-area  345197 125352 place  0 facing  0 pose  $8008DA20 route  end-stop
   7 stop  29 5 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DA70 route  end-stop
   8 stop  29 10 3 until  0 record  27 1 in-area  345197 125352 place  0 facing  0 pose  $8008DA20 route  end-stop
   9 stop  30 5 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DA70 route  end-stop
  10 stop  30 10 3 until  0 record  27 1 in-area  345197 125352 place  0 facing  0 pose  $8008DA20 route  end-stop
  11 stop  31 5 50 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DA70 route  end-stop
  12 stop  31 10 3 until  0 record  27 1 in-area  345197 125352 place  0 facing  0 pose  $8008DA20 route  end-stop
end-schedule

\ ---- slot 123: townsperson 34 ----
123 schedule
   0 stop  25 10 3 until  0 record  27 1 in-area  305238 314163 place  0 facing  0 pose  $8008DAC0 route  end-stop
   1 stop  25 14 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DAF8 route  end-stop
   2 stop  26 6 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB40 route  end-stop
   3 stop  26 9 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB88 route  end-stop
   4 stop  26 10 3 until  0 record  27 1 in-area  305238 314163 place  0 facing  0 pose  $8008DAC0 route  end-stop
   5 stop  26 14 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DAF8 route  end-stop
   6 stop  27 6 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB40 route  end-stop
   7 stop  27 9 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB88 route  end-stop
   8 stop  27 10 3 until  0 record  27 1 in-area  305238 314163 place  0 facing  0 pose  $8008DAC0 route  end-stop
   9 stop  27 14 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DAF8 route  end-stop
  10 stop  28 6 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB40 route  end-stop
  11 stop  28 9 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB88 route  end-stop
  12 stop  28 10 3 until  0 record  27 1 in-area  305238 314163 place  0 facing  0 pose  $8008DAC0 route  end-stop
  13 stop  28 14 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DAF8 route  end-stop
  14 stop  29 6 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB40 route  end-stop
  15 stop  29 9 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB88 route  end-stop
  16 stop  29 10 3 until  0 record  27 1 in-area  305238 314163 place  0 facing  0 pose  $8008DAC0 route  end-stop
  17 stop  29 14 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DAF8 route  end-stop
  18 stop  30 6 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB40 route  end-stop
  19 stop  30 9 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB88 route  end-stop
  20 stop  30 10 3 until  0 record  27 1 in-area  305238 314163 place  0 facing  0 pose  $8008DAC0 route  end-stop
  21 stop  30 14 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DAF8 route  end-stop
  22 stop  31 6 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB40 route  end-stop
  23 stop  31 9 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DB88 route  end-stop
  24 stop  31 10 3 until  0 record  27 1 in-area  305238 314163 place  0 facing  0 pose  $8008DAC0 route  end-stop
  25 stop  31 14 0 until  1 record  27 1 in-area  1 1 place  1 facing  0 pose  $8008DAF8 route  end-stop
end-schedule

\ ---- slot 17: Mrs. Roland (her handler's own rows: the placements by stop - src: actor_handler_17, D_80090D68,
\ D_80090D10; added by hand) - her route on the 26th brings her to stop 2, which lasts ----
17 schedule
   0 stop  26 6 0 until  0 record  3 1 in-area  178386 423087 place  3096 facing  161 pose  $80090D60 route  end-stop
   1 stop  26 11 0 until  1 record  3 1 in-area  178386 423087 place  3096 facing  161 pose  $80090D60 route  end-stop
   2 stop  31 23 59 until  2 record  3 1 in-area  178386 423087 place  3096 facing  161 pose  end-stop
end-schedule
