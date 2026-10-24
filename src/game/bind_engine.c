/* The engine's Forth words (vocabulary `engine`): input, the tick and draw hooks, 2D drawing, the HUD line,
 * screenshots, the console. The game's own words (the world, models, sound ...) join here as their
 * subsystems are built. docs/forth.md lists them. */
#include "engine.h"

#include "../core/files.h"
#include "../kernel/mactor.h"
#include "../render/render.h"
#include "../render/scene3d.h"
#include "../data/png.h"

#include <math.h>
#include <stdio.h>
#include <stdlib.h>
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

/* an element of a table of the executable by name and index (0 past its end, or with no table) */
static void p_exe_table(Forth *f, Word *w);
static Cell exe_element(Forth *f, int bytes, int is_signed) {
    Cell i = POP();
    Cell n, addr;
    const uint8_t *p;

    p_exe_table(f, NULL);   /* ( addr len -- addr n ) */
    n = POP();
    addr = POP();
    if (addr == 0 || i < 0 || (i + 1) * bytes > n) {
        return 0;
    }
    p = (const uint8_t *)addr + i * bytes;
    switch (bytes) {
    case 1: return is_signed ? (int8_t)p[0] : p[0];
    case 2: return is_signed ? (int16_t)(p[0] | p[1] << 8) : (uint16_t)(p[0] | p[1] << 8);
    default: return (int32_t)(p[0] | p[1] << 8 | p[2] << 16 | (uint32_t)p[3] << 24);
    }
}
PRIM(p_exe_u8) { PUSH(exe_element(f, 1, 0)); }    /* ( addr len i -- n ) element i of a table: a byte */
PRIM(p_exe_s8) { PUSH(exe_element(f, 1, 1)); }    /* ( addr len i -- n ) signed */
PRIM(p_exe_s16) { PUSH(exe_element(f, 2, 1)); }   /* ( addr len i -- n ) a signed 16-bit word */
PRIM(p_exe_s32) { PUSH(exe_element(f, 4, 1)); }   /* ( addr len i -- n ) a signed 32-bit word */

/* ---- the library's files ---- */
#define MAX_FILES 256
static struct {
    char path[160];
    uint8_t *data;
    size_t size;
} sFiles[MAX_FILES];
static int sNFiles;

