\ checks.fs - every area's look and act tables (docs/subsystems/look.md): converted once from the
\ executable's and the overlays' tables by tools/checks2forth.py; edited by hand from here on.
\ A row: its area and table, its zone (zone tables: the index on the floor; check tables: the id),
\ for checks the heading (4096 a turn) and degrees either side, its target, its hint's text.
IN: events.checks
USING: events events.all look look.rows ;   \ (look.rows last: its row words win)

\ ---- area 0 ----
0 look-zones row  3 zone  ask-area  -1 hint  end-row
0 look-zones row  6 zone  ask-area  end-row
0 look-zones row  5 zone  ask-area  end-row
0 look-zones row  0 zone  ask-area  end-row
0 look-zones row  4 zone  script events.ovl000:entry-80107E34  3 hint  end-row
0 look-checks row  5 zone  0 facing  80 either-side  script events.ovl000:entry-801079FC  end-row   \ 0 degrees
0 look-checks row  6 zone  0 facing  80 either-side  script events.ovl000:entry-80107A38  end-row   \ 0 degrees
0 look-checks row  3 zone  0 facing  80 either-side  script events.ovl000:entry-80107A74  end-row   \ 0 degrees
0 look-checks row  7 zone  0 facing  80 either-side  script events.ovl000:entry-80107AC4  end-row   \ 0 degrees
0 look-checks row  129 zone  1024 facing  80 either-side  0 ovl $80103498 native-at  end-row   \ 90 degrees
0 look-checks row  130 zone  3072 facing  80 either-side  script events.ovl000:entry-80107B34  end-row   \ 270 degrees
0 look-checks row  8 zone  0 facing  80 either-side  script events.ovl000:entry-80107B68  end-row   \ 0 degrees
0 look-checks row  9 zone  2048 facing  80 either-side  ask-area  end-row   \ 180 degrees
0 look-checks row  10 zone  2048 facing  80 either-side  ask-area  end-row   \ 180 degrees
0 look-checks row  11 zone  2048 facing  80 either-side  script events.ovl000:entry-80107C1C  end-row   \ 180 degrees
0 look-checks row  12 zone  3072 facing  80 either-side  script events.ovl000:entry-80107C58  end-row   \ 270 degrees
0 look-checks row  13 zone  1024 facing  80 either-side  script events.ovl000:entry-80107C8C  end-row   \ 90 degrees
0 look-checks row  2 zone  2048 facing  80 either-side  0 ovl $80103710 native-at  end-row   \ 180 degrees
0 look-checks row  128 zone  2048 facing  80 either-side  script events.ovl000:entry-80105320  16 hint  end-row   \ 180 degrees
0 look-checks row  15 zone  2048 facing  80 either-side  script events.ovl000:entry-801051B0  17 hint  end-row   \ 180 degrees
0 look-checks row  18 zone  0 facing  80 either-side  script events.ovl000:entry-80107E44  3 hint  end-row   \ 0 degrees
0 look-checks row  4 zone  0 facing  80 either-side  script events.ovl000:entry-80107970  end-row   \ 0 degrees
0 look-checks row  1 zone  2048 facing  80 either-side  main-exe $80047BD8 native-at  3 hint  end-row   \ 180 degrees
0 look-checks row  131 zone  2048 facing  80 either-side  ask-area  -1 hint  end-row   \ 180 degrees
0 look-checks row  17 zone  3218 facing  100 either-side  0 ovl $80103FEC native-at  3 hint  end-row   \ 282 degrees
0 act-checks row  9 zone  2048 facing  80 either-side  ask-area  end-row   \ 180 degrees

