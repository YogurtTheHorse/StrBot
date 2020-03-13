module YogurtTheBot.Game.Logic.Engine.Models

type Actor =
    { name: string }

type Action =
    { name: string }

type ActorAction =
    { actor: Actor 
      action: Action
      recipient: Actor }
    
type Callback =
    { reason: ActorAction
      result: ActorAction }
    
type Level =
    { name: string
      permissions: ActorAction list
      callbacks: Callback list
      solution: ActorAction list
      winCondition: ActorAction
      actors: Actor list
      actions: Action list }
    
let createAction actor action recipient =
    { actor = actor; action = action; recipient = recipient }