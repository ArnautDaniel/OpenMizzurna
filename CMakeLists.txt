cmake_minimum_required(VERSION 3.20)
project(openmizzurna VERSION 0.0.1 LANGUAGES C)

set(CMAKE_C_STANDARD 11)
set(CMAKE_C_STANDARD_REQUIRED ON)
set(CMAKE_C_EXTENSIONS ON)   # gnu11: __attribute__, M_PI
if(NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Debug)
endif()
add_compile_options(-Wall -Wextra -Wno-unused-parameter)

# A release (-DOM_RELEASE=ON) knows no paths of this machine: the scripts are found beside the executable.
option(OM_RELEASE "build for distribution (no built-in paths)" OFF)
get_filename_component(SCRIPT_DIR "${CMAKE_CURRENT_SOURCE_DIR}/scripts" ABSOLUTE)
if(OM_RELEASE)
    set(SCRIPT_DIR "")
endif()

# ---- the Forth system and the actor kernel (no SDL: the forth tool and the language tests) ----
add_library(forth STATIC src/forth/forth.c)
target_include_directories(forth PUBLIC src/forth)
target_link_libraries(forth PUBLIC m)

add_library(mactor STATIC src/kernel/mactor.c)
target_link_libraries(mactor PUBLIC forth)

add_executable(forth-tool src/forth/repl.c)
set_target_properties(forth-tool PROPERTIES OUTPUT_NAME forth)
target_link_libraries(forth-tool PRIVATE forth mactor)
target_compile_definitions(forth-tool PRIVATE "OM_SCRIPT_DIR=\"${SCRIPT_DIR}\"")

enable_testing()
file(GLOB FORTH_TESTS "${CMAKE_CURRENT_SOURCE_DIR}/tests/test_*.fs")
foreach(t ${FORTH_TESTS})
    get_filename_component(name "${t}" NAME_WE)
    add_test(NAME ${name} COMMAND forth-tool "${t}")
endforeach()

# ---- the game ----
find_package(SDL3 REQUIRED CONFIG)
# the game's data: the disc image, CD.BIN, the formats and their conversions (the engine's, the import's and
# the checks')
add_library(omdata STATIC src/data/disc.c src/data/cdbin.c src/data/container.c src/data/png.c src/data/tim.c
    src/data/text.c src/data/cdconv.c src/data/exetables.c src/data/vab.c src/data/seqmidi.c src/data/xa.c
    src/data/flacenc.c src/data/tmd.c src/data/vramctx.c src/data/modelgltf.c src/data/modelconv.c src/data/town.c src/data/str.c src/data/mfo.c src/data/movieconv.c src/data/motion.c src/data/roads.c src/data/billboards.c
    src/third_party/stb.c src/third_party/dr_flac.c src/third_party/cgltf.c)
target_include_directories(omdata PUBLIC src/third_party)
# the movies' encoder (the import) and decoder: libavcodec, when there is one
find_package(PkgConfig)
if(PkgConfig_FOUND)
    pkg_check_modules(AVCODEC IMPORTED_TARGET libavcodec libavutil)
endif()
# (a release links a static FFmpeg built with its MPEG-2 video alone: its private libraries come with it)
option(OM_STATIC_FFMPEG "link libavcodec statically (the release's own build of it)" OFF)
if(AVCODEC_FOUND AND OM_STATIC_FFMPEG)
    target_compile_definitions(omdata PUBLIC OM_HAVE_AVCODEC)
    target_include_directories(omdata PUBLIC ${AVCODEC_STATIC_INCLUDE_DIRS})
    target_link_libraries(omdata PUBLIC ${AVCODEC_STATIC_LDFLAGS})
    target_link_options(omdata INTERFACE -Wl,--as-needed)   # (its -latomic: unused on x86_64, not on every system)
elseif(AVCODEC_FOUND)
    target_compile_definitions(omdata PUBLIC OM_HAVE_AVCODEC)
    target_link_libraries(omdata PUBLIC PkgConfig::AVCODEC)
else()
    message(STATUS "no libavcodec: the import keeps the movies as STR")
