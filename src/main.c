/* OpenMizzurna: start-up and the main loop.
 *
 *   openmizzurna [options]
 *     --hidden          no window shown (tests, screenshots)
 *     --frames N        stop after N frames
 *     --eval "code"     run Forth after the scripts have loaded (repeatable)
 *     --test file.fs    run a test file after the scripts, headless, and exit with its status
 *     --import disc.cue the disc imported into the library, then out
 *
 * A release build first brings itself up to date (platform/update.c).
 *
 * The game's data is the library (platform/library.c, docs/library.md): on the first run, with no library, the
 * player picks their disc and it is imported.
 *
 * Start-up: the window (SDL3, OpenGL 4.6 core), the renderer, the Forth system with the engine's words and
 * the actor kernel, then scripts/prelude.fs and scripts/game.fs - which decide everything else. Each
 * frame: input, simulation steps at a fixed rate as real time asks (engine_tick: hooks, tasks, the actors), then
 * drawing at the display's rate (vsync), blending the last two steps. */
#include "forth/forth.h"
#include "game/engine.h"
#include "game/movie.h"
#include "audio/audio.h"
#include "kernel/mactor.h"
#include "platform/library.h"
#include "platform/update.h"
#include "platform/platform.h"
#include "render/render.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX_EVALS 16

static int sTestStatus;
static void p_exit_status(Forth *f, Word *w) {   /* ( n -- ) a test's exit status (--test) */
    (void)w;
    sTestStatus = (int)forth_pop(f);
}
static void p_error_count(Forth *f, Word *w) {   /* ( -- n ) errors raised (not in `catch`) so far */
    (void)w;
    forth_push(f, f->errors);
}

typedef struct Options {
    int hidden;
    long frames;   /* 0: run until closed */
    const char *evals[MAX_EVALS];
    int nevals;
    const char *test;
    const char *import;
} Options;

static int parse_options(int argc, char **argv, Options *o) {
    int i;

    memset(o, 0, sizeof(*o));
    for (i = 1; i < argc; i++) {
        if (strcmp(argv[i], "--hidden") == 0) {
            o->hidden = 1;
        } else if (strcmp(argv[i], "--frames") == 0 && i + 1 < argc) {
            o->frames = atol(argv[++i]);
        } else if (strcmp(argv[i], "--eval") == 0 && i + 1 < argc && o->nevals < MAX_EVALS) {
            o->evals[o->nevals++] = argv[++i];
        } else if (strcmp(argv[i], "--import") == 0 && i + 1 < argc) {
            o->import = argv[++i];
        } else if (strcmp(argv[i], "--test") == 0 && i + 1 < argc) {
            o->test = argv[++i];
            o->hidden = 1;
        } else {
            fprintf(stderr, "usage: openmizzurna [--hidden] [--frames N] [--eval code] [--test file.fs] [--import disc.cue]\n");
            return 0;
        }
    }
    return 1;
}

/* the scripts' folder: $OM_SCRIPTS, else the one this build was made from, else scripts/ beside the program */
static const char *script_dir(void) {
    static char dir[1024];
    const char *env = getenv("OM_SCRIPTS");

    if (env != NULL) {
        return env;
    }
    if (OM_SCRIPT_DIR[0]) {
        return OM_SCRIPT_DIR;
    }
    snprintf(dir, sizeof(dir), "%sscripts", SDL_GetBasePath());
    return dir;
}

static int start_scripts(Engine *e) {
    char path[1100];
    const char *dir = script_dir();

    e->forth = forth_new(16 << 20);
    forth_set_output(e->forth, console_output, &e->console);
    bind_engine(e->forth);
    bind_mactor(e->forth);
    forth_prim(e->forth, "exit-status", p_exit_status);
    forth_prim(e->forth, "error-count", p_error_count);
    forth_constant(e->forth, "hidden?", e->hidden ? -1 : 0);
    forth_constant(e->forth, "boot-mode", e->boot_mode);
    forth_add_root(e->forth, dir);
    snprintf(path, sizeof(path), "%s/prelude.fs", dir);
    if (forth_include(e->forth, path) != 0) {
        return 0;
    }
    snprintf(path, sizeof(path), "%s/game.fs", dir);
    return forth_include(e->forth, path) == 0;
}

static void draw(Engine *e) {
    platform_size(&e->width, &e->height);
    render_begin(e->width, e->height, e->clear[0], e->clear[1], e->clear[2]);
    engine_draw_2d(e);
    console_draw(&e->console, e->width, e->height);
    render_end();
    if (e->screenshot[0] && render_screenshot(e->screenshot)) {
        forth_printf(e->forth, "saved %s\n", e->screenshot);
        e->screenshot[0] = 0;
    }
    platform_swap();
}

