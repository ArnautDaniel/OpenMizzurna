/* The release's updater (update.h).
 *
 * The rolling release (.github/workflows/release-linux.yml) publishes beside its tarball a version.txt (the
 * version it was built as) and the tarball under a fixed name. On start a release build reads version.txt; when
 * it names another version, the tarball is fetched and unpacked into a staging folder beside the program, its
 * scripts and then its program are moved over these, and the new program runs in this one's place. The player's
 * library/ and saves/ are never touched. The fetching is the system's curl and tar (no TLS library in the game);
 * offline, without them, or without write access to the program's folder, the game simply runs as it is.
 *
 * OM_NO_UPDATE=1 in the environment turns it off; OM_UPDATED marks a run that has just updated (it doesn't look
 * again); OM_UPDATE_URL is another place to update from (a folder ending in '/', as file:///...: the tests). */
#include "update.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#ifndef OM_VERSION
#define OM_VERSION ""
#endif

#if defined(__linux__)
#include <spawn.h>
#include <sys/stat.h>
#include <sys/wait.h>
#include <unistd.h>

extern char **environ;

#define RELEASE_URL "https://github.com/ArnautDaniel/OpenMizzurna/releases/download/latest/"
#define VERSION_FILE "version.txt"
#define PACKAGE_FILE "openmizzurna-linux-x86_64.tar.gz"
#define CHECK_TIMEOUT "5"        /* seconds: asking which version is current */
#define DOWNLOAD_TIMEOUT "300"   /* seconds: the package */
#define STAGING ".update"        /* beside the program: the new one unpacked */
#define OLD_SUFFIX ".old"

/* a program run to its end (no shell): 1 when it exited 0 */
static int run(char *const argv[]) {
    pid_t pid;
    int status;

    if (posix_spawnp(&pid, argv[0], NULL, NULL, argv, environ) != 0) {
        return 0;
    }
    while (waitpid(pid, &status, 0) < 0) {
    }
    return WIFEXITED(status) && WEXITSTATUS(status) == 0;
}

static void remove_tree(const char *path) {
    char *argv[] = {"rm", "-rf", "--", (char *)path, NULL};
    run(argv);
}

/* the program's folder, with a trailing '/' */
static int program_dir(char *dir, size_t n) {
    ssize_t len = readlink("/proc/self/exe", dir, n - 1);
    char *slash;

    if (len <= 0) {
        return 0;
    }
    dir[len] = 0;
    slash = strrchr(dir, '/');
    if (slash == NULL) {
        return 0;
    }
    slash[1] = 0;
    return 1;
}

static const char *release_url(void) {
    const char *url = getenv("OM_UPDATE_URL");
    return url != NULL ? url : RELEASE_URL;
}

/* the version published now (its first word), into `version` */
static int published_version(const char *staging, char version[128]) {
    char file[1200], url[1100];
    FILE *f;
    int ok;

    snprintf(file, sizeof(file), "%s/" VERSION_FILE, staging);
    snprintf(url, sizeof(url), "%s" VERSION_FILE, release_url());
    {
        char *argv[] = {"curl", "-fsL", "--max-time", CHECK_TIMEOUT, "-o", file, url, NULL};
        if (!run(argv)) {
            return 0;
        }
    }
    f = fopen(file, "r");
    if (f == NULL) {
        return 0;
    }
    ok = fscanf(f, "%127s", version) == 1;
    fclose(f);
    return ok;
}

/* `name` in the staging folder moved over the program's (the old one aside, then gone): 1 done */
static int replace(const char *dir, const char *staging, const char *name) {
    char from[1200], to[1200], aside[1300];

    snprintf(from, sizeof(from), "%s/%s", staging, name);
    snprintf(to, sizeof(to), "%s%s", dir, name);
    snprintf(aside, sizeof(aside), "%s" OLD_SUFFIX, to);
    remove_tree(aside);
    if (rename(to, aside) != 0) {
        aside[0] = 0;   /* (there was none) */
    }
    if (rename(from, to) != 0) {
        if (aside[0]) {
            rename(aside, to);
        }
        return 0;
    }
    if (aside[0]) {
        remove_tree(aside);
    }
    return 1;
}

void update_on_start(char **argv) {
    char dir[1024], staging[1100], package[1200], url[1100], program[1200], version[128];
    struct stat st;

    if (OM_VERSION[0] == 0 || getenv("OM_NO_UPDATE") != NULL || getenv("OM_UPDATED") != NULL) {
        return;
    }
    if (!program_dir(dir, sizeof(dir)) || access(dir, W_OK) != 0) {
        return;
    }
    snprintf(staging, sizeof(staging), "%s" STAGING, dir);
    remove_tree(staging);
    if (mkdir(staging, 0755) != 0) {
        return;
    }
    if (!published_version(staging, version) || strcmp(version, OM_VERSION) == 0) {
        remove_tree(staging);
        return;
    }
    fprintf(stderr, "openmizzurna: updating %s to %s\n", OM_VERSION, version);
    snprintf(package, sizeof(package), "%s/" PACKAGE_FILE, staging);
    snprintf(url, sizeof(url), "%s" PACKAGE_FILE, release_url());
    {
        char *get[] = {"curl", "-fsSL", "--max-time", DOWNLOAD_TIMEOUT, "-o", package, url, NULL};
        char *unpack[] = {"tar", "xzf", package, "-C", staging, "--strip-components=1", NULL};
        if (!run(get) || !run(unpack)) {
            fprintf(stderr, "openmizzurna: the update couldn't be fetched; running %s\n", OM_VERSION);
            remove_tree(staging);
            return;
        }
    }
    snprintf(program, sizeof(program), "%s/openmizzurna", staging);
    if (stat(program, &st) != 0 || !(st.st_mode & S_IXUSR)) {
        fprintf(stderr, "openmizzurna: the update is incomplete; running %s\n", OM_VERSION);
        remove_tree(staging);
        return;
    }
    /* the scripts first (failing there leaves this version whole), then the program */
    if (!replace(dir, staging, "scripts") || !replace(dir, staging, "openmizzurna")) {
        fprintf(stderr, "openmizzurna: the update couldn't be installed; running %s\n", OM_VERSION);
        remove_tree(staging);
        return;
    }
    replace(dir, staging, "README.md");
    replace(dir, staging, "LICENSE");
    replace(dir, staging, "licenses");
    remove_tree(staging);
    snprintf(program, sizeof(program), "%sopenmizzurna", dir);
    setenv("OM_UPDATED", version, 1);
    execv(program, argv);
    fprintf(stderr, "openmizzurna: updated to %s; start it again\n", version);
    exit(0);
}

#else
void update_on_start(char **argv) { (void)argv; }   /* (other systems: not yet) */
#endif
