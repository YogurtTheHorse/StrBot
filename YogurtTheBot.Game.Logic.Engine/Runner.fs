module YogurtTheBot.Game.Logic.Engine.Runner

open YogurtTheBot.Game.Logic.Engine.Models

type ResultStatus =
    | Complete
    | Fail
    | Nothing

type RunResult =
    { status: ResultStatus
      ranActions: ActorAction list
      addedPermissions: ActorAction list }    
    
let rec run actions permissions level =
    match actions with
        | action :: tail when Level.allowed permissions action ->
            if action = level.winCondition then
                { status = Complete
                  ranActions = [action]
                  addedPermissions = [] }
            else 
                let newActions =
                    level.callbacks
                    |> List.filter (fun cb -> cb.reason = action)
                    |> List.map (fun cb -> cb.result)
                
                let result = run  (tail @ newActions) permissions level
                
                { result
                  with ranActions = result.ranActions @ [action] }
        | _ ->
            { status = Nothing
              ranActions = []
              addedPermissions = [] }
    
let runAction action = run [action] 
    

let testSolution level solution =
   run solution level.permissions level 
