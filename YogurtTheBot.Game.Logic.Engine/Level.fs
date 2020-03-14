module YogurtTheBot.Game.Logic.Engine.Level

open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Models

type Will = Will

let will = Will

type Be = Be

let be = Be

type To = To

let to_ = To

let getActorActions level =
    level.callbacks
    |> List.map (fun cb ->
        match cb.result with
        | Action a -> [cb.reason; a]
        | _ -> [cb.reason]
    )
    |> List.concat

let getActions level =
    getActorActions level
    |> List.map (fun a -> a.action)
    |> (@) level.actions
    |> Seq.distinctBy (fun a -> a.name)

let getActors level =
    getActorActions level
    |> List.map (fun a -> [ a.recipient; a.actor ])
    |> (@) [ level.actors ]
    |> Seq.concat
    |> Seq.distinctBy (fun a -> a.name)
    
    
let actorsMatch pattern actor =
    pattern = actor || pattern = somebody || pattern = everyone || actor = somebody || pattern = everyone

let actionMatch a1 a2 =
    actorsMatch a1.actor a2.actor && actorsMatch a1.recipient a2.recipient && a1.action = a2.action

let allowed permissions action =
    let rec allowed_ permissions def =
        match permissions with
        | [] -> def
        | p :: tail ->
            match p with
            | Grant a when actionMatch a action ->
                allowed_ tail true
            | Restriction a when actionMatch a action ->
                allowed_ tail false
            | _ -> allowed_ tail def 
     
    allowed_ permissions false
    
let reflexMatches pattern reflex =
    actorsMatch pattern.recipient reflex.recipient &&
    actorsMatch pattern.actor reflex.actor &&
    pattern.action = reflex.action
