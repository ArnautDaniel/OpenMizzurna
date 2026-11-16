\ The mods' order and the files layer, and every way a mod is turned off. Run with the test mods named
\ (CMakeLists.txt: mods_layers):
\   build/openmizzurna --mod tests/mods/layer-a --mod tests/mods/layer-b --mod tests/mods/future-mod
\       --mod tests/mods/needs-future --mod tests/mods/clock --mod tests/mods/endless-mod --test tests/mods/layers.fs
USING: tester engine actors clock ;

: first-byte ( addr len -- c )  library-file if  c@  else  drop 0  then ;

testing on: layer-a and layer-b (b needing a at 1.1: a is 1.2)
T{ s" layer-a" mod-on?  s" layer-b" mod-on? -> true true }T
testing off: a newer mod API, a mod needing an off one, a game's vocabulary's name, a script that never ends
T{ s" future-mod" mod-on?  s" needs-future" mod-on?  s" clock" mod-on?  s" endless-mod" mod-on? -> false false false false }T

testing the files: layer-a after layer-b (its after:) so its file wins; each one's own seen; an off mod's not
T{ s" mods-test/which.txt" first-byte -> char a }T
T{ s" mods-test/a-only.txt" first-byte  s" mods-test/b-only.txt" first-byte -> char a char b }T
T{ s" mods-test/nothing.txt" library-file nip -> 0 }T

testing the game as it was: its clock there, its words the game's (the clock mod's script never loaded)
T{ s" clock" actor-named 0<  25 9 0 date>time -> false new-game-time }T

test-summary
