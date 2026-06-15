#!/bin/bash

# Build LD58-CrunchTime and restore/build all required dependencies.

set -euo pipefail

SolutionDir="$(realpath "$(dirname "${BASH_SOURCE[0]}")")/"
cd "$SolutionDir"

dotnet build "./submodules/ChaosBuild.CodeFormatter.Tasks/"
dotnet build "./submodules/ChaosFrameworkBuild.ArchiveCreator/"
dotnet build "./LD58.sln"
