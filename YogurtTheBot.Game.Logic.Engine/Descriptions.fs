module YogurtTheBot.Game.Logic.Engine.Descriptions

open YogurtTheBot.Game.Logic.Engine.Models

type ActionDescription =
    { name: string
      causes: string }

type RuleDescription =
    { actor: string
      action: string
      reason: string }

type PermissionDescription =
    { actor: string
      action: string }

type LevelDescription =
    { name: string
      events: Event array
      rules: RuleDescription array
      permissions: PermissionDescription array
      builtInActors: Actor array
      builtInActions: ActionDescription array
      availableActors: Actor array
      availableActions: ActionDescription array }

let getActions desciption =
    Seq.append desciption.availableActions desciption.builtInActions

let getActors desciption =
    Seq.append desciption.availableActors desciption.builtInActors

let findAction name description =
    let actions = getActions description
    
    Seq.find (fun (a: ActionDescription) -> a.name = name) actions

let findActor name description =
    let actors = getActors description
    
    Seq.find (fun (a: Actor) -> a.name = name) actors