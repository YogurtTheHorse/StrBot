module YogurtTheBot.Game.Logic.Engine.Models

type GrammaticalNumber = Singular | Plural

type Actor =
    { name: string
      grammaticalNumber: GrammaticalNumber }

type Action =
    { name: string }

type ActorAction =
    { actor: Actor 
      action: Action
      recipient: Actor }
    
type Permission =
    | Grant of ActorAction
    | Restriction of ActorAction
    
type CallbackResult =
    | Action of ActorAction
    | Permission of Permission
    | Answer of string
    | Failure of string
    | NotAllowed of ActorAction
    | Tag of string
    
type Callback =
    { reason: ActorAction
      result: CallbackResult }
    
type Level =
    { name: string
      permissions: Permission list
      callbacks: Callback list
      solution: ActorAction list
      winCondition: string list
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
    
let restrict actor action recipient =
    createAction actor action recipient
    |> Restriction