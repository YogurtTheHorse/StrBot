module YogurtTheBot.Game.Logic.Engine.Level

open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.Engine.Default.Actors

type Will = Will

let will = Will

type Be = Be

let be = Be

type To = To

let to_ = To

let getActions level =
    level.callbacks
    |> List.map (fun cb -> [cb.reason; cb.result])
    |> List.concat
    |> List.map (fun a -> a.action)
    |> (@) level.actions
    |> Seq.distinctBy (fun a -> a.name)

let getActors level =
    level.callbacks
    |> List.map (fun cb -> [cb.reason; cb.result])
    |> List.concat
    |> (@) [level.winCondition]
    |> List.map (fun a -> [ a.recipient; a.actor ])
    |> (@) [ level.actors ]
    |> Seq.concat
    |> Seq.distinctBy (fun a -> a.name)
    
    
let actorsMatch pattern actor =
    pattern = actor || pattern = somebody || pattern = everyone || actor = somebody || pattern = everyone

let actionMatch a1 a2 =
    actorsMatch a1.actor a2.actor && actorsMatch a1.recipient a2.recipient && a1.action = a2.action

let allowed permissions action =
    List.exists (fun p -> actionMatch p action) permissions
    
let reflexMatches pattern reflex =
    actorsMatch pattern.recipient reflex.recipient &&
    actorsMatch pattern.actor reflex.actor &&
    pattern.action = reflex.action
