\ car-routes.fs - the cars' routes (docs/subsystems/traffic.md): converted once from the executable's
\ table by tools/carroutes2forth.py; edited by hand from here on. A route: the parking node whose lanes
\ it starts in, the road nodes it drives through (how many), then where it parks - or, going round, the
\ node (counted from 0) it drives on from again.
IN: events.car-routes
USING: traffic.rows ;

  0 car-route  2 lane  21 19 17 15 13 11 9 7 5 3 1 59 62 64 66 116 60  17 roads  0 parks  end-car-route
  1 car-route  2 lane  21 19 17 15 13 11 9 7 5 3 1 59 62 64 66 116 60  17 roads  0 parks  end-car-route
  2 car-route  0 lane  61 117 67 65 63 58 0 2 4 6 8 10 12 14 16 18 20  17 roads  2 parks  end-car-route
  3 car-route  1 lane  35 33 31 5 3 1 59 62 64 66 116 60  12 roads  0 parks  end-car-route
  4 car-route  3 lane  29 13 11 9 7 5 3 1 59 62 64 66 116 60  14 roads  0 parks  end-car-route
  5 car-route  4 lane  77 27 25 23 9 7 5 3 1 59 62 64 66 116 60  15 roads  0 parks  end-car-route
  6 car-route  5 lane  57 54 62 64 66 116 60  7 roads  0 parks  end-car-route
  7 car-route  6 lane  81 79 59 62 64 66 116 60  8 roads  0 parks  end-car-route
  8 car-route  0 lane  61 117 67 65 63 58 0 2 4 6 8 10 12 14 16 18 82  17 roads  7 parks  end-car-route
  9 car-route  0 lane  61 117 67 65 63 58 78 80  8 roads  6 parks  end-car-route
 10 car-route  6 lane  81 79 0 2 4 6 8 10 12 14 16 18 82  13 roads  7 parks  end-car-route
 11 car-route  2 lane  21 82  2 roads  7 parks  end-car-route
 12 car-route  7 lane  83 20  2 roads  2 parks  end-car-route
 13 car-route  7 lane  83 19 17 15 13 11 9 7 5 3 1 78 80  13 roads  6 parks  end-car-route
 14 car-route  2 lane  21 19 17 15 13 11 9 7 30 32 34  11 roads  1 parks  end-car-route
 15 car-route  0 lane  61 117 67 65 63 55 57  7 roads  5 parks  end-car-route
 16 car-route  6 lane  81 79 0 2 4 30 32 34  8 roads  1 parks  end-car-route
 17 car-route  1 lane  35 33 31 5 3 96 100 102  8 roads  14 parks  end-car-route
 18 car-route  1 lane  35 33 31 5 3 108  6 roads  8 parks  end-car-route
 19 car-route  8 lane  109 2 4 30 32 34  6 roads  1 parks  end-car-route
 20 car-route  9 lane  95 1 59 62 64 66 116 106  8 roads  10 parks  end-car-route
 21 car-route  1 lane  35 33 31 5 3 1 59 62 64 66 118  11 roads  11 parks  end-car-route
 22 car-route  11 lane  119 67 65 63 58 0 2 4 30 32 34  11 roads  1 parks  end-car-route
 23 car-route  1 lane  35 36 40 42 44 46 48 50 52 54 58 85 91 92 96 100 70 4 30 32  20 roads  1 loops-to  end-car-route
 24 car-route  1 lane  35 36 40 42 86 88 92 2 4 30 38  11 roads  2 loops-to  end-car-route
 25 car-route  2 lane  119 67 65 63 58 0 2 4 6 8 10 12 14 16 18 82  16 roads  7 parks  end-car-route
 26 car-route  7 lane  83 19 17 15 13 11 9 7 30 32 34  11 roads  1 parks  end-car-route
 27 car-route  10 lane  107 117 67 65 63 58 0 94  8 roads  9 parks  end-car-route
 28 car-route  10 lane  107 117 67 65 120  5 roads  12 parks  end-car-route
 29 car-route  9 lane  95 2 4 30 38 40 42 86 88 92  10 roads  0 loops-to  end-car-route
 30 car-route  92 lane  94  1 roads  9 parks  end-car-route
 31 car-route  13 lane  123 88 92 1 59 62 64 66 116 60  10 roads  0 parks  end-car-route
 32 car-route  5 lane  57 54 58 0 94  5 roads  9 parks  end-car-route
 33 car-route  0 lane  61 117 67 65 63 58 0 94  8 roads  9 parks  end-car-route
 34 car-route  9 lane  95 1 59 55 56  5 roads  5 parks  end-car-route
 35 car-route  9 lane  95 1 78 80  4 roads  6 parks  end-car-route
 36 car-route  0 lane  61 117 67 65 63 58 0 2 4 6 8 10 12 28  14 roads  3 parks  end-car-route
 37 car-route  6 lane  81 68 100 70 4 30 38 40 42 86 88 90 84 0 2 72 74 92 96 69 79 85 91 89 87 43 41 37 33 31 5 71 101 97 93 75 73 3 1 78  40 roads  1 loops-to  end-car-route
 38 car-route  6 lane  81 79 85 91 75 73 71 101 69 79 0 2 72 74 90 84 78 68 100 70 3 1  22 roads  2 loops-to  end-car-route
 39 car-route  78 lane  80  1 roads  6 parks  end-car-route
 40 car-route  1 lane  35 33 31 6 8 22 24 26 76  9 roads  4 parks  end-car-route
 41 car-route  0 lane  61 117 67 65 63 58 0 2 4 30 32 34  12 roads  1 parks  end-car-route
 42 car-route  1 lane  35 36 40 42 44 46 48 50 52 56  10 roads  5 parks  end-car-route
 43 car-route  5 lane  57 54 58 0 2 4 6 8 22 24 26 76  12 roads  4 parks  end-car-route
 44 car-route  4 lane  77 27 25 23 9 7 30 32 34  9 roads  1 parks  end-car-route
 45 car-route  3 lane  29 14 16 18 20  5 roads  2 parks  end-car-route
 46 car-route  15 lane  125 27 25 23 9 7 5 3 1 78 80  11 roads  6 parks  end-car-route
 47 car-route  7 lane  83 19 17 15 13 11 9 7 5 3 108  11 roads  8 parks  end-car-route
 48 car-route  16 lane  115 113 111 7 5 3 1 59 62 64 66 116 60  13 roads  0 parks  end-car-route
 49 car-route  1 lane  35 36 40 42 86 88 92 1 134  9 roads  19 parks  end-car-route
 50 car-route  19 lane  135 0 2 4 6 8 10 12 14 16 18 20  12 roads  2 parks  end-car-route
 51 car-route  1 lane  35 36 40 131 132  5 roads  18 parks  end-car-route
 52 car-route  3 lane  29 13 11 9 7 5 3 96 162 126 128  11 roads  17 parks  end-car-route
 53 car-route  16 lane  115 113 111 7 5 71 102  7 roads  14 parks  end-car-route
 54 car-route  1 lane  35 33 31 6 8 10 12 14 16 18 20  11 roads  2 parks  end-car-route
 55 car-route  30 lane  32 34  2 roads  1 parks  end-car-route
 56 car-route  11 lane  119 67 65 63 58 134  6 roads  19 parks  end-car-route
 57 car-route  19 lane  135 0 2 4 30 32 34  7 roads  1 parks  end-car-route
 58 car-route  2 lane  21 19 17 15 13 11 9 7 30 32 36 40 42 86 122  15 roads  13 parks  end-car-route
 59 car-route  13 lane  123 88 75 73 4 6 8 10 12 14 16 18 82  13 roads  7 parks  end-car-route
 60 car-route  7 lane  83 19 17 15 13 11 9 7 5 72 74 89 122  13 roads  13 parks  end-car-route
 61 car-route  3 lane  29 13 11 9 7 30 38 138  8 roads  20 parks  end-car-route
 62 car-route  20 lane  139 40 42 136  4 roads  21 parks  end-car-route
 63 car-route  12 lane  121 63 58 0 94  5 roads  9 parks  end-car-route
 64 car-route  9 lane  95 2 4 6 110 112 114  7 roads  16 parks  end-car-route
 65 car-route  79 lane  134  1 roads  19 parks  end-car-route
 66 car-route  19 lane  135 78 80  3 roads  6 parks  end-car-route
 67 car-route  9 lane  95 1 59 62 64 66 116 60  8 roads  0 parks  end-car-route
 68 car-route  6 lane  81 79 0 2 4 6 8 22 24 26 124  11 roads  15 parks  end-car-route
 69 car-route  22 lane  141 45 86 88 92 1 78 80  8 roads  6 parks  end-car-route
 70 car-route  6 lane  81 79 0 93 89 87 44 140  8 roads  22 parks  end-car-route
 71 car-route  23 lane  143 50 52 54 58 78 80  7 roads  6 parks  end-car-route
 72 car-route  6 lane  81 79 59 55 53 51 142  7 roads  23 parks  end-car-route
 73 car-route  6 lane  81 79 0 94  4 roads  9 parks  end-car-route
 74 car-route  9 lane  95 1 59 62 120  5 roads  12 parks  end-car-route
 75 car-route  10 lane  107 60  2 roads  0 parks  end-car-route
 76 car-route  8 lane  109 1 59 55 57  5 roads  5 parks  end-car-route
 77 car-route  9 lane  95 96 162 126 98 144  6 roads  24 parks  end-car-route
 78 car-route  24 lane  145 99 127 163 97 94  6 roads  9 parks  end-car-route
 79 car-route  6 lane  81 79 0 2 4 6 8 10 12 28  10 roads  3 parks  end-car-route
 80 car-route  3 lane  29 113 11 22 24 26 124  7 roads  15 parks  end-car-route
 81 car-route  25 lane  149 147 25 23 9 7 5 3 1 78 80  11 roads  6 parks  end-car-route
 82 car-route  6 lane  81 79 0 2 4 6 8 22 24 124 148  11 roads  25 parks  end-car-route
 83 car-route  3 lane  29 13 150  3 roads  26 parks  end-car-route
 84 car-route  26 lane  151 11 9 7 30 32 34  7 roads  1 parks  end-car-route
 85 car-route  26 lane  151 11 9 7 5 3 1 59 62 64 66 116 60  13 roads  0 parks  end-car-route
 86 car-route  0 lane  61 117 67 65 63 58 0 2 4 6 8 10 150  13 roads  26 parks  end-car-route
 87 car-route  27 lane  157 153 111 7 5 3 1 59 62 120  10 roads  12 parks  end-car-route
 88 car-route  12 lane  121 63 58 0 2 4 6 110 152 156  10 roads  27 parks  end-car-route
 89 car-route  5 lane  57 54 58 78 80  5 roads  6 parks  end-car-route
 90 car-route  5 lane  57 54 62 64 66 116 60 61 117 67 105 99 127 163 97 93 89 87 43 41 39 31 6 110 112 114 115 113 111 8 10 12 14 16 18 20 21 19 17 15 13 11 9 7 5 3 1 59  48 roads  2 loops-to  end-car-route
 91 car-route  59 lane  55 56  2 roads  5 parks  end-car-route
 92 car-route  20 lane  139 40 42 86 88 92 94  7 roads  9 parks  end-car-route
 93 car-route  6 lane  81 79 0 93 89 87 43 41 138  9 roads  20 parks  end-car-route
 94 car-route  12 lane  121 64 105 144  4 roads  24 parks  end-car-route
 95 car-route  5 lane  57 53 51 49 47 45 43 41 37 34  10 roads  1 parks  end-car-route
 96 car-route  5 lane  57 53 51 49 47 45 43 41 138  9 roads  20 parks  end-car-route
 97 car-route  20 lane  139 40 42 44 46 48 50 52 56  9 roads  5 parks  end-car-route
 98 car-route  21 lane  137 43 86 122  4 roads  13 parks  end-car-route
 99 car-route  20 lane  139 37 34  3 roads  1 parks  end-car-route
