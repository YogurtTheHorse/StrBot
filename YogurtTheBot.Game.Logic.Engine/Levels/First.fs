module YogurtTheBot.Game.Logic.Engine.Levels.First

open YogurtTheBot.Game.Logic.Engine.Level
open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Default.Actions

let level =
    lvl {
        acting player
        allowed door to_ complete
        allowed game to_ start
        allowed player to_ open_

        rules (ruleset {
            on somebody open_ door door will complete game
        })
        
        solution (ruleset {
            on game start somebody player will open_ door
        })
    }