PRIM(p_library_file) {   /* ( addr len -- addr n ) a library file's bytes (kept: read once); 0 0 if none */
    char path[160];
    int i;

    cstring(f, path, sizeof(path));
    for (i = 0; i < sNFiles; i++) {
        if (strcmp(sFiles[i].path, path) == 0) {
            PUSH(sFiles[i].data);
            PUSH(sFiles[i].size);
            return;
        }
    }
    if (sNFiles == MAX_FILES) {
        forth_error(f, "library-file: too many files");
    }
    snprintf(sFiles[sNFiles].path, sizeof(sFiles[sNFiles].path), "%s", path);
    sFiles[sNFiles].data = files_read(path, &sFiles[sNFiles].size);
    if (sFiles[sNFiles].data == NULL) {
        sFiles[sNFiles].size = 0;
    }
    PUSH(sFiles[sNFiles].data);
    PUSH(sFiles[sNFiles].size);
    sNFiles++;
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
PRIM(p_pen_blend) { render_blend((int)POP()); }   /* ( mode -- ) 0 as usual, 1 add, 2 subtract; 3 / 4 / 5 the PlayStation's add, half, opaque */
PRIM(p_draw_quad) {   /* ( x0 y0 x1 y1 x2 y2 x3 y3 c0 c1 c2 c3 -- ) corners 0 1 / 2 3, a colour each */
    uint32_t c[4];
    float xy[8];
    int k;

    for (k = 3; k >= 0; k--) {
        c[k] = (uint32_t)POP();
    }
    for (k = 7; k >= 0; k--) {
        xy[k] = (float)POP();
    }
    render_quad4(xy, c);
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

/* ---- the game's data ---- */
PRIM(p_exe_table) {   /* ( addr len -- addr n ) a table of the executable by name (data/exetables.c); 0 0 if none */
    char name[64], path[96];
    size_t n = 0;
    uint8_t *d;
    static uint8_t *loaded[64];
    static char names[64][64];
    static size_t sizes[64];
    static int nloaded;
    int i;

    cstring(f, name, sizeof(name));
    for (i = 0; i < nloaded; i++) {   /* (read once, kept: a table is a fact) */
        if (strcmp(names[i], name) == 0) {
            PUSH(loaded[i]);
            PUSH(sizes[i]);
            return;
        }
    }
    snprintf(path, sizeof(path), "exe/%s.bin", name);
    d = files_read(path, &n);
    if (d == NULL || nloaded == 64) {
        free(d);
        PUSH(0);
        PUSH(0);
        return;
    }
    snprintf(names[nloaded], sizeof(names[0]), "%s", name);
    loaded[nloaded] = d;
    sizes[nloaded++] = n;
    PUSH(d);
    PUSH(n);
}

/* ---- images: the library's pictures drawn in 2D (the sky) ---- */
#define MAX_IMAGES 128
static struct {
    char path[160];
    GpuTexture t;
} sImages[MAX_IMAGES];
static int sNImages;

PRIM(p_image) {   /* ( addr len -- image ) a picture from the library (a PNG); 0 if it isn't there */
    Cell len = POP();
    const char *s = (const char *)POP();
    char path[160];
    size_t n;
    uint8_t *file, *rgba;
    int i, iw = 0, ih = 0;

    snprintf(path, sizeof(path), "%.*s", (int)len, s);
    for (i = 0; i < sNImages; i++) {
        if (strcmp(sImages[i].path, path) == 0) {
            PUSH(sImages[i].t.id != 0 ? i + 1 : 0);
            return;
        }
    }
    if (sNImages == MAX_IMAGES) {
        forth_error(f, "image: too many images");
    }
    file = files_read(path, &n);
    rgba = file != NULL ? png_read_rgba(file, n, &iw, &ih) : NULL;
    free(file);
    snprintf(sImages[sNImages].path, sizeof(sImages[sNImages].path), "%s", path);
    sImages[sNImages].t = rgba != NULL ? render_texture_rgba(rgba, iw, ih) : (GpuTexture){0, 0, 0};
    free(rgba);
    sNImages++;
    PUSH(sImages[sNImages - 1].t.id != 0 ? sNImages : 0);
}
GpuTexture engine_image(Cell i) { return i >= 1 && i <= sNImages ? sImages[i - 1].t : (GpuTexture){0, 0, 0}; }
PRIM(p_image_size) {   /* ( image -- w h ) */
    Cell i = POP();
    PUSH(i >= 1 && i <= sNImages ? sImages[i - 1].t.w : 0);
    PUSH(i >= 1 && i <= sNImages ? sImages[i - 1].t.h : 0);
}
PRIM(p_draw_image) {   /* ( image x y w h -- ) in window pixels, tinted by the pen's colour (a negative size flips) */
    Cell h = POP(), wd = POP(), y = POP(), x = POP(), i = POP();

    if (i >= 1 && i <= sNImages && sImages[i - 1].t.id != 0) {
        render_image(sImages[i - 1].t, (float)x, (float)y, (float)wd, (float)h, gEngine.pen);
    }
}
PRIM(p_draw_image_quad) {   /* ( image x0 y0 x1 y1 x2 y2 x3 y3 -- ) on four corners (0 1 / 2 3), in window pixels, tinted */
    float xy[8];
    int k;
    Cell i;

    for (k = 7; k >= 0; k--) {
        xy[k] = (float)POP();
    }
    i = POP();
    if (i >= 1 && i <= sNImages && sImages[i - 1].t.id != 0) {
        render_image_quad(sImages[i - 1].t, xy, gEngine.pen);
    }
}
PRIM(p_draw_billboard) {   /* ( image x z bottom top half yaw grey blend -- ) an upright picture in the world, turned to
                             * face the heading yaw (4096 a turn); grey 128: as it is; blend as scene3d_quad's */
    Cell blend = POP(), grey = POP(), yaw = POP(), half = POP(), top = POP(), bottom = POP(), z = POP(), x = POP(),
         i = POP();
    float a = (float)yaw * 2.0f * 3.14159265f / 4096.0f, dc = (float)half * cosf(a), ds = (float)half * sinf(a);
    float corner[4][3] = {{x - dc, (float)top, z + ds}, {x + dc, (float)top, z - ds},
                          {x - dc, (float)bottom, z + ds}, {x + dc, (float)bottom, z - ds}};

    if (i >= 1 && i <= sNImages && sImages[i - 1].t.id != 0) {
        scene3d_quad(sImages[i - 1].t, corner, (float)grey / 128.0f, (int)blend);
    }
}
PRIM(p_draw_ground_quad) {   /* ( image x y z half grey blend -- ) a picture lying flat in the world, centred there,
                              * 2 half across; grey 128: as it is; blend as scene3d_quad's */
    Cell blend = POP(), grey = POP(), half = POP(), z = POP(), y = POP(), x = POP(), i = POP();
    float corner[4][3] = {{(float)(x - half), (float)y, (float)(z - half)}, {(float)(x + half), (float)y, (float)(z - half)},
                          {(float)(x - half), (float)y, (float)(z + half)}, {(float)(x + half), (float)y, (float)(z + half)}};

    if (i >= 1 && i <= sNImages && sImages[i - 1].t.id != 0) {
        scene3d_quad(sImages[i - 1].t, corner, (float)grey / 128.0f, (int)blend);
    }
}
PRIM(p_pad_rumble) {   /* ( port small? large -- ) the pad's motors this tick: the small on or off, the large 0..255 */
    Cell large = POP(), small = POP();
    platform_rumble((int)POP(), small != 0, (float)large / 255.0f);
}
PRIM(p_behind) { render_behind(POP() != 0); }   /* ( flag -- ) the 2D from now on behind the 3D (the sky) or over it */

/* ---- chance ---- */
PRIM(p_random) {   /* ( n -- 0..n-1 ) (a xorshift; seeded once from the clock) */
    static uint32_t state;
    Cell n = POP();

    if (state == 0) {
        state = (uint32_t)SDL_GetTicksNS() | 1u;
    }
    state ^= state << 13;
    state ^= state >> 17;
    state ^= state << 5;
    PUSH(n > 0 ? (Cell)(state % (uint32_t)n) : 0);
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
        {"draw-rect", p_draw_rect}, {"pen-blend", p_pen_blend}, {"draw-quad", p_draw_quad}, {"screen-size", p_screen_size}, {"char-size", p_char_size}, {"hud", p_hud},
        {"clear-color", p_clear_color}, {"random", p_random}, {"image", p_image}, {"image-size", p_image_size}, {"draw-image", p_draw_image}, {"draw-image-quad", p_draw_image_quad}, {"behind", p_behind}, {"pad-rumble", p_pad_rumble}, {"draw-billboard", p_draw_billboard}, {"draw-ground-quad", p_draw_ground_quad}, {"screenshot", p_screenshot},
        {"screenshot-pending?", p_screenshot_pending}, {"console!", p_console_store}, {"exe-table", p_exe_table}, {"library-file", p_library_file}, {"exe-u8", p_exe_u8}, {"exe-s8", p_exe_s8}, {"exe-s16", p_exe_s16}, {"exe-s32", p_exe_s32},
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
    bind_text(f);   /* (the text and fonts: text.c) */
    bind_scene(f);  /* (the 3D: bind_scene.c) */
    bind_roads(f);  /* (the roads: roads.c) */
    bind_records(f);   /* (records.c) */
    bind_audio(f);   /* (bind_audio.c: the sound) */
    bind_snow(f);      /* (snow.c) */
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
