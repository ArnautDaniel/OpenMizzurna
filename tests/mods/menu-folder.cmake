# The mods menu's tests' folder of mods (CMakeLists.txt: mods_menu): copies of the sample and two test mods, no
# order.txt yet.  cmake -DSRC=<source> -DDIR=<folder> -P menu-folder.cmake
file(REMOVE_RECURSE "${DIR}")
file(COPY "${SRC}/examples/mods/clock-line" "${SRC}/tests/mods/layer-a" "${SRC}/tests/mods/future-mod" DESTINATION "${DIR}")
