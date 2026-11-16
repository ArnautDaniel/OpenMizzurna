\ the hooks (scripts/mods.fs): a hook holding the game's way; two mods taking it in turn, each handling what it
\ wants and passing the rest to the one before; the game's way at the end of the chain
USING: tester mods ;

: game-greeting ( n -- n' )  1+ ;
' game-greeting hook: greeting ( n -- n' )

testing a hook as made: the game's way
T{ 5 greeting -> 6 }T
T{ ' greeting hook?  ' game-greeting hook? -> true false }T
T{ ' greeting hook-taken?  ' greeting hook-game's ' game-greeting = -> false true }T

testing a mod takes it: negatives its own, the rest passed on
hook-before: greeting first-before ( n -- n' )
: first-mod ( n -- n' )  dup 0< if  negate  else  first-before  then ;
' first-mod is greeting
T{ 5 greeting  -5 greeting -> 6 5 }T
T{ ' greeting hook-taken? -> true }T

testing a second mod after it: asked first, the first mod and the game's way behind it
hook-before: greeting second-before ( n -- n' )
: second-mod ( n -- n' )  dup 100 > if  drop 0  else  second-before  then ;
' second-mod is greeting
T{ 5 greeting  -5 greeting  500 greeting -> 6 5 0 }T
T{ ' greeting hook-game's ' game-greeting = -> true }T

testing only a hook can be kept so
T{ s" hook-before: game-greeting nope ( n -- n' )" ' evaluate catch nip nip 0= -> false }T

test-summary
