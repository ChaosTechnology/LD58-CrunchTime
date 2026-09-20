#!/bin/bash

# Build and run LD58-CrunchTime and restore/build all required dependencies.

set -euo pipefail

SolutionDir="$(realpath "$(dirname "${BASH_SOURCE[0]}")")/"
cd "$SolutionDir"

bash ./build.sh
SolutionDir=$SolutionDir bash -c 'dotnet run "./LD58.sln" --project "./LD58/" --framework net8.0 --configuration Release'
