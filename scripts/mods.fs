\ mods.fs - what the game gives mods besides its own vocabularies (docs/modding.md): the hooks. A mod's scripts
\ say `USING: mods` and any of the game's vocabularies. (The engine's words for mods - `.mods`, `mod-api`,
\ `mod-on?`, the mods menu's list - are `engine`'s: src/game/mods.c.)
IN: mods

\ ---- hooks: where the game asks, and a mod may answer in its place ----
\ A hook is a deferred word with the game's own behaviour in it, and its stack effect said: every mod's word set
\ to it has that effect, and its callers are checked with it.
\
\ The game offers one (in the subsystem's file; listed in docs/modding.md, the mod API's minor raised):
\     : walk-by-pad ( -- )  ... ;                    \ the game's own way
\     ' walk-by-pad hook: matthew-walk ( -- )        \ the hook, the game's way in it; the code calls matthew-walk
\
\ A mod takes it - keeping what was there (the game's way, or an earlier mod's), handling what it wants and passing
\ the rest on to the kept one, with the same arguments:
\     hook-before: matthew-walk walk-before ( -- )   \ walk-before: what matthew-walk did before this mod
\     : analog-walk ( -- )  stick-tilted? if  ...  else  walk-before  then ;
\     ' analog-walk is matthew-walk
\
\ So mods taking the same hook chain in load order: the last loaded is asked first, each passing on to the one
\ before, the game's way at the end.

64 constant most-hooks
begin-structure /hook
    field: hook.word       \ the hook (its xt)
    field: hook.game's     \ the game's own way: what it held when it was made
end-structure
create hooks  most-hooks /hook * allot
variable #hooks

<PRIVATE
: hook# ( i -- addr )  /hook * hooks + ;
: register ( hook game's -- )
    #hooks @ most-hooks < 0= abort" hook: more hooks than mods.fs keeps (most-hooks)"
    #hooks @ hook# {: h :}  h hook.game's !  h hook.word !  1 #hooks +! ;
: entry-of ( hook -- addr | 0 )
    {: w :}  0  #hooks @ 0 ?do  i hook# hook.word @ w = if  drop i hook# leave  then  loop ;
PRIVATE>

: hook: ( xt "name" -- )   \ a hook named so, holding xt (the game's way); its stack comment follows the name
    defer  latest {: hook :}  hook >body !  hook  hook >body @  register ;
: hook? ( xt -- flag )  entry-of 0<> ;
: hook-game's ( hook -- xt )   \ the game's own way of a hook (what a mod's chain ends in)
    entry-of dup 0= abort" hook-game's: not a hook (hook:)"  hook.game's @ ;
: hook-taken? ( hook -- flag )  dup >body @  swap hook-game's <> ;   \ has a mod set its own?
: hook-before: ( "hook" "name" -- )   \ name: a deferred word holding what the hook holds now; its stack comment
                                      \ (the hook's) follows the name
    ' dup hook? 0= abort" hook-before: that word isn't one of the game's hooks (hook:)"
    >body @  defer  latest >body ! ;