\ ---- area 1 ----
1 look-zones row  10 zone  ask-area  -1 hint  end-row
1 look-zones row  24 zone  ask-area  -1 hint  end-row
1 look-zones row  1 zone  ask-area  -1 hint  end-row
1 look-zones row  24 zone  ask-area  -1 hint  end-row
1 look-zones row  8 zone  ask-area  -1 hint  end-row
1 look-zones row  3 zone  ask-area  -1 hint  end-row
1 look-zones row  21 zone  ask-area  -1 hint  end-row
1 look-checks row  7 zone  2048 facing  80 either-side  script events.ovl001:entry-80104FA0  end-row   \ 180 degrees
1 look-checks row  10 zone  1024 facing  80 either-side  script events.ovl001:entry-80104FDC  end-row   \ 90 degrees
1 look-checks row  9 zone  3072 facing  80 either-side  script events.ovl001:entry-80105018  end-row   \ 270 degrees
1 look-checks row  8 zone  0 facing  80 either-side  script events.ovl001:entry-80105054  end-row   \ 0 degrees
1 look-checks row  11 zone  3072 facing  80 either-side  1 ovl $80103204 native-at  end-row   \ 270 degrees
1 look-checks row  12 zone  1024 facing  80 either-side  1 ovl $80103328 native-at  end-row   \ 90 degrees
1 look-checks row  15 zone  2560 facing  80 either-side  script events.ovl001:entry-80105124  end-row   \ 225 degrees
1 look-checks row  21 zone  1024 facing  80 either-side  script events.ovl001:entry-801057AC  15 hint  end-row   \ 90 degrees
1 look-checks row  22 zone  1024 facing  80 either-side  script events.ovl001:entry-80105778  11 hint  end-row   \ 90 degrees
1 look-checks row  13 zone  1024 facing  80 either-side  ask-area  -1 hint  end-row   \ 90 degrees
1 look-checks row  18 zone  0 facing  80 either-side  ask-area  -1 hint  end-row   \ 0 degrees
1 look-checks row  19 zone  1024 facing  80 either-side  ask-area  -1 hint  end-row   \ 90 degrees
1 look-checks row  20 zone  1024 facing  80 either-side  ask-area  -1 hint  end-row   \ 90 degrees
1 look-checks row  16 zone  1024 facing  80 either-side  ask-area  -1 hint  end-row   \ 90 degrees
1 look-checks row  17 zone  3072 facing  80 either-side  ask-area  -1 hint  end-row   \ 270 degrees
1 look-checks row  23 zone  1024 facing  80 either-side  ask-area  14 hint  end-row   \ 90 degrees
1 look-checks row  25 zone  1024 facing  80 either-side  ask-area  -1 hint  end-row   \ 90 degrees
1 act-checks row  23 zone  1024 facing  60 either-side  ask-area  14 hint  end-row   \ 90 degrees
1 act-checks row  17 zone  3072 facing  60 either-side  ask-area  end-row   \ 270 degrees
1 act-checks row  16 zone  1024 facing  60 either-side  ask-area  end-row   \ 90 degrees

\ ---- area 2 ----
2 look-checks row  2 zone  3072 facing  60 either-side  script events.ovl002:entry-8010653C  end-row   \ 270 degrees
2 look-checks row  3 zone  1024 facing  60 either-side  script events.ovl002:entry-80106578  end-row   \ 90 degrees
2 look-checks row  4 zone  1024 facing  60 either-side  ask-area  end-row   \ 90 degrees
2 look-checks row  5 zone  3072 facing  60 either-side  script events.ovl002:entry-801065F0  end-row   \ 270 degrees
2 look-checks row  6 zone  0 facing  60 either-side  script events.ovl002:entry-8010662C  end-row   \ 0 degrees
2 look-checks row  7 zone  2048 facing  60 either-side  script events.ovl002:entry-80106668  end-row   \ 180 degrees
2 look-checks row  65 zone  0 facing  60 either-side  script events.ovl002:entry-801066A4  end-row   \ 0 degrees
2 look-checks row  66 zone  2048 facing  60 either-side  script events.ovl002:entry-801066E0  end-row   \ 180 degrees
2 look-checks row  68 zone  1024 facing  60 either-side  script events.ovl002:entry-80106758  end-row   \ 90 degrees
2 look-checks row  70 zone  3072 facing  60 either-side  script events.ovl002:entry-801067D0  end-row   \ 270 degrees
2 look-checks row  72 zone  3072 facing  60 either-side  script events.ovl002:entry-80106848  end-row   \ 270 degrees
2 look-checks row  74 zone  1024 facing  60 either-side  script events.ovl002:entry-801068C4  end-row   \ 90 degrees
2 look-checks row  194 zone  2048 facing  60 either-side  script events.ovl002:entry-8010692C  end-row   \ 180 degrees
2 look-checks row  8 zone  2048 facing  80 either-side  script events.ovl002:entry-80107310  22 hint  end-row   \ 180 degrees
2 look-checks row  64 zone  2048 facing  80 either-side  script events.ovl002:entry-801073EC  22 hint  end-row   \ 180 degrees
2 look-checks row  195 zone  2048 facing  80 either-side  script events.ovl002:entry-801074C8  22 hint  end-row   \ 180 degrees
2 look-checks row  76 zone  0 facing  60 either-side  script events.ovl002:entry-801075C0  3 hint  end-row   \ 0 degrees
2 look-checks row  9 zone  2048 facing  60 either-side  2 ovl $80104188 native-at  16 hint  end-row   \ 180 degrees
2 look-checks row  75 zone  2048 facing  60 either-side  script events.ovl002:entry-80106BA0  17 hint  end-row   \ 180 degrees
2 look-checks row  192 zone  2048 facing  60 either-side  script events.ovl002:entry-80106D1C  16 hint  end-row   \ 180 degrees
2 look-checks row  10 zone  2048 facing  60 either-side  2 ovl $80104584 native-at  17 hint  end-row   \ 180 degrees
2 look-checks row  193 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees
2 look-checks row  67 zone  3072 facing  60 either-side  ask-area  end-row   \ 270 degrees
2 look-checks row  69 zone  1024 facing  60 either-side  2 ovl $8010434C native-at  end-row   \ 90 degrees
2 look-checks row  71 zone  1024 facing  60 either-side  2 ovl $8010448C native-at  end-row   \ 90 degrees
2 look-checks row  73 zone  3072 facing  60 either-side  ask-area  end-row   \ 270 degrees
2 look-checks row  13 zone  2048 facing  60 either-side  ask-area  end-row   \ 180 degrees
2 look-checks row  77 zone  1322 facing  80 either-side  ask-area  end-row   \ 116 degrees
2 act-checks row  73 zone  3072 facing  60 either-side  ask-area  end-row   \ 270 degrees
2 act-checks row  67 zone  3072 facing  60 either-side  ask-area  end-row   \ 270 degrees

