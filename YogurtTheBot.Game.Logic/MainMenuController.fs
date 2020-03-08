namespace YogurtTheBot.Game.Logic

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Handlers
open YogurtTheBot.Game.Core.Controllers.Language.Controllers

[<Controller(isMainController = true)>]
type MainMenuController(localizer) =
    inherit LanguageController<PlayerData>(localizer) 

    [<Action("screens.main_menu.start")>]
    member x.SayMyName(player: PlayerInfo) = x.Answer("Your name is " + player.SocialId)

    [<Action("screens.main_menu.attack")>]
    member x.AttackAction() = x.Answer "test"


