/* OpenMizzurna's side of the actor kernel. The kernel is ClankForth's (external/clankforth/src/actors.h, its
 * docs/actors.md): actors with private state, a behaviour (message handlers written in Forth) and messages queued
 * between them. OpenMizzurna's part is each actor's body - where it is in the world - and these names for the
 * kernel's calls, as the engine's C makes them. ("MActor": Mizzurna actor - not `Actor`, the original's character
 * record and the engine's animated figure, game/actor.h; the kernel was OpenHuey's HActor.) Docs: docs/actors.md. */
#ifndef MACTOR_H
#define MACTOR_H

#include "actors.h"

#define MACTOR_MAX FORTH_ACTORS_MAX   /* actors alive at once */
#define MMSG_ARGS FORTH_MSG_ARGS      /* cells a message carries */

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

/* the kernel's words and the bodies' (vocabulary `actors`) */
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
