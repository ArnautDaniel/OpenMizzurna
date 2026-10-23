/* The town's roads for the scripts (engine words; docs/subsystems/traffic.md): the road graph the library holds
 * as text (data/roads.h: cdbin/033_4/5/1.roads.txt the nodes, 2.roads.txt the bends), read once when first
 * asked for. */
#include "engine.h"

#include "../core/files.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define PRIM(name) static void name(Forth *f, Word *w)
#define POP() forth_pop(f)
#define PUSH(x) forth_push(f, (Cell)(x))

#define NODES_FILE "cdbin/033_4/5/1.roads.txt"
#define POINTS_FILE "cdbin/033_4/5/2.roads.txt"

typedef struct RoadNode { int x, y, z, points, first; } RoadNode;
typedef struct RoadPoint { int dx, dy, dz, length; } RoadPoint;

static int sTried;
static RoadNode *sNodes;
static RoadPoint *sPoints;
static int sNNodes, sNPoints;

/* the lines of a roads text: a count line, then a line an item, parsed by `item` */
static void *read_list(const char *path, const char *count_fmt, size_t size, int *n,
                       int (*item)(const char *line, void *items, int count)) {
    size_t len;
    char *text = (char *)files_read(path, &len), *line, *end;
    void *items = NULL;

    *n = 0;
    if (text == NULL) {
        return NULL;
    }
    for (line = text; line != NULL && *line; line = end != NULL ? end + 1 : NULL) {
        int count;

        end = strchr(line, '\n');
        if (end != NULL) {
            *end = 0;
        }
        if (items == NULL && sscanf(line, count_fmt, &count) == 1 && count > 0) {
            items = calloc((size_t)count, size);
            *n = count;
        } else if (items != NULL) {
            item(line, items, *n);
        }
    }
    free(text);
    return items;
}

static int node_line(const char *line, void *items, int count) {
    RoadNode r;
    int i;

    if (sscanf(line, "node %d at %d %d %d points %d first %d", &i, &r.x, &r.y, &r.z, &r.points, &r.first) != 6 ||
        i < 0 || i >= count) {
        return 0;
    }
    ((RoadNode *)items)[i] = r;
    return 1;
}

static int point_line(const char *line, void *items, int count) {
    RoadPoint r;
    int i;

    if (sscanf(line, "point %d step %d %d %d length %d", &i, &r.dx, &r.dy, &r.dz, &r.length) != 5 || i < 0 ||
        i >= count) {
        return 0;
    }
    ((RoadPoint *)items)[i] = r;
    return 1;
}

static void roads_load(void) {
    if (!sTried) {
        sTried = 1;
        sNodes = read_list(NODES_FILE, "nodes %d", sizeof(RoadNode), &sNNodes, node_line);
        sPoints = read_list(POINTS_FILE, "points %d", sizeof(RoadPoint), &sNPoints, point_line);
    }
}

PRIM(p_road_node) {   /* ( i -- x y z points first ) a node: where it is, the bends leaving it (0s: none) */
    Cell i = POP();
    RoadNode r = {0, 0, 0, 0, 0};

    roads_load();
    if (i >= 0 && i < sNNodes) {
        r = sNodes[i];
    }
    PUSH(r.x), PUSH(r.y), PUSH(r.z), PUSH(r.points), PUSH(r.first);
}
PRIM(p_road_point) {   /* ( i -- dx dy dz length ) a bend: its step and length (0s: none) */
    Cell i = POP();
    RoadPoint r = {0, 0, 0, 0};

    roads_load();
    if (i >= 0 && i < sNPoints) {
        r = sPoints[i];
    }
    PUSH(r.dx), PUSH(r.dy), PUSH(r.dz), PUSH(r.length);
}
PRIM(p_roads_ready) { roads_load(); PUSH(sNNodes > 0 && sNPoints > 0 ? -1 : 0); }   /* ( -- flag ) */

void bind_roads(Forth *f) {
    forth_prim(f, "road-node", p_road_node);
    forth_prim(f, "road-point", p_road_point);
    forth_prim(f, "roads-ready?", p_roads_ready);
}