\ ---- area 3 ----
3 look-zones row  2 zone  ask-area  end-row
3 look-checks row  129 zone  2048 facing  60 either-side  script events.ovl003:entry-801049F0  end-row   \ 180 degrees
3 look-checks row  130 zone  0 facing  60 either-side  script events.ovl003:entry-80104A4C  end-row   \ 0 degrees
3 look-checks row  1 zone  2048 facing  60 either-side  script events.ovl003:entry-80104ACC  end-row   \ 180 degrees
3 look-checks row  131 zone  0 facing  60 either-side  script events.ovl003:entry-80104B84  end-row   \ 0 degrees
3 look-checks row  132 zone  2048 facing  60 either-side  script events.ovl003:entry-80104C5C  end-row   \ 180 degrees
3 look-checks row  0 zone  1024 facing  60 either-side  ask-area  end-row   \ 90 degrees
3 look-checks row  128 zone  3072 facing  60 either-side  script events.ovl003:entry-801051F8  17 hint  end-row   \ 270 degrees
3 look-checks row  2 zone  1024 facing  60 either-side  script events.ovl003:entry-80105A70  3 hint  end-row   \ 90 degrees
3 look-checks row  133 zone  0 facing  60 either-side  ask-area  3 hint  end-row   \ 0 degrees
3 look-checks row  135 zone  7198 facing  60 either-side  ask-area  end-row   \ 632 degrees
3 look-checks row  136 zone  3072 facing  60 either-side  ask-area  3 hint  end-row   \ 270 degrees
3 act-checks row  133 zone  0 facing  60 either-side  ask-area  3 hint  end-row   \ 0 degrees
3 act-checks row  136 zone  3072 facing  60 either-side  ask-area  3 hint  end-row   \ 270 degrees
3 act-checks row  135 zone  3062 facing  80 either-side  ask-area  end-row   \ 269 degrees

\ ---- area 6 ----
6 look-checks row  0 zone  2048 facing  60 either-side  6 ovl $801021A8 native-at  end-row   \ 180 degrees
6 look-checks row  1 zone  0 facing  60 either-side  6 ovl $80102164 native-at  3 hint  end-row   \ 0 degrees

\ ---- area 7 ----
7 look-checks row  2 zone  3072 facing  60 either-side  script events.ovl007:entry-80103968  3 hint  end-row   \ 270 degrees

\ ---- area 8 ----
8 look-checks row  0 zone  9783 facing  60 either-side  ask-area  end-row   \ 859 degrees
8 look-checks row  1 zone  3632 facing  70 either-side  8 ovl $80102140 native-at  end-row   \ 319 degrees
8 look-checks row  3 zone  5484 facing  70 either-side  script events.ovl008:entry-80103228  end-row   \ 481 degrees
8 look-checks row  2 zone  512 facing  80 either-side  script events.ovl008:entry-801031D8  end-row   \ 45 degrees
8 act-checks row  0 zone  9783 facing  60 either-side  ask-area  end-row   \ 859 degrees

