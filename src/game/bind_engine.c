/* The engine's Forth words (vocabulary `engine`): input, the tick and draw hooks, 2D drawing, the HUD line,
 * screenshots, the console. The game's own words (the world, models, sound ...) join here as their
 * subsystems are built. docs/forth.md lists them. */
#include "engine.h"

#include "../kernel/mactor.h"
#include "../render/render.h"

#include <stdio.h>
#include <string.h>

Engine gEngine;

#define PRIM(name) static void name(Forth *f, Word *w)
#define POP() forth_pop(f)
#define PUSH(x) forth_push(f, (Cell)(x))

static char *cstring(Forth *f, char *buf, size_t size) {   /* ( addr len -- ) into buf */
    Cell len = POP();
    const char *s = (const char *)POP();

    if (len < 0 || (size_t)len >= size) {
        forth_error(f, "a string of %ld characters is too long here", (long)len);
    }
    memcpy(buf, s, (size_t)len);
    buf[len] = 0;
    return buf;
}

/* ---- input ---- */
PRIM(p_key_name) {   /* key: ( "name" -- scancode ) an SDL key name, _ for spaces */
    size_t len;
    const char *name = forth_parse_name(f, &len);
    char buf[64];
    size_t i;
    SDL_Scancode sc;

    if (len == 0 || len >= sizeof(buf)) {
        forth_error(f, "key: wants a key name");
    }
    for (i = 0; i < len; i++) {
        buf[i] = name[i] == '_' ? ' ' : name[i];
    }
    buf[len] = 0;
    sc = SDL_GetScancodeFromName(buf);
    if (sc == SDL_SCANCODE_UNKNOWN) {
        forth_error(f, "key: no key is called %s", buf);
    }
    if (f->compiling) {
        forth_compile_literal(f, sc);
    } else {
        PUSH(sc);
    }
}
static int scancode(Forth *f, Cell k) {
    if (k < 0 || k >= SDL_SCANCODE_COUNT) {
        forth_error(f, "%ld is not a key", (long)k);
    }
    return (int)k;
}
PRIM(p_key_down) {   /* ( scancode -- flag ) held now */
    int k = scancode(f, POP());
    PUSH((gEngine.input.keys != NULL && gEngine.input.keys[k]) || gEngine.held[k] ? -1 : 0);   /* (no poll yet: --test) */
}
PRIM(p_key_pressed) {   /* ( scancode -- flag ) went down this tick */
    int k = scancode(f, POP());
    PUSH(gEngine.input.pressed[k] ? -1 : 0);
}
PRIM(p_key_hold) {   /* ( scancode flag -- ) held (or let go) from a script; pressed the tick it is held */
    Cell on = POP();
    int k = scancode(f, POP());

    if (on && !gEngine.held[k]) {
        gEngine.input.pressed[k] = true;
    }
    gEngine.held[k] = on != 0;
}

/* ---- hooks: words run every tick / every frame's 2D ---- */
static void hook_add(Forth *f, Word **list, int *n, Word *xt) {
    if (*n == MAX_HOOKS) {
        forth_error(f, "too many hooks");
    }
    list[(*n)++] = xt;
}
static void hook_remove(Word **list, int *n, Word *xt) {
    int i;

    for (i = 0; i < *n; i++) {
        if (list[i] == xt) {
            memmove(list + i, list + i + 1, sizeof(*list) * (size_t)(*n - i - 1));
            (*n)--;
            return;
        }
    }
}
PRIM(p_on_tick) { hook_add(f, gEngine.tick_hooks, &gEngine.ntick_hooks, (Word *)POP()); }
PRIM(p_off_tick) { hook_remove(gEngine.tick_hooks, &gEngine.ntick_hooks, (Word *)POP()); }
PRIM(p_on_draw) { hook_add(f, gEngine.draw_hooks, &gEngine.ndraw_hooks, (Word *)POP()); }
PRIM(p_off_draw) { hook_remove(gEngine.draw_hooks, &gEngine.ndraw_hooks, (Word *)POP()); }
PRIM(p_ticks) { PUSH(gEngine.ticks); }
PRIM(p_dt) { forth_fpush(f, 1.0 / STEPS_PER_SECOND); }   /* ( F: -- seconds ) a step's length */
PRIM(p_seconds) {   /* ( F: seconds -- steps ) as whole steps, at least one for any time > 0 (for `wait`) */
    Float s = forth_fpop(f);
    Cell n = (Cell)(s * STEPS_PER_SECOND + 0.5);
    PUSH(s > 0 && n < 1 ? 1 : n);
}
PRIM(p_step_blend) { forth_fpush(f, gEngine.step_blend); }   /* ( F: -- 0..1 ) (drawing) */

