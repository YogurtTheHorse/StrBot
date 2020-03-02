namespace YogurtTheBot.Game.Logic

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Abstractions
open YogurtTheBot.Game.Core.Controllers.Handlers

[<Controller(isMainController = true)>]
type MainMenuController() =
    inherit ControllerBase()

    [<Action("screens.main_menu.say_my_name")>]
    member x.SayMyName(player: PlayerInfo) = x.Answer("Your name is " + player.SocialId)

    [<Action("screens.main_menu.attack")>]
    member x.AttackAction() = x.Answer "test"

    [<Default>]
    member x.DefaultAction() = x.Answer "default"

