# The notebook

Matthew's notebook (src: `fhbook.c`). `scripts/notebook.fs`, the actor `notebook`.

- **Opening.** Choosing the notebook in the item ring opens it. The world stands still while it is up
  (`ui-lock`).
- **The menu.** The pad picture (CD.BIN 042_1 and 042_2, two halves) is drawn at (−136, −96) to (136, 96),
  with the ✕ EXIT hint. Two lines sit at (−54, −40), in the dialogue font's brown palette (6): "Characters" and
  "Phone Numbers". The hand points at the chosen one.
  - Up and down choose;
  - ○ (or □) opens its page;
  - ✕ closes.
- **The people.** Those he knows, in the order of `notebook-people` (bits of `bios-known`), one at a time:
  - the portrait (kind, `portraits/<kind>.png`) at (−112, −88);
  - "Name" and the name, "Job" and the job, in the list font (the list text);
  - the page's lines (063_2) at (−112, 8).

  Up and down go to the one before and after; the arrows show when there are more.
- **The numbers.** Those he knows (bits of `numbers-known`), each 063_2's line 2 + its bit: the place and the
  number's buttons. They show eight to a page, with a gap after the fourth. Up and down turn a page.
- **✕** on a page goes back to the menu.

## Facts and messages

- `bios-known` starts as $2B4092 and `numbers-known` as $80F (src: notebook_init). Read them with
  `bio-known? ( bit )` and `number-known? ( bit )`.
- `learn-bio ( bit )` and `learn-number ( bit )` add to them:
  - a talk with someone: their page (src: character_met: `character-bios` by character; 101 and 111 their
    own);
  - a number answered (phone.fs);
  - the garage flyer read (area 1);
  - the funeral (area 7) and the newspaper archive (area 16).

## Status

**Built:** the menu, both pages, and what makes people and numbers known.
