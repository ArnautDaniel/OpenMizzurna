\ settings.fs - the player's settings, kept beside the saves (docs/subsystems/title.md; src: OPENING's menu,
\ 0x801FE004 the vibration off, 0x801FE008 / C the screen's position): read at start, written when the title's
\ menu changes them. The pads' rumble (rumble.fs) reads vibration?.
IN: settings
USING: engine ;

create settings-image 3 cells allot   settings-image 3 cells erase
: settings-name ( -- addr len )  s" settings" ;
: vibration? ( -- flag )  settings-image @ 0= ;   \ (the original kept "off")
: screen-offset ( -- x y )  settings-image cell+ @  settings-image 2 cells + @ ;
: settings-write ( -- )  settings-image 3 cells settings-name save-file drop ;
: vibration! ( flag -- )  0= settings-image !  settings-write ;
: screen-offset! ( x y -- )  settings-image 2 cells + !  settings-image cell+ !  settings-write ;
settings-image 3 cells settings-name load-file drop
