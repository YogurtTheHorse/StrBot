module YogurtTheBot.Game.Logic.Engine.LevelLoader

open System.IO
open Legivel.Serialization
open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.Engine.Level
open YogurtTheBot.Game.Logic.Engine.Descriptions

let internal loadDescription path: LevelDescription option =
    let levels =
        path
        |> File.ReadAllText
        |> Deserialize<LevelDescription>

    match levels with
    | [ Success { Data = level } ] -> Some(level)
    | _ -> None

let mapActionDescription levelDescription (actionDescription: ActionDescription): Action =
    {
        name = actionDescription.name
        causes = Array.find (fun e -> e.name = actionDescription.causes) levelDescription.events
    }

let mapRuleDescription levelDescription (ruleDesciption: RuleDescription): Rule =
    let actionDescription = Seq.find (fun (a: ActionDescription) -> a.name = ruleDesciption.action) (getActions levelDescription)
    
    {
        actor = findActor ruleDesciption.actor levelDescription
        action = mapActionDescription levelDescription actionDescription
        reason = Array.find (fun e -> e.name = ruleDesciption.reason) levelDescription.events
    }
    
let mapPermissionDescription levelDescription (permissionDescription: PermissionDescription): Permission =
    {
        actor = findActor permissionDescription.actor levelDescription
        action = mapActionDescription levelDescription (findAction permissionDescription.action levelDescription)
    }

let load path: Level option =
    let levelDescription = loadDescription path

    match levelDescription with
    | Some description ->
        let mapActions = Array.map (mapActionDescription description)
        
        Some {
            name = description.name
            permissions = Array.map (mapPermissionDescription description) description.permissions
            rules = Array.map (mapRuleDescription description) description.rules
            builtInActions = mapActions description.builtInActions
            builtInActors = description.builtInActors
            availableActors = description.availableActors
            availableActions = mapActions description.availableActions
        }
    | _ -> None
