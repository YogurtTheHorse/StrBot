module YogurtTheBot.Game.Logic.LevelController

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Communications
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Language.Controllers

open YogurtTheBot.Game.Core.Controllers.Language.Expressions
open YogurtTheBot.Game.Logic.Engine.Levels

[<Controller>]
type LevelController(cp, localizer) =   
    inherit LanguageController<PlayerData>(cp, localizer)
    
    member private x.Level = First.level
    
    [<LanguageAction("NewRuleRule")>]
    member x.NewRule(message: IncomingMessage) =
        x.Answer message.Text
        
    override x.OnOpen(info: PlayerInfo, data: PlayerData) =
         x.Answer "on open"

    member x.NewRuleRule = (LocalizedTerminal "screens.level.if")