/* ---- 2D (inside an on-draw hook), in window pixels ---- */
PRIM(p_pen_color) { gEngine.pen = (uint32_t)POP(); }   /* ( rgba -- ) */
PRIM(p_pen_scale) { gEngine.pen_scale = (int)POP(); }   /* ( n -- ) */
PRIM(p_draw_text) {   /* ( addr len x y -- ) */
    Cell y = POP(), x = POP(), len = POP();
    const char *s = (const char *)POP();

    render_text((float)x, (float)y, (float)gEngine.pen_scale, gEngine.pen, s, (int)len);
}
PRIM(p_draw_rect) {   /* ( x y w h -- ) */
    Cell h = POP(), wd = POP(), y = POP(), x = POP();
    render_rect((float)x, (float)y, (float)wd, (float)h, gEngine.pen);
}
PRIM(p_screen_size) { PUSH(gEngine.width); PUSH(gEngine.height); }
PRIM(p_char_size) {
    PUSH((Cell)render_text_width((float)gEngine.pen_scale, 1));
    PUSH((Cell)render_line_height((float)gEngine.pen_scale));
}
PRIM(p_hud) { cstring(f, gEngine.hud, sizeof(gEngine.hud)); }   /* ( addr len -- ) a line at the bottom */
PRIM(p_clear_color) {   /* ( F: r g b -- ) */
    gEngine.clear[2] = (float)forth_fpop(f);
    gEngine.clear[1] = (float)forth_fpop(f);
    gEngine.clear[0] = (float)forth_fpop(f);
}

/* ---- tools ---- */
PRIM(p_screenshot) { cstring(f, gEngine.screenshot, sizeof(gEngine.screenshot)); }   /* ( addr len -- ) */
PRIM(p_screenshot_pending) { PUSH(gEngine.screenshot[0] ? -1 : 0); }
PRIM(p_console_store) { gEngine.console.open = POP() != 0; }   /* ( flag -- ) */

void bind_engine(Forth *f) {
    static const struct { const char *name; Code code; } kWords[] = {
        {"key:", p_key_name}, {"key-down?", p_key_down}, {"key-pressed?", p_key_pressed}, {"key-hold", p_key_hold},
        {"on-tick", p_on_tick}, {"off-tick", p_off_tick}, {"on-draw", p_on_draw}, {"off-draw", p_off_draw},
        {"ticks", p_ticks}, {"dt", p_dt}, {"seconds", p_seconds}, {"step-blend", p_step_blend}, {"pen-color", p_pen_color}, {"pen-scale", p_pen_scale}, {"draw-text", p_draw_text},
        {"draw-rect", p_draw_rect}, {"screen-size", p_screen_size}, {"char-size", p_char_size}, {"hud", p_hud},
        {"clear-color", p_clear_color}, {"screenshot", p_screenshot},
        {"screenshot-pending?", p_screenshot_pending}, {"console!", p_console_store},
    };
    Vocab *was = f->m.current, *v = forth_vocab(f, "engine");
    size_t i;
    Word *w;

    forth_set_current(f, v);
    v->state = VOCAB_LOADED;   /* (made here: no file to load) */
    for (i = 0; i < sizeof(kWords) / sizeof(kWords[0]); i++) {
        w = forth_prim(f, kWords[i].name, kWords[i].code);
        if (strcmp(kWords[i].name, "key:") == 0) {
            w->flags |= WORD_IMMEDIATE;   /* (a key name compiled as a number) */
        }
    }
    forth_set_current(f, was);
    gEngine.pen = 0xFFFFFFFF;
    gEngine.pen_scale = 2;
}

void engine_tick(Engine *e) {
    int i;

    for (i = 0; i < e->ntick_hooks; i++) {
        Word *hook = e->tick_hooks[i];

        if (forth_call(e->forth, hook) != 0) {   /* (a hook that errs is taken off) */
            forth_printf(e->forth, "the tick hook %s failed and was removed\n", hook->name);
            hook_remove(e->tick_hooks, &e->ntick_hooks, hook);
            i--;
        }
    }
    forth_run_tasks(e->forth);
    mactor_frame(e->forth);
    e->ticks++;
}

void engine_draw_2d(Engine *e) {
    int i;

    for (i = 0; i < e->ndraw_hooks; i++) {
        Word *hook = e->draw_hooks[i];

        if (forth_call(e->forth, hook) != 0) {
            forth_printf(e->forth, "the draw hook %s failed and was removed\n", hook->name);
            hook_remove(e->draw_hooks, &e->ndraw_hooks, hook);
            i--;
        }
    }
    if (e->hud[0]) {
        float s = 2.0f, y = (float)e->height - render_line_height(s) - 8.0f;
        render_text(8.0f, y, s, 0xFFFFFFFF, e->hud, (int)strlen(e->hud));
    }
}
