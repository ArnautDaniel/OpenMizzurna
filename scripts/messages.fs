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
enum: clock-hold  story-stopped event-running no-control menu-open text-shown phone-up events-locked ;

\ ---- the story (docs/subsystems/story.md) ----
message new-game ( -- )                 \ (to the story) start the game afresh: 12/25 09:00, the opening
message prologue-over ( -- )            \ broadcast: the prologue ended (Matthew at the station, 12/25 10:00)

\ ---- the message window (docs/subsystems/window.md) ----
\ a text bank: 0..32 an area's, 33 the common text, 34 the list menus' labels, 200+ an event pack's (its CD.BIN entry): engine text-of
33 constant common-text
message window-say ( bank id portrait palette secs -- )   \ show a message; the speaker's portrait (a kind, -1 none);
                                         \ secs 0: it waits for ○ (else it closes itself)
message window-ask ( bank id kind -- )   \ a question with its answers (kind: src msg_choice)
message list-open ( id0 id1 id2 id3 id4 id5 id6 -- )   \ (to the list menu) its labels, top first (0: none)
message list-closed ( answer -- )         \ broadcast: the row picked (0..), or -1 cancelled
message window-shut ( -- )               \ close it now
message window-closed ( answer -- )      \ broadcast: the window closed (the answer chosen; -1 none)

