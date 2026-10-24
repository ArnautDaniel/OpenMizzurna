\ area005.fs - area 5's rules: Wolf's warehouse, the finale (src: ovl005.c; decomp/docs/areas/area05.md). The stay
\ is one chain of scripts the warp in starts (arrive-80104614): the confrontation, the fight with the driver,
\ Wolf's flight with Emma, the shoot-out, the last fight, out to the town. Its own C runs the shoot-out alone.
\ The flare (minigame 40) is flare.fs's.
IN: rules.area005
USING: engine actors messages events shootout ;

5 constant this-area
\ the shoot-out (src: shootout_update): Wolf three rounds, the gunman's the third
$1C056 constant his-x   $DA77 constant his-z   $1BF57 constant wolf-x   $EC0C constant wolf-z   $774 constant wolf-heading
3 constant wolf-hit   4 constant gunman-hit   12 constant grazed   20 constant shot-hard
$54A constant gun-stance   90 constant wolf-health   40 constant gunman-health   0 constant wolf-pool   1 constant gunman-pool
create the-shootout  4 plan-bytes allot   the-shootout 4 plan-bytes erase
: plan! ( -- )
    the-shootout {: p :}  s" shootout-5" p plan-table-len ! p plan-table !  4 p plan-rounds !
    his-x p plan-him-x !  his-z p plan-him-z !  -1 p plan-stance !  -1 p plan-shot-motion !  -1 p plan-aimed-motion !
    wolf-x p plan-foe-x !  wolf-z p plan-foe-z !  wolf-heading p plan-foe-heading !  gun-stance p plan-foe-stance !
    -1 p plan-foe-hit !  wolf-health p plan-full-0 !  gunman-health p plan-full-1 !  0 p plan-won !
    p 0 wolf-pool wolf-hit grazed ['] events.ovl005:entry-80105264 ['] events.ovl005:entry-80105D80 ['] events.ovl005:entry-80106274 round!
    p 1 wolf-pool wolf-hit shot-hard ['] events.ovl005:entry-8010543C ['] events.ovl005:entry-8010602C ['] events.ovl005:entry-80106300 round!
    p 2 gunman-pool gunman-hit grazed ['] events.ovl005:entry-80105614 ['] events.ovl005:entry-80106144 ['] events.ovl005:entry-8010638C round!
    p 3 wolf-pool wolf-hit shot-hard ['] events.ovl005:entry-80105748 ['] events.ovl005:entry-801061DC ['] events.ovl005:entry-80106418 round! ;
plan!
: shootout-begin ( -- )  the-shootout s" shootout" actor-named send shootout-start ;   \ 0x8010222C (src: shootout_begin)
: shootout-on ( -- n )  shooting? 1 and ;                  \ 0x8010224C (src: shootout_active)
101 constant the-flare-carrier   \ (src: actor 0x65)
: flare ( -- )  the-flare-carrier s" flare" actor-named send flare-on ;   \ minigame 40
' flare 40 minigame-code!
this-area ovl $8010222C ' shootout-begin native:   this-area ovl $8010224C ' shootout-on native:
