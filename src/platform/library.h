/* The game library (docs/library.md): the player's disc imported once into library/ - beside the program, or
 * in $OM_HOME - with a manifest of what was made. The engine reads only the library (core/files.h). */
#ifndef LIBRARY_H
#define LIBRARY_H

/* the library ready at start-up: there and whole, else (with a window) the player picks their disc and it is
 * imported, the progress shown; headless, none is an error. 0: no library (the reason printed) */
int library_choose(int headless);

/* `openmizzurna --import <disc.cue>`: the disc imported, the progress on stderr; 0 on failure */
int library_import_from(const char *disc);

const char *library_dir(void);   /* "<home>/library/" */
const char *library_home(void);  /* "<home>/": beside the program, or $OM_HOME */

#endif
