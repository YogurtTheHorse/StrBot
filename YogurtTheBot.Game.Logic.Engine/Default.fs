module YogurtTheBot.Game.Logic.Engine.Default

open YogurtTheBot.Game.Logic.Engine.Models

module Actors =
    let game: Actor = { name = "game" }
    
    let everyone: Actor = { name = "everyone" }
    
    let somebody: Actor = { name = "somebody" }
    
    let door: Actor = { name = "door" }
    let player: Actor = { name = "player" }

module Actions =
    let open_: Action =
        { name = "open" }

    let complete: Action =
        { name = "complete" }
        
    let start: Action =
        { name = "start" }
