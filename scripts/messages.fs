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
-1 constant nobody                        \ no speaker

\ ---- areas (docs/subsystems/areas.md) ----
message enter-area ( area scene -- )     \ (to the areas) play that area from that scene
message show-scene ( scene -- )          \ (to the areas) another of the area's scenes (its objects and camera)
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
