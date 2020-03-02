namespace YogurtTheBot.Game.Logic

open System.Collections.Generic
open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers

type PlayerData() =
    inherit PlayerDataBase()
    
    interface IControllersData with
        member x.ControllersStack = Stack<string>()