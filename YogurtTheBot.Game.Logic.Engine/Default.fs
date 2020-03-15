module YogurtTheBot.Game.Logic.Engine.Default

open YogurtTheBot.Game.Logic.Engine.Models

module Actors =
    let game: Actor = 
        { name = "game"
          grammaticalNumber = Singular }
    
    let everyone: Actor = { name = "everyone"; grammaticalNumber = Singular }
    
    let somebody: Actor = { name = "somebody"; grammaticalNumber = Singular }
    
    let door: Actor = { name = "door"; grammaticalNumber = Singular }
    let player: Actor = { name = "player"; grammaticalNumber = Singular }
    
    let key: Actor = { name = "key"; grammaticalNumber = Singular }
    
    let wolves: Actor = { name = "wolves"; grammaticalNumber = Plural }
    
    let grass: Actor = { name = "grass"; grammaticalNumber = Singular }
    
    let rabbits: Actor = { name = "rabbits"; grammaticalNumber = Plural }

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
        
    let find = { name = "find" } 
