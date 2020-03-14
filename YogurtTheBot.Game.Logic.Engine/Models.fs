module YogurtTheBot.Game.Logic.Engine.Models

type Actor =
    { name: string }

type Action =
    { name: string }

type ActorAction =
    { actor: Actor 
      action: Action
      recipient: Actor }
    
type CallbackResult =
    | Action of ActorAction
    | Grant of ActorAction
    | Answer of string
    | NotAllowed of ActorAction
    
type Callback =
    { reason: ActorAction
      result: CallbackResult }
    
type Level =
    { name: string
      permissions: ActorAction list
      callbacks: Callback list
      solution: ActorAction list
      winCondition: ActorAction
      actors: Actor list
      actions: Action list }
    
let isAction cbResult =
    match cbResult with
    | Action _ -> true
    | _ -> false
    
let createAction actor action recipient =
    { actor = actor; action = action; recipient = recipient }
    
let grant actor action recipient =
    createAction actor action recipient
    |> Grant 