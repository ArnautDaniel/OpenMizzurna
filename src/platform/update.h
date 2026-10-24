/* The release's updater: on start, a newer published release replaces this one, then runs (update.c). */
#ifndef OM_UPDATE_H
#define OM_UPDATE_H

/* a release build (OM_VERSION set) brings itself up to date and runs the new version in its place (this call
 * then doesn't return); with nothing newer, or anything failing, it returns and this version runs */
void update_on_start(char **argv);

#endif
