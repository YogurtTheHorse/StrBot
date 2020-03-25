module YogurtTheBot.Game.Logic.Engine.Levels.Second

open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Default.Actions
open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.Engine

let openDoor = createAction somebody open_ door
let takesKey = createAction player takes key

let level =
    {
        name = "second"
        permissions = [
            grant key open_ everyone
            grant player takes key
        ]
        callbacks = [
            { reason = takesKey
              result = grant player open_ door |> Permission }
            { reason = createAction key takes key
              result = grant player open_ door |> Permission }
            { reason = createAction player find key
              result = Answer "levels.second.found_key" }
            { reason = takesKey
              result = Answer "levels.second.takes_key"}
            { reason = openDoor
              result = Tag "door_open" }
        ]
        winCondition = Level.allTags ["door_open"] None
        actors = [player; key]
        actions = [open_; takes]
        solution = [openDoor]
    }
