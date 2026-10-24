# The boat

The rowing boat (src: `ovl027.c` boat_update and its boathouses, `player_misc.c` boat_mode_begin / _end, `pad_map.c`
field mode 7). `scripts/boat.fs`, the actor `boat`.

- **Begun** by the lake bar's scene (native $8001A290): the boat (actor 103, a townsfolk extra) is brought
  into the town where its scene put it. Matthew sits in it facing backwards. The pad is the boat's (`take-pad`:
  no look, no ring), and the camera looks the way the boat goes (`camera-turned-round`).
- **Rowing**, a game frame at a time (as the car's, its own limits):
  - ✕ pulls ahead, 0.4 a frame up to 8; □ backs water, 0.2 down to −6; neither, it coasts;
  - L1 and R1 turn it 8 a frame, within (200 − speed) / 10; let go, the turn eases a unit a frame;
  - each with its motions, his and the boat's (the town's own: $1648 sitting, $1649 pulling, $165A backing,
    $164A..$165F the oars). A motion is told only when it changes;
  - the speed is 20 × the velocity, along the heading; a wall (the town's tile obstacles) or a solid billboard
    stops it where it was.
- **The boathouses**: within 500 of either (east $71BC3, $70C0A; west $524DF, $6C294), ○'s prompt shows
  (`boat-prompt` $10, the HUD's). ○ plays that boathouse's landing: the screen fades, boat mode ends (native
  $8001A2C8), then the warp.
- The HUD's corner map turns the boat's way.
- Others read `rowing?` and `boat-prompt`.

## Status

**Built.**
