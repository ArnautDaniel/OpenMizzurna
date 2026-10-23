\ messages.fs - every message the actors pass, with its arguments: the game's API. Each subsystem's page
\ (docs/subsystems/) says who sends and who takes them.
IN: messages
USING: actors ;

\ ---- the clock (docs/subsystems/clock.md) ----
\ a time: whole game minutes since 12/25 00:00 (clock.fs: date>time, time>date)
message set-time ( t -- )               \ jump to t; the clock broadcasts time-set
message advance-minute ( -- )           \ one minute on, as a jump
message hold-clock ( reason -- )        \ stop the clock for this reason (clock-hold) until released
message release-clock ( reason -- )
message in-town ( flag -- )             \ the pace: the town's, or indoors'
message what-time ( -- )                \ answered with the-time
message the-time ( t -- )
message minute-passed ( t -- )          \ broadcast: a minute ran by; t the new time
message time-set ( t -- )               \ broadcast: the time jumped to t

\ why the clock is held (src: field_update's checks, each now said by its subsystem)
enum: clock-hold  story-stopped event-running no-control menu-open text-shown ;

\ ---- the story (docs/subsystems/story.md) ----
message new-game ( -- )                 \ (to the story) start the game afresh: 12/25 09:00, the opening
message prologue-over ( -- )            \ broadcast: the prologue ended (Matthew at the station, 12/25 10:00)

\ ---- the message window (docs/subsystems/window.md) ----
\ a text bank: 0..32 an area's, 33 the common text, 200+ an event pack's (its CD.BIN entry): engine text-of
33 constant common-text
message window-say ( bank id speaker palette secs -- )   \ show a message; secs 0: it waits for ○ (else it closes itself)
message window-ask ( bank id kind -- )   \ a question with its answers (kind: src msg_choice)
message window-shut ( -- )               \ close it now
message window-closed ( answer -- )      \ broadcast: the window closed (the answer chosen; -1 none)

\ ---- areas (docs/subsystems/areas.md) ----
message enter-area ( area scene -- )     \ (to the areas) play that area from that scene
message show-scene ( scene -- )          \ (to the areas) another of the area's scenes (its objects and camera)
message play-area-anim ( entry -- )      \ (to the areas) the area's animation list's entry: its object plays its motion once
message area-entered ( area scene -- )   \ broadcast: the area is in
message scene-shown ( scene -- )         \ broadcast: the scene changed
27 constant the-town                     \ the streamed outdoors (MAP.BIN)

\ ---- figures: the people's bodies, by actor slot (docs/subsystems/matthew.md) ----
message figure-place ( x y z -- )        \ stand there (world units)
message figure-turn ( rx ry rz -- )      \ turned so (4096 a turn)

\ ---- the camera (docs/subsystems/camera.md) ----
message camera-place ( look-y dist eye-y x y z yaw -- )   \ a fixed shot (src: the events' camera_set)
message camera-follow ( -- )             \ behind Matthew again, the scene's way
message camera-fixed ( -- )              \ stay where it is
message camera-reset ( -- )              \ behind Matthew now, where he stands (src: camera_reset)
message figure-anim ( id next loop -- )  \ play motion id (looping or not); then `next` (-1: hold its last frame)
message figure-hold ( -- )               \ at the motion's end, the figure keeps where it moved (src: anim_hold)
message player-control ( flag -- )       \ broadcast: Matthew is the player's to move, or not (src: player_control_on / off)

\ ---- the screen's effects (docs/subsystems/effects.md) ----
message screen-effect ( n -- )           \ start effect n (the executable's table; +256: under the window)
message effect-level ( from to -- )      \ the iris: from one level to another (0 shut .. 16 open)
message camera-move-to ( x y z speed -- )   \ the pivot travels there, speed units a frame (src: camera_move_to)
message camera-turn-to ( yaw step -- )       \ the heading steps there (step 0: at once)
message camera-height-to ( eye-y step -- )
message camera-dist-to ( dist step -- )
message camera-face ( yaw -- )               \ heading set now
message camera-pivot ( x y z -- )            \ pivot set now (src: camera_at)

\ ---- items (docs/subsystems/items.md) ----
message give-item ( item -- )            \ Matthew has it now (src: item_add)
message take-item ( item -- )            \ and no longer (src: item_remove)
message items-reset ( -- )               \ a new game's: the phone, the waist bag, the watch (src: items_init)
message person-turn ( slot heading -- )  \ (to the townsfolk) the person faces so, until their next stop
\ (to the townsfolk: what the events and routes do to a person; slot first)
message person-place ( slot x z -- )      \ stand there (height and area kept: src set_pos keeps y)
message person-floor ( slot floor -- )    \ on that floor (its height)
message person-walk ( slot x z speed motion -- )   \ walk there, speed units a frame, in that motion
message person-idle ( slot -- )           \ stop walking, stand (src: cmd 0x0C)
message person-forward ( slot speed motion -- )   \ straight ahead, speed a frame, in that motion (-1: as it is) until idle
message person-anim ( slot id next loop -- )
message person-hold ( slot -- )           \ at the motion's end they keep its move
message person-arrive ( slot -- )         \ the route is done: the next stop (src: actor_arrive)
message person-goto-stop ( slot stop -- ) \ put at that stop now (src: actor_goto_stop)
message person-depart ( slot -- )         \ leave their stop now, walking its route (src: actor_depart)
message person-through-door ( slot door room flip -- )   \ out of that door into room (flip: the other way)
message person-attach ( slot route speed -- )   \ get in their car: it drives that route, they ride (src: cmd 0x2C)
message person-get-in ( slot -- )         \ climb into their car (src: cmd 0x42)
message person-departs ( slot route -- )  \ broadcast: they left their stop (route: its address, 0 none)
message person-stopped ( slot -- )        \ broadcast: they no longer walk a route (put at a stop)

\ ---- traffic: the cars, slots 26-60 (docs/subsystems/traffic.md) ----
message car-park ( car route -- )        \ (to the traffic) stand at the route's start, in its lane (src: carline_start)
message car-hide ( car -- )              \ off the roads
message car-drive ( car route speed -- ) \ drive the route from its start (speed: units a frame)
message car-carry ( car route speed -- ) \ the same with someone riding: car-arrived at its end
message car-stop ( car -- )              \ stop where it is, riders still in (src: carline_stop)
message car-arrived ( car -- )           \ broadcast: a car with riders reached its route's end; they get out

\ ---- the day's light and the weather (docs/subsystems/daylight.md) ----
message set-weather ( weather hold -- )  \ (to the daylight) clear, overcast or snowing; hold: it stays (src: func_80025384)
