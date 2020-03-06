module YogurtTheBot.Game.Logic.Engine.Level

open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Default.Actions

type Will = Will

let will = Will

type Be = Be

let be = Be

type To = To

let to_ = To

type RulesetBuilder() =
    member x.Yield(()) = []

    [<CustomOperation("on")>]
    member x.AddReflex(rules: Rule list, stimulus: Actor, reason: Action, stimulusRecipient: Actor, reactor: Actor, _: Will,
                       action: Action, recipient: Actor) =
        Reflex (
                   { actor = stimulus; action = reason; recipient = stimulusRecipient },
                   { actor = reactor; action = action; recipient = recipient }
               )
        :: rules



type LevelBuilder() =

    [<CustomOperation("acting")>]
    member x.AddActor(l: Level, a: Actor) = { l with actors = a :: l.actors }

    [<CustomOperation("doing")>]
    member x.AddAction(l: Level, a: Action) = { l with actions = a :: l.actions }

    [<CustomOperation("rules")>]
    member x.SetRules(l: Level, rules: Rule list) = { l with rules = rules @ l.rules }

    [<CustomOperation("solution")>]
    member x.SetSolution(l: Level, solution: Rule list) = { l with solution = solution }


    [<CustomOperation("allowed")>]
    member x.AddPermission(l: Level, actor: Actor, _: To, action: Action) =
        { l with
              rules =
                  Permission
                      { actor = actor
                        action = action }
                  :: l.rules }

    member x.Yield(()) =
        { name = "empty"
          rules = List.empty
          solution = List.empty
          actors = List.empty
          actions = List.empty }

let getActions level =
    level.rules
    |> List.map (fun (r: Rule) ->
        match r with
        | Reflex ({ action = a1 }, { action = a2 }) -> [ a1; a2 ]
        | Permission { action = action } -> [ action ])
    |> (@) [ level.actions ]
    |> Seq.concat
    |> Seq.distinctBy (fun a -> a.name)
    
    
let actorsMatch pattern actor =
    pattern = actor || pattern = somebody || pattern = everyone || actor = somebody || pattern = everyone

let allowed (reflex: Reflex) l =
    List.exists (fun r ->
        match r with
        | Permission p ->
            actorsMatch p.actor reflex.actor && p.action = reflex.action
        | _ -> false
    ) l
    
let reflexMatches pattern reflex =
    actorsMatch pattern.recipient reflex.recipient &&
    actorsMatch pattern.actor reflex.actor &&
    pattern.action = reflex.action

type TestResult =
    | Complete
    | Fail
    | Stimuli of Reflex list

let testSolution level solution =
    
    let completeRuleset = level.rules @ solution
    
    let rec test (stimuli: Reflex list) =
        match stimuli with
        | stimulus :: tail when allowed stimulus completeRuleset ->
            match stimulus with
            | { action = a; recipient = r } when a = complete && r = game ->
                Complete
            | _ -> 
                let newStimuli =
                    completeRuleset
                    |> List.filter (fun r ->
                        match r with
                        | Reflex (reflex, _) when reflexMatches reflex stimulus -> true
                        | _ -> false)
                    |> List.map (fun r -> match r with Reflex (_, r_) -> r_)
                
                test (newStimuli @ tail) 
            
        | _ -> Fail
        
        
        
    test [{actor = game; action = start; recipient = everyone}]


let lvl = LevelBuilder()
let ruleset = RulesetBuilder()
