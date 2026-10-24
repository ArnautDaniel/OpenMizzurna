/* `forth`: ClankForth's tool with the actor kernel bound (its tests: tests/test_actors.fs). */
#include "forth.h"
#include "../src/kernel/mactor.h"

int main(int argc, char **argv) { return forth_tool_main(argc, argv, bind_mactor); }
