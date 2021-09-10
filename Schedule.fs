// Handles all the logic for creating a repeatable schedule

module Schedule

open System

// Note that while I describe some of my program's data here,
//  I never need to annotate anything else with it.
// This has some complex type annotations that I wouldn't
//  need if I had just used the "string" type instead of
//  the generic type "'A". An advantage to doing it this way
//  is that the code never needs to change if I decide that
//  "people" should be represented as something besides a string,
//  for instance, a class or other type
type T<'A when 'A: comparison> =
  { Default: 'A list
    Alternatives: Map<'A, 'A list> }

type Day =
  { Default: string
    Alternative: string }

// Takes an existing .NET library (Random) and makes its interface functional
//  instead of class based. This wrapper makes its signature more
//  generic and makes type inference work better with it.
let createRng () =
  let rng = Random(Config.load().RandomSeed)
  fun () -> rng.Next()

// This holds the main algorithm of shuffling a group of candidates
//  into a yearly schedule. It accepts a RNG provider which enables
//  me to ensure that I seed the randomizer in a predictable way,
//  which is before every creation of a schedule.
let buildYearlyWithRNG rand people =
  // This is a super simple shuffler that just sorts a list in random order
  let shuffleSort = List.sortBy (fun _ -> rand ())

  // List.unfold is the reverse of a fold/reduce.
  // It takes a value and creates a list, instead of fold/reduce
  //  which takes a list and creates a value.
  List.unfold
    (fun daysLeft ->
      let nextCount = daysLeft - (people |> List.length)

      if nextCount < 0 then
        None
      else
        Some(people |> shuffleSort, nextCount))
    366
  |> List.concat

let buildYearlyForPeople people =
  let buildWithSeededRng = createRng () |> buildYearlyWithRNG

  // This is a (contrived) example of partial application.
  // List.filter takes a function and a list.
  // Since I haven't given it a list, just a function,
  //  this creates a new function that just takes a list
  let without excluded = List.filter (fun i -> i <> excluded)

  { Default = buildWithSeededRng people
    Alternatives =
      people
      // Fold/reducing a list of people here into a new Map
      //  with each entry being a person and a schedule without that person
      |> List.fold
           (fun m person ->
             let listWithoutPerson = without person people

             m
             |> Map.add person (buildWithSeededRng listWithoutPerson))
           Map.empty }

let buildYearly () =
  Config.load().Candidates |> buildYearlyForPeople

let getDay day =
  let today = buildYearly().Default |> List.item day

  let alternative =
    buildYearly().Alternatives.[today]
    |> List.item day

  { Default = today
    Alternative = alternative }