\ ---- area 9 ----
9 look-checks row  128 zone  1536 facing  60 either-side  script events.ovl009:entry-80104170  end-row   \ 135 degrees
9 look-checks row  129 zone  512 facing  60 either-side  script events.ovl009:entry-801041C0  end-row   \ 45 degrees
9 look-checks row  130 zone  2560 facing  60 either-side  script events.ovl009:entry-801041FC  end-row   \ 225 degrees
9 look-checks row  131 zone  2560 facing  60 either-side  script events.ovl009:entry-80104238  end-row   \ 225 degrees
9 look-checks row  132 zone  512 facing  60 either-side  script events.ovl009:entry-80104284  end-row   \ 45 degrees
9 look-checks row  133 zone  4672 facing  80 either-side  9 ovl $80102380 native-at  17 hint  end-row   \ 410 degrees

\ ---- area 10 ----
10 look-checks row  0 zone  7150 facing  60 either-side  $4000 states  10 ovl $80101FA0 native-at  3 hint  end-row   \ 628 degrees
10 look-checks row  1 zone  6144 facing  60 either-side  script events.ovl010:entry-80102680  17 hint  end-row   \ 540 degrees

\ ---- area 11 ----
11 look-checks row  0 zone  0 facing  60 either-side  script events.ovl011:entry-80102560  end-row   \ 0 degrees
11 look-checks row  1 zone  1024 facing  60 either-side  script events.ovl011:entry-801025B0  end-row   \ 90 degrees
11 look-checks row  2 zone  3072 facing  60 either-side  script events.ovl011:entry-80102600  end-row   \ 270 degrees
11 look-checks row  3 zone  0 facing  60 either-side  script events.ovl011:entry-801026DC  3 hint  end-row   \ 0 degrees

\ ---- area 12 ----
12 look-checks row  0 zone  0 facing  60 either-side  script events.ovl012:entry-80108DC4  end-row   \ 0 degrees
12 look-checks row  1 zone  1024 facing  60 either-side  script events.ovl012:entry-80108E14  end-row   \ 90 degrees
12 look-checks row  3 zone  3072 facing  60 either-side  script events.ovl012:entry-80108E50  end-row   \ 270 degrees
12 look-checks row  4 zone  0 facing  60 either-side  script events.ovl012:entry-80108E8C  end-row   \ 0 degrees
12 look-checks row  5 zone  2048 facing  60 either-side  script events.ovl012:entry-80108EC8  end-row   \ 180 degrees

\ ---- area 13 ----
13 look-checks row  0 zone  2560 facing  80 either-side  script events.ovl013:entry-801032F0  16 hint  end-row   \ 225 degrees
13 look-checks row  65 zone  2560 facing  80 either-side  script events.ovl013:entry-80103568  16 hint  end-row   \ 225 degrees
13 look-checks row  64 zone  512 facing  80 either-side  script events.ovl013:entry-80103424  17 hint  end-row   \ 45 degrees
13 look-checks row  128 zone  512 facing  80 either-side  13 ovl $80102620 native-at  17 hint  end-row   \ 45 degrees

