module YogurtTheBot.Game.Logic.Engine.Levels.Second

open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Default.Actions
open YogurtTheBot.Game.Logic.Engine.Models

let openDoor = createAction somebody open_ door
let takesKey = createAction player takes key

let level =
    {
        name = "second"
        permissions = [
            createAction key open_ everyone
            createAction player takes key
        ]
        callbacks = [
            { reason = takesKey
              result = grant player open_ door }
            { reason = takesKey
              result = Answer "levels.second.takes_key"}
        ]
        winCondition = openDoor
        actors = [player; key]
        actions = [open_; takes]
        solution = [openDoor]
    }
