\ After tests/mods/menu.fs: OpenMizzurna started again with the same folder of mods - layer-a first and off
\ (mods/order.txt as the menu wrote it), the others as they were.
USING: tester engine ;

: row-name ( i -- addr len )  mods-menu-mod drop ;
: row-on? ( i -- flag )  mods-menu-mod nip nip ;

testing the menu's order.txt in force
T{ s" layer-a" mod-on?  s" clock-line" mod-on?  s" future-mod" mod-on? -> false true false }T
T{ 0 row-name s" layer-a" compare  0 row-on?  mods-menu-changed? -> 0 false false }T

test-summary
