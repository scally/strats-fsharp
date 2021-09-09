module Schedule

open System

type T<'A when 'A: comparison> =
  { Default: 'A list
    Alternatives: Map<'A, 'A list> }

type Day =
  { Default: string
    Alternative: string }

let createRng () =
  let rng = Random(Config.load().RandomSeed)
  fun () -> rng.Next()

let buildYearlyScheduleWithRNG rand people =
  let shuffleSort = List.sortBy (fun _ -> rand ())

  List.unfold
    (fun daysLeft ->
      let nextCount = daysLeft - (people |> List.length)

      if nextCount < 0 then
        None
      else
        Some(people |> shuffleSort, nextCount))
    366
  |> List.concat

let buildYearlyScheduleForPeople people =
  let buildWithSeededRng =
    createRng () |> buildYearlyScheduleWithRNG

  { Default = buildWithSeededRng people
    Alternatives =
      people
      |> List.fold
           (fun m person ->
             m
             |> Map.add person (buildWithSeededRng (List.filter (fun p -> p <> person) people)))
           Map.empty }

let buildYearlySchedule () =
  Config.load().Candidates
  |> buildYearlyScheduleForPeople

let getDay day =
  let today =
    buildYearlySchedule().Default |> List.item day

  let alternative =
    buildYearlySchedule().Alternatives.[today]
    |> List.item day

  { Default = today
    Alternative = alternative }