\ ---- area 16 ----
16 look-zones row  13 zone  ask-area  end-row
16 look-checks row  9 zone  3072 facing  60 either-side  16 ovl $80102FC0 native-at  end-row   \ 270 degrees
16 look-checks row  8 zone  3072 facing  60 either-side  16 ovl $80102F5C native-at  end-row   \ 270 degrees
16 look-checks row  6 zone  1024 facing  60 either-side  script events.ovl016:entry-801044B4  end-row   \ 90 degrees
16 look-checks row  7 zone  3072 facing  60 either-side  script events.ovl016:entry-801044F0  end-row   \ 270 degrees
16 look-checks row  1 zone  1024 facing  60 either-side  script events.ovl016:entry-8010452C  end-row   \ 90 degrees
16 look-checks row  2 zone  3072 facing  60 either-side  script events.ovl016:entry-80104568  end-row   \ 270 degrees
16 look-checks row  3 zone  3072 facing  60 either-side  script events.ovl016:entry-801045A4  end-row   \ 270 degrees
16 look-checks row  4 zone  1024 facing  60 either-side  script events.ovl016:entry-801045E0  end-row   \ 90 degrees
16 look-checks row  5 zone  3072 facing  60 either-side  16 ovl $80102DF8 native-at  end-row   \ 270 degrees
16 look-checks row  0 zone  2048 facing  60 either-side  ask-area  end-row   \ 180 degrees
16 look-checks row  30 zone  0 facing  60 either-side  script events.ovl016:entry-80104738  3 hint  end-row   \ 0 degrees
16 look-checks row  15 zone  3072 facing  80 either-side  script events.ovl016:entry-80104750  3 hint  end-row   \ 270 degrees
16 look-checks row  18 zone  1024 facing  80 either-side  script events.ovl016:entry-80104770  3 hint  end-row   \ 90 degrees
16 look-checks row  17 zone  0 facing  80 either-side  script events.ovl016:entry-80104760  3 hint  end-row   \ 0 degrees
16 look-checks row  16 zone  1024 facing  80 either-side  script events.ovl016:entry-80104780  3 hint  end-row   \ 90 degrees
16 look-checks row  24 zone  3072 facing  80 either-side  script events.ovl016:entry-80104780  3 hint  end-row   \ 270 degrees
16 look-checks row  20 zone  1024 facing  80 either-side  script events.ovl016:entry-801047A0  3 hint  end-row   \ 90 degrees
16 look-checks row  25 zone  3072 facing  80 either-side  script events.ovl016:entry-801047A0  3 hint  end-row   \ 270 degrees
16 look-checks row  21 zone  1024 facing  80 either-side  script events.ovl016:entry-801047B0  3 hint  end-row   \ 90 degrees
16 look-checks row  27 zone  3072 facing  80 either-side  script events.ovl016:entry-801047B0  3 hint  end-row   \ 270 degrees
16 look-checks row  19 zone  1024 facing  80 either-side  script events.ovl016:entry-80104790  3 hint  end-row   \ 90 degrees
16 look-checks row  28 zone  3072 facing  80 either-side  script events.ovl016:entry-80104790  3 hint  end-row   \ 270 degrees
16 look-checks row  23 zone  1024 facing  70 either-side  script events.ovl016:entry-801047D0  3 hint  end-row   \ 90 degrees
16 look-checks row  29 zone  3072 facing  80 either-side  script events.ovl016:entry-801047D0  3 hint  end-row   \ 270 degrees
16 look-checks row  22 zone  1024 facing  80 either-side  script events.ovl016:entry-801047C0  3 hint  end-row   \ 90 degrees
16 look-checks row  26 zone  3072 facing  80 either-side  script events.ovl016:entry-801047C0  3 hint  end-row   \ 270 degrees
16 look-checks row  12 zone  64552 facing  90 either-side  $4000 states  16 ovl $80102EA8 native-at  9 hint  end-row   \ 5673 degrees
16 look-checks row  14 zone  2048 facing  60 either-side  main-exe $80047BD8 native-at  3 hint  end-row   \ 180 degrees
16 look-checks row  11 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees
16 act-checks row  13 zone  2048 facing  40 either-side  16 ovl $80103024 native-at  22 hint  end-row   \ 180 degrees
16 act-checks row  0 zone  2048 facing  60 either-side  ask-area  end-row   \ 180 degrees

\ ---- area 17 ----
17 look-checks row  0 zone  0 facing  60 either-side  script events.ovl017:entry-80102CA4  16 hint  end-row   \ 0 degrees
17 look-checks row  65 zone  0 facing  60 either-side  script events.ovl017:entry-80102E0C  17 hint  end-row   \ 0 degrees
17 look-checks row  64 zone  0 facing  60 either-side  script events.ovl017:entry-80102F74  16 hint  end-row   \ 0 degrees
17 look-checks row  128 zone  0 facing  60 either-side  script events.ovl017:entry-801030DC  17 hint  end-row   \ 0 degrees
17 look-checks row  1 zone  1024 facing  60 either-side  17 ovl $80102068 native-at  end-row   \ 90 degrees
17 look-checks row  130 zone  2048 facing  60 either-side  script events.ovl017:entry-80102C2C  end-row   \ 180 degrees
17 look-checks row  131 zone  0 facing  60 either-side  script events.ovl017:entry-80102C68  end-row   \ 0 degrees
17 look-checks row  129 zone  63431 facing  80 either-side  ask-area  end-row   \ 5574 degrees

\ ---- area 18 ----
18 look-zones row  1 zone  ask-area  end-row
18 look-checks row  0 zone  3072 facing  60 either-side  script events.ovl018:entry-80102260  end-row   \ 270 degrees
18 look-checks row  1 zone  1024 facing  60 either-side  main-exe $80047BD8 native-at  3 hint  end-row   \ 90 degrees
18 look-checks row  2 zone  2048 facing  60 either-side  script events.ovl018:entry-801022B0  end-row   \ 180 degrees
18 look-checks row  3 zone  0 facing  60 either-side  script events.ovl018:entry-801022EC  end-row   \ 0 degrees

