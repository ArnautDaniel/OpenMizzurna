# The phone

Matthew's cell phone (src: `ftel.c`, `cui.c` `phone_incoming_update`). `scripts/phone.fs`, the actor `phone`.

## Dialling

- **Opening.** Choosing the phone in the item ring opens the dial. If a call is ringing, choosing it answers
  instead, except in the car.
- **While it is up:**
  - the clock is held (`phone-up`);
  - the field leaves the pad alone (`take-pad`: no look, no ring);
  - the band, the phone's picture (0), the caption and four digits are drawn.

  On foot Matthew takes it out ($61), keys holding it ($62), holds it up ($64), and listens ($65). If he hangs
  up he puts it away ($63). In the car or on a stool he only keys.
- **Keys** (the pad's mode 7):
  - a face button keys a symbol with its tone: ○ 0 (sound 1), ✕ 1 (2), □ 2 (4), △ 3 (3);
  - a shoulder button clears what is keyed, or with nothing keyed hangs up.

  The number is the four symbols, the first the lowest: d0 + 4·d1 + 16·d2 + 64·d3. The notebook's lines give
  each number with its symbols (063_2).
- **Ringing out.** At the fourth symbol the caption becomes "It's ringing." and the common sequence 3 rings for
  62 game frames. Then the dial closes and the number is called.

## Who answers (src: func_8004B9E4 and the handlers before it)

- Calling from the place the number rings (its area), the C's "right here" script plays.
- **Morgan (40) and Winona (195):** by where they are (stop × 2, + 1 while walking): `phone-morgan` and
  `phone-winona` give the script, and another one when that one played last time.
  - Morgan's pack is by the time of day; three night calls (1:00–5:59) are their own, and the 27th about
    noon is too.
  - Winona has a few times of her own (the 27th, the 28th, the nights).
- **By the time** (the general store, the hospital, Emma's house, the school, the diner, Cohen's lodge, Mary
  Lou's, Nozz's apartment, Morgan's house, Liz's, Dennis', James'):
  - the table's first row whose time is still ahead gives the script, or the other when it played last;
  - the last scripts are kept in the session block ($64..$74);
  - an answer that isn't one of the pack's first five makes the number known (`numbers-known`, the
    notebook's).
  - Some have their own first answers: the hospital and the school on the 25th, Cohen away, Dennis out back,
    Emma's house with Winona there.
- **Others:** an unlisted number (188) by day; Dr. Gruber's office once he expects the call; the party
  committee once heard of; the auto garage when the car has a way out. Club Wolves, Dr. Gruber's house,
  Isabella's and 82 don't answer. Any other number is the wrong number.

## The story's calls (src: phone_incoming_update, phone_answer)

- **Calls 1–10** ring at their times when their conditions hold: a day kept, story bits, where someone is.
- **Ringing.** The common sequence 2 rings for 254 game frames, then the call is dropped. Call 9 dropped comes
  again 3 minutes later, with him out of the car.
- **Answering.**
  - The call's effects are set: a day kept, a counter, a bit toggled.
  - Matthew holds it to his ear ($68, then $66) and pack 276's script plays (call 9: the C's own).
  - When it ends, the C's hang-up script plays: `call-over` ($67) frees the line.

## Facts and messages

- Facts: `phone-up?`, `ringing?`, `in-call?`. A number answered is sent to the notebook (`learn-number`).
- The phone hears `ring-chosen` for the item 0.

## Status

**Built:** dialling, every number, the story's calls, answering and hanging up.
