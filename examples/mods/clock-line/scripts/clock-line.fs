\ clock-line: the story's day and hour as a line at the top of the screen ("Mizzurna Falls, 12/25 09:00"), kept
\ by hearing the clock.
\
\ The sample mod: an actor of its own. It hears the clock's broadcasts (time-set, minute-passed: scripts/messages.fs),
\ keeps what it heard and draws a line over the game; it never touches another actor's state. It answers a question
\ (minutes-heard?) by a message back, as the game's actors do. Its caption is a file of its own,
\ library/clock-line/caption.txt, read through the files layer as the library's are (a mod could put its own over
\ any of the library's files the same way).
IN: clock-line
USING: engine actors messages clock screen title ;

message minutes-heard? ( -- )     \ asked: it answers the sender with minutes-heard
message minutes-heard ( n -- )    \ the minutes it has heard pass

\ what the line shows: written by the actor alone, read by the drawing (a frame's 2D, outside the actors)
variable shown-time   -1 shown-time !   \ the clock's time as last heard (-1: none yet - nothing drawn)
-152 constant line-x   -112 constant line-y   $F0E8C8FF constant line-colour   12 constant december

<PRIVATE
: chomp ( addr n -- addr n' )  dup if  2dup + 1- c@ 10 = if  1-  then  then ;   \ (the file's newline off)
: caption ( -- addr len )  s" clock-line/caption.txt" library-file chomp ;
: two-digits ( n -- )  dup 10 < if  s" 0" $+  then  n>str $+ ;
PRIVATE>

: line ( -- addr len )   \ "Mizzurna Falls, 12/25 09:00"
    shown-time @ time>date {: day hour minute :}
    $clear  caption $+  s"  " $+  december two-digits  s" /" $+  day two-digits  s"  " $+
    hour two-digits  s" :" $+  minute two-digits  $@ ;
: draw-line ( -- )
    shown-time @ 0<  title-up? or if  exit  then   \ (none heard yet, or the title up: no game)
    0 font  0 font-palette  scale pen-scale  line-colour pen-color
    line  line-x line-y >screen font-text ;
' draw-line on-draw

state: clock-line-state
    cell field heard          \ the minutes heard pass
end-state

behaviour watching-the-clock
  on spawned ( -- )  self subscribe time-set  self subscribe minute-passed ;
  on time-set ( t -- )  shown-time ! ;
  on minute-passed ( t -- )  shown-time !  1 heard +! ;
  on minutes-heard? ( -- )  heard @  sender send minutes-heard ;
end-behaviour

watching-the-clock clock-line-state s" clock-line" spawn constant clock-line