100 car-route  3 lane  29 13 11 9 7 5 3 1 59 62 120  11 roads  12 parks  end-car-route
101 car-route  10 lane  107 117 67 105 99 127 163 97 2 4 6 8 10 150  14 roads  26 parks  end-car-route
102 car-route  16 lane  115 113 111 8 22 24 26 76  8 roads  4 parks  end-car-route
103 car-route  4 lane  77 27 25 23 9 7 5 72 74 92 94  11 roads  9 parks  end-car-route
104 car-route  4 lane  77 25 23 9 110 112 114  7 roads  16 parks  end-car-route
105 car-route  16 lane  115 113 111 7 5 71 101 97 94  9 roads  9 parks  end-car-route
106 car-route  12 lane  121 64 66 116 106  5 roads  10 parks  end-car-route
107 car-route  16 lane  115 113 111 7 30 32 34  7 roads  1 parks  end-car-route
108 car-route  1 lane  35 36 40 42 86 88 92 94  8 roads  9 parks  end-car-route
109 car-route  9 lane  95 2 4 30 32 34  6 roads  1 parks  end-car-route
110 car-route  5 lane  57 54 58 134  4 roads  19 parks  end-car-route
111 car-route  19 lane  135 59 62 64 66 116 60  7 roads  0 parks  end-car-route
112 car-route  10 lane  107 117 67 65 63 58 78 80  8 roads  6 parks  end-car-route
113 car-route  6 lane  81 79 0 2 4 6 8 22 24 26 76  11 roads  4 parks  end-car-route
114 car-route  12 lane  121 64 105 99 128  5 roads  17 parks  end-car-route
115 car-route  28 lane  159 25 23 10 12 28  6 roads  3 parks  end-car-route
116 car-route  3 lane  29 13 11 22 24 158  6 roads  28 parks  end-car-route
117 car-route  24 lane  166 99 127 163 97 2 4 6 8 10 12 14 155 153 111 7 30 38 40 42 44 46 48 50 52 54 62 64 105  29 roads  1 loops-to  end-car-route
118 car-route  16 lane  115 113 111 7 5 3 1 59 62 64 66 116 106  13 roads  10 parks  end-car-route
119 car-route  29 lane  161 31 5 3 1 78 80  7 roads  6 parks  end-car-route
120 car-route  6 lane  81 79 0 2 4 30 160  7 roads  29 parks  end-car-route
121 car-route  5 lane  57 54 62 64 105 144  6 roads  24 parks  end-car-route
122 car-route  1 lane  35 31 5 3 1 59 62 120  8 roads  12 parks  end-car-route
123 car-route  12 lane  121 64 66 116 60  5 roads  0 parks  end-car-route
124 car-route  0 lane  61 117 67 65 120  5 roads  12 parks  end-car-route
125 car-route  13 lane  123 88 75 73 4 6 8 10 150  9 roads  26 parks  end-car-route
126 car-route  26 lane  151 11 9 7 5 72 74 89 122  9 roads  13 parks  end-car-route
127 car-route  18 lane  133 130 41 37 34  5 roads  1 parks  end-car-route
128 car-route  26 lane  151 11 9 7 5 72 74 92 94  9 roads  9 parks  end-car-route
129 car-route  12 lane  121 63 58 0 2 4 6 110 112 114  10 roads  16 parks  end-car-route
130 car-route  13 lane  123 88 92 2 4 6 8 10 12 28  10 roads  3 parks  end-car-route
131 car-route  6 lane  81 79 0 2 4 6 110 112 114  9 roads  16 parks  end-car-route
132 car-route  17 lane  129 127 163 97 2 4 6 8 10 12 28  11 roads  3 parks  end-car-route
133 car-route  3 lane  29 13 11 9 7 30 38 40 42 86 122  11 roads  13 parks  end-car-route
134 car-route  21 lane  137 43 41 138  4 roads  20 parks  end-car-route
135 car-route  20 lane  139 40 42 86 122  5 roads  13 parks  end-car-route
136 car-route  1 lane  35 33 31 6 8 10 12 28  8 roads  3 parks  end-car-route
137 car-route  3 lane  29 13 11 9 7 30 32 34  8 roads  1 parks  end-car-route
138 car-route  1 lane  35 36 40 42 136  5 roads  21 parks  end-car-route
139 car-route  14 lane  103 70 4 6 110 112 114  7 roads  16 parks  end-car-route
140 car-route  12 lane  121 63 58 0 2 71 102  7 roads  14 parks  end-car-route
141 car-route  14 lane  103 70 3 1 59 62 120  7 roads  12 parks  end-car-route
142 car-route  27 lane  157 153 111 7 5 71 103  7 roads  14 parks  end-car-route
143 car-route  14 lane  103 101 162 164  4 roads  30 parks  end-car-route
144 car-route  30 lane  165 126 98 104 65 121  6 roads  12 parks  end-car-route
145 car-route  14 lane  103 70 4 6 110 152 156  7 roads  27 parks  end-car-route
146 car-route  16 lane  115 113 111 7 5 3 96 162 164  9 roads  30 parks  end-car-route
147 car-route  27 lane  157 153 112 114  4 roads  16 parks  end-car-route
148 car-route  1 lane  35 36 40 42 44 46 48 50 168  9 roads  31 parks  end-car-route
149 car-route  31 lane  169 52 54 58 0 94  6 roads  9 parks  end-car-route
150 car-route  12 lane  121 63 58 0 2 4 6 8 22 24 26 76  12 roads  4 parks  end-car-route
151 car-route  4 lane  77 27 25 23 9 7 5 3 1 78 80  11 roads  6 parks  end-car-route
152 car-route  4 lane  77 27 25 23 9 7 5 3 1 59 62 120  12 roads  12 parks  end-car-route
153 car-route  30 lane  165 163 97 93 89 122  6 roads  13 parks  end-car-route
154 car-route  13 lane  123 88 92 96 162 164  6 roads  30 parks  end-car-route
155 car-route  4 lane  77 27 25 23 9 7 5 3 1 59 55 56  12 roads  5 parks  end-car-route
156 car-route  7 lane  83 19 17 155 153 111 7 5 72 74 92 94  12 roads  9 parks  end-car-route
157 car-route  9 lane  95 1 85 91 89 122  6 roads  13 parks  end-car-route
158 car-route  13 lane  123 87 43 41 37 34  6 roads  1 parks  end-car-route
159 car-route  24 lane  145 104 65 120  4 roads  12 parks  end-car-route
160 car-route  1 lane  35 33 31 6 110 112 114  7 roads  16 parks  end-car-route
161 car-route  16 lane  115 113 111 7 5 3 1 59 62 120  10 roads  12 parks  end-car-route
162 car-route  20 lane  139 40 42 86 88 92 96 100 102  9 roads  14 parks  end-car-route
163 car-route  14 lane  103 101 97 93 89 87 43 41 138  9 roads  20 parks  end-car-route
164 car-route  12 lane  120 63 59 0 2 4 30 32 34  9 roads  1 parks  end-car-route
165 car-route  5 lane  57 54 62 64 105 99 127 165  8 roads  30 parks  end-car-route
166 car-route  13 lane  123 88 92 1 59 55 56  7 roads  5 parks  end-car-route
167 car-route  11 lane  119 116 60  3 roads  0 parks  end-car-route
168 car-route  5 lane  57 54 58 85 91 89 122  7 roads  13 parks  end-car-route
169 car-route  30 lane  165 163 97 95  4 roads  9 parks  end-car-route
170 car-route  8 lane  109 2 4 6 8 10 12 14 16 18 82  11 roads  7 parks  end-car-route
171 car-route  0 lane  61 117 67 65 63 58 0 108  8 roads  8 parks  end-car-route
172 car-route  7 lane  83 19 17 155 153 111 7 5 72 74 92 94  12 roads  9 parks  end-car-route
173 car-route  4 lane  77 27 25 23 10 12 14 16 18 82  10 roads  7 parks  end-car-route
174 car-route  17 lane  129 98 104 65 63 58 0 108  8 roads  8 parks  end-car-route
175 car-route  7 lane  83 19 17 15 13 151  6 roads  26 parks  end-car-route
176 car-route  16 lane  115 113 152 154 16 18 82  7 roads  7 parks  end-car-route
177 car-route  8 lane  109 2 4 6 8 10 12 28  8 roads  3 parks  end-car-route
178 car-route  9 lane  95 96 100 102  4 roads  14 parks  end-car-route
179 car-route  24 lane  145 104 65 63 58 0 2 4 6 8 10 12 28  13 roads  3 parks  end-car-route
180 car-route  3 lane  29 14 155 153 111 8 10 12  8 roads  1 loops-to  end-car-route
