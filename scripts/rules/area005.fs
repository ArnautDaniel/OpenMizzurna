\ area005.fs - area 5's rules: Wolf's warehouse, the finale (src: ovl005.c; decomp/docs/areas/area05.md). The stay
\ is one chain of scripts the warp in starts (arrive-80104614): the confrontation, the fight with the driver,
\ Wolf's flight with Emma, the shoot-out, the last fight, out to the town. Its own C runs the shoot-out alone.
\ (The fights, the shoot-out and the flare: the minigames, to come - the scripts carry on past them.)
IN: rules.area005
USING: engine actors messages events ;

5 constant this-area
: shootout-begin ( -- )  0 s" the shoot-out" unbuilt ;   \ 0x8010222C (src: shootout_begin)
: shootout-on ( -- flag )  false ;                         \ 0x8010224C (src: shootout_active: over at once)
this-area ovl $8010222C ' shootout-begin native:   this-area ovl $8010224C ' shootout-on native:
