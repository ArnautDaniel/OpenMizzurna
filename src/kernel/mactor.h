/* The actor kernel: MActors ("Mizzurna actors" - not to be confused with `Actor`, the original's character
 * record and the engine's animated figure; the kernel was OpenHuey's HActor) - 
 * model in game/actor.h) with private state, a behaviour (a table of message handlers written
 * in Forth) and messages queued between them. C keeps the actors, the queue and the schedule;
 * Forth declares messages, behaviours and state types and writes the handlers.
 *
 * A frame: `tick` to its subscribers, deliver, `frame-end` to its subscribers, deliver. Deliver
 * runs in rounds - what a round sends waits for the next - until the queue is empty (or a
 * limit: the rest waits for the next frame). Everything is first in, first out, and
 * broadcasts go in spawn order, so a frame always runs the same way.
 *
 * Docs: docs/actors.md. */
#ifndef MACTOR_H
#define MACTOR_H

#include "../forth/forth.h"

#define MACTOR_MAX 256      /* actors alive at once */
#define MMSG_ARGS 16        /* cells a message carries */
#define MKINDS_MAX 512      /* message kinds */
#define MNAME_MAX 31

/* the kernel's own message kinds */
enum { MK_TICK, MK_FRAME_END, MK_SPAWNED, MK_KILLED, MK_HELD_TICK, MK_KERNEL_KINDS };

typedef struct MBehaviour MBehaviour;

/* where an actor physically is: a fact anyone may read, changed only by the actor itself
 * (docs/design.md, "Bodies"): the area it is in (a building, or the town outside), the floor of that
 * area it stands on (the area's walkable levels), where, which way it faces, how big it is. */
typedef struct MBody {
    int has, area, floor;
    float pos[3];           /* x y z: a vector the vec words take (body-at) */
    float yaw, radius, height;
    int has_head;           /* its head is told (body-head!): where others look at it */
    float head[3];          /* (the model's head bone; else its top: pos + height) */
} MBody;

/* actor `id`'s body (NULL: no such actor); the actor being run (-1: none) */
MBody *mactor_body(int id);
int mactor_self(void);
const char *mactor_name(int id);   /* its name, "" if none */
int mactor_kind(const char *name);   /* a message kind by name, -1 if there is none */

/* the words (vocabulary `actors`) */
void bind_mactor(Forth *f);
/* one frame of the actors (the engine calls it each tick) */
void mactor_frame(Forth *f);
/* a frame while the world is held (`pause!`: a menu up): `held-tick` to its subscribers only,
 * then deliver - messages still go to everyone */
void mactor_held_frame(Forth *f);
/* forget every actor and queued message (behaviours, kinds and state types stay) */
void mactor_reset(void);
/* from C: queue a message (from -1: the engine) / to every subscriber */
void mactor_send(int from, int to, int kind, const Cell *args, int n);
void mactor_broadcast(int from, int kind, const Cell *args, int n);

#endif