\ ---- area 19 ----
19 look-checks row  0 zone  3072 facing  60 either-side  script events.ovl019:entry-80105B3C  end-row   \ 270 degrees
19 look-checks row  1 zone  2048 facing  60 either-side  script events.ovl019:entry-80105B8C  end-row   \ 180 degrees
19 look-checks row  2 zone  0 facing  60 either-side  script events.ovl019:entry-80105BC8  end-row   \ 0 degrees
19 look-checks row  74 zone  2048 facing  60 either-side  script events.ovl019:entry-80105C04  end-row   \ 180 degrees
19 look-checks row  75 zone  0 facing  60 either-side  script events.ovl019:entry-80105C40  end-row   \ 0 degrees
19 look-checks row  65 zone  0 facing  60 either-side  script events.ovl019:entry-80106188  end-row   \ 0 degrees
19 look-checks row  68 zone  0 facing  60 either-side  script events.ovl019:entry-801061C4  end-row   \ 0 degrees
19 look-checks row  69 zone  2048 facing  60 either-side  script events.ovl019:entry-80106200  end-row   \ 180 degrees
19 look-checks row  3 zone  2072 facing  60 either-side  script events.ovl019:entry-80105C7C  end-row   \ 182 degrees
19 look-checks row  73 zone  3072 facing  60 either-side  script events.ovl019:entry-80106278  end-row   \ 270 degrees
19 look-checks row  66 zone  3072 facing  60 either-side  script events.ovl019:entry-801062BC  end-row   \ 270 degrees
19 look-checks row  67 zone  1024 facing  60 either-side  script events.ovl019:entry-801062F8  end-row   \ 90 degrees
19 look-checks row  4 zone  2072 facing  60 either-side  script events.ovl019:entry-80105D70  16 hint  end-row   \ 182 degrees
19 look-checks row  76 zone  2072 facing  60 either-side  script events.ovl019:entry-80105EEC  17 hint  end-row   \ 182 degrees
19 look-checks row  5 zone  2072 facing  60 either-side  script events.ovl019:entry-80105FA4  17 hint  end-row   \ 182 degrees
19 look-checks row  6 zone  0 facing  60 either-side  script events.ovl019:entry-8010603C  16 hint  end-row   \ 0 degrees
19 look-checks row  79 zone  3087 facing  80 either-side  ask-area  end-row   \ 271 degrees
19 look-checks row  7 zone  0 facing  80 either-side  ask-area  end-row   \ 0 degrees
19 look-checks row  70 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees
19 look-checks row  71 zone  2048 facing  60 either-side  19 ovl $80102934 native-at  end-row   \ 180 degrees
19 look-checks row  81 zone  1024 facing  120 either-side  $4000 states  19 ovl $801027A8 native-at  25 hint  end-row   \ 90 degrees
19 look-checks row  81 zone  1024 facing  120 either-side  19 ovl $80102888 native-at  3 hint  end-row   \ 90 degrees
19 act-checks row  70 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees
19 act-checks row  64 zone  2048 facing  60 either-side  ask-area  end-row   \ 180 degrees

\ ---- area 21 ----
21 look-checks row  0 zone  1024 facing  60 either-side  script events.ovl021:entry-80107414  end-row   \ 90 degrees
21 look-checks row  1 zone  3072 facing  60 either-side  script events.ovl021:entry-80107464  end-row   \ 270 degrees
21 look-checks row  2 zone  1024 facing  60 either-side  script events.ovl021:entry-801074A0  end-row   \ 90 degrees

