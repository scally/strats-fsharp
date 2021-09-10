# strats
Lightweight standup calendar

This is an experiment building the same micro-app in multiple languages for a bake-off.

This app is the F# version. There will also be one each for [OCaml](https://github.com/scally/strats-ocaml)/[Rescript](https://github.com/scally/strats-rescript), which are other ML descendants.

## build

First, install the dotnet 5.0 (or newer) SDK. https://dotnet.microsoft.com/download

Then, you'll need to install Paket, which is the F# package manager. Paket sits on top of Nuget, the typical C#/dotnet package manager, which you shouldn't need to know for this exercise but makes for interesting trivia.

`dotnet tool install --global Paket`

Then install dependencies:

`paket install`

And build 

`dotnet build`

## run

`RANDOM_SEED=23456 CANDIDATES="Alice,Bob,Charlie" dotnet run`

## api

The API is located at http://localhost:5000 by default, and offers these endpoints:

`/today` view today's schedule

`/day/:day` view schedule for day N

`/schedule` view yearly schedule

`/liveness` health check

`/config` view configuration
