/* OpenMizzurna's side of the actor kernel (ClankForth's: external/clankforth/src/actors.h, its docs/actors.md):
 * each actor's body - where it is in the world, as anyone may read and only it may change - and the words for it. */
#include "mactor.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define PRIM(name) static void name(Forth *f, Word *w __attribute__((unused)))
#define PUSH(x) forth_push(f, (Cell)(x))
#define POP() forth_pop(f)

static Forth *sForth;   /* (the game's one: the engine's C asks for bodies without one at hand) */

MBody *mactor_body(int id) { return sForth != NULL ? forth_actor_body(sForth, id) : NULL; }
int mactor_self(void) { return sForth != NULL ? forth_actor_self(sForth) : -1; }
const char *mactor_name(int id) { return sForth != NULL ? forth_actor_name(sForth, id) : ""; }
int mactor_kind(const char *name) { return sForth != NULL ? forth_actor_kind(sForth, name) : -1; }
void mactor_frame(Forth *f) { forth_actors_frame(f); }
void mactor_held_frame(Forth *f) { forth_actors_held_frame(f); }
void mactor_reset(void) {
    if (sForth != NULL) {
        forth_actors_reset(sForth);
    }
}
void mactor_send(int from, int to, int kind, const Cell *args, int n) { forth_actor_send(sForth, from, to, kind, args, n); }
void mactor_broadcast(int from, int kind, const Cell *args, int n) { forth_actor_broadcast(sForth, from, kind, args, n); }

/* ---- bodies: set by their own actor, read by anyone ---- */
/* the actor being run's body (an error if none is) */
static MBody *me(Forth *f, const char *what) {
    MBody *b = mactor_body(forth_actor_self(f));

    if (b == NULL) {
        forth_error(f, "%s", what);
    }
    return b;
}
PRIM(p_body_place) {   /* ( area floor -- ) ( F: x y z -- ) */
    MBody *a = me(f, "body-place (no actor is being run)");

    a->floor = (int)POP();
    a->area = (int)POP();
    a->pos[2] = (float)forth_fpop(f);
    a->pos[1] = (float)forth_fpop(f);
    a->pos[0] = (float)forth_fpop(f);
    a->has = 1;
}
PRIM(p_body_turn) { me(f, "body-turn (no actor is being run)")->yaw = (float)forth_fpop(f); }   /* ( F: yaw -- ) */
PRIM(p_body_size) {   /* ( F: radius height -- ) */
    MBody *a = me(f, "body-size (no actor is being run)");

    a->height = (float)forth_fpop(f);
    a->radius = (float)forth_fpop(f);
}
PRIM(p_body_off) { me(f, "body-off (no actor is being run)")->has = 0; }   /* out of the world (not in any area) */
/* actor id's body: it must be there, and in the world */
static MBody *bodied(Forth *f, Cell id, const char *what) {
    MBody *b = mactor_body((int)id);   /* (NULL for an id whose actor is gone) */

    if (b == NULL) {
        forth_error(f, "%s: no actor %ld", what, (long)id);
    }
    if (!b->has) {
        forth_error(f, "%s: %s has no body", what, mactor_name((int)id));
    }
    return b;
}
PRIM(p_has_body) {   /* ( id -- flag ) */
    MBody *b = mactor_body((int)POP());   /* (NULL for an id whose actor is gone) */

    PUSH(b != NULL && b->has ? -1 : 0);
}
PRIM(p_body_area) { PUSH(bodied(f, POP(), "body-area")->area); }
PRIM(p_body_floor) { PUSH(bodied(f, POP(), "body-floor")->floor); }
PRIM(p_body_pos) {   /* ( id -- ) ( F: -- x y z ) */
    MBody *a = bodied(f, POP(), "body-pos");

    forth_fpush(f, a->pos[0]);
    forth_fpush(f, a->pos[1]);
    forth_fpush(f, a->pos[2]);
}
PRIM(p_body_yaw) { forth_fpush(f, bodied(f, POP(), "body-yaw")->yaw); }
PRIM(p_body_at) { PUSH(bodied(f, POP(), "body-at")->pos); }   /* ( id -- v ) its position as a vector (read it, don't write it) */
PRIM(p_body_head) {   /* ( id -- v ) where its head is (as its owner tells: else the top of its body) */
    MBody *a = bodied(f, POP(), "body-head");

    if (!a->has_head) {
        a->head[0] = a->pos[0];
        a->head[1] = a->pos[1] + a->height;
        a->head[2] = a->pos[2];
    }
    PUSH(a->head);
}
PRIM(p_body_head_store) {   /* ( F: x y z -- ) my head is there */
    MBody *a = me(f, "body-head! (no actor is being run)");

    a->head[2] = (float)forth_fpop(f);
    a->head[1] = (float)forth_fpop(f);
    a->head[0] = (float)forth_fpop(f);
    a->has_head = 1;
}
PRIM(p_body_floor_store) { me(f, "body-floor! (no actor is being run)")->floor = (int)POP(); }   /* ( floor -- ) */
PRIM(p_body_area_store) { me(f, "body-area! (no actor is being run)")->area = (int)POP(); }   /* ( area -- ) */
PRIM(p_body_dims) {   /* ( id -- ) ( F: -- radius height ) */
    MBody *a = bodied(f, POP(), "body-dims");

    forth_fpush(f, a->radius);
    forth_fpush(f, a->height);
}

void bind_mactor(Forth *f) {
    static const struct { const char *name; Code code; const char *effect; } words[] = {
        {"body-place", p_body_place, "( area floor -- ) ( F: x y z -- )"}, {"body-turn", p_body_turn, "( F: yaw -- )"},
        {"body-size", p_body_size, "( F: radius height -- )"}, {"body-off", p_body_off, "( -- )"},
        {"body?", p_has_body, "( id -- flag )"}, {"body-area", p_body_area, "( id -- area )"},
        {"body-floor", p_body_floor, "( id -- floor )"}, {"body-pos", p_body_pos, "( id -- ) ( F: -- F: x y z )"},
        {"body-yaw", p_body_yaw, "( id -- ) ( F: -- F: yaw )"},
        {"body-dims", p_body_dims, "( id -- ) ( F: -- F: radius height )"}, {"body-at", p_body_at, "( id -- v )"},
        {"body-head", p_body_head, "( id -- v )"}, {"body-head!", p_body_head_store, "( F: x y z -- )"},
        {"body-floor!", p_body_floor_store, "( floor -- )"}, {"body-area!", p_body_area_store, "( area -- )"},
    };
    Vocab *saved = forth_current(f);
    size_t i;

    sForth = f;
    forth_actors(f, sizeof(MBody));
    forth_set_current(f, forth_vocab(f, "actors"));   /* (the bodies' words beside the kernel's) */
    for (i = 0; i < sizeof(words) / sizeof(words[0]); i++) {
        forth_set_effect(f, forth_prim(f, words[i].name, words[i].code), words[i].effect);
    }
    forth_set_current(f, saved);
}
