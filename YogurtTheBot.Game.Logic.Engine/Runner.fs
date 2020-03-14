module YogurtTheBot.Game.Logic.Engine.Runner

open YogurtTheBot.Game.Logic.Engine
open YogurtTheBot.Game.Logic.Engine.Models

type ResultStatus =
    | Complete
    | Fail
    | Nothing

type RunResult =
    { status: ResultStatus
      results: CallbackResult list
      addedPermissions: ActorAction list }    
    
let rec run actions permissions level =
    match actions with
        | Action action :: tail when Level.allowed (level.permissions @ permissions) action ->
            if Level.actionMatch level.winCondition action then
                { status = Complete
                  results = [Action action]
                  addedPermissions = [] }
            else 
                let newActions =
                    level.callbacks
                    |> List.filter (fun cb -> cb.reason = action)
                    |> List.map (fun cb -> cb.result)
                
                let result = run (tail @ newActions) permissions level
                
                { result
                  with results = result.results @ [Action action] }
        | cb :: tail ->
            match cb with
            | Action a -> 
                let result = run tail permissions level
                { result
                  with results = result.results @ [NotAllowed a] }
            | Grant g ->
                let result = run tail (g :: permissions) level
                { result 
                  with addedPermissions = g :: result.addedPermissions}
            | _ ->
                let result = run tail permissions level
                { result
                  with results = result.results @ [cb] }
        | _ ->
            { status = Nothing
              results = []
              addedPermissions = [] }
    
let runAction action = run [Action action] 
    

let testSolution level solution =
   run solution level.permissions level 
