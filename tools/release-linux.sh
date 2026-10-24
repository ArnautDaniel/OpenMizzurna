#!/bin/sh
# release-linux.sh: OpenMizzurna's Linux release, built the same way here and on GitHub's runners
# (.github/workflows/release-linux.yml).
#
# The program is one self-contained binary: SDL3 and a small FFmpeg (MPEG-2 video alone: the movies) are built
# from source and linked statically. SDL3 still finds the machine's X11 / Wayland and sound at run time. Built on
# an older system, it runs on newer ones (glibc is the floor).
#
#   tools/release-linux.sh [version]      -> build-release-linux/openmizzurna-<version>-linux-x86_64.tar.gz
#
# Needs a C compiler, CMake, Ninja, make, curl, and SDL3's build dependencies (the X11 / Wayland / sound headers
# it builds its backends against). FFmpeg is configured without assembly, so no nasm.
set -eu

SDL_VERSION=3.4.18
FFMPEG_VERSION=7.1.5

here=$(cd "$(dirname "$0")/.." && pwd)
version=${1:-$(git -C "$here" describe --tags --always 2>/dev/null || echo dev)}
out="$here/build-release-linux"
deps="$here/build-deps"            # the dependencies' sources and their install (kept between runs)
prefix="$deps/prefix"
jobs=$(nproc 2>/dev/null || echo 4)

mkdir -p "$deps/src" "$out"

# ---- SDL3, static ----
if [ ! -f "$prefix/lib/cmake/SDL3/SDL3Config.cmake" ] && [ ! -f "$prefix/lib64/cmake/SDL3/SDL3Config.cmake" ]; then
    cd "$deps/src"
    curl -fsSL -o sdl.tar.gz "https://github.com/libsdl-org/SDL/releases/download/release-$SDL_VERSION/SDL3-$SDL_VERSION.tar.gz"
    tar xzf sdl.tar.gz
    cmake -S "SDL3-$SDL_VERSION" -B sdl-build -G Ninja -DCMAKE_BUILD_TYPE=Release -DCMAKE_INSTALL_PREFIX="$prefix" \
        -DSDL_SHARED=OFF -DSDL_STATIC=ON -DSDL_TESTS=OFF -DSDL_EXAMPLES=OFF -DCMAKE_POSITION_INDEPENDENT_CODE=ON
    cmake --build sdl-build -j "$jobs"
    cmake --install sdl-build   # (its notice: share/licenses/SDL3/LICENSE.txt)
fi

# ---- FFmpeg, static: the MPEG-2 video decoder, encoder and parser, nothing else ----
if [ ! -f "$prefix/lib/pkgconfig/libavcodec.pc" ]; then
    cd "$deps/src"
    curl -fsSL -o ffmpeg.tar.xz "https://ffmpeg.org/releases/ffmpeg-$FFMPEG_VERSION.tar.xz"
    tar xJf ffmpeg.tar.xz
    cd "ffmpeg-$FFMPEG_VERSION"
    ./configure --prefix="$prefix" --libdir="$prefix/lib" --enable-static --disable-shared --enable-pic \
        --disable-everything --disable-autodetect --disable-programs --disable-doc --disable-network \
        --disable-avdevice --disable-avformat --disable-avfilter --disable-swscale --disable-swresample \
        --disable-x86asm \
        --enable-decoder=mpeg2video --enable-encoder=mpeg2video --enable-parser=mpegvideo
    make -j "$jobs"
    make install
    mkdir -p "$prefix/licenses" && cp COPYING.LGPLv2.1 "$prefix/licenses/FFmpeg-LGPL-2.1.txt"
fi

# ---- the program ----
build="$out/build"
PKG_CONFIG_PATH="$prefix/lib/pkgconfig" cmake -S "$here" -B "$build" -G Ninja -DCMAKE_BUILD_TYPE=Release \
    -DOM_RELEASE=ON -DOM_STATIC_FFMPEG=ON -DCMAKE_PREFIX_PATH="$prefix"
cmake --build "$build" -j "$jobs"

# the tests that need neither the disc nor a GPU: the language and the actor kernel
(cd "$build" && OM_SCRIPTS="$here/scripts" ctest --output-on-failure -R '^test_')

# ---- the package: the program, its scripts, the licenses ----
name="openmizzurna-$version-linux-x86_64"
stage="$out/$name"
rm -rf "$stage"
mkdir -p "$stage/licenses"
cp "$build/openmizzurna" "$stage/"
strip "$stage/openmizzurna"
cp -r "$here/scripts" "$stage/scripts"
cp "$here/README.md" "$here/LICENSE" "$stage/"
cp "$prefix/share/licenses/SDL3/LICENSE.txt" "$stage/licenses/SDL3.txt"
cp "$prefix/licenses/FFmpeg-LGPL-2.1.txt" "$stage/licenses/"
cat > "$stage/licenses/README.txt" <<EOF
OpenMizzurna is GPL-3.0 (../LICENSE). Built into it:
- SDL $SDL_VERSION (zlib license, SDL3.txt): https://github.com/libsdl-org/SDL
- FFmpeg $FFMPEG_VERSION, its MPEG-2 video codec alone (LGPL-2.1 or later, FFmpeg-LGPL-2.1.txt):
  https://ffmpeg.org, source at https://ffmpeg.org/releases/ffmpeg-$FFMPEG_VERSION.tar.xz
- stb, dr_flac (public domain or MIT), cgltf (MIT): their notices are in the source, src/third_party/.
EOF
tar czf "$out/$name.tar.gz" -C "$out" "$name"
echo "$out/$name.tar.gz"
