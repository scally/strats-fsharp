// Kind of like a controller, gluing routes to models.

open Falco
open Falco.Routing
open Falco.HostBuilder

let livenessHandler = Response.ofJson {| Ok = "Ok" |}

let dayHandler =
  Request.mapRoute
    // How to extract route variables from the route
    (fun r -> r.GetInt "day" 0)
    // The route handler receives the extracted route variable
    (fun dayNumber ->
      Response.ofJson
        {| day = dayNumber
           today = Schedule.getDay(dayNumber).Default
           alternative = Schedule.getDay(dayNumber).Alternative |})

let configHandler = Config.load () |> Response.ofJson

let scheduleHandler =
  Response.ofJson {| schedule = Schedule.buildYearly().Default |}

let todayHandler =
  let dayNumber = System.DateTime.Today.DayOfYear

  Response.ofJson
    {| day = dayNumber
       today = Schedule.getDay(dayNumber).Default
       alternative = Schedule.getDay(dayNumber).Alternative |}

// Assembles all request handlers into one webserver
// Configures routes to handlers
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
