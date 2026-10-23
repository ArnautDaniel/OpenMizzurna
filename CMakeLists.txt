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
add_executable(openmizzurna
    src/main.c
    src/platform/platform.c src/platform/gl.c
    src/render/render.c
    src/game/bind_engine.c src/game/console.c
    src/third_party/stb.c)
target_include_directories(openmizzurna PRIVATE src/third_party)
target_link_libraries(openmizzurna PRIVATE forth mactor SDL3::SDL3 m)
target_compile_definitions(openmizzurna PRIVATE "OM_SCRIPT_DIR=\"${SCRIPT_DIR}\"")

# the game's own tests: tests/<subsystem>/test_*.fs, run headless inside the game
file(GLOB GAME_TESTS "${CMAKE_CURRENT_SOURCE_DIR}/tests/*/test_*.fs")
foreach(t ${GAME_TESTS})
    get_filename_component(dir "${t}" DIRECTORY)
    get_filename_component(sub "${dir}" NAME)
    get_filename_component(name "${t}" NAME_WE)
    if(NOT sub STREQUAL "vocabs")
        add_test(NAME ${sub}_${name} COMMAND openmizzurna --test "${t}")
        set_tests_properties(${sub}_${name} PROPERTIES ENVIRONMENT "SDL_AUDIO_DRIVER=dummy")
    endif()
endforeach()
