module YogurtTheBot.Game.Logic.Engine.Default

open YogurtTheBot.Game.Logic.Engine.Models

module Actors =
    let game: Actor = { name = "game" }
    
    let everyone: Actor = { name = "everyone" }
    
    let somebody: Actor = { name = "somebody" }
    
    let door: Actor = { name = "door" }
    let player: Actor = { name = "player" }
    
    let key: Actor = { name = "key" }
    
    let wolves: Actor = { name = "wolves" }
    
    let grass: Actor = { name = "grass" }
    
    let rabbits: Actor = { name = "rabbits" }

module Actions =
    let open_: Action =
        { name = "open" }
        
    let eat: Action =     
        { name = "eat" }
        
    let takes: Action =
        { name = "takes" }

    let complete: Action =
        { name = "complete" }
        
    let start: Action =
        { name = "start" }
