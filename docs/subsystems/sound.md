# Sound

The game's sound on SDL3 audio: what the original's sound calls intend, not its sound chip (src: sound.c,
ambience.c, session.c `sound_init`, area.c `area_sound_load` / `area_sep_open`, actor_anim.c `func_8001C57C`,
draw.c `func_80018748` and the areas' ambience words, cevcon.c `func_80041D24`; the phase-1 port's psx/audio.c
and seq.c, which the engine's mixer follows). The engine's `src/audio/` and `scripts/sound.fs`, the actor
`sound`; `scripts/door-sounds.fs`.

## The engine (src/audio/)

- **Banks.** The library's `N.bank.txt` (made back into the header's bytes by data/vab.c) and `M.samples/<i>.wav`
  (16-bit at 44.1 kHz, each with its loop). A program is a set of tones; a tone plays a sample over a range of
  keys, with its volume, pan, centre key and fine tuning, bend range, priority, reverb, and the sound chip's two
  envelope words.
- **Voices** (24). A voice plays its sample at the key's pitch against the tone's centre (2^(Δ/1536) of 1/128
  semitones), with linear interpolation. Its envelope steps once a sample by the chip's attack / decay / sustain /
  release rules, so the banks' values keep their timing. When no voice is free, the quietest releasing one is
  taken, else the oldest of the least important not above the new one.
- **Sequences.** The library's `N.sep/` is made back into the original's bytes (data/seqmidi.c): Sony's MIDI. Its
  habits:
  - a tempo change is FF 51 and three bytes, with no length byte;
  - a loop is NRPN 99 = 20 plus data entry n at its start (n times; 0 or 127 for ever), and 99 = 30 jumps back;
  - its channels' volume, pan, expression and bend, and a set's fade.
- **The XA stream.** The library's `xa/TSG/<channel>.flac`, decoded a chunk at a time (dr_flac) and brought from
  37.8 kHz to 44.1 kHz.
- **Mixing.** In float at 44.1 kHz, with a generic room or echo reverb standing for the chip's types.
- **Headless.** Tests and screenshots have no device, but the state is kept. `audio-run` drives the mixer, and
  `audio-record` writes its output to a WAV file, for listening.
- **Forth words.** `bank-load`, `bank-free`, `key-on`, `key-off`, `loop-off`, `voice-pitch`, `voice-volume`,
  `voice-on?`, `voices-on`, `all-voices-off`, `sound-volume`, `reverb`, `sep-load`, `sep-free`, `seq-play`,
  `seq-stop`, `seq-fade`, `seq-playing?`, `xa-start`, `xa-end`, `xa-playing?`, `audio-run`, `audio-record`.

## The game's (scripts/sound.fs)

- **The banks.** The common bank (033_1 and 033_2) and the music's (033_4 part 10 and 058_3) are open from the
  start. Entering an area opens its bank (NNN_1, NNN_2) and its effect sequences (NNN_4 part 9). The common
  sequences are 033_4 part 9.
- **An effect** (`play-sound id`; `sfx`): under 100 a program of the common bank, else the area's (less 100).
  It plays its first tone at the middle key (60), at full volume or a given one (`play-sound-at`). Area 5's 0x71
  sounds as three voices, area 26's 0x6D as two.
- **A held loop** (`loop-sound` / `stop-loop`): every tone covering the middle key, three a bank at once.
- **Sequences** (`play-sequence id loop?` / `stop-sequence`): under 100 the common set's, else the area's.
- **The background** (`play-music row`; `music-row`): a row of the executable's `music` table. Each row is a
  sequence set's CD.BIN entry, played on the music bank once or for ever at its volume.
  - Entering an area starts its ambience (the executable's `area-sounds`; by the hour in area 21 and the town).
  - A cycle counts the original's frames: faded out over 3 seconds from 2520, stopped at 2700, started again at
    5400.
  - Silent outdoors at night (areas 0–3, 7, 9, 13, 29, 30 before 7:00 and from 20:00; area 12 before 20:00; the
    town before 6:00).
  - A scene's music takes its place and the cycle starts again; −1 stops it.
- **XA** (`play-stream n` / `stop-stream`): TSG.XA's channel n at its volume (0x4D, 0x4C, 0x4E, 0x52, 0x67,
  0x6D, 0x7F), with the background put aside while it plays.
- **The car's engine** (`engine-start id tones`, `engine-pitch key`, `engine-stop`): area effect 0x66, its two
  tones held. The first is pitched at |speed| / 10 + 0x29.
- **Footsteps** (src: func_8001C57C).
  - Any figure drawn: a motion player's `advance-heard` (figures.fs) tells the sound of the frame it reaches.
  - The walking and running motions sound on set frames: 1, 5, 0xBA, 0x159B, 0x159E on 3 and 9; 2, 0xBD,
    0x159C on 3 and 11; 0x3F on 20, 31, 42; 0x40 on 40, 45, 50; and so on.
  - The sound is the area's ground: by its scene, plus area 26's decks; else 0x64.
  - It's quieter by a twentieth of the distance from the camera, out of hearing at 127.
  - The original also sounded frames 9 and 16 of any other motion; that's left out until checked against the
    game.
- **The areas' own, a game frame at a time:**
  - area 5's scene-1 loop;
  - now-and-then sounds (1 in 64) in areas 7, 8, 10, 13, 23;
  - the held loops of areas 10 and 23;
  - the church's and the town's bells at 7, 12 and 17 o'clock (three tolls);
  - the forest: birds, a call, the wind, the river louder toward its end, and the cabin's hum;
  - the town by the hour (birds in the morning, a call in the afternoon, the night's), on foot, and the river by
    the bridge.
- **Door sounds** (op $176; door-sounds.fs): by the door's kind, one of the executable's little scripts on a
  thread of its own. The third kind plays the area's sound (`door-sounds`, by the content area) twice.
- **Facts:** `last-sound` (the effect played last: the tests'), `background-row`.

## Status

Built: `tests/library/test_sound.fs`, plus sounds checked in the fight, driving, dog chase and car chase tests.

To come:
- the pad's rumble;
- the opening's own bank and set;
- the stereo / mono option and the volume setting;
- each area's reverb type;
- the elevator's and other scenes' C sound calls;
- the original's frames-9-and-16 footsteps, if they prove right.
