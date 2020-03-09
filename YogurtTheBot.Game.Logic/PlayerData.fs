namespace YogurtTheBot.Game.Logic

open System.Collections.Generic
open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers.Abstractions

type PlayerData() =
    inherit PlayerDataBase()

    [<DefaultValue>]
    val mutable controllersStack: List<string>

    interface IControllersData with
        member x.ControllersStack
            with get () = x.controllersStack
            and set v = x.controllersStack <- v