\ ---- areas (docs/subsystems/areas.md) ----
message enter-area ( area scene -- )     \ (to the areas) play that area from that scene
message show-scene ( scene -- )          \ (to the areas) another of the area's scenes (its objects and camera)
message play-area-anim ( entry -- )      \ (to the areas) the area's animation list's entry: its object plays its motion once
message object-shown ( object flag -- )   \ (to the areas) one of the area's objects shown or hidden (until the next scene)
message reveal-room ( scene -- )          \ (to the areas) another room's first object shown too (seen through a door)
message object-motion ( object id next -- ) \ (to the areas) an object's motion (the common props'), then the next looping (-1: held)
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
message fight-start ( a style b style -- ) \ a fistfight: each the pad's (style 0) or the AI's (2 - 8) (src: fight_start)
message tail-pace ( -- )                   \ (to the tail) the lookout paces by his door (src: minigame 1)
message tail-start ( -- )
message robber-start ( -- )               \ (to the tail) the robber at the diner's door, then round his loop (src: minigame 18)
message robber-spoken-to ( -- )          \ talked to: the first time he's off (19), then caught (20)                  \ tailing him begins (src: minigame 2)
message dog-chase-start ( -- )             \ (to the dog chase) from the start (src: minigame 26's dog_reset)
message dog-chase-stop ( -- )              \ he's got away: it's over
message walker-start ( slot node dest speed motion -- )   \ (to the walkers) along the town's footpaths from that node to the
                                          \ destination (one of the first 24), speed a frame, in that motion (src: walker_start)
message walker-stop ( slot -- )            \ stop walking there, stand (src: walker_remove)
message flare-on ( slot -- )             \ (to the flare) a flare's glow at their feet, growing (src: ground_flare_draw)
message rumble-small ( port frames -- )    \ (to the rumble) the pad's small motor for so many of the original's pad frames (-1 for ever; 0 off)
message rumble-large ( port strength frames -- )   \ its large motor so hard (0 - 255) for so long
message rumble-stop ( -- )
message play-sound ( id -- )             \ (to the sound) an effect once: under 100 the common bank's program, else the area's
message play-sound-at ( id volume -- )  \ the same, at a volume (0 - 127: footsteps further off)
message loop-sound ( id -- )             \ an effect held, until stopped (src: ambient_start)
message stop-loop ( id -- )              \ (src: ambient_stop)
message play-sequence ( id loop? -- )    \ a sequence: under 100 the common set's, else the area's (src: seq_play)
message stop-sequence ( id -- )
message play-music ( row -- )            \ the music table's row in the background's place (-1: none; src: music_play)
message play-stream ( n -- )             \ TSG.XA's channel n (src: xa_play)
message stop-stream ( -- )
message engine-start ( id tones -- )     \ (to the sound) the car's engine: that effect's first tones held (src: func_80022DF4)
message engine-pitch ( key -- )          \ its first tone at that key (src: engine_sound)
message engine-stop ( -- )
message shootout-start ( plan -- )         \ a shoot-out by an area's plan (shootout.fs; src: shootout_begin)
message drive-hold ( flag -- )           \ (to the driving) the pad leaves the car alone (src: D_800A361C 0: the chase's sweep, its crash)
message car-bumped ( -- )                 \ (to the driving) a knock: its speed down to a sixteenth (src: the chase's bump)
message car-nudge ( dx dz -- )            \ (to the driving) the car moved by that (src: the chase's crash slide)
message left-the-car ( -- )               \ broadcast: Matthew got out of his car (src: car_exit's func_80106980)
message camera-driving ( flag -- )       \ (to the camera) following the car: its eye higher (src: camera_follow's g_driving)
message camera-turned-round ( flag -- )  \ (to the camera) Matthew faces backwards: it looks the other way (the boat)
message camera-behind ( look-y dist eye-y -- ) \ low behind Matthew, turning as he turns (src: ovl019's duct crawl)
message camera-reset ( -- )              \ behind Matthew now, where he stands (src: camera_reset)
message figure-anim ( id next loop -- )  \ play motion id (looping or not); then `next` (-1: hold its last frame)
message figure-hold ( -- )               \ at the motion's end, the figure keeps where it moved (src: anim_hold)
message figure-on-foot ( flag -- )         \ (to Matthew) the pad walks him, or not (on a stool: src pad_set_field_mode)
message figure-pushes ( crate -- )         \ (to Matthew) he pushes that crate as he walks (-1: lets go)
message figure-perch ( n -- )              \ he's on his feet (0), on a crate (1), on a stool (2) (src: talk_state)
message figure-crawl ( flag -- )        \ the pad drives him crawling, tank-like, while a scene runs (src: ovl019's duct)
message figure-shown ( flag -- )        \ (to Matthew) drawn or not (src: actors[0].visible)
message figure-coord ( which v -- )      \ (to Matthew) one of his place's: 0 x, 1 height, 2 z
message figure-nudge ( dx dy dz -- )     \ moved by that
message figure-floor ( floor -- )        \ his floor now, its height under him (src: actor_set_floor)
message figure-walk ( x z speed motion -- )   \ (to Matthew) a scene walks him there, speed a game frame
message figure-forward ( speed motion -- )     \ straight on, until idle (-1: the motion as it is)
message figure-idle ( -- )                     \ stop walking, stand
message figure-turning ( target speed -- )     \ turn toward the heading, speed a game frame (sign: the way; 0 at once)
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
message person-turning ( slot target speed -- )   \ turns toward the heading, speed a game frame (its sign the way; 0: at once)
\ (to the townsfolk: what the events and routes do to a person; slot first)
message person-place ( slot x z -- )      \ stand there (height and area kept: src set_pos keeps y)
message extra-place ( slot x y z -- )      \ an extra (no schedule) there, in the area being played; (0, 0) parks them
message person-floor ( slot floor -- )    \ on that floor (its height)
message person-walk ( slot x z speed motion -- )   \ walk there, speed units a frame, in that motion
message person-idle ( slot -- )           \ stop walking, stand (src: cmd 0x0C)
message person-forward ( slot speed motion -- )   \ straight ahead, speed a frame, in that motion (-1: as it is) until idle
message person-anim ( slot id next loop -- )
message person-hold ( slot -- )           \ at the motion's end they keep its move
message person-arrive ( slot -- )         \ the route is done: the next stop (src: actor_arrive)
message person-goto-stop ( slot stop -- ) \ put at that stop now (src: actor_goto_stop)
message person-set-stop ( slot stop travelling -- ) \ their stop now, not placed (-1: off their schedule; src: actor_set_stop)
message person-depart ( slot -- )         \ leave their stop now, walking its route (src: actor_depart)
message person-through-door ( slot door room flip stage -- )   \ out of that door into room (flip: the other way);
                                          \ stage: the area they're shown in (-1: the room's own)
message person-attach ( slot route speed -- )   \ get in their car: it drives that route, they ride (src: cmd 0x2C)
message person-get-in ( slot -- )         \ climb into their car (src: cmd 0x42)
message car-anim ( car id next -- )      \ (to the traffic) a motion of its own, then the next held (-1: its last frame; id -1: none)
message town-door-swing ( slot -- )       \ (to the town) a door's town side swings: its animation slot (src: func_8001A190)
message person-passenger ( slot car seat -- ) \ (to the townsfolk) in that car, to get out by that seat (src: car_enter_with_passenger)
message person-in-car ( slot car -- )      \ (to the townsfolk) out of sight in that car, out with its riders (src: cmd 0x50)
message person-ride ( slot mode -- )      \ (to the townsfolk) they go along with Matthew: 1 following on foot, 2 in his car
message person-unride ( slot -- )          \ no longer along with him
message companion-halt ( -- )              \ his companion stays where they are (still along: src D_800A37B4 = 0)
message person-at-door ( slot door side -- )   \ (to the townsfolk) they answer that door, on that side
message person-door-hold ( slot -- )       \ they wait at the door (src: cmd $53, door_wait 255 frames)
message person-coord ( slot which v -- )  \ (to the townsfolk) one of their place's: 0 x, 1 height, 2 z (src: cmds 0x21 - 0x23)
message person-nudge ( slot dx dy dz -- ) \ moved by that (src: cmd 0x38)
message person-keep ( slot -- )            \ their heading and motion kept, to come back to (src: actor_save_state)
message person-back ( slot -- )            \ as kept (src: actor_restore_state)
message person-door-free ( slot -- )       \ no longer at the door
message person-variant ( slot kind -- )   \ (to the townsfolk) they show that kind's model (0 or their own: theirs)
message person-away ( slot -- )           \ (to the townsfolk) between stops, out of sight until their next (src: travelling = 1)
message person-tilt ( slot rx rz -- )    \ (to the townsfolk) they're tilted so (src: rot_x, rot_z)
message person-partner ( slot car -- )    \ (to the townsfolk) that car their partner (ridden: Mel's bike) (src: func_80029B20)
message person-unlink ( slot -- )         \ no longer with their car: out of it where they are (src: partner = 0)
message seat-take ( slot bar seat -- )    \ (to the townsfolk) that stool is theirs (bar: at-diner, at-club; seat -1: the first free)
message seat-sit ( slot -- )               \ the person sits on their stool
message seat-leave ( slot -- )             \ their stool is free again
message person-departs ( slot route -- )  \ broadcast: they left their stop (route: its address, 0 none)
message person-stopped ( slot -- )        \ broadcast: they no longer walk a route (put at a stop)

\ ---- traffic: the cars, slots 26-60 (docs/subsystems/traffic.md) ----
message car-park ( car route -- )        \ (to the traffic) stand at the route's start, in its lane (src: carline_start)
message car-hide ( car -- )              \ off the roads
message car-drive ( car route speed -- ) \ drive the route from its start (speed: units a frame)
message car-carry ( car route speed -- ) \ the same with someone riding: car-arrived at its end
message car-place ( car x y z heading -- )   \ there, off its route, standing (Matthew's car as he drives it)
message car-speed ( car speed -- )        \ its top speed now, on its way (src: carline_set_speed)
message car-switch ( car route -- )       \ at that route's first node it turns onto it (src: cmd 0x37)
message car-stop ( car -- )              \ stop where it is, riders still in (src: carline_stop)
message car-arrived ( car -- )           \ broadcast: a car with riders reached its route's end; they get out

\ ---- the day's light and the weather (docs/subsystems/daylight.md) ----
message muzzle-flash ( -- )              \ (to the daylight) a shot lights the scene for a frame (src: shootout_flash_lights)
message set-weather ( weather hold -- )  \ (to the daylight) clear, overcast or snowing; hold: it stays (src: func_80025384)

\ ---- the screens over the field: the item ring and what it opens (docs/subsystems/ring.md) ----
message ui-lock ( -- )                   \ (to the ui) a screen is up: the world stands still (src: ui_lock)
message ui-unlock ( -- )                 \ (to the ui) that screen is gone (src: ui_unlock)
message take-pad ( -- )                  \ (to the ui) a screen has the pad, the world going on (the phone: src its pad mode)
message give-pad ( -- )
message ring-chosen ( item shown-to -- ) \ broadcast: the ring closed on an item (-1 none); shown-to: the person it
                                         \ was shown to (-1: chosen for use)

\ ---- the pictures beside the message box and above it (docs/subsystems/pictures.md) ----
message show-picture ( n -- )            \ (to the pictures) the small picture n beside the box (src: func_800484C8)
message hide-picture ( -- )
message show-big-picture ( left right -- )   \ (to the pictures) the big one above the box: two CD.BIN entries, its
                                             \ halves (0 0: none - src: the VM's cd_picture ops)

\ ---- the notebook (docs/subsystems/notebook.md) ----
message learn-bio ( bit -- )             \ (to the notebook) a person's page known (src: g_bio_known |= )
message learn-number ( bit -- )          \ (to the notebook) a phone number known (src: g_phone_known |= )
message know ( bios numbers -- )         \ (to the notebook) who and what he knows, as a save had them

\ ---- a save loaded (docs/subsystems/saves.md) ----
message items-set ( mask -- )            \ (to the items) the list from a save's mask (src: items_from_mask)
message his-car-at ( x z heading -- )    \ (to the driving) his car there (src: save_apply)
message fuel-set ( n -- )
message car-at-pump ( flag -- )         \ (to the driving) by a gas pump: ○ fills (the town's), not out of the car
message car-stop-now ( -- )             \ (to the driving) its speed gone (src: g_player_speed = 0)
message person-restore ( slot stop kind -- )   \ (to the townsfolk) at that stop, showing that kind (src: actor_load)

\ ---- the title (docs/subsystems/title.md) ----
message computer-open ( places opener -- )   \ (to the map) the police computer: the map in its other palette, those
                                         \ places' case files; opener ( place -- event | -1 ) plays one (src: ovl000 FPolice.C)
message title-boot ( mode -- )           \ (to the title) start it: 0 the logo, 1 / 2 an ending's movie and card
