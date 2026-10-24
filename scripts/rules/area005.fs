\ area005.fs - area 5's rules: Wolf's warehouse, the finale (src: ovl005.c; decomp/docs/areas/area05.md). The stay
\ is one chain of scripts the warp in starts (arrive-80104614): the confrontation, the fight with the driver,
\ Wolf's flight with Emma, the shoot-out, the last fight, out to the town. Its own C runs the shoot-out alone.
\ The flare (minigame 40) is flare.fs's.
IN: rules.area005
USING: engine actors messages screen areas events shootout ;

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
    -1 p plan-foe-hit !  wolf-health p plan-full-0 !  gunman-health p plan-full-1 !  wolf-name p plan-name-0 !  gang-name p plan-name-1 !  -1 p plan-prop !  0 p plan-won !  $71 p plan-shot-sound !  -1 p plan-hit-sound !
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
\ each frame (src: func_80102C04, before drawing): the fire's flames, sixteen frames (texture animation 0; after
\ the first it runs 1 - 15); in scene 0 the fire shown only while burning, and not burning, still
0 constant fire-anim   16 constant fire-frames
variable fire-frame
: fire-drawn ( -- )
    scene @ 0= if  fire-lit @ 0= if  the-fire false shown! exit  then  the-fire true shown!
    else  the-fire false shown!  then
    fire-anim fire-frame @ texture-frame
    1 fire-frame +!  fire-frame @ fire-frames >= if  1 fire-frame !  then ;
this-area ' fire-drawn area-every-frame!
this-area ovl $8010222C ' shootout-begin native:   this-area ovl $8010224C ' shootout-on native:
