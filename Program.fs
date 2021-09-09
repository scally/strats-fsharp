open Falco
open Falco.Routing
open Falco.HostBuilder

let livenessHandler = Response.ofJson {| Ok = "Ok" |}

let dayHandler =
  Request.mapRoute
    (fun r -> r.GetInt "day" 0)
    (fun dayNumber ->
      Response.ofJson
        {| day = dayNumber
           today = Schedule.getDay(dayNumber).Default
           alternative = Schedule.getDay(dayNumber).Alternative |})

let configHandler = Config.load () |> Response.ofJson

let scheduleHandler =
  Response.ofJson {| schedule = Schedule.buildYearlySchedule().Default |}

let todayHandler =
  let dayNumber = System.DateTime.Today.DayOfYear

  Response.ofJson
    {| day = dayNumber
       today = Schedule.getDay(dayNumber).Default
       alternative = Schedule.getDay(dayNumber).Alternative |}

[<EntryPoint>]
let main _argv =
  webHost [||] {
    endpoints
      [ get "/liveness" livenessHandler
        get "/config" configHandler
        get "/schedule" scheduleHandler
        get "/today" todayHandler
        get "/day/{day:int}" dayHandler ]
  }

  0
