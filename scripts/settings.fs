\ settings.fs - the player's settings, kept beside the saves (docs/subsystems/title.md, options.md; src: OPENING's
\ menu, 0x801FE004 the vibration off, 0x801FE008 / C the screen's position): read at start, written when a menu
\ changes them - the title's (the original's) or the options (F1: the engine's own, options.fs). The pads' rumble
\ (rumble.fs) reads vibration?. A field's 0 is its default, so a file from before a field was added reads the same.
IN: settings
USING: engine ;

begin-structure settings-record
  field: vibration-off       \ (the original kept "off")
  field: screen-x   field: screen-y
  field: volume-down         \ steps below full (0: full; volume-steps: silent)
  field: fullscreen-on
end-structure
create settings-image  settings-record allot   settings-image settings-record erase
: settings-name ( -- addr len )  s" settings" ;
: settings-write ( -- )  settings-image settings-record settings-name save-file drop ;

: vibration? ( -- flag )  settings-image vibration-off @ 0= ;
: vibration! ( flag -- )  0= settings-image vibration-off !  settings-write ;
: screen-offset ( -- x y )  settings-image screen-x @  settings-image screen-y @ ;
: screen-offset! ( x y -- )  settings-image screen-y !  settings-image screen-x !  settings-write ;

\ the sound's volume, in steps (the engine's sound-volume: 0 .. 127 on everything heard)
10 constant volume-steps   127 constant full-sound
: volume ( -- n )  volume-steps settings-image volume-down @ - ;   \ 0 .. volume-steps
: apply-volume ( -- )  volume full-sound * volume-steps / sound-volume ;
: volume! ( n -- )  0 max volume-steps min  volume-steps swap - settings-image volume-down !  apply-volume  settings-write ;

\ the window fullscreen (F11 and Alt+Return change it too, not kept)
: fullscreen-setting? ( -- flag )  settings-image fullscreen-on @ 0<> ;
: fullscreen-setting! ( flag -- )  0<> dup settings-image fullscreen-on !  fullscreen!  settings-write ;

: apply-settings ( -- )  apply-volume  fullscreen-setting? if  true fullscreen!  then ;   \ (as the game starts)
settings-image settings-record settings-name load-file drop   apply-settings
