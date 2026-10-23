\ story.fs - the story actor (docs/subsystems/story.md): starts a game and plays the events the story calls
\ for. The flags are facts (story-flags.fs): read by anyone, set by whoever's part of the story it is.
IN: story
USING: engine actors messages clock story-flags events events.all ;

1 constant matthews-house   \ the area of the opening (src: game_start's ovl001 scripts)

\ ---- the prologue's end (src: evfn_80047620, called by the main executable's script $80096598) ----
25 10 0 date>time constant prologue-end-time   \ 12/25 10:00: Matthew at the station, the car beside him
: end-prologue ( -- )
    \ (everyone to their first stop, Matthew and the car placed, the diner's and the club's seats freed: the
    \ townsfolk, Matthew and the areas, as they are built)
    story-stopped release  event-running release
    prologue-end-time the-clock send set-time
    prologue-done flag-set
    broadcast prologue-over ;
main-exe $80047620 ' end-prologue native:

state: story-state end-state

behaviour story
  on new-game ( -- )   \ src: game_start(0)
    story-clear  events-reset
    new-game-time the-clock send set-time   \ 12/25 09:00, held until the prologue ends
    story-stopped hold
    event-running hold                      \ (src: event_lock)
    matthews-house event-area !
    ['] events.ovl001:new-game-opening start-event ;
end-behaviour

: spawn-story ( -- id )  story story-state s" story" spawn ;
