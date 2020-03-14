namespace YogurtTheBot.Game.Logic

open Engine.Models
open System.Collections.Generic
open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers.Abstractions
open YogurtTheBot.Game.Logic.Engine

type PlayerData() as x =
    inherit PlayerDataBase()

    [<DefaultValue>]
    val mutable controllersStack: List<string>
    
    [<DefaultValue>]    
    val mutable availableLevelsCount: int
    
    [<DefaultValue>]
    val mutable savedPermissions: ActorAction list
    
    do
        x.controllersStack <- List<string>()
        x.availableLevelsCount <- 1
        x.savedPermissions <- List.empty

    interface IControllersData with
        member x.ControllersStack
            with get () = x.controllersStack
            and set v = x.controllersStack <- v
            
module PlayerData =
    let runAction level (data: PlayerData) action =
        let result = Runner.runAction action data.savedPermissions level
        
        data.savedPermissions <- data.savedPermissions @ result.addedPermissions
        
        result
        