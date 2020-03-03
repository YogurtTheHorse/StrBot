module YogurtTheBot.Game.Logic.Engine.Models

type Actor =
    { name: string }

type Event =
    { name: string }

type Action =
    { name: string
      causes: Event }

type Rule =
    { actor: Actor
      action: Action
      reason: Event }
    
type Permission =
    { actor: Actor
      action: Action }