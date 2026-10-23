/* The engine: what the main loop keeps between frames and the engine's Forth words (bind_engine.c) reach.
 * One instance, gEngine. */
#ifndef ENGINE_H
#define ENGINE_H

#include "../forth/forth.h"
#include "../platform/platform.h"
#include "console.h"

#define TICKS_PER_SECOND 30   /* the game's fixed tick (docs/design.md: "A frame") */
#define MAX_HOOKS 64

typedef struct Engine {
    Forth *forth;
    Input input;
    Console console;
    int width, height;           /* the window's drawable size */
    int hidden;                  /* no window shown (tests, screenshots) */
    long ticks;                  /* game ticks since start */
    bool held[SDL_SCANCODE_COUNT];   /* keys held by scripts (key-hold: demos, tests) */
    Word *tick_hooks[MAX_HOOKS];     /* on-tick */
    int ntick_hooks;
    Word *draw_hooks[MAX_HOOKS];     /* on-draw */
    int ndraw_hooks;
    uint32_t pen;                /* the 2D drawing words' colour (0xRRGGBBAA) and text size */
    int pen_scale;
    char hud[256];               /* a line at the bottom of the screen */
    char screenshot[512];        /* asked for: written after the next frame is drawn */
    float clear[3];
} Engine;

extern Engine gEngine;

void bind_engine(Forth *f);
/* one game tick: the scripts' hooks, their tasks, the actors' frame */
void engine_tick(Engine *e);
/* the 2D the scripts draw (on-draw hooks, the HUD line) */
void engine_draw_2d(Engine *e);

#endif