endif()
target_compile_options(omdata PRIVATE $<$<CONFIG:Debug>:-O2>)   # (the import's conversions: slow unoptimized)
target_link_libraries(omdata PUBLIC m)
add_executable(openmizzurna
    src/main.c
    src/platform/platform.c src/platform/gl.c
    src/render/render.c src/render/scene3d.c
    src/game/bind_engine.c src/game/text.c src/game/bind_scene.c src/game/roads.c src/game/records.c src/game/snow.c src/game/console.c
    src/game/bind_audio.c src/audio/audio.c src/audio/seq.c src/audio/xa.c src/game/movie.c
    src/platform/library.c src/core/files.c)
target_link_libraries(openmizzurna PRIVATE omdata)
target_include_directories(openmizzurna PRIVATE src/third_party)
target_link_libraries(openmizzurna PRIVATE forth mactor SDL3::SDL3 m)
target_compile_definitions(openmizzurna PRIVATE "OM_SCRIPT_DIR=\"${SCRIPT_DIR}\"")

# the game's own tests: tests/<subsystem>/test_*.fs, run headless inside the game
file(GLOB GAME_TESTS "${CMAKE_CURRENT_SOURCE_DIR}/tests/*/test_*.fs")
foreach(t ${GAME_TESTS})
    get_filename_component(dir "${t}" DIRECTORY)
    get_filename_component(sub "${dir}" NAME)
    get_filename_component(name "${t}" NAME_WE)
    if(NOT sub STREQUAL "vocabs" AND NOT sub STREQUAL "library")
        add_test(NAME ${sub}_${name} COMMAND openmizzurna --test "${t}")
        set_tests_properties(${sub}_${name} PROPERTIES ENVIRONMENT "SDL_AUDIO_DRIVER=dummy")
    endif()
endforeach()

# ---- the data checks (the developers' disc: OM_DEV_DISC, a .cue; skipped without it) ----
set(OM_DEV_DISC "${CMAKE_CURRENT_SOURCE_DIR}/../Mizzurna Falls (English 31-03-2021)/Mizzurna Falls (English 31-03-2021).cue"
    CACHE FILEPATH "a disc image for the data checks")
set(OM_DEV_EXTRACT "${CMAKE_CURRENT_SOURCE_DIR}/../extract" CACHE PATH "phase 1's extraction (fs/, cdbin/) to compare with")
add_executable(disc_ls tools/disc_ls.c)
target_link_libraries(disc_ls PRIVATE omdata)
add_executable(cdbin_check tools/cdbin_check.c)
target_link_libraries(cdbin_check PRIVATE omdata)
add_executable(library_check tools/library_check.c)
target_link_libraries(library_check PRIVATE omdata)
if(EXISTS "${OM_DEV_DISC}")
    # the import into a library of the test's own, then every entry checked against it
    add_test(NAME library_import COMMAND ${CMAKE_COMMAND} -E env OM_HOME=${CMAKE_BINARY_DIR}/test-home
             $<TARGET_FILE:openmizzurna> --import "${OM_DEV_DISC}")
    add_test(NAME library_check COMMAND library_check "${OM_DEV_DISC}" "${CMAKE_BINARY_DIR}/test-home/library")
    set_tests_properties(library_check PROPERTIES DEPENDS library_import)
    # the tests that need the game's data: tests/library/test_*.fs, run on the test's own library
    file(GLOB DATA_TESTS "${CMAKE_CURRENT_SOURCE_DIR}/tests/library/test_*.fs")
    foreach(t ${DATA_TESTS})
        get_filename_component(name "${t}" NAME_WE)
        add_test(NAME library_${name} COMMAND openmizzurna --test "${t}")
        set_tests_properties(library_${name} PROPERTIES DEPENDS library_import
                             ENVIRONMENT "SDL_AUDIO_DRIVER=dummy;OM_HOME=${CMAKE_BINARY_DIR}/test-home")
    endforeach()
endif()
if(EXISTS "${OM_DEV_DISC}" AND EXISTS "${OM_DEV_EXTRACT}/fs")
    add_test(NAME disc_check COMMAND disc_ls "${OM_DEV_DISC}" "${OM_DEV_EXTRACT}/fs")
    add_test(NAME cdbin_check COMMAND cdbin_check "${OM_DEV_DISC}" "${OM_DEV_EXTRACT}/cdbin")
endif()
