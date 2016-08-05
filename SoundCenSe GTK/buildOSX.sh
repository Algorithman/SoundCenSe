#!/bin/bash

wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -o nuget.exe
mono --runtime=v4.0 nuget.exe restore
mono --runtime=v4.0 nuget install Mono.Posix
xbuild
