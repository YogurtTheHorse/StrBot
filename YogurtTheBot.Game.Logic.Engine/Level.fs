module YogurtTheBot.Game.Logic.Engine.Level

open YogurtTheBot.Game.Logic.Engine.Models


type Level =
    { name: string
      rules: Rule array
      permissions: Permission array
      builtInActors: Actor array
      builtInActions: Action array
      availableActors: Actor array
      availableActions: Action array }

let getActions level =
    Array.concat [| level.builtInActions; level.availableActions |]
    
let getEvents level =
    level
    |> getActions
    |> Array.map (fun a -> a.causes)
    |> Array.distinctBy (fun e -> e.name)
