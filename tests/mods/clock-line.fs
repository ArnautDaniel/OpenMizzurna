\ The sample mod (examples/mods/clock-line): found, its scripts loaded in their own vocabulary, its file read through
\ the files layer, its actor hearing the clock's broadcasts and answering by message. Not a test_*.fs: it runs with
\ the mod turned on (CMakeLists.txt: mods_clock_line, and mods_clock_line_zipped from a zip of it):
\   build/openmizzurna --mod examples/mods/clock-line --test tests/mods/clock-line.fs
USING: tester engine actors messages clock clock-line ;

\ ---- an asker: sends minutes-heard? and keeps the answer ----
variable answered
behaviour asking
  on minutes-heard ( n -- )  answered ! ;
end-behaviour
state: asker-state end-state
asking asker-state s" asker" spawn constant asker
s" clock" actor-named constant the-clock
: settle ( -- )  4 0 do  deliver  loop ;
: ask-minutes ( -- n )  -1 answered !  asker enter  clock-line send minutes-heard?  leave-actor  settle  answered @ ;
settle

testing the mod is on, its actor there; a mod not there isn't on
T{ s" clock-line" mod-on?  s" clock-line" actor-named 0< -> true false }T
T{ s" no-such-mod" mod-on? -> false }T
T{ mod-api -> 102 }T   \ (1.1: walk-intent, 1.2: follow-yaw)

testing its own file, through the files layer
: caption-file ( -- addr n )  s" clock-line/caption.txt" library-file ;
T{ caption-file nip  caption-file 1- s" Mizzurna Falls," compare -> 16 0 }T

testing it hears the clock: the time set, minutes passing
T{ 25 10 30 date>time the-clock send set-time  settle  shown-time @ -> 25 10 30 date>time }T
T{ line s" Mizzurna Falls, 12/25 10:30" compare -> 0 }T
T{ the-clock send advance-minute  settle  line s" Mizzurna Falls, 12/25 10:31" compare -> 0 }T
: steps ( n -- )  0 ?do  actors-frame  loop ;
story-stopped the-clock send release-clock  settle   \ (indoors: a minute every 5 seconds)
T{ 10e seconds steps  line s" Mizzurna Falls, 12/25 10:33" compare -> 0 }T

testing it answers by message
T{ ask-minutes -> 2 }T

test-summary
