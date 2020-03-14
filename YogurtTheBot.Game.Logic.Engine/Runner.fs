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
      addedPermissions: Permission list
      savedTags: string list }    
    
let rec run actions permissions tags level =
    match actions with
        | Action action :: tail when Level.allowed (level.permissions @ permissions) action ->
            let newActions =
                level.callbacks
                |> List.filter (fun cb -> Level.actionMatch cb.reason action)
                |> List.map (fun cb -> cb.result)
            
            let result = run (tail @ newActions) permissions tags level
            
            { result
              with results = result.results @ [Action action] }
        | cb :: tail ->
            match cb with
            | Action a -> 
                let result = run tail permissions tags level
                { result
                  with results = result.results @ [NotAllowed a] }
            | Permission p ->
                let result = run tail (p :: permissions) tags level
                { result 
                  with addedPermissions = p :: result.addedPermissions}
            | Failure _ ->
                { status = Complete
                  results = [cb]
                  savedTags = []
                  addedPermissions = [] }
            | Tag t ->
                let result = run tail permissions (t :: tags) level
                { result 
                  with savedTags = t :: result.savedTags }
            | _ ->
                let result = run tail permissions tags level
                { result
                  with results = result.results @ [cb] }
        | _ ->
            let complete = 
                level.winCondition
                |> List.forall (fun t -> List.contains t tags)
                
            { status = if complete then Complete else Nothing
              results = []
              savedTags = []
              addedPermissions = [] }
    
let runAction action = run [Action action] 
    

let testSolution level solution =
   run solution level.permissions [] level 