\ ---- area 23 ----
23 look-zones row  9 zone  ask-area  end-row
23 look-checks row  0 zone  1024 facing  60 either-side  23 ovl $80102300 native-at  end-row   \ 90 degrees
23 look-checks row  129 zone  1024 facing  60 either-side  script events.ovl023:entry-80102C98  end-row   \ 90 degrees
23 look-checks row  130 zone  3072 facing  60 either-side  script events.ovl023:entry-80102CD4  end-row   \ 270 degrees
23 look-checks row  131 zone  2048 facing  60 either-side  script events.ovl023:entry-80102D10  end-row   \ 180 degrees
23 look-checks row  132 zone  0 facing  60 either-side  script events.ovl023:entry-80102D3C  end-row   \ 0 degrees
23 look-checks row  133 zone  3072 facing  60 either-side  ask-area  end-row   \ 270 degrees
23 look-checks row  134 zone  1024 facing  60 either-side  script events.ovl023:entry-80102D7C  end-row   \ 90 degrees
23 look-checks row  1 zone  2048 facing  60 either-side  script events.ovl023:entry-80102D98  16 hint  end-row   \ 180 degrees
23 look-checks row  128 zone  0 facing  60 either-side  script events.ovl023:entry-80102F18  17 hint  end-row   \ 0 degrees
23 look-checks row  137 zone  63468 facing  80 either-side  ask-area  end-row   \ 5578 degrees
23 act-checks row  137 zone  63468 facing  80 either-side  ask-area  end-row   \ 5578 degrees
23 act-checks row  133 zone  3072 facing  80 either-side  ask-area  end-row   \ 270 degrees

\ ---- area 24 ----
24 look-checks row  1 zone  0 facing  60 either-side  $4000 states  24 ovl $80103328 native-at  3 hint  end-row   \ 0 degrees
24 look-checks row  0 zone  2048 facing  60 either-side  script events.ovl024:entry-80105EDC  end-row   \ 180 degrees
24 look-checks row  1 zone  0 facing  60 either-side  24 ovl $80103760 native-at  end-row   \ 0 degrees
24 look-checks row  8 zone  0 facing  60 either-side  script events.ovl024:entry-80106060  end-row   \ 0 degrees
24 look-checks row  4 zone  0 facing  60 either-side  script events.ovl024:entry-8010609C  end-row   \ 0 degrees
24 look-checks row  9 zone  2048 facing  60 either-side  24 ovl $80103614 native-at  end-row   \ 180 degrees
24 look-checks row  10 zone  1024 facing  60 either-side  script events.ovl024:entry-80106114  end-row   \ 90 degrees
24 look-checks row  11 zone  3072 facing  60 either-side  script events.ovl024:entry-80106150  end-row   \ 270 degrees
24 look-checks row  129 zone  2048 facing  60 either-side  24 ovl $80103674 native-at  end-row   \ 180 degrees
24 look-checks row  5 zone  3072 facing  60 either-side  24 ovl $801034DC native-at  end-row   \ 270 degrees
24 look-checks row  2 zone  64512 facing  60 either-side  24 ovl $8010351C native-at  end-row   \ 5670 degrees
24 look-checks row  3 zone  2048 facing  60 either-side  24 ovl $8010355C native-at  end-row   \ 180 degrees
24 look-checks row  128 zone  0 facing  60 either-side  24 ovl $801035B8 native-at  end-row   \ 0 degrees
24 look-checks row  135 zone  3072 facing  60 either-side  script events.ovl024:entry-8010650C  3 hint  end-row   \ 270 degrees
24 look-checks row  7 zone  3072 facing  60 either-side  script events.ovl024:entry-80106204  17 hint  end-row   \ 270 degrees
24 look-checks row  131 zone  1024 facing  60 either-side  $4000 states  script events.ovl024:entry-801045D0  25 hint  end-row   \ 90 degrees
24 look-checks row  132 zone  1500 facing  80 either-side  $4000 states  script events.ovl024:entry-801045D0  25 hint  end-row   \ 131 degrees
24 look-checks row  133 zone  0 facing  60 either-side  $4000 states  script events.ovl024:entry-801045D0  25 hint  end-row   \ 0 degrees
24 look-checks row  12 zone  3072 facing  60 either-side  main-exe $80047BD8 native-at  3 hint  end-row   \ 270 degrees

\ ---- area 26 ----
26 look-checks row  3 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees
26 look-checks row  2 zone  2048 facing  60 either-side  ask-area  end-row   \ 180 degrees
26 look-checks row  9 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees
26 look-checks row  10 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees
26 look-checks row  0 zone  0 facing  60 either-side  script events.ovl026:entry-80106A0C  3 hint  end-row   \ 0 degrees
26 look-checks row  14 zone  3072 facing  60 either-side  script events.ovl026:entry-80106A24  3 hint  end-row   \ 270 degrees
26 act-checks row  9 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees
26 act-checks row  10 zone  0 facing  60 either-side  ask-area  end-row   \ 0 degrees

\ ---- area 28 ----
28 look-checks row  1 zone  1024 facing  60 either-side  script events.ovl028:entry-80102BFC  3 hint  end-row   \ 90 degrees
28 look-checks row  2 zone  2048 facing  60 either-side  script events.ovl028:entry-80102600  3 hint  end-row   \ 180 degrees
28 act-checks row  1 zone  1024 facing  60 either-side  28 ovl $80102080 native-at  1 hint  end-row   \ 90 degrees

