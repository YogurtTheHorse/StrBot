namespace StrategyBot.Game.Logic

open StrategyBot.Game.Core
open StrategyBot.Game.Core.Controllers
open StrategyBot.Game.Core.Controllers.Abstractions
open StrategyBot.Game.Core.Controllers.Handlers

[<Controller(isMainController = true)>]
type MainMenuController() =
    inherit ControllerBase()

    [<Action("screens.main_menu.say_my_name")>]
    member __.SayMyName(player: PlayerState) = __.Answer("Your name is " + player.SocialId)

    [<Action("screens.main_menu.attack")>]
    member __.AttackAction() = __.Answer "test"

    [<Default>]
    member __.DefaultAction() = __.Answer "default"