/* --test: its folder (and the one above, for shared helpers) are roots for its USING:s */
static int run_test(Engine *e, const char *test) {
    char root[1024], *slash;

    snprintf(root, sizeof(root), "%s", test);
    slash = strrchr(root, '/');
    if (slash != NULL) {
        *slash = 0;
        forth_add_root(e->forth, root);
        slash = strrchr(root, '/');
        if (slash != NULL) {
            *slash = 0;
            forth_add_root(e->forth, root);
        }
    }
    return forth_include(e->forth, test) != 0 ? 1 : sTestStatus;
}

int main(int argc, char **argv) {
    static const bool kNoKeys[SDL_SCANCODE_COUNT];
    Engine *e = &gEngine;
    Options opt;
    Uint64 last, lag = 0;
    const Uint64 tick_ns = 1000000000ull / STEPS_PER_SECOND;
    long frame = 0;
    int typing = 0, i, status = 0;

    if (!parse_options(argc, argv, &opt)) {
        return 2;
    }
    if (opt.import != NULL) {
        return library_import_from(opt.import) ? 0 : 1;
    }
    if (!opt.hidden && opt.test == NULL && opt.nevals == 0) {   /* (played, not a tool's run) */
        update_on_start(argv);
    }
    if (!platform_open("OpenMizzurna", 1280, 960, opt.hidden) || !render_init()) {
        return 1;
    }
    /* the library: a headless run goes on without one (the engine's own tests need none) */
    if (!library_choose(opt.hidden) && !opt.hidden) {
        return 1;
    }
    e->hidden = opt.hidden;
    au_init(!opt.hidden);   /* (headless: the sound's state without a device - tests run the mixer themselves) */
    platform_size(&e->width, &e->height);
    if (!start_scripts(e)) {
        fprintf(stderr, "openmizzurna: the scripts didn't load (see above); the console is open\n");
        e->console.open = 1;
    }
    for (i = 0; i < opt.nevals; i++) {
        forth_eval(e->forth, opt.evals[i], strlen(opt.evals[i]), "--eval");
    }
    if (opt.test != NULL) {
        status = run_test(e, opt.test);
        goto done;
    }

    last = SDL_GetTicksNS();
    while (!e->forth->bye) {
        Uint64 now = SDL_GetTicksNS();
        int ticks = 0;

        platform_poll(&e->input);
        if (e->input.quit) {
            break;
        }
        if (e->input.fullscreen_toggle) {
            static int fullscreen;
            fullscreen = !fullscreen;
            platform_fullscreen(fullscreen);
        }
        if (e->input.pressed[SDL_SCANCODE_GRAVE]) {   /* ` opens and shuts the console */
            e->console.open = !e->console.open;
            e->input.pressed[SDL_SCANCODE_GRAVE] = false;
        }
        if (e->console.open != typing) {
            typing = e->console.open;
            platform_text_input(typing);
        }
        if (e->console.open) {   /* (the game sees no keys while typing) */
            console_update(&e->console, &e->input, e->forth);
            e->input.keys = kNoKeys;
            memset(e->input.pressed, 0, sizeof(e->input.pressed));
        }

        /* fixed ticks: as many as real time asks for (at least one a frame in --frames runs) */
        lag += now - last;
        last = now;
        if (opt.frames > 0 && lag < tick_ns) {
            lag = tick_ns;
        }
        while (lag >= tick_ns && ticks < 4) {
            engine_tick(e);
            memset(e->input.pressed, 0, sizeof(e->input.pressed));   /* (presses count once) */
            lag -= tick_ns;
            ticks++;
        }
        if (ticks == 4) {
            lag = 0;   /* (far behind - a stall: don't try to catch up) */
        }
        if (e->reboot) {   /* back to the title (or an ending): the scripts' world afresh, as the original's sessions */
            e->reboot = 0;
            au_all_off();
            au_xa_stop();
            movie_close();
            mactor_reset();
            forth_free(e->forth);
            e->ntick_hooks = e->ndraw_hooks = 0;
            memset(e->held, 0, sizeof(e->held));
            if (!start_scripts(e)) {
                e->console.open = 1;
            }
        }
        e->step_blend = (float)lag / (float)tick_ns;   /* (drawing every display frame: vsync paces it) */
        draw(e);
        if (opt.frames > 0 && ++frame >= opt.frames) {
            break;
        }
    }

done:
    au_quit();
    forth_free(e->forth);
    render_shutdown();
    platform_close();
    return status;
}