\ ---- the town (area 27), a tile at a time: its look and act checks ----
4 1 look-checks town-row  0 zone  0 facing  60 either-side  27 ovl $80106858 native-at  end-row   \ 0 degrees
4 1 act-checks town-row  0 zone  0 facing  80 either-side  ask-area  end-row   \ 0 degrees
4 1 act-checks town-row  1 zone  7192 facing  80 either-side  ask-area  end-row   \ 632 degrees
5 1 look-checks town-row  0 zone  1024 facing  60 either-side  27 ovl $80106D24 native-at  end-row   \ 90 degrees
13 3 look-checks town-row  0 zone  2560 facing  60 either-side  27 ovl $8010679C native-at  16 hint  end-row   \ 225 degrees
12 5 look-checks town-row  0 zone  2048 facing  60 either-side  27 ovl $801065C8 native-at  end-row   \ 180 degrees
29 5 look-checks town-row  0 zone  1024 facing  60 either-side  27 ovl $80106630 native-at  end-row   \ 90 degrees
29 5 look-checks town-row  1 zone  1024 facing  60 either-side  27 ovl $80106664 native-at  end-row   \ 90 degrees
18 6 look-checks town-row  0 zone  0 facing  60 either-side  27 ovl $80106C50 native-at  end-row   \ 0 degrees
19 6 look-checks town-row  0 zone  1024 facing  80 either-side  27 ovl $801066CC native-at  end-row   \ 90 degrees
20 7 look-checks town-row  0 zone  1024 facing  80 either-side  27 ovl $80106698 native-at  end-row   \ 90 degrees
23 7 look-checks town-row  0 zone  1024 facing  60 either-side  script events.ovl027:entry-8010C66C  3 hint  end-row   \ 90 degrees
5 8 look-checks town-row  0 zone  2560 facing  60 either-side  27 ovl $80106824 native-at  end-row   \ 225 degrees
1 12 look-checks town-row  64 zone  3072 facing  60 either-side  27 ovl $801067D0 native-at  end-row   \ 270 degrees
1 12 act-checks town-row  64 zone  56385 facing  90 either-side  ask-area  end-row   \ 4955 degrees
16 12 look-checks town-row  0 zone  3072 facing  60 either-side  27 ovl $801065FC native-at  end-row   \ 270 degrees
16 12 look-checks town-row  1 zone  3072 facing  60 either-side  script events.ovl027:entry-8010C0DC  3 hint  end-row   \ 270 degrees
16 12 look-checks town-row  2 zone  1024 facing  60 either-side  script events.ovl027:entry-8010C114  3 hint  end-row   \ 90 degrees
17 12 look-checks town-row  0 zone  3072 facing  80 either-side  main-exe $80047BD8 native-at  3 hint  end-row   \ 270 degrees
17 13 look-checks town-row  0 zone  512 facing  60 either-side  27 ovl $80106700 native-at  end-row   \ 45 degrees
23 13 look-checks town-row  0 zone  2048 facing  80 either-side  27 ovl $80106768 native-at  end-row   \ 180 degrees
15 14 look-checks town-row  0 zone  1024 facing  60 either-side  27 ovl $8010688C native-at  end-row   \ 90 degrees
17 14 look-checks town-row  0 zone  1024 facing  60 either-side  script events.ovl027:entry-8010C284  3 hint  end-row   \ 90 degrees
5 16 look-checks town-row  0 zone  0 facing  60 either-side  27 ovl $80106734 native-at  end-row   \ 0 degrees
19 19 act-checks town-row  0 zone  2048 facing  60 either-side  ask-area  end-row   \ 180 degrees
23 19 look-checks town-row  3 zone  2048 facing  60 either-side  27 ovl $801067F0 native-at  end-row   \ 180 degrees
23 19 act-checks town-row  2 zone  2048 facing  80 either-side  27 ovl $80106D84 native-at  1 hint  end-row   \ 180 degrees
8 20 look-checks town-row  0 zone  0 facing  60 either-side  27 ovl $80106594 native-at  end-row   \ 0 degrees
8 20 act-checks town-row  0 zone  0 facing  90 either-side  ask-area  end-row   \ 0 degrees
16 20 look-checks town-row  0 zone  3584 facing  60 either-side  script events.ovl027:entry-8010C18C  end-row   \ 315 degrees
