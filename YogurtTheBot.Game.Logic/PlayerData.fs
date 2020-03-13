namespace YogurtTheBot.Game.Logic

open System.Collections.Generic
open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers.Abstractions
open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.Engine

type PlayerData() =
    inherit PlayerDataBase()

    [<DefaultValue>]
    val mutable controllersStack: List<string>

    interface IControllersData with
        member x.ControllersStack
            with get () = x.controllersStack
            and set v = x.controllersStack <- v
            
module PlayerData =
    let runAction level (data: PlayerData) action =
        let result = Runner.runAction action level.permissions level
        
        // TODO: Save permissions to data
        
        result
        