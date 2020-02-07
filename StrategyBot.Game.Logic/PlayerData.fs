namespace StrategyBot.Game.Logic

open System.Collections.Generic
open StrategyBot.Game.Core
open StrategyBot.Game.Core.Controllers

type PlayerData() =
    inherit PlayerDataBase()
    
    interface IControllersData with
        member __.ControllersStack = Stack<string>()