// Lightweight abstraction around retrieving config from ENV

module Config

open System

type T =
  { Candidates: string list
    RandomSeed: int }

let load () =
  let candidates =
    Environment.GetEnvironmentVariable "CANDIDATES"

  let randomSeed =
    Environment.GetEnvironmentVariable "RANDOM_SEED"

  { Candidates = candidates.Split(",") |> List.ofArray
    RandomSeed = int randomSeed }
