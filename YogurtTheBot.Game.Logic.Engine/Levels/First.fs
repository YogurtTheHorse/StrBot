module YogurtTheBot.Game.Logic.Engine.Levels.First

open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Default.Actions
open YogurtTheBot.Game.Logic.Engine.Models

let openDoor = createAction player open_ door

let level =
    {
        name = "first"
        permissions = [ grant player open_ everyone ]
        callbacks = [{
            reason = openDoor
            result = Tag "door_open"
        }]
        winCondition = ["door_open"]
        actors = [player]
        actions = [open_]
        solution = [openDoor]
    }
