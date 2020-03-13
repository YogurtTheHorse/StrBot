namespace YogurtTheBot.Game.Logic

open Engine.Models
open System.Collections.Generic
open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers.Abstractions
open YogurtTheBot.Game.Core.Localizations

type PlayerData() =
    inherit PlayerDataBase()

    [<DefaultValue>]
    val mutable controllersStack: List<string>
    
    member val rules = list<Rule>.Empty with get, set

    interface IControllersData with
        member x.ControllersStack
            with get () = x.controllersStack
            and set v = x.controllersStack <- v
        
type RuleAddingResult =
    | Added
    | Error of Localization
            
module PlayerData =
    let addRule level (data: PlayerData) rule =
        data.rules <- (data.rules @ [ rule ])
        
        Added
        