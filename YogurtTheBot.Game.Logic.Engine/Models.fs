module YogurtTheBot.Game.Logic.Engine.Models

type Actor =
    { name: string }

type Action =
    { name: string }

type Reflex =
    { actor: Actor 
      action: Action
      recipient: Actor }
    
type Permission =
    { actor: Actor
      action: Action }
    
type Rule =
    | Reflex of Reflex * Reflex
    | Permission of Permission
    
type Level =
    { name: string
      rules: Rule list
      solution: Rule list
      actors: Actor list
      actions: Action list }