\ the clock (docs/subsystems/clock.md)
USING: tester engine actors messages clock ;

testing times
T{ 25 0 0 date>time -> 0 }T
T{ 25 9 0 date>time -> 540 }T
T{ 28 3 0 date>time time>date -> 28 3 0 }T
T{ 31 23 59 date>time time>day -> 31 }T
T{ 26 13 7 date>time dup time>hour swap time>minute -> 13 7 }T

\ a listener: what the clock said, and its answers
variable heard-minutes   variable last-minute   variable last-set   variable answer
behaviour clock-listener
  on spawned ( -- )  self subscribe minute-passed  self subscribe time-set ;
  on minute-passed ( t -- )  last-minute !  1 heard-minutes +! ;
  on time-set ( t -- )  last-set ! ;
  on the-time ( t -- )  answer ! ;
end-behaviour
state: no-state end-state
clock-listener no-state s" clock-test" spawn constant listener
s" clock" actor-named constant the-clock
deliver

: steps ( n -- )  0 ?do  actors-frame  loop ;

testing a new game: 12/25 09:00, held until the story lets it run
T{ 30 steps  heard-minutes @ -> 0 }T

testing it runs indoors: a minute every 5 seconds (12 game seconds a second)
story-stopped the-clock send release-clock  deliver
T{ 0 heard-minutes !  5e seconds 1- steps  heard-minutes @ -> 0 }T
T{ 1 steps  heard-minutes @  last-minute @ -> 1  25 9 1 date>time }T

testing in the town: twice as fast
true the-clock send in-town  deliver
T{ 0 heard-minutes !  5e seconds steps  heard-minutes @ -> 2 }T

testing anything holding it stops it, until all let go
menu-open the-clock send hold-clock  no-control the-clock send hold-clock  deliver
T{ 0 heard-minutes !  10e seconds steps  heard-minutes @ -> 0 }T
menu-open the-clock send release-clock  deliver
T{ 10e seconds steps  heard-minutes @ -> 0 }T
no-control the-clock send release-clock  deliver
T{ 2.5e seconds steps  heard-minutes @ -> 1 }T

testing a jump: time-set, no minute-passed
T{ 0 heard-minutes !  28 3 0 date>time the-clock send set-time  deliver  last-set @  heard-minutes @
   -> 28 3 0 date>time 0 }T
T{ the-clock send advance-minute  deliver  last-set @ -> 28 3 1 date>time }T

testing asking the time
: ask-time ( -- t )  -1 answer !  listener enter  the-clock send what-time  leave-actor  deliver  answer @ ;
T{ ask-time -> 28 3 1 date>time }T

test-summary
