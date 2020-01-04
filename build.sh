#!/usr/bin/env bash

##########################################################################
# This is the Cake bootstrapper script for Linux and OS X.
# This file was downloaded from https://github.com/cake-build/resources
# Feel free to change this file to fit your needs.
##########################################################################

# Define directories.
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
TOOLS_DIR=$SCRIPT_DIR/tools
CAKE_VERSION=0.31.0
CAKE_DLL=$TOOLS_DIR/Cake.CoreCLR.$CAKE_VERSION/cake.coreclr/$CAKE_VERSION/Cake.dll
TOOLS_PROJ=$TOOLS_DIR/tools.csproj

# Define default arguments.
SCRIPT="build.cake"
CAKE_ARGUMENTS=()

# Parse arguments.
for i in "$@"; do
    case $1 in
        -s|--script) SCRIPT="$2"; shift ;;
        --) shift; CAKE_ARGUMENTS+=("$@"); break ;;
        *) CAKE_ARGUMENTS+=("$1") ;;
    esac
    shift
done

# Make sure the tools folder exist.
if [ ! -d "$TOOLS_DIR" ]; then
  mkdir "$TOOLS_DIR"
fi

# Make sure that cake exist.
if [ ! -f "$CAKE_DLL" ]; then
    echo "Downloading cake..."
    echo "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>netstandard1.6</TargetFramework></PropertyGroup></Project>" > $TOOLS_PROJ
    dotnet add $TOOLS_PROJ package cake.coreclr -v $CAKE_VERSION --package-directory $TOOLS_DIR/Cake.CoreCLR.$CAKE_VERSION
fi

# Make sure that Cake has been installed.
if [ ! -f "$CAKE_DLL" ]; then
    echo "Could not find Cake.dll at '$CAKE_DLL'."
    exit 1
fi

# Start Cake
exec dotnet "$CAKE_DLL" $SCRIPT "${CAKE_ARGUMENTS[@]}"
