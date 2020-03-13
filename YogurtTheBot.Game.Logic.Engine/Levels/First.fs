module YogurtTheBot.Game.Logic.Engine.Levels.First

open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Default.Actions
open YogurtTheBot.Game.Logic.Engine.Models

let openDoor = createAction player open_ door

let level =
    {
        name = "first"
        permissions = [ createAction player open_ everyone  ]
        callbacks = List.empty
        winCondition = openDoor
        actors = [player]
        actions = [open_]
        solution = [openDoor]
    }
