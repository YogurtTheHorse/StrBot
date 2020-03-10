namespace YogurtTheBot.Game.Logic

open Engine.Models
open System.Collections.Generic
open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers.Abstractions

type PlayerData() =
    inherit PlayerDataBase()

    [<DefaultValue>]
    val mutable controllersStack: List<string>
    
    // we don't store rules directly, cause we have some bug with union serialization
    member val reflexes  = list<Reflex * Reflex>.Empty with get, set

    interface IControllersData with
        member x.ControllersStack
            with get () = x.controllersStack
            and set v = x.controllersStack <- v