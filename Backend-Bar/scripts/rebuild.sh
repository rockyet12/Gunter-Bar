#!/usr/bin/env bash
set -euo pipefail
HERE="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT="$HERE/.."
cd "$ROOT"

echo "Cleaning solution..."
dotnet clean Backend-Bar.sln

echo "Restoring..."
dotnet restore Backend-Bar.sln

echo "Building..."
dotnet build Backend-Bar.sln -c Debug

echo "Run the API (foreground) - use Ctrl+C to stop"
cd BarGunter.API
dotnet run --project BarGunter.API.csproj
